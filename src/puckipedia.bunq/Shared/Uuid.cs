using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Shared
{
    [BunqEntity("Uuid")]
    public class Uuid : BunqObject
    {
        [JsonProperty("uuid")]
        public Guid UUID { get; set; }
    }
}
