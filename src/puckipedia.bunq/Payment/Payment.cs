using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using puckipedia.bunq.ApiCalls;
using puckipedia.bunq.MonetaryAccount;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace puckipedia.bunq.Payment
{
    public enum PaymentType
    {
        UNKNOWN, BUNQ, EBA_SCT, EBA_SDD, IDEAL, SWIFT, FIS
    }

    public enum PaymentSubType
    {
        UNKNOWN, PAYMENT, WITHDRAWAL, REVERSAL, REQUEST, BILLING, SCT, SDD, NLO
    }

    [BunqEntity("Payment")]
    public class Payment : PaymentOrPaymentBatch
    {
        [JsonProperty("id")] public int ID { get; private set; }
        [JsonProperty("created")] public DateTime Created { get; private set; }
        [JsonProperty("updated")] public DateTime Updated { get; private set; }
        [JsonProperty("monetary_account_id")] public int MonetaryAccountID { get; private set; }
        [JsonProperty("amount")] public Shared.CurrencyAmount Amount { get; private set; }
        [JsonProperty("alias")] public Shared.LabelMonetaryAccount Alias { get; private set; }
        [JsonProperty("counterparty_alias")] public Shared.LabelMonetaryAccount CounterpartyAlias { get; private set; }
        [JsonProperty("description")] public string Description { get; private set; }
        [JsonProperty("type"), JsonConverter(typeof(StringEnumConverter))] public PaymentType Type { get; private set; }
        [JsonProperty("sub_type"), JsonConverter(typeof(StringEnumConverter))] public PaymentSubType SubType { get; private set; }
        [JsonProperty("bunqto_status")] public string BunqToStatus { get; private set; } // xxx bunq: what statuses?
        [JsonProperty("bunqto_sub_status")] public string BunqToSubStatus { get; private set; }
        [JsonProperty("bunqto_share_url")] public string BunqToShareUrl { get; private set; }
        [JsonProperty("bunqto_expiry")] public DateTime? BunqToExpiry { get; private set; }
        [JsonProperty("bunqto_time_responded")] public DateTime? BunqToTimeResponded { get; private set; }
        [JsonProperty("attachment")] public Shared.AttachmentMonetaryAccount[] Attachments { get; private set; }
        [JsonProperty("merchant_reference")] public string MerchantReference { get; private set; }
        [JsonProperty("batch_id")] public int? BatchID { get; private set; }
        [JsonProperty("scheduled_id")] public int? ScheduledID { get; private set; }
        [JsonProperty("address_shipping")] public Shared.Address? AddressShipping { get; private set; }
        [JsonProperty("address_billing")] public Shared.Address? AddressBilling { get; private set; }
        [JsonProperty("geolocation")] public Shared.Geolocation? Geolocation { get; private set; }
        [JsonProperty("allow_chat")] public bool AllowChat { get; private set; }

        public class Request : BunqRequest
        {
            [JsonProperty("amount")] public Shared.CurrencyAmount Amount { get; set; }
            [JsonProperty("counterparty_alias")] public Shared.Pointer CounterpartyAlias { get; set; }
            [JsonProperty("description")] public string Description { get; set; }
            [JsonProperty("attachment")] public List<Shared.Id> Attachment { get; set; }
            [JsonProperty("merchant_reference")] public string MerchantReference { get; set; }
        }
    }

    public static class PaymentExtensions
    {
        public static async Task<BunqPaginatedResponse<Payment>> ListPayment(this MonetaryAccountBank monetaryAccount, int? count = 10)
        {
            return await monetaryAccount._session.ListAsync<Payment>($"/v1/user/{monetaryAccount._session.User.ID}/monetary-account/{monetaryAccount.ID}/payment?count={count}");
        }

        public static async Task<Payment> GetPayment(this MonetaryAccountBank monetaryAccount, int id)
        {
            return (await monetaryAccount._session.CallAsync<Payment>(HttpMethod.Get, $"/v1/user/{monetaryAccount._session.User.ID}/monetary-account/{monetaryAccount.ID}/payment/{id}")).Get<Payment>();
        }

        public static async Task<int> CreatePayment(this MonetaryAccountBank monetaryAccount, Payment.Request data)
        {
            return (await monetaryAccount._session.CallAsync<Shared.Id>(HttpMethod.Post, $"/v1/user/{monetaryAccount._session.User.ID}/monetary-account/{monetaryAccount.ID}/payment", data)).Get<Shared.Id>().ID;
        }

        public static async Task<Payment> CreateAndGetPayment(this MonetaryAccountBank monetaryAccount, Payment.Request data)
        {
            return await GetPayment(monetaryAccount, await CreatePayment(monetaryAccount, data));
        }
    }
}
