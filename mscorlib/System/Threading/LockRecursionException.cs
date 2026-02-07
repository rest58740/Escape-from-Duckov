using System;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x02000290 RID: 656
	[Serializable]
	public class LockRecursionException : Exception
	{
		// Token: 0x06001D86 RID: 7558 RVA: 0x00004B05 File Offset: 0x00002D05
		public LockRecursionException()
		{
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x000328A6 File Offset: 0x00030AA6
		public LockRecursionException(string message) : base(message)
		{
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x000328AF File Offset: 0x00030AAF
		public LockRecursionException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x00020FAB File Offset: 0x0001F1AB
		protected LockRecursionException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
