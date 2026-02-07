using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020004AD RID: 1197
	[ComVisible(true)]
	public class RSAOAEPKeyExchangeFormatter : AsymmetricKeyExchangeFormatter
	{
		// Token: 0x0600300F RID: 12303 RVA: 0x000AE878 File Offset: 0x000ACA78
		public RSAOAEPKeyExchangeFormatter()
		{
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x000AE880 File Offset: 0x000ACA80
		public RSAOAEPKeyExchangeFormatter(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06003011 RID: 12305 RVA: 0x000AE8A2 File Offset: 0x000ACAA2
		// (set) Token: 0x06003012 RID: 12306 RVA: 0x000AE8BE File Offset: 0x000ACABE
		public byte[] Parameter
		{
			get
			{
				if (this.ParameterValue != null)
				{
					return (byte[])this.ParameterValue.Clone();
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.ParameterValue = (byte[])value.Clone();
					return;
				}
				this.ParameterValue = null;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06003013 RID: 12307 RVA: 0x0000AF5E File Offset: 0x0000915E
		public override string Parameters
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06003014 RID: 12308 RVA: 0x000AE8DC File Offset: 0x000ACADC
		// (set) Token: 0x06003015 RID: 12309 RVA: 0x000AE8E4 File Offset: 0x000ACAE4
		public RandomNumberGenerator Rng
		{
			get
			{
				return this.RngValue;
			}
			set
			{
				this.RngValue = value;
			}
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x000AE8ED File Offset: 0x000ACAED
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
			this._rsaOverridesEncrypt = null;
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x000AE918 File Offset: 0x000ACB18
		[SecuritySafeCritical]
		public override byte[] CreateKeyExchange(byte[] rgbData)
		{
			if (this._rsaKey == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("No asymmetric key object has been associated with this formatter object."));
			}
			if (this.OverridesEncrypt)
			{
				return this._rsaKey.Encrypt(rgbData, RSAEncryptionPadding.OaepSHA1);
			}
			return Utils.RsaOaepEncrypt(this._rsaKey, SHA1.Create(), new PKCS1MaskGenerationMethod(), RandomNumberGenerator.Create(), rgbData);
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x000AE972 File Offset: 0x000ACB72
		public override byte[] CreateKeyExchange(byte[] rgbData, Type symAlgType)
		{
			return this.CreateKeyExchange(rgbData);
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06003019 RID: 12313 RVA: 0x000AE97C File Offset: 0x000ACB7C
		private bool OverridesEncrypt
		{
			get
			{
				if (this._rsaOverridesEncrypt == null)
				{
					this._rsaOverridesEncrypt = new bool?(Utils.DoesRsaKeyOverride(this._rsaKey, "Encrypt", new Type[]
					{
						typeof(byte[]),
						typeof(RSAEncryptionPadding)
					}));
				}
				return this._rsaOverridesEncrypt.Value;
			}
		}

		// Token: 0x040021CC RID: 8652
		private byte[] ParameterValue;

		// Token: 0x040021CD RID: 8653
		private RSA _rsaKey;

		// Token: 0x040021CE RID: 8654
		private bool? _rsaOverridesEncrypt;

		// Token: 0x040021CF RID: 8655
		private RandomNumberGenerator RngValue;
	}
}
