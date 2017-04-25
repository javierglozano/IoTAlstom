using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnmpFullFramework
{
	public class SnmpManagerV3 : ISnmpManager
	{	
		public bool DiscoverSnmpAgents(IEnumerable<Camera> cameras)
		{
			bool result = false;


			result = this.DiscoverAgents(cameras.First(p => p.Name == "SNMPV3").CameraIp);

			return result;
		}


		private bool DiscoverAgents(IPAddress cameraAddress)
		{
			bool result = false;
		
			System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
			var test = ping.Send(cameraAddress);

			UdpTarget target = new UdpTarget(cameraAddress, 161, 5000, 3);

			SecureAgentParameters param = new SecureAgentParameters();
			

			if (!target.Discovery(param))
			{
				Console.WriteLine("Discovery failed. Unable to continue...");
				target.Close();

			}
			else
			{

				param.SecurityName.Set("mySecureName");
				param.Authentication = AuthenticationDigests.MD5;
				param.AuthenticationSecret.Set("alstom1!");
				param.Privacy = PrivacyProtocols.None;

				param.Reportable = false;

				SnmpV3Packet testResult;

				try
				{
					testResult = (SnmpV3Packet)target.Request(this.CreatePDU(PduType.Get), param);
					result = true;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
					testResult = null;
					result = false;
				}

			}
			return result;

			//param.SecurityName.Set("mySecureName");
		}

		private Pdu CreatePDU(PduType pduType)
		{
			Pdu result = new Pdu()
			{
				Type = pduType,

			};

			result.VbList.Add(new Oid("1.3.6.1.4.1.368.4.1.3.1.4.1.4"));

			return result;
		}

		
	}
}
