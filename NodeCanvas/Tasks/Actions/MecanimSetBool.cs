using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200005C RID: 92
	[Name("Set Parameter Bool", 0)]
	[Category("Animator")]
	[Description("You can either use a parameter name OR hashID. Leave the parameter name empty or none to use hashID instead.")]
	public class MecanimSetBool : ActionTask<Animator>
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x00008378 File Offset: 0x00006578
		protected override string info
		{
			get
			{
				return string.Format("Mec.SetBool {0} to {1}", (string.IsNullOrEmpty(this.parameter.value) && !this.parameter.useBlackboard) ? this.parameterHashID.ToString() : this.parameter.ToString(), this.setTo);
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000083CC File Offset: 0x000065CC
		protected override void OnExecute()
		{
			if (!string.IsNullOrEmpty(this.parameter.value))
			{
				base.agent.SetBool(this.parameter.value, this.setTo.value);
			}
			else
			{
				base.agent.SetBool(this.parameterHashID.value, this.setTo.value);
			}
			base.EndAction(true);
		}

		// Token: 0x0400011E RID: 286
		public BBParameter<string> parameter;

		// Token: 0x0400011F RID: 287
		public BBParameter<int> parameterHashID;

		// Token: 0x04000120 RID: 288
		public BBParameter<bool> setTo;
	}
}
