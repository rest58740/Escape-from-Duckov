using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000033 RID: 51
	[StructLayout(LayoutKind.Auto)]
	public struct TaskPool<T> where T : class, ITaskPoolNode<T>
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00003CF0 File Offset: 0x00001EF0
		public int Size
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003CF8 File Offset: 0x00001EF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryPop(out T result)
		{
			if (Interlocked.CompareExchange(ref this.gate, 1, 0) == 0)
			{
				T t = this.root;
				if (t != null)
				{
					ref T nextNode = ref t.NextNode;
					this.root = nextNode;
					nextNode = default(T);
					this.size--;
					result = t;
					Volatile.Write(ref this.gate, 0);
					return true;
				}
				Volatile.Write(ref this.gate, 0);
			}
			result = default(T);
			return false;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00003D7C File Offset: 0x00001F7C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe bool TryPush(T item)
		{
			if (Interlocked.CompareExchange(ref this.gate, 1, 0) == 0)
			{
				if (this.size < TaskPool.MaxPoolSize)
				{
					*item.NextNode = this.root;
					this.root = item;
					this.size++;
					Volatile.Write(ref this.gate, 0);
					return true;
				}
				Volatile.Write(ref this.gate, 0);
			}
			return false;
		}

		// Token: 0x0400006D RID: 109
		private int gate;

		// Token: 0x0400006E RID: 110
		private int size;

		// Token: 0x0400006F RID: 111
		private T root;
	}
}
