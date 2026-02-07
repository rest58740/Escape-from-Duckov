using System;
using FMOD;

namespace FMODUnity
{
	// Token: 0x02000117 RID: 279
	public class BankLoadException : Exception
	{
		// Token: 0x06000760 RID: 1888 RVA: 0x0000A4A0 File Offset: 0x000086A0
		public BankLoadException(string path, RESULT result) : base(string.Format("[FMOD] Could not load bank '{0}' : {1} : {2}", path, result.ToString(), Error.String(result)))
		{
			this.Path = path;
			this.Result = result;
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0000A4D4 File Offset: 0x000086D4
		public BankLoadException(string path, string error) : base(string.Format("[FMOD] Could not load bank '{0}' : {1}", path, error))
		{
			this.Path = path;
			this.Result = RESULT.ERR_INTERNAL;
		}

		// Token: 0x040005B9 RID: 1465
		public string Path;

		// Token: 0x040005BA RID: 1466
		public RESULT Result;
	}
}
