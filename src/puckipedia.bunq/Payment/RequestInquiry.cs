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
    [BunqEntity("RequestInquiry")]
    public class RequestInquiry : BunqObject
    {
        [JsonProperty("id")] public int ID { get; private set; }
        [JsonProperty("created")] public DateTime Created { get; private set; }
        [JsonProperty("updated")] public DateTime Updated { get; private set; }
        [JsonProperty("time_responded")] public DateTime? TimeResponded { get; private set; }
        [JsonProperty("time_expiry")] public DateTime? TimeExpiry { get; private set; }
        [JsonProperty("monetary_account_id")] public int MonetaryAccountID { get; private set; }
        [JsonProperty("amount_inquired")] public Shared.CurrencyAmount AmountInquired { get; private set; }
        [JsonProperty("amount_responded")] public Shared.CurrencyAmount? AmountResponded { get; private set; }
        [JsonProperty("user_alias_created")] public Shared.LabelUser UserCreated { get; private set; }
        [JsonProperty("user_alias_revoked")] public Shared.LabelUser? UserRevoked { get; private set; }
        [JsonProperty("counterparty_alias")] public Shared.LabelMonetaryAccount CounterpartyAlias { get; private set; }
        [JsonProperty("description")] public string Description { get; private set; }
        [JsonProperty("merchant_reference")] public string MerchantReference { get; private set; }
        [JsonProperty("attachment")] public Shared.Id[] Attachments { get; private set; }
        [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))] public RequestStatusType Status { get; private set; }
        [JsonProperty("batch_id")] public int? BatchID { get; private set; }
        [JsonProperty("scheduled_id")] public int? ScheduledID { get; private set; }
        [JsonProperty("minimum_age")] public int? MinimumAge { get; private set; }
        [JsonProperty("require_address"), JsonConverter(typeof(StringEnumConverter))] public RequireAddress RequireAddress { get; private set; }
        [JsonProperty("redirect_url")] public string RedirectURL { get; private set; }
        [JsonProperty("address_shipping")] public Shared.Address? AddressShipping { get; private set; }
        [JsonProperty("address_billing")] public Shared.Address? AddressBilling { get; private set; }
        [JsonProperty("geolocation")] public Shared.Geolocation? Geolocation { get; private set; }
        [JsonProperty("allow_chat")] public bool AllowChat { get; private set; }

        public class Request : BunqRequest
        {
            [JsonProperty("amount_inquired")] public Shared.CurrencyAmount AmountInquired { get; set; }
            [JsonProperty("counterparty_alias")] public Shared.Pointer CounterpartyAlias { get; set; }
            [JsonProperty("description")] public string Description { get; set; }
            [JsonProperty("attachment")] public List<Shared.Id> Attachments { get; set; }
            [JsonProperty("merchant_reference")] public string MerchantReference { get; set; }
            [JsonProperty("minimum_age")] public int? MinimumAge { get; set; }
            [JsonProperty("require_address"), JsonConverter(typeof(StringEnumConverter))] public RequireAddress RequireAddress { get; set; }
            [JsonProperty("allow_bunqme")] public bool AllowBunqMe { get; set; } = true;
        }

        private class _updateStatus : BunqRequest
        {
            [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))] public RequestStatusType Status { get; set; }
        }

        /// <summary>
        /// Revokes this RequestInquiry
        /// </summary>
        /// <returns>The updated RequestInquiry</returns>
        /// <remarks>Changes the object Revoke() is called on too!</remarks>
        public async Task<RequestInquiry> Revoke()
        {
            return (await _session.CallAsync(HttpMethod.Put, $"/v1/user/{_session.User.ID}/monetary-account/{MonetaryAccountID}/request-inquiry/{ID}", new _updateStatus { Status = RequestStatusType.REVOKED }, this)).Get<RequestInquiry>();
        }

        public async Task<RequestInquiry> Refresh()
        {
            return (await _session.CallAsync(HttpMethod.Get, $"/v1/user/{_session.User.ID}/monetary-account/{MonetaryAccountID}/request-inquiry/{ID}", null, this)).Get <RequestInquiry > ();
        }
    }

    public static class RequestInquiryExtensions
    {
        public static async Task<BunqPaginatedResponse<RequestInquiry>> ListRequestInquiry(this MonetaryAccountBank monetaryAccount, int? count = 10)
        {
            return await monetaryAccount._session.ListAsync<RequestInquiry>($"/v1/user/{monetaryAccount._session.User.ID}/monetary-account/{monetaryAccount.ID}/request-inquiry?count={count}");
        }

        public static async Task<RequestInquiry> GetRequestInquiry(this MonetaryAccountBank monetaryAccount, int id)
        {
            return (await monetaryAccount._session.CallAsync<RequestInquiry>(HttpMethod.Get, $"/v1/user/{monetaryAccount._session.User.ID}/monetary-account/{monetaryAccount.ID}/request-inquiry/{id}")).Get<RequestInquiry>();
        }

        public static async Task<int> CreateRequestInquiry(this MonetaryAccountBank monetaryAccount, RequestInquiry.Request data)
        {
            return (await monetaryAccount._session.CallAsync<Shared.Id>(HttpMethod.Post, $"/v1/user/{monetaryAccount._session.User.ID}/monetary-account/{monetaryAccount.ID}/request-inquiry", data)).Get<Shared.Id>().ID;
        }

        public static async Task<RequestInquiry> CreateAndGetRequestInquiry(this MonetaryAccountBank monetaryAccount, RequestInquiry.Request data)
        {
            return await GetRequestInquiry(monetaryAccount, await CreateRequestInquiry(monetaryAccount, data));
        }
    }
}
