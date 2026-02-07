using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;

namespace Pathfinding
{
	// Token: 0x02000012 RID: 18
	[HelpURL("https://arongranberg.com/astar/documentation/stable/aipathalignedtosurface.html")]
	public class AIPathAlignedToSurface : AIPath
	{
		// Token: 0x0600007D RID: 125 RVA: 0x000041B7 File Offset: 0x000023B7
		protected override void OnEnable()
		{
			base.OnEnable();
			this.movementPlane = new SimpleMovementPlane(this.rotation);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000041D0 File Offset: 0x000023D0
		protected override void ApplyGravity(float deltaTime)
		{
			if (base.usingGravity)
			{
				this.verticalVelocity += deltaTime * (float.IsNaN(this.gravity.x) ? Physics.gravity.y : this.gravity.y);
				return;
			}
			this.verticalVelocity = 0f;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000422C File Offset: 0x0000242C
		public unsafe static void UpdateMovementPlanes(AIPathAlignedToSurface[] components, int count)
		{
			List<Mesh> list = ListPool<Mesh>.Claim();
			List<List<AIPathAlignedToSurface>> list2 = new List<List<AIPathAlignedToSurface>>();
			Dictionary<Mesh, int> dictionary = AIPathAlignedToSurface.scratchDictionary;
			for (int i = 0; i < count; i++)
			{
				MeshCollider meshCollider = components[i].lastRaycastHit.collider as MeshCollider;
				if (meshCollider != null && components[i].lastRaycastHit.triangleIndex != -1)
				{
					Mesh sharedMesh = meshCollider.sharedMesh;
					int index;
					if (dictionary.TryGetValue(sharedMesh, out index))
					{
						list2[index].Add(components[i]);
					}
					else if (sharedMesh != null && sharedMesh.isReadable)
					{
						dictionary[sharedMesh] = list.Count;
						list.Add(sharedMesh);
						list2.Add(ListPool<AIPathAlignedToSurface>.Claim());
						list2[list.Count - 1].Add(components[i]);
					}
					else
					{
						components[i].SetInterpolatedNormal(components[i].lastRaycastHit.normal);
					}
				}
				else
				{
					components[i].SetInterpolatedNormal(components[i].lastRaycastHit.normal);
				}
			}
			Mesh.MeshDataArray meshDataArray = Mesh.AcquireReadOnlyMeshData(list);
			for (int j = 0; j < list.Count; j++)
			{
				Mesh key = list[j];
				int index2 = dictionary[key];
				Mesh.MeshData meshData = meshDataArray[index2];
				List<AIPathAlignedToSurface> list3 = list2[index2];
				int vertexAttributeStream = meshData.GetVertexAttributeStream(VertexAttribute.Normal);
				if (vertexAttributeStream == -1)
				{
					for (int k = 0; k < list3.Count; k++)
					{
						list3[k].SetInterpolatedNormal(list3[k].lastRaycastHit.normal);
					}
				}
				else
				{
					NativeArray<byte> vertexData = meshData.GetVertexData<byte>(vertexAttributeStream);
					int vertexBufferStride = meshData.GetVertexBufferStride(vertexAttributeStream);
					int vertexAttributeOffset = meshData.GetVertexAttributeOffset(VertexAttribute.Normal);
					byte* ptr = (byte*)vertexData.GetUnsafeReadOnlyPtr<byte>() + vertexAttributeOffset;
					for (int l = 0; l < list3.Count; l++)
					{
						AIPathAlignedToSurface aipathAlignedToSurface = list3[l];
						RaycastHit lastRaycastHit = aipathAlignedToSurface.lastRaycastHit;
						int num;
						int num2;
						int num3;
						if (meshData.indexFormat == IndexFormat.UInt16)
						{
							NativeArray<ushort> indexData = meshData.GetIndexData<ushort>();
							num = (int)indexData[lastRaycastHit.triangleIndex * 3];
							num2 = (int)indexData[lastRaycastHit.triangleIndex * 3 + 1];
							num3 = (int)indexData[lastRaycastHit.triangleIndex * 3 + 2];
						}
						else
						{
							NativeArray<int> indexData2 = meshData.GetIndexData<int>();
							num = indexData2[lastRaycastHit.triangleIndex * 3];
							num2 = indexData2[lastRaycastHit.triangleIndex * 3 + 1];
							num3 = indexData2[lastRaycastHit.triangleIndex * 3 + 2];
						}
						Vector3 a = *(Vector3*)(ptr + num * vertexBufferStride);
						Vector3 a2 = *(Vector3*)(ptr + num2 * vertexBufferStride);
						Vector3 a3 = *(Vector3*)(ptr + num3 * vertexBufferStride);
						Vector3 barycentricCoordinate = lastRaycastHit.barycentricCoordinate;
						Vector3 vector = (a * barycentricCoordinate.x + a2 * barycentricCoordinate.y + a3 * barycentricCoordinate.z).normalized;
						vector = lastRaycastHit.collider.transform.TransformDirection(vector);
						aipathAlignedToSurface.SetInterpolatedNormal(vector);
					}
				}
			}
			meshDataArray.Dispose();
			for (int m = 0; m < list2.Count; m++)
			{
				ListPool<AIPathAlignedToSurface>.Release(list2[m]);
			}
			ListPool<Mesh>.Release(ref list);
			AIPathAlignedToSurface.scratchDictionary.Clear();
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004580 File Offset: 0x00002780
		private void SetInterpolatedNormal(Vector3 normal)
		{
			if (normal != Vector3.zero)
			{
				Vector3 forward = Vector3.Cross(this.movementPlane.rotation * Vector3.right, normal);
				this.movementPlane = new SimpleMovementPlane(Quaternion.LookRotation(forward, normal));
			}
			if (this.rvoController != null)
			{
				this.rvoController.movementPlane = this.movementPlane;
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000035CE File Offset: 0x000017CE
		protected override void UpdateMovementPlane()
		{
		}

		// Token: 0x04000071 RID: 113
		private static readonly Dictionary<Mesh, int> scratchDictionary = new Dictionary<Mesh, int>();
	}
}
