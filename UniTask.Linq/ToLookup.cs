using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200007A RID: 122
	internal static class ToLookup
	{
		// Token: 0x060003AF RID: 943 RVA: 0x0000DAA8 File Offset: 0x0000BCA8
		internal static UniTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			ToLookup.<ToLookupAsync>d__0<TSource, TKey> <ToLookupAsync>d__;
			<ToLookupAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<ILookup<TKey, TSource>>.Create();
			<ToLookupAsync>d__.source = source;
			<ToLookupAsync>d__.keySelector = keySelector;
			<ToLookupAsync>d__.comparer = comparer;
			<ToLookupAsync>d__.cancellationToken = cancellationToken;
			<ToLookupAsync>d__.<>1__state = -1;
			<ToLookupAsync>d__.<>t__builder.Start<ToLookup.<ToLookupAsync>d__0<TSource, TKey>>(ref <ToLookupAsync>d__);
			return <ToLookupAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000DB04 File Offset: 0x0000BD04
		internal static UniTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			ToLookup.<ToLookupAsync>d__1<TSource, TKey, TElement> <ToLookupAsync>d__;
			<ToLookupAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<ILookup<TKey, TElement>>.Create();
			<ToLookupAsync>d__.source = source;
			<ToLookupAsync>d__.keySelector = keySelector;
			<ToLookupAsync>d__.elementSelector = elementSelector;
			<ToLookupAsync>d__.comparer = comparer;
			<ToLookupAsync>d__.cancellationToken = cancellationToken;
			<ToLookupAsync>d__.<>1__state = -1;
			<ToLookupAsync>d__.<>t__builder.Start<ToLookup.<ToLookupAsync>d__1<TSource, TKey, TElement>>(ref <ToLookupAsync>d__);
			return <ToLookupAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000DB68 File Offset: 0x0000BD68
		internal static UniTask<ILookup<TKey, TSource>> ToLookupAwaitAsync<TSource, TKey>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			ToLookup.<ToLookupAwaitAsync>d__2<TSource, TKey> <ToLookupAwaitAsync>d__;
			<ToLookupAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<ILookup<TKey, TSource>>.Create();
			<ToLookupAwaitAsync>d__.source = source;
			<ToLookupAwaitAsync>d__.keySelector = keySelector;
			<ToLookupAwaitAsync>d__.comparer = comparer;
			<ToLookupAwaitAsync>d__.cancellationToken = cancellationToken;
			<ToLookupAwaitAsync>d__.<>1__state = -1;
			<ToLookupAwaitAsync>d__.<>t__builder.Start<ToLookup.<ToLookupAwaitAsync>d__2<TSource, TKey>>(ref <ToLookupAwaitAsync>d__);
			return <ToLookupAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000DBC4 File Offset: 0x0000BDC4
		internal static UniTask<ILookup<TKey, TElement>> ToLookupAwaitAsync<TSource, TKey, TElement>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, Func<TSource, UniTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			ToLookup.<ToLookupAwaitAsync>d__3<TSource, TKey, TElement> <ToLookupAwaitAsync>d__;
			<ToLookupAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<ILookup<TKey, TElement>>.Create();
			<ToLookupAwaitAsync>d__.source = source;
			<ToLookupAwaitAsync>d__.keySelector = keySelector;
			<ToLookupAwaitAsync>d__.elementSelector = elementSelector;
			<ToLookupAwaitAsync>d__.comparer = comparer;
			<ToLookupAwaitAsync>d__.cancellationToken = cancellationToken;
			<ToLookupAwaitAsync>d__.<>1__state = -1;
			<ToLookupAwaitAsync>d__.<>t__builder.Start<ToLookup.<ToLookupAwaitAsync>d__3<TSource, TKey, TElement>>(ref <ToLookupAwaitAsync>d__);
			return <ToLookupAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000DC28 File Offset: 0x0000BE28
		internal static UniTask<ILookup<TKey, TSource>> ToLookupAwaitWithCancellationAsync<TSource, TKey>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			ToLookup.<ToLookupAwaitWithCancellationAsync>d__4<TSource, TKey> <ToLookupAwaitWithCancellationAsync>d__;
			<ToLookupAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<ILookup<TKey, TSource>>.Create();
			<ToLookupAwaitWithCancellationAsync>d__.source = source;
			<ToLookupAwaitWithCancellationAsync>d__.keySelector = keySelector;
			<ToLookupAwaitWithCancellationAsync>d__.comparer = comparer;
			<ToLookupAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<ToLookupAwaitWithCancellationAsync>d__.<>1__state = -1;
			<ToLookupAwaitWithCancellationAsync>d__.<>t__builder.Start<ToLookup.<ToLookupAwaitWithCancellationAsync>d__4<TSource, TKey>>(ref <ToLookupAwaitWithCancellationAsync>d__);
			return <ToLookupAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000DC84 File Offset: 0x0000BE84
		internal static UniTask<ILookup<TKey, TElement>> ToLookupAwaitWithCancellationAsync<TSource, TKey, TElement>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, Func<TSource, CancellationToken, UniTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			ToLookup.<ToLookupAwaitWithCancellationAsync>d__5<TSource, TKey, TElement> <ToLookupAwaitWithCancellationAsync>d__;
			<ToLookupAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<ILookup<TKey, TElement>>.Create();
			<ToLookupAwaitWithCancellationAsync>d__.source = source;
			<ToLookupAwaitWithCancellationAsync>d__.keySelector = keySelector;
			<ToLookupAwaitWithCancellationAsync>d__.elementSelector = elementSelector;
			<ToLookupAwaitWithCancellationAsync>d__.comparer = comparer;
			<ToLookupAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<ToLookupAwaitWithCancellationAsync>d__.<>1__state = -1;
			<ToLookupAwaitWithCancellationAsync>d__.<>t__builder.Start<ToLookup.<ToLookupAwaitWithCancellationAsync>d__5<TSource, TKey, TElement>>(ref <ToLookupAwaitWithCancellationAsync>d__);
			return <ToLookupAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x020001E6 RID: 486
		private class Lookup<TKey, TElement> : ILookup<TKey, TElement>, IEnumerable<IGrouping<TKey, TElement>>, IEnumerable
		{
			// Token: 0x0600088A RID: 2186 RVA: 0x0004B362 File Offset: 0x00049562
			private Lookup(Dictionary<TKey, ToLookup.Grouping<TKey, TElement>> dict)
			{
				this.dict = dict;
			}

			// Token: 0x0600088B RID: 2187 RVA: 0x0004B371 File Offset: 0x00049571
			public static ToLookup.Lookup<TKey, TElement> CreateEmpty()
			{
				return ToLookup.Lookup<TKey, TElement>.empty;
			}

			// Token: 0x0600088C RID: 2188 RVA: 0x0004B378 File Offset: 0x00049578
			public static ToLookup.Lookup<TKey, TElement> Create(ArraySegment<TElement> source, Func<TElement, TKey> keySelector, IEqualityComparer<TKey> comparer)
			{
				Dictionary<TKey, ToLookup.Grouping<TKey, TElement>> dictionary = new Dictionary<TKey, ToLookup.Grouping<TKey, TElement>>(comparer);
				TElement[] array = source.Array;
				int count = source.Count;
				for (int i = source.Offset; i < count; i++)
				{
					TKey key = keySelector(array[i]);
					ToLookup.Grouping<TKey, TElement> grouping;
					if (!dictionary.TryGetValue(key, out grouping))
					{
						grouping = new ToLookup.Grouping<TKey, TElement>(key);
						dictionary[key] = grouping;
					}
					grouping.Add(array[i]);
				}
				return new ToLookup.Lookup<TKey, TElement>(dictionary);
			}

			// Token: 0x0600088D RID: 2189 RVA: 0x0004B3F0 File Offset: 0x000495F0
			public static ToLookup.Lookup<TKey, TElement> Create<TSource>(ArraySegment<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
			{
				Dictionary<TKey, ToLookup.Grouping<TKey, TElement>> dictionary = new Dictionary<TKey, ToLookup.Grouping<TKey, TElement>>(comparer);
				TSource[] array = source.Array;
				int count = source.Count;
				for (int i = source.Offset; i < count; i++)
				{
					TKey key = keySelector(array[i]);
					TElement value = elementSelector(array[i]);
					ToLookup.Grouping<TKey, TElement> grouping;
					if (!dictionary.TryGetValue(key, out grouping))
					{
						grouping = new ToLookup.Grouping<TKey, TElement>(key);
						dictionary[key] = grouping;
					}
					grouping.Add(value);
				}
				return new ToLookup.Lookup<TKey, TElement>(dictionary);
			}

			// Token: 0x0600088E RID: 2190 RVA: 0x0004B474 File Offset: 0x00049674
			public static UniTask<ToLookup.Lookup<TKey, TElement>> CreateAsync(ArraySegment<TElement> source, Func<TElement, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
			{
				ToLookup.Lookup<TKey, TElement>.<CreateAsync>d__6 <CreateAsync>d__;
				<CreateAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<ToLookup.Lookup<TKey, TElement>>.Create();
				<CreateAsync>d__.source = source;
				<CreateAsync>d__.keySelector = keySelector;
				<CreateAsync>d__.comparer = comparer;
				<CreateAsync>d__.<>1__state = -1;
				<CreateAsync>d__.<>t__builder.Start<ToLookup.Lookup<TKey, TElement>.<CreateAsync>d__6>(ref <CreateAsync>d__);
				return <CreateAsync>d__.<>t__builder.Task;
			}

			// Token: 0x0600088F RID: 2191 RVA: 0x0004B4C8 File Offset: 0x000496C8
			public static UniTask<ToLookup.Lookup<TKey, TElement>> CreateAsync<TSource>(ArraySegment<TSource> source, Func<TSource, UniTask<TKey>> keySelector, Func<TSource, UniTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer)
			{
				ToLookup.Lookup<TKey, TElement>.<CreateAsync>d__7<TSource> <CreateAsync>d__;
				<CreateAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<ToLookup.Lookup<TKey, TElement>>.Create();
				<CreateAsync>d__.source = source;
				<CreateAsync>d__.keySelector = keySelector;
				<CreateAsync>d__.elementSelector = elementSelector;
				<CreateAsync>d__.comparer = comparer;
				<CreateAsync>d__.<>1__state = -1;
				<CreateAsync>d__.<>t__builder.Start<ToLookup.Lookup<TKey, TElement>.<CreateAsync>d__7<TSource>>(ref <CreateAsync>d__);
				return <CreateAsync>d__.<>t__builder.Task;
			}

			// Token: 0x06000890 RID: 2192 RVA: 0x0004B524 File Offset: 0x00049724
			public static UniTask<ToLookup.Lookup<TKey, TElement>> CreateAsync(ArraySegment<TElement> source, Func<TElement, CancellationToken, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
			{
				ToLookup.Lookup<TKey, TElement>.<CreateAsync>d__8 <CreateAsync>d__;
				<CreateAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<ToLookup.Lookup<TKey, TElement>>.Create();
				<CreateAsync>d__.source = source;
				<CreateAsync>d__.keySelector = keySelector;
				<CreateAsync>d__.comparer = comparer;
				<CreateAsync>d__.cancellationToken = cancellationToken;
				<CreateAsync>d__.<>1__state = -1;
				<CreateAsync>d__.<>t__builder.Start<ToLookup.Lookup<TKey, TElement>.<CreateAsync>d__8>(ref <CreateAsync>d__);
				return <CreateAsync>d__.<>t__builder.Task;
			}

			// Token: 0x06000891 RID: 2193 RVA: 0x0004B580 File Offset: 0x00049780
			public static UniTask<ToLookup.Lookup<TKey, TElement>> CreateAsync<TSource>(ArraySegment<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, Func<TSource, CancellationToken, UniTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
			{
				ToLookup.Lookup<TKey, TElement>.<CreateAsync>d__9<TSource> <CreateAsync>d__;
				<CreateAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<ToLookup.Lookup<TKey, TElement>>.Create();
				<CreateAsync>d__.source = source;
				<CreateAsync>d__.keySelector = keySelector;
				<CreateAsync>d__.elementSelector = elementSelector;
				<CreateAsync>d__.comparer = comparer;
				<CreateAsync>d__.cancellationToken = cancellationToken;
				<CreateAsync>d__.<>1__state = -1;
				<CreateAsync>d__.<>t__builder.Start<ToLookup.Lookup<TKey, TElement>.<CreateAsync>d__9<TSource>>(ref <CreateAsync>d__);
				return <CreateAsync>d__.<>t__builder.Task;
			}

			// Token: 0x17000049 RID: 73
			public IEnumerable<TElement> this[TKey key]
			{
				get
				{
					ToLookup.Grouping<TKey, TElement> result;
					if (!this.dict.TryGetValue(key, out result))
					{
						return Enumerable.Empty<TElement>();
					}
					return result;
				}
			}

			// Token: 0x1700004A RID: 74
			// (get) Token: 0x06000893 RID: 2195 RVA: 0x0004B60A File Offset: 0x0004980A
			public int Count
			{
				get
				{
					return this.dict.Count;
				}
			}

			// Token: 0x06000894 RID: 2196 RVA: 0x0004B617 File Offset: 0x00049817
			public bool Contains(TKey key)
			{
				return this.dict.ContainsKey(key);
			}

			// Token: 0x06000895 RID: 2197 RVA: 0x0004B625 File Offset: 0x00049825
			public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
			{
				return this.dict.Values.GetEnumerator();
			}

			// Token: 0x06000896 RID: 2198 RVA: 0x0004B63C File Offset: 0x0004983C
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.dict.Values.GetEnumerator();
			}

			// Token: 0x0400126D RID: 4717
			private static readonly ToLookup.Lookup<TKey, TElement> empty = new ToLookup.Lookup<TKey, TElement>(new Dictionary<TKey, ToLookup.Grouping<TKey, TElement>>());

			// Token: 0x0400126E RID: 4718
			private readonly Dictionary<TKey, ToLookup.Grouping<TKey, TElement>> dict;
		}

		// Token: 0x020001E7 RID: 487
		private class Grouping<TKey, TElement> : IGrouping<TKey, TElement>, IEnumerable<TElement>, IEnumerable
		{
			// Token: 0x1700004B RID: 75
			// (get) Token: 0x06000898 RID: 2200 RVA: 0x0004B664 File Offset: 0x00049864
			// (set) Token: 0x06000899 RID: 2201 RVA: 0x0004B66C File Offset: 0x0004986C
			public TKey Key { get; private set; }

			// Token: 0x0600089A RID: 2202 RVA: 0x0004B675 File Offset: 0x00049875
			public Grouping(TKey key)
			{
				this.Key = key;
				this.elements = new List<TElement>();
			}

			// Token: 0x0600089B RID: 2203 RVA: 0x0004B68F File Offset: 0x0004988F
			public void Add(TElement value)
			{
				this.elements.Add(value);
			}

			// Token: 0x0600089C RID: 2204 RVA: 0x0004B69D File Offset: 0x0004989D
			public IEnumerator<TElement> GetEnumerator()
			{
				return this.elements.GetEnumerator();
			}

			// Token: 0x0600089D RID: 2205 RVA: 0x0004B6AF File Offset: 0x000498AF
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.elements.GetEnumerator();
			}

			// Token: 0x0600089E RID: 2206 RVA: 0x0004B6C1 File Offset: 0x000498C1
			public IUniTaskAsyncEnumerator<TElement> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
			{
				return this.ToUniTaskAsyncEnumerable<TElement>().GetAsyncEnumerator(cancellationToken);
			}

			// Token: 0x0600089F RID: 2207 RVA: 0x0004B6D0 File Offset: 0x000498D0
			public override string ToString()
			{
				string str = "Key: ";
				TKey key = this.Key;
				return str + ((key != null) ? key.ToString() : null) + ", Count: " + this.elements.Count.ToString();
			}

			// Token: 0x0400126F RID: 4719
			private readonly List<TElement> elements;
		}
	}
}
