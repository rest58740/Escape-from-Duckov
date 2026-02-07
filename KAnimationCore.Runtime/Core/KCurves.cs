using System;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Core
{
	// Token: 0x0200001B RID: 27
	public class KCurves
	{
		// Token: 0x0600003E RID: 62 RVA: 0x00003160 File Offset: 0x00001360
		public static float GetCurveLength(AnimationCurve curve)
		{
			float result = 0f;
			if (curve != null)
			{
				result = curve[curve.length - 1].time;
			}
			return result;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000318E File Offset: 0x0000138E
		public static float EaseSine(float a, float b, float alpha)
		{
			return Mathf.Lerp(a, b, -(Mathf.Cos(3.1415927f * alpha) - 1f) / 2f);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000031B0 File Offset: 0x000013B0
		public static float EaseCubic(float a, float b, float alpha)
		{
			alpha = (((double)alpha < 0.5) ? (4f * alpha * alpha * alpha) : (1f - Mathf.Pow(-2f * alpha + 2f, 3f) / 2f));
			return Mathf.Lerp(a, b, alpha);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003204 File Offset: 0x00001404
		public static float EaseCurve(float a, float b, float alpha, AnimationCurve curve)
		{
			alpha = ((curve != null) ? curve.Evaluate(alpha) : alpha);
			return Mathf.Lerp(a, b, alpha);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003220 File Offset: 0x00001420
		public static float Ease(float a, float b, float alpha, EaseMode ease)
		{
			alpha = Mathf.Clamp01(alpha);
			if (ease.easeFunc == EEaseFunc.Sine)
			{
				return KCurves.EaseSine(a, b, alpha);
			}
			if (ease.easeFunc == EEaseFunc.Cubic)
			{
				return KCurves.EaseCubic(a, b, alpha);
			}
			if (ease.easeFunc == EEaseFunc.Custom)
			{
				return KCurves.EaseCurve(a, b, alpha, ease.curve);
			}
			return Mathf.Lerp(a, b, alpha);
		}
	}
}
