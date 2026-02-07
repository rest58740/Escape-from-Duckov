using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks.Internal;
using UnityEngine;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000003 RID: 3
	public static class UniTaskAsyncEnumerable
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C3 File Offset: 0x000002C3
		public static UniTask<TSource> AggregateAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TSource, TSource>>(accumulator, "accumulator");
			return Aggregate.AggregateAsync<TSource>(source, accumulator, cancellationToken);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020E3 File Offset: 0x000002E3
		public static UniTask<TAccumulate> AggregateAsync<TSource, TAccumulate>(this IUniTaskAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TAccumulate, TSource, TAccumulate>>(accumulator, "accumulator");
			return Aggregate.AggregateAsync<TSource, TAccumulate>(source, seed, accumulator, cancellationToken);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002104 File Offset: 0x00000304
		public static UniTask<TResult> AggregateAsync<TSource, TAccumulate, TResult>(this IUniTaskAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TAccumulate, TSource, TAccumulate>>(accumulator, "accumulator");
			Error.ThrowArgumentNullException<Func<TAccumulate, TSource, TAccumulate>>(accumulator, "resultSelector");
			return Aggregate.AggregateAsync<TSource, TAccumulate, TResult>(source, seed, accumulator, resultSelector, cancellationToken);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002132 File Offset: 0x00000332
		public static UniTask<TSource> AggregateAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TSource, UniTask<TSource>> accumulator, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TSource, UniTask<TSource>>>(accumulator, "accumulator");
			return Aggregate.AggregateAwaitAsync<TSource>(source, accumulator, cancellationToken);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002152 File Offset: 0x00000352
		public static UniTask<TAccumulate> AggregateAwaitAsync<TSource, TAccumulate>(this IUniTaskAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, UniTask<TAccumulate>> accumulator, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TAccumulate, TSource, UniTask<TAccumulate>>>(accumulator, "accumulator");
			return Aggregate.AggregateAwaitAsync<TSource, TAccumulate>(source, seed, accumulator, cancellationToken);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002173 File Offset: 0x00000373
		public static UniTask<TResult> AggregateAwaitAsync<TSource, TAccumulate, TResult>(this IUniTaskAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, UniTask<TAccumulate>> accumulator, Func<TAccumulate, UniTask<TResult>> resultSelector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TAccumulate, TSource, UniTask<TAccumulate>>>(accumulator, "accumulator");
			Error.ThrowArgumentNullException<Func<TAccumulate, TSource, UniTask<TAccumulate>>>(accumulator, "resultSelector");
			return Aggregate.AggregateAwaitAsync<TSource, TAccumulate, TResult>(source, seed, accumulator, resultSelector, cancellationToken);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021A1 File Offset: 0x000003A1
		public static UniTask<TSource> AggregateAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TSource, CancellationToken, UniTask<TSource>> accumulator, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TSource, CancellationToken, UniTask<TSource>>>(accumulator, "accumulator");
			return Aggregate.AggregateAwaitWithCancellationAsync<TSource>(source, accumulator, cancellationToken);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021C1 File Offset: 0x000003C1
		public static UniTask<TAccumulate> AggregateAwaitWithCancellationAsync<TSource, TAccumulate>(this IUniTaskAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, CancellationToken, UniTask<TAccumulate>> accumulator, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TAccumulate, TSource, CancellationToken, UniTask<TAccumulate>>>(accumulator, "accumulator");
			return Aggregate.AggregateAwaitWithCancellationAsync<TSource, TAccumulate>(source, seed, accumulator, cancellationToken);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021E2 File Offset: 0x000003E2
		public static UniTask<TResult> AggregateAwaitWithCancellationAsync<TSource, TAccumulate, TResult>(this IUniTaskAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, CancellationToken, UniTask<TAccumulate>> accumulator, Func<TAccumulate, CancellationToken, UniTask<TResult>> resultSelector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TAccumulate, TSource, CancellationToken, UniTask<TAccumulate>>>(accumulator, "accumulator");
			Error.ThrowArgumentNullException<Func<TAccumulate, TSource, CancellationToken, UniTask<TAccumulate>>>(accumulator, "resultSelector");
			return Aggregate.AggregateAwaitWithCancellationAsync<TSource, TAccumulate, TResult>(source, seed, accumulator, resultSelector, cancellationToken);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002210 File Offset: 0x00000410
		public static UniTask<bool> AllAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, bool>>(predicate, "predicate");
			return All.AllAsync<TSource>(source, predicate, cancellationToken);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002230 File Offset: 0x00000430
		public static UniTask<bool> AllAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<bool>>>(predicate, "predicate");
			return All.AllAwaitAsync<TSource>(source, predicate, cancellationToken);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002250 File Offset: 0x00000450
		public static UniTask<bool> AllAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<bool>>>(predicate, "predicate");
			return All.AllAwaitWithCancellationAsync<TSource>(source, predicate, cancellationToken);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002270 File Offset: 0x00000470
		public static UniTask<bool> AnyAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return Any.AnyAsync<TSource>(source, cancellationToken);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002284 File Offset: 0x00000484
		public static UniTask<bool> AnyAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, bool>>(predicate, "predicate");
			return Any.AnyAsync<TSource>(source, predicate, cancellationToken);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000022A4 File Offset: 0x000004A4
		public static UniTask<bool> AnyAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<bool>>>(predicate, "predicate");
			return Any.AnyAwaitAsync<TSource>(source, predicate, cancellationToken);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022C4 File Offset: 0x000004C4
		public static UniTask<bool> AnyAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<bool>>>(predicate, "predicate");
			return Any.AnyAwaitWithCancellationAsync<TSource>(source, predicate, cancellationToken);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000022E4 File Offset: 0x000004E4
		public static IUniTaskAsyncEnumerable<TSource> Append<TSource>(this IUniTaskAsyncEnumerable<TSource> source, TSource element)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return new AppendPrepend<TSource>(source, element, true);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000022F9 File Offset: 0x000004F9
		public static IUniTaskAsyncEnumerable<TSource> Prepend<TSource>(this IUniTaskAsyncEnumerable<TSource> source, TSource element)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return new AppendPrepend<TSource>(source, element, false);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000230E File Offset: 0x0000050E
		public static IUniTaskAsyncEnumerable<TSource> AsUniTaskAsyncEnumerable<TSource>(this IUniTaskAsyncEnumerable<TSource> source)
		{
			return source;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002311 File Offset: 0x00000511
		public static UniTask<double> AverageAsync(this IUniTaskAsyncEnumerable<int> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<int>>(source, "source");
			return Average.AverageAsync(source, cancellationToken);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002325 File Offset: 0x00000525
		public static UniTask<double> AverageAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002345 File Offset: 0x00000545
		public static UniTask<double> AverageAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<int>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002365 File Offset: 0x00000565
		public static UniTask<double> AverageAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<int>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002385 File Offset: 0x00000585
		public static UniTask<double> AverageAsync(this IUniTaskAsyncEnumerable<long> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<long>>(source, "source");
			return Average.AverageAsync(source, cancellationToken);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002399 File Offset: 0x00000599
		public static UniTask<double> AverageAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000023B9 File Offset: 0x000005B9
		public static UniTask<double> AverageAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<long>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000023D9 File Offset: 0x000005D9
		public static UniTask<double> AverageAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<long>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000023F9 File Offset: 0x000005F9
		public static UniTask<float> AverageAsync(this IUniTaskAsyncEnumerable<float> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<float>>(source, "source");
			return Average.AverageAsync(source, cancellationToken);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000240D File Offset: 0x0000060D
		public static UniTask<float> AverageAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000242D File Offset: 0x0000062D
		public static UniTask<float> AverageAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<float>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000244D File Offset: 0x0000064D
		public static UniTask<float> AverageAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<float>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000246D File Offset: 0x0000066D
		public static UniTask<double> AverageAsync(this IUniTaskAsyncEnumerable<double> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<double>>(source, "source");
			return Average.AverageAsync(source, cancellationToken);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002481 File Offset: 0x00000681
		public static UniTask<double> AverageAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000024A1 File Offset: 0x000006A1
		public static UniTask<double> AverageAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<double>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000024C1 File Offset: 0x000006C1
		public static UniTask<double> AverageAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<double>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000024E1 File Offset: 0x000006E1
		public static UniTask<decimal> AverageAsync(this IUniTaskAsyncEnumerable<decimal> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<decimal>>(source, "source");
			return Average.AverageAsync(source, cancellationToken);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000024F5 File Offset: 0x000006F5
		public static UniTask<decimal> AverageAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002515 File Offset: 0x00000715
		public static UniTask<decimal> AverageAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<decimal>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002535 File Offset: 0x00000735
		public static UniTask<decimal> AverageAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<decimal>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002555 File Offset: 0x00000755
		public static UniTask<double?> AverageAsync(this IUniTaskAsyncEnumerable<int?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<int?>>(source, "source");
			return Average.AverageAsync(source, cancellationToken);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002569 File Offset: 0x00000769
		public static UniTask<double?> AverageAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002589 File Offset: 0x00000789
		public static UniTask<double?> AverageAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<int?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000025A9 File Offset: 0x000007A9
		public static UniTask<double?> AverageAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<int?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000025C9 File Offset: 0x000007C9
		public static UniTask<double?> AverageAsync(this IUniTaskAsyncEnumerable<long?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<long?>>(source, "source");
			return Average.AverageAsync(source, cancellationToken);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000025DD File Offset: 0x000007DD
		public static UniTask<double?> AverageAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000025FD File Offset: 0x000007FD
		public static UniTask<double?> AverageAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<long?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000261D File Offset: 0x0000081D
		public static UniTask<double?> AverageAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<long?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000263D File Offset: 0x0000083D
		public static UniTask<float?> AverageAsync(this IUniTaskAsyncEnumerable<float?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<float?>>(source, "source");
			return Average.AverageAsync(source, cancellationToken);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002651 File Offset: 0x00000851
		public static UniTask<float?> AverageAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002671 File Offset: 0x00000871
		public static UniTask<float?> AverageAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<float?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002691 File Offset: 0x00000891
		public static UniTask<float?> AverageAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<float?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000026B1 File Offset: 0x000008B1
		public static UniTask<double?> AverageAsync(this IUniTaskAsyncEnumerable<double?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<double?>>(source, "source");
			return Average.AverageAsync(source, cancellationToken);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000026C5 File Offset: 0x000008C5
		public static UniTask<double?> AverageAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000026E5 File Offset: 0x000008E5
		public static UniTask<double?> AverageAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<double?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002705 File Offset: 0x00000905
		public static UniTask<double?> AverageAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<double?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002725 File Offset: 0x00000925
		public static UniTask<decimal?> AverageAsync(this IUniTaskAsyncEnumerable<decimal?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<decimal?>>(source, "source");
			return Average.AverageAsync(source, cancellationToken);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002739 File Offset: 0x00000939
		public static UniTask<decimal?> AverageAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002759 File Offset: 0x00000959
		public static UniTask<decimal?> AverageAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002779 File Offset: 0x00000979
		public static UniTask<decimal?> AverageAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Average.AverageAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002799 File Offset: 0x00000999
		public static IUniTaskAsyncEnumerable<IList<TSource>> Buffer<TSource>(this IUniTaskAsyncEnumerable<TSource> source, int count)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			if (count <= 0)
			{
				throw Error.ArgumentOutOfRange("count");
			}
			return new Buffer<TSource>(source, count);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000027BC File Offset: 0x000009BC
		public static IUniTaskAsyncEnumerable<IList<TSource>> Buffer<TSource>(this IUniTaskAsyncEnumerable<TSource> source, int count, int skip)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			if (count <= 0)
			{
				throw Error.ArgumentOutOfRange("count");
			}
			if (skip <= 0)
			{
				throw Error.ArgumentOutOfRange("skip");
			}
			return new BufferSkip<TSource>(source, count, skip);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000027EF File Offset: 0x000009EF
		public static IUniTaskAsyncEnumerable<TResult> Cast<TResult>(this IUniTaskAsyncEnumerable<object> source)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<object>>(source, "source");
			return new Cast<TResult>(source);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002802 File Offset: 0x00000A02
		public static IUniTaskAsyncEnumerable<TResult> CombineLatest<T1, T2, TResult>(this IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, Func<T1, T2, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T1>>(source1, "source1");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T2>>(source2, "source2");
			Error.ThrowArgumentNullException<Func<T1, T2, TResult>>(resultSelector, "resultSelector");
			return new CombineLatest<T1, T2, TResult>(source1, source2, resultSelector);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000282D File Offset: 0x00000A2D
		public static IUniTaskAsyncEnumerable<TResult> CombineLatest<T1, T2, T3, TResult>(this IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, Func<T1, T2, T3, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T1>>(source1, "source1");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T2>>(source2, "source2");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T3>>(source3, "source3");
			Error.ThrowArgumentNullException<Func<T1, T2, T3, TResult>>(resultSelector, "resultSelector");
			return new CombineLatest<T1, T2, T3, TResult>(source1, source2, source3, resultSelector);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002864 File Offset: 0x00000A64
		public static IUniTaskAsyncEnumerable<TResult> CombineLatest<T1, T2, T3, T4, TResult>(this IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, Func<T1, T2, T3, T4, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T1>>(source1, "source1");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T2>>(source2, "source2");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T3>>(source3, "source3");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T4>>(source4, "source4");
			Error.ThrowArgumentNullException<Func<T1, T2, T3, T4, TResult>>(resultSelector, "resultSelector");
			return new CombineLatest<T1, T2, T3, T4, TResult>(source1, source2, source3, source4, resultSelector);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000028B4 File Offset: 0x00000AB4
		public static IUniTaskAsyncEnumerable<TResult> CombineLatest<T1, T2, T3, T4, T5, TResult>(this IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, Func<T1, T2, T3, T4, T5, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T1>>(source1, "source1");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T2>>(source2, "source2");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T3>>(source3, "source3");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T4>>(source4, "source4");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T5>>(source5, "source5");
			Error.ThrowArgumentNullException<Func<T1, T2, T3, T4, T5, TResult>>(resultSelector, "resultSelector");
			return new CombineLatest<T1, T2, T3, T4, T5, TResult>(source1, source2, source3, source4, source5, resultSelector);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002914 File Offset: 0x00000B14
		public static IUniTaskAsyncEnumerable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, TResult>(this IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T1>>(source1, "source1");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T2>>(source2, "source2");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T3>>(source3, "source3");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T4>>(source4, "source4");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T5>>(source5, "source5");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T6>>(source6, "source6");
			Error.ThrowArgumentNullException<Func<T1, T2, T3, T4, T5, T6, TResult>>(resultSelector, "resultSelector");
			return new CombineLatest<T1, T2, T3, T4, T5, T6, TResult>(source1, source2, source3, source4, source5, source6, resultSelector);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002980 File Offset: 0x00000B80
		public static IUniTaskAsyncEnumerable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>(this IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T1>>(source1, "source1");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T2>>(source2, "source2");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T3>>(source3, "source3");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T4>>(source4, "source4");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T5>>(source5, "source5");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T6>>(source6, "source6");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T7>>(source7, "source7");
			Error.ThrowArgumentNullException<Func<T1, T2, T3, T4, T5, T6, T7, TResult>>(resultSelector, "resultSelector");
			return new CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>(source1, source2, source3, source4, source5, source6, source7, resultSelector);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000029FC File Offset: 0x00000BFC
		public static IUniTaskAsyncEnumerable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T1>>(source1, "source1");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T2>>(source2, "source2");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T3>>(source3, "source3");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T4>>(source4, "source4");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T5>>(source5, "source5");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T6>>(source6, "source6");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T7>>(source7, "source7");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T8>>(source8, "source8");
			Error.ThrowArgumentNullException<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>(resultSelector, "resultSelector");
			return new CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, resultSelector);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002A84 File Offset: 0x00000C84
		public static IUniTaskAsyncEnumerable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, IUniTaskAsyncEnumerable<T9> source9, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T1>>(source1, "source1");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T2>>(source2, "source2");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T3>>(source3, "source3");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T4>>(source4, "source4");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T5>>(source5, "source5");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T6>>(source6, "source6");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T7>>(source7, "source7");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T8>>(source8, "source8");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T9>>(source9, "source9");
			Error.ThrowArgumentNullException<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>(resultSelector, "resultSelector");
			return new CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, resultSelector);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002B1C File Offset: 0x00000D1C
		public static IUniTaskAsyncEnumerable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, IUniTaskAsyncEnumerable<T9> source9, IUniTaskAsyncEnumerable<T10> source10, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T1>>(source1, "source1");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T2>>(source2, "source2");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T3>>(source3, "source3");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T4>>(source4, "source4");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T5>>(source5, "source5");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T6>>(source6, "source6");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T7>>(source7, "source7");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T8>>(source8, "source8");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T9>>(source9, "source9");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T10>>(source10, "source10");
			Error.ThrowArgumentNullException<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>(resultSelector, "resultSelector");
			return new CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, resultSelector);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002BC0 File Offset: 0x00000DC0
		public static IUniTaskAsyncEnumerable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, IUniTaskAsyncEnumerable<T9> source9, IUniTaskAsyncEnumerable<T10> source10, IUniTaskAsyncEnumerable<T11> source11, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T1>>(source1, "source1");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T2>>(source2, "source2");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T3>>(source3, "source3");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T4>>(source4, "source4");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T5>>(source5, "source5");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T6>>(source6, "source6");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T7>>(source7, "source7");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T8>>(source8, "source8");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T9>>(source9, "source9");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T10>>(source10, "source10");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T11>>(source11, "source11");
			Error.ThrowArgumentNullException<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>>(resultSelector, "resultSelector");
			return new CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, resultSelector);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002C74 File Offset: 0x00000E74
		public static IUniTaskAsyncEnumerable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, IUniTaskAsyncEnumerable<T9> source9, IUniTaskAsyncEnumerable<T10> source10, IUniTaskAsyncEnumerable<T11> source11, IUniTaskAsyncEnumerable<T12> source12, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T1>>(source1, "source1");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T2>>(source2, "source2");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T3>>(source3, "source3");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T4>>(source4, "source4");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T5>>(source5, "source5");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T6>>(source6, "source6");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T7>>(source7, "source7");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T8>>(source8, "source8");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T9>>(source9, "source9");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T10>>(source10, "source10");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T11>>(source11, "source11");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T12>>(source12, "source12");
			Error.ThrowArgumentNullException<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>>(resultSelector, "resultSelector");
			return new CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, resultSelector);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002D34 File Offset: 0x00000F34
		public static IUniTaskAsyncEnumerable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, IUniTaskAsyncEnumerable<T9> source9, IUniTaskAsyncEnumerable<T10> source10, IUniTaskAsyncEnumerable<T11> source11, IUniTaskAsyncEnumerable<T12> source12, IUniTaskAsyncEnumerable<T13> source13, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T1>>(source1, "source1");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T2>>(source2, "source2");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T3>>(source3, "source3");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T4>>(source4, "source4");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T5>>(source5, "source5");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T6>>(source6, "source6");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T7>>(source7, "source7");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T8>>(source8, "source8");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T9>>(source9, "source9");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T10>>(source10, "source10");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T11>>(source11, "source11");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T12>>(source12, "source12");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T13>>(source13, "source13");
			Error.ThrowArgumentNullException<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>>(resultSelector, "resultSelector");
			return new CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, resultSelector);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002E04 File Offset: 0x00001004
		public static IUniTaskAsyncEnumerable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, IUniTaskAsyncEnumerable<T9> source9, IUniTaskAsyncEnumerable<T10> source10, IUniTaskAsyncEnumerable<T11> source11, IUniTaskAsyncEnumerable<T12> source12, IUniTaskAsyncEnumerable<T13> source13, IUniTaskAsyncEnumerable<T14> source14, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T1>>(source1, "source1");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T2>>(source2, "source2");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T3>>(source3, "source3");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T4>>(source4, "source4");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T5>>(source5, "source5");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T6>>(source6, "source6");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T7>>(source7, "source7");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T8>>(source8, "source8");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T9>>(source9, "source9");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T10>>(source10, "source10");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T11>>(source11, "source11");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T12>>(source12, "source12");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T13>>(source13, "source13");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T14>>(source14, "source14");
			Error.ThrowArgumentNullException<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>>(resultSelector, "resultSelector");
			return new CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, resultSelector);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002EE0 File Offset: 0x000010E0
		public static IUniTaskAsyncEnumerable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, IUniTaskAsyncEnumerable<T9> source9, IUniTaskAsyncEnumerable<T10> source10, IUniTaskAsyncEnumerable<T11> source11, IUniTaskAsyncEnumerable<T12> source12, IUniTaskAsyncEnumerable<T13> source13, IUniTaskAsyncEnumerable<T14> source14, IUniTaskAsyncEnumerable<T15> source15, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T1>>(source1, "source1");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T2>>(source2, "source2");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T3>>(source3, "source3");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T4>>(source4, "source4");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T5>>(source5, "source5");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T6>>(source6, "source6");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T7>>(source7, "source7");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T8>>(source8, "source8");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T9>>(source9, "source9");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T10>>(source10, "source10");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T11>>(source11, "source11");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T12>>(source12, "source12");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T13>>(source13, "source13");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T14>>(source14, "source14");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T15>>(source15, "source15");
			Error.ThrowArgumentNullException<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>>(resultSelector, "resultSelector");
			return new CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, resultSelector);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002FCA File Offset: 0x000011CA
		public static IUniTaskAsyncEnumerable<TSource> Concat<TSource>(this IUniTaskAsyncEnumerable<TSource> first, IUniTaskAsyncEnumerable<TSource> second)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(first, "first");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(second, "second");
			return new Concat<TSource>(first, second);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002FE9 File Offset: 0x000011E9
		public static UniTask<bool> ContainsAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, TSource value, CancellationToken cancellationToken = default(CancellationToken))
		{
			return source.ContainsAsync(value, EqualityComparer<TSource>.Default, cancellationToken);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002FF8 File Offset: 0x000011F8
		public static UniTask<bool> ContainsAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IEqualityComparer<TSource>>(comparer, "comparer");
			return Contains.ContainsAsync<TSource>(source, value, comparer, cancellationToken);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003019 File Offset: 0x00001219
		public static UniTask<int> CountAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return Count.CountAsync<TSource>(source, cancellationToken);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000302D File Offset: 0x0000122D
		public static UniTask<int> CountAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, bool>>(predicate, "predicate");
			return Count.CountAsync<TSource>(source, predicate, cancellationToken);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000304D File Offset: 0x0000124D
		public static UniTask<int> CountAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<bool>>>(predicate, "predicate");
			return Count.CountAwaitAsync<TSource>(source, predicate, cancellationToken);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000306D File Offset: 0x0000126D
		public static UniTask<int> CountAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<bool>>>(predicate, "predicate");
			return Count.CountAwaitWithCancellationAsync<TSource>(source, predicate, cancellationToken);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000308D File Offset: 0x0000128D
		public static IUniTaskAsyncEnumerable<T> Create<T>(Func<IAsyncWriter<T>, CancellationToken, UniTask> create)
		{
			Error.ThrowArgumentNullException<Func<IAsyncWriter<T>, CancellationToken, UniTask>>(create, "create");
			return new Create<T>(create);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000030A0 File Offset: 0x000012A0
		public static IUniTaskAsyncEnumerable<TSource> DefaultIfEmpty<TSource>(this IUniTaskAsyncEnumerable<TSource> source)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return new DefaultIfEmpty<TSource>(source, default(TSource));
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000030C7 File Offset: 0x000012C7
		public static IUniTaskAsyncEnumerable<TSource> DefaultIfEmpty<TSource>(this IUniTaskAsyncEnumerable<TSource> source, TSource defaultValue)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return new DefaultIfEmpty<TSource>(source, defaultValue);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000030DB File Offset: 0x000012DB
		public static IUniTaskAsyncEnumerable<TSource> Distinct<TSource>(this IUniTaskAsyncEnumerable<TSource> source)
		{
			return source.Distinct(EqualityComparer<TSource>.Default);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000030E8 File Offset: 0x000012E8
		public static IUniTaskAsyncEnumerable<TSource> Distinct<TSource>(this IUniTaskAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IEqualityComparer<TSource>>(comparer, "comparer");
			return new Distinct<TSource>(source, comparer);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003107 File Offset: 0x00001307
		public static IUniTaskAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.Distinct(keySelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003115 File Offset: 0x00001315
		public static IUniTaskAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new Distinct<TSource, TKey>(source, keySelector, comparer);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003140 File Offset: 0x00001340
		public static IUniTaskAsyncEnumerable<TSource> DistinctAwait<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector)
		{
			return source.DistinctAwait(keySelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000314E File Offset: 0x0000134E
		public static IUniTaskAsyncEnumerable<TSource> DistinctAwait<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new DistinctAwait<TSource, TKey>(source, keySelector, comparer);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003179 File Offset: 0x00001379
		public static IUniTaskAsyncEnumerable<TSource> DistinctAwaitWithCancellation<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector)
		{
			return source.DistinctAwaitWithCancellation(keySelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003187 File Offset: 0x00001387
		public static IUniTaskAsyncEnumerable<TSource> DistinctAwaitWithCancellation<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new DistinctAwaitWithCancellation<TSource, TKey>(source, keySelector, comparer);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000031B2 File Offset: 0x000013B2
		public static IUniTaskAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IUniTaskAsyncEnumerable<TSource> source)
		{
			return source.DistinctUntilChanged(EqualityComparer<TSource>.Default);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000031BF File Offset: 0x000013BF
		public static IUniTaskAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IUniTaskAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IEqualityComparer<TSource>>(comparer, "comparer");
			return new DistinctUntilChanged<TSource>(source, comparer);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000031DE File Offset: 0x000013DE
		public static IUniTaskAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.DistinctUntilChanged(keySelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000031EC File Offset: 0x000013EC
		public static IUniTaskAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new DistinctUntilChanged<TSource, TKey>(source, keySelector, comparer);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003217 File Offset: 0x00001417
		public static IUniTaskAsyncEnumerable<TSource> DistinctUntilChangedAwait<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector)
		{
			return source.DistinctUntilChangedAwait(keySelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003225 File Offset: 0x00001425
		public static IUniTaskAsyncEnumerable<TSource> DistinctUntilChangedAwait<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new DistinctUntilChangedAwait<TSource, TKey>(source, keySelector, comparer);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003250 File Offset: 0x00001450
		public static IUniTaskAsyncEnumerable<TSource> DistinctUntilChangedAwaitWithCancellation<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector)
		{
			return source.DistinctUntilChangedAwaitWithCancellation(keySelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000325E File Offset: 0x0000145E
		public static IUniTaskAsyncEnumerable<TSource> DistinctUntilChangedAwaitWithCancellation<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new DistinctUntilChangedAwaitWithCancellation<TSource, TKey>(source, keySelector, comparer);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003289 File Offset: 0x00001489
		public static IUniTaskAsyncEnumerable<TSource> Do<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Action<TSource> onNext)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return source.Do(onNext, null, null);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000329F File Offset: 0x0000149F
		public static IUniTaskAsyncEnumerable<TSource> Do<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return source.Do(onNext, onError, null);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000032B5 File Offset: 0x000014B5
		public static IUniTaskAsyncEnumerable<TSource> Do<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Action<TSource> onNext, Action onCompleted)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return source.Do(onNext, null, onCompleted);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000032CB File Offset: 0x000014CB
		public static IUniTaskAsyncEnumerable<TSource> Do<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return new Do<TSource>(source, onNext, onError, onCompleted);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000032E4 File Offset: 0x000014E4
		public static IUniTaskAsyncEnumerable<TSource> Do<TSource>(this IUniTaskAsyncEnumerable<TSource> source, IObserver<TSource> observer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IObserver<TSource>>(observer, "observer");
			return source.Do(new Action<TSource>(observer.OnNext), new Action<Exception>(observer.OnError), new Action(observer.OnCompleted));
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003334 File Offset: 0x00001534
		public static UniTask<TSource> ElementAtAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return ElementAt.ElementAtAsync<TSource>(source, index, cancellationToken, false);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000334A File Offset: 0x0000154A
		public static UniTask<TSource> ElementAtOrDefaultAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return ElementAt.ElementAtAsync<TSource>(source, index, cancellationToken, true);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003360 File Offset: 0x00001560
		public static IUniTaskAsyncEnumerable<T> Empty<T>()
		{
			return Empty<T>.Instance;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003367 File Offset: 0x00001567
		public static IUniTaskAsyncEnumerable<TSource> Except<TSource>(this IUniTaskAsyncEnumerable<TSource> first, IUniTaskAsyncEnumerable<TSource> second)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(first, "first");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(second, "second");
			return new Except<TSource>(first, second, EqualityComparer<TSource>.Default);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000338B File Offset: 0x0000158B
		public static IUniTaskAsyncEnumerable<TSource> Except<TSource>(this IUniTaskAsyncEnumerable<TSource> first, IUniTaskAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(first, "first");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(second, "second");
			Error.ThrowArgumentNullException<IEqualityComparer<TSource>>(comparer, "comparer");
			return new Except<TSource>(first, second, comparer);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000033B6 File Offset: 0x000015B6
		public static UniTask<TSource> FirstAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return First.FirstAsync<TSource>(source, cancellationToken, false);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000033CB File Offset: 0x000015CB
		public static UniTask<TSource> FirstAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, bool>>(predicate, "predicate");
			return First.FirstAsync<TSource>(source, predicate, cancellationToken, false);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000033EC File Offset: 0x000015EC
		public static UniTask<TSource> FirstAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<bool>>>(predicate, "predicate");
			return First.FirstAwaitAsync<TSource>(source, predicate, cancellationToken, false);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000340D File Offset: 0x0000160D
		public static UniTask<TSource> FirstAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<bool>>>(predicate, "predicate");
			return First.FirstAwaitWithCancellationAsync<TSource>(source, predicate, cancellationToken, false);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000342E File Offset: 0x0000162E
		public static UniTask<TSource> FirstOrDefaultAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return First.FirstAsync<TSource>(source, cancellationToken, true);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003443 File Offset: 0x00001643
		public static UniTask<TSource> FirstOrDefaultAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, bool>>(predicate, "predicate");
			return First.FirstAsync<TSource>(source, predicate, cancellationToken, true);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003464 File Offset: 0x00001664
		public static UniTask<TSource> FirstOrDefaultAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<bool>>>(predicate, "predicate");
			return First.FirstAwaitAsync<TSource>(source, predicate, cancellationToken, true);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003485 File Offset: 0x00001685
		public static UniTask<TSource> FirstOrDefaultAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<bool>>>(predicate, "predicate");
			return First.FirstAwaitWithCancellationAsync<TSource>(source, predicate, cancellationToken, true);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000034A6 File Offset: 0x000016A6
		public static UniTask ForEachAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Action<TSource> action, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Action<TSource>>(action, "action");
			return ForEach.ForEachAsync<TSource>(source, action, cancellationToken);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000034C6 File Offset: 0x000016C6
		public static UniTask ForEachAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Action<TSource, int> action, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Action<TSource, int>>(action, "action");
			return ForEach.ForEachAsync<TSource>(source, action, cancellationToken);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000034E6 File Offset: 0x000016E6
		[Obsolete("Use ForEachAwaitAsync instead.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static UniTask ForEachAsync<T>(this IUniTaskAsyncEnumerable<T> source, Func<T, UniTask> action, CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotSupportedException("Use ForEachAwaitAsync instead.");
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000034F2 File Offset: 0x000016F2
		[Obsolete("Use ForEachAwaitAsync instead.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static UniTask ForEachAsync<T>(this IUniTaskAsyncEnumerable<T> source, Func<T, int, UniTask> action, CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotSupportedException("Use ForEachAwaitAsync instead.");
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000034FE File Offset: 0x000016FE
		public static UniTask ForEachAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask> action, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask>>(action, "action");
			return ForEach.ForEachAwaitAsync<TSource>(source, action, cancellationToken);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000351E File Offset: 0x0000171E
		public static UniTask ForEachAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask> action, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, UniTask>>(action, "action");
			return ForEach.ForEachAwaitAsync<TSource>(source, action, cancellationToken);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000353E File Offset: 0x0000173E
		public static UniTask ForEachAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask> action, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask>>(action, "action");
			return ForEach.ForEachAwaitWithCancellationAsync<TSource>(source, action, cancellationToken);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000355E File Offset: 0x0000175E
		public static UniTask ForEachAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, UniTask> action, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, CancellationToken, UniTask>>(action, "action");
			return ForEach.ForEachAwaitWithCancellationAsync<TSource>(source, action, cancellationToken);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003580 File Offset: 0x00001780
		public static IUniTaskAsyncEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			return new GroupBy<TSource, TKey, TSource>(source, keySelector, (TSource x) => x, EqualityComparer<TKey>.Default);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000035D0 File Offset: 0x000017D0
		public static IUniTaskAsyncEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new GroupBy<TSource, TKey, TSource>(source, keySelector, (TSource x) => x, comparer);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003625 File Offset: 0x00001825
		public static IUniTaskAsyncEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, TElement>>(elementSelector, "elementSelector");
			return new GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003655 File Offset: 0x00001855
		public static IUniTaskAsyncEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, TElement>>(elementSelector, "elementSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000368C File Offset: 0x0000188C
		public static IUniTaskAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TKey, IEnumerable<TSource>, TResult>>(resultSelector, "resultSelector");
			return new GroupBy<TSource, TKey, TSource, TResult>(source, keySelector, (TSource x) => x, resultSelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000036E8 File Offset: 0x000018E8
		public static IUniTaskAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TKey, IEnumerable<TSource>, TResult>>(resultSelector, "resultSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new GroupBy<TSource, TKey, TSource, TResult>(source, keySelector, (TSource x) => x, resultSelector, comparer);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003749 File Offset: 0x00001949
		public static IUniTaskAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, TElement>>(elementSelector, "elementSelector");
			Error.ThrowArgumentNullException<Func<TKey, IEnumerable<TElement>, TResult>>(resultSelector, "resultSelector");
			return new GroupBy<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003788 File Offset: 0x00001988
		public static IUniTaskAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, TElement>>(elementSelector, "elementSelector");
			Error.ThrowArgumentNullException<Func<TKey, IEnumerable<TElement>, TResult>>(resultSelector, "resultSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new GroupBy<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, comparer);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000037D8 File Offset: 0x000019D8
		public static IUniTaskAsyncEnumerable<IGrouping<TKey, TSource>> GroupByAwait<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			return new GroupByAwait<TSource, TKey, TSource>(source, keySelector, (TSource x) => UniTask.FromResult<TSource>(x), EqualityComparer<TKey>.Default);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003828 File Offset: 0x00001A28
		public static IUniTaskAsyncEnumerable<IGrouping<TKey, TSource>> GroupByAwait<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new GroupByAwait<TSource, TKey, TSource>(source, keySelector, (TSource x) => UniTask.FromResult<TSource>(x), comparer);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000387D File Offset: 0x00001A7D
		public static IUniTaskAsyncEnumerable<IGrouping<TKey, TElement>> GroupByAwait<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, Func<TSource, UniTask<TElement>> elementSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TElement>>>(elementSelector, "elementSelector");
			return new GroupByAwait<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000038AD File Offset: 0x00001AAD
		public static IUniTaskAsyncEnumerable<IGrouping<TKey, TElement>> GroupByAwait<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, Func<TSource, UniTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TElement>>>(elementSelector, "elementSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new GroupByAwait<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000038E4 File Offset: 0x00001AE4
		public static IUniTaskAsyncEnumerable<TResult> GroupByAwait<TSource, TKey, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, Func<TKey, IEnumerable<TSource>, UniTask<TResult>> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TKey, IEnumerable<TSource>, UniTask<TResult>>>(resultSelector, "resultSelector");
			return new GroupByAwait<TSource, TKey, TSource, TResult>(source, keySelector, (TSource x) => UniTask.FromResult<TSource>(x), resultSelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000393E File Offset: 0x00001B3E
		public static IUniTaskAsyncEnumerable<TResult> GroupByAwait<TSource, TKey, TElement, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, Func<TSource, UniTask<TElement>> elementSelector, Func<TKey, IEnumerable<TElement>, UniTask<TResult>> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TElement>>>(elementSelector, "elementSelector");
			Error.ThrowArgumentNullException<Func<TKey, IEnumerable<TElement>, UniTask<TResult>>>(resultSelector, "resultSelector");
			return new GroupByAwait<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000397C File Offset: 0x00001B7C
		public static IUniTaskAsyncEnumerable<TResult> GroupByAwait<TSource, TKey, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, Func<TKey, IEnumerable<TSource>, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TKey, IEnumerable<TSource>, UniTask<TResult>>>(resultSelector, "resultSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new GroupByAwait<TSource, TKey, TSource, TResult>(source, keySelector, (TSource x) => UniTask.FromResult<TSource>(x), resultSelector, comparer);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000039E0 File Offset: 0x00001BE0
		public static IUniTaskAsyncEnumerable<TResult> GroupByAwait<TSource, TKey, TElement, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, Func<TSource, UniTask<TElement>> elementSelector, Func<TKey, IEnumerable<TElement>, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TElement>>>(elementSelector, "elementSelector");
			Error.ThrowArgumentNullException<Func<TKey, IEnumerable<TElement>, UniTask<TResult>>>(resultSelector, "resultSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new GroupByAwait<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, comparer);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003A30 File Offset: 0x00001C30
		public static IUniTaskAsyncEnumerable<IGrouping<TKey, TSource>> GroupByAwaitWithCancellation<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			return new GroupByAwaitWithCancellation<TSource, TKey, TSource>(source, keySelector, (TSource x, CancellationToken _) => UniTask.FromResult<TSource>(x), EqualityComparer<TKey>.Default);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003A80 File Offset: 0x00001C80
		public static IUniTaskAsyncEnumerable<IGrouping<TKey, TSource>> GroupByAwaitWithCancellation<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new GroupByAwaitWithCancellation<TSource, TKey, TSource>(source, keySelector, (TSource x, CancellationToken _) => UniTask.FromResult<TSource>(x), comparer);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003AD5 File Offset: 0x00001CD5
		public static IUniTaskAsyncEnumerable<IGrouping<TKey, TElement>> GroupByAwaitWithCancellation<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, Func<TSource, CancellationToken, UniTask<TElement>> elementSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TElement>>>(elementSelector, "elementSelector");
			return new GroupByAwaitWithCancellation<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003B05 File Offset: 0x00001D05
		public static IUniTaskAsyncEnumerable<IGrouping<TKey, TElement>> GroupByAwaitWithCancellation<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, Func<TSource, CancellationToken, UniTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TElement>>>(elementSelector, "elementSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new GroupByAwaitWithCancellation<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003B3C File Offset: 0x00001D3C
		public static IUniTaskAsyncEnumerable<TResult> GroupByAwaitWithCancellation<TSource, TKey, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, Func<TKey, IEnumerable<TSource>, CancellationToken, UniTask<TResult>> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TKey, IEnumerable<TSource>, CancellationToken, UniTask<TResult>>>(resultSelector, "resultSelector");
			return new GroupByAwaitWithCancellation<TSource, TKey, TSource, TResult>(source, keySelector, (TSource x, CancellationToken _) => UniTask.FromResult<TSource>(x), resultSelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003B96 File Offset: 0x00001D96
		public static IUniTaskAsyncEnumerable<TResult> GroupByAwaitWithCancellation<TSource, TKey, TElement, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, Func<TSource, CancellationToken, UniTask<TElement>> elementSelector, Func<TKey, IEnumerable<TElement>, CancellationToken, UniTask<TResult>> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TElement>>>(elementSelector, "elementSelector");
			Error.ThrowArgumentNullException<Func<TKey, IEnumerable<TElement>, CancellationToken, UniTask<TResult>>>(resultSelector, "resultSelector");
			return new GroupByAwaitWithCancellation<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003BD4 File Offset: 0x00001DD4
		public static IUniTaskAsyncEnumerable<TResult> GroupByAwaitWithCancellation<TSource, TKey, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, Func<TKey, IEnumerable<TSource>, CancellationToken, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TKey, IEnumerable<TSource>, CancellationToken, UniTask<TResult>>>(resultSelector, "resultSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new GroupByAwaitWithCancellation<TSource, TKey, TSource, TResult>(source, keySelector, (TSource x, CancellationToken _) => UniTask.FromResult<TSource>(x), resultSelector, comparer);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003C38 File Offset: 0x00001E38
		public static IUniTaskAsyncEnumerable<TResult> GroupByAwaitWithCancellation<TSource, TKey, TElement, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, Func<TSource, CancellationToken, UniTask<TElement>> elementSelector, Func<TKey, IEnumerable<TElement>, CancellationToken, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TElement>>>(elementSelector, "elementSelector");
			Error.ThrowArgumentNullException<Func<TKey, IEnumerable<TElement>, CancellationToken, UniTask<TResult>>>(resultSelector, "resultSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new GroupByAwaitWithCancellation<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, comparer);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003C88 File Offset: 0x00001E88
		public static IUniTaskAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TOuter>>(outer, "outer");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TInner>>(inner, "inner");
			Error.ThrowArgumentNullException<Func<TOuter, TKey>>(outerKeySelector, "outerKeySelector");
			Error.ThrowArgumentNullException<Func<TInner, TKey>>(innerKeySelector, "innerKeySelector");
			Error.ThrowArgumentNullException<Func<TOuter, IEnumerable<TInner>, TResult>>(resultSelector, "resultSelector");
			return new GroupJoin<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003CE0 File Offset: 0x00001EE0
		public static IUniTaskAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TOuter>>(outer, "outer");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TInner>>(inner, "inner");
			Error.ThrowArgumentNullException<Func<TOuter, TKey>>(outerKeySelector, "outerKeySelector");
			Error.ThrowArgumentNullException<Func<TInner, TKey>>(innerKeySelector, "innerKeySelector");
			Error.ThrowArgumentNullException<Func<TOuter, IEnumerable<TInner>, TResult>>(resultSelector, "resultSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new GroupJoin<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003D40 File Offset: 0x00001F40
		public static IUniTaskAsyncEnumerable<TResult> GroupJoinAwait<TOuter, TInner, TKey, TResult>(this IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, UniTask<TKey>> outerKeySelector, Func<TInner, UniTask<TKey>> innerKeySelector, Func<TOuter, IEnumerable<TInner>, UniTask<TResult>> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TOuter>>(outer, "outer");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TInner>>(inner, "inner");
			Error.ThrowArgumentNullException<Func<TOuter, UniTask<TKey>>>(outerKeySelector, "outerKeySelector");
			Error.ThrowArgumentNullException<Func<TInner, UniTask<TKey>>>(innerKeySelector, "innerKeySelector");
			Error.ThrowArgumentNullException<Func<TOuter, IEnumerable<TInner>, UniTask<TResult>>>(resultSelector, "resultSelector");
			return new GroupJoinAwait<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003D98 File Offset: 0x00001F98
		public static IUniTaskAsyncEnumerable<TResult> GroupJoinAwait<TOuter, TInner, TKey, TResult>(this IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, UniTask<TKey>> outerKeySelector, Func<TInner, UniTask<TKey>> innerKeySelector, Func<TOuter, IEnumerable<TInner>, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TOuter>>(outer, "outer");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TInner>>(inner, "inner");
			Error.ThrowArgumentNullException<Func<TOuter, UniTask<TKey>>>(outerKeySelector, "outerKeySelector");
			Error.ThrowArgumentNullException<Func<TInner, UniTask<TKey>>>(innerKeySelector, "innerKeySelector");
			Error.ThrowArgumentNullException<Func<TOuter, IEnumerable<TInner>, UniTask<TResult>>>(resultSelector, "resultSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new GroupJoinAwait<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003DF8 File Offset: 0x00001FF8
		public static IUniTaskAsyncEnumerable<TResult> GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>(this IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, CancellationToken, UniTask<TKey>> outerKeySelector, Func<TInner, CancellationToken, UniTask<TKey>> innerKeySelector, Func<TOuter, IEnumerable<TInner>, CancellationToken, UniTask<TResult>> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TOuter>>(outer, "outer");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TInner>>(inner, "inner");
			Error.ThrowArgumentNullException<Func<TOuter, CancellationToken, UniTask<TKey>>>(outerKeySelector, "outerKeySelector");
			Error.ThrowArgumentNullException<Func<TInner, CancellationToken, UniTask<TKey>>>(innerKeySelector, "innerKeySelector");
			Error.ThrowArgumentNullException<Func<TOuter, IEnumerable<TInner>, CancellationToken, UniTask<TResult>>>(resultSelector, "resultSelector");
			return new GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003E50 File Offset: 0x00002050
		public static IUniTaskAsyncEnumerable<TResult> GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>(this IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, CancellationToken, UniTask<TKey>> outerKeySelector, Func<TInner, CancellationToken, UniTask<TKey>> innerKeySelector, Func<TOuter, IEnumerable<TInner>, CancellationToken, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TOuter>>(outer, "outer");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TInner>>(inner, "inner");
			Error.ThrowArgumentNullException<Func<TOuter, CancellationToken, UniTask<TKey>>>(outerKeySelector, "outerKeySelector");
			Error.ThrowArgumentNullException<Func<TInner, CancellationToken, UniTask<TKey>>>(innerKeySelector, "innerKeySelector");
			Error.ThrowArgumentNullException<Func<TOuter, IEnumerable<TInner>, CancellationToken, UniTask<TResult>>>(resultSelector, "resultSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003EAE File Offset: 0x000020AE
		public static IUniTaskAsyncEnumerable<TSource> Intersect<TSource>(this IUniTaskAsyncEnumerable<TSource> first, IUniTaskAsyncEnumerable<TSource> second)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(first, "first");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(second, "second");
			return new Intersect<TSource>(first, second, EqualityComparer<TSource>.Default);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003ED2 File Offset: 0x000020D2
		public static IUniTaskAsyncEnumerable<TSource> Intersect<TSource>(this IUniTaskAsyncEnumerable<TSource> first, IUniTaskAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(first, "first");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(second, "second");
			Error.ThrowArgumentNullException<IEqualityComparer<TSource>>(comparer, "comparer");
			return new Intersect<TSource>(first, second, comparer);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003F00 File Offset: 0x00002100
		public static IUniTaskAsyncEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TOuter>>(outer, "outer");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TInner>>(inner, "inner");
			Error.ThrowArgumentNullException<Func<TOuter, TKey>>(outerKeySelector, "outerKeySelector");
			Error.ThrowArgumentNullException<Func<TInner, TKey>>(innerKeySelector, "innerKeySelector");
			Error.ThrowArgumentNullException<Func<TOuter, TInner, TResult>>(resultSelector, "resultSelector");
			return new Join<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003F58 File Offset: 0x00002158
		public static IUniTaskAsyncEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TOuter>>(outer, "outer");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TInner>>(inner, "inner");
			Error.ThrowArgumentNullException<Func<TOuter, TKey>>(outerKeySelector, "outerKeySelector");
			Error.ThrowArgumentNullException<Func<TInner, TKey>>(innerKeySelector, "innerKeySelector");
			Error.ThrowArgumentNullException<Func<TOuter, TInner, TResult>>(resultSelector, "resultSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new Join<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003FB8 File Offset: 0x000021B8
		public static IUniTaskAsyncEnumerable<TResult> JoinAwait<TOuter, TInner, TKey, TResult>(this IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, UniTask<TKey>> outerKeySelector, Func<TInner, UniTask<TKey>> innerKeySelector, Func<TOuter, TInner, UniTask<TResult>> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TOuter>>(outer, "outer");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TInner>>(inner, "inner");
			Error.ThrowArgumentNullException<Func<TOuter, UniTask<TKey>>>(outerKeySelector, "outerKeySelector");
			Error.ThrowArgumentNullException<Func<TInner, UniTask<TKey>>>(innerKeySelector, "innerKeySelector");
			Error.ThrowArgumentNullException<Func<TOuter, TInner, UniTask<TResult>>>(resultSelector, "resultSelector");
			return new JoinAwait<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004010 File Offset: 0x00002210
		public static IUniTaskAsyncEnumerable<TResult> JoinAwait<TOuter, TInner, TKey, TResult>(this IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, UniTask<TKey>> outerKeySelector, Func<TInner, UniTask<TKey>> innerKeySelector, Func<TOuter, TInner, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TOuter>>(outer, "outer");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TInner>>(inner, "inner");
			Error.ThrowArgumentNullException<Func<TOuter, UniTask<TKey>>>(outerKeySelector, "outerKeySelector");
			Error.ThrowArgumentNullException<Func<TInner, UniTask<TKey>>>(innerKeySelector, "innerKeySelector");
			Error.ThrowArgumentNullException<Func<TOuter, TInner, UniTask<TResult>>>(resultSelector, "resultSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new JoinAwait<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004070 File Offset: 0x00002270
		public static IUniTaskAsyncEnumerable<TResult> JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>(this IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, CancellationToken, UniTask<TKey>> outerKeySelector, Func<TInner, CancellationToken, UniTask<TKey>> innerKeySelector, Func<TOuter, TInner, CancellationToken, UniTask<TResult>> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TOuter>>(outer, "outer");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TInner>>(inner, "inner");
			Error.ThrowArgumentNullException<Func<TOuter, CancellationToken, UniTask<TKey>>>(outerKeySelector, "outerKeySelector");
			Error.ThrowArgumentNullException<Func<TInner, CancellationToken, UniTask<TKey>>>(innerKeySelector, "innerKeySelector");
			Error.ThrowArgumentNullException<Func<TOuter, TInner, CancellationToken, UniTask<TResult>>>(resultSelector, "resultSelector");
			return new JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000040C8 File Offset: 0x000022C8
		public static IUniTaskAsyncEnumerable<TResult> JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>(this IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, CancellationToken, UniTask<TKey>> outerKeySelector, Func<TInner, CancellationToken, UniTask<TKey>> innerKeySelector, Func<TOuter, TInner, CancellationToken, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TOuter>>(outer, "outer");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TInner>>(inner, "inner");
			Error.ThrowArgumentNullException<Func<TOuter, CancellationToken, UniTask<TKey>>>(outerKeySelector, "outerKeySelector");
			Error.ThrowArgumentNullException<Func<TInner, CancellationToken, UniTask<TKey>>>(innerKeySelector, "innerKeySelector");
			Error.ThrowArgumentNullException<Func<TOuter, TInner, CancellationToken, UniTask<TResult>>>(resultSelector, "resultSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return new JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004126 File Offset: 0x00002326
		public static UniTask<TSource> LastAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return Last.LastAsync<TSource>(source, cancellationToken, false);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000413B File Offset: 0x0000233B
		public static UniTask<TSource> LastAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, bool>>(predicate, "predicate");
			return Last.LastAsync<TSource>(source, predicate, cancellationToken, false);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000415C File Offset: 0x0000235C
		public static UniTask<TSource> LastAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<bool>>>(predicate, "predicate");
			return Last.LastAwaitAsync<TSource>(source, predicate, cancellationToken, false);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000417D File Offset: 0x0000237D
		public static UniTask<TSource> LastAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<bool>>>(predicate, "predicate");
			return Last.LastAwaitWithCancellationAsync<TSource>(source, predicate, cancellationToken, false);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000419E File Offset: 0x0000239E
		public static UniTask<TSource> LastOrDefaultAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return Last.LastAsync<TSource>(source, cancellationToken, true);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000041B3 File Offset: 0x000023B3
		public static UniTask<TSource> LastOrDefaultAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, bool>>(predicate, "predicate");
			return Last.LastAsync<TSource>(source, predicate, cancellationToken, true);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000041D4 File Offset: 0x000023D4
		public static UniTask<TSource> LastOrDefaultAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<bool>>>(predicate, "predicate");
			return Last.LastAwaitAsync<TSource>(source, predicate, cancellationToken, true);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000041F5 File Offset: 0x000023F5
		public static UniTask<TSource> LastOrDefaultAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<bool>>>(predicate, "predicate");
			return Last.LastAwaitWithCancellationAsync<TSource>(source, predicate, cancellationToken, true);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004216 File Offset: 0x00002416
		public static UniTask<long> LongCountAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return LongCount.LongCountAsync<TSource>(source, cancellationToken);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000422A File Offset: 0x0000242A
		public static UniTask<long> LongCountAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, bool>>(predicate, "predicate");
			return LongCount.LongCountAsync<TSource>(source, predicate, cancellationToken);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000424A File Offset: 0x0000244A
		public static UniTask<long> LongCountAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<bool>>>(predicate, "predicate");
			return LongCount.LongCountAwaitAsync<TSource>(source, predicate, cancellationToken);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000426A File Offset: 0x0000246A
		public static UniTask<long> LongCountAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<bool>>>(predicate, "predicate");
			return LongCount.LongCountAwaitWithCancellationAsync<TSource>(source, predicate, cancellationToken);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000428A File Offset: 0x0000248A
		public static UniTask<TSource> MaxAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return Max.MaxAsync<TSource>(source, cancellationToken);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000429E File Offset: 0x0000249E
		public static UniTask<TResult> MaxAsync<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TResult> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAsync<TSource, TResult>(source, selector, cancellationToken);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000042BE File Offset: 0x000024BE
		public static UniTask<TResult> MaxAwaitAsync<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TResult>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitAsync<TSource, TResult>(source, selector, cancellationToken);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000042DE File Offset: 0x000024DE
		public static UniTask<TResult> MaxAwaitWithCancellationAsync<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TResult>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitWithCancellationAsync<TSource, TResult>(source, selector, cancellationToken);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000042FE File Offset: 0x000024FE
		public static IUniTaskAsyncEnumerable<T> Merge<T>(this IUniTaskAsyncEnumerable<T> first, IUniTaskAsyncEnumerable<T> second)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T>>(first, "first");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T>>(second, "second");
			return new Merge<T>(new IUniTaskAsyncEnumerable<T>[]
			{
				first,
				second
			});
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004329 File Offset: 0x00002529
		public static IUniTaskAsyncEnumerable<T> Merge<T>(this IUniTaskAsyncEnumerable<T> first, IUniTaskAsyncEnumerable<T> second, IUniTaskAsyncEnumerable<T> third)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T>>(first, "first");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T>>(second, "second");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<T>>(third, "third");
			return new Merge<T>(new IUniTaskAsyncEnumerable<T>[]
			{
				first,
				second,
				third
			});
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004364 File Offset: 0x00002564
		public static IUniTaskAsyncEnumerable<T> Merge<T>(this IEnumerable<IUniTaskAsyncEnumerable<T>> sources)
		{
			IUniTaskAsyncEnumerable<T>[] array = sources as IUniTaskAsyncEnumerable<T>[];
			if (array == null)
			{
				return new Merge<T>(sources.ToArray<IUniTaskAsyncEnumerable<T>>());
			}
			return new Merge<T>(array);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000438D File Offset: 0x0000258D
		public static IUniTaskAsyncEnumerable<T> Merge<T>(params IUniTaskAsyncEnumerable<T>[] sources)
		{
			return new Merge<T>(sources);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004395 File Offset: 0x00002595
		public static UniTask<TSource> MinAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return Min.MinAsync<TSource>(source, cancellationToken);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000043A9 File Offset: 0x000025A9
		public static UniTask<TResult> MinAsync<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TResult> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAsync<TSource, TResult>(source, selector, cancellationToken);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000043C9 File Offset: 0x000025C9
		public static UniTask<TResult> MinAwaitAsync<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TResult>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitAsync<TSource, TResult>(source, selector, cancellationToken);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000043E9 File Offset: 0x000025E9
		public static UniTask<TResult> MinAwaitWithCancellationAsync<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TResult>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitWithCancellationAsync<TSource, TResult>(source, selector, cancellationToken);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004409 File Offset: 0x00002609
		public static UniTask<int> MinAsync(this IUniTaskAsyncEnumerable<int> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<int>>(source, "source");
			return Min.MinAsync(source, cancellationToken);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0000441D File Offset: 0x0000261D
		public static UniTask<int> MinAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000443D File Offset: 0x0000263D
		public static UniTask<int> MinAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<int>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000445D File Offset: 0x0000265D
		public static UniTask<int> MinAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<int>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000447D File Offset: 0x0000267D
		public static UniTask<long> MinAsync(this IUniTaskAsyncEnumerable<long> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<long>>(source, "source");
			return Min.MinAsync(source, cancellationToken);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004491 File Offset: 0x00002691
		public static UniTask<long> MinAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000044B1 File Offset: 0x000026B1
		public static UniTask<long> MinAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<long>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000044D1 File Offset: 0x000026D1
		public static UniTask<long> MinAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<long>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000044F1 File Offset: 0x000026F1
		public static UniTask<float> MinAsync(this IUniTaskAsyncEnumerable<float> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<float>>(source, "source");
			return Min.MinAsync(source, cancellationToken);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004505 File Offset: 0x00002705
		public static UniTask<float> MinAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004525 File Offset: 0x00002725
		public static UniTask<float> MinAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<float>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004545 File Offset: 0x00002745
		public static UniTask<float> MinAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<float>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004565 File Offset: 0x00002765
		public static UniTask<double> MinAsync(this IUniTaskAsyncEnumerable<double> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<double>>(source, "source");
			return Min.MinAsync(source, cancellationToken);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004579 File Offset: 0x00002779
		public static UniTask<double> MinAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004599 File Offset: 0x00002799
		public static UniTask<double> MinAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<double>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000045B9 File Offset: 0x000027B9
		public static UniTask<double> MinAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<double>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000045D9 File Offset: 0x000027D9
		public static UniTask<decimal> MinAsync(this IUniTaskAsyncEnumerable<decimal> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<decimal>>(source, "source");
			return Min.MinAsync(source, cancellationToken);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000045ED File Offset: 0x000027ED
		public static UniTask<decimal> MinAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000460D File Offset: 0x0000280D
		public static UniTask<decimal> MinAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<decimal>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000462D File Offset: 0x0000282D
		public static UniTask<decimal> MinAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<decimal>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000464D File Offset: 0x0000284D
		public static UniTask<int?> MinAsync(this IUniTaskAsyncEnumerable<int?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<int?>>(source, "source");
			return Min.MinAsync(source, cancellationToken);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004661 File Offset: 0x00002861
		public static UniTask<int?> MinAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004681 File Offset: 0x00002881
		public static UniTask<int?> MinAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<int?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000046A1 File Offset: 0x000028A1
		public static UniTask<int?> MinAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<int?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000046C1 File Offset: 0x000028C1
		public static UniTask<long?> MinAsync(this IUniTaskAsyncEnumerable<long?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<long?>>(source, "source");
			return Min.MinAsync(source, cancellationToken);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000046D5 File Offset: 0x000028D5
		public static UniTask<long?> MinAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000046F5 File Offset: 0x000028F5
		public static UniTask<long?> MinAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<long?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004715 File Offset: 0x00002915
		public static UniTask<long?> MinAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<long?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004735 File Offset: 0x00002935
		public static UniTask<float?> MinAsync(this IUniTaskAsyncEnumerable<float?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<float?>>(source, "source");
			return Min.MinAsync(source, cancellationToken);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004749 File Offset: 0x00002949
		public static UniTask<float?> MinAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004769 File Offset: 0x00002969
		public static UniTask<float?> MinAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<float?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004789 File Offset: 0x00002989
		public static UniTask<float?> MinAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<float?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000047A9 File Offset: 0x000029A9
		public static UniTask<double?> MinAsync(this IUniTaskAsyncEnumerable<double?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<double?>>(source, "source");
			return Min.MinAsync(source, cancellationToken);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000047BD File Offset: 0x000029BD
		public static UniTask<double?> MinAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000047DD File Offset: 0x000029DD
		public static UniTask<double?> MinAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<double?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000047FD File Offset: 0x000029FD
		public static UniTask<double?> MinAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<double?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000481D File Offset: 0x00002A1D
		public static UniTask<decimal?> MinAsync(this IUniTaskAsyncEnumerable<decimal?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<decimal?>>(source, "source");
			return Min.MinAsync(source, cancellationToken);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004831 File Offset: 0x00002A31
		public static UniTask<decimal?> MinAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004851 File Offset: 0x00002A51
		public static UniTask<decimal?> MinAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004871 File Offset: 0x00002A71
		public static UniTask<decimal?> MinAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Min.MinAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004891 File Offset: 0x00002A91
		public static UniTask<int> MaxAsync(this IUniTaskAsyncEnumerable<int> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<int>>(source, "source");
			return Max.MaxAsync(source, cancellationToken);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000048A5 File Offset: 0x00002AA5
		public static UniTask<int> MaxAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000048C5 File Offset: 0x00002AC5
		public static UniTask<int> MaxAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<int>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000048E5 File Offset: 0x00002AE5
		public static UniTask<int> MaxAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<int>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004905 File Offset: 0x00002B05
		public static UniTask<long> MaxAsync(this IUniTaskAsyncEnumerable<long> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<long>>(source, "source");
			return Max.MaxAsync(source, cancellationToken);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004919 File Offset: 0x00002B19
		public static UniTask<long> MaxAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004939 File Offset: 0x00002B39
		public static UniTask<long> MaxAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<long>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004959 File Offset: 0x00002B59
		public static UniTask<long> MaxAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<long>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004979 File Offset: 0x00002B79
		public static UniTask<float> MaxAsync(this IUniTaskAsyncEnumerable<float> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<float>>(source, "source");
			return Max.MaxAsync(source, cancellationToken);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000498D File Offset: 0x00002B8D
		public static UniTask<float> MaxAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000049AD File Offset: 0x00002BAD
		public static UniTask<float> MaxAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<float>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000049CD File Offset: 0x00002BCD
		public static UniTask<float> MaxAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<float>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000049ED File Offset: 0x00002BED
		public static UniTask<double> MaxAsync(this IUniTaskAsyncEnumerable<double> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<double>>(source, "source");
			return Max.MaxAsync(source, cancellationToken);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004A01 File Offset: 0x00002C01
		public static UniTask<double> MaxAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004A21 File Offset: 0x00002C21
		public static UniTask<double> MaxAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<double>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00004A41 File Offset: 0x00002C41
		public static UniTask<double> MaxAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<double>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00004A61 File Offset: 0x00002C61
		public static UniTask<decimal> MaxAsync(this IUniTaskAsyncEnumerable<decimal> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<decimal>>(source, "source");
			return Max.MaxAsync(source, cancellationToken);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004A75 File Offset: 0x00002C75
		public static UniTask<decimal> MaxAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00004A95 File Offset: 0x00002C95
		public static UniTask<decimal> MaxAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<decimal>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00004AB5 File Offset: 0x00002CB5
		public static UniTask<decimal> MaxAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<decimal>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004AD5 File Offset: 0x00002CD5
		public static UniTask<int?> MaxAsync(this IUniTaskAsyncEnumerable<int?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<int?>>(source, "source");
			return Max.MaxAsync(source, cancellationToken);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004AE9 File Offset: 0x00002CE9
		public static UniTask<int?> MaxAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004B09 File Offset: 0x00002D09
		public static UniTask<int?> MaxAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<int?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00004B29 File Offset: 0x00002D29
		public static UniTask<int?> MaxAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<int?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004B49 File Offset: 0x00002D49
		public static UniTask<long?> MaxAsync(this IUniTaskAsyncEnumerable<long?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<long?>>(source, "source");
			return Max.MaxAsync(source, cancellationToken);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00004B5D File Offset: 0x00002D5D
		public static UniTask<long?> MaxAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00004B7D File Offset: 0x00002D7D
		public static UniTask<long?> MaxAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<long?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00004B9D File Offset: 0x00002D9D
		public static UniTask<long?> MaxAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<long?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004BBD File Offset: 0x00002DBD
		public static UniTask<float?> MaxAsync(this IUniTaskAsyncEnumerable<float?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<float?>>(source, "source");
			return Max.MaxAsync(source, cancellationToken);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00004BD1 File Offset: 0x00002DD1
		public static UniTask<float?> MaxAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00004BF1 File Offset: 0x00002DF1
		public static UniTask<float?> MaxAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<float?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00004C11 File Offset: 0x00002E11
		public static UniTask<float?> MaxAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<float?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00004C31 File Offset: 0x00002E31
		public static UniTask<double?> MaxAsync(this IUniTaskAsyncEnumerable<double?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<double?>>(source, "source");
			return Max.MaxAsync(source, cancellationToken);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00004C45 File Offset: 0x00002E45
		public static UniTask<double?> MaxAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00004C65 File Offset: 0x00002E65
		public static UniTask<double?> MaxAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<double?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00004C85 File Offset: 0x00002E85
		public static UniTask<double?> MaxAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<double?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00004CA5 File Offset: 0x00002EA5
		public static UniTask<decimal?> MaxAsync(this IUniTaskAsyncEnumerable<decimal?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<decimal?>>(source, "source");
			return Max.MaxAsync(source, cancellationToken);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00004CB9 File Offset: 0x00002EB9
		public static UniTask<decimal?> MaxAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00004CD9 File Offset: 0x00002ED9
		public static UniTask<decimal?> MaxAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00004CF9 File Offset: 0x00002EF9
		public static UniTask<decimal?> MaxAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Max.MaxAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00004D19 File Offset: 0x00002F19
		public static IUniTaskAsyncEnumerable<T> Never<T>()
		{
			return Never<T>.Instance;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00004D20 File Offset: 0x00002F20
		public static IUniTaskAsyncEnumerable<TResult> OfType<TResult>(this IUniTaskAsyncEnumerable<object> source)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<object>>(source, "source");
			return new OfType<TResult>(source);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00004D33 File Offset: 0x00002F33
		public static IUniTaskOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			return new OrderedAsyncEnumerable<TSource, TKey>(source, keySelector, Comparer<TKey>.Default, false, null);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00004D59 File Offset: 0x00002F59
		public static IUniTaskOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IComparer<TKey>>(comparer, "comparer");
			return new OrderedAsyncEnumerable<TSource, TKey>(source, keySelector, comparer, false, null);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00004D86 File Offset: 0x00002F86
		public static IUniTaskOrderedAsyncEnumerable<TSource> OrderByAwait<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			return new OrderedAsyncEnumerableAwait<TSource, TKey>(source, keySelector, Comparer<TKey>.Default, false, null);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00004DAC File Offset: 0x00002FAC
		public static IUniTaskOrderedAsyncEnumerable<TSource> OrderByAwait<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, IComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IComparer<TKey>>(comparer, "comparer");
			return new OrderedAsyncEnumerableAwait<TSource, TKey>(source, keySelector, comparer, false, null);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00004DD9 File Offset: 0x00002FD9
		public static IUniTaskOrderedAsyncEnumerable<TSource> OrderByAwaitWithCancellation<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			return new OrderedAsyncEnumerableAwaitWithCancellation<TSource, TKey>(source, keySelector, Comparer<TKey>.Default, false, null);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00004DFF File Offset: 0x00002FFF
		public static IUniTaskOrderedAsyncEnumerable<TSource> OrderByAwaitWithCancellation<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, IComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IComparer<TKey>>(comparer, "comparer");
			return new OrderedAsyncEnumerableAwaitWithCancellation<TSource, TKey>(source, keySelector, comparer, false, null);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00004E2C File Offset: 0x0000302C
		public static IUniTaskOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			return new OrderedAsyncEnumerable<TSource, TKey>(source, keySelector, Comparer<TKey>.Default, true, null);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00004E52 File Offset: 0x00003052
		public static IUniTaskOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IComparer<TKey>>(comparer, "comparer");
			return new OrderedAsyncEnumerable<TSource, TKey>(source, keySelector, comparer, true, null);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00004E7F File Offset: 0x0000307F
		public static IUniTaskOrderedAsyncEnumerable<TSource> OrderByDescendingAwait<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			return new OrderedAsyncEnumerableAwait<TSource, TKey>(source, keySelector, Comparer<TKey>.Default, true, null);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00004EA5 File Offset: 0x000030A5
		public static IUniTaskOrderedAsyncEnumerable<TSource> OrderByDescendingAwait<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, IComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IComparer<TKey>>(comparer, "comparer");
			return new OrderedAsyncEnumerableAwait<TSource, TKey>(source, keySelector, comparer, true, null);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00004ED2 File Offset: 0x000030D2
		public static IUniTaskOrderedAsyncEnumerable<TSource> OrderByDescendingAwaitWithCancellation<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			return new OrderedAsyncEnumerableAwaitWithCancellation<TSource, TKey>(source, keySelector, Comparer<TKey>.Default, true, null);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00004EF8 File Offset: 0x000030F8
		public static IUniTaskOrderedAsyncEnumerable<TSource> OrderByDescendingAwaitWithCancellation<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, IComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IComparer<TKey>>(comparer, "comparer");
			return new OrderedAsyncEnumerableAwaitWithCancellation<TSource, TKey>(source, keySelector, comparer, true, null);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00004F25 File Offset: 0x00003125
		public static IUniTaskOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IUniTaskOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			Error.ThrowArgumentNullException<IUniTaskOrderedAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			return source.CreateOrderedEnumerable<TKey>(keySelector, Comparer<TKey>.Default, false);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00004F4A File Offset: 0x0000314A
		public static IUniTaskOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IUniTaskOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskOrderedAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IComparer<TKey>>(comparer, "comparer");
			return source.CreateOrderedEnumerable<TKey>(keySelector, comparer, false);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00004F76 File Offset: 0x00003176
		public static IUniTaskOrderedAsyncEnumerable<TSource> ThenByAwait<TSource, TKey>(this IUniTaskOrderedAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector)
		{
			Error.ThrowArgumentNullException<IUniTaskOrderedAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			return source.CreateOrderedEnumerable<TKey>(keySelector, Comparer<TKey>.Default, false);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00004F9B File Offset: 0x0000319B
		public static IUniTaskOrderedAsyncEnumerable<TSource> ThenByAwait<TSource, TKey>(this IUniTaskOrderedAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, IComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskOrderedAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IComparer<TKey>>(comparer, "comparer");
			return source.CreateOrderedEnumerable<TKey>(keySelector, comparer, false);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00004FC7 File Offset: 0x000031C7
		public static IUniTaskOrderedAsyncEnumerable<TSource> ThenByAwaitWithCancellation<TSource, TKey>(this IUniTaskOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector)
		{
			Error.ThrowArgumentNullException<IUniTaskOrderedAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			return source.CreateOrderedEnumerable<TKey>(keySelector, Comparer<TKey>.Default, false);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00004FEC File Offset: 0x000031EC
		public static IUniTaskOrderedAsyncEnumerable<TSource> ThenByAwaitWithCancellation<TSource, TKey>(this IUniTaskOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, IComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskOrderedAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IComparer<TKey>>(comparer, "comparer");
			return source.CreateOrderedEnumerable<TKey>(keySelector, comparer, false);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005018 File Offset: 0x00003218
		public static IUniTaskOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IUniTaskOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			Error.ThrowArgumentNullException<IUniTaskOrderedAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			return source.CreateOrderedEnumerable<TKey>(keySelector, Comparer<TKey>.Default, true);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000503D File Offset: 0x0000323D
		public static IUniTaskOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IUniTaskOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskOrderedAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IComparer<TKey>>(comparer, "comparer");
			return source.CreateOrderedEnumerable<TKey>(keySelector, comparer, true);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00005069 File Offset: 0x00003269
		public static IUniTaskOrderedAsyncEnumerable<TSource> ThenByDescendingAwait<TSource, TKey>(this IUniTaskOrderedAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector)
		{
			Error.ThrowArgumentNullException<IUniTaskOrderedAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			return source.CreateOrderedEnumerable<TKey>(keySelector, Comparer<TKey>.Default, true);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000508E File Offset: 0x0000328E
		public static IUniTaskOrderedAsyncEnumerable<TSource> ThenByDescendingAwait<TSource, TKey>(this IUniTaskOrderedAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, IComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskOrderedAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IComparer<TKey>>(comparer, "comparer");
			return source.CreateOrderedEnumerable<TKey>(keySelector, comparer, true);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000050BA File Offset: 0x000032BA
		public static IUniTaskOrderedAsyncEnumerable<TSource> ThenByDescendingAwaitWithCancellation<TSource, TKey>(this IUniTaskOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector)
		{
			Error.ThrowArgumentNullException<IUniTaskOrderedAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			return source.CreateOrderedEnumerable<TKey>(keySelector, Comparer<TKey>.Default, true);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000050DF File Offset: 0x000032DF
		public static IUniTaskOrderedAsyncEnumerable<TSource> ThenByDescendingAwaitWithCancellation<TSource, TKey>(this IUniTaskOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, IComparer<TKey> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskOrderedAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IComparer<TKey>>(comparer, "comparer");
			return source.CreateOrderedEnumerable<TKey>(keySelector, comparer, true);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000510B File Offset: 0x0000330B
		public static IUniTaskAsyncEnumerable<ValueTuple<TSource, TSource>> Pairwise<TSource>(this IUniTaskAsyncEnumerable<TSource> source)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return new Pairwise<TSource>(source);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000511E File Offset: 0x0000331E
		public static IConnectableUniTaskAsyncEnumerable<TSource> Publish<TSource>(this IUniTaskAsyncEnumerable<TSource> source)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return new Publish<TSource>(source);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005131 File Offset: 0x00003331
		public static IUniTaskAsyncEnumerable<TSource> Queue<TSource>(this IUniTaskAsyncEnumerable<TSource> source)
		{
			return new QueueOperator<TSource>(source);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005139 File Offset: 0x00003339
		public static IUniTaskAsyncEnumerable<int> Range(int start, int count)
		{
			if (count < 0)
			{
				throw Error.ArgumentOutOfRange("count");
			}
			if ((long)start + (long)count - 1L > 2147483647L)
			{
				throw Error.ArgumentOutOfRange("count");
			}
			if (count == 0)
			{
				UniTaskAsyncEnumerable.Empty<int>();
			}
			return new Range(start, count);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00005175 File Offset: 0x00003375
		public static IUniTaskAsyncEnumerable<TElement> Repeat<TElement>(TElement element, int count)
		{
			if (count < 0)
			{
				throw Error.ArgumentOutOfRange("count");
			}
			return new Repeat<TElement>(element, count);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000518D File Offset: 0x0000338D
		public static IUniTaskAsyncEnumerable<TValue> Return<TValue>(TValue value)
		{
			return new Return<TValue>(value);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005195 File Offset: 0x00003395
		public static IUniTaskAsyncEnumerable<TSource> Reverse<TSource>(this IUniTaskAsyncEnumerable<TSource> source)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return new Reverse<TSource>(source);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000051A8 File Offset: 0x000033A8
		public static IUniTaskAsyncEnumerable<TResult> Select<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TResult>>(selector, "selector");
			return new Select<TSource, TResult>(source, selector);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000051C7 File Offset: 0x000033C7
		public static IUniTaskAsyncEnumerable<TResult> Select<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, TResult> selector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, TResult>>(selector, "selector");
			return new SelectInt<TSource, TResult>(source, selector);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000051E6 File Offset: 0x000033E6
		public static IUniTaskAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TResult>> selector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TResult>>>(selector, "selector");
			return new SelectAwait<TSource, TResult>(source, selector);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005205 File Offset: 0x00003405
		public static IUniTaskAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<TResult>> selector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, UniTask<TResult>>>(selector, "selector");
			return new SelectIntAwait<TSource, TResult>(source, selector);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00005224 File Offset: 0x00003424
		public static IUniTaskAsyncEnumerable<TResult> SelectAwaitWithCancellation<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TResult>> selector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TResult>>>(selector, "selector");
			return new SelectAwaitWithCancellation<TSource, TResult>(source, selector);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005243 File Offset: 0x00003443
		public static IUniTaskAsyncEnumerable<TResult> SelectAwaitWithCancellation<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, UniTask<TResult>> selector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, CancellationToken, UniTask<TResult>>>(selector, "selector");
			return new SelectIntAwaitWithCancellation<TSource, TResult>(source, selector);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005262 File Offset: 0x00003462
		public static IUniTaskAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, IUniTaskAsyncEnumerable<TResult>> selector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, IUniTaskAsyncEnumerable<TResult>>>(selector, "selector");
			return new SelectMany<TSource, TResult, TResult>(source, selector, (TSource x, TResult y) => y);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000052A0 File Offset: 0x000034A0
		public static IUniTaskAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, IUniTaskAsyncEnumerable<TResult>> selector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, IUniTaskAsyncEnumerable<TResult>>>(selector, "selector");
			return new SelectMany<TSource, TResult, TResult>(source, selector, (TSource x, TResult y) => y);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000052DE File Offset: 0x000034DE
		public static IUniTaskAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, IUniTaskAsyncEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, IUniTaskAsyncEnumerable<TCollection>>>(collectionSelector, "collectionSelector");
			return new SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000052FE File Offset: 0x000034FE
		public static IUniTaskAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, IUniTaskAsyncEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, IUniTaskAsyncEnumerable<TCollection>>>(collectionSelector, "collectionSelector");
			return new SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000531E File Offset: 0x0000351E
		public static IUniTaskAsyncEnumerable<TResult> SelectManyAwait<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<IUniTaskAsyncEnumerable<TResult>>> selector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<IUniTaskAsyncEnumerable<TResult>>>>(selector, "selector");
			return new SelectManyAwait<TSource, TResult, TResult>(source, selector, (TSource x, TResult y) => UniTask.FromResult<TResult>(y));
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000535C File Offset: 0x0000355C
		public static IUniTaskAsyncEnumerable<TResult> SelectManyAwait<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<IUniTaskAsyncEnumerable<TResult>>> selector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, UniTask<IUniTaskAsyncEnumerable<TResult>>>>(selector, "selector");
			return new SelectManyAwait<TSource, TResult, TResult>(source, selector, (TSource x, TResult y) => UniTask.FromResult<TResult>(y));
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000539A File Offset: 0x0000359A
		public static IUniTaskAsyncEnumerable<TResult> SelectManyAwait<TSource, TCollection, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<IUniTaskAsyncEnumerable<TCollection>>> collectionSelector, Func<TSource, TCollection, UniTask<TResult>> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<IUniTaskAsyncEnumerable<TCollection>>>>(collectionSelector, "collectionSelector");
			return new SelectManyAwait<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000053BA File Offset: 0x000035BA
		public static IUniTaskAsyncEnumerable<TResult> SelectManyAwait<TSource, TCollection, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<IUniTaskAsyncEnumerable<TCollection>>> collectionSelector, Func<TSource, TCollection, UniTask<TResult>> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, UniTask<IUniTaskAsyncEnumerable<TCollection>>>>(collectionSelector, "collectionSelector");
			return new SelectManyAwait<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000053DA File Offset: 0x000035DA
		public static IUniTaskAsyncEnumerable<TResult> SelectManyAwaitWithCancellation<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<IUniTaskAsyncEnumerable<TResult>>> selector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<IUniTaskAsyncEnumerable<TResult>>>>(selector, "selector");
			return new SelectManyAwaitWithCancellation<TSource, TResult, TResult>(source, selector, (TSource x, TResult y, CancellationToken c) => UniTask.FromResult<TResult>(y));
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00005418 File Offset: 0x00003618
		public static IUniTaskAsyncEnumerable<TResult> SelectManyAwaitWithCancellation<TSource, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, UniTask<IUniTaskAsyncEnumerable<TResult>>> selector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, CancellationToken, UniTask<IUniTaskAsyncEnumerable<TResult>>>>(selector, "selector");
			return new SelectManyAwaitWithCancellation<TSource, TResult, TResult>(source, selector, (TSource x, TResult y, CancellationToken c) => UniTask.FromResult<TResult>(y));
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00005456 File Offset: 0x00003656
		public static IUniTaskAsyncEnumerable<TResult> SelectManyAwaitWithCancellation<TSource, TCollection, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<IUniTaskAsyncEnumerable<TCollection>>> collectionSelector, Func<TSource, TCollection, CancellationToken, UniTask<TResult>> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<IUniTaskAsyncEnumerable<TCollection>>>>(collectionSelector, "collectionSelector");
			return new SelectManyAwaitWithCancellation<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00005476 File Offset: 0x00003676
		public static IUniTaskAsyncEnumerable<TResult> SelectManyAwaitWithCancellation<TSource, TCollection, TResult>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, UniTask<IUniTaskAsyncEnumerable<TCollection>>> collectionSelector, Func<TSource, TCollection, CancellationToken, UniTask<TResult>> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, CancellationToken, UniTask<IUniTaskAsyncEnumerable<TCollection>>>>(collectionSelector, "collectionSelector");
			return new SelectManyAwaitWithCancellation<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00005496 File Offset: 0x00003696
		public static UniTask<bool> SequenceEqualAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> first, IUniTaskAsyncEnumerable<TSource> second, CancellationToken cancellationToken = default(CancellationToken))
		{
			return first.SequenceEqualAsync(second, EqualityComparer<TSource>.Default, cancellationToken);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000054A5 File Offset: 0x000036A5
		public static UniTask<bool> SequenceEqualAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> first, IUniTaskAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(first, "first");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(second, "second");
			Error.ThrowArgumentNullException<IEqualityComparer<TSource>>(comparer, "comparer");
			return SequenceEqual.SequenceEqualAsync<TSource>(first, second, comparer, cancellationToken);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000054D1 File Offset: 0x000036D1
		public static UniTask<TSource> SingleAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return SingleOperator.SingleAsync<TSource>(source, cancellationToken, false);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000054E6 File Offset: 0x000036E6
		public static UniTask<TSource> SingleAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, bool>>(predicate, "predicate");
			return SingleOperator.SingleAsync<TSource>(source, predicate, cancellationToken, false);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005507 File Offset: 0x00003707
		public static UniTask<TSource> SingleAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<bool>>>(predicate, "predicate");
			return SingleOperator.SingleAwaitAsync<TSource>(source, predicate, cancellationToken, false);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005528 File Offset: 0x00003728
		public static UniTask<TSource> SingleAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<bool>>>(predicate, "predicate");
			return SingleOperator.SingleAwaitWithCancellationAsync<TSource>(source, predicate, cancellationToken, false);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005549 File Offset: 0x00003749
		public static UniTask<TSource> SingleOrDefaultAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return SingleOperator.SingleAsync<TSource>(source, cancellationToken, true);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000555E File Offset: 0x0000375E
		public static UniTask<TSource> SingleOrDefaultAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, bool>>(predicate, "predicate");
			return SingleOperator.SingleAsync<TSource>(source, predicate, cancellationToken, true);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000557F File Offset: 0x0000377F
		public static UniTask<TSource> SingleOrDefaultAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<bool>>>(predicate, "predicate");
			return SingleOperator.SingleAwaitAsync<TSource>(source, predicate, cancellationToken, true);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000055A0 File Offset: 0x000037A0
		public static UniTask<TSource> SingleOrDefaultAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<bool>>>(predicate, "predicate");
			return SingleOperator.SingleAwaitWithCancellationAsync<TSource>(source, predicate, cancellationToken, true);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000055C1 File Offset: 0x000037C1
		public static IUniTaskAsyncEnumerable<TSource> Skip<TSource>(this IUniTaskAsyncEnumerable<TSource> source, int count)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return new Skip<TSource>(source, count);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000055D5 File Offset: 0x000037D5
		public static IUniTaskAsyncEnumerable<TSource> SkipLast<TSource>(this IUniTaskAsyncEnumerable<TSource> source, int count)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			if (count <= 0)
			{
				return source;
			}
			return new SkipLast<TSource>(source, count);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000055EF File Offset: 0x000037EF
		public static IUniTaskAsyncEnumerable<TSource> SkipUntil<TSource>(this IUniTaskAsyncEnumerable<TSource> source, UniTask other)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return new SkipUntil<TSource>(source, other, null);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00005604 File Offset: 0x00003804
		public static IUniTaskAsyncEnumerable<TSource> SkipUntil<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<CancellationToken, UniTask> other)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "other");
			return new SkipUntil<TSource>(source, default(UniTask), other);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00005637 File Offset: 0x00003837
		public static IUniTaskAsyncEnumerable<TSource> SkipUntilCanceled<TSource>(this IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return new SkipUntilCanceled<TSource>(source, cancellationToken);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000564B File Offset: 0x0000384B
		public static IUniTaskAsyncEnumerable<TSource> SkipWhile<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, bool>>(predicate, "predicate");
			return new SkipWhile<TSource>(source, predicate);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000566A File Offset: 0x0000386A
		public static IUniTaskAsyncEnumerable<TSource> SkipWhile<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, bool>>(predicate, "predicate");
			return new SkipWhileInt<TSource>(source, predicate);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00005689 File Offset: 0x00003889
		public static IUniTaskAsyncEnumerable<TSource> SkipWhileAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<bool>>>(predicate, "predicate");
			return new SkipWhileAwait<TSource>(source, predicate);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000056A8 File Offset: 0x000038A8
		public static IUniTaskAsyncEnumerable<TSource> SkipWhileAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<bool>> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, UniTask<bool>>>(predicate, "predicate");
			return new SkipWhileIntAwait<TSource>(source, predicate);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000056C7 File Offset: 0x000038C7
		public static IUniTaskAsyncEnumerable<TSource> SkipWhileAwaitWithCancellation<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<bool>>>(predicate, "predicate");
			return new SkipWhileAwaitWithCancellation<TSource>(source, predicate);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000056E6 File Offset: 0x000038E6
		public static IUniTaskAsyncEnumerable<TSource> SkipWhileAwaitWithCancellation<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, UniTask<bool>> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, CancellationToken, UniTask<bool>>>(predicate, "predicate");
			return new SkipWhileIntAwaitWithCancellation<TSource>(source, predicate);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005708 File Offset: 0x00003908
		public static IDisposable Subscribe<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Action<TSource> action)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Action<TSource>>(action, "action");
			CancellationTokenDisposable cancellationTokenDisposable = new CancellationTokenDisposable();
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeCore<TSource>(source, action, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationTokenDisposable.Token).Forget();
			return cancellationTokenDisposable;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005754 File Offset: 0x00003954
		public static IDisposable Subscribe<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTaskVoid> action)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTaskVoid>>(action, "action");
			CancellationTokenDisposable cancellationTokenDisposable = new CancellationTokenDisposable();
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeCore<TSource>(source, action, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationTokenDisposable.Token).Forget();
			return cancellationTokenDisposable;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000057A0 File Offset: 0x000039A0
		public static IDisposable Subscribe<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTaskVoid> action)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTaskVoid>>(action, "action");
			CancellationTokenDisposable cancellationTokenDisposable = new CancellationTokenDisposable();
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeCore<TSource>(source, action, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationTokenDisposable.Token).Forget();
			return cancellationTokenDisposable;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000057EC File Offset: 0x000039EC
		public static void Subscribe<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Action<TSource> action, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Action<TSource>>(action, "action");
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeCore<TSource>(source, action, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationToken).Forget();
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000582C File Offset: 0x00003A2C
		public static void Subscribe<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTaskVoid> action, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTaskVoid>>(action, "action");
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeCore<TSource>(source, action, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationToken).Forget();
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000586C File Offset: 0x00003A6C
		public static void Subscribe<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTaskVoid> action, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTaskVoid>>(action, "action");
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeCore<TSource>(source, action, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationToken).Forget();
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000058AC File Offset: 0x00003AAC
		public static IDisposable SubscribeAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask> onNext)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask>>(onNext, "onNext");
			CancellationTokenDisposable cancellationTokenDisposable = new CancellationTokenDisposable();
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeAwaitCore<TSource>(source, onNext, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationTokenDisposable.Token).Forget();
			return cancellationTokenDisposable;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000058F8 File Offset: 0x00003AF8
		public static void SubscribeAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask> onNext, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask>>(onNext, "onNext");
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeAwaitCore<TSource>(source, onNext, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationToken).Forget();
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00005938 File Offset: 0x00003B38
		public static IDisposable SubscribeAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask> onNext)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask>>(onNext, "onNext");
			CancellationTokenDisposable cancellationTokenDisposable = new CancellationTokenDisposable();
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeAwaitCore<TSource>(source, onNext, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationTokenDisposable.Token).Forget();
			return cancellationTokenDisposable;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00005984 File Offset: 0x00003B84
		public static void SubscribeAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask> onNext, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask>>(onNext, "onNext");
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeAwaitCore<TSource>(source, onNext, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationToken).Forget();
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000059C4 File Offset: 0x00003BC4
		public static IDisposable Subscribe<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Action<TSource>>(onNext, "onNext");
			Error.ThrowArgumentNullException<Action<Exception>>(onError, "onError");
			CancellationTokenDisposable cancellationTokenDisposable = new CancellationTokenDisposable();
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeCore<TSource>(source, onNext, onError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationTokenDisposable.Token).Forget();
			return cancellationTokenDisposable;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00005A14 File Offset: 0x00003C14
		public static IDisposable Subscribe<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTaskVoid> onNext, Action<Exception> onError)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTaskVoid>>(onNext, "onNext");
			Error.ThrowArgumentNullException<Action<Exception>>(onError, "onError");
			CancellationTokenDisposable cancellationTokenDisposable = new CancellationTokenDisposable();
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeCore<TSource>(source, onNext, onError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationTokenDisposable.Token).Forget();
			return cancellationTokenDisposable;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00005A64 File Offset: 0x00003C64
		public static void Subscribe<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Action<TSource>>(onNext, "onNext");
			Error.ThrowArgumentNullException<Action<Exception>>(onError, "onError");
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeCore<TSource>(source, onNext, onError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationToken).Forget();
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005AA8 File Offset: 0x00003CA8
		public static void Subscribe<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTaskVoid> onNext, Action<Exception> onError, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTaskVoid>>(onNext, "onNext");
			Error.ThrowArgumentNullException<Action<Exception>>(onError, "onError");
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeCore<TSource>(source, onNext, onError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationToken).Forget();
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00005AEC File Offset: 0x00003CEC
		public static IDisposable SubscribeAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask> onNext, Action<Exception> onError)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask>>(onNext, "onNext");
			Error.ThrowArgumentNullException<Action<Exception>>(onError, "onError");
			CancellationTokenDisposable cancellationTokenDisposable = new CancellationTokenDisposable();
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeAwaitCore<TSource>(source, onNext, onError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationTokenDisposable.Token).Forget();
			return cancellationTokenDisposable;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00005B3C File Offset: 0x00003D3C
		public static void SubscribeAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask> onNext, Action<Exception> onError, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask>>(onNext, "onNext");
			Error.ThrowArgumentNullException<Action<Exception>>(onError, "onError");
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeAwaitCore<TSource>(source, onNext, onError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationToken).Forget();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00005B80 File Offset: 0x00003D80
		public static IDisposable SubscribeAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask> onNext, Action<Exception> onError)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask>>(onNext, "onNext");
			Error.ThrowArgumentNullException<Action<Exception>>(onError, "onError");
			CancellationTokenDisposable cancellationTokenDisposable = new CancellationTokenDisposable();
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeAwaitCore<TSource>(source, onNext, onError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationTokenDisposable.Token).Forget();
			return cancellationTokenDisposable;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00005BD0 File Offset: 0x00003DD0
		public static void SubscribeAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask> onNext, Action<Exception> onError, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask>>(onNext, "onNext");
			Error.ThrowArgumentNullException<Action<Exception>>(onError, "onError");
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeAwaitCore<TSource>(source, onNext, onError, Cysharp.Threading.Tasks.Linq.Subscribe.NopCompleted, cancellationToken).Forget();
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00005C14 File Offset: 0x00003E14
		public static IDisposable Subscribe<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Action<TSource> onNext, Action onCompleted)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Action<TSource>>(onNext, "onNext");
			Error.ThrowArgumentNullException<Action>(onCompleted, "onCompleted");
			CancellationTokenDisposable cancellationTokenDisposable = new CancellationTokenDisposable();
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeCore<TSource>(source, onNext, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, onCompleted, cancellationTokenDisposable.Token).Forget();
			return cancellationTokenDisposable;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00005C64 File Offset: 0x00003E64
		public static IDisposable Subscribe<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTaskVoid> onNext, Action onCompleted)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTaskVoid>>(onNext, "onNext");
			Error.ThrowArgumentNullException<Action>(onCompleted, "onCompleted");
			CancellationTokenDisposable cancellationTokenDisposable = new CancellationTokenDisposable();
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeCore<TSource>(source, onNext, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, onCompleted, cancellationTokenDisposable.Token).Forget();
			return cancellationTokenDisposable;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00005CB4 File Offset: 0x00003EB4
		public static void Subscribe<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Action<TSource> onNext, Action onCompleted, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Action<TSource>>(onNext, "onNext");
			Error.ThrowArgumentNullException<Action>(onCompleted, "onCompleted");
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeCore<TSource>(source, onNext, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, onCompleted, cancellationToken).Forget();
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00005CF8 File Offset: 0x00003EF8
		public static void Subscribe<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTaskVoid> onNext, Action onCompleted, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTaskVoid>>(onNext, "onNext");
			Error.ThrowArgumentNullException<Action>(onCompleted, "onCompleted");
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeCore<TSource>(source, onNext, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, onCompleted, cancellationToken).Forget();
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00005D3C File Offset: 0x00003F3C
		public static IDisposable SubscribeAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask> onNext, Action onCompleted)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask>>(onNext, "onNext");
			Error.ThrowArgumentNullException<Action>(onCompleted, "onCompleted");
			CancellationTokenDisposable cancellationTokenDisposable = new CancellationTokenDisposable();
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeAwaitCore<TSource>(source, onNext, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, onCompleted, cancellationTokenDisposable.Token).Forget();
			return cancellationTokenDisposable;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00005D8C File Offset: 0x00003F8C
		public static void SubscribeAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask> onNext, Action onCompleted, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask>>(onNext, "onNext");
			Error.ThrowArgumentNullException<Action>(onCompleted, "onCompleted");
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeAwaitCore<TSource>(source, onNext, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, onCompleted, cancellationToken).Forget();
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00005DD0 File Offset: 0x00003FD0
		public static IDisposable SubscribeAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask> onNext, Action onCompleted)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask>>(onNext, "onNext");
			Error.ThrowArgumentNullException<Action>(onCompleted, "onCompleted");
			CancellationTokenDisposable cancellationTokenDisposable = new CancellationTokenDisposable();
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeAwaitCore<TSource>(source, onNext, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, onCompleted, cancellationTokenDisposable.Token).Forget();
			return cancellationTokenDisposable;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00005E20 File Offset: 0x00004020
		public static void SubscribeAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask> onNext, Action onCompleted, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask>>(onNext, "onNext");
			Error.ThrowArgumentNullException<Action>(onCompleted, "onCompleted");
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeAwaitCore<TSource>(source, onNext, Cysharp.Threading.Tasks.Linq.Subscribe.NopError, onCompleted, cancellationToken).Forget();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00005E64 File Offset: 0x00004064
		public static IDisposable Subscribe<TSource>(this IUniTaskAsyncEnumerable<TSource> source, IObserver<TSource> observer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IObserver<TSource>>(observer, "observer");
			CancellationTokenDisposable cancellationTokenDisposable = new CancellationTokenDisposable();
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeCore<TSource>(source, observer, cancellationTokenDisposable.Token).Forget();
			return cancellationTokenDisposable;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00005EA4 File Offset: 0x000040A4
		public static void Subscribe<TSource>(this IUniTaskAsyncEnumerable<TSource> source, IObserver<TSource> observer, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IObserver<TSource>>(observer, "observer");
			Cysharp.Threading.Tasks.Linq.Subscribe.SubscribeCore<TSource>(source, observer, cancellationToken).Forget();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00005ED7 File Offset: 0x000040D7
		public static UniTask<int> SumAsync(this IUniTaskAsyncEnumerable<int> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<int>>(source, "source");
			return Sum.SumAsync(source, cancellationToken);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00005EEB File Offset: 0x000040EB
		public static UniTask<int> SumAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00005F0B File Offset: 0x0000410B
		public static UniTask<int> SumAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<int>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00005F2B File Offset: 0x0000412B
		public static UniTask<int> SumAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<int>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00005F4B File Offset: 0x0000414B
		public static UniTask<long> SumAsync(this IUniTaskAsyncEnumerable<long> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<long>>(source, "source");
			return Sum.SumAsync(source, cancellationToken);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00005F5F File Offset: 0x0000415F
		public static UniTask<long> SumAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00005F7F File Offset: 0x0000417F
		public static UniTask<long> SumAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<long>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00005F9F File Offset: 0x0000419F
		public static UniTask<long> SumAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<long>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00005FBF File Offset: 0x000041BF
		public static UniTask<float> SumAsync(this IUniTaskAsyncEnumerable<float> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<float>>(source, "source");
			return Sum.SumAsync(source, cancellationToken);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00005FD3 File Offset: 0x000041D3
		public static UniTask<float> SumAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00005FF3 File Offset: 0x000041F3
		public static UniTask<float> SumAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<float>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00006013 File Offset: 0x00004213
		public static UniTask<float> SumAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<float>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00006033 File Offset: 0x00004233
		public static UniTask<double> SumAsync(this IUniTaskAsyncEnumerable<double> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<double>>(source, "source");
			return Sum.SumAsync(source, cancellationToken);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006047 File Offset: 0x00004247
		public static UniTask<double> SumAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006067 File Offset: 0x00004267
		public static UniTask<double> SumAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<double>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00006087 File Offset: 0x00004287
		public static UniTask<double> SumAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<double>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000060A7 File Offset: 0x000042A7
		public static UniTask<decimal> SumAsync(this IUniTaskAsyncEnumerable<decimal> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<decimal>>(source, "source");
			return Sum.SumAsync(source, cancellationToken);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000060BB File Offset: 0x000042BB
		public static UniTask<decimal> SumAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000060DB File Offset: 0x000042DB
		public static UniTask<decimal> SumAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<decimal>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000060FB File Offset: 0x000042FB
		public static UniTask<decimal> SumAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<decimal>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000611B File Offset: 0x0000431B
		public static UniTask<int?> SumAsync(this IUniTaskAsyncEnumerable<int?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<int?>>(source, "source");
			return Sum.SumAsync(source, cancellationToken);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000612F File Offset: 0x0000432F
		public static UniTask<int?> SumAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000614F File Offset: 0x0000434F
		public static UniTask<int?> SumAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<int?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000616F File Offset: 0x0000436F
		public static UniTask<int?> SumAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<int?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000618F File Offset: 0x0000438F
		public static UniTask<long?> SumAsync(this IUniTaskAsyncEnumerable<long?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<long?>>(source, "source");
			return Sum.SumAsync(source, cancellationToken);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000061A3 File Offset: 0x000043A3
		public static UniTask<long?> SumAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000061C3 File Offset: 0x000043C3
		public static UniTask<long?> SumAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<long?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000061E3 File Offset: 0x000043E3
		public static UniTask<long?> SumAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<long?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00006203 File Offset: 0x00004403
		public static UniTask<float?> SumAsync(this IUniTaskAsyncEnumerable<float?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<float?>>(source, "source");
			return Sum.SumAsync(source, cancellationToken);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00006217 File Offset: 0x00004417
		public static UniTask<float?> SumAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00006237 File Offset: 0x00004437
		public static UniTask<float?> SumAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<float?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00006257 File Offset: 0x00004457
		public static UniTask<float?> SumAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<float?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00006277 File Offset: 0x00004477
		public static UniTask<double?> SumAsync(this IUniTaskAsyncEnumerable<double?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<double?>>(source, "source");
			return Sum.SumAsync(source, cancellationToken);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000628B File Offset: 0x0000448B
		public static UniTask<double?> SumAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000062AB File Offset: 0x000044AB
		public static UniTask<double?> SumAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<double?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000062CB File Offset: 0x000044CB
		public static UniTask<double?> SumAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<double?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000062EB File Offset: 0x000044EB
		public static UniTask<decimal?> SumAsync(this IUniTaskAsyncEnumerable<decimal?> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<decimal?>>(source, "source");
			return Sum.SumAsync(source, cancellationToken);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000062FF File Offset: 0x000044FF
		public static UniTask<decimal?> SumAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000631F File Offset: 0x0000451F
		public static UniTask<decimal?> SumAwaitAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000633F File Offset: 0x0000453F
		public static UniTask<decimal?> SumAwaitWithCancellationAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "selector");
			return Sum.SumAwaitWithCancellationAsync<TSource>(source, selector, cancellationToken);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000635F File Offset: 0x0000455F
		public static IUniTaskAsyncEnumerable<TSource> Take<TSource>(this IUniTaskAsyncEnumerable<TSource> source, int count)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return new Take<TSource>(source, count);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00006373 File Offset: 0x00004573
		public static IUniTaskAsyncEnumerable<TSource> TakeLast<TSource>(this IUniTaskAsyncEnumerable<TSource> source, int count)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			if (count <= 0)
			{
				return UniTaskAsyncEnumerable.Empty<TSource>();
			}
			return new TakeLast<TSource>(source, count);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00006391 File Offset: 0x00004591
		public static IUniTaskAsyncEnumerable<TSource> TakeUntil<TSource>(this IUniTaskAsyncEnumerable<TSource> source, UniTask other)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return new TakeUntil<TSource>(source, other, null);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000063A8 File Offset: 0x000045A8
		public static IUniTaskAsyncEnumerable<TSource> TakeUntil<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<CancellationToken, UniTask> other)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "other");
			return new TakeUntil<TSource>(source, default(UniTask), other);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000063DB File Offset: 0x000045DB
		public static IUniTaskAsyncEnumerable<TSource> TakeUntilCanceled<TSource>(this IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return new TakeUntilCanceled<TSource>(source, cancellationToken);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000063EF File Offset: 0x000045EF
		public static IUniTaskAsyncEnumerable<TSource> TakeWhile<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, bool>>(predicate, "predicate");
			return new TakeWhile<TSource>(source, predicate);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000640E File Offset: 0x0000460E
		public static IUniTaskAsyncEnumerable<TSource> TakeWhile<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, bool>>(predicate, "predicate");
			return new TakeWhileInt<TSource>(source, predicate);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000642D File Offset: 0x0000462D
		public static IUniTaskAsyncEnumerable<TSource> TakeWhileAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<bool>>>(predicate, "predicate");
			return new TakeWhileAwait<TSource>(source, predicate);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000644C File Offset: 0x0000464C
		public static IUniTaskAsyncEnumerable<TSource> TakeWhileAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<bool>> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, UniTask<bool>>>(predicate, "predicate");
			return new TakeWhileIntAwait<TSource>(source, predicate);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000646B File Offset: 0x0000466B
		public static IUniTaskAsyncEnumerable<TSource> TakeWhileAwaitWithCancellation<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<bool>>>(predicate, "predicate");
			return new TakeWhileAwaitWithCancellation<TSource>(source, predicate);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000648A File Offset: 0x0000468A
		public static IUniTaskAsyncEnumerable<TSource> TakeWhileAwaitWithCancellation<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, UniTask<bool>> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, CancellationToken, UniTask<bool>>>(predicate, "predicate");
			return new TakeWhileIntAwaitWithCancellation<TSource>(source, predicate);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x000064A9 File Offset: 0x000046A9
		public static IUniTaskAsyncEnumerable<TValue> Throw<TValue>(Exception exception)
		{
			return new Throw<TValue>(exception);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x000064B1 File Offset: 0x000046B1
		public static UniTask<TSource[]> ToArrayAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return ToArray.ToArrayAsync<TSource>(source, cancellationToken);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000064C5 File Offset: 0x000046C5
		public static UniTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			return ToDictionary.ToDictionaryAsync<TSource, TKey>(source, keySelector, EqualityComparer<TKey>.Default, cancellationToken);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000064EA File Offset: 0x000046EA
		public static UniTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return ToDictionary.ToDictionaryAsync<TSource, TKey>(source, keySelector, comparer, cancellationToken);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00006516 File Offset: 0x00004716
		public static UniTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, TElement>>(elementSelector, "elementSelector");
			return ToDictionary.ToDictionaryAsync<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default, cancellationToken);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00006547 File Offset: 0x00004747
		public static UniTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, TElement>>(elementSelector, "elementSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return ToDictionary.ToDictionaryAsync<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00006580 File Offset: 0x00004780
		public static UniTask<Dictionary<TKey, TSource>> ToDictionaryAwaitAsync<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			return ToDictionary.ToDictionaryAwaitAsync<TSource, TKey>(source, keySelector, EqualityComparer<TKey>.Default, cancellationToken);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x000065A5 File Offset: 0x000047A5
		public static UniTask<Dictionary<TKey, TSource>> ToDictionaryAwaitAsync<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return ToDictionary.ToDictionaryAwaitAsync<TSource, TKey>(source, keySelector, comparer, cancellationToken);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x000065D1 File Offset: 0x000047D1
		public static UniTask<Dictionary<TKey, TElement>> ToDictionaryAwaitAsync<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, Func<TSource, UniTask<TElement>> elementSelector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TElement>>>(elementSelector, "elementSelector");
			return ToDictionary.ToDictionaryAwaitAsync<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default, cancellationToken);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00006602 File Offset: 0x00004802
		public static UniTask<Dictionary<TKey, TElement>> ToDictionaryAwaitAsync<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, Func<TSource, UniTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TElement>>>(elementSelector, "elementSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return ToDictionary.ToDictionaryAwaitAsync<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000663B File Offset: 0x0000483B
		public static UniTask<Dictionary<TKey, TSource>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			return ToDictionary.ToDictionaryAwaitWithCancellationAsync<TSource, TKey>(source, keySelector, EqualityComparer<TKey>.Default, cancellationToken);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00006660 File Offset: 0x00004860
		public static UniTask<Dictionary<TKey, TSource>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return ToDictionary.ToDictionaryAwaitWithCancellationAsync<TSource, TKey>(source, keySelector, comparer, cancellationToken);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000668C File Offset: 0x0000488C
		public static UniTask<Dictionary<TKey, TElement>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, Func<TSource, CancellationToken, UniTask<TElement>> elementSelector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TElement>>>(elementSelector, "elementSelector");
			return ToDictionary.ToDictionaryAwaitWithCancellationAsync<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default, cancellationToken);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000066BD File Offset: 0x000048BD
		public static UniTask<Dictionary<TKey, TElement>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, Func<TSource, CancellationToken, UniTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TElement>>>(elementSelector, "elementSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return ToDictionary.ToDictionaryAwaitWithCancellationAsync<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000066F6 File Offset: 0x000048F6
		public static UniTask<HashSet<TSource>> ToHashSetAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return ToHashSet.ToHashSetAsync<TSource>(source, EqualityComparer<TSource>.Default, cancellationToken);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000670F File Offset: 0x0000490F
		public static UniTask<HashSet<TSource>> ToHashSetAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<IEqualityComparer<TSource>>(comparer, "comparer");
			return ToHashSet.ToHashSetAsync<TSource>(source, comparer, cancellationToken);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000672F File Offset: 0x0000492F
		public static UniTask<List<TSource>> ToListAsync<TSource>(this IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return ToList.ToListAsync<TSource>(source, cancellationToken);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00006743 File Offset: 0x00004943
		public static UniTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			return ToLookup.ToLookupAsync<TSource, TKey>(source, keySelector, EqualityComparer<TKey>.Default, cancellationToken);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00006768 File Offset: 0x00004968
		public static UniTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return ToLookup.ToLookupAsync<TSource, TKey>(source, keySelector, comparer, cancellationToken);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00006794 File Offset: 0x00004994
		public static UniTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, TElement>>(elementSelector, "elementSelector");
			return ToLookup.ToLookupAsync<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default, cancellationToken);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000067C5 File Offset: 0x000049C5
		public static UniTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, TKey>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, TElement>>(elementSelector, "elementSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return ToLookup.ToLookupAsync<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000067FE File Offset: 0x000049FE
		public static UniTask<ILookup<TKey, TSource>> ToLookupAwaitAsync<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			return ToLookup.ToLookupAwaitAsync<TSource, TKey>(source, keySelector, EqualityComparer<TKey>.Default, cancellationToken);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00006823 File Offset: 0x00004A23
		public static UniTask<ILookup<TKey, TSource>> ToLookupAwaitAsync<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return ToLookup.ToLookupAwaitAsync<TSource, TKey>(source, keySelector, comparer, cancellationToken);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000684F File Offset: 0x00004A4F
		public static UniTask<ILookup<TKey, TElement>> ToLookupAwaitAsync<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, Func<TSource, UniTask<TElement>> elementSelector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TElement>>>(elementSelector, "elementSelector");
			return ToLookup.ToLookupAwaitAsync<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default, cancellationToken);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00006880 File Offset: 0x00004A80
		public static UniTask<ILookup<TKey, TElement>> ToLookupAwaitAsync<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, Func<TSource, UniTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<TElement>>>(elementSelector, "elementSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return ToLookup.ToLookupAwaitAsync<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000068B9 File Offset: 0x00004AB9
		public static UniTask<ILookup<TKey, TSource>> ToLookupAwaitWithCancellationAsync<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			return ToLookup.ToLookupAwaitWithCancellationAsync<TSource, TKey>(source, keySelector, EqualityComparer<TKey>.Default, cancellationToken);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000068DE File Offset: 0x00004ADE
		public static UniTask<ILookup<TKey, TSource>> ToLookupAwaitWithCancellationAsync<TSource, TKey>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return ToLookup.ToLookupAwaitWithCancellationAsync<TSource, TKey>(source, keySelector, comparer, cancellationToken);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000690A File Offset: 0x00004B0A
		public static UniTask<ILookup<TKey, TElement>> ToLookupAwaitWithCancellationAsync<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, Func<TSource, CancellationToken, UniTask<TElement>> elementSelector, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TElement>>>(elementSelector, "elementSelector");
			return ToLookup.ToLookupAwaitWithCancellationAsync<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default, cancellationToken);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000693B File Offset: 0x00004B3B
		public static UniTask<ILookup<TKey, TElement>> ToLookupAwaitWithCancellationAsync<TSource, TKey, TElement>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, Func<TSource, CancellationToken, UniTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TKey>>>(keySelector, "keySelector");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<TElement>>>(elementSelector, "elementSelector");
			Error.ThrowArgumentNullException<IEqualityComparer<TKey>>(comparer, "comparer");
			return ToLookup.ToLookupAwaitWithCancellationAsync<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00006974 File Offset: 0x00004B74
		public static IObservable<TSource> ToObservable<TSource>(this IUniTaskAsyncEnumerable<TSource> source)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			return new ToObservable<TSource>(source);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00006987 File Offset: 0x00004B87
		public static IUniTaskAsyncEnumerable<TSource> ToUniTaskAsyncEnumerable<TSource>(this IEnumerable<TSource> source)
		{
			Error.ThrowArgumentNullException<IEnumerable<TSource>>(source, "source");
			return new ToUniTaskAsyncEnumerable<TSource>(source);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000699A File Offset: 0x00004B9A
		public static IUniTaskAsyncEnumerable<TSource> ToUniTaskAsyncEnumerable<TSource>(this Task<TSource> source)
		{
			Error.ThrowArgumentNullException<Task<TSource>>(source, "source");
			return new ToUniTaskAsyncEnumerableTask<TSource>(source);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000069AD File Offset: 0x00004BAD
		public static IUniTaskAsyncEnumerable<TSource> ToUniTaskAsyncEnumerable<TSource>(this UniTask<TSource> source)
		{
			return new ToUniTaskAsyncEnumerableUniTask<TSource>(source);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000069B5 File Offset: 0x00004BB5
		public static IUniTaskAsyncEnumerable<TSource> ToUniTaskAsyncEnumerable<TSource>(this IObservable<TSource> source)
		{
			Error.ThrowArgumentNullException<IObservable<TSource>>(source, "source");
			return new ToUniTaskAsyncEnumerableObservable<TSource>(source);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000069C8 File Offset: 0x00004BC8
		public static IUniTaskAsyncEnumerable<TSource> Union<TSource>(this IUniTaskAsyncEnumerable<TSource> first, IUniTaskAsyncEnumerable<TSource> second)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(first, "first");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(second, "second");
			return first.Union(second, EqualityComparer<TSource>.Default);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000069EC File Offset: 0x00004BEC
		public static IUniTaskAsyncEnumerable<TSource> Union<TSource>(this IUniTaskAsyncEnumerable<TSource> first, IUniTaskAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(first, "first");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(second, "second");
			Error.ThrowArgumentNullException<IEqualityComparer<TSource>>(comparer, "comparer");
			return first.Concat(second).Distinct(comparer);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00006A1C File Offset: 0x00004C1C
		public static IUniTaskAsyncEnumerable<AsyncUnit> EveryUpdate(PlayerLoopTiming updateTiming = 8, bool cancelImmediately = false)
		{
			return new EveryUpdate(updateTiming, cancelImmediately);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00006A25 File Offset: 0x00004C25
		public static IUniTaskAsyncEnumerable<TProperty> EveryValueChanged<TTarget, TProperty>(TTarget target, Func<TTarget, TProperty> propertySelector, PlayerLoopTiming monitorTiming = 8, IEqualityComparer<TProperty> equalityComparer = null, bool cancelImmediately = false) where TTarget : class
		{
			if (target is Object)
			{
				return new EveryValueChangedUnityObject<TTarget, TProperty>(target, propertySelector, equalityComparer ?? UnityEqualityComparer.GetDefault<TProperty>(), monitorTiming, cancelImmediately);
			}
			return new EveryValueChangedStandardObject<TTarget, TProperty>(target, propertySelector, equalityComparer ?? UnityEqualityComparer.GetDefault<TProperty>(), monitorTiming, cancelImmediately);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00006A60 File Offset: 0x00004C60
		public static IUniTaskAsyncEnumerable<AsyncUnit> Timer(TimeSpan dueTime, PlayerLoopTiming updateTiming = 8, bool ignoreTimeScale = false, bool cancelImmediately = false)
		{
			return new Timer(dueTime, null, updateTiming, ignoreTimeScale, cancelImmediately);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00006A7F File Offset: 0x00004C7F
		public static IUniTaskAsyncEnumerable<AsyncUnit> Timer(TimeSpan dueTime, TimeSpan period, PlayerLoopTiming updateTiming = 8, bool ignoreTimeScale = false, bool cancelImmediately = false)
		{
			return new Timer(dueTime, new TimeSpan?(period), updateTiming, ignoreTimeScale, cancelImmediately);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00006A91 File Offset: 0x00004C91
		public static IUniTaskAsyncEnumerable<AsyncUnit> Interval(TimeSpan period, PlayerLoopTiming updateTiming = 8, bool ignoreTimeScale = false, bool cancelImmediately = false)
		{
			return new Timer(period, new TimeSpan?(period), updateTiming, ignoreTimeScale, cancelImmediately);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00006AA4 File Offset: 0x00004CA4
		public static IUniTaskAsyncEnumerable<AsyncUnit> TimerFrame(int dueTimeFrameCount, PlayerLoopTiming updateTiming = 8, bool cancelImmediately = false)
		{
			if (dueTimeFrameCount < 0)
			{
				throw new ArgumentOutOfRangeException("Delay does not allow minus delayFrameCount. dueTimeFrameCount:" + dueTimeFrameCount.ToString());
			}
			return new TimerFrame(dueTimeFrameCount, null, updateTiming, cancelImmediately);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00006AE0 File Offset: 0x00004CE0
		public static IUniTaskAsyncEnumerable<AsyncUnit> TimerFrame(int dueTimeFrameCount, int periodFrameCount, PlayerLoopTiming updateTiming = 8, bool cancelImmediately = false)
		{
			if (dueTimeFrameCount < 0)
			{
				throw new ArgumentOutOfRangeException("Delay does not allow minus delayFrameCount. dueTimeFrameCount:" + dueTimeFrameCount.ToString());
			}
			if (periodFrameCount < 0)
			{
				throw new ArgumentOutOfRangeException("Delay does not allow minus periodFrameCount. periodFrameCount:" + dueTimeFrameCount.ToString());
			}
			return new TimerFrame(dueTimeFrameCount, new int?(periodFrameCount), updateTiming, cancelImmediately);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00006B31 File Offset: 0x00004D31
		public static IUniTaskAsyncEnumerable<AsyncUnit> IntervalFrame(int intervalFrameCount, PlayerLoopTiming updateTiming = 8, bool cancelImmediately = false)
		{
			if (intervalFrameCount < 0)
			{
				throw new ArgumentOutOfRangeException("Delay does not allow minus intervalFrameCount. intervalFrameCount:" + intervalFrameCount.ToString());
			}
			return new TimerFrame(intervalFrameCount, new int?(intervalFrameCount), updateTiming, cancelImmediately);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00006B5C File Offset: 0x00004D5C
		public static IUniTaskAsyncEnumerable<TSource> Where<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, bool>>(predicate, "predicate");
			return new Where<TSource>(source, predicate);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00006B7B File Offset: 0x00004D7B
		public static IUniTaskAsyncEnumerable<TSource> Where<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, bool>>(predicate, "predicate");
			return new WhereInt<TSource>(source, predicate);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00006B9A File Offset: 0x00004D9A
		public static IUniTaskAsyncEnumerable<TSource> WhereAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, UniTask<bool>>>(predicate, "predicate");
			return new WhereAwait<TSource>(source, predicate);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00006BB9 File Offset: 0x00004DB9
		public static IUniTaskAsyncEnumerable<TSource> WhereAwait<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<bool>> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, UniTask<bool>>>(predicate, "predicate");
			return new WhereIntAwait<TSource>(source, predicate);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00006BD8 File Offset: 0x00004DD8
		public static IUniTaskAsyncEnumerable<TSource> WhereAwaitWithCancellation<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, CancellationToken, UniTask<bool>>>(predicate, "predicate");
			return new WhereAwaitWithCancellation<TSource>(source, predicate);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00006BF7 File Offset: 0x00004DF7
		public static IUniTaskAsyncEnumerable<TSource> WhereAwaitWithCancellation<TSource>(this IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, UniTask<bool>> predicate)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSource>>(source, "source");
			Error.ThrowArgumentNullException<Func<TSource, int, CancellationToken, UniTask<bool>>>(predicate, "predicate");
			return new WhereIntAwaitWithCancellation<TSource>(source, predicate);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00006C16 File Offset: 0x00004E16
		[return: TupleElementNames(new string[]
		{
			"First",
			"Second"
		})]
		public static IUniTaskAsyncEnumerable<ValueTuple<TFirst, TSecond>> Zip<TFirst, TSecond>(this IUniTaskAsyncEnumerable<TFirst> first, IUniTaskAsyncEnumerable<TSecond> second)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TFirst>>(first, "first");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSecond>>(second, "second");
			return first.Zip(second, (TFirst x, TSecond y) => new ValueTuple<TFirst, TSecond>(x, y));
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00006C54 File Offset: 0x00004E54
		public static IUniTaskAsyncEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IUniTaskAsyncEnumerable<TFirst> first, IUniTaskAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TFirst>>(first, "first");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSecond>>(second, "second");
			Error.ThrowArgumentNullException<Func<TFirst, TSecond, TResult>>(resultSelector, "resultSelector");
			return new Zip<TFirst, TSecond, TResult>(first, second, resultSelector);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00006C7F File Offset: 0x00004E7F
		public static IUniTaskAsyncEnumerable<TResult> ZipAwait<TFirst, TSecond, TResult>(this IUniTaskAsyncEnumerable<TFirst> first, IUniTaskAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, UniTask<TResult>> selector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TFirst>>(first, "first");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSecond>>(second, "second");
			Error.ThrowArgumentNullException<Func<TFirst, TSecond, UniTask<TResult>>>(selector, "selector");
			return new ZipAwait<TFirst, TSecond, TResult>(first, second, selector);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00006CAA File Offset: 0x00004EAA
		public static IUniTaskAsyncEnumerable<TResult> ZipAwaitWithCancellation<TFirst, TSecond, TResult>(this IUniTaskAsyncEnumerable<TFirst> first, IUniTaskAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, CancellationToken, UniTask<TResult>> selector)
		{
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TFirst>>(first, "first");
			Error.ThrowArgumentNullException<IUniTaskAsyncEnumerable<TSecond>>(second, "second");
			Error.ThrowArgumentNullException<Func<TFirst, TSecond, CancellationToken, UniTask<TResult>>>(selector, "selector");
			return new ZipAwaitWithCancellation<TFirst, TSecond, TResult>(first, second, selector);
		}
	}
}
