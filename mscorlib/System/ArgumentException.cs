using System;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x020000F3 RID: 243
	[Serializable]
	public class ArgumentException : SystemException
	{
		// Token: 0x060006FE RID: 1790 RVA: 0x00020FB5 File Offset: 0x0001F1B5
		public ArgumentException() : base("Value does not fall within the expected range.")
		{
			base.HResult = -2147024809;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00020FCD File Offset: 0x0001F1CD
		public ArgumentException(string message) : base(message)
		{
			base.HResult = -2147024809;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00020FE1 File Offset: 0x0001F1E1
		public ArgumentException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147024809;
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00020FF6 File Offset: 0x0001F1F6
		public ArgumentException(string message, string paramName, Exception innerException) : base(message, innerException)
		{
			this._paramName = paramName;
			base.HResult = -2147024809;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00021012 File Offset: 0x0001F212
		public ArgumentException(string message, string paramName) : base(message)
		{
			this._paramName = paramName;
			base.HResult = -2147024809;
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0002102D File Offset: 0x0001F22D
		protected ArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._paramName = info.GetString("ParamName");
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x00021048 File Offset: 0x0001F248
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("ParamName", this._paramName, typeof(string));
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x00021070 File Offset: 0x0001F270
		public override string Message
		{
			get
			{
				string message = base.Message;
				if (!string.IsNullOrEmpty(this._paramName))
				{
					string str = SR.Format("Parameter name: {0}", this._paramName);
					return message + Environment.NewLine + str;
				}
				return message;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x000210B0 File Offset: 0x0001F2B0
		public virtual string ParamName
		{
			get
			{
				return this._paramName;
			}
		}

		// Token: 0x04001048 RID: 4168
		private string _paramName;
	}
}
