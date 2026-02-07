using System;

namespace System.Collections
{
	// Token: 0x02000A4F RID: 2639
	[Obsolete("Please use IEqualityComparer instead.")]
	public interface IHashCodeProvider
	{
		// Token: 0x06005EAB RID: 24235
		int GetHashCode(object obj);
	}
}
