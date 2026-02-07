using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200010B RID: 267
	[Serializable]
	public sealed class DataMisalignedException : SystemException
	{
		// Token: 0x060009A3 RID: 2467 RVA: 0x000258FE File Offset: 0x00023AFE
		public DataMisalignedException() : base("A datatype misalignment was detected in a load or store instruction.")
		{
			base.HResult = -2146233023;
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00025916 File Offset: 0x00023B16
		public DataMisalignedException(string message) : base(message)
		{
			base.HResult = -2146233023;
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0002592A File Offset: 0x00023B2A
		public DataMisalignedException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233023;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x00020A69 File Offset: 0x0001EC69
		internal DataMisalignedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
