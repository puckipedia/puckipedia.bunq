using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using puckipedia.bunq.ApiCalls;

namespace puckipedia.bunq.Tabs
{
    public enum TabUsageMultipleStatus
    {
        OPEN, PAYABLE, CLOSED
    }

    [BunqEntity("TabUsageMultiple")]
    public class TabUsageMultiple : Tab
    {
        [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))] public TabUsageMultipleStatus Status { get; private set; }

        private Tuple<int, int> _data;

        internal override void _hydrate(BunqSession session, object data)
        {
            if (data is Tuple<int, int>) _data = (Tuple<int, int>)data;

            base._hydrate(session, data);
        }
    }
}
