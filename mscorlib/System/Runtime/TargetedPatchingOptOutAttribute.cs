using System;

namespace System.Runtime
{
	// Token: 0x0200054D RID: 1357
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class TargetedPatchingOptOutAttribute : Attribute
	{
		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x060035A9 RID: 13737 RVA: 0x000C1F98 File Offset: 0x000C0198
		public string Reason { get; }

		// Token: 0x060035AA RID: 13738 RVA: 0x000C1FA0 File Offset: 0x000C01A0
		public TargetedPatchingOptOutAttribute(string reason)
		{
			this.Reason = reason;
		}
	}
}
