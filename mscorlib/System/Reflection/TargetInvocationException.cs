using System;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x020008CB RID: 2251
	[Serializable]
	public sealed class TargetInvocationException : ApplicationException
	{
		// Token: 0x06004AE9 RID: 19177 RVA: 0x000EFD98 File Offset: 0x000EDF98
		public TargetInvocationException(Exception inner) : base("Exception has been thrown by the target of an invocation.", inner)
		{
			base.HResult = -2146232828;
		}

		// Token: 0x06004AEA RID: 19178 RVA: 0x000EFDB1 File Offset: 0x000EDFB1
		public TargetInvocationException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146232828;
		}

		// Token: 0x06004AEB RID: 19179 RVA: 0x0006E7C9 File Offset: 0x0006C9C9
		internal TargetInvocationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
