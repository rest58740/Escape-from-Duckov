using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000062 RID: 98
	[Name("Set Parameter Trigger", 0)]
	[Category("Animator")]
	[Description("You can either use a parameter name OR hashID. Leave the parameter name empty or none to use hashID instead.")]
	public class MecanimSetTrigger : ActionTask<Animator>
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x000088AC File Offset: 0x00006AAC
		protected override string info
		{
			get
			{
				return string.Format("Mec.SetTrigger {0}", (string.IsNullOrEmpty(this.parameter.value) && !this.parameter.useBlackboard) ? this.parameterHashID.ToString() : this.parameter.ToString());
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000088FC File Offset: 0x00006AFC
		protected override void OnExecute()
		{
			if (!string.IsNullOrEmpty(this.parameter.value))
			{
				base.agent.SetTrigger(this.parameter.value);
			}
			else
			{
				base.agent.SetTrigger(this.parameterHashID.value);
			}
			base.EndAction();
		}

		// Token: 0x04000132 RID: 306
		public BBParameter<string> parameter;

		// Token: 0x04000133 RID: 307
		public BBParameter<int> parameterHashID;
	}
}
