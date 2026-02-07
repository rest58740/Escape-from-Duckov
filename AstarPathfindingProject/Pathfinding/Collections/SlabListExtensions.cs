using System;
using System.Runtime.CompilerServices;

namespace Pathfinding.Collections
{
	// Token: 0x02000264 RID: 612
	public static class SlabListExtensions
	{
		// Token: 0x06000E90 RID: 3728 RVA: 0x0005A560 File Offset: 0x00058760
		public static void Remove<[IsUnmanaged] T>(this SlabAllocator<T>.List list, T value) where T : struct, ValueType, IEquatable<T>
		{
			int num = list.span.IndexOf(value);
			if (num != -1)
			{
				list.RemoveAt(num);
			}
		}
	}
}
