using System;
using System.Collections.Generic;

namespace Sirenix.Utilities
{
	// Token: 0x02000007 RID: 7
	public static class ListExtensions
	{
		// Token: 0x06000033 RID: 51 RVA: 0x00002AC4 File Offset: 0x00000CC4
		public static void SetLength<T>(ref IList<T> list, int length)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (length < 0)
			{
				throw new ArgumentException("Length must be larger than or equal to 0.");
			}
			if (list.GetType().IsArray)
			{
				if (list.Count != length)
				{
					T[] array = (T[])list;
					Array.Resize<T>(ref array, length);
					list = array;
					return;
				}
			}
			else
			{
				while (list.Count < length)
				{
					list.Add(default(T));
				}
				while (list.Count > length)
				{
					list.RemoveAt(list.Count - 1);
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002B54 File Offset: 0x00000D54
		public static void SetLength<T>(ref IList<T> list, int length, Func<T> newElement)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (length < 0)
			{
				throw new ArgumentException("Length must be larger than or equal to 0.");
			}
			if (newElement == null)
			{
				throw new ArgumentNullException("newElement");
			}
			if (list.GetType().IsArray)
			{
				if (list.Count != length)
				{
					T[] array = (T[])list;
					Array.Resize<T>(ref array, length);
					list = array;
					return;
				}
			}
			else
			{
				while (list.Count < length)
				{
					list.Add(newElement.Invoke());
				}
				while (list.Count > length)
				{
					list.RemoveAt(list.Count - 1);
				}
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002BEC File Offset: 0x00000DEC
		public static void SetLength<T>(this IList<T> list, int length)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (length < 0)
			{
				throw new ArgumentException("Length must be larger than or equal to 0.");
			}
			if (list.GetType().IsArray)
			{
				throw new ArgumentException("Cannot use the SetLength extension method on an array. Use Array.Resize or the ListUtilities.SetLength(ref IList<T> list, int length) overload.");
			}
			while (list.Count < length)
			{
				list.Add(default(T));
			}
			while (list.Count > length)
			{
				list.RemoveAt(list.Count - 1);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002C60 File Offset: 0x00000E60
		public static void SetLength<T>(this IList<T> list, int length, Func<T> newElement)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (length < 0)
			{
				throw new ArgumentException("Length must be larger than or equal to 0.");
			}
			if (newElement == null)
			{
				throw new ArgumentNullException("newElement");
			}
			if (list.GetType().IsArray)
			{
				throw new ArgumentException("Cannot use the SetLength extension method on an array. Use Array.Resize or the ListUtilities.SetLength(ref IList<T> list, int length) overload.");
			}
			while (list.Count < length)
			{
				list.Add(newElement.Invoke());
			}
			while (list.Count > length)
			{
				list.RemoveAt(list.Count - 1);
			}
		}
	}
}
