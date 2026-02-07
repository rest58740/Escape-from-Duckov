using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200004F RID: 79
	[StructLayout(LayoutKind.Auto)]
	public struct UniTaskCompletionSourceCore<TResult>
	{
		// Token: 0x060001A1 RID: 417 RVA: 0x00006B20 File Offset: 0x00004D20
		[DebuggerHidden]
		public void Reset()
		{
			this.ReportUnhandledError();
			this.version += 1;
			this.completedCount = 0;
			this.result = default(TResult);
			this.error = null;
			this.hasUnhandledError = false;
			this.continuation = null;
			this.continuationState = null;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00006B74 File Offset: 0x00004D74
		private void ReportUnhandledError()
		{
			if (this.hasUnhandledError)
			{
				try
				{
					OperationCanceledException ex = this.error as OperationCanceledException;
					if (ex != null)
					{
						UniTaskScheduler.PublishUnobservedTaskException(ex);
					}
					else
					{
						ExceptionHolder exceptionHolder = this.error as ExceptionHolder;
						if (exceptionHolder != null)
						{
							UniTaskScheduler.PublishUnobservedTaskException(exceptionHolder.GetException().SourceException);
						}
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00006BD4 File Offset: 0x00004DD4
		internal void MarkHandled()
		{
			this.hasUnhandledError = false;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00006BE0 File Offset: 0x00004DE0
		[DebuggerHidden]
		public bool TrySetResult(TResult result)
		{
			if (Interlocked.Increment(ref this.completedCount) == 1)
			{
				this.result = result;
				if (this.continuation != null || Interlocked.CompareExchange<Action<object>>(ref this.continuation, UniTaskCompletionSourceCoreShared.s_sentinel, null) != null)
				{
					this.continuation(this.continuationState);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00006C34 File Offset: 0x00004E34
		[DebuggerHidden]
		public bool TrySetException(Exception error)
		{
			if (Interlocked.Increment(ref this.completedCount) == 1)
			{
				this.hasUnhandledError = true;
				if (error is OperationCanceledException)
				{
					this.error = error;
				}
				else
				{
					this.error = new ExceptionHolder(ExceptionDispatchInfo.Capture(error));
				}
				if (this.continuation != null || Interlocked.CompareExchange<Action<object>>(ref this.continuation, UniTaskCompletionSourceCoreShared.s_sentinel, null) != null)
				{
					this.continuation(this.continuationState);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00006CA8 File Offset: 0x00004EA8
		[DebuggerHidden]
		public bool TrySetCanceled(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (Interlocked.Increment(ref this.completedCount) == 1)
			{
				this.hasUnhandledError = true;
				this.error = new OperationCanceledException(cancellationToken);
				if (this.continuation != null || Interlocked.CompareExchange<Action<object>>(ref this.continuation, UniTaskCompletionSourceCoreShared.s_sentinel, null) != null)
				{
					this.continuation(this.continuationState);
				}
				return true;
			}
			return false;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00006D05 File Offset: 0x00004F05
		[DebuggerHidden]
		public short Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00006D0D File Offset: 0x00004F0D
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UniTaskStatus GetStatus(short token)
		{
			this.ValidateToken(token);
			if (this.continuation == null || this.completedCount == 0)
			{
				return UniTaskStatus.Pending;
			}
			if (this.error == null)
			{
				return UniTaskStatus.Succeeded;
			}
			if (!(this.error is OperationCanceledException))
			{
				return UniTaskStatus.Faulted;
			}
			return UniTaskStatus.Canceled;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00006D42 File Offset: 0x00004F42
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UniTaskStatus UnsafeGetStatus()
		{
			if (this.continuation == null || this.completedCount == 0)
			{
				return UniTaskStatus.Pending;
			}
			if (this.error == null)
			{
				return UniTaskStatus.Succeeded;
			}
			if (!(this.error is OperationCanceledException))
			{
				return UniTaskStatus.Faulted;
			}
			return UniTaskStatus.Canceled;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00006D70 File Offset: 0x00004F70
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TResult GetResult(short token)
		{
			this.ValidateToken(token);
			if (this.completedCount == 0)
			{
				throw new InvalidOperationException("Not yet completed, UniTask only allow to use await.");
			}
			if (this.error == null)
			{
				return this.result;
			}
			this.hasUnhandledError = false;
			OperationCanceledException ex = this.error as OperationCanceledException;
			if (ex != null)
			{
				throw ex;
			}
			ExceptionHolder exceptionHolder = this.error as ExceptionHolder;
			if (exceptionHolder != null)
			{
				exceptionHolder.GetException().Throw();
			}
			throw new InvalidOperationException("Critical: invalid exception type was held.");
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00006DE4 File Offset: 0x00004FE4
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			if (continuation == null)
			{
				throw new ArgumentNullException("continuation");
			}
			this.ValidateToken(token);
			object obj = this.continuation;
			if (obj == null)
			{
				this.continuationState = state;
				obj = Interlocked.CompareExchange<Action<object>>(ref this.continuation, continuation, null);
			}
			if (obj != null)
			{
				if (obj != UniTaskCompletionSourceCoreShared.s_sentinel)
				{
					throw new InvalidOperationException("Already continuation registered, can not await twice or get Status after await.");
				}
				continuation(state);
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00006E42 File Offset: 0x00005042
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ValidateToken(short token)
		{
			if (token != this.version)
			{
				throw new InvalidOperationException("Token version is not matched, can not await twice or get Status after await.");
			}
		}

		// Token: 0x040000A0 RID: 160
		private TResult result;

		// Token: 0x040000A1 RID: 161
		private object error;

		// Token: 0x040000A2 RID: 162
		private short version;

		// Token: 0x040000A3 RID: 163
		private bool hasUnhandledError;

		// Token: 0x040000A4 RID: 164
		private int completedCount;

		// Token: 0x040000A5 RID: 165
		private Action<object> continuation;

		// Token: 0x040000A6 RID: 166
		private object continuationState;
	}
}
