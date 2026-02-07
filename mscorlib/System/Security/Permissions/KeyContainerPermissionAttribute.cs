using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000449 RID: 1097
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class KeyContainerPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002C7E RID: 11390 RVA: 0x0009FC22 File Offset: 0x0009DE22
		public KeyContainerPermissionAttribute(SecurityAction action) : base(action)
		{
			this._spec = -1;
			this._type = -1;
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06002C7F RID: 11391 RVA: 0x0009FC39 File Offset: 0x0009DE39
		// (set) Token: 0x06002C80 RID: 11392 RVA: 0x0009FC41 File Offset: 0x0009DE41
		public KeyContainerPermissionFlags Flags
		{
			get
			{
				return this._flags;
			}
			set
			{
				this._flags = value;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06002C81 RID: 11393 RVA: 0x0009FC4A File Offset: 0x0009DE4A
		// (set) Token: 0x06002C82 RID: 11394 RVA: 0x0009FC52 File Offset: 0x0009DE52
		public string KeyContainerName
		{
			get
			{
				return this._containerName;
			}
			set
			{
				this._containerName = value;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06002C83 RID: 11395 RVA: 0x0009FC5B File Offset: 0x0009DE5B
		// (set) Token: 0x06002C84 RID: 11396 RVA: 0x0009FC63 File Offset: 0x0009DE63
		public int KeySpec
		{
			get
			{
				return this._spec;
			}
			set
			{
				this._spec = value;
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06002C85 RID: 11397 RVA: 0x0009FC6C File Offset: 0x0009DE6C
		// (set) Token: 0x06002C86 RID: 11398 RVA: 0x0009FC74 File Offset: 0x0009DE74
		public string KeyStore
		{
			get
			{
				return this._store;
			}
			set
			{
				this._store = value;
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06002C87 RID: 11399 RVA: 0x0009FC7D File Offset: 0x0009DE7D
		// (set) Token: 0x06002C88 RID: 11400 RVA: 0x0009FC85 File Offset: 0x0009DE85
		public string ProviderName
		{
			get
			{
				return this._providerName;
			}
			set
			{
				this._providerName = value;
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06002C89 RID: 11401 RVA: 0x0009FC8E File Offset: 0x0009DE8E
		// (set) Token: 0x06002C8A RID: 11402 RVA: 0x0009FC96 File Offset: 0x0009DE96
		public int ProviderType
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x0009FCA0 File Offset: 0x0009DEA0
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new KeyContainerPermission(PermissionState.Unrestricted);
			}
			if (this.EmptyEntry())
			{
				return new KeyContainerPermission(this._flags);
			}
			KeyContainerPermissionAccessEntry[] accessList = new KeyContainerPermissionAccessEntry[]
			{
				new KeyContainerPermissionAccessEntry(this._store, this._providerName, this._type, this._containerName, this._spec, this._flags)
			};
			return new KeyContainerPermission(this._flags, accessList);
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x0009FD0F File Offset: 0x0009DF0F
		private bool EmptyEntry()
		{
			return this._containerName == null && this._spec == 0 && this._store == null && this._providerName == null && this._type == 0;
		}

		// Token: 0x04002055 RID: 8277
		private KeyContainerPermissionFlags _flags;

		// Token: 0x04002056 RID: 8278
		private string _containerName;

		// Token: 0x04002057 RID: 8279
		private int _spec;

		// Token: 0x04002058 RID: 8280
		private string _store;

		// Token: 0x04002059 RID: 8281
		private string _providerName;

		// Token: 0x0400205A RID: 8282
		private int _type;
	}
}
