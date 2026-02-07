using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Runtime.Remoting
{
	// Token: 0x0200056D RID: 1389
	[ComVisible(true)]
	[Serializable]
	public class RemotingTimeoutException : RemotingException
	{
		// Token: 0x060036A8 RID: 13992 RVA: 0x000C5969 File Offset: 0x000C3B69
		public RemotingTimeoutException()
		{
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x000C5971 File Offset: 0x000C3B71
		public RemotingTimeoutException(string message) : base(message)
		{
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x000C597A File Offset: 0x000C3B7A
		public RemotingTimeoutException(string message, Exception InnerException) : base(message, InnerException)
		{
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x000C5984 File Offset: 0x000C3B84
		internal RemotingTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
