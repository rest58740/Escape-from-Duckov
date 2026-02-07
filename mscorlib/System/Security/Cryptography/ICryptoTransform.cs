using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000498 RID: 1176
	[ComVisible(true)]
	public interface ICryptoTransform : IDisposable
	{
		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06002F1E RID: 12062
		int InputBlockSize { get; }

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06002F1F RID: 12063
		int OutputBlockSize { get; }

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06002F20 RID: 12064
		bool CanTransformMultipleBlocks { get; }

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06002F21 RID: 12065
		bool CanReuseTransform { get; }

		// Token: 0x06002F22 RID: 12066
		int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset);

		// Token: 0x06002F23 RID: 12067
		byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount);
	}
}
