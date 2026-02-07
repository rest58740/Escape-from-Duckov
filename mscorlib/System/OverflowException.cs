using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200016B RID: 363
	[Serializable]
	public class OverflowException : ArithmeticException
	{
		// Token: 0x06000E6E RID: 3694 RVA: 0x0003AE11 File Offset: 0x00039011
		public OverflowException() : base("Arithmetic operation resulted in an overflow.")
		{
			base.HResult = -2146233066;
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x0003AE29 File Offset: 0x00039029
		public OverflowException(string message) : base(message)
		{
			base.HResult = -2146233066;
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x0003AE3D File Offset: 0x0003903D
		public OverflowException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233066;
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00028207 File Offset: 0x00026407
		protected OverflowException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
