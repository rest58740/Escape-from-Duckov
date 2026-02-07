using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000462 RID: 1122
	[ComVisible(true)]
	[Serializable]
	public sealed class UrlIdentityPermission : CodeAccessPermission, IBuiltInPermission
	{
		// Token: 0x06002D99 RID: 11673 RVA: 0x000A35F3 File Offset: 0x000A17F3
		public UrlIdentityPermission(PermissionState state)
		{
			CodeAccessPermission.CheckPermissionState(state, false);
			this.url = string.Empty;
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x000A360E File Offset: 0x000A180E
		public UrlIdentityPermission(string site)
		{
			if (site == null)
			{
				throw new ArgumentNullException("site");
			}
			this.url = site;
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06002D9B RID: 11675 RVA: 0x000A362B File Offset: 0x000A182B
		// (set) Token: 0x06002D9C RID: 11676 RVA: 0x000A3633 File Offset: 0x000A1833
		public string Url
		{
			get
			{
				return this.url;
			}
			set
			{
				this.url = ((value == null) ? string.Empty : value);
			}
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x000A3646 File Offset: 0x000A1846
		public override IPermission Copy()
		{
			if (this.url == null)
			{
				return new UrlIdentityPermission(PermissionState.None);
			}
			return new UrlIdentityPermission(this.url);
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x000A3664 File Offset: 0x000A1864
		public override void FromXml(SecurityElement esd)
		{
			CodeAccessPermission.CheckSecurityElement(esd, "esd", 1, 1);
			string text = esd.Attribute("Url");
			if (text == null)
			{
				this.url = string.Empty;
				return;
			}
			this.Url = text;
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x000A36A4 File Offset: 0x000A18A4
		public override IPermission Intersect(IPermission target)
		{
			UrlIdentityPermission urlIdentityPermission = this.Cast(target);
			if (urlIdentityPermission == null || this.IsEmpty())
			{
				return null;
			}
			if (!this.Match(urlIdentityPermission.url))
			{
				return null;
			}
			if (this.url.Length > urlIdentityPermission.url.Length)
			{
				return this.Copy();
			}
			return urlIdentityPermission.Copy();
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x000A36FC File Offset: 0x000A18FC
		public override bool IsSubsetOf(IPermission target)
		{
			UrlIdentityPermission urlIdentityPermission = this.Cast(target);
			if (urlIdentityPermission == null)
			{
				return this.IsEmpty();
			}
			if (this.IsEmpty())
			{
				return true;
			}
			if (urlIdentityPermission.url == null)
			{
				return false;
			}
			int num = urlIdentityPermission.url.LastIndexOf('*');
			if (num == -1)
			{
				num = urlIdentityPermission.url.Length;
			}
			return string.Compare(this.url, 0, urlIdentityPermission.url, 0, num, true, CultureInfo.InvariantCulture) == 0;
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x000A376C File Offset: 0x000A196C
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = base.Element(1);
			if (!this.IsEmpty())
			{
				securityElement.AddAttribute("Url", this.url);
			}
			return securityElement;
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x000A379C File Offset: 0x000A199C
		public override IPermission Union(IPermission target)
		{
			UrlIdentityPermission urlIdentityPermission = this.Cast(target);
			if (urlIdentityPermission == null)
			{
				return this.Copy();
			}
			if (this.IsEmpty() && urlIdentityPermission.IsEmpty())
			{
				return null;
			}
			if (urlIdentityPermission.IsEmpty())
			{
				return this.Copy();
			}
			if (this.IsEmpty())
			{
				return urlIdentityPermission.Copy();
			}
			if (!this.Match(urlIdentityPermission.url))
			{
				throw new ArgumentException(Locale.GetText("Cannot union two different urls."), "target");
			}
			if (this.url.Length < urlIdentityPermission.url.Length)
			{
				return this.Copy();
			}
			return urlIdentityPermission.Copy();
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x0003D1EA File Offset: 0x0003B3EA
		int IBuiltInPermission.GetTokenIndex()
		{
			return 13;
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000A3833 File Offset: 0x000A1A33
		private bool IsEmpty()
		{
			return this.url == null || this.url.Length == 0;
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x000A384D File Offset: 0x000A1A4D
		private UrlIdentityPermission Cast(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			UrlIdentityPermission urlIdentityPermission = target as UrlIdentityPermission;
			if (urlIdentityPermission == null)
			{
				CodeAccessPermission.ThrowInvalidPermission(target, typeof(UrlIdentityPermission));
			}
			return urlIdentityPermission;
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x000A3870 File Offset: 0x000A1A70
		private bool Match(string target)
		{
			if (this.url == null || target == null)
			{
				return false;
			}
			int num = this.url.LastIndexOf('*');
			int num2 = target.LastIndexOf('*');
			int length;
			if (num == -1 && num2 == -1)
			{
				length = Math.Max(this.url.Length, target.Length);
			}
			else if (num == -1)
			{
				length = num2;
			}
			else if (num2 == -1)
			{
				length = num;
			}
			else
			{
				length = Math.Min(num, num2);
			}
			return string.Compare(this.url, 0, target, 0, length, true, CultureInfo.InvariantCulture) == 0;
		}

		// Token: 0x040020BD RID: 8381
		private const int version = 1;

		// Token: 0x040020BE RID: 8382
		private string url;
	}
}
