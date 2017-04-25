using SnmpSharpNet;

namespace DeviceSimulator
{
    public static class AxisOidsHelper
    {
        public const string AxisBaseTree = "1.3.6.1.4.1.368";
        public const string TemperatureSensorStatus = "4.1.3.1.3";
        public const string TemperatureSensorValue = "4.1.3.1.4";
        public const string TemperatureSensorEnum = "4.1.3.1.5";
        public const string AlarmStatus = "4.2.0.1";


        public const string ColdStart = "1.3.6.1.6.3.1.1.5.1";
        public const string WarmStart = "1.3.6.1.6.3.1.1.5.2";
        public const string Community = "public";


        public static Oid GetTemperatureSensorStatus(int sensorType, int sensorId)
        {
            return new Oid(string.Format($"{AxisBaseTree}.{TemperatureSensorStatus}.{sensorType}.{sensorId}"));
        }

        public static Oid GetTemperatureSensorValue(int sensorType, int sensorId)
        {
            return new Oid(string.Format($"{AxisBaseTree}.{TemperatureSensorValue}.{sensorType}.{sensorId}"));
        }
        public static Oid GetTemperatureSensorEnum(int sensorType, int sensorId)
        {
            return new Oid(string.Format($"{AxisBaseTree}.{TemperatureSensorEnum}.{sensorType}.{sensorId}"));
        }

        public static Oid GetAlarmStatus()
        {
            return new Oid(string.Format($"{AxisBaseTree}.{AlarmStatus}"));
        }


    }

}

