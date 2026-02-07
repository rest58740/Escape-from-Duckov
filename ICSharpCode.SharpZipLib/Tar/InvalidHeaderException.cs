using System;
using System.Runtime.Serialization;

namespace ICSharpCode.SharpZipLib.Tar
{
	// Token: 0x02000040 RID: 64
	[Serializable]
	public class InvalidHeaderException : TarException
	{
		// Token: 0x060002B7 RID: 695 RVA: 0x000128CC File Offset: 0x00010ACC
		protected InvalidHeaderException(SerializationInfo information, StreamingContext context) : base(information, context)
		{
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x000128D8 File Offset: 0x00010AD8
		public InvalidHeaderException()
		{
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x000128E0 File Offset: 0x00010AE0
		public InvalidHeaderException(string message) : base(message)
		{
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000128EC File Offset: 0x00010AEC
		public InvalidHeaderException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
