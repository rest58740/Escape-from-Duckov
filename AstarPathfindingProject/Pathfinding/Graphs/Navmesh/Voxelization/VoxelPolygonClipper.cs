using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Voxelization
{
	// Token: 0x020001C8 RID: 456
	internal struct VoxelPolygonClipper
	{
		// Token: 0x170001BE RID: 446
		public unsafe Vector3 this[int i]
		{
			set
			{
				*(ref this.x.FixedElementField + (IntPtr)i * 4) = value.x;
				*(ref this.y.FixedElementField + (IntPtr)i * 4) = value.y;
				*(ref this.z.FixedElementField + (IntPtr)i * 4) = value.z;
			}
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x000473BC File Offset: 0x000455BC
		public unsafe void ClipPolygonAlongX([NoAlias] ref VoxelPolygonClipper result, float multi, float offset)
		{
			int num = 0;
			float num2 = multi * *(ref this.x.FixedElementField + (IntPtr)(this.n - 1) * 4) + offset;
			int i = 0;
			int num3 = this.n - 1;
			while (i < this.n)
			{
				float num4 = multi * *(ref this.x.FixedElementField + (IntPtr)i * 4) + offset;
				bool flag = num2 >= 0f;
				bool flag2 = num4 >= 0f;
				if (flag != flag2)
				{
					float num5 = num2 / (num2 - num4);
					*(ref result.x.FixedElementField + (IntPtr)num * 4) = *(ref this.x.FixedElementField + (IntPtr)num3 * 4) + (*(ref this.x.FixedElementField + (IntPtr)i * 4) - *(ref this.x.FixedElementField + (IntPtr)num3 * 4)) * num5;
					*(ref result.y.FixedElementField + (IntPtr)num * 4) = *(ref this.y.FixedElementField + (IntPtr)num3 * 4) + (*(ref this.y.FixedElementField + (IntPtr)i * 4) - *(ref this.y.FixedElementField + (IntPtr)num3 * 4)) * num5;
					*(ref result.z.FixedElementField + (IntPtr)num * 4) = *(ref this.z.FixedElementField + (IntPtr)num3 * 4) + (*(ref this.z.FixedElementField + (IntPtr)i * 4) - *(ref this.z.FixedElementField + (IntPtr)num3 * 4)) * num5;
					num++;
				}
				if (flag2)
				{
					*(ref result.x.FixedElementField + (IntPtr)num * 4) = *(ref this.x.FixedElementField + (IntPtr)i * 4);
					*(ref result.y.FixedElementField + (IntPtr)num * 4) = *(ref this.y.FixedElementField + (IntPtr)i * 4);
					*(ref result.z.FixedElementField + (IntPtr)num * 4) = *(ref this.z.FixedElementField + (IntPtr)i * 4);
					num++;
				}
				num2 = num4;
				num3 = i;
				i++;
			}
			result.n = num;
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x000475A0 File Offset: 0x000457A0
		public unsafe void ClipPolygonAlongZWithYZ([NoAlias] ref VoxelPolygonClipper result, float multi, float offset)
		{
			int num = 0;
			Hint.Assume(this.n >= 0);
			Hint.Assume(this.n <= 8);
			float num2 = multi * *(ref this.z.FixedElementField + (IntPtr)(this.n - 1) * 4) + offset;
			int i = 0;
			int num3 = this.n - 1;
			while (i < this.n)
			{
				float num4 = multi * *(ref this.z.FixedElementField + (IntPtr)i * 4) + offset;
				bool flag = num2 >= 0f;
				bool flag2 = num4 >= 0f;
				if (flag != flag2)
				{
					float num5 = num2 / (num2 - num4);
					*(ref result.y.FixedElementField + (IntPtr)num * 4) = *(ref this.y.FixedElementField + (IntPtr)num3 * 4) + (*(ref this.y.FixedElementField + (IntPtr)i * 4) - *(ref this.y.FixedElementField + (IntPtr)num3 * 4)) * num5;
					*(ref result.z.FixedElementField + (IntPtr)num * 4) = *(ref this.z.FixedElementField + (IntPtr)num3 * 4) + (*(ref this.z.FixedElementField + (IntPtr)i * 4) - *(ref this.z.FixedElementField + (IntPtr)num3 * 4)) * num5;
					num++;
				}
				if (flag2)
				{
					*(ref result.y.FixedElementField + (IntPtr)num * 4) = *(ref this.y.FixedElementField + (IntPtr)i * 4);
					*(ref result.z.FixedElementField + (IntPtr)num * 4) = *(ref this.z.FixedElementField + (IntPtr)i * 4);
					num++;
				}
				num2 = num4;
				num3 = i;
				i++;
			}
			result.n = num;
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x0004773C File Offset: 0x0004593C
		public unsafe void ClipPolygonAlongZWithY([NoAlias] ref VoxelPolygonClipper result, float multi, float offset)
		{
			int num = 0;
			Hint.Assume(this.n >= 3);
			Hint.Assume(this.n <= 8);
			float num2 = multi * *(ref this.z.FixedElementField + (IntPtr)(this.n - 1) * 4) + offset;
			int i = 0;
			int num3 = this.n - 1;
			while (i < this.n)
			{
				float num4 = multi * *(ref this.z.FixedElementField + (IntPtr)i * 4) + offset;
				bool flag = num2 >= 0f;
				bool flag2 = num4 >= 0f;
				if (flag != flag2)
				{
					float num5 = num2 / (num2 - num4);
					*(ref result.y.FixedElementField + (IntPtr)num * 4) = *(ref this.y.FixedElementField + (IntPtr)num3 * 4) + (*(ref this.y.FixedElementField + (IntPtr)i * 4) - *(ref this.y.FixedElementField + (IntPtr)num3 * 4)) * num5;
					num++;
				}
				if (flag2)
				{
					*(ref result.y.FixedElementField + (IntPtr)num * 4) = *(ref this.y.FixedElementField + (IntPtr)i * 4);
					num++;
				}
				num2 = num4;
				num3 = i;
				i++;
			}
			result.n = num;
		}

		// Token: 0x04000851 RID: 2129
		[FixedBuffer(typeof(float), 8)]
		public VoxelPolygonClipper.<x>e__FixedBuffer x;

		// Token: 0x04000852 RID: 2130
		[FixedBuffer(typeof(float), 8)]
		public VoxelPolygonClipper.<y>e__FixedBuffer y;

		// Token: 0x04000853 RID: 2131
		[FixedBuffer(typeof(float), 8)]
		public VoxelPolygonClipper.<z>e__FixedBuffer z;

		// Token: 0x04000854 RID: 2132
		public int n;

		// Token: 0x020001C9 RID: 457
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 32)]
		public struct <x>e__FixedBuffer
		{
			// Token: 0x04000855 RID: 2133
			public float FixedElementField;
		}

		// Token: 0x020001CA RID: 458
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 32)]
		public struct <y>e__FixedBuffer
		{
			// Token: 0x04000856 RID: 2134
			public float FixedElementField;
		}

		// Token: 0x020001CB RID: 459
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 32)]
		public struct <z>e__FixedBuffer
		{
			// Token: 0x04000857 RID: 2135
			public float FixedElementField;
		}
	}
}
