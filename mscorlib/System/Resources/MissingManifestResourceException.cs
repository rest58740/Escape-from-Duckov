using System;
using System.Runtime.Serialization;

namespace System.Resources
{
	// Token: 0x0200085A RID: 2138
	[Serializable]
	public class MissingManifestResourceException : SystemException
	{
		// Token: 0x06004736 RID: 18230 RVA: 0x000E8057 File Offset: 0x000E6257
		public MissingManifestResourceException() : base("Unable to find manifest resource.")
		{
			base.HResult = -2146233038;
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x000E806F File Offset: 0x000E626F
		public MissingManifestResourceException(string message) : base(message)
		{
			base.HResult = -2146233038;
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x000E8083 File Offset: 0x000E6283
		public MissingManifestResourceException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233038;
		}

		// Token: 0x06004739 RID: 18233 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected MissingManifestResourceException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
