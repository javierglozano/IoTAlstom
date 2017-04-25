using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading;
using SnmpSharpNet;

namespace DeviceSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            //var agents = new List<SNMPAgent>();
            //for(var i=0; i<1; i++)
            //{
            //    agents.Add(new SNMPAgent(i));
            //}
            SendTrapLoop();
        }


        private static void SendTrapLoop()
        {
            Random rnd = new Random();
            while (true)
            {
                var temp = rnd.Next(-60, 100);
                var status = temp < 0 ? TemperatureStatus.NONE : (temp > 60 ? TemperatureStatus.NOK : TemperatureStatus.OK);

                VbCollection collection = new VbCollection
                {
                    {AxisOidsHelper.GetTemperatureSensorStatus(1, 1), new Integer32((int) status)},
                    {AxisOidsHelper.GetTemperatureSensorValue(1, 1), new Integer32(temp)}
                };

                SNMPAgent.SendTrap(ConfigurationManager.AppSettings["TrapReceiverAddress"], 
                    int.Parse(ConfigurationManager.AppSettings["TrapReceiverPort"]), 
                    new Oid(AxisOidsHelper.GetTemperatureSensorStatus(1, 1)), collection);

                var opStatus = rnd.Next(0, 4);

                collection = new VbCollection { {AxisOidsHelper.GetAlarmStatus(), new Integer32(opStatus)} };

                SNMPAgent.SendTrap(ConfigurationManager.AppSettings["TrapReceiverAddress"],
                    int.Parse(ConfigurationManager.AppSettings["TrapReceiverPort"]),
                    AxisOidsHelper.GetAlarmStatus(), collection);
                
                Thread.Sleep(int.Parse(ConfigurationManager.AppSettings["TrapWaitTime"]));
            }
        }
    }
}
