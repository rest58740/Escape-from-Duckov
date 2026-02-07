using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200003A RID: 58
	public static class Mathw
	{
		// Token: 0x0600012B RID: 299 RVA: 0x0000B3D8 File Offset: 0x000095D8
		public static Vector3 Clamp(Vector3 v, float min, float max)
		{
			if (v.x < min)
			{
				v.x = min;
			}
			else if (v.x > max)
			{
				v.x = max;
			}
			if (v.y < min)
			{
				v.y = min;
			}
			else if (v.y > max)
			{
				v.y = max;
			}
			if (v.z < min)
			{
				v.z = min;
			}
			else if (v.z > max)
			{
				v.z = max;
			}
			return v;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000B452 File Offset: 0x00009652
		public static Vector3 FloatToVector3(float v)
		{
			return new Vector3(v, v, v);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000B45C File Offset: 0x0000965C
		public static float SinDeg(float angle)
		{
			return Mathf.Sin(angle * 0.017453292f);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000B46C File Offset: 0x0000966C
		public static float GetMax(Vector3 v)
		{
			float num = v.x;
			if (v.y > num)
			{
				num = v.y;
			}
			if (v.z > num)
			{
				num = v.z;
			}
			return num;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000B4A1 File Offset: 0x000096A1
		public static Vector3 SetMin(Vector3 v, float min)
		{
			if (v.x < min)
			{
				v.x = min;
			}
			if (v.y < min)
			{
				v.y = min;
			}
			if (v.z < min)
			{
				v.z = min;
			}
			return v;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000B4D8 File Offset: 0x000096D8
		public static Vector3 Snap(Vector3 v, float snapSize)
		{
			v.x = Mathf.Floor(v.x / snapSize) * snapSize;
			v.y = Mathf.Floor(v.y / snapSize) * snapSize;
			v.z = Mathf.Floor(v.z / snapSize) * snapSize;
			return v;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000B528 File Offset: 0x00009728
		public static Vector3 SnapRound(Vector3 v, float snapSize)
		{
			v.x = Mathf.Round(v.x / snapSize) * snapSize;
			v.y = Mathf.Round(v.y / snapSize) * snapSize;
			v.z = Mathf.Round(v.z / snapSize) * snapSize;
			return v;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000B578 File Offset: 0x00009778
		public static Vector3 Divide(Vector3 a, Vector3 b)
		{
			a.x /= b.x;
			a.y /= b.y;
			a.z /= b.z;
			return a;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000B5AE File Offset: 0x000097AE
		public static Vector3 Divide(float a, Vector3 b)
		{
			b.x = a / b.x;
			b.y = a / b.y;
			b.z = a / b.z;
			return b;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000B5DE File Offset: 0x000097DE
		public static Vector3 Scale(Vector3 a, Int3 b)
		{
			a.x *= (float)b.x;
			a.y *= (float)b.y;
			a.z *= (float)b.z;
			return a;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000B618 File Offset: 0x00009818
		public static Vector3 Abs(Vector3 v)
		{
			return new Vector3((v.x < 0f) ? (-v.x) : v.x, (v.y < 0f) ? (-v.y) : v.y, (v.z < 0f) ? (-v.z) : v.z);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000B680 File Offset: 0x00009880
		public static bool IntersectAABB3Sphere3(AABB3 box, Sphere3 sphere)
		{
			Vector3 center = sphere.center;
			Vector3 min = box.min;
			Vector3 max = box.max;
			float num = 0f;
			if (center.x < min.x)
			{
				float num2 = center.x - min.x;
				num += num2 * num2;
			}
			else if (center.x > max.x)
			{
				float num2 = center.x - max.x;
				num += num2 * num2;
			}
			if (center.y < min.y)
			{
				float num2 = center.y - min.y;
				num += num2 * num2;
			}
			else if (center.y > max.y)
			{
				float num2 = center.y - max.y;
				num += num2 * num2;
			}
			if (center.z < min.z)
			{
				float num2 = center.z - min.z;
				num += num2 * num2;
			}
			else if (center.z > max.z)
			{
				float num2 = center.z - max.z;
				num += num2 * num2;
			}
			return num <= sphere.radius * sphere.radius;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000B7A0 File Offset: 0x000099A0
		public static bool IntersectAABB3Triangle3(Vector3 boxCenter, Vector3 boxHalfSize, Triangle3 triangle)
		{
			Vector3 vector = triangle.a - boxCenter;
			Vector3 vector2 = triangle.b - boxCenter;
			Vector3 vector3 = triangle.c - boxCenter;
			Vector3 lhs = vector2 - vector;
			Vector3 rhs = vector3 - vector2;
			Vector3 vector4 = vector - vector3;
			float fb = Mathw.Abs(lhs[0]);
			float num = Mathw.Abs(lhs[1]);
			float fa = Mathw.Abs(lhs[2]);
			float num2;
			float num3;
			if (!Mathw.AxisTest_X01(vector, vector3, boxHalfSize, lhs[2], lhs[1], fa, num, out num2, out num3))
			{
				return false;
			}
			if (!Mathw.AxisTest_Y02(vector, vector3, boxHalfSize, lhs[2], lhs[0], fa, fb, out num2, out num3))
			{
				return false;
			}
			if (!Mathw.AxisTest_Z12(vector2, vector3, boxHalfSize, lhs[1], lhs[0], num, fb, out num2, out num3))
			{
				return false;
			}
			fb = Mathw.Abs(rhs[0]);
			num = Mathw.Abs(rhs[1]);
			fa = Mathw.Abs(rhs[2]);
			if (!Mathw.AxisTest_X01(vector, vector3, boxHalfSize, rhs[2], rhs[1], fa, num, out num2, out num3))
			{
				return false;
			}
			if (!Mathw.AxisTest_Y02(vector, vector3, boxHalfSize, rhs[2], rhs[0], fa, fb, out num2, out num3))
			{
				return false;
			}
			if (!Mathw.AxisTest_Z0(vector, vector2, boxHalfSize, rhs[1], rhs[0], num, fb, out num2, out num3))
			{
				return false;
			}
			fb = Mathw.Abs(vector4[0]);
			num = Mathw.Abs(vector4[1]);
			fa = Mathw.Abs(vector4[2]);
			if (!Mathw.AxisTest_X2(vector, vector2, boxHalfSize, vector4[2], vector4[1], fa, num, out num2, out num3))
			{
				return false;
			}
			if (!Mathw.AxisTest_Y1(vector, vector2, boxHalfSize, vector4[2], vector4[0], fa, fb, out num2, out num3))
			{
				return false;
			}
			if (!Mathw.AxisTest_Z12(vector2, vector3, boxHalfSize, vector4[1], vector4[0], num, fb, out num2, out num3))
			{
				return false;
			}
			Mathw.GetMinMax(vector[0], vector2[0], vector3[0], out num2, out num3);
			if (num2 > boxHalfSize[0] || num3 < -boxHalfSize[0])
			{
				return false;
			}
			Mathw.GetMinMax(vector[1], vector2[1], vector3[1], out num2, out num3);
			if (num2 > boxHalfSize[1] || num3 < -boxHalfSize[1])
			{
				return false;
			}
			Mathw.GetMinMax(vector[2], vector2[2], vector3[2], out num2, out num3);
			return num2 <= boxHalfSize[2] && num3 >= -boxHalfSize[2] && Mathw.PlaneBoxOverlap(Vector3.Cross(lhs, rhs), vector, boxHalfSize);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000BA80 File Offset: 0x00009C80
		private static void GetMinMax(float x0, float x1, float x2, out float min, out float max)
		{
			max = x0;
			min = x0;
			if (x1 < min)
			{
				min = x1;
			}
			else if (x1 > max)
			{
				max = x1;
			}
			if (x2 < min)
			{
				min = x2;
				return;
			}
			if (x2 > max)
			{
				max = x2;
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000BAC0 File Offset: 0x00009CC0
		private static bool PlaneBoxOverlap(Vector3 normal, Vector3 vert, Vector3 maxBox)
		{
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			for (int i = 0; i <= 2; i++)
			{
				float num = vert[i];
				if (normal[i] > 0f)
				{
					zero[i] = -maxBox[i] - num;
					zero2[i] = maxBox[i] - num;
				}
				else
				{
					zero[i] = maxBox[i] - num;
					zero2[i] = -maxBox[i] - num;
				}
			}
			return Vector3.Dot(normal, zero) <= 0f && Vector3.Dot(normal, zero2) >= 0f;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000BB6A File Offset: 0x00009D6A
		private static float Abs(float v)
		{
			if (v >= 0f)
			{
				return v;
			}
			return -v;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000BB78 File Offset: 0x00009D78
		private static bool AxisTest_X01(Vector3 v0, Vector3 v2, Vector3 boxHalfSize, float a, float b, float fa, float fb, out float min, out float max)
		{
			float num = a * v0[1] - b * v0[2];
			float num2 = a * v2[1] - b * v2[2];
			if (num < num2)
			{
				min = num;
				max = num2;
			}
			else
			{
				min = num2;
				max = num;
			}
			float num3 = fa * boxHalfSize[1] + fb * boxHalfSize[2];
			return min <= num3 && max >= -num3;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000BBF4 File Offset: 0x00009DF4
		private static bool AxisTest_X2(Vector3 v0, Vector3 v1, Vector3 boxHalfSize, float a, float b, float fa, float fb, out float min, out float max)
		{
			float num = a * v0[1] - b * v0[2];
			float num2 = a * v1[1] - b * v1[2];
			if (num < num2)
			{
				min = num;
				max = num2;
			}
			else
			{
				min = num2;
				max = num;
			}
			float num3 = fa * boxHalfSize[1] + fb * boxHalfSize[2];
			return min <= num3 && max >= -num3;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000BC70 File Offset: 0x00009E70
		private static bool AxisTest_Y02(Vector3 v0, Vector3 v2, Vector3 boxHalfSize, float a, float b, float fa, float fb, out float min, out float max)
		{
			float num = -a * v0[0] + b * v0[2];
			float num2 = -a * v2[0] + b * v2[2];
			if (num < num2)
			{
				min = num;
				max = num2;
			}
			else
			{
				min = num2;
				max = num;
			}
			float num3 = fa * boxHalfSize[0] + fb * boxHalfSize[2];
			return min <= num3 && max >= -num3;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000BCEC File Offset: 0x00009EEC
		private static bool AxisTest_Y1(Vector3 v0, Vector3 v1, Vector3 boxHalfSize, float a, float b, float fa, float fb, out float min, out float max)
		{
			float num = -a * v0[0] + b * v0[2];
			float num2 = -a * v1[0] + b * v1[2];
			if (num < num2)
			{
				min = num;
				max = num2;
			}
			else
			{
				min = num2;
				max = num;
			}
			float num3 = fa * boxHalfSize[0] + fb * boxHalfSize[2];
			return min <= num3 && max >= -num3;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000BD68 File Offset: 0x00009F68
		private static bool AxisTest_Z12(Vector3 v1, Vector3 v2, Vector3 boxHalfSize, float a, float b, float fa, float fb, out float min, out float max)
		{
			float num = a * v1[0] - b * v1[1];
			float num2 = a * v2[0] - b * v2[1];
			if (num2 < num)
			{
				min = num2;
				max = num;
			}
			else
			{
				min = num;
				max = num2;
			}
			float num3 = fa * boxHalfSize[0] + fb * boxHalfSize[1];
			return min <= num3 && max >= -num3;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000BDE4 File Offset: 0x00009FE4
		private static bool AxisTest_Z0(Vector3 v0, Vector3 v1, Vector3 boxHalfSize, float a, float b, float fa, float fb, out float min, out float max)
		{
			float num = a * v0[0] - b * v0[1];
			float num2 = a * v1[0] - b * v1[1];
			if (num < num2)
			{
				min = num;
				max = num2;
			}
			else
			{
				min = num2;
				max = num;
			}
			float num3 = fa * boxHalfSize[0] + fb * boxHalfSize[1];
			return min <= num3 && max >= -num3;
		}

		// Token: 0x04000156 RID: 342
		public static readonly int[] bits = new int[]
		{
			1,
			2,
			4,
			8,
			16,
			32,
			64,
			128,
			256,
			512,
			1024,
			2048,
			4096,
			8192,
			16384,
			32768,
			65536,
			131072,
			262144,
			524288,
			1048576,
			2097152,
			4194304,
			8388608,
			16777216,
			33554432,
			67108864,
			134217728,
			268435456,
			536870912,
			1073741824,
			int.MinValue
		};
	}
}
