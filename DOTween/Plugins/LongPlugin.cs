using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Plugins
{
	// Token: 0x02000022 RID: 34
	public class LongPlugin : ABSTweenPlugin<long, long, NoOptions>
	{
		// Token: 0x060001BD RID: 445 RVA: 0x00008DCD File Offset: 0x00006FCD
		public override void Reset(TweenerCore<long, long, NoOptions> t)
		{
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000968C File Offset: 0x0000788C
		public override void SetFrom(TweenerCore<long, long, NoOptions> t, bool isRelative)
		{
			long endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue + endValue) : endValue);
			t.setter(t.startValue);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000096D8 File Offset: 0x000078D8
		public override void SetFrom(TweenerCore<long, long, NoOptions> t, long fromValue, bool setImmediately, bool isRelative)
		{
			if (isRelative)
			{
				long num = t.getter();
				t.endValue += num;
				fromValue += num;
			}
			t.startValue = fromValue;
			if (setImmediately)
			{
				t.setter(fromValue);
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008F27 File Offset: 0x00007127
		public override long ConvertToStartValue(TweenerCore<long, long, NoOptions> t, long value)
		{
			return value;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000971E File Offset: 0x0000791E
		public override void SetRelativeEndValue(TweenerCore<long, long, NoOptions> t)
		{
			t.endValue += t.startValue;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00009733 File Offset: 0x00007933
		public override void SetChangeValue(TweenerCore<long, long, NoOptions> t)
		{
			t.changeValue = t.endValue - t.startValue;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00009748 File Offset: 0x00007948
		public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, long changeValue)
		{
			float num = (float)changeValue / unitsXSecond;
			if (num < 0f)
			{
				num = -num;
			}
			return num;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00009768 File Offset: 0x00007968
		public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<long> getter, DOSetter<long> setter, float elapsed, long startValue, long changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (t.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (long)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (long)((t.loopType == LoopType.Incremental) ? t.loops : 1) * (long)(t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops);
			}
			setter((long)Math.Round((double)((float)startValue + (float)changeValue * EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod))));
		}
	}
}
