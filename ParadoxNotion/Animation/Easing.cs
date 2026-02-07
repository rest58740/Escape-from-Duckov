using System;
using UnityEngine;

namespace ParadoxNotion.Animation
{
	// Token: 0x020000C8 RID: 200
	public static class Easing
	{
		// Token: 0x0600071E RID: 1822 RVA: 0x00016853 File Offset: 0x00014A53
		public static float Ease(EaseType type, float from, float to, float t)
		{
			if (t <= 0f)
			{
				return from;
			}
			if (t >= 1f)
			{
				return to;
			}
			return Mathf.LerpUnclamped(from, to, Easing.Function(type).Invoke(t));
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0001687C File Offset: 0x00014A7C
		public static Vector3 Ease(EaseType type, Vector3 from, Vector3 to, float t)
		{
			if (t <= 0f)
			{
				return from;
			}
			if (t >= 1f)
			{
				return to;
			}
			return Vector3.LerpUnclamped(from, to, Easing.Function(type).Invoke(t));
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x000168A5 File Offset: 0x00014AA5
		public static Quaternion Ease(EaseType type, Quaternion from, Quaternion to, float t)
		{
			if (t <= 0f)
			{
				return from;
			}
			if (t >= 1f)
			{
				return to;
			}
			return Quaternion.LerpUnclamped(from, to, Easing.Function(type).Invoke(t));
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x000168CE File Offset: 0x00014ACE
		public static Color Ease(EaseType type, Color from, Color to, float t)
		{
			if (t <= 0f)
			{
				return from;
			}
			if (t >= 1f)
			{
				return to;
			}
			return Color.LerpUnclamped(from, to, Easing.Function(type).Invoke(t));
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x000168F8 File Offset: 0x00014AF8
		public static Func<float, float> Function(EaseType type)
		{
			switch (type)
			{
			case EaseType.Linear:
				return new Func<float, float>(Easing.Linear);
			case EaseType.QuadraticIn:
				return new Func<float, float>(Easing.QuadraticIn);
			case EaseType.QuadraticOut:
				return new Func<float, float>(Easing.QuadraticOut);
			case EaseType.QuadraticInOut:
				return new Func<float, float>(Easing.QuadraticInOut);
			case EaseType.QuarticIn:
				return new Func<float, float>(Easing.QuarticIn);
			case EaseType.QuarticOut:
				return new Func<float, float>(Easing.QuarticOut);
			case EaseType.QuarticInOut:
				return new Func<float, float>(Easing.QuarticInOut);
			case EaseType.QuinticIn:
				return new Func<float, float>(Easing.QuinticIn);
			case EaseType.QuinticOut:
				return new Func<float, float>(Easing.QuinticOut);
			case EaseType.QuinticInOut:
				return new Func<float, float>(Easing.QuinticInOut);
			case EaseType.CubicIn:
				return new Func<float, float>(Easing.CubicIn);
			case EaseType.CubicOut:
				return new Func<float, float>(Easing.CubicOut);
			case EaseType.CubicInOut:
				return new Func<float, float>(Easing.CubicInOut);
			case EaseType.ExponentialIn:
				return new Func<float, float>(Easing.ExponentialIn);
			case EaseType.ExponentialOut:
				return new Func<float, float>(Easing.ExponentialOut);
			case EaseType.ExponentialInOut:
				return new Func<float, float>(Easing.ExponentialInOut);
			case EaseType.CircularIn:
				return new Func<float, float>(Easing.CircularIn);
			case EaseType.CircularOut:
				return new Func<float, float>(Easing.CircularOut);
			case EaseType.CircularInOut:
				return new Func<float, float>(Easing.CircularInOut);
			case EaseType.SinusoidalIn:
				return new Func<float, float>(Easing.SinusoidalIn);
			case EaseType.SinusoidalOut:
				return new Func<float, float>(Easing.SinusoidalOut);
			case EaseType.SinusoidalInOut:
				return new Func<float, float>(Easing.SinusoidalInOut);
			case EaseType.ElasticIn:
				return new Func<float, float>(Easing.ElasticIn);
			case EaseType.ElasticOut:
				return new Func<float, float>(Easing.ElasticOut);
			case EaseType.ElasticInOut:
				return new Func<float, float>(Easing.ElasticInOut);
			case EaseType.BounceIn:
				return new Func<float, float>(Easing.BounceIn);
			case EaseType.BounceOut:
				return new Func<float, float>(Easing.BounceOut);
			case EaseType.BounceInOut:
				return new Func<float, float>(Easing.BounceInOut);
			case EaseType.BackIn:
				return new Func<float, float>(Easing.BackIn);
			case EaseType.BackOut:
				return new Func<float, float>(Easing.BackOut);
			case EaseType.BackInOut:
				return new Func<float, float>(Easing.BackInOut);
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00016B24 File Offset: 0x00014D24
		public static float Linear(float t)
		{
			return t;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00016B27 File Offset: 0x00014D27
		public static float QuadraticIn(float t)
		{
			return t * t;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00016B2C File Offset: 0x00014D2C
		public static float QuadraticOut(float t)
		{
			return 1f - (1f - t) * (1f - t);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00016B43 File Offset: 0x00014D43
		public static float QuadraticInOut(float t)
		{
			if (t >= 0.5f)
			{
				return 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
			}
			return 2f * t * t;
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00016B7A File Offset: 0x00014D7A
		public static float QuarticIn(float t)
		{
			return t * t * t * t;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00016B83 File Offset: 0x00014D83
		public static float QuarticOut(float t)
		{
			return 1f - (t -= 1f) * t * t * t;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00016B9B File Offset: 0x00014D9B
		public static float QuarticInOut(float t)
		{
			if ((t *= 2f) < 1f)
			{
				return 0.5f * t * t * t * t;
			}
			return -0.5f * ((t -= 2f) * t * t * t - 2f);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00016BD8 File Offset: 0x00014DD8
		public static float QuinticIn(float t)
		{
			return t * t * t * t * t;
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00016BE3 File Offset: 0x00014DE3
		public static float QuinticOut(float t)
		{
			return (t -= 1f) * t * t * t * t + 1f;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00016C00 File Offset: 0x00014E00
		public static float QuinticInOut(float t)
		{
			if ((t *= 2f) < 1f)
			{
				return 0.5f * t * t * t * t * t;
			}
			return 0.5f * ((t -= 2f) * t * t * t * t + 2f);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00016C4C File Offset: 0x00014E4C
		public static float CubicIn(float t)
		{
			return t * t * t;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00016C53 File Offset: 0x00014E53
		public static float CubicOut(float t)
		{
			return (t -= 1f) * t * t + 1f;
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00016C69 File Offset: 0x00014E69
		public static float CubicInOut(float t)
		{
			if ((double)t >= 0.5)
			{
				return 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
			}
			return 4f * t * t * t;
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00016CA7 File Offset: 0x00014EA7
		public static float SinusoidalIn(float t)
		{
			return 1f - Mathf.Cos(t * 3.1415927f / 2f);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00016CC1 File Offset: 0x00014EC1
		public static float SinusoidalOut(float t)
		{
			return Mathf.Sin(t * 3.1415927f / 2f);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00016CD5 File Offset: 0x00014ED5
		public static float SinusoidalInOut(float t)
		{
			return 0.5f * (1f - Mathf.Cos(3.1415927f * t));
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00016CEF File Offset: 0x00014EEF
		public static float ExponentialIn(float t)
		{
			if (t != 0f)
			{
				return Mathf.Pow(2f, 10f * t - 10f);
			}
			return 0f;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00016D16 File Offset: 0x00014F16
		public static float ExponentialOut(float t)
		{
			if (t != 1f)
			{
				return 1f - Mathf.Pow(2f, -10f * t);
			}
			return 1f;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00016D40 File Offset: 0x00014F40
		public static float ExponentialInOut(float t)
		{
			if (t >= 0.5f)
			{
				return (2f - Mathf.Pow(2f, -20f * t + 10f)) / 2f;
			}
			return Mathf.Pow(2f, 20f * t - 10f) / 2f;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00016D96 File Offset: 0x00014F96
		public static float CircularIn(float t)
		{
			return 1f - Mathf.Sqrt(1f - t * t);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00016DAC File Offset: 0x00014FAC
		public static float CircularOut(float t)
		{
			return Mathf.Sqrt(1f - (t -= 1f) * t);
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00016DC8 File Offset: 0x00014FC8
		public static float CircularInOut(float t)
		{
			if (t >= 0.5f)
			{
				return (Mathf.Sqrt(1f - (t -= 2f) * t) + 1f) / 2f;
			}
			return (Mathf.Sqrt(1f - t * t) - 1f) / 2f;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00016E1C File Offset: 0x0001501C
		public static float ElasticIn(float t)
		{
			float num = 2.0943952f;
			return -Mathf.Pow(2f, 10f * t - 10f) * Mathf.Sin((t * 10f - 10.75f) * num);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00016E5C File Offset: 0x0001505C
		public static float ElasticOut(float t)
		{
			float num = 2.0943952f;
			return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * num) + 1f;
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00016E9C File Offset: 0x0001509C
		public static float ElasticInOut(float t)
		{
			float num = 1.3962635f;
			if (t < 0.5f)
			{
				return -(Mathf.Pow(2f, 20f * t - 10f) * Mathf.Sin((20f * t - 11.125f) * num)) / 2f;
			}
			return Mathf.Pow(2f, -20f * t + 10f) * Mathf.Sin((20f * t - 11.125f) * num) / 2f + 1f;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00016F23 File Offset: 0x00015123
		public static float BounceIn(float t)
		{
			return 1f - Easing.BounceOut(1f - t);
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00016F38 File Offset: 0x00015138
		public static float BounceOut(float t)
		{
			if (t < 0.36363637f)
			{
				return 7.5625f * t * t;
			}
			if (t < 0.72727275f)
			{
				return 7.5625f * (t -= 0.54545456f) * t + 0.75f;
			}
			if (t < 0.90909094f)
			{
				return 7.5625f * (t -= 0.8181818f) * t + 0.9375f;
			}
			return 7.5625f * (t -= 0.95454544f) * t + 0.984375f;
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00016FB1 File Offset: 0x000151B1
		public static float BounceInOut(float t)
		{
			if (t >= 0.5f)
			{
				return Easing.BounceOut(t * 2f - 1f) * 0.5f + 0.5f;
			}
			return Easing.BounceIn(t * 2f) * 0.5f;
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00016FEC File Offset: 0x000151EC
		public static float BackIn(float t)
		{
			float num = 1.70158f;
			return t * t * ((num + 1f) * t - num);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00017010 File Offset: 0x00015210
		public static float BackOut(float t)
		{
			float num = 1.70158f;
			return (t -= 1f) * t * ((num + 1f) * t + num) + 1f;
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00017044 File Offset: 0x00015244
		public static float BackInOut(float t)
		{
			float num = 2.5949094f;
			if ((t *= 2f) < 1f)
			{
				return 0.5f * (t * t * ((num + 1f) * t - num));
			}
			return 0.5f * ((t -= 2f) * t * ((num + 1f) * t + num) + 2f);
		}
	}
}
