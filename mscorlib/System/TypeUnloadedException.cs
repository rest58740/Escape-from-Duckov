using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020001A5 RID: 421
	[Serializable]
	public class TypeUnloadedException : SystemException
	{
		// Token: 0x060011F0 RID: 4592 RVA: 0x00047D37 File Offset: 0x00045F37
		public TypeUnloadedException() : base("Type had been unloaded.")
		{
			base.HResult = -2146234349;
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x00047D4F File Offset: 0x00045F4F
		public TypeUnloadedException(string message) : base(message)
		{
			base.HResult = -2146234349;
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x00047D63 File Offset: 0x00045F63
		public TypeUnloadedException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146234349;
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected TypeUnloadedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
