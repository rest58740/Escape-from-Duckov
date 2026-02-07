using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020004AC RID: 1196
	[ComVisible(true)]
	public class RSAOAEPKeyExchangeDeformatter : AsymmetricKeyExchangeDeformatter
	{
		// Token: 0x06003008 RID: 12296 RVA: 0x000AE76C File Offset: 0x000AC96C
		public RSAOAEPKeyExchangeDeformatter()
		{
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x000AE774 File Offset: 0x000AC974
		public RSAOAEPKeyExchangeDeformatter(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x0600300A RID: 12298 RVA: 0x0000AF5E File Offset: 0x0000915E
		// (set) Token: 0x0600300B RID: 12299 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public override string Parameters
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x0600300C RID: 12300 RVA: 0x000AE798 File Offset: 0x000AC998
		[SecuritySafeCritical]
		public override byte[] DecryptKeyExchange(byte[] rgbData)
		{
			if (this._rsaKey == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("No asymmetric key object has been associated with this formatter object."));
			}
			if (this.OverridesDecrypt)
			{
				return this._rsaKey.Decrypt(rgbData, RSAEncryptionPadding.OaepSHA1);
			}
			return Utils.RsaOaepDecrypt(this._rsaKey, SHA1.Create(), new PKCS1MaskGenerationMethod(), rgbData);
		}

		// Token: 0x0600300D RID: 12301 RVA: 0x000AE7ED File Offset: 0x000AC9ED
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
			this._rsaOverridesDecrypt = null;
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x0600300E RID: 12302 RVA: 0x000AE818 File Offset: 0x000ACA18
		private bool OverridesDecrypt
		{
			get
			{
				if (this._rsaOverridesDecrypt == null)
				{
					this._rsaOverridesDecrypt = new bool?(Utils.DoesRsaKeyOverride(this._rsaKey, "Decrypt", new Type[]
					{
						typeof(byte[]),
						typeof(RSAEncryptionPadding)
					}));
				}
				return this._rsaOverridesDecrypt.Value;
			}
		}

		// Token: 0x040021CA RID: 8650
		private RSA _rsaKey;

		// Token: 0x040021CB RID: 8651
		private bool? _rsaOverridesDecrypt;
	}
}
