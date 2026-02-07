using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x0200000A RID: 10
	public static class DOVirtual
	{
		// Token: 0x06000067 RID: 103 RVA: 0x00002ED4 File Offset: 0x000010D4
		public static Tweener Float(float from, float to, float duration, TweenCallback<float> onVirtualUpdate)
		{
			return DOTween.To(() => from, delegate(float x)
			{
				from = x;
			}, to, duration).OnUpdate(delegate
			{
				onVirtualUpdate(from);
			});
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002F28 File Offset: 0x00001128
		public static Tweener Int(int from, int to, float duration, TweenCallback<int> onVirtualUpdate)
		{
			return DOTween.To(() => from, delegate(int x)
			{
				from = x;
			}, to, duration).OnUpdate(delegate
			{
				onVirtualUpdate(from);
			});
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002F7C File Offset: 0x0000117C
		public static Tweener Vector3(Vector3 from, Vector3 to, float duration, TweenCallback<Vector3> onVirtualUpdate)
		{
			return DOTween.To(() => from, delegate(Vector3 x)
			{
				from = x;
			}, to, duration).OnUpdate(delegate
			{
				onVirtualUpdate(from);
			});
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002FD0 File Offset: 0x000011D0
		public static Tweener Color(Color from, Color to, float duration, TweenCallback<Color> onVirtualUpdate)
		{
			return DOTween.To(() => from, delegate(Color x)
			{
				from = x;
			}, to, duration).OnUpdate(delegate
			{
				onVirtualUpdate(from);
			});
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003021 File Offset: 0x00001221
		public static float EasedValue(float from, float to, float lifetimePercentage, Ease easeType)
		{
			return from + (to - from) * EaseManager.Evaluate(easeType, null, lifetimePercentage, 1f, DOTween.defaultEaseOvershootOrAmplitude, DOTween.defaultEasePeriod);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003040 File Offset: 0x00001240
		public static float EasedValue(float from, float to, float lifetimePercentage, Ease easeType, float overshoot)
		{
			return from + (to - from) * EaseManager.Evaluate(easeType, null, lifetimePercentage, 1f, overshoot, DOTween.defaultEasePeriod);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000305C File Offset: 0x0000125C
		public static float EasedValue(float from, float to, float lifetimePercentage, Ease easeType, float amplitude, float period)
		{
			return from + (to - from) * EaseManager.Evaluate(easeType, null, lifetimePercentage, 1f, amplitude, period);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003075 File Offset: 0x00001275
		public static float EasedValue(float from, float to, float lifetimePercentage, AnimationCurve easeCurve)
		{
			return from + (to - from) * EaseManager.Evaluate(Ease.INTERNAL_Custom, new EaseFunction(new EaseCurve(easeCurve).Evaluate), lifetimePercentage, 1f, DOTween.defaultEaseOvershootOrAmplitude, DOTween.defaultEasePeriod);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000030A5 File Offset: 0x000012A5
		public static Tween DelayedCall(float delay, TweenCallback callback, bool ignoreTimeScale = true)
		{
			return DOTween.Sequence().AppendInterval(delay).OnStepComplete(callback).SetUpdate(UpdateType.Normal, ignoreTimeScale).SetAutoKill(true);
		}
	}
}
