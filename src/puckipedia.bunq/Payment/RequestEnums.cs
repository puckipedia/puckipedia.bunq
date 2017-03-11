using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.Payment
{
    public enum RequireAddress
    {
        NONE = 0,
        BILLING = 1,
        SHIPPING = 2,
        BILLING_SHIPPING = 3,
        OPTIONAL = 4
    }

    public enum RequestStatusType
    {
        PENDING,
        ACCEPTED,
        REJECTED,
        REVOKED
    }
}
