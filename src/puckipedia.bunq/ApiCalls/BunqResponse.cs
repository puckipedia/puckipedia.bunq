using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace puckipedia.bunq.ApiCalls
{

    public struct BunqResponseErrorData
    {
        [JsonProperty("error_description")] public string ErrorDescription { get; set; }
        [JsonProperty("error_description_translated")] public string ErrorDescriptionTranslated { get; set; }
    }

    /// <summary>
    /// The response of a bunq API call.
    /// </summary>
    /// <typeparam name="T">The base type of BunqObject that is in the response array.</typeparam>
    [JsonObject]
    public class BunqResponse<T> : IEnumerable<T> where T : BunqObject
    {

        /// <summary>
        /// The items in this response.
        /// </summary>
        [JsonProperty("Response"), JsonConverter(typeof(BunqEntitySerializer))]
        public T[] Response { get; internal set; }

        [JsonProperty("Error")]
        public BunqResponseErrorData[] Error { get; internal set; }

        [JsonIgnore]
        internal BunqSession Session { get; set; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Response.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)Response).GetEnumerator();
        }

        /// <summary>
        /// Gets the first item of a specific type.
        /// </summary>
        /// <typeparam name="R">The type to look for</typeparam>
        /// <returns>The first item of type R in this response</returns>
        public R Get<R>()
            where R : T
        {
            return Response.FirstOrDefault(a => a is R) as R;
        }
    }
}
