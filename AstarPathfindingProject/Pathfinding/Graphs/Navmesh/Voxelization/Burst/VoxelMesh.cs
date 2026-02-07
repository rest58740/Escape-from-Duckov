using System;
using Pathfinding.Jobs;
using Unity.Collections;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001D4 RID: 468
	public struct VoxelMesh : IArenaDisposable
	{
		// Token: 0x06000C48 RID: 3144 RVA: 0x0004961E File Offset: 0x0004781E
		void IArenaDisposable.DisposeWith(DisposeArena arena)
		{
			arena.Add<Int3>(this.verts);
			arena.Add<int>(this.tris);
			arena.Add<int>(this.areas);
		}

		// Token: 0x04000884 RID: 2180
		public NativeList<Int3> verts;

		// Token: 0x04000885 RID: 2181
		public NativeList<int> tris;

		// Token: 0x04000886 RID: 2182
		public NativeList<int> areas;
	}
}
