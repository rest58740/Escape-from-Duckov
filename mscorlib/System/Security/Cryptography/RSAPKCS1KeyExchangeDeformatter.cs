using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020004AE RID: 1198
	[ComVisible(true)]
	public class RSAPKCS1KeyExchangeDeformatter : AsymmetricKeyExchangeDeformatter
	{
		// Token: 0x0600301A RID: 12314 RVA: 0x000AE76C File Offset: 0x000AC96C
		public RSAPKCS1KeyExchangeDeformatter()
		{
		}

		// Token: 0x0600301B RID: 12315 RVA: 0x000AE9DC File Offset: 0x000ACBDC
		public RSAPKCS1KeyExchangeDeformatter(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x0600301C RID: 12316 RVA: 0x000AE9FE File Offset: 0x000ACBFE
		// (set) Token: 0x0600301D RID: 12317 RVA: 0x000AEA06 File Offset: 0x000ACC06
		public RandomNumberGenerator RNG
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

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x0600301E RID: 12318 RVA: 0x0000AF5E File Offset: 0x0000915E
		// (set) Token: 0x0600301F RID: 12319 RVA: 0x00004BF9 File Offset: 0x00002DF9
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

		// Token: 0x06003020 RID: 12320 RVA: 0x000AEA10 File Offset: 0x000ACC10
		public override byte[] DecryptKeyExchange(byte[] rgbIn)
		{
			if (this._rsaKey == null)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("No asymmetric key object has been associated with this formatter object."));
			}
			byte[] array;
			if (this.OverridesDecrypt)
			{
				array = this._rsaKey.Decrypt(rgbIn, RSAEncryptionPadding.Pkcs1);
			}
			else
			{
				byte[] array2 = this._rsaKey.DecryptValue(rgbIn);
				int num = 2;
				while (num < array2.Length && array2[num] != 0)
				{
					num++;
				}
				if (num >= array2.Length)
				{
					throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Error occurred while decoding PKCS1 padding."));
				}
				num++;
				array = new byte[array2.Length - num];
				Buffer.InternalBlockCopy(array2, num, array, 0, array.Length);
			}
			return array;
		}

		// Token: 0x06003021 RID: 12321 RVA: 0x000AEAA4 File Offset: 0x000ACCA4
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this._rsaKey = (RSA)key;
			this._rsaOverridesDecrypt = null;
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06003022 RID: 12322 RVA: 0x000AEACC File Offset: 0x000ACCCC
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

		// Token: 0x040021D0 RID: 8656
		private RSA _rsaKey;

		// Token: 0x040021D1 RID: 8657
		private bool? _rsaOverridesDecrypt;

		// Token: 0x040021D2 RID: 8658
		private RandomNumberGenerator RngValue;
	}
}
