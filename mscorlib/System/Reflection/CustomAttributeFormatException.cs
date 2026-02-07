using System;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x02000896 RID: 2198
	[Serializable]
	public class CustomAttributeFormatException : FormatException
	{
		// Token: 0x06004894 RID: 18580 RVA: 0x000EE211 File Offset: 0x000EC411
		public CustomAttributeFormatException() : this("Binary format of the specified custom attribute was invalid.")
		{
		}

		// Token: 0x06004895 RID: 18581 RVA: 0x000EE21E File Offset: 0x000EC41E
		public CustomAttributeFormatException(string message) : this(message, null)
		{
		}

		// Token: 0x06004896 RID: 18582 RVA: 0x000EE228 File Offset: 0x000EC428
		public CustomAttributeFormatException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146232827;
		}

		// Token: 0x06004897 RID: 18583 RVA: 0x000EE23D File Offset: 0x000EC43D
		protected CustomAttributeFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
