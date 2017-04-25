using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Alstom.Common
{
    /// <summary>
    /// Http client to communicate with configuration end point
    /// </summary>
    public class HttpClient : IHttpClient
    {
        /// <summary>
        /// Http web client
        /// </summary>
        private WebClient _client { get; set; } = new WebClient();

        /// <summary>
        /// Basic post method.
        /// </summary>
        /// <param name="uri">The url to make the post request</param>
        /// <param name="item">The payload for the post</param>
        /// <returns></returns>
        public async Task PostAsync<T>(Uri uri, T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            var request = JsonConvert.SerializeObject(item);

            _client.Headers.Add(HttpRequestHeader.ContentType, "application/json");

            await _client.UploadStringTaskAsync(uri, request);
        }

        /// <summary>
        /// Basic get method.
        /// </summary>
        /// <param name="uri">The url to make the get request</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(Uri uri)
        {
            _client.Headers.Add(HttpRequestHeader.ContentType, "application/json");

            var result = await _client.DownloadStringTaskAsync(uri);

            return await Task.Factory.StartNew<T>(() => JsonConvert.DeserializeObject<T>(result));
        }
    }
}
