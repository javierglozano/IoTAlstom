using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnVifSimulatorClientApp.OnVifClientApp;

namespace OnVifSimulatorClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Creating service client...");
            OnVifDeviceClient client = new OnVifDeviceClient("WSHttpBinding_IOnVifDevice");
            Console.WriteLine("Ok!");

            Console.Write("Calling Operation 'SetDiscoveryMode()' ....");
            client.SetDiscoveryMode(DiscoveryMode.Discoverable);
            Console.WriteLine("Ok!");

            Console.Write("Calling Operation 'GetDiscoveryMode()' ....");
            DiscoveryMode mode = client.GetDiscoveryMode();
            Console.WriteLine("Ok!. Result: {0}", mode);

        }
    }
}
