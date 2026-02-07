using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Plugins
{
	// Token: 0x02000023 RID: 35
	public class UlongPlugin : ABSTweenPlugin<ulong, ulong, NoOptions>
	{
		// Token: 0x060001C6 RID: 454 RVA: 0x00008DCD File Offset: 0x00006FCD
		public override void Reset(TweenerCore<ulong, ulong, NoOptions> t)
		{
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00009838 File Offset: 0x00007A38
		public override void SetFrom(TweenerCore<ulong, ulong, NoOptions> t, bool isRelative)
		{
			ulong endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue + endValue) : endValue);
			t.setter(t.startValue);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00009884 File Offset: 0x00007A84
		public override void SetFrom(TweenerCore<ulong, ulong, NoOptions> t, ulong fromValue, bool setImmediately, bool isRelative)
		{
			if (isRelative)
			{
				ulong num = t.getter();
				t.endValue += num;
				fromValue += num;
			}
			t.startValue = fromValue;
			if (setImmediately)
			{
				t.setter(fromValue);
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00008F27 File Offset: 0x00007127
		public override ulong ConvertToStartValue(TweenerCore<ulong, ulong, NoOptions> t, ulong value)
		{
			return value;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000098CA File Offset: 0x00007ACA
		public override void SetRelativeEndValue(TweenerCore<ulong, ulong, NoOptions> t)
		{
			t.endValue += t.startValue;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000098DF File Offset: 0x00007ADF
		public override void SetChangeValue(TweenerCore<ulong, ulong, NoOptions> t)
		{
			t.changeValue = t.endValue - t.startValue;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000098F4 File Offset: 0x00007AF4
		public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, ulong changeValue)
		{
			float num = changeValue / unitsXSecond;
			if (num < 0f)
			{
				num = -num;
			}
			return num;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00009914 File Offset: 0x00007B14
		public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<ulong> getter, DOSetter<ulong> setter, float elapsed, ulong startValue, ulong changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (t.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (ulong)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (ulong)((t.loopType == LoopType.Incremental) ? t.loops : 1) * (ulong)(t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops);
			}
			setter((ulong)(startValue + changeValue * (decimal)EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod)));
		}
	}
}
