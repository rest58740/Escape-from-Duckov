using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200000A RID: 10
	[Name("Check Parameter Int", 0)]
	[Category("Animator")]
	public class MecanimCheckInt : ConditionTask<Animator>
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000028 RID: 40 RVA: 0x0000258B File Offset: 0x0000078B
		protected override string info
		{
			get
			{
				string text = "Mec.Int ";
				string text2 = this.parameter.ToString();
				string compareString = OperationTools.GetCompareString(this.comparison);
				BBParameter<int> bbparameter = this.value;
				return text + text2 + compareString + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000025BF File Offset: 0x000007BF
		protected override bool OnCheck()
		{
			return OperationTools.Compare(base.agent.GetInteger(this.parameter.value), this.value.value, this.comparison);
		}

		// Token: 0x04000016 RID: 22
		[RequiredField]
		public BBParameter<string> parameter;

		// Token: 0x04000017 RID: 23
		public CompareMethod comparison;

		// Token: 0x04000018 RID: 24
		public BBParameter<int> value;
	}
}
