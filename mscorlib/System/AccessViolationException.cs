using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000DB RID: 219
	[Serializable]
	public class AccessViolationException : SystemException
	{
		// Token: 0x0600068E RID: 1678 RVA: 0x00020A28 File Offset: 0x0001EC28
		public AccessViolationException() : base("Attempted to read or write protected memory. This is often an indication that other memory is corrupt.")
		{
			base.HResult = -2147467261;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00020A40 File Offset: 0x0001EC40
		public AccessViolationException(string message) : base(message)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00020A54 File Offset: 0x0001EC54
		public AccessViolationException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected AccessViolationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x04001044 RID: 4164
		private IntPtr _ip;

		// Token: 0x04001045 RID: 4165
		private IntPtr _target;

		// Token: 0x04001046 RID: 4166
		private int _accessType;
	}
}
