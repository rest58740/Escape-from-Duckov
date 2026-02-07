using System;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Core
{
	// Token: 0x02000018 RID: 24
	[Serializable]
	public struct VectorCurve
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002FE8 File Offset: 0x000011E8
		public static VectorCurve Linear(float timeStart, float timeEnd, float valueStart, float valueEnd)
		{
			return new VectorCurve
			{
				x = AnimationCurve.Linear(timeStart, timeEnd, valueStart, valueEnd),
				y = AnimationCurve.Linear(timeStart, timeEnd, valueStart, valueEnd),
				z = AnimationCurve.Linear(timeStart, timeEnd, valueStart, valueEnd)
			};
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003030 File Offset: 0x00001230
		public static VectorCurve Constant(float timeStart, float timeEnd, float value)
		{
			return new VectorCurve
			{
				x = AnimationCurve.Constant(timeStart, timeEnd, value),
				y = AnimationCurve.Constant(timeStart, timeEnd, value),
				z = AnimationCurve.Constant(timeStart, timeEnd, value)
			};
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003074 File Offset: 0x00001274
		public float GetCurveLength()
		{
			float num = -1f;
			float curveLength = KCurves.GetCurveLength(this.x);
			num = ((curveLength > num) ? curveLength : num);
			curveLength = KCurves.GetCurveLength(this.y);
			num = ((curveLength > num) ? curveLength : num);
			curveLength = KCurves.GetCurveLength(this.z);
			return (curveLength > num) ? curveLength : num;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000030C7 File Offset: 0x000012C7
		public Vector3 GetValue(float time)
		{
			return new Vector3(this.x.Evaluate(time), this.y.Evaluate(time), this.z.Evaluate(time));
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000030F2 File Offset: 0x000012F2
		public bool IsValid()
		{
			return this.x != null && this.y != null && this.z != null;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000310F File Offset: 0x0000130F
		public VectorCurve(Keyframe[] keyFrame)
		{
			this.x = new AnimationCurve(keyFrame);
			this.y = new AnimationCurve(keyFrame);
			this.z = new AnimationCurve(keyFrame);
		}

		// Token: 0x04000037 RID: 55
		public AnimationCurve x;

		// Token: 0x04000038 RID: 56
		public AnimationCurve y;

		// Token: 0x04000039 RID: 57
		public AnimationCurve z;
	}
}
