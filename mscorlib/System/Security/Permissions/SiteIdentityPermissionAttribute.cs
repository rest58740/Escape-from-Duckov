using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200045B RID: 1115
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class SiteIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002D51 RID: 11601 RVA: 0x0009DD00 File Offset: 0x0009BF00
		public SiteIdentityPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06002D52 RID: 11602 RVA: 0x000A24A0 File Offset: 0x000A06A0
		// (set) Token: 0x06002D53 RID: 11603 RVA: 0x000A24A8 File Offset: 0x000A06A8
		public string Site
		{
			get
			{
				return this.site;
			}
			set
			{
				this.site = value;
			}
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x000A24B4 File Offset: 0x000A06B4
		public override IPermission CreatePermission()
		{
			SiteIdentityPermission result;
			if (base.Unrestricted)
			{
				result = new SiteIdentityPermission(PermissionState.Unrestricted);
			}
			else if (this.site == null)
			{
				result = new SiteIdentityPermission(PermissionState.None);
			}
			else
			{
				result = new SiteIdentityPermission(this.site);
			}
			return result;
		}

		// Token: 0x040020AC RID: 8364
		private string site;
	}
}
