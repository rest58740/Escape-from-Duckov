using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace System.Security.Permissions
{
	// Token: 0x02000446 RID: 1094
	[ComVisible(true)]
	[Serializable]
	public sealed class KeyContainerPermissionAccessEntry
	{
		// Token: 0x06002C59 RID: 11353 RVA: 0x0009F884 File Offset: 0x0009DA84
		public KeyContainerPermissionAccessEntry(CspParameters parameters, KeyContainerPermissionFlags flags)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			this.ProviderName = parameters.ProviderName;
			this.ProviderType = parameters.ProviderType;
			this.KeyContainerName = parameters.KeyContainerName;
			this.KeySpec = parameters.KeyNumber;
			this.Flags = flags;
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x0009F8DC File Offset: 0x0009DADC
		public KeyContainerPermissionAccessEntry(string keyContainerName, KeyContainerPermissionFlags flags)
		{
			this.KeyContainerName = keyContainerName;
			this.Flags = flags;
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x0009F8F2 File Offset: 0x0009DAF2
		public KeyContainerPermissionAccessEntry(string keyStore, string providerName, int providerType, string keyContainerName, int keySpec, KeyContainerPermissionFlags flags)
		{
			this.KeyStore = keyStore;
			this.ProviderName = providerName;
			this.ProviderType = providerType;
			this.KeyContainerName = keyContainerName;
			this.KeySpec = keySpec;
			this.Flags = flags;
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06002C5C RID: 11356 RVA: 0x0009F927 File Offset: 0x0009DB27
		// (set) Token: 0x06002C5D RID: 11357 RVA: 0x0009F92F File Offset: 0x0009DB2F
		public KeyContainerPermissionFlags Flags
		{
			get
			{
				return this._flags;
			}
			set
			{
				if ((value & KeyContainerPermissionFlags.AllFlags) != KeyContainerPermissionFlags.NoFlags)
				{
					throw new ArgumentException(string.Format(Locale.GetText("Invalid enum {0}"), value), "KeyContainerPermissionFlags");
				}
				this._flags = value;
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06002C5E RID: 11358 RVA: 0x0009F961 File Offset: 0x0009DB61
		// (set) Token: 0x06002C5F RID: 11359 RVA: 0x0009F969 File Offset: 0x0009DB69
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

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06002C60 RID: 11360 RVA: 0x0009F972 File Offset: 0x0009DB72
		// (set) Token: 0x06002C61 RID: 11361 RVA: 0x0009F97A File Offset: 0x0009DB7A
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

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06002C62 RID: 11362 RVA: 0x0009F983 File Offset: 0x0009DB83
		// (set) Token: 0x06002C63 RID: 11363 RVA: 0x0009F98B File Offset: 0x0009DB8B
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

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06002C64 RID: 11364 RVA: 0x0009F994 File Offset: 0x0009DB94
		// (set) Token: 0x06002C65 RID: 11365 RVA: 0x0009F99C File Offset: 0x0009DB9C
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

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06002C66 RID: 11366 RVA: 0x0009F9A5 File Offset: 0x0009DBA5
		// (set) Token: 0x06002C67 RID: 11367 RVA: 0x0009F9AD File Offset: 0x0009DBAD
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

		// Token: 0x06002C68 RID: 11368 RVA: 0x0009F9B8 File Offset: 0x0009DBB8
		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = o as KeyContainerPermissionAccessEntry;
			return keyContainerPermissionAccessEntry != null && this._flags == keyContainerPermissionAccessEntry._flags && !(this._containerName != keyContainerPermissionAccessEntry._containerName) && !(this._store != keyContainerPermissionAccessEntry._store) && !(this._providerName != keyContainerPermissionAccessEntry._providerName) && this._type == keyContainerPermissionAccessEntry._type;
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x0009FA38 File Offset: 0x0009DC38
		public override int GetHashCode()
		{
			int num = this._type ^ this._spec ^ (int)this._flags;
			if (this._containerName != null)
			{
				num ^= this._containerName.GetHashCode();
			}
			if (this._store != null)
			{
				num ^= this._store.GetHashCode();
			}
			if (this._providerName != null)
			{
				num ^= this._providerName.GetHashCode();
			}
			return num;
		}

		// Token: 0x0400204D RID: 8269
		private KeyContainerPermissionFlags _flags;

		// Token: 0x0400204E RID: 8270
		private string _containerName;

		// Token: 0x0400204F RID: 8271
		private int _spec;

		// Token: 0x04002050 RID: 8272
		private string _store;

		// Token: 0x04002051 RID: 8273
		private string _providerName;

		// Token: 0x04002052 RID: 8274
		private int _type;
	}
}
