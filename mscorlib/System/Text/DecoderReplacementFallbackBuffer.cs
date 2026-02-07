using System;

namespace System.Text
{
	// Token: 0x0200039A RID: 922
	public sealed class DecoderReplacementFallbackBuffer : DecoderFallbackBuffer
	{
		// Token: 0x060025E0 RID: 9696 RVA: 0x000867A7 File Offset: 0x000849A7
		public DecoderReplacementFallbackBuffer(DecoderReplacementFallback fallback)
		{
			this._strDefault = fallback.DefaultString;
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x000867C9 File Offset: 0x000849C9
		public override bool Fallback(byte[] bytesUnknown, int index)
		{
			if (this._fallbackCount >= 1)
			{
				base.ThrowLastBytesRecursive(bytesUnknown);
			}
			if (this._strDefault.Length == 0)
			{
				return false;
			}
			this._fallbackCount = this._strDefault.Length;
			this._fallbackIndex = -1;
			return true;
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x00086804 File Offset: 0x00084A04
		public override char GetNextChar()
		{
			this._fallbackCount--;
			this._fallbackIndex++;
			if (this._fallbackCount < 0)
			{
				return '\0';
			}
			if (this._fallbackCount == 2147483647)
			{
				this._fallbackCount = -1;
				return '\0';
			}
			return this._strDefault[this._fallbackIndex];
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x0008685F File Offset: 0x00084A5F
		public override bool MovePrevious()
		{
			if (this._fallbackCount >= -1 && this._fallbackIndex >= 0)
			{
				this._fallbackIndex--;
				this._fallbackCount++;
				return true;
			}
			return false;
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060025E4 RID: 9700 RVA: 0x00086892 File Offset: 0x00084A92
		public override int Remaining
		{
			get
			{
				if (this._fallbackCount >= 0)
				{
					return this._fallbackCount;
				}
				return 0;
			}
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x000868A5 File Offset: 0x00084AA5
		public override void Reset()
		{
			this._fallbackCount = -1;
			this._fallbackIndex = -1;
			this.byteStart = null;
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x000868BD File Offset: 0x00084ABD
		internal unsafe override int InternalFallback(byte[] bytes, byte* pBytes)
		{
			return this._strDefault.Length;
		}

		// Token: 0x04001DA1 RID: 7585
		private string _strDefault;

		// Token: 0x04001DA2 RID: 7586
		private int _fallbackCount = -1;

		// Token: 0x04001DA3 RID: 7587
		private int _fallbackIndex = -1;
	}
}
