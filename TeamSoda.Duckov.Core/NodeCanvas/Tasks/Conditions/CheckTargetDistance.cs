using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000422 RID: 1058
	public class CheckTargetDistance : ConditionTask<AICharacterController>
	{
		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x0600266F RID: 9839 RVA: 0x000851B2 File Offset: 0x000833B2
		protected override string info
		{
			get
			{
				return "is target in range";
			}
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x000851BC File Offset: 0x000833BC
		protected override bool OnCheck()
		{
			if (this.useTransform && this.targetTransform.value == null)
			{
				return false;
			}
			Vector3 vector = this.useTransform ? this.targetTransform.value.position : this.targetPoint.value;
			float num;
			if (this.useShootRange)
			{
				num = base.agent.CharacterMainControl.GetAimRange() * this.shootRangeMultiplier.value;
			}
			else
			{
				num = this.distance.value;
			}
			return Vector3.Distance(base.agent.transform.position, vector) <= num;
		}

		// Token: 0x04001A2C RID: 6700
		public bool useTransform;

		// Token: 0x04001A2D RID: 6701
		[ShowIf("useTransform", 1)]
		public BBParameter<Transform> targetTransform;

		// Token: 0x04001A2E RID: 6702
		[ShowIf("useTransform", 0)]
		public BBParameter<Vector3> targetPoint;

		// Token: 0x04001A2F RID: 6703
		public bool useShootRange;

		// Token: 0x04001A30 RID: 6704
		[ShowIf("useShootRange", 1)]
		public BBParameter<float> shootRangeMultiplier = 1f;

		// Token: 0x04001A31 RID: 6705
		[ShowIf("useShootRange", 0)]
		public BBParameter<float> distance;
	}
}
