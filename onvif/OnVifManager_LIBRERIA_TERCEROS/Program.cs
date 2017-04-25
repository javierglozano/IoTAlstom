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
using odm.core;
using System.Reactive.Disposables;
using onvif.utils;
using System.Collections.Generic;
using System.Linq;
using utils;
using onvif.services;



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
          
    }
      

        public static void ManejaEventos(string uriStr)
        {
            NetworkCredential account = new NetworkCredential("julian", "julian");
            Uri uri;
            if (!Uri.TryCreate(uriStr, UriKind.Absolute, out uri))
            {
                Console.WriteLine("Uri string is in incorrect format! " + uriStr);
                Console.ReadKey();
            }
            SampleRunner runner = new SampleRunner(uri, account);

        }


        public static void MuestraInfo(string uri)
        {
            System.DateTime UTCTime = System.DateTime.UtcNow;

            Console.Write(string.Format("Client UTC Time: {0}", UTCTime.ToString("HH:mm:ss")));

            HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();
            var httpTransportBinding = new HttpTransportBindingElement { AuthenticationScheme = AuthenticationSchemes.Digest };
            var textMessageEncodingBinding = new TextMessageEncodingBindingElement { MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None) };
            var customBinding = new CustomBinding(textMessageEncodingBinding, httpTransportBinding);
            TextMessageEncodingBindingElement textMessageEncoding = new TextMessageEncodingBindingElement(MessageVersion.Soap12, Encoding.UTF8);

            EndpointAddress serviceAddress = new EndpointAddress(uri);
            ChannelFactory<Device> channelFactory = new ChannelFactory<Device>(customBinding, serviceAddress);

            var passwordDigestBehavior = new PasswordDigestBehavior("julian", "julian");
            channelFactory.Endpoint.Behaviors.Remove(typeof(ClientCredentials));
            channelFactory.Endpoint.Behaviors.Add(passwordDigestBehavior);

            var deviceClient = new DeviceClient(customBinding, serviceAddress);
            deviceClient.Endpoint.Behaviors.Add(passwordDigestBehavior);

            var unitTime = deviceClient.GetSystemDateAndTime();
            Console.Write((string.Format(" Camera UTC Time: {0}:{1}:{2}", unitTime.UTCDateTime.Time.Hour, unitTime.UTCDateTime.Time.Minute, unitTime.UTCDateTime.Time.Second)));

            ServiceReference1.CapabilityCategory[] cc = new ServiceReference1.CapabilityCategory[100];
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

        public class SampleRunner : IDisposable
        {
            public SampleRunner(Uri uri, NetworkCredential account)
            {
                // TODO: Complete member initialization

                Init(uri, account);
            }
            CompositeDisposable disposables = new CompositeDisposable();

            private void Init(Uri uri, NetworkCredential account)
            {
                NvtSessionFactory factory = new NvtSessionFactory(account);

                disposables.Add(factory.CreateSession(new[] { uri })
                    .Subscribe(
                    session => {
                        disposables.Add(new EventManager(session));
                        disposables.Add(new ConfigurationEditor(session));
                    }, err => {
                        Console.WriteLine(err.Message);
                    }
                ));
            }

            public void Dispose()
            {
                disposables.Dispose();
            }
        }

        public class ConfigurationEditor : IDisposable
        {
            private INvtSession session;
            onvif.utils.OdmSession facade;
            CompositeDisposable disposables = new CompositeDisposable();
            public ConfigurationEditor(INvtSession session)
            {
                this.session = session;
                RunConfigChanges();
            }
            void RunConfigChanges()
            {
                disposables.Add(
                    session.GetDeviceInformation()
                    .Subscribe(di => {
                        Console.WriteLine();
                        Console.WriteLine("Device information:");
                        Console.WriteLine("firmware     - " + di.FirmwareVersion);
                        Console.WriteLine("hardware     - " + di.HardwareId);
                        Console.WriteLine("manufacturer - " + di.Manufacturer);
                        Console.WriteLine("model        - " + di.Model);
                        Console.WriteLine("serial number- " + di.SerialNumber);
                        Console.WriteLine();
                    }, err => {
                        Console.WriteLine(err.Message);
                    }));
                disposables.Add(session.GetScopes()
                    .Subscribe(sc => {
                        var stringScopes = sc.Select(s => s.scopeItem);

                        DisplayNameLocation(stringScopes.ToArray());

                        //Uncomment this to change name/location
                        //SetNewNameLocation();
                    }, err => {
                        Console.WriteLine(err.Message);
                    }));
            }
            void DisplayNameLocation(string[] scopes)
            {
                Console.WriteLine();
                Console.WriteLine("Device name/location:");
                Console.WriteLine("name         - " + ScopeHelper.GetName(scopes));
                Console.WriteLine("location     - " + ScopeHelper.GetLocation(scopes));
                Console.WriteLine();
            }
            void SetNewNameLocation()
            {
                facade = new onvif.utils.OdmSession(session);

                disposables.Add(facade.SetNameLocation("New Name", "New Location")
                    .Subscribe(success => {
                        Console.WriteLine("-------------------------");
                        Console.WriteLine("New name/ location setted");
                        Console.WriteLine("-------------------------");

                        disposables.Add(session.GetScopes()
                            .Subscribe(sc => {
                                var stringScopes = sc.Select(s => s.scopeItem);
                                Console.WriteLine("");
                                Console.WriteLine("Checkng new name:");
                                Console.WriteLine("");
                                DisplayNameLocation(stringScopes.ToArray());
                            }, err => {
                                Console.WriteLine(err.Message);
                            }));

                    }, err => {
                        Console.WriteLine(err.Message);
                    }));
            }
            public void Dispose()
            {
            }
        }

        public class EventManager : IDisposable
        {
            public EventManager(INvtSession session)
            {
                // TODO: Complete member initialization
                this.session = session;

                Run();
            }

            private INvtSession session;
            CompositeDisposable disposables = new CompositeDisposable();

            public void Dispose()
            {
                disposables.Dispose();
            }

            private void Run()
            {
                OdmSession odmSess = new OdmSession(session);

                disposables.Add(odmSess.GetPullPointEvents()
                    .Subscribe(
                    evnt => {
                        //Parse onvif event here
                        Console.WriteLine(EventParse.ParseTopic(evnt.topic));
                        var messages = EventParse.ParseMessage(evnt.message);
                        messages.ForEach(msg => Console.WriteLine(msg));
                        Console.WriteLine("----------------------------------------");
                        Console.WriteLine();
                        Console.WriteLine();
                    }, err => {
                        Console.WriteLine(err.Message);
                    }
                ));
            }
        }
        public static class EventParse
        {
            public static string ParseTopic(onvif.services.TopicExpressionType topic)
            {
                string topicString = "";

                topic.Any.ForEach(node => {
                    topicString += "value: " + node.Value;
                });

                return topicString;
            }
            public static string[] ParseMessage(onvif.services.Message message)
            {
                List<string> messageStrings = new List<string>();

                messageStrings.Add("message id: " + message.key);

                if (message.source != null)
                    message.source.simpleItem.ForEach(sitem => {
                        string txt = sitem.name + "	" + sitem.value;
                        messageStrings.Add(txt);
                    });

                if (message.data != null)
                    message.data.simpleItem.ForEach(sitem => {
                        string txt = sitem.name + "	" + sitem.value;
                        messageStrings.Add(txt);
                    });

                return messageStrings.ToArray();
            }
        }
    }

}



