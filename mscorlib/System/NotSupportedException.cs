using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000161 RID: 353
	[Serializable]
	public class NotSupportedException : SystemException
	{
		// Token: 0x06000DE0 RID: 3552 RVA: 0x00035FC0 File Offset: 0x000341C0
		public NotSupportedException() : base("Specified method is not supported.")
		{
			base.HResult = -2146233067;
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x00035FD8 File Offset: 0x000341D8
		public NotSupportedException(string message) : base(message)
		{
			base.HResult = -2146233067;
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00035FEC File Offset: 0x000341EC
		public NotSupportedException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233067;
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected NotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
