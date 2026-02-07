using System;
using System.Globalization;

namespace System.Text
{
	// Token: 0x02000394 RID: 916
	public sealed class DecoderExceptionFallbackBuffer : DecoderFallbackBuffer
	{
		// Token: 0x060025AC RID: 9644 RVA: 0x00085F36 File Offset: 0x00084136
		public override bool Fallback(byte[] bytesUnknown, int index)
		{
			this.Throw(bytesUnknown, index);
			return true;
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override char GetNextChar()
		{
			return '\0';
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override bool MovePrevious()
		{
			return false;
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x060025AF RID: 9647 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override int Remaining
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x00085F44 File Offset: 0x00084144
		private void Throw(byte[] bytesUnknown, int index)
		{
			StringBuilder stringBuilder = new StringBuilder(bytesUnknown.Length * 3);
			int num = 0;
			while (num < bytesUnknown.Length && num < 20)
			{
				stringBuilder.Append('[');
				stringBuilder.Append(bytesUnknown[num].ToString("X2", CultureInfo.InvariantCulture));
				stringBuilder.Append(']');
				num++;
			}
			if (num == 20)
			{
				stringBuilder.Append(" ...");
			}
			throw new DecoderFallbackException(SR.Format("Unable to translate bytes {0} at index {1} from specified code page to Unicode.", stringBuilder, index), bytesUnknown, index);
		}
	}
}
