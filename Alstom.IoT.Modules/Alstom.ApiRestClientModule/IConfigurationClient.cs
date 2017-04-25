using System.Threading.Tasks;
using System;

namespace Alstom.ApiRestClientModule
{
    /// <summary>
    /// Http client to communicate with configuration end point
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IConfigurationClient<T> where T: class
    {        
        Task PostAsync(Uri uri, T item);
    }
}
