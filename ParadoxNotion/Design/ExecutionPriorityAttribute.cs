using System;

namespace ParadoxNotion.Design
{
	// Token: 0x020000CC RID: 204
	[AttributeUsage(4)]
	public class ExecutionPriorityAttribute : Attribute
	{
		// Token: 0x06000745 RID: 1861 RVA: 0x000170BA File Offset: 0x000152BA
		public ExecutionPriorityAttribute(int priority)
		{
			this.priority = priority;
		}

		// Token: 0x04000239 RID: 569
		public readonly int priority;
	}
}
