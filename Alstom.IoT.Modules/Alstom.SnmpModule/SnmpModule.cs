﻿using Alstom.Common;
using Alstom.DTO;
using Microsoft.Azure.IoT.Gateway;
using SnmpFullFramework;
using System;

namespace Alstom.SnmpModule
{
    public class SnmpModule : IGatewayModule, IGatewayModuleStart
    {
        // CHECK THIS EXAMPLE: https://github.com/Azure/iot-gateway-opc-ua/blob/master/src/Opc.Ua.Client.Module/Module.cs#L68

        private const string _moduleName = "SNMP Module";
        private Broker _broker;

        private TrapListener TrapListener { get; set; } = new TrapListener();

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
            TrapListener.Initialize();
            TrapListener.TrapEvent += this.PublishInformation;
            TrapListener.Start();



            Console.WriteLine($"{_moduleName}: started.");
        }

        private void PublishInformation(object sender, DeviceEvent deviceEvent)
        {
           if (_broker != null && deviceEvent != null)
                _broker.Publish(deviceEvent.ToBrokerMessage());     
        }



        /// <summary>
        ///     The module's callback function that is called upon message receipt.
        /// </summary>
        /// <param name="received_message">The message being sent to the module.</param>
        /// <returns></returns>  
        public void Receive(Message received_message)
        {
            Console.WriteLine($"{_moduleName}: who is calling?.");
        }

        /// <summary>
        ///     Disposes of the resources allocated by/for this module.
        /// </summary>
        /// <returns></returns>
        public void Destroy()
        {
            Console.WriteLine($"{_moduleName}:  destroyed.");
        }

    }
}
