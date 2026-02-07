using System;

namespace Sirenix.Serialization
{
	// Token: 0x0200005E RID: 94
	[AttributeUsage(1, AllowMultiple = true)]
	public sealed class BindTypeNameToTypeAttribute : Attribute
	{
		// Token: 0x06000337 RID: 823 RVA: 0x00017004 File Offset: 0x00015204
		public BindTypeNameToTypeAttribute(string oldFullTypeName, Type newType)
		{
			this.OldTypeName = oldFullTypeName;
			this.NewType = newType;
		}

		// Token: 0x04000107 RID: 263
		internal readonly Type NewType;

		// Token: 0x04000108 RID: 264
		internal readonly string OldTypeName;
	}
}
