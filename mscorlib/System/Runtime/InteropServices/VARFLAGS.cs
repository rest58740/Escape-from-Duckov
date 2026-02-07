using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000735 RID: 1845
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.VARFLAGS instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Flags]
	[Serializable]
	public enum VARFLAGS : short
	{
		// Token: 0x04002B9C RID: 11164
		VARFLAG_FREADONLY = 1,
		// Token: 0x04002B9D RID: 11165
		VARFLAG_FSOURCE = 2,
		// Token: 0x04002B9E RID: 11166
		VARFLAG_FBINDABLE = 4,
		// Token: 0x04002B9F RID: 11167
		VARFLAG_FREQUESTEDIT = 8,
		// Token: 0x04002BA0 RID: 11168
		VARFLAG_FDISPLAYBIND = 16,
		// Token: 0x04002BA1 RID: 11169
		VARFLAG_FDEFAULTBIND = 32,
		// Token: 0x04002BA2 RID: 11170
		VARFLAG_FHIDDEN = 64,
		// Token: 0x04002BA3 RID: 11171
		VARFLAG_FRESTRICTED = 128,
		// Token: 0x04002BA4 RID: 11172
		VARFLAG_FDEFAULTCOLLELEM = 256,
		// Token: 0x04002BA5 RID: 11173
		VARFLAG_FUIDEFAULT = 512,
		// Token: 0x04002BA6 RID: 11174
		VARFLAG_FNONBROWSABLE = 1024,
		// Token: 0x04002BA7 RID: 11175
		VARFLAG_FREPLACEABLE = 2048,
		// Token: 0x04002BA8 RID: 11176
		VARFLAG_FIMMEDIATEBIND = 4096
	}
}
