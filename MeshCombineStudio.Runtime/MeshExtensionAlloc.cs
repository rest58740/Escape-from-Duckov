using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000018 RID: 24
	public static class MeshExtensionAlloc
	{
		// Token: 0x0600005B RID: 91 RVA: 0x0000501C File Offset: 0x0000321C
		public static void ApplyVertices(Mesh mesh, Vector3[] vertices, int length)
		{
			Vector3[] array = new Vector3[length];
			Array.Copy(vertices, array, length);
			mesh.vertices = array;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00005040 File Offset: 0x00003240
		public static void ApplyNormals(Mesh mesh, Vector3[] normals, int length)
		{
			Vector3[] array = new Vector3[length];
			Array.Copy(normals, array, length);
			mesh.normals = array;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00005064 File Offset: 0x00003264
		public static void ApplyTangents(Mesh mesh, Vector4[] tangents, int length)
		{
			Vector4[] array = new Vector4[length];
			Array.Copy(tangents, array, length);
			mesh.tangents = array;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00005088 File Offset: 0x00003288
		public static void ApplyUvs(Mesh mesh, int channel, Vector2[] uvs, int length)
		{
			Vector2[] array = new Vector2[length];
			Array.Copy(uvs, array, length);
			if (channel == 0)
			{
				mesh.uv = array;
				return;
			}
			if (channel == 1)
			{
				mesh.uv2 = array;
				return;
			}
			if (channel == 2)
			{
				mesh.uv3 = array;
				return;
			}
			mesh.uv4 = array;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000050D0 File Offset: 0x000032D0
		public static void ApplyColors32(Mesh mesh, Color32[] colors, int length)
		{
			Color32[] array = new Color32[length];
			Array.Copy(colors, array, length);
			mesh.colors32 = array;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000050F4 File Offset: 0x000032F4
		public static void ApplyTriangles(Mesh mesh, int[] triangles, int length)
		{
			int[] array = new int[length];
			Array.Copy(triangles, array, length);
			mesh.triangles = array;
		}
	}
}
