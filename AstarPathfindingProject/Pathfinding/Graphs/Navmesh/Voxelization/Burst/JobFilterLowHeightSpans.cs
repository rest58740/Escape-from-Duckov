using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001DC RID: 476
	[BurstCompile(CompileSynchronously = true)]
	internal struct JobFilterLowHeightSpans : IJob
	{
		// Token: 0x06000C62 RID: 3170 RVA: 0x0004AFEC File Offset: 0x000491EC
		public void Execute()
		{
			int num = this.field.width * this.field.depth;
			NativeList<LinkedVoxelSpan> linkedSpans = this.field.linkedSpans;
			int i = 0;
			int num2 = 0;
			while (i < num)
			{
				for (int j = 0; j < this.field.width; j++)
				{
					int num3 = i + j;
					while (num3 != -1 && linkedSpans[num3].bottom != 4294967295U)
					{
						uint top = linkedSpans[num3].top;
						if (((linkedSpans[num3].next != -1) ? linkedSpans[linkedSpans[num3].next].bottom : 65536U) - top < this.voxelWalkableHeight)
						{
							LinkedVoxelSpan value = linkedSpans[num3];
							value.area = 0;
							linkedSpans[num3] = value;
						}
						num3 = linkedSpans[num3].next;
					}
				}
				i += this.field.width;
				num2++;
			}
		}

		// Token: 0x040008A8 RID: 2216
		public LinkedVoxelField field;

		// Token: 0x040008A9 RID: 2217
		public uint voxelWalkableHeight;
	}
}
