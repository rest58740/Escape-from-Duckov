using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Security.Policy
{
	// Token: 0x0200041B RID: 1051
	[ComVisible(true)]
	[Serializable]
	public class PolicyException : SystemException
	{
		// Token: 0x06002AE4 RID: 10980 RVA: 0x0009AEF0 File Offset: 0x000990F0
		public PolicyException() : base(Locale.GetText("Cannot run because of policy."))
		{
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x0006E6B1 File Offset: 0x0006C8B1
		public PolicyException(string message) : base(message)
		{
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected PolicyException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06002AE7 RID: 10983 RVA: 0x0006E6BA File Offset: 0x0006C8BA
		public PolicyException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
