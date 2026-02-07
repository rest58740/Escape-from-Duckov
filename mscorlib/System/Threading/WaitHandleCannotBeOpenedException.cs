using System;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x0200029C RID: 668
	[Serializable]
	public class WaitHandleCannotBeOpenedException : ApplicationException
	{
		// Token: 0x06001DA7 RID: 7591 RVA: 0x0006E788 File Offset: 0x0006C988
		public WaitHandleCannotBeOpenedException() : base("No handle of the given name exists.")
		{
			base.HResult = -2146233044;
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x0006E7A0 File Offset: 0x0006C9A0
		public WaitHandleCannotBeOpenedException(string message) : base(message)
		{
			base.HResult = -2146233044;
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x0006E7B4 File Offset: 0x0006C9B4
		public WaitHandleCannotBeOpenedException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233044;
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x0006E7C9 File Offset: 0x0006C9C9
		protected WaitHandleCannotBeOpenedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
