using System;

namespace FMODUnity
{
	// Token: 0x02000116 RID: 278
	public class VCANotFoundException : Exception
	{
		// Token: 0x0600075F RID: 1887 RVA: 0x0000A481 File Offset: 0x00008681
		public VCANotFoundException(string path) : base("[FMOD] VCA not found '" + path + "'")
		{
			this.Path = path;
		}

		// Token: 0x040005B8 RID: 1464
		public string Path;
	}
}
