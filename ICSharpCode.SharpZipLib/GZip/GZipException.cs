using System;
using System.Runtime.Serialization;

namespace ICSharpCode.SharpZipLib.GZip
{
	// Token: 0x02000058 RID: 88
	[Serializable]
	public class GZipException : SharpZipBaseException
	{
		// Token: 0x060003DF RID: 991 RVA: 0x00016DC0 File Offset: 0x00014FC0
		protected GZipException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00016DCC File Offset: 0x00014FCC
		public GZipException()
		{
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00016DD4 File Offset: 0x00014FD4
		public GZipException(string message) : base(message)
		{
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00016DE0 File Offset: 0x00014FE0
		public GZipException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
