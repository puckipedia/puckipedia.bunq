using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Tabs
{
    [BunqEntity("TabResultInquiry")]
    public class TabResultInquiry : BunqObject
    {
        [JsonProperty("tab"), JsonConverter(typeof(BunqEntitySerializer))] public Tab Tab { get; private set; } // xxx bunq/puck: what the heck
        [JsonProperty("payment")] public Payment.Payment Payment { get; private set; }
    }
}
