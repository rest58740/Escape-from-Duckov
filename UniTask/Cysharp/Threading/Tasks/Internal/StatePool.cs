using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x02000112 RID: 274
	internal static class StatePool<T1>
	{
		// Token: 0x06000652 RID: 1618 RVA: 0x0000E90C File Offset: 0x0000CB0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static StateTuple<T1> Create(T1 item1)
		{
			StateTuple<T1> stateTuple;
			if (StatePool<T1>.queue.TryDequeue(out stateTuple))
			{
				stateTuple.Item1 = item1;
				return stateTuple;
			}
			return new StateTuple<T1>
			{
				Item1 = item1
			};
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0000E93C File Offset: 0x0000CB3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Return(StateTuple<T1> tuple)
		{
			tuple.Item1 = default(T1);
			StatePool<T1>.queue.Enqueue(tuple);
		}

		// Token: 0x04000121 RID: 289
		private static readonly ConcurrentQueue<StateTuple<T1>> queue = new ConcurrentQueue<StateTuple<T1>>();
	}
}
