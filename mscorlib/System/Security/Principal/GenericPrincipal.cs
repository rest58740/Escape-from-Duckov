using System;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace System.Security.Principal
{
	// Token: 0x020004E2 RID: 1250
	[ComVisible(true)]
	[Serializable]
	public class GenericPrincipal : ClaimsPrincipal
	{
		// Token: 0x060031E9 RID: 12777 RVA: 0x000B7AD8 File Offset: 0x000B5CD8
		public GenericPrincipal(IIdentity identity, string[] roles)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			this.m_identity = identity;
			if (roles != null)
			{
				this.m_roles = new string[roles.Length];
				for (int i = 0; i < roles.Length; i++)
				{
					this.m_roles[i] = roles[i];
				}
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060031EA RID: 12778 RVA: 0x000B7B2A File Offset: 0x000B5D2A
		internal string[] Roles
		{
			get
			{
				return this.m_roles;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x060031EB RID: 12779 RVA: 0x000B7B32 File Offset: 0x000B5D32
		public override IIdentity Identity
		{
			get
			{
				return this.m_identity;
			}
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x000B7B3C File Offset: 0x000B5D3C
		public override bool IsInRole(string role)
		{
			if (this.m_roles == null)
			{
				return false;
			}
			int length = role.Length;
			foreach (string text in this.m_roles)
			{
				if (text != null && length == text.Length && string.Compare(role, 0, text, 0, length, true) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040022BD RID: 8893
		private IIdentity m_identity;

		// Token: 0x040022BE RID: 8894
		private string[] m_roles;
	}
}
