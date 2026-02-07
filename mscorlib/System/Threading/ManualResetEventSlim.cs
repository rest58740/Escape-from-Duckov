using System;
using System.Diagnostics;

namespace System.Threading
{
	// Token: 0x020002A2 RID: 674
	[DebuggerDisplay("Set = {IsSet}")]
	public class ManualResetEventSlim : IDisposable
	{
		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06001DDD RID: 7645 RVA: 0x0006EE0E File Offset: 0x0006D00E
		public WaitHandle WaitHandle
		{
			get
			{
				this.ThrowIfDisposed();
				if (this.m_eventObj == null)
				{
					this.LazyInitializeEvent();
				}
				return this.m_eventObj;
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06001DDE RID: 7646 RVA: 0x0006EE2F File Offset: 0x0006D02F
		// (set) Token: 0x06001DDF RID: 7647 RVA: 0x0006EE46 File Offset: 0x0006D046
		public bool IsSet
		{
			get
			{
				return ManualResetEventSlim.ExtractStatePortion(this.m_combinedState, int.MinValue) != 0;
			}
			private set
			{
				this.UpdateStateAtomically((value ? 1 : 0) << 31, int.MinValue);
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06001DE0 RID: 7648 RVA: 0x0006EE5D File Offset: 0x0006D05D
		// (set) Token: 0x06001DE1 RID: 7649 RVA: 0x0006EE73 File Offset: 0x0006D073
		public int SpinCount
		{
			get
			{
				return ManualResetEventSlim.ExtractStatePortionAndShiftRight(this.m_combinedState, 1073217536, 19);
			}
			private set
			{
				this.m_combinedState = ((this.m_combinedState & -1073217537) | value << 19);
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06001DE2 RID: 7650 RVA: 0x0006EE90 File Offset: 0x0006D090
		// (set) Token: 0x06001DE3 RID: 7651 RVA: 0x0006EEA5 File Offset: 0x0006D0A5
		private int Waiters
		{
			get
			{
				return ManualResetEventSlim.ExtractStatePortionAndShiftRight(this.m_combinedState, 524287, 0);
			}
			set
			{
				if (value >= 524287)
				{
					throw new InvalidOperationException(string.Format("There are too many threads currently waiting on the event. A maximum of {0} waiting threads are supported.", 524287));
				}
				this.UpdateStateAtomically(value, 524287);
			}
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x0006EED5 File Offset: 0x0006D0D5
		public ManualResetEventSlim() : this(false)
		{
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x0006EEDE File Offset: 0x0006D0DE
		public ManualResetEventSlim(bool initialState)
		{
			this.Initialize(initialState, SpinWait.SpinCountforSpinBeforeWait);
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x0006EEF4 File Offset: 0x0006D0F4
		public ManualResetEventSlim(bool initialState, int spinCount)
		{
			if (spinCount < 0)
			{
				throw new ArgumentOutOfRangeException("spinCount");
			}
			if (spinCount > 2047)
			{
				throw new ArgumentOutOfRangeException("spinCount", string.Format("The spinCount argument must be in the range 0 to {0}, inclusive.", 2047));
			}
			this.Initialize(initialState, spinCount);
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x0006EF45 File Offset: 0x0006D145
		private void Initialize(bool initialState, int spinCount)
		{
			this.m_combinedState = (initialState ? int.MinValue : 0);
			this.SpinCount = (PlatformHelper.IsSingleProcessor ? 1 : spinCount);
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x0006EF6C File Offset: 0x0006D16C
		private void EnsureLockObjectCreated()
		{
			if (this.m_lock != null)
			{
				return;
			}
			object value = new object();
			Interlocked.CompareExchange(ref this.m_lock, value, null);
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x0006EF98 File Offset: 0x0006D198
		private bool LazyInitializeEvent()
		{
			bool isSet = this.IsSet;
			ManualResetEvent manualResetEvent = new ManualResetEvent(isSet);
			if (Interlocked.CompareExchange<ManualResetEvent>(ref this.m_eventObj, manualResetEvent, null) != null)
			{
				manualResetEvent.Dispose();
				return false;
			}
			if (this.IsSet != isSet)
			{
				ManualResetEvent obj = manualResetEvent;
				lock (obj)
				{
					if (this.m_eventObj == manualResetEvent)
					{
						manualResetEvent.Set();
					}
				}
			}
			return true;
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x0006F010 File Offset: 0x0006D210
		public void Set()
		{
			this.Set(false);
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x0006F01C File Offset: 0x0006D21C
		private void Set(bool duringCancellation)
		{
			this.IsSet = true;
			if (this.Waiters > 0)
			{
				object @lock = this.m_lock;
				lock (@lock)
				{
					Monitor.PulseAll(this.m_lock);
				}
			}
			ManualResetEvent eventObj = this.m_eventObj;
			if (eventObj != null && !duringCancellation)
			{
				ManualResetEvent obj = eventObj;
				lock (obj)
				{
					if (this.m_eventObj != null)
					{
						this.m_eventObj.Set();
					}
				}
			}
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x0006F0C0 File Offset: 0x0006D2C0
		public void Reset()
		{
			this.ThrowIfDisposed();
			if (this.m_eventObj != null)
			{
				this.m_eventObj.Reset();
			}
			this.IsSet = false;
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x0006F0E8 File Offset: 0x0006D2E8
		public void Wait()
		{
			this.Wait(-1, default(CancellationToken));
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x0006F106 File Offset: 0x0006D306
		public void Wait(CancellationToken cancellationToken)
		{
			this.Wait(-1, cancellationToken);
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x0006F114 File Offset: 0x0006D314
		public bool Wait(TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			return this.Wait((int)num, default(CancellationToken));
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x0006F154 File Offset: 0x0006D354
		public bool Wait(TimeSpan timeout, CancellationToken cancellationToken)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			return this.Wait((int)num, cancellationToken);
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x0006F18C File Offset: 0x0006D38C
		public bool Wait(int millisecondsTimeout)
		{
			return this.Wait(millisecondsTimeout, default(CancellationToken));
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x0006F1AC File Offset: 0x0006D3AC
		public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
		{
			this.ThrowIfDisposed();
			cancellationToken.ThrowIfCancellationRequested();
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout");
			}
			if (!this.IsSet)
			{
				if (millisecondsTimeout == 0)
				{
					return false;
				}
				uint startTime = 0U;
				bool flag = false;
				int num = millisecondsTimeout;
				if (millisecondsTimeout != -1)
				{
					startTime = TimeoutHelper.GetTime();
					flag = true;
				}
				int spinCount = this.SpinCount;
				SpinWait spinWait = default(SpinWait);
				while (spinWait.Count < spinCount)
				{
					spinWait.SpinOnce(40);
					if (this.IsSet)
					{
						return true;
					}
					if (spinWait.Count >= 100 && spinWait.Count % 10 == 0)
					{
						cancellationToken.ThrowIfCancellationRequested();
					}
				}
				this.EnsureLockObjectCreated();
				using (cancellationToken.InternalRegisterWithoutEC(ManualResetEventSlim.s_cancellationTokenCallback, this))
				{
					object @lock = this.m_lock;
					lock (@lock)
					{
						while (!this.IsSet)
						{
							cancellationToken.ThrowIfCancellationRequested();
							if (flag)
							{
								num = TimeoutHelper.UpdateTimeOut(startTime, millisecondsTimeout);
								if (num <= 0)
								{
									return false;
								}
							}
							this.Waiters++;
							if (this.IsSet)
							{
								int waiters = this.Waiters;
								this.Waiters = waiters - 1;
								return true;
							}
							try
							{
								if (!Monitor.Wait(this.m_lock, num))
								{
									return false;
								}
							}
							finally
							{
								this.Waiters--;
							}
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x0006F330 File Offset: 0x0006D530
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x0006F340 File Offset: 0x0006D540
		protected virtual void Dispose(bool disposing)
		{
			if ((this.m_combinedState & 1073741824) != 0)
			{
				return;
			}
			this.m_combinedState |= 1073741824;
			if (disposing)
			{
				ManualResetEvent eventObj = this.m_eventObj;
				if (eventObj != null)
				{
					ManualResetEvent obj = eventObj;
					lock (obj)
					{
						eventObj.Dispose();
						this.m_eventObj = null;
					}
				}
			}
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x0006F3BC File Offset: 0x0006D5BC
		private void ThrowIfDisposed()
		{
			if ((this.m_combinedState & 1073741824) != 0)
			{
				throw new ObjectDisposedException("The event has been disposed.");
			}
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x0006F3DC File Offset: 0x0006D5DC
		private static void CancellationTokenCallback(object obj)
		{
			ManualResetEventSlim manualResetEventSlim = obj as ManualResetEventSlim;
			object @lock = manualResetEventSlim.m_lock;
			lock (@lock)
			{
				Monitor.PulseAll(manualResetEventSlim.m_lock);
			}
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x0006F42C File Offset: 0x0006D62C
		private void UpdateStateAtomically(int newBits, int updateBitsMask)
		{
			SpinWait spinWait = default(SpinWait);
			for (;;)
			{
				int combinedState = this.m_combinedState;
				int value = (combinedState & ~updateBitsMask) | newBits;
				if (Interlocked.CompareExchange(ref this.m_combinedState, value, combinedState) == combinedState)
				{
					break;
				}
				spinWait.SpinOnce();
			}
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x0006F46A File Offset: 0x0006D66A
		private static int ExtractStatePortionAndShiftRight(int state, int mask, int rightBitShiftCount)
		{
			return (int)((uint)(state & mask) >> rightBitShiftCount);
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x0006F474 File Offset: 0x0006D674
		private static int ExtractStatePortion(int state, int mask)
		{
			return state & mask;
		}

		// Token: 0x04001A5A RID: 6746
		private const int DEFAULT_SPIN_SP = 1;

		// Token: 0x04001A5B RID: 6747
		private volatile object m_lock;

		// Token: 0x04001A5C RID: 6748
		private volatile ManualResetEvent m_eventObj;

		// Token: 0x04001A5D RID: 6749
		private volatile int m_combinedState;

		// Token: 0x04001A5E RID: 6750
		private const int SignalledState_BitMask = -2147483648;

		// Token: 0x04001A5F RID: 6751
		private const int SignalledState_ShiftCount = 31;

		// Token: 0x04001A60 RID: 6752
		private const int Dispose_BitMask = 1073741824;

		// Token: 0x04001A61 RID: 6753
		private const int SpinCountState_BitMask = 1073217536;

		// Token: 0x04001A62 RID: 6754
		private const int SpinCountState_ShiftCount = 19;

		// Token: 0x04001A63 RID: 6755
		private const int SpinCountState_MaxValue = 2047;

		// Token: 0x04001A64 RID: 6756
		private const int NumWaitersState_BitMask = 524287;

		// Token: 0x04001A65 RID: 6757
		private const int NumWaitersState_ShiftCount = 0;

		// Token: 0x04001A66 RID: 6758
		private const int NumWaitersState_MaxValue = 524287;

		// Token: 0x04001A67 RID: 6759
		private static Action<object> s_cancellationTokenCallback = new Action<object>(ManualResetEventSlim.CancellationTokenCallback);
	}
}
