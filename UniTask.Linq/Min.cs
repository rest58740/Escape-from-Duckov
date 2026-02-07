using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000041 RID: 65
	internal static class Min
	{
		// Token: 0x060002D5 RID: 725 RVA: 0x0000AB6C File Offset: 0x00008D6C
		public static UniTask<TSource> MinAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__0<TSource> <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TSource>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__0<TSource>>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000ABB8 File Offset: 0x00008DB8
		public static UniTask<TResult> MinAsync<TSource, TResult>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TResult> selector, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__1<TSource, TResult> <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TResult>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.selector = selector;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__1<TSource, TResult>>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000AC0C File Offset: 0x00008E0C
		public static UniTask<TResult> MinAwaitAsync<TSource, TResult>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TResult>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitAsync>d__2<TSource, TResult> <MinAwaitAsync>d__;
			<MinAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TResult>.Create();
			<MinAwaitAsync>d__.source = source;
			<MinAwaitAsync>d__.selector = selector;
			<MinAwaitAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitAsync>d__.<>1__state = -1;
			<MinAwaitAsync>d__.<>t__builder.Start<Min.<MinAwaitAsync>d__2<TSource, TResult>>(ref <MinAwaitAsync>d__);
			return <MinAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000AC60 File Offset: 0x00008E60
		public static UniTask<TResult> MinAwaitWithCancellationAsync<TSource, TResult>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TResult>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitWithCancellationAsync>d__3<TSource, TResult> <MinAwaitWithCancellationAsync>d__;
			<MinAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TResult>.Create();
			<MinAwaitWithCancellationAsync>d__.source = source;
			<MinAwaitWithCancellationAsync>d__.selector = selector;
			<MinAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MinAwaitWithCancellationAsync>d__.<>t__builder.Start<Min.<MinAwaitWithCancellationAsync>d__3<TSource, TResult>>(ref <MinAwaitWithCancellationAsync>d__);
			return <MinAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000ACB4 File Offset: 0x00008EB4
		public static UniTask<int> MinAsync(IUniTaskAsyncEnumerable<int> source, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__4 <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__4>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000AD00 File Offset: 0x00008F00
		public static UniTask<int> MinAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__5<TSource> <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.selector = selector;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__5<TSource>>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000AD54 File Offset: 0x00008F54
		public static UniTask<int> MinAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<int>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitAsync>d__6<TSource> <MinAwaitAsync>d__;
			<MinAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<MinAwaitAsync>d__.source = source;
			<MinAwaitAsync>d__.selector = selector;
			<MinAwaitAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitAsync>d__.<>1__state = -1;
			<MinAwaitAsync>d__.<>t__builder.Start<Min.<MinAwaitAsync>d__6<TSource>>(ref <MinAwaitAsync>d__);
			return <MinAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000ADA8 File Offset: 0x00008FA8
		public static UniTask<int> MinAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<int>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitWithCancellationAsync>d__7<TSource> <MinAwaitWithCancellationAsync>d__;
			<MinAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<MinAwaitWithCancellationAsync>d__.source = source;
			<MinAwaitWithCancellationAsync>d__.selector = selector;
			<MinAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MinAwaitWithCancellationAsync>d__.<>t__builder.Start<Min.<MinAwaitWithCancellationAsync>d__7<TSource>>(ref <MinAwaitWithCancellationAsync>d__);
			return <MinAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000ADFC File Offset: 0x00008FFC
		public static UniTask<long> MinAsync(IUniTaskAsyncEnumerable<long> source, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__8 <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__8>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000AE48 File Offset: 0x00009048
		public static UniTask<long> MinAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__9<TSource> <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.selector = selector;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__9<TSource>>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000AE9C File Offset: 0x0000909C
		public static UniTask<long> MinAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<long>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitAsync>d__10<TSource> <MinAwaitAsync>d__;
			<MinAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long>.Create();
			<MinAwaitAsync>d__.source = source;
			<MinAwaitAsync>d__.selector = selector;
			<MinAwaitAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitAsync>d__.<>1__state = -1;
			<MinAwaitAsync>d__.<>t__builder.Start<Min.<MinAwaitAsync>d__10<TSource>>(ref <MinAwaitAsync>d__);
			return <MinAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000AEF0 File Offset: 0x000090F0
		public static UniTask<long> MinAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<long>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitWithCancellationAsync>d__11<TSource> <MinAwaitWithCancellationAsync>d__;
			<MinAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long>.Create();
			<MinAwaitWithCancellationAsync>d__.source = source;
			<MinAwaitWithCancellationAsync>d__.selector = selector;
			<MinAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MinAwaitWithCancellationAsync>d__.<>t__builder.Start<Min.<MinAwaitWithCancellationAsync>d__11<TSource>>(ref <MinAwaitWithCancellationAsync>d__);
			return <MinAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000AF44 File Offset: 0x00009144
		public static UniTask<float> MinAsync(IUniTaskAsyncEnumerable<float> source, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__12 <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__12>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000AF90 File Offset: 0x00009190
		public static UniTask<float> MinAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__13<TSource> <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.selector = selector;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__13<TSource>>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000AFE4 File Offset: 0x000091E4
		public static UniTask<float> MinAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<float>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitAsync>d__14<TSource> <MinAwaitAsync>d__;
			<MinAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float>.Create();
			<MinAwaitAsync>d__.source = source;
			<MinAwaitAsync>d__.selector = selector;
			<MinAwaitAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitAsync>d__.<>1__state = -1;
			<MinAwaitAsync>d__.<>t__builder.Start<Min.<MinAwaitAsync>d__14<TSource>>(ref <MinAwaitAsync>d__);
			return <MinAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000B038 File Offset: 0x00009238
		public static UniTask<float> MinAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<float>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitWithCancellationAsync>d__15<TSource> <MinAwaitWithCancellationAsync>d__;
			<MinAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float>.Create();
			<MinAwaitWithCancellationAsync>d__.source = source;
			<MinAwaitWithCancellationAsync>d__.selector = selector;
			<MinAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MinAwaitWithCancellationAsync>d__.<>t__builder.Start<Min.<MinAwaitWithCancellationAsync>d__15<TSource>>(ref <MinAwaitWithCancellationAsync>d__);
			return <MinAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000B08C File Offset: 0x0000928C
		public static UniTask<double> MinAsync(IUniTaskAsyncEnumerable<double> source, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__16 <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__16>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000B0D8 File Offset: 0x000092D8
		public static UniTask<double> MinAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__17<TSource> <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.selector = selector;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__17<TSource>>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000B12C File Offset: 0x0000932C
		public static UniTask<double> MinAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<double>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitAsync>d__18<TSource> <MinAwaitAsync>d__;
			<MinAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<MinAwaitAsync>d__.source = source;
			<MinAwaitAsync>d__.selector = selector;
			<MinAwaitAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitAsync>d__.<>1__state = -1;
			<MinAwaitAsync>d__.<>t__builder.Start<Min.<MinAwaitAsync>d__18<TSource>>(ref <MinAwaitAsync>d__);
			return <MinAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000B180 File Offset: 0x00009380
		public static UniTask<double> MinAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<double>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitWithCancellationAsync>d__19<TSource> <MinAwaitWithCancellationAsync>d__;
			<MinAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double>.Create();
			<MinAwaitWithCancellationAsync>d__.source = source;
			<MinAwaitWithCancellationAsync>d__.selector = selector;
			<MinAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MinAwaitWithCancellationAsync>d__.<>t__builder.Start<Min.<MinAwaitWithCancellationAsync>d__19<TSource>>(ref <MinAwaitWithCancellationAsync>d__);
			return <MinAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000B1D4 File Offset: 0x000093D4
		public static UniTask<decimal> MinAsync(IUniTaskAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__20 <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__20>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000B220 File Offset: 0x00009420
		public static UniTask<decimal> MinAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__21<TSource> <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.selector = selector;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__21<TSource>>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000B274 File Offset: 0x00009474
		public static UniTask<decimal> MinAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<decimal>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitAsync>d__22<TSource> <MinAwaitAsync>d__;
			<MinAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal>.Create();
			<MinAwaitAsync>d__.source = source;
			<MinAwaitAsync>d__.selector = selector;
			<MinAwaitAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitAsync>d__.<>1__state = -1;
			<MinAwaitAsync>d__.<>t__builder.Start<Min.<MinAwaitAsync>d__22<TSource>>(ref <MinAwaitAsync>d__);
			return <MinAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000B2C8 File Offset: 0x000094C8
		public static UniTask<decimal> MinAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<decimal>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitWithCancellationAsync>d__23<TSource> <MinAwaitWithCancellationAsync>d__;
			<MinAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal>.Create();
			<MinAwaitWithCancellationAsync>d__.source = source;
			<MinAwaitWithCancellationAsync>d__.selector = selector;
			<MinAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MinAwaitWithCancellationAsync>d__.<>t__builder.Start<Min.<MinAwaitWithCancellationAsync>d__23<TSource>>(ref <MinAwaitWithCancellationAsync>d__);
			return <MinAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000B31C File Offset: 0x0000951C
		public static UniTask<int?> MinAsync(IUniTaskAsyncEnumerable<int?> source, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__24 <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int?>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__24>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000B368 File Offset: 0x00009568
		public static UniTask<int?> MinAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__25<TSource> <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int?>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.selector = selector;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__25<TSource>>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000B3BC File Offset: 0x000095BC
		public static UniTask<int?> MinAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<int?>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitAsync>d__26<TSource> <MinAwaitAsync>d__;
			<MinAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int?>.Create();
			<MinAwaitAsync>d__.source = source;
			<MinAwaitAsync>d__.selector = selector;
			<MinAwaitAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitAsync>d__.<>1__state = -1;
			<MinAwaitAsync>d__.<>t__builder.Start<Min.<MinAwaitAsync>d__26<TSource>>(ref <MinAwaitAsync>d__);
			return <MinAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000B410 File Offset: 0x00009610
		public static UniTask<int?> MinAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<int?>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitWithCancellationAsync>d__27<TSource> <MinAwaitWithCancellationAsync>d__;
			<MinAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<int?>.Create();
			<MinAwaitWithCancellationAsync>d__.source = source;
			<MinAwaitWithCancellationAsync>d__.selector = selector;
			<MinAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MinAwaitWithCancellationAsync>d__.<>t__builder.Start<Min.<MinAwaitWithCancellationAsync>d__27<TSource>>(ref <MinAwaitWithCancellationAsync>d__);
			return <MinAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000B464 File Offset: 0x00009664
		public static UniTask<long?> MinAsync(IUniTaskAsyncEnumerable<long?> source, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__28 <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long?>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__28>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000B4B0 File Offset: 0x000096B0
		public static UniTask<long?> MinAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__29<TSource> <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long?>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.selector = selector;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__29<TSource>>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000B504 File Offset: 0x00009704
		public static UniTask<long?> MinAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<long?>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitAsync>d__30<TSource> <MinAwaitAsync>d__;
			<MinAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long?>.Create();
			<MinAwaitAsync>d__.source = source;
			<MinAwaitAsync>d__.selector = selector;
			<MinAwaitAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitAsync>d__.<>1__state = -1;
			<MinAwaitAsync>d__.<>t__builder.Start<Min.<MinAwaitAsync>d__30<TSource>>(ref <MinAwaitAsync>d__);
			return <MinAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000B558 File Offset: 0x00009758
		public static UniTask<long?> MinAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<long?>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitWithCancellationAsync>d__31<TSource> <MinAwaitWithCancellationAsync>d__;
			<MinAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long?>.Create();
			<MinAwaitWithCancellationAsync>d__.source = source;
			<MinAwaitWithCancellationAsync>d__.selector = selector;
			<MinAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MinAwaitWithCancellationAsync>d__.<>t__builder.Start<Min.<MinAwaitWithCancellationAsync>d__31<TSource>>(ref <MinAwaitWithCancellationAsync>d__);
			return <MinAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000B5AC File Offset: 0x000097AC
		public static UniTask<float?> MinAsync(IUniTaskAsyncEnumerable<float?> source, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__32 <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float?>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__32>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000B5F8 File Offset: 0x000097F8
		public static UniTask<float?> MinAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__33<TSource> <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float?>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.selector = selector;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__33<TSource>>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000B64C File Offset: 0x0000984C
		public static UniTask<float?> MinAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<float?>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitAsync>d__34<TSource> <MinAwaitAsync>d__;
			<MinAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float?>.Create();
			<MinAwaitAsync>d__.source = source;
			<MinAwaitAsync>d__.selector = selector;
			<MinAwaitAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitAsync>d__.<>1__state = -1;
			<MinAwaitAsync>d__.<>t__builder.Start<Min.<MinAwaitAsync>d__34<TSource>>(ref <MinAwaitAsync>d__);
			return <MinAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000B6A0 File Offset: 0x000098A0
		public static UniTask<float?> MinAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<float?>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitWithCancellationAsync>d__35<TSource> <MinAwaitWithCancellationAsync>d__;
			<MinAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<float?>.Create();
			<MinAwaitWithCancellationAsync>d__.source = source;
			<MinAwaitWithCancellationAsync>d__.selector = selector;
			<MinAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MinAwaitWithCancellationAsync>d__.<>t__builder.Start<Min.<MinAwaitWithCancellationAsync>d__35<TSource>>(ref <MinAwaitWithCancellationAsync>d__);
			return <MinAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000B6F4 File Offset: 0x000098F4
		public static UniTask<double?> MinAsync(IUniTaskAsyncEnumerable<double?> source, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__36 <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__36>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000B740 File Offset: 0x00009940
		public static UniTask<double?> MinAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__37<TSource> <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.selector = selector;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__37<TSource>>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000B794 File Offset: 0x00009994
		public static UniTask<double?> MinAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<double?>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitAsync>d__38<TSource> <MinAwaitAsync>d__;
			<MinAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<MinAwaitAsync>d__.source = source;
			<MinAwaitAsync>d__.selector = selector;
			<MinAwaitAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitAsync>d__.<>1__state = -1;
			<MinAwaitAsync>d__.<>t__builder.Start<Min.<MinAwaitAsync>d__38<TSource>>(ref <MinAwaitAsync>d__);
			return <MinAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000B7E8 File Offset: 0x000099E8
		public static UniTask<double?> MinAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<double?>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitWithCancellationAsync>d__39<TSource> <MinAwaitWithCancellationAsync>d__;
			<MinAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<double?>.Create();
			<MinAwaitWithCancellationAsync>d__.source = source;
			<MinAwaitWithCancellationAsync>d__.selector = selector;
			<MinAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MinAwaitWithCancellationAsync>d__.<>t__builder.Start<Min.<MinAwaitWithCancellationAsync>d__39<TSource>>(ref <MinAwaitWithCancellationAsync>d__);
			return <MinAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000B83C File Offset: 0x00009A3C
		public static UniTask<decimal?> MinAsync(IUniTaskAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__40 <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal?>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__40>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000B888 File Offset: 0x00009A88
		public static UniTask<decimal?> MinAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
		{
			Min.<MinAsync>d__41<TSource> <MinAsync>d__;
			<MinAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal?>.Create();
			<MinAsync>d__.source = source;
			<MinAsync>d__.selector = selector;
			<MinAsync>d__.cancellationToken = cancellationToken;
			<MinAsync>d__.<>1__state = -1;
			<MinAsync>d__.<>t__builder.Start<Min.<MinAsync>d__41<TSource>>(ref <MinAsync>d__);
			return <MinAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000B8DC File Offset: 0x00009ADC
		public static UniTask<decimal?> MinAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<decimal?>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitAsync>d__42<TSource> <MinAwaitAsync>d__;
			<MinAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal?>.Create();
			<MinAwaitAsync>d__.source = source;
			<MinAwaitAsync>d__.selector = selector;
			<MinAwaitAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitAsync>d__.<>1__state = -1;
			<MinAwaitAsync>d__.<>t__builder.Start<Min.<MinAwaitAsync>d__42<TSource>>(ref <MinAwaitAsync>d__);
			return <MinAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000B930 File Offset: 0x00009B30
		public static UniTask<decimal?> MinAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<decimal?>> selector, CancellationToken cancellationToken)
		{
			Min.<MinAwaitWithCancellationAsync>d__43<TSource> <MinAwaitWithCancellationAsync>d__;
			<MinAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<decimal?>.Create();
			<MinAwaitWithCancellationAsync>d__.source = source;
			<MinAwaitWithCancellationAsync>d__.selector = selector;
			<MinAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<MinAwaitWithCancellationAsync>d__.<>1__state = -1;
			<MinAwaitWithCancellationAsync>d__.<>t__builder.Start<Min.<MinAwaitWithCancellationAsync>d__43<TSource>>(ref <MinAwaitWithCancellationAsync>d__);
			return <MinAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}
	}
}
