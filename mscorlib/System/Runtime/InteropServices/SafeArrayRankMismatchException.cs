using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006DD RID: 1757
	[Serializable]
	public class SafeArrayRankMismatchException : SystemException
	{
		// Token: 0x06004046 RID: 16454 RVA: 0x000E0DA2 File Offset: 0x000DEFA2
		public SafeArrayRankMismatchException() : base("Specified array was not of the expected rank.")
		{
			base.HResult = -2146233032;
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x000E0DBA File Offset: 0x000DEFBA
		public SafeArrayRankMismatchException(string message) : base(message)
		{
			base.HResult = -2146233032;
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x000E0DCE File Offset: 0x000DEFCE
		public SafeArrayRankMismatchException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233032;
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected SafeArrayRankMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
