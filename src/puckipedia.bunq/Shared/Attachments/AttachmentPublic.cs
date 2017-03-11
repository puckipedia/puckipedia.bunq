using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Shared
{
    [BunqEntity("AttachmentPublic")]
    public class AttachmentPublic : AttachmentBase
    {
        [JsonProperty("uuid")] public Guid UUID { get; private set; }
    }
}
