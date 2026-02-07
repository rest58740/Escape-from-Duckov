using System;
using FMOD;

namespace FMODUnity
{
	// Token: 0x02000118 RID: 280
	public class SystemNotInitializedException : Exception
	{
		// Token: 0x06000762 RID: 1890 RVA: 0x0000A4F7 File Offset: 0x000086F7
		public SystemNotInitializedException(RESULT result, string location) : base(string.Format("[FMOD] Initialization failed : {2} : {0} : {1}", result.ToString(), Error.String(result), location))
		{
			this.Result = result;
			this.Location = location;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0000A52B File Offset: 0x0000872B
		public SystemNotInitializedException(Exception inner) : base("[FMOD] Initialization failed", inner)
		{
		}

		// Token: 0x040005BB RID: 1467
		public RESULT Result;

		// Token: 0x040005BC RID: 1468
		public string Location;
	}
}
