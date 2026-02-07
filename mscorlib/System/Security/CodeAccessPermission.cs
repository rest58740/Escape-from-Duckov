using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security
{
	// Token: 0x020003DD RID: 989
	[MonoTODO("CAS support is experimental (and unsupported).")]
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, ControlEvidence = true, ControlPolicy = true)]
	[Serializable]
	public abstract class CodeAccessPermission : IPermission, ISecurityEncodable, IStackWalk
	{
		// Token: 0x0600288C RID: 10380 RVA: 0x00093084 File Offset: 0x00091284
		[SecuritySafeCritical]
		[MonoTODO("CAS support is experimental (and unsupported). Imperative mode is not implemented.")]
		public void Assert()
		{
			new PermissionSet(this).Assert();
		}

		// Token: 0x0600288D RID: 10381
		public abstract IPermission Copy();

		// Token: 0x0600288E RID: 10382 RVA: 0x00093091 File Offset: 0x00091291
		[SecuritySafeCritical]
		public void Demand()
		{
			if (!SecurityManager.SecurityEnabled)
			{
				return;
			}
			new PermissionSet(this).CasOnlyDemand(3);
		}

		// Token: 0x0600288F RID: 10383 RVA: 0x000930A7 File Offset: 0x000912A7
		[Obsolete("Deny is obsolete and will be removed in a future release of the .NET Framework. See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
		[MonoTODO("CAS support is experimental (and unsupported). Imperative mode is not implemented.")]
		[SecuritySafeCritical]
		public void Deny()
		{
			new PermissionSet(this).Deny();
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x000930B4 File Offset: 0x000912B4
		[ComVisible(false)]
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != base.GetType())
			{
				return false;
			}
			CodeAccessPermission codeAccessPermission = obj as CodeAccessPermission;
			return this.IsSubsetOf(codeAccessPermission) && codeAccessPermission.IsSubsetOf(this);
		}

		// Token: 0x06002891 RID: 10385
		public abstract void FromXml(SecurityElement elem);

		// Token: 0x06002892 RID: 10386 RVA: 0x000930F4 File Offset: 0x000912F4
		[ComVisible(false)]
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06002893 RID: 10387
		public abstract IPermission Intersect(IPermission target);

		// Token: 0x06002894 RID: 10388
		public abstract bool IsSubsetOf(IPermission target);

		// Token: 0x06002895 RID: 10389 RVA: 0x000930FC File Offset: 0x000912FC
		public override string ToString()
		{
			return this.ToXml().ToString();
		}

		// Token: 0x06002896 RID: 10390
		public abstract SecurityElement ToXml();

		// Token: 0x06002897 RID: 10391 RVA: 0x00093109 File Offset: 0x00091309
		public virtual IPermission Union(IPermission other)
		{
			if (other != null)
			{
				throw new NotSupportedException();
			}
			return null;
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x00093115 File Offset: 0x00091315
		[SecuritySafeCritical]
		[MonoTODO("CAS support is experimental (and unsupported). Imperative mode is not implemented.")]
		public void PermitOnly()
		{
			new PermissionSet(this).PermitOnly();
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x00093122 File Offset: 0x00091322
		[MonoTODO("CAS support is experimental (and unsupported). Imperative mode is not implemented.")]
		public static void RevertAll()
		{
			if (!SecurityManager.SecurityEnabled)
			{
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x00093122 File Offset: 0x00091322
		[MonoTODO("CAS support is experimental (and unsupported). Imperative mode is not implemented.")]
		public static void RevertAssert()
		{
			if (!SecurityManager.SecurityEnabled)
			{
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x00093122 File Offset: 0x00091322
		[MonoTODO("CAS support is experimental (and unsupported). Imperative mode is not implemented.")]
		public static void RevertDeny()
		{
			if (!SecurityManager.SecurityEnabled)
			{
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x00093122 File Offset: 0x00091322
		[MonoTODO("CAS support is experimental (and unsupported). Imperative mode is not implemented.")]
		public static void RevertPermitOnly()
		{
			if (!SecurityManager.SecurityEnabled)
			{
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x00093134 File Offset: 0x00091334
		internal SecurityElement Element(int version)
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			Type type = base.GetType();
			securityElement.AddAttribute("class", type.FullName + ", " + type.Assembly.ToString().Replace('"', '\''));
			securityElement.AddAttribute("version", version.ToString());
			return securityElement;
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x00093193 File Offset: 0x00091393
		internal static PermissionState CheckPermissionState(PermissionState state, bool allowUnrestricted)
		{
			if (state != PermissionState.None && state != PermissionState.Unrestricted)
			{
				throw new ArgumentException(string.Format(Locale.GetText("Invalid enum {0}"), state), "state");
			}
			return state;
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x000931C0 File Offset: 0x000913C0
		internal static int CheckSecurityElement(SecurityElement se, string parameterName, int minimumVersion, int maximumVersion)
		{
			if (se == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			if (se.Tag != "IPermission")
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

		// Token: 0x060028A0 RID: 10400 RVA: 0x0009327C File Offset: 0x0009147C
		internal static bool IsUnrestricted(SecurityElement se)
		{
			string text = se.Attribute("Unrestricted");
			return text != null && string.Compare(text, bool.TrueString, true, CultureInfo.InvariantCulture) == 0;
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x000932AE File Offset: 0x000914AE
		internal static void ThrowInvalidPermission(IPermission target, Type expected)
		{
			throw new ArgumentException(string.Format(Locale.GetText("Invalid permission type '{0}', expected type '{1}'."), target.GetType(), expected), "target");
		}
	}
}
