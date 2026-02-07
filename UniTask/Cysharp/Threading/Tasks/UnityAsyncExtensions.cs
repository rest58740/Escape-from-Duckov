using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks.Sources;
using Cysharp.Threading.Tasks.CompilerServices;
using Cysharp.Threading.Tasks.Internal;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200005A RID: 90
	public static class UnityAsyncExtensions
	{
		// Token: 0x06000240 RID: 576 RVA: 0x000096A9 File Offset: 0x000078A9
		public static UnityAsyncExtensions.AssetBundleRequestAllAssetsAwaiter AwaitForAllAssets(this AssetBundleRequest asyncOperation)
		{
			Error.ThrowArgumentNullException<AssetBundleRequest>(asyncOperation, "asyncOperation");
			return new UnityAsyncExtensions.AssetBundleRequestAllAssetsAwaiter(asyncOperation);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x000096BC File Offset: 0x000078BC
		public static UniTask<Object[]> AwaitForAllAssets(this AssetBundleRequest asyncOperation, CancellationToken cancellationToken)
		{
			return asyncOperation.AwaitForAllAssets(null, PlayerLoopTiming.Update, cancellationToken, false);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x000096C8 File Offset: 0x000078C8
		public static UniTask<Object[]> AwaitForAllAssets(this AssetBundleRequest asyncOperation, CancellationToken cancellationToken, bool cancelImmediately)
		{
			return asyncOperation.AwaitForAllAssets(null, PlayerLoopTiming.Update, cancellationToken, cancelImmediately);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x000096D4 File Offset: 0x000078D4
		public static UniTask<Object[]> AwaitForAllAssets(this AssetBundleRequest asyncOperation, IProgress<float> progress = null, PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			Error.ThrowArgumentNullException<AssetBundleRequest>(asyncOperation, "asyncOperation");
			if (cancellationToken.IsCancellationRequested)
			{
				return UniTask.FromCanceled<Object[]>(cancellationToken);
			}
			if (asyncOperation.isDone)
			{
				return UniTask.FromResult<Object[]>(asyncOperation.allAssets);
			}
			short token;
			return new UniTask<Object[]>(UnityAsyncExtensions.AssetBundleRequestAllAssetsConfiguredSource.Create(asyncOperation, timing, progress, cancellationToken, cancelImmediately, out token), token);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00009724 File Offset: 0x00007924
		public static UniTask<AsyncGPUReadbackRequest>.Awaiter GetAwaiter(this AsyncGPUReadbackRequest asyncOperation)
		{
			return asyncOperation.ToUniTask(PlayerLoopTiming.Update, default(CancellationToken), false).GetAwaiter();
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000974A File Offset: 0x0000794A
		public static UniTask<AsyncGPUReadbackRequest> WithCancellation(this AsyncGPUReadbackRequest asyncOperation, CancellationToken cancellationToken)
		{
			return asyncOperation.ToUniTask(PlayerLoopTiming.Update, cancellationToken, false);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00009755 File Offset: 0x00007955
		public static UniTask<AsyncGPUReadbackRequest> WithCancellation(this AsyncGPUReadbackRequest asyncOperation, CancellationToken cancellationToken, bool cancelImmediately)
		{
			return asyncOperation.ToUniTask(PlayerLoopTiming.Update, cancellationToken, cancelImmediately);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00009760 File Offset: 0x00007960
		public static UniTask<AsyncGPUReadbackRequest> ToUniTask(this AsyncGPUReadbackRequest asyncOperation, PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			if (asyncOperation.done)
			{
				return UniTask.FromResult<AsyncGPUReadbackRequest>(asyncOperation);
			}
			short token;
			return new UniTask<AsyncGPUReadbackRequest>(UnityAsyncExtensions.AsyncGPUReadbackRequestAwaiterConfiguredSource.Create(asyncOperation, timing, cancellationToken, cancelImmediately, out token), token);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000978E File Offset: 0x0000798E
		public static UnityAsyncExtensions.AsyncOperationAwaiter GetAwaiter(this AsyncOperation asyncOperation)
		{
			Error.ThrowArgumentNullException<AsyncOperation>(asyncOperation, "asyncOperation");
			return new UnityAsyncExtensions.AsyncOperationAwaiter(asyncOperation);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x000097A1 File Offset: 0x000079A1
		public static UniTask WithCancellation(this AsyncOperation asyncOperation, CancellationToken cancellationToken)
		{
			return asyncOperation.ToUniTask(null, PlayerLoopTiming.Update, cancellationToken, false);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x000097AD File Offset: 0x000079AD
		public static UniTask WithCancellation(this AsyncOperation asyncOperation, CancellationToken cancellationToken, bool cancelImmediately)
		{
			return asyncOperation.ToUniTask(null, PlayerLoopTiming.Update, cancellationToken, cancelImmediately);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x000097BC File Offset: 0x000079BC
		public static UniTask ToUniTask(this AsyncOperation asyncOperation, IProgress<float> progress = null, PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			Error.ThrowArgumentNullException<AsyncOperation>(asyncOperation, "asyncOperation");
			if (cancellationToken.IsCancellationRequested)
			{
				return UniTask.FromCanceled(cancellationToken);
			}
			if (asyncOperation.isDone)
			{
				return UniTask.CompletedTask;
			}
			short token;
			return new UniTask(UnityAsyncExtensions.AsyncOperationConfiguredSource.Create(asyncOperation, timing, progress, cancellationToken, cancelImmediately, out token), token);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00009805 File Offset: 0x00007A05
		public static UnityAsyncExtensions.ResourceRequestAwaiter GetAwaiter(this ResourceRequest asyncOperation)
		{
			Error.ThrowArgumentNullException<ResourceRequest>(asyncOperation, "asyncOperation");
			return new UnityAsyncExtensions.ResourceRequestAwaiter(asyncOperation);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00009818 File Offset: 0x00007A18
		public static UniTask<Object> WithCancellation(this ResourceRequest asyncOperation, CancellationToken cancellationToken)
		{
			return asyncOperation.ToUniTask(null, PlayerLoopTiming.Update, cancellationToken, false);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00009824 File Offset: 0x00007A24
		public static UniTask<Object> WithCancellation(this ResourceRequest asyncOperation, CancellationToken cancellationToken, bool cancelImmediately)
		{
			return asyncOperation.ToUniTask(null, PlayerLoopTiming.Update, cancellationToken, cancelImmediately);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00009830 File Offset: 0x00007A30
		public static UniTask<Object> ToUniTask(this ResourceRequest asyncOperation, IProgress<float> progress = null, PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			Error.ThrowArgumentNullException<ResourceRequest>(asyncOperation, "asyncOperation");
			if (cancellationToken.IsCancellationRequested)
			{
				return UniTask.FromCanceled<Object>(cancellationToken);
			}
			if (asyncOperation.isDone)
			{
				return UniTask.FromResult<Object>(asyncOperation.asset);
			}
			short token;
			return new UniTask<Object>(UnityAsyncExtensions.ResourceRequestConfiguredSource.Create(asyncOperation, timing, progress, cancellationToken, cancelImmediately, out token), token);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000987F File Offset: 0x00007A7F
		public static UnityAsyncExtensions.AssetBundleRequestAwaiter GetAwaiter(this AssetBundleRequest asyncOperation)
		{
			Error.ThrowArgumentNullException<AssetBundleRequest>(asyncOperation, "asyncOperation");
			return new UnityAsyncExtensions.AssetBundleRequestAwaiter(asyncOperation);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00009892 File Offset: 0x00007A92
		public static UniTask<Object> WithCancellation(this AssetBundleRequest asyncOperation, CancellationToken cancellationToken)
		{
			return asyncOperation.ToUniTask(null, PlayerLoopTiming.Update, cancellationToken, false);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000989E File Offset: 0x00007A9E
		public static UniTask<Object> WithCancellation(this AssetBundleRequest asyncOperation, CancellationToken cancellationToken, bool cancelImmediately)
		{
			return asyncOperation.ToUniTask(null, PlayerLoopTiming.Update, cancellationToken, cancelImmediately);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x000098AC File Offset: 0x00007AAC
		public static UniTask<Object> ToUniTask(this AssetBundleRequest asyncOperation, IProgress<float> progress = null, PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			Error.ThrowArgumentNullException<AssetBundleRequest>(asyncOperation, "asyncOperation");
			if (cancellationToken.IsCancellationRequested)
			{
				return UniTask.FromCanceled<Object>(cancellationToken);
			}
			if (asyncOperation.isDone)
			{
				return UniTask.FromResult<Object>(asyncOperation.asset);
			}
			short token;
			return new UniTask<Object>(UnityAsyncExtensions.AssetBundleRequestConfiguredSource.Create(asyncOperation, timing, progress, cancellationToken, cancelImmediately, out token), token);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x000098FB File Offset: 0x00007AFB
		public static UnityAsyncExtensions.AssetBundleCreateRequestAwaiter GetAwaiter(this AssetBundleCreateRequest asyncOperation)
		{
			Error.ThrowArgumentNullException<AssetBundleCreateRequest>(asyncOperation, "asyncOperation");
			return new UnityAsyncExtensions.AssetBundleCreateRequestAwaiter(asyncOperation);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000990E File Offset: 0x00007B0E
		public static UniTask<AssetBundle> WithCancellation(this AssetBundleCreateRequest asyncOperation, CancellationToken cancellationToken)
		{
			return asyncOperation.ToUniTask(null, PlayerLoopTiming.Update, cancellationToken, false);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000991A File Offset: 0x00007B1A
		public static UniTask<AssetBundle> WithCancellation(this AssetBundleCreateRequest asyncOperation, CancellationToken cancellationToken, bool cancelImmediately)
		{
			return asyncOperation.ToUniTask(null, PlayerLoopTiming.Update, cancellationToken, cancelImmediately);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00009928 File Offset: 0x00007B28
		public static UniTask<AssetBundle> ToUniTask(this AssetBundleCreateRequest asyncOperation, IProgress<float> progress = null, PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			Error.ThrowArgumentNullException<AssetBundleCreateRequest>(asyncOperation, "asyncOperation");
			if (cancellationToken.IsCancellationRequested)
			{
				return UniTask.FromCanceled<AssetBundle>(cancellationToken);
			}
			if (asyncOperation.isDone)
			{
				return UniTask.FromResult<AssetBundle>(asyncOperation.assetBundle);
			}
			short token;
			return new UniTask<AssetBundle>(UnityAsyncExtensions.AssetBundleCreateRequestConfiguredSource.Create(asyncOperation, timing, progress, cancellationToken, cancelImmediately, out token), token);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00009977 File Offset: 0x00007B77
		public static UnityAsyncExtensions.UnityWebRequestAsyncOperationAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOperation)
		{
			Error.ThrowArgumentNullException<UnityWebRequestAsyncOperation>(asyncOperation, "asyncOperation");
			return new UnityAsyncExtensions.UnityWebRequestAsyncOperationAwaiter(asyncOperation);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000998A File Offset: 0x00007B8A
		public static UniTask<UnityWebRequest> WithCancellation(this UnityWebRequestAsyncOperation asyncOperation, CancellationToken cancellationToken)
		{
			return asyncOperation.ToUniTask(null, PlayerLoopTiming.Update, cancellationToken, false);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00009996 File Offset: 0x00007B96
		public static UniTask<UnityWebRequest> WithCancellation(this UnityWebRequestAsyncOperation asyncOperation, CancellationToken cancellationToken, bool cancelImmediately)
		{
			return asyncOperation.ToUniTask(null, PlayerLoopTiming.Update, cancellationToken, cancelImmediately);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x000099A4 File Offset: 0x00007BA4
		public static UniTask<UnityWebRequest> ToUniTask(this UnityWebRequestAsyncOperation asyncOperation, IProgress<float> progress = null, PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			Error.ThrowArgumentNullException<UnityWebRequestAsyncOperation>(asyncOperation, "asyncOperation");
			if (cancellationToken.IsCancellationRequested)
			{
				return UniTask.FromCanceled<UnityWebRequest>(cancellationToken);
			}
			if (!asyncOperation.isDone)
			{
				short token;
				return new UniTask<UnityWebRequest>(UnityAsyncExtensions.UnityWebRequestAsyncOperationConfiguredSource.Create(asyncOperation, timing, progress, cancellationToken, cancelImmediately, out token), token);
			}
			if (asyncOperation.webRequest.IsError())
			{
				return UniTask.FromException<UnityWebRequest>(new UnityWebRequestException(asyncOperation.webRequest));
			}
			return UniTask.FromResult<UnityWebRequest>(asyncOperation.webRequest);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00009A14 File Offset: 0x00007C14
		public static UniTask WaitAsync(this JobHandle jobHandle, PlayerLoopTiming waitTiming, CancellationToken cancellationToken = default(CancellationToken))
		{
			UnityAsyncExtensions.<WaitAsync>d__41 <WaitAsync>d__;
			<WaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WaitAsync>d__.jobHandle = jobHandle;
			<WaitAsync>d__.waitTiming = waitTiming;
			<WaitAsync>d__.cancellationToken = cancellationToken;
			<WaitAsync>d__.<>1__state = -1;
			<WaitAsync>d__.<>t__builder.Start<UnityAsyncExtensions.<WaitAsync>d__41>(ref <WaitAsync>d__);
			return <WaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00009A68 File Offset: 0x00007C68
		public static UniTask.Awaiter GetAwaiter(this JobHandle jobHandle)
		{
			short token;
			UnityAsyncExtensions.JobHandlePromise jobHandlePromise = UnityAsyncExtensions.JobHandlePromise.Create(jobHandle, out token);
			PlayerLoopHelper.AddAction(PlayerLoopTiming.EarlyUpdate, jobHandlePromise);
			PlayerLoopHelper.AddAction(PlayerLoopTiming.PreUpdate, jobHandlePromise);
			PlayerLoopHelper.AddAction(PlayerLoopTiming.Update, jobHandlePromise);
			PlayerLoopHelper.AddAction(PlayerLoopTiming.PreLateUpdate, jobHandlePromise);
			PlayerLoopHelper.AddAction(PlayerLoopTiming.PostLateUpdate, jobHandlePromise);
			return new UniTask(jobHandlePromise, token).GetAwaiter();
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00009AB4 File Offset: 0x00007CB4
		public static UniTask ToUniTask(this JobHandle jobHandle, PlayerLoopTiming waitTiming)
		{
			short token;
			UnityAsyncExtensions.JobHandlePromise jobHandlePromise = UnityAsyncExtensions.JobHandlePromise.Create(jobHandle, out token);
			PlayerLoopHelper.AddAction(waitTiming, jobHandlePromise);
			return new UniTask(jobHandlePromise, token);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00009AD8 File Offset: 0x00007CD8
		public static UniTask StartAsyncCoroutine(this MonoBehaviour monoBehaviour, Func<CancellationToken, UniTask> asyncCoroutine)
		{
			CancellationToken cancellationTokenOnDestroy = monoBehaviour.GetCancellationTokenOnDestroy();
			return asyncCoroutine(cancellationTokenOnDestroy);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00009AF3 File Offset: 0x00007CF3
		public static AsyncUnityEventHandler GetAsyncEventHandler(this UnityEvent unityEvent, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler(unityEvent, cancellationToken, false);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00009AFD File Offset: 0x00007CFD
		public static UniTask OnInvokeAsync(this UnityEvent unityEvent, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler(unityEvent, cancellationToken, true).OnInvokeAsync();
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00009B0C File Offset: 0x00007D0C
		public static IUniTaskAsyncEnumerable<AsyncUnit> OnInvokeAsAsyncEnumerable(this UnityEvent unityEvent, CancellationToken cancellationToken)
		{
			return new UnityEventHandlerAsyncEnumerable(unityEvent, cancellationToken);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00009B15 File Offset: 0x00007D15
		public static AsyncUnityEventHandler<T> GetAsyncEventHandler<T>(this UnityEvent<T> unityEvent, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<T>(unityEvent, cancellationToken, false);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009B1F File Offset: 0x00007D1F
		public static UniTask<T> OnInvokeAsync<T>(this UnityEvent<T> unityEvent, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<T>(unityEvent, cancellationToken, true).OnInvokeAsync();
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00009B2E File Offset: 0x00007D2E
		public static IUniTaskAsyncEnumerable<T> OnInvokeAsAsyncEnumerable<T>(this UnityEvent<T> unityEvent, CancellationToken cancellationToken)
		{
			return new UnityEventHandlerAsyncEnumerable<T>(unityEvent, cancellationToken);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00009B37 File Offset: 0x00007D37
		public static IAsyncClickEventHandler GetAsyncClickEventHandler(this Button button)
		{
			return new AsyncUnityEventHandler(button.onClick, button.GetCancellationTokenOnDestroy(), false);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00009B4B File Offset: 0x00007D4B
		public static IAsyncClickEventHandler GetAsyncClickEventHandler(this Button button, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler(button.onClick, cancellationToken, false);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00009B5A File Offset: 0x00007D5A
		public static UniTask OnClickAsync(this Button button)
		{
			return new AsyncUnityEventHandler(button.onClick, button.GetCancellationTokenOnDestroy(), true).OnInvokeAsync();
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00009B73 File Offset: 0x00007D73
		public static UniTask OnClickAsync(this Button button, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler(button.onClick, cancellationToken, true).OnInvokeAsync();
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00009B87 File Offset: 0x00007D87
		public static IUniTaskAsyncEnumerable<AsyncUnit> OnClickAsAsyncEnumerable(this Button button)
		{
			return new UnityEventHandlerAsyncEnumerable(button.onClick, button.GetCancellationTokenOnDestroy());
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00009B9A File Offset: 0x00007D9A
		public static IUniTaskAsyncEnumerable<AsyncUnit> OnClickAsAsyncEnumerable(this Button button, CancellationToken cancellationToken)
		{
			return new UnityEventHandlerAsyncEnumerable(button.onClick, cancellationToken);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00009BA8 File Offset: 0x00007DA8
		public static IAsyncValueChangedEventHandler<bool> GetAsyncValueChangedEventHandler(this Toggle toggle)
		{
			return new AsyncUnityEventHandler<bool>(toggle.onValueChanged, toggle.GetCancellationTokenOnDestroy(), false);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00009BBC File Offset: 0x00007DBC
		public static IAsyncValueChangedEventHandler<bool> GetAsyncValueChangedEventHandler(this Toggle toggle, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<bool>(toggle.onValueChanged, cancellationToken, false);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00009BCB File Offset: 0x00007DCB
		public static UniTask<bool> OnValueChangedAsync(this Toggle toggle)
		{
			return new AsyncUnityEventHandler<bool>(toggle.onValueChanged, toggle.GetCancellationTokenOnDestroy(), true).OnInvokeAsync();
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00009BE4 File Offset: 0x00007DE4
		public static UniTask<bool> OnValueChangedAsync(this Toggle toggle, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<bool>(toggle.onValueChanged, cancellationToken, true).OnInvokeAsync();
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00009BF8 File Offset: 0x00007DF8
		public static IUniTaskAsyncEnumerable<bool> OnValueChangedAsAsyncEnumerable(this Toggle toggle)
		{
			return new UnityEventHandlerAsyncEnumerable<bool>(toggle.onValueChanged, toggle.GetCancellationTokenOnDestroy());
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00009C0B File Offset: 0x00007E0B
		public static IUniTaskAsyncEnumerable<bool> OnValueChangedAsAsyncEnumerable(this Toggle toggle, CancellationToken cancellationToken)
		{
			return new UnityEventHandlerAsyncEnumerable<bool>(toggle.onValueChanged, cancellationToken);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00009C19 File Offset: 0x00007E19
		public static IAsyncValueChangedEventHandler<float> GetAsyncValueChangedEventHandler(this Scrollbar scrollbar)
		{
			return new AsyncUnityEventHandler<float>(scrollbar.onValueChanged, scrollbar.GetCancellationTokenOnDestroy(), false);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00009C2D File Offset: 0x00007E2D
		public static IAsyncValueChangedEventHandler<float> GetAsyncValueChangedEventHandler(this Scrollbar scrollbar, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<float>(scrollbar.onValueChanged, cancellationToken, false);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00009C3C File Offset: 0x00007E3C
		public static UniTask<float> OnValueChangedAsync(this Scrollbar scrollbar)
		{
			return new AsyncUnityEventHandler<float>(scrollbar.onValueChanged, scrollbar.GetCancellationTokenOnDestroy(), true).OnInvokeAsync();
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00009C55 File Offset: 0x00007E55
		public static UniTask<float> OnValueChangedAsync(this Scrollbar scrollbar, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<float>(scrollbar.onValueChanged, cancellationToken, true).OnInvokeAsync();
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00009C69 File Offset: 0x00007E69
		public static IUniTaskAsyncEnumerable<float> OnValueChangedAsAsyncEnumerable(this Scrollbar scrollbar)
		{
			return new UnityEventHandlerAsyncEnumerable<float>(scrollbar.onValueChanged, scrollbar.GetCancellationTokenOnDestroy());
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00009C7C File Offset: 0x00007E7C
		public static IUniTaskAsyncEnumerable<float> OnValueChangedAsAsyncEnumerable(this Scrollbar scrollbar, CancellationToken cancellationToken)
		{
			return new UnityEventHandlerAsyncEnumerable<float>(scrollbar.onValueChanged, cancellationToken);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00009C8A File Offset: 0x00007E8A
		public static IAsyncValueChangedEventHandler<Vector2> GetAsyncValueChangedEventHandler(this ScrollRect scrollRect)
		{
			return new AsyncUnityEventHandler<Vector2>(scrollRect.onValueChanged, scrollRect.GetCancellationTokenOnDestroy(), false);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00009C9E File Offset: 0x00007E9E
		public static IAsyncValueChangedEventHandler<Vector2> GetAsyncValueChangedEventHandler(this ScrollRect scrollRect, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<Vector2>(scrollRect.onValueChanged, cancellationToken, false);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00009CAD File Offset: 0x00007EAD
		public static UniTask<Vector2> OnValueChangedAsync(this ScrollRect scrollRect)
		{
			return new AsyncUnityEventHandler<Vector2>(scrollRect.onValueChanged, scrollRect.GetCancellationTokenOnDestroy(), true).OnInvokeAsync();
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00009CC6 File Offset: 0x00007EC6
		public static UniTask<Vector2> OnValueChangedAsync(this ScrollRect scrollRect, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<Vector2>(scrollRect.onValueChanged, cancellationToken, true).OnInvokeAsync();
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00009CDA File Offset: 0x00007EDA
		public static IUniTaskAsyncEnumerable<Vector2> OnValueChangedAsAsyncEnumerable(this ScrollRect scrollRect)
		{
			return new UnityEventHandlerAsyncEnumerable<Vector2>(scrollRect.onValueChanged, scrollRect.GetCancellationTokenOnDestroy());
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00009CED File Offset: 0x00007EED
		public static IUniTaskAsyncEnumerable<Vector2> OnValueChangedAsAsyncEnumerable(this ScrollRect scrollRect, CancellationToken cancellationToken)
		{
			return new UnityEventHandlerAsyncEnumerable<Vector2>(scrollRect.onValueChanged, cancellationToken);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00009CFB File Offset: 0x00007EFB
		public static IAsyncValueChangedEventHandler<float> GetAsyncValueChangedEventHandler(this Slider slider)
		{
			return new AsyncUnityEventHandler<float>(slider.onValueChanged, slider.GetCancellationTokenOnDestroy(), false);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00009D0F File Offset: 0x00007F0F
		public static IAsyncValueChangedEventHandler<float> GetAsyncValueChangedEventHandler(this Slider slider, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<float>(slider.onValueChanged, cancellationToken, false);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00009D1E File Offset: 0x00007F1E
		public static UniTask<float> OnValueChangedAsync(this Slider slider)
		{
			return new AsyncUnityEventHandler<float>(slider.onValueChanged, slider.GetCancellationTokenOnDestroy(), true).OnInvokeAsync();
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00009D37 File Offset: 0x00007F37
		public static UniTask<float> OnValueChangedAsync(this Slider slider, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<float>(slider.onValueChanged, cancellationToken, true).OnInvokeAsync();
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00009D4B File Offset: 0x00007F4B
		public static IUniTaskAsyncEnumerable<float> OnValueChangedAsAsyncEnumerable(this Slider slider)
		{
			return new UnityEventHandlerAsyncEnumerable<float>(slider.onValueChanged, slider.GetCancellationTokenOnDestroy());
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00009D5E File Offset: 0x00007F5E
		public static IUniTaskAsyncEnumerable<float> OnValueChangedAsAsyncEnumerable(this Slider slider, CancellationToken cancellationToken)
		{
			return new UnityEventHandlerAsyncEnumerable<float>(slider.onValueChanged, cancellationToken);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00009D6C File Offset: 0x00007F6C
		public static IAsyncEndEditEventHandler<string> GetAsyncEndEditEventHandler(this InputField inputField)
		{
			return new AsyncUnityEventHandler<string>(inputField.onEndEdit, inputField.GetCancellationTokenOnDestroy(), false);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00009D80 File Offset: 0x00007F80
		public static IAsyncEndEditEventHandler<string> GetAsyncEndEditEventHandler(this InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<string>(inputField.onEndEdit, cancellationToken, false);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00009D8F File Offset: 0x00007F8F
		public static UniTask<string> OnEndEditAsync(this InputField inputField)
		{
			return new AsyncUnityEventHandler<string>(inputField.onEndEdit, inputField.GetCancellationTokenOnDestroy(), true).OnInvokeAsync();
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00009DA8 File Offset: 0x00007FA8
		public static UniTask<string> OnEndEditAsync(this InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<string>(inputField.onEndEdit, cancellationToken, true).OnInvokeAsync();
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00009DBC File Offset: 0x00007FBC
		public static IUniTaskAsyncEnumerable<string> OnEndEditAsAsyncEnumerable(this InputField inputField)
		{
			return new UnityEventHandlerAsyncEnumerable<string>(inputField.onEndEdit, inputField.GetCancellationTokenOnDestroy());
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00009DCF File Offset: 0x00007FCF
		public static IUniTaskAsyncEnumerable<string> OnEndEditAsAsyncEnumerable(this InputField inputField, CancellationToken cancellationToken)
		{
			return new UnityEventHandlerAsyncEnumerable<string>(inputField.onEndEdit, cancellationToken);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00009DDD File Offset: 0x00007FDD
		public static IAsyncValueChangedEventHandler<string> GetAsyncValueChangedEventHandler(this InputField inputField)
		{
			return new AsyncUnityEventHandler<string>(inputField.onValueChanged, inputField.GetCancellationTokenOnDestroy(), false);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00009DF1 File Offset: 0x00007FF1
		public static IAsyncValueChangedEventHandler<string> GetAsyncValueChangedEventHandler(this InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<string>(inputField.onValueChanged, cancellationToken, false);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00009E00 File Offset: 0x00008000
		public static UniTask<string> OnValueChangedAsync(this InputField inputField)
		{
			return new AsyncUnityEventHandler<string>(inputField.onValueChanged, inputField.GetCancellationTokenOnDestroy(), true).OnInvokeAsync();
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00009E19 File Offset: 0x00008019
		public static UniTask<string> OnValueChangedAsync(this InputField inputField, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<string>(inputField.onValueChanged, cancellationToken, true).OnInvokeAsync();
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00009E2D File Offset: 0x0000802D
		public static IUniTaskAsyncEnumerable<string> OnValueChangedAsAsyncEnumerable(this InputField inputField)
		{
			return new UnityEventHandlerAsyncEnumerable<string>(inputField.onValueChanged, inputField.GetCancellationTokenOnDestroy());
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00009E40 File Offset: 0x00008040
		public static IUniTaskAsyncEnumerable<string> OnValueChangedAsAsyncEnumerable(this InputField inputField, CancellationToken cancellationToken)
		{
			return new UnityEventHandlerAsyncEnumerable<string>(inputField.onValueChanged, cancellationToken);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00009E4E File Offset: 0x0000804E
		public static IAsyncValueChangedEventHandler<int> GetAsyncValueChangedEventHandler(this Dropdown dropdown)
		{
			return new AsyncUnityEventHandler<int>(dropdown.onValueChanged, dropdown.GetCancellationTokenOnDestroy(), false);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00009E62 File Offset: 0x00008062
		public static IAsyncValueChangedEventHandler<int> GetAsyncValueChangedEventHandler(this Dropdown dropdown, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<int>(dropdown.onValueChanged, cancellationToken, false);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00009E71 File Offset: 0x00008071
		public static UniTask<int> OnValueChangedAsync(this Dropdown dropdown)
		{
			return new AsyncUnityEventHandler<int>(dropdown.onValueChanged, dropdown.GetCancellationTokenOnDestroy(), true).OnInvokeAsync();
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00009E8A File Offset: 0x0000808A
		public static UniTask<int> OnValueChangedAsync(this Dropdown dropdown, CancellationToken cancellationToken)
		{
			return new AsyncUnityEventHandler<int>(dropdown.onValueChanged, cancellationToken, true).OnInvokeAsync();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00009E9E File Offset: 0x0000809E
		public static IUniTaskAsyncEnumerable<int> OnValueChangedAsAsyncEnumerable(this Dropdown dropdown)
		{
			return new UnityEventHandlerAsyncEnumerable<int>(dropdown.onValueChanged, dropdown.GetCancellationTokenOnDestroy());
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00009EB1 File Offset: 0x000080B1
		public static IUniTaskAsyncEnumerable<int> OnValueChangedAsAsyncEnumerable(this Dropdown dropdown, CancellationToken cancellationToken)
		{
			return new UnityEventHandlerAsyncEnumerable<int>(dropdown.onValueChanged, cancellationToken);
		}

		// Token: 0x020001EE RID: 494
		public struct AssetBundleRequestAllAssetsAwaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x06000B04 RID: 2820 RVA: 0x00027BFC File Offset: 0x00025DFC
			public AssetBundleRequestAllAssetsAwaiter(AssetBundleRequest asyncOperation)
			{
				this.asyncOperation = asyncOperation;
				this.continuationAction = null;
			}

			// Token: 0x06000B05 RID: 2821 RVA: 0x00027C0C File Offset: 0x00025E0C
			public UnityAsyncExtensions.AssetBundleRequestAllAssetsAwaiter GetAwaiter()
			{
				return this;
			}

			// Token: 0x17000077 RID: 119
			// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00027C14 File Offset: 0x00025E14
			public bool IsCompleted
			{
				get
				{
					return this.asyncOperation.isDone;
				}
			}

			// Token: 0x06000B07 RID: 2823 RVA: 0x00027C24 File Offset: 0x00025E24
			public Object[] GetResult()
			{
				if (this.continuationAction != null)
				{
					this.asyncOperation.completed -= this.continuationAction;
					this.continuationAction = null;
					Object[] allAssets = this.asyncOperation.allAssets;
					this.asyncOperation = null;
					return allAssets;
				}
				Object[] allAssets2 = this.asyncOperation.allAssets;
				this.asyncOperation = null;
				return allAssets2;
			}

			// Token: 0x06000B08 RID: 2824 RVA: 0x00027C76 File Offset: 0x00025E76
			public void OnCompleted(Action continuation)
			{
				this.UnsafeOnCompleted(continuation);
			}

			// Token: 0x06000B09 RID: 2825 RVA: 0x00027C7F File Offset: 0x00025E7F
			public void UnsafeOnCompleted(Action continuation)
			{
				Error.ThrowWhenContinuationIsAlreadyRegistered<Action<AsyncOperation>>(this.continuationAction);
				this.continuationAction = PooledDelegate<AsyncOperation>.Create(continuation);
				this.asyncOperation.completed += this.continuationAction;
			}

			// Token: 0x04000487 RID: 1159
			private AssetBundleRequest asyncOperation;

			// Token: 0x04000488 RID: 1160
			private Action<AsyncOperation> continuationAction;
		}

		// Token: 0x020001EF RID: 495
		private sealed class AssetBundleRequestAllAssetsConfiguredSource : IUniTaskSource<Object[]>, IUniTaskSource, IValueTaskSource, IValueTaskSource<Object[]>, IPlayerLoopItem, ITaskPoolNode<UnityAsyncExtensions.AssetBundleRequestAllAssetsConfiguredSource>
		{
			// Token: 0x17000078 RID: 120
			// (get) Token: 0x06000B0A RID: 2826 RVA: 0x00027CA9 File Offset: 0x00025EA9
			public ref UnityAsyncExtensions.AssetBundleRequestAllAssetsConfiguredSource NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x06000B0B RID: 2827 RVA: 0x00027CB1 File Offset: 0x00025EB1
			static AssetBundleRequestAllAssetsConfiguredSource()
			{
				TaskPool.RegisterSizeGetter(typeof(UnityAsyncExtensions.AssetBundleRequestAllAssetsConfiguredSource), () => UnityAsyncExtensions.AssetBundleRequestAllAssetsConfiguredSource.pool.Size);
			}

			// Token: 0x06000B0C RID: 2828 RVA: 0x00027CD2 File Offset: 0x00025ED2
			private AssetBundleRequestAllAssetsConfiguredSource()
			{
				this.continuationAction = new Action<AsyncOperation>(this.Continuation);
			}

			// Token: 0x06000B0D RID: 2829 RVA: 0x00027CEC File Offset: 0x00025EEC
			public static IUniTaskSource<Object[]> Create(AssetBundleRequest asyncOperation, PlayerLoopTiming timing, IProgress<float> progress, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource<Object[]>.CreateFromCanceled(cancellationToken, out token);
				}
				UnityAsyncExtensions.AssetBundleRequestAllAssetsConfiguredSource assetBundleRequestAllAssetsConfiguredSource;
				if (!UnityAsyncExtensions.AssetBundleRequestAllAssetsConfiguredSource.pool.TryPop(out assetBundleRequestAllAssetsConfiguredSource))
				{
					assetBundleRequestAllAssetsConfiguredSource = new UnityAsyncExtensions.AssetBundleRequestAllAssetsConfiguredSource();
				}
				assetBundleRequestAllAssetsConfiguredSource.asyncOperation = asyncOperation;
				assetBundleRequestAllAssetsConfiguredSource.progress = progress;
				assetBundleRequestAllAssetsConfiguredSource.cancellationToken = cancellationToken;
				assetBundleRequestAllAssetsConfiguredSource.cancelImmediately = cancelImmediately;
				assetBundleRequestAllAssetsConfiguredSource.completed = false;
				asyncOperation.completed += assetBundleRequestAllAssetsConfiguredSource.continuationAction;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					assetBundleRequestAllAssetsConfiguredSource.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UnityAsyncExtensions.AssetBundleRequestAllAssetsConfiguredSource assetBundleRequestAllAssetsConfiguredSource2 = (UnityAsyncExtensions.AssetBundleRequestAllAssetsConfiguredSource)state;
						assetBundleRequestAllAssetsConfiguredSource2.core.TrySetCanceled(assetBundleRequestAllAssetsConfiguredSource2.cancellationToken);
					}, assetBundleRequestAllAssetsConfiguredSource);
				}
				PlayerLoopHelper.AddAction(timing, assetBundleRequestAllAssetsConfiguredSource);
				token = assetBundleRequestAllAssetsConfiguredSource.core.Version;
				return assetBundleRequestAllAssetsConfiguredSource;
			}

			// Token: 0x06000B0E RID: 2830 RVA: 0x00027DA0 File Offset: 0x00025FA0
			public Object[] GetResult(short token)
			{
				Object[] result;
				try
				{
					result = this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
				return result;
			}

			// Token: 0x06000B0F RID: 2831 RVA: 0x00027DEC File Offset: 0x00025FEC
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000B10 RID: 2832 RVA: 0x00027DF6 File Offset: 0x00025FF6
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000B11 RID: 2833 RVA: 0x00027E04 File Offset: 0x00026004
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000B12 RID: 2834 RVA: 0x00027E11 File Offset: 0x00026011
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000B13 RID: 2835 RVA: 0x00027E24 File Offset: 0x00026024
			public bool MoveNext()
			{
				if (this.completed || this.asyncOperation == null)
				{
					return false;
				}
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				if (this.progress != null)
				{
					this.progress.Report(this.asyncOperation.progress);
				}
				if (this.asyncOperation.isDone)
				{
					this.core.TrySetResult(this.asyncOperation.allAssets);
					return false;
				}
				return true;
			}

			// Token: 0x06000B14 RID: 2836 RVA: 0x00027EAC File Offset: 0x000260AC
			private bool TryReturn()
			{
				this.core.Reset();
				this.asyncOperation = null;
				this.progress = null;
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UnityAsyncExtensions.AssetBundleRequestAllAssetsConfiguredSource.pool.TryPush(this);
			}

			// Token: 0x06000B15 RID: 2837 RVA: 0x00027EFC File Offset: 0x000260FC
			private void Continuation(AsyncOperation _)
			{
				if (this.completed)
				{
					return;
				}
				this.completed = true;
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return;
				}
				this.core.TrySetResult(this.asyncOperation.allAssets);
			}

			// Token: 0x04000489 RID: 1161
			private static TaskPool<UnityAsyncExtensions.AssetBundleRequestAllAssetsConfiguredSource> pool;

			// Token: 0x0400048A RID: 1162
			private UnityAsyncExtensions.AssetBundleRequestAllAssetsConfiguredSource nextNode;

			// Token: 0x0400048B RID: 1163
			private AssetBundleRequest asyncOperation;

			// Token: 0x0400048C RID: 1164
			private IProgress<float> progress;

			// Token: 0x0400048D RID: 1165
			private CancellationToken cancellationToken;

			// Token: 0x0400048E RID: 1166
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x0400048F RID: 1167
			private bool cancelImmediately;

			// Token: 0x04000490 RID: 1168
			private bool completed;

			// Token: 0x04000491 RID: 1169
			private UniTaskCompletionSourceCore<Object[]> core;

			// Token: 0x04000492 RID: 1170
			private Action<AsyncOperation> continuationAction;
		}

		// Token: 0x020001F0 RID: 496
		private sealed class AsyncGPUReadbackRequestAwaiterConfiguredSource : IUniTaskSource<AsyncGPUReadbackRequest>, IUniTaskSource, IValueTaskSource, IValueTaskSource<AsyncGPUReadbackRequest>, IPlayerLoopItem, ITaskPoolNode<UnityAsyncExtensions.AsyncGPUReadbackRequestAwaiterConfiguredSource>
		{
			// Token: 0x17000079 RID: 121
			// (get) Token: 0x06000B16 RID: 2838 RVA: 0x00027F50 File Offset: 0x00026150
			public ref UnityAsyncExtensions.AsyncGPUReadbackRequestAwaiterConfiguredSource NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x06000B17 RID: 2839 RVA: 0x00027F58 File Offset: 0x00026158
			static AsyncGPUReadbackRequestAwaiterConfiguredSource()
			{
				TaskPool.RegisterSizeGetter(typeof(UnityAsyncExtensions.AsyncGPUReadbackRequestAwaiterConfiguredSource), () => UnityAsyncExtensions.AsyncGPUReadbackRequestAwaiterConfiguredSource.pool.Size);
			}

			// Token: 0x06000B18 RID: 2840 RVA: 0x00027F79 File Offset: 0x00026179
			private AsyncGPUReadbackRequestAwaiterConfiguredSource()
			{
			}

			// Token: 0x06000B19 RID: 2841 RVA: 0x00027F84 File Offset: 0x00026184
			public static IUniTaskSource<AsyncGPUReadbackRequest> Create(AsyncGPUReadbackRequest asyncOperation, PlayerLoopTiming timing, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource<AsyncGPUReadbackRequest>.CreateFromCanceled(cancellationToken, out token);
				}
				UnityAsyncExtensions.AsyncGPUReadbackRequestAwaiterConfiguredSource asyncGPUReadbackRequestAwaiterConfiguredSource;
				if (!UnityAsyncExtensions.AsyncGPUReadbackRequestAwaiterConfiguredSource.pool.TryPop(out asyncGPUReadbackRequestAwaiterConfiguredSource))
				{
					asyncGPUReadbackRequestAwaiterConfiguredSource = new UnityAsyncExtensions.AsyncGPUReadbackRequestAwaiterConfiguredSource();
				}
				asyncGPUReadbackRequestAwaiterConfiguredSource.asyncOperation = asyncOperation;
				asyncGPUReadbackRequestAwaiterConfiguredSource.cancellationToken = cancellationToken;
				asyncGPUReadbackRequestAwaiterConfiguredSource.cancelImmediately = cancelImmediately;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					asyncGPUReadbackRequestAwaiterConfiguredSource.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UnityAsyncExtensions.AsyncGPUReadbackRequestAwaiterConfiguredSource asyncGPUReadbackRequestAwaiterConfiguredSource2 = (UnityAsyncExtensions.AsyncGPUReadbackRequestAwaiterConfiguredSource)state;
						asyncGPUReadbackRequestAwaiterConfiguredSource2.core.TrySetCanceled(asyncGPUReadbackRequestAwaiterConfiguredSource2.cancellationToken);
					}, asyncGPUReadbackRequestAwaiterConfiguredSource);
				}
				PlayerLoopHelper.AddAction(timing, asyncGPUReadbackRequestAwaiterConfiguredSource);
				token = asyncGPUReadbackRequestAwaiterConfiguredSource.core.Version;
				return asyncGPUReadbackRequestAwaiterConfiguredSource;
			}

			// Token: 0x06000B1A RID: 2842 RVA: 0x0002801C File Offset: 0x0002621C
			public AsyncGPUReadbackRequest GetResult(short token)
			{
				AsyncGPUReadbackRequest result;
				try
				{
					result = this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
				return result;
			}

			// Token: 0x06000B1B RID: 2843 RVA: 0x00028068 File Offset: 0x00026268
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000B1C RID: 2844 RVA: 0x00028072 File Offset: 0x00026272
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000B1D RID: 2845 RVA: 0x00028080 File Offset: 0x00026280
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000B1E RID: 2846 RVA: 0x0002808D File Offset: 0x0002628D
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000B1F RID: 2847 RVA: 0x000280A0 File Offset: 0x000262A0
			public bool MoveNext()
			{
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				if (this.asyncOperation.hasError)
				{
					this.core.TrySetException(new Exception("AsyncGPUReadbackRequest.hasError = true"));
					return false;
				}
				if (this.asyncOperation.done)
				{
					this.core.TrySetResult(this.asyncOperation);
					return false;
				}
				return true;
			}

			// Token: 0x06000B20 RID: 2848 RVA: 0x00028118 File Offset: 0x00026318
			private bool TryReturn()
			{
				this.core.Reset();
				this.asyncOperation = default(AsyncGPUReadbackRequest);
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UnityAsyncExtensions.AsyncGPUReadbackRequestAwaiterConfiguredSource.pool.TryPush(this);
			}

			// Token: 0x04000493 RID: 1171
			private static TaskPool<UnityAsyncExtensions.AsyncGPUReadbackRequestAwaiterConfiguredSource> pool;

			// Token: 0x04000494 RID: 1172
			private UnityAsyncExtensions.AsyncGPUReadbackRequestAwaiterConfiguredSource nextNode;

			// Token: 0x04000495 RID: 1173
			private AsyncGPUReadbackRequest asyncOperation;

			// Token: 0x04000496 RID: 1174
			private CancellationToken cancellationToken;

			// Token: 0x04000497 RID: 1175
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x04000498 RID: 1176
			private bool cancelImmediately;

			// Token: 0x04000499 RID: 1177
			private UniTaskCompletionSourceCore<AsyncGPUReadbackRequest> core;
		}

		// Token: 0x020001F1 RID: 497
		public struct AsyncOperationAwaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x06000B21 RID: 2849 RVA: 0x00028165 File Offset: 0x00026365
			public AsyncOperationAwaiter(AsyncOperation asyncOperation)
			{
				this.asyncOperation = asyncOperation;
				this.continuationAction = null;
			}

			// Token: 0x1700007A RID: 122
			// (get) Token: 0x06000B22 RID: 2850 RVA: 0x00028175 File Offset: 0x00026375
			public bool IsCompleted
			{
				get
				{
					return this.asyncOperation.isDone;
				}
			}

			// Token: 0x06000B23 RID: 2851 RVA: 0x00028182 File Offset: 0x00026382
			public void GetResult()
			{
				if (this.continuationAction != null)
				{
					this.asyncOperation.completed -= this.continuationAction;
					this.continuationAction = null;
					this.asyncOperation = null;
					return;
				}
				this.asyncOperation = null;
			}

			// Token: 0x06000B24 RID: 2852 RVA: 0x000281B3 File Offset: 0x000263B3
			public void OnCompleted(Action continuation)
			{
				this.UnsafeOnCompleted(continuation);
			}

			// Token: 0x06000B25 RID: 2853 RVA: 0x000281BC File Offset: 0x000263BC
			public void UnsafeOnCompleted(Action continuation)
			{
				Error.ThrowWhenContinuationIsAlreadyRegistered<Action<AsyncOperation>>(this.continuationAction);
				this.continuationAction = PooledDelegate<AsyncOperation>.Create(continuation);
				this.asyncOperation.completed += this.continuationAction;
			}

			// Token: 0x0400049A RID: 1178
			private AsyncOperation asyncOperation;

			// Token: 0x0400049B RID: 1179
			private Action<AsyncOperation> continuationAction;
		}

		// Token: 0x020001F2 RID: 498
		private sealed class AsyncOperationConfiguredSource : IUniTaskSource, IValueTaskSource, IPlayerLoopItem, ITaskPoolNode<UnityAsyncExtensions.AsyncOperationConfiguredSource>
		{
			// Token: 0x1700007B RID: 123
			// (get) Token: 0x06000B26 RID: 2854 RVA: 0x000281E6 File Offset: 0x000263E6
			public ref UnityAsyncExtensions.AsyncOperationConfiguredSource NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x06000B27 RID: 2855 RVA: 0x000281EE File Offset: 0x000263EE
			static AsyncOperationConfiguredSource()
			{
				TaskPool.RegisterSizeGetter(typeof(UnityAsyncExtensions.AsyncOperationConfiguredSource), () => UnityAsyncExtensions.AsyncOperationConfiguredSource.pool.Size);
			}

			// Token: 0x06000B28 RID: 2856 RVA: 0x0002820F File Offset: 0x0002640F
			private AsyncOperationConfiguredSource()
			{
				this.continuationAction = new Action<AsyncOperation>(this.Continuation);
			}

			// Token: 0x06000B29 RID: 2857 RVA: 0x0002822C File Offset: 0x0002642C
			public static IUniTaskSource Create(AsyncOperation asyncOperation, PlayerLoopTiming timing, IProgress<float> progress, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
				}
				UnityAsyncExtensions.AsyncOperationConfiguredSource asyncOperationConfiguredSource;
				if (!UnityAsyncExtensions.AsyncOperationConfiguredSource.pool.TryPop(out asyncOperationConfiguredSource))
				{
					asyncOperationConfiguredSource = new UnityAsyncExtensions.AsyncOperationConfiguredSource();
				}
				asyncOperationConfiguredSource.asyncOperation = asyncOperation;
				asyncOperationConfiguredSource.progress = progress;
				asyncOperationConfiguredSource.cancellationToken = cancellationToken;
				asyncOperationConfiguredSource.cancelImmediately = cancelImmediately;
				asyncOperationConfiguredSource.completed = false;
				asyncOperation.completed += asyncOperationConfiguredSource.continuationAction;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					asyncOperationConfiguredSource.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UnityAsyncExtensions.AsyncOperationConfiguredSource asyncOperationConfiguredSource2 = (UnityAsyncExtensions.AsyncOperationConfiguredSource)state;
						asyncOperationConfiguredSource2.core.TrySetCanceled(asyncOperationConfiguredSource2.cancellationToken);
					}, asyncOperationConfiguredSource);
				}
				PlayerLoopHelper.AddAction(timing, asyncOperationConfiguredSource);
				token = asyncOperationConfiguredSource.core.Version;
				return asyncOperationConfiguredSource;
			}

			// Token: 0x06000B2A RID: 2858 RVA: 0x000282E0 File Offset: 0x000264E0
			public void GetResult(short token)
			{
				try
				{
					this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
			}

			// Token: 0x06000B2B RID: 2859 RVA: 0x0002832C File Offset: 0x0002652C
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000B2C RID: 2860 RVA: 0x0002833A File Offset: 0x0002653A
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000B2D RID: 2861 RVA: 0x00028347 File Offset: 0x00026547
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000B2E RID: 2862 RVA: 0x00028358 File Offset: 0x00026558
			public bool MoveNext()
			{
				if (this.completed || this.asyncOperation == null)
				{
					return false;
				}
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				if (this.progress != null)
				{
					this.progress.Report(this.asyncOperation.progress);
				}
				if (this.asyncOperation.isDone)
				{
					this.core.TrySetResult(AsyncUnit.Default);
					return false;
				}
				return true;
			}

			// Token: 0x06000B2F RID: 2863 RVA: 0x000283D8 File Offset: 0x000265D8
			private bool TryReturn()
			{
				this.core.Reset();
				this.asyncOperation.completed -= this.continuationAction;
				this.asyncOperation = null;
				this.progress = null;
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UnityAsyncExtensions.AsyncOperationConfiguredSource.pool.TryPush(this);
			}

			// Token: 0x06000B30 RID: 2864 RVA: 0x00028438 File Offset: 0x00026638
			private void Continuation(AsyncOperation _)
			{
				if (this.completed)
				{
					return;
				}
				this.completed = true;
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return;
				}
				this.core.TrySetResult(AsyncUnit.Default);
			}

			// Token: 0x0400049C RID: 1180
			private static TaskPool<UnityAsyncExtensions.AsyncOperationConfiguredSource> pool;

			// Token: 0x0400049D RID: 1181
			private UnityAsyncExtensions.AsyncOperationConfiguredSource nextNode;

			// Token: 0x0400049E RID: 1182
			private AsyncOperation asyncOperation;

			// Token: 0x0400049F RID: 1183
			private IProgress<float> progress;

			// Token: 0x040004A0 RID: 1184
			private CancellationToken cancellationToken;

			// Token: 0x040004A1 RID: 1185
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x040004A2 RID: 1186
			private bool cancelImmediately;

			// Token: 0x040004A3 RID: 1187
			private bool completed;

			// Token: 0x040004A4 RID: 1188
			private UniTaskCompletionSourceCore<AsyncUnit> core;

			// Token: 0x040004A5 RID: 1189
			private Action<AsyncOperation> continuationAction;
		}

		// Token: 0x020001F3 RID: 499
		public struct ResourceRequestAwaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x06000B31 RID: 2865 RVA: 0x00028486 File Offset: 0x00026686
			public ResourceRequestAwaiter(ResourceRequest asyncOperation)
			{
				this.asyncOperation = asyncOperation;
				this.continuationAction = null;
			}

			// Token: 0x1700007C RID: 124
			// (get) Token: 0x06000B32 RID: 2866 RVA: 0x00028496 File Offset: 0x00026696
			public bool IsCompleted
			{
				get
				{
					return this.asyncOperation.isDone;
				}
			}

			// Token: 0x06000B33 RID: 2867 RVA: 0x000284A4 File Offset: 0x000266A4
			public Object GetResult()
			{
				if (this.continuationAction != null)
				{
					this.asyncOperation.completed -= this.continuationAction;
					this.continuationAction = null;
					Object asset = this.asyncOperation.asset;
					this.asyncOperation = null;
					return asset;
				}
				Object asset2 = this.asyncOperation.asset;
				this.asyncOperation = null;
				return asset2;
			}

			// Token: 0x06000B34 RID: 2868 RVA: 0x000284F6 File Offset: 0x000266F6
			public void OnCompleted(Action continuation)
			{
				this.UnsafeOnCompleted(continuation);
			}

			// Token: 0x06000B35 RID: 2869 RVA: 0x000284FF File Offset: 0x000266FF
			public void UnsafeOnCompleted(Action continuation)
			{
				Error.ThrowWhenContinuationIsAlreadyRegistered<Action<AsyncOperation>>(this.continuationAction);
				this.continuationAction = PooledDelegate<AsyncOperation>.Create(continuation);
				this.asyncOperation.completed += this.continuationAction;
			}

			// Token: 0x040004A6 RID: 1190
			private ResourceRequest asyncOperation;

			// Token: 0x040004A7 RID: 1191
			private Action<AsyncOperation> continuationAction;
		}

		// Token: 0x020001F4 RID: 500
		private sealed class ResourceRequestConfiguredSource : IUniTaskSource<Object>, IUniTaskSource, IValueTaskSource, IValueTaskSource<Object>, IPlayerLoopItem, ITaskPoolNode<UnityAsyncExtensions.ResourceRequestConfiguredSource>
		{
			// Token: 0x1700007D RID: 125
			// (get) Token: 0x06000B36 RID: 2870 RVA: 0x00028529 File Offset: 0x00026729
			public ref UnityAsyncExtensions.ResourceRequestConfiguredSource NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x06000B37 RID: 2871 RVA: 0x00028531 File Offset: 0x00026731
			static ResourceRequestConfiguredSource()
			{
				TaskPool.RegisterSizeGetter(typeof(UnityAsyncExtensions.ResourceRequestConfiguredSource), () => UnityAsyncExtensions.ResourceRequestConfiguredSource.pool.Size);
			}

			// Token: 0x06000B38 RID: 2872 RVA: 0x00028552 File Offset: 0x00026752
			private ResourceRequestConfiguredSource()
			{
				this.continuationAction = new Action<AsyncOperation>(this.Continuation);
			}

			// Token: 0x06000B39 RID: 2873 RVA: 0x0002856C File Offset: 0x0002676C
			public static IUniTaskSource<Object> Create(ResourceRequest asyncOperation, PlayerLoopTiming timing, IProgress<float> progress, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource<Object>.CreateFromCanceled(cancellationToken, out token);
				}
				UnityAsyncExtensions.ResourceRequestConfiguredSource resourceRequestConfiguredSource;
				if (!UnityAsyncExtensions.ResourceRequestConfiguredSource.pool.TryPop(out resourceRequestConfiguredSource))
				{
					resourceRequestConfiguredSource = new UnityAsyncExtensions.ResourceRequestConfiguredSource();
				}
				resourceRequestConfiguredSource.asyncOperation = asyncOperation;
				resourceRequestConfiguredSource.progress = progress;
				resourceRequestConfiguredSource.cancellationToken = cancellationToken;
				resourceRequestConfiguredSource.cancelImmediately = cancelImmediately;
				resourceRequestConfiguredSource.completed = false;
				asyncOperation.completed += resourceRequestConfiguredSource.continuationAction;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					resourceRequestConfiguredSource.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UnityAsyncExtensions.ResourceRequestConfiguredSource resourceRequestConfiguredSource2 = (UnityAsyncExtensions.ResourceRequestConfiguredSource)state;
						resourceRequestConfiguredSource2.core.TrySetCanceled(resourceRequestConfiguredSource2.cancellationToken);
					}, resourceRequestConfiguredSource);
				}
				PlayerLoopHelper.AddAction(timing, resourceRequestConfiguredSource);
				token = resourceRequestConfiguredSource.core.Version;
				return resourceRequestConfiguredSource;
			}

			// Token: 0x06000B3A RID: 2874 RVA: 0x00028620 File Offset: 0x00026820
			public Object GetResult(short token)
			{
				Object result;
				try
				{
					result = this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
				return result;
			}

			// Token: 0x06000B3B RID: 2875 RVA: 0x0002866C File Offset: 0x0002686C
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000B3C RID: 2876 RVA: 0x00028676 File Offset: 0x00026876
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000B3D RID: 2877 RVA: 0x00028684 File Offset: 0x00026884
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000B3E RID: 2878 RVA: 0x00028691 File Offset: 0x00026891
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000B3F RID: 2879 RVA: 0x000286A4 File Offset: 0x000268A4
			public bool MoveNext()
			{
				if (this.completed || this.asyncOperation == null)
				{
					return false;
				}
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				if (this.progress != null)
				{
					this.progress.Report(this.asyncOperation.progress);
				}
				if (this.asyncOperation.isDone)
				{
					this.core.TrySetResult(this.asyncOperation.asset);
					return false;
				}
				return true;
			}

			// Token: 0x06000B40 RID: 2880 RVA: 0x0002872C File Offset: 0x0002692C
			private bool TryReturn()
			{
				this.core.Reset();
				this.asyncOperation.completed -= this.continuationAction;
				this.asyncOperation = null;
				this.progress = null;
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UnityAsyncExtensions.ResourceRequestConfiguredSource.pool.TryPush(this);
			}

			// Token: 0x06000B41 RID: 2881 RVA: 0x0002878C File Offset: 0x0002698C
			private void Continuation(AsyncOperation _)
			{
				if (this.completed)
				{
					return;
				}
				this.completed = true;
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return;
				}
				this.core.TrySetResult(this.asyncOperation.asset);
			}

			// Token: 0x040004A8 RID: 1192
			private static TaskPool<UnityAsyncExtensions.ResourceRequestConfiguredSource> pool;

			// Token: 0x040004A9 RID: 1193
			private UnityAsyncExtensions.ResourceRequestConfiguredSource nextNode;

			// Token: 0x040004AA RID: 1194
			private ResourceRequest asyncOperation;

			// Token: 0x040004AB RID: 1195
			private IProgress<float> progress;

			// Token: 0x040004AC RID: 1196
			private CancellationToken cancellationToken;

			// Token: 0x040004AD RID: 1197
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x040004AE RID: 1198
			private bool cancelImmediately;

			// Token: 0x040004AF RID: 1199
			private bool completed;

			// Token: 0x040004B0 RID: 1200
			private UniTaskCompletionSourceCore<Object> core;

			// Token: 0x040004B1 RID: 1201
			private Action<AsyncOperation> continuationAction;
		}

		// Token: 0x020001F5 RID: 501
		public struct AssetBundleRequestAwaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x06000B42 RID: 2882 RVA: 0x000287E0 File Offset: 0x000269E0
			public AssetBundleRequestAwaiter(AssetBundleRequest asyncOperation)
			{
				this.asyncOperation = asyncOperation;
				this.continuationAction = null;
			}

			// Token: 0x1700007E RID: 126
			// (get) Token: 0x06000B43 RID: 2883 RVA: 0x000287F0 File Offset: 0x000269F0
			public bool IsCompleted
			{
				get
				{
					return this.asyncOperation.isDone;
				}
			}

			// Token: 0x06000B44 RID: 2884 RVA: 0x00028800 File Offset: 0x00026A00
			public Object GetResult()
			{
				if (this.continuationAction != null)
				{
					this.asyncOperation.completed -= this.continuationAction;
					this.continuationAction = null;
					Object asset = this.asyncOperation.asset;
					this.asyncOperation = null;
					return asset;
				}
				Object asset2 = this.asyncOperation.asset;
				this.asyncOperation = null;
				return asset2;
			}

			// Token: 0x06000B45 RID: 2885 RVA: 0x00028852 File Offset: 0x00026A52
			public void OnCompleted(Action continuation)
			{
				this.UnsafeOnCompleted(continuation);
			}

			// Token: 0x06000B46 RID: 2886 RVA: 0x0002885B File Offset: 0x00026A5B
			public void UnsafeOnCompleted(Action continuation)
			{
				Error.ThrowWhenContinuationIsAlreadyRegistered<Action<AsyncOperation>>(this.continuationAction);
				this.continuationAction = PooledDelegate<AsyncOperation>.Create(continuation);
				this.asyncOperation.completed += this.continuationAction;
			}

			// Token: 0x040004B2 RID: 1202
			private AssetBundleRequest asyncOperation;

			// Token: 0x040004B3 RID: 1203
			private Action<AsyncOperation> continuationAction;
		}

		// Token: 0x020001F6 RID: 502
		private sealed class AssetBundleRequestConfiguredSource : IUniTaskSource<Object>, IUniTaskSource, IValueTaskSource, IValueTaskSource<Object>, IPlayerLoopItem, ITaskPoolNode<UnityAsyncExtensions.AssetBundleRequestConfiguredSource>
		{
			// Token: 0x1700007F RID: 127
			// (get) Token: 0x06000B47 RID: 2887 RVA: 0x00028885 File Offset: 0x00026A85
			public ref UnityAsyncExtensions.AssetBundleRequestConfiguredSource NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x06000B48 RID: 2888 RVA: 0x0002888D File Offset: 0x00026A8D
			static AssetBundleRequestConfiguredSource()
			{
				TaskPool.RegisterSizeGetter(typeof(UnityAsyncExtensions.AssetBundleRequestConfiguredSource), () => UnityAsyncExtensions.AssetBundleRequestConfiguredSource.pool.Size);
			}

			// Token: 0x06000B49 RID: 2889 RVA: 0x000288AE File Offset: 0x00026AAE
			private AssetBundleRequestConfiguredSource()
			{
				this.continuationAction = new Action<AsyncOperation>(this.Continuation);
			}

			// Token: 0x06000B4A RID: 2890 RVA: 0x000288C8 File Offset: 0x00026AC8
			public static IUniTaskSource<Object> Create(AssetBundleRequest asyncOperation, PlayerLoopTiming timing, IProgress<float> progress, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource<Object>.CreateFromCanceled(cancellationToken, out token);
				}
				UnityAsyncExtensions.AssetBundleRequestConfiguredSource assetBundleRequestConfiguredSource;
				if (!UnityAsyncExtensions.AssetBundleRequestConfiguredSource.pool.TryPop(out assetBundleRequestConfiguredSource))
				{
					assetBundleRequestConfiguredSource = new UnityAsyncExtensions.AssetBundleRequestConfiguredSource();
				}
				assetBundleRequestConfiguredSource.asyncOperation = asyncOperation;
				assetBundleRequestConfiguredSource.progress = progress;
				assetBundleRequestConfiguredSource.cancellationToken = cancellationToken;
				assetBundleRequestConfiguredSource.cancelImmediately = cancelImmediately;
				assetBundleRequestConfiguredSource.completed = false;
				asyncOperation.completed += assetBundleRequestConfiguredSource.continuationAction;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					assetBundleRequestConfiguredSource.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UnityAsyncExtensions.AssetBundleRequestConfiguredSource assetBundleRequestConfiguredSource2 = (UnityAsyncExtensions.AssetBundleRequestConfiguredSource)state;
						assetBundleRequestConfiguredSource2.core.TrySetCanceled(assetBundleRequestConfiguredSource2.cancellationToken);
					}, assetBundleRequestConfiguredSource);
				}
				PlayerLoopHelper.AddAction(timing, assetBundleRequestConfiguredSource);
				token = assetBundleRequestConfiguredSource.core.Version;
				return assetBundleRequestConfiguredSource;
			}

			// Token: 0x06000B4B RID: 2891 RVA: 0x0002897C File Offset: 0x00026B7C
			public Object GetResult(short token)
			{
				Object result;
				try
				{
					result = this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
				return result;
			}

			// Token: 0x06000B4C RID: 2892 RVA: 0x000289C8 File Offset: 0x00026BC8
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000B4D RID: 2893 RVA: 0x000289D2 File Offset: 0x00026BD2
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000B4E RID: 2894 RVA: 0x000289E0 File Offset: 0x00026BE0
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000B4F RID: 2895 RVA: 0x000289ED File Offset: 0x00026BED
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000B50 RID: 2896 RVA: 0x00028A00 File Offset: 0x00026C00
			public bool MoveNext()
			{
				if (this.completed || this.asyncOperation == null)
				{
					return false;
				}
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				if (this.progress != null)
				{
					this.progress.Report(this.asyncOperation.progress);
				}
				if (this.asyncOperation.isDone)
				{
					this.core.TrySetResult(this.asyncOperation.asset);
					return false;
				}
				return true;
			}

			// Token: 0x06000B51 RID: 2897 RVA: 0x00028A88 File Offset: 0x00026C88
			private bool TryReturn()
			{
				this.core.Reset();
				this.asyncOperation.completed -= this.continuationAction;
				this.asyncOperation = null;
				this.progress = null;
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UnityAsyncExtensions.AssetBundleRequestConfiguredSource.pool.TryPush(this);
			}

			// Token: 0x06000B52 RID: 2898 RVA: 0x00028AE8 File Offset: 0x00026CE8
			private void Continuation(AsyncOperation _)
			{
				if (this.completed)
				{
					return;
				}
				this.completed = true;
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return;
				}
				this.core.TrySetResult(this.asyncOperation.asset);
			}

			// Token: 0x040004B4 RID: 1204
			private static TaskPool<UnityAsyncExtensions.AssetBundleRequestConfiguredSource> pool;

			// Token: 0x040004B5 RID: 1205
			private UnityAsyncExtensions.AssetBundleRequestConfiguredSource nextNode;

			// Token: 0x040004B6 RID: 1206
			private AssetBundleRequest asyncOperation;

			// Token: 0x040004B7 RID: 1207
			private IProgress<float> progress;

			// Token: 0x040004B8 RID: 1208
			private CancellationToken cancellationToken;

			// Token: 0x040004B9 RID: 1209
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x040004BA RID: 1210
			private bool cancelImmediately;

			// Token: 0x040004BB RID: 1211
			private bool completed;

			// Token: 0x040004BC RID: 1212
			private UniTaskCompletionSourceCore<Object> core;

			// Token: 0x040004BD RID: 1213
			private Action<AsyncOperation> continuationAction;
		}

		// Token: 0x020001F7 RID: 503
		public struct AssetBundleCreateRequestAwaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x06000B53 RID: 2899 RVA: 0x00028B3C File Offset: 0x00026D3C
			public AssetBundleCreateRequestAwaiter(AssetBundleCreateRequest asyncOperation)
			{
				this.asyncOperation = asyncOperation;
				this.continuationAction = null;
			}

			// Token: 0x17000080 RID: 128
			// (get) Token: 0x06000B54 RID: 2900 RVA: 0x00028B4C File Offset: 0x00026D4C
			public bool IsCompleted
			{
				get
				{
					return this.asyncOperation.isDone;
				}
			}

			// Token: 0x06000B55 RID: 2901 RVA: 0x00028B5C File Offset: 0x00026D5C
			public AssetBundle GetResult()
			{
				if (this.continuationAction != null)
				{
					this.asyncOperation.completed -= this.continuationAction;
					this.continuationAction = null;
					AssetBundle assetBundle = this.asyncOperation.assetBundle;
					this.asyncOperation = null;
					return assetBundle;
				}
				AssetBundle assetBundle2 = this.asyncOperation.assetBundle;
				this.asyncOperation = null;
				return assetBundle2;
			}

			// Token: 0x06000B56 RID: 2902 RVA: 0x00028BAE File Offset: 0x00026DAE
			public void OnCompleted(Action continuation)
			{
				this.UnsafeOnCompleted(continuation);
			}

			// Token: 0x06000B57 RID: 2903 RVA: 0x00028BB7 File Offset: 0x00026DB7
			public void UnsafeOnCompleted(Action continuation)
			{
				Error.ThrowWhenContinuationIsAlreadyRegistered<Action<AsyncOperation>>(this.continuationAction);
				this.continuationAction = PooledDelegate<AsyncOperation>.Create(continuation);
				this.asyncOperation.completed += this.continuationAction;
			}

			// Token: 0x040004BE RID: 1214
			private AssetBundleCreateRequest asyncOperation;

			// Token: 0x040004BF RID: 1215
			private Action<AsyncOperation> continuationAction;
		}

		// Token: 0x020001F8 RID: 504
		private sealed class AssetBundleCreateRequestConfiguredSource : IUniTaskSource<AssetBundle>, IUniTaskSource, IValueTaskSource, IValueTaskSource<AssetBundle>, IPlayerLoopItem, ITaskPoolNode<UnityAsyncExtensions.AssetBundleCreateRequestConfiguredSource>
		{
			// Token: 0x17000081 RID: 129
			// (get) Token: 0x06000B58 RID: 2904 RVA: 0x00028BE1 File Offset: 0x00026DE1
			public ref UnityAsyncExtensions.AssetBundleCreateRequestConfiguredSource NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x06000B59 RID: 2905 RVA: 0x00028BE9 File Offset: 0x00026DE9
			static AssetBundleCreateRequestConfiguredSource()
			{
				TaskPool.RegisterSizeGetter(typeof(UnityAsyncExtensions.AssetBundleCreateRequestConfiguredSource), () => UnityAsyncExtensions.AssetBundleCreateRequestConfiguredSource.pool.Size);
			}

			// Token: 0x06000B5A RID: 2906 RVA: 0x00028C0A File Offset: 0x00026E0A
			private AssetBundleCreateRequestConfiguredSource()
			{
				this.continuationAction = new Action<AsyncOperation>(this.Continuation);
			}

			// Token: 0x06000B5B RID: 2907 RVA: 0x00028C24 File Offset: 0x00026E24
			public static IUniTaskSource<AssetBundle> Create(AssetBundleCreateRequest asyncOperation, PlayerLoopTiming timing, IProgress<float> progress, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource<AssetBundle>.CreateFromCanceled(cancellationToken, out token);
				}
				UnityAsyncExtensions.AssetBundleCreateRequestConfiguredSource assetBundleCreateRequestConfiguredSource;
				if (!UnityAsyncExtensions.AssetBundleCreateRequestConfiguredSource.pool.TryPop(out assetBundleCreateRequestConfiguredSource))
				{
					assetBundleCreateRequestConfiguredSource = new UnityAsyncExtensions.AssetBundleCreateRequestConfiguredSource();
				}
				assetBundleCreateRequestConfiguredSource.asyncOperation = asyncOperation;
				assetBundleCreateRequestConfiguredSource.progress = progress;
				assetBundleCreateRequestConfiguredSource.cancellationToken = cancellationToken;
				assetBundleCreateRequestConfiguredSource.cancelImmediately = cancelImmediately;
				assetBundleCreateRequestConfiguredSource.completed = false;
				asyncOperation.completed += assetBundleCreateRequestConfiguredSource.continuationAction;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					assetBundleCreateRequestConfiguredSource.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UnityAsyncExtensions.AssetBundleCreateRequestConfiguredSource assetBundleCreateRequestConfiguredSource2 = (UnityAsyncExtensions.AssetBundleCreateRequestConfiguredSource)state;
						assetBundleCreateRequestConfiguredSource2.core.TrySetCanceled(assetBundleCreateRequestConfiguredSource2.cancellationToken);
					}, assetBundleCreateRequestConfiguredSource);
				}
				PlayerLoopHelper.AddAction(timing, assetBundleCreateRequestConfiguredSource);
				token = assetBundleCreateRequestConfiguredSource.core.Version;
				return assetBundleCreateRequestConfiguredSource;
			}

			// Token: 0x06000B5C RID: 2908 RVA: 0x00028CD8 File Offset: 0x00026ED8
			public AssetBundle GetResult(short token)
			{
				AssetBundle result;
				try
				{
					result = this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
				return result;
			}

			// Token: 0x06000B5D RID: 2909 RVA: 0x00028D24 File Offset: 0x00026F24
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000B5E RID: 2910 RVA: 0x00028D2E File Offset: 0x00026F2E
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000B5F RID: 2911 RVA: 0x00028D3C File Offset: 0x00026F3C
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000B60 RID: 2912 RVA: 0x00028D49 File Offset: 0x00026F49
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000B61 RID: 2913 RVA: 0x00028D5C File Offset: 0x00026F5C
			public bool MoveNext()
			{
				if (this.completed || this.asyncOperation == null)
				{
					return false;
				}
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				if (this.progress != null)
				{
					this.progress.Report(this.asyncOperation.progress);
				}
				if (this.asyncOperation.isDone)
				{
					this.core.TrySetResult(this.asyncOperation.assetBundle);
					return false;
				}
				return true;
			}

			// Token: 0x06000B62 RID: 2914 RVA: 0x00028DE4 File Offset: 0x00026FE4
			private bool TryReturn()
			{
				this.core.Reset();
				this.asyncOperation.completed -= this.continuationAction;
				this.asyncOperation = null;
				this.progress = null;
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UnityAsyncExtensions.AssetBundleCreateRequestConfiguredSource.pool.TryPush(this);
			}

			// Token: 0x06000B63 RID: 2915 RVA: 0x00028E44 File Offset: 0x00027044
			private void Continuation(AsyncOperation _)
			{
				if (this.completed)
				{
					return;
				}
				this.completed = true;
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return;
				}
				this.core.TrySetResult(this.asyncOperation.assetBundle);
			}

			// Token: 0x040004C0 RID: 1216
			private static TaskPool<UnityAsyncExtensions.AssetBundleCreateRequestConfiguredSource> pool;

			// Token: 0x040004C1 RID: 1217
			private UnityAsyncExtensions.AssetBundleCreateRequestConfiguredSource nextNode;

			// Token: 0x040004C2 RID: 1218
			private AssetBundleCreateRequest asyncOperation;

			// Token: 0x040004C3 RID: 1219
			private IProgress<float> progress;

			// Token: 0x040004C4 RID: 1220
			private CancellationToken cancellationToken;

			// Token: 0x040004C5 RID: 1221
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x040004C6 RID: 1222
			private bool cancelImmediately;

			// Token: 0x040004C7 RID: 1223
			private bool completed;

			// Token: 0x040004C8 RID: 1224
			private UniTaskCompletionSourceCore<AssetBundle> core;

			// Token: 0x040004C9 RID: 1225
			private Action<AsyncOperation> continuationAction;
		}

		// Token: 0x020001F9 RID: 505
		public struct UnityWebRequestAsyncOperationAwaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x06000B64 RID: 2916 RVA: 0x00028E98 File Offset: 0x00027098
			public UnityWebRequestAsyncOperationAwaiter(UnityWebRequestAsyncOperation asyncOperation)
			{
				this.asyncOperation = asyncOperation;
				this.continuationAction = null;
			}

			// Token: 0x17000082 RID: 130
			// (get) Token: 0x06000B65 RID: 2917 RVA: 0x00028EA8 File Offset: 0x000270A8
			public bool IsCompleted
			{
				get
				{
					return this.asyncOperation.isDone;
				}
			}

			// Token: 0x06000B66 RID: 2918 RVA: 0x00028EB8 File Offset: 0x000270B8
			public UnityWebRequest GetResult()
			{
				if (this.continuationAction != null)
				{
					this.asyncOperation.completed -= this.continuationAction;
					this.continuationAction = null;
					UnityWebRequest webRequest = this.asyncOperation.webRequest;
					this.asyncOperation = null;
					if (webRequest.IsError())
					{
						throw new UnityWebRequestException(webRequest);
					}
					return webRequest;
				}
				else
				{
					UnityWebRequest webRequest2 = this.asyncOperation.webRequest;
					this.asyncOperation = null;
					if (webRequest2.IsError())
					{
						throw new UnityWebRequestException(webRequest2);
					}
					return webRequest2;
				}
			}

			// Token: 0x06000B67 RID: 2919 RVA: 0x00028F2C File Offset: 0x0002712C
			public void OnCompleted(Action continuation)
			{
				this.UnsafeOnCompleted(continuation);
			}

			// Token: 0x06000B68 RID: 2920 RVA: 0x00028F35 File Offset: 0x00027135
			public void UnsafeOnCompleted(Action continuation)
			{
				Error.ThrowWhenContinuationIsAlreadyRegistered<Action<AsyncOperation>>(this.continuationAction);
				this.continuationAction = PooledDelegate<AsyncOperation>.Create(continuation);
				this.asyncOperation.completed += this.continuationAction;
			}

			// Token: 0x040004CA RID: 1226
			private UnityWebRequestAsyncOperation asyncOperation;

			// Token: 0x040004CB RID: 1227
			private Action<AsyncOperation> continuationAction;
		}

		// Token: 0x020001FA RID: 506
		private sealed class UnityWebRequestAsyncOperationConfiguredSource : IUniTaskSource<UnityWebRequest>, IUniTaskSource, IValueTaskSource, IValueTaskSource<UnityWebRequest>, IPlayerLoopItem, ITaskPoolNode<UnityAsyncExtensions.UnityWebRequestAsyncOperationConfiguredSource>
		{
			// Token: 0x17000083 RID: 131
			// (get) Token: 0x06000B69 RID: 2921 RVA: 0x00028F5F File Offset: 0x0002715F
			public ref UnityAsyncExtensions.UnityWebRequestAsyncOperationConfiguredSource NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x06000B6A RID: 2922 RVA: 0x00028F67 File Offset: 0x00027167
			static UnityWebRequestAsyncOperationConfiguredSource()
			{
				TaskPool.RegisterSizeGetter(typeof(UnityAsyncExtensions.UnityWebRequestAsyncOperationConfiguredSource), () => UnityAsyncExtensions.UnityWebRequestAsyncOperationConfiguredSource.pool.Size);
			}

			// Token: 0x06000B6B RID: 2923 RVA: 0x00028F88 File Offset: 0x00027188
			private UnityWebRequestAsyncOperationConfiguredSource()
			{
				this.continuationAction = new Action<AsyncOperation>(this.Continuation);
			}

			// Token: 0x06000B6C RID: 2924 RVA: 0x00028FA4 File Offset: 0x000271A4
			public static IUniTaskSource<UnityWebRequest> Create(UnityWebRequestAsyncOperation asyncOperation, PlayerLoopTiming timing, IProgress<float> progress, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource<UnityWebRequest>.CreateFromCanceled(cancellationToken, out token);
				}
				UnityAsyncExtensions.UnityWebRequestAsyncOperationConfiguredSource unityWebRequestAsyncOperationConfiguredSource;
				if (!UnityAsyncExtensions.UnityWebRequestAsyncOperationConfiguredSource.pool.TryPop(out unityWebRequestAsyncOperationConfiguredSource))
				{
					unityWebRequestAsyncOperationConfiguredSource = new UnityAsyncExtensions.UnityWebRequestAsyncOperationConfiguredSource();
				}
				unityWebRequestAsyncOperationConfiguredSource.asyncOperation = asyncOperation;
				unityWebRequestAsyncOperationConfiguredSource.progress = progress;
				unityWebRequestAsyncOperationConfiguredSource.cancellationToken = cancellationToken;
				unityWebRequestAsyncOperationConfiguredSource.cancelImmediately = cancelImmediately;
				unityWebRequestAsyncOperationConfiguredSource.completed = false;
				asyncOperation.completed += unityWebRequestAsyncOperationConfiguredSource.continuationAction;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					unityWebRequestAsyncOperationConfiguredSource.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						UnityAsyncExtensions.UnityWebRequestAsyncOperationConfiguredSource unityWebRequestAsyncOperationConfiguredSource2 = (UnityAsyncExtensions.UnityWebRequestAsyncOperationConfiguredSource)state;
						unityWebRequestAsyncOperationConfiguredSource2.asyncOperation.webRequest.Abort();
						unityWebRequestAsyncOperationConfiguredSource2.core.TrySetCanceled(unityWebRequestAsyncOperationConfiguredSource2.cancellationToken);
					}, unityWebRequestAsyncOperationConfiguredSource);
				}
				PlayerLoopHelper.AddAction(timing, unityWebRequestAsyncOperationConfiguredSource);
				token = unityWebRequestAsyncOperationConfiguredSource.core.Version;
				return unityWebRequestAsyncOperationConfiguredSource;
			}

			// Token: 0x06000B6D RID: 2925 RVA: 0x00029058 File Offset: 0x00027258
			public UnityWebRequest GetResult(short token)
			{
				UnityWebRequest result;
				try
				{
					result = this.core.GetResult(token);
				}
				finally
				{
					if (!this.cancelImmediately || !this.cancellationToken.IsCancellationRequested)
					{
						this.TryReturn();
					}
				}
				return result;
			}

			// Token: 0x06000B6E RID: 2926 RVA: 0x000290A4 File Offset: 0x000272A4
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000B6F RID: 2927 RVA: 0x000290AE File Offset: 0x000272AE
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000B70 RID: 2928 RVA: 0x000290BC File Offset: 0x000272BC
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000B71 RID: 2929 RVA: 0x000290C9 File Offset: 0x000272C9
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000B72 RID: 2930 RVA: 0x000290DC File Offset: 0x000272DC
			public bool MoveNext()
			{
				if (this.completed || this.asyncOperation == null)
				{
					return false;
				}
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.asyncOperation.webRequest.Abort();
					this.core.TrySetCanceled(this.cancellationToken);
					return false;
				}
				if (this.progress != null)
				{
					this.progress.Report(this.asyncOperation.progress);
				}
				if (this.asyncOperation.isDone)
				{
					if (this.asyncOperation.webRequest.IsError())
					{
						this.core.TrySetException(new UnityWebRequestException(this.asyncOperation.webRequest));
					}
					else
					{
						this.core.TrySetResult(this.asyncOperation.webRequest);
					}
					return false;
				}
				return true;
			}

			// Token: 0x06000B73 RID: 2931 RVA: 0x000291A4 File Offset: 0x000273A4
			private bool TryReturn()
			{
				this.core.Reset();
				this.asyncOperation.completed -= this.continuationAction;
				this.asyncOperation = null;
				this.progress = null;
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return UnityAsyncExtensions.UnityWebRequestAsyncOperationConfiguredSource.pool.TryPush(this);
			}

			// Token: 0x06000B74 RID: 2932 RVA: 0x00029204 File Offset: 0x00027404
			private void Continuation(AsyncOperation _)
			{
				if (this.completed)
				{
					return;
				}
				this.completed = true;
				if (this.cancellationToken.IsCancellationRequested)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return;
				}
				if (this.asyncOperation.webRequest.IsError())
				{
					this.core.TrySetException(new UnityWebRequestException(this.asyncOperation.webRequest));
					return;
				}
				this.core.TrySetResult(this.asyncOperation.webRequest);
			}

			// Token: 0x040004CC RID: 1228
			private static TaskPool<UnityAsyncExtensions.UnityWebRequestAsyncOperationConfiguredSource> pool;

			// Token: 0x040004CD RID: 1229
			private UnityAsyncExtensions.UnityWebRequestAsyncOperationConfiguredSource nextNode;

			// Token: 0x040004CE RID: 1230
			private UnityWebRequestAsyncOperation asyncOperation;

			// Token: 0x040004CF RID: 1231
			private IProgress<float> progress;

			// Token: 0x040004D0 RID: 1232
			private CancellationToken cancellationToken;

			// Token: 0x040004D1 RID: 1233
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x040004D2 RID: 1234
			private bool cancelImmediately;

			// Token: 0x040004D3 RID: 1235
			private bool completed;

			// Token: 0x040004D4 RID: 1236
			private UniTaskCompletionSourceCore<UnityWebRequest> core;

			// Token: 0x040004D5 RID: 1237
			private Action<AsyncOperation> continuationAction;
		}

		// Token: 0x020001FB RID: 507
		private sealed class JobHandlePromise : IUniTaskSource, IValueTaskSource, IPlayerLoopItem
		{
			// Token: 0x06000B75 RID: 2933 RVA: 0x00029288 File Offset: 0x00027488
			public static UnityAsyncExtensions.JobHandlePromise Create(JobHandle jobHandle, out short token)
			{
				UnityAsyncExtensions.JobHandlePromise jobHandlePromise = new UnityAsyncExtensions.JobHandlePromise();
				jobHandlePromise.jobHandle = jobHandle;
				token = jobHandlePromise.core.Version;
				return jobHandlePromise;
			}

			// Token: 0x06000B76 RID: 2934 RVA: 0x000292B0 File Offset: 0x000274B0
			public void GetResult(short token)
			{
				this.core.GetResult(token);
			}

			// Token: 0x06000B77 RID: 2935 RVA: 0x000292BF File Offset: 0x000274BF
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000B78 RID: 2936 RVA: 0x000292CD File Offset: 0x000274CD
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000B79 RID: 2937 RVA: 0x000292DA File Offset: 0x000274DA
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000B7A RID: 2938 RVA: 0x000292EA File Offset: 0x000274EA
			public bool MoveNext()
			{
				if (this.jobHandle.IsCompleted | PlayerLoopHelper.IsEditorApplicationQuitting)
				{
					this.jobHandle.Complete();
					this.core.TrySetResult(AsyncUnit.Default);
					return false;
				}
				return true;
			}

			// Token: 0x040004D6 RID: 1238
			private JobHandle jobHandle;

			// Token: 0x040004D7 RID: 1239
			private UniTaskCompletionSourceCore<AsyncUnit> core;
		}
	}
}
