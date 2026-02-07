using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000083 RID: 131
	[Name("Set Integer Random", 0)]
	[Category("✫ Blackboard")]
	[Description("Set a blackboard integer variable at random between min and max value")]
	public class SetIntRandom : ActionTask
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00009718 File Offset: 0x00007918
		protected override string info
		{
			get
			{
				string[] array = new string[7];
				array[0] = "Set ";
				int num = 1;
				BBParameter<int> bbparameter = this.intVariable;
				array[num] = ((bbparameter != null) ? bbparameter.ToString() : null);
				array[2] = " Random(";
				int num2 = 3;
				BBParameter<int> bbparameter2 = this.minValue;
				array[num2] = ((bbparameter2 != null) ? bbparameter2.ToString() : null);
				array[4] = ", ";
				int num3 = 5;
				BBParameter<int> bbparameter3 = this.maxValue;
				array[num3] = ((bbparameter3 != null) ? bbparameter3.ToString() : null);
				array[6] = ")";
				return string.Concat(array);
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000978F File Offset: 0x0000798F
		protected override void OnExecute()
		{
			this.intVariable.value = Random.Range(this.minValue.value, this.maxValue.value + 1);
			base.EndAction();
		}

		// Token: 0x04000182 RID: 386
		public BBParameter<int> minValue;

		// Token: 0x04000183 RID: 387
		public BBParameter<int> maxValue;

		// Token: 0x04000184 RID: 388
		[BlackboardOnly]
		public BBParameter<int> intVariable;
	}
}
