using System;
using UnityEngine;

namespace DG.Tweening.Core.Easing
{
	// Token: 0x02000063 RID: 99
	public class EaseCurve
	{
		// Token: 0x0600032F RID: 815 RVA: 0x00012FD5 File Offset: 0x000111D5
		public EaseCurve(AnimationCurve animCurve)
		{
			this._animCurve = animCurve;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00012FE4 File Offset: 0x000111E4
		public float Evaluate(float time, float duration, float unusedOvershoot, float unusedPeriod)
		{
			float time2 = this._animCurve[this._animCurve.length - 1].time;
			float num = time / duration;
			return this._animCurve.Evaluate(num * time2);
		}

		// Token: 0x040001D6 RID: 470
		private readonly AnimationCurve _animCurve;
	}
}
