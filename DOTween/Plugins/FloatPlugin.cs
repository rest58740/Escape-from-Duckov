using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Plugins
{
	// Token: 0x02000030 RID: 48
	public class FloatPlugin : ABSTweenPlugin<float, float, FloatOptions>
	{
		// Token: 0x0600023D RID: 573 RVA: 0x00008DCD File Offset: 0x00006FCD
		public override void Reset(TweenerCore<float, float, FloatOptions> t)
		{
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000D100 File Offset: 0x0000B300
		public override void SetFrom(TweenerCore<float, float, FloatOptions> t, bool isRelative)
		{
			float endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue + endValue) : endValue);
			t.setter((!t.plugOptions.snapping) ? t.startValue : ((float)Math.Round((double)t.startValue)));
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000D168 File Offset: 0x0000B368
		public override void SetFrom(TweenerCore<float, float, FloatOptions> t, float fromValue, bool setImmediately, bool isRelative)
		{
			if (isRelative)
			{
				float num = t.getter();
				t.endValue += num;
				fromValue += num;
			}
			t.startValue = fromValue;
			if (setImmediately)
			{
				t.setter((!t.plugOptions.snapping) ? fromValue : ((float)Math.Round((double)fromValue)));
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00008F27 File Offset: 0x00007127
		public override float ConvertToStartValue(TweenerCore<float, float, FloatOptions> t, float value)
		{
			return value;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000D1C5 File Offset: 0x0000B3C5
		public override void SetRelativeEndValue(TweenerCore<float, float, FloatOptions> t)
		{
			t.endValue += t.startValue;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000D1DA File Offset: 0x0000B3DA
		public override void SetChangeValue(TweenerCore<float, float, FloatOptions> t)
		{
			t.changeValue = t.endValue - t.startValue;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000D1F0 File Offset: 0x0000B3F0
		public override float GetSpeedBasedDuration(FloatOptions options, float unitsXSecond, float changeValue)
		{
			float num = changeValue / unitsXSecond;
			if (num < 0f)
			{
				num = -num;
			}
			return num;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000D210 File Offset: 0x0000B410
		public override void EvaluateAndApply(FloatOptions options, Tween t, bool isRelative, DOGetter<float> getter, DOSetter<float> setter, float elapsed, float startValue, float changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (t.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (float)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (float)((t.loopType == LoopType.Incremental) ? t.loops : 1) * (float)(t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops);
			}
			setter((!options.snapping) ? (startValue + changeValue * EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod)) : ((float)Math.Round((double)(startValue + changeValue * EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod)))));
		}
	}
}
