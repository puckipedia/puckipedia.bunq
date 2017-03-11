using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Shared
{
    public class AnchoredAvatar : Avatar
    {
        [JsonProperty("anchor_uuid")] public Guid AnchorUUID { get; private set; }
    }
}
