using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace UI_Spline_Renderer
{
	// Token: 0x02000009 RID: 9
	public static class CurveSampling
	{
		// Token: 0x0400000E RID: 14
		private const float DefaultWeight = 0f;

		// Token: 0x0200000A RID: 10
		public static class ThreadSafe
		{
			// Token: 0x06000027 RID: 39 RVA: 0x00002967 File Offset: 0x00000B67
			public static float Evaluate(NativeArray<Keyframe> keys, float curveT)
			{
				return CurveSampling.ThreadSafe.EvaluateWithinRange(keys, curveT, 0, keys.Length - 1);
			}

			// Token: 0x06000028 RID: 40 RVA: 0x0000297C File Offset: 0x00000B7C
			public static float EvaluateWithHint(NativeArray<Keyframe> keys, float curveT, ref int hintIndex)
			{
				int start = 0;
				int num = keys.Length - 1;
				if (num <= hintIndex)
				{
					return keys[hintIndex].value;
				}
				curveT = math.clamp(curveT, keys[hintIndex].time, keys[num].time);
				int num2;
				int num3;
				CurveSampling.ThreadSafe.FindIndexForSampling(keys, curveT, start, num, hintIndex, out num2, out num3);
				Keyframe lhs = keys[hintIndex];
				Keyframe rhs = keys[num3];
				return CurveSampling.ThreadSafe.InterpolateKeyframe(lhs, rhs, curveT);
			}

			// Token: 0x06000029 RID: 41 RVA: 0x00002A04 File Offset: 0x00000C04
			public static float EvaluateWithinRange(NativeArray<Keyframe> keys, float curveT, int startIndex, int endIndex)
			{
				if (endIndex <= startIndex)
				{
					return keys[startIndex].value;
				}
				curveT = math.clamp(curveT, keys[startIndex].time, keys[endIndex].time);
				int num;
				int num2;
				CurveSampling.ThreadSafe.FindIndexForSampling(keys, curveT, startIndex, endIndex, -1, out num, out num2);
				Keyframe lhs = keys[num];
				Keyframe rhs = keys[num2];
				return CurveSampling.ThreadSafe.InterpolateKeyframe(lhs, rhs, curveT);
			}

			// Token: 0x0600002A RID: 42 RVA: 0x00002A74 File Offset: 0x00000C74
			private static void FindIndexForSampling(NativeArray<Keyframe> keys, float curveT, int start, int end, int hint, out int lhs, out int rhs)
			{
				if (hint != -1)
				{
					hint = math.clamp(hint, start, end);
					float time = keys[hint].time;
					if (curveT > time)
					{
						for (int i = 0; i < 3; i++)
						{
							int num = hint + i;
							if (num + 1 < end && keys[num + 1].time > curveT)
							{
								lhs = num;
								rhs = math.min(lhs + 1, end);
								return;
							}
						}
					}
				}
				int j = end - start;
				int num2 = start;
				while (j > 0)
				{
					int num3 = j >> 1;
					int num4 = num2 + num3;
					if (curveT < keys[num4].time)
					{
						j = num3;
					}
					else
					{
						num2 = num4;
						num2++;
						j = j - num3 - 1;
					}
				}
				lhs = num2 - 1;
				rhs = math.min(end, num2);
			}

			// Token: 0x0600002B RID: 43 RVA: 0x00002B3C File Offset: 0x00000D3C
			public static float InterpolateKeyframe(Keyframe lhs, Keyframe rhs, float curveT)
			{
				float result;
				if ((lhs.weightedMode & 2) != null || (rhs.weightedMode & 1) != null)
				{
					result = CurveSampling.ThreadSafe.BezierInterpolate(curveT, lhs, rhs);
				}
				else
				{
					result = CurveSampling.ThreadSafe.HermiteInterpolate(curveT, lhs, rhs);
				}
				CurveSampling.ThreadSafe.HandleSteppedCurve(lhs, rhs, ref result);
				return result;
			}

			// Token: 0x0600002C RID: 44 RVA: 0x00002B80 File Offset: 0x00000D80
			private static float HermiteInterpolate(float curveT, Keyframe lhs, Keyframe rhs)
			{
				float num = rhs.time - lhs.time;
				float t;
				float m;
				float m2;
				if (num != 0f)
				{
					t = (curveT - lhs.time) / num;
					m = lhs.outTangent * num;
					m2 = rhs.inTangent * num;
				}
				else
				{
					t = 0f;
					m = 0f;
					m2 = 0f;
				}
				return CurveSampling.ThreadSafe.HermiteInterpolate(t, lhs.value, m, m2, rhs.value);
			}

			// Token: 0x0600002D RID: 45 RVA: 0x00002BF0 File Offset: 0x00000DF0
			private static float HermiteInterpolate(float t, float p0, float m0, float m1, float p1)
			{
				float num = t * t;
				float num2 = num * t;
				float num3 = 2f * num2 - 3f * num + 1f;
				float num4 = num2 - 2f * num + t;
				float num5 = num2 - num;
				float num6 = -2f * num2 + 3f * num;
				return num3 * p0 + num4 * m0 + num5 * m1 + num6 * p1;
			}

			// Token: 0x0600002E RID: 46 RVA: 0x00002C4C File Offset: 0x00000E4C
			private static float BezierInterpolate(float curveT, Keyframe lhs, Keyframe rhs)
			{
				float w = ((lhs.weightedMode & 2) != null) ? lhs.outWeight : 0f;
				float w2 = ((rhs.weightedMode & 1) != null) ? rhs.inWeight : 0f;
				float num = rhs.time - lhs.time;
				if (num == 0f)
				{
					return lhs.value;
				}
				return CurveSampling.ThreadSafe.BezierInterpolate((curveT - lhs.time) / num, lhs.value, lhs.outTangent * num, w, rhs.value, rhs.inTangent * num, w2);
			}

			// Token: 0x0600002F RID: 47 RVA: 0x00002CDF File Offset: 0x00000EDF
			private static float FAST_CBRT_POSITIVE(float x)
			{
				return math.exp(math.log(x) / 3f);
			}

			// Token: 0x06000030 RID: 48 RVA: 0x00002CF2 File Offset: 0x00000EF2
			private static float FAST_CBRT(float x)
			{
				if (x >= 0f)
				{
					return math.exp(math.log(x) / 3f);
				}
				return -math.exp(math.log(-x) / 3f);
			}

			// Token: 0x06000031 RID: 49 RVA: 0x00002D24 File Offset: 0x00000F24
			private static float BezierExtractU(float t, float w1, float w2)
			{
				float num = 3f * w1 - 3f * w2 + 1f;
				float num2 = -6f * w1 + 3f * w2;
				float num3 = 3f * w1;
				float num4 = -t;
				if (math.abs(num) > 0.001f)
				{
					float num5 = -num2 / (3f * num);
					float num6 = num5 * num5;
					float num7 = num6 * num5 + (num2 * num3 - 3f * num * num4) / (6f * num * num);
					float num8 = num7 * num7;
					float num9 = num3 / (3f * num) - num6;
					float num10 = num8 + num9 * num9 * num9;
					if (num10 < 0f)
					{
						float num11 = math.sqrt(-num10);
						float x = math.sqrt(-num10 + num8);
						float num12 = math.atan2(num11, num7);
						float num13 = CurveSampling.ThreadSafe.FAST_CBRT_POSITIVE(x);
						float num14 = num12 / 3f;
						float num15 = 2f * num13 * math.cos(num14) + num5;
						float num16 = 2f * num13 * math.cos(num14 + 2.0943952f) + num5;
						float num17 = 2f * num13 * math.cos(num14 - 2.0943952f) + num5;
						if (num15 >= 0f && num15 <= 1f)
						{
							return num15;
						}
						if (num16 >= 0f && num16 <= 1f)
						{
							return num16;
						}
						if (num17 >= 0f && num17 <= 1f)
						{
							return num17;
						}
						if (t >= 0.5f)
						{
							return 1f;
						}
						return 0f;
					}
					else
					{
						float num18 = math.sqrt(num10);
						float num19 = CurveSampling.ThreadSafe.FAST_CBRT(num7 + num18) + CurveSampling.ThreadSafe.FAST_CBRT(num7 - num18) + num5;
						if (num19 >= 0f && num19 <= 1f)
						{
							return num19;
						}
						if (t >= 0.5f)
						{
							return 1f;
						}
						return 0f;
					}
				}
				else if (math.abs(num2) > 0.001f)
				{
					float num20 = math.sqrt(num3 * num3 - 4f * num2 * num4);
					float num21 = (-num3 - num20) / (2f * num2);
					float num22 = (-num3 + num20) / (2f * num2);
					if (num21 >= 0f && num21 <= 1f)
					{
						return num21;
					}
					if (num22 >= 0f && num22 <= 1f)
					{
						return num22;
					}
					if (t >= 0.5f)
					{
						return 1f;
					}
					return 0f;
				}
				else
				{
					if (math.abs(num3) > 0.001f)
					{
						return -num4 / num3;
					}
					return 0f;
				}
			}

			// Token: 0x06000032 RID: 50 RVA: 0x00002F7D File Offset: 0x0000117D
			private static float BezierInterpolate(float t, float v1, float m1, float w1, float v2, float m2, float w2)
			{
				return CurveSampling.ThreadSafe.BezierInterpolate(CurveSampling.ThreadSafe.BezierExtractU(t, w1, 1f - w2), v1, w1 * m1 + v1, v2 - w2 * m2, v2);
			}

			// Token: 0x06000033 RID: 51 RVA: 0x00002FA4 File Offset: 0x000011A4
			private static float BezierInterpolate(float t, float p0, float p1, float p2, float p3)
			{
				float num = t * t;
				float num2 = num * t;
				float num3 = 1f - t;
				float num4 = num3 * num3;
				return num4 * num3 * p0 + 3f * t * num4 * p1 + 3f * num * num3 * p2 + num2 * p3;
			}

			// Token: 0x06000034 RID: 52 RVA: 0x00002FE7 File Offset: 0x000011E7
			private static void HandleSteppedCurve(Keyframe lhs, Keyframe rhs, ref float value)
			{
				if (float.IsInfinity(lhs.outTangent) || float.IsInfinity(rhs.inTangent))
				{
					value = lhs.value;
				}
			}
		}
	}
}
