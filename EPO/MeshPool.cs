using System;
using System.Collections.Generic;
using UnityEngine;

namespace EPOOutline
{
	// Token: 0x0200000F RID: 15
	public class MeshPool : IDisposable
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00003174 File Offset: 0x00001374
		public Mesh AllocateMesh()
		{
			while (this.freeMeshes.Count > 0 && this.freeMeshes.Peek() == null)
			{
				this.freeMeshes.Dequeue();
			}
			if (this.freeMeshes.Count == 0)
			{
				Mesh mesh = new Mesh();
				mesh.MarkDynamic();
				this.allMeshes.Add(mesh);
				this.freeMeshes.Enqueue(mesh);
			}
			return this.freeMeshes.Dequeue();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000031EC File Offset: 0x000013EC
		public void ReleaseAllMeshes()
		{
			this.freeMeshes.Clear();
			foreach (Mesh item in this.allMeshes)
			{
				this.freeMeshes.Enqueue(item);
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003250 File Offset: 0x00001450
		public void Dispose()
		{
			foreach (Mesh obj in this.allMeshes)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			this.allMeshes.Clear();
			this.freeMeshes.Clear();
		}

		// Token: 0x04000038 RID: 56
		private Queue<Mesh> freeMeshes = new Queue<Mesh>();

		// Token: 0x04000039 RID: 57
		private List<Mesh> allMeshes = new List<Mesh>();
	}
}
