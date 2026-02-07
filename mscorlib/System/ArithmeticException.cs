using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000F6 RID: 246
	[Serializable]
	public class ArithmeticException : SystemException
	{
		// Token: 0x06000715 RID: 1813 RVA: 0x0002122D File Offset: 0x0001F42D
		public ArithmeticException() : base("Overflow or underflow in the arithmetic operation.")
		{
			base.HResult = -2147024362;
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00021245 File Offset: 0x0001F445
		public ArithmeticException(string message) : base(message)
		{
			base.HResult = -2147024362;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00021259 File Offset: 0x0001F459
		public ArithmeticException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147024362;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected ArithmeticException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
