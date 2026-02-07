using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Pathfinding.Jobs
{
	// Token: 0x02000170 RID: 368
	[BurstCompile]
	public struct JobCopyRectangle<T> : IJob where T : struct
	{
		// Token: 0x06000AC4 RID: 2756 RVA: 0x0003CF32 File Offset: 0x0003B132
		public void Execute()
		{
			JobCopyRectangle<T>.Copy(this.input, this.output, this.inputSlice, this.outputSlice);
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0003CF54 File Offset: 0x0003B154
		public static void Copy(NativeArray<T> input, NativeArray<T> output, Slice3D inputSlice, Slice3D outputSlice)
		{
			inputSlice.AssertMatchesOuter<T>(input);
			outputSlice.AssertMatchesOuter<T>(output);
			inputSlice.AssertSameSize(outputSlice);
			if (inputSlice.coversEverything && outputSlice.coversEverything)
			{
				input.CopyTo(output);
				return;
			}
			for (int i = 0; i < outputSlice.slice.size.y; i++)
			{
				for (int j = 0; j < outputSlice.slice.size.z; j++)
				{
					int srcIndex = inputSlice.InnerCoordinateToOuterIndex(0, i, j);
					int dstIndex = outputSlice.InnerCoordinateToOuterIndex(0, i, j);
					NativeArray<T>.Copy(input, srcIndex, output, dstIndex, outputSlice.slice.size.x);
				}
			}
		}

		// Token: 0x04000732 RID: 1842
		[ReadOnly]
		[DisableUninitializedReadCheck]
		public NativeArray<T> input;

		// Token: 0x04000733 RID: 1843
		[WriteOnly]
		public NativeArray<T> output;

		// Token: 0x04000734 RID: 1844
		public Slice3D inputSlice;

		// Token: 0x04000735 RID: 1845
		public Slice3D outputSlice;
	}
}
