using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000186 RID: 390
	[Serializable]
	public sealed class StackOverflowException : SystemException
	{
		// Token: 0x06000F9A RID: 3994 RVA: 0x000414BE File Offset: 0x0003F6BE
		public StackOverflowException() : base("Operation caused a stack overflow.")
		{
			base.HResult = -2147023895;
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x000414D6 File Offset: 0x0003F6D6
		public StackOverflowException(string message) : base(message)
		{
			base.HResult = -2147023895;
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x000414EA File Offset: 0x0003F6EA
		public StackOverflowException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147023895;
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00020A69 File Offset: 0x0001EC69
		internal StackOverflowException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
