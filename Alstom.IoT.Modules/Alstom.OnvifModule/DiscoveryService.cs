using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Discovery;
using System.Xml;

namespace Alstom.OnvifModule
{
    public class DiscoveryService
    {
        private ConcurrentDictionary<string,Device> _devices = new ConcurrentDictionary<string,Device>();

        public void DiscoverDevices()
        {

        }

        public void Discover(int discoveryTimeOutMiliseconds, EventHandler<FindCompletedEventArgs> discoveryCompleted)
        {            
            var endPoint = new UdpDiscoveryEndpoint(DiscoveryVersion.WSDiscoveryApril2005);
            FindCriteria findCriteria = new FindCriteria();
            var contractTypeName = "NetworkVideoTransmitter";
            var contractTypeNamespace = "http://www.onvif.org/ver10/network/wsdl";

            // those parametes are defined by onvif standard
            findCriteria.ContractTypeNames.Add(new XmlQualifiedName(contractTypeName, contractTypeNamespace));

            var discoveryClient = new DiscoveryClient(endPoint);
            discoveryClient.FindProgressChanged += discoveryClient_FindProgressChanged;
            discoveryClient.FindCompleted += discoveryCompleted;

            findCriteria.Duration = TimeSpan.FromMilliseconds(discoveryTimeOutMiliseconds);
            findCriteria.MaxResults = int.MaxValue;
            discoveryClient.FindAsync(findCriteria);
        }

        public List<Device> GetDevices()
        {
            return _devices.Values.ToList();
        }

        /// <summary>
        /// Every time a new device is found this methos executed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void discoveryClient_FindProgressChanged(object sender, FindProgressChangedEventArgs e)
        {
            var uris = e.EndpointDiscoveryMetadata.ListenUris;

            if (uris.Any())
            {
                var device = new Device()
                {
                    // Get mac address should actually initialize the device object with more information
                    macAddress = GetMacAddress(uris.First()),
                    uris = uris
                };

                // Review this method
                _devices.TryAdd(device.macAddress, device);
            }
        }

        private string GetMacAddress(Uri deviceUri)
        {
            var httpTransport = new HttpTransportBindingElement();
            var httpTransportBinding = new HttpTransportBindingElement { AuthenticationScheme = AuthenticationSchemes.Digest };
            var textMessageEncodingBinding = new TextMessageEncodingBindingElement { MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None) };
            var customBinding = new CustomBinding(textMessageEncodingBinding, httpTransportBinding);

            EndpointAddress serviceAddress = new EndpointAddress(deviceUri);

            var passwordDigestBehavior = new PasswordDigestBehavior("julian", "julian");

            var deviceClient = new DeviceManagementService.DeviceClient(customBinding, serviceAddress);
            deviceClient.Endpoint.Behaviors.Add(passwordDigestBehavior);

            string model, firmwareVersion, serialNumber, hardwareId;
            deviceClient.GetDeviceInformation(out model, out firmwareVersion, out serialNumber, out hardwareId);

            return serialNumber;
        }
    }
}
