using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting
{
	// Token: 0x0200056A RID: 1386
	[ComVisible(true)]
	[Serializable]
	public class RemotingException : SystemException
	{
		// Token: 0x06003666 RID: 13926 RVA: 0x00092A55 File Offset: 0x00090C55
		public RemotingException()
		{
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x0006E6B1 File Offset: 0x0006C8B1
		public RemotingException(string message) : base(message)
		{
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected RemotingException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x0006E6BA File Offset: 0x0006C8BA
		public RemotingException(string message, Exception InnerException) : base(message, InnerException)
		{
		}
	}
}
