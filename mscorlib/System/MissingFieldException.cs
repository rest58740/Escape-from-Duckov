using System;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x020001CE RID: 462
	[Serializable]
	public class MissingFieldException : MissingMemberException, ISerializable
	{
		// Token: 0x060013C9 RID: 5065 RVA: 0x0004EA32 File Offset: 0x0004CC32
		public MissingFieldException() : base("Attempted to access a non-existing field.")
		{
			base.HResult = -2146233071;
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x0004EA4A File Offset: 0x0004CC4A
		public MissingFieldException(string message) : base(message)
		{
			base.HResult = -2146233071;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0004EA5E File Offset: 0x0004CC5E
		public MissingFieldException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233071;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00035DBF File Offset: 0x00033FBF
		public MissingFieldException(string className, string fieldName)
		{
			this.ClassName = className;
			this.MemberName = fieldName;
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00035DD5 File Offset: 0x00033FD5
		protected MissingFieldException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060013CE RID: 5070 RVA: 0x0004EA74 File Offset: 0x0004CC74
		public override string Message
		{
			[SecuritySafeCritical]
			get
			{
				if (this.ClassName == null)
				{
					return base.Message;
				}
				return SR.Format("Field '{0}' not found.", ((this.Signature != null) ? (MissingMemberException.FormatSignature(this.Signature) + " ") : "") + this.ClassName + "." + this.MemberName);
			}
		}
	}
}
