using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using puckipedia.bunq.ApiCalls;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace puckipedia.bunq.Tabs
{
    public enum CashRegisterStatus
    {
        PENDING_APPROVAL, ACTIVE, DENIED, CLOSED
    }

    [BunqEntity("CashRegister")]
    public class CashRegister : BunqObject
    {
        [JsonProperty("id")] public int ID { get; private set; }
        [JsonProperty("created")] public DateTime Created { get; private set; }
        [JsonProperty("updated")] public DateTime Updated { get; private set; }
        [JsonProperty("name")] public string Name { get; private set; }
        [JsonProperty("location")] public Shared.Geolocation? Location { get; private set; }
        [JsonProperty("notification_filters")] public Shared.NotificationFilter[] NotificationFilters { get; private set; }
        [JsonProperty("avatar")] public Shared.AnchoredAvatar Avatar { get; private set; }
        [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))] public CashRegisterStatus Status { get; private set; }

        public class Request : BunqRequest
        {
            [JsonProperty("name")] public string Name { get; set; }
            [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))] public CashRegisterStatus Status { get; set; }
            [JsonProperty("avatar_uuid")] public Guid AvatarUUID { get; set; }
            [JsonProperty("location")] public Shared.Geolocation? Location { get; set; }
            [JsonProperty("notification_filters")] public List<Shared.NotificationFilter> NotificationFilters { get; set; }
        }

        private int _monetaryAccountID;

        // workaround for bunq API limitations. xxx bunq: please add monetary_account_id into object.
        internal override void _hydrate(BunqSession session, object data)
        {
            if (data is int) _monetaryAccountID = (int)data;

            base._hydrate(session, data);
        }

        public async Task<CashRegister> Update(Request data)
        {
            return (await _session.CallAsync(HttpMethod.Put, $"/v1/user/{_session.User.ID}/monetary-account/{_monetaryAccountID}/cash-register/{ID}", data, this, _monetaryAccountID)).Get<CashRegister>();
        }

        public async Task<CashRegister> Refresh()
        {
            return (await _session.CallAsync(HttpMethod.Get, $"/v1/user/{_session.User.ID}/monetary-account/{_monetaryAccountID}/cash-register/{ID}", null, this, _monetaryAccountID)).Get<CashRegister>();
        }
    }

    public static class CashRegisterExtensions
    {
        public static async Task<BunqPaginatedResponse<CashRegister>> ListCashRegister(this MonetaryAccount.MonetaryAccountBank monetaryAccountBank)
        {
            return await monetaryAccountBank._session.ListAsync<CashRegister>($"/v1/user/{monetaryAccountBank._session.User.ID}/monetary-account/{monetaryAccountBank.ID}/cash-register", monetaryAccountBank.ID);
        }

        public static async Task<CashRegister> GetCashRegister(this MonetaryAccount.MonetaryAccountBank monetaryAccountBank, int id)
        {
            return (await monetaryAccountBank._session.CallAsync<CashRegister>(HttpMethod.Get, $"/v1/user/{monetaryAccountBank._session.User.ID}/monetary-account/{monetaryAccountBank.ID}/cash-register/{id}", null, null, monetaryAccountBank.ID)).Get<CashRegister>();
        }

        public static async Task<int> CreateCashRegister(this MonetaryAccount.MonetaryAccountBank monetaryAccountBank, CashRegister.Request data)
        {
            return (await monetaryAccountBank._session.CallAsync<Shared.Id>(HttpMethod.Post, $"/v1/user/{monetaryAccountBank._session.User.ID}/monetary-account/{monetaryAccountBank.ID}/cash-register", data)).Get<Shared.Id>().ID;
        }

        public static async Task<CashRegister> CreateAndGetCashRegister(this MonetaryAccount.MonetaryAccountBank monetaryAccountBank, CashRegister.Request data)
        {
            return await GetCashRegister(monetaryAccountBank, await CreateCashRegister(monetaryAccountBank, data));
        }
    }

}