using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000465 RID: 1125
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class ZoneIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002DB7 RID: 11703 RVA: 0x000A3B3B File Offset: 0x000A1D3B
		public ZoneIdentityPermissionAttribute(SecurityAction action) : base(action)
		{
			this.zone = SecurityZone.NoZone;
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06002DB8 RID: 11704 RVA: 0x000A3B4B File Offset: 0x000A1D4B
		// (set) Token: 0x06002DB9 RID: 11705 RVA: 0x000A3B53 File Offset: 0x000A1D53
		public SecurityZone Zone
		{
			get
			{
				return this.zone;
			}
			set
			{
				this.zone = value;
			}
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x000A3B5C File Offset: 0x000A1D5C
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new ZoneIdentityPermission(PermissionState.Unrestricted);
			}
			return new ZoneIdentityPermission(this.zone);
		}

		// Token: 0x040020C2 RID: 8386
		private SecurityZone zone;
	}
}
