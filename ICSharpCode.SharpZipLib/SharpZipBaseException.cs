using System;
using System.Runtime.Serialization;

namespace ICSharpCode.SharpZipLib
{
	// Token: 0x02000059 RID: 89
	[Serializable]
	public class SharpZipBaseException : ApplicationException
	{
		// Token: 0x060003E3 RID: 995 RVA: 0x00016DEC File Offset: 0x00014FEC
		protected SharpZipBaseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00016DF8 File Offset: 0x00014FF8
		public SharpZipBaseException()
		{
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00016E00 File Offset: 0x00015000
		public SharpZipBaseException(string message) : base(message)
		{
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00016E0C File Offset: 0x0001500C
		public SharpZipBaseException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
