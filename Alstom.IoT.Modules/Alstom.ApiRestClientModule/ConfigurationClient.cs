using System.Net;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;

namespace Alstom.ApiRestClientModule
{
    /// <summary>
    /// Http client to communicate with configuration end point
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ConfigurationClient<T> : IConfigurationClient<T> where T : class
    {

        private const string _moduleName = "eHubWebClient";

        /// <summary>
        /// Http web client
        /// </summary>
        private WebClient Client { get; set; } = new WebClient();

        /// <summary>
        /// Basic post method, it is not using any kind of parameter for sending the identifier, just the payload.
        /// </summary>
        /// <param name="uri">The url to make the post request</param>
        /// <param name="item">The payload for the post</param>
        /// <returns></returns>
        public async Task PostAsync(Uri uri, T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            var request = JsonConvert.SerializeObject(item);

            Client.Headers.Add(HttpRequestHeader.ContentType, "application/json");

            await Client.UploadStringTaskAsync(uri, request);
        }
    }
}
