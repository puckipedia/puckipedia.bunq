using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using puckipedia.bunq.ApiCalls;
using puckipedia.bunq.MonetaryAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace puckipedia.bunq.Payment
{
    [BunqEntity("RequestInquiryBatch")]
    public class RequestInquiryBatch : BunqObject
    {
        [JsonProperty("id")] public int ID { get; private set; } // xxx bunq: this is responded, but not in the API.
        [JsonProperty("created")] public DateTime Created { get; private set; }
        [JsonProperty("updated")] public DateTime Updated { get; private set; }
        [JsonProperty("request_inquiries")] public RequestInquiry[] RequestInquiries { get; private set; }
        [JsonProperty("total_amount_inquired")] public Shared.CurrencyAmount TotalAmountInquired { get; private set; }

        public class Request : BunqRequest
        {
            [JsonProperty("request_inquiries")] public List<RequestInquiry.Request> RequestInquiries { get; set; }
            [JsonProperty("total_amount_inquired")] public Shared.CurrencyAmount TotalAmountInquired { get; set; }
        }

        private class _statusRequest : BunqRequest
        {
            [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))] public RequestStatusType Status { get; set; }
        }

        /// <summary>
        /// Revokes all RequestInquiries that are still open.
        /// </summary>
        /// <returns>The updated RequestInquiryBatch</returns>
        /// <remarks>Changes the object RevokeAll() is called on too!</remarks>

        public async Task<RequestInquiryBatch> RevokeAll()
        {
            return (await _session.CallAsync(HttpMethod.Put, $"/v1/user/{_session.User.ID}/monetary-account/{RequestInquiries.First().MonetaryAccountID}/request-inquiry-batch/{ID}", new _statusRequest { Status = RequestStatusType.REVOKED }, this)).Get<RequestInquiryBatch>();
        }

        public async Task<RequestInquiryBatch> Refresh()
        {
            return (await _session.CallAsync(HttpMethod.Get, $"/v1/user/{_session.User.ID}/monetary-account/{RequestInquiries.First().MonetaryAccountID}/request-inquiry-batch/{ID}", null, this)).Get<RequestInquiryBatch>();
        }
    }

    public static class RequestInquiryBatchExtensions
    {
        public static async Task<BunqPaginatedResponse<RequestInquiryBatch>> ListRequestInquiryBatch(this MonetaryAccountBank monetaryAccount, int? count = 10)
        {
            return await monetaryAccount._session.ListAsync<RequestInquiryBatch>($"/v1/user/{monetaryAccount._session.User.ID}/monetary-account/{monetaryAccount.ID}/request-inquiry-batch?count={count}");
        }

        public static async Task<RequestInquiryBatch> GetRequestInquiryBatch(this MonetaryAccountBank monetaryAccount, int id)
        {
            return (await monetaryAccount._session.CallAsync<RequestInquiryBatch>(HttpMethod.Get, $"/v1/user/{monetaryAccount._session.User.ID}/monetary-account/{monetaryAccount.ID}/request-inquiry-batch/{id}")).Get<RequestInquiryBatch>();
        }

        public static async Task<int> CreateRequestInquiryBatch(this MonetaryAccountBank monetaryAccount, RequestInquiryBatch.Request data)
        {
            return (await monetaryAccount._session.CallAsync<Shared.Id>(HttpMethod.Post, $"/v1/user/{monetaryAccount._session.User.ID}/monetary-account/{monetaryAccount.ID}/request-inquiry-batch", data)).Get<Shared.Id>().ID;
            // xxx bunq: this is wrong too.
        }

        public static async Task<RequestInquiryBatch> CreateAndGetRequestInquiryBatch(this MonetaryAccountBank monetaryAccount, RequestInquiryBatch.Request data)
        {
            return await GetRequestInquiryBatch(monetaryAccount, await CreateRequestInquiryBatch(monetaryAccount, data));
        }
    }
}
