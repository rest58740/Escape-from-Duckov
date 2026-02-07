using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins
{
	// Token: 0x02000026 RID: 38
	public class ColorPlugin : ABSTweenPlugin<Color, Color, ColorOptions>
	{
		// Token: 0x060001E5 RID: 485 RVA: 0x00008DCD File Offset: 0x00006FCD
		public override void Reset(TweenerCore<Color, Color, ColorOptions> t)
		{
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000A764 File Offset: 0x00008964
		public override void SetFrom(TweenerCore<Color, Color, ColorOptions> t, bool isRelative)
		{
			Color endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue + endValue) : endValue);
			Color pNewValue = t.endValue;
			if (!t.plugOptions.alphaOnly)
			{
				pNewValue = t.startValue;
			}
			else
			{
				pNewValue.a = t.startValue.a;
			}
			t.setter(pNewValue);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000A7DC File Offset: 0x000089DC
		public override void SetFrom(TweenerCore<Color, Color, ColorOptions> t, Color fromValue, bool setImmediately, bool isRelative)
		{
			if (isRelative)
			{
				Color b = t.getter();
				t.endValue += b;
				fromValue += b;
			}
			t.startValue = fromValue;
			if (setImmediately)
			{
				Color pNewValue = fromValue;
				if (t.plugOptions.alphaOnly)
				{
					pNewValue = t.getter();
					pNewValue.a = fromValue.a;
				}
				t.setter(pNewValue);
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00008F27 File Offset: 0x00007127
		public override Color ConvertToStartValue(TweenerCore<Color, Color, ColorOptions> t, Color value)
		{
			return value;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000A852 File Offset: 0x00008A52
		public override void SetRelativeEndValue(TweenerCore<Color, Color, ColorOptions> t)
		{
			t.endValue += t.startValue;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000A86B File Offset: 0x00008A6B
		public override void SetChangeValue(TweenerCore<Color, Color, ColorOptions> t)
		{
			t.changeValue = t.endValue - t.startValue;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000092A9 File Offset: 0x000074A9
		public override float GetSpeedBasedDuration(ColorOptions options, float unitsXSecond, Color changeValue)
		{
			return 1f / unitsXSecond;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000A884 File Offset: 0x00008A84
		public override void EvaluateAndApply(ColorOptions options, Tween t, bool isRelative, DOGetter<Color> getter, DOSetter<Color> setter, float elapsed, Color startValue, Color changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (t.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (float)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (float)((t.loopType == LoopType.Incremental) ? t.loops : 1) * (float)(t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops);
			}
			float num = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
			if (!options.alphaOnly)
			{
				startValue.r += changeValue.r * num;
				startValue.g += changeValue.g * num;
				startValue.b += changeValue.b * num;
				startValue.a += changeValue.a * num;
				setter(startValue);
				return;
			}
			Color pNewValue = getter();
			pNewValue.a = startValue.a + changeValue.a * num;
			setter(pNewValue);
		}
	}
}
