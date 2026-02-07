using System;
using System.Runtime.ConstrainedExecution;
using System.Security;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000715 RID: 1813
	[SecurityCritical]
	public abstract class CriticalHandle : CriticalFinalizerObject, IDisposable
	{
		// Token: 0x060040C9 RID: 16585 RVA: 0x000E1680 File Offset: 0x000DF880
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected CriticalHandle(IntPtr invalidHandleValue)
		{
			this.handle = invalidHandleValue;
			this._isClosed = false;
		}

		// Token: 0x060040CA RID: 16586 RVA: 0x000E1698 File Offset: 0x000DF898
		[SecuritySafeCritical]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		~CriticalHandle()
		{
			this.Dispose(false);
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x000E16C8 File Offset: 0x000DF8C8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SecurityCritical]
		private void Cleanup()
		{
			if (this.IsClosed)
			{
				return;
			}
			this._isClosed = true;
			if (this.IsInvalid)
			{
				return;
			}
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (!this.ReleaseHandle())
			{
				CriticalHandle.FireCustomerDebugProbe();
			}
			Marshal.SetLastWin32Error(lastWin32Error);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060040CC RID: 16588 RVA: 0x00004BF9 File Offset: 0x00002DF9
		private static void FireCustomerDebugProbe()
		{
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x000E1700 File Offset: 0x000DF900
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected void SetHandle(IntPtr handle)
		{
			this.handle = handle;
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x060040CE RID: 16590 RVA: 0x000E1709 File Offset: 0x000DF909
		public bool IsClosed
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._isClosed;
			}
		}

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x060040CF RID: 16591
		public abstract bool IsInvalid { [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)] get; }

		// Token: 0x060040D0 RID: 16592 RVA: 0x000E1711 File Offset: 0x000DF911
		[SecurityCritical]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x060040D1 RID: 16593 RVA: 0x000E1711 File Offset: 0x000DF911
		[SecuritySafeCritical]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x000E171A File Offset: 0x000DF91A
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SecurityCritical]
		protected virtual void Dispose(bool disposing)
		{
			this.Cleanup();
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x000E1722 File Offset: 0x000DF922
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void SetHandleAsInvalid()
		{
			this._isClosed = true;
			GC.SuppressFinalize(this);
		}

		// Token: 0x060040D4 RID: 16596
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected abstract bool ReleaseHandle();

		// Token: 0x04002AF8 RID: 11000
		protected IntPtr handle;

		// Token: 0x04002AF9 RID: 11001
		private bool _isClosed;
	}
}
