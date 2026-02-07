using System;
using Pathfinding.Collections;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Profiling;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001A0 RID: 416
	public class NavmeshTile : INavmeshHolder, ITransformedGraph
	{
		// Token: 0x06000B52 RID: 2898 RVA: 0x0003FEC7 File Offset: 0x0003E0C7
		public void GetTileCoordinates(int tileIndex, out int x, out int z)
		{
			x = this.x;
			z = this.z;
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x00022E0A File Offset: 0x0002100A
		public int GetVertexArrayIndex(int index)
		{
			return index & 4095;
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x0003FEDC File Offset: 0x0003E0DC
		public unsafe Int3 GetVertex(int index)
		{
			int index2 = index & 4095;
			return *this.verts[index2];
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0003FF02 File Offset: 0x0003E102
		[IgnoredByDeepProfiler]
		public unsafe Int3 GetVertexInGraphSpace(int index)
		{
			return *this.vertsInGraphSpace[index & 4095];
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000B56 RID: 2902 RVA: 0x0003FF1B File Offset: 0x0003E11B
		public GraphTransform transform
		{
			get
			{
				return this.graph.transform;
			}
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x0003FF28 File Offset: 0x0003E128
		public void GetNodes(Action<GraphNode> action)
		{
			if (this.nodes == null)
			{
				return;
			}
			for (int i = 0; i < this.nodes.Length; i++)
			{
				action(this.nodes[i]);
			}
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0003FF60 File Offset: 0x0003E160
		public void Dispose()
		{
			this.bbTree.Dispose();
			this.vertsInGraphSpace.Free(Allocator.Persistent);
			this.verts.Free(Allocator.Persistent);
			this.tris.Free(Allocator.Persistent);
			this.preCutTags.Free(Allocator.Persistent);
			this.preCutVertsInTileSpace.Free(Allocator.Persistent);
			this.preCutTris.Free(Allocator.Persistent);
			this.vertsInGraphSpace = default(UnsafeSpan<Int3>);
			this.verts = default(UnsafeSpan<Int3>);
			this.tris = default(UnsafeSpan<int>);
			this.preCutTags = default(UnsafeSpan<uint>);
			this.preCutVertsInTileSpace = default(UnsafeSpan<Int3>);
			this.preCutTris = default(UnsafeSpan<int>);
			this.isCut = false;
		}

		// Token: 0x040007B0 RID: 1968
		public UnsafeSpan<Int3> vertsInGraphSpace;

		// Token: 0x040007B1 RID: 1969
		public UnsafeSpan<Int3> verts;

		// Token: 0x040007B2 RID: 1970
		public UnsafeSpan<int> tris;

		// Token: 0x040007B3 RID: 1971
		public bool isCut;

		// Token: 0x040007B4 RID: 1972
		public UnsafeSpan<Int3> preCutVertsInTileSpace;

		// Token: 0x040007B5 RID: 1973
		public UnsafeSpan<int> preCutTris;

		// Token: 0x040007B6 RID: 1974
		public UnsafeSpan<uint> preCutTags;

		// Token: 0x040007B7 RID: 1975
		public int x;

		// Token: 0x040007B8 RID: 1976
		public int z;

		// Token: 0x040007B9 RID: 1977
		public int w;

		// Token: 0x040007BA RID: 1978
		public int d;

		// Token: 0x040007BB RID: 1979
		public TriangleMeshNode[] nodes;

		// Token: 0x040007BC RID: 1980
		public BBTree bbTree;

		// Token: 0x040007BD RID: 1981
		public bool flag;

		// Token: 0x040007BE RID: 1982
		public NavmeshBase graph;
	}
}
