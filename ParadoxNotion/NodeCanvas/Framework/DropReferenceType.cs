using System;

namespace NodeCanvas.Framework
{
	// Token: 0x02000004 RID: 4
	[AttributeUsage(4)]
	public class DropReferenceType : Attribute
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000020CB File Offset: 0x000002CB
		public DropReferenceType(Type type)
		{
			this.type = type;
		}

		// Token: 0x04000005 RID: 5
		public readonly Type type;
	}
}
