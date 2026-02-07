using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x0200003B RID: 59
	[AsyncMethodBuilder(typeof(AsyncUniTaskMethodBuilder<>))]
	[StructLayout(LayoutKind.Auto)]
	public readonly struct UniTask<T>
	{
		// Token: 0x06000177 RID: 375 RVA: 0x000066BD File Offset: 0x000048BD
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UniTask(T result)
		{
			this.source = null;
			this.token = 0;
			this.result = result;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000066D4 File Offset: 0x000048D4
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UniTask(IUniTaskSource<T> source, short token)
		{
			this.source = source;
			this.token = token;
			this.result = default(T);
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000179 RID: 377 RVA: 0x000066F0 File Offset: 0x000048F0
		public UniTaskStatus Status
		{
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (this.source != null)
				{
					return this.source.GetStatus(this.token);
				}
				return UniTaskStatus.Succeeded;
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000670D File Offset: 0x0000490D
		[DebuggerHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public UniTask<T>.Awaiter GetAwaiter()
		{
			return new UniTask<T>.Awaiter(ref this);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00006715 File Offset: 0x00004915
		public UniTask<T> Preserve()
		{
			if (this.source == null)
			{
				return this;
			}
			return new UniTask<T>(new UniTask<T>.MemoizeSource(this.source), this.token);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000673C File Offset: 0x0000493C
		public UniTask AsUniTask()
		{
			if (this.source == null)
			{
				return UniTask.CompletedTask;
			}
			if (this.source.GetStatus(this.token).IsCompletedSuccessfully())
			{
				this.source.GetResult(this.token);
				return UniTask.CompletedTask;
			}
			return new UniTask(this.source, this.token);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00006798 File Offset: 0x00004998
		public static implicit operator UniTask(UniTask<T> self)
		{
			return self.AsUniTask();
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000067A1 File Offset: 0x000049A1
		public static implicit operator ValueTask<T>(in UniTask<T> self)
		{
			if (self.source == null)
			{
				return new ValueTask<T>(self.result);
			}
			return new ValueTask<T>(self.source, self.token);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000067C8 File Offset: 0x000049C8
		[return: TupleElementNames(new string[]
		{
			"IsCanceled",
			"Result"
		})]
		public UniTask<ValueTuple<bool, T>> SuppressCancellationThrow()
		{
			if (this.source == null)
			{
				return new UniTask<ValueTuple<bool, T>>(new ValueTuple<bool, T>(false, this.result));
			}
			return new UniTask<ValueTuple<bool, T>>(new UniTask<T>.IsCanceledSource(this.source), this.token);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000067FC File Offset: 0x000049FC
		public override string ToString()
		{
			if (this.source != null)
			{
				return "(" + this.source.UnsafeGetStatus().ToString() + ")";
			}
			T t = this.result;
			if (t == null)
			{
				return null;
			}
			return t.ToString();
		}

		// Token: 0x04000080 RID: 128
		private readonly IUniTaskSource<T> source;

		// Token: 0x04000081 RID: 129
		private readonly T result;

		// Token: 0x04000082 RID: 130
		private readonly short token;

		// Token: 0x020001BA RID: 442
		private sealed class IsCanceledSource : IUniTaskSource<ValueTuple<bool, T>>, IUniTaskSource, IValueTaskSource, IValueTaskSource<ValueTuple<bool, T>>
		{
			// Token: 0x06000A4C RID: 2636 RVA: 0x00024A92 File Offset: 0x00022C92
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public IsCanceledSource(IUniTaskSource<T> source)
			{
				this.source = source;
			}

			// Token: 0x06000A4D RID: 2637 RVA: 0x00024AA4 File Offset: 0x00022CA4
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ValueTuple<bool, T> GetResult(short token)
			{
				if (this.source.GetStatus(token) == UniTaskStatus.Canceled)
				{
					return new ValueTuple<bool, T>(true, default(T));
				}
				T result = this.source.GetResult(token);
				return new ValueTuple<bool, T>(false, result);
			}

			// Token: 0x06000A4E RID: 2638 RVA: 0x00024AE4 File Offset: 0x00022CE4
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000A4F RID: 2639 RVA: 0x00024AEE File Offset: 0x00022CEE
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public UniTaskStatus GetStatus(short token)
			{
				return this.source.GetStatus(token);
			}

			// Token: 0x06000A50 RID: 2640 RVA: 0x00024AFC File Offset: 0x00022CFC
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.source.UnsafeGetStatus();
			}

			// Token: 0x06000A51 RID: 2641 RVA: 0x00024B09 File Offset: 0x00022D09
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.source.OnCompleted(continuation, state, token);
			}

			// Token: 0x04000395 RID: 917
			private readonly IUniTaskSource<T> source;
		}

		// Token: 0x020001BB RID: 443
		private sealed class MemoizeSource : IUniTaskSource<T>, IUniTaskSource, IValueTaskSource, IValueTaskSource<T>
		{
			// Token: 0x06000A52 RID: 2642 RVA: 0x00024B19 File Offset: 0x00022D19
			public MemoizeSource(IUniTaskSource<T> source)
			{
				this.source = source;
			}

			// Token: 0x06000A53 RID: 2643 RVA: 0x00024B28 File Offset: 0x00022D28
			public T GetResult(short token)
			{
				if (this.source == null)
				{
					if (this.exception != null)
					{
						this.exception.Throw();
					}
					return this.result;
				}
				T t;
				try
				{
					this.result = this.source.GetResult(token);
					this.status = UniTaskStatus.Succeeded;
					t = this.result;
				}
				catch (Exception ex)
				{
					this.exception = ExceptionDispatchInfo.Capture(ex);
					if (ex is OperationCanceledException)
					{
						this.status = UniTaskStatus.Canceled;
					}
					else
					{
						this.status = UniTaskStatus.Faulted;
					}
					throw;
				}
				finally
				{
					this.source = null;
				}
				return t;
			}

			// Token: 0x06000A54 RID: 2644 RVA: 0x00024BC8 File Offset: 0x00022DC8
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x06000A55 RID: 2645 RVA: 0x00024BD2 File Offset: 0x00022DD2
			public UniTaskStatus GetStatus(short token)
			{
				if (this.source == null)
				{
					return this.status;
				}
				return this.source.GetStatus(token);
			}

			// Token: 0x06000A56 RID: 2646 RVA: 0x00024BEF File Offset: 0x00022DEF
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				if (this.source == null)
				{
					continuation(state);
					return;
				}
				this.source.OnCompleted(continuation, state, token);
			}

			// Token: 0x06000A57 RID: 2647 RVA: 0x00024C0F File Offset: 0x00022E0F
			public UniTaskStatus UnsafeGetStatus()
			{
				if (this.source == null)
				{
					return this.status;
				}
				return this.source.UnsafeGetStatus();
			}

			// Token: 0x04000396 RID: 918
			private IUniTaskSource<T> source;

			// Token: 0x04000397 RID: 919
			private T result;

			// Token: 0x04000398 RID: 920
			private ExceptionDispatchInfo exception;

			// Token: 0x04000399 RID: 921
			private UniTaskStatus status;
		}

		// Token: 0x020001BC RID: 444
		public readonly struct Awaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x06000A58 RID: 2648 RVA: 0x00024C2B File Offset: 0x00022E2B
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Awaiter(in UniTask<T> task)
			{
				this.task = task;
			}

			// Token: 0x1700006C RID: 108
			// (get) Token: 0x06000A59 RID: 2649 RVA: 0x00024C39 File Offset: 0x00022E39
			public bool IsCompleted
			{
				[DebuggerHidden]
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this.task.Status.IsCompleted();
				}
			}

			// Token: 0x06000A5A RID: 2650 RVA: 0x00024C4C File Offset: 0x00022E4C
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public T GetResult()
			{
				IUniTaskSource<T> source = this.task.source;
				if (source == null)
				{
					return this.task.result;
				}
				return source.GetResult(this.task.token);
			}

			// Token: 0x06000A5B RID: 2651 RVA: 0x00024C88 File Offset: 0x00022E88
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void OnCompleted(Action continuation)
			{
				IUniTaskSource<T> source = this.task.source;
				if (source == null)
				{
					continuation();
					return;
				}
				source.OnCompleted(AwaiterActions.InvokeContinuationDelegate, continuation, this.task.token);
			}

			// Token: 0x06000A5C RID: 2652 RVA: 0x00024CC4 File Offset: 0x00022EC4
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UnsafeOnCompleted(Action continuation)
			{
				IUniTaskSource<T> source = this.task.source;
				if (source == null)
				{
					continuation();
					return;
				}
				source.OnCompleted(AwaiterActions.InvokeContinuationDelegate, continuation, this.task.token);
			}

			// Token: 0x06000A5D RID: 2653 RVA: 0x00024D00 File Offset: 0x00022F00
			[DebuggerHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void SourceOnCompleted(Action<object> continuation, object state)
			{
				IUniTaskSource<T> source = this.task.source;
				if (source == null)
				{
					continuation(state);
					return;
				}
				source.OnCompleted(continuation, state, this.task.token);
			}

			// Token: 0x0400039A RID: 922
			private readonly UniTask<T> task;
		}
	}
}
