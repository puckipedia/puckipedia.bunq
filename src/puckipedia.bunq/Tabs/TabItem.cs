using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Tabs
{
    [BunqEntity("TabItem")]
    public class TabItem : BunqObject
    {
        [JsonProperty("id")] public int ID { get; private set; }
        [JsonProperty("description")] public string Description { get; private set; }
        [JsonProperty("ean_code")] public string EANCode { get; private set; }
        [JsonProperty("avatar_attachment")] public Shared.Attachment? AvatarAttachment { get; private set; }
        // xxx bunq: what is up with tab_attachment
        [JsonProperty("quantity")] public string Quantity { get; private set; } // xxx bunq: wrong in the API
        [JsonProperty("amount")] public Shared.CurrencyAmount Amount { get; private set; }
    }
}
