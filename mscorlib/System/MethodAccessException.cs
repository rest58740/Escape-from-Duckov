using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200015A RID: 346
	[Serializable]
	public class MethodAccessException : MemberAccessException
	{
		// Token: 0x06000DC4 RID: 3524 RVA: 0x00035D3D File Offset: 0x00033F3D
		public MethodAccessException() : base("Attempt to access the method failed.")
		{
			base.HResult = -2146233072;
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00035D55 File Offset: 0x00033F55
		public MethodAccessException(string message) : base(message)
		{
			base.HResult = -2146233072;
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00035D69 File Offset: 0x00033F69
		public MethodAccessException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233072;
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0002889F File Offset: 0x00026A9F
		protected MethodAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
