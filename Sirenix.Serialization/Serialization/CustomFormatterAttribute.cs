using System;
using System.ComponentModel;

namespace Sirenix.Serialization
{
	// Token: 0x0200005B RID: 91
	[AttributeUsage(4)]
	[Obsolete("Use a RegisterFormatterAttribute applied to the containing assembly instead.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class CustomFormatterAttribute : Attribute
	{
		// Token: 0x06000330 RID: 816 RVA: 0x00016F40 File Offset: 0x00015140
		public CustomFormatterAttribute()
		{
			this.Priority = 0;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00016F4F File Offset: 0x0001514F
		public CustomFormatterAttribute(int priority = 0)
		{
			this.Priority = priority;
		}

		// Token: 0x04000102 RID: 258
		public readonly int Priority;
	}
}
