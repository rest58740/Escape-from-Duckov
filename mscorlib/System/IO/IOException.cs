using System;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x02000B0B RID: 2827
	[Serializable]
	public class IOException : SystemException
	{
		// Token: 0x060064DE RID: 25822 RVA: 0x00156BD6 File Offset: 0x00154DD6
		public IOException() : base("I/O error occurred.")
		{
			base.HResult = -2146232800;
		}

		// Token: 0x060064DF RID: 25823 RVA: 0x00156BEE File Offset: 0x00154DEE
		public IOException(string message) : base(message)
		{
			base.HResult = -2146232800;
		}

		// Token: 0x060064E0 RID: 25824 RVA: 0x00032814 File Offset: 0x00030A14
		public IOException(string message, int hresult) : base(message)
		{
			base.HResult = hresult;
		}

		// Token: 0x060064E1 RID: 25825 RVA: 0x00156C02 File Offset: 0x00154E02
		public IOException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146232800;
		}

		// Token: 0x060064E2 RID: 25826 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected IOException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
