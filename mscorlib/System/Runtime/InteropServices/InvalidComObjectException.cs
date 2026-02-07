using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006DA RID: 1754
	[Serializable]
	public class InvalidComObjectException : SystemException
	{
		// Token: 0x06004039 RID: 16441 RVA: 0x000E0CE4 File Offset: 0x000DEEE4
		public InvalidComObjectException() : base("Attempt has been made to use a COM object that does not have a backing class factory.")
		{
			base.HResult = -2146233049;
		}

		// Token: 0x0600403A RID: 16442 RVA: 0x000E0CFC File Offset: 0x000DEEFC
		public InvalidComObjectException(string message) : base(message)
		{
			base.HResult = -2146233049;
		}

		// Token: 0x0600403B RID: 16443 RVA: 0x000E0D10 File Offset: 0x000DEF10
		public InvalidComObjectException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233049;
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected InvalidComObjectException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
