using System;
using System.Runtime.Serialization;
using System.Security;

namespace System.ComponentModel.Composition
{
	// Token: 0x02000024 RID: 36
	[Serializable]
	public class CompositionContractMismatchException : Exception
	{
		// Token: 0x06000131 RID: 305 RVA: 0x000043C1 File Offset: 0x000025C1
		public CompositionContractMismatchException() : this(null, null)
		{
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000043CB File Offset: 0x000025CB
		public CompositionContractMismatchException(string message) : this(message, null)
		{
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000043D5 File Offset: 0x000025D5
		public CompositionContractMismatchException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000020DC File Offset: 0x000002DC
		[SecuritySafeCritical]
		protected CompositionContractMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
