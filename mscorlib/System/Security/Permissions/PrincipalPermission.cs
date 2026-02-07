using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;

namespace System.Security.Permissions
{
	// Token: 0x0200044C RID: 1100
	[ComVisible(true)]
	[Serializable]
	public sealed class PrincipalPermission : IPermission, ISecurityEncodable, IUnrestrictedPermission, IBuiltInPermission
	{
		// Token: 0x06002C9B RID: 11419 RVA: 0x0009FF24 File Offset: 0x0009E124
		public PrincipalPermission(PermissionState state)
		{
			this.principals = new ArrayList();
			if (CodeAccessPermission.CheckPermissionState(state, true) == PermissionState.Unrestricted)
			{
				PrincipalPermission.PrincipalInfo value = new PrincipalPermission.PrincipalInfo(null, null, true);
				this.principals.Add(value);
			}
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x0009FF62 File Offset: 0x0009E162
		public PrincipalPermission(string name, string role) : this(name, role, true)
		{
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x0009FF70 File Offset: 0x0009E170
		public PrincipalPermission(string name, string role, bool isAuthenticated)
		{
			this.principals = new ArrayList();
			PrincipalPermission.PrincipalInfo value = new PrincipalPermission.PrincipalInfo(name, role, isAuthenticated);
			this.principals.Add(value);
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x0009FFA4 File Offset: 0x0009E1A4
		internal PrincipalPermission(ArrayList principals)
		{
			this.principals = (ArrayList)principals.Clone();
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x0009FFBD File Offset: 0x0009E1BD
		public IPermission Copy()
		{
			return new PrincipalPermission(this.principals);
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x0009FFCC File Offset: 0x0009E1CC
		[SecuritySafeCritical]
		public void Demand()
		{
			IPrincipal currentPrincipal = Thread.CurrentPrincipal;
			if (currentPrincipal == null)
			{
				throw new SecurityException("no Principal");
			}
			if (this.principals.Count > 0)
			{
				bool flag = false;
				foreach (object obj in this.principals)
				{
					PrincipalPermission.PrincipalInfo principalInfo = (PrincipalPermission.PrincipalInfo)obj;
					if ((principalInfo.Name == null || principalInfo.Name == currentPrincipal.Identity.Name) && (principalInfo.Role == null || currentPrincipal.IsInRole(principalInfo.Role)) && ((principalInfo.IsAuthenticated && currentPrincipal.Identity.IsAuthenticated) || !principalInfo.IsAuthenticated))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					throw new SecurityException("Demand for principal refused.");
				}
			}
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x000A00B0 File Offset: 0x0009E2B0
		public void FromXml(SecurityElement elem)
		{
			this.CheckSecurityElement(elem, "elem", 1, 1);
			this.principals.Clear();
			if (elem.Children != null)
			{
				foreach (object obj in elem.Children)
				{
					SecurityElement securityElement = (SecurityElement)obj;
					if (securityElement.Tag != "Identity")
					{
						throw new ArgumentException("not IPermission/Identity");
					}
					string name = securityElement.Attribute("ID");
					string role = securityElement.Attribute("Role");
					string text = securityElement.Attribute("Authenticated");
					bool isAuthenticated = false;
					if (text != null)
					{
						try
						{
							isAuthenticated = bool.Parse(text);
						}
						catch
						{
						}
					}
					PrincipalPermission.PrincipalInfo value = new PrincipalPermission.PrincipalInfo(name, role, isAuthenticated);
					this.principals.Add(value);
				}
			}
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x000A01A4 File Offset: 0x0009E3A4
		public IPermission Intersect(IPermission target)
		{
			PrincipalPermission principalPermission = this.Cast(target);
			if (principalPermission == null)
			{
				return null;
			}
			if (this.IsUnrestricted())
			{
				return principalPermission.Copy();
			}
			if (principalPermission.IsUnrestricted())
			{
				return this.Copy();
			}
			PrincipalPermission principalPermission2 = new PrincipalPermission(PermissionState.None);
			foreach (object obj in this.principals)
			{
				PrincipalPermission.PrincipalInfo principalInfo = (PrincipalPermission.PrincipalInfo)obj;
				foreach (object obj2 in principalPermission.principals)
				{
					PrincipalPermission.PrincipalInfo principalInfo2 = (PrincipalPermission.PrincipalInfo)obj2;
					if (principalInfo.IsAuthenticated == principalInfo2.IsAuthenticated)
					{
						string text = null;
						if (principalInfo.Name == principalInfo2.Name || principalInfo2.Name == null)
						{
							text = principalInfo.Name;
						}
						else if (principalInfo.Name == null)
						{
							text = principalInfo2.Name;
						}
						string text2 = null;
						if (principalInfo.Role == principalInfo2.Role || principalInfo2.Role == null)
						{
							text2 = principalInfo.Role;
						}
						else if (principalInfo.Role == null)
						{
							text2 = principalInfo2.Role;
						}
						if (text != null || text2 != null)
						{
							PrincipalPermission.PrincipalInfo value = new PrincipalPermission.PrincipalInfo(text, text2, principalInfo.IsAuthenticated);
							principalPermission2.principals.Add(value);
						}
					}
				}
			}
			if (principalPermission2.principals.Count <= 0)
			{
				return null;
			}
			return principalPermission2;
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x000A0358 File Offset: 0x0009E558
		public bool IsSubsetOf(IPermission target)
		{
			PrincipalPermission principalPermission = this.Cast(target);
			if (principalPermission == null)
			{
				return this.IsEmpty();
			}
			if (this.IsUnrestricted())
			{
				return principalPermission.IsUnrestricted();
			}
			if (principalPermission.IsUnrestricted())
			{
				return true;
			}
			foreach (object obj in this.principals)
			{
				PrincipalPermission.PrincipalInfo principalInfo = (PrincipalPermission.PrincipalInfo)obj;
				bool flag = false;
				foreach (object obj2 in principalPermission.principals)
				{
					PrincipalPermission.PrincipalInfo principalInfo2 = (PrincipalPermission.PrincipalInfo)obj2;
					if ((principalInfo.Name == principalInfo2.Name || principalInfo2.Name == null) && (principalInfo.Role == principalInfo2.Role || principalInfo2.Role == null) && principalInfo.IsAuthenticated == principalInfo2.IsAuthenticated)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x000A0480 File Offset: 0x0009E680
		public bool IsUnrestricted()
		{
			foreach (object obj in this.principals)
			{
				PrincipalPermission.PrincipalInfo principalInfo = (PrincipalPermission.PrincipalInfo)obj;
				if (principalInfo.Name == null && principalInfo.Role == null && principalInfo.IsAuthenticated)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x000A04F4 File Offset: 0x0009E6F4
		public override string ToString()
		{
			return this.ToXml().ToString();
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x000A0504 File Offset: 0x0009E704
		public SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("Permission");
			Type type = base.GetType();
			securityElement.AddAttribute("class", type.FullName + ", " + type.Assembly.ToString().Replace('"', '\''));
			securityElement.AddAttribute("version", 1.ToString());
			foreach (object obj in this.principals)
			{
				PrincipalPermission.PrincipalInfo principalInfo = (PrincipalPermission.PrincipalInfo)obj;
				SecurityElement securityElement2 = new SecurityElement("Identity");
				if (principalInfo.Name != null)
				{
					securityElement2.AddAttribute("ID", principalInfo.Name);
				}
				if (principalInfo.Role != null)
				{
					securityElement2.AddAttribute("Role", principalInfo.Role);
				}
				if (principalInfo.IsAuthenticated)
				{
					securityElement2.AddAttribute("Authenticated", "true");
				}
				securityElement.AddChild(securityElement2);
			}
			return securityElement;
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x000A0618 File Offset: 0x0009E818
		public IPermission Union(IPermission other)
		{
			PrincipalPermission principalPermission = this.Cast(other);
			if (principalPermission == null)
			{
				return this.Copy();
			}
			if (this.IsUnrestricted() || principalPermission.IsUnrestricted())
			{
				return new PrincipalPermission(PermissionState.Unrestricted);
			}
			PrincipalPermission principalPermission2 = new PrincipalPermission(this.principals);
			foreach (object obj in principalPermission.principals)
			{
				PrincipalPermission.PrincipalInfo value = (PrincipalPermission.PrincipalInfo)obj;
				principalPermission2.principals.Add(value);
			}
			return principalPermission2;
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x000A06B0 File Offset: 0x0009E8B0
		[ComVisible(false)]
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			PrincipalPermission principalPermission = obj as PrincipalPermission;
			if (principalPermission == null)
			{
				return false;
			}
			if (this.principals.Count != principalPermission.principals.Count)
			{
				return false;
			}
			foreach (object obj2 in this.principals)
			{
				PrincipalPermission.PrincipalInfo principalInfo = (PrincipalPermission.PrincipalInfo)obj2;
				bool flag = false;
				foreach (object obj3 in principalPermission.principals)
				{
					PrincipalPermission.PrincipalInfo principalInfo2 = (PrincipalPermission.PrincipalInfo)obj3;
					if ((principalInfo.Name == principalInfo2.Name || principalInfo2.Name == null) && (principalInfo.Role == principalInfo2.Role || principalInfo2.Role == null) && principalInfo.IsAuthenticated == principalInfo2.IsAuthenticated)
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

		// Token: 0x06002CA9 RID: 11433 RVA: 0x000930F4 File Offset: 0x000912F4
		[ComVisible(false)]
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x00047F75 File Offset: 0x00046175
		int IBuiltInPermission.GetTokenIndex()
		{
			return 8;
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x000A07DC File Offset: 0x0009E9DC
		private PrincipalPermission Cast(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			PrincipalPermission principalPermission = target as PrincipalPermission;
			if (principalPermission == null)
			{
				CodeAccessPermission.ThrowInvalidPermission(target, typeof(PrincipalPermission));
			}
			return principalPermission;
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x000A07FC File Offset: 0x0009E9FC
		private bool IsEmpty()
		{
			return this.principals.Count == 0;
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x000A080C File Offset: 0x0009EA0C
		internal int CheckSecurityElement(SecurityElement se, string parameterName, int minimumVersion, int maximumVersion)
		{
			if (se == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			if (se.Tag != "Permission")
			{
				throw new ArgumentException(string.Format(Locale.GetText("Invalid tag {0}"), se.Tag), parameterName);
			}
			int num = minimumVersion;
			string text = se.Attribute("version");
			if (text != null)
			{
				try
				{
					num = int.Parse(text);
				}
				catch (Exception innerException)
				{
					throw new ArgumentException(string.Format(Locale.GetText("Couldn't parse version from '{0}'."), text), parameterName, innerException);
				}
			}
			if (num < minimumVersion || num > maximumVersion)
			{
				throw new ArgumentException(string.Format(Locale.GetText("Unknown version '{0}', expected versions between ['{1}','{2}']."), num, minimumVersion, maximumVersion), parameterName);
			}
			return num;
		}

		// Token: 0x0400206C RID: 8300
		private const int version = 1;

		// Token: 0x0400206D RID: 8301
		private ArrayList principals;

		// Token: 0x0200044D RID: 1101
		internal class PrincipalInfo
		{
			// Token: 0x06002CAE RID: 11438 RVA: 0x000A08C8 File Offset: 0x0009EAC8
			public PrincipalInfo(string name, string role, bool isAuthenticated)
			{
				this._name = name;
				this._role = role;
				this._isAuthenticated = isAuthenticated;
			}

			// Token: 0x170005B1 RID: 1457
			// (get) Token: 0x06002CAF RID: 11439 RVA: 0x000A08E5 File Offset: 0x0009EAE5
			public string Name
			{
				get
				{
					return this._name;
				}
			}

			// Token: 0x170005B2 RID: 1458
			// (get) Token: 0x06002CB0 RID: 11440 RVA: 0x000A08ED File Offset: 0x0009EAED
			public string Role
			{
				get
				{
					return this._role;
				}
			}

			// Token: 0x170005B3 RID: 1459
			// (get) Token: 0x06002CB1 RID: 11441 RVA: 0x000A08F5 File Offset: 0x0009EAF5
			public bool IsAuthenticated
			{
				get
				{
					return this._isAuthenticated;
				}
			}

			// Token: 0x0400206E RID: 8302
			private string _name;

			// Token: 0x0400206F RID: 8303
			private string _role;

			// Token: 0x04002070 RID: 8304
			private bool _isAuthenticated;
		}
	}
}
