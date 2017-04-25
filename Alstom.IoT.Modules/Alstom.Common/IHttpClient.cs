using System;
using System.Threading.Tasks;

namespace Alstom.Common
{
    /// <summary>
    /// Http client
    /// </summary>
    public interface IHttpClient
    {
        /// <summary>
        /// Basic post method.
        /// </summary>
        /// <param name="uri">The url to make the post request</param>
        /// <param name="item">The payload for the post</param>
        /// <returns></returns>
        Task PostAsync<T>(Uri uri, T item);

        /// <summary>
        /// Basic get method.
        /// </summary>
        /// <param name="uri">The url to make the get request</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(Uri uri);
    }
}
