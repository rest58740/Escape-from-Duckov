using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting
{
	// Token: 0x0200056E RID: 1390
	[ComVisible(true)]
	[Serializable]
	public class ServerException : SystemException
	{
		// Token: 0x060036AC RID: 13996 RVA: 0x00092A55 File Offset: 0x00090C55
		public ServerException()
		{
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x0006E6B1 File Offset: 0x0006C8B1
		public ServerException(string message) : base(message)
		{
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x0006E6BA File Offset: 0x0006C8BA
		public ServerException(string message, Exception InnerException) : base(message, InnerException)
		{
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x00020A69 File Offset: 0x0001EC69
		internal ServerException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
