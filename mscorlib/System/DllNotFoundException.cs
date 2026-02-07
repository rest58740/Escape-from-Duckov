using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000111 RID: 273
	[Serializable]
	public class DllNotFoundException : TypeLoadException
	{
		// Token: 0x06000A8F RID: 2703 RVA: 0x00028211 File Offset: 0x00026411
		public DllNotFoundException() : base("Dll was not found.")
		{
			base.HResult = -2146233052;
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x00028229 File Offset: 0x00026429
		public DllNotFoundException(string message) : base(message)
		{
			base.HResult = -2146233052;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0002823D File Offset: 0x0002643D
		public DllNotFoundException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233052;
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00028252 File Offset: 0x00026452
		protected DllNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
