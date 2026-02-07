using System;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001CE RID: 462
	public struct CompactVoxelSpan
	{
		// Token: 0x06000C30 RID: 3120 RVA: 0x00047AAF File Offset: 0x00045CAF
		public CompactVoxelSpan(ushort bottom, uint height)
		{
			this.con = 24U;
			this.y = bottom;
			this.h = height;
			this.reg = 0;
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x00047AD0 File Offset: 0x00045CD0
		public void SetConnection(int dir, uint value)
		{
			int num = dir * 6;
			this.con = (uint)(((ulong)this.con & (ulong)(~(63L << (num & 31)))) | (ulong)((ulong)(value & 63U) << num));
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x00047B04 File Offset: 0x00045D04
		public int GetConnection(int dir)
		{
			return (int)this.con >> dir * 6 & 63;
		}

		// Token: 0x04000863 RID: 2147
		public ushort y;

		// Token: 0x04000864 RID: 2148
		public uint con;

		// Token: 0x04000865 RID: 2149
		public uint h;

		// Token: 0x04000866 RID: 2150
		public int reg;
	}
}
