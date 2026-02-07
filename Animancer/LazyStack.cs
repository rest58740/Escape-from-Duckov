using System;
using System.Collections.Generic;

namespace Animancer
{
	// Token: 0x0200000A RID: 10
	public class LazyStack<T> where T : new()
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000CC RID: 204 RVA: 0x000031D3 File Offset: 0x000013D3
		// (set) Token: 0x060000CD RID: 205 RVA: 0x000031DB File Offset: 0x000013DB
		public T Current { get; private set; }

		// Token: 0x060000CE RID: 206 RVA: 0x000031E4 File Offset: 0x000013E4
		public LazyStack()
		{
			this.Stack = new List<T>();
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00003200 File Offset: 0x00001400
		public LazyStack(int capacity)
		{
			this.Stack = new List<T>(capacity);
			for (int i = 0; i < capacity; i++)
			{
				this.Stack[i] = Activator.CreateInstance<T>();
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003244 File Offset: 0x00001444
		public T Increment()
		{
			this._CurrentIndex++;
			if (this._CurrentIndex == this.Stack.Count)
			{
				this.Current = Activator.CreateInstance<T>();
				this.Stack.Add(this.Current);
			}
			else
			{
				this.Current = this.Stack[this._CurrentIndex];
			}
			return this.Current;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000032B0 File Offset: 0x000014B0
		public void Decrement()
		{
			this._CurrentIndex--;
			if (this._CurrentIndex >= 0)
			{
				this.Current = this.Stack[this._CurrentIndex];
				return;
			}
			this.Current = default(T);
		}

		// Token: 0x0400000D RID: 13
		private readonly List<T> Stack;

		// Token: 0x0400000E RID: 14
		private int _CurrentIndex = -1;
	}
}
