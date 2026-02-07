using System;
using Pathfinding.Collections;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000214 RID: 532
	[BurstCompile(FloatMode = FloatMode.Fast, CompileSynchronously = true)]
	public struct JobCalculateGridConnections : IJobParallelForBatched
	{
		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000CFC RID: 3324 RVA: 0x000185BF File Offset: 0x000167BF
		public bool allowBoundsChecks
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0005168C File Offset: 0x0004F88C
		public static bool IsValidConnection(float y, float y2, float maxStepHeight)
		{
			return math.abs(y - y2) <= maxStepHeight;
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x0005169C File Offset: 0x0004F89C
		public static bool IsValidConnection(float2 yRange, float2 yRange2, float maxStepHeight, float characterHeight)
		{
			if (!JobCalculateGridConnections.IsValidConnection(yRange.x, yRange2.x, maxStepHeight))
			{
				return false;
			}
			float num = math.max(yRange.x, yRange2.x);
			return math.min(yRange.y, yRange2.y) - num >= characterHeight;
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x000516EC File Offset: 0x0004F8EC
		private unsafe static float ConnectionY(UnsafeSpan<float3> nodePositions, UnsafeSpan<float4> nodeNormals, NativeArray<float4> normalToHeightOffset, int nodeIndex, int dir, float4 up, bool reverse)
		{
			Hint.Assume(nodeIndex >= 0 && (long)nodeIndex < (long)((ulong)nodePositions.length));
			Hint.Assume(nodeIndex >= 0 && (long)nodeIndex < (long)((ulong)nodeNormals.length));
			Hint.Assume(dir >= 0 && dir < normalToHeightOffset.Length);
			float4 y = new float4(*nodePositions[(uint)nodeIndex], 0f);
			return math.dot(up, y) + (float)(reverse ? -1 : 1) * math.dot(*nodeNormals[nodeIndex], normalToHeightOffset[dir]);
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x00051788 File Offset: 0x0004F988
		private unsafe static float2 ConnectionYRange(UnsafeSpan<float3> nodePositions, UnsafeSpan<float4> nodeNormals, NativeArray<float4> normalToHeightOffset, int nodeIndex, int layerStride, int y, int maxY, int dir, float4 up, bool reverse)
		{
			float x = JobCalculateGridConnections.ConnectionY(nodePositions, nodeNormals, normalToHeightOffset, nodeIndex, dir, up, reverse);
			int num = nodeIndex + layerStride;
			float y2;
			if (num < (int)nodeNormals.length && math.any(*nodeNormals[(uint)num]))
			{
				y2 = JobCalculateGridConnections.ConnectionY(nodePositions, nodeNormals, normalToHeightOffset, num, dir, up, reverse);
			}
			else
			{
				y2 = float.PositiveInfinity;
			}
			return new float2(x, y2);
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x000517E4 File Offset: 0x0004F9E4
		private static NativeArray<float4> HeightOffsetProjections(float4x4 graphToWorldTranform, bool maxStepUsesSlope)
		{
			NativeArray<float4> result = new NativeArray<float4>(8, Allocator.Temp, NativeArrayOptions.ClearMemory);
			if (maxStepUsesSlope)
			{
				for (int i = 0; i < result.Length; i++)
				{
					float3 xyz = (float)GridGraph.neighbourXOffsets[i] * graphToWorldTranform.c0.xyz + (float)GridGraph.neighbourZOffsets[i] * graphToWorldTranform.c2.xyz;
					result[i] = -new float4(xyz, 0f) * 0.5f;
				}
			}
			return result;
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x0005186C File Offset: 0x0004FA6C
		public void Execute(int start, int count)
		{
			if (this.nodePositions.Length != this.nodeNormals.Length)
			{
				throw new Exception("nodePositions and nodeNormals must have the same length");
			}
			if (this.nodePositions.Length != this.nodeWalkable.Length)
			{
				throw new Exception("nodePositions and nodeWalkable must have the same length");
			}
			if (this.nodePositions.Length != this.nodeConnections.Length)
			{
				throw new Exception("nodePositions and nodeConnections must have the same length");
			}
			if (this.layeredDataLayout)
			{
				this.ExecuteLayered(start, count);
				return;
			}
			this.ExecuteFlat(start, count);
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x000518FC File Offset: 0x0004FAFC
		public unsafe void ExecuteFlat(int start, int count)
		{
			if (this.maxStepHeight <= 0f || this.use2D)
			{
				this.maxStepHeight = float.PositiveInfinity;
			}
			float4 c = this.graphToWorld.c1;
			NativeArray<int> nativeArray = new NativeArray<int>(8, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < 8; i++)
			{
				nativeArray[i] = GridGraph.neighbourZOffsets[i] * this.arrayBounds.x + GridGraph.neighbourXOffsets[i];
			}
			UnsafeSpan<float3> unsafeSpan = this.nodePositions.Reinterpret<float3>();
			NativeArray<float4> normalToHeightOffset = JobCalculateGridConnections.HeightOffsetProjections(this.graphToWorld, this.maxStepUsesSlope);
			start += this.bounds.min.z;
			for (int j = start; j < start + count; j++)
			{
				int num = 255;
				if (j == 0)
				{
					num &= -146;
				}
				if (j == this.arrayBounds.z - 1)
				{
					num &= -101;
				}
				for (int k = this.bounds.min.x; k < this.bounds.max.x; k++)
				{
					int num2 = j * this.arrayBounds.x + k;
					if (!(*this.nodeWalkable[num2]))
					{
						*this.nodeConnections[num2] = 0UL;
					}
					else
					{
						int num3 = num;
						if (k == 0)
						{
							num3 &= -201;
						}
						if (k == this.arrayBounds.x - 1)
						{
							num3 &= -51;
						}
						for (int l = 0; l < 8; l++)
						{
							float y = JobCalculateGridConnections.ConnectionY(unsafeSpan, this.nodeNormals, normalToHeightOffset, num2, l, c, false);
							int num4 = num2 + nativeArray[l];
							if ((num3 & 1 << l) != 0)
							{
								float y2 = JobCalculateGridConnections.ConnectionY(unsafeSpan, this.nodeNormals, normalToHeightOffset, num4, l, c, true);
								if (!(*this.nodeWalkable[num4]) || !JobCalculateGridConnections.IsValidConnection(y, y2, this.maxStepHeight))
								{
									num3 &= ~(1 << l);
								}
							}
						}
						*this.nodeConnections[num2] = (ulong)((long)GridNode.FilterDiagonalConnections(num3, this.neighbours, this.cutCorners));
					}
				}
			}
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x00051B28 File Offset: 0x0004FD28
		public unsafe void ExecuteLayered(int start, int count)
		{
			if (this.maxStepHeight <= 0f || this.use2D)
			{
				this.maxStepHeight = float.PositiveInfinity;
			}
			float4 c = this.graphToWorld.c1;
			NativeArray<int> nativeArray = new NativeArray<int>(8, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < 8; i++)
			{
				nativeArray[i] = GridGraph.neighbourZOffsets[i] * this.arrayBounds.x + GridGraph.neighbourXOffsets[i];
			}
			UnsafeSpan<float3> unsafeSpan = this.nodePositions.Reinterpret<float3>();
			NativeArray<float4> normalToHeightOffset = JobCalculateGridConnections.HeightOffsetProjections(this.graphToWorld, this.maxStepUsesSlope);
			int num = this.arrayBounds.z * this.arrayBounds.x;
			start += this.bounds.min.z;
			for (int j = this.bounds.min.y; j < this.bounds.max.y; j++)
			{
				for (int k = start; k < start + count; k++)
				{
					for (int l = this.bounds.min.x; l < this.bounds.max.x; l++)
					{
						ulong num2 = 0UL;
						int num3 = k * this.arrayBounds.x + l;
						int num4 = num3 + j * num;
						if (*this.nodeWalkable[num4])
						{
							for (int m = 0; m < 8; m++)
							{
								int num5 = l + GridGraph.neighbourXOffsets[m];
								int num6 = k + GridGraph.neighbourZOffsets[m];
								int num7 = 15;
								if (num5 >= 0 && num6 >= 0 && num5 < this.arrayBounds.x && num6 < this.arrayBounds.z)
								{
									float2 yRange = JobCalculateGridConnections.ConnectionYRange(unsafeSpan, this.nodeNormals, normalToHeightOffset, num4, num, j, this.arrayBounds.y, m, c, false);
									int num8 = num3 + nativeArray[m];
									for (int n = 0; n < this.arrayBounds.y; n++)
									{
										int num9 = num8 + n * num;
										if (*this.nodeWalkable[num9])
										{
											float2 yRange2 = JobCalculateGridConnections.ConnectionYRange(unsafeSpan, this.nodeNormals, normalToHeightOffset, num9, num, n, this.arrayBounds.y, m, c, true);
											if (JobCalculateGridConnections.IsValidConnection(yRange, yRange2, this.maxStepHeight, this.characterHeight))
											{
												num7 = n;
												break;
											}
										}
									}
								}
								num2 |= (ulong)((ulong)((long)num7) << 4 * m);
							}
						}
						else
						{
							num2 = (ulong)-1;
						}
						*this.nodeConnections[num4] = num2;
					}
				}
			}
		}

		// Token: 0x040009B4 RID: 2484
		public float maxStepHeight;

		// Token: 0x040009B5 RID: 2485
		public float4x4 graphToWorld;

		// Token: 0x040009B6 RID: 2486
		public IntBounds bounds;

		// Token: 0x040009B7 RID: 2487
		public int3 arrayBounds;

		// Token: 0x040009B8 RID: 2488
		public NumNeighbours neighbours;

		// Token: 0x040009B9 RID: 2489
		public float characterHeight;

		// Token: 0x040009BA RID: 2490
		public bool use2D;

		// Token: 0x040009BB RID: 2491
		public bool cutCorners;

		// Token: 0x040009BC RID: 2492
		public bool maxStepUsesSlope;

		// Token: 0x040009BD RID: 2493
		public bool layeredDataLayout;

		// Token: 0x040009BE RID: 2494
		[ReadOnly]
		public UnsafeSpan<bool> nodeWalkable;

		// Token: 0x040009BF RID: 2495
		[ReadOnly]
		public UnsafeSpan<float4> nodeNormals;

		// Token: 0x040009C0 RID: 2496
		[ReadOnly]
		public UnsafeSpan<Vector3> nodePositions;

		// Token: 0x040009C1 RID: 2497
		[WriteOnly]
		public UnsafeSpan<ulong> nodeConnections;
	}
}
