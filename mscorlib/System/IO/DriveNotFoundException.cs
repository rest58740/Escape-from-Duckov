using System;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x02000B2C RID: 2860
	[Serializable]
	public class DriveNotFoundException : IOException
	{
		// Token: 0x060066DB RID: 26331 RVA: 0x0015F44C File Offset: 0x0015D64C
		public DriveNotFoundException() : base("Could not find the drive. The drive might not be ready or might not be mapped.")
		{
			base.HResult = -2147024893;
		}

		// Token: 0x060066DC RID: 26332 RVA: 0x001566F7 File Offset: 0x001548F7
		public DriveNotFoundException(string message) : base(message)
		{
			base.HResult = -2147024893;
		}

		// Token: 0x060066DD RID: 26333 RVA: 0x0015670B File Offset: 0x0015490B
		public DriveNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147024893;
		}

		// Token: 0x060066DE RID: 26334 RVA: 0x00156720 File Offset: 0x00154920
		protected DriveNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
