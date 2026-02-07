using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000D7 RID: 215
	[Category("GameObject")]
	[Description("Checks the current speed of the agent against a value based on it's Rigidbody velocity")]
	public class CheckSpeed : ConditionTask<Rigidbody>
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0000E59B File Offset: 0x0000C79B
		protected override string info
		{
			get
			{
				string text = "Speed";
				string compareString = OperationTools.GetCompareString(this.checkType);
				BBParameter<float> bbparameter = this.value;
				return text + compareString + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000E5C4 File Offset: 0x0000C7C4
		protected override bool OnCheck()
		{
			return OperationTools.Compare(base.agent.velocity.magnitude, this.value.value, this.checkType, this.differenceThreshold);
		}

		// Token: 0x04000281 RID: 641
		public CompareMethod checkType;

		// Token: 0x04000282 RID: 642
		public BBParameter<float> value;

		// Token: 0x04000283 RID: 643
		[SliderField(0f, 0.1f)]
		public float differenceThreshold = 0.05f;
	}
}
