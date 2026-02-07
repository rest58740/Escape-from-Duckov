using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000009 RID: 9
	[Name("Check Parameter Float", 0)]
	[Category("Animator")]
	public class MecanimCheckFloat : ConditionTask<Animator>
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000025 RID: 37 RVA: 0x0000251A File Offset: 0x0000071A
		protected override string info
		{
			get
			{
				string text = "Mec.Float ";
				string text2 = this.parameter.ToString();
				string compareString = OperationTools.GetCompareString(this.comparison);
				BBParameter<float> bbparameter = this.value;
				return text + text2 + compareString + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000254E File Offset: 0x0000074E
		protected override bool OnCheck()
		{
			return OperationTools.Compare(base.agent.GetFloat(this.parameter.value), this.value.value, this.comparison, 0.1f);
		}

		// Token: 0x04000013 RID: 19
		[RequiredField]
		public BBParameter<string> parameter;

		// Token: 0x04000014 RID: 20
		public CompareMethod comparison;

		// Token: 0x04000015 RID: 21
		public BBParameter<float> value;
	}
}
