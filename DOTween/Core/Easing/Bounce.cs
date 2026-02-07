using System;

namespace DG.Tweening.Core.Easing
{
	// Token: 0x02000061 RID: 97
	public static class Bounce
	{
		// Token: 0x06000328 RID: 808 RVA: 0x00012264 File Offset: 0x00010464
		public static float EaseIn(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			return 1f - Bounce.EaseOut(duration - time, duration, -1f, -1f);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00012280 File Offset: 0x00010480
		public static float EaseOut(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			if ((time /= duration) < 0.36363637f)
			{
				return 7.5625f * time * time;
			}
			if (time < 0.72727275f)
			{
				return 7.5625f * (time -= 0.54545456f) * time + 0.75f;
			}
			if (time < 0.90909094f)
			{
				return 7.5625f * (time -= 0.8181818f) * time + 0.9375f;
			}
			return 7.5625f * (time -= 0.95454544f) * time + 0.984375f;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00012300 File Offset: 0x00010500
		public static float EaseInOut(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
		{
			if (time < duration * 0.5f)
			{
				return Bounce.EaseIn(time * 2f, duration, -1f, -1f) * 0.5f;
			}
			return Bounce.EaseOut(time * 2f - duration, duration, -1f, -1f) * 0.5f + 0.5f;
		}
	}
}
