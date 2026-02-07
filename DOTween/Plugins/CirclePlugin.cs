using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using UnityEngine;

namespace DG.Tweening.Plugins
{
	// Token: 0x0200001F RID: 31
	public class CirclePlugin : ABSTweenPlugin<Vector2, Vector2, CircleOptions>
	{
		// Token: 0x060001A0 RID: 416 RVA: 0x00008DCD File Offset: 0x00006FCD
		public override void Reset(TweenerCore<Vector2, Vector2, CircleOptions> t)
		{
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00008DD0 File Offset: 0x00006FD0
		public override void SetFrom(TweenerCore<Vector2, Vector2, CircleOptions> t, bool isRelative)
		{
			if (!t.plugOptions.initialized)
			{
				t.startValue = t.getter();
				t.plugOptions.Initialize(t.startValue, t.endValue);
			}
			float endValueDegrees = t.plugOptions.endValueDegrees;
			t.plugOptions.endValueDegrees = t.plugOptions.startValueDegrees;
			t.plugOptions.startValueDegrees = (isRelative ? (t.plugOptions.endValueDegrees + endValueDegrees) : endValueDegrees);
			t.startValue = this.GetPositionOnCircle(t.plugOptions, t.plugOptions.startValueDegrees);
			t.setter(t.startValue);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00008E80 File Offset: 0x00007080
		public override void SetFrom(TweenerCore<Vector2, Vector2, CircleOptions> t, Vector2 fromValue, bool setImmediately, bool isRelative)
		{
			if (!t.plugOptions.initialized)
			{
				t.startValue = t.getter();
				t.plugOptions.Initialize(t.startValue, t.endValue);
			}
			float num = fromValue.x;
			if (isRelative)
			{
				float startValueDegrees = t.plugOptions.startValueDegrees;
				t.plugOptions.endValueDegrees = t.plugOptions.endValueDegrees + startValueDegrees;
				num += startValueDegrees;
			}
			t.plugOptions.startValueDegrees = num;
			t.startValue = this.GetPositionOnCircle(t.plugOptions, num);
			if (setImmediately)
			{
				t.setter(t.startValue);
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00008F20 File Offset: 0x00007120
		public static ABSTweenPlugin<Vector2, Vector2, CircleOptions> Get()
		{
			return PluginsManager.GetCustomPlugin<CirclePlugin, Vector2, Vector2, CircleOptions>();
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00008F27 File Offset: 0x00007127
		public override Vector2 ConvertToStartValue(TweenerCore<Vector2, Vector2, CircleOptions> t, Vector2 value)
		{
			return value;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00008F2A File Offset: 0x0000712A
		public override void SetRelativeEndValue(TweenerCore<Vector2, Vector2, CircleOptions> t)
		{
			if (!t.plugOptions.initialized)
			{
				t.plugOptions.Initialize(t.startValue, t.endValue);
			}
			t.plugOptions.endValueDegrees = t.plugOptions.endValueDegrees + t.plugOptions.startValueDegrees;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00008F6C File Offset: 0x0000716C
		public override void SetChangeValue(TweenerCore<Vector2, Vector2, CircleOptions> t)
		{
			if (!t.plugOptions.initialized)
			{
				t.plugOptions.Initialize(t.startValue, t.endValue);
			}
			t.changeValue = new Vector2(t.plugOptions.endValueDegrees - t.plugOptions.startValueDegrees, 0f);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00008FC4 File Offset: 0x000071C4
		public override float GetSpeedBasedDuration(CircleOptions options, float unitsXSecond, Vector2 changeValue)
		{
			return changeValue.x / unitsXSecond;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00008FD0 File Offset: 0x000071D0
		public override void EvaluateAndApply(CircleOptions options, Tween t, bool isRelative, DOGetter<Vector2> getter, DOSetter<Vector2> setter, float elapsed, Vector2 startValue, Vector2 changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			float num = options.startValueDegrees;
			if (t.loopType == LoopType.Incremental)
			{
				num += changeValue.x * (float)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				num += changeValue.x * (float)((t.loopType == LoopType.Incremental) ? t.loops : 1) * (float)(t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops);
			}
			float num2 = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
			setter(this.GetPositionOnCircle(options, num + changeValue.x * num2));
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x000090AC File Offset: 0x000072AC
		public Vector2 GetPositionOnCircle(CircleOptions options, float degrees)
		{
			Vector2 pointOnCircle = DOTweenUtils.GetPointOnCircle(options.center, options.radius, degrees);
			if (options.snapping)
			{
				pointOnCircle.x = Mathf.Round(pointOnCircle.x);
				pointOnCircle.y = Mathf.Round(pointOnCircle.y);
			}
			return pointOnCircle;
		}
	}
}
