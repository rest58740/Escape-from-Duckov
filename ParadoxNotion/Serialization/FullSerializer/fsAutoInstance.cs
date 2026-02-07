using System;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x0200009E RID: 158
	[AttributeUsage(4)]
	public sealed class fsAutoInstance : Attribute
	{
		// Token: 0x060005F2 RID: 1522 RVA: 0x00011229 File Offset: 0x0000F429
		public fsAutoInstance(bool makeInstance = true)
		{
			this.makeInstance = makeInstance;
		}

		// Token: 0x040001D7 RID: 471
		public readonly bool makeInstance;
	}
}
