using System;
using System.Collections.Generic;
using Pathfinding.Collections;
using Pathfinding.Graphs.Grid.Jobs;
using Pathfinding.Jobs;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid
{
	// Token: 0x020001F7 RID: 503
	public struct GridGraphScanData
	{
		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x0004F296 File Offset: 0x0004D496
		[Obsolete("Use nodes.bounds or heightHitsBounds depending on if you are using the heightHits array or not")]
		public IntBounds bounds
		{
			get
			{
				return this.nodes.bounds;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x0004F2A3 File Offset: 0x0004D4A3
		[Obsolete("Use nodes.layeredDataLayout instead")]
		public bool layeredDataLayout
		{
			get
			{
				return this.nodes.layeredDataLayout;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x0004F2B0 File Offset: 0x0004D4B0
		[Obsolete("Use nodes.positions instead")]
		public NativeArray<Vector3> nodePositions
		{
			get
			{
				return this.nodes.positions;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x0004F2BD File Offset: 0x0004D4BD
		[Obsolete("Use nodes.connections instead")]
		public NativeArray<ulong> nodeConnections
		{
			get
			{
				return this.nodes.connections;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000CAC RID: 3244 RVA: 0x0004F2CA File Offset: 0x0004D4CA
		[Obsolete("Use nodes.penalties instead")]
		public NativeArray<uint> nodePenalties
		{
			get
			{
				return this.nodes.penalties;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000CAD RID: 3245 RVA: 0x0004F2D7 File Offset: 0x0004D4D7
		[Obsolete("Use nodes.tags instead")]
		public NativeArray<int> nodeTags
		{
			get
			{
				return this.nodes.tags;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x0004F2E4 File Offset: 0x0004D4E4
		[Obsolete("Use nodes.normals instead")]
		public NativeArray<float4> nodeNormals
		{
			get
			{
				return this.nodes.normals;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x0004F2F1 File Offset: 0x0004D4F1
		[Obsolete("Use nodes.walkable instead")]
		public NativeArray<bool> nodeWalkable
		{
			get
			{
				return this.nodes.walkable;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x0004F2FE File Offset: 0x0004D4FE
		[Obsolete("Use nodes.walkableWithErosion instead")]
		public NativeArray<bool> nodeWalkableWithErosion
		{
			get
			{
				return this.nodes.walkableWithErosion;
			}
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0004F30B File Offset: 0x0004D50B
		public void SetDefaultPenalties(uint initialPenalty)
		{
			this.nodes.penalties.MemSet(initialPenalty).Schedule(this.dependencyTracker);
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0004F32C File Offset: 0x0004D52C
		public void SetDefaultNodePositions(GraphTransform transform)
		{
			new JobNodeGridLayout
			{
				graphToWorld = transform.matrix,
				bounds = this.nodes.bounds,
				nodePositions = this.nodes.positions
			}.Schedule(this.dependencyTracker);
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0004F380 File Offset: 0x0004D580
		public JobHandle HeightCheck(GraphCollision collision, float nodeWidth, int maxHits, IntBounds recalculationBounds, NativeArray<int> outLayerCount, float characterHeight, Allocator allocator)
		{
			int num = recalculationBounds.size.x * recalculationBounds.size.z;
			this.heightHits = this.dependencyTracker.NewNativeArray<RaycastHit>(num * maxHits, allocator, NativeArrayOptions.ClearMemory);
			this.heightHitsBounds = recalculationBounds;
			Vector3 raycastOffset = this.up * collision.fromHeight;
			Vector3 raycastDirection = -this.up * (collision.fromHeight + 0.01f);
			if (collision.thickRaycast)
			{
				if (maxHits > 1)
				{
					throw new NotImplementedException("Thick raycasts are not supported for layered grid graphs");
				}
				NativeArray<SpherecastCommand> nativeArray = this.dependencyTracker.NewNativeArray<SpherecastCommand>(num, allocator, NativeArrayOptions.ClearMemory);
				new JobPrepareGridRaycastThick
				{
					graphToWorld = this.transform.matrix,
					bounds = recalculationBounds,
					physicsScene = Physics.defaultPhysicsScene,
					raycastOffset = raycastOffset,
					raycastDirection = raycastDirection,
					raycastMask = collision.heightMask,
					raycastCommands = nativeArray,
					radius = collision.thickRaycastDiameter * nodeWidth * 0.5f
				}.Schedule(this.dependencyTracker);
				this.dependencyTracker.ScheduleBatch(nativeArray, this.heightHits, 2048);
				new JobClampHitToRay
				{
					hits = this.heightHits,
					commands = nativeArray
				}.Schedule(this.dependencyTracker);
				outLayerCount[0] = 1;
				return default(JobHandle);
			}
			else
			{
				NativeArray<RaycastCommand> nativeArray2 = this.dependencyTracker.NewNativeArray<RaycastCommand>(num, allocator, NativeArrayOptions.ClearMemory);
				JobHandle dependency = new JobPrepareGridRaycast
				{
					graphToWorld = this.transform.matrix,
					bounds = recalculationBounds,
					physicsScene = Physics.defaultPhysicsScene,
					raycastOffset = raycastOffset,
					raycastDirection = raycastDirection,
					raycastMask = collision.heightMask,
					raycastCommands = nativeArray2
				}.Schedule(this.dependencyTracker);
				if (maxHits > 1)
				{
					float minStep = characterHeight * 0.5f;
					JobHandle dependsOn = new JobRaycastAll(nativeArray2, this.heightHits, Physics.defaultPhysicsScene, maxHits, allocator, this.dependencyTracker, minStep).Schedule(dependency);
					return new JobMaxHitCount
					{
						hits = this.heightHits,
						maxHits = maxHits,
						layerStride = num,
						maxHitCount = outLayerCount
					}.Schedule(dependsOn);
				}
				this.dependencyTracker.ScheduleBatch(nativeArray2, this.heightHits, 2048);
				outLayerCount[0] = 1;
				return default(JobHandle);
			}
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0004F604 File Offset: 0x0004D804
		public void CopyHits(IntBounds recalculationBounds)
		{
			this.nodes.normals.MemSet(float4.zero).Schedule(this.dependencyTracker);
			new JobCopyHits
			{
				hits = this.heightHits,
				points = this.nodes.positions,
				normals = this.nodes.normals,
				slice = new Slice3D(this.nodes.bounds, recalculationBounds)
			}.Schedule(this.dependencyTracker);
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0004F690 File Offset: 0x0004D890
		public void CalculateWalkabilityFromHeightData(bool useRaycastNormal, bool unwalkableWhenNoGround, float maxSlope, float characterHeight)
		{
			new JobNodeWalkability
			{
				useRaycastNormal = useRaycastNormal,
				unwalkableWhenNoGround = unwalkableWhenNoGround,
				maxSlope = maxSlope,
				up = this.up,
				nodeNormals = this.nodes.normals,
				nodeWalkable = this.nodes.walkable,
				nodePositions = this.nodes.positions.Reinterpret<float3>(),
				characterHeight = characterHeight,
				layerStride = this.nodes.bounds.size.x * this.nodes.bounds.size.z
			}.Schedule(this.dependencyTracker);
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0004F750 File Offset: 0x0004D950
		public IEnumerator<JobHandle> CollisionCheck(GraphCollision collision, IntBounds calculationBounds)
		{
			if (collision.type == ColliderType.Ray && !collision.use2D)
			{
				NativeArray<bool> nativeArray = this.dependencyTracker.NewNativeArray<bool>(this.nodes.numNodes, this.nodes.allocationMethod, NativeArrayOptions.UninitializedMemory);
				collision.JobCollisionRay(this.nodes.positions, nativeArray, this.up, this.nodes.allocationMethod, this.dependencyTracker);
				this.nodes.walkable.BitwiseAndWith(nativeArray).WithLength(this.nodes.numNodes).Schedule(this.dependencyTracker);
				return null;
			}
			return new JobCheckCollisions
			{
				nodePositions = this.nodes.positions,
				collisionResult = this.nodes.walkable,
				collision = collision
			}.ExecuteMainThreadJob(this.dependencyTracker);
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0004F82C File Offset: 0x0004DA2C
		public void Connections(float maxStepHeight, bool maxStepUsesSlope, IntBounds calculationBounds, NumNeighbours neighbours, bool cutCorners, bool use2D, bool useErodedWalkability, float characterHeight)
		{
			JobCalculateGridConnections jobCalculateGridConnections = new JobCalculateGridConnections
			{
				maxStepHeight = maxStepHeight,
				maxStepUsesSlope = maxStepUsesSlope,
				graphToWorld = this.transform.matrix,
				bounds = calculationBounds.Offset(-this.nodes.bounds.min),
				arrayBounds = this.nodes.bounds.size,
				neighbours = neighbours,
				use2D = use2D,
				cutCorners = cutCorners,
				nodeWalkable = (useErodedWalkability ? this.nodes.walkableWithErosion : this.nodes.walkable).AsUnsafeSpanNoChecks<bool>(),
				nodePositions = this.nodes.positions.AsUnsafeSpanNoChecks<Vector3>(),
				nodeNormals = this.nodes.normals.AsUnsafeSpanNoChecks<float4>(),
				nodeConnections = this.nodes.connections.AsUnsafeSpanNoChecks<ulong>(),
				characterHeight = characterHeight,
				layeredDataLayout = this.nodes.layeredDataLayout
			};
			if (this.dependencyTracker != null)
			{
				jobCalculateGridConnections.ScheduleBatch(calculationBounds.size.z, 20, this.dependencyTracker, default(JobHandle));
			}
			else
			{
				jobCalculateGridConnections.RunBatch(calculationBounds.size.z);
			}
			if (this.nodes.layeredDataLayout)
			{
				JobFilterDiagonalConnections jobFilterDiagonalConnections = new JobFilterDiagonalConnections
				{
					slice = new Slice3D(this.nodes.bounds, calculationBounds),
					neighbours = neighbours,
					cutCorners = cutCorners,
					nodeConnections = this.nodes.connections.AsUnsafeSpanNoChecks<ulong>()
				};
				if (this.dependencyTracker != null)
				{
					jobFilterDiagonalConnections.ScheduleBatch(calculationBounds.size.z, 20, this.dependencyTracker, default(JobHandle));
					return;
				}
				jobFilterDiagonalConnections.RunBatch(calculationBounds.size.z);
			}
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0004FA24 File Offset: 0x0004DC24
		public void Erosion(NumNeighbours neighbours, int erodeIterations, IntBounds erosionWriteMask, bool erosionUsesTags, int erosionStartTag, int erosionTagsPrecedenceMask)
		{
			if (!this.nodes.layeredDataLayout)
			{
				new JobErosion<FlatGridAdjacencyMapper>
				{
					bounds = this.nodes.bounds,
					writeMask = erosionWriteMask,
					neighbours = neighbours,
					nodeConnections = this.nodes.connections,
					erosion = erodeIterations,
					nodeWalkable = this.nodes.walkable,
					outNodeWalkable = this.nodes.walkableWithErosion,
					nodeTags = this.nodes.tags,
					erosionUsesTags = erosionUsesTags,
					erosionStartTag = erosionStartTag,
					erosionTagsPrecedenceMask = erosionTagsPrecedenceMask
				}.Schedule(this.dependencyTracker);
				return;
			}
			new JobErosion<LayeredGridAdjacencyMapper>
			{
				bounds = this.nodes.bounds,
				writeMask = erosionWriteMask,
				neighbours = neighbours,
				nodeConnections = this.nodes.connections,
				erosion = erodeIterations,
				nodeWalkable = this.nodes.walkable,
				outNodeWalkable = this.nodes.walkableWithErosion,
				nodeTags = this.nodes.tags,
				erosionUsesTags = erosionUsesTags,
				erosionStartTag = erosionStartTag,
				erosionTagsPrecedenceMask = erosionTagsPrecedenceMask
			}.Schedule(this.dependencyTracker);
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0004FB88 File Offset: 0x0004DD88
		public unsafe void AssignNodeConnections(GridNodeBase[] nodes, int3 nodeArrayBounds, IntBounds writeBounds)
		{
			IntBounds bounds = this.nodes.bounds;
			int3 @int = writeBounds.min - bounds.min;
			UnsafeSpan<ulong> unsafeSpan = this.nodes.connections.AsUnsafeReadOnlySpan<ulong>();
			for (int i = 0; i < writeBounds.size.y; i++)
			{
				int num = (i + writeBounds.min.y) * nodeArrayBounds.x * nodeArrayBounds.z;
				for (int j = 0; j < writeBounds.size.z; j++)
				{
					int num2 = num + (j + writeBounds.min.z) * nodeArrayBounds.x + writeBounds.min.x;
					int num3 = (i + @int.y) * bounds.size.x * bounds.size.z + (j + @int.z) * bounds.size.x + @int.x;
					for (int k = 0; k < writeBounds.size.x; k++)
					{
						GridNodeBase gridNodeBase = nodes[num2 + k];
						int index = num3 + k;
						ulong num4 = *unsafeSpan[index];
						if (gridNodeBase != null)
						{
							LevelGridNode levelGridNode = gridNodeBase as LevelGridNode;
							if (levelGridNode != null)
							{
								levelGridNode.SetAllConnectionInternal(num4);
							}
							else
							{
								(gridNodeBase as GridNode).SetAllConnectionInternal((int)num4);
							}
						}
					}
				}
			}
		}

		// Token: 0x04000952 RID: 2386
		public JobDependencyTracker dependencyTracker;

		// Token: 0x04000953 RID: 2387
		public Vector3 up;

		// Token: 0x04000954 RID: 2388
		public GraphTransform transform;

		// Token: 0x04000955 RID: 2389
		public GridGraphNodeData nodes;

		// Token: 0x04000956 RID: 2390
		public NativeArray<RaycastHit> heightHits;

		// Token: 0x04000957 RID: 2391
		public IntBounds heightHitsBounds;
	}
}
