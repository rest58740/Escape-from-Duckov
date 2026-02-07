using System;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x0200087C RID: 2172
	[Serializable]
	public sealed class AmbiguousMatchException : SystemException
	{
		// Token: 0x0600484A RID: 18506 RVA: 0x000EDFA4 File Offset: 0x000EC1A4
		public AmbiguousMatchException() : base("Ambiguous match found.")
		{
			base.HResult = -2147475171;
		}

		// Token: 0x0600484B RID: 18507 RVA: 0x000EDFBC File Offset: 0x000EC1BC
		public AmbiguousMatchException(string message) : base(message)
		{
			base.HResult = -2147475171;
		}

		// Token: 0x0600484C RID: 18508 RVA: 0x000EDFD0 File Offset: 0x000EC1D0
		public AmbiguousMatchException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2147475171;
		}

		// Token: 0x0600484D RID: 18509 RVA: 0x00020A69 File Offset: 0x0001EC69
		internal AmbiguousMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
