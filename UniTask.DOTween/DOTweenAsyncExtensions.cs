using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks.Sources;
using Cysharp.Threading.Tasks.Internal;
using DG.Tweening;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000004 RID: 4
	public static class DOTweenAsyncExtensions
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020BB File Offset: 0x000002BB
		public static DOTweenAsyncExtensions.TweenAwaiter GetAwaiter(this Tween tween)
		{
			return new DOTweenAsyncExtensions.TweenAwaiter(tween);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020C4 File Offset: 0x000002C4
		public static UniTask WithCancellation(this Tween tween, CancellationToken cancellationToken)
		{
			Error.ThrowArgumentNullException<Tween>(tween, "tween");
			if (!TweenExtensions.IsActive(tween))
			{
				return UniTask.CompletedTask;
			}
			short token;
			return new UniTask(DOTweenAsyncExtensions.TweenConfiguredSource.Create(tween, TweenCancelBehaviour.Kill, cancellationToken, DOTweenAsyncExtensions.CallbackType.Kill, out token), token);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020FC File Offset: 0x000002FC
		public static UniTask ToUniTask(this Tween tween, TweenCancelBehaviour tweenCancelBehaviour = TweenCancelBehaviour.Kill, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<Tween>(tween, "tween");
			if (!TweenExtensions.IsActive(tween))
			{
				return UniTask.CompletedTask;
			}
			short token;
			return new UniTask(DOTweenAsyncExtensions.TweenConfiguredSource.Create(tween, tweenCancelBehaviour, cancellationToken, DOTweenAsyncExtensions.CallbackType.Kill, out token), token);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002134 File Offset: 0x00000334
		public static UniTask AwaitForComplete(this Tween tween, TweenCancelBehaviour tweenCancelBehaviour = TweenCancelBehaviour.Kill, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<Tween>(tween, "tween");
			if (!TweenExtensions.IsActive(tween))
			{
				return UniTask.CompletedTask;
			}
			short token;
			return new UniTask(DOTweenAsyncExtensions.TweenConfiguredSource.Create(tween, tweenCancelBehaviour, cancellationToken, DOTweenAsyncExtensions.CallbackType.Complete, out token), token);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000216C File Offset: 0x0000036C
		public static UniTask AwaitForPause(this Tween tween, TweenCancelBehaviour tweenCancelBehaviour = TweenCancelBehaviour.Kill, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<Tween>(tween, "tween");
			if (!TweenExtensions.IsActive(tween))
			{
				return UniTask.CompletedTask;
			}
			short token;
			return new UniTask(DOTweenAsyncExtensions.TweenConfiguredSource.Create(tween, tweenCancelBehaviour, cancellationToken, DOTweenAsyncExtensions.CallbackType.Pause, out token), token);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021A4 File Offset: 0x000003A4
		public static UniTask AwaitForPlay(this Tween tween, TweenCancelBehaviour tweenCancelBehaviour = TweenCancelBehaviour.Kill, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<Tween>(tween, "tween");
			if (!TweenExtensions.IsActive(tween))
			{
				return UniTask.CompletedTask;
			}
			short token;
			return new UniTask(DOTweenAsyncExtensions.TweenConfiguredSource.Create(tween, tweenCancelBehaviour, cancellationToken, DOTweenAsyncExtensions.CallbackType.Play, out token), token);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021DC File Offset: 0x000003DC
		public static UniTask AwaitForRewind(this Tween tween, TweenCancelBehaviour tweenCancelBehaviour = TweenCancelBehaviour.Kill, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<Tween>(tween, "tween");
			if (!TweenExtensions.IsActive(tween))
			{
				return UniTask.CompletedTask;
			}
			short token;
			return new UniTask(DOTweenAsyncExtensions.TweenConfiguredSource.Create(tween, tweenCancelBehaviour, cancellationToken, DOTweenAsyncExtensions.CallbackType.Rewind, out token), token);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002214 File Offset: 0x00000414
		public static UniTask AwaitForStepComplete(this Tween tween, TweenCancelBehaviour tweenCancelBehaviour = TweenCancelBehaviour.Kill, CancellationToken cancellationToken = default(CancellationToken))
		{
			Error.ThrowArgumentNullException<Tween>(tween, "tween");
			if (!TweenExtensions.IsActive(tween))
			{
				return UniTask.CompletedTask;
			}
			short token;
			return new UniTask(DOTweenAsyncExtensions.TweenConfiguredSource.Create(tween, tweenCancelBehaviour, cancellationToken, DOTweenAsyncExtensions.CallbackType.StepComplete, out token), token);
		}

		// Token: 0x02000008 RID: 8
		private enum CallbackType
		{
			// Token: 0x04000016 RID: 22
			Kill,
			// Token: 0x04000017 RID: 23
			Complete,
			// Token: 0x04000018 RID: 24
			Pause,
			// Token: 0x04000019 RID: 25
			Play,
			// Token: 0x0400001A RID: 26
			Rewind,
			// Token: 0x0400001B RID: 27
			StepComplete
		}

		// Token: 0x02000009 RID: 9
		public struct TweenAwaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x17000001 RID: 1
			// (get) Token: 0x0600000F RID: 15 RVA: 0x000022D3 File Offset: 0x000004D3
			public bool IsCompleted
			{
				get
				{
					return !TweenExtensions.IsActive(this.tween);
				}
			}

			// Token: 0x06000010 RID: 16 RVA: 0x000022E3 File Offset: 0x000004E3
			public TweenAwaiter(Tween tween)
			{
				this.tween = tween;
			}

			// Token: 0x06000011 RID: 17 RVA: 0x000022EC File Offset: 0x000004EC
			public DOTweenAsyncExtensions.TweenAwaiter GetAwaiter()
			{
				return this;
			}

			// Token: 0x06000012 RID: 18 RVA: 0x000022F4 File Offset: 0x000004F4
			public void GetResult()
			{
			}

			// Token: 0x06000013 RID: 19 RVA: 0x000022F6 File Offset: 0x000004F6
			public void OnCompleted(Action continuation)
			{
				this.UnsafeOnCompleted(continuation);
			}

			// Token: 0x06000014 RID: 20 RVA: 0x000022FF File Offset: 0x000004FF
			public void UnsafeOnCompleted(Action continuation)
			{
				this.tween.onKill = PooledTweenCallback.Create(continuation);
			}

			// Token: 0x0400001C RID: 28
			private readonly Tween tween;
		}

		// Token: 0x0200000A RID: 10
		private sealed class TweenConfiguredSource : IUniTaskSource, IValueTaskSource, ITaskPoolNode<DOTweenAsyncExtensions.TweenConfiguredSource>
		{
			// Token: 0x17000002 RID: 2
			// (get) Token: 0x06000015 RID: 21 RVA: 0x00002312 File Offset: 0x00000512
			public ref DOTweenAsyncExtensions.TweenConfiguredSource NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x06000016 RID: 22 RVA: 0x0000231A File Offset: 0x0000051A
			static TweenConfiguredSource()
			{
				TaskPool.RegisterSizeGetter(typeof(DOTweenAsyncExtensions.TweenConfiguredSource), () => DOTweenAsyncExtensions.TweenConfiguredSource.pool.Size);
			}

			// Token: 0x06000017 RID: 23 RVA: 0x0000233B File Offset: 0x0000053B
			private TweenConfiguredSource()
			{
				this.onCompleteCallbackDelegate = new TweenCallback(this.OnCompleteCallbackDelegate);
			}

			// Token: 0x06000018 RID: 24 RVA: 0x00002358 File Offset: 0x00000558
			public static IUniTaskSource Create(Tween tween, TweenCancelBehaviour cancelBehaviour, CancellationToken cancellationToken, DOTweenAsyncExtensions.CallbackType callbackType, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					DOTweenAsyncExtensions.TweenConfiguredSource.DoCancelBeforeCreate(tween, cancelBehaviour);
					return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
				}
				DOTweenAsyncExtensions.TweenConfiguredSource tweenConfiguredSource;
				if (!DOTweenAsyncExtensions.TweenConfiguredSource.pool.TryPop(out tweenConfiguredSource))
				{
					tweenConfiguredSource = new DOTweenAsyncExtensions.TweenConfiguredSource();
				}
				tweenConfiguredSource.tween = tween;
				tweenConfiguredSource.cancelBehaviour = cancelBehaviour;
				tweenConfiguredSource.cancellationToken = cancellationToken;
				tweenConfiguredSource.callbackType = callbackType;
				tweenConfiguredSource.canceled = false;
				switch (callbackType)
				{
				case DOTweenAsyncExtensions.CallbackType.Kill:
					tweenConfiguredSource.originalCompleteAction = tween.onKill;
					tween.onKill = tweenConfiguredSource.onCompleteCallbackDelegate;
					break;
				case DOTweenAsyncExtensions.CallbackType.Complete:
					tweenConfiguredSource.originalCompleteAction = tween.onComplete;
					tween.onComplete = tweenConfiguredSource.onCompleteCallbackDelegate;
					break;
				case DOTweenAsyncExtensions.CallbackType.Pause:
					tweenConfiguredSource.originalCompleteAction = tween.onPause;
					tween.onPause = tweenConfiguredSource.onCompleteCallbackDelegate;
					break;
				case DOTweenAsyncExtensions.CallbackType.Play:
					tweenConfiguredSource.originalCompleteAction = tween.onPlay;
					tween.onPlay = tweenConfiguredSource.onCompleteCallbackDelegate;
					break;
				case DOTweenAsyncExtensions.CallbackType.Rewind:
					tweenConfiguredSource.originalCompleteAction = tween.onRewind;
					tween.onRewind = tweenConfiguredSource.onCompleteCallbackDelegate;
					break;
				case DOTweenAsyncExtensions.CallbackType.StepComplete:
					tweenConfiguredSource.originalCompleteAction = tween.onStepComplete;
					tween.onStepComplete = tweenConfiguredSource.onCompleteCallbackDelegate;
					break;
				}
				if (tweenConfiguredSource.originalCompleteAction == tweenConfiguredSource.onCompleteCallbackDelegate)
				{
					tweenConfiguredSource.originalCompleteAction = null;
				}
				if (cancellationToken.CanBeCanceled)
				{
					tweenConfiguredSource.cancellationRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object x)
					{
						DOTweenAsyncExtensions.TweenConfiguredSource tweenConfiguredSource2 = (DOTweenAsyncExtensions.TweenConfiguredSource)x;
						switch (tweenConfiguredSource2.cancelBehaviour)
						{
						default:
							TweenExtensions.Kill(tweenConfiguredSource2.tween, false);
							return;
						case TweenCancelBehaviour.KillWithCompleteCallback:
							TweenExtensions.Kill(tweenConfiguredSource2.tween, true);
							return;
						case TweenCancelBehaviour.Complete:
							TweenExtensions.Complete(tweenConfiguredSource2.tween, false);
							return;
						case TweenCancelBehaviour.CompleteWithSequenceCallback:
							TweenExtensions.Complete(tweenConfiguredSource2.tween, true);
							return;
						case TweenCancelBehaviour.CancelAwait:
							tweenConfiguredSource2.RestoreOriginalCallback();
							tweenConfiguredSource2.core.TrySetCanceled(tweenConfiguredSource2.cancellationToken);
							return;
						case TweenCancelBehaviour.KillAndCancelAwait:
							tweenConfiguredSource2.canceled = true;
							TweenExtensions.Kill(tweenConfiguredSource2.tween, false);
							return;
						case TweenCancelBehaviour.KillWithCompleteCallbackAndCancelAwait:
							tweenConfiguredSource2.canceled = true;
							TweenExtensions.Kill(tweenConfiguredSource2.tween, true);
							return;
						case TweenCancelBehaviour.CompleteAndCancelAwait:
							tweenConfiguredSource2.canceled = true;
							TweenExtensions.Complete(tweenConfiguredSource2.tween, false);
							return;
						case TweenCancelBehaviour.CompleteWithSequenceCallbackAndCancelAwait:
							tweenConfiguredSource2.canceled = true;
							TweenExtensions.Complete(tweenConfiguredSource2.tween, true);
							return;
						}
					}, tweenConfiguredSource);
				}
				token = tweenConfiguredSource.core.Version;
				return tweenConfiguredSource;
			}

			// Token: 0x06000019 RID: 25 RVA: 0x000024D4 File Offset: 0x000006D4
			private void OnCompleteCallbackDelegate()
			{
				if (this.cancellationToken.IsCancellationRequested && (this.cancelBehaviour == TweenCancelBehaviour.KillAndCancelAwait || this.cancelBehaviour == TweenCancelBehaviour.KillWithCompleteCallbackAndCancelAwait || this.cancelBehaviour == TweenCancelBehaviour.CompleteAndCancelAwait || this.cancelBehaviour == TweenCancelBehaviour.CompleteWithSequenceCallbackAndCancelAwait || this.cancelBehaviour == TweenCancelBehaviour.CancelAwait))
				{
					this.canceled = true;
				}
				if (this.canceled)
				{
					this.core.TrySetCanceled(this.cancellationToken);
					return;
				}
				TweenCallback tweenCallback = this.originalCompleteAction;
				if (tweenCallback != null)
				{
					tweenCallback.Invoke();
				}
				this.core.TrySetResult(AsyncUnit.Default);
			}

			// Token: 0x0600001A RID: 26 RVA: 0x00002560 File Offset: 0x00000760
			private static void DoCancelBeforeCreate(Tween tween, TweenCancelBehaviour tweenCancelBehaviour)
			{
				switch (tweenCancelBehaviour)
				{
				default:
					TweenExtensions.Kill(tween, false);
					return;
				case TweenCancelBehaviour.KillWithCompleteCallback:
					TweenExtensions.Kill(tween, true);
					return;
				case TweenCancelBehaviour.Complete:
					TweenExtensions.Complete(tween, false);
					return;
				case TweenCancelBehaviour.CompleteWithSequenceCallback:
					TweenExtensions.Complete(tween, true);
					return;
				case TweenCancelBehaviour.CancelAwait:
					break;
				case TweenCancelBehaviour.KillAndCancelAwait:
					TweenExtensions.Kill(tween, false);
					return;
				case TweenCancelBehaviour.KillWithCompleteCallbackAndCancelAwait:
					TweenExtensions.Kill(tween, true);
					return;
				case TweenCancelBehaviour.CompleteAndCancelAwait:
					TweenExtensions.Complete(tween, false);
					return;
				case TweenCancelBehaviour.CompleteWithSequenceCallbackAndCancelAwait:
					TweenExtensions.Complete(tween, true);
					break;
				}
			}

			// Token: 0x0600001B RID: 27 RVA: 0x000025D8 File Offset: 0x000007D8
			public void GetResult(short token)
			{
				try
				{
					this.core.GetResult(token);
				}
				finally
				{
					this.TryReturn();
				}
			}

			// Token: 0x0600001C RID: 28 RVA: 0x0000260C File Offset: 0x0000080C
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x0600001D RID: 29 RVA: 0x0000261A File Offset: 0x0000081A
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x0600001E RID: 30 RVA: 0x00002627 File Offset: 0x00000827
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x0600001F RID: 31 RVA: 0x00002638 File Offset: 0x00000838
			private bool TryReturn()
			{
				this.core.Reset();
				this.cancellationRegistration.Dispose();
				this.RestoreOriginalCallback();
				this.tween = null;
				this.cancellationToken = default(CancellationToken);
				this.originalCompleteAction = null;
				return DOTweenAsyncExtensions.TweenConfiguredSource.pool.TryPush(this);
			}

			// Token: 0x06000020 RID: 32 RVA: 0x00002688 File Offset: 0x00000888
			private void RestoreOriginalCallback()
			{
				switch (this.callbackType)
				{
				case DOTweenAsyncExtensions.CallbackType.Kill:
					this.tween.onKill = this.originalCompleteAction;
					return;
				case DOTweenAsyncExtensions.CallbackType.Complete:
					this.tween.onComplete = this.originalCompleteAction;
					return;
				case DOTweenAsyncExtensions.CallbackType.Pause:
					this.tween.onPause = this.originalCompleteAction;
					return;
				case DOTweenAsyncExtensions.CallbackType.Play:
					this.tween.onPlay = this.originalCompleteAction;
					return;
				case DOTweenAsyncExtensions.CallbackType.Rewind:
					this.tween.onRewind = this.originalCompleteAction;
					return;
				case DOTweenAsyncExtensions.CallbackType.StepComplete:
					this.tween.onStepComplete = this.originalCompleteAction;
					return;
				default:
					return;
				}
			}

			// Token: 0x0400001D RID: 29
			private static TaskPool<DOTweenAsyncExtensions.TweenConfiguredSource> pool;

			// Token: 0x0400001E RID: 30
			private DOTweenAsyncExtensions.TweenConfiguredSource nextNode;

			// Token: 0x0400001F RID: 31
			private readonly TweenCallback onCompleteCallbackDelegate;

			// Token: 0x04000020 RID: 32
			private Tween tween;

			// Token: 0x04000021 RID: 33
			private TweenCancelBehaviour cancelBehaviour;

			// Token: 0x04000022 RID: 34
			private CancellationToken cancellationToken;

			// Token: 0x04000023 RID: 35
			private CancellationTokenRegistration cancellationRegistration;

			// Token: 0x04000024 RID: 36
			private DOTweenAsyncExtensions.CallbackType callbackType;

			// Token: 0x04000025 RID: 37
			private bool canceled;

			// Token: 0x04000026 RID: 38
			private TweenCallback originalCompleteAction;

			// Token: 0x04000027 RID: 39
			private UniTaskCompletionSourceCore<AsyncUnit> core;
		}
	}
}
