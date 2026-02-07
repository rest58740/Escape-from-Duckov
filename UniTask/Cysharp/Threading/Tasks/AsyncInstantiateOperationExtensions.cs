using System;
using System.Threading;
using System.Threading.Tasks.Sources;
using Cysharp.Threading.Tasks.Internal;
using UnityEngine;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200005B RID: 91
	public static class AsyncInstantiateOperationExtensions
	{
		// Token: 0x06000296 RID: 662 RVA: 0x00009EBF File Offset: 0x000080BF
		public static UniTask<Object[]> WithCancellation<T>(this AsyncInstantiateOperation asyncOperation, CancellationToken cancellationToken)
		{
			return asyncOperation.ToUniTask(null, PlayerLoopTiming.Update, cancellationToken, false);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00009ECB File Offset: 0x000080CB
		public static UniTask<Object[]> WithCancellation<T>(this AsyncInstantiateOperation asyncOperation, CancellationToken cancellationToken, bool cancelImmediately)
		{
			return asyncOperation.ToUniTask(null, PlayerLoopTiming.Update, cancellationToken, cancelImmediately);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00009ED8 File Offset: 0x000080D8
		public static UniTask<Object[]> ToUniTask(this AsyncInstantiateOperation asyncOperation, IProgress<float> progress = null, PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false)
		{
			Error.ThrowArgumentNullException<AsyncInstantiateOperation>(asyncOperation, "asyncOperation");
			if (cancellationToken.IsCancellationRequested)
			{
				return UniTask.FromCanceled<Object[]>(cancellationToken);
			}
			if (asyncOperation.isDone)
			{
				return UniTask.FromResult<Object[]>(asyncOperation.Result);
			}
			short token;
			return new UniTask<Object[]>(AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource.Create(asyncOperation, timing, progress, cancellationToken, cancelImmediately, out token), token);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00009F27 File Offset: 0x00008127
		public static UniTask<T[]> WithCancellation<T>(this AsyncInstantiateOperation<T> asyncOperation, CancellationToken cancellationToken) where T : Object
		{
			return asyncOperation.ToUniTask(null, PlayerLoopTiming.Update, cancellationToken, false);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00009F33 File Offset: 0x00008133
		public static UniTask<T[]> WithCancellation<T>(this AsyncInstantiateOperation<T> asyncOperation, CancellationToken cancellationToken, bool cancelImmediately) where T : Object
		{
			return asyncOperation.ToUniTask(null, PlayerLoopTiming.Update, cancellationToken, cancelImmediately);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00009F40 File Offset: 0x00008140
		public static UniTask<T[]> ToUniTask<T>(this AsyncInstantiateOperation<T> asyncOperation, IProgress<float> progress = null, PlayerLoopTiming timing = PlayerLoopTiming.Update, CancellationToken cancellationToken = default(CancellationToken), bool cancelImmediately = false) where T : Object
		{
			Error.ThrowArgumentNullException<AsyncInstantiateOperation<T>>(asyncOperation, "asyncOperation");
			if (cancellationToken.IsCancellationRequested)
			{
				return UniTask.FromCanceled<T[]>(cancellationToken);
			}
			if (asyncOperation.isDone)
			{
				return UniTask.FromResult<T[]>(asyncOperation.Result);
			}
			short token;
			return new UniTask<T[]>(AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource<T>.Create(asyncOperation, timing, progress, cancellationToken, cancelImmediately, out token), token);
		}

		// Token: 0x020001FD RID: 509
		private sealed class AsyncInstantiateOperationConfiguredSource : IUniTaskSource<Object[]>, IUniTaskSource, IValueTaskSource, IValueTaskSource<Object[]>, IPlayerLoopItem, ITaskPoolNode<AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource>
		{
			// Token: 0x17000084 RID: 132
			// (get) Token: 0x06000B7E RID: 2942 RVA: 0x00029402 File Offset: 0x00027602
			public ref AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x06000B7F RID: 2943 RVA: 0x0002940A File Offset: 0x0002760A
			static AsyncInstantiateOperationConfiguredSource()
			{
				TaskPool.RegisterSizeGetter(typeof(AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource), () => AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource.pool.Size);
			}

			// Token: 0x06000B80 RID: 2944 RVA: 0x0002942B File Offset: 0x0002762B
			private AsyncInstantiateOperationConfiguredSource()
			{
				this.continuationAction = new Action<AsyncOperation>(this.Continuation);
			}

			// Token: 0x06000B81 RID: 2945 RVA: 0x00029448 File Offset: 0x00027648
			public static IUniTaskSource<Object[]> Create(AsyncInstantiateOperation asyncOperation, PlayerLoopTiming timing, IProgress<float> progress, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource<Object[]>.CreateFromCanceled(cancellationToken, out token);
				}
				AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource asyncInstantiateOperationConfiguredSource;
				if (!AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource.pool.TryPop(out asyncInstantiateOperationConfiguredSource))
				{
					asyncInstantiateOperationConfiguredSource = new AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource();
				}
				asyncInstantiateOperationConfiguredSource.asyncOperation = asyncOperation;
				asyncInstantiateOperationConfiguredSource.progress = progress;
				asyncInstantiateOperationConfiguredSource.cancellationToken = cancellationToken;
				asyncInstantiateOperationConfiguredSource.cancelImmediately = cancelImmediately;
				asyncInstantiateOperationConfiguredSource.completed = false;
				asyncOperation.completed += asyncInstantiateOperationConfiguredSource.continuationAction;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					asyncInstantiateOperationConfiguredSource.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource asyncInstantiateOperationConfiguredSource2 = (AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource)state;
						asyncInstantiateOperationConfiguredSource2.core.TrySetCanceled(asyncInstantiateOperationConfiguredSource2.cancellationToken);
					}, asyncInstantiateOperationConfiguredSource);
				}
				PlayerLoopHelper.AddAction(timing, asyncInstantiateOperationConfiguredSource);
				token = asyncInstantiateOperationConfiguredSource.core.Version;
				return asyncInstantiateOperationConfiguredSource;
			}

			// Token: 0x06000B82 RID: 2946 RVA: 0x000294FC File Offset: 0x000276FC
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

			// Token: 0x06000B83 RID: 2947 RVA: 0x00029548 File Offset: 0x00027748
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000B84 RID: 2948 RVA: 0x00029552 File Offset: 0x00027752
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000B85 RID: 2949 RVA: 0x00029560 File Offset: 0x00027760
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000B86 RID: 2950 RVA: 0x0002956D File Offset: 0x0002776D
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000B87 RID: 2951 RVA: 0x00029580 File Offset: 0x00027780
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
					this.core.TrySetResult(this.asyncOperation.Result);
					return false;
				}
				return true;
			}

			// Token: 0x06000B88 RID: 2952 RVA: 0x00029608 File Offset: 0x00027808
			private bool TryReturn()
			{
				this.core.Reset();
				this.asyncOperation.completed -= this.continuationAction;
				this.asyncOperation = null;
				this.progress = null;
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource.pool.TryPush(this);
			}

			// Token: 0x06000B89 RID: 2953 RVA: 0x00029668 File Offset: 0x00027868
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
				this.core.TrySetResult(this.asyncOperation.Result);
			}

			// Token: 0x040004DE RID: 1246
			private static TaskPool<AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource> pool;

			// Token: 0x040004DF RID: 1247
			private AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource nextNode;

			// Token: 0x040004E0 RID: 1248
			private AsyncInstantiateOperation asyncOperation;

			// Token: 0x040004E1 RID: 1249
			private IProgress<float> progress;

			// Token: 0x040004E2 RID: 1250
			private CancellationToken cancellationToken;

			// Token: 0x040004E3 RID: 1251
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x040004E4 RID: 1252
			private bool cancelImmediately;

			// Token: 0x040004E5 RID: 1253
			private bool completed;

			// Token: 0x040004E6 RID: 1254
			private UniTaskCompletionSourceCore<Object[]> core;

			// Token: 0x040004E7 RID: 1255
			private Action<AsyncOperation> continuationAction;
		}

		// Token: 0x020001FE RID: 510
		private sealed class AsyncInstantiateOperationConfiguredSource<T> : IUniTaskSource<T[]>, IUniTaskSource, IValueTaskSource, IValueTaskSource<T[]>, IPlayerLoopItem, ITaskPoolNode<AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource<T>> where T : Object
		{
			// Token: 0x17000085 RID: 133
			// (get) Token: 0x06000B8A RID: 2954 RVA: 0x000296BC File Offset: 0x000278BC
			public ref AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource<T> NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x06000B8B RID: 2955 RVA: 0x000296C4 File Offset: 0x000278C4
			static AsyncInstantiateOperationConfiguredSource()
			{
				TaskPool.RegisterSizeGetter(typeof(AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource<T>), () => AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource<T>.pool.Size);
			}

			// Token: 0x06000B8C RID: 2956 RVA: 0x000296E5 File Offset: 0x000278E5
			private AsyncInstantiateOperationConfiguredSource()
			{
				this.continuationAction = new Action<AsyncOperation>(this.Continuation);
			}

			// Token: 0x06000B8D RID: 2957 RVA: 0x00029700 File Offset: 0x00027900
			public static IUniTaskSource<T[]> Create(AsyncInstantiateOperation<T> asyncOperation, PlayerLoopTiming timing, IProgress<float> progress, CancellationToken cancellationToken, bool cancelImmediately, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource<T[]>.CreateFromCanceled(cancellationToken, out token);
				}
				AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource<T> asyncInstantiateOperationConfiguredSource;
				if (!AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource<T>.pool.TryPop(out asyncInstantiateOperationConfiguredSource))
				{
					asyncInstantiateOperationConfiguredSource = new AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource<T>();
				}
				asyncInstantiateOperationConfiguredSource.asyncOperation = asyncOperation;
				asyncInstantiateOperationConfiguredSource.progress = progress;
				asyncInstantiateOperationConfiguredSource.cancellationToken = cancellationToken;
				asyncInstantiateOperationConfiguredSource.cancelImmediately = cancelImmediately;
				asyncInstantiateOperationConfiguredSource.completed = false;
				asyncOperation.completed += asyncInstantiateOperationConfiguredSource.continuationAction;
				if (cancelImmediately && cancellationToken.CanBeCanceled)
				{
					asyncInstantiateOperationConfiguredSource.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(delegate(object state)
					{
						AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource<T> asyncInstantiateOperationConfiguredSource2 = (AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource<T>)state;
						asyncInstantiateOperationConfiguredSource2.core.TrySetCanceled(asyncInstantiateOperationConfiguredSource2.cancellationToken);
					}, asyncInstantiateOperationConfiguredSource);
				}
				PlayerLoopHelper.AddAction(timing, asyncInstantiateOperationConfiguredSource);
				token = asyncInstantiateOperationConfiguredSource.core.Version;
				return asyncInstantiateOperationConfiguredSource;
			}

			// Token: 0x06000B8E RID: 2958 RVA: 0x000297B4 File Offset: 0x000279B4
			public T[] GetResult(short token)
			{
				T[] result;
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

			// Token: 0x06000B8F RID: 2959 RVA: 0x00029800 File Offset: 0x00027A00
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000B90 RID: 2960 RVA: 0x0002980A File Offset: 0x00027A0A
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000B91 RID: 2961 RVA: 0x00029818 File Offset: 0x00027A18
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000B92 RID: 2962 RVA: 0x00029825 File Offset: 0x00027A25
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000B93 RID: 2963 RVA: 0x00029838 File Offset: 0x00027A38
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
					this.core.TrySetResult(this.asyncOperation.Result);
					return false;
				}
				return true;
			}

			// Token: 0x06000B94 RID: 2964 RVA: 0x000298C0 File Offset: 0x00027AC0
			private bool TryReturn()
			{
				this.core.Reset();
				this.asyncOperation.completed -= this.continuationAction;
				this.asyncOperation = null;
				this.progress = null;
				this.cancellationToken = default(CancellationToken);
				this.cancellationTokenRegistration.Dispose();
				this.cancelImmediately = false;
				return AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource<T>.pool.TryPush(this);
			}

			// Token: 0x06000B95 RID: 2965 RVA: 0x00029920 File Offset: 0x00027B20
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
				this.core.TrySetResult(this.asyncOperation.Result);
			}

			// Token: 0x040004E8 RID: 1256
			private static TaskPool<AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource<T>> pool;

			// Token: 0x040004E9 RID: 1257
			private AsyncInstantiateOperationExtensions.AsyncInstantiateOperationConfiguredSource<T> nextNode;

			// Token: 0x040004EA RID: 1258
			private AsyncInstantiateOperation<T> asyncOperation;

			// Token: 0x040004EB RID: 1259
			private IProgress<float> progress;

			// Token: 0x040004EC RID: 1260
			private CancellationToken cancellationToken;

			// Token: 0x040004ED RID: 1261
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x040004EE RID: 1262
			private bool cancelImmediately;

			// Token: 0x040004EF RID: 1263
			private bool completed;

			// Token: 0x040004F0 RID: 1264
			private UniTaskCompletionSourceCore<T[]> core;

			// Token: 0x040004F1 RID: 1265
			private Action<AsyncOperation> continuationAction;
		}
	}
}
