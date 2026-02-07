using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace ECM2
{
	// Token: 0x02000013 RID: 19
	public static class MeshUtility
	{
		// Token: 0x06000268 RID: 616 RVA: 0x0000A5A8 File Offset: 0x000087A8
		public static Vector3 FindMeshOpposingNormal(Mesh sharedMesh, ref RaycastHit inHit)
		{
			Vector3 point;
			Vector3 point2;
			Vector3 point3;
			if (sharedMesh.indexFormat == IndexFormat.UInt16)
			{
				MeshUtility._triangles16.Clear();
				int subMeshCount = sharedMesh.subMeshCount;
				if (subMeshCount == 1)
				{
					sharedMesh.GetTriangles(MeshUtility._triangles16, 0, true);
				}
				else
				{
					for (int i = 0; i < subMeshCount; i++)
					{
						sharedMesh.GetTriangles(MeshUtility._scratchBuffer16, i, true);
						MeshUtility._triangles16.AddRange(MeshUtility._scratchBuffer16);
					}
				}
				sharedMesh.GetVertices(MeshUtility._vertices);
				point = MeshUtility._vertices[(int)MeshUtility._triangles16[inHit.triangleIndex * 3]];
				point2 = MeshUtility._vertices[(int)MeshUtility._triangles16[inHit.triangleIndex * 3 + 1]];
				point3 = MeshUtility._vertices[(int)MeshUtility._triangles16[inHit.triangleIndex * 3 + 2]];
			}
			else
			{
				MeshUtility._triangles32.Clear();
				int subMeshCount2 = sharedMesh.subMeshCount;
				if (subMeshCount2 == 1)
				{
					sharedMesh.GetTriangles(MeshUtility._triangles32, 0);
				}
				else
				{
					for (int j = 0; j < subMeshCount2; j++)
					{
						sharedMesh.GetTriangles(MeshUtility._scratchBuffer32, j);
						MeshUtility._triangles32.AddRange(MeshUtility._scratchBuffer32);
					}
				}
				sharedMesh.GetVertices(MeshUtility._vertices);
				point = MeshUtility._vertices[MeshUtility._triangles32[inHit.triangleIndex * 3]];
				point2 = MeshUtility._vertices[MeshUtility._triangles32[inHit.triangleIndex * 3 + 1]];
				point3 = MeshUtility._vertices[MeshUtility._triangles32[inHit.triangleIndex * 3 + 2]];
			}
			Matrix4x4 localToWorldMatrix = inHit.transform.localToWorldMatrix;
			Vector3 b = localToWorldMatrix.MultiplyPoint3x4(point);
			Vector3 a = localToWorldMatrix.MultiplyPoint3x4(point2);
			Vector3 a2 = localToWorldMatrix.MultiplyPoint3x4(point3);
			Vector3 vector = a - b;
			Vector3 vector2 = a2 - b;
			Vector3 normalized = Vector3.Cross(vector, vector2).normalized;
			if (Vector3.Dot(normalized, inHit.normal) < 0f)
			{
				normalized = Vector3.Cross(vector2, vector).normalized;
			}
			return normalized;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000A7B1 File Offset: 0x000089B1
		public static void FlushBuffers()
		{
			MeshUtility._vertices.Clear();
			MeshUtility._scratchBuffer16.Clear();
			MeshUtility._scratchBuffer32.Clear();
			MeshUtility._triangles16.Clear();
			MeshUtility._triangles32.Clear();
		}

		// Token: 0x040000E2 RID: 226
		private const int kMaxVertices = 1024;

		// Token: 0x040000E3 RID: 227
		private const int kMaxTriangles = 3072;

		// Token: 0x040000E4 RID: 228
		private static readonly List<Vector3> _vertices = new List<Vector3>(1024);

		// Token: 0x040000E5 RID: 229
		private static readonly List<ushort> _triangles16 = new List<ushort>(3072);

		// Token: 0x040000E6 RID: 230
		private static readonly List<int> _triangles32 = new List<int>();

		// Token: 0x040000E7 RID: 231
		private static readonly List<ushort> _scratchBuffer16 = new List<ushort>(3072);

		// Token: 0x040000E8 RID: 232
		private static readonly List<int> _scratchBuffer32 = new List<int>();
	}
}
