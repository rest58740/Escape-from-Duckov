using System;
using System.Runtime.Serialization;

namespace System.Collections.Generic
{
	// Token: 0x02000AA0 RID: 2720
	[Serializable]
	public class KeyNotFoundException : SystemException
	{
		// Token: 0x06006137 RID: 24887 RVA: 0x001451DC File Offset: 0x001433DC
		public KeyNotFoundException() : base("The given key was not present in the dictionary.")
		{
			base.HResult = -2146232969;
		}

		// Token: 0x06006138 RID: 24888 RVA: 0x001451F4 File Offset: 0x001433F4
		public KeyNotFoundException(string message) : base(message)
		{
			base.HResult = -2146232969;
		}

		// Token: 0x06006139 RID: 24889 RVA: 0x00145208 File Offset: 0x00143408
		public KeyNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146232969;
		}

		// Token: 0x0600613A RID: 24890 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected KeyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
