using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000734 RID: 1844
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.FUNCFLAGS instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Flags]
	[Serializable]
	public enum FUNCFLAGS : short
	{
		// Token: 0x04002B8E RID: 11150
		FUNCFLAG_FRESTRICTED = 1,
		// Token: 0x04002B8F RID: 11151
		FUNCFLAG_FSOURCE = 2,
		// Token: 0x04002B90 RID: 11152
		FUNCFLAG_FBINDABLE = 4,
		// Token: 0x04002B91 RID: 11153
		FUNCFLAG_FREQUESTEDIT = 8,
		// Token: 0x04002B92 RID: 11154
		FUNCFLAG_FDISPLAYBIND = 16,
		// Token: 0x04002B93 RID: 11155
		FUNCFLAG_FDEFAULTBIND = 32,
		// Token: 0x04002B94 RID: 11156
		FUNCFLAG_FHIDDEN = 64,
		// Token: 0x04002B95 RID: 11157
		FUNCFLAG_FUSESGETLASTERROR = 128,
		// Token: 0x04002B96 RID: 11158
		FUNCFLAG_FDEFAULTCOLLELEM = 256,
		// Token: 0x04002B97 RID: 11159
		FUNCFLAG_FUIDEFAULT = 512,
		// Token: 0x04002B98 RID: 11160
		FUNCFLAG_FNONBROWSABLE = 1024,
		// Token: 0x04002B99 RID: 11161
		FUNCFLAG_FREPLACEABLE = 2048,
		// Token: 0x04002B9A RID: 11162
		FUNCFLAG_FIMMEDIATEBIND = 4096
	}
}
