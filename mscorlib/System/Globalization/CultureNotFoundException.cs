using System;
using System.Runtime.Serialization;
using System.Security;

namespace System.Globalization
{
	// Token: 0x02000957 RID: 2391
	[Serializable]
	public class CultureNotFoundException : ArgumentException
	{
		// Token: 0x06005499 RID: 21657 RVA: 0x0011AB4E File Offset: 0x00118D4E
		public CultureNotFoundException() : base(CultureNotFoundException.DefaultMessage)
		{
		}

		// Token: 0x0600549A RID: 21658 RVA: 0x0011AB5B File Offset: 0x00118D5B
		public CultureNotFoundException(string message) : base(message)
		{
		}

		// Token: 0x0600549B RID: 21659 RVA: 0x0011AB64 File Offset: 0x00118D64
		public CultureNotFoundException(string paramName, string message) : base(message, paramName)
		{
		}

		// Token: 0x0600549C RID: 21660 RVA: 0x0011AB6E File Offset: 0x00118D6E
		public CultureNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600549D RID: 21661 RVA: 0x0011AB78 File Offset: 0x00118D78
		public CultureNotFoundException(string paramName, string invalidCultureName, string message) : base(message, paramName)
		{
			this._invalidCultureName = invalidCultureName;
		}

		// Token: 0x0600549E RID: 21662 RVA: 0x0011AB89 File Offset: 0x00118D89
		public CultureNotFoundException(string message, string invalidCultureName, Exception innerException) : base(message, innerException)
		{
			this._invalidCultureName = invalidCultureName;
		}

		// Token: 0x0600549F RID: 21663 RVA: 0x0011AB9A File Offset: 0x00118D9A
		public CultureNotFoundException(string message, int invalidCultureId, Exception innerException) : base(message, innerException)
		{
			this._invalidCultureId = new int?(invalidCultureId);
		}

		// Token: 0x060054A0 RID: 21664 RVA: 0x0011ABB0 File Offset: 0x00118DB0
		public CultureNotFoundException(string paramName, int invalidCultureId, string message) : base(message, paramName)
		{
			this._invalidCultureId = new int?(invalidCultureId);
		}

		// Token: 0x060054A1 RID: 21665 RVA: 0x0011ABC8 File Offset: 0x00118DC8
		protected CultureNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._invalidCultureId = (int?)info.GetValue("InvalidCultureId", typeof(int?));
			this._invalidCultureName = (string)info.GetValue("InvalidCultureName", typeof(string));
		}

		// Token: 0x060054A2 RID: 21666 RVA: 0x0011AC20 File Offset: 0x00118E20
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("InvalidCultureId", this._invalidCultureId, typeof(int?));
			info.AddValue("InvalidCultureName", this._invalidCultureName, typeof(string));
		}

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x060054A3 RID: 21667 RVA: 0x0011AC70 File Offset: 0x00118E70
		public virtual int? InvalidCultureId
		{
			get
			{
				return this._invalidCultureId;
			}
		}

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x060054A4 RID: 21668 RVA: 0x0011AC78 File Offset: 0x00118E78
		public virtual string InvalidCultureName
		{
			get
			{
				return this._invalidCultureName;
			}
		}

		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x060054A5 RID: 21669 RVA: 0x0011AC80 File Offset: 0x00118E80
		private static string DefaultMessage
		{
			get
			{
				return "Culture is not supported.";
			}
		}

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x060054A6 RID: 21670 RVA: 0x0011AC88 File Offset: 0x00118E88
		private string FormatedInvalidCultureId
		{
			get
			{
				if (this.InvalidCultureId == null)
				{
					return this.InvalidCultureName;
				}
				return string.Format(CultureInfo.InvariantCulture, "{0} (0x{0:x4})", this.InvalidCultureId.Value);
			}
		}

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x060054A7 RID: 21671 RVA: 0x0011ACD0 File Offset: 0x00118ED0
		public override string Message
		{
			get
			{
				string message = base.Message;
				if (this._invalidCultureId == null && this._invalidCultureName == null)
				{
					return message;
				}
				string text = SR.Format("{0} is an invalid culture identifier.", this.FormatedInvalidCultureId);
				if (message == null)
				{
					return text;
				}
				return message + Environment.NewLine + text;
			}
		}

		// Token: 0x040033D2 RID: 13266
		private string _invalidCultureName;

		// Token: 0x040033D3 RID: 13267
		private int? _invalidCultureId;
	}
}
