using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Shared
{
    public struct Address
    {
        [JsonProperty("street")] public string Street { get; private set; }
        [JsonProperty("house_number")] public string HouseNumber { get; private set; }
        [JsonProperty("po_box")] public string POBox { get; private set; }
        [JsonProperty("postal_code")] public string PostalCode { get; private set; }
        [JsonProperty("city")] public string City { get; private set; }
        [JsonProperty("country")] public string Country { get; private set; }
        [JsonProperty("province")] public string Province { get; private set; }
    }
}
