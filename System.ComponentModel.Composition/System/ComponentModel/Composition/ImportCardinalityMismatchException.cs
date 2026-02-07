using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security;

namespace System.ComponentModel.Composition
{
	// Token: 0x02000045 RID: 69
	[DebuggerTypeProxy(typeof(ImportCardinalityMismatchExceptionDebuggerProxy))]
	[DebuggerDisplay("{Message}")]
	[Serializable]
	public class ImportCardinalityMismatchException : Exception
	{
		// Token: 0x060001F4 RID: 500 RVA: 0x00005E2C File Offset: 0x0000402C
		public ImportCardinalityMismatchException() : this(null, null)
		{
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00005E36 File Offset: 0x00004036
		public ImportCardinalityMismatchException(string message) : this(message, null)
		{
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x000043D5 File Offset: 0x000025D5
		public ImportCardinalityMismatchException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000020DC File Offset: 0x000002DC
		[SecuritySafeCritical]
		protected ImportCardinalityMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
