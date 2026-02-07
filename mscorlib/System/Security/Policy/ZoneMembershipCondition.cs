using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x02000428 RID: 1064
	[ComVisible(true)]
	[Serializable]
	public sealed class ZoneMembershipCondition : IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable, IConstantMembershipCondition
	{
		// Token: 0x06002B8B RID: 11147 RVA: 0x0009D401 File Offset: 0x0009B601
		internal ZoneMembershipCondition()
		{
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x0009D410 File Offset: 0x0009B610
		public ZoneMembershipCondition(SecurityZone zone)
		{
			this.SecurityZone = zone;
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06002B8D RID: 11149 RVA: 0x0009D426 File Offset: 0x0009B626
		// (set) Token: 0x06002B8E RID: 11150 RVA: 0x0009D430 File Offset: 0x0009B630
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
					throw new ArgumentException(Locale.GetText("invalid zone"));
				}
				if (value == SecurityZone.NoZone)
				{
					throw new ArgumentException(Locale.GetText("NoZone isn't valid for membership condition"));
				}
				this.zone = value;
			}
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x0009D480 File Offset: 0x0009B680
		public bool Check(Evidence evidence)
		{
			if (evidence == null)
			{
				return false;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				object obj = hostEnumerator.Current;
				Zone zone = obj as Zone;
				if (zone != null && zone.SecurityZone == this.zone)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x0009D4C3 File Offset: 0x0009B6C3
		public IMembershipCondition Copy()
		{
			return new ZoneMembershipCondition(this.zone);
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x0009D4D0 File Offset: 0x0009B6D0
		public override bool Equals(object o)
		{
			ZoneMembershipCondition zoneMembershipCondition = o as ZoneMembershipCondition;
			return zoneMembershipCondition != null && zoneMembershipCondition.SecurityZone == this.zone;
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x0009D4F7 File Offset: 0x0009B6F7
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x0009D504 File Offset: 0x0009B704
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			MembershipConditionHelper.CheckSecurityElement(e, "e", this.version, this.version);
			string text = e.Attribute("Zone");
			if (text != null)
			{
				this.zone = (SecurityZone)Enum.Parse(typeof(SecurityZone), text);
			}
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x0009D553 File Offset: 0x0009B753
		public override int GetHashCode()
		{
			return this.zone.GetHashCode();
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x0009D566 File Offset: 0x0009B766
		public override string ToString()
		{
			return "Zone - " + this.zone.ToString();
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x0009D583 File Offset: 0x0009B783
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x0009D58C File Offset: 0x0009B78C
		public SecurityElement ToXml(PolicyLevel level)
		{
			SecurityElement securityElement = MembershipConditionHelper.Element(typeof(ZoneMembershipCondition), this.version);
			securityElement.AddAttribute("Zone", this.zone.ToString());
			return securityElement;
		}

		// Token: 0x04001FCB RID: 8139
		private readonly int version = 1;

		// Token: 0x04001FCC RID: 8140
		private SecurityZone zone;
	}
}
