using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Cards
{
    public enum CardDebitStatus
    {
        ACTIVE, DEACTIVATED, LOST, STOLEN, CANCELLED, EXPIRED, PIN_TRIES_EXCEEDED
    }

    public enum CardDebitOrderStatus
    {
        CARD_UPDATE_REQUESTED, CARD_UPDATE_SENT, CARD_UPDATE_ACCEPTED, ACCEPTED_FOR_PRODUCTION, DELIVERED_TO_CUSTOMER
    }

    public enum CardLimitType
    {
        CARD_LIMIT_CONTACTLESS, CARD_LIMIT_ATM, CARD_LIMIT_DIPPING, CARD_LIMIT_POS_ICC
    }

    public struct CardLimit
    {
        [JsonProperty("id")] public int ID { get; private set; }
        [JsonProperty("currency")] public string Currency { get; set; }
        [JsonProperty("daily_limit")] public string DailyLimit { get; set; }
        [JsonProperty("type"), JsonConverter(typeof(StringEnumConverter))] public CardLimitType Type { get; set; }
    }

    public struct CardMagStripePermission
    {
        [JsonProperty("expiry_time")] public DateTime ExpiryTime { get; set; }
    }

    public struct CardCountryPermission
    {
        [JsonProperty("id")] public int ID { get; private set; }
        [JsonProperty("country")] public string Country { get; set; }
        [JsonProperty("expiry_time")] public DateTime ExpiryTime { get; set; }
    }

    [BunqEntity("CardDebit")]
    public class CardDebit : Card
    {
        [JsonProperty("id")] public int ID { get; private set; }
        [JsonProperty("created")] public DateTime Created { get; private set; }
        [JsonProperty("updated")] public DateTime Updated { get; private set; }
        [JsonProperty("second_line")] public string SecondLine { get; private set; }
        [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))] public CardDebitStatus Status { get; private set; }
        [JsonProperty("order_status"), JsonConverter(typeof(StringEnumConverter))] public CardDebitStatus OrderStatus { get; private set; }
        [JsonProperty("expiry_date")] public DateTime ExpiryDate { get; private set; }
        [JsonProperty("name_on_card")] public string NameOnCard { get; private set; }
        [JsonProperty("limit")] public CardLimit[] Limits { get; private set; }
        [JsonProperty("mag_stripe_permission")] public CardMagStripePermission? MagStripePermission { get; private set; }
        [JsonProperty("country-permission")] public CardCountryPermission[] CountryPermissions { get; private set; }
        [JsonProperty("label_monetary_account_ordered")] public Shared.LabelMonetaryAccount MonetaryAccountOrdered { get; private set; }
        [JsonProperty("label_monetary_account_current")] public Shared.LabelMonetaryAccount MonetaryAccountCurrent { get; private set; }
    }
}
