using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200044E RID: 1102
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class PrincipalPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002CB2 RID: 11442 RVA: 0x000A08FD File Offset: 0x0009EAFD
		public PrincipalPermissionAttribute(SecurityAction action) : base(action)
		{
			this.authenticated = true;
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06002CB3 RID: 11443 RVA: 0x000A090D File Offset: 0x0009EB0D
		// (set) Token: 0x06002CB4 RID: 11444 RVA: 0x000A0915 File Offset: 0x0009EB15
		public bool Authenticated
		{
			get
			{
				return this.authenticated;
			}
			set
			{
				this.authenticated = value;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06002CB5 RID: 11445 RVA: 0x000A091E File Offset: 0x0009EB1E
		// (set) Token: 0x06002CB6 RID: 11446 RVA: 0x000A0926 File Offset: 0x0009EB26
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06002CB7 RID: 11447 RVA: 0x000A092F File Offset: 0x0009EB2F
		// (set) Token: 0x06002CB8 RID: 11448 RVA: 0x000A0937 File Offset: 0x0009EB37
		public string Role
		{
			get
			{
				return this.role;
			}
			set
			{
				this.role = value;
			}
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x000A0940 File Offset: 0x0009EB40
		public override IPermission CreatePermission()
		{
			PrincipalPermission result;
			if (base.Unrestricted)
			{
				result = new PrincipalPermission(PermissionState.Unrestricted);
			}
			else
			{
				result = new PrincipalPermission(this.name, this.role, this.authenticated);
			}
			return result;
		}

		// Token: 0x04002071 RID: 8305
		private bool authenticated;

		// Token: 0x04002072 RID: 8306
		private string name;

		// Token: 0x04002073 RID: 8307
		private string role;
	}
}
