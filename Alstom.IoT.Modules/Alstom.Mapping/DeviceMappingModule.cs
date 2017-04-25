using Microsoft.Azure.IoT.Gateway;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Alstom.Mapping
{
    public class DeviceConf
    {
        [JsonProperty("deviceID")]
        public string deviceid { get; set; }
        [JsonProperty("iothub_deviceId")]
        public string iothubdeviceid { get; set; }
        [JsonProperty("iothub_deviceKey")]
        public string iothubdevicekey { get; set; }
    }

    public class DeviceMappingModule : IGatewayModule
    {
        private const string _moduleName = "Mapping Module";

        private Broker broker;
        private string configuration;
        private List<DeviceConf> Mapping;
        public void Create(Broker broker, byte[] configuration)
        {
            Console.WriteLine($"{_moduleName}:  creating...");

            this.broker = broker;
            this.configuration = System.Text.Encoding.UTF8.GetString(configuration);

            //LOAD MAPPING CONFIG
            //var m = JsonConvert.DeserializeObject(this.configuration);
            this.Mapping = JsonConvert.DeserializeObject<List<DeviceConf>>(this.configuration);

            Console.WriteLine($"{_moduleName}:  created.");

        }

        public void Destroy()
        {
            //TODO
        }

        public void Receive(Message received_message)
        {
            //if (received_message.Properties["source"] == "Alstom.camera")
            //{
                //MAPPING
                string curMessage = System.Text.Encoding.UTF8.GetString(received_message.Content, 0, received_message.Content.Length);
                var curMessageJson = JsonConvert.DeserializeObject(curMessage);
                string curAlstomDeviceID = received_message.Properties["deviceId"];

                DeviceConf result = this.Mapping.Find(x => x.deviceid == curAlstomDeviceID);
                if(result != null)
                {
                    Dictionary<string, string> thisIsMyProperty = new Dictionary<string, string>();
                    //Dictionary<string, string> thisIsMyProperty = received_message.Properties;

                    foreach (var curProperty in received_message.Properties)
                    {
                        thisIsMyProperty.Add(curProperty.Key, curProperty.Value);
                    }

                    thisIsMyProperty.Add("Alstom.deviceID", result.iothubdeviceid);
                    thisIsMyProperty.Add("Alstom.deviceKey", result.iothubdevicekey);
                
                    //var messageString = JsonConvert.SerializeObject(cameraData);
                    Message messageToPublish = new Message(curMessage, thisIsMyProperty);

                    this.broker.Publish(messageToPublish);
                    Console.WriteLine("Mapping Module mapped succcessfully message from " + curAlstomDeviceID + " to " + result.iothubdeviceid);
                }
                else
                    Console.WriteLine("Mapping Module failed to map message from : " + curAlstomDeviceID);

            //}
        }
    }
}
