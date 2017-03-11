using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace puckipedia.bunq.ApiCalls
{
    /// <summary>
    /// A paginated response.
    /// </summary>
    /// <typeparam name="T">The value of the objects in the response.</typeparam>
    public class BunqPaginatedResponse<T> : BunqResponse<T> where T : BunqObject
    {
        [JsonProperty("Pagination")]
        public Pagination Pagination { get; private set; }

        internal object _populateData;

        /// <summary>
        /// If we can retrieve older items
        /// </summary>
        public bool HasOlderItems => Pagination.OlderURL != null;

        /// <summary>
        /// If we can retrieve newer items.
        /// </summary>
        public bool HasNewerItems => Pagination.NewerURL != null;

        /// <summary>
        /// This API call will return any items that are made after the current item, if we are currently at the newest item.
        /// </summary>
        public bool HasFutureItems => Pagination.FutureURL != null;

        /// <summary>
        /// Gets future items, made after the newest item in here
        /// </summary>
        /// <returns>Paginated response containing future items</returns>
        public async Task<BunqPaginatedResponse<T>> GetFutureItems() => await Session.ListAsync<T>(Pagination.FutureURL, _populateData);

        /// <summary>
        /// Gets items newer than these.
        /// </summary>
        /// <returns>A paginated response</returns>
        public async Task<BunqPaginatedResponse<T>> GetNewerItems() => await Session.ListAsync<T>(Pagination.NewerURL, _populateData);

        /// <summary>
        /// Gets items older than these.
        /// </summary>
        /// <returns>A paginated response</returns>
        public async Task<BunqPaginatedResponse<T>> GetOlderItems() => await Session.ListAsync<T>(Pagination.OlderURL, _populateData);

        /// <summary>
        /// Enumerates from newest to oldest, and returns all items in this list.
        /// </summary>
        /// <remarks>This call might be very expensive, as it might have to call a lot of endpoints</remarks>
        /// <returns>An array of all the items in this list</returns>
        public async Task<T[]> GetAll()
        {
            var result = new List<T>();
            if (HasNewerItems)
            {
                Stack<T[]> data = new Stack<T[]>();
                var response = this;
                do
                {
                    response = await response.GetNewerItems();
                    data.Push(response.Response);
                } while (response.HasNewerItems);

                foreach (var item in data)
                    result.AddRange(item);
            }

            result.AddRange(Response);

            if (HasOlderItems)
            {
                var response = this;
                do
                {
                    response = await response.GetOlderItems();
                    result.AddRange(response.Response);
                } while (response.HasOlderItems);
            }

            return result.ToArray();
        }
    }
}
