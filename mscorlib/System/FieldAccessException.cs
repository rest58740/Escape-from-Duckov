using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000119 RID: 281
	[Serializable]
	public class FieldAccessException : MemberAccessException
	{
		// Token: 0x06000ADD RID: 2781 RVA: 0x0002885E File Offset: 0x00026A5E
		public FieldAccessException() : base("Attempted to access a field that is not accessible by the caller.")
		{
			base.HResult = -2146233081;
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x00028876 File Offset: 0x00026A76
		public FieldAccessException(string message) : base(message)
		{
			base.HResult = -2146233081;
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0002888A File Offset: 0x00026A8A
		public FieldAccessException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233081;
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0002889F File Offset: 0x00026A9F
		protected FieldAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
