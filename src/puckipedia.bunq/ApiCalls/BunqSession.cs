using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace puckipedia.bunq.ApiCalls
{
    /// <summary>
    /// A connection to the bunq api. Keeps a session active when needed.
    /// </summary>
    public class BunqSession
    {

        // The RSA keys for signing requests and verifying responses.
        private AsymmetricCipherKeyPair _sessionKey;
        private AsymmetricKeyParameter _serverPublicKey;

        // the actual API key. Needed to make a SessionServer and DeviceServer
        private string _apiSecret;

        // Installation info: required to recover server public key and create a session server
        private int _installationId;
        private string _installationToken;

        // DeviceServer info. Needed to recover after a sandbox wipe.
        private string _deviceDescription;
        private string[] _devicePermittedIPs;

        // SessionServer info. Needed for requests
        private string _sessionToken;
        private DateTime _sessionExpiry;

        private Uri _baseUri;

        private HttpClient _client;

        private BunqSession() { }

        public User.User User { get; private set; }

        private class _serializedData
        {
            public string ApiSecret { get; set; }
            public string SessionKey { get; set; }
            public string ServerKey { get; set; }
            public int InstallationId { get; set; }
            public string InstallationToken { get; set; }
            public string DeviceDescription { get; set; }
            public string[] DevicePermittedIPs { get; set; }
            public Uri BaseUri { get; set; }
        }

        /// <summary>
        /// Gets all the data needed to resume the session with this API key, InstallationServer, and DeviceServer
        /// </summary>
        /// <returns>A JSON object containing all the data needed to resume this session.</returns>
        public string StoreResumeData()
        {
            string sessionKey;
            string serverPublicKey;

            using (var sessionKeyWriter = new StringWriter())
            {
                var sessionPemWriter = new PemWriter(sessionKeyWriter);
                sessionPemWriter.WriteObject(_sessionKey);

                sessionKeyWriter.Flush();
                sessionKey = sessionKeyWriter.ToString();
            }

            using (var serverPublicKeyWriter = new StringWriter())
            {
                var serverPublicPemWriter = new PemWriter(serverPublicKeyWriter);
                serverPublicPemWriter.WriteObject(_serverPublicKey);

                serverPublicKeyWriter.Flush();
                serverPublicKey = serverPublicKeyWriter.ToString();
            }

            var data = new _serializedData
            {
                ApiSecret = _apiSecret,
                SessionKey = sessionKey,
                ServerKey = serverPublicKey,
                InstallationId = _installationId,
                InstallationToken = _installationToken,
                DeviceDescription = _deviceDescription,
                DevicePermittedIPs = _devicePermittedIPs,
                BaseUri = _baseUri
            };

            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// Builds the HttpClient to communicate
        /// </summary>
        /// <param name="userAgent"></param>
        private void _setUpClient(string userAgent)
        {
            _client = new HttpClient(new BunqHttpMessageHandler(this, new HttpClientHandler()));
            _client.BaseAddress = _baseUri;
            _client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            _client.DefaultRequestHeaders.Add("User-Agent", "puckipedia.bunq/1.0 (" + userAgent + ")");
            _client.DefaultRequestHeaders.Add("X-Bunq-Language", "en_US");
            _client.DefaultRequestHeaders.Add("X-Bunq-Region", "nl_NL");
            _client.DefaultRequestHeaders.Add("X-Bunq-Geolocation", "0 0 0 0 000");
        }

#region Everything signatures
        internal async Task<string> _buildSignature(HttpRequestMessage message)
        {
            var headers = new Dictionary<string, string>();
            foreach (var header in _client.DefaultRequestHeaders)
            {
                if (header.Key.StartsWith("X-Bunq-") || header.Key == "Cache-Control" || header.Key == "User-Agent")
                    headers.Add(header.Key, string.Join(" ", header.Value));
            }

            foreach (var header in message.Headers)
            {
                if (header.Key.StartsWith("X-Bunq-") || header.Key == "Cache-Control" || header.Key == "User-Agent")
                    headers[header.Key] = string.Join(" ", header.Value);
            }

            var builder = new StringBuilder();
            builder.Append($"{message.Method.Method} {message.RequestUri.PathAndQuery}\n");

            foreach (var header in headers.OrderBy(a => a.Key))
            {
                builder.Append($"{header.Key}: {header.Value}\n");
            }

            builder.Append("\n");
            if (message.Content != null)
                builder.Append(await message.Content.ReadAsStringAsync());

            var bytes = Encoding.UTF8.GetBytes(builder.ToString());

            var signEngine = SignerUtilities.GetSigner("SHA256withRSA");
            signEngine.Init(true, _sessionKey.Private);
            signEngine.BlockUpdate(bytes, 0, bytes.Length);

            return Convert.ToBase64String(signEngine.GenerateSignature());
        }

        internal async Task<bool> _verifySignature(HttpResponseMessage message)
        {
            if (_serverPublicKey == null || message.StatusCode != HttpStatusCode.OK) return true;
            var headers = new Dictionary<string, string>();
            foreach (var header in message.Headers)
            {
                if (header.Key.StartsWith("X-Bunq-") && header.Key != "X-Bunq-Server-Signature")
                    headers.Add(header.Key, header.Value.First());
            }

            var builder = new StringBuilder();
            builder.Append($"{(int)message.StatusCode}\n");
            foreach (var header in headers.OrderBy(a => a.Key))
                builder.Append($"{header.Key}: {header.Value}\n");
            builder.Append("\n");
            builder.Append(await message.Content.ReadAsStringAsync());

            var bytes = Encoding.UTF8.GetBytes(builder.ToString());
            var signature = Convert.FromBase64String(message.Headers.GetValues("X-Bunq-Server-Signature").FirstOrDefault() ?? "");
            var signer = SignerUtilities.GetSigner("SHA256withRSA");
            signer.Init(false, _serverPublicKey);
            signer.BlockUpdate(bytes, 0, bytes.Length);

            return signer.VerifySignature(signature);
        }

        private string ExportPublicKey()
        {
            var stringWriter = new StringWriter();
            var pemWriter = new PemWriter(stringWriter);
            pemWriter.WriteObject(_sessionKey.Public);
            stringWriter.Flush();
            return stringWriter.ToString();
        }

        private byte[] _encodeDefiniteLength(int value)
        {
            if (value <= 127)
                return new byte[] { (byte)value };
            else
            {
                var neededBytes = (int)Math.Ceiling((1 + Math.Log(value) / Math.Log(2)) / 8);
                var result = new byte[neededBytes + 1];
                result[0] = (byte)(neededBytes | 0x80);
                for (int i = 0; i < neededBytes; i++)
                {
                    result[result.Length - i - 1] = (byte)(value >> (i * 8) & 0xFF);
                }

                return result;
            }
        }
        
        private Tuple<int, int> _decodeDefiniteLength(byte[] value, int start)
        {
            if ((value[start] & 0x80) == 0)
                return new Tuple<int, int>(start + 1, (int)value[start]);
            else
            {
                var count = (value[start] & 0x7F);
                start += 1;
                var result = 0;
                for (int i = 0; i < count; i++, start++)
                {
                    result = result << 8 | value[start];
                }

                return new Tuple<int, int>(start, result);
            }
        }

        private AsymmetricKeyParameter _decodePublicKey(string key)
        {
            var reader = new PemReader(new StringReader(key));
            return (AsymmetricKeyParameter)reader.ReadObject();
        }

        private AsymmetricCipherKeyPair _decodeKeypair(string key)
        {
            var reader = new PemReader(new StringReader(key));
            return (AsymmetricCipherKeyPair)reader.ReadObject();
        }

        #endregion

        /// <summary>
        /// Calls a non-paginated bunq API.
        /// </summary>
        /// <typeparam name="R">The base type of the response objects.</typeparam>
        /// <param name="method">Which HTTP method to use</param>
        /// <param name="uri">The URI to call (relative to base URI)</param>
        /// <param name="obj">An object to send to the server (required if method != HttpMethod.Get)</param>
        /// <param name="resultObject">If this is passed, this object will be filled up with the values of the response. Used in update calls.</param>
        /// <returns>A bunq response.</returns>
        public async Task<BunqResponse<R>> CallAsync<R>(HttpMethod method, string uri, BunqObject obj = null, R resultObject = null, object populateData = null)
            where R : BunqObject
        {
            await _controlSessionExpiry(); // ensure up-to-date session

            var requestMessage = new HttpRequestMessage(method, uri);
            requestMessage.Headers.Add("X-Bunq-Client-Authentication", _sessionToken ?? _installationToken);
            if (method != HttpMethod.Get)
            {
                var serialized = JsonConvert.SerializeObject(obj);
                requestMessage.Content = new StringContent(serialized);
            }

            var response = await _client.SendAsync(requestMessage);

            var deserialized = JsonConvert.DeserializeObject<BunqResponse<R>>(await response.Content.ReadAsStringAsync());
            deserialized.Session = this;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                foreach (var resultObj in deserialized.Response) { resultObj._hydrate(this, populateData); }

                if (resultObject != null)
                {
                    resultObject._populate(deserialized.Response.First());
                }

                return deserialized;
            }
            else
            {
                throw new BunqException(deserialized.Error.First());
            }
        }

        /// <summary>
        /// Calls a paginated bunq endpoint.
        /// </summary>
        /// <typeparam name="R">The type of object in the response.</typeparam>
        /// <param name="uri">The URI to call (relative to base URI)</param>
        /// <returns>A paginated bunq response, which can be enumerated.</returns>
        public async Task<BunqPaginatedResponse<R>> ListAsync<R>(string uri, object data = null)
            where R : BunqObject
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            requestMessage.Headers.Add("X-Bunq-Client-Authentication", _sessionToken ?? _installationToken);

            var response = await _client.SendAsync(requestMessage);

            var deserialized = JsonConvert.DeserializeObject<BunqPaginatedResponse<R>>(await response.Content.ReadAsStringAsync());
            deserialized.Session = this;
            deserialized._populateData = data;
            foreach (var resultObj in deserialized.Response) { resultObj._hydrate(this, data); }

            return deserialized;
        }

        private class _sessionServerRequest : BunqRequest {
            [JsonProperty("secret")] public string Secret { get; set; }
        }

        private async Task _controlSessionExpiry(bool loop = false)
        {
            if ((DateTime.Now > _sessionExpiry || _sessionToken == null) && (loop || User != null))
            {
                // Build a new session!

                _sessionToken = null; // evil

                User = null;
                var sessionInfo = await CallAsync<BunqObject>(HttpMethod.Post, "/v1/session-server", new _sessionServerRequest { Secret = _apiSecret });
                _sessionToken = sessionInfo.Get<Token>().TokenValue;
                User = sessionInfo.Get<User.User>();
                _sessionExpiry = DateTime.Now.AddSeconds(User.SessionTimeout);
            }
            else if (DateTime.Now.AddSeconds(30) > _sessionExpiry && User != null)
                _sessionExpiry = _sessionExpiry.AddSeconds(Math.Min(300, User.SessionTimeout));
        }

        /// <summary>
        /// Resumes a previous session set up using BunqSession.Create
        /// </summary>
        /// <param name="serializedData">The deserialized data</param>
        /// <param name="userAgent">The comment to use in the user agent</param>
        /// <returns>A bunq session</returns>
        public static async Task<BunqSession> Resume(string serializedData, string userAgent)
        {
            var session = new BunqSession();
            var _data = JsonConvert.DeserializeObject<_serializedData>(serializedData);

            session._apiSecret = _data.ApiSecret;
            session._sessionKey = session._decodeKeypair(_data.SessionKey);
            session._serverPublicKey = session._decodePublicKey(_data.ServerKey);
            session._installationId = _data.InstallationId;
            session._installationToken = _data.InstallationToken;
            session._deviceDescription = _data.DeviceDescription;
            session._devicePermittedIPs = _data.DevicePermittedIPs;
            session._baseUri = _data.BaseUri;

            session._setUpClient(userAgent);

            await session._controlSessionExpiry();
            return session;
        }

        /// <summary>
        /// Creates a new session.
        /// </summary>
        /// <param name="apiHostname">The hostname to use</param>
        /// <param name="userAgent">The comment to use in the user agent</param>
        /// <param name="description">The description, which will be shown in the API keys screen</param>
        /// <param name="apiKey">The API key</param>
        /// <param name="permittedIPs">A list of IPs that can be used for this API key.</param>
        /// <returns></returns>
        public static async Task<BunqSession> Create(string apiHostname, string userAgent, string description, string apiKey, List<string> permittedIPs)
        {
            var session = new BunqSession();

            var keyPairGenerator = new RsaKeyPairGenerator();
            keyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
            session._sessionKey = keyPairGenerator.GenerateKeyPair();

            session._apiSecret = apiKey;

            session._baseUri = new Uri($"https://{apiHostname}/");
            session._setUpClient(userAgent);

            // set up installation

            var clientPublicKey = new ClientPublicKey { PublicKey = session.ExportPublicKey() };
            var serialized = JsonConvert.SerializeObject(clientPublicKey);

            var response = await session._client.PostAsync("/v1/installation", new StringContent(serialized));
            var strResponse = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<BunqResponse<BunqObject>>(strResponse);

            var installationId = deserializedResponse.Get<Shared.Id>();
            var token = deserializedResponse.Get<Token>();
            var serverPublicKey = deserializedResponse.Get<ServerPublicKey>();

            session._serverPublicKey = session._decodePublicKey(serverPublicKey.PublicKey);

            session._installationId = installationId.ID;
            session._installationToken = token.TokenValue;

            // Now that the installation is set up, We can just use CallAsync and simplify a ton of code. See:

            var deviceServerId = (await session.CallAsync<Shared.Id>(HttpMethod.Post, "/v1/device-server", new DeviceServer.Post
            {
                Description = description,
                PermittedIPs = permittedIPs,
                Secret = apiKey
            })).Get<Shared.Id>().ID;

            // tadaaa!

            // and now make a session, just to be sure
            await session._controlSessionExpiry(true);

            return session;
        }

    }
}
