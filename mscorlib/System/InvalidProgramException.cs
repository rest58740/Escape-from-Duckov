using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200014D RID: 333
	[Serializable]
	public sealed class InvalidProgramException : SystemException
	{
		// Token: 0x06000C92 RID: 3218 RVA: 0x00032865 File Offset: 0x00030A65
		public InvalidProgramException() : base("Common Language Runtime detected an invalid program.")
		{
			base.HResult = -2146233030;
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x0003287D File Offset: 0x00030A7D
		public InvalidProgramException(string message) : base(message)
		{
			base.HResult = -2146233030;
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x00032891 File Offset: 0x00030A91
		public InvalidProgramException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233030;
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x00020A69 File Offset: 0x0001EC69
		internal InvalidProgramException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
