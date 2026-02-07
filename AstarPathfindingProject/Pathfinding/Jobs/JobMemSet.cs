using System;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Pathfinding.Jobs
{
	// Token: 0x02000171 RID: 369
	[BurstCompile]
	public struct JobMemSet<[IsUnmanaged] T> : IJob where T : struct, ValueType
	{
		// Token: 0x06000AC6 RID: 2758 RVA: 0x0003D005 File Offset: 0x0003B205
		public void Execute()
		{
			this.data.AsUnsafeSpan<T>().Fill(this.value);
		}

		// Token: 0x04000736 RID: 1846
		[WriteOnly]
		public NativeArray<T> data;

		// Token: 0x04000737 RID: 1847
		public T value;
	}
}
