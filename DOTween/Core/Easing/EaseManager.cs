using System;

namespace DG.Tweening.Core.Easing
{
	// Token: 0x02000062 RID: 98
	public static class EaseManager
	{
		// Token: 0x0600032B RID: 811 RVA: 0x0001235A File Offset: 0x0001055A
		public static float Evaluate(Tween t, float time, float duration, float overshootOrAmplitude, float period)
		{
			return EaseManager.Evaluate(t.easeType, t.customEase, time, duration, overshootOrAmplitude, period);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00012374 File Offset: 0x00010574
		public static float Evaluate(Ease easeType, EaseFunction customEase, float time, float duration, float overshootOrAmplitude, float period)
		{
			switch (easeType)
			{
			case Ease.Linear:
				return time / duration;
			case Ease.InSine:
				return -(float)Math.Cos((double)(time / duration * 1.5707964f)) + 1f;
			case Ease.OutSine:
				return (float)Math.Sin((double)(time / duration * 1.5707964f));
			case Ease.InOutSine:
				return -0.5f * ((float)Math.Cos((double)(3.1415927f * time / duration)) - 1f);
			case Ease.InQuad:
				return (time /= duration) * time;
			case Ease.OutQuad:
				return -(time /= duration) * (time - 2f);
			case Ease.InOutQuad:
				if ((time /= duration * 0.5f) < 1f)
				{
					return 0.5f * time * time;
				}
				return -0.5f * ((time -= 1f) * (time - 2f) - 1f);
			case Ease.InCubic:
				return (time /= duration) * time * time;
			case Ease.OutCubic:
				return (time = time / duration - 1f) * time * time + 1f;
			case Ease.InOutCubic:
				if ((time /= duration * 0.5f) < 1f)
				{
					return 0.5f * time * time * time;
				}
				return 0.5f * ((time -= 2f) * time * time + 2f);
			case Ease.InQuart:
				return (time /= duration) * time * time * time;
			case Ease.OutQuart:
				return -((time = time / duration - 1f) * time * time * time - 1f);
			case Ease.InOutQuart:
				if ((time /= duration * 0.5f) < 1f)
				{
					return 0.5f * time * time * time * time;
				}
				return -0.5f * ((time -= 2f) * time * time * time - 2f);
			case Ease.InQuint:
				return (time /= duration) * time * time * time * time;
			case Ease.OutQuint:
				return (time = time / duration - 1f) * time * time * time * time + 1f;
			case Ease.InOutQuint:
				if ((time /= duration * 0.5f) < 1f)
				{
					return 0.5f * time * time * time * time * time;
				}
				return 0.5f * ((time -= 2f) * time * time * time * time + 2f);
			case Ease.InExpo:
				if (time != 0f)
				{
					return (float)Math.Pow(2.0, (double)(10f * (time / duration - 1f)));
				}
				return 0f;
			case Ease.OutExpo:
				if (time == duration)
				{
					return 1f;
				}
				return -(float)Math.Pow(2.0, (double)(-10f * time / duration)) + 1f;
			case Ease.InOutExpo:
				if (time == 0f)
				{
					return 0f;
				}
				if (time == duration)
				{
					return 1f;
				}
				if ((time /= duration * 0.5f) < 1f)
				{
					return 0.5f * (float)Math.Pow(2.0, (double)(10f * (time - 1f)));
				}
				return 0.5f * (-(float)Math.Pow(2.0, (double)(-10f * (time -= 1f))) + 2f);
			case Ease.InCirc:
				return -((float)Math.Sqrt((double)(1f - (time /= duration) * time)) - 1f);
			case Ease.OutCirc:
				return (float)Math.Sqrt((double)(1f - (time = time / duration - 1f) * time));
			case Ease.InOutCirc:
				if ((time /= duration * 0.5f) < 1f)
				{
					return -0.5f * ((float)Math.Sqrt((double)(1f - time * time)) - 1f);
				}
				return 0.5f * ((float)Math.Sqrt((double)(1f - (time -= 2f) * time)) + 1f);
			case Ease.InElastic:
			{
				if (time == 0f)
				{
					return 0f;
				}
				if ((time /= duration) == 1f)
				{
					return 1f;
				}
				if (period == 0f)
				{
					period = duration * 0.3f;
				}
				float num;
				if (overshootOrAmplitude < 1f)
				{
					overshootOrAmplitude = 1f;
					num = period / 4f;
				}
				else
				{
					num = period / 6.2831855f * (float)Math.Asin((double)(1f / overshootOrAmplitude));
				}
				return -(overshootOrAmplitude * (float)Math.Pow(2.0, (double)(10f * (time -= 1f))) * (float)Math.Sin((double)((time * duration - num) * 6.2831855f / period)));
			}
			case Ease.OutElastic:
			{
				if (time == 0f)
				{
					return 0f;
				}
				if ((time /= duration) == 1f)
				{
					return 1f;
				}
				if (period == 0f)
				{
					period = duration * 0.3f;
				}
				float num2;
				if (overshootOrAmplitude < 1f)
				{
					overshootOrAmplitude = 1f;
					num2 = period / 4f;
				}
				else
				{
					num2 = period / 6.2831855f * (float)Math.Asin((double)(1f / overshootOrAmplitude));
				}
				return overshootOrAmplitude * (float)Math.Pow(2.0, (double)(-10f * time)) * (float)Math.Sin((double)((time * duration - num2) * 6.2831855f / period)) + 1f;
			}
			case Ease.InOutElastic:
			{
				if (time == 0f)
				{
					return 0f;
				}
				if ((time /= duration * 0.5f) == 2f)
				{
					return 1f;
				}
				if (period == 0f)
				{
					period = duration * 0.45000002f;
				}
				float num3;
				if (overshootOrAmplitude < 1f)
				{
					overshootOrAmplitude = 1f;
					num3 = period / 4f;
				}
				else
				{
					num3 = period / 6.2831855f * (float)Math.Asin((double)(1f / overshootOrAmplitude));
				}
				if (time < 1f)
				{
					return -0.5f * (overshootOrAmplitude * (float)Math.Pow(2.0, (double)(10f * (time -= 1f))) * (float)Math.Sin((double)((time * duration - num3) * 6.2831855f / period)));
				}
				return overshootOrAmplitude * (float)Math.Pow(2.0, (double)(-10f * (time -= 1f))) * (float)Math.Sin((double)((time * duration - num3) * 6.2831855f / period)) * 0.5f + 1f;
			}
			case Ease.InBack:
				return (time /= duration) * time * ((overshootOrAmplitude + 1f) * time - overshootOrAmplitude);
			case Ease.OutBack:
				return (time = time / duration - 1f) * time * ((overshootOrAmplitude + 1f) * time + overshootOrAmplitude) + 1f;
			case Ease.InOutBack:
				if ((time /= duration * 0.5f) < 1f)
				{
					return 0.5f * (time * time * (((overshootOrAmplitude *= 1.525f) + 1f) * time - overshootOrAmplitude));
				}
				return 0.5f * ((time -= 2f) * time * (((overshootOrAmplitude *= 1.525f) + 1f) * time + overshootOrAmplitude) + 2f);
			case Ease.InBounce:
				return Bounce.EaseIn(time, duration, overshootOrAmplitude, period);
			case Ease.OutBounce:
				return Bounce.EaseOut(time, duration, overshootOrAmplitude, period);
			case Ease.InOutBounce:
				return Bounce.EaseInOut(time, duration, overshootOrAmplitude, period);
			case Ease.Flash:
				return Flash.Ease(time, duration, overshootOrAmplitude, period);
			case Ease.InFlash:
				return Flash.EaseIn(time, duration, overshootOrAmplitude, period);
			case Ease.OutFlash:
				return Flash.EaseOut(time, duration, overshootOrAmplitude, period);
			case Ease.InOutFlash:
				return Flash.EaseInOut(time, duration, overshootOrAmplitude, period);
			case Ease.INTERNAL_Zero:
				return 1f;
			case Ease.INTERNAL_Custom:
				return customEase(time, duration, overshootOrAmplitude, period);
			default:
				return -(time /= duration) * (time - 2f);
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00012AA4 File Offset: 0x00010CA4
		public static EaseFunction ToEaseFunction(Ease ease)
		{
			switch (ease)
			{
			case Ease.Linear:
				return (float time, float duration, float overshootOrAmplitude, float period) => time / duration;
			case Ease.InSine:
				return (float time, float duration, float overshootOrAmplitude, float period) => -(float)Math.Cos((double)(time / duration * 1.5707964f)) + 1f;
			case Ease.OutSine:
				return (float time, float duration, float overshootOrAmplitude, float period) => (float)Math.Sin((double)(time / duration * 1.5707964f));
			case Ease.InOutSine:
				return (float time, float duration, float overshootOrAmplitude, float period) => -0.5f * ((float)Math.Cos((double)(3.1415927f * time / duration)) - 1f);
			case Ease.InQuad:
				return (float time, float duration, float overshootOrAmplitude, float period) => (time /= duration) * time;
			case Ease.OutQuad:
				return (float time, float duration, float overshootOrAmplitude, float period) => -(time /= duration) * (time - 2f);
			case Ease.InOutQuad:
				return delegate(float time, float duration, float overshootOrAmplitude, float period)
				{
					if ((time /= duration * 0.5f) < 1f)
					{
						return 0.5f * time * time;
					}
					return -0.5f * ((time -= 1f) * (time - 2f) - 1f);
				};
			case Ease.InCubic:
				return (float time, float duration, float overshootOrAmplitude, float period) => (time /= duration) * time * time;
			case Ease.OutCubic:
				return (float time, float duration, float overshootOrAmplitude, float period) => (time = time / duration - 1f) * time * time + 1f;
			case Ease.InOutCubic:
				return delegate(float time, float duration, float overshootOrAmplitude, float period)
				{
					if ((time /= duration * 0.5f) < 1f)
					{
						return 0.5f * time * time * time;
					}
					return 0.5f * ((time -= 2f) * time * time + 2f);
				};
			case Ease.InQuart:
				return (float time, float duration, float overshootOrAmplitude, float period) => (time /= duration) * time * time * time;
			case Ease.OutQuart:
				return (float time, float duration, float overshootOrAmplitude, float period) => -((time = time / duration - 1f) * time * time * time - 1f);
			case Ease.InOutQuart:
				return delegate(float time, float duration, float overshootOrAmplitude, float period)
				{
					if ((time /= duration * 0.5f) < 1f)
					{
						return 0.5f * time * time * time * time;
					}
					return -0.5f * ((time -= 2f) * time * time * time - 2f);
				};
			case Ease.InQuint:
				return (float time, float duration, float overshootOrAmplitude, float period) => (time /= duration) * time * time * time * time;
			case Ease.OutQuint:
				return (float time, float duration, float overshootOrAmplitude, float period) => (time = time / duration - 1f) * time * time * time * time + 1f;
			case Ease.InOutQuint:
				return delegate(float time, float duration, float overshootOrAmplitude, float period)
				{
					if ((time /= duration * 0.5f) < 1f)
					{
						return 0.5f * time * time * time * time * time;
					}
					return 0.5f * ((time -= 2f) * time * time * time * time + 2f);
				};
			case Ease.InExpo:
				return delegate(float time, float duration, float overshootOrAmplitude, float period)
				{
					if (time != 0f)
					{
						return (float)Math.Pow(2.0, (double)(10f * (time / duration - 1f)));
					}
					return 0f;
				};
			case Ease.OutExpo:
				return delegate(float time, float duration, float overshootOrAmplitude, float period)
				{
					if (time == duration)
					{
						return 1f;
					}
					return -(float)Math.Pow(2.0, (double)(-10f * time / duration)) + 1f;
				};
			case Ease.InOutExpo:
				return delegate(float time, float duration, float overshootOrAmplitude, float period)
				{
					if (time == 0f)
					{
						return 0f;
					}
					if (time == duration)
					{
						return 1f;
					}
					if ((time /= duration * 0.5f) < 1f)
					{
						return 0.5f * (float)Math.Pow(2.0, (double)(10f * (time - 1f)));
					}
					return 0.5f * (-(float)Math.Pow(2.0, (double)(-10f * (time -= 1f))) + 2f);
				};
			case Ease.InCirc:
				return (float time, float duration, float overshootOrAmplitude, float period) => -((float)Math.Sqrt((double)(1f - (time /= duration) * time)) - 1f);
			case Ease.OutCirc:
				return (float time, float duration, float overshootOrAmplitude, float period) => (float)Math.Sqrt((double)(1f - (time = time / duration - 1f) * time));
			case Ease.InOutCirc:
				return delegate(float time, float duration, float overshootOrAmplitude, float period)
				{
					if ((time /= duration * 0.5f) < 1f)
					{
						return -0.5f * ((float)Math.Sqrt((double)(1f - time * time)) - 1f);
					}
					return 0.5f * ((float)Math.Sqrt((double)(1f - (time -= 2f) * time)) + 1f);
				};
			case Ease.InElastic:
				return delegate(float time, float duration, float overshootOrAmplitude, float period)
				{
					if (time == 0f)
					{
						return 0f;
					}
					if ((time /= duration) == 1f)
					{
						return 1f;
					}
					if (period == 0f)
					{
						period = duration * 0.3f;
					}
					float num;
					if (overshootOrAmplitude < 1f)
					{
						overshootOrAmplitude = 1f;
						num = period / 4f;
					}
					else
					{
						num = period / 6.2831855f * (float)Math.Asin((double)(1f / overshootOrAmplitude));
					}
					return -(overshootOrAmplitude * (float)Math.Pow(2.0, (double)(10f * (time -= 1f))) * (float)Math.Sin((double)((time * duration - num) * 6.2831855f / period)));
				};
			case Ease.OutElastic:
				return delegate(float time, float duration, float overshootOrAmplitude, float period)
				{
					if (time == 0f)
					{
						return 0f;
					}
					if ((time /= duration) == 1f)
					{
						return 1f;
					}
					if (period == 0f)
					{
						period = duration * 0.3f;
					}
					float num;
					if (overshootOrAmplitude < 1f)
					{
						overshootOrAmplitude = 1f;
						num = period / 4f;
					}
					else
					{
						num = period / 6.2831855f * (float)Math.Asin((double)(1f / overshootOrAmplitude));
					}
					return overshootOrAmplitude * (float)Math.Pow(2.0, (double)(-10f * time)) * (float)Math.Sin((double)((time * duration - num) * 6.2831855f / period)) + 1f;
				};
			case Ease.InOutElastic:
				return delegate(float time, float duration, float overshootOrAmplitude, float period)
				{
					if (time == 0f)
					{
						return 0f;
					}
					if ((time /= duration * 0.5f) == 2f)
					{
						return 1f;
					}
					if (period == 0f)
					{
						period = duration * 0.45000002f;
					}
					float num;
					if (overshootOrAmplitude < 1f)
					{
						overshootOrAmplitude = 1f;
						num = period / 4f;
					}
					else
					{
						num = period / 6.2831855f * (float)Math.Asin((double)(1f / overshootOrAmplitude));
					}
					if (time < 1f)
					{
						return -0.5f * (overshootOrAmplitude * (float)Math.Pow(2.0, (double)(10f * (time -= 1f))) * (float)Math.Sin((double)((time * duration - num) * 6.2831855f / period)));
					}
					return overshootOrAmplitude * (float)Math.Pow(2.0, (double)(-10f * (time -= 1f))) * (float)Math.Sin((double)((time * duration - num) * 6.2831855f / period)) * 0.5f + 1f;
				};
			case Ease.InBack:
				return (float time, float duration, float overshootOrAmplitude, float period) => (time /= duration) * time * ((overshootOrAmplitude + 1f) * time - overshootOrAmplitude);
			case Ease.OutBack:
				return (float time, float duration, float overshootOrAmplitude, float period) => (time = time / duration - 1f) * time * ((overshootOrAmplitude + 1f) * time + overshootOrAmplitude) + 1f;
			case Ease.InOutBack:
				return delegate(float time, float duration, float overshootOrAmplitude, float period)
				{
					if ((time /= duration * 0.5f) < 1f)
					{
						return 0.5f * (time * time * (((overshootOrAmplitude *= 1.525f) + 1f) * time - overshootOrAmplitude));
					}
					return 0.5f * ((time -= 2f) * time * (((overshootOrAmplitude *= 1.525f) + 1f) * time + overshootOrAmplitude) + 2f);
				};
			case Ease.InBounce:
				return (float time, float duration, float overshootOrAmplitude, float period) => Bounce.EaseIn(time, duration, overshootOrAmplitude, period);
			case Ease.OutBounce:
				return (float time, float duration, float overshootOrAmplitude, float period) => Bounce.EaseOut(time, duration, overshootOrAmplitude, period);
			case Ease.InOutBounce:
				return (float time, float duration, float overshootOrAmplitude, float period) => Bounce.EaseInOut(time, duration, overshootOrAmplitude, period);
			case Ease.Flash:
				return (float time, float duration, float overshootOrAmplitude, float period) => Flash.Ease(time, duration, overshootOrAmplitude, period);
			case Ease.InFlash:
				return (float time, float duration, float overshootOrAmplitude, float period) => Flash.EaseIn(time, duration, overshootOrAmplitude, period);
			case Ease.OutFlash:
				return (float time, float duration, float overshootOrAmplitude, float period) => Flash.EaseOut(time, duration, overshootOrAmplitude, period);
			case Ease.InOutFlash:
				return (float time, float duration, float overshootOrAmplitude, float period) => Flash.EaseInOut(time, duration, overshootOrAmplitude, period);
			default:
				return (float time, float duration, float overshootOrAmplitude, float period) => -(time /= duration) * (time - 2f);
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00012FC9 File Offset: 0x000111C9
		internal static bool IsFlashEase(Ease ease)
		{
			return ease - Ease.Flash <= 3;
		}

		// Token: 0x040001D4 RID: 468
		private const float _PiOver2 = 1.5707964f;

		// Token: 0x040001D5 RID: 469
		private const float _TwoPi = 6.2831855f;
	}
}
