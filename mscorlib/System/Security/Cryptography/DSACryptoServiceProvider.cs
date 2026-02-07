using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Mono.Security.Cryptography;

namespace System.Security.Cryptography
{
	// Token: 0x020004C9 RID: 1225
	[ComVisible(true)]
	public sealed class DSACryptoServiceProvider : DSA, ICspAsymmetricAlgorithm
	{
		// Token: 0x060030FC RID: 12540 RVA: 0x000B3A02 File Offset: 0x000B1C02
		public DSACryptoServiceProvider() : this(1024)
		{
		}

		// Token: 0x060030FD RID: 12541 RVA: 0x000B3A0F File Offset: 0x000B1C0F
		public DSACryptoServiceProvider(CspParameters parameters) : this(1024, parameters)
		{
		}

		// Token: 0x060030FE RID: 12542 RVA: 0x000B3A1D File Offset: 0x000B1C1D
		public DSACryptoServiceProvider(int dwKeySize)
		{
			this.privateKeyExportable = true;
			base..ctor();
			this.Common(dwKeySize, false);
		}

		// Token: 0x060030FF RID: 12543 RVA: 0x000B3A34 File Offset: 0x000B1C34
		public DSACryptoServiceProvider(int dwKeySize, CspParameters parameters)
		{
			this.privateKeyExportable = true;
			base..ctor();
			bool flag = parameters != null;
			this.Common(dwKeySize, flag);
			if (flag)
			{
				this.Common(parameters);
			}
		}

		// Token: 0x06003100 RID: 12544 RVA: 0x000B3A68 File Offset: 0x000B1C68
		private void Common(int dwKeySize, bool parameters)
		{
			this.LegalKeySizesValue = new KeySizes[1];
			this.LegalKeySizesValue[0] = new KeySizes(512, 1024, 64);
			this.KeySize = dwKeySize;
			this.dsa = new DSAManaged(dwKeySize);
			this.dsa.KeyGenerated += this.OnKeyGenerated;
			this.persistKey = parameters;
			if (parameters)
			{
				return;
			}
			CspParameters cspParameters = new CspParameters(13);
			if (DSACryptoServiceProvider.useMachineKeyStore)
			{
				cspParameters.Flags |= CspProviderFlags.UseMachineKeyStore;
			}
			this.store = new KeyPairPersistence(cspParameters);
		}

		// Token: 0x06003101 RID: 12545 RVA: 0x000B3AF8 File Offset: 0x000B1CF8
		private void Common(CspParameters parameters)
		{
			this.store = new KeyPairPersistence(parameters);
			this.store.Load();
			if (this.store.KeyValue != null)
			{
				this.persisted = true;
				this.FromXmlString(this.store.KeyValue);
			}
			this.privateKeyExportable = ((parameters.Flags & CspProviderFlags.UseNonExportableKey) == CspProviderFlags.NoFlags);
		}

