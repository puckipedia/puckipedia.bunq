using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Shared
{
    public enum NotificationFilterDeliveryMethod
    {
        URL, PUSH
    }

    public enum NotificationFilterCategory
    {
        BILLING, CARD_TRANSACTION_FAILED, CARD_TRANSACTION_SUCCESSFUL, CHAT, DRAFT_PAYMENT, IDEAL, SOFORT, FRIEND_SIGN_UP, MONETARY_ACCOUNT_PROFILE, MUTATION, PAYMENT, PROMOTION, REQUEST, SCHEDULE_RESULT, SCHEDULE_STATUS, SHARE, SUPPORT, TAB_RESULT, USER_APPROVAL,

        WHITELIST, WHITELIST_RESULT, SLICE_CHAT, SLICE_REGISTRY_ENTRY, SLICE_REGISTRY_MEMBERSHIP, SLICE_REGISTRY_SETTLEMENT
    }

    public class NotificationFilter
    {
        [JsonProperty("notification_delivery_method"), JsonConverter(typeof(StringEnumConverter))] public NotificationFilterDeliveryMethod NotificationDeliveryMethod { get; private set; }
        [JsonProperty("notification_target")] public string NotificationTarget { get; private set; }
        [JsonProperty("category"), JsonConverter(typeof(StringEnumConverter))] public NotificationFilterCategory Category { get; private set; }
    }
}
