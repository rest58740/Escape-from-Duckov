using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x0200066C RID: 1644
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	[ComVisible(true)]
	public sealed class OptionalFieldAttribute : Attribute
	{
		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x06003D65 RID: 15717 RVA: 0x000D4A73 File Offset: 0x000D2C73
		// (set) Token: 0x06003D66 RID: 15718 RVA: 0x000D4A7B File Offset: 0x000D2C7B
		public int VersionAdded
		{
			get
			{
				return this.versionAdded;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Version value must be positive."));
				}
				this.versionAdded = value;
			}
		}

		// Token: 0x04002781 RID: 10113
		private int versionAdded = 1;
	}
}
