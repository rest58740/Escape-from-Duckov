using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000457 RID: 1111
	[ComVisible(true)]
	[Serializable]
	public sealed class SecurityPermission : CodeAccessPermission, IUnrestrictedPermission, IBuiltInPermission
	{
		// Token: 0x06002D13 RID: 11539 RVA: 0x000A1B44 File Offset: 0x0009FD44
		public SecurityPermission(PermissionState state)
		{
			if (CodeAccessPermission.CheckPermissionState(state, true) == PermissionState.Unrestricted)
			{
				this.flags = SecurityPermissionFlag.AllFlags;
				return;
			}
			this.flags = SecurityPermissionFlag.NoFlags;
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x000A1B69 File Offset: 0x0009FD69
		public SecurityPermission(SecurityPermissionFlag flag)
		{
			this.Flags = flag;
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06002D15 RID: 11541 RVA: 0x000A1B78 File Offset: 0x0009FD78
		// (set) Token: 0x06002D16 RID: 11542 RVA: 0x000A1B80 File Offset: 0x0009FD80
		public SecurityPermissionFlag Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				if ((value & SecurityPermissionFlag.AllFlags) != value)
				{
					throw new ArgumentException(string.Format(Locale.GetText("Invalid flags {0}"), value), "SecurityPermissionFlag");
				}
				this.flags = value;
			}
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x000A1BB3 File Offset: 0x0009FDB3
		public bool IsUnrestricted()
		{
			return this.flags == SecurityPermissionFlag.AllFlags;
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x000A1BC2 File Offset: 0x0009FDC2
		public override IPermission Copy()
		{
			return new SecurityPermission(this.flags);
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x000A1BD0 File Offset: 0x0009FDD0
		public override IPermission Intersect(IPermission target)
		{
			SecurityPermission securityPermission = this.Cast(target);
			if (securityPermission == null)
			{
				return null;
			}
			if (this.IsEmpty() || securityPermission.IsEmpty())
			{
				return null;
			}
			if (this.IsUnrestricted() && securityPermission.IsUnrestricted())
			{
				return new SecurityPermission(PermissionState.Unrestricted);
			}
			if (this.IsUnrestricted())
			{
				return securityPermission.Copy();
			}
			if (securityPermission.IsUnrestricted())
			{
				return this.Copy();
			}
			SecurityPermissionFlag securityPermissionFlag = this.flags & securityPermission.flags;
			if (securityPermissionFlag == SecurityPermissionFlag.NoFlags)
			{
				return null;
			}
			return new SecurityPermission(securityPermissionFlag);
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x000A1C4C File Offset: 0x0009FE4C
		public override IPermission Union(IPermission target)
		{
			SecurityPermission securityPermission = this.Cast(target);
			if (securityPermission == null)
			{
				return this.Copy();
			}
			if (this.IsUnrestricted() || securityPermission.IsUnrestricted())
			{
				return new SecurityPermission(PermissionState.Unrestricted);
			}
			return new SecurityPermission(this.flags | securityPermission.flags);
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x000A1C94 File Offset: 0x0009FE94
		public override bool IsSubsetOf(IPermission target)
		{
			SecurityPermission securityPermission = this.Cast(target);
			if (securityPermission == null)
			{
				return this.IsEmpty();
			}
			return securityPermission.IsUnrestricted() || (!this.IsUnrestricted() && (this.flags & ~securityPermission.flags) == SecurityPermissionFlag.NoFlags);
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x000A1CD8 File Offset: 0x0009FED8
		public override void FromXml(SecurityElement esd)
		{
			CodeAccessPermission.CheckSecurityElement(esd, "esd", 1, 1);
			if (CodeAccessPermission.IsUnrestricted(esd))
			{
				this.flags = SecurityPermissionFlag.AllFlags;
				return;
			}
			string text = esd.Attribute("Flags");
			if (text == null)
			{
				this.flags = SecurityPermissionFlag.NoFlags;
				return;
			}
			this.flags = (SecurityPermissionFlag)Enum.Parse(typeof(SecurityPermissionFlag), text);
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x000A1D3C File Offset: 0x0009FF3C
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = base.Element(1);
			if (this.IsUnrestricted())
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			else
			{
				securityElement.AddAttribute("Flags", this.flags.ToString());
			}
			return securityElement;
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x000224A7 File Offset: 0x000206A7
		int IBuiltInPermission.GetTokenIndex()
		{
			return 6;
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x000A1D88 File Offset: 0x0009FF88
		private bool IsEmpty()
		{
			return this.flags == SecurityPermissionFlag.NoFlags;
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x000A1D93 File Offset: 0x0009FF93
		private SecurityPermission Cast(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			SecurityPermission securityPermission = target as SecurityPermission;
			if (securityPermission == null)
			{
				CodeAccessPermission.ThrowInvalidPermission(target, typeof(SecurityPermission));
			}
			return securityPermission;
		}

		// Token: 0x04002095 RID: 8341
		private const int version = 1;

		// Token: 0x04002096 RID: 8342
		private SecurityPermissionFlag flags;
	}
}
