using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200045A RID: 1114
	[ComVisible(true)]
	[Serializable]
	public sealed class SiteIdentityPermission : CodeAccessPermission, IBuiltInPermission
	{
		// Token: 0x06002D41 RID: 11585 RVA: 0x0009EC58 File Offset: 0x0009CE58
		public SiteIdentityPermission(PermissionState state)
		{
			CodeAccessPermission.CheckPermissionState(state, false);
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000A20F6 File Offset: 0x000A02F6
		public SiteIdentityPermission(string site)
		{
			this.Site = site;
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06002D43 RID: 11587 RVA: 0x000A2105 File Offset: 0x000A0305
		// (set) Token: 0x06002D44 RID: 11588 RVA: 0x000A2120 File Offset: 0x000A0320
		public string Site
		{
			get
			{
				if (this.IsEmpty())
				{
					throw new NullReferenceException("No site.");
				}
				return this._site;
			}
			set
			{
				if (!this.IsValid(value))
				{
					throw new ArgumentException("Invalid site.");
				}
				this._site = value;
			}
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x000A213D File Offset: 0x000A033D
		public override IPermission Copy()
		{
			if (this.IsEmpty())
			{
				return new SiteIdentityPermission(PermissionState.None);
			}
			return new SiteIdentityPermission(this._site);
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x000A215C File Offset: 0x000A035C
		public override void FromXml(SecurityElement esd)
		{
			CodeAccessPermission.CheckSecurityElement(esd, "esd", 1, 1);
			string text = esd.Attribute("Site");
			if (text != null)
			{
				this.Site = text;
			}
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x000A2190 File Offset: 0x000A0390
		public override IPermission Intersect(IPermission target)
		{
			SiteIdentityPermission siteIdentityPermission = this.Cast(target);
			if (siteIdentityPermission == null || this.IsEmpty())
			{
				return null;
			}
			if (this.Match(siteIdentityPermission._site))
			{
				return new SiteIdentityPermission((this._site.Length > siteIdentityPermission._site.Length) ? this._site : siteIdentityPermission._site);
			}
			return null;
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x000A21F0 File Offset: 0x000A03F0
		public override bool IsSubsetOf(IPermission target)
		{
			SiteIdentityPermission siteIdentityPermission = this.Cast(target);
			if (siteIdentityPermission == null)
			{
				return this.IsEmpty();
			}
			if (this._site == null && siteIdentityPermission._site == null)
			{
				return true;
			}
			if (this._site == null || siteIdentityPermission._site == null)
			{
				return false;
			}
			int num = siteIdentityPermission._site.IndexOf('*');
			if (num == -1)
			{
				return this._site == siteIdentityPermission._site;
			}
			return this._site.EndsWith(siteIdentityPermission._site.Substring(num + 1));
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x000A2270 File Offset: 0x000A0470
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = base.Element(1);
			if (this._site != null)
			{
				securityElement.AddAttribute("Site", this._site);
			}
			return securityElement;
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x000A22A0 File Offset: 0x000A04A0
		public override IPermission Union(IPermission target)
		{
			SiteIdentityPermission siteIdentityPermission = this.Cast(target);
			if (siteIdentityPermission == null || siteIdentityPermission.IsEmpty())
			{
				return this.Copy();
			}
			if (this.IsEmpty())
			{
				return siteIdentityPermission.Copy();
			}
			if (this.Match(siteIdentityPermission._site))
			{
				return new SiteIdentityPermission((this._site.Length < siteIdentityPermission._site.Length) ? this._site : siteIdentityPermission._site);
			}
			throw new ArgumentException(Locale.GetText("Cannot union two different sites."), "target");
		}

		// Token: 0x06002D4B RID: 11595 RVA: 0x00032734 File Offset: 0x00030934
		int IBuiltInPermission.GetTokenIndex()
		{
			return 11;
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x000A2324 File Offset: 0x000A0524
		private bool IsEmpty()
		{
			return this._site == null;
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x000A232F File Offset: 0x000A052F
		private SiteIdentityPermission Cast(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			SiteIdentityPermission siteIdentityPermission = target as SiteIdentityPermission;
			if (siteIdentityPermission == null)
			{
				CodeAccessPermission.ThrowInvalidPermission(target, typeof(SiteIdentityPermission));
			}
			return siteIdentityPermission;
		}

		// Token: 0x06002D4E RID: 11598 RVA: 0x000A2350 File Offset: 0x000A0550
		private bool IsValid(string s)
		{
			if (s == null || s.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < s.Length; i++)
			{
				ushort num = (ushort)s[i];
				if (num < 33 || num > 126)
				{
					return false;
				}
				if (num == 42 && s.Length > 1 && (i > 0 || s[i + 1] != '.'))
				{
					return false;
				}
				if (!SiteIdentityPermission.valid[(int)(num - 33)])
				{
					return false;
				}
			}
			return s.Length != 1 || s[0] != '.';
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x000A23D8 File Offset: 0x000A05D8
		private bool Match(string target)
		{
			if (this._site == null || target == null)
			{
				return false;
			}
			int num = this._site.IndexOf('*');
			int num2 = target.IndexOf('*');
			if (num == -1 && num2 == -1)
			{
				return this._site == target;
			}
			if (num == -1)
			{
				return this._site.EndsWith(target.Substring(num2 + 1));
			}
			if (num2 == -1)
			{
				return target.EndsWith(this._site.Substring(num + 1));
			}
			string text = this._site.Substring(num + 1);
			target = target.Substring(num2 + 1);
			if (text.Length > target.Length)
			{
				return text.EndsWith(target);
			}
			return target.EndsWith(text);
		}

		// Token: 0x040020A9 RID: 8361
		private const int version = 1;

		// Token: 0x040020AA RID: 8362
		private string _site;

		// Token: 0x040020AB RID: 8363
		private static bool[] valid = new bool[]
		{
			true,
			false,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			false,
			false,
			true,
			true,
			false,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			false,
			false,
			false,
			false,
			false,
			false,
			false,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			false,
			false,
			false,
			true,
			true,
			false,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			false,
			true,
			true
		};
	}
}
