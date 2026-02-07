using System;
using Pathfinding.Collections;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000219 RID: 537
	[BurstCompile]
	public struct JobFilterDiagonalConnections : IJobParallelForBatched
	{
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x000185BF File Offset: 0x000167BF
		public bool allowBoundsChecks
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x000526F8 File Offset: 0x000508F8
		public unsafe void Execute(int start, int count)
		{
			this.slice.AssertMatchesOuter<ulong>(this.nodeConnections);
			int3 outerSize = this.slice.outerSize;
			NativeArray<int> nativeArray = new NativeArray<int>(8, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < 8; i++)
			{
				nativeArray[i] = GridGraph.neighbourZOffsets[i] * outerSize.x + GridGraph.neighbourXOffsets[i];
			}
			ulong num = 0UL;
			for (int j = 0; j < GridGraph.hexagonNeighbourIndices.Length; j++)
			{
				num |= 15UL << 4 * GridGraph.hexagonNeighbourIndices[j];
			}
			int num2 = this.cutCorners ? 1 : 2;
			int num3 = outerSize.x * outerSize.z;
			start += this.slice.slice.min.z;
			for (int k = this.slice.slice.min.y; k < this.slice.slice.max.y; k++)
			{
				for (int l = start; l < start + count; l++)
				{
					for (int m = this.slice.slice.min.x; m < this.slice.slice.max.x; m++)
					{
						int num4 = l * outerSize.x + m;
						int index = num4 + k * num3;
						switch (this.neighbours)
						{
						case NumNeighbours.Four:
							*this.nodeConnections[index] = (*this.nodeConnections[index] | (ulong)-65536);
							break;
						case NumNeighbours.Eight:
						{
							ulong num5 = *this.nodeConnections[index];
							if (num5 != (ulong)-1)
							{
								for (int n = 0; n < 4; n++)
								{
									int num6 = 0;
									ulong num7 = num5 >> n * 4 & 15UL;
									ulong num8 = num5 >> (n + 1) % 4 * 4 & 15UL;
									ulong num9 = num5 >> (n + 4) * 4 & 15UL;
									if (num9 != 15UL)
									{
										if (num7 != 15UL)
										{
											int num10 = (n + 1) % 4;
											int index2 = num4 + nativeArray[n] + (int)num7 * num3;
											if ((*this.nodeConnections[index2] >> num10 * 4 & 15UL) == num9)
											{
												num6++;
											}
										}
										if (num8 != 15UL)
										{
											int num11 = n;
											int index3 = num4 + nativeArray[(n + 1) % 4] + (int)num8 * num3;
											if ((*this.nodeConnections[index3] >> num11 * 4 & 15UL) == num9)
											{
												num6++;
											}
										}
										if (num6 < num2)
										{
											num5 |= 15UL << (n + 4) * 4;
										}
									}
								}
								*this.nodeConnections[index] = num5;
							}
							break;
						}
						case NumNeighbours.Six:
							*this.nodeConnections[index] = ((*this.nodeConnections[index] | ~num) & (ulong)-1);
							break;
						}
					}
				}
			}
		}

		// Token: 0x040009D8 RID: 2520
		public Slice3D slice;

		// Token: 0x040009D9 RID: 2521
		public NumNeighbours neighbours;

		// Token: 0x040009DA RID: 2522
		public bool cutCorners;

		// Token: 0x040009DB RID: 2523
		public UnsafeSpan<ulong> nodeConnections;
	}
}
