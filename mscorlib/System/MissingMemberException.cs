using System;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x020001CF RID: 463
	[Serializable]
	public class MissingMemberException : MemberAccessException
	{
		// Token: 0x060013CF RID: 5071 RVA: 0x0004EAD4 File Offset: 0x0004CCD4
		public MissingMemberException() : base("Attempted to access a missing member.")
		{
			base.HResult = -2146233070;
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x0004EAEC File Offset: 0x0004CCEC
		public MissingMemberException(string message) : base(message)
		{
			base.HResult = -2146233070;
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x0004EB00 File Offset: 0x0004CD00
		public MissingMemberException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233070;
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x0004EB15 File Offset: 0x0004CD15
		public MissingMemberException(string className, string memberName)
		{
			this.ClassName = className;
			this.MemberName = memberName;
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0004EB2C File Offset: 0x0004CD2C
		protected MissingMemberException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.ClassName = info.GetString("MMClassName");
			this.MemberName = info.GetString("MMMemberName");
			this.Signature = (byte[])info.GetValue("MMSignature", typeof(byte[]));
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0004EB84 File Offset: 0x0004CD84
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("MMClassName", this.ClassName, typeof(string));
			info.AddValue("MMMemberName", this.MemberName, typeof(string));
			info.AddValue("MMSignature", this.Signature, typeof(byte[]));
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060013D5 RID: 5077 RVA: 0x0004EBEC File Offset: 0x0004CDEC
		public override string Message
		{
			[SecuritySafeCritical]
			get
			{
				if (this.ClassName == null)
				{
					return base.Message;
				}
				return SR.Format("Member '{0}' not found.", this.ClassName + "." + this.MemberName + ((this.Signature != null) ? (" " + MissingMemberException.FormatSignature(this.Signature)) : string.Empty));
			}
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x000258DF File Offset: 0x00023ADF
		internal static string FormatSignature(byte[] signature)
		{
			return string.Empty;
		}

		// Token: 0x04001459 RID: 5209
		protected string ClassName;

		// Token: 0x0400145A RID: 5210
		protected string MemberName;

		// Token: 0x0400145B RID: 5211
		protected byte[] Signature;
	}
}
