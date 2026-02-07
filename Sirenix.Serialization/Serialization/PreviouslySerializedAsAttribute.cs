using System;

namespace Sirenix.Serialization
{
	// Token: 0x02000070 RID: 112
	[AttributeUsage(384)]
	public class PreviouslySerializedAsAttribute : Attribute
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0001953C File Offset: 0x0001773C
		// (set) Token: 0x06000392 RID: 914 RVA: 0x00019544 File Offset: 0x00017744
		public string Name { get; private set; }

		// Token: 0x06000393 RID: 915 RVA: 0x0001954D File Offset: 0x0001774D
		public PreviouslySerializedAsAttribute(string name)
		{
			this.Name = name;
		}
	}
}
