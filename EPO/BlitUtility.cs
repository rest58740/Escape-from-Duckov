using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace EPOOutline
{
	// Token: 0x02000004 RID: 4
	public static class BlitUtility
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002293 File Offset: 0x00000493
		private static bool SupportsInstancing
		{
			get
			{
				if (BlitUtility.supportsInstancing != null)
				{
					return BlitUtility.supportsInstancing.Value;
				}
				BlitUtility.supportsInstancing = new bool?(SystemInfo.supportsInstancing);
				return BlitUtility.supportsInstancing.Value;
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000022C8 File Offset: 0x000004C8
		private static void UpdateBounds(Renderer renderer, OutlineTarget target)
		{
			if (target.renderer is MeshRenderer)
			{
				MeshFilter component = renderer.GetComponent<MeshFilter>();
				if (component.sharedMesh != null)
				{
					component.sharedMesh.RecalculateBounds();
					return;
				}
			}
			else
			{
				SkinnedMeshRenderer skinnedMeshRenderer = target.renderer as SkinnedMeshRenderer;
				if (skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null)
				{
					skinnedMeshRenderer.sharedMesh.RecalculateBounds();
				}
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000232C File Offset: 0x0000052C
		public static void PrepareForRendering(OutlineParameters parameters)
		{
			if (parameters.BlitMesh == null)
			{
				parameters.BlitMesh = parameters.MeshPool.AllocateMesh();
			}
			BlitUtility.MeshSetupResult? meshSetupResult = BlitUtility.SupportsInstancing ? BlitUtility.SetupForInstancing(parameters) : BlitUtility.SetupForBruteForce(parameters);
			BlitUtility.currentSetupResult = meshSetupResult;
			if (meshSetupResult == null)
			{
				return;
			}
			parameters.BlitMesh.SetVertexBufferParams(meshSetupResult.Value.VertexIndex, BlitUtility.vertexParams);
			parameters.BlitMesh.SetVertexBufferData<BlitUtility.Vertex>(BlitUtility.vertices, 0, 0, meshSetupResult.Value.VertexIndex, 0, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
			parameters.BlitMesh.SetIndexBufferParams(meshSetupResult.Value.TriangleIndex, IndexFormat.UInt16);
			parameters.BlitMesh.SetIndexBufferData<ushort>(BlitUtility.indices, 0, 0, meshSetupResult.Value.TriangleIndex, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
			parameters.BlitMesh.subMeshCount = 1;
			parameters.BlitMesh.SetSubMesh(0, new SubMeshDescriptor(0, meshSetupResult.Value.TriangleIndex, MeshTopology.Triangles), MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002420 File Offset: 0x00000620
		private static void CheckModel()
		{
			if (BlitUtility.normals != null && BlitUtility.tempVertecies != null && BlitUtility.tempIndicies != null)
			{
				return;
			}
			Mesh mesh = Resources.Load<Mesh>("Easy performant outline/Models/Rounded box");
			BlitUtility.tempVertecies = Array.ConvertAll<Vector3, Vector4>(mesh.vertices, (Vector3 x) => new Vector4(x.x, x.y, x.z, 1f));
			BlitUtility.tempIndicies = Array.ConvertAll<int, ushort>(mesh.triangles, (int x) => (ushort)x);
			BlitUtility.normals = Array.ConvertAll<Vector3, Vector4>(mesh.normals, (Vector3 x) => x);
			Resources.UnloadAsset(mesh);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000024E0 File Offset: 0x000006E0
		private static BlitUtility.MeshSetupResult? SetupForInstancing(OutlineParameters parameters)
		{
			BlitUtility.CheckModel();
			if (BlitUtility.vertices.Length < BlitUtility.tempVertecies.Length)
			{
				Array.Resize<BlitUtility.Vertex>(ref BlitUtility.vertices, BlitUtility.tempVertecies.Length);
			}
			if (BlitUtility.indices.Length < BlitUtility.tempIndicies.Length)
			{
				Array.Resize<ushort>(ref BlitUtility.indices, BlitUtility.tempIndicies.Length);
			}
			int num = 0;
			using (List<Outlinable>.Enumerator enumerator = parameters.OutlinablesToRender.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Outlinable outlinable = enumerator.Current;
					num += outlinable.OutlineTargets.Count;
				}
				goto IL_9B;
			}
			IL_88:
			Array.Resize<Matrix4x4>(ref BlitUtility.matrices, BlitUtility.matrices.Length * 2);
			IL_9B:
			if (BlitUtility.matrices.Length >= num)
			{
				while (BlitUtility.rotationMatrices.Length < num)
				{
					Array.Resize<Matrix4x4>(ref BlitUtility.rotationMatrices, BlitUtility.rotationMatrices.Length * 2);
				}
				int vertexIndex = 0;
				for (int i = 0; i < BlitUtility.tempIndicies.Length; i++)
				{
					BlitUtility.indices[i] = BlitUtility.tempIndicies[i];
				}
				for (int j = 0; j < BlitUtility.tempVertecies.Length; j++)
				{
					BlitUtility.vertices[vertexIndex++] = new BlitUtility.Vertex
					{
						Position = BlitUtility.tempVertecies[j],
						Normal = BlitUtility.normals[j]
					};
				}
				int num2 = 0;
				foreach (Outlinable outlinable2 in parameters.OutlinablesToRender)
				{
					if (outlinable2.DrawingMode == OutlinableDrawingMode.Normal)
					{
						foreach (OutlineTarget outlineTarget in outlinable2.OutlineTargets)
						{
							Renderer renderer = outlineTarget.Renderer;
							if (outlineTarget.IsVisible)
							{
								bool flag = false;
								Bounds bounds = default(Bounds);
								if (outlineTarget.BoundsMode == BoundsMode.Manual)
								{
									bounds = outlineTarget.Bounds;
									Vector3 size = bounds.size;
									Vector3 localScale = renderer.transform.localScale;
									size.x /= localScale.x;
									size.y /= localScale.y;
									size.z /= localScale.z;
									bounds.size = size;
								}
								else
								{
									if (outlineTarget.BoundsMode == BoundsMode.ForceRecalculate)
									{
										BlitUtility.UpdateBounds(outlineTarget.Renderer, outlineTarget);
									}
									MeshRenderer meshRenderer = renderer as MeshRenderer;
									int num3 = ((meshRenderer == null) ? 0 : meshRenderer.subMeshStartIndex) + outlineTarget.SubmeshIndex;
									MeshFilter meshFilter = (meshRenderer == null) ? null : meshRenderer.GetComponent<MeshFilter>();
									Mesh mesh = (meshFilter == null) ? null : meshFilter.sharedMesh;
									if (mesh != null && mesh.subMeshCount > num3)
									{
										bounds = mesh.GetSubMesh(num3).bounds;
										flag = meshRenderer.isPartOfStaticBatch;
									}
									else
									{
										flag = true;
										bounds = renderer.bounds;
									}
								}
								if (flag)
								{
									BlitUtility.rotationMatrices[num2] = Matrix4x4.identity;
									BlitUtility.matrices[num2++] = Matrix4x4.TRS(bounds.center, Quaternion.identity, bounds.size);
								}
								else
								{
									Transform transform = outlineTarget.renderer.transform;
									Vector3 size2 = bounds.size;
									BlitUtility.rotationMatrices[num2] = Matrix4x4.Rotate(transform.rotation);
									BlitUtility.matrices[num2++] = transform.localToWorldMatrix * Matrix4x4.Translate(bounds.center) * Matrix4x4.Scale(size2);
								}
							}
						}
					}
				}
				return new BlitUtility.MeshSetupResult?(new BlitUtility.MeshSetupResult(num2, vertexIndex, BlitUtility.tempIndicies.Length));
			}
			goto IL_88;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000028E4 File Offset: 0x00000AE4
		private static BlitUtility.MeshSetupResult? SetupForBruteForce(OutlineParameters parameters)
		{
			BlitUtility.CheckModel();
			int num = BlitUtility.tempVertecies.Length;
			int num2 = 0;
			int triangleIndex = 0;
			int num3 = 0;
			foreach (Outlinable outlinable in parameters.OutlinablesToRender)
			{
				num3 += num * outlinable.OutlineTargets.Count;
			}
			if (BlitUtility.vertices.Length < num3)
			{
				Array.Resize<BlitUtility.Vertex>(ref BlitUtility.vertices, num3 * 2);
				Array.Resize<ushort>(ref BlitUtility.indices, BlitUtility.vertices.Length * 5);
			}
			foreach (Outlinable outlinable2 in parameters.OutlinablesToRender)
			{
				if (outlinable2.DrawingMode == OutlinableDrawingMode.Normal)
				{
					for (int i = 0; i < outlinable2.OutlineTargets.Count; i++)
					{
						OutlineTarget outlineTarget = outlinable2.OutlineTargets[i];
						Renderer renderer = outlineTarget.Renderer;
						if (outlineTarget.IsVisible)
						{
							bool flag = false;
							Bounds bounds = default(Bounds);
							if (outlineTarget.BoundsMode == BoundsMode.Manual)
							{
								bounds = outlineTarget.Bounds;
								Vector3 size = bounds.size;
								Vector3 localScale = renderer.transform.localScale;
								size.x /= localScale.x;
								size.y /= localScale.y;
								size.z /= localScale.z;
								bounds.size = size;
							}
							else
							{
								if (outlineTarget.BoundsMode == BoundsMode.ForceRecalculate)
								{
									BlitUtility.UpdateBounds(outlineTarget.Renderer, outlineTarget);
								}
								MeshRenderer meshRenderer = renderer as MeshRenderer;
								int num4 = ((meshRenderer == null) ? 0 : meshRenderer.subMeshStartIndex) + outlineTarget.SubmeshIndex;
								MeshFilter meshFilter = (meshRenderer == null) ? null : meshRenderer.GetComponent<MeshFilter>();
								Mesh mesh = (meshFilter == null) ? null : meshFilter.sharedMesh;
								if (mesh != null && mesh.subMeshCount > num4)
								{
									bounds = mesh.GetSubMesh(num4).bounds;
								}
								else
								{
									flag = true;
									bounds = renderer.bounds;
								}
							}
							Vector4 vector = bounds.size;
							vector.w = 1f;
							Vector4 a = bounds.center;
							Matrix4x4 lhs = Matrix4x4.identity;
							Matrix4x4 lhs2 = Matrix4x4.identity;
							if (!flag && (outlineTarget.BoundsMode == BoundsMode.Manual || !renderer.isPartOfStaticBatch))
							{
								lhs = outlineTarget.renderer.transform.localToWorldMatrix;
								lhs2 = Matrix4x4.Rotate(renderer.transform.rotation);
							}
							int num5 = BlitUtility.tempIndicies.Length;
							for (int j = 0; j < num5; j++)
							{
								BlitUtility.indices[triangleIndex++] = (ushort)((int)BlitUtility.tempIndicies[j] + num2);
							}
							for (int k = 0; k < num; k++)
							{
								Vector4 v = lhs2 * BlitUtility.normals[k];
								Vector4 vector2 = BlitUtility.tempVertecies[k];
								Vector4 b = new Vector4(vector2.x * vector.x, vector2.y * vector.y, vector2.z * vector.z, 1f);
								BlitUtility.Vertex vertex = new BlitUtility.Vertex
								{
									Position = lhs * (a + b),
									Normal = v
								};
								BlitUtility.vertices[num2++] = vertex;
							}
						}
					}
				}
			}
			BlitUtility.rotationMatrices[0] = Matrix4x4.identity;
			return new BlitUtility.MeshSetupResult?(new BlitUtility.MeshSetupResult(1, num2, triangleIndex));
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002CC0 File Offset: 0x00000EC0
		private static void RenderInstancedBatched(CommandBufferWrapper buffer, Mesh mesh, Material material, int pass, int count)
		{
			if (BlitUtility.propertyBlock == null)
			{
				BlitUtility.propertyBlock = new MaterialPropertyBlock();
			}
			BlitUtility.propertyBlock.Clear();
			int num = 0;
			while (count > 0)
			{
				int num2 = Mathf.Min(128, count);
				Array.Copy(BlitUtility.rotationMatrices, num, BlitUtility.batchRotationMatrices, 0, num2);
				Array.Copy(BlitUtility.matrices, num, BlitUtility.batchMatrices, 0, num2);
				BlitUtility.propertyBlock.SetMatrixArray(BlitUtility.NormalMatricesHash, BlitUtility.batchRotationMatrices);
				buffer.DrawMeshInstanced(mesh, 0, material, pass, BlitUtility.batchMatrices, num2, BlitUtility.propertyBlock);
				count -= num2;
				num += num2;
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002D58 File Offset: 0x00000F58
		public static void Blit(OutlineParameters parameters, RTHandle source, RTHandle destination, RTHandle destinationDepth, int eyeSlice, Material material, int pass = -1, Rect? viewport = null)
		{
			if (BlitUtility.currentSetupResult == null)
			{
				Debug.LogError("Setup process wasn't completed.");
				return;
			}
			CommandBufferWrapper buffer = parameters.Buffer;
			buffer.SetRenderTarget(destination, destinationDepth, eyeSlice);
			if (viewport != null)
			{
				parameters.Buffer.SetViewport(viewport.Value);
			}
			buffer.SetGlobalTexture(BlitUtility.MainTexHash, source);
			if (BlitUtility.SupportsInstancing)
			{
				BlitUtility.RenderInstancedBatched(buffer, parameters.BlitMesh, material, pass, BlitUtility.currentSetupResult.Value.ItemsToDraw);
				return;
			}
			material.SetMatrixArray(BlitUtility.NormalMatricesHash, BlitUtility.identityMatrixArray);
			buffer.DrawMesh(parameters.BlitMesh, Matrix4x4.identity, material, 0, pass);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002E04 File Offset: 0x00001004
		public static void Draw(OutlineParameters parameters, RTHandle destination, RTHandle destinationDepth, int eyeSlice, Material material, int pass = -1, Rect? viewport = null)
		{
			if (BlitUtility.currentSetupResult == null)
			{
				Debug.LogError("Setup process wasn't completed.");
				return;
			}
			CommandBufferWrapper buffer = parameters.Buffer;
			buffer.SetRenderTarget(destination, destinationDepth, eyeSlice);
			if (viewport != null)
			{
				parameters.Buffer.SetViewport(viewport.Value);
			}
			if (BlitUtility.SupportsInstancing)
			{
				if (BlitUtility.propertyBlock == null)
				{
					BlitUtility.propertyBlock = new MaterialPropertyBlock();
				}
				BlitUtility.propertyBlock.Clear();
				BlitUtility.propertyBlock.SetMatrixArray(BlitUtility.NormalMatricesHash, BlitUtility.rotationMatrices);
				buffer.DrawMeshInstanced(parameters.BlitMesh, 0, material, pass, BlitUtility.matrices, BlitUtility.currentSetupResult.Value.ItemsToDraw, BlitUtility.propertyBlock);
				return;
			}
			material.SetMatrixArray(BlitUtility.NormalMatricesHash, BlitUtility.identityMatrixArray);
			buffer.DrawMesh(parameters.BlitMesh, Matrix4x4.identity, material, 0, pass);
		}

		// Token: 0x04000002 RID: 2
		private static readonly int MainTexHash = Shader.PropertyToID("_MainTex");

		// Token: 0x04000003 RID: 3
		private static readonly int NormalMatricesHash = Shader.PropertyToID("_NormalMatrices");

		// Token: 0x04000004 RID: 4
		private static Vector4[] normals;

		// Token: 0x04000005 RID: 5
		private static ushort[] tempIndicies;

		// Token: 0x04000006 RID: 6
		private static Vector4[] tempVertecies;

		// Token: 0x04000007 RID: 7
		private static readonly VertexAttributeDescriptor[] vertexParams = new VertexAttributeDescriptor[]
		{
			new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 4, 0),
			new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3, 0)
		};

		// Token: 0x04000008 RID: 8
		private const int BatchSize = 128;

		// Token: 0x04000009 RID: 9
		private const int DefaultBufferSize = 16;

		// Token: 0x0400000A RID: 10
		private static BlitUtility.Vertex[] vertices = new BlitUtility.Vertex[4096];

		// Token: 0x0400000B RID: 11
		private static ushort[] indices = new ushort[20480];

		// Token: 0x0400000C RID: 12
		private static Matrix4x4[] matrices = new Matrix4x4[16];

		// Token: 0x0400000D RID: 13
		private static Matrix4x4[] batchMatrices = new Matrix4x4[128];

		// Token: 0x0400000E RID: 14
		private static Matrix4x4[] rotationMatrices = new Matrix4x4[16];

		// Token: 0x0400000F RID: 15
		private static Matrix4x4[] batchRotationMatrices = new Matrix4x4[128];

		// Token: 0x04000010 RID: 16
		private static readonly Matrix4x4[] identityMatrixArray = new Matrix4x4[]
		{
			Matrix4x4.identity
		};

		// Token: 0x04000011 RID: 17
		private static BlitUtility.MeshSetupResult? currentSetupResult;

		// Token: 0x04000012 RID: 18
		private static MaterialPropertyBlock propertyBlock;

		// Token: 0x04000013 RID: 19
		private static bool? supportsInstancing;

		// Token: 0x02000028 RID: 40
		private struct MeshSetupResult
		{
			// Token: 0x060000F0 RID: 240 RVA: 0x00006E60 File Offset: 0x00005060
			public MeshSetupResult(int itemsToDraw, int vertexIndex, int triangleIndex)
			{
				this.ItemsToDraw = itemsToDraw;
				this.VertexIndex = vertexIndex;
				this.TriangleIndex = triangleIndex;
			}

			// Token: 0x040000D6 RID: 214
			public readonly int ItemsToDraw;

			// Token: 0x040000D7 RID: 215
			public readonly int VertexIndex;

			// Token: 0x040000D8 RID: 216
			public readonly int TriangleIndex;
		}

		// Token: 0x02000029 RID: 41
		public struct Vertex
		{
			// Token: 0x040000D9 RID: 217
			public Vector4 Position;

			// Token: 0x040000DA RID: 218
			public Vector3 Normal;
		}
	}
}
