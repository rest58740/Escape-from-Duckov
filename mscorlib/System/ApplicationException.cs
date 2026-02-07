using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000F2 RID: 242
	[Serializable]
	public class ApplicationException : Exception
	{
		// Token: 0x060006FA RID: 1786 RVA: 0x00020F6A File Offset: 0x0001F16A
		public ApplicationException() : base("Error in the application.")
		{
			base.HResult = -2146232832;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00020F82 File Offset: 0x0001F182
		public ApplicationException(string message) : base(message)
		{
			base.HResult = -2146232832;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00020F96 File Offset: 0x0001F196
		public ApplicationException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146232832;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00020FAB File Offset: 0x0001F1AB
		protected ApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
