using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000455 RID: 1109
	[ComVisible(true)]
	[Serializable]
	public enum SecurityAction
	{
		// Token: 0x0400208A RID: 8330
		Demand = 2,
		// Token: 0x0400208B RID: 8331
		Assert,
		// Token: 0x0400208C RID: 8332
		[Obsolete("This requests should not be used")]
		Deny,
		// Token: 0x0400208D RID: 8333
		PermitOnly,
		// Token: 0x0400208E RID: 8334
		LinkDemand,
		// Token: 0x0400208F RID: 8335
		InheritanceDemand,
		// Token: 0x04002090 RID: 8336
		[Obsolete("This requests should not be used")]
		RequestMinimum,
		// Token: 0x04002091 RID: 8337
		[Obsolete("This requests should not be used")]
		RequestOptional,
		// Token: 0x04002092 RID: 8338
		[Obsolete("This requests should not be used")]
		RequestRefuse
	}
}
