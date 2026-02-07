using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006C7 RID: 1735
	[Serializable]
	public class MarshalDirectiveException : SystemException
	{
		// Token: 0x06003FDC RID: 16348 RVA: 0x000DFE77 File Offset: 0x000DE077
		public MarshalDirectiveException() : base("Marshaling directives are invalid.")
		{
			base.HResult = -2146233035;
		}

		// Token: 0x06003FDD RID: 16349 RVA: 0x000DFE8F File Offset: 0x000DE08F
		public MarshalDirectiveException(string message) : base(message)
		{
			base.HResult = -2146233035;
		}

		// Token: 0x06003FDE RID: 16350 RVA: 0x000DFEA3 File Offset: 0x000DE0A3
		public MarshalDirectiveException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233035;
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected MarshalDirectiveException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
