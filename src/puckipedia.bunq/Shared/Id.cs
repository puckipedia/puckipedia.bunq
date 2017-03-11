using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Shared
{
    [BunqEntity("Id")]
    public class Id : BunqObject
    {
        [JsonProperty("id")]
        public int ID { get; set; }
    }
}
