using System;
using Unity.Mathematics;

namespace Pathfinding
{
	// Token: 0x020000B0 RID: 176
	public struct PathNode
	{
		// Token: 0x06000591 RID: 1425 RVA: 0x0001B139 File Offset: 0x00019339
		public static uint ReverseFractionAlongEdge(uint v)
		{
			return 15U - v;
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001B13F File Offset: 0x0001933F
		public static uint QuantizeFractionAlongEdge(float v)
		{
			v *= 15f;
			v += 0.5f;
			return math.clamp((uint)v, 0U, 15U);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001B15D File Offset: 0x0001935D
		public static float UnQuantizeFractionAlongEdge(uint v)
		{
			return v * 0.06666667f;
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x0001B168 File Offset: 0x00019368
		// (set) Token: 0x06000595 RID: 1429 RVA: 0x0001B179 File Offset: 0x00019379
		public uint fractionAlongEdge
		{
			get
			{
				return (this.flags & 1006632960U) >> 26;
			}
			set
			{
				this.flags = ((this.flags & 3288334335U) | (value << 26 & 1006632960U));
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x0001B198 File Offset: 0x00019398
		// (set) Token: 0x06000597 RID: 1431 RVA: 0x0001B1A6 File Offset: 0x000193A6
		public uint parentIndex
		{
			get
			{
				return this.flags & 67108863U;
			}
			set
			{
				this.flags = ((this.flags & 4227858432U) | value);
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x0001B1BC File Offset: 0x000193BC
		// (set) Token: 0x06000599 RID: 1433 RVA: 0x0001B1CD File Offset: 0x000193CD
		public bool flag1
		{
			get
			{
				return (this.flags & 1073741824U) > 0U;
			}
			set
			{
				this.flags = ((this.flags & 3221225471U) | (value ? 1073741824U : 0U));
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x0001B1ED File Offset: 0x000193ED
		// (set) Token: 0x0600059B RID: 1435 RVA: 0x0001B1FE File Offset: 0x000193FE
		public bool flag2
		{
			get
			{
				return (this.flags & 2147483648U) > 0U;
			}
			set
			{
				this.flags = ((this.flags & 2147483647U) | (value ? 2147483648U : 0U));
			}
		}

		// Token: 0x040003A7 RID: 935
		public ushort pathID;

		// Token: 0x040003A8 RID: 936
		public ushort heapIndex;

		// Token: 0x040003A9 RID: 937
		private uint flags;

		// Token: 0x040003AA RID: 938
		public static readonly PathNode Default = new PathNode
		{
			pathID = 0,
			heapIndex = ushort.MaxValue,
			flags = 0U
		};

		// Token: 0x040003AB RID: 939
		private const uint ParentIndexMask = 67108863U;

		// Token: 0x040003AC RID: 940
		private const int FractionAlongEdgeOffset = 26;

		// Token: 0x040003AD RID: 941
		private const uint FractionAlongEdgeMask = 1006632960U;

		// Token: 0x040003AE RID: 942
		public const int FractionAlongEdgeQuantization = 16;

		// Token: 0x040003AF RID: 943
		private const int Flag1Offset = 30;

		// Token: 0x040003B0 RID: 944
		private const uint Flag1Mask = 1073741824U;

		// Token: 0x040003B1 RID: 945
		private const int Flag2Offset = 31;

		// Token: 0x040003B2 RID: 946
		private const uint Flag2Mask = 2147483648U;
	}
}
