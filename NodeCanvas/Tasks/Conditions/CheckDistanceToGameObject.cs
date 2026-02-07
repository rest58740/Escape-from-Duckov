using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200001F RID: 31
	[Name("Target Within Distance", 0)]
	[Category("GameObject")]
	public class CheckDistanceToGameObject : ConditionTask<Transform>
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00003B48 File Offset: 0x00001D48
		protected override string info
		{
			get
			{
				string[] array = new string[5];
				array[0] = "Distance";
				array[1] = OperationTools.GetCompareString(this.checkType);
				int num = 2;
				BBParameter<float> bbparameter = this.distance;
				array[num] = ((bbparameter != null) ? bbparameter.ToString() : null);
				array[3] = " to ";
				int num2 = 4;
				BBParameter<GameObject> bbparameter2 = this.checkTarget;
				array[num2] = ((bbparameter2 != null) ? bbparameter2.ToString() : null);
				return string.Concat(array);
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003BA8 File Offset: 0x00001DA8
		protected override bool OnCheck()
		{
			return OperationTools.Compare(Vector3.Distance(base.agent.position, this.checkTarget.value.transform.position), this.distance.value, this.checkType, this.floatingPoint);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003BF6 File Offset: 0x00001DF6
		public override void OnDrawGizmosSelected()
		{
			if (base.agent != null)
			{
				Gizmos.DrawWireSphere(base.agent.position, this.distance.value);
			}
		}

		// Token: 0x04000059 RID: 89
		[RequiredField]
		public BBParameter<GameObject> checkTarget;

		// Token: 0x0400005A RID: 90
		public CompareMethod checkType = CompareMethod.LessThan;

		// Token: 0x0400005B RID: 91
		public BBParameter<float> distance = 10f;

		// Token: 0x0400005C RID: 92
		[SliderField(0f, 0.1f)]
		public float floatingPoint = 0.05f;
	}
}
