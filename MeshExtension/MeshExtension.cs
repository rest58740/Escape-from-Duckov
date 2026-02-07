using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
public static class MeshExtension
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public unsafe static void ApplyVertices(Mesh mesh, Vector3[] vertices, int count)
	{
		fixed (IntPtr* ptr = vertices)
		{
			UIntPtr* ptr2 = (UIntPtr*)ptr - sizeof(UIntPtr) / sizeof(UIntPtr);
			UIntPtr uintPtr = *ptr2;
			try
			{
				*ptr2 = (UIntPtr)((ulong)((long)count));
				mesh.vertices = vertices;
			}
			finally
			{
				*ptr2 = uintPtr;
			}
		}
	}

	// Token: 0x06000002 RID: 2 RVA: 0x000020AC File Offset: 0x000002AC
	public unsafe static void ApplyNormals(Mesh mesh, Vector3[] normals, int count)
	{
		fixed (IntPtr* ptr = normals)
		{
			UIntPtr* ptr2 = (UIntPtr*)ptr - sizeof(UIntPtr) / sizeof(UIntPtr);
			UIntPtr uintPtr = *ptr2;
			try
			{
				*ptr2 = (UIntPtr)((ulong)((long)count));
				mesh.normals = normals;
			}
			finally
			{
				*ptr2 = uintPtr;
			}
		}
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002108 File Offset: 0x00000308
	public unsafe static void ApplyTangents(Mesh mesh, Vector4[] tangents, int count)
	{
		fixed (IntPtr* ptr = tangents)
		{
			UIntPtr* ptr2 = (UIntPtr*)ptr - sizeof(UIntPtr) / sizeof(UIntPtr);
			UIntPtr uintPtr = *ptr2;
			try
			{
				*ptr2 = (UIntPtr)((ulong)((long)count));
				mesh.tangents = tangents;
			}
			finally
			{
				*ptr2 = uintPtr;
			}
		}
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002164 File Offset: 0x00000364
	public unsafe static void ApplyUvs(Mesh mesh, Vector2[] uvs, int channel, int count)
	{
		fixed (IntPtr* ptr = uvs)
		{
			UIntPtr* ptr2 = (UIntPtr*)ptr - sizeof(UIntPtr) / sizeof(UIntPtr);
			UIntPtr uintPtr = *ptr2;
			try
			{
				*ptr2 = (UIntPtr)((ulong)((long)count));
				if (channel == 0)
				{
					mesh.uv = uvs;
				}
				else if (channel == 1)
				{
					mesh.uv2 = uvs;
				}
				else if (channel == 2)
				{
					mesh.uv3 = uvs;
				}
				else if (channel == 3)
				{
					mesh.uv4 = uvs;
				}
			}
			finally
			{
				*ptr2 = uintPtr;
			}
		}
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000021E8 File Offset: 0x000003E8
	public unsafe static void ApplyColors32(Mesh mesh, Color32[] colors32, int count)
	{
		fixed (IntPtr* ptr = colors32)
		{
			UIntPtr* ptr2 = (UIntPtr*)ptr - sizeof(UIntPtr) / sizeof(UIntPtr);
			UIntPtr uintPtr = *ptr2;
			try
			{
				*ptr2 = (UIntPtr)((ulong)((long)count));
				mesh.colors32 = colors32;
			}
			finally
			{
				*ptr2 = uintPtr;
			}
		}
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002244 File Offset: 0x00000444
	public unsafe static void ApplyColors(Mesh mesh, Color[] colors, int count)
	{
		fixed (IntPtr* ptr = colors)
		{
			UIntPtr* ptr2 = (UIntPtr*)ptr - sizeof(UIntPtr) / sizeof(UIntPtr);
			UIntPtr uintPtr = *ptr2;
			try
			{
				*ptr2 = (UIntPtr)((ulong)((long)count));
				mesh.colors = colors;
			}
			finally
			{
				*ptr2 = uintPtr;
			}
		}
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000022A0 File Offset: 0x000004A0
	public unsafe static void ApplyColors(Mesh mesh, Color32[] colors32, int count)
	{
		fixed (IntPtr* ptr = colors32)
		{
			UIntPtr* ptr2 = (UIntPtr*)ptr - sizeof(UIntPtr) / sizeof(UIntPtr);
			UIntPtr uintPtr = *ptr2;
			try
			{
				*ptr2 = (UIntPtr)((ulong)((long)count));
				mesh.colors32 = colors32;
			}
			finally
			{
				*ptr2 = uintPtr;
			}
		}
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000022FC File Offset: 0x000004FC
	public unsafe static void ApplyTriangles(Mesh mesh, int[] triangles, int count)
	{
		fixed (IntPtr* ptr = triangles)
		{
			UIntPtr* ptr2 = (UIntPtr*)ptr - sizeof(UIntPtr) / sizeof(UIntPtr);
			UIntPtr uintPtr = *ptr2;
			try
			{
				*ptr2 = (UIntPtr)((ulong)((long)count));
				mesh.triangles = triangles;
			}
			finally
			{
				*ptr2 = uintPtr;
			}
		}
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002358 File Offset: 0x00000558
	public unsafe static void ApplyTriangles(Mesh mesh, int subMeshIndex, int[] triangles, int count)
	{
		fixed (IntPtr* ptr = triangles)
		{
			UIntPtr* ptr2 = (UIntPtr*)ptr - sizeof(UIntPtr) / sizeof(UIntPtr);
			UIntPtr uintPtr = *ptr2;
			try
			{
				*ptr2 = (UIntPtr)((ulong)((long)count));
				mesh.SetTriangles(triangles, subMeshIndex);
			}
			finally
			{
				*ptr2 = uintPtr;
			}
		}
	}
}
