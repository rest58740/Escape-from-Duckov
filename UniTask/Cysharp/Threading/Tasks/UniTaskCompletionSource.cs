using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks.Sources;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000053 RID: 83
	public class UniTaskCompletionSource : IUniTaskSource, IValueTaskSource, IPromise, IResolvePromise, IRejectPromise, ICancelPromise
	{
		// Token: 0x060001D1 RID: 465 RVA: 0x00007245 File Offset: 0x00005445
		[DebuggerHidden]
		internal void MarkHandled()
		{
			if (!this.handled)
			{
				this.handled = true;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00007256 File Offset: 0x00005456
		public UniTask Task
		{
			[DebuggerHidden]
			get
			{
				return new UniTask(this, 0);
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000725F File Offset: 0x0000545F
		[DebuggerHidden]
		public bool TrySetResult()
		{
			return this.TrySignalCompletion(UniTaskStatus.Succeeded);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00007268 File Offset: 0x00005468
		[DebuggerHidden]
		public bool TrySetCanceled(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (this.UnsafeGetStatus() != UniTaskStatus.Pending)
			{
				return false;
			}
			this.cancellationToken = cancellationToken;
			return this.TrySignalCompletion(UniTaskStatus.Canceled);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00007284 File Offset: 0x00005484
		[DebuggerHidden]
		public bool TrySetException(Exception exception)
		{
			OperationCanceledException ex = exception as OperationCanceledException;
			if (ex != null)
			{
				return this.TrySetCanceled(ex.CancellationToken);
			}
			if (this.UnsafeGetStatus() != UniTaskStatus.Pending)
			{
				return false;
			}
			this.exception = new ExceptionHolder(ExceptionDispatchInfo.Capture(exception));
			return this.TrySignalCompletion(UniTaskStatus.Faulted);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000072CC File Offset: 0x000054CC
		[DebuggerHidden]
		public void GetResult(short token)
		{
			this.MarkHandled();
			switch (this.intStatus)
			{
			case 1:
				return;
			case 2:
				this.exception.GetException().Throw();
				return;
			case 3:
				throw new OperationCanceledException(this.cancellationToken);
			}
			throw new InvalidOperationException("not yet completed.");
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00007326 File Offset: 0x00005526
		[DebuggerHidden]
		public UniTaskStatus GetStatus(short token)
		{
			return (UniTaskStatus)this.intStatus;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000732E File Offset: 0x0000552E
		[DebuggerHidden]
		public UniTaskStatus UnsafeGetStatus()
		{
			return (UniTaskStatus)this.intStatus;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00007338 File Offset: 0x00005538
		[DebuggerHidden]
		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			if (this.gate == null)
			{
				Interlocked.CompareExchange(ref this.gate, new object(), null);
			}
			object obj = Thread.VolatileRead(ref this.gate);
			lock (obj)
			{
				if (this.intStatus != 0)
				{
					continuation(state);
				}
				else if (this.singleContinuation == null)
				{
					this.singleContinuation = continuation;
					this.singleState = state;
				}
				else
				{
					if (this.secondaryContinuationList == null)
					{
						this.secondaryContinuationList = new List<ValueTuple<Action<object>, object>>();
					}
					this.secondaryContinuationList.Add(new ValueTuple<Action<object>, object>(continuation, state));
				}
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000073E0 File Offset: 0x000055E0
		[DebuggerHidden]
		private bool TrySignalCompletion(UniTaskStatus status)
		{
			if (Interlocked.CompareExchange(ref this.intStatus, (int)status, 0) == 0)
			{
				if (this.gate == null)
				{
					Interlocked.CompareExchange(ref this.gate, new object(), null);
				}
				object obj = Thread.VolatileRead(ref this.gate);
				lock (obj)
				{
					if (this.singleContinuation != null)
					{
						try
						{
							this.singleContinuation(this.singleState);
						}
						catch (Exception ex)
						{
							UniTaskScheduler.PublishUnobservedTaskException(ex);
						}
					}
					if (this.secondaryContinuationList != null)
					{
						foreach (ValueTuple<Action<object>, object> valueTuple in this.secondaryContinuationList)
						{
							Action<object> item = valueTuple.Item1;
							object item2 = valueTuple.Item2;
							try
							{
								item(item2);
							}
							catch (Exception ex2)
							{
								UniTaskScheduler.PublishUnobservedTaskException(ex2);
							}
						}
					}
					this.singleContinuation = null;
					this.singleState = null;
					this.secondaryContinuationList = null;
				}
				return true;
			}
			return false;
		}

		// Token: 0x040000B0 RID: 176
		private CancellationToken cancellationToken;

		// Token: 0x040000B1 RID: 177
		private ExceptionHolder exception;

		// Token: 0x040000B2 RID: 178
		private object gate;

		// Token: 0x040000B3 RID: 179
		private Action<object> singleContinuation;

		// Token: 0x040000B4 RID: 180
		private object singleState;

		// Token: 0x040000B5 RID: 181
		private List<ValueTuple<Action<object>, object>> secondaryContinuationList;

		// Token: 0x040000B6 RID: 182
		private int intStatus;

		// Token: 0x040000B7 RID: 183
		private bool handled;
	}
}
