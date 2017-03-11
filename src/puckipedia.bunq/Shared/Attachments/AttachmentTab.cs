using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Shared
{
    [BunqEntity("AttachmentTab")]
    public class AttachmentTab : AttachmentBase
    {
        [JsonProperty("id")] public int ID { get; private set; }
    }
}
