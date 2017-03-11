using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.ApiCalls
{
    public enum DeviceServerStatus
    {
        ACTIVE, BLOCKED, NEEDS_CONFIRMATION, OBSOLETE
    }

    [BunqEntity("DeviceServer")]
    public class DeviceServer : BunqObject
    {
        public class Post : BunqRequest
        {
            [JsonProperty("description")] public string Description { get; set;  }
            [JsonProperty("permitted_ips")] public List<string> PermittedIPs { get; set;  }
            [JsonProperty("secret")] public string Secret { get; set;  }

        }

        [JsonProperty("id")] public int ID { get; private set; }
        [JsonProperty("created")] public DateTime Created { get; private set; }
        [JsonProperty("updated")] public DateTime Updated { get; private set; }
        [JsonProperty("description")] public string Description { get; private set; }
        [JsonProperty("ip")] public string IP { get; private set; }
        [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))] public DeviceServerStatus Status { get; private set; }
    }
}
