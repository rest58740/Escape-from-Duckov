using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200003F RID: 63
	internal static class Max
	{
		// Token: 0x060002A7 RID: 679 RVA: 0x00009D28 File Offset: 0x00007F28
		public static UniTask<TSource> MaxAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__0<TSource> <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TSource>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__0<TSource>>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00009D74 File Offset: 0x00007F74
		public static UniTask<TResult> MaxAsync<TSource, TResult>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TResult> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__1<TSource, TResult> <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TResult>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.selector = selector;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__1<TSource, TResult>>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00009DC8 File Offset: 0x00007FC8
		public static UniTask<TResult> MaxAwaitAsync<TSource, TResult>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TResult>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitAsync>d__2<TSource, TResult> <MaxAwaitAsync>d__;
			<MaxAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TResult>.Create();
			<MaxAwaitAsync>d__.source = source;
			<MaxAwaitAsync>d__.selector = selector;
			<MaxAwaitAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitAsync>d__.<>1__state = -1;
			<MaxAwaitAsync>d__.<>t__builder.Start<Max.<MaxAwaitAsync>d__2<TSource, TResult>>(ref <MaxAwaitAsync>d__);
			return <MaxAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00009E1C File Offset: 0x0000801C
		public static UniTask<TResult> MaxAwaitWithCancellationAsync<TSource, TResult>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TResult>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitWithCancellationAsync>d__3<TSource, TResult> <MaxAwaitWithCancellationAsync>d__;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TResult>.Create();
			<MaxAwaitWithCancellationAsync>d__.source = source;
			<MaxAwaitWithCancellationAsync>d__.selector = selector;
			<MaxAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder.Start<Max.<MaxAwaitWithCancellationAsync>d__3<TSource, TResult>>(ref <MaxAwaitWithCancellationAsync>d__);
			return <MaxAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00009E70 File Offset: 0x00008070
		public static UniTask<int> MaxAsync(IUniTaskAsyncEnumerable<int> source, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__4 <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__4>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00009EBC File Offset: 0x000080BC
		public static UniTask<int> MaxAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__5<TSource> <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.selector = selector;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__5<TSource>>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00009F10 File Offset: 0x00008110
		public static UniTask<int> MaxAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<int>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitAsync>d__6<TSource> <MaxAwaitAsync>d__;
			<MaxAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<MaxAwaitAsync>d__.source = source;
			<MaxAwaitAsync>d__.selector = selector;
			<MaxAwaitAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitAsync>d__.<>1__state = -1;
			<MaxAwaitAsync>d__.<>t__builder.Start<Max.<MaxAwaitAsync>d__6<TSource>>(ref <MaxAwaitAsync>d__);
			return <MaxAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00009F64 File Offset: 0x00008164
		public static UniTask<int> MaxAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<int>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitWithCancellationAsync>d__7<TSource> <MaxAwaitWithCancellationAsync>d__;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<MaxAwaitWithCancellationAsync>d__.source = source;
			<MaxAwaitWithCancellationAsync>d__.selector = selector;
			<MaxAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder.Start<Max.<MaxAwaitWithCancellationAsync>d__7<TSource>>(ref <MaxAwaitWithCancellationAsync>d__);
			return <MaxAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00009FB8 File Offset: 0x000081B8
		public static UniTask<long> MaxAsync(IUniTaskAsyncEnumerable<long> source, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__8 <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__8>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000A004 File Offset: 0x00008204
		public static UniTask<long> MaxAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__9<TSource> <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.selector = selector;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__9<TSource>>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000A058 File Offset: 0x00008258
		public static UniTask<long> MaxAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<long>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitAsync>d__10<TSource> <MaxAwaitAsync>d__;
			<MaxAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long>.Create();
			<MaxAwaitAsync>d__.source = source;
			<MaxAwaitAsync>d__.selector = selector;
			<MaxAwaitAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitAsync>d__.<>1__state = -1;
			<MaxAwaitAsync>d__.<>t__builder.Start<Max.<MaxAwaitAsync>d__10<TSource>>(ref <MaxAwaitAsync>d__);
			return <MaxAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000A0AC File Offset: 0x000082AC
		public static UniTask<long> MaxAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<long>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitWithCancellationAsync>d__11<TSource> <MaxAwaitWithCancellationAsync>d__;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long>.Create();
			<MaxAwaitWithCancellationAsync>d__.source = source;
			<MaxAwaitWithCancellationAsync>d__.selector = selector;
			<MaxAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder.Start<Max.<MaxAwaitWithCancellationAsync>d__11<TSource>>(ref <MaxAwaitWithCancellationAsync>d__);
			return <MaxAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000A100 File Offset: 0x00008300
		public static UniTask<float> MaxAsync(IUniTaskAsyncEnumerable<float> source, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__12 <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__12>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000A14C File Offset: 0x0000834C
		public static UniTask<float> MaxAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__13<TSource> <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.selector = selector;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__13<TSource>>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000A1A0 File Offset: 0x000083A0
		public static UniTask<float> MaxAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<float>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitAsync>d__14<TSource> <MaxAwaitAsync>d__;
			<MaxAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float>.Create();
			<MaxAwaitAsync>d__.source = source;
			<MaxAwaitAsync>d__.selector = selector;
			<MaxAwaitAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitAsync>d__.<>1__state = -1;
			<MaxAwaitAsync>d__.<>t__builder.Start<Max.<MaxAwaitAsync>d__14<TSource>>(ref <MaxAwaitAsync>d__);
			return <MaxAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000A1F4 File Offset: 0x000083F4
		public static UniTask<float> MaxAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<float>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitWithCancellationAsync>d__15<TSource> <MaxAwaitWithCancellationAsync>d__;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float>.Create();
			<MaxAwaitWithCancellationAsync>d__.source = source;
			<MaxAwaitWithCancellationAsync>d__.selector = selector;
			<MaxAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder.Start<Max.<MaxAwaitWithCancellationAsync>d__15<TSource>>(ref <MaxAwaitWithCancellationAsync>d__);
			return <MaxAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000A248 File Offset: 0x00008448
		public static UniTask<double> MaxAsync(IUniTaskAsyncEnumerable<double> source, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__16 <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__16>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000A294 File Offset: 0x00008494
		public static UniTask<double> MaxAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__17<TSource> <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.selector = selector;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__17<TSource>>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000A2E8 File Offset: 0x000084E8
		public static UniTask<double> MaxAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<double>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitAsync>d__18<TSource> <MaxAwaitAsync>d__;
			<MaxAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<MaxAwaitAsync>d__.source = source;
			<MaxAwaitAsync>d__.selector = selector;
			<MaxAwaitAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitAsync>d__.<>1__state = -1;
			<MaxAwaitAsync>d__.<>t__builder.Start<Max.<MaxAwaitAsync>d__18<TSource>>(ref <MaxAwaitAsync>d__);
			return <MaxAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000A33C File Offset: 0x0000853C
		public static UniTask<double> MaxAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<double>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitWithCancellationAsync>d__19<TSource> <MaxAwaitWithCancellationAsync>d__;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<MaxAwaitWithCancellationAsync>d__.source = source;
			<MaxAwaitWithCancellationAsync>d__.selector = selector;
			<MaxAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder.Start<Max.<MaxAwaitWithCancellationAsync>d__19<TSource>>(ref <MaxAwaitWithCancellationAsync>d__);
			return <MaxAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000A390 File Offset: 0x00008590
		public static UniTask<decimal> MaxAsync(IUniTaskAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__20 <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__20>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000A3DC File Offset: 0x000085DC
		public static UniTask<decimal> MaxAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__21<TSource> <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.selector = selector;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__21<TSource>>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000A430 File Offset: 0x00008630
		public static UniTask<decimal> MaxAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<decimal>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitAsync>d__22<TSource> <MaxAwaitAsync>d__;
			<MaxAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal>.Create();
			<MaxAwaitAsync>d__.source = source;
			<MaxAwaitAsync>d__.selector = selector;
			<MaxAwaitAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitAsync>d__.<>1__state = -1;
			<MaxAwaitAsync>d__.<>t__builder.Start<Max.<MaxAwaitAsync>d__22<TSource>>(ref <MaxAwaitAsync>d__);
			return <MaxAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000A484 File Offset: 0x00008684
		public static UniTask<decimal> MaxAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<decimal>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitWithCancellationAsync>d__23<TSource> <MaxAwaitWithCancellationAsync>d__;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal>.Create();
			<MaxAwaitWithCancellationAsync>d__.source = source;
			<MaxAwaitWithCancellationAsync>d__.selector = selector;
			<MaxAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder.Start<Max.<MaxAwaitWithCancellationAsync>d__23<TSource>>(ref <MaxAwaitWithCancellationAsync>d__);
			return <MaxAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000A4D8 File Offset: 0x000086D8
		public static UniTask<int?> MaxAsync(IUniTaskAsyncEnumerable<int?> source, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__24 <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int?>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__24>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000A524 File Offset: 0x00008724
		public static UniTask<int?> MaxAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__25<TSource> <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int?>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.selector = selector;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__25<TSource>>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000A578 File Offset: 0x00008778
		public static UniTask<int?> MaxAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<int?>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitAsync>d__26<TSource> <MaxAwaitAsync>d__;
			<MaxAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int?>.Create();
			<MaxAwaitAsync>d__.source = source;
			<MaxAwaitAsync>d__.selector = selector;
			<MaxAwaitAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitAsync>d__.<>1__state = -1;
			<MaxAwaitAsync>d__.<>t__builder.Start<Max.<MaxAwaitAsync>d__26<TSource>>(ref <MaxAwaitAsync>d__);
			return <MaxAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000A5CC File Offset: 0x000087CC
		public static UniTask<int?> MaxAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<int?>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitWithCancellationAsync>d__27<TSource> <MaxAwaitWithCancellationAsync>d__;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int?>.Create();
			<MaxAwaitWithCancellationAsync>d__.source = source;
			<MaxAwaitWithCancellationAsync>d__.selector = selector;
			<MaxAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder.Start<Max.<MaxAwaitWithCancellationAsync>d__27<TSource>>(ref <MaxAwaitWithCancellationAsync>d__);
			return <MaxAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000A620 File Offset: 0x00008820
		public static UniTask<long?> MaxAsync(IUniTaskAsyncEnumerable<long?> source, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__28 <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long?>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__28>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000A66C File Offset: 0x0000886C
		public static UniTask<long?> MaxAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__29<TSource> <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long?>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.selector = selector;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__29<TSource>>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000A6C0 File Offset: 0x000088C0
		public static UniTask<long?> MaxAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<long?>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitAsync>d__30<TSource> <MaxAwaitAsync>d__;
			<MaxAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long?>.Create();
			<MaxAwaitAsync>d__.source = source;
			<MaxAwaitAsync>d__.selector = selector;
			<MaxAwaitAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitAsync>d__.<>1__state = -1;
			<MaxAwaitAsync>d__.<>t__builder.Start<Max.<MaxAwaitAsync>d__30<TSource>>(ref <MaxAwaitAsync>d__);
			return <MaxAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000A714 File Offset: 0x00008914
		public static UniTask<long?> MaxAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<long?>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitWithCancellationAsync>d__31<TSource> <MaxAwaitWithCancellationAsync>d__;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long?>.Create();
			<MaxAwaitWithCancellationAsync>d__.source = source;
			<MaxAwaitWithCancellationAsync>d__.selector = selector;
			<MaxAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder.Start<Max.<MaxAwaitWithCancellationAsync>d__31<TSource>>(ref <MaxAwaitWithCancellationAsync>d__);
			return <MaxAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000A768 File Offset: 0x00008968
		public static UniTask<float?> MaxAsync(IUniTaskAsyncEnumerable<float?> source, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__32 <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float?>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__32>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000A7B4 File Offset: 0x000089B4
		public static UniTask<float?> MaxAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__33<TSource> <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float?>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.selector = selector;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__33<TSource>>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000A808 File Offset: 0x00008A08
		public static UniTask<float?> MaxAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<float?>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitAsync>d__34<TSource> <MaxAwaitAsync>d__;
			<MaxAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float?>.Create();
			<MaxAwaitAsync>d__.source = source;
			<MaxAwaitAsync>d__.selector = selector;
			<MaxAwaitAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitAsync>d__.<>1__state = -1;
			<MaxAwaitAsync>d__.<>t__builder.Start<Max.<MaxAwaitAsync>d__34<TSource>>(ref <MaxAwaitAsync>d__);
			return <MaxAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000A85C File Offset: 0x00008A5C
		public static UniTask<float?> MaxAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<float?>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitWithCancellationAsync>d__35<TSource> <MaxAwaitWithCancellationAsync>d__;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float?>.Create();
			<MaxAwaitWithCancellationAsync>d__.source = source;
			<MaxAwaitWithCancellationAsync>d__.selector = selector;
			<MaxAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder.Start<Max.<MaxAwaitWithCancellationAsync>d__35<TSource>>(ref <MaxAwaitWithCancellationAsync>d__);
			return <MaxAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000A8B0 File Offset: 0x00008AB0
		public static UniTask<double?> MaxAsync(IUniTaskAsyncEnumerable<double?> source, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__36 <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__36>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000A8FC File Offset: 0x00008AFC
		public static UniTask<double?> MaxAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__37<TSource> <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.selector = selector;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__37<TSource>>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000A950 File Offset: 0x00008B50
		public static UniTask<double?> MaxAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<double?>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitAsync>d__38<TSource> <MaxAwaitAsync>d__;
			<MaxAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<MaxAwaitAsync>d__.source = source;
			<MaxAwaitAsync>d__.selector = selector;
			<MaxAwaitAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitAsync>d__.<>1__state = -1;
			<MaxAwaitAsync>d__.<>t__builder.Start<Max.<MaxAwaitAsync>d__38<TSource>>(ref <MaxAwaitAsync>d__);
			return <MaxAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000A9A4 File Offset: 0x00008BA4
		public static UniTask<double?> MaxAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<double?>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitWithCancellationAsync>d__39<TSource> <MaxAwaitWithCancellationAsync>d__;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<MaxAwaitWithCancellationAsync>d__.source = source;
			<MaxAwaitWithCancellationAsync>d__.selector = selector;
			<MaxAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder.Start<Max.<MaxAwaitWithCancellationAsync>d__39<TSource>>(ref <MaxAwaitWithCancellationAsync>d__);
			return <MaxAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000A9F8 File Offset: 0x00008BF8
		public static UniTask<decimal?> MaxAsync(IUniTaskAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__40 <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal?>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__40>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000AA44 File Offset: 0x00008C44
		public static UniTask<decimal?> MaxAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAsync>d__41<TSource> <MaxAsync>d__;
			<MaxAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal?>.Create();
			<MaxAsync>d__.source = source;
			<MaxAsync>d__.selector = selector;
			<MaxAsync>d__.cancellationToken = cancellationToken;
			<MaxAsync>d__.<>1__state = -1;
			<MaxAsync>d__.<>t__builder.Start<Max.<MaxAsync>d__41<TSource>>(ref <MaxAsync>d__);
			return <MaxAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000AA98 File Offset: 0x00008C98
		public static UniTask<decimal?> MaxAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<decimal?>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitAsync>d__42<TSource> <MaxAwaitAsync>d__;
			<MaxAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal?>.Create();
			<MaxAwaitAsync>d__.source = source;
			<MaxAwaitAsync>d__.selector = selector;
			<MaxAwaitAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitAsync>d__.<>1__state = -1;
			<MaxAwaitAsync>d__.<>t__builder.Start<Max.<MaxAwaitAsync>d__42<TSource>>(ref <MaxAwaitAsync>d__);
			return <MaxAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000AAEC File Offset: 0x00008CEC
		public static UniTask<decimal?> MaxAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<decimal?>> selector, CancellationToken cancellationToken)
		{
			Max.<MaxAwaitWithCancellationAsync>d__43<TSource> <MaxAwaitWithCancellationAsync>d__;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal?>.Create();
			<MaxAwaitWithCancellationAsync>d__.source = source;
			<MaxAwaitWithCancellationAsync>d__.selector = selector;
			<MaxAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MaxAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MaxAwaitWithCancellationAsync>d__.<>t__builder.Start<Max.<MaxAwaitWithCancellationAsync>d__43<TSource>>(ref <MaxAwaitWithCancellationAsync>d__);
			return <MaxAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}
	}
}
