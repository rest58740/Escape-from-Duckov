using System;
using System.IO;
using System.Security.Cryptography;

namespace ES3Internal
{
	// Token: 0x020000D0 RID: 208
	public class AESEncryptionAlgorithm : EncryptionAlgorithm
	{
		// Token: 0x06000412 RID: 1042 RVA: 0x0001A8D4 File Offset: 0x00018AD4
		public override byte[] Encrypt(byte[] bytes, string password, int bufferSize)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				using (MemoryStream memoryStream2 = new MemoryStream())
				{
					this.Encrypt(memoryStream, memoryStream2, password, bufferSize);
					result = memoryStream2.ToArray();
				}
			}
			return result;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001A934 File Offset: 0x00018B34
		public override byte[] Decrypt(byte[] bytes, string password, int bufferSize)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				using (MemoryStream memoryStream2 = new MemoryStream())
				{
					this.Decrypt(memoryStream, memoryStream2, password, bufferSize);
					result = memoryStream2.ToArray();
				}
			}
			return result;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0001A994 File Offset: 0x00018B94
		public override void Encrypt(Stream input, Stream output, string password, int bufferSize)
		{
			input.Position = 0L;
			using (Aes aes = Aes.Create())
			{
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;
				aes.GenerateIV();
				Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, aes.IV, 100);
				aes.Key = rfc2898DeriveBytes.GetBytes(16);
				output.Write(aes.IV, 0, 16);
				using (ICryptoTransform cryptoTransform = aes.CreateEncryptor())
				{
					using (CryptoStream cryptoStream = new CryptoStream(output, cryptoTransform, CryptoStreamMode.Write))
					{
						EncryptionAlgorithm.CopyStream(input, cryptoStream, bufferSize);
					}
				}
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001AA50 File Offset: 0x00018C50
		public override void Decrypt(Stream input, Stream output, string password, int bufferSize)
		{
			using (Aes aes = Aes.Create())
			{
				byte[] array = new byte[16];
				input.Read(array, 0, 16);
				aes.IV = array;
				Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, aes.IV, 100);
				aes.Key = rfc2898DeriveBytes.GetBytes(16);
				using (ICryptoTransform cryptoTransform = aes.CreateDecryptor())
				{
					using (CryptoStream cryptoStream = new CryptoStream(input, cryptoTransform, CryptoStreamMode.Read))
					{
						EncryptionAlgorithm.CopyStream(cryptoStream, output, bufferSize);
					}
				}
			}
			output.Position = 0L;
		}

		// Token: 0x04000114 RID: 276
		private const int ivSize = 16;

		// Token: 0x04000115 RID: 277
		private const int keySize = 16;

		// Token: 0x04000116 RID: 278
		private const int pwIterations = 100;
	}
}
