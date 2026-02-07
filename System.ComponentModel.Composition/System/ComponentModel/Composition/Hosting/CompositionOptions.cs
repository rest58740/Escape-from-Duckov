using System;

namespace System.ComponentModel.Composition.Hosting
{
	// Token: 0x020000D1 RID: 209
	[Flags]
	public enum CompositionOptions
	{
		// Token: 0x0400024F RID: 591
		Default = 0,
		// Token: 0x04000250 RID: 592
		DisableSilentRejection = 1,
		// Token: 0x04000251 RID: 593
		IsThreadSafe = 2,
		// Token: 0x04000252 RID: 594
		ExportCompositionService = 4
	}
}
