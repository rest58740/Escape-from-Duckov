using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Security.Permissions
{
	// Token: 0x02000433 RID: 1075
	[ComVisible(true)]
	[Serializable]
	public sealed class EnvironmentPermission : CodeAccessPermission, IUnrestrictedPermission, IBuiltInPermission
	{
		// Token: 0x06002B9A RID: 11162 RVA: 0x0009D5C8 File Offset: 0x0009B7C8
		public EnvironmentPermission(PermissionState state)
		{
			this._state = CodeAccessPermission.CheckPermissionState(state, true);
			this.readList = new ArrayList();
			this.writeList = new ArrayList();
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x0009D5F3 File Offset: 0x0009B7F3
		public EnvironmentPermission(EnvironmentPermissionAccess flag, string pathList)
		{
			this.readList = new ArrayList();
			this.writeList = new ArrayList();
			this.SetPathList(flag, pathList);
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x0009D61C File Offset: 0x0009B81C
		public void AddPathList(EnvironmentPermissionAccess flag, string pathList)
		{
			if (pathList == null)
			{
				throw new ArgumentNullException("pathList");
			}
			switch (flag)
			{
			case EnvironmentPermissionAccess.NoAccess:
				break;
			case EnvironmentPermissionAccess.Read:
				foreach (string text in pathList.Split(';', StringSplitOptions.None))
				{
					if (!this.readList.Contains(text))
					{
						this.readList.Add(text);
					}
				}
				return;
			case EnvironmentPermissionAccess.Write:
				foreach (string text2 in pathList.Split(';', StringSplitOptions.None))
				{
					if (!this.writeList.Contains(text2))
					{
						this.writeList.Add(text2);
					}
				}
				return;
			case EnvironmentPermissionAccess.AllAccess:
				foreach (string text3 in pathList.Split(';', StringSplitOptions.None))
				{
					if (!this.readList.Contains(text3))
					{
						this.readList.Add(text3);
					}
					if (!this.writeList.Contains(text3))
					{
						this.writeList.Add(text3);
					}
				}
				return;
			default:
				this.ThrowInvalidFlag(flag, false);
				break;
			}
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x0009D720 File Offset: 0x0009B920
		public override IPermission Copy()
		{
			EnvironmentPermission environmentPermission = new EnvironmentPermission(this._state);
			string pathList = this.GetPathList(EnvironmentPermissionAccess.Read);
			if (pathList != null)
			{
				environmentPermission.SetPathList(EnvironmentPermissionAccess.Read, pathList);
			}
			pathList = this.GetPathList(EnvironmentPermissionAccess.Write);
			if (pathList != null)
			{
				environmentPermission.SetPathList(EnvironmentPermissionAccess.Write, pathList);
			}
			return environmentPermission;
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x0009D760 File Offset: 0x0009B960
		public override void FromXml(SecurityElement esd)
		{
			CodeAccessPermission.CheckSecurityElement(esd, "esd", 1, 1);
			if (CodeAccessPermission.IsUnrestricted(esd))
			{
				this._state = PermissionState.Unrestricted;
			}
			string text = esd.Attribute("Read");
			if (text != null && text.Length > 0)
			{
				this.SetPathList(EnvironmentPermissionAccess.Read, text);
			}
			string text2 = esd.Attribute("Write");
			if (text2 != null && text2.Length > 0)
			{
				this.SetPathList(EnvironmentPermissionAccess.Write, text2);
			}
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x0009D7CC File Offset: 0x0009B9CC
		public string GetPathList(EnvironmentPermissionAccess flag)
		{
			switch (flag)
			{
			case EnvironmentPermissionAccess.NoAccess:
			case EnvironmentPermissionAccess.AllAccess:
				this.ThrowInvalidFlag(flag, true);
				break;
			case EnvironmentPermissionAccess.Read:
				return this.GetPathList(this.readList);
			case EnvironmentPermissionAccess.Write:
				return this.GetPathList(this.writeList);
			default:
				this.ThrowInvalidFlag(flag, false);
				break;
			}
			return null;
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x0009D820 File Offset: 0x0009BA20
		[SecuritySafeCritical]
		public override IPermission Intersect(IPermission target)
		{
			EnvironmentPermission environmentPermission = this.Cast(target);
			if (environmentPermission == null)
			{
				return null;
			}
			if (this.IsUnrestricted())
			{
				return environmentPermission.Copy();
			}
			if (environmentPermission.IsUnrestricted())
			{
				return this.Copy();
			}
			int num = 0;
			EnvironmentPermission environmentPermission2 = new EnvironmentPermission(PermissionState.None);
			string pathList = environmentPermission.GetPathList(EnvironmentPermissionAccess.Read);
			if (pathList != null)
			{
				foreach (string text in pathList.Split(';', StringSplitOptions.None))
				{
					if (this.readList.Contains(text))
					{
						environmentPermission2.AddPathList(EnvironmentPermissionAccess.Read, text);
						num++;
					}
				}
			}
			string pathList2 = environmentPermission.GetPathList(EnvironmentPermissionAccess.Write);
			if (pathList2 != null)
			{
				foreach (string text2 in pathList2.Split(';', StringSplitOptions.None))
				{
					if (this.writeList.Contains(text2))
					{
						environmentPermission2.AddPathList(EnvironmentPermissionAccess.Write, text2);
						num++;
					}
				}
			}
			if (num <= 0)
			{
				return null;
			}
			return environmentPermission2;
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x0009D904 File Offset: 0x0009BB04
		[SecuritySafeCritical]
		public override bool IsSubsetOf(IPermission target)
		{
			EnvironmentPermission environmentPermission = this.Cast(target);
			if (environmentPermission == null)
			{
				return false;
			}
			if (this.IsUnrestricted())
			{
				return environmentPermission.IsUnrestricted();
			}
			if (environmentPermission.IsUnrestricted())
			{
				return true;
			}
			foreach (object obj in this.readList)
			{
				string item = (string)obj;
				if (!environmentPermission.readList.Contains(item))
				{
					return false;
				}
			}
			foreach (object obj2 in this.writeList)
			{
				string item2 = (string)obj2;
				if (!environmentPermission.writeList.Contains(item2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x0009D9EC File Offset: 0x0009BBEC
		public bool IsUnrestricted()
		{
			return this._state == PermissionState.Unrestricted;
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x0009D9F8 File Offset: 0x0009BBF8
		public void SetPathList(EnvironmentPermissionAccess flag, string pathList)
		{
			if (pathList == null)
			{
				throw new ArgumentNullException("pathList");
			}
			switch (flag)
			{
			case EnvironmentPermissionAccess.NoAccess:
				break;
			case EnvironmentPermissionAccess.Read:
				this.readList.Clear();
				foreach (string value in pathList.Split(';', StringSplitOptions.None))
				{
					this.readList.Add(value);
				}
				return;
			case EnvironmentPermissionAccess.Write:
				this.writeList.Clear();
				foreach (string value2 in pathList.Split(';', StringSplitOptions.None))
				{
					this.writeList.Add(value2);
				}
				return;
			case EnvironmentPermissionAccess.AllAccess:
				this.readList.Clear();
				this.writeList.Clear();
				foreach (string value3 in pathList.Split(';', StringSplitOptions.None))
				{
					this.readList.Add(value3);
					this.writeList.Add(value3);
				}
				return;
			default:
				this.ThrowInvalidFlag(flag, false);
				break;
			}
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x0009DAF0 File Offset: 0x0009BCF0
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = base.Element(1);
			if (this._state == PermissionState.Unrestricted)
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			else
			{
				string pathList = this.GetPathList(EnvironmentPermissionAccess.Read);
				if (pathList != null)
				{
					securityElement.AddAttribute("Read", pathList);
				}
				pathList = this.GetPathList(EnvironmentPermissionAccess.Write);
				if (pathList != null)
				{
					securityElement.AddAttribute("Write", pathList);
				}
			}
			return securityElement;
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x0009DB50 File Offset: 0x0009BD50
		[SecuritySafeCritical]
		public override IPermission Union(IPermission other)
		{
			EnvironmentPermission environmentPermission = this.Cast(other);
			if (environmentPermission == null)
			{
				return this.Copy();
			}
			if (this.IsUnrestricted() || environmentPermission.IsUnrestricted())
			{
				return new EnvironmentPermission(PermissionState.Unrestricted);
			}
			if (this.IsEmpty() && environmentPermission.IsEmpty())
			{
				return null;
			}
			EnvironmentPermission environmentPermission2 = (EnvironmentPermission)this.Copy();
			string pathList = environmentPermission.GetPathList(EnvironmentPermissionAccess.Read);
			if (pathList != null)
			{
				environmentPermission2.AddPathList(EnvironmentPermissionAccess.Read, pathList);
			}
			pathList = environmentPermission.GetPathList(EnvironmentPermissionAccess.Write);
			if (pathList != null)
			{
				environmentPermission2.AddPathList(EnvironmentPermissionAccess.Write, pathList);
			}
			return environmentPermission2;
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		int IBuiltInPermission.GetTokenIndex()
		{
			return 0;
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x0009DBCB File Offset: 0x0009BDCB
		private bool IsEmpty()
		{
			return this._state == PermissionState.None && this.readList.Count == 0 && this.writeList.Count == 0;
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x0009DBF2 File Offset: 0x0009BDF2
		private EnvironmentPermission Cast(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			EnvironmentPermission environmentPermission = target as EnvironmentPermission;
			if (environmentPermission == null)
			{
				CodeAccessPermission.ThrowInvalidPermission(target, typeof(EnvironmentPermission));
			}
			return environmentPermission;
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x0009DC14 File Offset: 0x0009BE14
		internal void ThrowInvalidFlag(EnvironmentPermissionAccess flag, bool context)
		{
			string text;
			if (context)
			{
				text = Locale.GetText("Unknown flag '{0}'.");
			}
			else
			{
				text = Locale.GetText("Invalid flag '{0}' in this context.");
			}
			throw new ArgumentException(string.Format(text, flag), "flag");
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x0009DC54 File Offset: 0x0009BE54
		private string GetPathList(ArrayList list)
		{
			if (this.IsUnrestricted())
			{
				return string.Empty;
			}
			if (list.Count == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in list)
			{
				string value = (string)obj;
				stringBuilder.Append(value);
				stringBuilder.Append(";");
			}
			string text = stringBuilder.ToString();
			int length = text.Length;
			if (length > 0)
			{
				return text.Substring(0, length - 1);
			}
			return string.Empty;
		}

		// Token: 0x04002004 RID: 8196
		private const int version = 1;

		// Token: 0x04002005 RID: 8197
		private PermissionState _state;

		// Token: 0x04002006 RID: 8198
		private ArrayList readList;

		// Token: 0x04002007 RID: 8199
		private ArrayList writeList;
	}
}
