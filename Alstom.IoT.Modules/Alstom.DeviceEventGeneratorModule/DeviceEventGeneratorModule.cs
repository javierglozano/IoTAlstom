using Alstom.Common;
using Alstom.DTO;
using Microsoft.Azure.IoT.Gateway;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;

namespace Alstom.DeviceEventGeneratorModule
{
    public class DeviceEventGeneratorModule : IGatewayModule, IGatewayModuleStart
    {
        // CHECK THIS EXAMPLE: https://github.com/Azure/iot-gateway-opc-ua/blob/master/src/Opc.Ua.Client.Module/Module.cs#L68

        private const string _moduleName = "Device Event Generator Module";
        private Broker _broker;
        private Thread _workerThread;
        private bool _keepRunningWorkerThread = true;
        private Configuration _configuration;

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
            {
                Console.WriteLine($"{_moduleName}: Broker object is null, this module will not be able to publish messages");
            }

            // Read configuration here...
            string configString = Encoding.UTF8.GetString(configuration);

            try
            {
                _configuration = JsonConvert.DeserializeObject<Configuration>(configString);
                Console.WriteLine($"{_moduleName}:  Thread interval set to {_configuration.threadInternalMiliseconds} miliseconds.");
            }
            catch(Exception ex)
            {
                throw new ArgumentException($"{_moduleName}: Configuration provided could not be parsed", ex);
            }

            Console.WriteLine($"{_moduleName}:  created.");
        }

        /// <summary>
        ///     Informs module the gateway is ready to send and receive messages.
        /// </summary>
        /// <returns></returns>
        public void Start()
        {
            Console.WriteLine($"{_moduleName}: starting...");

            _workerThread = new Thread(this.DoWork);
            _workerThread.Start();

            Console.WriteLine($"{_moduleName}: started.");
        }

        /// <summary>
        ///     The module's callback function that is called upon message receipt.
        /// </summary>
        /// <param name="received_message">The message being sent to the module.</param>
        /// <returns></returns>  
        public void Receive(Message received_message)
        {
            // Not expecting recieve anything
            Console.WriteLine($"{_moduleName}:  This module is not expecting recieve anything, message discarted.");
        }

        /// <summary>
        ///     Disposes of the resources allocated by/for this module.
        /// </summary>
        /// <returns></returns>
        public void Destroy()
        {
            Console.WriteLine($"{_moduleName}:  destroying...");

            _keepRunningWorkerThread = false;

            _workerThread.Join();

            Console.WriteLine($"{_moduleName}:  destroyed.");
        }

        private void DoWork()
        {
            var randomizer = new Random(DateTime.Now.Millisecond);

            while (_keepRunningWorkerThread)
            {
                var deviceEvent = new DeviceEvent()
                {
                    deviceId = Guid.NewGuid().ToString(),
                    eventName = "randomName " + randomizer.Next(),
                    dateTime = DateTime.UtcNow,
                    content = "some random content " + randomizer.Next()
                };

                Publish(deviceEvent);

                LogingHelps.LogMessage(deviceEvent, _moduleName, LogingHelps.PublishedAction);

                Thread.Sleep(_configuration.threadInternalMiliseconds);
            }
        }

        /// <summary>
        /// Publish message to bus
        /// </summary>
        /// <param name="message"></param>
        private void Publish(DeviceEvent deviceEvent)
        {
            if (_broker != null)
            {
                _broker.Publish(deviceEvent.ToBrokerMessage());
            }
        }

    }
}
