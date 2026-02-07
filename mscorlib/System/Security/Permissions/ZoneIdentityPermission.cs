using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000464 RID: 1124
	[ComVisible(true)]
	[Serializable]
	public sealed class ZoneIdentityPermission : CodeAccessPermission, IBuiltInPermission
	{
		// Token: 0x06002DAB RID: 11691 RVA: 0x000A3934 File Offset: 0x000A1B34
		public ZoneIdentityPermission(PermissionState state)
		{
			CodeAccessPermission.CheckPermissionState(state, false);
			this.zone = SecurityZone.NoZone;
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x000A394B File Offset: 0x000A1B4B
		public ZoneIdentityPermission(SecurityZone zone)
		{
			this.SecurityZone = zone;
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x000A395A File Offset: 0x000A1B5A
		public override IPermission Copy()
		{
			return new ZoneIdentityPermission(this.zone);
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x000A3968 File Offset: 0x000A1B68
		public override bool IsSubsetOf(IPermission target)
		{
			ZoneIdentityPermission zoneIdentityPermission = this.Cast(target);
			if (zoneIdentityPermission == null)
			{
				return this.zone == SecurityZone.NoZone;
			}
			return this.zone == SecurityZone.NoZone || this.zone == zoneIdentityPermission.zone;
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x000A39A4 File Offset: 0x000A1BA4
		public override IPermission Union(IPermission target)
		{
			ZoneIdentityPermission zoneIdentityPermission = this.Cast(target);
			if (zoneIdentityPermission == null)
			{
				if (this.zone != SecurityZone.NoZone)
				{
					return this.Copy();
				}
				return null;
			}
			else
			{
				if (this.zone == zoneIdentityPermission.zone || zoneIdentityPermission.zone == SecurityZone.NoZone)
				{
					return this.Copy();
				}
				if (this.zone == SecurityZone.NoZone)
				{
					return zoneIdentityPermission.Copy();
				}
				throw new ArgumentException(Locale.GetText("Union impossible"));
			}
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x000A3A0C File Offset: 0x000A1C0C
		public override IPermission Intersect(IPermission target)
		{
			ZoneIdentityPermission zoneIdentityPermission = this.Cast(target);
			if (zoneIdentityPermission == null || this.zone == SecurityZone.NoZone)
			{
				return null;
			}
			if (this.zone == zoneIdentityPermission.zone)
			{
				return this.Copy();
			}
			return null;
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x000A3A48 File Offset: 0x000A1C48
		public override void FromXml(SecurityElement esd)
		{
			CodeAccessPermission.CheckSecurityElement(esd, "esd", 1, 1);
			string text = esd.Attribute("Zone");
			if (text == null)
			{
				this.zone = SecurityZone.NoZone;
				return;
			}
			this.zone = (SecurityZone)Enum.Parse(typeof(SecurityZone), text);
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x000A3A98 File Offset: 0x000A1C98
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = base.Element(1);
			if (this.zone != SecurityZone.NoZone)
			{
				securityElement.AddAttribute("Zone", this.zone.ToString());
			}
			return securityElement;
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06002DB3 RID: 11699 RVA: 0x000A3AD3 File Offset: 0x000A1CD3
		// (set) Token: 0x06002DB4 RID: 11700 RVA: 0x000A3ADB File Offset: 0x000A1CDB
		public SecurityZone SecurityZone
		{
			get
			{
				return this.zone;
			}
			set
			{
				if (!Enum.IsDefined(typeof(SecurityZone), value))
				{
					throw new ArgumentException(string.Format(Locale.GetText("Invalid enum {0}"), value), "SecurityZone");
				}
				this.zone = value;
			}
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x000286A6 File Offset: 0x000268A6
		int IBuiltInPermission.GetTokenIndex()
		{
			return 14;
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x000A3B1B File Offset: 0x000A1D1B
		private ZoneIdentityPermission Cast(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			ZoneIdentityPermission zoneIdentityPermission = target as ZoneIdentityPermission;
			if (zoneIdentityPermission == null)
			{
				CodeAccessPermission.ThrowInvalidPermission(target, typeof(ZoneIdentityPermission));
			}
			return zoneIdentityPermission;
		}

		// Token: 0x040020C0 RID: 8384
		private const int version = 1;

		// Token: 0x040020C1 RID: 8385
		private SecurityZone zone;
	}
}
