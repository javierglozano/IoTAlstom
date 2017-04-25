using Alstom.DTO;
using System;
using System.Globalization;
using System.Text;

namespace Alstom.Common
{
    public static class LogingHelps
    {
        public const string PublishedAction = "published";
        public const string RecievedAction = "recieved";

        public static void LogMessage(DeviceEvent deviceEvent, string moduleName, string action)
        {
            var messageToLog = new StringBuilder();

            messageToLog.Append($"{moduleName}:  Message {action} with id: {deviceEvent.deviceId}." + Environment.NewLine);
            messageToLog.Append($"and event name: {deviceEvent.eventName}." + Environment.NewLine);
            messageToLog.Append($"and event time: {deviceEvent.dateTime.ToString(CultureInfo.InvariantCulture)}." + Environment.NewLine);
            messageToLog.Append($"and content: {deviceEvent.content}.");

            Console.WriteLine(messageToLog.ToString());
        }
    }
}
