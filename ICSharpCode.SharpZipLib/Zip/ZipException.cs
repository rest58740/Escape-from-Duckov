using System;
using System.Runtime.Serialization;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x0200001D RID: 29
	[Serializable]
	public class ZipException : SharpZipBaseException
	{
		// Token: 0x06000128 RID: 296 RVA: 0x00009A40 File Offset: 0x00007C40
		protected ZipException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00009A4C File Offset: 0x00007C4C
		public ZipException()
		{
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00009A54 File Offset: 0x00007C54
		public ZipException(string message) : base(message)
		{
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00009A60 File Offset: 0x00007C60
		public ZipException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
