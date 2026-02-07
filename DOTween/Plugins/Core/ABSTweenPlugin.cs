using System;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;

namespace DG.Tweening.Plugins.Core
{
	// Token: 0x02000042 RID: 66
	public abstract class ABSTweenPlugin<T1, T2, TPlugOptions> : ITweenPlugin where TPlugOptions : struct, IPlugOptions
	{
		// Token: 0x06000262 RID: 610
		public abstract void Reset(TweenerCore<T1, T2, TPlugOptions> t);

		// Token: 0x06000263 RID: 611
		public abstract void SetFrom(TweenerCore<T1, T2, TPlugOptions> t, bool isRelative);

		// Token: 0x06000264 RID: 612
		public abstract void SetFrom(TweenerCore<T1, T2, TPlugOptions> t, T2 fromValue, bool setImmediately, bool isRelative);

		// Token: 0x06000265 RID: 613
		public abstract T2 ConvertToStartValue(TweenerCore<T1, T2, TPlugOptions> t, T1 value);

		// Token: 0x06000266 RID: 614
		public abstract void SetRelativeEndValue(TweenerCore<T1, T2, TPlugOptions> t);

		// Token: 0x06000267 RID: 615
		public abstract void SetChangeValue(TweenerCore<T1, T2, TPlugOptions> t);

		// Token: 0x06000268 RID: 616
		public abstract float GetSpeedBasedDuration(TPlugOptions options, float unitsXSecond, T2 changeValue);

		// Token: 0x06000269 RID: 617
		public abstract void EvaluateAndApply(TPlugOptions options, Tween t, bool isRelative, DOGetter<T1> getter, DOSetter<T1> setter, float elapsed, T2 startValue, T2 changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice);
	}
}
