using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000021 RID: 33
	[NullableContext(1)]
	[Nullable(0)]
	public class ReuseableDataContainer64
	{
		// Token: 0x060000CF RID: 207 RVA: 0x00005F54 File Offset: 0x00004154
		public ReuseableDataContainer64()
		{
			this._minimaList = new List<LocalMinima>();
			this._vertexList = new List<Vertex>();
			this._vertexPool = default(VertexPool);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005F80 File Offset: 0x00004180
		public void Clear()
		{
			foreach (Vertex v in this._vertexList)
			{
				this._vertexPool.Pool(v);
			}
			this._minimaList.Clear();
			this._vertexList.Clear();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005FF4 File Offset: 0x000041F4
		public void AddPaths(List<List<Point64>> paths, PathType pt, bool isOpen)
		{
			ClipperEngine.AddPathsToVertexList(paths, pt, isOpen, this._minimaList, this._vertexList, this._vertexPool);
		}

		// Token: 0x0400006E RID: 110
		internal readonly List<LocalMinima> _minimaList;

		// Token: 0x0400006F RID: 111
		internal readonly List<Vertex> _vertexList;

		// Token: 0x04000070 RID: 112
		internal readonly VertexPool _vertexPool;
	}
}
