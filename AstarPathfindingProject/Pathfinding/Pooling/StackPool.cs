using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Pooling
{
	// Token: 0x0200024A RID: 586
	public static class StackPool<T>
	{
		// Token: 0x06000DCD RID: 3533 RVA: 0x00056E60 File Offset: 0x00055060
		public static Stack<T> Claim()
		{
			List<Stack<T>> obj = StackPool<T>.pool;
			lock (obj)
			{
				if (StackPool<T>.pool.Count > 0)
				{
					Stack<T> result = StackPool<T>.pool[StackPool<T>.pool.Count - 1];
					StackPool<T>.pool.RemoveAt(StackPool<T>.pool.Count - 1);
					return result;
				}
			}
			return new Stack<T>();
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x00056EDC File Offset: 0x000550DC
		public static void Warmup(int count)
		{
			Stack<T>[] array = new Stack<T>[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = StackPool<T>.Claim();
			}
			for (int j = 0; j < count; j++)
			{
				StackPool<T>.Release(array[j]);
			}
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x00056F18 File Offset: 0x00055118
		public static void Release(Stack<T> stack)
		{
			stack.Clear();
			List<Stack<T>> obj = StackPool<T>.pool;
			lock (obj)
			{
				for (int i = 0; i < StackPool<T>.pool.Count; i++)
				{
					if (StackPool<T>.pool[i] == stack)
					{
						Debug.LogError("The Stack is released even though it is inside the pool");
					}
				}
				StackPool<T>.pool.Add(stack);
			}
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x00056F90 File Offset: 0x00055190
		public static void Clear()
		{
			List<Stack<T>> obj = StackPool<T>.pool;
			lock (obj)
			{
				StackPool<T>.pool.Clear();
			}
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x00056FD4 File Offset: 0x000551D4
		public static int GetSize()
		{
			return StackPool<T>.pool.Count;
		}

		// Token: 0x04000A96 RID: 2710
		private static readonly List<Stack<T>> pool = new List<Stack<T>>();
	}
}
