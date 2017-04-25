using System.Collections.Generic;

namespace SnmpFullFramework
{
	interface IEquipmentProvider
	{
		IEnumerable<Camera> GetAllCamerasOverNetwork();
	}
}
