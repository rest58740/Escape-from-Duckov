using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200011B RID: 283
	[Serializable]
	public class FormatException : SystemException
	{
		// Token: 0x06000AE2 RID: 2786 RVA: 0x000288A9 File Offset: 0x00026AA9
		public FormatException() : base("One of the identified items was in an invalid format.")
		{
			base.HResult = -2146233033;
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x000288C1 File Offset: 0x00026AC1
		public FormatException(string message) : base(message)
		{
			base.HResult = -2146233033;
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x000288D5 File Offset: 0x00026AD5
		public FormatException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233033;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected FormatException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
