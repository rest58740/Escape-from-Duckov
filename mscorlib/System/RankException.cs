using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000174 RID: 372
	[Serializable]
	public class RankException : SystemException
	{
		// Token: 0x06000EAE RID: 3758 RVA: 0x0003C135 File Offset: 0x0003A335
		public RankException() : base("Attempted to operate on an array with the incorrect number of dimensions.")
		{
			base.HResult = -2146233065;
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0003C14D File Offset: 0x0003A34D
		public RankException(string message) : base(message)
		{
			base.HResult = -2146233065;
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0003C161 File Offset: 0x0003A361
		public RankException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233065;
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected RankException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
