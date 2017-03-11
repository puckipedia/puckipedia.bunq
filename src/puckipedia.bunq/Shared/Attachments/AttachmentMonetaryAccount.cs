using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Shared
{
    public class AttachmentMonetaryAccount
    {
        [JsonProperty("id")] public int ID { get; private set; }
        [JsonProperty("monetary_account_id")] public int MonetaryAccountID { get; private set; }
    }
}
