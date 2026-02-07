using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Plugins
{
	// Token: 0x02000021 RID: 33
	public class DoublePlugin : ABSTweenPlugin<double, double, NoOptions>
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x00008DCD File Offset: 0x00006FCD
		public override void Reset(TweenerCore<double, double, NoOptions> t)
		{
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000094E8 File Offset: 0x000076E8
		public override void SetFrom(TweenerCore<double, double, NoOptions> t, bool isRelative)
		{
			double endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue + endValue) : endValue);
			t.setter(t.startValue);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00009534 File Offset: 0x00007734
		public override void SetFrom(TweenerCore<double, double, NoOptions> t, double fromValue, bool setImmediately, bool isRelative)
		{
			if (isRelative)
			{
				double num = t.getter();
				t.endValue += num;
				fromValue += num;
			}
			t.startValue = fromValue;
			if (setImmediately)
			{
				t.setter(fromValue);
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00008F27 File Offset: 0x00007127
		public override double ConvertToStartValue(TweenerCore<double, double, NoOptions> t, double value)
		{
			return value;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000957A File Offset: 0x0000777A
		public override void SetRelativeEndValue(TweenerCore<double, double, NoOptions> t)
		{
			t.endValue += t.startValue;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000958F File Offset: 0x0000778F
		public override void SetChangeValue(TweenerCore<double, double, NoOptions> t)
		{
			t.changeValue = t.endValue - t.startValue;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000095A4 File Offset: 0x000077A4
		public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, double changeValue)
		{
			float num = (float)changeValue / unitsXSecond;
			if (num < 0f)
			{
				num = -num;
			}
			return num;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000095C4 File Offset: 0x000077C4
		public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<double> getter, DOSetter<double> setter, float elapsed, double startValue, double changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (t.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (double)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (double)((t.loopType == LoopType.Incremental) ? t.loops : 1) * (double)(t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops);
			}
			setter(startValue + changeValue * (double)EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod));
		}
	}
}
