using System;
using System.Security;

namespace System.Threading
{
	// Token: 0x020002CA RID: 714
	public struct AsyncFlowControl : IDisposable
	{
		// Token: 0x06001EE8 RID: 7912 RVA: 0x00072BE4 File Offset: 0x00070DE4
		[SecurityCritical]
		internal void Setup()
		{
			this.useEC = true;
			Thread currentThread = Thread.CurrentThread;
			this._ec = currentThread.GetMutableExecutionContext();
			this._ec.isFlowSuppressed = true;
			this._thread = currentThread;
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x00072C1D File Offset: 0x00070E1D
		public void Dispose()
		{
			this.Undo();
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x00072C28 File Offset: 0x00070E28
		[SecuritySafeCritical]
		public void Undo()
		{
			if (this._thread == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("AsyncFlowControl object can be used only once to call Undo()."));
			}
			if (this._thread != Thread.CurrentThread)
			{
				throw new InvalidOperationException(Environment.GetResourceString("AsyncFlowControl object must be used on the thread where it was created."));
			}
			if (this.useEC)
			{
				if (Thread.CurrentThread.GetMutableExecutionContext() != this._ec)
				{
					throw new InvalidOperationException(Environment.GetResourceString("AsyncFlowControl objects can be used to restore flow only on the Context that had its flow suppressed."));
				}
				ExecutionContext.RestoreFlow();
			}
			this._thread = null;
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x00072CA0 File Offset: 0x00070EA0
		public override int GetHashCode()
		{
			if (this._thread != null)
			{
				return this._thread.GetHashCode();
			}
			return this.ToString().GetHashCode();
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x00072CC7 File Offset: 0x00070EC7
		public override bool Equals(object obj)
		{
			return obj is AsyncFlowControl && this.Equals((AsyncFlowControl)obj);
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x00072CDF File Offset: 0x00070EDF
		public bool Equals(AsyncFlowControl obj)
		{
			return obj.useEC == this.useEC && obj._ec == this._ec && obj._thread == this._thread;
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x00072D0D File Offset: 0x00070F0D
		public static bool operator ==(AsyncFlowControl a, AsyncFlowControl b)
		{
			return a.Equals(b);
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x00072D17 File Offset: 0x00070F17
		public static bool operator !=(AsyncFlowControl a, AsyncFlowControl b)
		{
			return !(a == b);
		}

		// Token: 0x04001AF2 RID: 6898
		private bool useEC;

		// Token: 0x04001AF3 RID: 6899
		private ExecutionContext _ec;

		// Token: 0x04001AF4 RID: 6900
		private Thread _thread;
	}
}
