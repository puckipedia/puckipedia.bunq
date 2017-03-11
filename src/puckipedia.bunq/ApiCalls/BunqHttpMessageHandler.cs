using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace puckipedia.bunq.ApiCalls
{
    class BunqHttpMessageHandler : DelegatingHandler
    {
        private BunqSession _session;

        public BunqHttpMessageHandler(BunqSession session, HttpMessageHandler innerHandler) : base(innerHandler)
        {
            _session = session;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains("X-Bunq-Client-Request-Id")) request.Headers.Add("X-Bunq-Client-Request-Id", Guid.NewGuid().ToString());
            var signature = await _session._buildSignature(request);
            request.Headers.Add("X-Bunq-Client-Signature", signature);

            var response = await base.SendAsync(request, cancellationToken);
            if (!await _session._verifySignature(response)) throw new Exception("Invalid signature on response!");

            return response;
        }
    }
}
