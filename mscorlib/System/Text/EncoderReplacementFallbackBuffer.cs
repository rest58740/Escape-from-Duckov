using System;

namespace System.Text
{
	// Token: 0x020003A5 RID: 933
	public sealed class EncoderReplacementFallbackBuffer : EncoderFallbackBuffer
	{
		// Token: 0x06002641 RID: 9793 RVA: 0x00087923 File Offset: 0x00085B23
		public EncoderReplacementFallbackBuffer(EncoderReplacementFallback fallback)
		{
			this._strDefault = fallback.DefaultString + fallback.DefaultString;
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x00087950 File Offset: 0x00085B50
		public override bool Fallback(char charUnknown, int index)
		{
			if (this._fallbackCount >= 1)
			{
				if (char.IsHighSurrogate(charUnknown) && this._fallbackCount >= 0 && char.IsLowSurrogate(this._strDefault[this._fallbackIndex + 1]))
				{
					base.ThrowLastCharRecursive(char.ConvertToUtf32(charUnknown, this._strDefault[this._fallbackIndex + 1]));
				}
				base.ThrowLastCharRecursive((int)charUnknown);
			}
			this._fallbackCount = this._strDefault.Length / 2;
			this._fallbackIndex = -1;
			return this._fallbackCount != 0;
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x000879DC File Offset: 0x00085BDC
		public override bool Fallback(char charUnknownHigh, char charUnknownLow, int index)
		{
			if (!char.IsHighSurrogate(charUnknownHigh))
			{
				throw new ArgumentOutOfRangeException("charUnknownHigh", SR.Format("Valid values are between {0} and {1}, inclusive.", 55296, 56319));
			}
			if (!char.IsLowSurrogate(charUnknownLow))
			{
				throw new ArgumentOutOfRangeException("charUnknownLow", SR.Format("Valid values are between {0} and {1}, inclusive.", 56320, 57343));
			}
			if (this._fallbackCount >= 1)
			{
				base.ThrowLastCharRecursive(char.ConvertToUtf32(charUnknownHigh, charUnknownLow));
			}
			this._fallbackCount = this._strDefault.Length;
			this._fallbackIndex = -1;
			return this._fallbackCount != 0;
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x00087A84 File Offset: 0x00085C84
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

		// Token: 0x06002645 RID: 9797 RVA: 0x00087ADF File Offset: 0x00085CDF
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

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06002646 RID: 9798 RVA: 0x00087B12 File Offset: 0x00085D12
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

		// Token: 0x06002647 RID: 9799 RVA: 0x00087B25 File Offset: 0x00085D25
		public override void Reset()
		{
			this._fallbackCount = -1;
			this._fallbackIndex = 0;
			this.charStart = null;
			this.bFallingBack = false;
		}

		// Token: 0x04001DC1 RID: 7617
		private string _strDefault;

		// Token: 0x04001DC2 RID: 7618
		private int _fallbackCount = -1;

		// Token: 0x04001DC3 RID: 7619
		private int _fallbackIndex = -1;
	}
}
