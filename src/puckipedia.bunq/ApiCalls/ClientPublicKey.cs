using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.ApiCalls
{
    [BunqEntity("ClientPublicKey")]
    public class ClientPublicKey : BunqObject
    {
        [JsonProperty("client_public_key")] public string PublicKey { get; set;  }
    }
}
