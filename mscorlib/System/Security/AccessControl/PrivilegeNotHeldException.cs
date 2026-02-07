using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Security.AccessControl
{
	// Token: 0x020004FE RID: 1278
	[Serializable]
	public sealed class PrivilegeNotHeldException : UnauthorizedAccessException, ISerializable
	{
		// Token: 0x06003335 RID: 13109 RVA: 0x000BC76C File Offset: 0x000BA96C
		public PrivilegeNotHeldException() : base("The process does not possess some privilege required for this operation.")
		{
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x000BC779 File Offset: 0x000BA979
		public PrivilegeNotHeldException(string privilege) : base(string.Format(CultureInfo.CurrentCulture, "The process does not possess the '{0}' privilege which is required for this operation.", privilege))
		{
			this._privilegeName = privilege;
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x000BC798 File Offset: 0x000BA998
		public PrivilegeNotHeldException(string privilege, Exception inner) : base(string.Format(CultureInfo.CurrentCulture, "The process does not possess the '{0}' privilege which is required for this operation.", privilege), inner)
		{
			this._privilegeName = privilege;
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x000BC7B8 File Offset: 0x000BA9B8
		private PrivilegeNotHeldException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._privilegeName = info.GetString("PrivilegeName");
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x000BC7D3 File Offset: 0x000BA9D3
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("PrivilegeName", this._privilegeName, typeof(string));
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x0600333A RID: 13114 RVA: 0x000BC7F8 File Offset: 0x000BA9F8
		public string PrivilegeName
		{
			get
			{
				return this._privilegeName;
			}
		}

		// Token: 0x040023F8 RID: 9208
		private readonly string _privilegeName;
	}
}
