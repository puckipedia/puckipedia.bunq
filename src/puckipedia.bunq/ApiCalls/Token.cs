using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.ApiCalls
{
    [BunqEntity("Token")]
    public class Token : BunqObject
    {
        [JsonProperty("id")] public int ID { get; private set; }
        [JsonProperty("created")] public DateTime Created { get; private set; }
        [JsonProperty("updated")] public DateTime Updated { get; private set; }
        [JsonProperty("token")] public string TokenValue { get; private set; }
    }
}
