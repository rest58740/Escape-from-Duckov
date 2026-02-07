using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200005F RID: 95
	public struct Int3 : IEquatable<Int3>
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000FD1C File Offset: 0x0000DF1C
		public static Int3 zero
		{
			get
			{
				return default(Int3);
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000FD34 File Offset: 0x0000DF34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int3(Vector3 position)
		{
			this.x = (int)Math.Round((double)(position.x * 1000f));
			this.y = (int)Math.Round((double)(position.y * 1000f));
			this.z = (int)Math.Round((double)(position.z * 1000f));
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000FD8C File Offset: 0x0000DF8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int3(int _x, int _y, int _z)
		{
			this.x = _x;
			this.y = _y;
			this.z = _z;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000FDA3 File Offset: 0x0000DFA3
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Int3 lhs, Int3 rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000FDD1 File Offset: 0x0000DFD1
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Int3 lhs, Int3 rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000FE02 File Offset: 0x0000E002
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Int3(Vector3 ob)
		{
			return new Int3((int)Math.Round((double)(ob.x * 1000f)), (int)Math.Round((double)(ob.y * 1000f)), (int)Math.Round((double)(ob.z * 1000f)));
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000FE42 File Offset: 0x0000E042
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Vector3(Int3 ob)
		{
			return new Vector3((float)ob.x * 0.001f, (float)ob.y * 0.001f, (float)ob.z * 0.001f);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000FE70 File Offset: 0x0000E070
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator float3(Int3 ob)
		{
			return (int3)ob * 0.001f;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000FE87 File Offset: 0x0000E087
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int3(Int3 ob)
		{
			return new int3(ob.x, ob.y, ob.z);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000FEA0 File Offset: 0x0000E0A0
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int3 operator -(Int3 lhs, Int3 rhs)
		{
			lhs.x -= rhs.x;
			lhs.y -= rhs.y;
			lhs.z -= rhs.z;
			return lhs;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000FED6 File Offset: 0x0000E0D6
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int3 operator -(Int3 lhs)
		{
			lhs.x = -lhs.x;
			lhs.y = -lhs.y;
			lhs.z = -lhs.z;
			return lhs;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000FF03 File Offset: 0x0000E103
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int3 operator +(Int3 lhs, Int3 rhs)
		{
			lhs.x += rhs.x;
			lhs.y += rhs.y;
			lhs.z += rhs.z;
			return lhs;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000FF39 File Offset: 0x0000E139
		[IgnoredByDeepProfiler]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int3 operator *(Int3 lhs, int rhs)
		{
			lhs.x *= rhs;
			lhs.y *= rhs;
			lhs.z *= rhs;
			return lhs;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000FF60 File Offset: 0x0000E160
		public static Int3 operator *(Int3 lhs, float rhs)
		{
			lhs.x = (int)Math.Round((double)((float)lhs.x * rhs));
			lhs.y = (int)Math.Round((double)((float)lhs.y * rhs));
			lhs.z = (int)Math.Round((double)((float)lhs.z * rhs));
			return lhs;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000FFB4 File Offset: 0x0000E1B4
		public static Int3 operator *(Int3 lhs, double rhs)
		{
			lhs.x = (int)Math.Round((double)lhs.x * rhs);
			lhs.y = (int)Math.Round((double)lhs.y * rhs);
			lhs.z = (int)Math.Round((double)lhs.z * rhs);
			return lhs;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00010004 File Offset: 0x0000E204
		public static Int3 operator /(Int3 lhs, float rhs)
		{
			lhs.x = (int)Math.Round((double)((float)lhs.x / rhs));
			lhs.y = (int)Math.Round((double)((float)lhs.y / rhs));
			lhs.z = (int)Math.Round((double)((float)lhs.z / rhs));
			return lhs;
		}

		// Token: 0x170000A0 RID: 160
		public int this[int i]
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (i == 0)
				{
					return this.x;
				}
				if (i != 1)
				{
					return this.z;
				}
				return this.y;
			}
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				if (i == 0)
				{
					this.x = value;
					return;
				}
				if (i == 1)
				{
					this.y = value;
					return;
				}
				this.z = value;
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00010094 File Offset: 0x0000E294
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int3 Max(Int3 lhs, Int3 rhs)
		{
			return new Int3(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z));
		}

		// Token: 0x06000354 RID: 852 RVA: 0x000100CE File Offset: 0x0000E2CE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int3 Min(Int3 lhs, Int3 rhs)
		{
			return new Int3(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z));
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00010108 File Offset: 0x0000E308
		public static float Angle(Int3 lhs, Int3 rhs)
		{
			double num = (double)Int3.Dot(lhs, rhs) / ((double)lhs.magnitude * (double)rhs.magnitude);
			num = ((num < -1.0) ? -1.0 : ((num > 1.0) ? 1.0 : num));
			return (float)Math.Acos(num);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00010167 File Offset: 0x0000E367
		public static int Dot(Int3 lhs, Int3 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00010192 File Offset: 0x0000E392
		public static long DotLong(Int3 lhs, Int3 rhs)
		{
			return (long)lhs.x * (long)rhs.x + (long)lhs.y * (long)rhs.y + (long)lhs.z * (long)rhs.z;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x000101C3 File Offset: 0x0000E3C3
		public Int3 Normal2D()
		{
			return new Int3(this.z, this.y, -this.x);
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000359 RID: 857 RVA: 0x000101E0 File Offset: 0x0000E3E0
		public float magnitude
		{
			get
			{
				double num = (double)this.x;
				double num2 = (double)this.y;
				double num3 = (double)this.z;
				return (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600035A RID: 858 RVA: 0x00010214 File Offset: 0x0000E414
		public int costMagnitude
		{
			get
			{
				return (int)Math.Round((double)this.magnitude);
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600035B RID: 859 RVA: 0x00010224 File Offset: 0x0000E424
		public float sqrMagnitude
		{
			get
			{
				double num = (double)this.x;
				double num2 = (double)this.y;
				double num3 = (double)this.z;
				return (float)(num * num + num2 * num2 + num3 * num3);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600035C RID: 860 RVA: 0x00010254 File Offset: 0x0000E454
		public long sqrMagnitudeLong
		{
			get
			{
				long num = (long)this.x;
				long num2 = (long)this.y;
				long num3 = (long)this.z;
				return num * num + num2 * num2 + num3 * num3;
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00010282 File Offset: 0x0000E482
		public static implicit operator string(Int3 obj)
		{
			return obj.ToString();
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00010294 File Offset: 0x0000E494
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"( ",
				this.x.ToString(),
				", ",
				this.y.ToString(),
				", ",
				this.z.ToString(),
				")"
			});
		}

		// Token: 0x0600035F RID: 863 RVA: 0x000102F8 File Offset: 0x0000E4F8
		public override bool Equals(object obj)
		{
			if (!(obj is Int3))
			{
				return false;
			}
			Int3 @int = (Int3)obj;
			return this.x == @int.x && this.y == @int.y && this.z == @int.z;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000FDA3 File Offset: 0x0000DFA3
		public bool Equals(Int3 other)
		{
			return this.x == other.x && this.y == other.y && this.z == other.z;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00010342 File Offset: 0x0000E542
		public override int GetHashCode()
		{
			return this.x * 73856093 ^ this.y * 19349669 ^ this.z * 83492791;
		}

		// Token: 0x040001FF RID: 511
		public int x;

		// Token: 0x04000200 RID: 512
		public int y;

		// Token: 0x04000201 RID: 513
		public int z;

		// Token: 0x04000202 RID: 514
		public const int Precision = 1000;

		// Token: 0x04000203 RID: 515
		public const float FloatPrecision = 1000f;

		// Token: 0x04000204 RID: 516
		public const float PrecisionFactor = 0.001f;
	}
}
