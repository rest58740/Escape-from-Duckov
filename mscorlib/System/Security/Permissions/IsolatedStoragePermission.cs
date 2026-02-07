using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000443 RID: 1091
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, ControlEvidence = true, ControlPolicy = true)]
	[Serializable]
	public abstract class IsolatedStoragePermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x06002C3C RID: 11324 RVA: 0x0009F3E0 File Offset: 0x0009D5E0
		protected IsolatedStoragePermission(PermissionState state)
		{
			if (CodeAccessPermission.CheckPermissionState(state, true) == PermissionState.Unrestricted)
			{
				this.UsageAllowed = IsolatedStorageContainment.UnrestrictedIsolatedStorage;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06002C3D RID: 11325 RVA: 0x0009F3FD File Offset: 0x0009D5FD
		// (set) Token: 0x06002C3E RID: 11326 RVA: 0x0009F405 File Offset: 0x0009D605
		public long UserQuota
		{
			get
			{
				return this.m_userQuota;
			}
			set
			{
				this.m_userQuota = value;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06002C3F RID: 11327 RVA: 0x0009F40E File Offset: 0x0009D60E
		// (set) Token: 0x06002C40 RID: 11328 RVA: 0x0009F418 File Offset: 0x0009D618
		public IsolatedStorageContainment UsageAllowed
		{
			get
			{
				return this.m_allowed;
			}
			set
			{
				if (!Enum.IsDefined(typeof(IsolatedStorageContainment), value))
				{
					throw new ArgumentException(string.Format(Locale.GetText("Invalid enum {0}"), value), "IsolatedStorageContainment");
				}
				this.m_allowed = value;
				if (this.m_allowed == IsolatedStorageContainment.UnrestrictedIsolatedStorage)
				{
					this.m_userQuota = long.MaxValue;
					this.m_machineQuota = long.MaxValue;
					this.m_expirationDays = long.MaxValue;
					this.m_permanentData = true;
				}
			}
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x0009F4A4 File Offset: 0x0009D6A4
		public bool IsUnrestricted()
		{
			return IsolatedStorageContainment.UnrestrictedIsolatedStorage == this.m_allowed;
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x0009F4B4 File Offset: 0x0009D6B4
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = base.Element(1);
			if (this.m_allowed == IsolatedStorageContainment.UnrestrictedIsolatedStorage)
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			else
			{
				securityElement.AddAttribute("Allowed", this.m_allowed.ToString());
				if (this.m_userQuota > 0L)
				{
					securityElement.AddAttribute("UserQuota", this.m_userQuota.ToString());
				}
			}
			return securityElement;
		}

		// Token: 0x06002C43 RID: 11331 RVA: 0x0009F528 File Offset: 0x0009D728
		public override void FromXml(SecurityElement esd)
		{
			CodeAccessPermission.CheckSecurityElement(esd, "esd", 1, 1);
			this.m_userQuota = 0L;
			this.m_machineQuota = 0L;
			this.m_expirationDays = 0L;
			this.m_permanentData = false;
			this.m_allowed = IsolatedStorageContainment.None;
			if (CodeAccessPermission.IsUnrestricted(esd))
			{
				this.UsageAllowed = IsolatedStorageContainment.UnrestrictedIsolatedStorage;
				return;
			}
			string text = esd.Attribute("Allowed");
			if (text != null)
			{
				this.UsageAllowed = (IsolatedStorageContainment)Enum.Parse(typeof(IsolatedStorageContainment), text);
			}
			text = esd.Attribute("UserQuota");
			if (text != null)
			{
				this.m_userQuota = long.Parse(text, CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x0009F5C7 File Offset: 0x0009D7C7
		internal bool IsEmpty()
		{
			return this.m_userQuota == 0L && this.m_allowed == IsolatedStorageContainment.None;
		}

		// Token: 0x04002042 RID: 8258
		private const int version = 1;

		// Token: 0x04002043 RID: 8259
		internal long m_userQuota;

		// Token: 0x04002044 RID: 8260
		internal long m_machineQuota;

		// Token: 0x04002045 RID: 8261
		internal long m_expirationDays;

		// Token: 0x04002046 RID: 8262
		internal bool m_permanentData;

		// Token: 0x04002047 RID: 8263
		internal IsolatedStorageContainment m_allowed;
	}
}
