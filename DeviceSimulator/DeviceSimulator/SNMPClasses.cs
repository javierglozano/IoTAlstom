using System;
using System.Net;
using System.Net.Sockets;
using SnmpSharpNet;

namespace DeviceSimulator
{
    public class SNMPState
    {
        public UdpClient udpClient { get; set; }
        public IPEndPoint ipEndPoint { get; set; }
        public SNMPState(UdpClient udpClient, IPEndPoint ipEndpoint)
        {
            this.udpClient = udpClient;
            this.ipEndPoint = ipEndPoint;
        }
    }

    public class LogEventArgs : EventArgs
    {
        public string text;
        public LogEventArgs(string text)
        {
            this.text = text;
        }
    }

    public class ReceiveEventArgs : EventArgs
    {
        public byte[] message;
        public IPEndPoint inEndPoint;
        public ReceiveEventArgs(byte[] message, IPEndPoint inEndPoint)
        {
            this.message = message;
            this.inEndPoint = inEndPoint;
        }
    }

    public class GetSetEventArgs : EventArgs
    {
        public Vb value;
        public IPEndPoint inEndPoint;
        public GetSetEventArgs(Vb value, IPEndPoint inEndPoint)
        {
            this.value = value;
            this.inEndPoint = inEndPoint;
        }
    }
}
