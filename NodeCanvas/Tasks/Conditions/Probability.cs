using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000057 RID: 87
	[Category("✫ Utility")]
	[Description("Return true or false based on the probability settings. The chance is rolled for once whenever the condition is enabled.")]
	public class Probability : ConditionTask
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00007CB8 File Offset: 0x00005EB8
		protected override string info
		{
			get
			{
				return (this.probability.value / this.maxValue.value * 100f).ToString() + "%";
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00007CF4 File Offset: 0x00005EF4
		protected override void OnEnable()
		{
			this.success = (Random.Range(0f, this.maxValue.value) <= this.probability.value);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00007D21 File Offset: 0x00005F21
		protected override bool OnCheck()
		{
			return this.success;
		}

		// Token: 0x04000101 RID: 257
		public BBParameter<float> probability = 0.5f;

		// Token: 0x04000102 RID: 258
		public BBParameter<float> maxValue = 1f;

		// Token: 0x04000103 RID: 259
		private bool success;
	}
}
