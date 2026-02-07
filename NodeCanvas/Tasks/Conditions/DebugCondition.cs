using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000056 RID: 86
	[Category("✫ Utility")]
	[Description("Simply use to debug return true or false by inverting the condition if needed")]
	public class DebugCondition : ConditionTask
	{
		// Token: 0x060001BB RID: 443 RVA: 0x00007CAA File Offset: 0x00005EAA
		protected override bool OnCheck()
		{
			return false;
		}
	}
}
