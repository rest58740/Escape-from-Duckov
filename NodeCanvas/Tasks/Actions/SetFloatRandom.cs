using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000081 RID: 129
	[Category("✫ Blackboard")]
	[Description("Set a blackboard float variable at random between min and max value")]
	public class SetFloatRandom : ActionTask
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000248 RID: 584 RVA: 0x000095F8 File Offset: 0x000077F8
		protected override string info
		{
			get
			{
				string[] array = new string[7];
				array[0] = "Set ";
				int num = 1;
				BBParameter<float> bbparameter = this.floatVariable;
				array[num] = ((bbparameter != null) ? bbparameter.ToString() : null);
				array[2] = " Random(";
				int num2 = 3;
				BBParameter<float> bbparameter2 = this.minValue;
				array[num2] = ((bbparameter2 != null) ? bbparameter2.ToString() : null);
				array[4] = ", ";
				int num3 = 5;
				BBParameter<float> bbparameter3 = this.maxValue;
				array[num3] = ((bbparameter3 != null) ? bbparameter3.ToString() : null);
				array[6] = ")";
				return string.Concat(array);
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000966F File Offset: 0x0000786F
		protected override void OnExecute()
		{
			this.floatVariable.value = Random.Range(this.minValue.value, this.maxValue.value);
			base.EndAction();
		}

		// Token: 0x0400017C RID: 380
		public BBParameter<float> minValue;

		// Token: 0x0400017D RID: 381
		public BBParameter<float> maxValue;

		// Token: 0x0400017E RID: 382
		[BlackboardOnly]
		public BBParameter<float> floatVariable;
	}
}
