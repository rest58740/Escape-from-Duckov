using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200000F RID: 15
	[Category("✫ Blackboard")]
	public class CheckFloat : ConditionTask
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002731 File Offset: 0x00000931
		protected override string info
		{
			get
			{
				BBParameter<float> bbparameter = this.valueA;
				string text = (bbparameter != null) ? bbparameter.ToString() : null;
				string compareString = OperationTools.GetCompareString(this.checkType);
				BBParameter<float> bbparameter2 = this.valueB;
				return text + compareString + ((bbparameter2 != null) ? bbparameter2.ToString() : null);
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002767 File Offset: 0x00000967
		protected override bool OnCheck()
		{
			return OperationTools.Compare(this.valueA.value, this.valueB.value, this.checkType, this.differenceThreshold);
		}

		// Token: 0x0400001F RID: 31
		[BlackboardOnly]
		public BBParameter<float> valueA;

		// Token: 0x04000020 RID: 32
		public CompareMethod checkType;

		// Token: 0x04000021 RID: 33
		public BBParameter<float> valueB;

		// Token: 0x04000022 RID: 34
		[SliderField(0f, 0.1f)]
		public float differenceThreshold = 0.05f;
	}
}
