using System;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x02000098 RID: 152
	[AttributeUsage(256)]
	public sealed class fsSerializeAsAttribute : Attribute
	{
		// Token: 0x060005EB RID: 1515 RVA: 0x000111DC File Offset: 0x0000F3DC
		public fsSerializeAsAttribute()
		{
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000111E4 File Offset: 0x0000F3E4
		public fsSerializeAsAttribute(string name)
		{
			this.Name = name;
		}

		// Token: 0x040001D4 RID: 468
		public readonly string Name;
	}
}
