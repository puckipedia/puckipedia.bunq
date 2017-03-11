using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.User
{
    [BunqEntity("UserCompany")]
    public class UserCompany : User
    {
        public class UBO
        {
            [JsonProperty("name")] public string Name { get; private set; }
            [JsonProperty("date_of_birth")] public DateTime DateOfBirth { get; private set; }
            [JsonProperty("nationality")] public string Nationality { get; private set; }
        }

        [JsonProperty("name")] public string Name { get; private set; }
        [JsonProperty("chamber_of_commerce_number")] public string ChamberOfCommerceNumber { get; private set; }
        [JsonProperty("director_alias")] public Shared.LabelUser DirectorAlias { get; private set; }
        [JsonProperty("ubo")] public UBO[] UBOs { get; private set; }
    }
}
