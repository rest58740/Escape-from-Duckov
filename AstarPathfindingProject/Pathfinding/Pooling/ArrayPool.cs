using System;
using System.Collections.Generic;

namespace Pathfinding.Pooling
{
	// Token: 0x02000243 RID: 579
	public static class ArrayPool<T>
	{
		// Token: 0x06000DB0 RID: 3504 RVA: 0x00056518 File Offset: 0x00054718
		public static T[] Claim(int minimumLength)
		{
			if (minimumLength <= 0)
			{
				return ArrayPool<T>.ClaimWithExactLength(0);
			}
			int num = 0;
			while (1 << num < minimumLength && num < 30)
			{
				num++;
			}
			if (num == 30)
			{
				throw new ArgumentException("Too high minimum length");
			}
			Stack<T[]>[] obj = ArrayPool<T>.pool;
			lock (obj)
			{
				if (ArrayPool<T>.pool[num] == null)
				{
					ArrayPool<T>.pool[num] = new Stack<T[]>();
				}
				if (ArrayPool<T>.pool[num].Count > 0)
				{
					return ArrayPool<T>.pool[num].Pop();
				}
			}
			return new T[1 << num];
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x000565C4 File Offset: 0x000547C4
		public static T[] ClaimWithExactLength(int length)
		{
			if (length != 0 && (length & length - 1) == 0)
			{
				return ArrayPool<T>.Claim(length);
			}
			if (length <= 256)
			{
				Stack<T[]>[] obj = ArrayPool<T>.pool;
				lock (obj)
				{
					Stack<T[]> stack = ArrayPool<T>.exactPool[length];
					if (stack != null && stack.Count > 0)
					{
						return stack.Pop();
					}
				}
			}
			return new T[length];
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00056644 File Offset: 0x00054844
		public static void Release(ref T[] array, bool allowNonPowerOfTwo = false)
		{
			if (array == null)
			{
				return;
			}
			if (array.GetType() != typeof(T[]))
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Expected array type ",
					typeof(T[]).Name,
					" but found ",
					array.GetType().Name,
					"\nAre you using the correct generic class?\n"
				}));
			}
			bool flag = array.Length != 0 && (array.Length & array.Length - 1) == 0;
			if (!flag && !allowNonPowerOfTwo && array.Length != 0)
			{
				throw new ArgumentException("Length is not a power of 2");
			}
			Stack<T[]>[] obj = ArrayPool<T>.pool;
			lock (obj)
			{
				if (flag)
				{
					int num = 0;
					while (1 << num < array.Length && num < 30)
					{
						num++;
					}
					if (ArrayPool<T>.pool[num] == null)
					{
						ArrayPool<T>.pool[num] = new Stack<T[]>();
					}
					ArrayPool<T>.pool[num].Push(array);
				}
				else if (array.Length <= 256)
				{
					Stack<T[]> stack = ArrayPool<T>.exactPool[array.Length];
					if (stack == null)
					{
						stack = (ArrayPool<T>.exactPool[array.Length] = new Stack<T[]>());
					}
					stack.Push(array);
				}
			}
			array = null;
		}

		// Token: 0x04000A89 RID: 2697
		private const int MaximumExactArrayLength = 256;

		// Token: 0x04000A8A RID: 2698
		private static readonly Stack<T[]>[] pool = new Stack<T[]>[31];

		// Token: 0x04000A8B RID: 2699
		private static readonly Stack<T[]>[] exactPool = new Stack<T[]>[257];
	}
}
