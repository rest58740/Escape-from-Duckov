using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200001B RID: 27
	public struct MeshColliderAdd
	{
		// Token: 0x06000090 RID: 144 RVA: 0x0000715E File Offset: 0x0000535E
		public MeshColliderAdd(GameObject go, Mesh mesh)
		{
			this.go = go;
			this.mesh = mesh;
		}

		// Token: 0x040000E2 RID: 226
		public GameObject go;

		// Token: 0x040000E3 RID: 227
		public Mesh mesh;
	}
}
