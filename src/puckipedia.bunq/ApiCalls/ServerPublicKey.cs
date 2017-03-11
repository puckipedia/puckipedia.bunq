using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.ApiCalls
{
    [BunqEntity("ServerPublicKey")]
    public class ServerPublicKey : BunqObject
    {
        [JsonProperty("server_public_key")] public string PublicKey { get; private set; }
    }
}
