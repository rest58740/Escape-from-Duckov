using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200043B RID: 1083
	[ComVisible(true)]
	[Serializable]
	public sealed class GacIdentityPermission : CodeAccessPermission, IBuiltInPermission
	{
		// Token: 0x06002C01 RID: 11265 RVA: 0x0009E0EE File Offset: 0x0009C2EE
		public GacIdentityPermission()
		{
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x0009EC58 File Offset: 0x0009CE58
		public GacIdentityPermission(PermissionState state)
		{
			CodeAccessPermission.CheckPermissionState(state, false);
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x0009A092 File Offset: 0x00098292
		public override IPermission Copy()
		{
			return new GacIdentityPermission();
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x0009EC68 File Offset: 0x0009CE68
		public override IPermission Intersect(IPermission target)
		{
			if (this.Cast(target) == null)
			{
				return null;
			}
			return this.Copy();
		}

		// Token: 0x06002C05 RID: 11269 RVA: 0x0009EC7B File Offset: 0x0009CE7B
		public override bool IsSubsetOf(IPermission target)
		{
			return this.Cast(target) != null;
		}

		// Token: 0x06002C06 RID: 11270 RVA: 0x0009EC87 File Offset: 0x0009CE87
		public override IPermission Union(IPermission target)
		{
			this.Cast(target);
			return this.Copy();
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x0009EC97 File Offset: 0x0009CE97
		public override void FromXml(SecurityElement securityElement)
		{
			CodeAccessPermission.CheckSecurityElement(securityElement, "securityElement", 1, 1);
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x0009ECA7 File Offset: 0x0009CEA7
		public override SecurityElement ToXml()
		{
			return base.Element(1);
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x0006AFDA File Offset: 0x000691DA
		int IBuiltInPermission.GetTokenIndex()
		{
			return 15;
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x0009ECB0 File Offset: 0x0009CEB0
		private GacIdentityPermission Cast(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			GacIdentityPermission gacIdentityPermission = target as GacIdentityPermission;
			if (gacIdentityPermission == null)
			{
				CodeAccessPermission.ThrowInvalidPermission(target, typeof(GacIdentityPermission));
			}
			return gacIdentityPermission;
		}

		// Token: 0x0400202C RID: 8236
		private const int version = 1;
	}
}
