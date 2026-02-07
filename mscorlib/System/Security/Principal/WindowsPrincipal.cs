using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Permissions;
using Mono;
using Unity;

namespace System.Security.Principal
{
	// Token: 0x020004EE RID: 1262
	[ComVisible(true)]
	[Serializable]
	public class WindowsPrincipal : ClaimsPrincipal
	{
		// Token: 0x06003278 RID: 12920 RVA: 0x000B99A3 File Offset: 0x000B7BA3
		public WindowsPrincipal(WindowsIdentity ntIdentity)
		{
			if (ntIdentity == null)
			{
				throw new ArgumentNullException("ntIdentity");
			}
			this._identity = ntIdentity;
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06003279 RID: 12921 RVA: 0x000B99C0 File Offset: 0x000B7BC0
		public override IIdentity Identity
		{
			get
			{
				return this._identity;
			}
		}

		// Token: 0x0600327A RID: 12922 RVA: 0x000B99C8 File Offset: 0x000B7BC8
		public virtual bool IsInRole(int rid)
		{
			if (Environment.IsUnix)
			{
				return WindowsPrincipal.IsMemberOfGroupId(this.Token, (IntPtr)rid);
			}
			string role;
			switch (rid)
			{
			case 544:
				role = "BUILTIN\\Administrators";
				break;
			case 545:
				role = "BUILTIN\\Users";
				break;
			case 546:
				role = "BUILTIN\\Guests";
				break;
			case 547:
				role = "BUILTIN\\Power Users";
				break;
			case 548:
				role = "BUILTIN\\Account Operators";
				break;
			case 549:
				role = "BUILTIN\\System Operators";
				break;
			case 550:
				role = "BUILTIN\\Print Operators";
				break;
			case 551:
				role = "BUILTIN\\Backup Operators";
				break;
			case 552:
				role = "BUILTIN\\Replicator";
				break;
			default:
				return false;
			}
			return this.IsInRole(role);
		}

		// Token: 0x0600327B RID: 12923 RVA: 0x000B9A74 File Offset: 0x000B7C74
		[SecuritySafeCritical]
		[SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
		public override bool IsInRole(string role)
		{
			if (role == null)
			{
				return false;
			}
			if (Environment.IsUnix)
			{
				using (SafeStringMarshal safeStringMarshal = new SafeStringMarshal(role))
				{
					return WindowsPrincipal.IsMemberOfGroupName(this.Token, safeStringMarshal.Value);
				}
			}
			if (this.m_roles == null)
			{
				this.m_roles = WindowsIdentity._GetRoles(this.Token);
			}
			role = role.ToUpperInvariant();
			foreach (string text in this.m_roles)
			{
				if (text != null && role == text.ToUpperInvariant())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x000B9B1C File Offset: 0x000B7D1C
		public virtual bool IsInRole(WindowsBuiltInRole role)
		{
			if (!Environment.IsUnix)
			{
				return this.IsInRole((int)role);
			}
			if (role == WindowsBuiltInRole.Administrator)
			{
				string role2 = "root";
				return this.IsInRole(role2);
			}
			return false;
		}

		// Token: 0x0600327D RID: 12925 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("not implemented")]
		[ComVisible(false)]
		public virtual bool IsInRole(SecurityIdentifier sid)
		{
			throw new NotImplementedException();
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x0600327E RID: 12926 RVA: 0x000B9B53 File Offset: 0x000B7D53
		private IntPtr Token
		{
			get
			{
				return this._identity.Token;
			}
		}

		// Token: 0x0600327F RID: 12927
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsMemberOfGroupId(IntPtr user, IntPtr group);

		// Token: 0x06003280 RID: 12928
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsMemberOfGroupName(IntPtr user, IntPtr group);

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06003281 RID: 12929 RVA: 0x000B990B File Offset: 0x000B7B0B
		public virtual IEnumerable<Claim> DeviceClaims
		{
			get
			{
				ThrowStub.ThrowNotSupportedException();
				return 0;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06003282 RID: 12930 RVA: 0x000B990B File Offset: 0x000B7B0B
		public virtual IEnumerable<Claim> UserClaims
		{
			get
			{
				ThrowStub.ThrowNotSupportedException();
				return 0;
			}
		}

		// Token: 0x04002346 RID: 9030
		private WindowsIdentity _identity;

		// Token: 0x04002347 RID: 9031
		private string[] m_roles;
	}
}
