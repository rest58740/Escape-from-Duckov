using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200045E RID: 1118
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class StrongNameIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002D73 RID: 11635 RVA: 0x0009DD00 File Offset: 0x0009BF00
		public StrongNameIdentityPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06002D74 RID: 11636 RVA: 0x000A2FD0 File Offset: 0x000A11D0
		// (set) Token: 0x06002D75 RID: 11637 RVA: 0x000A2FD8 File Offset: 0x000A11D8
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

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06002D76 RID: 11638 RVA: 0x000A2FE1 File Offset: 0x000A11E1
		// (set) Token: 0x06002D77 RID: 11639 RVA: 0x000A2FE9 File Offset: 0x000A11E9
		public string PublicKey
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06002D78 RID: 11640 RVA: 0x000A2FF2 File Offset: 0x000A11F2
		// (set) Token: 0x06002D79 RID: 11641 RVA: 0x000A2FFA File Offset: 0x000A11FA
		public string Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x000A3004 File Offset: 0x000A1204
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new StrongNameIdentityPermission(PermissionState.Unrestricted);
			}
			if (this.name == null && this.key == null && this.version == null)
			{
				return new StrongNameIdentityPermission(PermissionState.None);
			}
			if (this.key == null)
			{
				throw new ArgumentException(Locale.GetText("PublicKey is required"));
			}
			StrongNamePublicKeyBlob blob = StrongNamePublicKeyBlob.FromString(this.key);
			Version version = null;
			if (this.version != null)
			{
				version = new Version(this.version);
			}
			return new StrongNameIdentityPermission(blob, this.name, version);
		}

		// Token: 0x040020B4 RID: 8372
		private string name;

		// Token: 0x040020B5 RID: 8373
		private string key;

		// Token: 0x040020B6 RID: 8374
		private string version;
	}
}
