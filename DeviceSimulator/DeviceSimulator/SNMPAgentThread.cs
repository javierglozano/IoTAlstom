using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DeviceSimulator
{
    /// <summary>
    /// Thread to receive the Get and Set commands.
    /// </summary>
    class SNMPAgentThread
    {
        private UdpClient SNMPListener;
        private IPEndPoint ipEnPoint;
        public delegate void ReceiveEventHandler(object sender, ReceiveEventArgs re);
        public event ReceiveEventHandler ReceiveEvent;
        public static bool messageReceived = true;
      
        /// <summary>
        /// Listening tread.
        /// </summary>
        public void ListenThreadLoop()
        {
            ipEnPoint = new IPEndPoint(IPAddress.Loopback, 161);
            SNMPListener = new UdpClient(ipEnPoint);
            SNMPState state = new SNMPState(SNMPListener, ipEnPoint);
            SNMPListener.BeginReceive(ReceiveCallback, state);
            while (Thread.CurrentThread.IsAlive)
                Thread.Sleep(100);
        }
        
        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            byte[] receiveBytes = SNMPListener.EndReceive(asyncResult, ref ipEnPoint);
            ReceiveEventArgs receiveArgs = new ReceiveEventArgs(receiveBytes, ipEnPoint);
            ReceiveEvent.Invoke(this, receiveArgs);
            SNMPListener.BeginReceive(ReceiveCallback, new SNMPState(SNMPListener, ipEnPoint));
        }
    }
}
