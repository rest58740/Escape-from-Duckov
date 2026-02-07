using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000195 RID: 405
	[Serializable]
	public class TimeoutException : SystemException
	{
		// Token: 0x0600102F RID: 4143 RVA: 0x000425C7 File Offset: 0x000407C7
		public TimeoutException() : base("The operation has timed out.")
		{
			base.HResult = -2146233083;
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x000425DF File Offset: 0x000407DF
		public TimeoutException(string message) : base(message)
		{
			base.HResult = -2146233083;
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x000425F3 File Offset: 0x000407F3
		public TimeoutException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233083;
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected TimeoutException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
