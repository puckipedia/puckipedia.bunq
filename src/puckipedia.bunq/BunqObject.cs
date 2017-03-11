using Newtonsoft.Json;
using puckipedia.bunq.ApiCalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace puckipedia.bunq
{
    public class BunqObject
    {
        protected BunqObject() { }

        [JsonIgnore]
        internal BunqSession _session { get; set; }

        /// <summary>
        /// Sets this object's (and all its properties') _session value.
        /// </summary>
        /// <param name="session">The session to link this object to.</param>
        internal virtual void _hydrate(BunqSession session, object data)
        {
            if (_session == session) return; // avoid loops

            _session = session;
            foreach (var property in GetType().GetProperties())
            {
                if (typeof(BunqObject).IsAssignableFrom(property.GetType()))
                {
                    var obj = (BunqObject) property.GetMethod.Invoke(this, new object[] { });
                    obj._hydrate(session, data);
                }
            }
        }

        internal void _populate(BunqObject other)
        {
            if (other.GetType().IsAssignableFrom(GetType()))
            {
                foreach (var property in other.GetType().GetProperties())
                {
                    property.SetValue(this, property.GetValue(other));
                }
            }
        }
    }
}