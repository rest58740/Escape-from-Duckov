using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200014B RID: 331
	[Serializable]
	public class InvalidCastException : SystemException
	{
		// Token: 0x06000C89 RID: 3209 RVA: 0x000327D3 File Offset: 0x000309D3
		public InvalidCastException() : base("Specified cast is not valid.")
		{
			base.HResult = -2147467262;
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x000327EB File Offset: 0x000309EB
		public InvalidCastException(string message) : base(message)
		{
			base.HResult = -2147467262;
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x000327FF File Offset: 0x000309FF
		public InvalidCastException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147467262;
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00032814 File Offset: 0x00030A14
		public InvalidCastException(string message, int errorCode) : base(message)
		{
			base.HResult = errorCode;
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected InvalidCastException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
