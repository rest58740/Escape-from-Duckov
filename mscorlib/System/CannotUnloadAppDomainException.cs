using System;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020001C9 RID: 457
	[Serializable]
	public class CannotUnloadAppDomainException : SystemException
	{
		// Token: 0x060013B5 RID: 5045 RVA: 0x0004E7D0 File Offset: 0x0004C9D0
		public CannotUnloadAppDomainException() : base("Attempt to unload the AppDomain failed.")
		{
			base.HResult = -2146234347;
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x0004E7E8 File Offset: 0x0004C9E8
		public CannotUnloadAppDomainException(string message) : base(message)
		{
			base.HResult = -2146234347;
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x0004E7FC File Offset: 0x0004C9FC
		public CannotUnloadAppDomainException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146234347;
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected CannotUnloadAppDomainException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0400144C RID: 5196
		internal const int COR_E_CANNOTUNLOADAPPDOMAIN = -2146234347;
	}
}
