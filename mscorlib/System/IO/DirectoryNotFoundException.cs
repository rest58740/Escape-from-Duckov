using System;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x02000B02 RID: 2818
	[Serializable]
	public class DirectoryNotFoundException : IOException
	{
		// Token: 0x060064B8 RID: 25784 RVA: 0x001566DF File Offset: 0x001548DF
		public DirectoryNotFoundException() : base("Attempted to access a path that is not on the disk.")
		{
			base.HResult = -2147024893;
		}

		// Token: 0x060064B9 RID: 25785 RVA: 0x001566F7 File Offset: 0x001548F7
		public DirectoryNotFoundException(string message) : base(message)
		{
			base.HResult = -2147024893;
		}

		// Token: 0x060064BA RID: 25786 RVA: 0x0015670B File Offset: 0x0015490B
		public DirectoryNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147024893;
		}

		// Token: 0x060064BB RID: 25787 RVA: 0x00156720 File Offset: 0x00154920
		protected DirectoryNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
