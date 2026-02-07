using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020001C7 RID: 455
	[Serializable]
	public class AppDomainUnloadedException : SystemException
	{
		// Token: 0x060013A5 RID: 5029 RVA: 0x0004E4A9 File Offset: 0x0004C6A9
		public AppDomainUnloadedException() : base("Attempted to access an unloaded AppDomain.")
		{
			base.HResult = -2146234348;
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x0004E4C1 File Offset: 0x0004C6C1
		public AppDomainUnloadedException(string message) : base(message)
		{
			base.HResult = -2146234348;
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x0004E4D5 File Offset: 0x0004C6D5
		public AppDomainUnloadedException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146234348;
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected AppDomainUnloadedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x04001446 RID: 5190
		internal const int COR_E_APPDOMAINUNLOADED = -2146234348;
	}
}
