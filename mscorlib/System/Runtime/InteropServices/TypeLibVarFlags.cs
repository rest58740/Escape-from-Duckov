using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006F8 RID: 1784
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum TypeLibVarFlags
	{
		// Token: 0x04002A63 RID: 10851
		FReadOnly = 1,
		// Token: 0x04002A64 RID: 10852
		FSource = 2,
		// Token: 0x04002A65 RID: 10853
		FBindable = 4,
		// Token: 0x04002A66 RID: 10854
		FRequestEdit = 8,
		// Token: 0x04002A67 RID: 10855
		FDisplayBind = 16,
		// Token: 0x04002A68 RID: 10856
		FDefaultBind = 32,
		// Token: 0x04002A69 RID: 10857
		FHidden = 64,
		// Token: 0x04002A6A RID: 10858
		FRestricted = 128,
		// Token: 0x04002A6B RID: 10859
		FDefaultCollelem = 256,
		// Token: 0x04002A6C RID: 10860
		FUiDefault = 512,
		// Token: 0x04002A6D RID: 10861
		FNonBrowsable = 1024,
		// Token: 0x04002A6E RID: 10862
		FReplaceable = 2048,
		// Token: 0x04002A6F RID: 10863
		FImmediateBind = 4096
	}
}
