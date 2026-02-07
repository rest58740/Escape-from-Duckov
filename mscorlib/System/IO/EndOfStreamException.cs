using System;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x02000B03 RID: 2819
	[Serializable]
	public class EndOfStreamException : IOException
	{
		// Token: 0x060064BC RID: 25788 RVA: 0x0015672A File Offset: 0x0015492A
		public EndOfStreamException() : base("Attempted to read past the end of the stream.")
		{
			base.HResult = -2147024858;
		}

		// Token: 0x060064BD RID: 25789 RVA: 0x00156742 File Offset: 0x00154942
		public EndOfStreamException(string message) : base(message)
		{
			base.HResult = -2147024858;
		}

		// Token: 0x060064BE RID: 25790 RVA: 0x00156756 File Offset: 0x00154956
		public EndOfStreamException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147024858;
		}

		// Token: 0x060064BF RID: 25791 RVA: 0x00156720 File Offset: 0x00154920
		protected EndOfStreamException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
