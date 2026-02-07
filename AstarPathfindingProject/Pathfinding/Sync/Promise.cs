using System;
using Unity.Jobs;

namespace Pathfinding.Sync
{
	// Token: 0x0200022A RID: 554
	public struct Promise<T> : IProgress, IDisposable where T : IProgress, IDisposable
	{
		// Token: 0x06000D33 RID: 3379 RVA: 0x00053A40 File Offset: 0x00051C40
		public Promise(JobHandle handle, T result)
		{
			this.handle = handle;
			this.result = result;
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000D34 RID: 3380 RVA: 0x00053A50 File Offset: 0x00051C50
		public bool IsCompleted
		{
			get
			{
				return this.handle.IsCompleted;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000D35 RID: 3381 RVA: 0x00053A5D File Offset: 0x00051C5D
		public float Progress
		{
			get
			{
				return this.result.Progress;
			}
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00053A70 File Offset: 0x00051C70
		public T GetValue()
		{
			return this.result;
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00053A78 File Offset: 0x00051C78
		public T Complete()
		{
			this.handle.Complete();
			return this.result;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00053A8B File Offset: 0x00051C8B
		public void Dispose()
		{
			this.Complete();
			this.result.Dispose();
		}

		// Token: 0x04000A39 RID: 2617
		public JobHandle handle;

		// Token: 0x04000A3A RID: 2618
		private T result;
	}
}
