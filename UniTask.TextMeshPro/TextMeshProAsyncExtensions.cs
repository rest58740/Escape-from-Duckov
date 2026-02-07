using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;
using TMPro;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000003 RID: 3
	public static class TextMeshProAsyncExtensions
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020BC File Offset: 0x000002BC
		public static void BindTo(this IUniTaskAsyncEnumerable<string> source, TMP_Text text, bool rebindOnError = true)
		{
			TextMeshProAsyncExtensions.BindToCore(source, text, text.GetCancellationTokenOnDestroy(), rebindOnError).Forget();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020E0 File Offset: 0x000002E0
		public static void BindTo(this IUniTaskAsyncEnumerable<string> source, TMP_Text text, CancellationToken cancellationToken, bool rebindOnError = true)
		{
			TextMeshProAsyncExtensions.BindToCore(source, text, cancellationToken, rebindOnError).Forget();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002100 File Offset: 0x00000300
		private static UniTaskVoid BindToCore(IUniTaskAsyncEnumerable<string> source, TMP_Text text, CancellationToken cancellationToken, bool rebindOnError)
		{
			TextMeshProAsyncExtensions.<BindToCore>d__2 <BindToCore>d__;
			<BindToCore>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<BindToCore>d__.source = source;
			<BindToCore>d__.text = text;
			<BindToCore>d__.cancellationToken = cancellationToken;
			<BindToCore>d__.rebindOnError = rebindOnError;
			<BindToCore>d__.<>1__state = -1;
			<BindToCore>d__.<>t__builder.Start<TextMeshProAsyncExtensions.<BindToCore>d__2>(ref <BindToCore>d__);
			return <BindToCore>d__.<>t__builder.Task;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000215C File Offset: 0x0000035C
		public static void BindTo<T>(this IUniTaskAsyncEnumerable<T> source, TMP_Text text, bool rebindOnError = true)
		{
			TextMeshProAsyncExtensions.BindToCore<T>(source, text, text.GetCancellationTokenOnDestroy(), rebindOnError).Forget();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002180 File Offset: 0x00000380
		public static void BindTo<T>(this IUniTaskAsyncEnumerable<T> source, TMP_Text text, CancellationToken cancellationToken, bool rebindOnError = true)
		{
			TextMeshProAsyncExtensions.BindToCore<T>(source, text, cancellationToken, rebindOnError).Forget();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021A0 File Offset: 0x000003A0
		public static void BindTo<T>(this AsyncReactiveProperty<T> source, TMP_Text text, bool rebindOnError = true)
		{
			TextMeshProAsyncExtensions.BindToCore<T>(source, text, text.GetCancellationTokenOnDestroy(), rebindOnError).Forget();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021C4 File Offset: 0x000003C4
		private static UniTaskVoid BindToCore<T>(IUniTaskAsyncEnumerable<T> source, TMP_Text text, CancellationToken cancellationToken, bool rebindOnError)
		{
			TextMeshProAsyncExtensions.<BindToCore>d__6<T> <BindToCore>d__;
			<BindToCore>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<BindToCore>d__.source = source;
			<BindToCore>d__.text = text;
			<BindToCore>d__.cancellationToken = cancellationToken;
			<BindToCore>d__.rebindOnError = rebindOnError;
			<BindToCore>d__.<>1__state = -1;
			<BindToCore>d__.<>t__builder.Start<TextMeshProAsyncExtensions.<BindToCore>d__6<T>>(ref <BindToCore>d__);
			return <BindToCore>d__.<>t__builder.Task;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000221F File Offset: 0x0000041F
		public static IAsyncValueChangedEventHandler<string> GetAsyncValueChangedEventHandler(this TMP_InputField inputField)
		{
			return new AsyncUnityEventHandler<string>(inputField.onValueChanged, inputField.GetCancellationTokenOnDestroy(), false);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002233 File Offset: 0x00000433
		public static IAsyncValueChangedEventHandler<string> GetAsyncValueChangedEventHandler(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<string>(inputField.onValueChanged, cancellationToken, false);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002242 File Offset: 0x00000442
		public static UniTask<string> OnValueChangedAsync(this TMP_InputField inputField)
		{
			return new AsyncUnityEventHandler<string>(inputField.onValueChanged, inputField.GetCancellationTokenOnDestroy(), true).OnInvokeAsync();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000225B File Offset: 0x0000045B
		public static UniTask<string> OnValueChangedAsync(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<string>(inputField.onValueChanged, cancellationToken, true).OnInvokeAsync();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000226F File Offset: 0x0000046F
		public static IUniTaskAsyncEnumerable<string> OnValueChangedAsAsyncEnumerable(this TMP_InputField inputField)
		{
			return new UnityEventHandlerAsyncEnumerable<string>(inputField.onValueChanged, inputField.GetCancellationTokenOnDestroy());
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002282 File Offset: 0x00000482
		public static IUniTaskAsyncEnumerable<string> OnValueChangedAsAsyncEnumerable(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new UnityEventHandlerAsyncEnumerable<string>(inputField.onValueChanged, cancellationToken);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002290 File Offset: 0x00000490
		public static IAsyncEndEditEventHandler<string> GetAsyncEndEditEventHandler(this TMP_InputField inputField)
		{
			return new AsyncUnityEventHandler<string>(inputField.onEndEdit, inputField.GetCancellationTokenOnDestroy(), false);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000022A4 File Offset: 0x000004A4
		public static IAsyncEndEditEventHandler<string> GetAsyncEndEditEventHandler(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<string>(inputField.onEndEdit, cancellationToken, false);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022B3 File Offset: 0x000004B3
		public static UniTask<string> OnEndEditAsync(this TMP_InputField inputField)
		{
			return new AsyncUnityEventHandler<string>(inputField.onEndEdit, inputField.GetCancellationTokenOnDestroy(), true).OnInvokeAsync();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000022CC File Offset: 0x000004CC
		public static UniTask<string> OnEndEditAsync(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<string>(inputField.onEndEdit, cancellationToken, true).OnInvokeAsync();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000022E0 File Offset: 0x000004E0
		public static IUniTaskAsyncEnumerable<string> OnEndEditAsAsyncEnumerable(this TMP_InputField inputField)
		{
			return new UnityEventHandlerAsyncEnumerable<string>(inputField.onEndEdit, inputField.GetCancellationTokenOnDestroy());
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022F3 File Offset: 0x000004F3
		public static IUniTaskAsyncEnumerable<string> OnEndEditAsAsyncEnumerable(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new UnityEventHandlerAsyncEnumerable<string>(inputField.onEndEdit, cancellationToken);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002301 File Offset: 0x00000501
		public static IAsyncEndTextSelectionEventHandler<ValueTuple<string, int, int>> GetAsyncEndTextSelectionEventHandler(this TMP_InputField inputField)
		{
			return new AsyncUnityEventHandler<ValueTuple<string, int, int>>(new TextSelectionEventConverter(inputField.onEndTextSelection), inputField.GetCancellationTokenOnDestroy(), false);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000231A File Offset: 0x0000051A
		public static IAsyncEndTextSelectionEventHandler<ValueTuple<string, int, int>> GetAsyncEndTextSelectionEventHandler(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<ValueTuple<string, int, int>>(new TextSelectionEventConverter(inputField.onEndTextSelection), cancellationToken, false);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000232E File Offset: 0x0000052E
		public static UniTask<ValueTuple<string, int, int>> OnEndTextSelectionAsync(this TMP_InputField inputField)
		{
			return new AsyncUnityEventHandler<ValueTuple<string, int, int>>(new TextSelectionEventConverter(inputField.onEndTextSelection), inputField.GetCancellationTokenOnDestroy(), true).OnInvokeAsync();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000234C File Offset: 0x0000054C
		public static UniTask<ValueTuple<string, int, int>> OnEndTextSelectionAsync(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<ValueTuple<string, int, int>>(new TextSelectionEventConverter(inputField.onEndTextSelection), cancellationToken, true).OnInvokeAsync();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002365 File Offset: 0x00000565
		public static IUniTaskAsyncEnumerable<ValueTuple<string, int, int>> OnEndTextSelectionAsAsyncEnumerable(this TMP_InputField inputField)
		{
			return new UnityEventHandlerAsyncEnumerable<ValueTuple<string, int, int>>(new TextSelectionEventConverter(inputField.onEndTextSelection), inputField.GetCancellationTokenOnDestroy());
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000237D File Offset: 0x0000057D
		public static IUniTaskAsyncEnumerable<ValueTuple<string, int, int>> OnEndTextSelectionAsAsyncEnumerable(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new UnityEventHandlerAsyncEnumerable<ValueTuple<string, int, int>>(new TextSelectionEventConverter(inputField.onEndTextSelection), cancellationToken);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002390 File Offset: 0x00000590
		public static IAsyncTextSelectionEventHandler<ValueTuple<string, int, int>> GetAsyncTextSelectionEventHandler(this TMP_InputField inputField)
		{
			return new AsyncUnityEventHandler<ValueTuple<string, int, int>>(new TextSelectionEventConverter(inputField.onTextSelection), inputField.GetCancellationTokenOnDestroy(), false);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000023A9 File Offset: 0x000005A9
		public static IAsyncTextSelectionEventHandler<ValueTuple<string, int, int>> GetAsyncTextSelectionEventHandler(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<ValueTuple<string, int, int>>(new TextSelectionEventConverter(inputField.onTextSelection), cancellationToken, false);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000023BD File Offset: 0x000005BD
		public static UniTask<ValueTuple<string, int, int>> OnTextSelectionAsync(this TMP_InputField inputField)
		{
			return new AsyncUnityEventHandler<ValueTuple<string, int, int>>(new TextSelectionEventConverter(inputField.onTextSelection), inputField.GetCancellationTokenOnDestroy(), true).OnInvokeAsync();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000023DB File Offset: 0x000005DB
		public static UniTask<ValueTuple<string, int, int>> OnTextSelectionAsync(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<ValueTuple<string, int, int>>(new TextSelectionEventConverter(inputField.onTextSelection), cancellationToken, true).OnInvokeAsync();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000023F4 File Offset: 0x000005F4
		public static IUniTaskAsyncEnumerable<ValueTuple<string, int, int>> OnTextSelectionAsAsyncEnumerable(this TMP_InputField inputField)
		{
			return new UnityEventHandlerAsyncEnumerable<ValueTuple<string, int, int>>(new TextSelectionEventConverter(inputField.onTextSelection), inputField.GetCancellationTokenOnDestroy());
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000240C File Offset: 0x0000060C
		public static IUniTaskAsyncEnumerable<ValueTuple<string, int, int>> OnTextSelectionAsAsyncEnumerable(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new UnityEventHandlerAsyncEnumerable<ValueTuple<string, int, int>>(new TextSelectionEventConverter(inputField.onTextSelection), cancellationToken);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000241F File Offset: 0x0000061F
		public static IAsyncDeselectEventHandler<string> GetAsyncDeselectEventHandler(this TMP_InputField inputField)
		{
			return new AsyncUnityEventHandler<string>(inputField.onDeselect, inputField.GetCancellationTokenOnDestroy(), false);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002433 File Offset: 0x00000633
		public static IAsyncDeselectEventHandler<string> GetAsyncDeselectEventHandler(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<string>(inputField.onDeselect, cancellationToken, false);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002442 File Offset: 0x00000642
		public static UniTask<string> OnDeselectAsync(this TMP_InputField inputField)
		{
			return new AsyncUnityEventHandler<string>(inputField.onDeselect, inputField.GetCancellationTokenOnDestroy(), true).OnInvokeAsync();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000245B File Offset: 0x0000065B
		public static UniTask<string> OnDeselectAsync(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<string>(inputField.onDeselect, cancellationToken, true).OnInvokeAsync();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000246F File Offset: 0x0000066F
		public static IUniTaskAsyncEnumerable<string> OnDeselectAsAsyncEnumerable(this TMP_InputField inputField)
		{
			return new UnityEventHandlerAsyncEnumerable<string>(inputField.onDeselect, inputField.GetCancellationTokenOnDestroy());
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002482 File Offset: 0x00000682
		public static IUniTaskAsyncEnumerable<string> OnDeselectAsAsyncEnumerable(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new UnityEventHandlerAsyncEnumerable<string>(inputField.onDeselect, cancellationToken);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002490 File Offset: 0x00000690
		public static IAsyncSelectEventHandler<string> GetAsyncSelectEventHandler(this TMP_InputField inputField)
		{
			return new AsyncUnityEventHandler<string>(inputField.onSelect, inputField.GetCancellationTokenOnDestroy(), false);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000024A4 File Offset: 0x000006A4
		public static IAsyncSelectEventHandler<string> GetAsyncSelectEventHandler(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<string>(inputField.onSelect, cancellationToken, false);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000024B3 File Offset: 0x000006B3
		public static UniTask<string> OnSelectAsync(this TMP_InputField inputField)
		{
			return new AsyncUnityEventHandler<string>(inputField.onSelect, inputField.GetCancellationTokenOnDestroy(), true).OnInvokeAsync();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000024CC File Offset: 0x000006CC
		public static UniTask<string> OnSelectAsync(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<string>(inputField.onSelect, cancellationToken, true).OnInvokeAsync();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000024E0 File Offset: 0x000006E0
		public static IUniTaskAsyncEnumerable<string> OnSelectAsAsyncEnumerable(this TMP_InputField inputField)
		{
			return new UnityEventHandlerAsyncEnumerable<string>(inputField.onSelect, inputField.GetCancellationTokenOnDestroy());
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000024F3 File Offset: 0x000006F3
		public static IUniTaskAsyncEnumerable<string> OnSelectAsAsyncEnumerable(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new UnityEventHandlerAsyncEnumerable<string>(inputField.onSelect, cancellationToken);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002501 File Offset: 0x00000701
		public static IAsyncSubmitEventHandler<string> GetAsyncSubmitEventHandler(this TMP_InputField inputField)
		{
			return new AsyncUnityEventHandler<string>(inputField.onSubmit, inputField.GetCancellationTokenOnDestroy(), false);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002515 File Offset: 0x00000715
		public static IAsyncSubmitEventHandler<string> GetAsyncSubmitEventHandler(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<string>(inputField.onSubmit, cancellationToken, false);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002524 File Offset: 0x00000724
		public static UniTask<string> OnSubmitAsync(this TMP_InputField inputField)
		{
			return new AsyncUnityEventHandler<string>(inputField.onSubmit, inputField.GetCancellationTokenOnDestroy(), true).OnInvokeAsync();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000253D File Offset: 0x0000073D
		public static UniTask<string> OnSubmitAsync(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<string>(inputField.onSubmit, cancellationToken, true).OnInvokeAsync();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002551 File Offset: 0x00000751
		public static IUniTaskAsyncEnumerable<string> OnSubmitAsAsyncEnumerable(this TMP_InputField inputField)
		{
			return new UnityEventHandlerAsyncEnumerable<string>(inputField.onSubmit, inputField.GetCancellationTokenOnDestroy());
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002564 File Offset: 0x00000764
		public static IUniTaskAsyncEnumerable<string> OnSubmitAsAsyncEnumerable(this TMP_InputField inputField, CancellationToken cancellationToken)
		{
			return new UnityEventHandlerAsyncEnumerable<string>(inputField.onSubmit, cancellationToken);
		}
	}
}
