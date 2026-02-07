using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000118 RID: 280
	[Obsolete("This type previously indicated an unspecified fatal error in the runtime. The runtime no longer raises this exception so this type is obsolete.")]
	[Serializable]
	public sealed class ExecutionEngineException : SystemException
	{
		// Token: 0x06000AD9 RID: 2777 RVA: 0x0002881D File Offset: 0x00026A1D
		public ExecutionEngineException() : base("Internal error in the runtime.")
		{
			base.HResult = -2146233082;
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x00028835 File Offset: 0x00026A35
		public ExecutionEngineException(string message) : base(message)
		{
			base.HResult = -2146233082;
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x00028849 File Offset: 0x00026A49
		public ExecutionEngineException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233082;
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x00020A69 File Offset: 0x0001EC69
		internal ExecutionEngineException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
