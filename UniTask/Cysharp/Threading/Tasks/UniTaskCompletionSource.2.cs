using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks.Sources;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000054 RID: 84
	public class UniTaskCompletionSource<T> : IUniTaskSource<T>, IUniTaskSource, IValueTaskSource, IValueTaskSource<T>, IPromise<T>, IResolvePromise<T>, IRejectPromise, ICancelPromise
	{
		// Token: 0x060001DC RID: 476 RVA: 0x00007508 File Offset: 0x00005708
		[DebuggerHidden]
		internal void MarkHandled()
		{
			if (!this.handled)
			{
				this.handled = true;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00007519 File Offset: 0x00005719
		public UniTask<T> Task
		{
			[DebuggerHidden]
			get
			{
				return new UniTask<T>(this, 0);
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00007522 File Offset: 0x00005722
		[DebuggerHidden]
		public bool TrySetResult(T result)
		{
			if (this.UnsafeGetStatus() != UniTaskStatus.Pending)
			{
				return false;
			}
			this.result = result;
			return this.TrySignalCompletion(UniTaskStatus.Succeeded);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000753C File Offset: 0x0000573C
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

		// Token: 0x060001E0 RID: 480 RVA: 0x00007558 File Offset: 0x00005758
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

		// Token: 0x060001E1 RID: 481 RVA: 0x000075A0 File Offset: 0x000057A0
		[DebuggerHidden]
		public T GetResult(short token)
		{
			this.MarkHandled();
			switch (this.intStatus)
			{
			case 1:
				return this.result;
			case 2:
				this.exception.GetException().Throw();
				return default(T);
			case 3:
				throw new OperationCanceledException(this.cancellationToken);
			}
			throw new InvalidOperationException("not yet completed.");
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00007609 File Offset: 0x00005809
		[DebuggerHidden]
		void IUniTaskSource.GetResult(short token)
		{
			this.GetResult(token);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00007613 File Offset: 0x00005813
		[DebuggerHidden]
		public UniTaskStatus GetStatus(short token)
		{
			return (UniTaskStatus)this.intStatus;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000761B File Offset: 0x0000581B
		[DebuggerHidden]
		public UniTaskStatus UnsafeGetStatus()
		{
			return (UniTaskStatus)this.intStatus;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00007624 File Offset: 0x00005824
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

		// Token: 0x060001E6 RID: 486 RVA: 0x000076CC File Offset: 0x000058CC
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

		// Token: 0x040000B8 RID: 184
		private CancellationToken cancellationToken;

		// Token: 0x040000B9 RID: 185
		private T result;

		// Token: 0x040000BA RID: 186
		private ExceptionHolder exception;

		// Token: 0x040000BB RID: 187
		private object gate;

		// Token: 0x040000BC RID: 188
		private Action<object> singleContinuation;

		// Token: 0x040000BD RID: 189
		private object singleState;

		// Token: 0x040000BE RID: 190
		private List<ValueTuple<Action<object>, object>> secondaryContinuationList;

		// Token: 0x040000BF RID: 191
		private int intStatus;

		// Token: 0x040000C0 RID: 192
		private bool handled;
	}
}
