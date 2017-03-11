using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Tabs
{
    public struct TabVisibility
    {
        [JsonProperty("cash_register_qr_code")] public bool CashRegisterQrCode { get; private set; }
        [JsonProperty("tab_qr_code")] public bool TabQrCode { get; private set; }
        [JsonProperty("location")] public Shared.Geolocation? Location { get; private set; }
    }

    public class Tab : BunqObject
    {
        [JsonProperty("uuid")] public Guid UUID { get; private set; }
        [JsonProperty("created")] public DateTime Created { get; private set; }
        [JsonProperty("updated")] public DateTime Updated { get; private set; }
        [JsonProperty("description")] public string Description { get; private set; }
        [JsonProperty("amount_total")] public Shared.CurrencyAmount AmountTotal { get; private set; }
        [JsonProperty("qr_code_token")] public string QrCodeToken { get; private set; }
        [JsonProperty("tab_url")] public string TabURL { get; private set; }
        [JsonProperty("visibility")] public TabVisibility Visibility { get; private set; }
        [JsonProperty("minimum_age")] public int? MinimumAge { get; private set; }
        [JsonProperty("require_address")] public Payment.RequireAddress RequireAddress { get; private set; }
        [JsonProperty("redirect_url")] public string RedirectURL { get; private set; }
        [JsonProperty("expiration")] public DateTime? Expiration { get; private set; }
        [JsonProperty("alias")] public Shared.LabelMonetaryAccount Alias { get; private set; }
        [JsonProperty("cash_register_location")] public Shared.Geolocation CashRegisterLocation { get; private set; }
        [JsonProperty("tab_item")] public TabItem[] TabItems { get; private set; }
        [JsonProperty("tab_attachment")] public Shared.Id[] TabAttachments { get; private set; } // xxx bunq: probably wrong
    }
}
