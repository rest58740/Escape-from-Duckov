using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace System.Security.Principal
{
	// Token: 0x020004DC RID: 1244
	[Serializable]
	public class GenericIdentity : ClaimsIdentity
	{
		// Token: 0x060031DA RID: 12762 RVA: 0x000B79E0 File Offset: 0x000B5BE0
		public GenericIdentity(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.m_name = name;
			this.m_type = "";
			this.AddNameClaim();
		}

		// Token: 0x060031DB RID: 12763 RVA: 0x000B7A0E File Offset: 0x000B5C0E
		public GenericIdentity(string name, string type)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.m_name = name;
			this.m_type = type;
			this.AddNameClaim();
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x000B7A46 File Offset: 0x000B5C46
		private GenericIdentity()
		{
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x000B7A4E File Offset: 0x000B5C4E
		protected GenericIdentity(GenericIdentity identity) : base(identity)
		{
			this.m_name = identity.m_name;
			this.m_type = identity.m_type;
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x000B7A6F File Offset: 0x000B5C6F
		public override ClaimsIdentity Clone()
		{
			return new GenericIdentity(this);
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x060031DF RID: 12767 RVA: 0x000B7A77 File Offset: 0x000B5C77
		public override IEnumerable<Claim> Claims
		{
			get
			{
				return base.Claims;
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x060031E0 RID: 12768 RVA: 0x000B7A7F File Offset: 0x000B5C7F
		public override string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x060031E1 RID: 12769 RVA: 0x000B7A87 File Offset: 0x000B5C87
		public override string AuthenticationType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060031E2 RID: 12770 RVA: 0x000B7A8F File Offset: 0x000B5C8F
		public override bool IsAuthenticated
		{
			get
			{
				return !this.m_name.Equals("");
			}
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x000B7AA4 File Offset: 0x000B5CA4
		private void AddNameClaim()
		{
			if (this.m_name != null)
			{
				base.AddClaim(new Claim(base.NameClaimType, this.m_name, "http://www.w3.org/2001/XMLSchema#string", "LOCAL AUTHORITY", "LOCAL AUTHORITY", this));
			}
		}

		// Token: 0x040022A3 RID: 8867
		private readonly string m_name;

		// Token: 0x040022A4 RID: 8868
		private readonly string m_type;
	}
}
