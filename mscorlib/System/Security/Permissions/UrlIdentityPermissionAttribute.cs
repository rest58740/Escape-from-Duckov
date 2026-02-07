using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000463 RID: 1123
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class UrlIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002DA7 RID: 11687 RVA: 0x0009DD00 File Offset: 0x0009BF00
		public UrlIdentityPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06002DA8 RID: 11688 RVA: 0x000A38F8 File Offset: 0x000A1AF8
		// (set) Token: 0x06002DA9 RID: 11689 RVA: 0x000A3900 File Offset: 0x000A1B00
		public string Url
		{
			get
			{
				return this.url;
			}
			set
			{
				this.url = value;
			}
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x000A3909 File Offset: 0x000A1B09
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new UrlIdentityPermission(PermissionState.Unrestricted);
			}
			if (this.url == null)
			{
				return new UrlIdentityPermission(PermissionState.None);
			}
			return new UrlIdentityPermission(this.url);
		}

		// Token: 0x040020BF RID: 8383
		private string url;
	}
}
