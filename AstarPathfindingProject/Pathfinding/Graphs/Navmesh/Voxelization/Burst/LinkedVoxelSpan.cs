using System;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001D1 RID: 465
	public struct LinkedVoxelSpan
	{
		// Token: 0x06000C3C RID: 3132 RVA: 0x0004809E File Offset: 0x0004629E
		public LinkedVoxelSpan(uint bottom, uint top, int area)
		{
			this.bottom = bottom;
			this.top = top;
			this.area = area;
			this.next = -1;
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x000480BC File Offset: 0x000462BC
		public LinkedVoxelSpan(uint bottom, uint top, int area, int next)
		{
			this.bottom = bottom;
			this.top = top;
			this.area = area;
			this.next = next;
		}

		// Token: 0x04000875 RID: 2165
		public uint bottom;

		// Token: 0x04000876 RID: 2166
		public uint top;

		// Token: 0x04000877 RID: 2167
		public int next;

		// Token: 0x04000878 RID: 2168
		public int area;
	}
}
