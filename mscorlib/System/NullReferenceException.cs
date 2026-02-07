using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000162 RID: 354
	[Serializable]
	public class NullReferenceException : SystemException
	{
		// Token: 0x06000DE4 RID: 3556 RVA: 0x00036001 File Offset: 0x00034201
		public NullReferenceException() : base("Object reference not set to an instance of an object.")
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x00020A40 File Offset: 0x0001EC40
		public NullReferenceException(string message) : base(message)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00020A54 File Offset: 0x0001EC54
		public NullReferenceException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected NullReferenceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
