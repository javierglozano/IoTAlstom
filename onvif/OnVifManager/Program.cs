using System;

using System.Text;

using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Xml;
using System.Net;

using System.ServiceModel.Channels;
using OnVifManager.ServiceReference1;
using System.ServiceModel.Description;
using OnVifManager.ServiceReference3;

namespace OnVifManager
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Discovering devices...");
            var endPoint = new UdpDiscoveryEndpoint(DiscoveryVersion.WSDiscoveryApril2005);
            FindCriteria findCriteria = new FindCriteria();
            var contractTypeName = "NetworkVideoTransmitter";
            var contractTypeNamespace = "http://www.onvif.org/ver10/network/wsdl";

            // those parametes are defined by onvif standard
            findCriteria.ContractTypeNames.Add(new XmlQualifiedName(contractTypeName, contractTypeNamespace));

            var discoveryClient = new DiscoveryClient(endPoint);
            discoveryClient.FindProgressChanged += discoveryClient_FindProgressChanged;
            discoveryClient.FindCompleted += discoveryClient_FindCompleted;


            findCriteria.Duration = TimeSpan.MaxValue;
            findCriteria.MaxResults = int.MaxValue;
            discoveryClient.FindAsync(findCriteria);

            Console.ReadKey();
        }
        static void discoveryClient_FindCompleted(object sender, FindCompletedEventArgs e)
        {
            Console.WriteLine("--------------- Search finished ---------------");
        }


        static void discoveryClient_FindProgressChanged(object sender, FindProgressChangedEventArgs e)
        {
            //var lines = "\r\n--------" + System.DateTime.Now.ToString() + "\r\n" + e.EndpointDiscoveryMetadata.Address.Uri.AbsoluteUri.ToString();
            //Console.WriteLine("\r\n--------" + System.DateTime.Now.ToString() + "\r\n" + e.EndpointDiscoveryMetadata.Address.Uri.AbsoluteUri.ToString());

            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"info.txt", true))
            //{
            //    file.WriteLine(lines);
            //}


            foreach (var item in e.EndpointDiscoveryMetadata.ListenUris)
                {
                    string uri = item.OriginalString;
                    Console.WriteLine("Uri found: " + uri);
                try
                {
                    if (uri.Contains("http://172.0.1.214"))
                    {
                        //MuestraInfo(uri);
                        ManejaEventos(uri);
                        //ManejaEventos2(uri);





                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine("excepcion requesting data to: " + uri);
                }
            }

            //MuestraInfoTest("http://172.0.1.213:5357/OnVifSimulatorService");

        }
      

        public static void ManejaEventos(string uri)
        {
            try
            {

                ServicePointManager.Expect100Continue = false;
                EndpointAddress endPointAddress = new EndpointAddress(uri);
                HttpTransportBindingElement httpTransportBinding = new HttpTransportBindingElement { AuthenticationScheme = AuthenticationSchemes.Digest };
                httpTransportBinding.KeepAliveEnabled = true;
                TextMessageEncodingBindingElement textMessageEncodingBinding = new TextMessageEncodingBindingElement { MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.WSAddressing10) };
                PasswordDigestBehavior passwordDigestBehavior = new PasswordDigestBehavior("julian", "julian");

                CustomBinding customBinding = new CustomBinding(textMessageEncodingBinding, httpTransportBinding);
                customBinding.SendTimeout = new TimeSpan(0, 0, 10);


                
                EventPortTypeClient ept = new EventPortTypeClient(customBinding, endPointAddress);
                ept.Endpoint.Behaviors.Add(passwordDigestBehavior);
                Console.WriteLine("Status 1 : " + ept.State.ToString());
                
                ept.Open();
                Console.WriteLine("Status 2 : " + ept.State.ToString());

                ept.GetType();
                
                ept.GetHashCode();





                FilterType filter = new FilterType();
                string initTermTime = null;
                CreatePullPointSubscriptionSubscriptionPolicy policy =
                    new CreatePullPointSubscriptionSubscriptionPolicy();

                XmlElement[] elems1 = new XmlElement[10];
                System.DateTime time1 = new System.DateTime();
                Nullable<System.DateTime> time2 = new System.DateTime();

                EndpointReferenceType endpoint = ept.CreatePullPointSubscription(filter, "PT60M", policy, ref elems1, out time1, out time2);


                System.DateTime CurrentTime = new System.DateTime();
                NotificationMessageHolderType[] NotificationMessages =   new NotificationMessageHolderType[0];
                PullPointSubscriptionClient.PullMessages()

                PullPointSubscriptionClient.PullMessages("PT5M", 99, Any, out CurrentTime, out NotificationMessages);


                PullPointSubscriptionClient subbind = new PullPointSubscriptionClient(customBinding, endPointAddress);
                subbind.Endpoint.Behaviors.Add(passwordDigestBehavior);



                string timeOut = "PT60.000S";
                int mesLimit = 100;
                XmlElement[] elemns2 = new XmlElement[10];
                System.DateTime termTime = new System.DateTime();
                NotificationMessageHolderType[] messHolder =
                    new NotificationMessageHolderType[0];



                subbind.PullMessages(timeOut, mesLimit, elemns2, out termTime, out messHolder);

                for (int i = 0; i < messHolder.Length; i++)
                {
                    Console.WriteLine("parsing");

                }


                Console.WriteLine("--------- NO CATCH --------- ");

            }
            catch (Exception eee)
            {

                Console.WriteLine("manejando eventos: " + eee);
            }
        }

        private static void Cap_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static void MuestraInfoTest(string uri)
        {
            System.DateTime UTCTime = System.DateTime.UtcNow;

            Console.Write(string.Format("Client UTC Time: {0}", UTCTime.ToString("HH:mm:ss")));

            HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();
            var httpTransportBinding = new HttpTransportBindingElement { AuthenticationScheme = AuthenticationSchemes.Anonymous };
            var textMessageEncodingBinding = new TextMessageEncodingBindingElement { MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None) };
            var customBinding = new CustomBinding(textMessageEncodingBinding, httpTransportBinding);

            EndpointAddress serviceAddress = new EndpointAddress(uri);

            var deviceClient = new DeviceClient(customBinding, serviceAddress);
            Console.Write(" GetHostname: " + deviceClient.GetHostname().Name);
            var unitTime = deviceClient.GetSystemDateAndTime();
            Console.Write((string.Format(" Camera UTC Time: {0}:{1}:{2}", unitTime.UTCDateTime.Time.Hour, unitTime.UTCDateTime.Time.Minute, unitTime.UTCDateTime.Time.Second)));

            CapabilityCategory[] cc = new CapabilityCategory[100];
            deviceClient.GetCapabilities(cc);

            Console.Write(" GetHostname: " + deviceClient.GetHostname().Name);
            Console.Write(" GetWsdlUrl: " + deviceClient.GetWsdlUrl());


            string model, firmwareVersion, serialNumber, hardwareId;
            deviceClient.GetDeviceInformation(out model, out firmwareVersion, out serialNumber, out hardwareId);
            Console.Write(" Model: " + model);
            Console.Write(" firmwareVersion: " + firmwareVersion);
            Console.Write(" serialNumber: " + serialNumber);
            Console.WriteLine("hardwareId: " + hardwareId + "\n\n");
        }


        public static void MuestraInfo(string uri)
        {
            System.DateTime UTCTime = System.DateTime.UtcNow;

            Console.Write(string.Format("Client UTC Time: {0}", UTCTime.ToString("HH:mm:ss")));

            HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();
            var httpTransportBinding = new HttpTransportBindingElement { AuthenticationScheme = AuthenticationSchemes.Digest };
            var textMessageEncodingBinding = new TextMessageEncodingBindingElement { MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None) };
            var customBinding = new CustomBinding(textMessageEncodingBinding, httpTransportBinding);

            EndpointAddress serviceAddress = new EndpointAddress(uri);

            var passwordDigestBehavior = new PasswordDigestBehavior("julian", "julian");

            var deviceClient = new DeviceClient(customBinding, serviceAddress);
            deviceClient.Endpoint.Behaviors.Add(passwordDigestBehavior);
            Console.Write(" GetHostname: " + deviceClient.GetHostname().Name);
            var unitTime = deviceClient.GetSystemDateAndTime();
            Console.Write((string.Format(" Camera UTC Time: {0}:{1}:{2}", unitTime.UTCDateTime.Time.Hour, unitTime.UTCDateTime.Time.Minute, unitTime.UTCDateTime.Time.Second)));

            CapabilityCategory[] cc = new CapabilityCategory[100];
            deviceClient.GetCapabilities(cc);

            Console.Write(" GetHostname: " + deviceClient.GetHostname().Name);
            Console.Write(" GetWsdlUrl: " + deviceClient.GetWsdlUrl());


            string model, firmwareVersion, serialNumber, hardwareId;
            deviceClient.GetDeviceInformation(out model, out firmwareVersion, out serialNumber, out hardwareId);
            Console.Write(" Model: " + model);
            Console.Write(" firmwareVersion: " + firmwareVersion);
            Console.Write(" serialNumber: " + serialNumber);
            Console.WriteLine("hardwareId: " + hardwareId + "\n\n");
        }
        private static void Filter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

}



