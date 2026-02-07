using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020007D8 RID: 2008
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class AccessedThroughPropertyAttribute : Attribute
	{
		// Token: 0x060045B8 RID: 17848 RVA: 0x000E5158 File Offset: 0x000E3358
		public AccessedThroughPropertyAttribute(string propertyName)
		{
			this.PropertyName = propertyName;
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x060045B9 RID: 17849 RVA: 0x000E5167 File Offset: 0x000E3367
		public string PropertyName { get; }
	}
}
