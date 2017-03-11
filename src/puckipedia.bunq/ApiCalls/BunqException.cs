using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.ApiCalls
{
    public class BunqException : Exception
    {
        public string TranslatedMessage { get; }

        public BunqException(BunqResponseErrorData data) : base(data.ErrorDescription) { TranslatedMessage = data.ErrorDescriptionTranslated; }
    }
}
