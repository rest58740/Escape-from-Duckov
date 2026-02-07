using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000F4 RID: 244
	[Serializable]
	public class ArgumentNullException : ArgumentException
	{
		// Token: 0x06000707 RID: 1799 RVA: 0x000210B8 File Offset: 0x0001F2B8
		public ArgumentNullException() : base("Value cannot be null.")
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x000210D0 File Offset: 0x0001F2D0
		public ArgumentNullException(string paramName) : base("Value cannot be null.", paramName)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x000210E9 File Offset: 0x0001F2E9
		public ArgumentNullException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x000210FE File Offset: 0x0001F2FE
		public ArgumentNullException(string paramName, string message) : base(message, paramName)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00021113 File Offset: 0x0001F313
		protected ArgumentNullException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
