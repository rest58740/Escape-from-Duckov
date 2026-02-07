using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000113 RID: 275
	[Serializable]
	public class DuplicateWaitObjectException : ArgumentException
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x00028757 File Offset: 0x00026957
		private static string DuplicateWaitObjectMessage
		{
			get
			{
				if (DuplicateWaitObjectException.s_duplicateWaitObjectMessage == null)
				{
					DuplicateWaitObjectException.s_duplicateWaitObjectMessage = "Duplicate objects in argument.";
				}
				return DuplicateWaitObjectException.s_duplicateWaitObjectMessage;
			}
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00028775 File Offset: 0x00026975
		public DuplicateWaitObjectException() : base(DuplicateWaitObjectException.DuplicateWaitObjectMessage)
		{
			base.HResult = -2146233047;
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x0002878D File Offset: 0x0002698D
		public DuplicateWaitObjectException(string parameterName) : base(DuplicateWaitObjectException.DuplicateWaitObjectMessage, parameterName)
		{
			base.HResult = -2146233047;
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x000287A6 File Offset: 0x000269A6
		public DuplicateWaitObjectException(string parameterName, string message) : base(message, parameterName)
		{
			base.HResult = -2146233047;
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x000287BB File Offset: 0x000269BB
		public DuplicateWaitObjectException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233047;
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00021113 File Offset: 0x0001F313
		protected DuplicateWaitObjectException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x040010E1 RID: 4321
		private static volatile string s_duplicateWaitObjectMessage;
	}
}
