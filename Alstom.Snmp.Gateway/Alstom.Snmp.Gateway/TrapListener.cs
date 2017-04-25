using Alstom.DTO;
using SnmpSharpNet;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SnmpFullFramework
{
    public class TrapListener
    {
        private Socket _socket;
        private IPEndPoint _ipep;
        private EndPoint _ep;
        private bool _run;

        private const string OIDAlarm = "1.3.6.1.4.1.368.4.2.0.1";
        private const string OIDStatus = "1.3.6.1.4.1.368.4.1.3.1.3.1.1";

        private static Random rand;

        public event EventHandler<DeviceEvent> TrapEvent;


        public void Initialize()
        {
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this._ipep = new IPEndPoint(IPAddress.Any, 162);
            this._ep = _ipep;
            this._socket.Bind(_ep);
            // Disable timeout processing. Just block until packet is received 
            this._socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 0);

        }

        public void Start()
        {
            Task.Run(() =>
            {
                this._run = true;

                while (this._run)
                {
                    byte[] indata = new byte[16 * 1024];
                    // 16KB receive buffer int inlen = 0;
                    IPEndPoint peer = new IPEndPoint(IPAddress.Any, 0);

                    EndPoint inep = (EndPoint)peer;

                    int inlen = -1;
                    try
                    {

                        inlen = _socket.ReceiveFrom(indata, ref inep);


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception {0}", ex.Message);
                        inlen = -1;
                    }
                    if (inlen > 0)
                    {
                        Task.Run(() =>
                        {
                            // Check protocol version int 
                            int ver = SnmpPacket.GetProtocolVersion(indata, inlen);

                            // Parse SNMP Version 2 TRAP packet 
                            SnmpV2Packet pkt = new SnmpV2Packet();
                            pkt.decode(indata, inlen);
                            Console.WriteLine("** SNMP Version 2 TRAP received from {0}:", inep.ToString());

                            //TrapOID .1.3.6.1.4.1.368.4.1.3.1.3.1.1(TemperatureSensorStatus)

                            //Variable bindings
                            //1.3.6.1.4.1.368.4.1.3.1.3.1.1(TemperatureSensorStatus)->value = 0(NONE), 1(OK), 2(NOK)
                            //1.3.6.1.4.1.368.4.1.3.1.4.1.1(TemperatureSensorValue)->value = [-60, 100]

                            //---------------------- -

                            //TrapOID 1.3.6.1.4.1.368.4.2.0.1(AlarmStatus)

                            //Variable bindings
                            //1.3.6.1.4.1.368.4.2.0.1(AlarmStatus)->value = 0, 1, 2, 3
                            if ((SnmpSharpNet.PduType)pkt.Pdu.Type != PduType.V2Trap)
                            {
                                Console.WriteLine("*** NOT an SNMPv2 trap ****");
                            }
                            else
                            {
                                var valueReceived = pkt.Pdu.VbList.FirstOrDefault(p => p.Oid.ToString().CompareTo(pkt.Pdu.TrapObjectID.ToString())==0);
                                RaiseEvent(valueReceived.Oid.ToString().CompareTo(OIDAlarm) == 0 ? "Alarm" : "OperationalStatus", valueReceived.Value.ToString());

                                Console.WriteLine("*** VarBind content:");
                                foreach (Vb v in pkt.Pdu.VbList)
                                {
                                    Console.WriteLine("**** {0} {1}: {2}",
                                        v.Oid.ToString(), SnmpConstants.GetTypeName(v.Value.Type), v.Value.ToString());
                                }
                                Console.WriteLine("** End of SNMP Version 2 TRAP data.");
                            }
                        });
                    }
                    else
                    {
                        if (inlen == 0)
                            Console.WriteLine("Zero length packet received.");
                    }
                }

            });
        }

        private void RandomInitialize()
        {
            using (RNGCryptoServiceProvider rg = new RNGCryptoServiceProvider())
            {
                byte[] rno = new byte[5];
                rg.GetBytes(rno);
                int randomvalue = BitConverter.ToInt32(rno, 0);
                rand = new Random(randomvalue);
            }
        }

        private string GetRandomName()
        {
            return $"Camera {rand.Next(1, 80000)}";
        }

        private void RaiseEvent(string eventName, string value)
        {
            var alarm = DeviceAlarms.NoAlarms;
            var status = DeviceStatus.Working;
            switch (eventName)
            {
                case "Alarms":
                    Enum.TryParse(value, out alarm);
                    break;
                case "OperationalStatus":
                    Enum.TryParse(value, out status);
                    break;
                default:break;
            };
            var device = new DeviceEvent
            {
                
                id = Guid.NewGuid(),
                eventName = eventName,
                deviceId = GetRandomName(),
                dateTime = DateTime.UtcNow,
                alarm = alarm,
                status = status,
                content = string.Empty                
            };

            TrapEvent?.Invoke(this, device);
        }
    }
}
