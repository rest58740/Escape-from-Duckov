using System;
using System.Runtime.Serialization;

namespace System.Security
{
	// Token: 0x020003C4 RID: 964
	[Serializable]
	public class VerificationException : SystemException
	{
		// Token: 0x06002848 RID: 10312 RVA: 0x00092A14 File Offset: 0x00090C14
		public VerificationException() : base("Operation could destabilize the runtime.")
		{
			base.HResult = -2146233075;
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x00092A2C File Offset: 0x00090C2C
		public VerificationException(string message) : base(message)
		{
			base.HResult = -2146233075;
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x00092A40 File Offset: 0x00090C40
		public VerificationException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233075;
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected VerificationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
