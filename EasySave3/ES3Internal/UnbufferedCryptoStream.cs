using System;
using System.IO;

namespace ES3Internal
{
	// Token: 0x020000D1 RID: 209
	public class UnbufferedCryptoStream : MemoryStream
	{
		// Token: 0x06000417 RID: 1047 RVA: 0x0001AB10 File Offset: 0x00018D10
		public UnbufferedCryptoStream(Stream stream, bool isReadStream, string password, int bufferSize, EncryptionAlgorithm alg)
		{
			this.stream = stream;
			this.isReadStream = isReadStream;
			this.password = password;
			this.bufferSize = bufferSize;
			this.alg = alg;
			if (isReadStream)
			{
				alg.Decrypt(stream, this, password, bufferSize);
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001AB4C File Offset: 0x00018D4C
		protected override void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			if (!this.isReadStream)
			{
				this.alg.Encrypt(this, this.stream, this.password, this.bufferSize);
			}
			this.stream.Dispose();
			base.Dispose(disposing);
		}

		// Token: 0x04000117 RID: 279
		private readonly Stream stream;

		// Token: 0x04000118 RID: 280
		private readonly bool isReadStream;

		// Token: 0x04000119 RID: 281
		private string password;

		// Token: 0x0400011A RID: 282
		private int bufferSize;

		// Token: 0x0400011B RID: 283
		private EncryptionAlgorithm alg;

		// Token: 0x0400011C RID: 284
		private bool disposed;
	}
}
