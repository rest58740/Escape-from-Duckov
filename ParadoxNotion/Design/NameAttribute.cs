using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000D0 RID: 208
	[AttributeUsage(32767)]
	public class NameAttribute : Attribute
	{
		// Token: 0x06000749 RID: 1865 RVA: 0x000170F6 File Offset: 0x000152F6
		public NameAttribute(string name, int priority = 0)
		{
			this.name = name;
			this.priority = priority;
		}

		// Token: 0x0400023D RID: 573
		public readonly string name;

		// Token: 0x0400023E RID: 574
		public readonly int priority;
	}
}
