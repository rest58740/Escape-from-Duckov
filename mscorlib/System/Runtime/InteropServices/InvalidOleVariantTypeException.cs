using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006DB RID: 1755
	[Serializable]
	public class InvalidOleVariantTypeException : SystemException
	{
		// Token: 0x0600403D RID: 16445 RVA: 0x000E0D25 File Offset: 0x000DEF25
		public InvalidOleVariantTypeException() : base("Specified OLE variant was invalid.")
		{
			base.HResult = -2146233039;
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x000E0D3D File Offset: 0x000DEF3D
		public InvalidOleVariantTypeException(string message) : base(message)
		{
			base.HResult = -2146233039;
		}

		// Token: 0x0600403F RID: 16447 RVA: 0x000E0D51 File Offset: 0x000DEF51
		public InvalidOleVariantTypeException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233039;
		}

		// Token: 0x06004040 RID: 16448 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected InvalidOleVariantTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
