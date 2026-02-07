using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000015 RID: 21
	[Category("✫ Blackboard")]
	public class CheckVectorDistance : ConditionTask
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002948 File Offset: 0x00000B48
		protected override string info
		{
			get
			{
				return string.Format("Distance ({0}, {1}) {2} {3}", new object[]
				{
					this.vectorA,
					this.vectorB,
					OperationTools.GetCompareString(this.comparison),
					this.distance
				});
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002983 File Offset: 0x00000B83
		protected override bool OnCheck()
		{
			return OperationTools.Compare(Vector3.Distance(this.vectorA.value, this.vectorB.value), this.distance.value, this.comparison, 0f);
		}

		// Token: 0x0400002D RID: 45
		[BlackboardOnly]
		public BBParameter<Vector3> vectorA;

		// Token: 0x0400002E RID: 46
		[BlackboardOnly]
		public BBParameter<Vector3> vectorB;

		// Token: 0x0400002F RID: 47
		public CompareMethod comparison;

		// Token: 0x04000030 RID: 48
		public BBParameter<float> distance;
	}
}
