using System;
using System.Runtime.Serialization;

namespace ICSharpCode.SharpZipLib.BZip2
{
	// Token: 0x0200003F RID: 63
	[Serializable]
	public class BZip2Exception : SharpZipBaseException
	{
		// Token: 0x060002B3 RID: 691 RVA: 0x000128A0 File Offset: 0x00010AA0
		protected BZip2Exception(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x000128AC File Offset: 0x00010AAC
		public BZip2Exception()
		{
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x000128B4 File Offset: 0x00010AB4
		public BZip2Exception(string message) : base(message)
		{
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x000128C0 File Offset: 0x00010AC0
		public BZip2Exception(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
