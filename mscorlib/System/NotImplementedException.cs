using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000160 RID: 352
	[Serializable]
	public class NotImplementedException : SystemException
	{
		// Token: 0x06000DDC RID: 3548 RVA: 0x00035F7F File Offset: 0x0003417F
		public NotImplementedException() : base("The method or operation is not implemented.")
		{
			base.HResult = -2147467263;
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x00035F97 File Offset: 0x00034197
		public NotImplementedException(string message) : base(message)
		{
			base.HResult = -2147467263;
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x00035FAB File Offset: 0x000341AB
		public NotImplementedException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2147467263;
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected NotImplementedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
