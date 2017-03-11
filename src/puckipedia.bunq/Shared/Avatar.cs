using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Shared
{
    public class Avatar
    {
        [JsonProperty("uuid")] public Guid UUID { get; private set; }
        [JsonProperty("image")] public ImageMetadata[] Image { get; private set; }
    }
}
