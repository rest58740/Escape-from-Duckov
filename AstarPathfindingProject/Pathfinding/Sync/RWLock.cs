using System;
using Unity.Jobs;

namespace Pathfinding.Sync
{
	// Token: 0x0200022B RID: 555
	public class RWLock
	{
		// Token: 0x06000D39 RID: 3385 RVA: 0x000035CE File Offset: 0x000017CE
		private void AddPendingSync()
		{
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x000035CE File Offset: 0x000017CE
		private void RemovePendingSync()
		{
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x000035CE File Offset: 0x000017CE
		private void AddPendingAsync()
		{
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x000035CE File Offset: 0x000017CE
		private void RemovePendingAsync()
		{
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00053AA5 File Offset: 0x00051CA5
		public RWLock.LockSync ReadSync()
		{
			this.AddPendingSync();
			this.lastWrite.Complete();
			this.lastWrite = default(JobHandle);
			return new RWLock.LockSync(this);
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00053ACA File Offset: 0x00051CCA
		public RWLock.ReadLockAsync Read()
		{
			this.AddPendingAsync();
			return new RWLock.ReadLockAsync(this, this.lastWrite);
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00053ADE File Offset: 0x00051CDE
		public RWLock.LockSync WriteSync()
		{
			this.AddPendingSync();
			this.lastWrite.Complete();
			this.lastWrite = default(JobHandle);
			this.lastRead.Complete();
			return new RWLock.LockSync(this);
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00053B0E File Offset: 0x00051D0E
		public RWLock.WriteLockAsync Write()
		{
			this.AddPendingAsync();
			return new RWLock.WriteLockAsync(this, JobHandle.CombineDependencies(this.lastRead, this.lastWrite));
		}

		// Token: 0x04000A3B RID: 2619
		private JobHandle lastWrite;

		// Token: 0x04000A3C RID: 2620
		private JobHandle lastRead;

		// Token: 0x0200022C RID: 556
		public readonly struct CombinedReadLockAsync
		{
			// Token: 0x06000D42 RID: 3394 RVA: 0x00053B2D File Offset: 0x00051D2D
			public CombinedReadLockAsync(RWLock.ReadLockAsync lock1, RWLock.ReadLockAsync lock2)
			{
				this.lock1 = lock1.inner;
				this.lock2 = lock2.inner;
				this.dependency = JobHandle.CombineDependencies(lock1.dependency, lock2.dependency);
			}

			// Token: 0x06000D43 RID: 3395 RVA: 0x00053B60 File Offset: 0x00051D60
			public void UnlockAfter(JobHandle handle)
			{
				if (this.lock1 != null)
				{
					this.lock1.RemovePendingAsync();
					this.lock1.lastRead = JobHandle.CombineDependencies(this.lock1.lastRead, handle);
				}
				if (this.lock2 != null)
				{
					this.lock2.RemovePendingAsync();
					this.lock2.lastRead = JobHandle.CombineDependencies(this.lock2.lastRead, handle);
				}
			}

			// Token: 0x04000A3D RID: 2621
			private readonly RWLock lock1;

			// Token: 0x04000A3E RID: 2622
			private readonly RWLock lock2;

			// Token: 0x04000A3F RID: 2623
			public readonly JobHandle dependency;
		}

		// Token: 0x0200022D RID: 557
		public readonly struct ReadLockAsync
		{
			// Token: 0x06000D44 RID: 3396 RVA: 0x00053BCB File Offset: 0x00051DCB
			public ReadLockAsync(RWLock inner, JobHandle dependency)
			{
				this.inner = inner;
				this.dependency = dependency;
			}

			// Token: 0x06000D45 RID: 3397 RVA: 0x00053BDB File Offset: 0x00051DDB
			public void UnlockAfter(JobHandle handle)
			{
				if (this.inner != null)
				{
					this.inner.RemovePendingAsync();
					this.inner.lastRead = JobHandle.CombineDependencies(this.inner.lastRead, handle);
				}
			}

			// Token: 0x04000A40 RID: 2624
			internal readonly RWLock inner;

			// Token: 0x04000A41 RID: 2625
			public readonly JobHandle dependency;
		}

		// Token: 0x0200022E RID: 558
		public readonly struct WriteLockAsync
		{
			// Token: 0x06000D46 RID: 3398 RVA: 0x00053C0C File Offset: 0x00051E0C
			public WriteLockAsync(RWLock inner, JobHandle dependency)
			{
				this.inner = inner;
				this.dependency = dependency;
			}

			// Token: 0x06000D47 RID: 3399 RVA: 0x00053C1C File Offset: 0x00051E1C
			public void UnlockAfter(JobHandle handle)
			{
				if (this.inner != null)
				{
					this.inner.RemovePendingAsync();
					this.inner.lastWrite = handle;
				}
			}

			// Token: 0x04000A42 RID: 2626
			private readonly RWLock inner;

			// Token: 0x04000A43 RID: 2627
			public readonly JobHandle dependency;
		}

		// Token: 0x0200022F RID: 559
		public readonly struct LockSync : IDisposable
		{
			// Token: 0x06000D48 RID: 3400 RVA: 0x00053C3D File Offset: 0x00051E3D
			public LockSync(RWLock inner)
			{
				this.inner = inner;
			}

			// Token: 0x06000D49 RID: 3401 RVA: 0x00053C46 File Offset: 0x00051E46
			public void Unlock()
			{
				if (this.inner != null)
				{
					this.inner.RemovePendingSync();
				}
			}

			// Token: 0x06000D4A RID: 3402 RVA: 0x00053C5B File Offset: 0x00051E5B
			void IDisposable.Dispose()
			{
				this.Unlock();
			}

			// Token: 0x04000A44 RID: 2628
			private readonly RWLock inner;
		}
	}
}
