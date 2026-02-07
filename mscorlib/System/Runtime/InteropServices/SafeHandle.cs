using System;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Threading;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200071D RID: 1821
	[SecurityCritical]
	[StructLayout(LayoutKind.Sequential)]
	public abstract class SafeHandle : CriticalFinalizerObject, IDisposable
	{
		// Token: 0x060040ED RID: 16621 RVA: 0x000E17F8 File Offset: 0x000DF9F8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected SafeHandle(IntPtr invalidHandleValue, bool ownsHandle)
		{
			this.handle = invalidHandleValue;
			this._state = 4;
			this._ownsHandle = ownsHandle;
			if (!ownsHandle)
			{
				GC.SuppressFinalize(this);
			}
			this._fullyInitialized = true;
		}

		// Token: 0x060040EE RID: 16622 RVA: 0x000E1828 File Offset: 0x000DFA28
		[SecuritySafeCritical]
		~SafeHandle()
		{
			this.Dispose(false);
		}

		// Token: 0x060040EF RID: 16623 RVA: 0x000E1858 File Offset: 0x000DFA58
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected void SetHandle(IntPtr handle)
		{
			this.handle = handle;
		}

		// Token: 0x060040F0 RID: 16624 RVA: 0x000E1861 File Offset: 0x000DFA61
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public IntPtr DangerousGetHandle()
		{
			return this.handle;
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x060040F1 RID: 16625 RVA: 0x000E1869 File Offset: 0x000DFA69
		public bool IsClosed
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return (this._state & 1) == 1;
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x060040F2 RID: 16626
		public abstract bool IsInvalid { [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)] get; }

		// Token: 0x060040F3 RID: 16627 RVA: 0x000E1876 File Offset: 0x000DFA76
		[SecurityCritical]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x000E1876 File Offset: 0x000DFA76
		[SecuritySafeCritical]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060040F5 RID: 16629 RVA: 0x000E187F File Offset: 0x000DFA7F
		[SecurityCritical]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.InternalDispose();
				return;
			}
			this.InternalFinalize();
		}

		// Token: 0x060040F6 RID: 16630
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected abstract bool ReleaseHandle();

		// Token: 0x060040F7 RID: 16631 RVA: 0x000E1894 File Offset: 0x000DFA94
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void SetHandleAsInvalid()
		{
			try
			{
			}
			finally
			{
				int state;
				int value;
				do
				{
					state = this._state;
					value = (state | 1);
				}
				while (Interlocked.CompareExchange(ref this._state, value, state) != state);
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060040F8 RID: 16632 RVA: 0x000E18D8 File Offset: 0x000DFAD8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void DangerousAddRef(ref bool success)
		{
			try
			{
			}
			finally
			{
				if (!this._fullyInitialized)
				{
					throw new InvalidOperationException();
				}
				for (;;)
				{
					int state = this._state;
					if ((state & 1) != 0)
					{
						break;
					}
					int value = state + 4;
					if (Interlocked.CompareExchange(ref this._state, value, state) == state)
					{
						goto Block_5;
					}
				}
				throw new ObjectDisposedException(null, "Safe handle has been closed");
				Block_5:
				success = true;
			}
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x000E1938 File Offset: 0x000DFB38
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void DangerousRelease()
		{
			this.DangerousReleaseInternal(false);
		}

		// Token: 0x060040FA RID: 16634 RVA: 0x000E1941 File Offset: 0x000DFB41
		private void InternalDispose()
		{
			if (!this._fullyInitialized)
			{
				throw new InvalidOperationException();
			}
			this.DangerousReleaseInternal(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x000E195E File Offset: 0x000DFB5E
		private void InternalFinalize()
		{
			if (this._fullyInitialized)
			{
				this.DangerousReleaseInternal(true);
			}
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x000E1970 File Offset: 0x000DFB70
		private void DangerousReleaseInternal(bool dispose)
		{
			try
			{
			}
			finally
			{
				if (!this._fullyInitialized)
				{
					throw new InvalidOperationException();
				}
				bool flag;
				for (;;)
				{
					int state = this._state;
					if (dispose && (state & 2) != 0)
					{
						break;
					}
					if ((state & 2147483644) == 0)
					{
						goto Block_6;
					}
					flag = ((state & 2147483644) == 4 && (state & 1) == 0 && this._ownsHandle && !this.IsInvalid);
					int num = state - 4;
					if ((state & 2147483644) == 4)
					{
						num |= 1;
					}
					if (dispose)
					{
						num |= 2;
					}
					if (Interlocked.CompareExchange(ref this._state, num, state) == state)
					{
						goto IL_9A;
					}
				}
				flag = false;
				goto IL_9A;
				Block_6:
				throw new ObjectDisposedException(null, "Safe handle has been closed");
				IL_9A:
				if (flag)
				{
					this.ReleaseHandle();
				}
			}
		}

		// Token: 0x04002B02 RID: 11010
		protected IntPtr handle;

		// Token: 0x04002B03 RID: 11011
		private int _state;

		// Token: 0x04002B04 RID: 11012
		private bool _ownsHandle;

		// Token: 0x04002B05 RID: 11013
		private bool _fullyInitialized;

		// Token: 0x04002B06 RID: 11014
		private const int RefCount_Mask = 2147483644;

		// Token: 0x04002B07 RID: 11015
		private const int RefCount_One = 4;

		// Token: 0x0200071E RID: 1822
		private enum State
		{
			// Token: 0x04002B09 RID: 11017
			Closed = 1,
			// Token: 0x04002B0A RID: 11018
			Disposed
		}
	}
}
