using System;

namespace FMODUnity
{
	// Token: 0x02000115 RID: 277
	public class BusNotFoundException : Exception
	{
		// Token: 0x0600075E RID: 1886 RVA: 0x0000A462 File Offset: 0x00008662
		public BusNotFoundException(string path) : base("[FMOD] Bus not found '" + path + "'")
		{
			this.Path = path;
		}

		// Token: 0x040005B7 RID: 1463
		public string Path;
	}
}
