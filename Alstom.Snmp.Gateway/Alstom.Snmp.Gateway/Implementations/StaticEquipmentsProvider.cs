using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnmpFullFramework
{
	public class StaticEquipmentsProvider : IEquipmentProvider
	{
		public IEnumerable<Camera> GetAllCamerasOverNetwork()
		{
			return new List<Camera>
			{
				new Camera { Name = "OnVifManager", CameraIp = IPAddress.Parse("172.0.1.214")},
				new Camera { Name = "SNMPV3", CameraIp = IPAddress.Parse("172.0.1.212") },
				new Camera { Name = "SNMPV2", CameraIp = IPAddress.Parse("172.0.1.211") }
			};
		}
	}
}
