using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using puckipedia.bunq.ApiCalls;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace puckipedia.bunq.MonetaryAccount
{
    public enum MonetaryAccountBankStatus
    {
        ACTIVE, BLOCKED, CANCELLED, PENDING_REOPEN
    }

    public enum MonetaryAccountBankSubStatus
    {
        // ACTIVE, PENDING_REOPEN
        NONE,

        // BLOCKED
        COMPLETELY, ONLY_ACCEPTING_INCOMING,

        // CANCELLED
        REDEMPTION_INVOLUNTARY, REDEMPTION_VOLUNTARY, PERMANENT
    }

    public enum MonetaryAccountBankCloseReason
    {
        OTHER
    }

    public enum MonetaryAccountBankAvatarStatus
    {
        AVATAR_DEFAULT, AVATAR_CUSTOM, AVATAR_UNDETERMINED
    }

    public enum MonetaryAccountBankRestrictionChat
    {
        ALLOW_INCOMING, BLOCK_INCOMING
    }

    [BunqEntity("MonetaryAccountBank")]
    public class MonetaryAccountBank : MonetaryAccount
    {
        [JsonProperty("id")] public int ID { get; private set; }
        [JsonProperty("created")] public DateTime Created { get; private set; }
        [JsonProperty("updated")] public DateTime Updated { get; private set; }
        [JsonProperty("avatar")] public Shared.Avatar Avatar { get; private set; }
        [JsonProperty("currency")] public string Currency { get; private set; }
        [JsonProperty("description")] public string Description { get; private set; }
        [JsonProperty("daily_limit")] public Shared.CurrencyAmount DailyLimit { get; private set; }
        [JsonProperty("daily_spent")] public Shared.CurrencyAmount DailySpent { get; private set; }
        [JsonProperty("overdraft_limit")] public Shared.CurrencyAmount OverdraftLimit { get; private set; }
        [JsonProperty("balance")] public Shared.CurrencyAmount Balance { get; private set; }
        [JsonProperty("alias")] public Shared.Pointer[] Aliases { get; private set; }
        [JsonProperty("public_uuid")] public Guid PublicUUID { get; private set; }
        [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))] public MonetaryAccountBankStatus Status { get; private set; }
        [JsonProperty("sub_status"), JsonConverter(typeof(StringEnumConverter))] public MonetaryAccountBankSubStatus SubStatus { get; private set; }
        [JsonProperty("reason")] public MonetaryAccountBankCloseReason? CloseReason { get; private set; }
        [JsonProperty("reason_description")] public string CloseDescription { get; private set; }
        [JsonProperty("user_id")] public int UserId { get; private set; }
        // todo: monetary_account_profile

        [JsonProperty("notification_filters")] public Shared.NotificationFilter[] NotificationFilters { get; private set; }

        public struct BankAccountSettings
        {
            [JsonProperty("color")] public string Color { get; private set; }
            [JsonProperty("default_avatar_status"), JsonConverter(typeof(StringEnumConverter))] public MonetaryAccountBankAvatarStatus AvatarStatus { get; private set; }
            [JsonProperty("restriction_chat"), JsonConverter(typeof(StringEnumConverter))] public MonetaryAccountBankRestrictionChat RestrictionChat { get; private set; }
        }

        [JsonProperty("setting")] public BankAccountSettings Settings { get; private set; }

        public class Request : BunqRequest
        {
            [JsonProperty("currency")] public string Currency { get; set; }
            [JsonProperty("description")] public string Description { get; set; }
            [JsonProperty("daily_limit")] public Shared.CurrencyAmount? DailyLimit { get; set; }
            [JsonProperty("avatar_uuid")] public string AvatarUUID { get; set; }
            [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))] public MonetaryAccountBankStatus? Status { get; set; }
            [JsonProperty("sub_status"), JsonConverter(typeof(StringEnumConverter))] public MonetaryAccountBankSubStatus? SubStatus { get; set; }
            [JsonProperty("reason"), JsonConverter(typeof(StringEnumConverter))] public MonetaryAccountBankCloseReason? CloseReason { get; set; }
            [JsonProperty("reason_description")] public string CloseDescription { get; set; }
            [JsonProperty("notification_filters")] public List<Shared.NotificationFilter> NotificationFilters { get; set; }
            [JsonProperty("setting")] public BankAccountSettings? Settings { get; set; }
        }

        /// <summary>
        /// Updates this MonetaryAccountBank's settings
        /// </summary>
        /// <param name="data">The settings to change</param>
        /// <returns>The updated MonetaryAccountBank</returns>
        /// <remarks>the object Update() is called on is also updated with the new parameters.</remarks>
        public async Task<MonetaryAccountBank> Update(Request data)
        {
            return (await _session.CallAsync(HttpMethod.Put, $"/v1/user/{_session.User.ID}/monetary-account-bank/{ID}", data, this)).Get<MonetaryAccountBank>();
        }

        public async Task<MonetaryAccountBank> Refresh()
        {
            return (await _session.CallAsync(HttpMethod.Get, $"/v1/user/{_session.User.ID}/monetary-account-bank/{ID}", null, this)).Get<MonetaryAccountBank>();
        }
    }

    public static class MonetaryAccountBankExtensions
    {
        /// <summary>
        /// Gets the newest {count} MonetaryAccountBanks that the user of this Session can see.
        /// </summary>
        /// <param name="session">The session to use.</param>
        /// <param name="count">The amount of MonteryAccountBanks to get in one call.</param>
        /// <returns>A paginated response containing MonetaryAccountBanks</returns>
        public static async Task<BunqPaginatedResponse<MonetaryAccountBank>> ListMonetaryAccountBank(this BunqSession session, int count = 10)
        {
            return await session.ListAsync<MonetaryAccountBank>($"/v1/user/{session.User.ID}/monetary-account-bank?count={count}");
        }

        /// <summary>
        /// Gets a specific MonetaryAccountBank by numeric ID.
        /// </summary>
        /// <param name="session">The session to use.</param>
        /// <param name="id">The ID of the MonetaryAccountBank to look up</param>
        /// <returns>A MonetaryAccountBank</returns>
        public static async Task<MonetaryAccountBank> GetMonetaryAccountBank(this BunqSession session, int id)
        {
            return (await session.CallAsync<MonetaryAccountBank>(HttpMethod.Get, $"/v1/user/{session.User.ID}/monetary-account-bank/{id}")).Get<MonetaryAccountBank>();
        }

        /// <summary>
        /// Creates a new MonetaryAccountBank.
        /// </summary>
        /// <param name="session">The Session to use.</param>
        /// <param name="data">The parameters to set up this new MonetaryBankAccount.</param>
        /// <returns>A new MonetaryAccountBank</returns>
        public static async Task<MonetaryAccountBank> CreateMonetaryAccountBank(this BunqSession session, MonetaryAccountBank.Request data)
        {
            return (await session.CallAsync<MonetaryAccountBank>(HttpMethod.Post, $"/v1/user/{session.User.ID}/monetary-account-bank", data)).Get<MonetaryAccountBank>();
        }
    }
}
