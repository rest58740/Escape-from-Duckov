using System;

namespace System.Text
{
	// Token: 0x0200039F RID: 927
	public sealed class EncoderExceptionFallbackBuffer : EncoderFallbackBuffer
	{
		// Token: 0x0600260A RID: 9738 RVA: 0x0008704E File Offset: 0x0008524E
		public override bool Fallback(char charUnknown, int index)
		{
			throw new EncoderFallbackException(SR.Format("Unable to translate Unicode character \\\\u{0:X4} at index {1} to specified code page.", (int)charUnknown, index), charUnknown, index);
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x00087070 File Offset: 0x00085270
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
			int num = char.ConvertToUtf32(charUnknownHigh, charUnknownLow);
			throw new EncoderFallbackException(SR.Format("Unable to translate Unicode character \\\\u{0:X4} at index {1} to specified code page.", num, index), charUnknownHigh, charUnknownLow, index);
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override char GetNextChar()
		{
			return '\0';
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override bool MovePrevious()
		{
			return false;
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x0600260E RID: 9742 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override int Remaining
		{
			get
			{
				return 0;
			}
		}
	}
}
