using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000011 RID: 17
	[Category("✫ Blackboard")]
	[Description("Check whether or not a variable is null")]
	public class CheckNull : ConditionTask
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002806 File Offset: 0x00000A06
		protected override string info
		{
			get
			{
				BBParameter<object> bbparameter = this.variable;
				return ((bbparameter != null) ? bbparameter.ToString() : null) + " == null";
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002824 File Offset: 0x00000A24
		protected override bool OnCheck()
		{
			return ObjectUtils.AnyEquals(this.variable.value, null);
		}

		// Token: 0x04000026 RID: 38
		[BlackboardOnly]
		public BBParameter<object> variable;
	}
}
