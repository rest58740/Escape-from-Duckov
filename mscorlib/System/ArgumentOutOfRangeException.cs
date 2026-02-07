using System;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x020000F5 RID: 245
	[Serializable]
	public class ArgumentOutOfRangeException : ArgumentException
	{
		// Token: 0x0600070C RID: 1804 RVA: 0x0002111D File Offset: 0x0001F31D
		public ArgumentOutOfRangeException() : base("Specified argument was out of the range of valid values.")
		{
			base.HResult = -2146233086;
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00021135 File Offset: 0x0001F335
		public ArgumentOutOfRangeException(string paramName) : base("Specified argument was out of the range of valid values.", paramName)
		{
			base.HResult = -2146233086;
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0002114E File Offset: 0x0001F34E
		public ArgumentOutOfRangeException(string paramName, string message) : base(message, paramName)
		{
			base.HResult = -2146233086;
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00021163 File Offset: 0x0001F363
		public ArgumentOutOfRangeException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233086;
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00021178 File Offset: 0x0001F378
		public ArgumentOutOfRangeException(string paramName, object actualValue, string message) : base(message, paramName)
		{
			this._actualValue = actualValue;
			base.HResult = -2146233086;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00021194 File Offset: 0x0001F394
		protected ArgumentOutOfRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._actualValue = info.GetValue("ActualValue", typeof(object));
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x000211B9 File Offset: 0x0001F3B9
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("ActualValue", this._actualValue, typeof(object));
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x000211E0 File Offset: 0x0001F3E0
		public override string Message
		{
			get
			{
				string message = base.Message;
				if (this._actualValue == null)
				{
					return message;
				}
				string text = SR.Format("Actual value was {0}.", this._actualValue.ToString());
				if (message == null)
				{
					return text;
				}
				return message + Environment.NewLine + text;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x00021225 File Offset: 0x0001F425
		public virtual object ActualValue
		{
			get
			{
				return this._actualValue;
			}
		}

		// Token: 0x04001049 RID: 4169
		private object _actualValue;
	}
}
