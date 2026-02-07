using System;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Encryption
{
	// Token: 0x0200006C RID: 108
	internal class PkzipClassicDecryptCryptoTransform : PkzipClassicCryptoBase, ICryptoTransform, IDisposable
	{
		// Token: 0x0600048D RID: 1165 RVA: 0x00019128 File Offset: 0x00017328
		internal PkzipClassicDecryptCryptoTransform(byte[] keyBlock)
		{
			base.SetKeys(keyBlock);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00019138 File Offset: 0x00017338
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			byte[] array = new byte[inputCount];
			this.TransformBlock(inputBuffer, inputOffset, inputCount, array, 0);
			return array;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0001915C File Offset: 0x0001735C
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			for (int i = inputOffset; i < inputOffset + inputCount; i++)
			{
				byte b = inputBuffer[i] ^ base.TransformByte();
				outputBuffer[outputOffset++] = b;
				base.UpdateKeys(b);
			}
			return inputCount;
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x0001919C File Offset: 0x0001739C
		public bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x000191A0 File Offset: 0x000173A0
		public int InputBlockSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x000191A4 File Offset: 0x000173A4
		public int OutputBlockSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x000191A8 File Offset: 0x000173A8
		public bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x000191AC File Offset: 0x000173AC
		public void Dispose()
		{
			base.Reset();
		}
	}
}
