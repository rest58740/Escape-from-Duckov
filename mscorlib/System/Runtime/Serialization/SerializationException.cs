using System;

namespace System.Runtime.Serialization
{
	// Token: 0x0200064B RID: 1611
	[Serializable]
	public class SerializationException : SystemException
	{
		// Token: 0x06003C4F RID: 15439 RVA: 0x000D1487 File Offset: 0x000CF687
		public SerializationException() : base(SerializationException.s_nullMessage)
		{
			base.HResult = -2146233076;
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x000D149F File Offset: 0x000CF69F
		public SerializationException(string message) : base(message)
		{
			base.HResult = -2146233076;
		}

		// Token: 0x06003C51 RID: 15441 RVA: 0x000D14B3 File Offset: 0x000CF6B3
		public SerializationException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233076;
		}

		// Token: 0x06003C52 RID: 15442 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected SerializationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x04002715 RID: 10005
		private static string s_nullMessage = "Serialization error.";
	}
}
