using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.ApiCalls
{
    [BunqEntity("Pagination")]
    public class Pagination : BunqObject
    {
        [JsonProperty("future_url")]
        public string FutureURL { get; private set; }

        [JsonProperty("newer_url")]
        public string NewerURL { get; private set; }

        [JsonProperty("older_url")]
        public string OlderURL { get; private set; }
    }
}
