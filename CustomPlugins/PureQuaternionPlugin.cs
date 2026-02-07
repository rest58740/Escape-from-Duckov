using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.CustomPlugins
{
	// Token: 0x0200004A RID: 74
	public class PureQuaternionPlugin : ABSTweenPlugin<Quaternion, Quaternion, NoOptions>
	{
		// Token: 0x06000299 RID: 665 RVA: 0x0000F24E File Offset: 0x0000D44E
		public static PureQuaternionPlugin Plug()
		{
			if (PureQuaternionPlugin._plug == null)
			{
				PureQuaternionPlugin._plug = new PureQuaternionPlugin();
			}
			return PureQuaternionPlugin._plug;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00008DCD File Offset: 0x00006FCD
		public override void Reset(TweenerCore<Quaternion, Quaternion, NoOptions> t)
		{
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000F268 File Offset: 0x0000D468
		public override void SetFrom(TweenerCore<Quaternion, Quaternion, NoOptions> t, bool isRelative)
		{
			Quaternion endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue * endValue) : endValue);
			t.setter(t.startValue);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000F2B8 File Offset: 0x0000D4B8
		public override void SetFrom(TweenerCore<Quaternion, Quaternion, NoOptions> t, Quaternion fromValue, bool setImmediately, bool isRelative)
		{
			if (isRelative)
			{
				Quaternion lhs = t.getter();
				t.endValue = lhs * t.endValue;
				fromValue = lhs * fromValue;
			}
			t.startValue = fromValue;
			if (setImmediately)
			{
				t.setter(fromValue);
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00008F27 File Offset: 0x00007127
		public override Quaternion ConvertToStartValue(TweenerCore<Quaternion, Quaternion, NoOptions> t, Quaternion value)
		{
			return value;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000F306 File Offset: 0x0000D506
		public override void SetRelativeEndValue(TweenerCore<Quaternion, Quaternion, NoOptions> t)
		{
			t.endValue *= t.startValue;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000F31F File Offset: 0x0000D51F
		public override void SetChangeValue(TweenerCore<Quaternion, Quaternion, NoOptions> t)
		{
			t.changeValue = t.endValue;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000F330 File Offset: 0x0000D530
		public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, Quaternion changeValue)
		{
			return changeValue.eulerAngles.magnitude / unitsXSecond;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000F350 File Offset: 0x0000D550
		public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<Quaternion> getter, DOSetter<Quaternion> setter, float elapsed, Quaternion startValue, Quaternion changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			float t2 = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
			setter(Quaternion.Slerp(startValue, changeValue, t2));
		}

		// Token: 0x04000142 RID: 322
		private static PureQuaternionPlugin _plug;
	}
}
