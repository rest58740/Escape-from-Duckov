using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000156 RID: 342
	[Serializable]
	public class MemberAccessException : SystemException
	{
		// Token: 0x06000D49 RID: 3401 RVA: 0x00033817 File Offset: 0x00031A17
		public MemberAccessException() : base("Cannot access member.")
		{
			base.HResult = -2146233062;
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x0003382F File Offset: 0x00031A2F
		public MemberAccessException(string message) : base(message)
		{
			base.HResult = -2146233062;
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00033843 File Offset: 0x00031A43
		public MemberAccessException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233062;
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected MemberAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
