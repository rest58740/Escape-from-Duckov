using System;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x0200027F RID: 639
	[Serializable]
	public class AbandonedMutexException : SystemException
	{
		// Token: 0x06001D50 RID: 7504 RVA: 0x0006D9A7 File Offset: 0x0006BBA7
		public AbandonedMutexException() : base("The wait completed due to an abandoned mutex.")
		{
			base.HResult = -2146233043;
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x0006D9C6 File Offset: 0x0006BBC6
		public AbandonedMutexException(string message) : base(message)
		{
			base.HResult = -2146233043;
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x0006D9E1 File Offset: 0x0006BBE1
		public AbandonedMutexException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233043;
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x0006D9FD File Offset: 0x0006BBFD
		public AbandonedMutexException(int location, WaitHandle handle) : base("The wait completed due to an abandoned mutex.")
		{
			base.HResult = -2146233043;
			this.SetupException(location, handle);
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x0006DA24 File Offset: 0x0006BC24
		public AbandonedMutexException(string message, int location, WaitHandle handle) : base(message)
		{
			base.HResult = -2146233043;
			this.SetupException(location, handle);
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x0006DA47 File Offset: 0x0006BC47
		public AbandonedMutexException(string message, Exception inner, int location, WaitHandle handle) : base(message, inner)
		{
			base.HResult = -2146233043;
			this.SetupException(location, handle);
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x0006DA6C File Offset: 0x0006BC6C
		protected AbandonedMutexException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x0006DA7D File Offset: 0x0006BC7D
		private void SetupException(int location, WaitHandle handle)
		{
			this._mutexIndex = location;
			if (handle != null)
			{
				this._mutex = (handle as Mutex);
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001D58 RID: 7512 RVA: 0x0006DA95 File Offset: 0x0006BC95
		public Mutex Mutex
		{
			get
			{
				return this._mutex;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001D59 RID: 7513 RVA: 0x0006DA9D File Offset: 0x0006BC9D
		public int MutexIndex
		{
			get
			{
				return this._mutexIndex;
			}
		}

		// Token: 0x04001A1E RID: 6686
		private int _mutexIndex = -1;

		// Token: 0x04001A1F RID: 6687
		private Mutex _mutex;
	}
}
