using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public class Decayer
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00002DC6 File Offset: 0x00000FC6
		public void SetT(float v)
		{
			this.t = v;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002DD0 File Offset: 0x00000FD0
		public void Update()
		{
			this.t = Mathf.Max(0f, this.t - this.decaySpeed * Time.deltaTime);
			float num = (this.curve.keys.Length != 0) ? this.curve.Evaluate(1f - this.t) : this.t;
			this.value = num * this.magnitude;
			this.valueInv = (1f - num) * this.magnitude;
		}

		// Token: 0x04000034 RID: 52
		public float decaySpeed;

		// Token: 0x04000035 RID: 53
		public float magnitude;

		// Token: 0x04000036 RID: 54
		public AnimationCurve curve;

		// Token: 0x04000037 RID: 55
		[NonSerialized]
		public float value;

		// Token: 0x04000038 RID: 56
		[NonSerialized]
		public float valueInv;

		// Token: 0x04000039 RID: 57
		[NonSerialized]
		public float t;
	}
}
