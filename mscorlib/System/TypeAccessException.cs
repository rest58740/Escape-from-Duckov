using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020001A2 RID: 418
	[Serializable]
	public class TypeAccessException : TypeLoadException
	{
		// Token: 0x060011E5 RID: 4581 RVA: 0x00047C43 File Offset: 0x00045E43
		public TypeAccessException() : base("Attempt to access the type failed.")
		{
			base.HResult = -2146233021;
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x00047C5B File Offset: 0x00045E5B
		public TypeAccessException(string message) : base(message)
		{
			base.HResult = -2146233021;
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x00047C6F File Offset: 0x00045E6F
		public TypeAccessException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233021;
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x00028252 File Offset: 0x00026452
		protected TypeAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
