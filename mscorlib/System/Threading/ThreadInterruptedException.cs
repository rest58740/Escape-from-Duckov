using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x020002D8 RID: 728
	[ComVisible(true)]
	[Serializable]
	public class ThreadInterruptedException : SystemException
	{
		// Token: 0x06001FF8 RID: 8184 RVA: 0x000749AB File Offset: 0x00072BAB
		public ThreadInterruptedException() : base(Exception.GetMessageFromNativeResources(Exception.ExceptionMessageKind.ThreadInterrupted))
		{
			base.SetErrorCode(-2146233063);
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x000749C4 File Offset: 0x00072BC4
		public ThreadInterruptedException(string message) : base(message)
		{
			base.SetErrorCode(-2146233063);
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x000749D8 File Offset: 0x00072BD8
		public ThreadInterruptedException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2146233063);
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected ThreadInterruptedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
