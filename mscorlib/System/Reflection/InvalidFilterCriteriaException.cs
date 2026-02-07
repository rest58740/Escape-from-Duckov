using System;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x020008A7 RID: 2215
	[Serializable]
	public class InvalidFilterCriteriaException : ApplicationException
	{
		// Token: 0x06004904 RID: 18692 RVA: 0x000EE8FC File Offset: 0x000ECAFC
		public InvalidFilterCriteriaException() : this("Specified filter criteria was invalid.")
		{
		}

		// Token: 0x06004905 RID: 18693 RVA: 0x000EE909 File Offset: 0x000ECB09
		public InvalidFilterCriteriaException(string message) : this(message, null)
		{
		}

		// Token: 0x06004906 RID: 18694 RVA: 0x000EE913 File Offset: 0x000ECB13
		public InvalidFilterCriteriaException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146232831;
		}

		// Token: 0x06004907 RID: 18695 RVA: 0x0006E7C9 File Offset: 0x0006C9C9
		protected InvalidFilterCriteriaException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
