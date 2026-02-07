using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000110 RID: 272
	[Serializable]
	public class DivideByZeroException : ArithmeticException
	{
		// Token: 0x06000A8B RID: 2699 RVA: 0x000281C6 File Offset: 0x000263C6
		public DivideByZeroException() : base("Attempted to divide by zero.")
		{
			base.HResult = -2147352558;
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x000281DE File Offset: 0x000263DE
		public DivideByZeroException(string message) : base(message)
		{
			base.HResult = -2147352558;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x000281F2 File Offset: 0x000263F2
		public DivideByZeroException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147352558;
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x00028207 File Offset: 0x00026407
		protected DivideByZeroException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
