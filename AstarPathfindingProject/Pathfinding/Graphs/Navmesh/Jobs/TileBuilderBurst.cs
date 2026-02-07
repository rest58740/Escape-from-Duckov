using System;
using Pathfinding.Graphs.Navmesh.Voxelization.Burst;
using Pathfinding.Jobs;
using Unity.Collections;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001E5 RID: 485
	public struct TileBuilderBurst : IArenaDisposable
	{
		// Token: 0x06000C75 RID: 3189 RVA: 0x0004CF5C File Offset: 0x0004B15C
		public TileBuilderBurst(int width, int depth, int voxelWalkableHeight, int maximumVoxelYCoord)
		{
			this.linkedVoxelField = new LinkedVoxelField(width, depth, maximumVoxelYCoord);
			this.compactVoxelField = new CompactVoxelField(width, depth, voxelWalkableHeight, Allocator.Persistent);
			this.tmpQueue1 = new NativeQueue<Int3>(Allocator.Persistent);
			this.tmpQueue2 = new NativeQueue<Int3>(Allocator.Persistent);
			this.distanceField = new NativeList<ushort>(0, Allocator.Persistent);
			this.contours = new NativeList<VoxelContour>(Allocator.Persistent);
			this.contourVertices = new NativeList<int>(Allocator.Persistent);
			this.voxelMesh = new VoxelMesh
			{
				verts = new NativeList<Int3>(Allocator.Persistent),
				tris = new NativeList<int>(Allocator.Persistent),
				areas = new NativeList<int>(Allocator.Persistent)
			};
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x0004D024 File Offset: 0x0004B224
		void IArenaDisposable.DisposeWith(DisposeArena arena)
		{
			arena.Add<LinkedVoxelField>(this.linkedVoxelField);
			arena.Add<CompactVoxelField>(this.compactVoxelField);
			arena.Add<ushort>(this.distanceField);
			arena.Add<Int3>(this.tmpQueue1);
			arena.Add<Int3>(this.tmpQueue2);
			arena.Add<VoxelContour>(this.contours);
			arena.Add<int>(this.contourVertices);
			arena.Add<VoxelMesh>(this.voxelMesh);
		}

		// Token: 0x040008D9 RID: 2265
		public LinkedVoxelField linkedVoxelField;

		// Token: 0x040008DA RID: 2266
		public CompactVoxelField compactVoxelField;

		// Token: 0x040008DB RID: 2267
		public NativeList<ushort> distanceField;

		// Token: 0x040008DC RID: 2268
		public NativeQueue<Int3> tmpQueue1;

		// Token: 0x040008DD RID: 2269
		public NativeQueue<Int3> tmpQueue2;

		// Token: 0x040008DE RID: 2270
		public NativeList<VoxelContour> contours;

		// Token: 0x040008DF RID: 2271
		public NativeList<int> contourVertices;

		// Token: 0x040008E0 RID: 2272
		public VoxelMesh voxelMesh;
	}
}
