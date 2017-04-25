using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using OnVifSimulator;

namespace OnVifSimulatorHostApp
{
    class Program
    {
        static int _port = 5357;
        static string _hostname = "127.0.0.1";

        static void Main(string[] args)
        {
            if (args.Length <= 1)
            {
                Console.WriteLine("Usage: {0} <hostname> <portname>");
            }
            else
            {
                _hostname = args[0];
                int.TryParse(args[1], out _port);
            }

            // Step 1 Create a URI to serve as the base address.  
            string uriString = String.Format("http://{0}:{1}", _hostname, _port);
            // string secUriString = String.Format("https://{0}:{1}/OnVifSimulatorService", _hostname, _port);
            Uri baseAddress = new Uri(uriString);
            // Uri secBaseAddress = new Uri(secUriString);
            // Uri[] baseAddresses = { baseAddress, secBaseAddress };

            Console.WriteLine("Service host will listen at {0}", baseAddress.ToString());

            // Step 2 Create a ServiceHost instance  
            ServiceHost selfHost = new ServiceHost(typeof(OnVifSimulatorService), baseAddress );
            // OnVifSimulatorService deviceHost = new OnVifSimulatorService();
            // ServiceHost selfHost = new ServiceHost(deviceHost, baseAddress);
            try
            {
                // Step 3 Add a service endpoint.  
                // WSHttpBinding() | BasicHttpBinding
                selfHost.AddServiceEndpoint(typeof(IOnVifDevice), new WSHttpBinding(), "OnVifSimulatorService");

                // Step 4 Enable metadata exchange.  
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                selfHost.Description.Behaviors.Add(smb);

                // Step 5 Start the service.  
                selfHost.Open();
                Console.WriteLine("The service is ready.");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.WriteLine();
                Console.ReadLine();

                // Close the ServiceHostBase to shutdown the service.  
                selfHost.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("An exception occurred: {0}", ce.Message);
                selfHost.Abort();
            }
        }
    }
}
