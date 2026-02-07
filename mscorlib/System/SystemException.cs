using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200018E RID: 398
	[Serializable]
	public class SystemException : Exception
	{
		// Token: 0x06000FC8 RID: 4040 RVA: 0x000419FE File Offset: 0x0003FBFE
		public SystemException() : base("System error.")
		{
			base.HResult = -2146233087;
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00041A16 File Offset: 0x0003FC16
		public SystemException(string message) : base(message)
		{
			base.HResult = -2146233087;
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x00041A2A File Offset: 0x0003FC2A
		public SystemException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233087;
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00020FAB File Offset: 0x0001F1AB
		protected SystemException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
