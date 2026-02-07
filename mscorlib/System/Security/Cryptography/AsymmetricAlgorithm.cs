using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000478 RID: 1144
	[ComVisible(true)]
	public abstract class AsymmetricAlgorithm : IDisposable
	{
		// Token: 0x06002E4A RID: 11850 RVA: 0x000A6167 File Offset: 0x000A4367
		public void Dispose()
		{
			this.Clear();
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x000A616F File Offset: 0x000A436F
		public void Clear()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06002E4D RID: 11853 RVA: 0x000A617E File Offset: 0x000A437E
		// (set) Token: 0x06002E4E RID: 11854 RVA: 0x000A6188 File Offset: 0x000A4388
		public virtual int KeySize
		{
			get
			{
				return this.KeySizeValue;
			}
			set
			{
				for (int i = 0; i < this.LegalKeySizesValue.Length; i++)
				{
					if (this.LegalKeySizesValue[i].SkipSize == 0)
					{
						if (this.LegalKeySizesValue[i].MinSize == value)
						{
							this.KeySizeValue = value;
							return;
						}
					}
					else
					{
						for (int j = this.LegalKeySizesValue[i].MinSize; j <= this.LegalKeySizesValue[i].MaxSize; j += this.LegalKeySizesValue[i].SkipSize)
						{
							if (j == value)
							{
								this.KeySizeValue = value;
								return;
							}
						}
					}
				}
				throw new CryptographicException(Environment.GetResourceString("Specified key is not a valid size for this algorithm."));
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06002E4F RID: 11855 RVA: 0x000A621A File Offset: 0x000A441A
		public virtual KeySizes[] LegalKeySizes
		{
			get
			{
				return (KeySizes[])this.LegalKeySizesValue.Clone();
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06002E50 RID: 11856 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual string SignatureAlgorithm
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06002E51 RID: 11857 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual string KeyExchangeAlgorithm
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x000A622C File Offset: 0x000A442C
		public static AsymmetricAlgorithm Create()
		{
			return AsymmetricAlgorithm.Create("System.Security.Cryptography.AsymmetricAlgorithm");
		}

		// Token: 0x06002E53 RID: 11859 RVA: 0x000A6238 File Offset: 0x000A4438
		public static AsymmetricAlgorithm Create(string algName)
		{
			return (AsymmetricAlgorithm)CryptoConfig.CreateFromName(algName);
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual void FromXmlString(string xmlString)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x000479FC File Offset: 0x00045BFC
		public virtual string ToXmlString(bool includePrivateParameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002E56 RID: 11862 RVA: 0x0001B98F File Offset: 0x00019B8F
		public virtual byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x0001B98F File Offset: 0x00019B8F
		public virtual byte[] ExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x0001B98F File Offset: 0x00019B8F
		public virtual byte[] ExportPkcs8PrivateKey()
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x0001B98F File Offset: 0x00019B8F
		public virtual byte[] ExportSubjectPublicKeyInfo()
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x0001B98F File Offset: 0x00019B8F
		public virtual void ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, ReadOnlySpan<byte> source, out int bytesRead)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x0001B98F File Offset: 0x00019B8F
		public virtual void ImportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, ReadOnlySpan<byte> source, out int bytesRead)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x0001B98F File Offset: 0x00019B8F
		public virtual void ImportPkcs8PrivateKey(ReadOnlySpan<byte> source, out int bytesRead)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x0001B98F File Offset: 0x00019B8F
		public virtual void ImportSubjectPublicKeyInfo(ReadOnlySpan<byte> source, out int bytesRead)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x0001B98F File Offset: 0x00019B8F
		public virtual bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<byte> passwordBytes, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x06002E5F RID: 11871 RVA: 0x0001B98F File Offset: 0x00019B8F
		public virtual bool TryExportEncryptedPkcs8PrivateKey(ReadOnlySpan<char> password, PbeParameters pbeParameters, Span<byte> destination, out int bytesWritten)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x0001B98F File Offset: 0x00019B8F
		public virtual bool TryExportPkcs8PrivateKey(Span<byte> destination, out int bytesWritten)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x0001B98F File Offset: 0x00019B8F
		public virtual bool TryExportSubjectPublicKeyInfo(Span<byte> destination, out int bytesWritten)
		{
			throw new PlatformNotSupportedException();
		}

		// Token: 0x0400212B RID: 8491
		protected int KeySizeValue;

		// Token: 0x0400212C RID: 8492
		protected KeySizes[] LegalKeySizesValue;
	}
}
