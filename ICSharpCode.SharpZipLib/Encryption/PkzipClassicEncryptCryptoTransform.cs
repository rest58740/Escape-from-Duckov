using System;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Encryption
{
	// Token: 0x0200006B RID: 107
	internal class PkzipClassicEncryptCryptoTransform : PkzipClassicCryptoBase, ICryptoTransform, IDisposable
	{
		// Token: 0x06000485 RID: 1157 RVA: 0x00019098 File Offset: 0x00017298
		internal PkzipClassicEncryptCryptoTransform(byte[] keyBlock)
		{
			base.SetKeys(keyBlock);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x000190A8 File Offset: 0x000172A8
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			byte[] array = new byte[inputCount];
			this.TransformBlock(inputBuffer, inputOffset, inputCount, array, 0);
			return array;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x000190CC File Offset: 0x000172CC
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			for (int i = inputOffset; i < inputOffset + inputCount; i++)
			{
				byte ch = inputBuffer[i];
				outputBuffer[outputOffset++] = (inputBuffer[i] ^ base.TransformByte());
				base.UpdateKeys(ch);
			}
			return inputCount;
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x00019110 File Offset: 0x00017310
		public bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x00019114 File Offset: 0x00017314
		public int InputBlockSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x00019118 File Offset: 0x00017318
		public int OutputBlockSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x0001911C File Offset: 0x0001731C
		public bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00019120 File Offset: 0x00017320
		public void Dispose()
		{
			base.Reset();
		}
	}
}
