using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Tabs
{
    public enum TokenQrCashRegisterStatus
    {
        ACTIVE, INACTIVE
    }

    [BunqEntity("TokenQrCashRegister")]
    class TokenQrCashRegister : BunqObject
    {
        [JsonProperty("id")] public int ID { get; private set; }
        [JsonProperty("created")] public DateTime Created { get; private set; }
        [JsonProperty("updated")] public DateTime Updated { get; private set; }
        [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))] public TokenQrCashRegisterStatus Status { get; private set; }
        [JsonProperty("cash_register")] public CashRegister CashRegister { get; private set; }
        [JsonProperty("tab_object"), JsonConverter(typeof(BunqEntitySerializer))] public Tab TabObject { get; private set; } // xxx bunq: wrong in API!
    }
}
