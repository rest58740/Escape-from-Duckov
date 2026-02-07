using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006F6 RID: 1782
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum TypeLibTypeFlags
	{
		// Token: 0x04002A46 RID: 10822
		FAppObject = 1,
		// Token: 0x04002A47 RID: 10823
		FCanCreate = 2,
		// Token: 0x04002A48 RID: 10824
		FLicensed = 4,
		// Token: 0x04002A49 RID: 10825
		FPreDeclId = 8,
		// Token: 0x04002A4A RID: 10826
		FHidden = 16,
		// Token: 0x04002A4B RID: 10827
		FControl = 32,
		// Token: 0x04002A4C RID: 10828
		FDual = 64,
		// Token: 0x04002A4D RID: 10829
		FNonExtensible = 128,
		// Token: 0x04002A4E RID: 10830
		FOleAutomation = 256,
		// Token: 0x04002A4F RID: 10831
		FRestricted = 512,
		// Token: 0x04002A50 RID: 10832
		FAggregatable = 1024,
		// Token: 0x04002A51 RID: 10833
		FReplaceable = 2048,
		// Token: 0x04002A52 RID: 10834
		FDispatchable = 4096,
		// Token: 0x04002A53 RID: 10835
		FReverseBind = 8192
	}
}
