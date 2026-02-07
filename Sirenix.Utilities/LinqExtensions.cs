using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sirenix.Utilities
{
	// Token: 0x02000006 RID: 6
	public static class LinqExtensions
	{
		// Token: 0x06000012 RID: 18 RVA: 0x000025F2 File Offset: 0x000007F2
		public static IEnumerable<T> Examine<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (T t in source)
			{
				action.Invoke(t);
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000260C File Offset: 0x0000080C
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (T t in source)
			{
				action.Invoke(t);
			}
			return source;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002658 File Offset: 0x00000858
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
		{
			int num = 0;
			foreach (T t in source)
			{
				action.Invoke(t, num++);
			}
			return source;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000026A8 File Offset: 0x000008A8
		public static IEnumerable<T> Convert<T>(this IEnumerable source, Func<object, T> converter)
		{
			foreach (object obj in source)
			{
				yield return converter.Invoke(obj);
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000026C0 File Offset: 0x000008C0
		public static ImmutableList<T> ToImmutableList<T>(this IEnumerable<T> source)
		{
			IList<T> list = source as IList<T>;
			if (list == null)
			{
				list = source.ToArray<T>();
			}
			return new ImmutableList<T>(list);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000026E4 File Offset: 0x000008E4
		public static IEnumerable<T> PrependWith<T>(this IEnumerable<T> source, Func<T> prepend)
		{
			yield return prepend.Invoke();
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000026FB File Offset: 0x000008FB
		public static IEnumerable<T> PrependWith<T>(this IEnumerable<T> source, T prepend)
		{
			yield return prepend;
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002712 File Offset: 0x00000912
		public static IEnumerable<T> PrependWith<T>(this IEnumerable<T> source, IEnumerable<T> prepend)
		{
			foreach (T t in prepend)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			foreach (T t2 in source)
			{
				yield return t2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002729 File Offset: 0x00000929
		public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, bool condition, Func<T> prepend)
		{
			if (condition)
			{
				yield return prepend.Invoke();
			}
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002747 File Offset: 0x00000947
		public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, bool condition, T prepend)
		{
			if (condition)
			{
				yield return prepend;
			}
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002765 File Offset: 0x00000965
		public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, bool condition, IEnumerable<T> prepend)
		{
			IEnumerator<T> enumerator;
			if (condition)
			{
				foreach (T t in prepend)
				{
					yield return t;
				}
				enumerator = null;
			}
			foreach (T t2 in source)
			{
				yield return t2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002783 File Offset: 0x00000983
		public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, Func<bool> condition, Func<T> prepend)
		{
			if (condition.Invoke())
			{
				yield return prepend.Invoke();
			}
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000027A1 File Offset: 0x000009A1
		public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, Func<bool> condition, T prepend)
		{
			if (condition.Invoke())
			{
				yield return prepend;
			}
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000027BF File Offset: 0x000009BF
		public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, Func<bool> condition, IEnumerable<T> prepend)
		{
			IEnumerator<T> enumerator;
			if (condition.Invoke())
			{
				foreach (T t in prepend)
				{
					yield return t;
				}
				enumerator = null;
			}
			foreach (T t2 in source)
			{
				yield return t2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000027DD File Offset: 0x000009DD
		public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, Func<IEnumerable<T>, bool> condition, Func<T> prepend)
		{
			if (condition.Invoke(source))
			{
				yield return prepend.Invoke();
			}
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000027FB File Offset: 0x000009FB
		public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, Func<IEnumerable<T>, bool> condition, T prepend)
		{
			if (condition.Invoke(source))
			{
				yield return prepend;
			}
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002819 File Offset: 0x00000A19
		public static IEnumerable<T> PrependIf<T>(this IEnumerable<T> source, Func<IEnumerable<T>, bool> condition, IEnumerable<T> prepend)
		{
			IEnumerator<T> enumerator;
			if (condition.Invoke(source))
			{
				foreach (T t in prepend)
				{
					yield return t;
				}
				enumerator = null;
			}
			foreach (T t2 in source)
			{
				yield return t2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002837 File Offset: 0x00000A37
		public static IEnumerable<T> AppendWith<T>(this IEnumerable<T> source, Func<T> append)
		{
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			yield return append.Invoke();
			yield break;
			yield break;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000284E File Offset: 0x00000A4E
		public static IEnumerable<T> AppendWith<T>(this IEnumerable<T> source, T append)
		{
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			yield return append;
			yield break;
			yield break;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002865 File Offset: 0x00000A65
		public static IEnumerable<T> AppendWith<T>(this IEnumerable<T> source, IEnumerable<T> append)
		{
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			foreach (T t2 in append)
			{
				yield return t2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000287C File Offset: 0x00000A7C
		public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> source, bool condition, Func<T> append)
		{
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			if (condition)
			{
				yield return append.Invoke();
			}
			yield break;
			yield break;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000289A File Offset: 0x00000A9A
		public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> source, bool condition, T append)
		{
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			if (condition)
			{
				yield return append;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000028B8 File Offset: 0x00000AB8
		public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> source, bool condition, IEnumerable<T> append)
		{
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			if (condition)
			{
				foreach (T t2 in append)
				{
					yield return t2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000028D6 File Offset: 0x00000AD6
		public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> source, Func<bool> condition, Func<T> append)
		{
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			if (condition.Invoke())
			{
				yield return append.Invoke();
			}
			yield break;
			yield break;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000028F4 File Offset: 0x00000AF4
		public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> source, Func<bool> condition, T append)
		{
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			if (condition.Invoke())
			{
				yield return append;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002912 File Offset: 0x00000B12
		public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> source, Func<bool> condition, IEnumerable<T> append)
		{
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			if (condition.Invoke())
			{
				foreach (T t2 in append)
				{
					yield return t2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002930 File Offset: 0x00000B30
		public static IEnumerable<T> FilterCast<T>(this IEnumerable source)
		{
			foreach (object obj in source)
			{
				if (obj is T)
				{
					yield return (T)((object)obj);
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002940 File Offset: 0x00000B40
		public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> range)
		{
			foreach (T item in range)
			{
				hashSet.Add(item);
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000298C File Offset: 0x00000B8C
		public static bool IsNullOrEmpty<T>(this IList<T> list)
		{
			return list == null || list.Count == 0;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000299C File Offset: 0x00000B9C
		public static void Populate<T>(this IList<T> list, T item)
		{
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				list[i] = item;
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000029C4 File Offset: 0x00000BC4
		public static void AddRange<T>(this IList<T> list, IEnumerable<T> collection)
		{
			if (list is List<T>)
			{
				((List<T>)list).AddRange(collection);
				return;
			}
			foreach (T t in collection)
			{
				list.Add(t);
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002A24 File Offset: 0x00000C24
		public static void Sort<T>(this IList<T> list, Comparison<T> comparison)
		{
			if (list is List<T>)
			{
				((List<T>)list).Sort(comparison);
				return;
			}
			List<T> list2 = new List<T>(list);
			list2.Sort(comparison);
			for (int i = 0; i < list.Count; i++)
			{
				list[i] = list2[i];
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002A74 File Offset: 0x00000C74
		public static void Sort<T>(this IList<T> list)
		{
			if (list is List<T>)
			{
				((List<T>)list).Sort();
				return;
			}
			List<T> list2 = new List<T>(list);
			list2.Sort();
			for (int i = 0; i < list.Count; i++)
			{
				list[i] = list2[i];
			}
		}
	}
}
