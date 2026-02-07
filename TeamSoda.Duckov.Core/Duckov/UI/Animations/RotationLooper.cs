using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003F9 RID: 1017
	public class RotationLooper : LooperElement
	{
		// Token: 0x0600250C RID: 9484 RVA: 0x00081A18 File Offset: 0x0007FC18
		protected override void OnTick(LooperClock clock, float t)
		{
			if (base.transform == null)
			{
				return;
			}
			Vector3 vector = Vector3.Lerp(this.eulerRotationA, this.eulerRotationB, this.curve.Evaluate(t));
			base.transform.localRotation = Quaternion.Euler(vector);
		}

		// Token: 0x04001922 RID: 6434
		[SerializeField]
		private Vector3 eulerRotationA;

		// Token: 0x04001923 RID: 6435
		[SerializeField]
		private Vector3 eulerRotationB;

		// Token: 0x04001924 RID: 6436
		[SerializeField]
		private AnimationCurve curve;
	}
}
