using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Policy
{
	// Token: 0x02000419 RID: 1049
	[ComVisible(true)]
	[Serializable]
	public sealed class NetCodeGroup : CodeGroup
	{
		// Token: 0x06002ACA RID: 10954 RVA: 0x0009A862 File Offset: 0x00098A62
		public NetCodeGroup(IMembershipCondition membershipCondition) : base(membershipCondition, null)
		{
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x0009A877 File Offset: 0x00098A77
		internal NetCodeGroup(SecurityElement e, PolicyLevel level) : base(e, level)
		{
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06002ACC RID: 10956 RVA: 0x0000AF5E File Offset: 0x0000915E
		public override string AttributeString
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06002ACD RID: 10957 RVA: 0x00099C94 File Offset: 0x00097E94
		public override string MergeLogic
		{
			get
			{
				return "Union";
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06002ACE RID: 10958 RVA: 0x0009A88C File Offset: 0x00098A8C
		public override string PermissionSetName
		{
			get
			{
				return "Same site Web";
			}
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x0009A894 File Offset: 0x00098A94
		[MonoTODO("(2.0) missing validations")]
		public void AddConnectAccess(string originScheme, CodeConnectAccess connectAccess)
		{
			if (originScheme == null)
			{
				throw new ArgumentException("originScheme");
			}
			if (originScheme == NetCodeGroup.AbsentOriginScheme && connectAccess.Scheme == CodeConnectAccess.OriginScheme)
			{
				throw new ArgumentOutOfRangeException("connectAccess", Locale.GetText("Schema == CodeConnectAccess.OriginScheme"));
			}
			if (this._rules.ContainsKey(originScheme))
			{
				if (connectAccess != null)
				{
					CodeConnectAccess[] array = (CodeConnectAccess[])this._rules[originScheme];
					CodeConnectAccess[] array2 = new CodeConnectAccess[array.Length + 1];
					Array.Copy(array, 0, array2, 0, array.Length);
					array2[array.Length] = connectAccess;
					this._rules[originScheme] = array2;
					return;
				}
			}
			else
			{
				CodeConnectAccess[] value = new CodeConnectAccess[]
				{
					connectAccess
				};
				this._rules.Add(originScheme, value);
			}
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x0009A94C File Offset: 0x00098B4C
		public override CodeGroup Copy()
		{
			NetCodeGroup netCodeGroup = new NetCodeGroup(base.MembershipCondition);
			netCodeGroup.Name = base.Name;
			netCodeGroup.Description = base.Description;
			netCodeGroup.PolicyStatement = base.PolicyStatement;
			foreach (object obj in base.Children)
			{
				CodeGroup codeGroup = (CodeGroup)obj;
				netCodeGroup.AddChild(codeGroup.Copy());
			}
			return netCodeGroup;
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x0009A9DC File Offset: 0x00098BDC
		private bool Equals(CodeConnectAccess[] rules1, CodeConnectAccess[] rules2)
		{
			for (int i = 0; i < rules1.Length; i++)
			{
				bool flag = false;
				for (int j = 0; j < rules2.Length; j++)
				{
					if (rules1[i].Equals(rules2[j]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x0009AA20 File Offset: 0x00098C20
		public override bool Equals(object o)
		{
			if (!base.Equals(o))
			{
				return false;
			}
			NetCodeGroup netCodeGroup = o as NetCodeGroup;
			if (netCodeGroup == null)
			{
				return false;
			}
			foreach (object obj in this._rules)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				CodeConnectAccess[] array = (CodeConnectAccess[])netCodeGroup._rules[dictionaryEntry.Key];
				bool flag;
				if (array != null)
				{
					flag = this.Equals((CodeConnectAccess[])dictionaryEntry.Value, array);
				}
				else
				{
					flag = (dictionaryEntry.Value == null);
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x0009AADC File Offset: 0x00098CDC
		public DictionaryEntry[] GetConnectAccessRules()
		{
			DictionaryEntry[] array = new DictionaryEntry[this._rules.Count];
			this._rules.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06002AD4 RID: 10964 RVA: 0x0009AB08 File Offset: 0x00098D08
		public override int GetHashCode()
		{
			if (this._hashcode == 0)
			{
				this._hashcode = base.GetHashCode();
				foreach (object obj in this._rules)
				{
					CodeConnectAccess[] array = (CodeConnectAccess[])((DictionaryEntry)obj).Value;
					if (array != null)
					{
						foreach (CodeConnectAccess codeConnectAccess in array)
						{
							this._hashcode ^= codeConnectAccess.GetHashCode();
						}
					}
				}
			}
			return this._hashcode;
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x0009ABB8 File Offset: 0x00098DB8
		public override PolicyStatement Resolve(Evidence evidence)
		{
			if (evidence == null)
			{
				throw new ArgumentNullException("evidence");
			}
			if (!base.MembershipCondition.Check(evidence))
			{
				return null;
			}
			PermissionSet permissionSet = null;
			if (base.PolicyStatement == null)
			{
				permissionSet = new PermissionSet(PermissionState.None);
			}
			else
			{
				permissionSet = base.PolicyStatement.PermissionSet.Copy();
			}
			if (base.Children.Count > 0)
			{
				foreach (object obj in base.Children)
				{
					PolicyStatement policyStatement = ((CodeGroup)obj).Resolve(evidence);
					if (policyStatement != null)
					{
						permissionSet = permissionSet.Union(policyStatement.PermissionSet);
					}
				}
			}
			PolicyStatement policyStatement2 = base.PolicyStatement.Copy();
			policyStatement2.PermissionSet = permissionSet;
			return policyStatement2;
		}

		// Token: 0x06002AD6 RID: 10966 RVA: 0x0009AC84 File Offset: 0x00098E84
		public void ResetConnectAccess()
		{
			this._rules.Clear();
		}

		// Token: 0x06002AD7 RID: 10967 RVA: 0x0009AC94 File Offset: 0x00098E94
		public override CodeGroup ResolveMatchingCodeGroups(Evidence evidence)
		{
			if (evidence == null)
			{
				throw new ArgumentNullException("evidence");
			}
			CodeGroup codeGroup = null;
			if (base.MembershipCondition.Check(evidence))
			{
				codeGroup = this.Copy();
				foreach (object obj in base.Children)
				{
					CodeGroup codeGroup2 = ((CodeGroup)obj).ResolveMatchingCodeGroups(evidence);
					if (codeGroup2 != null)
					{
						codeGroup.AddChild(codeGroup2);
					}
				}
			}
			return codeGroup;
		}

		// Token: 0x06002AD8 RID: 10968 RVA: 0x0009AD1C File Offset: 0x00098F1C
		[MonoTODO("(2.0) Add new stuff (CodeConnectAccess) into XML")]
		protected override void CreateXml(SecurityElement element, PolicyLevel level)
		{
			base.CreateXml(element, level);
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x0009AD26 File Offset: 0x00098F26
		[MonoTODO("(2.0) Parse new stuff (CodeConnectAccess) from XML")]
		protected override void ParseXml(SecurityElement e, PolicyLevel level)
		{
			base.ParseXml(e, level);
		}

		// Token: 0x04001FA1 RID: 8097
		public static readonly string AbsentOriginScheme = string.Empty;

		// Token: 0x04001FA2 RID: 8098
		public static readonly string AnyOtherOriginScheme = "*";

		// Token: 0x04001FA3 RID: 8099
		private Hashtable _rules = new Hashtable();

		// Token: 0x04001FA4 RID: 8100
		private int _hashcode;
	}
}
