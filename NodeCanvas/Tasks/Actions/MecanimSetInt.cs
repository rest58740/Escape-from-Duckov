using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200005F RID: 95
	[Name("Set Parameter Integer", 0)]
	[Category("Animator")]
	[Description("You can either use a parameter name OR hashID. Leave the parameter name empty or none to use hashID instead.")]
	public class MecanimSetInt : ActionTask<Animator>
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000867C File Offset: 0x0000687C
		protected override string info
		{
			get
			{
				return string.Format("Mec.SetInt {0} to {1}", (string.IsNullOrEmpty(this.parameter.value) && !this.parameter.useBlackboard) ? this.parameterHashID.ToString() : this.parameter.ToString(), this.setTo);
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000086D0 File Offset: 0x000068D0
		protected override void OnExecute()
		{
			if (!string.IsNullOrEmpty(this.parameter.value))
			{
				base.agent.SetInteger(this.parameter.value, this.setTo.value);
			}
			else
			{
				base.agent.SetInteger(this.parameterHashID.value, this.setTo.value);
			}
			base.EndAction();
		}

		// Token: 0x04000129 RID: 297
		public BBParameter<string> parameter;

		// Token: 0x0400012A RID: 298
		public BBParameter<int> parameterHashID;

		// Token: 0x0400012B RID: 299
		public BBParameter<int> setTo;
	}
}
