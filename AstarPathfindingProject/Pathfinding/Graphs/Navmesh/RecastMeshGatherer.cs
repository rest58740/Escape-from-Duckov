using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Graphs.Navmesh.Voxelization.Burst;
using Pathfinding.Jobs;
using Pathfinding.Pooling;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001A2 RID: 418
	[BurstCompile]
	public class RecastMeshGatherer
	{
		// Token: 0x06000B5D RID: 2909 RVA: 0x0004002C File Offset: 0x0003E22C
		public RecastMeshGatherer(PhysicsScene physicsScene, PhysicsScene2D physicsScene2D, Bounds bounds, int terrainDownsamplingFactor, LayerMask mask, List<string> tagMask, List<RecastGraph.PerLayerModification> perLayerModifications, float maxColliderApproximationError)
		{
			terrainDownsamplingFactor = Math.Max(terrainDownsamplingFactor, 1);
			this.bounds = bounds;
			this.terrainDownsamplingFactor = terrainDownsamplingFactor;
			this.mask = mask;
			this.tagMask = (tagMask ?? new List<string>());
			this.maxColliderApproximationError = maxColliderApproximationError;
			this.physicsScene = physicsScene;
			this.physicsScene2D = physicsScene2D;
			this.meshes = ListPool<RecastMeshGatherer.GatheredMesh>.Claim();
			this.vertexBuffers = ListPool<NativeArray<Vector3>>.Claim();
			this.triangleBuffers = ListPool<NativeArray<int>>.Claim();
			this.cachedMeshes = ObjectPoolSimple<Dictionary<RecastMeshGatherer.MeshCacheItem, int>>.Claim();
			this.meshData = ListPool<Mesh>.Claim();
			this.modificationsByLayer = RecastGraph.PerLayerModification.ToLayerLookup(perLayerModifications, RecastGraph.PerLayerModification.Default);
			RecastGraph.PerLayerModification @default = RecastGraph.PerLayerModification.Default;
			@default.mode = RecastNavmeshModifier.Mode.UnwalkableSurface;
			this.modificationsByLayer2D = RecastGraph.PerLayerModification.ToLayerLookup(perLayerModifications, @default);
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0004010D File Offset: 0x0003E30D
		[BurstCompile]
		private static void CalculateBounds(ref UnsafeSpan<float3> vertices, ref float4x4 localToWorldMatrix, out Bounds bounds)
		{
			RecastMeshGatherer.CalculateBounds_00000A98$BurstDirectCall.Invoke(ref vertices, ref localToWorldMatrix, out bounds);
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x00040118 File Offset: 0x0003E318
		public new RecastMeshGatherer.MeshCollection Finalize()
		{
			Mesh.MeshDataArray meshDataArray = Mesh.AcquireReadOnlyMeshData(this.meshData);
			NativeArray<RasterizationMesh> nativeArray = new NativeArray<RasterizationMesh>(this.meshes.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			int count = this.vertexBuffers.Count;
			for (int i = 0; i < meshDataArray.Length; i++)
			{
				NativeArray<Vector3> item;
				NativeArray<int> item2;
				MeshUtility.GetMeshData(meshDataArray, i, out item, out item2);
				this.vertexBuffers.Add(item);
				this.triangleBuffers.Add(item2);
			}
			for (int j = 0; j < nativeArray.Length; j++)
			{
				RecastMeshGatherer.GatheredMesh gatheredMesh = this.meshes[j];
				int index;
				if (gatheredMesh.meshDataIndex >= 0)
				{
					index = count + gatheredMesh.meshDataIndex;
				}
				else
				{
					index = -(gatheredMesh.meshDataIndex + 1);
				}
				Bounds lhs = gatheredMesh.bounds;
				UnsafeSpan<float3> vertices = this.vertexBuffers[index].Reinterpret<float3>().AsUnsafeReadOnlySpan<float3>();
				if (lhs == default(Bounds))
				{
					float4x4 float4x = gatheredMesh.matrix;
					RecastMeshGatherer.CalculateBounds(ref vertices, ref float4x, out lhs);
				}
				NativeArray<int> arr = this.triangleBuffers[index];
				nativeArray[j] = new RasterizationMesh
				{
					vertices = vertices,
					triangles = arr.AsUnsafeSpan<int>().Slice(gatheredMesh.indexStart, ((gatheredMesh.indexEnd != -1) ? gatheredMesh.indexEnd : arr.Length) - gatheredMesh.indexStart),
					area = gatheredMesh.area,
					areaIsTag = gatheredMesh.areaIsTag,
					bounds = lhs,
					matrix = gatheredMesh.matrix,
					solid = gatheredMesh.solid,
					doubleSided = gatheredMesh.doubleSided,
					flatten = gatheredMesh.flatten
				};
			}
			this.cachedMeshes.Clear();
			ObjectPoolSimple<Dictionary<RecastMeshGatherer.MeshCacheItem, int>>.Release(ref this.cachedMeshes);
			ListPool<RecastMeshGatherer.GatheredMesh>.Release(ref this.meshes);
			meshDataArray.Dispose();
			return new RecastMeshGatherer.MeshCollection(this.vertexBuffers, this.triangleBuffers, nativeArray);
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0004032A File Offset: 0x0003E52A
		public int AddMeshBuffers(Vector3[] vertices, int[] triangles)
		{
			return this.AddMeshBuffers(new NativeArray<Vector3>(vertices, Allocator.Persistent), new NativeArray<int>(triangles, Allocator.Persistent));
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00040340 File Offset: 0x0003E540
		public int AddMeshBuffers(NativeArray<Vector3> vertices, NativeArray<int> triangles)
		{
			int result = -this.vertexBuffers.Count - 1;
			this.vertexBuffers.Add(vertices);
			this.triangleBuffers.Add(triangles);
			return result;
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x00040368 File Offset: 0x0003E568
		public void AddMesh(Renderer renderer, Mesh gatheredMesh)
		{
			RecastMeshGatherer.GatheredMesh item;
			if (this.ConvertMeshToGatheredMesh(renderer, gatheredMesh, out item))
			{
				this.meshes.Add(item);
			}
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0004038D File Offset: 0x0003E58D
		public void AddMesh(RecastMeshGatherer.GatheredMesh gatheredMesh)
		{
			this.meshes.Add(gatheredMesh);
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0004039C File Offset: 0x0003E59C
		private bool MeshFilterShouldBeIncluded(MeshFilter filter)
		{
			Renderer renderer;
			RecastNavmeshModifier recastNavmeshModifier;
			return filter.TryGetComponent<Renderer>(out renderer) && filter.sharedMesh != null && renderer.enabled && ((1 << filter.gameObject.layer & this.mask) != 0 || (this.tagMask.Count > 0 && this.tagMask.Contains(filter.tag))) && (!filter.TryGetComponent<RecastNavmeshModifier>(out recastNavmeshModifier) || !recastNavmeshModifier.enabled);
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x00040420 File Offset: 0x0003E620
		private bool ConvertMeshToGatheredMesh(Renderer renderer, Mesh mesh, out RecastMeshGatherer.GatheredMesh gatheredMesh)
		{
			if (!mesh.HasVertexAttribute(VertexAttribute.Position))
			{
				gatheredMesh = default(RecastMeshGatherer.GatheredMesh);
				return false;
			}
			if (!mesh.isReadable)
			{
				if (!this.anyNonReadableMesh)
				{
					Debug.LogError("Some meshes could not be included when scanning the graph because they are marked as not readable. This includes the mesh '" + mesh.name + "'. You need to mark the mesh with read/write enabled in the mesh importer. Alternatively you can only rasterize colliders and not meshes. Mesh Collider meshes still need to be readable.", mesh);
				}
				this.anyNonReadableMesh = true;
				gatheredMesh = default(RecastMeshGatherer.GatheredMesh);
				return false;
			}
			renderer.GetSharedMaterials(this.dummyMaterials);
			MeshRenderer meshRenderer = renderer as MeshRenderer;
			int num = (meshRenderer != null) ? meshRenderer.subMeshStartIndex : 0;
			int count = this.dummyMaterials.Count;
			int indexStart = 0;
			int indexEnd = -1;
			if (num > 0 || count < mesh.subMeshCount)
			{
				SubMeshDescriptor subMesh = mesh.GetSubMesh(num);
				SubMeshDescriptor subMesh2 = mesh.GetSubMesh(num + count - 1);
				indexStart = subMesh.indexStart;
				indexEnd = subMesh2.indexStart + subMesh2.indexCount;
			}
			int count2;
			if (!this.cachedMeshes.TryGetValue(new RecastMeshGatherer.MeshCacheItem(mesh), out count2))
			{
				count2 = this.meshData.Count;
				this.meshData.Add(mesh);
				this.cachedMeshes[new RecastMeshGatherer.MeshCacheItem(mesh)] = count2;
			}
			gatheredMesh = new RecastMeshGatherer.GatheredMesh
			{
				meshDataIndex = count2,
				bounds = renderer.bounds,
				indexStart = indexStart,
				indexEnd = indexEnd,
				matrix = renderer.localToWorldMatrix,
				doubleSided = false,
				flatten = false
			};
			return true;
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x00040580 File Offset: 0x0003E780
		private RecastMeshGatherer.GatheredMesh? GetColliderMesh(MeshCollider collider, Matrix4x4 localToWorldMatrix)
		{
			if (!(collider.sharedMesh != null))
			{
				return null;
			}
			Mesh sharedMesh = collider.sharedMesh;
			if (!sharedMesh.HasVertexAttribute(VertexAttribute.Position))
			{
				return null;
			}
			if (!sharedMesh.isReadable)
			{
				if (!this.anyNonReadableMesh)
				{
					Debug.LogError("Some mesh collider meshes could not be included when scanning the graph because they are marked as not readable. This includes the mesh '" + sharedMesh.name + "'. You need to mark the mesh with read/write enabled in the mesh importer.", sharedMesh);
				}
				this.anyNonReadableMesh = true;
				return null;
			}
			int count;
			if (!this.cachedMeshes.TryGetValue(new RecastMeshGatherer.MeshCacheItem(sharedMesh), out count))
			{
				count = this.meshData.Count;
				this.meshData.Add(sharedMesh);
				this.cachedMeshes[new RecastMeshGatherer.MeshCacheItem(sharedMesh)] = count;
			}
			return new RecastMeshGatherer.GatheredMesh?(new RecastMeshGatherer.GatheredMesh
			{
				meshDataIndex = count,
				bounds = collider.bounds,
				areaIsTag = false,
				area = 0,
				indexStart = 0,
				indexEnd = -1,
				solid = collider.convex,
				matrix = localToWorldMatrix,
				doubleSided = false,
				flatten = false
			});
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x000406A8 File Offset: 0x0003E8A8
		public void CollectSceneMeshes()
		{
			if (this.tagMask.Count > 0 || this.mask != 0)
			{
				MeshFilter[] array = UnityCompatibility.FindObjectsByTypeSorted<MeshFilter>();
				bool flag = false;
				foreach (MeshFilter meshFilter in array)
				{
					if (this.MeshFilterShouldBeIncluded(meshFilter))
					{
						Renderer renderer;
						meshFilter.TryGetComponent<Renderer>(out renderer);
						RecastMeshGatherer.GatheredMesh item;
						if (renderer.isPartOfStaticBatch)
						{
							flag = true;
						}
						else if (renderer.bounds.Intersects(this.bounds) && this.ConvertMeshToGatheredMesh(renderer, meshFilter.sharedMesh, out item))
						{
							item.ApplyLayerModification(this.modificationsByLayer[meshFilter.gameObject.layer]);
							this.meshes.Add(item);
						}
					}
				}
				if (flag)
				{
					Debug.LogWarning("Some meshes were statically batched. These meshes can not be used for navmesh calculation due to technical constraints.\nDuring runtime scripts cannot access the data of meshes which have been statically batched.\nOne way to solve this problem is to use cached startup (Save & Load tab in the inspector) to only calculate the graph when the game is not playing.");
				}
			}
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x00040770 File Offset: 0x0003E970
		private static int AreaFromSurfaceMode(RecastNavmeshModifier.Mode mode, int surfaceID)
		{
			switch (mode)
			{
			default:
				return -1;
			case RecastNavmeshModifier.Mode.WalkableSurface:
				return 0;
			case RecastNavmeshModifier.Mode.WalkableSurfaceWithSeam:
			case RecastNavmeshModifier.Mode.WalkableSurfaceWithTag:
				return surfaceID;
			}
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x00040790 File Offset: 0x0003E990
		public void CollectRecastNavmeshModifiers()
		{
			List<RecastNavmeshModifier> list = ListPool<RecastNavmeshModifier>.Claim();
			RecastNavmeshModifier.GetAllInBounds(list, this.bounds);
			for (int i = 0; i < list.Count; i++)
			{
				this.AddNavmeshModifier(list[i]);
			}
			ListPool<RecastNavmeshModifier>.Release(ref list);
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x000407D4 File Offset: 0x0003E9D4
		private void AddNavmeshModifier(RecastNavmeshModifier navmeshModifier)
		{
			if (navmeshModifier.includeInScan == RecastNavmeshModifier.ScanInclusion.AlwaysExclude)
			{
				return;
			}
			if (navmeshModifier.includeInScan == RecastNavmeshModifier.ScanInclusion.Auto && (this.mask >> navmeshModifier.gameObject.layer & 1) == 0 && !this.tagMask.Contains(navmeshModifier.tag))
			{
				return;
			}
			MeshFilter meshFilter;
			Collider collider;
			Collider2D x;
			navmeshModifier.ResolveMeshSource(out meshFilter, out collider, out x);
			if (meshFilter != null)
			{
				Mesh sharedMesh = meshFilter.sharedMesh;
				MeshRenderer renderer;
				RecastMeshGatherer.GatheredMesh item;
				if (meshFilter.TryGetComponent<MeshRenderer>(out renderer) && sharedMesh != null && this.ConvertMeshToGatheredMesh(renderer, meshFilter.sharedMesh, out item))
				{
					item.ApplyNavmeshModifier(navmeshModifier);
					this.meshes.Add(item);
					return;
				}
			}
			else if (collider != null)
			{
				RecastMeshGatherer.GatheredMesh? gatheredMesh = this.ConvertColliderToGatheredMesh(collider);
				if (gatheredMesh != null)
				{
					RecastMeshGatherer.GatheredMesh valueOrDefault = gatheredMesh.GetValueOrDefault();
					valueOrDefault.ApplyNavmeshModifier(navmeshModifier);
					this.meshes.Add(valueOrDefault);
					return;
				}
			}
			else if (!(x != null))
			{
				if (navmeshModifier.geometrySource == RecastNavmeshModifier.GeometrySource.Auto)
				{
					Debug.LogError("Couldn't get geometry source for RecastNavmeshModifier (" + navmeshModifier.gameObject.name + "). It didn't have a collider or MeshFilter+Renderer attached", navmeshModifier.gameObject);
					return;
				}
				Debug.LogError(string.Concat(new string[]
				{
					"Couldn't get geometry source for RecastNavmeshModifier (",
					navmeshModifier.gameObject.name,
					"). It didn't have a ",
					navmeshModifier.geometrySource.ToString(),
					" attached"
				}), navmeshModifier.gameObject);
			}
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0004094C File Offset: 0x0003EB4C
		public void CollectTerrainMeshes(bool rasterizeTrees, float desiredChunkSize)
		{
			Terrain[] activeTerrains = Terrain.activeTerrains;
			if (activeTerrains.Length != 0)
			{
				for (int i = 0; i < activeTerrains.Length; i++)
				{
					if (!(activeTerrains[i].terrainData == null))
					{
						bool flag = this.GenerateTerrainChunks(activeTerrains[i], this.bounds, desiredChunkSize);
						if (rasterizeTrees && flag)
						{
							this.CollectTreeMeshes(activeTerrains[i]);
						}
					}
				}
			}
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x000409A0 File Offset: 0x0003EBA0
		private static int NonNegativeModulus(int x, int m)
		{
			int num = x % m;
			if (num >= 0)
			{
				return num;
			}
			return num + m;
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x000409BA File Offset: 0x0003EBBA
		private static int CeilDivision(int lhs, int rhs)
		{
			return (lhs + rhs - 1) / rhs;
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x000409C4 File Offset: 0x0003EBC4
		private bool GenerateTerrainChunks(Terrain terrain, Bounds bounds, float desiredChunkSize)
		{
			TerrainData terrainData = terrain.terrainData;
			if (terrainData == null)
			{
				throw new ArgumentException("Terrain contains no terrain data");
			}
			Vector3 position = terrain.GetPosition();
			Vector3 size = terrainData.size;
			Vector3 center = position + size * 0.5f;
			Bounds bounds2 = new Bounds(center, size);
			if (!bounds2.Intersects(bounds))
			{
				return false;
			}
			int heightmapResolution = terrainData.heightmapResolution;
			int heightmapResolution2 = terrainData.heightmapResolution;
			Vector3 heightmapScale = terrainData.heightmapScale;
			heightmapScale.y = size.y;
			int num = Mathf.CeilToInt(Mathf.Max(desiredChunkSize / (heightmapScale.x * (float)this.terrainDownsamplingFactor), 12f)) * this.terrainDownsamplingFactor;
			int num2 = Mathf.CeilToInt(Mathf.Max(desiredChunkSize / (heightmapScale.z * (float)this.terrainDownsamplingFactor), 12f)) * this.terrainDownsamplingFactor;
			num = Mathf.Min(num, heightmapResolution);
			num2 = Mathf.Min(num2, heightmapResolution2);
			Vector2Int offset;
			Vector2Int vector2Int;
			if (float.IsFinite(bounds.size.x))
			{
				offset = new Vector2Int(Mathf.FloorToInt((bounds.min.x - position.x) / heightmapScale.x), Mathf.FloorToInt((bounds.min.z - position.z) / heightmapScale.z));
				offset.x -= RecastMeshGatherer.NonNegativeModulus(offset.x, this.terrainDownsamplingFactor);
				offset.y -= RecastMeshGatherer.NonNegativeModulus(offset.y, this.terrainDownsamplingFactor);
				float num3 = (float)num * heightmapScale.x;
				float num4 = (float)num2 * heightmapScale.z;
				vector2Int = new Vector2Int(Mathf.CeilToInt((bounds.max.x - position.x - (float)offset.x * heightmapScale.x) / num3), Mathf.CeilToInt((bounds.max.z - position.z - (float)offset.y * heightmapScale.z) / num4));
			}
			else
			{
				offset = new Vector2Int(0, 0);
				vector2Int = new Vector2Int(RecastMeshGatherer.CeilDivision(heightmapResolution, num), RecastMeshGatherer.CeilDivision(heightmapResolution2, num2));
			}
			IntRect intRect = new IntRect(0, 0, vector2Int.x * num, vector2Int.y * num2).Offset(offset);
			IntRect b = new IntRect(0, 0, heightmapResolution - 1, heightmapResolution2 - 1);
			intRect = IntRect.Intersection(intRect, b);
			if (!intRect.IsValid())
			{
				return false;
			}
			vector2Int = new Vector2Int(RecastMeshGatherer.CeilDivision(intRect.Width, num), RecastMeshGatherer.CeilDivision(intRect.Height, num2));
			float[,] heights = terrainData.GetHeights(intRect.xmin, intRect.ymin, intRect.Width, intRect.Height);
			bool[,] holes = terrainData.GetHoles(intRect.xmin, intRect.ymin, intRect.Width - 1, intRect.Height - 1);
			ulong gcHandle;
			UnsafeSpan<float> unsafeSpan = new UnsafeSpan<float>(heights, ref gcHandle);
			ulong gcHandle2;
			UnsafeSpan<bool> unsafeSpan2 = new UnsafeSpan<bool>(holes, ref gcHandle2);
			Matrix4x4 matrix = Matrix4x4.TRS(position + new Vector3((float)intRect.xmin * heightmapScale.x, 0f, (float)intRect.ymin * heightmapScale.z), Quaternion.identity, heightmapScale);
			for (int i = 0; i < vector2Int.y; i++)
			{
				for (int j = 0; j < vector2Int.x; j++)
				{
					UnsafeSpan<Vector3> unsafeSpan3;
					UnsafeSpan<int> unsafeSpan4;
					RecastMeshGatherer.GenerateHeightmapChunk(ref unsafeSpan, ref unsafeSpan2, intRect.Width, intRect.Height, j * num, i * num2, num, num2, this.terrainDownsamplingFactor, out unsafeSpan3, out unsafeSpan4);
					NativeArray<Vector3> vertices = unsafeSpan3.MoveToNativeArray(Allocator.Persistent);
					NativeArray<int> triangles = unsafeSpan4.MoveToNativeArray(Allocator.Persistent);
					int meshDataIndex = this.AddMeshBuffers(vertices, triangles);
					RecastMeshGatherer.GatheredMesh item = new RecastMeshGatherer.GatheredMesh
					{
						meshDataIndex = meshDataIndex,
						bounds = default(Bounds),
						indexStart = 0,
						indexEnd = -1,
						areaIsTag = false,
						area = 0,
						solid = false,
						matrix = matrix,
						doubleSided = false,
						flatten = false
					};
					item.ApplyLayerModification(this.modificationsByLayer[terrain.gameObject.layer]);
					this.meshes.Add(item);
				}
			}
			UnsafeUtility.ReleaseGCObject(gcHandle);
			UnsafeUtility.ReleaseGCObject(gcHandle2);
			return true;
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00040E20 File Offset: 0x0003F020
		[BurstCompile]
		public static void GenerateHeightmapChunk(ref UnsafeSpan<float> heights, ref UnsafeSpan<bool> holes, int heightmapWidth, int heightmapDepth, int x0, int z0, int width, int depth, int stride, out UnsafeSpan<Vector3> verts, out UnsafeSpan<int> tris)
		{
			RecastMeshGatherer.GenerateHeightmapChunk_00000AA9$BurstDirectCall.Invoke(ref heights, ref holes, heightmapWidth, heightmapDepth, x0, z0, width, depth, stride, out verts, out tris);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x00040E44 File Offset: 0x0003F044
		private void CollectTreeMeshes(Terrain terrain)
		{
			TerrainData terrainData = terrain.terrainData;
			TreeInstance[] treeInstances = terrainData.treeInstances;
			TreePrototype[] treePrototypes = terrainData.treePrototypes;
			Vector3 position = terrain.transform.position;
			Vector3 size = terrainData.size;
			RecastMeshGatherer.TreeInfo[] array = new RecastMeshGatherer.TreeInfo[treePrototypes.Length];
			for (int i = 0; i < treePrototypes.Length; i++)
			{
				TreePrototype treePrototype = treePrototypes[i];
				if (!(treePrototype.prefab == null))
				{
					RecastMeshGatherer.TreeInfo treeInfo;
					if (!this.cachedTreePrefabs.TryGetValue(treePrototype.prefab, out treeInfo))
					{
						treeInfo.submeshes = new List<RecastMeshGatherer.GatheredMesh>();
						LODGroup lodgroup;
						treeInfo.supportsRotation = treePrototype.prefab.TryGetComponent<LODGroup>(out lodgroup);
						treeInfo.localScale = treePrototype.prefab.transform.localScale;
						List<Collider> list = ListPool<Collider>.Claim();
						Matrix4x4 inverse = treePrototype.prefab.transform.localToWorldMatrix.inverse;
						treePrototype.prefab.GetComponentsInChildren<Collider>(false, list);
						for (int j = 0; j < list.Count; j++)
						{
							Collider collider = list[j];
							RecastMeshGatherer.GatheredMesh? gatheredMesh = this.ConvertColliderToGatheredMesh(collider, inverse * collider.transform.localToWorldMatrix);
							if (gatheredMesh != null)
							{
								RecastMeshGatherer.GatheredMesh valueOrDefault = gatheredMesh.GetValueOrDefault();
								RecastNavmeshModifier recastNavmeshModifier;
								if (collider.gameObject.TryGetComponent<RecastNavmeshModifier>(out recastNavmeshModifier) && recastNavmeshModifier.enabled)
								{
									if (recastNavmeshModifier.includeInScan == RecastNavmeshModifier.ScanInclusion.AlwaysExclude)
									{
										goto IL_177;
									}
									valueOrDefault.ApplyNavmeshModifier(recastNavmeshModifier);
								}
								else
								{
									valueOrDefault.ApplyLayerModification(this.modificationsByLayer[collider.gameObject.layer]);
								}
								valueOrDefault.RecalculateBounds();
								treeInfo.submeshes.Add(valueOrDefault);
							}
							IL_177:;
						}
						ListPool<Collider>.Release(ref list);
						this.cachedTreePrefabs[treePrototype.prefab] = treeInfo;
					}
					array[i] = treeInfo;
				}
			}
			foreach (TreeInstance treeInstance in treeInstances)
			{
				RecastMeshGatherer.TreeInfo treeInfo2 = array[treeInstance.prototypeIndex];
				if (treeInfo2.submeshes != null && treeInfo2.submeshes.Count != 0)
				{
					Vector3 pos = position + Vector3.Scale(treeInstance.position, size);
					Vector3 s = Vector3.Scale(new Vector3(treeInstance.widthScale, treeInstance.heightScale, treeInstance.widthScale), treeInfo2.localScale);
					Quaternion q = treeInfo2.supportsRotation ? Quaternion.AngleAxis(treeInstance.rotation * 57.29578f, Vector3.up) : Quaternion.identity;
					Matrix4x4 lhs = Matrix4x4.TRS(pos, q, s);
					for (int l = 0; l < treeInfo2.submeshes.Count; l++)
					{
						RecastMeshGatherer.GatheredMesh gatheredMesh2 = treeInfo2.submeshes[l];
						gatheredMesh2.matrix = lhs * gatheredMesh2.matrix;
						this.meshes.Add(gatheredMesh2);
					}
				}
			}
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x00041120 File Offset: 0x0003F320
		private bool ShouldIncludeCollider(Collider collider)
		{
			RecastNavmeshModifier recastNavmeshModifier;
			if (!collider.enabled || collider.isTrigger || !collider.bounds.Intersects(this.bounds) || (collider.TryGetComponent<RecastNavmeshModifier>(out recastNavmeshModifier) && recastNavmeshModifier.enabled))
			{
				return false;
			}
			GameObject gameObject = collider.gameObject;
			if ((this.mask >> gameObject.layer & 1) != 0)
			{
				return true;
			}
			for (int i = 0; i < this.tagMask.Count; i++)
			{
				if (gameObject.CompareTag(this.tagMask[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x000411B8 File Offset: 0x0003F3B8
		public void CollectColliderMeshes()
		{
			if (this.tagMask.Count == 0 && this.mask == 0)
			{
				return;
			}
			int num = 256;
			Collider[] array = null;
			bool flag = math.all(math.isfinite(this.bounds.extents));
			if (!flag)
			{
				array = UnityCompatibility.FindObjectsByTypeSorted<Collider>();
				num = array.Length;
			}
			else
			{
				do
				{
					if (array != null)
					{
						ArrayPool<Collider>.Release(ref array, false);
					}
					array = ArrayPool<Collider>.Claim(num * 4);
					num = this.physicsScene.OverlapBox(this.bounds.center, this.bounds.extents, array, Quaternion.identity, -1, QueryTriggerInteraction.Ignore);
				}
				while (num == array.Length);
			}
			for (int i = 0; i < num; i++)
			{
				Collider collider = array[i];
				if (this.ShouldIncludeCollider(collider))
				{
					RecastMeshGatherer.GatheredMesh? gatheredMesh = this.ConvertColliderToGatheredMesh(collider);
					if (gatheredMesh != null)
					{
						RecastMeshGatherer.GatheredMesh valueOrDefault = gatheredMesh.GetValueOrDefault();
						valueOrDefault.ApplyLayerModification(this.modificationsByLayer[collider.gameObject.layer]);
						this.meshes.Add(valueOrDefault);
					}
				}
			}
			if (flag)
			{
				ArrayPool<Collider>.Release(ref array, false);
			}
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x000412D5 File Offset: 0x0003F4D5
		private RecastMeshGatherer.GatheredMesh? ConvertColliderToGatheredMesh(Collider col)
		{
			return this.ConvertColliderToGatheredMesh(col, col.transform.localToWorldMatrix);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x000412EC File Offset: 0x0003F4EC
		public RecastMeshGatherer.GatheredMesh? ConvertColliderToGatheredMesh(Collider col, Matrix4x4 localToWorldMatrix)
		{
			BoxCollider boxCollider = col as BoxCollider;
			if (boxCollider != null)
			{
				return new RecastMeshGatherer.GatheredMesh?(this.RasterizeBoxCollider(boxCollider, localToWorldMatrix));
			}
			if (col is SphereCollider || col is CapsuleCollider)
			{
				SphereCollider sphereCollider = col as SphereCollider;
				CapsuleCollider capsuleCollider = col as CapsuleCollider;
				float num = (sphereCollider != null) ? sphereCollider.radius : capsuleCollider.radius;
				float height = (sphereCollider != null) ? 0f : (capsuleCollider.height * 0.5f / num - 1f);
				Quaternion q = Quaternion.identity;
				if (capsuleCollider != null)
				{
					q = Quaternion.Euler((float)((capsuleCollider.direction == 2) ? 90 : 0), 0f, (float)((capsuleCollider.direction == 0) ? 90 : 0));
				}
				Matrix4x4 matrix4x = Matrix4x4.TRS((sphereCollider != null) ? sphereCollider.center : capsuleCollider.center, q, Vector3.one * num);
				matrix4x = localToWorldMatrix * matrix4x;
				return new RecastMeshGatherer.GatheredMesh?(this.RasterizeCapsuleCollider(num, height, col.bounds, matrix4x));
			}
			MeshCollider meshCollider = col as MeshCollider;
			if (meshCollider != null)
			{
				return this.GetColliderMesh(meshCollider, localToWorldMatrix);
			}
			return null;
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x00041418 File Offset: 0x0003F618
		private RecastMeshGatherer.GatheredMesh RasterizeBoxCollider(BoxCollider collider, Matrix4x4 localToWorldMatrix)
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(collider.center, Quaternion.identity, collider.size * 0.5f);
			matrix4x = localToWorldMatrix * matrix4x;
			int num;
			if (!this.cachedMeshes.TryGetValue(RecastMeshGatherer.MeshCacheItem.Box, out num))
			{
				num = this.AddMeshBuffers(RecastMeshGatherer.BoxColliderVerts, RecastMeshGatherer.BoxColliderTris);
				this.cachedMeshes[RecastMeshGatherer.MeshCacheItem.Box] = num;
			}
			return new RecastMeshGatherer.GatheredMesh
			{
				meshDataIndex = num,
				bounds = collider.bounds,
				indexStart = 0,
				indexEnd = -1,
				areaIsTag = false,
				area = 0,
				solid = true,
				matrix = matrix4x,
				doubleSided = false,
				flatten = false
			};
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x000414E4 File Offset: 0x0003F6E4
		private static int CircleSteps(Matrix4x4 matrix, float radius, float maxError)
		{
			float num = math.sqrt(math.max(math.max(math.lengthsq(matrix.GetColumn(0)), math.lengthsq(matrix.GetColumn(1))), math.lengthsq(matrix.GetColumn(2))));
			float num2 = radius * num;
			float num3 = 1f - maxError / num2;
			if (num3 >= 0f)
			{
				return (int)math.ceil(3.1415927f / math.acos(num3));
			}
			return 3;
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x00041570 File Offset: 0x0003F770
		private static float CircleRadiusAdjustmentFactor(int steps)
		{
			return 0.5f * (1f - math.cos(6.2831855f / (float)steps));
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0004158C File Offset: 0x0003F78C
		private RecastMeshGatherer.GatheredMesh RasterizeCapsuleCollider(float radius, float height, Bounds bounds, Matrix4x4 localToWorldMatrix)
		{
			int num = RecastMeshGatherer.CircleSteps(localToWorldMatrix, radius, this.maxColliderApproximationError);
			int num2 = num;
			RecastMeshGatherer.MeshCacheItem key = new RecastMeshGatherer.MeshCacheItem
			{
				type = RecastMeshGatherer.MeshType.Capsule,
				mesh = null,
				rows = num,
				quantizedHeight = Mathf.RoundToInt(height / this.maxColliderApproximationError)
			};
			int num3;
			if (!this.cachedMeshes.TryGetValue(key, out num3))
			{
				NativeArray<Vector3> vertices = new NativeArray<Vector3>(num * num2 + 2, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				NativeArray<int> triangles = new NativeArray<int>(num * num2 * 2 * 3, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				for (int i = 0; i < num; i++)
				{
					for (int j = 0; j < num2; j++)
					{
						vertices[j + i * num2] = new Vector3(Mathf.Cos((float)j * 3.1415927f * 2f / (float)num2) * Mathf.Sin((float)i * 3.1415927f / (float)(num - 1)), Mathf.Cos((float)i * 3.1415927f / (float)(num - 1)) + ((i < num / 2) ? height : (-height)), Mathf.Sin((float)j * 3.1415927f * 2f / (float)num2) * Mathf.Sin((float)i * 3.1415927f / (float)(num - 1)));
					}
				}
				vertices[vertices.Length - 1] = Vector3.up;
				vertices[vertices.Length - 2] = Vector3.down;
				int num4 = 0;
				int k = 0;
				int value = num2 - 1;
				while (k < num2)
				{
					triangles[num4] = vertices.Length - 1;
					triangles[num4 + 1] = value;
					triangles[num4 + 2] = k;
					num4 += 3;
					value = k++;
				}
				for (int l = 1; l < num; l++)
				{
					int m = 0;
					int num5 = num2 - 1;
					while (m < num2)
					{
						triangles[num4] = l * num2 + m;
						triangles[num4 + 1] = l * num2 + num5;
						triangles[num4 + 2] = (l - 1) * num2 + m;
						num4 += 3;
						triangles[num4] = (l - 1) * num2 + num5;
						triangles[num4 + 1] = (l - 1) * num2 + m;
						triangles[num4 + 2] = l * num2 + num5;
						num4 += 3;
						num5 = m++;
					}
				}
				int n = 0;
				int num6 = num2 - 1;
				while (n < num2)
				{
					triangles[num4] = vertices.Length - 2;
					triangles[num4 + 1] = (num - 1) * num2 + num6;
					triangles[num4 + 2] = (num - 1) * num2 + n;
					num4 += 3;
					num6 = n++;
				}
				num3 = this.AddMeshBuffers(vertices, triangles);
				this.cachedMeshes[key] = num3;
			}
			return new RecastMeshGatherer.GatheredMesh
			{
				meshDataIndex = num3,
				bounds = bounds,
				areaIsTag = false,
				area = 0,
				indexStart = 0,
				indexEnd = -1,
				solid = true,
				matrix = localToWorldMatrix,
				doubleSided = false,
				flatten = false
			};
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x000418B8 File Offset: 0x0003FAB8
		private bool ShouldIncludeCollider2D(Collider2D collider)
		{
			if ((this.mask >> collider.gameObject.layer & 1) != 0)
			{
				return true;
			}
			RecastNavmeshModifier recastNavmeshModifier;
			if ((collider.attachedRigidbody ?? collider).TryGetComponent<RecastNavmeshModifier>(out recastNavmeshModifier) && recastNavmeshModifier.enabled && recastNavmeshModifier.includeInScan == RecastNavmeshModifier.ScanInclusion.AlwaysInclude)
			{
				return true;
			}
			for (int i = 0; i < this.tagMask.Count; i++)
			{
				if (collider.CompareTag(this.tagMask[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0004193C File Offset: 0x0003FB3C
		public void Collect2DColliderMeshes()
		{
			if (this.tagMask.Count == 0 && this.mask == 0)
			{
				return;
			}
			int num = 256;
			Collider2D[] array = null;
			bool flag = math.isfinite(this.bounds.extents.x) && math.isfinite(this.bounds.extents.y);
			if (!flag)
			{
				array = UnityCompatibility.FindObjectsByTypeSorted<Collider2D>();
				num = array.Length;
			}
			else
			{
				Vector2 pointA = this.bounds.min;
				Vector2 pointB = this.bounds.max;
				ContactFilter2D contactFilter = default(ContactFilter2D).NoFilter();
				contactFilter.useTriggers = false;
				do
				{
					if (array != null)
					{
						ArrayPool<Collider2D>.Release(ref array, false);
					}
					array = ArrayPool<Collider2D>.Claim(num * 4);
					num = this.physicsScene2D.OverlapArea(pointA, pointB, contactFilter, array);
				}
				while (num == array.Length);
			}
			for (int i = 0; i < num; i++)
			{
				if (!this.ShouldIncludeCollider2D(array[i]))
				{
					array[i] = null;
				}
			}
			NativeArray<float3> nativeArray;
			NativeArray<int> triangles;
			NativeArray<ColliderMeshBuilder2D.ShapeMesh> nativeArray2;
			int num2 = ColliderMeshBuilder2D.GenerateMeshesFromColliders(array, num, this.maxColliderApproximationError, out nativeArray, out triangles, out nativeArray2);
			int meshDataIndex = this.AddMeshBuffers(nativeArray.Reinterpret<Vector3>(), triangles);
			for (int j = 0; j < num2; j++)
			{
				ColliderMeshBuilder2D.ShapeMesh shapeMesh = nativeArray2[j];
				if (this.bounds.Intersects(shapeMesh.bounds))
				{
					Collider2D collider2D = array[shapeMesh.tag];
					RecastNavmeshModifier recastNavmeshModifier;
					(collider2D.attachedRigidbody ?? collider2D).TryGetComponent<RecastNavmeshModifier>(out recastNavmeshModifier);
					RecastMeshGatherer.GatheredMesh item = new RecastMeshGatherer.GatheredMesh
					{
						meshDataIndex = meshDataIndex,
						bounds = shapeMesh.bounds,
						indexStart = shapeMesh.startIndex,
						indexEnd = shapeMesh.endIndex,
						areaIsTag = false,
						area = -1,
						solid = false,
						matrix = shapeMesh.matrix,
						doubleSided = true,
						flatten = true
					};
					if (recastNavmeshModifier != null)
					{
						if (recastNavmeshModifier.includeInScan == RecastNavmeshModifier.ScanInclusion.AlwaysExclude)
						{
							goto IL_23A;
						}
						item.ApplyNavmeshModifier(recastNavmeshModifier);
					}
					else
					{
						item.ApplyLayerModification(this.modificationsByLayer2D[collider2D.gameObject.layer]);
					}
					item.solid = false;
					this.meshes.Add(item);
				}
				IL_23A:;
			}
			if (flag)
			{
				ArrayPool<Collider2D>.Release(ref array, false);
			}
			nativeArray2.Dispose();
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x00041CAC File Offset: 0x0003FEAC
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void CalculateBounds$BurstManaged(ref UnsafeSpan<float3> vertices, ref float4x4 localToWorldMatrix, out Bounds bounds)
		{
			if (vertices.Length == 0)
			{
				bounds = default(Bounds);
				return;
			}
			float3 @float = float.NegativeInfinity;
			float3 float2 = float.PositiveInfinity;
			uint num = 0U;
			while ((ulong)num < (ulong)((long)vertices.Length))
			{
				float3 y = math.transform(localToWorldMatrix, *vertices[num]);
				@float = math.max(@float, y);
				float2 = math.min(float2, y);
				num += 1U;
			}
			bounds = new Bounds((float2 + @float) * 0.5f, @float - float2);
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x00041D48 File Offset: 0x0003FF48
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void GenerateHeightmapChunk$BurstManaged(ref UnsafeSpan<float> heights, ref UnsafeSpan<bool> holes, int heightmapWidth, int heightmapDepth, int x0, int z0, int width, int depth, int stride, out UnsafeSpan<Vector3> verts, out UnsafeSpan<int> tris)
		{
			int num = RecastMeshGatherer.CeilDivision(Mathf.Min(width, heightmapWidth - x0), stride) + 1;
			int num2 = RecastMeshGatherer.CeilDivision(Mathf.Min(depth, heightmapDepth - z0), stride) + 1;
			int length = num * num2;
			int length2 = (num - 1) * (num2 - 1) * 2 * 3;
			verts = new UnsafeSpan<Vector3>(Allocator.Persistent, length);
			tris = new UnsafeSpan<int>(Allocator.Persistent, length2);
			for (int i = 0; i < num2; i++)
			{
				int num3 = Math.Min(z0 + i * stride, heightmapDepth - 1);
				for (int j = 0; j < num; j++)
				{
					int num4 = Math.Min(x0 + j * stride, heightmapWidth - 1);
					*verts[i * num + j] = new Vector3((float)num4, *heights[num3 * heightmapWidth + num4], (float)num3);
				}
			}
			int num5 = 0;
			for (int k = 0; k < num2 - 1; k++)
			{
				for (int l = 0; l < num - 1; l++)
				{
					int num6 = Math.Min(x0 + stride / 2 + l * stride, heightmapWidth - 2);
					int num7 = Math.Min(z0 + stride / 2 + k * stride, heightmapDepth - 2);
					if (*holes[num7 * (heightmapWidth - 1) + num6])
					{
						*tris[num5] = k * num + l;
						*tris[num5 + 1] = (k + 1) * num + l + 1;
						*tris[num5 + 2] = k * num + l + 1;
						num5 += 3;
						*tris[num5] = k * num + l;
						*tris[num5 + 1] = (k + 1) * num + l;
						*tris[num5 + 2] = (k + 1) * num + l + 1;
						num5 += 3;
					}
				}
			}
			tris = tris.Slice(0, num5);
		}

		// Token: 0x040007BF RID: 1983
		private readonly int terrainDownsamplingFactor;

		// Token: 0x040007C0 RID: 1984
		public readonly LayerMask mask;

		// Token: 0x040007C1 RID: 1985
		public readonly List<string> tagMask;

		// Token: 0x040007C2 RID: 1986
		private readonly float maxColliderApproximationError;

		// Token: 0x040007C3 RID: 1987
		public readonly Bounds bounds;

		// Token: 0x040007C4 RID: 1988
		public readonly PhysicsScene physicsScene;

		// Token: 0x040007C5 RID: 1989
		public readonly PhysicsScene2D physicsScene2D;

		// Token: 0x040007C6 RID: 1990
		private Dictionary<RecastMeshGatherer.MeshCacheItem, int> cachedMeshes = new Dictionary<RecastMeshGatherer.MeshCacheItem, int>();

		// Token: 0x040007C7 RID: 1991
		private readonly Dictionary<GameObject, RecastMeshGatherer.TreeInfo> cachedTreePrefabs = new Dictionary<GameObject, RecastMeshGatherer.TreeInfo>();

		// Token: 0x040007C8 RID: 1992
		private readonly List<NativeArray<Vector3>> vertexBuffers;

		// Token: 0x040007C9 RID: 1993
		private readonly List<NativeArray<int>> triangleBuffers;

		// Token: 0x040007CA RID: 1994
		private readonly List<Mesh> meshData;

		// Token: 0x040007CB RID: 1995
		private readonly RecastGraph.PerLayerModification[] modificationsByLayer;

		// Token: 0x040007CC RID: 1996
		private readonly RecastGraph.PerLayerModification[] modificationsByLayer2D;

		// Token: 0x040007CD RID: 1997
		private bool anyNonReadableMesh;

		// Token: 0x040007CE RID: 1998
		private List<RecastMeshGatherer.GatheredMesh> meshes;

		// Token: 0x040007CF RID: 1999
		private List<Material> dummyMaterials = new List<Material>();

		// Token: 0x040007D0 RID: 2000
		private static readonly int[] BoxColliderTris = new int[]
		{
			0,
			1,
			2,
			0,
			2,
			3,
			6,
			5,
			4,
			7,
			6,
			4,
			0,
			5,
			1,
			0,
			4,
			5,
			1,
			6,
			2,
			1,
			5,
			6,
			2,
			7,
			3,
			2,
			6,
			7,
			3,
			4,
			0,
			3,
			7,
			4
		};

		// Token: 0x040007D1 RID: 2001
		private static readonly Vector3[] BoxColliderVerts = new Vector3[]
		{
			new Vector3(-1f, -1f, -1f),
			new Vector3(1f, -1f, -1f),
			new Vector3(1f, -1f, 1f),
			new Vector3(-1f, -1f, 1f),
			new Vector3(-1f, 1f, -1f),
			new Vector3(1f, 1f, -1f),
			new Vector3(1f, 1f, 1f),
			new Vector3(-1f, 1f, 1f)
		};

		// Token: 0x020001A3 RID: 419
		private struct TreeInfo
		{
			// Token: 0x040007D2 RID: 2002
			public List<RecastMeshGatherer.GatheredMesh> submeshes;

			// Token: 0x040007D3 RID: 2003
			public Vector3 localScale;

			// Token: 0x040007D4 RID: 2004
			public bool supportsRotation;
		}

		// Token: 0x020001A4 RID: 420
		public struct MeshCollection : IArenaDisposable
		{
			// Token: 0x06000B7E RID: 2942 RVA: 0x00041F28 File Offset: 0x00040128
			public MeshCollection(List<NativeArray<Vector3>> vertexBuffers, List<NativeArray<int>> triangleBuffers, NativeArray<RasterizationMesh> meshes)
			{
				this.vertexBuffers = vertexBuffers;
				this.triangleBuffers = triangleBuffers;
				this.meshes = meshes;
			}

			// Token: 0x06000B7F RID: 2943 RVA: 0x00041F40 File Offset: 0x00040140
			void IArenaDisposable.DisposeWith(DisposeArena arena)
			{
				for (int i = 0; i < this.vertexBuffers.Count; i++)
				{
					arena.Add<Vector3>(this.vertexBuffers[i]);
					arena.Add<int>(this.triangleBuffers[i]);
				}
				arena.Add<RasterizationMesh>(this.meshes);
			}

			// Token: 0x040007D5 RID: 2005
			private List<NativeArray<Vector3>> vertexBuffers;

			// Token: 0x040007D6 RID: 2006
			private List<NativeArray<int>> triangleBuffers;

			// Token: 0x040007D7 RID: 2007
			public NativeArray<RasterizationMesh> meshes;
		}

		// Token: 0x020001A5 RID: 421
		public struct GatheredMesh
		{
			// Token: 0x06000B80 RID: 2944 RVA: 0x00041F93 File Offset: 0x00040193
			public void RecalculateBounds()
			{
				this.bounds = default(Bounds);
			}

			// Token: 0x06000B81 RID: 2945 RVA: 0x00041FA1 File Offset: 0x000401A1
			public void ApplyNavmeshModifier(RecastNavmeshModifier navmeshModifier)
			{
				this.area = RecastMeshGatherer.AreaFromSurfaceMode(navmeshModifier.mode, navmeshModifier.surfaceID);
				this.areaIsTag = (navmeshModifier.mode == RecastNavmeshModifier.Mode.WalkableSurfaceWithTag);
				this.solid |= navmeshModifier.solid;
			}

			// Token: 0x06000B82 RID: 2946 RVA: 0x00041FDC File Offset: 0x000401DC
			public void ApplyLayerModification(RecastGraph.PerLayerModification modification)
			{
				this.area = RecastMeshGatherer.AreaFromSurfaceMode(modification.mode, modification.surfaceID);
				this.areaIsTag = (modification.mode == RecastNavmeshModifier.Mode.WalkableSurfaceWithTag);
			}

			// Token: 0x040007D8 RID: 2008
			public int meshDataIndex;

			// Token: 0x040007D9 RID: 2009
			public int area;

			// Token: 0x040007DA RID: 2010
			public int indexStart;

			// Token: 0x040007DB RID: 2011
			public int indexEnd;

			// Token: 0x040007DC RID: 2012
			public Bounds bounds;

			// Token: 0x040007DD RID: 2013
			public Matrix4x4 matrix;

			// Token: 0x040007DE RID: 2014
			public bool solid;

			// Token: 0x040007DF RID: 2015
			public bool doubleSided;

			// Token: 0x040007E0 RID: 2016
			public bool flatten;

			// Token: 0x040007E1 RID: 2017
			public bool areaIsTag;
		}

		// Token: 0x020001A6 RID: 422
		private enum MeshType
		{
			// Token: 0x040007E3 RID: 2019
			Mesh,
			// Token: 0x040007E4 RID: 2020
			Box,
			// Token: 0x040007E5 RID: 2021
			Capsule
		}

		// Token: 0x020001A7 RID: 423
		private struct MeshCacheItem : IEquatable<RecastMeshGatherer.MeshCacheItem>
		{
			// Token: 0x06000B83 RID: 2947 RVA: 0x00042004 File Offset: 0x00040204
			public MeshCacheItem(Mesh mesh)
			{
				this.type = RecastMeshGatherer.MeshType.Mesh;
				this.mesh = mesh;
				this.rows = 0;
				this.quantizedHeight = 0;
			}

			// Token: 0x06000B84 RID: 2948 RVA: 0x00042024 File Offset: 0x00040224
			public bool Equals(RecastMeshGatherer.MeshCacheItem other)
			{
				return this.type == other.type && this.mesh == other.mesh && this.rows == other.rows && this.quantizedHeight == other.quantizedHeight;
			}

			// Token: 0x06000B85 RID: 2949 RVA: 0x00042070 File Offset: 0x00040270
			public override int GetHashCode()
			{
				return (int)(((this.type * (RecastMeshGatherer.MeshType)31 ^ (RecastMeshGatherer.MeshType)((this.mesh != null) ? this.mesh.GetHashCode() : -1)) * (RecastMeshGatherer.MeshType)31 ^ (RecastMeshGatherer.MeshType)this.rows) * (RecastMeshGatherer.MeshType)31 ^ (RecastMeshGatherer.MeshType)this.quantizedHeight);
			}

			// Token: 0x040007E6 RID: 2022
			public RecastMeshGatherer.MeshType type;

			// Token: 0x040007E7 RID: 2023
			public Mesh mesh;

			// Token: 0x040007E8 RID: 2024
			public int rows;

			// Token: 0x040007E9 RID: 2025
			public int quantizedHeight;

			// Token: 0x040007EA RID: 2026
			public static readonly RecastMeshGatherer.MeshCacheItem Box = new RecastMeshGatherer.MeshCacheItem
			{
				type = RecastMeshGatherer.MeshType.Box,
				mesh = null,
				rows = 0,
				quantizedHeight = 0
			};
		}

		// Token: 0x020001A8 RID: 424
		// (Invoke) Token: 0x06000B88 RID: 2952
		internal delegate void CalculateBounds_00000A98$PostfixBurstDelegate(ref UnsafeSpan<float3> vertices, ref float4x4 localToWorldMatrix, out Bounds bounds);

		// Token: 0x020001A9 RID: 425
		internal static class CalculateBounds_00000A98$BurstDirectCall
		{
			// Token: 0x06000B8B RID: 2955 RVA: 0x000420E7 File Offset: 0x000402E7
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RecastMeshGatherer.CalculateBounds_00000A98$BurstDirectCall.Pointer == 0)
				{
					RecastMeshGatherer.CalculateBounds_00000A98$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RecastMeshGatherer.CalculateBounds_00000A98$BurstDirectCall.DeferredCompilation, methodof(RecastMeshGatherer.CalculateBounds$BurstManaged(UnsafeSpan<float3>*, float4x4*, Bounds*)).MethodHandle, typeof(RecastMeshGatherer.CalculateBounds_00000A98$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RecastMeshGatherer.CalculateBounds_00000A98$BurstDirectCall.Pointer;
			}

			// Token: 0x06000B8C RID: 2956 RVA: 0x00042114 File Offset: 0x00040314
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				RecastMeshGatherer.CalculateBounds_00000A98$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06000B8D RID: 2957 RVA: 0x0004212C File Offset: 0x0004032C
			public unsafe static void Constructor()
			{
				RecastMeshGatherer.CalculateBounds_00000A98$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RecastMeshGatherer.CalculateBounds(UnsafeSpan<float3>*, float4x4*, Bounds*)).MethodHandle);
			}

			// Token: 0x06000B8E RID: 2958 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x06000B8F RID: 2959 RVA: 0x0004213D File Offset: 0x0004033D
			// Note: this type is marked as 'beforefieldinit'.
			static CalculateBounds_00000A98$BurstDirectCall()
			{
				RecastMeshGatherer.CalculateBounds_00000A98$BurstDirectCall.Constructor();
			}

			// Token: 0x06000B90 RID: 2960 RVA: 0x00042144 File Offset: 0x00040344
			public static void Invoke(ref UnsafeSpan<float3> vertices, ref float4x4 localToWorldMatrix, out Bounds bounds)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RecastMeshGatherer.CalculateBounds_00000A98$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Collections.UnsafeSpan`1<Unity.Mathematics.float3>&,Unity.Mathematics.float4x4&,UnityEngine.Bounds&), ref vertices, ref localToWorldMatrix, ref bounds, functionPointer);
						return;
					}
				}
				RecastMeshGatherer.CalculateBounds$BurstManaged(ref vertices, ref localToWorldMatrix, out bounds);
			}

			// Token: 0x040007EB RID: 2027
			private static IntPtr Pointer;

			// Token: 0x040007EC RID: 2028
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x020001AA RID: 426
		// (Invoke) Token: 0x06000B92 RID: 2962
		internal delegate void GenerateHeightmapChunk_00000AA9$PostfixBurstDelegate(ref UnsafeSpan<float> heights, ref UnsafeSpan<bool> holes, int heightmapWidth, int heightmapDepth, int x0, int z0, int width, int depth, int stride, out UnsafeSpan<Vector3> verts, out UnsafeSpan<int> tris);

		// Token: 0x020001AB RID: 427
		internal static class GenerateHeightmapChunk_00000AA9$BurstDirectCall
		{
			// Token: 0x06000B95 RID: 2965 RVA: 0x00042179 File Offset: 0x00040379
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (RecastMeshGatherer.GenerateHeightmapChunk_00000AA9$BurstDirectCall.Pointer == 0)
				{
					RecastMeshGatherer.GenerateHeightmapChunk_00000AA9$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(RecastMeshGatherer.GenerateHeightmapChunk_00000AA9$BurstDirectCall.DeferredCompilation, methodof(RecastMeshGatherer.GenerateHeightmapChunk$BurstManaged(UnsafeSpan<float>*, UnsafeSpan<bool>*, int, int, int, int, int, int, int, UnsafeSpan<Vector3>*, UnsafeSpan<int>*)).MethodHandle, typeof(RecastMeshGatherer.GenerateHeightmapChunk_00000AA9$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = RecastMeshGatherer.GenerateHeightmapChunk_00000AA9$BurstDirectCall.Pointer;
			}

			// Token: 0x06000B96 RID: 2966 RVA: 0x000421A8 File Offset: 0x000403A8
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				RecastMeshGatherer.GenerateHeightmapChunk_00000AA9$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06000B97 RID: 2967 RVA: 0x000421C0 File Offset: 0x000403C0
			public unsafe static void Constructor()
			{
				RecastMeshGatherer.GenerateHeightmapChunk_00000AA9$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(RecastMeshGatherer.GenerateHeightmapChunk(UnsafeSpan<float>*, UnsafeSpan<bool>*, int, int, int, int, int, int, int, UnsafeSpan<Vector3>*, UnsafeSpan<int>*)).MethodHandle);
			}

			// Token: 0x06000B98 RID: 2968 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x06000B99 RID: 2969 RVA: 0x000421D1 File Offset: 0x000403D1
			// Note: this type is marked as 'beforefieldinit'.
			static GenerateHeightmapChunk_00000AA9$BurstDirectCall()
			{
				RecastMeshGatherer.GenerateHeightmapChunk_00000AA9$BurstDirectCall.Constructor();
			}

			// Token: 0x06000B9A RID: 2970 RVA: 0x000421D8 File Offset: 0x000403D8
			public static void Invoke(ref UnsafeSpan<float> heights, ref UnsafeSpan<bool> holes, int heightmapWidth, int heightmapDepth, int x0, int z0, int width, int depth, int stride, out UnsafeSpan<Vector3> verts, out UnsafeSpan<int> tris)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = RecastMeshGatherer.GenerateHeightmapChunk_00000AA9$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Collections.UnsafeSpan`1<System.Single>&,Pathfinding.Collections.UnsafeSpan`1<System.Boolean>&,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,Pathfinding.Collections.UnsafeSpan`1<UnityEngine.Vector3>&,Pathfinding.Collections.UnsafeSpan`1<System.Int32>&), ref heights, ref holes, heightmapWidth, heightmapDepth, x0, z0, width, depth, stride, ref verts, ref tris, functionPointer);
						return;
					}
				}
				RecastMeshGatherer.GenerateHeightmapChunk$BurstManaged(ref heights, ref holes, heightmapWidth, heightmapDepth, x0, z0, width, depth, stride, out verts, out tris);
			}

			// Token: 0x040007ED RID: 2029
			private static IntPtr Pointer;

			// Token: 0x040007EE RID: 2030
			private static IntPtr DeferredCompilation;
		}
	}
}
