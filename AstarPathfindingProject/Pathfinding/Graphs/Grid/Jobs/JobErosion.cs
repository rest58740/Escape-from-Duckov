using System;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000218 RID: 536
	[BurstCompile]
	public struct JobErosion<AdjacencyMapper> : IJob where AdjacencyMapper : GridAdjacencyMapper, new()
	{
		// Token: 0x06000D09 RID: 3337 RVA: 0x00051FB4 File Offset: 0x000501B4
		public void Execute()
		{
			Slice3D slice3D = new Slice3D(this.bounds, this.bounds);
			int3 size = slice3D.slice.size;
			slice3D.AssertMatchesOuter<ulong>(this.nodeConnections);
			slice3D.AssertMatchesOuter<bool>(this.nodeWalkable);
			slice3D.AssertMatchesOuter<bool>(this.outNodeWalkable);
			slice3D.AssertMatchesOuter<int>(this.nodeTags);
			ValueTuple<int, int, int> outerStrides = slice3D.outerStrides;
			int item = outerStrides.Item1;
			int item2 = outerStrides.Item2;
			int item3 = outerStrides.Item3;
			ValueTuple<int, int, int> innerStrides = slice3D.innerStrides;
			int item4 = innerStrides.Item1;
			int item5 = innerStrides.Item2;
			int item6 = innerStrides.Item3;
			NativeArray<int> neighbourOffsets = new NativeArray<int>(8, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < 8; i++)
			{
				neighbourOffsets[i] = GridGraph.neighbourZOffsets[i] * item6 + GridGraph.neighbourXOffsets[i] * item4;
			}
			NativeArray<int> nativeArray = new NativeArray<int>(slice3D.length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			AdjacencyMapper adjacencyMapper = Activator.CreateInstance<AdjacencyMapper>();
			int num = adjacencyMapper.LayerCount(slice3D.slice);
			int outerStartIndex = slice3D.outerStartIndex;
			if (this.neighbours == NumNeighbours.Six)
			{
				for (int j = 1; j < size.z - 1; j++)
				{
					for (int k = 1; k < size.x - 1; k++)
					{
						for (int l = 0; l < num; l++)
						{
							int nodeIndex = j * item3 + k * item + l * item2 + outerStartIndex;
							int num2 = j * item6 + k * item4;
							int num3 = num2 + l * item5;
							int num4 = int.MaxValue;
							for (int m = 3; m < 6; m++)
							{
								int direction = JobErosion<AdjacencyMapper>.hexagonNeighbourIndices[m];
								if (!adjacencyMapper.HasConnection(nodeIndex, direction, this.nodeConnections))
								{
									num4 = -1;
								}
								else
								{
									num4 = math.min(num4, nativeArray[adjacencyMapper.GetNeighbourIndex(num2, num3, direction, this.nodeConnections, neighbourOffsets, item5)]);
								}
							}
							nativeArray[num3] = num4 + 1;
						}
					}
				}
				for (int n = size.z - 2; n > 0; n--)
				{
					for (int num5 = size.x - 2; num5 > 0; num5--)
					{
						for (int num6 = 0; num6 < num; num6++)
						{
							int nodeIndex2 = n * item3 + num5 * item + num6 * item2 + outerStartIndex;
							int num7 = n * item6 + num5 * item4;
							int num8 = num7 + num6 * item5;
							int num9 = int.MaxValue;
							for (int num10 = 3; num10 < 6; num10++)
							{
								int direction2 = JobErosion<AdjacencyMapper>.hexagonNeighbourIndices[num10];
								if (!adjacencyMapper.HasConnection(nodeIndex2, direction2, this.nodeConnections))
								{
									num9 = -1;
								}
								else
								{
									num9 = math.min(num9, nativeArray[adjacencyMapper.GetNeighbourIndex(num7, num8, direction2, this.nodeConnections, neighbourOffsets, item5)]);
								}
							}
							nativeArray[num8] = math.min(nativeArray[num8], num9 + 1);
						}
					}
				}
			}
			else
			{
				for (int num11 = 1; num11 < size.z - 1; num11++)
				{
					for (int num12 = 1; num12 < size.x - 1; num12++)
					{
						for (int num13 = 0; num13 < num; num13++)
						{
							int nodeIndex3 = num11 * item3 + num12 * item + num13 * item2 + outerStartIndex;
							int num14 = num11 * item6 + num12 * item4;
							int num15 = num14 + num13 * item5;
							int x = -1;
							if (adjacencyMapper.HasConnection(nodeIndex3, 0, this.nodeConnections))
							{
								x = nativeArray[adjacencyMapper.GetNeighbourIndex(num14, num15, 0, this.nodeConnections, neighbourOffsets, item5)];
							}
							int y = -1;
							if (adjacencyMapper.HasConnection(nodeIndex3, 3, this.nodeConnections))
							{
								y = nativeArray[adjacencyMapper.GetNeighbourIndex(num14, num15, 3, this.nodeConnections, neighbourOffsets, item5)];
							}
							nativeArray[num15] = math.min(x, y) + 1;
						}
					}
				}
				for (int num16 = size.z - 2; num16 > 0; num16--)
				{
					for (int num17 = size.x - 2; num17 > 0; num17--)
					{
						for (int num18 = 0; num18 < num; num18++)
						{
							int num19 = num16 * item3 + num17 * item + num18 * item2 + outerStartIndex;
							int num20 = num16 * item6 + num17 * item4;
							int num21 = num20 + num18 * item5;
							int x2 = -1;
							if (adjacencyMapper.HasConnection(num19, 2, this.nodeConnections))
							{
								x2 = nativeArray[adjacencyMapper.GetNeighbourIndex(num20, num21, 2, this.nodeConnections, neighbourOffsets, item5)];
							}
							int y2 = -1;
							if (adjacencyMapper.HasConnection(num19, 1, this.nodeConnections))
							{
								y2 = nativeArray[adjacencyMapper.GetNeighbourIndex(num20, num21, 1, this.nodeConnections, neighbourOffsets, item5)];
							}
							nativeArray[num21] = math.min(nativeArray[num19], math.min(x2, y2) + 1);
						}
					}
				}
			}
			IntBounds intBounds = this.writeMask.Offset(-this.bounds.min);
			for (int num22 = this.erosionStartTag; num22 < this.erosionStartTag + this.erosion; num22++)
			{
				this.erosionTagsPrecedenceMask |= 1 << num22;
			}
			for (int num23 = intBounds.min.y; num23 < intBounds.max.y; num23++)
			{
				for (int num24 = intBounds.min.z; num24 < intBounds.max.z; num24++)
				{
					for (int num25 = intBounds.min.x; num25 < intBounds.max.x; num25++)
					{
						int index = num25 * item + num23 * item2 + num24 * item3 + outerStartIndex;
						int index2 = num25 * item4 + num23 * item5 + num24 * item6;
						if (this.erosionUsesTags)
						{
							int num26 = this.nodeTags[index];
							this.outNodeWalkable[index] = this.nodeWalkable[index];
							if (nativeArray[index2] < this.erosion)
							{
								if ((this.erosionTagsPrecedenceMask >> num26 & 1) != 0)
								{
									this.nodeTags[index] = (this.nodeWalkable[index] ? math.min(31, nativeArray[index2] + this.erosionStartTag) : 0);
								}
							}
							else if (num26 >= this.erosionStartTag && num26 < this.erosionStartTag + this.erosion)
							{
								this.nodeTags[index] = 0;
							}
						}
						else
						{
							this.outNodeWalkable[index] = (this.nodeWalkable[index] & nativeArray[index2] >= this.erosion);
						}
					}
				}
			}
		}

		// Token: 0x040009CC RID: 2508
		public IntBounds bounds;

		// Token: 0x040009CD RID: 2509
		public IntBounds writeMask;

		// Token: 0x040009CE RID: 2510
		public NumNeighbours neighbours;

		// Token: 0x040009CF RID: 2511
		public int erosion;

		// Token: 0x040009D0 RID: 2512
		public bool erosionUsesTags;

		// Token: 0x040009D1 RID: 2513
		public int erosionStartTag;

		// Token: 0x040009D2 RID: 2514
		[ReadOnly]
		public NativeArray<ulong> nodeConnections;

		// Token: 0x040009D3 RID: 2515
		[ReadOnly]
		public NativeArray<bool> nodeWalkable;

		// Token: 0x040009D4 RID: 2516
		[WriteOnly]
		public NativeArray<bool> outNodeWalkable;

		// Token: 0x040009D5 RID: 2517
		public NativeArray<int> nodeTags;

		// Token: 0x040009D6 RID: 2518
		public int erosionTagsPrecedenceMask;

		// Token: 0x040009D7 RID: 2519
		private static readonly int[] hexagonNeighbourIndices = new int[]
		{
			1,
			2,
			5,
			0,
			3,
			7
		};
	}
}
