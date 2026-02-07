using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000753 RID: 1875
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum TypeLibExporterFlags
	{
		// Token: 0x04002C08 RID: 11272
		OnlyReferenceRegistered = 1,
		// Token: 0x04002C09 RID: 11273
		None = 0,
		// Token: 0x04002C0A RID: 11274
		CallerResolvedReferences = 2,
		// Token: 0x04002C0B RID: 11275
		OldNames = 4,
		// Token: 0x04002C0C RID: 11276
		ExportAs32Bit = 16,
		// Token: 0x04002C0D RID: 11277
		ExportAs64Bit = 32
	}
}
