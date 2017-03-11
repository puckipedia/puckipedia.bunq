using Newtonsoft.Json;
using puckipedia.bunq.ApiCalls;
using puckipedia.bunq.MonetaryAccount;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace puckipedia.bunq.Payment
{
    [BunqEntity("DraftPayment")]
    public class DraftPayment : BunqObject
    {
        public struct Response
        {
            [JsonProperty("status")] public string Status { get; private set; }
            [JsonProperty("user_alias_created")] public Shared.LabelUser UserAliasCreated { get; private set; }
        }

        public struct Entry
        {
            [JsonProperty("id")] public int ID { get; private set; }
            [JsonProperty("amount")] public Shared.CurrencyAmount Amount { get; private set; }
            [JsonProperty("alias")] public Shared.LabelMonetaryAccount Alias { get; private set; }
            [JsonProperty("counterparty_alias")] public Shared.LabelMonetaryAccount CounterpartyAlias { get; private set; }
            [JsonProperty("description")] public string Description { get; private set; }
            [JsonProperty("merchant_reference")] public string MerchantReference { get; private set; }
            [JsonProperty("type")] public string Type { get; private set; } // xxx bunq: what types?
            [JsonProperty("attachment")] public List<Shared.Id> Attachment { get; private set; }
        }

        [JsonProperty("id")] public int ID { get; private set; }
        [JsonProperty("monetary_account_id")] public int MonetaryAccountID { get; private set; }
        [JsonProperty("user_alias_created")] public Shared.LabelUser UserAliasCreated { get; private set; }
        [JsonProperty("responses")] public DraftPayment.Response[] Responses { get; private set; }
        [JsonProperty("status")] public string Status { get; private set; } // xxx bunq: what statuses?
        [JsonProperty("type")] public string Type { get; private set; } // xxx bunq: same here
        [JsonProperty("entries")] public DraftPayment.Entry[] Entries { get; private set; }

        [JsonConverter(typeof(BunqEntitySerializer))]
        [JsonProperty("object")]
        public PaymentOrPaymentBatch ResultObject { get; private set; }

        public class Request : BunqRequest
        {
            [JsonProperty("status")] public string Status { get; set; } // xxx bunq: same here
            [JsonProperty("entries")] public List<Entry> Entries { get; set; }
            [JsonProperty("previous_updated_timestamp")] public DateTime PreviousUpdatedTimestamp { get; set; }
            [JsonProperty("number_of_required_accepts")] public int? NumberOfRequiredAccepts { get; set; }
        }

        /// <summary>
        /// Updates this DraftPayment
        /// </summary>
        /// <param name="data">The parameters to change</param>
        /// <returns>The updated DraftPayment</returns>
        /// <remarks>The object Update is called on is also updated.</remarks>
        public async Task<DraftPayment> Update(Request data)
        {
            return (await _session.CallAsync(HttpMethod.Put, $"/v1/user/{_session.User.ID}/monetary-account/{MonetaryAccountID}/draft-payment/{ID}", data, this)).Get<DraftPayment>();
        }

        public async Task<DraftPayment> Refresh()
        {
            return (await _session.CallAsync(HttpMethod.Get, $"/v1/user/{_session.User.ID}/monetary-account/{MonetaryAccountID}/draft-payment/{ID}", null, this)).Get <DraftPayment > ();
        }
    }


    public static class DraftPaymentExtensions
    {
        /// <summary>
        /// Gets all DraftPayments
        /// </summary>
        /// <param name="monetaryAccount">The MonetaryAccount to get DraftPayments for</param>
        /// <param name="count">The amount to get in one page</param>
        /// <returns>A paginated response containing DraftPayments</returns>
        public static async Task<BunqPaginatedResponse<DraftPayment>> ListDraftPayment(this MonetaryAccountBank monetaryAccount, int? count = 10)
        {
            return await monetaryAccount._session.ListAsync<DraftPayment>($"/v1/user/{monetaryAccount._session.User.ID}/monetary-account/{monetaryAccount.ID}/draft-payment?count={count}");
        }

        /// <summary>
        /// Gets a single DraftPayment by ID
        /// </summary>
        /// <param name="monetaryAccount">The MonetaryAccount to get the DraftPayment for</param>
        /// <param name="id">The ID of the DraftPayment</param>
        /// <returns>The DraftPayment</returns>
        public static async Task<DraftPayment> GetDraftPayment(this MonetaryAccountBank monetaryAccount, int id)
        {
            return (await monetaryAccount._session.CallAsync<DraftPayment>(HttpMethod.Get, $"/v1/user/{monetaryAccount._session.User.ID}/monetary-account/{monetaryAccount.ID}/draft-payment/{id}")).Get<DraftPayment>();
        }

        /// <summary>
        /// Creates a new DraftPayment
        /// </summary>
        /// <param name="monetaryAccount">The MonetaryAccount to create the DraftPayment for</param>
        /// <param name="data">The data to create the payment with</param>
        /// <returns>The ID of the new DraftPayment</returns>
        public static async Task<int> CreateDraftPayment(this MonetaryAccountBank monetaryAccount, DraftPayment.Request data)
        {
            return (await monetaryAccount._session.CallAsync<Shared.Id>(HttpMethod.Post, $"/v1/user/{monetaryAccount._session.User.ID}/monetary-account/{monetaryAccount.ID}/draft-payment", data)).Get<Shared.Id>().ID;
        }

        public static async Task<DraftPayment> CreateAndGetDraftPayment(this MonetaryAccountBank monetaryAccount, DraftPayment.Request data)
        {
            return await GetDraftPayment(monetaryAccount, await CreateDraftPayment(monetaryAccount, data));
        }
    }
}
