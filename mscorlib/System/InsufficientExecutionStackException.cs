using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000147 RID: 327
	[Serializable]
	public sealed class InsufficientExecutionStackException : SystemException
	{
		// Token: 0x06000C1A RID: 3098 RVA: 0x00031F52 File Offset: 0x00030152
		public InsufficientExecutionStackException() : base("Insufficient stack to continue executing the program safely. This can happen from having too many functions on the call stack or function on the stack using too much stack space.")
		{
			base.HResult = -2146232968;
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00031F6A File Offset: 0x0003016A
		public InsufficientExecutionStackException(string message) : base(message)
		{
			base.HResult = -2146232968;
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00031F7E File Offset: 0x0003017E
		public InsufficientExecutionStackException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146232968;
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x00020A69 File Offset: 0x0001EC69
		internal InsufficientExecutionStackException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
