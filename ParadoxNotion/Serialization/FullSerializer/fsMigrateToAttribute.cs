using System;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x02000099 RID: 153
	[AttributeUsage(4)]
	public class fsMigrateToAttribute : Attribute
	{
		// Token: 0x060005ED RID: 1517 RVA: 0x000111F3 File Offset: 0x0000F3F3
		public fsMigrateToAttribute(Type targetType)
		{
			this.targetType = targetType;
		}

		// Token: 0x040001D5 RID: 469
		public readonly Type targetType;
	}
}
