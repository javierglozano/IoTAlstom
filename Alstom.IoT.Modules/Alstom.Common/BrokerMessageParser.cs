using Alstom.DTO;
using Microsoft.Azure.IoT.Gateway;
using System;
using System.Text;

namespace Alstom.Common
{
    public static class BrokerMessageParser
    {
        public const string _idProperty = "deviceId";
        public const string _eventProperty = "event";
        public const string _timeProperty = "time";
        public const string _alarm = "alarm";
        public const string _status = "status";

        public static DeviceEvent ToDeviceEvent(this Message received_message)
        {
            var somethingWentWrong = false;

            if (!received_message.Properties.ContainsKey(_idProperty))
            {
                Console.WriteLine($"Message recieved does not contain an {_idProperty} property.");
                somethingWentWrong = true;
            }

            if (!received_message.Properties.ContainsKey(_eventProperty))
            {
                Console.WriteLine($"Message recieved does not contain an {_eventProperty} property.");
                somethingWentWrong = true;
            }

            if (!received_message.Properties.ContainsKey(_timeProperty))
            {
                Console.WriteLine($"Message recieved does not contain an {_timeProperty} property.");
                somethingWentWrong = true;
            }

            long ticks = 0;
            if (!long.TryParse(received_message.Properties[_timeProperty], out ticks) || ticks < 0)
            {
                Console.WriteLine($"Message recieved contains an {_timeProperty} property. but it caannot be parser into a positive long object representing time ticks.");
                somethingWentWrong = true;
            }

            if (!received_message.Properties.ContainsKey(_alarm))
            {
                Console.WriteLine($"Message recieved does not contain an {_alarm} property.");
                somethingWentWrong = true;
            }

            if (!received_message.Properties.ContainsKey(_status))
            {
                Console.WriteLine($"Message recieved does not contain an {_status} property.");
                somethingWentWrong = true;
            }

            if (somethingWentWrong)
            {
                return null;
            }

            return new DeviceEvent()
            {
                id = Guid.NewGuid(),
                deviceId = received_message.Properties[_idProperty],
                eventName = received_message.Properties[_eventProperty],
                dateTime = new DateTime(ticks, DateTimeKind.Utc),
                alarm = (DeviceAlarms)Enum.Parse(typeof(DeviceAlarms), received_message.Properties[_alarm], true),
                status = (DeviceStatus)Enum.Parse(typeof(DeviceStatus), received_message.Properties[_status], true),
                content = Encoding.UTF8.GetString(received_message.Content)
            };
        }
    }
}
