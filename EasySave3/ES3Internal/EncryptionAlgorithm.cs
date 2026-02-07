using System;
using System.IO;

namespace ES3Internal
{
	// Token: 0x020000CF RID: 207
	public abstract class EncryptionAlgorithm
	{
		// Token: 0x0600040C RID: 1036
		public abstract byte[] Encrypt(byte[] bytes, string password, int bufferSize);

		// Token: 0x0600040D RID: 1037
		public abstract byte[] Decrypt(byte[] bytes, string password, int bufferSize);

		// Token: 0x0600040E RID: 1038
		public abstract void Encrypt(Stream input, Stream output, string password, int bufferSize);

		// Token: 0x0600040F RID: 1039
		public abstract void Decrypt(Stream input, Stream output, string password, int bufferSize);

		// Token: 0x06000410 RID: 1040 RVA: 0x0001A89C File Offset: 0x00018A9C
		protected static void CopyStream(Stream input, Stream output, int bufferSize)
		{
			byte[] buffer = new byte[bufferSize];
			int count;
			while ((count = input.Read(buffer, 0, bufferSize)) > 0)
			{
				output.Write(buffer, 0, count);
			}
		}
	}
}
