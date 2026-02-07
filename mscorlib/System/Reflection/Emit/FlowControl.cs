using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000927 RID: 2343
	[ComVisible(true)]
	[Serializable]
	public enum FlowControl
	{
		// Token: 0x0400316E RID: 12654
		Branch,
		// Token: 0x0400316F RID: 12655
		Break,
		// Token: 0x04003170 RID: 12656
		Call,
		// Token: 0x04003171 RID: 12657
		Cond_Branch,
		// Token: 0x04003172 RID: 12658
		Meta,
		// Token: 0x04003173 RID: 12659
		Next,
		// Token: 0x04003174 RID: 12660
		[Obsolete("This API has been deprecated.")]
		Phi,
		// Token: 0x04003175 RID: 12661
		Return,
		// Token: 0x04003176 RID: 12662
		Throw
	}
}
