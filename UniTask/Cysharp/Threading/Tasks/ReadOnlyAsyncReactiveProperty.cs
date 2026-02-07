using System;
using System.Threading;
using System.Threading.Tasks.Sources;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000009 RID: 9
	public class ReadOnlyAsyncReactiveProperty<T> : IReadOnlyAsyncReactiveProperty<T>, IUniTaskAsyncEnumerable<T>, IDisposable
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000026 RID: 38 RVA: 0x0000262A File Offset: 0x0000082A
		public T Value
		{
			get
			{
				return this.latestValue;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002634 File Offset: 0x00000834
		public ReadOnlyAsyncReactiveProperty(T initialValue, IUniTaskAsyncEnumerable<T> source, CancellationToken cancellationToken)
		{
			this.latestValue = initialValue;
			this.ConsumeEnumerator(source, cancellationToken).Forget();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002660 File Offset: 0x00000860
		public ReadOnlyAsyncReactiveProperty(IUniTaskAsyncEnumerable<T> source, CancellationToken cancellationToken)
		{
			this.ConsumeEnumerator(source, cancellationToken).Forget();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002684 File Offset: 0x00000884
		private UniTaskVoid ConsumeEnumerator(IUniTaskAsyncEnumerable<T> source, CancellationToken cancellationToken)
		{
			ReadOnlyAsyncReactiveProperty<T>.<ConsumeEnumerator>d__7 <ConsumeEnumerator>d__;
			<ConsumeEnumerator>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<ConsumeEnumerator>d__.<>4__this = this;
			<ConsumeEnumerator>d__.source = source;
			<ConsumeEnumerator>d__.cancellationToken = cancellationToken;
			<ConsumeEnumerator>d__.<>1__state = -1;
			<ConsumeEnumerator>d__.<>t__builder.Start<ReadOnlyAsyncReactiveProperty<T>.<ConsumeEnumerator>d__7>(ref <ConsumeEnumerator>d__);
			return <ConsumeEnumerator>d__.<>t__builder.Task;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000026D7 File Offset: 0x000008D7
		public IUniTaskAsyncEnumerable<T> WithoutCurrent()
		{
			return new ReadOnlyAsyncReactiveProperty<T>.WithoutCurrentEnumerable(this);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000026DF File Offset: 0x000008DF
		public IUniTaskAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken)
		{
			return new ReadOnlyAsyncReactiveProperty<T>.Enumerator(this, cancellationToken, true);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000026E9 File Offset: 0x000008E9
		public void Dispose()
		{
			if (this.enumerator != null)
			{
				this.enumerator.DisposeAsync().Forget();
			}
			this.triggerEvent.SetCompleted();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000270E File Offset: 0x0000090E
		public static implicit operator T(ReadOnlyAsyncReactiveProperty<T> value)
		{
			return value.Value;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002718 File Offset: 0x00000918
		public override string ToString()
		{
			if (ReadOnlyAsyncReactiveProperty<T>.isValueType)
			{
				return this.latestValue.ToString();
			}
			ref T ptr = ref this.latestValue;
			T t = default(T);
			if (t == null)
			{
				t = this.latestValue;
				ptr = ref t;
				if (t == null)
				{
					return null;
				}
			}
			return ptr.ToString();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002774 File Offset: 0x00000974
		public UniTask<T> WaitAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			short token;
			return new UniTask<T>(ReadOnlyAsyncReactiveProperty<T>.WaitAsyncSource.Create(this, cancellationToken, out token), token);
		}

		// Token: 0x04000011 RID: 17
		private TriggerEvent<T> triggerEvent;

		// Token: 0x04000012 RID: 18
		private T latestValue;

		// Token: 0x04000013 RID: 19
		private IUniTaskAsyncEnumerator<T> enumerator;

		// Token: 0x04000014 RID: 20
		private static bool isValueType = typeof(T).IsValueType;

		// Token: 0x02000132 RID: 306
		private sealed class WaitAsyncSource : IUniTaskSource<T>, IUniTaskSource, IValueTaskSource, IValueTaskSource<T>, ITriggerHandler<!0>, ITaskPoolNode<ReadOnlyAsyncReactiveProperty<T>.WaitAsyncSource>
		{
			// Token: 0x1700004E RID: 78
			// (get) Token: 0x06000714 RID: 1812 RVA: 0x0001044A File Offset: 0x0000E64A
			ref ReadOnlyAsyncReactiveProperty<T>.WaitAsyncSource ITaskPoolNode<ReadOnlyAsyncReactiveProperty<!0>.WaitAsyncSource>.NextNode
			{
				get
				{
					return ref this.nextNode;
				}
			}

			// Token: 0x06000715 RID: 1813 RVA: 0x00010452 File Offset: 0x0000E652
			static WaitAsyncSource()
			{
				TaskPool.RegisterSizeGetter(typeof(ReadOnlyAsyncReactiveProperty<T>.WaitAsyncSource), () => ReadOnlyAsyncReactiveProperty<T>.WaitAsyncSource.pool.Size);
			}

			// Token: 0x06000716 RID: 1814 RVA: 0x00010484 File Offset: 0x0000E684
			private WaitAsyncSource()
			{
			}

			// Token: 0x06000717 RID: 1815 RVA: 0x0001048C File Offset: 0x0000E68C
			public static IUniTaskSource<T> Create(ReadOnlyAsyncReactiveProperty<T> parent, CancellationToken cancellationToken, out short token)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return AutoResetUniTaskCompletionSource<T>.CreateFromCanceled(cancellationToken, out token);
				}
				ReadOnlyAsyncReactiveProperty<T>.WaitAsyncSource waitAsyncSource;
				if (!ReadOnlyAsyncReactiveProperty<T>.WaitAsyncSource.pool.TryPop(out waitAsyncSource))
				{
					waitAsyncSource = new ReadOnlyAsyncReactiveProperty<T>.WaitAsyncSource();
				}
				waitAsyncSource.parent = parent;
				waitAsyncSource.cancellationToken = cancellationToken;
				if (cancellationToken.CanBeCanceled)
				{
					waitAsyncSource.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(ReadOnlyAsyncReactiveProperty<T>.WaitAsyncSource.cancellationCallback, waitAsyncSource);
				}
				waitAsyncSource.parent.triggerEvent.Add(waitAsyncSource);
				token = waitAsyncSource.core.Version;
				return waitAsyncSource;
			}

			// Token: 0x06000718 RID: 1816 RVA: 0x00010508 File Offset: 0x0000E708
			private bool TryReturn()
			{
				this.core.Reset();
				this.cancellationTokenRegistration.Dispose();
				this.cancellationTokenRegistration = default(CancellationTokenRegistration);
				this.parent.triggerEvent.Remove(this);
				this.parent = null;
				this.cancellationToken = default(CancellationToken);
				return ReadOnlyAsyncReactiveProperty<T>.WaitAsyncSource.pool.TryPush(this);
			}

			// Token: 0x06000719 RID: 1817 RVA: 0x00010566 File Offset: 0x0000E766
			private static void CancellationCallback(object state)
			{
				ReadOnlyAsyncReactiveProperty<T>.WaitAsyncSource waitAsyncSource = (ReadOnlyAsyncReactiveProperty<T>.WaitAsyncSource)state;
				waitAsyncSource.OnCanceled(waitAsyncSource.cancellationToken);
			}

			// Token: 0x0600071A RID: 1818 RVA: 0x0001057C File Offset: 0x0000E77C
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

			// Token: 0x0600071B RID: 1819 RVA: 0x000105B4 File Offset: 0x0000E7B4
			void IUniTaskSource.GetResult(short token)
			{
				this.GetResult(token);
			}

			// Token: 0x0600071C RID: 1820 RVA: 0x000105BE File Offset: 0x0000E7BE
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x0600071D RID: 1821 RVA: 0x000105CE File Offset: 0x0000E7CE
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x0600071E RID: 1822 RVA: 0x000105DC File Offset: 0x0000E7DC
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x1700004F RID: 79
			// (get) Token: 0x0600071F RID: 1823 RVA: 0x000105E9 File Offset: 0x0000E7E9
			// (set) Token: 0x06000720 RID: 1824 RVA: 0x000105F1 File Offset: 0x0000E7F1
			ITriggerHandler<T> ITriggerHandler<!0>.Prev { get; set; }

			// Token: 0x17000050 RID: 80
			// (get) Token: 0x06000721 RID: 1825 RVA: 0x000105FA File Offset: 0x0000E7FA
			// (set) Token: 0x06000722 RID: 1826 RVA: 0x00010602 File Offset: 0x0000E802
			ITriggerHandler<T> ITriggerHandler<!0>.Next { get; set; }

			// Token: 0x06000723 RID: 1827 RVA: 0x0001060B File Offset: 0x0000E80B
			public void OnCanceled(CancellationToken cancellationToken)
			{
				this.core.TrySetCanceled(cancellationToken);
			}

			// Token: 0x06000724 RID: 1828 RVA: 0x0001061A File Offset: 0x0000E81A
			public void OnCompleted()
			{
				this.core.TrySetCanceled(CancellationToken.None);
			}

			// Token: 0x06000725 RID: 1829 RVA: 0x0001062D File Offset: 0x0000E82D
			public void OnError(Exception ex)
			{
				this.core.TrySetException(ex);
			}

			// Token: 0x06000726 RID: 1830 RVA: 0x0001063C File Offset: 0x0000E83C
			public void OnNext(T value)
			{
				this.core.TrySetResult(value);
			}

			// Token: 0x04000189 RID: 393
			private static Action<object> cancellationCallback = new Action<object>(ReadOnlyAsyncReactiveProperty<T>.WaitAsyncSource.CancellationCallback);

			// Token: 0x0400018A RID: 394
			private static TaskPool<ReadOnlyAsyncReactiveProperty<T>.WaitAsyncSource> pool;

			// Token: 0x0400018B RID: 395
			private ReadOnlyAsyncReactiveProperty<T>.WaitAsyncSource nextNode;

			// Token: 0x0400018C RID: 396
			private ReadOnlyAsyncReactiveProperty<T> parent;

			// Token: 0x0400018D RID: 397
			private CancellationToken cancellationToken;

			// Token: 0x0400018E RID: 398
			private CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x0400018F RID: 399
			private UniTaskCompletionSourceCore<T> core;
		}

		// Token: 0x02000133 RID: 307
		private sealed class WithoutCurrentEnumerable : IUniTaskAsyncEnumerable<T>
		{
			// Token: 0x06000727 RID: 1831 RVA: 0x0001064B File Offset: 0x0000E84B
			public WithoutCurrentEnumerable(ReadOnlyAsyncReactiveProperty<T> parent)
			{
				this.parent = parent;
			}

			// Token: 0x06000728 RID: 1832 RVA: 0x0001065A File Offset: 0x0000E85A
			public IUniTaskAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
			{
				return new ReadOnlyAsyncReactiveProperty<T>.Enumerator(this.parent, cancellationToken, false);
			}

			// Token: 0x04000192 RID: 402
			private readonly ReadOnlyAsyncReactiveProperty<T> parent;
		}

		// Token: 0x02000134 RID: 308
		private sealed class Enumerator : MoveNextSource, IUniTaskAsyncEnumerator<T>, IUniTaskAsyncDisposable, ITriggerHandler<!0>
		{
			// Token: 0x06000729 RID: 1833 RVA: 0x0001066C File Offset: 0x0000E86C
			public Enumerator(ReadOnlyAsyncReactiveProperty<T> parent, CancellationToken cancellationToken, bool publishCurrentValue)
			{
				this.parent = parent;
				this.cancellationToken = cancellationToken;
				this.firstCall = publishCurrentValue;
				parent.triggerEvent.Add(this);
				if (cancellationToken.CanBeCanceled)
				{
					this.cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(ReadOnlyAsyncReactiveProperty<T>.Enumerator.cancellationCallback, this);
				}
			}

			// Token: 0x17000051 RID: 81
			// (get) Token: 0x0600072A RID: 1834 RVA: 0x000106BB File Offset: 0x0000E8BB
			public T Current
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x17000052 RID: 82
			// (get) Token: 0x0600072B RID: 1835 RVA: 0x000106C3 File Offset: 0x0000E8C3
			// (set) Token: 0x0600072C RID: 1836 RVA: 0x000106CB File Offset: 0x0000E8CB
			ITriggerHandler<T> ITriggerHandler<!0>.Prev { get; set; }

			// Token: 0x17000053 RID: 83
			// (get) Token: 0x0600072D RID: 1837 RVA: 0x000106D4 File Offset: 0x0000E8D4
			// (set) Token: 0x0600072E RID: 1838 RVA: 0x000106DC File Offset: 0x0000E8DC
			ITriggerHandler<T> ITriggerHandler<!0>.Next { get; set; }

			// Token: 0x0600072F RID: 1839 RVA: 0x000106E8 File Offset: 0x0000E8E8
			public UniTask<bool> MoveNextAsync()
			{
				if (this.firstCall)
				{
					this.firstCall = false;
					this.value = this.parent.Value;
					return CompletedTasks.True;
				}
				this.completionSource.Reset();
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000730 RID: 1840 RVA: 0x00010738 File Offset: 0x0000E938
			public UniTask DisposeAsync()
			{
				if (!this.isDisposed)
				{
					this.isDisposed = true;
					this.completionSource.TrySetCanceled(this.cancellationToken);
					this.parent.triggerEvent.Remove(this);
				}
				return default(UniTask);
			}

			// Token: 0x06000731 RID: 1841 RVA: 0x00010780 File Offset: 0x0000E980
			public void OnNext(T value)
			{
				this.value = value;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x06000732 RID: 1842 RVA: 0x00010796 File Offset: 0x0000E996
			public void OnCanceled(CancellationToken cancellationToken)
			{
				this.DisposeAsync().Forget();
			}

			// Token: 0x06000733 RID: 1843 RVA: 0x000107A3 File Offset: 0x0000E9A3
			public void OnCompleted()
			{
				this.completionSource.TrySetResult(false);
			}

			// Token: 0x06000734 RID: 1844 RVA: 0x000107B2 File Offset: 0x0000E9B2
			public void OnError(Exception ex)
			{
				this.completionSource.TrySetException(ex);
			}

			// Token: 0x06000735 RID: 1845 RVA: 0x000107C1 File Offset: 0x0000E9C1
			private static void CancellationCallback(object state)
			{
				((ReadOnlyAsyncReactiveProperty<T>.Enumerator)state).DisposeAsync().Forget();
			}

			// Token: 0x04000193 RID: 403
			private static Action<object> cancellationCallback = new Action<object>(ReadOnlyAsyncReactiveProperty<T>.Enumerator.CancellationCallback);

			// Token: 0x04000194 RID: 404
			private readonly ReadOnlyAsyncReactiveProperty<T> parent;

			// Token: 0x04000195 RID: 405
			private readonly CancellationToken cancellationToken;

			// Token: 0x04000196 RID: 406
			private readonly CancellationTokenRegistration cancellationTokenRegistration;

			// Token: 0x04000197 RID: 407
			private T value;

			// Token: 0x04000198 RID: 408
			private bool isDisposed;

			// Token: 0x04000199 RID: 409
			private bool firstCall;
		}
	}
}
