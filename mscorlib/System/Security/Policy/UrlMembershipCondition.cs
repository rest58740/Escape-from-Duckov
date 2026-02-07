using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using Mono.Security;

namespace System.Security.Policy
{
	// Token: 0x02000426 RID: 1062
	[ComVisible(true)]
	[Serializable]
	public sealed class UrlMembershipCondition : IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable, IConstantMembershipCondition
	{
		// Token: 0x06002B72 RID: 11122 RVA: 0x0009CFA4 File Offset: 0x0009B1A4
		public UrlMembershipCondition(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			this.CheckUrl(url);
			this.userUrl = url;
			this.url = new Url(url);
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x0009CFDB File Offset: 0x0009B1DB
		internal UrlMembershipCondition(Url url, string userUrl)
		{
			this.url = (Url)url.Copy();
			this.userUrl = userUrl;
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06002B74 RID: 11124 RVA: 0x0009D002 File Offset: 0x0009B202
		// (set) Token: 0x06002B75 RID: 11125 RVA: 0x0009D023 File Offset: 0x0009B223
		public string Url
		{
			get
			{
				if (this.userUrl == null)
				{
					this.userUrl = this.url.Value;
				}
				return this.userUrl;
			}
			set
			{
				this.url = new Url(value);
			}
		}

		// Token: 0x06002B76 RID: 11126 RVA: 0x0009D034 File Offset: 0x0009B234
		public bool Check(Evidence evidence)
		{
			if (evidence == null)
			{
				return false;
			}
			string value = this.url.Value;
			int num = value.LastIndexOf("*");
			if (num == -1)
			{
				num = value.Length;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				if (hostEnumerator.Current is Url && string.Compare(value, 0, (hostEnumerator.Current as Url).Value, 0, num, true, CultureInfo.InvariantCulture) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002B77 RID: 11127 RVA: 0x0009D0AB File Offset: 0x0009B2AB
		public IMembershipCondition Copy()
		{
			return new UrlMembershipCondition(this.url, this.userUrl);
		}

		// Token: 0x06002B78 RID: 11128 RVA: 0x0009D0C0 File Offset: 0x0009B2C0
		public override bool Equals(object o)
		{
			UrlMembershipCondition urlMembershipCondition = o as UrlMembershipCondition;
			if (o == null)
			{
				return false;
			}
			string value = this.url.Value;
			int num = value.Length;
			if (value[num - 1] == '*')
			{
				num--;
				if (value[num - 1] == '/')
				{
					num--;
				}
			}
			return string.Compare(value, 0, urlMembershipCondition.Url, 0, num, true, CultureInfo.InvariantCulture) == 0;
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x0009D126 File Offset: 0x0009B326
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x0009D130 File Offset: 0x0009B330
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			MembershipConditionHelper.CheckSecurityElement(e, "e", this.version, this.version);
			string text = e.Attribute("Url");
			if (text != null)
			{
				this.CheckUrl(text);
				this.url = new Url(text);
			}
			else
			{
				this.url = null;
			}
			this.userUrl = text;
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x0009D187 File Offset: 0x0009B387
		public override int GetHashCode()
		{
			return this.url.GetHashCode();
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x0009D194 File Offset: 0x0009B394
		public override string ToString()
		{
			return "Url - " + this.Url;
		}

		// Token: 0x06002B7D RID: 11133 RVA: 0x0009D1A6 File Offset: 0x0009B3A6
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x06002B7E RID: 11134 RVA: 0x0009D1AF File Offset: 0x0009B3AF
		public SecurityElement ToXml(PolicyLevel level)
		{
			SecurityElement securityElement = MembershipConditionHelper.Element(typeof(UrlMembershipCondition), this.version);
			securityElement.AddAttribute("Url", this.userUrl);
			return securityElement;
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x0009D1D8 File Offset: 0x0009B3D8
		internal void CheckUrl(string url)
		{
			if (new Uri((url.IndexOf(Uri.SchemeDelimiter) < 0) ? ("file://" + url) : url, false, false).Host.IndexOf('*') >= 1)
			{
				throw new ArgumentException(Locale.GetText("Invalid * character in url"), "name");
			}
		}

		// Token: 0x04001FC7 RID: 8135
		private readonly int version = 1;

		// Token: 0x04001FC8 RID: 8136
		private Url url;

		// Token: 0x04001FC9 RID: 8137
		private string userUrl;
	}
}
