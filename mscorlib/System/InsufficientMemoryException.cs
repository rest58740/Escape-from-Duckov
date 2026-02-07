using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020001CD RID: 461
	[Serializable]
	public sealed class InsufficientMemoryException : OutOfMemoryException
	{
		// Token: 0x060013C5 RID: 5061 RVA: 0x0004E9E7 File Offset: 0x0004CBE7
		public InsufficientMemoryException() : base("Insufficient memory to continue the execution of the program.")
		{
			base.HResult = -2146233027;
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x0004E9FF File Offset: 0x0004CBFF
		public InsufficientMemoryException(string message) : base(message)
		{
			base.HResult = -2146233027;
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x0004EA13 File Offset: 0x0004CC13
		public InsufficientMemoryException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233027;
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x0004EA28 File Offset: 0x0004CC28
		private InsufficientMemoryException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
