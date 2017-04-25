using System.Collections.Generic;
using System.Net;

namespace SnmpFullFramework
{
	public interface ISnmpManager
	{
		bool DiscoverSnmpAgents(IEnumerable<Camera> cameras);
	}
}