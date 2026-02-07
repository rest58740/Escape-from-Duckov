using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200015D RID: 349
	[Serializable]
	public sealed class MulticastNotSupportedException : SystemException
	{
		// Token: 0x06000DCE RID: 3534 RVA: 0x00035E40 File Offset: 0x00034040
		public MulticastNotSupportedException() : base("Attempted to add multiple callbacks to a delegate that does not support multicast.")
		{
			base.HResult = -2146233068;
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x00035E58 File Offset: 0x00034058
		public MulticastNotSupportedException(string message) : base(message)
		{
			base.HResult = -2146233068;
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x00035E6C File Offset: 0x0003406C
		public MulticastNotSupportedException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233068;
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x00020A69 File Offset: 0x0001EC69
		internal MulticastNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