		// Token: 0x06003102 RID: 12546 RVA: 0x000B3B54 File Offset: 0x000B1D54
		~DSACryptoServiceProvider()
		{
			this.Dispose(false);
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06003103 RID: 12547 RVA: 0x0000AF5E File Offset: 0x0000915E
		public override string KeyExchangeAlgorithm
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06003104 RID: 12548 RVA: 0x000B3B84 File Offset: 0x000B1D84
		public override int KeySize
		{
			get
			{
				return this.dsa.KeySize;
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06003105 RID: 12549 RVA: 0x000B3B91 File Offset: 0x000B1D91
		// (set) Token: 0x06003106 RID: 12550 RVA: 0x000B3B99 File Offset: 0x000B1D99
		public bool PersistKeyInCsp
		{
			get
			{
				return this.persistKey;
			}
			set
			{
				this.persistKey = value;
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06003107 RID: 12551 RVA: 0x000B3BA2 File Offset: 0x000B1DA2
		[ComVisible(false)]
		public bool PublicOnly
		{
			get
			{
				return this.dsa.PublicOnly;
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06003108 RID: 12552 RVA: 0x00012A75 File Offset: 0x00010C75
		public override string SignatureAlgorithm
		{
			get
			{
				return "http://www.w3.org/2000/09/xmldsig#dsa-sha1";
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06003109 RID: 12553 RVA: 0x000B3BAF File Offset: 0x000B1DAF
		// (set) Token: 0x0600310A RID: 12554 RVA: 0x000B3BB6 File Offset: 0x000B1DB6
		public static bool UseMachineKeyStore
		{
			get
			{
				return DSACryptoServiceProvider.useMachineKeyStore;
			}
			set
			{
				DSACryptoServiceProvider.useMachineKeyStore = value;
			}
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x000B3BBE File Offset: 0x000B1DBE
		public override DSAParameters ExportParameters(bool includePrivateParameters)
		{
			if (includePrivateParameters && !this.privateKeyExportable)
			{
				throw new CryptographicException(Locale.GetText("Cannot export private key"));
			}
			return this.dsa.ExportParameters(includePrivateParameters);
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x000B3BE7 File Offset: 0x000B1DE7
		public override void ImportParameters(DSAParameters parameters)
		{
			this.dsa.ImportParameters(parameters);
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x000B3BF5 File Offset: 0x000B1DF5
		public override byte[] CreateSignature(byte[] rgbHash)
		{
			return this.dsa.CreateSignature(rgbHash);
		}

		// Token: 0x0600310E RID: 12558 RVA: 0x000B3C04 File Offset: 0x000B1E04
		public byte[] SignData(byte[] buffer)
		{
			byte[] rgbHash = SHA1.Create().ComputeHash(buffer);
			return this.dsa.CreateSignature(rgbHash);
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x000B3C2C File Offset: 0x000B1E2C
		public byte[] SignData(byte[] buffer, int offset, int count)
		{
			byte[] rgbHash = SHA1.Create().ComputeHash(buffer, offset, count);
			return this.dsa.CreateSignature(rgbHash);
		}

		// Token: 0x06003110 RID: 12560 RVA: 0x000B3C54 File Offset: 0x000B1E54
		public byte[] SignData(Stream inputStream)
		{
			byte[] rgbHash = SHA1.Create().ComputeHash(inputStream);
			return this.dsa.CreateSignature(rgbHash);
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x000B3C79 File Offset: 0x000B1E79
		public byte[] SignHash(byte[] rgbHash, string str)
		{
			if (string.Compare(str, "SHA1", true, CultureInfo.InvariantCulture) != 0)
			{
				throw new CryptographicException(Locale.GetText("Only SHA1 is supported."));
			}
			return this.dsa.CreateSignature(rgbHash);
		}

		// Token: 0x06003112 RID: 12562 RVA: 0x000B3CAC File Offset: 0x000B1EAC
		public bool VerifyData(byte[] rgbData, byte[] rgbSignature)
		{
			byte[] rgbHash = SHA1.Create().ComputeHash(rgbData);
			return this.dsa.VerifySignature(rgbHash, rgbSignature);
		}

		// Token: 0x06003113 RID: 12563 RVA: 0x000B3CD2 File Offset: 0x000B1ED2
		public bool VerifyHash(byte[] rgbHash, string str, byte[] rgbSignature)
		{
			if (str == null)
			{
				str = "SHA1";
			}
			if (string.Compare(str, "SHA1", true, CultureInfo.InvariantCulture) != 0)
			{
				throw new CryptographicException(Locale.GetText("Only SHA1 is supported."));
			}
			return this.dsa.VerifySignature(rgbHash, rgbSignature);
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x000B3D0E File Offset: 0x000B1F0E
		public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature)
		{
			return this.dsa.VerifySignature(rgbHash, rgbSignature);
		}

		// Token: 0x06003115 RID: 12565 RVA: 0x000B3D20 File Offset: 0x000B1F20
		protected override byte[] HashData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm)
		{
			if (hashAlgorithm != HashAlgorithmName.SHA1)
			{
				throw new CryptographicException(Environment.GetResourceString("'{0}' is not a known hash algorithm.", new object[]
				{
					hashAlgorithm.Name
				}));
			}
			return HashAlgorithm.Create(hashAlgorithm.Name).ComputeHash(data, offset, count);
		}

		// Token: 0x06003116 RID: 12566 RVA: 0x000B3D70 File Offset: 0x000B1F70
		protected override byte[] HashData(Stream data, HashAlgorithmName hashAlgorithm)
		{
			if (hashAlgorithm != HashAlgorithmName.SHA1)
			{
				throw new CryptographicException(Environment.GetResourceString("'{0}' is not a known hash algorithm.", new object[]
				{
					hashAlgorithm.Name
				}));
			}
			return HashAlgorithm.Create(hashAlgorithm.Name).ComputeHash(data);
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x000B3DBC File Offset: 0x000B1FBC
		protected override void Dispose(bool disposing)
		{
			if (!this.m_disposed)
			{
				if (this.persisted && !this.persistKey)
				{
					this.store.Remove();
				}
				if (this.dsa != null)
				{
					this.dsa.Clear();
				}
				this.m_disposed = true;
			}
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x000B3DFC File Offset: 0x000B1FFC
		private void OnKeyGenerated(object sender, EventArgs e)
		{
			if (this.persistKey && !this.persisted)
			{
				this.store.KeyValue = this.ToXmlString(!this.dsa.PublicOnly);
				this.store.Save();
				this.persisted = true;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06003119 RID: 12569 RVA: 0x0000AF5E File Offset: 0x0000915E
		[MonoTODO("call into KeyPairPersistence to get details")]
		[ComVisible(false)]
		public CspKeyContainerInfo CspKeyContainerInfo
		{
			[SecuritySafeCritical]
			get
			{
				return null;
			}
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x000B3E4C File Offset: 0x000B204C
		[ComVisible(false)]
		[SecuritySafeCritical]
		public byte[] ExportCspBlob(bool includePrivateParameters)
		{
			byte[] result;
			if (includePrivateParameters)
			{
				result = CryptoConvert.ToCapiPrivateKeyBlob(this);
			}
			else
			{
				result = CryptoConvert.ToCapiPublicKeyBlob(this);
			}
			return result;
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x000B3E70 File Offset: 0x000B2070
		[SecuritySafeCritical]
		[ComVisible(false)]
		public void ImportCspBlob(byte[] keyBlob)
		{
			if (keyBlob == null)
			{
				throw new ArgumentNullException("keyBlob");
			}
			DSA dsa = CryptoConvert.FromCapiKeyBlobDSA(keyBlob);
			if (dsa is DSACryptoServiceProvider)
			{
				DSAParameters parameters = dsa.ExportParameters(!(dsa as DSACryptoServiceProvider).PublicOnly);
				this.ImportParameters(parameters);
				return;
			}
			try
			{
				DSAParameters parameters2 = dsa.ExportParameters(true);
				this.ImportParameters(parameters2);
			}
			catch
			{
				DSAParameters parameters3 = dsa.ExportParameters(false);
				this.ImportParameters(parameters3);
			}
		}

		// Token: 0x04002255 RID: 8789
		private const int PROV_DSS_DH = 13;

		// Token: 0x04002256 RID: 8790
		private KeyPairPersistence store;

		// Token: 0x04002257 RID: 8791
		private bool persistKey;

		// Token: 0x04002258 RID: 8792
		private bool persisted;

		// Token: 0x04002259 RID: 8793
		private bool privateKeyExportable;

		// Token: 0x0400225A RID: 8794
		private bool m_disposed;

		// Token: 0x0400225B RID: 8795
		private DSAManaged dsa;

		// Token: 0x0400225C RID: 8796
		private static bool useMachineKeyStore;
	}
}
