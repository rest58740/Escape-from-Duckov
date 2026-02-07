using System;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001CD RID: 461
	public struct CompactVoxelCell
	{
		// Token: 0x06000C2F RID: 3119 RVA: 0x00047A9F File Offset: 0x00045C9F
		public CompactVoxelCell(int i, int c)
		{
			this.index = i;
			this.count = c;
		}

		// Token: 0x04000861 RID: 2145
		public int index;

		// Token: 0x04000862 RID: 2146
		public int count;
	}
}
