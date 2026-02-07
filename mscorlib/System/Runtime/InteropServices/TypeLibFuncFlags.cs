using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006F7 RID: 1783
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum TypeLibFuncFlags
	{
		// Token: 0x04002A55 RID: 10837
		FRestricted = 1,
		// Token: 0x04002A56 RID: 10838
		FSource = 2,
		// Token: 0x04002A57 RID: 10839
		FBindable = 4,
		// Token: 0x04002A58 RID: 10840
		FRequestEdit = 8,
		// Token: 0x04002A59 RID: 10841
		FDisplayBind = 16,
		// Token: 0x04002A5A RID: 10842
		FDefaultBind = 32,
		// Token: 0x04002A5B RID: 10843
		FHidden = 64,
		// Token: 0x04002A5C RID: 10844
		FUsesGetLastError = 128,
		// Token: 0x04002A5D RID: 10845
		FDefaultCollelem = 256,
		// Token: 0x04002A5E RID: 10846
		FUiDefault = 512,
		// Token: 0x04002A5F RID: 10847
		FNonBrowsable = 1024,
		// Token: 0x04002A60 RID: 10848
		FReplaceable = 2048,
		// Token: 0x04002A61 RID: 10849
		FImmediateBind = 4096
	}
}
