using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Shared
{
    public class AttachmentBase : BunqObject
    {
        [JsonProperty("created")] public DateTime Created { get; private set; }
        [JsonProperty("updated")] public DateTime Updated { get; private set; }
        [JsonProperty("attachment")] public Attachment Attachment { get; private set; }
    }
}
