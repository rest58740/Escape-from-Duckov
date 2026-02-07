using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	// Token: 0x020002F1 RID: 753
	[ComVisible(true)]
	public struct LockCookie
	{
		// Token: 0x060020CE RID: 8398 RVA: 0x00076A9A File Offset: 0x00074C9A
		internal LockCookie(int thread_id)
		{
			this.ThreadId = thread_id;
			this.ReaderLocks = 0;
			this.WriterLocks = 0;
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x00076AB1 File Offset: 0x00074CB1
		internal LockCookie(int thread_id, int reader_locks, int writer_locks)
		{
			this.ThreadId = thread_id;
			this.ReaderLocks = reader_locks;
			this.WriterLocks = writer_locks;
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x00076AC8 File Offset: 0x00074CC8
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x00076ADA File Offset: 0x00074CDA
		public bool Equals(LockCookie obj)
		{
			return this.ThreadId == obj.ThreadId && this.ReaderLocks == obj.ReaderLocks && this.WriterLocks == obj.WriterLocks;
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x00076B09 File Offset: 0x00074D09
		public override bool Equals(object obj)
		{
			return obj is LockCookie && obj.Equals(this);
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x00076B26 File Offset: 0x00074D26
		public static bool operator ==(LockCookie a, LockCookie b)
		{
			return a.Equals(b);
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x00076B30 File Offset: 0x00074D30
		public static bool operator !=(LockCookie a, LockCookie b)
		{
			return !a.Equals(b);
		}

		// Token: 0x04001B6D RID: 7021
		internal int ThreadId;

		// Token: 0x04001B6E RID: 7022
		internal int ReaderLocks;

		// Token: 0x04001B6F RID: 7023
		internal int WriterLocks;
	}
}
