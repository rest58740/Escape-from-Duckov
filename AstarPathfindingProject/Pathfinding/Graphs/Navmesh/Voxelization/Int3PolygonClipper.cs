using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Pathfinding.Collections;

namespace Pathfinding.Graphs.Navmesh.Voxelization
{
	// Token: 0x020001C6 RID: 454
	internal struct Int3PolygonClipper
	{
		// Token: 0x06000C26 RID: 3110 RVA: 0x00047228 File Offset: 0x00045428
		public unsafe int ClipPolygon(UnsafeSpan<Int3> vIn, int n, UnsafeSpan<Int3> vOut, int multi, int offset, int axis)
		{
			for (int i = 0; i < n; i++)
			{
				*(ref this.clipPolygonCache.FixedElementField + (IntPtr)i * 4) = (float)(multi * vIn[i][axis] + offset);
			}
			int num = 0;
			int j = 0;
			int num2 = n - 1;
			while (j < n)
			{
				bool flag = *(ref this.clipPolygonCache.FixedElementField + (IntPtr)num2 * 4) >= 0f;
				bool flag2 = *(ref this.clipPolygonCache.FixedElementField + (IntPtr)j * 4) >= 0f;
				if (flag != flag2)
				{
					double rhs = (double)(*(ref this.clipPolygonCache.FixedElementField + (IntPtr)num2 * 4)) / (double)(*(ref this.clipPolygonCache.FixedElementField + (IntPtr)num2 * 4) - *(ref this.clipPolygonCache.FixedElementField + (IntPtr)j * 4));
					*vOut[num] = *vIn[num2] + (*vIn[j] - *vIn[num2]) * rhs;
					num++;
				}
				if (flag2)
				{
					*vOut[num] = *vIn[j];
					num++;
				}
				num2 = j;
				j++;
			}
			return num;
		}

		// Token: 0x0400084F RID: 2127
		[FixedBuffer(typeof(float), 21)]
		private Int3PolygonClipper.<clipPolygonCache>e__FixedBuffer clipPolygonCache;

		// Token: 0x020001C7 RID: 455
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 84)]
		public struct <clipPolygonCache>e__FixedBuffer
		{
			// Token: 0x04000850 RID: 2128
			public float FixedElementField;
		}
	}
}
