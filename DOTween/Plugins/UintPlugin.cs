using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Plugins
{
	// Token: 0x0200002B RID: 43
	public class UintPlugin : ABSTweenPlugin<uint, uint, UintOptions>
	{
		// Token: 0x06000213 RID: 531 RVA: 0x00008DCD File Offset: 0x00006FCD
		public override void Reset(TweenerCore<uint, uint, UintOptions> t)
		{
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000BBD0 File Offset: 0x00009DD0
		public override void SetFrom(TweenerCore<uint, uint, UintOptions> t, bool isRelative)
		{
			uint endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue + endValue) : endValue);
			t.setter(t.startValue);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000BC1C File Offset: 0x00009E1C
		public override void SetFrom(TweenerCore<uint, uint, UintOptions> t, uint fromValue, bool setImmediately, bool isRelative)
		{
			if (isRelative)
			{
				uint num = t.getter();
				t.endValue += num;
				fromValue += num;
			}
			t.startValue = fromValue;
			if (setImmediately)
			{
				t.setter(fromValue);
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00008F27 File Offset: 0x00007127
		public override uint ConvertToStartValue(TweenerCore<uint, uint, UintOptions> t, uint value)
		{
			return value;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000BC62 File Offset: 0x00009E62
		public override void SetRelativeEndValue(TweenerCore<uint, uint, UintOptions> t)
		{
			t.endValue += t.startValue;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000BC78 File Offset: 0x00009E78
		public override void SetChangeValue(TweenerCore<uint, uint, UintOptions> t)
		{
			t.plugOptions.isNegativeChangeValue = (t.endValue < t.startValue);
			t.changeValue = (t.plugOptions.isNegativeChangeValue ? (t.startValue - t.endValue) : (t.endValue - t.startValue));
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000BCD0 File Offset: 0x00009ED0
		public override float GetSpeedBasedDuration(UintOptions options, float unitsXSecond, uint changeValue)
		{
			float num = changeValue / unitsXSecond;
			if (num < 0f)
			{
				num = -num;
			}
			return num;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000BCF0 File Offset: 0x00009EF0
		public override void EvaluateAndApply(UintOptions options, Tween t, bool isRelative, DOGetter<uint> getter, DOSetter<uint> setter, float elapsed, uint startValue, uint changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			uint num;
			if (t.loopType == LoopType.Incremental)
			{
				num = (uint)((ulong)changeValue * (ulong)((long)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops)));
				if (options.isNegativeChangeValue)
				{
					startValue -= num;
				}
				else
				{
					startValue += num;
				}
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				num = (uint)((ulong)changeValue * (ulong)((long)((t.loopType == LoopType.Incremental) ? t.loops : 1)) * (ulong)((long)(t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops)));
				if (options.isNegativeChangeValue)
				{
					startValue -= num;
				}
				else
				{
					startValue += num;
				}
			}
			num = (uint)Math.Round((double)(changeValue * EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod)));
			if (options.isNegativeChangeValue)
			{
				setter(startValue - num);
				return;
			}
			setter(startValue + num);
		}
	}
}
