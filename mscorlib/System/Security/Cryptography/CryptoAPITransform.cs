using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
	// Token: 0x020004C5 RID: 1221
	[ComVisible(true)]
	public sealed class CryptoAPITransform : ICryptoTransform, IDisposable
	{
		// Token: 0x060030CD RID: 12493 RVA: 0x000B1A1B File Offset: 0x000AFC1B
		internal CryptoAPITransform()
		{
			this.m_disposed = false;
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x060030CE RID: 12494 RVA: 0x000040F7 File Offset: 0x000022F7
		public bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x060030CF RID: 12495 RVA: 0x000040F7 File Offset: 0x000022F7
		public bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x060030D0 RID: 12496 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public int InputBlockSize
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x060030D1 RID: 12497 RVA: 0x000B1A2A File Offset: 0x000AFC2A
		public IntPtr KeyHandle
		{
			[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
			get
			{
				return IntPtr.Zero;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x060030D2 RID: 12498 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public int OutputBlockSize
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x000B1A31 File Offset: 0x000AFC31
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x000B1A40 File Offset: 0x000AFC40
		public void Clear()
		{
			this.Dispose(false);
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x000B1A49 File Offset: 0x000AFC49
		private void Dispose(bool disposing)
		{
			if (!this.m_disposed)
			{
				this.m_disposed = true;
			}
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		[SecuritySafeCritical]
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			return 0;
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x0000AF5E File Offset: 0x0000915E
		[SecuritySafeCritical]
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			return null;
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[ComVisible(false)]
		public void Reset()
		{
		}

		// Token: 0x04002243 RID: 8771
		private bool m_disposed;
	}
}
