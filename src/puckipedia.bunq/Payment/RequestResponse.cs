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
    public enum RequestResponseType
    {
        DIRECT_DEBIT, IDEAL, INTERNAL
    }

    public enum RequestResponseSubType
    {
        NONE, ONCE, RECURRING
    }

    [BunqEntity("RequestResponse")]
    public class RequestResponse : BunqObject
    {
        [JsonProperty("id")] public int ID { get; private set; } // todo: this should probably be in the API
        [JsonProperty("time_responded")] public DateTime? TimeResponded { get; private set; }
        [JsonProperty("time_expiry")] public DateTime? TimeExpiry { get; private set; }
        [JsonProperty("created")] public DateTime Created { get; private set; } // todo: bunq: I bet this is in the API.
        [JsonProperty("updated")] public DateTime Updated { get; private set; }// todo: bunq: I bet this is in the API.
        [JsonProperty("monetary_account_id")] public int MonetaryAccountID { get; private set; }
        [JsonProperty("amount_inquired")] public Shared.CurrencyAmount AmountInquired { get; private set; }
        [JsonProperty("amount_responded")] public Shared.CurrencyAmount? AmountResponded { get; private set; }
        [JsonProperty("alias")] public Shared.LabelMonetaryAccount Alias { get; private set; }
        [JsonProperty("counterparty_alias")] public Shared.LabelMonetaryAccount CounterpartyAlias { get; private set; }
        [JsonProperty("description")] public string Description { get; private set; }
        [JsonProperty("attachment")] public Shared.Attachment[] Attachments { get; private set; } // todo: bunq: ????????????????????????????
        [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))] public RequestStatusType Status { get; private set; }
        [JsonProperty("minimum_age")] public int? MinimumAge { get; private set; }
        [JsonProperty("require_address"), JsonConverter(typeof(StringEnumConverter))] public RequireAddress RequireAddress { get; private set; }
        [JsonProperty("address_shipping")] public Shared.Address? AddressShipping { get; private set; }
        [JsonProperty("address_billing")] public Shared.Address? AddressBilling { get; private set; }
        [JsonProperty("geolocation")] public Shared.Geolocation? Geolocation { get; private set; }
        [JsonProperty("redirect_url")] public string RedirectURL { get; private set; }
        [JsonProperty("type"), JsonConverter(typeof(StringEnumConverter))] public RequestResponseType Type { get; private set; }
        [JsonProperty("sub_type"), JsonConverter(typeof(StringEnumConverter))] public RequestResponseSubType SubType { get; private set; }
        [JsonProperty("allow_chat")] public bool AllowChat { get; private set; }
        [JsonProperty("eligible_whitelist_id")] public int? EligibleWhitelistID { get; private set; }

        private class _statusUpdate : BunqRequest
        {
            [JsonProperty("amount_responded")] public Shared.CurrencyAmount? AmountResponded { get; set; }
            [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))] public RequestStatusType Status { get; set; }
            [JsonProperty("address_shipping")] public Shared.Address? AddressShipping { get; set; }
            [JsonProperty("address_billing")] public Shared.Address? AddressBilling { get; set; }
        }
        /// <summary>
        /// Accepts this RequestResponse
        /// </summary>
        /// <returns>The updated RequestResponse</returns>
        /// <remarks>Changes the object Accept() is called on too!</remarks>
        public async Task<RequestResponse> Accept(Shared.CurrencyAmount? amount_responded = null, Shared.Address? address_shipping = null, Shared.Address? address_billing = null)
        {
            return (await _session.CallAsync(HttpMethod.Put, $"/v1/user/{_session.User.ID}/monetary-account/{MonetaryAccountID}/request-response/{ID}",
                new _statusUpdate { Status = RequestStatusType.ACCEPTED, AmountResponded = amount_responded, AddressShipping = address_shipping, AddressBilling = address_billing }, this)).Get<RequestResponse>();
        }
        /// <summary>
        /// Rejects this RequestResponse
        /// </summary>
        /// <returns>The updated RequestResponse</returns>
        /// <remarks>Changes the object Reject() is called on too!</remarks>

        public async Task<RequestResponse> Reject()
        {
            return (await _session.CallAsync(HttpMethod.Put, $"/v1/user/{_session.User.ID}/monetary-account/{MonetaryAccountID}/request-response/{ID}", new _statusUpdate { Status = RequestStatusType.REJECTED }, this)).Get<RequestResponse>();
        }

        public async Task<RequestResponse> Refresh()
        {
            return (await _session.CallAsync(HttpMethod.Get, $"/v1/user/{_session.User.ID}/monetary-account/{MonetaryAccountID}/request-response/{ID}", null, this)).Get <RequestResponse > ();
        }
    }

    public static class RequestResponseExtensions
    {
        public static async Task<BunqPaginatedResponse<RequestResponse>> ListRequestResponse(this MonetaryAccountBank monetaryAccount, int? count = 10)
        {
            return await monetaryAccount._session.ListAsync<RequestResponse>($"/v1/user/{monetaryAccount._session.User.ID}/monetary-account/{monetaryAccount.ID}/request-response?count={count}");
        }

        public static async Task<RequestResponse> GetRequestResponse(this MonetaryAccountBank monetaryAccount, int id)
        {
            return (await monetaryAccount._session.CallAsync<RequestResponse>(HttpMethod.Get, $"/v1/user/{monetaryAccount._session.User.ID}/monetary-account/{monetaryAccount.ID}/request-response/{id}")).Get<RequestResponse>();
        }
    }
}
