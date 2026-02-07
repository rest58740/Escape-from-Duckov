using System;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x020008CA RID: 2250
	[Serializable]
	public class TargetException : ApplicationException
	{
		// Token: 0x06004AE5 RID: 19173 RVA: 0x000EFD70 File Offset: 0x000EDF70
		public TargetException() : this(null)
		{
		}

		// Token: 0x06004AE6 RID: 19174 RVA: 0x000EFD79 File Offset: 0x000EDF79
		public TargetException(string message) : this(message, null)
		{
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x000EFD83 File Offset: 0x000EDF83
		public TargetException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146232829;
		}

		// Token: 0x06004AE8 RID: 19176 RVA: 0x0006E7C9 File Offset: 0x0006C9C9
		protected TargetException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
