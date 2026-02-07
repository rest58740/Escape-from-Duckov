using System;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x0200029A RID: 666
	[Serializable]
	public class ThreadStateException : SystemException
	{
		// Token: 0x06001DA2 RID: 7586 RVA: 0x0006E736 File Offset: 0x0006C936
		public ThreadStateException() : base("Thread was in an invalid state for the operation being executed.")
		{
			base.HResult = -2146233056;
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x0006E74E File Offset: 0x0006C94E
		public ThreadStateException(string message) : base(message)
		{
			base.HResult = -2146233056;
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x0006E762 File Offset: 0x0006C962
		public ThreadStateException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233056;
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected ThreadStateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
