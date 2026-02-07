using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000069 RID: 105
	public static class UnityBindingExtensions
	{
		// Token: 0x060002C8 RID: 712 RVA: 0x0000A418 File Offset: 0x00008618
		public static void BindTo(this IUniTaskAsyncEnumerable<string> source, Text text, bool rebindOnError = true)
		{
			UnityBindingExtensions.BindToCore(source, text, text.GetCancellationTokenOnDestroy(), rebindOnError).Forget();
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000A43C File Offset: 0x0000863C
		public static void BindTo(this IUniTaskAsyncEnumerable<string> source, Text text, CancellationToken cancellationToken, bool rebindOnError = true)
		{
			UnityBindingExtensions.BindToCore(source, text, cancellationToken, rebindOnError).Forget();
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000A45C File Offset: 0x0000865C
		private static UniTaskVoid BindToCore(IUniTaskAsyncEnumerable<string> source, Text text, CancellationToken cancellationToken, bool rebindOnError)
		{
			UnityBindingExtensions.<BindToCore>d__2 <BindToCore>d__;
			<BindToCore>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<BindToCore>d__.source = source;
			<BindToCore>d__.text = text;
			<BindToCore>d__.cancellationToken = cancellationToken;
			<BindToCore>d__.rebindOnError = rebindOnError;
			<BindToCore>d__.<>1__state = -1;
			<BindToCore>d__.<>t__builder.Start<UnityBindingExtensions.<BindToCore>d__2>(ref <BindToCore>d__);
			return <BindToCore>d__.<>t__builder.Task;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000A4B8 File Offset: 0x000086B8
		public static void BindTo<T>(this IUniTaskAsyncEnumerable<T> source, Text text, bool rebindOnError = true)
		{
			UnityBindingExtensions.BindToCore<T>(source, text, text.GetCancellationTokenOnDestroy(), rebindOnError).Forget();
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000A4DC File Offset: 0x000086DC
		public static void BindTo<T>(this IUniTaskAsyncEnumerable<T> source, Text text, CancellationToken cancellationToken, bool rebindOnError = true)
		{
			UnityBindingExtensions.BindToCore<T>(source, text, cancellationToken, rebindOnError).Forget();
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000A4FC File Offset: 0x000086FC
		public static void BindTo<T>(this AsyncReactiveProperty<T> source, Text text, bool rebindOnError = true)
		{
			UnityBindingExtensions.BindToCore<T>(source, text, text.GetCancellationTokenOnDestroy(), rebindOnError).Forget();
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000A520 File Offset: 0x00008720
		private static UniTaskVoid BindToCore<T>(IUniTaskAsyncEnumerable<T> source, Text text, CancellationToken cancellationToken, bool rebindOnError)
		{
			UnityBindingExtensions.<BindToCore>d__6<T> <BindToCore>d__;
			<BindToCore>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<BindToCore>d__.source = source;
			<BindToCore>d__.text = text;
			<BindToCore>d__.cancellationToken = cancellationToken;
			<BindToCore>d__.rebindOnError = rebindOnError;
			<BindToCore>d__.<>1__state = -1;
			<BindToCore>d__.<>t__builder.Start<UnityBindingExtensions.<BindToCore>d__6<T>>(ref <BindToCore>d__);
			return <BindToCore>d__.<>t__builder.Task;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000A57C File Offset: 0x0000877C
		public static void BindTo(this IUniTaskAsyncEnumerable<bool> source, Selectable selectable, bool rebindOnError = true)
		{
			UnityBindingExtensions.BindToCore(source, selectable, selectable.GetCancellationTokenOnDestroy(), rebindOnError).Forget();
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000A5A0 File Offset: 0x000087A0
		public static void BindTo(this IUniTaskAsyncEnumerable<bool> source, Selectable selectable, CancellationToken cancellationToken, bool rebindOnError = true)
		{
			UnityBindingExtensions.BindToCore(source, selectable, cancellationToken, rebindOnError).Forget();
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000A5C0 File Offset: 0x000087C0
		private static UniTaskVoid BindToCore(IUniTaskAsyncEnumerable<bool> source, Selectable selectable, CancellationToken cancellationToken, bool rebindOnError)
		{
			UnityBindingExtensions.<BindToCore>d__9 <BindToCore>d__;
			<BindToCore>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<BindToCore>d__.source = source;
			<BindToCore>d__.selectable = selectable;
			<BindToCore>d__.cancellationToken = cancellationToken;
			<BindToCore>d__.rebindOnError = rebindOnError;
			<BindToCore>d__.<>1__state = -1;
			<BindToCore>d__.<>t__builder.Start<UnityBindingExtensions.<BindToCore>d__9>(ref <BindToCore>d__);
			return <BindToCore>d__.<>t__builder.Task;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000A61C File Offset: 0x0000881C
		public static void BindTo<TSource, TObject>(this IUniTaskAsyncEnumerable<TSource> source, TObject monoBehaviour, Action<TObject, TSource> bindAction, bool rebindOnError = true) where TObject : MonoBehaviour
		{
			UnityBindingExtensions.BindToCore<TSource, TObject>(source, monoBehaviour, bindAction, monoBehaviour.GetCancellationTokenOnDestroy(), rebindOnError).Forget();
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000A648 File Offset: 0x00008848
		public static void BindTo<TSource, TObject>(this IUniTaskAsyncEnumerable<TSource> source, TObject bindTarget, Action<TObject, TSource> bindAction, CancellationToken cancellationToken, bool rebindOnError = true)
		{
			UnityBindingExtensions.BindToCore<TSource, TObject>(source, bindTarget, bindAction, cancellationToken, rebindOnError).Forget();
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000A668 File Offset: 0x00008868
		private static UniTaskVoid BindToCore<TSource, TObject>(IUniTaskAsyncEnumerable<TSource> source, TObject bindTarget, Action<TObject, TSource> bindAction, CancellationToken cancellationToken, bool rebindOnError)
		{
			UnityBindingExtensions.<BindToCore>d__12<TSource, TObject> <BindToCore>d__;
			<BindToCore>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<BindToCore>d__.source = source;
			<BindToCore>d__.bindTarget = bindTarget;
			<BindToCore>d__.bindAction = bindAction;
			<BindToCore>d__.cancellationToken = cancellationToken;
			<BindToCore>d__.rebindOnError = rebindOnError;
			<BindToCore>d__.<>1__state = -1;
			<BindToCore>d__.<>t__builder.Start<UnityBindingExtensions.<BindToCore>d__12<TSource, TObject>>(ref <BindToCore>d__);
			return <BindToCore>d__.<>t__builder.Task;
		}
	}
}
