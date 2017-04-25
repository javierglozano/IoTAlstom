using System;

namespace Alstom.DTO
{
    public class DeviceEvent
    {
        public Guid id;
        public string deviceId;
        public DeviceAlarms alarm;
        public DeviceStatus status;
        public string eventName;
        public DateTime dateTime;
        public string content;
    }
}