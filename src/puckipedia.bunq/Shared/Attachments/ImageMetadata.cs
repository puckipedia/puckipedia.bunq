using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Shared
{
    public class ImageMetadata
    {
        [JsonProperty("attachment_public_uuid")] public Guid AttachmentPublicUUID { get; private set; }
        [JsonProperty("content_type")] public string ContentType { get; private set; }
        [JsonProperty("width")] public int Width { get; private set; }
        [JsonProperty("height")] public int Height { get; private set; }
    }
}
