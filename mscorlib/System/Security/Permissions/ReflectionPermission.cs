using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000451 RID: 1105
	[ComVisible(true)]
	[Serializable]
	public sealed class ReflectionPermission : CodeAccessPermission, IUnrestrictedPermission, IBuiltInPermission
	{
		// Token: 0x06002CCE RID: 11470 RVA: 0x000A0C46 File Offset: 0x0009EE46
		public ReflectionPermission(PermissionState state)
		{
			if (CodeAccessPermission.CheckPermissionState(state, true) == PermissionState.Unrestricted)
			{
				this.flags = ReflectionPermissionFlag.AllFlags;
				return;
			}
			this.flags = ReflectionPermissionFlag.NoFlags;
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x000A0C67 File Offset: 0x0009EE67
		public ReflectionPermission(ReflectionPermissionFlag flag)
		{
			this.Flags = flag;
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06002CD0 RID: 11472 RVA: 0x000A0C76 File Offset: 0x0009EE76
		// (set) Token: 0x06002CD1 RID: 11473 RVA: 0x000A0C7E File Offset: 0x0009EE7E
		public ReflectionPermissionFlag Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				if ((value & (ReflectionPermissionFlag.AllFlags | ReflectionPermissionFlag.RestrictedMemberAccess)) != value)
				{
					throw new ArgumentException(string.Format(Locale.GetText("Invalid flags {0}"), value), "ReflectionPermissionFlag");
				}
				this.flags = value;
			}
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x000A0CAE File Offset: 0x0009EEAE
		public override IPermission Copy()
		{
			return new ReflectionPermission(this.flags);
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x000A0CBC File Offset: 0x0009EEBC
		public override void FromXml(SecurityElement esd)
		{
			CodeAccessPermission.CheckSecurityElement(esd, "esd", 1, 1);
			if (CodeAccessPermission.IsUnrestricted(esd))
			{
				this.flags = ReflectionPermissionFlag.AllFlags;
				return;
			}
			this.flags = ReflectionPermissionFlag.NoFlags;
			string text = esd.Attributes["Flags"] as string;
			if (text.IndexOf("MemberAccess") >= 0)
			{
				this.flags |= ReflectionPermissionFlag.MemberAccess;
			}
			if (text.IndexOf("ReflectionEmit") >= 0)
			{
				this.flags |= ReflectionPermissionFlag.ReflectionEmit;
			}
			if (text.IndexOf("TypeInformation") >= 0)
			{
				this.flags |= ReflectionPermissionFlag.TypeInformation;
			}
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x000A0D58 File Offset: 0x0009EF58
		public override IPermission Intersect(IPermission target)
		{
			ReflectionPermission reflectionPermission = this.Cast(target);
			if (reflectionPermission == null)
			{
				return null;
			}
			if (this.IsUnrestricted())
			{
				if (reflectionPermission.Flags == ReflectionPermissionFlag.NoFlags)
				{
					return null;
				}
				return reflectionPermission.Copy();
			}
			else if (reflectionPermission.IsUnrestricted())
			{
				if (this.flags == ReflectionPermissionFlag.NoFlags)
				{
					return null;
				}
				return this.Copy();
			}
			else
			{
				ReflectionPermission reflectionPermission2 = (ReflectionPermission)reflectionPermission.Copy();
				reflectionPermission2.Flags &= this.flags;
				if (reflectionPermission2.Flags != ReflectionPermissionFlag.NoFlags)
				{
					return reflectionPermission2;
				}
				return null;
			}
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x000A0DD0 File Offset: 0x0009EFD0
		public override bool IsSubsetOf(IPermission target)
		{
			ReflectionPermission reflectionPermission = this.Cast(target);
			if (reflectionPermission == null)
			{
				return this.flags == ReflectionPermissionFlag.NoFlags;
			}
			if (this.IsUnrestricted())
			{
				return reflectionPermission.IsUnrestricted();
			}
			return reflectionPermission.IsUnrestricted() || (this.flags & reflectionPermission.Flags) == this.flags;
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x000A0E20 File Offset: 0x0009F020
		public bool IsUnrestricted()
		{
			return this.flags == ReflectionPermissionFlag.AllFlags;
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000A0E2C File Offset: 0x0009F02C
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = base.Element(1);
			if (this.IsUnrestricted())
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			else if (this.flags == ReflectionPermissionFlag.NoFlags)
			{
				securityElement.AddAttribute("Flags", "NoFlags");
			}
			else if ((this.flags & ReflectionPermissionFlag.AllFlags) == ReflectionPermissionFlag.AllFlags)
			{
				securityElement.AddAttribute("Flags", "AllFlags");
			}
			else
			{
				string text = "";
				if ((this.flags & ReflectionPermissionFlag.MemberAccess) == ReflectionPermissionFlag.MemberAccess)
				{
					text = "MemberAccess";
				}
				if ((this.flags & ReflectionPermissionFlag.ReflectionEmit) == ReflectionPermissionFlag.ReflectionEmit)
				{
					if (text.Length > 0)
					{
						text += ", ";
					}
					text += "ReflectionEmit";
				}
				if ((this.flags & ReflectionPermissionFlag.TypeInformation) == ReflectionPermissionFlag.TypeInformation)
				{
					if (text.Length > 0)
					{
						text += ", ";
					}
					text += "TypeInformation";
				}
				securityElement.AddAttribute("Flags", text);
			}
			return securityElement;
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000A0F14 File Offset: 0x0009F114
		public override IPermission Union(IPermission other)
		{
			ReflectionPermission reflectionPermission = this.Cast(other);
			if (other == null)
			{
				return this.Copy();
			}
			if (this.IsUnrestricted() || reflectionPermission.IsUnrestricted())
			{
				return new ReflectionPermission(PermissionState.Unrestricted);
			}
			ReflectionPermission reflectionPermission2 = (ReflectionPermission)reflectionPermission.Copy();
			reflectionPermission2.Flags |= this.flags;
			return reflectionPermission2;
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x0002280B File Offset: 0x00020A0B
		int IBuiltInPermission.GetTokenIndex()
		{
			return 4;
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x000A0F68 File Offset: 0x0009F168
		private ReflectionPermission Cast(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			ReflectionPermission reflectionPermission = target as ReflectionPermission;
			if (reflectionPermission == null)
			{
				CodeAccessPermission.ThrowInvalidPermission(target, typeof(ReflectionPermission));
			}
			return reflectionPermission;
		}

		// Token: 0x04002079 RID: 8313
		private const int version = 1;

		// Token: 0x0400207A RID: 8314
		private ReflectionPermissionFlag flags;
	}
}
