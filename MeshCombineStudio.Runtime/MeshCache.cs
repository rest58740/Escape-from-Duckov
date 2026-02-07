using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000017 RID: 23
	public class MeshCache
	{
		// Token: 0x0600005A RID: 90 RVA: 0x00004F90 File Offset: 0x00003190
		public MeshCache(Mesh mesh)
		{
			this.mesh = mesh;
			this.subMeshCount = mesh.subMeshCount;
			this.subMeshCache = new MeshCache.SubMeshCache[this.subMeshCount];
			if (this.subMeshCount == 1)
			{
				this.subMeshCache[0] = new MeshCache.SubMeshCache(mesh, true);
				return;
			}
			MeshCache.SubMeshCache sub = new MeshCache.SubMeshCache(mesh, false);
			for (int i = 0; i < this.subMeshCache.Length; i++)
			{
				this.subMeshCache[i] = new MeshCache.SubMeshCache(mesh, i);
				this.subMeshCache[i].RebuildVertexBuffer(sub, true);
			}
		}

		// Token: 0x0400005E RID: 94
		public Mesh mesh;

		// Token: 0x0400005F RID: 95
		public MeshCache.SubMeshCache[] subMeshCache;

		// Token: 0x04000060 RID: 96
		public int subMeshCount;

		// Token: 0x02000063 RID: 99
		public class SubMeshCache
		{
			// Token: 0x060001D0 RID: 464 RVA: 0x00010E10 File Offset: 0x0000F010
			public SubMeshCache()
			{
			}

			// Token: 0x060001D1 RID: 465 RVA: 0x00010E18 File Offset: 0x0000F018
			public void CopySubMeshCache(MeshCache.SubMeshCache source)
			{
				this.vertexCount = source.vertexCount;
				Array.Copy(source.vertices, 0, this.vertices, 0, this.vertexCount);
				this.hasNormals = source.hasNormals;
				this.hasTangents = source.hasTangents;
				this.hasColors = source.hasColors;
				this.hasUv = source.hasUv;
				this.hasUv2 = source.hasUv2;
				this.hasUv3 = source.hasUv3;
				this.hasUv4 = source.hasUv4;
				if (source.hasNormals)
				{
					this.CopyArray<Vector3>(source.normals, ref this.normals, this.vertexCount);
				}
				if (source.hasTangents)
				{
					this.CopyArray<Vector4>(source.tangents, ref this.tangents, this.vertexCount);
				}
				if (source.hasUv)
				{
					this.CopyArray<Vector2>(source.uv, ref this.uv, this.vertexCount);
				}
				if (source.hasUv2)
				{
					this.CopyArray<Vector2>(source.uv2, ref this.uv2, this.vertexCount);
				}
				if (source.hasUv3)
				{
					this.CopyArray<Vector2>(source.uv3, ref this.uv3, this.vertexCount);
				}
				if (source.hasUv4)
				{
					this.CopyArray<Vector2>(source.uv4, ref this.uv4, this.vertexCount);
				}
				if (source.hasColors)
				{
					this.CopyArray<Color32>(source.colors32, ref this.colors32, this.vertexCount);
				}
			}

			// Token: 0x060001D2 RID: 466 RVA: 0x00010F7E File Offset: 0x0000F17E
			public void CopyArray<T>(Array sourceArray, ref T[] destinationArray, int vertexCount)
			{
				if (destinationArray == null)
				{
					destinationArray = new T[65534];
				}
				Array.Copy(sourceArray, 0, destinationArray, 0, vertexCount);
			}

			// Token: 0x060001D3 RID: 467 RVA: 0x00010F9B File Offset: 0x0000F19B
			public SubMeshCache(Mesh mesh, int subMeshIndex)
			{
				this.triangles = mesh.GetTriangles(subMeshIndex);
				this.triangleCount = this.triangles.Length;
			}

			// Token: 0x060001D4 RID: 468 RVA: 0x00010FC0 File Offset: 0x0000F1C0
			public SubMeshCache(Mesh mesh, bool assignTriangles)
			{
				this.vertices = mesh.vertices;
				this.normals = mesh.normals;
				this.tangents = mesh.tangents;
				this.uv = mesh.uv;
				this.uv2 = mesh.uv2;
				this.uv3 = mesh.uv3;
				this.uv4 = mesh.uv4;
				this.colors32 = mesh.colors32;
				if (assignTriangles)
				{
					this.triangles = mesh.triangles;
					this.triangleCount = this.triangles.Length;
				}
				this.CheckHasArrays();
				this.vertexCount = this.vertices.Length;
			}

			// Token: 0x060001D5 RID: 469 RVA: 0x00011064 File Offset: 0x0000F264
			public void CheckHasArrays()
			{
				if (this.normals != null && this.normals.Length != 0)
				{
					this.hasNormals = true;
				}
				if (this.tangents != null && this.tangents.Length != 0)
				{
					this.hasTangents = true;
				}
				if (this.uv != null && this.uv.Length != 0)
				{
					this.hasUv = true;
				}
				if (this.uv2 != null && this.uv2.Length != 0)
				{
					this.hasUv2 = true;
				}
				if (this.uv3 != null && this.uv3.Length != 0)
				{
					this.hasUv3 = true;
				}
				if (this.uv4 != null && this.uv4.Length != 0)
				{
					this.hasUv4 = true;
				}
				if (this.colors32 != null && this.colors32.Length != 0)
				{
					this.hasColors = true;
				}
			}

			// Token: 0x060001D6 RID: 470 RVA: 0x0001111C File Offset: 0x0000F31C
			public void ResetHasBooleans()
			{
				this.hasNormals = (this.hasTangents = (this.hasUv = (this.hasUv2 = (this.hasUv3 = (this.hasUv4 = (this.hasColors = false))))));
			}

			// Token: 0x060001D7 RID: 471 RVA: 0x00011166 File Offset: 0x0000F366
			public void Init(bool initTriangles = true)
			{
				this.vertices = new Vector3[65534];
				if (initTriangles)
				{
					this.triangles = new int[786408];
				}
			}

			// Token: 0x060001D8 RID: 472 RVA: 0x0001118C File Offset: 0x0000F38C
			public void RebuildVertexBuffer(MeshCache.SubMeshCache sub, bool resizeArrays)
			{
				int[] array = new int[sub.vertices.Length];
				int[] array2 = new int[array.Length];
				this.vertexCount = 0;
				for (int i = 0; i < this.triangleCount; i++)
				{
					int num = this.triangles[i];
					if (array[num] == 0)
					{
						array[num] = this.vertexCount + 1;
						array2[this.vertexCount] = num;
						this.triangles[i] = this.vertexCount;
						this.vertexCount++;
					}
					else
					{
						this.triangles[i] = array[num] - 1;
					}
				}
				if (resizeArrays)
				{
					this.vertices = new Vector3[this.vertexCount];
				}
				this.hasNormals = sub.hasNormals;
				this.hasTangents = sub.hasTangents;
				this.hasColors = sub.hasColors;
				this.hasUv = sub.hasUv;
				this.hasUv2 = sub.hasUv2;
				this.hasUv3 = sub.hasUv3;
				this.hasUv4 = sub.hasUv4;
				if (resizeArrays)
				{
					if (this.hasNormals)
					{
						this.normals = new Vector3[this.vertexCount];
					}
					if (this.hasTangents)
					{
						this.tangents = new Vector4[this.vertexCount];
					}
					if (this.hasUv)
					{
						this.uv = new Vector2[this.vertexCount];
					}
					if (this.hasUv2)
					{
						this.uv2 = new Vector2[this.vertexCount];
					}
					if (this.hasUv3)
					{
						this.uv3 = new Vector2[this.vertexCount];
					}
					if (this.hasUv4)
					{
						this.uv4 = new Vector2[this.vertexCount];
					}
					if (this.hasColors)
					{
						this.colors32 = new Color32[this.vertexCount];
					}
				}
				for (int j = 0; j < this.vertexCount; j++)
				{
					int num2 = array2[j];
					this.vertices[j] = sub.vertices[num2];
					if (this.hasNormals)
					{
						this.normals[j] = sub.normals[num2];
					}
					if (this.hasTangents)
					{
						this.tangents[j] = sub.tangents[num2];
					}
					if (this.hasUv)
					{
						this.uv[j] = sub.uv[num2];
					}
					if (this.hasUv2)
					{
						this.uv2[j] = sub.uv2[num2];
					}
					if (this.hasUv3)
					{
						this.uv3[j] = sub.uv3[num2];
					}
					if (this.hasUv4)
					{
						this.uv4[j] = sub.uv4[num2];
					}
					if (this.hasColors)
					{
						this.colors32[j] = sub.colors32[num2];
					}
				}
			}

			// Token: 0x04000264 RID: 612
			public Vector3[] vertices;

			// Token: 0x04000265 RID: 613
			public Vector3[] normals;

			// Token: 0x04000266 RID: 614
			public Vector4[] tangents;

			// Token: 0x04000267 RID: 615
			public Vector2[] uv;

			// Token: 0x04000268 RID: 616
			public Vector2[] uv2;

			// Token: 0x04000269 RID: 617
			public Vector2[] uv3;

			// Token: 0x0400026A RID: 618
			public Vector2[] uv4;

			// Token: 0x0400026B RID: 619
			public Color32[] colors32;

			// Token: 0x0400026C RID: 620
			public int[] triangles;

			// Token: 0x0400026D RID: 621
			public bool hasNormals;

			// Token: 0x0400026E RID: 622
			public bool hasTangents;

			// Token: 0x0400026F RID: 623
			public bool hasUv;

			// Token: 0x04000270 RID: 624
			public bool hasUv2;

			// Token: 0x04000271 RID: 625
			public bool hasUv3;

			// Token: 0x04000272 RID: 626
			public bool hasUv4;

			// Token: 0x04000273 RID: 627
			public bool hasColors;

			// Token: 0x04000274 RID: 628
			public int vertexCount;

			// Token: 0x04000275 RID: 629
			public int triangleCount;
		}
	}
}
