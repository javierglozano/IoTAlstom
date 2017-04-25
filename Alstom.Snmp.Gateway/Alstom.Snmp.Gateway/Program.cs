using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnmpFullFramework
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("*****************************************\n");
			Console.WriteLine("Snmp test\n,");
			Console.WriteLine("Press 2 to Test SNMPV2\n");
			Console.WriteLine("Press 3 to Test SNMPV3\n");
			Console.WriteLine("Press 4 to run trap listner\n");
			Console.WriteLine("Press any other key to exit");
			var selectedOption = Console.ReadLine();
			ISnmpManager snmpManager = null;
			TrapListener trapListener = null;
			switch(selectedOption)
			{
				case "2":
					snmpManager = new SnmpManagerV2();					
					break;
				case "3":
					snmpManager = new SnmpManagerV3();
					break;
				case "4":
					trapListener = new TrapListener();
					break;
			}
			
			
			if (snmpManager != null)
			{
				IEquipmentProvider equipmentProvider = new StaticEquipmentsProvider();
				var result = snmpManager.DiscoverSnmpAgents(equipmentProvider.GetAllCamerasOverNetwork());
				
				Console.WriteLine("camara discovered {0}",result);
				
			}

			if (trapListener != null)
			{
				trapListener.Initialize();
				trapListener.Start();
			}

			Console.ReadLine();
		}
	}
}
