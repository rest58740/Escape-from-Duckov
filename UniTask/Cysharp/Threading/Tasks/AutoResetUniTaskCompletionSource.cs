using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks.Sources;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000051 RID: 81
	public class AutoResetUniTaskCompletionSource : IUniTaskSource, IValueTaskSource, ITaskPoolNode<AutoResetUniTaskCompletionSource>, IPromise, IResolvePromise, IRejectPromise, ICancelPromise
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00006E77 File Offset: 0x00005077
		public ref AutoResetUniTaskCompletionSource NextNode
		{
			get
			{
				return ref this.nextNode;
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00006E7F File Offset: 0x0000507F
		static AutoResetUniTaskCompletionSource()
		{
			TaskPool.RegisterSizeGetter(typeof(AutoResetUniTaskCompletionSource), () => AutoResetUniTaskCompletionSource.pool.Size);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00006EA0 File Offset: 0x000050A0
		private AutoResetUniTaskCompletionSource()
		{
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00006EA8 File Offset: 0x000050A8
		[DebuggerHidden]
		public static AutoResetUniTaskCompletionSource Create()
		{
			AutoResetUniTaskCompletionSource autoResetUniTaskCompletionSource;
			if (!AutoResetUniTaskCompletionSource.pool.TryPop(out autoResetUniTaskCompletionSource))
			{
				autoResetUniTaskCompletionSource = new AutoResetUniTaskCompletionSource();
			}
			autoResetUniTaskCompletionSource.version = autoResetUniTaskCompletionSource.core.Version;
			return autoResetUniTaskCompletionSource;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00006EDC File Offset: 0x000050DC
		[DebuggerHidden]
		public static AutoResetUniTaskCompletionSource CreateFromCanceled(CancellationToken cancellationToken, out short token)
		{
			AutoResetUniTaskCompletionSource autoResetUniTaskCompletionSource = AutoResetUniTaskCompletionSource.Create();
			autoResetUniTaskCompletionSource.TrySetCanceled(cancellationToken);
			token = autoResetUniTaskCompletionSource.core.Version;
			return autoResetUniTaskCompletionSource;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00006F08 File Offset: 0x00005108
		[DebuggerHidden]
		public static AutoResetUniTaskCompletionSource CreateFromException(Exception exception, out short token)
		{
			AutoResetUniTaskCompletionSource autoResetUniTaskCompletionSource = AutoResetUniTaskCompletionSource.Create();
			autoResetUniTaskCompletionSource.TrySetException(exception);
			token = autoResetUniTaskCompletionSource.core.Version;
			return autoResetUniTaskCompletionSource;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00006F34 File Offset: 0x00005134
		[DebuggerHidden]
		public static AutoResetUniTaskCompletionSource CreateCompleted(out short token)
		{
			AutoResetUniTaskCompletionSource autoResetUniTaskCompletionSource = AutoResetUniTaskCompletionSource.Create();
			autoResetUniTaskCompletionSource.TrySetResult();
			token = autoResetUniTaskCompletionSource.core.Version;
			return autoResetUniTaskCompletionSource;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00006F5C File Offset: 0x0000515C
		public UniTask Task
		{
			[DebuggerHidden]
			get
			{
				return new UniTask(this, this.core.Version);
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00006F6F File Offset: 0x0000516F
		[DebuggerHidden]
		public bool TrySetResult()
		{
			return this.version == this.core.Version && this.core.TrySetResult(AsyncUnit.Default);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00006F96 File Offset: 0x00005196
		[DebuggerHidden]
		public bool TrySetCanceled(CancellationToken cancellationToken = default(CancellationToken))
		{
			return this.version == this.core.Version && this.core.TrySetCanceled(cancellationToken);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00006FB9 File Offset: 0x000051B9
		[DebuggerHidden]
		public bool TrySetException(Exception exception)
		{
			return this.version == this.core.Version && this.core.TrySetException(exception);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00006FDC File Offset: 0x000051DC
		[DebuggerHidden]
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

		// Token: 0x060001BB RID: 443 RVA: 0x00007010 File Offset: 0x00005210
		[DebuggerHidden]
		public UniTaskStatus GetStatus(short token)
		{
			return this.core.GetStatus(token);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000701E File Offset: 0x0000521E
		[DebuggerHidden]
		public UniTaskStatus UnsafeGetStatus()
		{
			return this.core.UnsafeGetStatus();
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000702B File Offset: 0x0000522B
		[DebuggerHidden]
		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			this.core.OnCompleted(continuation, state, token);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000703B File Offset: 0x0000523B
		[DebuggerHidden]
		private bool TryReturn()
		{
			this.core.Reset();
			return AutoResetUniTaskCompletionSource.pool.TryPush(this);
		}

		// Token: 0x040000A8 RID: 168
		private static TaskPool<AutoResetUniTaskCompletionSource> pool;

		// Token: 0x040000A9 RID: 169
		private AutoResetUniTaskCompletionSource nextNode;

		// Token: 0x040000AA RID: 170
		private UniTaskCompletionSourceCore<AsyncUnit> core;

		// Token: 0x040000AB RID: 171
		private short version;
	}
}
