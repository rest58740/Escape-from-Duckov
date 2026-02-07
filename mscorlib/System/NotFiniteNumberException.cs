using System;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x0200015F RID: 351
	[Serializable]
	public class NotFiniteNumberException : ArithmeticException
	{
		// Token: 0x06000DD3 RID: 3539 RVA: 0x00035E81 File Offset: 0x00034081
		public NotFiniteNumberException() : base("Arg_NotFiniteNumberException = Number encountered was not a finite quantity.")
		{
			this._offendingNumber = 0.0;
			base.HResult = -2146233048;
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00035EA8 File Offset: 0x000340A8
		public NotFiniteNumberException(double offendingNumber)
		{
			this._offendingNumber = offendingNumber;
			base.HResult = -2146233048;
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00035EC2 File Offset: 0x000340C2
		public NotFiniteNumberException(string message) : base(message)
		{
			this._offendingNumber = 0.0;
			base.HResult = -2146233048;
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00035EE5 File Offset: 0x000340E5
		public NotFiniteNumberException(string message, double offendingNumber) : base(message)
		{
			this._offendingNumber = offendingNumber;
			base.HResult = -2146233048;
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00035F00 File Offset: 0x00034100
		public NotFiniteNumberException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233048;
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x00035F15 File Offset: 0x00034115
		public NotFiniteNumberException(string message, double offendingNumber, Exception innerException) : base(message, innerException)
		{
			this._offendingNumber = offendingNumber;
			base.HResult = -2146233048;
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00035F31 File Offset: 0x00034131
		protected NotFiniteNumberException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._offendingNumber = (double)info.GetInt32("OffendingNumber");
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x00035F4D File Offset: 0x0003414D
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("OffendingNumber", this._offendingNumber, typeof(int));
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x00035F77 File Offset: 0x00034177
		public double OffendingNumber
		{
			get
			{
				return this._offendingNumber;
			}
		}

		// Token: 0x04001284 RID: 4740
		private double _offendingNumber;
	}
}
