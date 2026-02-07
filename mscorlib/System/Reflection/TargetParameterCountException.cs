using System;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x020008CC RID: 2252
	[Serializable]
	public sealed class TargetParameterCountException : ApplicationException
	{
		// Token: 0x06004AEC RID: 19180 RVA: 0x000EFDC6 File Offset: 0x000EDFC6
		public TargetParameterCountException() : base("Number of parameters specified does not match the expected number.")
		{
			base.HResult = -2147352562;
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x000EFDDE File Offset: 0x000EDFDE
		public TargetParameterCountException(string message) : base(message)
		{
			base.HResult = -2147352562;
		}

		// Token: 0x06004AEE RID: 19182 RVA: 0x000EFDF2 File Offset: 0x000EDFF2
		public TargetParameterCountException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2147352562;
		}

		// Token: 0x06004AEF RID: 19183 RVA: 0x0006E7C9 File Offset: 0x0006C9C9
		internal TargetParameterCountException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
