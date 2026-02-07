using System;
using System.Runtime.ExceptionServices;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x0200011D RID: 285
	internal sealed class AsyncSubject<T> : IObservable<T>, IObserver<T>
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x0000F413 File Offset: 0x0000D613
		public T Value
		{
			get
			{
				this.ThrowIfDisposed();
				if (!this.isStopped)
				{
					throw new InvalidOperationException("AsyncSubject is not completed yet");
				}
				if (this.lastError != null)
				{
					ExceptionDispatchInfo.Capture(this.lastError).Throw();
				}
				return this.lastValue;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x0000F44C File Offset: 0x0000D64C
		public bool HasObservers
		{
			get
			{
				return !(this.outObserver is EmptyObserver<T>) && !this.isStopped && !this.isDisposed;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x0000F46E File Offset: 0x0000D66E
		public bool IsCompleted
		{
			get
			{
				return this.isStopped;
			}
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0000F478 File Offset: 0x0000D678
		public void OnCompleted()
		{
			object obj = this.observerLock;
			IObserver<T> observer;
			T value;
			bool flag2;
			lock (obj)
			{
				this.ThrowIfDisposed();
				if (this.isStopped)
				{
					return;
				}
				observer = this.outObserver;
				this.outObserver = EmptyObserver<T>.Instance;
				this.isStopped = true;
				value = this.lastValue;
				flag2 = this.hasValue;
			}
			if (flag2)
			{
				observer.OnNext(value);
				observer.OnCompleted();
				return;
			}
			observer.OnCompleted();
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0000F504 File Offset: 0x0000D704
		public void OnError(Exception error)
		{
			if (error == null)
			{
				throw new ArgumentNullException("error");
			}
			object obj = this.observerLock;
			IObserver<T> observer;
			lock (obj)
			{
				this.ThrowIfDisposed();
				if (this.isStopped)
				{
					return;
				}
				observer = this.outObserver;
				this.outObserver = EmptyObserver<T>.Instance;
				this.isStopped = true;
				this.lastError = error;
			}
			observer.OnError(error);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0000F584 File Offset: 0x0000D784
		public void OnNext(T value)
		{
			object obj = this.observerLock;
			lock (obj)
			{
				this.ThrowIfDisposed();
				if (!this.isStopped)
				{
					this.hasValue = true;
					this.lastValue = value;
				}
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0000F5DC File Offset: 0x0000D7DC
		public IDisposable Subscribe(IObserver<T> observer)
		{
			if (observer == null)
			{
				throw new ArgumentNullException("observer");
			}
			Exception ex = null;
			T value = default(T);
			bool flag = false;
			object obj = this.observerLock;
			lock (obj)
			{
				this.ThrowIfDisposed();
				if (!this.isStopped)
				{
					ListObserver<T> listObserver = this.outObserver as ListObserver<T>;
					if (listObserver != null)
					{
						this.outObserver = listObserver.Add(observer);
					}
					else
					{
						IObserver<T> observer2 = this.outObserver;
						if (observer2 is EmptyObserver<T>)
						{
							this.outObserver = observer;
						}
						else
						{
							this.outObserver = new ListObserver<T>(new ImmutableList<IObserver<T>>(new IObserver<T>[]
							{
								observer2,
								observer
							}));
						}
					}
					return new AsyncSubject<T>.Subscription(this, observer);
				}
				ex = this.lastError;
				value = this.lastValue;
				flag = this.hasValue;
			}
			if (ex != null)
			{
				observer.OnError(ex);
			}
			else if (flag)
			{
				observer.OnNext(value);
				observer.OnCompleted();
			}
			else
			{
				observer.OnCompleted();
			}
			return EmptyDisposable.Instance;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0000F6E8 File Offset: 0x0000D8E8
		public void Dispose()
		{
			object obj = this.observerLock;
			lock (obj)
			{
				this.isDisposed = true;
				this.outObserver = DisposedObserver<T>.Instance;
				this.lastError = null;
				this.lastValue = default(T);
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0000F748 File Offset: 0x0000D948
		private void ThrowIfDisposed()
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("");
			}
		}

		// Token: 0x0400014E RID: 334
		private object observerLock = new object();

		// Token: 0x0400014F RID: 335
		private T lastValue;

		// Token: 0x04000150 RID: 336
		private bool hasValue;

		// Token: 0x04000151 RID: 337
		private bool isStopped;

		// Token: 0x04000152 RID: 338
		private bool isDisposed;

		// Token: 0x04000153 RID: 339
		private Exception lastError;

		// Token: 0x04000154 RID: 340
		private IObserver<T> outObserver = EmptyObserver<T>.Instance;

		// Token: 0x0200021D RID: 541
		private class Subscription : IDisposable
		{
			// Token: 0x06000C00 RID: 3072 RVA: 0x0002B273 File Offset: 0x00029473
			public Subscription(AsyncSubject<T> parent, IObserver<T> unsubscribeTarget)
			{
				this.parent = parent;
				this.unsubscribeTarget = unsubscribeTarget;
			}

			// Token: 0x06000C01 RID: 3073 RVA: 0x0002B294 File Offset: 0x00029494
			public void Dispose()
			{
				object obj = this.gate;
				lock (obj)
				{
					if (this.parent != null)
					{
						object observerLock = this.parent.observerLock;
						lock (observerLock)
						{
							ListObserver<T> listObserver = this.parent.outObserver as ListObserver<T>;
							if (listObserver != null)
							{
								this.parent.outObserver = listObserver.Remove(this.unsubscribeTarget);
							}
							else
							{
								this.parent.outObserver = EmptyObserver<T>.Instance;
							}
							this.unsubscribeTarget = null;
							this.parent = null;
						}
					}
				}
			}

			// Token: 0x04000555 RID: 1365
			private readonly object gate = new object();

			// Token: 0x04000556 RID: 1366
			private AsyncSubject<T> parent;

			// Token: 0x04000557 RID: 1367
			private IObserver<T> unsubscribeTarget;
		}
	}
}
