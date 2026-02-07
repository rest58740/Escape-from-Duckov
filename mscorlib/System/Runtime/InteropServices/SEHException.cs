using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006DC RID: 1756
	[Serializable]
	public class SEHException : ExternalException
	{
		// Token: 0x06004041 RID: 16449 RVA: 0x000E0D66 File Offset: 0x000DEF66
		public SEHException()
		{
			base.HResult = -2147467259;
		}

		// Token: 0x06004042 RID: 16450 RVA: 0x000E0D79 File Offset: 0x000DEF79
		public SEHException(string message) : base(message)
		{
			base.HResult = -2147467259;
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x000E0D8D File Offset: 0x000DEF8D
		public SEHException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2147467259;
		}

		// Token: 0x06004044 RID: 16452 RVA: 0x000E0B7B File Offset: 0x000DED7B
		protected SEHException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06004045 RID: 16453 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool CanResume()
		{
			return false;
		}
	}
}
