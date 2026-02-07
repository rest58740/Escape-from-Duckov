using System;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x02000295 RID: 661
	[Serializable]
	public class SynchronizationLockException : SystemException
	{
		// Token: 0x06001D97 RID: 7575 RVA: 0x0006E6C4 File Offset: 0x0006C8C4
		public SynchronizationLockException() : base("Object synchronization method was called from an unsynchronized block of code.")
		{
			base.HResult = -2146233064;
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x0006E6DC File Offset: 0x0006C8DC
		public SynchronizationLockException(string message) : base(message)
		{
			base.HResult = -2146233064;
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x0006E6F0 File Offset: 0x0006C8F0
		public SynchronizationLockException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233064;
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected SynchronizationLockException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
