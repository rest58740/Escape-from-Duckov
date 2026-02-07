using System;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x0200009A RID: 154
	[AttributeUsage(4)]
	public class fsMigrateVersionsAttribute : Attribute
	{
		// Token: 0x060005EE RID: 1518 RVA: 0x00011202 File Offset: 0x0000F402
		public fsMigrateVersionsAttribute(params Type[] previousTypes)
		{
			this.previousTypes = previousTypes;
		}

		// Token: 0x040001D6 RID: 470
		public readonly Type[] previousTypes;
	}
}
