using Alstom.DTO;
using Microsoft.Azure.IoT.Gateway;
using System;
using System.Collections.Generic;
using System.ServiceModel.Discovery;
using System.Threading;
using Alstom.Common;

namespace Alstom.OnvifModule
{
    public class OnvifModule : IGatewayModule, IGatewayModuleStart
    {
        // CHECK THIS EXAMPLE: https://github.com/Azure/iot-gateway-opc-ua/blob/master/src/Opc.Ua.Client.Module/Module.cs#L68

        private const string _moduleName = "Onvif Module";
        static private Broker _broker;
        private DiscoveryService _discoveryService;
        private Thread _workerThread;
        private bool _keepRunningWorkerThread = true;
        private int _discoveryTimeOutMiliseconds = 10000;

        private List<Device> _currentKnownDevices;

        /// <summary>
        /// Create module, throws if configuration is bad
        /// </summary>
        /// <param name="broker"></param>
        /// <param name="configuration"></param>
        public void Create(Broker broker, byte[] configuration)
        {
            Console.WriteLine($"{_moduleName}:  creating...");

            _broker = broker;
            _discoveryService = new DiscoveryService();

            if (_broker == null)
            {
                Console.WriteLine($"{_moduleName}: Broker object is null, this module will not be able to publish messages");
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

            _discoveryService = new DiscoveryService();

            Console.WriteLine($"{_moduleName}: Discovering devices...");
            _discoveryService.Discover(_discoveryTimeOutMiliseconds, DiscoveryCompleted);

            //_workerThread = new Thread(this.DoWork);
            //_workerThread.Start();

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

        //private void DoWork()
        //{
        //    var randomizer = new Random(DateTime.Now.Millisecond);



        //    //while (_keepRunningWorkerThread)
        //    //{                              
        //    //    Thread.Sleep(2000);
        //    //}
        //}

        internal void DiscoveryCompleted(object sender, FindCompletedEventArgs e)
        {
            var newKnownDevices = _discoveryService.GetDevices();

            Console.WriteLine($"{_moduleName}: Discovered completed, {newKnownDevices.Count} found.");

            if (_currentKnownDevices == null)
            {
                AnnounceNewDevices(newKnownDevices);
            }
            else
            {
                CompareDevices(newKnownDevices);
            }

            _currentKnownDevices = newKnownDevices;

            _discoveryService = new DiscoveryService();
            _discoveryService.Discover(_discoveryTimeOutMiliseconds, DiscoveryCompleted);
        }

        private void AnnounceNewDevices(List<Device> newKnownDevices)
        {
            foreach (var newKnownDevice in newKnownDevices)
            {
                Publish(new DeviceEvent() { deviceId = newKnownDevice.macAddress, dateTime = DateTime.UtcNow, eventName = "OperationalStatus", status = DeviceStatus.Working, alarm = DeviceAlarms.NoAlarms });
            }
        }

        private void CompareDevices(List<Device> newKnownDevices)
        {
            foreach (var newKnownDevice in newKnownDevices)
            {
                var resultDevice = _currentKnownDevices.Find((device) => device.macAddress == newKnownDevice.macAddress);

                if (resultDevice == null)
                {
                    Publish(new DeviceEvent() { deviceId = newKnownDevice.macAddress, dateTime = DateTime.UtcNow, eventName = "OperationalStatus", status = DeviceStatus.Working, alarm = DeviceAlarms.NoAlarms });
                }
            }
            foreach (var currentKnownDevice in _currentKnownDevices)
            {
                var resultDevice = newKnownDevices.Find((device) => device.macAddress == currentKnownDevice.macAddress);

                if (resultDevice == null)
                {
                    Publish(new DeviceEvent() { deviceId = currentKnownDevice.macAddress, dateTime = DateTime.UtcNow, eventName = "OperationalStatus", status = DeviceStatus.NotOperational, alarm = DeviceAlarms.NoAlarms });
                }
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
