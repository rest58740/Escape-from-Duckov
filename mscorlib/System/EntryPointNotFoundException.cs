using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000114 RID: 276
	[Serializable]
	public class EntryPointNotFoundException : TypeLoadException
	{
		// Token: 0x06000ACB RID: 2763 RVA: 0x000287D0 File Offset: 0x000269D0
		public EntryPointNotFoundException() : base("Entry point was not found.")
		{
			base.HResult = -2146233053;
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x000287E8 File Offset: 0x000269E8
		public EntryPointNotFoundException(string message) : base(message)
		{
			base.HResult = -2146233053;
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x000287FC File Offset: 0x000269FC
		public EntryPointNotFoundException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233053;
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x00028252 File Offset: 0x00026452
		protected EntryPointNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
