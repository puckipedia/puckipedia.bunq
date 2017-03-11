using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using puckipedia.bunq.ApiCalls;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace puckipedia.bunq.User
{
    public enum UserStatus
    {
        ACTIVE, SIGNUP, RECOVERY
    }

    public enum UserSubStatus
    {
        NONE, FACE_RESET, APPROVAL, APPROVAL_DIRECTOR, APPROVAL_PARENT, APPROVAL_SUPPORT, COUNTER_IBAN, IDEAL, SUBMIT
    }

    public class User : BunqObject
    {
        [JsonProperty("id")] public int ID { get; private set; }
        [JsonProperty("created")] public DateTime Created { get; private set; }
        [JsonProperty("updated")] public DateTime Updated { get; private set; }
        [JsonProperty("alias")] public Shared.Pointer[] Aliases { get; private set; }
        [JsonProperty("avatar")] public Shared.Avatar Avatar { get; private set; }
        [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))] public UserStatus Status { get; private set; }
        [JsonProperty("sub_status"), JsonConverter(typeof(StringEnumConverter))] public UserSubStatus SubStatus { get; private set; }
        [JsonProperty("public_uuid")] public Guid PublicUUID { get; private set; }
        [JsonProperty("display_name")] public string DisplayName { get; private set; }
        [JsonProperty("public_nick_name")] public string PublicNickName { get; private set; }
        [JsonProperty("language")] public string Language { get; private set; }
        [JsonProperty("region")] public string Region { get; private set; }
        [JsonProperty("session_timeout")] public int SessionTimeout { get; private set; }
        [JsonProperty("daily_limit_without_confirmation_login")] public Shared.CurrencyAmount DailyLimitWithoutConfirmationLogin { get; private set; }
        [JsonProperty("notification_filters")] public Shared.NotificationFilter[] NotificationFilters { get; private set; }
        [JsonProperty("address_main")] public Shared.Address AddressMain { get; private set; }
        [JsonProperty("address_postal")] public Shared.Address AddressPostal { get; private set; }
        [JsonProperty("counter_bank_iban")] public string CounterBankIBAN { get; private set; }
        [JsonProperty("version_terms_of_service")] public string VersionTermsOfService { get; private set; }
    }



    public static class UserExtensions
    {
        /// <summary>
        /// Gets all users this session can read.
        /// </summary>
        /// <param name="session">The session to use</param>
        /// <param name="count">The amount of users in one page</param>
        /// <returns>A paginated response containing Users</returns>
        public static async Task<BunqPaginatedResponse<User>> UserGetAll(this BunqSession session, int? count = 10)
        {
            return await session.ListAsync<User>($"/v1/user?count={count}");
        }

        /// <summary>
        /// Gets a single User by ID
        /// </summary>
        /// <param name="session">The session to use</param>
        /// <param name="id">The ID of the User</param>
        /// <returns>A User</returns>
        public static async Task<User> UserGetById(this BunqSession session, int id)
        {
            return (await session.CallAsync<User>(HttpMethod.Get, $"/v1/user/{id}")).Get<User>();
        }
    }
}
