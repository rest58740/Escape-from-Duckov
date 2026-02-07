using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System.Threading
{
	// Token: 0x020002D7 RID: 727
	[ComVisible(true)]
	[Serializable]
	public sealed class ThreadAbortException : SystemException
	{
		// Token: 0x06001FF5 RID: 8181 RVA: 0x00074986 File Offset: 0x00072B86
		private ThreadAbortException() : base(Exception.GetMessageFromNativeResources(Exception.ExceptionMessageKind.ThreadAbort))
		{
			base.SetErrorCode(-2146233040);
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x00020A69 File Offset: 0x0001EC69
		internal ThreadAbortException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06001FF7 RID: 8183 RVA: 0x0007499F File Offset: 0x00072B9F
		public object ExceptionState
		{
			[SecuritySafeCritical]
			get
			{
				return Thread.CurrentThread.AbortReason;
			}
		}
	}
}
