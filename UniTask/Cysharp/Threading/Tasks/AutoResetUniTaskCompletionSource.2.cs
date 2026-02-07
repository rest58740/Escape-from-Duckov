using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks.Sources;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000052 RID: 82
	public class AutoResetUniTaskCompletionSource<T> : IUniTaskSource<T>, IUniTaskSource, IValueTaskSource, IValueTaskSource<T>, ITaskPoolNode<AutoResetUniTaskCompletionSource<T>>, IPromise<T>, IResolvePromise<T>, IRejectPromise, ICancelPromise
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00007053 File Offset: 0x00005253
		public ref AutoResetUniTaskCompletionSource<T> NextNode
		{
			get
			{
				return ref this.nextNode;
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000705B File Offset: 0x0000525B
		static AutoResetUniTaskCompletionSource()
		{
			TaskPool.RegisterSizeGetter(typeof(AutoResetUniTaskCompletionSource<T>), () => AutoResetUniTaskCompletionSource<T>.pool.Size);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000707C File Offset: 0x0000527C
		private AutoResetUniTaskCompletionSource()
		{
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00007084 File Offset: 0x00005284
		[DebuggerHidden]
		public static AutoResetUniTaskCompletionSource<T> Create()
		{
			AutoResetUniTaskCompletionSource<T> autoResetUniTaskCompletionSource;
			if (!AutoResetUniTaskCompletionSource<T>.pool.TryPop(out autoResetUniTaskCompletionSource))
			{
				autoResetUniTaskCompletionSource = new AutoResetUniTaskCompletionSource<T>();
			}
			autoResetUniTaskCompletionSource.version = autoResetUniTaskCompletionSource.core.Version;
			return autoResetUniTaskCompletionSource;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000070B8 File Offset: 0x000052B8
		[DebuggerHidden]
		public static AutoResetUniTaskCompletionSource<T> CreateFromCanceled(CancellationToken cancellationToken, out short token)
		{
			AutoResetUniTaskCompletionSource<T> autoResetUniTaskCompletionSource = AutoResetUniTaskCompletionSource<T>.Create();
			autoResetUniTaskCompletionSource.TrySetCanceled(cancellationToken);
			token = autoResetUniTaskCompletionSource.core.Version;
			return autoResetUniTaskCompletionSource;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000070E4 File Offset: 0x000052E4
		[DebuggerHidden]
		public static AutoResetUniTaskCompletionSource<T> CreateFromException(Exception exception, out short token)
		{
			AutoResetUniTaskCompletionSource<T> autoResetUniTaskCompletionSource = AutoResetUniTaskCompletionSource<T>.Create();
			autoResetUniTaskCompletionSource.TrySetException(exception);
			token = autoResetUniTaskCompletionSource.core.Version;
			return autoResetUniTaskCompletionSource;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00007110 File Offset: 0x00005310
		[DebuggerHidden]
		public static AutoResetUniTaskCompletionSource<T> CreateFromResult(T result, out short token)
		{
			AutoResetUniTaskCompletionSource<T> autoResetUniTaskCompletionSource = AutoResetUniTaskCompletionSource<T>.Create();
			autoResetUniTaskCompletionSource.TrySetResult(result);
			token = autoResetUniTaskCompletionSource.core.Version;
			return autoResetUniTaskCompletionSource;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00007139 File Offset: 0x00005339
		public UniTask<T> Task
		{
			[DebuggerHidden]
			get
			{
				return new UniTask<T>(this, this.core.Version);
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000714C File Offset: 0x0000534C
		[DebuggerHidden]
		public bool TrySetResult(T result)
		{
			return this.version == this.core.Version && this.core.TrySetResult(result);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000716F File Offset: 0x0000536F
		[DebuggerHidden]
		public bool TrySetCanceled(CancellationToken cancellationToken = default(CancellationToken))
		{
			return this.version == this.core.Version && this.core.TrySetCanceled(cancellationToken);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00007192 File Offset: 0x00005392
		[DebuggerHidden]
		public bool TrySetException(Exception exception)
		{
			return this.version == this.core.Version && this.core.TrySetException(exception);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000071B8 File Offset: 0x000053B8
		[DebuggerHidden]
		public T GetResult(short token)
		{
			T result;
			try
			{
				result = this.core.GetResult(token);
			}
			finally
			{
				this.TryReturn();
			}
			return result;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000071F0 File Offset: 0x000053F0
		[DebuggerHidden]
		void IUniTaskSource.GetResult(short token)
		{
			this.GetResult(token);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000071FA File Offset: 0x000053FA
		[DebuggerHidden]
		public UniTaskStatus GetStatus(short token)
		{
			return this.core.GetStatus(token);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00007208 File Offset: 0x00005408
		[DebuggerHidden]
		public UniTaskStatus UnsafeGetStatus()
		{
			return this.core.UnsafeGetStatus();
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00007215 File Offset: 0x00005415
		[DebuggerHidden]
		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			this.core.OnCompleted(continuation, state, token);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00007225 File Offset: 0x00005425
		[DebuggerHidden]
		private bool TryReturn()
		{
			this.core.Reset();
			return AutoResetUniTaskCompletionSource<T>.pool.TryPush(this);
		}

		// Token: 0x040000AC RID: 172
		private static TaskPool<AutoResetUniTaskCompletionSource<T>> pool;

		// Token: 0x040000AD RID: 173
		private AutoResetUniTaskCompletionSource<T> nextNode;

		// Token: 0x040000AE RID: 174
		private UniTaskCompletionSourceCore<T> core;

		// Token: 0x040000AF RID: 175
		private short version;
	}
}
