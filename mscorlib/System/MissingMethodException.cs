using System;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x0200015C RID: 348
	[Serializable]
	public class MissingMethodException : MissingMemberException
	{
		// Token: 0x06000DC8 RID: 3528 RVA: 0x00035D7E File Offset: 0x00033F7E
		public MissingMethodException() : base("Attempted to access a missing method.")
		{
			base.HResult = -2146233069;
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x00035D96 File Offset: 0x00033F96
		public MissingMethodException(string message) : base(message)
		{
			base.HResult = -2146233069;
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x00035DAA File Offset: 0x00033FAA
		public MissingMethodException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233069;
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x00035DBF File Offset: 0x00033FBF
		public MissingMethodException(string className, string methodName)
		{
			this.ClassName = className;
			this.MemberName = methodName;
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x00035DD5 File Offset: 0x00033FD5
		protected MissingMethodException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000DCD RID: 3533 RVA: 0x00035DE0 File Offset: 0x00033FE0
		public override string Message
		{
			[SecuritySafeCritical]
			get
			{
				if (this.ClassName != null)
				{
					return SR.Format("Method '{0}' not found.", this.ClassName + "." + this.MemberName + ((this.Signature != null) ? (" " + MissingMemberException.FormatSignature(this.Signature)) : string.Empty));
				}
				return base.Message;
			}
		}
	}
}
