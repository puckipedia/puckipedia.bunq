using System;
using System.Collections.Generic;
using System.Text;

namespace puckipedia.bunq.ApiCalls
{
    public class BunqFatalError : Exception
    {
        public BunqFatalError() : base("A fatal error (500) occured on bunq's side.") { }
    }
}
