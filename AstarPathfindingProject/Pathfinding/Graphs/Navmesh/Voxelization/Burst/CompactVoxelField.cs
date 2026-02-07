using System;
using Pathfinding.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001CC RID: 460
	public struct CompactVoxelField : IArenaDisposable
	{
		// Token: 0x06000C2B RID: 3115 RVA: 0x0004786C File Offset: 0x00045A6C
		public CompactVoxelField(int width, int depth, int voxelWalkableHeight, Allocator allocator)
		{
			this.spans = new NativeList<CompactVoxelSpan>(0, allocator);
			this.cells = new NativeList<CompactVoxelCell>(0, allocator);
			this.areaTypes = new NativeList<int>(0, allocator);
			this.width = width;
			this.depth = depth;
			this.voxelWalkableHeight = voxelWalkableHeight;
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x000478C7 File Offset: 0x00045AC7
		void IArenaDisposable.DisposeWith(DisposeArena arena)
		{
			arena.Add<CompactVoxelSpan>(this.spans);
			arena.Add<CompactVoxelCell>(this.cells);
			arena.Add<int>(this.areaTypes);
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x000478ED File Offset: 0x00045AED
		public int GetNeighbourIndex(int index, int direction)
		{
			return index + VoxelUtilityBurst.DX[direction] + VoxelUtilityBurst.DZ[direction] * this.width;
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x00047908 File Offset: 0x00045B08
		public void BuildFromLinkedField(LinkedVoxelField field)
		{
			int num = 0;
			int num2 = field.width;
			int num3 = field.depth;
			int num4 = num2 * num3;
			int spanCount = field.GetSpanCount();
			this.spans.Resize(spanCount, NativeArrayOptions.UninitializedMemory);
			this.areaTypes.Resize(spanCount, NativeArrayOptions.UninitializedMemory);
			this.cells.Resize(num4, NativeArrayOptions.UninitializedMemory);
			NativeList<LinkedVoxelSpan> linkedSpans = field.linkedSpans;
			for (int i = 0; i < num4; i += num2)
			{
				for (int j = 0; j < num2; j++)
				{
					int num5 = j + i;
					if (linkedSpans[num5].bottom == 4294967295U)
					{
						this.cells[j + i] = new CompactVoxelCell(0, 0);
					}
					else
					{
						int i2 = num;
						int num6 = 0;
						while (num5 != -1)
						{
							if (linkedSpans[num5].area != 0)
							{
								int top = (int)linkedSpans[num5].top;
								int next = linkedSpans[num5].next;
								int num7 = (int)((next != -1) ? linkedSpans[next].bottom : 65536U);
								this.spans[num] = new CompactVoxelSpan((ushort)math.min(top, 65535), (uint)math.min(num7 - top, 65535));
								this.areaTypes[num] = linkedSpans[num5].area;
								num++;
								num6++;
							}
							num5 = linkedSpans[num5].next;
						}
						this.cells[j + i] = new CompactVoxelCell(i2, num6);
					}
				}
			}
		}

		// Token: 0x04000858 RID: 2136
		public const int UnwalkableArea = 0;

		// Token: 0x04000859 RID: 2137
		public const uint NotConnected = 63U;

		// Token: 0x0400085A RID: 2138
		public readonly int voxelWalkableHeight;

		// Token: 0x0400085B RID: 2139
		public readonly int width;

		// Token: 0x0400085C RID: 2140
		public readonly int depth;

		// Token: 0x0400085D RID: 2141
		public NativeList<CompactVoxelSpan> spans;

		// Token: 0x0400085E RID: 2142
		public NativeList<CompactVoxelCell> cells;

		// Token: 0x0400085F RID: 2143
		public NativeList<int> areaTypes;

		// Token: 0x04000860 RID: 2144
		public const int MaxLayers = 65535;
	}
}
