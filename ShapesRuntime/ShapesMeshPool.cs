using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000079 RID: 121
	public static class ShapesMeshPool
	{
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x0001C693 File Offset: 0x0001A893
		public static int MeshCountInPool
		{
			get
			{
				return ShapesMeshPool.meshPool.Count;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x0001C69F File Offset: 0x0001A89F
		public static int MeshesAllocatedCount
		{
			get
			{
				return ShapesMeshPool.meshesAllocated;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x0001C6A6 File Offset: 0x0001A8A6
		public static int MeshCountInUse
		{
			get
			{
				return ShapesMeshPool.MeshesAllocatedCount - ShapesMeshPool.MeshCountInPool;
			}
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0001C6B4 File Offset: 0x0001A8B4
		public static Mesh GetMesh()
		{
			if (ShapesMeshPool.meshPool.Count > 0)
			{
				Mesh mesh = ShapesMeshPool.meshPool.Pop();
				mesh.Clear();
				return mesh;
			}
			ShapesMeshPool.meshesAllocated++;
			return new Mesh
			{
				name = "Pooled Mesh",
				hideFlags = HideFlags.DontSave
			};
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0001C703 File Offset: 0x0001A903
		public static void Release(Mesh m)
		{
			ShapesMeshPool.meshPool.Push(m);
		}

		// Token: 0x040002FC RID: 764
		private static int meshesAllocated = 0;

		// Token: 0x040002FD RID: 765
		private static Stack<Mesh> meshPool = new Stack<Mesh>();
	}
}
