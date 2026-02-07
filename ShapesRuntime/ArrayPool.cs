using System;
using System.Collections.Generic;

namespace Shapes
{
	// Token: 0x0200005C RID: 92
	internal static class ArrayPool<T>
	{
		// Token: 0x06000C91 RID: 3217 RVA: 0x0001931F File Offset: 0x0001751F
		public static T[] Alloc(int maxCount)
		{
			if (ArrayPool<T>.pool.Count != 0)
			{
				return ArrayPool<T>.pool.Pop();
			}
			return new T[maxCount];
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x0001933E File Offset: 0x0001753E
		public static void Free(T[] obj)
		{
			ArrayPool<T>.pool.Push(obj);
		}

		// Token: 0x04000204 RID: 516
		private static readonly Stack<T[]> pool = new Stack<T[]>();
	}
}
