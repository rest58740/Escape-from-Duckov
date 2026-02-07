using System;
using System.Diagnostics;

namespace Animancer
{
	// Token: 0x0200001C RID: 28
	[AttributeUsage(AttributeTargets.Field)]
	[Conditional("UNITY_EDITOR")]
	public class DefaultValueAttribute : Attribute
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000316 RID: 790 RVA: 0x00008EDC File Offset: 0x000070DC
		// (set) Token: 0x06000317 RID: 791 RVA: 0x00008EE4 File Offset: 0x000070E4
		public virtual object Primary { get; protected set; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000318 RID: 792 RVA: 0x00008EED File Offset: 0x000070ED
		// (set) Token: 0x06000319 RID: 793 RVA: 0x00008EF5 File Offset: 0x000070F5
		public virtual object Secondary { get; protected set; }

		// Token: 0x0600031A RID: 794 RVA: 0x00008EFE File Offset: 0x000070FE
		public DefaultValueAttribute(object primary, object secondary = null)
		{
			this.Primary = primary;
			this.Secondary = secondary;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00008F14 File Offset: 0x00007114
		protected DefaultValueAttribute()
		{
		}
	}
}
