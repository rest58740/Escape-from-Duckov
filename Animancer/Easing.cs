using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000043 RID: 67
	public static class Easing
	{
		// Token: 0x06000446 RID: 1094 RVA: 0x0000BCB4 File Offset: 0x00009EB4
		public static Func<float, float> GetDelegate(this Easing.Function function)
		{
			Func<float, float> func;
			if (Easing._FunctionDelegates == null)
			{
				Easing._FunctionDelegates = new Func<float, float>[31];
			}
			else
			{
				func = Easing._FunctionDelegates[(int)function];
				if (func != null)
				{
					return func;
				}
			}
			switch (function)
			{
			case Easing.Function.Linear:
				func = new Func<float, float>(Easing.Linear);
				break;
			case Easing.Function.QuadraticIn:
				func = new Func<float, float>(Easing.Quadratic.In);
				break;
			case Easing.Function.QuadraticOut:
				func = new Func<float, float>(Easing.Quadratic.Out);
				break;
			case Easing.Function.QuadraticInOut:
				func = new Func<float, float>(Easing.Quadratic.InOut);
				break;
			case Easing.Function.CubicIn:
				func = new Func<float, float>(Easing.Cubic.In);
				break;
			case Easing.Function.CubicOut:
				func = new Func<float, float>(Easing.Cubic.Out);
				break;
			case Easing.Function.CubicInOut:
				func = new Func<float, float>(Easing.Cubic.InOut);
				break;
			case Easing.Function.QuarticIn:
				func = new Func<float, float>(Easing.Quartic.In);
				break;
			case Easing.Function.QuarticOut:
				func = new Func<float, float>(Easing.Quartic.Out);
				break;
			case Easing.Function.QuarticInOut:
				func = new Func<float, float>(Easing.Quartic.InOut);
				break;
			case Easing.Function.QuinticIn:
				func = new Func<float, float>(Easing.Quintic.In);
				break;
			case Easing.Function.QuinticOut:
				func = new Func<float, float>(Easing.Quintic.Out);
				break;
			case Easing.Function.QuinticInOut:
				func = new Func<float, float>(Easing.Quintic.InOut);
				break;
			case Easing.Function.SineIn:
				func = new Func<float, float>(Easing.Sine.In);
				break;
			case Easing.Function.SineOut:
				func = new Func<float, float>(Easing.Sine.Out);
				break;
			case Easing.Function.SineInOut:
				func = new Func<float, float>(Easing.Sine.InOut);
				break;
			case Easing.Function.ExponentialIn:
				func = new Func<float, float>(Easing.Exponential.In);
				break;
			case Easing.Function.ExponentialOut:
				func = new Func<float, float>(Easing.Exponential.Out);
				break;
			case Easing.Function.ExponentialInOut:
				func = new Func<float, float>(Easing.Exponential.InOut);
				break;
			case Easing.Function.CircularIn:
				func = new Func<float, float>(Easing.Circular.In);
				break;
			case Easing.Function.CircularOut:
				func = new Func<float, float>(Easing.Circular.Out);
				break;
			case Easing.Function.CircularInOut:
				func = new Func<float, float>(Easing.Circular.InOut);
				break;
			case Easing.Function.BackIn:
				func = new Func<float, float>(Easing.Back.In);
				break;
			case Easing.Function.BackOut:
				func = new Func<float, float>(Easing.Back.Out);
				break;
			case Easing.Function.BackInOut:
				func = new Func<float, float>(Easing.Back.InOut);
				break;
			case Easing.Function.BounceIn:
				func = new Func<float, float>(Easing.Bounce.In);
				break;
			case Easing.Function.BounceOut:
				func = new Func<float, float>(Easing.Bounce.Out);
				break;
			case Easing.Function.BounceInOut:
				func = new Func<float, float>(Easing.Bounce.InOut);
				break;
			case Easing.Function.ElasticIn:
				func = new Func<float, float>(Easing.Elastic.In);
				break;
			case Easing.Function.ElasticOut:
				func = new Func<float, float>(Easing.Elastic.Out);
				break;
			case Easing.Function.ElasticInOut:
				func = new Func<float, float>(Easing.Elastic.InOut);
				break;
			default:
				throw new ArgumentOutOfRangeException("function");
			}
			Easing._FunctionDelegates[(int)function] = func;
			return func;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000BF98 File Offset: 0x0000A198
		public static Func<float, float> GetDerivativeDelegate(this Easing.Function function)
		{
			Func<float, float> func;
			if (Easing._DerivativeDelegates == null)
			{
				Easing._DerivativeDelegates = new Func<float, float>[31];
			}
			else
			{
				func = Easing._DerivativeDelegates[(int)function];
				if (func != null)
				{
					return func;
				}
			}
			switch (function)
			{
			case Easing.Function.Linear:
				func = new Func<float, float>(Easing.LinearDerivative);
				break;
			case Easing.Function.QuadraticIn:
				func = new Func<float, float>(Easing.Quadratic.InDerivative);
				break;
			case Easing.Function.QuadraticOut:
				func = new Func<float, float>(Easing.Quadratic.OutDerivative);
				break;
			case Easing.Function.QuadraticInOut:
				func = new Func<float, float>(Easing.Quadratic.InOutDerivative);
				break;
			case Easing.Function.CubicIn:
				func = new Func<float, float>(Easing.Cubic.InDerivative);
				break;
			case Easing.Function.CubicOut:
				func = new Func<float, float>(Easing.Cubic.OutDerivative);
				break;
			case Easing.Function.CubicInOut:
				func = new Func<float, float>(Easing.Cubic.InOutDerivative);
				break;
			case Easing.Function.QuarticIn:
				func = new Func<float, float>(Easing.Quartic.InDerivative);
				break;
			case Easing.Function.QuarticOut:
				func = new Func<float, float>(Easing.Quartic.OutDerivative);
				break;
			case Easing.Function.QuarticInOut:
				func = new Func<float, float>(Easing.Quartic.InOutDerivative);
				break;
			case Easing.Function.QuinticIn:
				func = new Func<float, float>(Easing.Quintic.InDerivative);
				break;
			case Easing.Function.QuinticOut:
				func = new Func<float, float>(Easing.Quintic.OutDerivative);
				break;
			case Easing.Function.QuinticInOut:
				func = new Func<float, float>(Easing.Quintic.InOutDerivative);
				break;
			case Easing.Function.SineIn:
				func = new Func<float, float>(Easing.Sine.InDerivative);
				break;
			case Easing.Function.SineOut:
				func = new Func<float, float>(Easing.Sine.OutDerivative);
				break;
			case Easing.Function.SineInOut:
				func = new Func<float, float>(Easing.Sine.InOutDerivative);
				break;
			case Easing.Function.ExponentialIn:
				func = new Func<float, float>(Easing.Exponential.InDerivative);
				break;
			case Easing.Function.ExponentialOut:
				func = new Func<float, float>(Easing.Exponential.OutDerivative);
				break;
			case Easing.Function.ExponentialInOut:
				func = new Func<float, float>(Easing.Exponential.InOutDerivative);
				break;
			case Easing.Function.CircularIn:
				func = new Func<float, float>(Easing.Circular.InDerivative);
				break;
			case Easing.Function.CircularOut:
				func = new Func<float, float>(Easing.Circular.OutDerivative);
				break;
			case Easing.Function.CircularInOut:
				func = new Func<float, float>(Easing.Circular.InOutDerivative);
				break;
			case Easing.Function.BackIn:
				func = new Func<float, float>(Easing.Back.InDerivative);
				break;
			case Easing.Function.BackOut:
				func = new Func<float, float>(Easing.Back.OutDerivative);
				break;
			case Easing.Function.BackInOut:
				func = new Func<float, float>(Easing.Back.InOutDerivative);
				break;
			case Easing.Function.BounceIn:
				func = new Func<float, float>(Easing.Bounce.InDerivative);
				break;
			case Easing.Function.BounceOut:
				func = new Func<float, float>(Easing.Bounce.OutDerivative);
				break;
			case Easing.Function.BounceInOut:
				func = new Func<float, float>(Easing.Bounce.InOutDerivative);
				break;
			case Easing.Function.ElasticIn:
				func = new Func<float, float>(Easing.Elastic.InDerivative);
				break;
			case Easing.Function.ElasticOut:
				func = new Func<float, float>(Easing.Elastic.OutDerivative);
				break;
			case Easing.Function.ElasticInOut:
				func = new Func<float, float>(Easing.Elastic.InOutDerivative);
				break;
			default:
				throw new ArgumentOutOfRangeException("function");
			}
			Easing._DerivativeDelegates[(int)function] = func;
			return func;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000C27C File Offset: 0x0000A47C
		public static Easing.RangedDelegate GetRangedDelegate(this Easing.Function function)
		{
			Easing.RangedDelegate rangedDelegate;
			if (Easing._RangedFunctionDelegates == null)
			{
				Easing._RangedFunctionDelegates = new Easing.RangedDelegate[31];
			}
			else
			{
				rangedDelegate = Easing._RangedFunctionDelegates[(int)function];
				if (rangedDelegate != null)
				{
					return rangedDelegate;
				}
			}
			switch (function)
			{
			case Easing.Function.Linear:
				rangedDelegate = new Easing.RangedDelegate(Easing.Linear);
				break;
			case Easing.Function.QuadraticIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quadratic.In);
				break;
			case Easing.Function.QuadraticOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quadratic.Out);
				break;
			case Easing.Function.QuadraticInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quadratic.InOut);
				break;
			case Easing.Function.CubicIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Cubic.In);
				break;
			case Easing.Function.CubicOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Cubic.Out);
				break;
			case Easing.Function.CubicInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Cubic.InOut);
				break;
			case Easing.Function.QuarticIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quartic.In);
				break;
			case Easing.Function.QuarticOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quartic.Out);
				break;
			case Easing.Function.QuarticInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quartic.InOut);
				break;
			case Easing.Function.QuinticIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quintic.In);
				break;
			case Easing.Function.QuinticOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quintic.Out);
				break;
			case Easing.Function.QuinticInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quintic.InOut);
				break;
			case Easing.Function.SineIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Sine.In);
				break;
			case Easing.Function.SineOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Sine.Out);
				break;
			case Easing.Function.SineInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Sine.InOut);
				break;
			case Easing.Function.ExponentialIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Exponential.In);
				break;
			case Easing.Function.ExponentialOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Exponential.Out);
				break;
			case Easing.Function.ExponentialInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Exponential.InOut);
				break;
			case Easing.Function.CircularIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Circular.In);
				break;
			case Easing.Function.CircularOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Circular.Out);
				break;
			case Easing.Function.CircularInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Circular.InOut);
				break;
			case Easing.Function.BackIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Back.In);
				break;
			case Easing.Function.BackOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Back.Out);
				break;
			case Easing.Function.BackInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Back.InOut);
				break;
			case Easing.Function.BounceIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Bounce.In);
				break;
			case Easing.Function.BounceOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Bounce.Out);
				break;
			case Easing.Function.BounceInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Bounce.InOut);
				break;
			case Easing.Function.ElasticIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Elastic.In);
				break;
			case Easing.Function.ElasticOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Elastic.Out);
				break;
			case Easing.Function.ElasticInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Elastic.InOut);
				break;
			default:
				throw new ArgumentOutOfRangeException("function");
			}
			Easing._RangedFunctionDelegates[(int)function] = rangedDelegate;
			return rangedDelegate;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0000C560 File Offset: 0x0000A760
		public static Easing.RangedDelegate GetRangedDerivativeDelegate(this Easing.Function function)
		{
			Easing.RangedDelegate rangedDelegate;
			if (Easing._RangedDerivativeDelegates == null)
			{
				Easing._RangedDerivativeDelegates = new Easing.RangedDelegate[31];
			}
			else
			{
				rangedDelegate = Easing._RangedDerivativeDelegates[(int)function];
				if (rangedDelegate != null)
				{
					return rangedDelegate;
				}
			}
			switch (function)
			{
			case Easing.Function.Linear:
				rangedDelegate = new Easing.RangedDelegate(Easing.LinearDerivative);
				break;
			case Easing.Function.QuadraticIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quadratic.InDerivative);
				break;
			case Easing.Function.QuadraticOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quadratic.OutDerivative);
				break;
			case Easing.Function.QuadraticInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quadratic.InOutDerivative);
				break;
			case Easing.Function.CubicIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Cubic.InDerivative);
				break;
			case Easing.Function.CubicOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Cubic.OutDerivative);
				break;
			case Easing.Function.CubicInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Cubic.InOutDerivative);
				break;
			case Easing.Function.QuarticIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quartic.InDerivative);
				break;
			case Easing.Function.QuarticOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quartic.OutDerivative);
				break;
			case Easing.Function.QuarticInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quartic.InOutDerivative);
				break;
			case Easing.Function.QuinticIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quintic.InDerivative);
				break;
			case Easing.Function.QuinticOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quintic.OutDerivative);
				break;
			case Easing.Function.QuinticInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Quintic.InOutDerivative);
				break;
			case Easing.Function.SineIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Sine.InDerivative);
				break;
			case Easing.Function.SineOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Sine.OutDerivative);
				break;
			case Easing.Function.SineInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Sine.InOutDerivative);
				break;
			case Easing.Function.ExponentialIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Exponential.InDerivative);
				break;
			case Easing.Function.ExponentialOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Exponential.OutDerivative);
				break;
			case Easing.Function.ExponentialInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Exponential.InOutDerivative);
				break;
			case Easing.Function.CircularIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Circular.InDerivative);
				break;
			case Easing.Function.CircularOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Circular.OutDerivative);
				break;
			case Easing.Function.CircularInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Circular.InOutDerivative);
				break;
			case Easing.Function.BackIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Back.InDerivative);
				break;
			case Easing.Function.BackOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Back.OutDerivative);
				break;
			case Easing.Function.BackInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Back.InOutDerivative);
				break;
			case Easing.Function.BounceIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Bounce.InDerivative);
				break;
			case Easing.Function.BounceOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Bounce.OutDerivative);
				break;
			case Easing.Function.BounceInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Bounce.InOutDerivative);
				break;
			case Easing.Function.ElasticIn:
				rangedDelegate = new Easing.RangedDelegate(Easing.Elastic.InDerivative);
				break;
			case Easing.Function.ElasticOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Elastic.OutDerivative);
				break;
			case Easing.Function.ElasticInOut:
				rangedDelegate = new Easing.RangedDelegate(Easing.Elastic.InOutDerivative);
				break;
			default:
				throw new ArgumentOutOfRangeException("function");
			}
			Easing._RangedDerivativeDelegates[(int)function] = rangedDelegate;
			return rangedDelegate;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000C842 File Offset: 0x0000AA42
		public static float Lerp(float start, float end, float value)
		{
			return start + (end - start) * value;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0000C84B File Offset: 0x0000AA4B
		public static float UnLerp(float start, float end, float value)
		{
			if (start != end)
			{
				return (value - start) / (end - start);
			}
			return 0f;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000C85E File Offset: 0x0000AA5E
		public static float ReScale(float start, float end, float value, Func<float, float> function)
		{
			return Easing.Lerp(start, end, function(Easing.UnLerp(start, end, value)));
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000C875 File Offset: 0x0000AA75
		public static float Linear(float value)
		{
			return value;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000C878 File Offset: 0x0000AA78
		public static float LinearDerivative(float value)
		{
			return 1f;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000C87F File Offset: 0x0000AA7F
		public static float Linear(float start, float end, float value)
		{
			return value;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000C882 File Offset: 0x0000AA82
		public static float LinearDerivative(float start, float end, float value)
		{
			return end - start;
		}

		// Token: 0x040000B2 RID: 178
		public const float Ln2 = 0.6931472f;

		// Token: 0x040000B3 RID: 179
		public const int FunctionCount = 31;

		// Token: 0x040000B4 RID: 180
		private static Func<float, float>[] _FunctionDelegates;

		// Token: 0x040000B5 RID: 181
		private static Func<float, float>[] _DerivativeDelegates;

		// Token: 0x040000B6 RID: 182
		private static Easing.RangedDelegate[] _RangedFunctionDelegates;

		// Token: 0x040000B7 RID: 183
		private static Easing.RangedDelegate[] _RangedDerivativeDelegates;

		// Token: 0x02000099 RID: 153
		// (Invoke) Token: 0x06000675 RID: 1653
		public delegate float RangedDelegate(float start, float end, float value);

		// Token: 0x0200009A RID: 154
		public enum Function
		{
			// Token: 0x04000158 RID: 344
			Linear,
			// Token: 0x04000159 RID: 345
			QuadraticIn,
			// Token: 0x0400015A RID: 346
			QuadraticOut,
			// Token: 0x0400015B RID: 347
			QuadraticInOut,
			// Token: 0x0400015C RID: 348
			CubicIn,
			// Token: 0x0400015D RID: 349
			CubicOut,
			// Token: 0x0400015E RID: 350
			CubicInOut,
			// Token: 0x0400015F RID: 351
			QuarticIn,
			// Token: 0x04000160 RID: 352
			QuarticOut,
			// Token: 0x04000161 RID: 353
			QuarticInOut,
			// Token: 0x04000162 RID: 354
			QuinticIn,
			// Token: 0x04000163 RID: 355
			QuinticOut,
			// Token: 0x04000164 RID: 356
			QuinticInOut,
			// Token: 0x04000165 RID: 357
			SineIn,
			// Token: 0x04000166 RID: 358
			SineOut,
			// Token: 0x04000167 RID: 359
			SineInOut,
			// Token: 0x04000168 RID: 360
			ExponentialIn,
			// Token: 0x04000169 RID: 361
			ExponentialOut,
			// Token: 0x0400016A RID: 362
			ExponentialInOut,
			// Token: 0x0400016B RID: 363
			CircularIn,
			// Token: 0x0400016C RID: 364
			CircularOut,
			// Token: 0x0400016D RID: 365
			CircularInOut,
			// Token: 0x0400016E RID: 366
			BackIn,
			// Token: 0x0400016F RID: 367
			BackOut,
			// Token: 0x04000170 RID: 368
			BackInOut,
			// Token: 0x04000171 RID: 369
			BounceIn,
			// Token: 0x04000172 RID: 370
			BounceOut,
			// Token: 0x04000173 RID: 371
			BounceInOut,
			// Token: 0x04000174 RID: 372
			ElasticIn,
			// Token: 0x04000175 RID: 373
			ElasticOut,
			// Token: 0x04000176 RID: 374
			ElasticInOut
		}

		// Token: 0x0200009B RID: 155
		public static class Quadratic
		{
			// Token: 0x06000678 RID: 1656 RVA: 0x0001152D File Offset: 0x0000F72D
			public static float In(float value)
			{
				return value * value;
			}

			// Token: 0x06000679 RID: 1657 RVA: 0x00011532 File Offset: 0x0000F732
			public static float Out(float value)
			{
				value -= 1f;
				return -value * value + 1f;
			}

			// Token: 0x0600067A RID: 1658 RVA: 0x00011547 File Offset: 0x0000F747
			public static float InOut(float value)
			{
				value *= 2f;
				if (value <= 1f)
				{
					return 0.5f * value * value;
				}
				value -= 2f;
				return 0.5f * (-value * value + 2f);
			}

			// Token: 0x0600067B RID: 1659 RVA: 0x0001157D File Offset: 0x0000F77D
			public static float InDerivative(float value)
			{
				return 2f * value;
			}

			// Token: 0x0600067C RID: 1660 RVA: 0x00011586 File Offset: 0x0000F786
			public static float OutDerivative(float value)
			{
				return 2f - 2f * value;
			}

			// Token: 0x0600067D RID: 1661 RVA: 0x00011595 File Offset: 0x0000F795
			public static float InOutDerivative(float value)
			{
				value *= 2f;
				if (value <= 1f)
				{
					return 2f * value;
				}
				value -= 1f;
				return 2f - 2f * value;
			}

			// Token: 0x0600067E RID: 1662 RVA: 0x000115C6 File Offset: 0x0000F7C6
			public static float In(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Quadratic.In(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x0600067F RID: 1663 RVA: 0x000115DC File Offset: 0x0000F7DC
			public static float Out(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Quadratic.Out(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x06000680 RID: 1664 RVA: 0x000115F2 File Offset: 0x0000F7F2
			public static float InOut(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Quadratic.InOut(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x06000681 RID: 1665 RVA: 0x00011608 File Offset: 0x0000F808
			public static float InDerivative(float start, float end, float value)
			{
				return Easing.Quadratic.InDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x06000682 RID: 1666 RVA: 0x0001161B File Offset: 0x0000F81B
			public static float OutDerivative(float start, float end, float value)
			{
				return Easing.Quadratic.OutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x06000683 RID: 1667 RVA: 0x0001162E File Offset: 0x0000F82E
			public static float InOutDerivative(float start, float end, float value)
			{
				return Easing.Quadratic.InOutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}
		}

		// Token: 0x0200009C RID: 156
		public static class Cubic
		{
			// Token: 0x06000684 RID: 1668 RVA: 0x00011641 File Offset: 0x0000F841
			public static float In(float value)
			{
				return value * value * value;
			}

			// Token: 0x06000685 RID: 1669 RVA: 0x00011648 File Offset: 0x0000F848
			public static float Out(float value)
			{
				value -= 1f;
				return value * value * value + 1f;
			}

			// Token: 0x06000686 RID: 1670 RVA: 0x0001165E File Offset: 0x0000F85E
			public static float InOut(float value)
			{
				value *= 2f;
				if (value <= 1f)
				{
					return 0.5f * value * value * value;
				}
				value -= 2f;
				return 0.5f * (value * value * value + 2f);
			}

			// Token: 0x06000687 RID: 1671 RVA: 0x00011697 File Offset: 0x0000F897
			public static float InDerivative(float value)
			{
				return 3f * value * value;
			}

			// Token: 0x06000688 RID: 1672 RVA: 0x000116A2 File Offset: 0x0000F8A2
			public static float OutDerivative(float value)
			{
				value -= 1f;
				return 3f * value * value;
			}

			// Token: 0x06000689 RID: 1673 RVA: 0x000116B6 File Offset: 0x0000F8B6
			public static float InOutDerivative(float value)
			{
				value *= 2f;
				if (value <= 1f)
				{
					return 3f * value * value;
				}
				value -= 2f;
				return 3f * value * value;
			}

			// Token: 0x0600068A RID: 1674 RVA: 0x000116E5 File Offset: 0x0000F8E5
			public static float In(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Cubic.In(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x0600068B RID: 1675 RVA: 0x000116FB File Offset: 0x0000F8FB
			public static float Out(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Cubic.Out(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x0600068C RID: 1676 RVA: 0x00011711 File Offset: 0x0000F911
			public static float InOut(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Cubic.InOut(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x0600068D RID: 1677 RVA: 0x00011727 File Offset: 0x0000F927
			public static float InDerivative(float start, float end, float value)
			{
				return Easing.Cubic.InDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x0600068E RID: 1678 RVA: 0x0001173A File Offset: 0x0000F93A
			public static float OutDerivative(float start, float end, float value)
			{
				return Easing.Cubic.OutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x0600068F RID: 1679 RVA: 0x0001174D File Offset: 0x0000F94D
			public static float InOutDerivative(float start, float end, float value)
			{
				return Easing.Cubic.InOutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}
		}

		// Token: 0x0200009D RID: 157
		public static class Quartic
		{
			// Token: 0x06000690 RID: 1680 RVA: 0x00011760 File Offset: 0x0000F960
			public static float In(float value)
			{
				return value * value * value * value;
			}

			// Token: 0x06000691 RID: 1681 RVA: 0x00011769 File Offset: 0x0000F969
			public static float Out(float value)
			{
				value -= 1f;
				return -value * value * value * value + 1f;
			}

			// Token: 0x06000692 RID: 1682 RVA: 0x00011782 File Offset: 0x0000F982
			public static float InOut(float value)
			{
				value *= 2f;
				if (value <= 1f)
				{
					return 0.5f * value * value * value * value;
				}
				value -= 2f;
				return 0.5f * (-value * value * value * value + 2f);
			}

			// Token: 0x06000693 RID: 1683 RVA: 0x000117C0 File Offset: 0x0000F9C0
			public static float InDerivative(float value)
			{
				return 4f * value * value * value;
			}

			// Token: 0x06000694 RID: 1684 RVA: 0x000117CD File Offset: 0x0000F9CD
			public static float OutDerivative(float value)
			{
				value -= 1f;
				return -4f * value * value * value;
			}

			// Token: 0x06000695 RID: 1685 RVA: 0x000117E3 File Offset: 0x0000F9E3
			public static float InOutDerivative(float value)
			{
				value *= 2f;
				if (value <= 1f)
				{
					return 4f * value * value * value;
				}
				value -= 2f;
				return -4f * value * value * value;
			}

			// Token: 0x06000696 RID: 1686 RVA: 0x00011816 File Offset: 0x0000FA16
			public static float In(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Quartic.In(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x06000697 RID: 1687 RVA: 0x0001182C File Offset: 0x0000FA2C
			public static float Out(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Quartic.Out(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x06000698 RID: 1688 RVA: 0x00011842 File Offset: 0x0000FA42
			public static float InOut(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Quartic.InOut(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x06000699 RID: 1689 RVA: 0x00011858 File Offset: 0x0000FA58
			public static float InDerivative(float start, float end, float value)
			{
				return Easing.Quartic.InDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x0600069A RID: 1690 RVA: 0x0001186B File Offset: 0x0000FA6B
			public static float OutDerivative(float start, float end, float value)
			{
				return Easing.Quartic.OutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x0600069B RID: 1691 RVA: 0x0001187E File Offset: 0x0000FA7E
			public static float InOutDerivative(float start, float end, float value)
			{
				return Easing.Quartic.InOutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}
		}

		// Token: 0x0200009E RID: 158
		public static class Quintic
		{
			// Token: 0x0600069C RID: 1692 RVA: 0x00011891 File Offset: 0x0000FA91
			public static float In(float value)
			{
				return value * value * value * value * value;
			}

			// Token: 0x0600069D RID: 1693 RVA: 0x0001189C File Offset: 0x0000FA9C
			public static float Out(float value)
			{
				value -= 1f;
				return value * value * value * value * value + 1f;
			}

			// Token: 0x0600069E RID: 1694 RVA: 0x000118B8 File Offset: 0x0000FAB8
			public static float InOut(float value)
			{
				value *= 2f;
				if (value <= 1f)
				{
					return 0.5f * value * value * value * value * value;
				}
				value -= 2f;
				return 0.5f * (value * value * value * value * value + 2f);
			}

			// Token: 0x0600069F RID: 1695 RVA: 0x00011904 File Offset: 0x0000FB04
			public static float InDerivative(float value)
			{
				return 5f * value * value * value * value;
			}

			// Token: 0x060006A0 RID: 1696 RVA: 0x00011913 File Offset: 0x0000FB13
			public static float OutDerivative(float value)
			{
				value -= 1f;
				return 5f * value * value * value * value;
			}

			// Token: 0x060006A1 RID: 1697 RVA: 0x0001192B File Offset: 0x0000FB2B
			public static float InOutDerivative(float value)
			{
				value *= 2f;
				if (value <= 1f)
				{
					return 5f * value * value * value * value;
				}
				value -= 2f;
				return 5f * value * value * value * value;
			}

			// Token: 0x060006A2 RID: 1698 RVA: 0x00011962 File Offset: 0x0000FB62
			public static float In(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Quintic.In(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006A3 RID: 1699 RVA: 0x00011978 File Offset: 0x0000FB78
			public static float Out(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Quintic.Out(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006A4 RID: 1700 RVA: 0x0001198E File Offset: 0x0000FB8E
			public static float InOut(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Quintic.InOut(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006A5 RID: 1701 RVA: 0x000119A4 File Offset: 0x0000FBA4
			public static float InDerivative(float start, float end, float value)
			{
				return Easing.Quintic.InDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x060006A6 RID: 1702 RVA: 0x000119B7 File Offset: 0x0000FBB7
			public static float OutDerivative(float start, float end, float value)
			{
				return Easing.Quintic.OutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x060006A7 RID: 1703 RVA: 0x000119CA File Offset: 0x0000FBCA
			public static float InOutDerivative(float start, float end, float value)
			{
				return Easing.Quintic.InOutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}
		}

		// Token: 0x0200009F RID: 159
		public static class Sine
		{
			// Token: 0x060006A8 RID: 1704 RVA: 0x000119DD File Offset: 0x0000FBDD
			public static float In(float value)
			{
				return -Mathf.Cos(value * 1.5707964f) + 1f;
			}

			// Token: 0x060006A9 RID: 1705 RVA: 0x000119F2 File Offset: 0x0000FBF2
			public static float Out(float value)
			{
				return Mathf.Sin(value * 1.5707964f);
			}

			// Token: 0x060006AA RID: 1706 RVA: 0x00011A00 File Offset: 0x0000FC00
			public static float InOut(float value)
			{
				return -0.5f * (Mathf.Cos(3.1415927f * value) - 1f);
			}

			// Token: 0x060006AB RID: 1707 RVA: 0x00011A1A File Offset: 0x0000FC1A
			public static float InDerivative(float value)
			{
				return 1.5707964f * Mathf.Sin(1.5707964f * value);
			}

			// Token: 0x060006AC RID: 1708 RVA: 0x00011A2E File Offset: 0x0000FC2E
			public static float OutDerivative(float value)
			{
				return 1.5707964f * Mathf.Cos(value * 1.5707964f);
			}

			// Token: 0x060006AD RID: 1709 RVA: 0x00011A42 File Offset: 0x0000FC42
			public static float InOutDerivative(float value)
			{
				return 1.5707964f * Mathf.Sin(3.1415927f * value);
			}

			// Token: 0x060006AE RID: 1710 RVA: 0x00011A56 File Offset: 0x0000FC56
			public static float In(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Sine.In(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006AF RID: 1711 RVA: 0x00011A6C File Offset: 0x0000FC6C
			public static float Out(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Sine.Out(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006B0 RID: 1712 RVA: 0x00011A82 File Offset: 0x0000FC82
			public static float InOut(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Sine.InOut(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006B1 RID: 1713 RVA: 0x00011A98 File Offset: 0x0000FC98
			public static float InDerivative(float start, float end, float value)
			{
				return Easing.Sine.InDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x060006B2 RID: 1714 RVA: 0x00011AAB File Offset: 0x0000FCAB
			public static float OutDerivative(float start, float end, float value)
			{
				return Easing.Sine.OutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x060006B3 RID: 1715 RVA: 0x00011ABE File Offset: 0x0000FCBE
			public static float InOutDerivative(float start, float end, float value)
			{
				return Easing.Sine.InOutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}
		}

		// Token: 0x020000A0 RID: 160
		public static class Exponential
		{
			// Token: 0x060006B4 RID: 1716 RVA: 0x00011AD1 File Offset: 0x0000FCD1
			public static float In(float value)
			{
				return Mathf.Pow(2f, 10f * (value - 1f));
			}

			// Token: 0x060006B5 RID: 1717 RVA: 0x00011AEA File Offset: 0x0000FCEA
			public static float Out(float value)
			{
				return -Mathf.Pow(2f, -10f * value) + 1f;
			}

			// Token: 0x060006B6 RID: 1718 RVA: 0x00011B04 File Offset: 0x0000FD04
			public static float InOut(float value)
			{
				value *= 2f;
				if (value <= 1f)
				{
					return 0.5f * Mathf.Pow(2f, 10f * (value - 1f));
				}
				value -= 1f;
				return 0.5f * (-Mathf.Pow(2f, -10f * value) + 2f);
			}

			// Token: 0x060006B7 RID: 1719 RVA: 0x00011B67 File Offset: 0x0000FD67
			public static float InDerivative(float value)
			{
				return 6.931472f * Mathf.Pow(2f, 10f * (value - 1f));
			}

			// Token: 0x060006B8 RID: 1720 RVA: 0x00011B86 File Offset: 0x0000FD86
			public static float OutDerivative(float value)
			{
				return 3.465736f * Mathf.Pow(2f, 1f - 10f * value);
			}

			// Token: 0x060006B9 RID: 1721 RVA: 0x00011BA8 File Offset: 0x0000FDA8
			public static float InOutDerivative(float value)
			{
				value *= 2f;
				if (value <= 1f)
				{
					return 6.931472f * Mathf.Pow(2f, 10f * (value - 1f));
				}
				value -= 1f;
				return 3.465736f * Mathf.Pow(2f, 1f - 10f * value);
			}

			// Token: 0x060006BA RID: 1722 RVA: 0x00011C0A File Offset: 0x0000FE0A
			public static float In(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Exponential.In(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006BB RID: 1723 RVA: 0x00011C20 File Offset: 0x0000FE20
			public static float Out(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Exponential.Out(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006BC RID: 1724 RVA: 0x00011C36 File Offset: 0x0000FE36
			public static float InOut(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Exponential.InOut(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006BD RID: 1725 RVA: 0x00011C4C File Offset: 0x0000FE4C
			public static float InDerivative(float start, float end, float value)
			{
				return Easing.Exponential.InDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x060006BE RID: 1726 RVA: 0x00011C5F File Offset: 0x0000FE5F
			public static float OutDerivative(float start, float end, float value)
			{
				return Easing.Exponential.OutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x060006BF RID: 1727 RVA: 0x00011C72 File Offset: 0x0000FE72
			public static float InOutDerivative(float start, float end, float value)
			{
				return Easing.Exponential.InOutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}
		}

		// Token: 0x020000A1 RID: 161
		public static class Circular
		{
			// Token: 0x060006C0 RID: 1728 RVA: 0x00011C85 File Offset: 0x0000FE85
			public static float In(float value)
			{
				return -(Mathf.Sqrt(1f - value * value) - 1f);
			}

			// Token: 0x060006C1 RID: 1729 RVA: 0x00011C9C File Offset: 0x0000FE9C
			public static float Out(float value)
			{
				value -= 1f;
				return Mathf.Sqrt(1f - value * value);
			}

			// Token: 0x060006C2 RID: 1730 RVA: 0x00011CB8 File Offset: 0x0000FEB8
			public static float InOut(float value)
			{
				value *= 2f;
				if (value <= 1f)
				{
					return -0.5f * (Mathf.Sqrt(1f - value * value) - 1f);
				}
				value -= 2f;
				return 0.5f * (Mathf.Sqrt(1f - value * value) + 1f);
			}

			// Token: 0x060006C3 RID: 1731 RVA: 0x00011D14 File Offset: 0x0000FF14
			public static float InDerivative(float value)
			{
				return value / Mathf.Sqrt(1f - value * value);
			}

			// Token: 0x060006C4 RID: 1732 RVA: 0x00011D26 File Offset: 0x0000FF26
			public static float OutDerivative(float value)
			{
				value -= 1f;
				return -value / Mathf.Sqrt(1f - value * value);
			}

			// Token: 0x060006C5 RID: 1733 RVA: 0x00011D44 File Offset: 0x0000FF44
			public static float InOutDerivative(float value)
			{
				value *= 2f;
				if (value <= 1f)
				{
					return value / (2f * Mathf.Sqrt(1f - value * value));
				}
				value -= 2f;
				return -value / (2f * Mathf.Sqrt(1f - value * value));
			}

			// Token: 0x060006C6 RID: 1734 RVA: 0x00011D99 File Offset: 0x0000FF99
			public static float In(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Circular.In(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006C7 RID: 1735 RVA: 0x00011DAF File Offset: 0x0000FFAF
			public static float Out(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Circular.Out(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006C8 RID: 1736 RVA: 0x00011DC5 File Offset: 0x0000FFC5
			public static float InOut(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Circular.InOut(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006C9 RID: 1737 RVA: 0x00011DDB File Offset: 0x0000FFDB
			public static float InDerivative(float start, float end, float value)
			{
				return Easing.Circular.InDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x060006CA RID: 1738 RVA: 0x00011DEE File Offset: 0x0000FFEE
			public static float OutDerivative(float start, float end, float value)
			{
				return Easing.Circular.OutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x060006CB RID: 1739 RVA: 0x00011E01 File Offset: 0x00010001
			public static float InOutDerivative(float start, float end, float value)
			{
				return Easing.Circular.InOutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}
		}

		// Token: 0x020000A2 RID: 162
		public static class Back
		{
			// Token: 0x060006CC RID: 1740 RVA: 0x00011E14 File Offset: 0x00010014
			public static float In(float value)
			{
				return value * value * (2.758f * value - 1.758f);
			}

			// Token: 0x060006CD RID: 1741 RVA: 0x00011E27 File Offset: 0x00010027
			public static float Out(float value)
			{
				value -= 1f;
				return value * value * (2.758f * value + 1.758f) + 1f;
			}

			// Token: 0x060006CE RID: 1742 RVA: 0x00011E4C File Offset: 0x0001004C
			public static float InOut(float value)
			{
				value *= 2f;
				if (value <= 1f)
				{
					return 0.5f * value * value * (2.758f * value - 1.758f);
				}
				value -= 2f;
				return 0.5f * (value * value * (2.758f * value + 1.758f) + 2f);
			}

			// Token: 0x060006CF RID: 1743 RVA: 0x00011EA8 File Offset: 0x000100A8
			public static float InDerivative(float value)
			{
				return 8.274f * value * value - 3.516f * value;
			}

			// Token: 0x060006D0 RID: 1744 RVA: 0x00011EBB File Offset: 0x000100BB
			public static float OutDerivative(float value)
			{
				value -= 1f;
				return 2.758f * value * value + 2f * value * (2.758f * value + 1.758f);
			}

			// Token: 0x060006D1 RID: 1745 RVA: 0x00011EE8 File Offset: 0x000100E8
			public static float InOutDerivative(float value)
			{
				value *= 2f;
				if (value <= 1f)
				{
					return 8.274f * value * value - 3.516f * value;
				}
				value -= 2f;
				return 2.758f * value * value + 2f * value * (2.758f * value + 1.758f);
			}

			// Token: 0x060006D2 RID: 1746 RVA: 0x00011F40 File Offset: 0x00010140
			public static float In(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Back.In(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006D3 RID: 1747 RVA: 0x00011F56 File Offset: 0x00010156
			public static float Out(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Back.Out(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006D4 RID: 1748 RVA: 0x00011F6C File Offset: 0x0001016C
			public static float InOut(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Back.InOut(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006D5 RID: 1749 RVA: 0x00011F82 File Offset: 0x00010182
			public static float InDerivative(float start, float end, float value)
			{
				return Easing.Back.InDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x060006D6 RID: 1750 RVA: 0x00011F95 File Offset: 0x00010195
			public static float OutDerivative(float start, float end, float value)
			{
				return Easing.Back.OutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x060006D7 RID: 1751 RVA: 0x00011FA8 File Offset: 0x000101A8
			public static float InOutDerivative(float start, float end, float value)
			{
				return Easing.Back.InOutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x04000177 RID: 375
			private const float C = 1.758f;
		}

		// Token: 0x020000A3 RID: 163
		public static class Bounce
		{
			// Token: 0x060006D8 RID: 1752 RVA: 0x00011FBB File Offset: 0x000101BB
			public static float In(float value)
			{
				return 1f - Easing.Bounce.Out(1f - value);
			}

			// Token: 0x060006D9 RID: 1753 RVA: 0x00011FD0 File Offset: 0x000101D0
			public static float Out(float value)
			{
				if (value == 0f)
				{
					return 0f;
				}
				if (value == 1f)
				{
					return 1f;
				}
				if (value < 0.36363637f)
				{
					return 7.5625f * value * value;
				}
				if (value < 0.72727275f)
				{
					value -= 0.54545456f;
					return 7.5625f * value * value + 0.75f;
				}
				if (value < 0.90909094f)
				{
					value -= 0.8181818f;
					return 7.5625f * value * value + 0.9375f;
				}
				value -= 0.95454544f;
				return 7.5625f * value * value + 0.984375f;
			}

			// Token: 0x060006DA RID: 1754 RVA: 0x00012067 File Offset: 0x00010267
			public static float InOut(float value)
			{
				if (value < 0.5f)
				{
					return 0.5f * Easing.Bounce.In(value * 2f);
				}
				return 0.5f + 0.5f * Easing.Bounce.Out(value * 2f - 1f);
			}

			// Token: 0x060006DB RID: 1755 RVA: 0x000120A2 File Offset: 0x000102A2
			public static float InDerivative(float value)
			{
				return Easing.Bounce.OutDerivative(1f - value);
			}

			// Token: 0x060006DC RID: 1756 RVA: 0x000120B0 File Offset: 0x000102B0
			public static float OutDerivative(float value)
			{
				if (value < 0.36363637f)
				{
					return 15.125f * value;
				}
				if (value < 0.72727275f)
				{
					value -= 0.54545456f;
					return 15.125f * value;
				}
				if (value < 0.90909094f)
				{
					value -= 0.8181818f;
					return 15.125f * value;
				}
				value -= 0.95454544f;
				return 15.125f * value;
			}

			// Token: 0x060006DD RID: 1757 RVA: 0x0001210F File Offset: 0x0001030F
			public static float InOutDerivative(float value)
			{
				value *= 2f;
				if (value <= 1f)
				{
					return Easing.Bounce.OutDerivative(1f - value);
				}
				return Easing.Bounce.OutDerivative(value - 1f);
			}

			// Token: 0x060006DE RID: 1758 RVA: 0x0001213B File Offset: 0x0001033B
			public static float In(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Bounce.In(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006DF RID: 1759 RVA: 0x00012151 File Offset: 0x00010351
			public static float Out(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Bounce.Out(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006E0 RID: 1760 RVA: 0x00012167 File Offset: 0x00010367
			public static float InOut(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Bounce.InOut(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006E1 RID: 1761 RVA: 0x0001217D File Offset: 0x0001037D
			public static float InDerivative(float start, float end, float value)
			{
				return Easing.Bounce.InDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x060006E2 RID: 1762 RVA: 0x00012190 File Offset: 0x00010390
			public static float OutDerivative(float start, float end, float value)
			{
				return Easing.Bounce.OutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x060006E3 RID: 1763 RVA: 0x000121A3 File Offset: 0x000103A3
			public static float InOutDerivative(float start, float end, float value)
			{
				return Easing.Bounce.InOutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}
		}

		// Token: 0x020000A4 RID: 164
		public static class Elastic
		{
			// Token: 0x060006E4 RID: 1764 RVA: 0x000121B8 File Offset: 0x000103B8
			public static float In(float value)
			{
				if (value == 0f)
				{
					return 0f;
				}
				if (value != 1f)
				{
					return -Mathf.Pow(2f, 10f * value - 10f) * Mathf.Sin((value * 10f - 10.75f) * 2.0943952f);
				}
				return 1f;
			}

			// Token: 0x060006E5 RID: 1765 RVA: 0x00012214 File Offset: 0x00010414
			public static float Out(float value)
			{
				if (value == 0f)
				{
					return 0f;
				}
				if (value != 1f)
				{
					return 1f + Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * -10f - 0.75f) * 2.0943952f);
				}
				return 1f;
			}

			// Token: 0x060006E6 RID: 1766 RVA: 0x00012270 File Offset: 0x00010470
			public static float InOut(float value)
			{
				if (value == 0f)
				{
					return 0f;
				}
				if (value == 0.5f)
				{
					return 0.5f;
				}
				if (value == 1f)
				{
					return 1f;
				}
				value *= 2f;
				if (value <= 1f)
				{
					return 0.5f * (-Mathf.Pow(2f, 10f * value - 10f) * Mathf.Sin((value * 10f - 10.75f) * 2.0943952f));
				}
				value -= 1f;
				return 0.5f + 0.5f * (1f + Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * -10f - 0.75f) * 2.0943952f));
			}

			// Token: 0x060006E7 RID: 1767 RVA: 0x00012338 File Offset: 0x00010538
			public static float InDerivative(float value)
			{
				return -(5f * Mathf.Pow(2f, 10f * value - 9f) * (2.0794415f * Mathf.Sin(3.1415927f * (40f * value - 43f) / 6f) + 6.2831855f * Mathf.Cos(3.1415927f * (40f * value - 43f) / 6f))) / 3f;
			}

			// Token: 0x060006E8 RID: 1768 RVA: 0x000123B4 File Offset: 0x000105B4
			public static float OutDerivative(float value)
			{
				return -(20.794415f * Mathf.Sin(6.2831855f * (10f * value - 0.75f) / 3f) - 62.831856f * Mathf.Cos(6.2831855f * (10f * value - 0.75f) / 3f)) / (3f * Mathf.Pow(2f, 10f * value));
			}

			// Token: 0x060006E9 RID: 1769 RVA: 0x00012423 File Offset: 0x00010623
			public static float InOutDerivative(float value)
			{
				value *= 2f;
				if (value <= 1f)
				{
					return Easing.Elastic.OutDerivative(1f - value);
				}
				return Easing.Elastic.OutDerivative(value - 1f);
			}

			// Token: 0x060006EA RID: 1770 RVA: 0x0001244F File Offset: 0x0001064F
			public static float In(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Elastic.In(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006EB RID: 1771 RVA: 0x00012465 File Offset: 0x00010665
			public static float Out(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Elastic.Out(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006EC RID: 1772 RVA: 0x0001247B File Offset: 0x0001067B
			public static float InOut(float start, float end, float value)
			{
				return Easing.Lerp(start, end, Easing.Elastic.InOut(Easing.UnLerp(start, end, value)));
			}

			// Token: 0x060006ED RID: 1773 RVA: 0x00012491 File Offset: 0x00010691
			public static float InDerivative(float start, float end, float value)
			{
				return Easing.Elastic.InDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x060006EE RID: 1774 RVA: 0x000124A4 File Offset: 0x000106A4
			public static float OutDerivative(float start, float end, float value)
			{
				return Easing.Elastic.OutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x060006EF RID: 1775 RVA: 0x000124B7 File Offset: 0x000106B7
			public static float InOutDerivative(float start, float end, float value)
			{
				return Easing.Elastic.InOutDerivative(Easing.UnLerp(start, end, value)) * (end - start);
			}

			// Token: 0x04000178 RID: 376
			public const float TwoThirdsPi = 2.0943952f;
		}
	}
}
