using Alstom.Common;
using Microsoft.Azure.IoT.Gateway;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Alstom.ApiRestClientModule
{
    public class ApiRestClientModule : IGatewayModule, IGatewayModuleStart
    {
        // CHECK THIS EXAMPLE: https://github.com/Azure/iot-gateway-opc-ua/blob/master/src/Opc.Ua.Client.Module/Module.cs#L68

        private const string _moduleName = "ApiRestClient Module";        
        private Configuration _configuration;
        private Uri _restApiUri;

        /// <summary>
        /// The broker where we read all the device notifications
        /// </summary>
        private Broker _broker { get; set; }

        /// <summary>
        /// The web client to send all the events
        /// </summary>
        private HttpClient _client { get; set; } = new HttpClient();

        /// <summary>
        ///     Informs module the gateway is ready to send and receive messages.
        /// </summary>
        /// <returns></returns>
        public void Start()
        {
            Console.WriteLine($"{_moduleName}: starting...");

            //Do something here...

            Console.WriteLine($"{_moduleName}: started.");
        }

        /// <summary>
        /// Create module, throws if configuration is bad
        /// </summary>
        /// <param name="broker"></param>
        /// <param name="configuration"></param>
        public void Create(Broker broker, byte[] configuration)
        {
            Console.WriteLine($"{_moduleName}:  creating...");

            _broker = broker;

            if (_broker == null)
                Console.WriteLine($"{_moduleName}:  Broker object is null, this module will not be able to publish messages");

            // Read configuration here...
            string configString = Encoding.UTF8.GetString(configuration);

            try
            {
                _configuration = JsonConvert.DeserializeObject<Configuration>(configString);

                if (Uri.TryCreate(_configuration.restApiEndPoint, UriKind.Absolute, out _restApiUri))
                {
                    Console.WriteLine($"{_moduleName}: rest end point set to {_configuration.restApiEndPoint}.");
                }
                else
                {
                    var message = $"{_moduleName}: Impossible to set the rest end point";
                    Console.WriteLine(message);
                    throw new ArgumentException(message);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{_moduleName}: Configuration provided could not be parsed", ex);
            }

            Console.WriteLine($"{_moduleName}:  created.");
        }

        /// <summary>
        ///     The module's callback function that is called upon message receipt.
        /// </summary>
        /// <param name="received_message">The message being sent to the module.</param>
        /// <returns></returns>  
        public void Receive(Message received_message)
        {

            Console.WriteLine($"{_moduleName}:  receiving...");

            var deviceEvent = received_message.ToDeviceEvent();

            if (deviceEvent != null)
                _client.PostAsync(_restApiUri, deviceEvent).Wait();

            Console.WriteLine($"{_moduleName}:  received");
        }

        /// <summary>
        /// Publish message to bus
        /// </summary>
        /// <param name="message"></param>
        public void Publish(Message message)
        {
            Console.WriteLine($"{_moduleName}:  publishing...");

            _broker?.Publish(message);

            Console.WriteLine($"{_moduleName}:  published");
        }

        /// <summary>
        ///     Disposes of the resources allocated by/for this module.
        /// </summary>
        /// <returns></returns>
        public void Destroy()
        {
            Console.WriteLine($"{_moduleName}:  destroying...");
            _broker = null;
            Console.WriteLine($"{_moduleName}:  destroyed.");
        }

    }
}
