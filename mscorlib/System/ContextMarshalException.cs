using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020001EE RID: 494
	[ComVisible(true)]
	[Serializable]
	public class ContextMarshalException : SystemException
	{
		// Token: 0x0600154E RID: 5454 RVA: 0x0005395D File Offset: 0x00051B5D
		public ContextMarshalException() : base(Environment.GetResourceString("Attempted to marshal an object across a context boundary."))
		{
			base.SetErrorCode(-2146233084);
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x0005397A File Offset: 0x00051B7A
		public ContextMarshalException(string message) : base(message)
		{
			base.SetErrorCode(-2146233084);
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x0005398E File Offset: 0x00051B8E
		public ContextMarshalException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233084);
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected ContextMarshalException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
