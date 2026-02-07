using System;
using System.Runtime.CompilerServices;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x0200010E RID: 270
	internal sealed class PooledDelegate<T> : ITaskPoolNode<PooledDelegate<T>>
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x0000E6D4 File Offset: 0x0000C8D4
		public ref PooledDelegate<T> NextNode
		{
			get
			{
				return ref this.nextNode;
			}
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0000E6DC File Offset: 0x0000C8DC
		static PooledDelegate()
		{
			TaskPool.RegisterSizeGetter(typeof(PooledDelegate<T>), () => PooledDelegate<T>.pool.Size);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0000E6FD File Offset: 0x0000C8FD
		private PooledDelegate()
		{
			this.runDelegate = new Action<T>(this.Run);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0000E718 File Offset: 0x0000C918
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Action<T> Create(Action continuation)
		{
			PooledDelegate<T> pooledDelegate;
			if (!PooledDelegate<T>.pool.TryPop(out pooledDelegate))
			{
				pooledDelegate = new PooledDelegate<T>();
			}
			pooledDelegate.continuation = continuation;
			return pooledDelegate.runDelegate;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0000E748 File Offset: 0x0000C948
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Run(T _)
		{
			Action action = this.continuation;
			this.continuation = null;
			if (action != null)
			{
				PooledDelegate<T>.pool.TryPush(this);
				action();
			}
		}

		// Token: 0x0400011C RID: 284
		private static TaskPool<PooledDelegate<T>> pool;

		// Token: 0x0400011D RID: 285
		private PooledDelegate<T> nextNode;

		// Token: 0x0400011E RID: 286
		private readonly Action<T> runDelegate;

		// Token: 0x0400011F RID: 287
		private Action continuation;
	}
}
