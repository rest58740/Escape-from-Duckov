using System;
using DG.Tweening.Core.Easing;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x0200000C RID: 12
	public class EaseFactory
	{
		// Token: 0x06000070 RID: 112 RVA: 0x000030C8 File Offset: 0x000012C8
		public static EaseFunction StopMotion(int motionFps, Ease? ease = null)
		{
			EaseFunction customEase = EaseManager.ToEaseFunction((ease == null) ? DOTween.defaultEaseType : ease.Value);
			return EaseFactory.StopMotion(motionFps, customEase);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000030F9 File Offset: 0x000012F9
		public static EaseFunction StopMotion(int motionFps, AnimationCurve animCurve)
		{
			return EaseFactory.StopMotion(motionFps, new EaseFunction(new EaseCurve(animCurve).Evaluate));
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003112 File Offset: 0x00001312
		public static EaseFunction StopMotion(int motionFps, EaseFunction customEase)
		{
			float motionDelay = 1f / (float)motionFps;
			return delegate(float time, float duration, float overshootOrAmplitude, float period)
			{
				float time2 = (time < duration) ? (time - time % motionDelay) : time;
				return customEase(time2, duration, overshootOrAmplitude, period);
			};
		}
	}
}
