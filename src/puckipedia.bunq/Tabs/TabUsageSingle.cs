using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Tabs
{
    public enum TabUsageSingleStatus
    {
        OPEN, WAITING_FOR_PAYMENT, PAID, CANCELED
    }

    [BunqEntity("TabUsageSingle")]
    public class TabUsageSingle : Tab
    {
        [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))] public TabUsageSingleStatus Status { get; private set; } 
        [JsonProperty("amount_paid")] public Shared.CurrencyAmount? AmountPaid { get; private set; }
        [JsonProperty("merchant_reference")] public string MerchantReference { get; private set; }
    }
}
