using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200000C RID: 12
	[Category("✫ Blackboard")]
	public class CheckBoolean : ConditionTask
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002E RID: 46 RVA: 0x0000261C File Offset: 0x0000081C
		protected override string info
		{
			get
			{
				BBParameter<bool> bbparameter = this.valueA;
				string text = (bbparameter != null) ? bbparameter.ToString() : null;
				string text2 = " == ";
				BBParameter<bool> bbparameter2 = this.valueB;
				return text + text2 + ((bbparameter2 != null) ? bbparameter2.ToString() : null);
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000264C File Offset: 0x0000084C
		protected override bool OnCheck()
		{
			return this.valueA.value == this.valueB.value;
		}

		// Token: 0x0400001A RID: 26
		[BlackboardOnly]
		public BBParameter<bool> valueA;

		// Token: 0x0400001B RID: 27
		public BBParameter<bool> valueB = true;
	}
}
