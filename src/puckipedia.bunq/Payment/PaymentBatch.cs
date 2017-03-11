using Newtonsoft.Json;
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
    [BunqEntity("PaymentBatch")]
    public class PaymentBatch : PaymentOrPaymentBatch
    {
        [JsonProperty("id")] public int ID { get; private set; } // xxx bunq: not documented
        [JsonProperty("created")] public DateTime Created { get; private set; }
        [JsonProperty("updated")] public DateTime Updated { get; private set; }
        [JsonProperty("payments")] public Payment[] Payments { get; private set; }

        public class Request : BunqRequest
        {
            [JsonProperty("payments")] public List<Payment.Request> Payments { get; set; }
        }

        public async Task<PaymentBatch> Refresh()
        {
            return (await _session.CallAsync(HttpMethod.Get, $"/v1/user/{_session.User.ID}/monetary-account/{Payments.First().MonetaryAccountID}/payment-batch/{ID}", null, this)).Get<PaymentBatch>();
        }
    }

    public static class PaymentBatchExtensions
    {
        public static async Task<BunqPaginatedResponse<PaymentBatch>> ListPaymentBatch(this MonetaryAccountBank monetaryAccount, int? count = 10)
        {
            return await monetaryAccount._session.ListAsync<PaymentBatch>($"/v1/user/{monetaryAccount._session.User.ID}/monetary-account/{monetaryAccount.ID}/payment-batch?count={count}");
        }

        public static async Task<PaymentBatch> GetPaymentBatch(this MonetaryAccountBank monetaryAccount, int id)
        {
            return (await monetaryAccount._session.CallAsync<PaymentBatch>(HttpMethod.Get, $"/v1/user/{monetaryAccount._session.User.ID}/monetary-account/{monetaryAccount.ID}/payment-batch/{id}")).Get<PaymentBatch>();
        }

        public static async Task<int> CreatePaymentBatch(this MonetaryAccountBank monetaryAccount, PaymentBatch.Request data)
        {
            return (await monetaryAccount._session.CallAsync<Shared.Id>(HttpMethod.Post, $"/v1/user/{monetaryAccount._session.User.ID}/monetary-account/{monetaryAccount.ID}/payment-batch", data)).Get<Shared.Id>().ID;
        }

        public static async Task<PaymentBatch> CreateAndGetPaymentBatch(this MonetaryAccountBank monetaryAccount, PaymentBatch.Request data)
        {
            return await GetPaymentBatch(monetaryAccount, await CreatePaymentBatch(monetaryAccount, data));
        }
    }
}
