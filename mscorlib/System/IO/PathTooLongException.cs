using System;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x02000B0E RID: 2830
	[Serializable]
	public class PathTooLongException : IOException
	{
		// Token: 0x0600652D RID: 25901 RVA: 0x001584D7 File Offset: 0x001566D7
		public PathTooLongException() : base("The specified file name or path is too long, or a component of the specified path is too long.")
		{
			base.HResult = -2147024690;
		}

		// Token: 0x0600652E RID: 25902 RVA: 0x001584EF File Offset: 0x001566EF
		public PathTooLongException(string message) : base(message)
		{
			base.HResult = -2147024690;
		}

		// Token: 0x0600652F RID: 25903 RVA: 0x00158503 File Offset: 0x00156703
		public PathTooLongException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147024690;
		}

		// Token: 0x06006530 RID: 25904 RVA: 0x00156720 File Offset: 0x00154920
		protected PathTooLongException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
