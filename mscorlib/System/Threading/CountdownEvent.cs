using System;
using System.Diagnostics;

namespace System.Threading
{
	// Token: 0x0200029D RID: 669
	[DebuggerDisplay("Initial Count={InitialCount}, Current Count={CurrentCount}")]
	public class CountdownEvent : IDisposable
	{
		// Token: 0x06001DAB RID: 7595 RVA: 0x0006E7D3 File Offset: 0x0006C9D3
		public CountdownEvent(int initialCount)
		{
			if (initialCount < 0)
			{
				throw new ArgumentOutOfRangeException("initialCount");
			}
			this._initialCount = initialCount;
			this._currentCount = initialCount;
			this._event = new ManualResetEventSlim();
			if (initialCount == 0)
			{
				this._event.Set();
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06001DAC RID: 7596 RVA: 0x0006E814 File Offset: 0x0006CA14
		public int CurrentCount
		{
			get
			{
				int currentCount = this._currentCount;
				if (currentCount >= 0)
				{
					return currentCount;
				}
				return 0;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06001DAD RID: 7597 RVA: 0x0006E831 File Offset: 0x0006CA31
		public int InitialCount
		{
			get
			{
				return this._initialCount;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06001DAE RID: 7598 RVA: 0x0006E839 File Offset: 0x0006CA39
		public bool IsSet
		{
			get
			{
				return this._currentCount <= 0;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06001DAF RID: 7599 RVA: 0x0006E849 File Offset: 0x0006CA49
		public WaitHandle WaitHandle
		{
			get
			{
				this.ThrowIfDisposed();
				return this._event.WaitHandle;
			}
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x0006E85C File Offset: 0x0006CA5C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x0006E86B File Offset: 0x0006CA6B
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._event.Dispose();
				this._disposed = true;
			}
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x0006E884 File Offset: 0x0006CA84
		public bool Signal()
		{
			this.ThrowIfDisposed();
			if (this._currentCount <= 0)
			{
				throw new InvalidOperationException("Invalid attempt made to decrement the event's count below zero.");
			}
			int num = Interlocked.Decrement(ref this._currentCount);
			if (num == 0)
			{
				this._event.Set();
				return true;
			}
			if (num < 0)
			{
				throw new InvalidOperationException("Invalid attempt made to decrement the event's count below zero.");
			}
			return false;
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x0006E8DC File Offset: 0x0006CADC
		public bool Signal(int signalCount)
		{
			if (signalCount <= 0)
			{
				throw new ArgumentOutOfRangeException("signalCount");
			}
			this.ThrowIfDisposed();
			SpinWait spinWait = default(SpinWait);
			int currentCount;
			for (;;)
			{
				currentCount = this._currentCount;
				if (currentCount < signalCount)
				{
					break;
				}
				if (Interlocked.CompareExchange(ref this._currentCount, currentCount - signalCount, currentCount) == currentCount)
				{
					goto IL_50;
				}
				spinWait.SpinOnce();
			}
			throw new InvalidOperationException("Invalid attempt made to decrement the event's count below zero.");
			IL_50:
			if (currentCount == signalCount)
			{
				this._event.Set();
				return true;
			}
			return false;
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x0006E94B File Offset: 0x0006CB4B
		public void AddCount()
		{
			this.AddCount(1);
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x0006E954 File Offset: 0x0006CB54
		public bool TryAddCount()
		{
			return this.TryAddCount(1);
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x0006E95D File Offset: 0x0006CB5D
		public void AddCount(int signalCount)
		{
			if (!this.TryAddCount(signalCount))
			{
				throw new InvalidOperationException("The event is already signaled and cannot be incremented.");
			}
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x0006E974 File Offset: 0x0006CB74
		public bool TryAddCount(int signalCount)
		{
			if (signalCount <= 0)
			{
				throw new ArgumentOutOfRangeException("signalCount");
			}
			this.ThrowIfDisposed();
			SpinWait spinWait = default(SpinWait);
			for (;;)
			{
				int currentCount = this._currentCount;
				if (currentCount <= 0)
				{
					break;
				}
				if (currentCount > 2147483647 - signalCount)
				{
					goto Block_3;
				}
				if (Interlocked.CompareExchange(ref this._currentCount, currentCount + signalCount, currentCount) == currentCount)
				{
					return true;
				}
				spinWait.SpinOnce();
			}
			return false;
			Block_3:
			throw new InvalidOperationException("The increment operation would cause the CurrentCount to overflow.");
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x0006E9DE File Offset: 0x0006CBDE
		public void Reset()
		{
			this.Reset(this._initialCount);
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x0006E9EC File Offset: 0x0006CBEC
		public void Reset(int count)
		{
			this.ThrowIfDisposed();
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this._currentCount = count;
			this._initialCount = count;
			if (count == 0)
			{
				this._event.Set();
				return;
			}
			this._event.Reset();
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x0006EA38 File Offset: 0x0006CC38
		public void Wait()
		{
			this.Wait(-1, default(CancellationToken));
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x0006EA56 File Offset: 0x0006CC56
		public void Wait(CancellationToken cancellationToken)
		{
			this.Wait(-1, cancellationToken);
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x0006EA64 File Offset: 0x0006CC64
		public bool Wait(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			return this.Wait((int)num, default(CancellationToken));
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x0006EAA4 File Offset: 0x0006CCA4
		public bool Wait(TimeSpan timeout, CancellationToken cancellationToken)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			return this.Wait((int)num, cancellationToken);
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x0006EADC File Offset: 0x0006CCDC
		public bool Wait(int millisecondsTimeout)
		{
			return this.Wait(millisecondsTimeout, default(CancellationToken));
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x0006EAFC File Offset: 0x0006CCFC
		public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
		{
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout");
			}
			this.ThrowIfDisposed();
			cancellationToken.ThrowIfCancellationRequested();
			bool flag = this.IsSet;
			if (!flag)
			{
				flag = this._event.Wait(millisecondsTimeout, cancellationToken);
			}
			return flag;
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x0006EB3E File Offset: 0x0006CD3E
		private void ThrowIfDisposed()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("CountdownEvent");
			}
		}

		// Token: 0x04001A52 RID: 6738
		private int _initialCount;

		// Token: 0x04001A53 RID: 6739
		private volatile int _currentCount;

		// Token: 0x04001A54 RID: 6740
		private ManualResetEventSlim _event;

		// Token: 0x04001A55 RID: 6741
		private volatile bool _disposed;
	}
}
