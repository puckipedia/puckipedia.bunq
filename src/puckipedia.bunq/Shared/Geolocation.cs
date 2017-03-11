using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Shared
{
    public struct Geolocation
    {
        [JsonProperty("latitude")]
        public float Latitude { get; private set; }

        [JsonProperty("longitude")]
        public float Longitude { get; private set; }

        [JsonProperty("altitude")]
        public float Altitude { get; private set; }

        [JsonProperty("radius")]
        public float Radius { get; private set; }
    }
}
