using System;
using Pathfinding.Collections;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000213 RID: 531
	public struct JobAllocateNodes : IJob
	{
		// Token: 0x06000CFB RID: 3323 RVA: 0x0005154C File Offset: 0x0004F74C
		public unsafe void Execute()
		{
			int3 size = this.dataBounds.size;
			UnsafeSpan<float4> unsafeSpan = this.nodeNormals.AsUnsafeReadOnlySpan<float4>();
			for (int i = 1; i < size.y; i++)
			{
				for (int j = 0; j < size.z; j++)
				{
					int num = ((i + this.dataBounds.min.y) * this.nodeArrayBounds.z + (j + this.dataBounds.min.z)) * this.nodeArrayBounds.x + this.dataBounds.min.x;
					for (int k = 0; k < size.x; k++)
					{
						int num2 = num + k;
						bool flag = math.any(*unsafeSpan[num2]);
						GridNodeBase gridNodeBase = this.nodes[num2];
						bool flag2 = gridNodeBase != null;
						if (flag != flag2)
						{
							if (flag)
							{
								gridNodeBase = (this.nodes[num2] = this.newGridNodeDelegate());
								this.active.InitializeNode(gridNodeBase);
							}
							else
							{
								gridNodeBase.ClearCustomConnections(true);
								gridNodeBase.ResetConnectionsInternal();
								gridNodeBase.Destroy();
								this.nodes[num2] = null;
							}
						}
					}
				}
			}
		}

		// Token: 0x040009AE RID: 2478
		public AstarPath active;

		// Token: 0x040009AF RID: 2479
		[ReadOnly]
		public NativeArray<float4> nodeNormals;

		// Token: 0x040009B0 RID: 2480
		public IntBounds dataBounds;

		// Token: 0x040009B1 RID: 2481
		public int3 nodeArrayBounds;

		// Token: 0x040009B2 RID: 2482
		public GridNodeBase[] nodes;

		// Token: 0x040009B3 RID: 2483
		public Func<GridNodeBase> newGridNodeDelegate;
	}
}
