using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000435 RID: 1077
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class EnvironmentPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002BAB RID: 11179 RVA: 0x0009DD00 File Offset: 0x0009BF00
		public EnvironmentPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06002BAC RID: 11180 RVA: 0x0009DD09 File Offset: 0x0009BF09
		// (set) Token: 0x06002BAD RID: 11181 RVA: 0x0009DD15 File Offset: 0x0009BF15
		public string All
		{
			get
			{
				throw new NotSupportedException("All");
			}
			set
			{
				this.read = value;
				this.write = value;
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06002BAE RID: 11182 RVA: 0x0009DD25 File Offset: 0x0009BF25
		// (set) Token: 0x06002BAF RID: 11183 RVA: 0x0009DD2D File Offset: 0x0009BF2D
		public string Read
		{
			get
			{
				return this.read;
			}
			set
			{
				this.read = value;
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06002BB0 RID: 11184 RVA: 0x0009DD36 File Offset: 0x0009BF36
		// (set) Token: 0x06002BB1 RID: 11185 RVA: 0x0009DD3E File Offset: 0x0009BF3E
		public string Write
		{
			get
			{
				return this.write;
			}
			set
			{
				this.write = value;
			}
		}

		// Token: 0x06002BB2 RID: 11186 RVA: 0x0009DD48 File Offset: 0x0009BF48
		public override IPermission CreatePermission()
		{
			EnvironmentPermission environmentPermission;
			if (base.Unrestricted)
			{
				environmentPermission = new EnvironmentPermission(PermissionState.Unrestricted);
			}
			else
			{
				environmentPermission = new EnvironmentPermission(PermissionState.None);
				if (this.read != null)
				{
					environmentPermission.AddPathList(EnvironmentPermissionAccess.Read, this.read);
				}
				if (this.write != null)
				{
					environmentPermission.AddPathList(EnvironmentPermissionAccess.Write, this.write);
				}
			}
			return environmentPermission;
		}

		// Token: 0x0400200D RID: 8205
		private string read;

		// Token: 0x0400200E RID: 8206
		private string write;
	}
}
