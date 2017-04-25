using Alstom.DTO;
using Microsoft.Azure.IoT.Gateway;
using System.Collections.Generic;
using System.Globalization;

namespace Alstom.Common
{
    public static class DeviceEventParser
    {
        public static Message ToBrokerMessage(this DeviceEvent deviceEvent)
        {
            var properties = new Dictionary<string, string>()
            {
                { BrokerMessageParser._idProperty , deviceEvent.deviceId },
                { BrokerMessageParser._eventProperty , deviceEvent.eventName },
                { BrokerMessageParser._timeProperty , deviceEvent.dateTime.Ticks.ToString(CultureInfo.InvariantCulture) },
                { BrokerMessageParser._alarm , deviceEvent.alarm.ToString() },
                { BrokerMessageParser._status , deviceEvent.status.ToString() },
            };

            return new Message(deviceEvent.content??string.Empty, properties);
        }
    }
}
