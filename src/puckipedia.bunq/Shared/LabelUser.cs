using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Shared
{
    public struct LabelUser
    {
        [JsonProperty("uuid")]
        public Guid UUID { get; private set; }

        [JsonProperty("avatar")]
        public Shared.Avatar Avatar { get; private set; }

        [JsonProperty("public_nick_name")]
        public string PublicNickName { get; private set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; private set; }

        [JsonProperty("country")]
        public string Country { get; private set; }
    }
}
