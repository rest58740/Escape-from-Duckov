using System;

namespace DG.DemiLib.Attributes
{
	// Token: 0x02000010 RID: 16
	[AttributeUsage(AttributeTargets.Class)]
	public class DeScriptExecutionOrderAttribute : Attribute
	{
		// Token: 0x0600002A RID: 42 RVA: 0x000030A6 File Offset: 0x000012A6
		public DeScriptExecutionOrderAttribute(int order)
		{
			this.order = order;
		}

		// Token: 0x0400003B RID: 59
		internal int order;
	}
}
