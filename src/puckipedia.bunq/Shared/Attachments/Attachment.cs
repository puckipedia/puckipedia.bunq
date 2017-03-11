using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Shared
{
    public struct Attachment
    {
        [JsonProperty("uuid")] public Guid UUID { get; private set; }
        [JsonProperty("description")] public string Description { get; private set; }
        [JsonProperty("content_type")] public string ContentType { get; private set; }
    }
}
