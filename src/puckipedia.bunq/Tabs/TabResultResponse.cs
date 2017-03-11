using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Tabs
{
    [BunqEntity("TabResultResponse")]
    public class TabResultResponse : BunqObject
    {
        [JsonProperty("tab"), JsonConverter(typeof(BunqEntitySerializer))] public Tab Tab { get; private set; } // xxx bunq: wrong in API docs
        [JsonProperty("payment")] public Payment.Payment Payment { get; private set; }
    }
}
