using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SnmpSharpNet;

namespace DeviceSimulator
{
    class SNMPAgent
    {
        public delegate void LogEventHandler(object sender, LogEventArgs le);
        public delegate void GetSetEventHandler(object sender, GetSetEventArgs gse);
        public event GetSetEventHandler GetEvent;
        public delegate void pGet(GetSetEventArgs gse);
        public event GetSetEventHandler SetEvent;
        private Thread snmpThread;
        public int Id;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public SNMPAgent(int id)
        {
            SNMPAgentThread snmpAgentThread = new SNMPAgentThread();
            snmpAgentThread.ReceiveEvent += new SNMPAgentThread.ReceiveEventHandler(snmpAgentThread_ReceiveEvent);
            snmpThread = new Thread(snmpAgentThread.ListenThreadLoop);
            snmpThread.Start();
            Id = id;
        }
        
        private void snmpAgentThread_ReceiveEvent(object sender, ReceiveEventArgs re)
        {
            int version = SnmpPacket.GetProtocolVersion(re.message, re.message.Length);
            if (version == (int)SnmpVersion.Ver2)
            {
                SnmpV2Packet inPacket = new SnmpV2Packet();
                try
                {
                    inPacket.decode(re.message, re.message.Length);
                }
                catch (Exception ex)
                { }
                if (inPacket.Pdu.Type == PduType.Get)
                {
                    foreach (Vb vb in inPacket.Pdu.VbList)
                    {
                        var value = "Sample";
                        Console.WriteLine($"Request received on {re.inEndPoint.Address}:{re.inEndPoint.Port} : {vb.Oid}");
                        Console.WriteLine($"Answering : {vb.Oid} = {value}");
                        SendResponse(vb.Oid, value, re.inEndPoint);
                    }
                }

            }
            
        }

        /// <summary>
        /// Send a trap at the IP specified.
        /// </summary>
        /// <param name="trapReceiverIp">IP of the recipient</param>
        /// <param name="trapReceiverPort">The port of the recipient</param>
        /// <param name="oidTrap"></param>
        /// <param name="oidCollection"></param>
        public static void SendTrap(string trapReceiverIp, int trapReceiverPort, Oid oidTrap, VbCollection oidCollection)
        {
            TrapAgent agent = new TrapAgent();
            agent.SendV2Trap(new IpAddress(trapReceiverIp), trapReceiverPort, AxisOidsHelper.Community, 13433, oidTrap, oidCollection);
        }

        /// <summary>
        /// Send a response after a Get or Set command.
        /// </summary>
        /// <param name="oid">Oid of the response</param>
        /// <param name="value">Value</param>
        /// <param name="outEndPoint">Out endpoint</param>
        public void SendResponse(Oid oid, string value, IPEndPoint outEndPoint)
        {
            SnmpV2Packet outPacket = new SnmpV2Packet();
            Pdu pdu = new Pdu(PduType.Response);
            VbCollection collection = new VbCollection();
            collection.Add(oid, new OctetString(value));
            pdu.SetVbList(collection);
            outPacket._pdu = pdu;
            byte[] outdata = outPacket.encode();
            Socket outSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            outSocket.Connect(outEndPoint);
            outSocket.Send(outdata);
        }
    }
}



