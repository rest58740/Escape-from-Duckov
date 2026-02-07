using System;

namespace System
{
	// Token: 0x020001CA RID: 458
	public enum LoaderOptimization
	{
		// Token: 0x0400144E RID: 5198
		[Obsolete("This method has been deprecated. Please use Assembly.Load() instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		DisallowBindings = 4,
		// Token: 0x0400144F RID: 5199
		[Obsolete("This method has been deprecated. Please use Assembly.Load() instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		DomainMask = 3,
		// Token: 0x04001450 RID: 5200
		MultiDomain = 2,
		// Token: 0x04001451 RID: 5201
		MultiDomainHost,
		// Token: 0x04001452 RID: 5202
		NotSpecified = 0,
		// Token: 0x04001453 RID: 5203
		SingleDomain
	}
}
