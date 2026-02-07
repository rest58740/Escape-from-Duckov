using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000020 RID: 32
	[Name("Target Within Distance 2D", 0)]
	[Category("GameObject")]
	public class CheckDistanceToGameObject2D : ConditionTask<Transform>
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003C4C File Offset: 0x00001E4C
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

		// Token: 0x06000076 RID: 118 RVA: 0x00003CAC File Offset: 0x00001EAC
		protected override bool OnCheck()
		{
			return OperationTools.Compare(Vector2.Distance(base.agent.position, this.checkTarget.value.transform.position), this.distance.value, this.checkType, this.floatingPoint);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003D04 File Offset: 0x00001F04
		public override void OnDrawGizmosSelected()
		{
			if (base.agent != null)
			{
				Gizmos.DrawWireSphere(base.agent.position, this.distance.value);
			}
		}

		// Token: 0x0400005D RID: 93
		[RequiredField]
		public BBParameter<GameObject> checkTarget;

		// Token: 0x0400005E RID: 94
		public CompareMethod checkType = CompareMethod.LessThan;

		// Token: 0x0400005F RID: 95
		public BBParameter<float> distance = 10f;

		// Token: 0x04000060 RID: 96
		[SliderField(0f, 0.1f)]
		public float floatingPoint = 0.05f;
	}
}
