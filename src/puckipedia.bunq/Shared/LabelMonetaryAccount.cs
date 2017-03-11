using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Shared
{
    public struct LabelMonetaryAccount
    {
        [JsonProperty("iban")]
        public string IBAN { get; private set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; private set; }

        [JsonProperty("avatar")]
        public Shared.Avatar Avatar { get; private set; }

        [JsonProperty("label_user")]
        public Shared.LabelUser LabelUser { get; private set; }

        [JsonProperty("country")]
        public string Country { get; private set; }

        [JsonProperty("bunq_me")]
        public Shared.Pointer BunqMe { get; set; }
    }
}
