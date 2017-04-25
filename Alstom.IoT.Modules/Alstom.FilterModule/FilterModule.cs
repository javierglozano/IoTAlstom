using Alstom.Common;
using Alstom.DTO;
using Microsoft.Azure.IoT.Gateway;
using System;

namespace Alstom.FilterModule
{
    public class FilterModule : IGatewayModule, IGatewayModuleStart
    {
        // CHECK THIS EXAMPLE: https://github.com/Azure/iot-gateway-opc-ua/blob/master/src/Opc.Ua.Client.Module/Module.cs#L68

        private const string _moduleName = "Filter Module";
        private Broker _broker;

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
                Console.WriteLine($"{_moduleName}:  Broker object is null, this module will not be able to publish messages");
            }

            // Read configuration here...
            //string configString = Encoding.UTF8.GetString(configuration);

            Console.WriteLine($"{_moduleName}:  created.");
        }

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
        ///     The module's callback function that is called upon message receipt.
        /// </summary>
        /// <param name="received_message">The message being sent to the module.</param>
        /// <returns></returns>  
        public void Receive(Message received_message)
        {
            DeviceEvent deviceEvent = received_message.ToDeviceEvent();

            if (deviceEvent == null)
            {
                Console.WriteLine($"{_moduleName}: Message recieved with wrong expected format. Message will be discarted");
                return;
            }

            LogingHelps.LogMessage(deviceEvent, _moduleName, LogingHelps.RecievedAction);
            
            //This should be async
            DeviceEvent deviceEventAfterLogicApplied = ApplyLogic(deviceEvent);

            if (deviceEventAfterLogicApplied == null)
            {
                Console.WriteLine($"{_moduleName}: Message for {deviceEvent.deviceId} has been filtered out");
                return;
            }

            Publish(deviceEventAfterLogicApplied);

            LogingHelps.LogMessage(deviceEventAfterLogicApplied, _moduleName, LogingHelps.PublishedAction);
        }

        /// <summary>
        ///     Disposes of the resources allocated by/for this module.
        /// </summary>
        /// <returns></returns>
        public void Destroy()
        {
            Console.WriteLine($"{_moduleName}:  destroyed.");
        }

        private DeviceEvent ApplyLogic(DeviceEvent deviceEvent)
        {
            return deviceEvent; // TODO: ADD LOGIC HERE TO FILTER OR WHAT EVER...
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
