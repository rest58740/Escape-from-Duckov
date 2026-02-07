using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200014C RID: 332
	[Serializable]
	public class InvalidOperationException : SystemException
	{
		// Token: 0x06000C8E RID: 3214 RVA: 0x00032824 File Offset: 0x00030A24
		public InvalidOperationException() : base("Operation is not valid due to the current state of the object.")
		{
			base.HResult = -2146233079;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x0003283C File Offset: 0x00030A3C
		public InvalidOperationException(string message) : base(message)
		{
			base.HResult = -2146233079;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00032850 File Offset: 0x00030A50
		public InvalidOperationException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233079;
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected InvalidOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
