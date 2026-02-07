using System;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200004F RID: 79
	public static class AstarMath
	{
		// Token: 0x060002D5 RID: 725 RVA: 0x0000D7EC File Offset: 0x0000B9EC
		public static float ThreadSafeRandomFloat()
		{
			object globalRandomLock = AstarMath.GlobalRandomLock;
			float result;
			lock (globalRandomLock)
			{
				result = AstarMath.GlobalRandom.NextFloat();
			}
			return result;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000D834 File Offset: 0x0000BA34
		public static float2 ThreadSafeRandomFloat2()
		{
			object globalRandomLock = AstarMath.GlobalRandomLock;
			float2 result;
			lock (globalRandomLock)
			{
				result = AstarMath.GlobalRandom.NextFloat2();
			}
			return result;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000D87C File Offset: 0x0000BA7C
		public static long SaturatingConvertFloatToLong(float v)
		{
			if (v <= 9.223372E+18f)
			{
				return (long)v;
			}
			return long.MaxValue;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000D892 File Offset: 0x0000BA92
		public static float MapTo(float startMin, float startMax, float targetMin, float targetMax, float value)
		{
			return Mathf.Lerp(targetMin, targetMax, Mathf.InverseLerp(startMin, startMax, value));
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000D8A4 File Offset: 0x0000BAA4
		private static int Bit(int a, int b)
		{
			return a >> b & 1;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000D8B0 File Offset: 0x0000BAB0
		public static Color IntToColor(int i, float a)
		{
			float num = (float)(AstarMath.Bit(i, 2) + AstarMath.Bit(i, 3) * 2 + 1);
			int num2 = AstarMath.Bit(i, 1) + AstarMath.Bit(i, 4) * 2 + 1;
			int num3 = AstarMath.Bit(i, 0) + AstarMath.Bit(i, 5) * 2 + 1;
			return new Color(num * 0.25f, (float)num2 * 0.25f, (float)num3 * 0.25f, a);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000D918 File Offset: 0x0000BB18
		public static Color HSVToRGB(float h, float s, float v)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = s * v;
			float num5 = h / 60f;
			float num6 = num4 * (1f - Math.Abs(num5 % 2f - 1f));
			if (num5 < 1f)
			{
				num = num4;
				num2 = num6;
			}
			else if (num5 < 2f)
			{
				num = num6;
				num2 = num4;
			}
			else if (num5 < 3f)
			{
				num2 = num4;
				num3 = num6;
			}
			else if (num5 < 4f)
			{
				num2 = num6;
				num3 = num4;
			}
			else if (num5 < 5f)
			{
				num = num6;
				num3 = num4;
			}
			else if (num5 < 6f)
			{
				num = num4;
				num3 = num6;
			}
			float num7 = v - num4;
			num += num7;
			num2 += num7;
			num3 += num7;
			return new Color(num, num2, num3);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000D9DC File Offset: 0x0000BBDC
		public static float DeltaAngle(float angle1, float angle2)
		{
			float num = (angle2 - angle1 + 3.1415927f) % 6.2831855f - 3.1415927f;
			return math.select(num, num + 6.2831855f, num < -3.1415927f);
		}

		// Token: 0x040001DF RID: 479
		private static Unity.Mathematics.Random GlobalRandom = Unity.Mathematics.Random.CreateFromIndex(0U);

		// Token: 0x040001E0 RID: 480
		private static object GlobalRandomLock = new object();
	}
}
