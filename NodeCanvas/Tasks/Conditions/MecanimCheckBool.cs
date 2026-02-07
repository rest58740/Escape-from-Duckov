using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000008 RID: 8
	[Name("Check Parameter Bool", 0)]
	[Category("Animator")]
	public class MecanimCheckBool : ConditionTask<Animator>
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000024BF File Offset: 0x000006BF
		protected override string info
		{
			get
			{
				string text = "Mec.Bool ";
				string text2 = this.parameter.ToString();
				string text3 = " == ";
				BBParameter<bool> bbparameter = this.value;
				return text + text2 + text3 + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000024ED File Offset: 0x000006ED
		protected override bool OnCheck()
		{
			return base.agent.GetBool(this.parameter.value) == this.value.value;
		}

		// Token: 0x04000011 RID: 17
		[RequiredField]
		public BBParameter<string> parameter;

		// Token: 0x04000012 RID: 18
		public BBParameter<bool> value;
	}
}
