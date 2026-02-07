using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020007F2 RID: 2034
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	public sealed class FixedBufferAttribute : Attribute
	{
		// Token: 0x060045FC RID: 17916 RVA: 0x000E57A5 File Offset: 0x000E39A5
		public FixedBufferAttribute(Type elementType, int length)
		{
			this.ElementType = elementType;
			this.Length = length;
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x060045FD RID: 17917 RVA: 0x000E57BB File Offset: 0x000E39BB
		public Type ElementType { get; }

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x060045FE RID: 17918 RVA: 0x000E57C3 File Offset: 0x000E39C3
		public int Length { get; }
	}
}
