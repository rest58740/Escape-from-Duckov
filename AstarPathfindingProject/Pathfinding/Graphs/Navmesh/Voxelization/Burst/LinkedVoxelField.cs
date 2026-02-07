using System;
using Pathfinding.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001D0 RID: 464
	public struct LinkedVoxelField : IArenaDisposable
	{
		// Token: 0x06000C33 RID: 3123 RVA: 0x00047B18 File Offset: 0x00045D18
		public LinkedVoxelField(int width, int depth, int height)
		{
			this.width = width;
			this.depth = depth;
			this.height = height;
			this.flatten = true;
			this.linkedSpans = new NativeList<LinkedVoxelSpan>(0, Allocator.Persistent);
			this.removedStack = new NativeList<int>(128, Allocator.Persistent);
			this.linkedCellMinMax = new NativeList<CellMinMax>(0, Allocator.Persistent);
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x00047B7B File Offset: 0x00045D7B
		void IArenaDisposable.DisposeWith(DisposeArena arena)
		{
			arena.Add<LinkedVoxelSpan>(this.linkedSpans);
			arena.Add<int>(this.removedStack);
			arena.Add<CellMinMax>(this.linkedCellMinMax);
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x00047BA4 File Offset: 0x00045DA4
		public void ResetLinkedVoxelSpans()
		{
			int num = this.width * this.depth;
			LinkedVoxelSpan value = new LinkedVoxelSpan(uint.MaxValue, uint.MaxValue, -1, -1);
			this.linkedSpans.ResizeUninitialized(num);
			this.linkedCellMinMax.Resize(num, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < num; i++)
			{
				this.linkedSpans[i] = value;
				this.linkedCellMinMax[i] = new CellMinMax
				{
					objectID = -1,
					min = 0,
					max = 0
				};
			}
			this.removedStack.Clear();
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x00047C34 File Offset: 0x00045E34
		private void PushToSpanRemovedStack(int index)
		{
			this.removedStack.Add(index);
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x00047C44 File Offset: 0x00045E44
		public int GetSpanCount()
		{
			int num = 0;
			int num2 = this.width * this.depth;
			for (int i = 0; i < num2; i++)
			{
				int num3 = i;
				while (num3 != -1 && this.linkedSpans[num3].bottom != 4294967295U)
				{
					num += ((this.linkedSpans[num3].area != 0) ? 1 : 0);
					num3 = this.linkedSpans[num3].next;
				}
			}
			return num;
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00047CB8 File Offset: 0x00045EB8
		public void ResolveSolid(int index, int objectID, int voxelWalkableClimb)
		{
			CellMinMax cellMinMax = this.linkedCellMinMax[index];
			if (cellMinMax.objectID != objectID)
			{
				return;
			}
			if (cellMinMax.min < cellMinMax.max - 1)
			{
				this.AddLinkedSpan(index, cellMinMax.min, cellMinMax.max - 1, 0, voxelWalkableClimb, objectID);
			}
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x00047D04 File Offset: 0x00045F04
		public void SetWalkableBackground()
		{
			int num = this.width * this.depth;
			for (int i = 0; i < num; i++)
			{
				this.linkedSpans[i] = new LinkedVoxelSpan(0U, 1U, 1);
			}
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x00047D40 File Offset: 0x00045F40
		public void AddFlattenedSpan(int index, int area)
		{
			if (this.linkedSpans[index].bottom == 4294967295U)
			{
				this.linkedSpans[index] = new LinkedVoxelSpan(0U, 1U, area);
				return;
			}
			this.linkedSpans[index] = new LinkedVoxelSpan(0U, 1U, (this.linkedSpans[index].area == 0 || area == 0) ? 0 : math.max(this.linkedSpans[index].area, area));
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x00047DBC File Offset: 0x00045FBC
		public void AddLinkedSpan(int index, int bottom, int top, int area, int voxelWalkableClimb, int objectID)
		{
			CellMinMax cellMinMax = this.linkedCellMinMax[index];
			if (cellMinMax.objectID != objectID)
			{
				this.linkedCellMinMax[index] = new CellMinMax
				{
					objectID = objectID,
					min = bottom,
					max = top
				};
			}
			else
			{
				cellMinMax.min = math.min(cellMinMax.min, bottom);
				cellMinMax.max = math.max(cellMinMax.max, top);
				this.linkedCellMinMax[index] = cellMinMax;
			}
			top = math.min(top, this.height);
			bottom = math.max(bottom, 0);
			if (bottom >= top)
			{
				return;
			}
			uint num = (uint)top;
			uint num2 = (uint)bottom;
			if (this.linkedSpans[index].bottom == 4294967295U)
			{
				this.linkedSpans[index] = new LinkedVoxelSpan(num2, num, area);
				return;
			}
			int num3 = -1;
			int index2 = index;
			while (index != -1)
			{
				LinkedVoxelSpan linkedVoxelSpan = this.linkedSpans[index];
				if (linkedVoxelSpan.bottom > num)
				{
					break;
				}
				if (linkedVoxelSpan.top < num2)
				{
					num3 = index;
					index = linkedVoxelSpan.next;
				}
				else
				{
					if (math.abs((int)(num - linkedVoxelSpan.top)) < voxelWalkableClimb && (area == 0 || linkedVoxelSpan.area == 0))
					{
						area = math.max(area, linkedVoxelSpan.area);
					}
					else if (num < linkedVoxelSpan.top)
					{
						area = linkedVoxelSpan.area;
					}
					num2 = math.min(linkedVoxelSpan.bottom, num2);
					num = math.max(linkedVoxelSpan.top, num);
					int next = linkedVoxelSpan.next;
					if (num3 != -1)
					{
						LinkedVoxelSpan value = this.linkedSpans[num3];
						value.next = next;
						this.linkedSpans[num3] = value;
						this.PushToSpanRemovedStack(index);
						index = next;
					}
					else
					{
						if (next == -1)
						{
							this.linkedSpans[index2] = new LinkedVoxelSpan(num2, num, area);
							return;
						}
						this.linkedSpans[index2] = this.linkedSpans[next];
						this.PushToSpanRemovedStack(next);
					}
				}
			}
			int num4;
			if (this.removedStack.Length > 0)
			{
				num4 = this.removedStack[this.removedStack.Length - 1];
				this.removedStack.RemoveAtSwapBack(this.removedStack.Length - 1);
			}
			else
			{
				num4 = this.linkedSpans.Length;
				this.linkedSpans.Resize(this.linkedSpans.Length + 1, NativeArrayOptions.UninitializedMemory);
			}
			if (num3 != -1)
			{
				this.linkedSpans[num4] = new LinkedVoxelSpan(num2, num, area, this.linkedSpans[num3].next);
				LinkedVoxelSpan value2 = this.linkedSpans[num3];
				value2.next = num4;
				this.linkedSpans[num3] = value2;
				return;
			}
			this.linkedSpans[num4] = this.linkedSpans[index2];
			this.linkedSpans[index2] = new LinkedVoxelSpan(num2, num, area, num4);
		}

		// Token: 0x0400086A RID: 2154
		public const uint MaxHeight = 65536U;

		// Token: 0x0400086B RID: 2155
		public const int MaxHeightInt = 65536;

		// Token: 0x0400086C RID: 2156
		public const uint InvalidSpanValue = 4294967295U;

		// Token: 0x0400086D RID: 2157
		public const float AvgSpanLayerCountEstimate = 8f;

		// Token: 0x0400086E RID: 2158
		public int width;

		// Token: 0x0400086F RID: 2159
		public int depth;

		// Token: 0x04000870 RID: 2160
		public int height;

		// Token: 0x04000871 RID: 2161
		public bool flatten;

		// Token: 0x04000872 RID: 2162
		public NativeList<LinkedVoxelSpan> linkedSpans;

		// Token: 0x04000873 RID: 2163
		private NativeList<int> removedStack;

		// Token: 0x04000874 RID: 2164
		private NativeList<CellMinMax> linkedCellMinMax;
	}
}
