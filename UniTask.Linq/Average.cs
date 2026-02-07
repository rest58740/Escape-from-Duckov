using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200000A RID: 10
	internal static class Average
	{
		// Token: 0x0600020D RID: 525 RVA: 0x00007808 File Offset: 0x00005A08
		public static UniTask<double> AverageAsync(IUniTaskAsyncEnumerable<int> source, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__0 <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__0>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00007854 File Offset: 0x00005A54
		public static UniTask<double> AverageAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__1<TSource> <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.selector = selector;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__1<TSource>>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x000078A8 File Offset: 0x00005AA8
		public static UniTask<double> AverageAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<int>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitAsync>d__2<TSource> <AverageAwaitAsync>d__;
			<AverageAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<AverageAwaitAsync>d__.source = source;
			<AverageAwaitAsync>d__.selector = selector;
			<AverageAwaitAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitAsync>d__.<>1__state = -1;
			<AverageAwaitAsync>d__.<>t__builder.Start<Average.<AverageAwaitAsync>d__2<TSource>>(ref <AverageAwaitAsync>d__);
			return <AverageAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x000078FC File Offset: 0x00005AFC
		public static UniTask<double> AverageAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<int>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitWithCancellationAsync>d__3<TSource> <AverageAwaitWithCancellationAsync>d__;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<AverageAwaitWithCancellationAsync>d__.source = source;
			<AverageAwaitWithCancellationAsync>d__.selector = selector;
			<AverageAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitWithCancellationAsync>d__.<>1__state = -1;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder.Start<Average.<AverageAwaitWithCancellationAsync>d__3<TSource>>(ref <AverageAwaitWithCancellationAsync>d__);
			return <AverageAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00007950 File Offset: 0x00005B50
		public static UniTask<double> AverageAsync(IUniTaskAsyncEnumerable<long> source, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__4 <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__4>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000799C File Offset: 0x00005B9C
		public static UniTask<double> AverageAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__5<TSource> <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.selector = selector;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__5<TSource>>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000079F0 File Offset: 0x00005BF0
		public static UniTask<double> AverageAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<long>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitAsync>d__6<TSource> <AverageAwaitAsync>d__;
			<AverageAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<AverageAwaitAsync>d__.source = source;
			<AverageAwaitAsync>d__.selector = selector;
			<AverageAwaitAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitAsync>d__.<>1__state = -1;
			<AverageAwaitAsync>d__.<>t__builder.Start<Average.<AverageAwaitAsync>d__6<TSource>>(ref <AverageAwaitAsync>d__);
			return <AverageAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00007A44 File Offset: 0x00005C44
		public static UniTask<double> AverageAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<long>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitWithCancellationAsync>d__7<TSource> <AverageAwaitWithCancellationAsync>d__;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<AverageAwaitWithCancellationAsync>d__.source = source;
			<AverageAwaitWithCancellationAsync>d__.selector = selector;
			<AverageAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitWithCancellationAsync>d__.<>1__state = -1;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder.Start<Average.<AverageAwaitWithCancellationAsync>d__7<TSource>>(ref <AverageAwaitWithCancellationAsync>d__);
			return <AverageAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00007A98 File Offset: 0x00005C98
		public static UniTask<float> AverageAsync(IUniTaskAsyncEnumerable<float> source, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__8 <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__8>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00007AE4 File Offset: 0x00005CE4
		public static UniTask<float> AverageAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__9<TSource> <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.selector = selector;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__9<TSource>>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00007B38 File Offset: 0x00005D38
		public static UniTask<float> AverageAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<float>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitAsync>d__10<TSource> <AverageAwaitAsync>d__;
			<AverageAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float>.Create();
			<AverageAwaitAsync>d__.source = source;
			<AverageAwaitAsync>d__.selector = selector;
			<AverageAwaitAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitAsync>d__.<>1__state = -1;
			<AverageAwaitAsync>d__.<>t__builder.Start<Average.<AverageAwaitAsync>d__10<TSource>>(ref <AverageAwaitAsync>d__);
			return <AverageAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00007B8C File Offset: 0x00005D8C
		public static UniTask<float> AverageAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<float>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitWithCancellationAsync>d__11<TSource> <AverageAwaitWithCancellationAsync>d__;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float>.Create();
			<AverageAwaitWithCancellationAsync>d__.source = source;
			<AverageAwaitWithCancellationAsync>d__.selector = selector;
			<AverageAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitWithCancellationAsync>d__.<>1__state = -1;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder.Start<Average.<AverageAwaitWithCancellationAsync>d__11<TSource>>(ref <AverageAwaitWithCancellationAsync>d__);
			return <AverageAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00007BE0 File Offset: 0x00005DE0
		public static UniTask<double> AverageAsync(IUniTaskAsyncEnumerable<double> source, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__12 <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__12>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00007C2C File Offset: 0x00005E2C
		public static UniTask<double> AverageAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__13<TSource> <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.selector = selector;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__13<TSource>>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00007C80 File Offset: 0x00005E80
		public static UniTask<double> AverageAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<double>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitAsync>d__14<TSource> <AverageAwaitAsync>d__;
			<AverageAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<AverageAwaitAsync>d__.source = source;
			<AverageAwaitAsync>d__.selector = selector;
			<AverageAwaitAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitAsync>d__.<>1__state = -1;
			<AverageAwaitAsync>d__.<>t__builder.Start<Average.<AverageAwaitAsync>d__14<TSource>>(ref <AverageAwaitAsync>d__);
			return <AverageAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00007CD4 File Offset: 0x00005ED4
		public static UniTask<double> AverageAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<double>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitWithCancellationAsync>d__15<TSource> <AverageAwaitWithCancellationAsync>d__;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<AverageAwaitWithCancellationAsync>d__.source = source;
			<AverageAwaitWithCancellationAsync>d__.selector = selector;
			<AverageAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitWithCancellationAsync>d__.<>1__state = -1;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder.Start<Average.<AverageAwaitWithCancellationAsync>d__15<TSource>>(ref <AverageAwaitWithCancellationAsync>d__);
			return <AverageAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00007D28 File Offset: 0x00005F28
		public static UniTask<decimal> AverageAsync(IUniTaskAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__16 <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__16>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00007D74 File Offset: 0x00005F74
		public static UniTask<decimal> AverageAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__17<TSource> <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.selector = selector;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__17<TSource>>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00007DC8 File Offset: 0x00005FC8
		public static UniTask<decimal> AverageAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<decimal>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitAsync>d__18<TSource> <AverageAwaitAsync>d__;
			<AverageAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal>.Create();
			<AverageAwaitAsync>d__.source = source;
			<AverageAwaitAsync>d__.selector = selector;
			<AverageAwaitAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitAsync>d__.<>1__state = -1;
			<AverageAwaitAsync>d__.<>t__builder.Start<Average.<AverageAwaitAsync>d__18<TSource>>(ref <AverageAwaitAsync>d__);
			return <AverageAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00007E1C File Offset: 0x0000601C
		public static UniTask<decimal> AverageAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<decimal>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitWithCancellationAsync>d__19<TSource> <AverageAwaitWithCancellationAsync>d__;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal>.Create();
			<AverageAwaitWithCancellationAsync>d__.source = source;
			<AverageAwaitWithCancellationAsync>d__.selector = selector;
			<AverageAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitWithCancellationAsync>d__.<>1__state = -1;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder.Start<Average.<AverageAwaitWithCancellationAsync>d__19<TSource>>(ref <AverageAwaitWithCancellationAsync>d__);
			return <AverageAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00007E70 File Offset: 0x00006070
		public static UniTask<double?> AverageAsync(IUniTaskAsyncEnumerable<int?> source, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__20 <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__20>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00007EBC File Offset: 0x000060BC
		public static UniTask<double?> AverageAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__21<TSource> <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.selector = selector;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__21<TSource>>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00007F10 File Offset: 0x00006110
		public static UniTask<double?> AverageAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<int?>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitAsync>d__22<TSource> <AverageAwaitAsync>d__;
			<AverageAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<AverageAwaitAsync>d__.source = source;
			<AverageAwaitAsync>d__.selector = selector;
			<AverageAwaitAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitAsync>d__.<>1__state = -1;
			<AverageAwaitAsync>d__.<>t__builder.Start<Average.<AverageAwaitAsync>d__22<TSource>>(ref <AverageAwaitAsync>d__);
			return <AverageAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00007F64 File Offset: 0x00006164
		public static UniTask<double?> AverageAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<int?>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitWithCancellationAsync>d__23<TSource> <AverageAwaitWithCancellationAsync>d__;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<AverageAwaitWithCancellationAsync>d__.source = source;
			<AverageAwaitWithCancellationAsync>d__.selector = selector;
			<AverageAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitWithCancellationAsync>d__.<>1__state = -1;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder.Start<Average.<AverageAwaitWithCancellationAsync>d__23<TSource>>(ref <AverageAwaitWithCancellationAsync>d__);
			return <AverageAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00007FB8 File Offset: 0x000061B8
		public static UniTask<double?> AverageAsync(IUniTaskAsyncEnumerable<long?> source, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__24 <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__24>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00008004 File Offset: 0x00006204
		public static UniTask<double?> AverageAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__25<TSource> <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.selector = selector;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__25<TSource>>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00008058 File Offset: 0x00006258
		public static UniTask<double?> AverageAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<long?>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitAsync>d__26<TSource> <AverageAwaitAsync>d__;
			<AverageAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<AverageAwaitAsync>d__.source = source;
			<AverageAwaitAsync>d__.selector = selector;
			<AverageAwaitAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitAsync>d__.<>1__state = -1;
			<AverageAwaitAsync>d__.<>t__builder.Start<Average.<AverageAwaitAsync>d__26<TSource>>(ref <AverageAwaitAsync>d__);
			return <AverageAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x000080AC File Offset: 0x000062AC
		public static UniTask<double?> AverageAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<long?>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitWithCancellationAsync>d__27<TSource> <AverageAwaitWithCancellationAsync>d__;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<AverageAwaitWithCancellationAsync>d__.source = source;
			<AverageAwaitWithCancellationAsync>d__.selector = selector;
			<AverageAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitWithCancellationAsync>d__.<>1__state = -1;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder.Start<Average.<AverageAwaitWithCancellationAsync>d__27<TSource>>(ref <AverageAwaitWithCancellationAsync>d__);
			return <AverageAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00008100 File Offset: 0x00006300
		public static UniTask<float?> AverageAsync(IUniTaskAsyncEnumerable<float?> source, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__28 <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float?>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__28>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000814C File Offset: 0x0000634C
		public static UniTask<float?> AverageAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__29<TSource> <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float?>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.selector = selector;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__29<TSource>>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x000081A0 File Offset: 0x000063A0
		public static UniTask<float?> AverageAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<float?>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitAsync>d__30<TSource> <AverageAwaitAsync>d__;
			<AverageAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float?>.Create();
			<AverageAwaitAsync>d__.source = source;
			<AverageAwaitAsync>d__.selector = selector;
			<AverageAwaitAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitAsync>d__.<>1__state = -1;
			<AverageAwaitAsync>d__.<>t__builder.Start<Average.<AverageAwaitAsync>d__30<TSource>>(ref <AverageAwaitAsync>d__);
			return <AverageAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x000081F4 File Offset: 0x000063F4
		public static UniTask<float?> AverageAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<float?>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitWithCancellationAsync>d__31<TSource> <AverageAwaitWithCancellationAsync>d__;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float?>.Create();
			<AverageAwaitWithCancellationAsync>d__.source = source;
			<AverageAwaitWithCancellationAsync>d__.selector = selector;
			<AverageAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitWithCancellationAsync>d__.<>1__state = -1;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder.Start<Average.<AverageAwaitWithCancellationAsync>d__31<TSource>>(ref <AverageAwaitWithCancellationAsync>d__);
			return <AverageAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00008248 File Offset: 0x00006448
		public static UniTask<double?> AverageAsync(IUniTaskAsyncEnumerable<double?> source, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__32 <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__32>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00008294 File Offset: 0x00006494
		public static UniTask<double?> AverageAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__33<TSource> <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.selector = selector;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__33<TSource>>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x000082E8 File Offset: 0x000064E8
		public static UniTask<double?> AverageAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<double?>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitAsync>d__34<TSource> <AverageAwaitAsync>d__;
			<AverageAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<AverageAwaitAsync>d__.source = source;
			<AverageAwaitAsync>d__.selector = selector;
			<AverageAwaitAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitAsync>d__.<>1__state = -1;
			<AverageAwaitAsync>d__.<>t__builder.Start<Average.<AverageAwaitAsync>d__34<TSource>>(ref <AverageAwaitAsync>d__);
			return <AverageAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000833C File Offset: 0x0000653C
		public static UniTask<double?> AverageAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<double?>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitWithCancellationAsync>d__35<TSource> <AverageAwaitWithCancellationAsync>d__;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<AverageAwaitWithCancellationAsync>d__.source = source;
			<AverageAwaitWithCancellationAsync>d__.selector = selector;
			<AverageAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitWithCancellationAsync>d__.<>1__state = -1;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder.Start<Average.<AverageAwaitWithCancellationAsync>d__35<TSource>>(ref <AverageAwaitWithCancellationAsync>d__);
			return <AverageAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00008390 File Offset: 0x00006590
		public static UniTask<decimal?> AverageAsync(IUniTaskAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__36 <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal?>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__36>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x000083DC File Offset: 0x000065DC
		public static UniTask<decimal?> AverageAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAsync>d__37<TSource> <AverageAsync>d__;
			<AverageAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal?>.Create();
			<AverageAsync>d__.source = source;
			<AverageAsync>d__.selector = selector;
			<AverageAsync>d__.cancellationToken = cancellationToken;
			<AverageAsync>d__.<>1__state = -1;
			<AverageAsync>d__.<>t__builder.Start<Average.<AverageAsync>d__37<TSource>>(ref <AverageAsync>d__);
			return <AverageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00008430 File Offset: 0x00006630
		public static UniTask<decimal?> AverageAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<decimal?>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitAsync>d__38<TSource> <AverageAwaitAsync>d__;
			<AverageAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal?>.Create();
			<AverageAwaitAsync>d__.source = source;
			<AverageAwaitAsync>d__.selector = selector;
			<AverageAwaitAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitAsync>d__.<>1__state = -1;
			<AverageAwaitAsync>d__.<>t__builder.Start<Average.<AverageAwaitAsync>d__38<TSource>>(ref <AverageAwaitAsync>d__);
			return <AverageAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00008484 File Offset: 0x00006684
		public static UniTask<decimal?> AverageAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<decimal?>> selector, CancellationToken cancellationToken)
		{
			Average.<AverageAwaitWithCancellationAsync>d__39<TSource> <AverageAwaitWithCancellationAsync>d__;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal?>.Create();
			<AverageAwaitWithCancellationAsync>d__.source = source;
			<AverageAwaitWithCancellationAsync>d__.selector = selector;
			<AverageAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<AverageAwaitWithCancellationAsync>d__.<>1__state = -1;
			<AverageAwaitWithCancellationAsync>d__.<>t__builder.Start<Average.<AverageAwaitWithCancellationAsync>d__39<TSource>>(ref <AverageAwaitWithCancellationAsync>d__);
			return <AverageAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}
	}
}
