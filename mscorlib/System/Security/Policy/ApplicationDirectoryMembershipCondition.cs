using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using Mono.Security;

namespace System.Security.Policy
{
	// Token: 0x02000400 RID: 1024
	[ComVisible(true)]
	[Serializable]
	public sealed class ApplicationDirectoryMembershipCondition : IConstantMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x060029DB RID: 10715 RVA: 0x00098004 File Offset: 0x00096204
		public bool Check(Evidence evidence)
		{
			if (evidence == null)
			{
				return false;
			}
			string codeBase = Assembly.GetCallingAssembly().CodeBase;
			Uri uri = new Uri(codeBase);
			Url url = new Url(codeBase);
			bool flag = false;
			bool flag2 = false;
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				object obj = hostEnumerator.Current;
				if (!flag && obj is ApplicationDirectory)
				{
					string directory = (obj as ApplicationDirectory).Directory;
					flag = (string.Compare(directory, 0, uri.ToString(), 0, directory.Length, true, CultureInfo.InvariantCulture) == 0);
				}
				else if (!flag2 && obj is Url)
				{
					flag2 = url.Equals(obj);
				}
				if (flag && flag2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060029DC RID: 10716 RVA: 0x000980A6 File Offset: 0x000962A6
		public IMembershipCondition Copy()
		{
			return new ApplicationDirectoryMembershipCondition();
		}

		// Token: 0x060029DD RID: 10717 RVA: 0x000980AD File Offset: 0x000962AD
		public override bool Equals(object o)
		{
			return o is ApplicationDirectoryMembershipCondition;
		}

		// Token: 0x060029DE RID: 10718 RVA: 0x000980B8 File Offset: 0x000962B8
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x060029DF RID: 10719 RVA: 0x000980C2 File Offset: 0x000962C2
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			MembershipConditionHelper.CheckSecurityElement(e, "e", this.version, this.version);
		}

		// Token: 0x060029E0 RID: 10720 RVA: 0x000980DC File Offset: 0x000962DC
		public override int GetHashCode()
		{
			return typeof(ApplicationDirectoryMembershipCondition).GetHashCode();
		}

		// Token: 0x060029E1 RID: 10721 RVA: 0x000980ED File Offset: 0x000962ED
		public override string ToString()
		{
			return "ApplicationDirectory";
		}

		// Token: 0x060029E2 RID: 10722 RVA: 0x000980F4 File Offset: 0x000962F4
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x060029E3 RID: 10723 RVA: 0x000980FD File Offset: 0x000962FD
		public SecurityElement ToXml(PolicyLevel level)
		{
			return MembershipConditionHelper.Element(typeof(ApplicationDirectoryMembershipCondition), this.version);
		}

		// Token: 0x04001F54 RID: 8020
		private readonly int version = 1;
	}
}
