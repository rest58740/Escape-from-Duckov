using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020001D0 RID: 464
	[Serializable]
	public class OutOfMemoryException : SystemException
	{
		// Token: 0x060013D7 RID: 5079 RVA: 0x0004EC4C File Offset: 0x0004CE4C
		public OutOfMemoryException() : base("Insufficient memory to continue the execution of the program.")
		{
			base.HResult = -2147024882;
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x0004EC64 File Offset: 0x0004CE64
		public OutOfMemoryException(string message) : base(message)
		{
			base.HResult = -2147024882;
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x0004EC78 File Offset: 0x0004CE78
		public OutOfMemoryException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147024882;
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected OutOfMemoryException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
