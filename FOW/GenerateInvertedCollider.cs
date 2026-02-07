using System;
using System.Linq;
using UnityEngine;

namespace FOW
{
	// Token: 0x02000006 RID: 6
	public class GenerateInvertedCollider : MonoBehaviour
	{
		// Token: 0x06000038 RID: 56 RVA: 0x000040F0 File Offset: 0x000022F0
		public Mesh GetFlippedMesh(Mesh mesh)
		{
			return new Mesh
			{
				vertices = mesh.vertices,
				triangles = mesh.triangles,
				triangles = mesh.triangles.Reverse<int>().ToArray<int>()
			};
		}

		// Token: 0x04000072 RID: 114
		public bool IncludeChildren = true;

		// Token: 0x04000073 RID: 115
		public bool DisableOldColliders;

		// Token: 0x04000074 RID: 116
		public LayerMask LayersToFlip;
	}
}
