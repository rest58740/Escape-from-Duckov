using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200000D RID: 13
	[Category("✫ Blackboard")]
	[Description("Check if a boolean variable is true and if so, it is immediately reset to false.")]
	public class CheckBooleanTrigger : ConditionTask
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0000267A File Offset: 0x0000087A
		protected override string info
		{
			get
			{
				return string.Format("Trigger {0}", this.trigger);
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000268C File Offset: 0x0000088C
		protected override bool OnCheck()
		{
			if (this.trigger.value)
			{
				this.trigger.value = false;
				return true;
			}
			return false;
		}

		// Token: 0x0400001C RID: 28
		[BlackboardOnly]
		public BBParameter<bool> trigger;
	}
}
