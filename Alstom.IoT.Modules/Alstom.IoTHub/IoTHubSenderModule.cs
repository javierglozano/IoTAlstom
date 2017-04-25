using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using IoTGateway = Microsoft.Azure.IoT.Gateway;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace Alstom.IoTHub
{
    public class IoTHubSenderModule : IoTGateway.IGatewayModule
    {
        private const string _moduleName = "IoTHub dotnet Module";

        static DeviceClient deviceClient_cli1;
        static DeviceClient deviceClient_cli2;
        static DeviceClient deviceClient_cli3;
        private string iotHubUri;

        private string configuration;
        public void Create(IoTGateway.Broker broker, byte[] configuration)
        {
            Console.WriteLine($"{_moduleName}:  creating...");

            this.configuration = System.Text.Encoding.UTF8.GetString(configuration);
            var m = JsonConvert.DeserializeObject(this.configuration);
            try
            {
                this.iotHubUri = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)m).First).First).Value.ToString();
                deviceClient_cli1 = DeviceClient.Create(this.iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("cam1", "lZcmVuk6jzjvBEj5hefis/JfCfrUJp0+7T+lTxypymY="), TransportType.Http1);
                deviceClient_cli2 = DeviceClient.Create(this.iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("cam2", "a1DpgC+Fpx6Dnu/dBGndN4boeU8B/fx6HH14yZRTbUQ="), TransportType.Http1);
                deviceClient_cli3 = DeviceClient.Create(this.iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("cam3", "Apb+9448sdfS34slpf+fBnabgs3d0mQcx1wWpCBtpjs="), TransportType.Http1);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            Console.WriteLine($"{_moduleName}:  created.");
        }

        public void Destroy()
        {
        }

        public void Receive(IoTGateway.Message received_message)
        {
            if (received_message.Properties.ContainsKey("Alstom.deviceID") && received_message.Properties.ContainsKey("Alstom.deviceKey"))
            //if (received_message.Properties["source"] == "Alstom.camera")
            {
                string curDeviceID = received_message.Properties["Alstom.deviceID"];
                string curDeviceKey = received_message.Properties["Alstom.deviceKey"];
                string curEventDeviceID = received_message.Properties["deviceId"];
                string curEvent = received_message.Properties["event"];
                string curEventTimestamp = received_message.Properties["time"];

                string message = @"{
                    'id': '" + curEventDeviceID + @"',
                    'deviceId': '" + curDeviceID + @"',
                    'alarm': 0,
                    'status': 1,
                    'eventName': '" + curEvent + @"',
                    'content': '',
                    'dateTime': '" + curEventTimestamp + @"'
                  }";
                
                var message2Send = new Message(Encoding.ASCII.GetBytes(message));

                switch(curDeviceID)
                {
                    case "cam1":
                        deviceClient_cli1.SendEventAsync(message2Send);
                        break;
                    case "cam2":
                        deviceClient_cli2.SendEventAsync(message2Send);
                        break;
                    case "cam3":
                        deviceClient_cli3.SendEventAsync(message2Send);
                        break;
                    default:
                        Console.WriteLine("IotTHub Module did not sent message from " + curDeviceID + ".");
                        break;

                }
                //deviceClient.CloseAsync();

                //Async
                //SendDeviceToCloudMessagesAsync(message2Send, curDeviceID);


                Console.WriteLine("IotTHub Module sent message from device " + curDeviceID + " : " + message);

            }
        }

        //private static async void SendDeviceToCloudMessagesAsync(Message message, string deviceiID)
        //{
        //    await deviceClient.SendEventAsync(message);
        //    await deviceClient.CloseAsync();
        //    Console.WriteLine("IotTHub Module sent message from device " + deviceiID + " : " + message);

        //}

    }
}
