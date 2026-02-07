using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200006A RID: 106
	internal static class Sum
	{
		// Token: 0x06000368 RID: 872 RVA: 0x0000C8C4 File Offset: 0x0000AAC4
		public static UniTask<int> SumAsync(IUniTaskAsyncEnumerable<int> source, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__0 <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__0>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000C910 File Offset: 0x0000AB10
		public static UniTask<int> SumAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__1<TSource> <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.selector = selector;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__1<TSource>>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000C964 File Offset: 0x0000AB64
		public static UniTask<int> SumAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<int>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitAsync>d__2<TSource> <SumAwaitAsync>d__;
			<SumAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<SumAwaitAsync>d__.source = source;
			<SumAwaitAsync>d__.selector = selector;
			<SumAwaitAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitAsync>d__.<>1__state = -1;
			<SumAwaitAsync>d__.<>t__builder.Start<Sum.<SumAwaitAsync>d__2<TSource>>(ref <SumAwaitAsync>d__);
			return <SumAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000C9B8 File Offset: 0x0000ABB8
		public static UniTask<int> SumAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<int>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitWithCancellationAsync>d__3<TSource> <SumAwaitWithCancellationAsync>d__;
			<SumAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<SumAwaitWithCancellationAsync>d__.source = source;
			<SumAwaitWithCancellationAsync>d__.selector = selector;
			<SumAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitWithCancellationAsync>d__.<>1__state = -1;
			<SumAwaitWithCancellationAsync>d__.<>t__builder.Start<Sum.<SumAwaitWithCancellationAsync>d__3<TSource>>(ref <SumAwaitWithCancellationAsync>d__);
			return <SumAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000CA0C File Offset: 0x0000AC0C
		public static UniTask<long> SumAsync(IUniTaskAsyncEnumerable<long> source, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__4 <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__4>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000CA58 File Offset: 0x0000AC58
		public static UniTask<long> SumAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__5<TSource> <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.selector = selector;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__5<TSource>>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000CAAC File Offset: 0x0000ACAC
		public static UniTask<long> SumAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<long>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitAsync>d__6<TSource> <SumAwaitAsync>d__;
			<SumAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long>.Create();
			<SumAwaitAsync>d__.source = source;
			<SumAwaitAsync>d__.selector = selector;
			<SumAwaitAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitAsync>d__.<>1__state = -1;
			<SumAwaitAsync>d__.<>t__builder.Start<Sum.<SumAwaitAsync>d__6<TSource>>(ref <SumAwaitAsync>d__);
			return <SumAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000CB00 File Offset: 0x0000AD00
		public static UniTask<long> SumAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<long>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitWithCancellationAsync>d__7<TSource> <SumAwaitWithCancellationAsync>d__;
			<SumAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long>.Create();
			<SumAwaitWithCancellationAsync>d__.source = source;
			<SumAwaitWithCancellationAsync>d__.selector = selector;
			<SumAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitWithCancellationAsync>d__.<>1__state = -1;
			<SumAwaitWithCancellationAsync>d__.<>t__builder.Start<Sum.<SumAwaitWithCancellationAsync>d__7<TSource>>(ref <SumAwaitWithCancellationAsync>d__);
			return <SumAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000CB54 File Offset: 0x0000AD54
		public static UniTask<float> SumAsync(IUniTaskAsyncEnumerable<float> source, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__8 <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__8>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000CBA0 File Offset: 0x0000ADA0
		public static UniTask<float> SumAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__9<TSource> <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.selector = selector;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__9<TSource>>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000CBF4 File Offset: 0x0000ADF4
		public static UniTask<float> SumAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<float>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitAsync>d__10<TSource> <SumAwaitAsync>d__;
			<SumAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float>.Create();
			<SumAwaitAsync>d__.source = source;
			<SumAwaitAsync>d__.selector = selector;
			<SumAwaitAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitAsync>d__.<>1__state = -1;
			<SumAwaitAsync>d__.<>t__builder.Start<Sum.<SumAwaitAsync>d__10<TSource>>(ref <SumAwaitAsync>d__);
			return <SumAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000CC48 File Offset: 0x0000AE48
		public static UniTask<float> SumAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<float>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitWithCancellationAsync>d__11<TSource> <SumAwaitWithCancellationAsync>d__;
			<SumAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float>.Create();
			<SumAwaitWithCancellationAsync>d__.source = source;
			<SumAwaitWithCancellationAsync>d__.selector = selector;
			<SumAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitWithCancellationAsync>d__.<>1__state = -1;
			<SumAwaitWithCancellationAsync>d__.<>t__builder.Start<Sum.<SumAwaitWithCancellationAsync>d__11<TSource>>(ref <SumAwaitWithCancellationAsync>d__);
			return <SumAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000CC9C File Offset: 0x0000AE9C
		public static UniTask<double> SumAsync(IUniTaskAsyncEnumerable<double> source, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__12 <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__12>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000CCE8 File Offset: 0x0000AEE8
		public static UniTask<double> SumAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__13<TSource> <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.selector = selector;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__13<TSource>>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000CD3C File Offset: 0x0000AF3C
		public static UniTask<double> SumAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<double>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitAsync>d__14<TSource> <SumAwaitAsync>d__;
			<SumAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<SumAwaitAsync>d__.source = source;
			<SumAwaitAsync>d__.selector = selector;
			<SumAwaitAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitAsync>d__.<>1__state = -1;
			<SumAwaitAsync>d__.<>t__builder.Start<Sum.<SumAwaitAsync>d__14<TSource>>(ref <SumAwaitAsync>d__);
			return <SumAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000CD90 File Offset: 0x0000AF90
		public static UniTask<double> SumAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<double>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitWithCancellationAsync>d__15<TSource> <SumAwaitWithCancellationAsync>d__;
			<SumAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<SumAwaitWithCancellationAsync>d__.source = source;
			<SumAwaitWithCancellationAsync>d__.selector = selector;
			<SumAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitWithCancellationAsync>d__.<>1__state = -1;
			<SumAwaitWithCancellationAsync>d__.<>t__builder.Start<Sum.<SumAwaitWithCancellationAsync>d__15<TSource>>(ref <SumAwaitWithCancellationAsync>d__);
			return <SumAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000CDE4 File Offset: 0x0000AFE4
		public static UniTask<decimal> SumAsync(IUniTaskAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__16 <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__16>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000CE30 File Offset: 0x0000B030
		public static UniTask<decimal> SumAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__17<TSource> <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.selector = selector;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__17<TSource>>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000CE84 File Offset: 0x0000B084
		public static UniTask<decimal> SumAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<decimal>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitAsync>d__18<TSource> <SumAwaitAsync>d__;
			<SumAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal>.Create();
			<SumAwaitAsync>d__.source = source;
			<SumAwaitAsync>d__.selector = selector;
			<SumAwaitAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitAsync>d__.<>1__state = -1;
			<SumAwaitAsync>d__.<>t__builder.Start<Sum.<SumAwaitAsync>d__18<TSource>>(ref <SumAwaitAsync>d__);
			return <SumAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000CED8 File Offset: 0x0000B0D8
		public static UniTask<decimal> SumAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<decimal>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitWithCancellationAsync>d__19<TSource> <SumAwaitWithCancellationAsync>d__;
			<SumAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal>.Create();
			<SumAwaitWithCancellationAsync>d__.source = source;
			<SumAwaitWithCancellationAsync>d__.selector = selector;
			<SumAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitWithCancellationAsync>d__.<>1__state = -1;
			<SumAwaitWithCancellationAsync>d__.<>t__builder.Start<Sum.<SumAwaitWithCancellationAsync>d__19<TSource>>(ref <SumAwaitWithCancellationAsync>d__);
			return <SumAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000CF2C File Offset: 0x0000B12C
		public static UniTask<int?> SumAsync(IUniTaskAsyncEnumerable<int?> source, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__20 <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int?>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__20>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000CF78 File Offset: 0x0000B178
		public static UniTask<int?> SumAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__21<TSource> <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int?>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.selector = selector;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__21<TSource>>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000CFCC File Offset: 0x0000B1CC
		public static UniTask<int?> SumAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<int?>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitAsync>d__22<TSource> <SumAwaitAsync>d__;
			<SumAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int?>.Create();
			<SumAwaitAsync>d__.source = source;
			<SumAwaitAsync>d__.selector = selector;
			<SumAwaitAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitAsync>d__.<>1__state = -1;
			<SumAwaitAsync>d__.<>t__builder.Start<Sum.<SumAwaitAsync>d__22<TSource>>(ref <SumAwaitAsync>d__);
			return <SumAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000D020 File Offset: 0x0000B220
		public static UniTask<int?> SumAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<int?>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitWithCancellationAsync>d__23<TSource> <SumAwaitWithCancellationAsync>d__;
			<SumAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int?>.Create();
			<SumAwaitWithCancellationAsync>d__.source = source;
			<SumAwaitWithCancellationAsync>d__.selector = selector;
			<SumAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitWithCancellationAsync>d__.<>1__state = -1;
			<SumAwaitWithCancellationAsync>d__.<>t__builder.Start<Sum.<SumAwaitWithCancellationAsync>d__23<TSource>>(ref <SumAwaitWithCancellationAsync>d__);
			return <SumAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000D074 File Offset: 0x0000B274
		public static UniTask<long?> SumAsync(IUniTaskAsyncEnumerable<long?> source, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__24 <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long?>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__24>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000D0C0 File Offset: 0x0000B2C0
		public static UniTask<long?> SumAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__25<TSource> <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long?>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.selector = selector;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__25<TSource>>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000D114 File Offset: 0x0000B314
		public static UniTask<long?> SumAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<long?>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitAsync>d__26<TSource> <SumAwaitAsync>d__;
			<SumAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long?>.Create();
			<SumAwaitAsync>d__.source = source;
			<SumAwaitAsync>d__.selector = selector;
			<SumAwaitAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitAsync>d__.<>1__state = -1;
			<SumAwaitAsync>d__.<>t__builder.Start<Sum.<SumAwaitAsync>d__26<TSource>>(ref <SumAwaitAsync>d__);
			return <SumAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000D168 File Offset: 0x0000B368
		public static UniTask<long?> SumAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<long?>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitWithCancellationAsync>d__27<TSource> <SumAwaitWithCancellationAsync>d__;
			<SumAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long?>.Create();
			<SumAwaitWithCancellationAsync>d__.source = source;
			<SumAwaitWithCancellationAsync>d__.selector = selector;
			<SumAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitWithCancellationAsync>d__.<>1__state = -1;
			<SumAwaitWithCancellationAsync>d__.<>t__builder.Start<Sum.<SumAwaitWithCancellationAsync>d__27<TSource>>(ref <SumAwaitWithCancellationAsync>d__);
			return <SumAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000D1BC File Offset: 0x0000B3BC
		public static UniTask<float?> SumAsync(IUniTaskAsyncEnumerable<float?> source, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__28 <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float?>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__28>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000D208 File Offset: 0x0000B408
		public static UniTask<float?> SumAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__29<TSource> <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float?>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.selector = selector;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__29<TSource>>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000D25C File Offset: 0x0000B45C
		public static UniTask<float?> SumAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<float?>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitAsync>d__30<TSource> <SumAwaitAsync>d__;
			<SumAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float?>.Create();
			<SumAwaitAsync>d__.source = source;
			<SumAwaitAsync>d__.selector = selector;
			<SumAwaitAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitAsync>d__.<>1__state = -1;
			<SumAwaitAsync>d__.<>t__builder.Start<Sum.<SumAwaitAsync>d__30<TSource>>(ref <SumAwaitAsync>d__);
			return <SumAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000D2B0 File Offset: 0x0000B4B0
		public static UniTask<float?> SumAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<float?>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitWithCancellationAsync>d__31<TSource> <SumAwaitWithCancellationAsync>d__;
			<SumAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float?>.Create();
			<SumAwaitWithCancellationAsync>d__.source = source;
			<SumAwaitWithCancellationAsync>d__.selector = selector;
			<SumAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitWithCancellationAsync>d__.<>1__state = -1;
			<SumAwaitWithCancellationAsync>d__.<>t__builder.Start<Sum.<SumAwaitWithCancellationAsync>d__31<TSource>>(ref <SumAwaitWithCancellationAsync>d__);
			return <SumAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000D304 File Offset: 0x0000B504
		public static UniTask<double?> SumAsync(IUniTaskAsyncEnumerable<double?> source, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__32 <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__32>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000D350 File Offset: 0x0000B550
		public static UniTask<double?> SumAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__33<TSource> <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.selector = selector;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__33<TSource>>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000D3A4 File Offset: 0x0000B5A4
		public static UniTask<double?> SumAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<double?>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitAsync>d__34<TSource> <SumAwaitAsync>d__;
			<SumAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<SumAwaitAsync>d__.source = source;
			<SumAwaitAsync>d__.selector = selector;
			<SumAwaitAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitAsync>d__.<>1__state = -1;
			<SumAwaitAsync>d__.<>t__builder.Start<Sum.<SumAwaitAsync>d__34<TSource>>(ref <SumAwaitAsync>d__);
			return <SumAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000D3F8 File Offset: 0x0000B5F8
		public static UniTask<double?> SumAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<double?>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitWithCancellationAsync>d__35<TSource> <SumAwaitWithCancellationAsync>d__;
			<SumAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<SumAwaitWithCancellationAsync>d__.source = source;
			<SumAwaitWithCancellationAsync>d__.selector = selector;
			<SumAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitWithCancellationAsync>d__.<>1__state = -1;
			<SumAwaitWithCancellationAsync>d__.<>t__builder.Start<Sum.<SumAwaitWithCancellationAsync>d__35<TSource>>(ref <SumAwaitWithCancellationAsync>d__);
			return <SumAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000D44C File Offset: 0x0000B64C
		public static UniTask<decimal?> SumAsync(IUniTaskAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__36 <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal?>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__36>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000D498 File Offset: 0x0000B698
		public static UniTask<decimal?> SumAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAsync>d__37<TSource> <SumAsync>d__;
			<SumAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal?>.Create();
			<SumAsync>d__.source = source;
			<SumAsync>d__.selector = selector;
			<SumAsync>d__.cancellationToken = cancellationToken;
			<SumAsync>d__.<>1__state = -1;
			<SumAsync>d__.<>t__builder.Start<Sum.<SumAsync>d__37<TSource>>(ref <SumAsync>d__);
			return <SumAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000D4EC File Offset: 0x0000B6EC
		public static UniTask<decimal?> SumAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<decimal?>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitAsync>d__38<TSource> <SumAwaitAsync>d__;
			<SumAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal?>.Create();
			<SumAwaitAsync>d__.source = source;
			<SumAwaitAsync>d__.selector = selector;
			<SumAwaitAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitAsync>d__.<>1__state = -1;
			<SumAwaitAsync>d__.<>t__builder.Start<Sum.<SumAwaitAsync>d__38<TSource>>(ref <SumAwaitAsync>d__);
			return <SumAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000D540 File Offset: 0x0000B740
		public static UniTask<decimal?> SumAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<decimal?>> selector, CancellationToken cancellationToken)
		{
			Sum.<SumAwaitWithCancellationAsync>d__39<TSource> <SumAwaitWithCancellationAsync>d__;
			<SumAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal?>.Create();
			<SumAwaitWithCancellationAsync>d__.source = source;
			<SumAwaitWithCancellationAsync>d__.selector = selector;
			<SumAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<SumAwaitWithCancellationAsync>d__.<>1__state = -1;
			<SumAwaitWithCancellationAsync>d__.<>t__builder.Start<Sum.<SumAwaitWithCancellationAsync>d__39<TSource>>(ref <SumAwaitWithCancellationAsync>d__);
			return <SumAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}
	}
}
