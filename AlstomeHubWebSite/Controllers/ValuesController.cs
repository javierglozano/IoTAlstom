using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace AlstomeHubWebSite.Controllers
{
    [RoutePrefix("api/documentDB")]
    public class ValuesController : ApiController
    {
        private const string _endpointUrl = "https://alstomiot.documents.azure.com:443/";
        private const string _authorizationKey = "9Ge1sFHnYg2gNUPqYwyAnEmeMGgVYwRizXOEExxZ6J5cwSiMsY93SG6zp98gZF1rUw9AhRre9WfRDc6GiN1nFQ==";
        private const string _moduleName = "WebSite";
        private const string _databaseName = "monitorDB";
        private const string _collectionName = "deviceCollection";
        private const string _documentDBName = "iotalstom";

        private DocumentClient Client { get; set; }

        /// <summary>
        /// Saves a Json into a DocumentDB 
        /// </summary>
        /// <param name="device">A json to be saved into a documentDB</param>
        /// <returns></returns>
        [HttpPost, Route("devices")]
        public async Task<IHttpActionResult> Post([FromBody] JObject device)
        {
            Console.WriteLine($"{_moduleName}: post received");
            string jsonObject = JsonConvert.SerializeObject(device);
            Console.WriteLine(jsonObject);

            using (Client = new DocumentClient(new Uri(_endpointUrl), _authorizationKey))
            {
                //verify that the database exists
                //await CreateDatabaseIfNotExists();

                //verify that the collection exists
                //await CreateDocumentCollectionIfNotExists();

                //create or update the doc

                var document = await CreateorUpdateDocument(device);

                return Ok(document);

            }
        }

        /// <summary>
        /// Verifies that the database is already created. If not the method will create it
        /// </summary>
        /// <returns></returns>
        private async Task CreateDatabaseIfNotExists()
        {
            // Check to verify a database with the id=FamilyDB does not exist
            try
            {
                await Client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_databaseName));
                Console.WriteLine($"{_moduleName}: Found {_databaseName}");
            }
            catch (DocumentClientException de)
            {
                // If the database does not exist, create a new database
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await Client.CreateDatabaseAsync(new Database { Id = _databaseName });
                    Console.WriteLine($"{_moduleName}: Created {_databaseName}");
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Verifies that the collection is created in the database. If not the method will create it
        /// </summary>
        /// <returns></returns>
        private async Task CreateDocumentCollectionIfNotExists()
        {
            try
            {
                await Client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName));
                Console.WriteLine($"{_moduleName}: Found {_collectionName}");
            }
            catch (DocumentClientException de)
            {
                // If the document collection does not exist, create a new collection
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    DocumentCollection collectionInfo = new DocumentCollection();
                    collectionInfo.Id = _collectionName;

                    // Configure collections for maximum query flexibility including string range queries.
                    collectionInfo.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

                    // Here we create a collection with 400 RU/s.
                    await Client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(_databaseName),
                        collectionInfo,
                        new RequestOptions { OfferThroughput = 400 });

                    Console.WriteLine($"{_moduleName}: Created {_collectionName}");
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Verify if the document is already created. If not, create a new one. If exists, just update the document
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task<Document> CreateorUpdateDocument(JObject item)
        {

            var id = item.SelectToken("id").ToString();
            var partitionKey = item.SelectToken("deviceId").ToString();

            var doc = Client.CreateDocumentQuery<JObject>(UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName), $"SELECT * FROM c WHERE c.id = '{id}'", new FeedOptions { EnableCrossPartitionQuery = true }).AsEnumerable().FirstOrDefault();

            if (doc == null)
            {
                var document = await Client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName), item);
                Console.WriteLine($"{_moduleName}: Created {id}");

                return document;
            }
            else
            {

                Console.WriteLine($"{_moduleName}: Updated {id}");

                var document = await Client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseName, _collectionName, id), item);

                Console.WriteLine($"{_moduleName}: Updated {id}");

                return document;
            }
        }
    }
}
