using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text
{
	// Token: 0x020003AF RID: 943
	[Serializable]
	public class UTF7Encoding : Encoding
	{
		// Token: 0x06002708 RID: 9992 RVA: 0x0008BAE6 File Offset: 0x00089CE6
		public UTF7Encoding() : this(false)
		{
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x0008BAEF File Offset: 0x00089CEF
		public UTF7Encoding(bool allowOptionals) : base(65000)
		{
			this._allowOptionals = allowOptionals;
			this.MakeTables();
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x0008BB0C File Offset: 0x00089D0C
		private void MakeTables()
		{
			this._base64Bytes = new byte[64];
			for (int i = 0; i < 64; i++)
			{
				this._base64Bytes[i] = (byte)"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"[i];
			}
			this._base64Values = new sbyte[128];
			for (int j = 0; j < 128; j++)
			{
				this._base64Values[j] = -1;
			}
			for (int k = 0; k < 64; k++)
			{
				this._base64Values[(int)this._base64Bytes[k]] = (sbyte)k;
			}
			this._directEncode = new bool[128];
			int length = "\t\n\r '(),-./0123456789:?ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".Length;
			for (int l = 0; l < length; l++)
			{
				this._directEncode[(int)"\t\n\r '(),-./0123456789:?ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"[l]] = true;
			}
			if (this._allowOptionals)
			{
				length = "!\"#$%&*;<=>@[]^_`{|}".Length;
				for (int m = 0; m < length; m++)
				{
					this._directEncode[(int)"!\"#$%&*;<=>@[]^_`{|}"[m]] = true;
				}
			}
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x0008BC04 File Offset: 0x00089E04
		internal override void SetDefaultFallbacks()
		{
			this.encoderFallback = new EncoderReplacementFallback(string.Empty);
			this.decoderFallback = new UTF7Encoding.DecoderUTF7Fallback();
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x0008BC24 File Offset: 0x00089E24
		public override bool Equals(object value)
		{
			UTF7Encoding utf7Encoding = value as UTF7Encoding;
			return utf7Encoding != null && (this._allowOptionals == utf7Encoding._allowOptionals && base.EncoderFallback.Equals(utf7Encoding.EncoderFallback)) && base.DecoderFallback.Equals(utf7Encoding.DecoderFallback);
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x0008BC71 File Offset: 0x00089E71
		public override int GetHashCode()
		{
			return this.CodePage + base.EncoderFallback.GetHashCode() + base.DecoderFallback.GetHashCode();
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x0008BC94 File Offset: 0x00089E94
		public unsafe override int GetByteCount(char[] chars, int index, int count)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", "Array cannot be null.");
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", "Non-negative number required.");
			}
			if (chars.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("chars", "Index and count must refer to a location within the buffer.");
			}
			if (count == 0)
			{
				return 0;
			}
			char* ptr;
			if (chars == null || chars.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &chars[0];
			}
			return this.GetByteCount(ptr + index, count, null);
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x0008BD1C File Offset: 0x00089F1C
		public unsafe override int GetByteCount(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			char* ptr = s;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return this.GetByteCount(ptr, s.Length, null);
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x00084CA1 File Offset: 0x00082EA1
		[CLSCompliant(false)]
		public unsafe override int GetByteCount(char* chars, int count)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", "Array cannot be null.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			return this.GetByteCount(chars, count, null);
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x0008BD58 File Offset: 0x00089F58
		public unsafe override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			if (s == null || bytes == null)
			{
				throw new ArgumentNullException((s == null) ? "s" : "bytes", "Array cannot be null.");
			}
			if (charIndex < 0 || charCount < 0)
			{
				throw new ArgumentOutOfRangeException((charIndex < 0) ? "charIndex" : "charCount", "Non-negative number required.");
			}
			if (s.Length - charIndex < charCount)
			{
				throw new ArgumentOutOfRangeException("s", "Index and count must refer to a location within the string.");
			}
			if (byteIndex < 0 || byteIndex > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			int byteCount = bytes.Length - byteIndex;
			char* ptr = s;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			fixed (byte* reference = MemoryMarshal.GetReference<byte>(bytes))
			{
				byte* ptr2 = reference;
				return this.GetBytes(ptr + charIndex, charCount, ptr2 + byteIndex, byteCount, null);
			}
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x0008BE20 File Offset: 0x0008A020
		public unsafe override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			if (chars == null || bytes == null)
			{
				throw new ArgumentNullException((chars == null) ? "chars" : "bytes", "Array cannot be null.");
			}
			if (charIndex < 0 || charCount < 0)
			{
				throw new ArgumentOutOfRangeException((charIndex < 0) ? "charIndex" : "charCount", "Non-negative number required.");
			}
			if (chars.Length - charIndex < charCount)
			{
				throw new ArgumentOutOfRangeException("chars", "Index and count must refer to a location within the buffer.");
			}
			if (byteIndex < 0 || byteIndex > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (charCount == 0)
			{
				return 0;
			}
			int byteCount = bytes.Length - byteIndex;
			char* ptr;
			if (chars == null || chars.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &chars[0];
			}
			fixed (byte* reference = MemoryMarshal.GetReference<byte>(bytes))
			{
				byte* ptr2 = reference;
				return this.GetBytes(ptr + charIndex, charCount, ptr2 + byteIndex, byteCount, null);
			}
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x0008BEF0 File Offset: 0x0008A0F0
		[CLSCompliant(false)]
		public unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (bytes == null || chars == null)
			{
				throw new ArgumentNullException((bytes == null) ? "bytes" : "chars", "Array cannot be null.");
			}
			if (charCount < 0 || byteCount < 0)
			{
				throw new ArgumentOutOfRangeException((charCount < 0) ? "charCount" : "byteCount", "Non-negative number required.");
			}
			return this.GetBytes(chars, charCount, bytes, byteCount, null);
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x0008BF54 File Offset: 0x0008A154
		public unsafe override int GetCharCount(byte[] bytes, int index, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", "Array cannot be null.");
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", "Non-negative number required.");
			}
			if (bytes.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("bytes", "Index and count must refer to a location within the buffer.");
			}
			if (count == 0)
			{
				return 0;
			}
			byte* ptr;
			if (bytes == null || bytes.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &bytes[0];
			}
			return this.GetCharCount(ptr + index, count, null);
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x00084F57 File Offset: 0x00083157
		[CLSCompliant(false)]
		public unsafe override int GetCharCount(byte* bytes, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", "Array cannot be null.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			return this.GetCharCount(bytes, count, null);
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x0008BFD8 File Offset: 0x0008A1D8
		public unsafe override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			if (bytes == null || chars == null)
			{
				throw new ArgumentNullException((bytes == null) ? "bytes" : "chars", "Array cannot be null.");
			}
			if (byteIndex < 0 || byteCount < 0)
			{
				throw new ArgumentOutOfRangeException((byteIndex < 0) ? "byteIndex" : "byteCount", "Non-negative number required.");
			}
			if (bytes.Length - byteIndex < byteCount)
			{
				throw new ArgumentOutOfRangeException("bytes", "Index and count must refer to a location within the buffer.");
			}
			if (charIndex < 0 || charIndex > chars.Length)
			{
				throw new ArgumentOutOfRangeException("charIndex", "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (byteCount == 0)
			{
				return 0;
			}
			int charCount = chars.Length - charIndex;
			byte* ptr;
			if (bytes == null || bytes.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &bytes[0];
			}
			fixed (char* reference = MemoryMarshal.GetReference<char>(chars))
			{
				char* ptr2 = reference;
				return this.GetChars(ptr + byteIndex, byteCount, ptr2 + charIndex, charCount, null);
			}
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x0008C0A8 File Offset: 0x0008A2A8
		[CLSCompliant(false)]
		public unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
		{
			if (bytes == null || chars == null)
			{
				throw new ArgumentNullException((bytes == null) ? "bytes" : "chars", "Array cannot be null.");
			}
			if (charCount < 0 || byteCount < 0)
			{
				throw new ArgumentOutOfRangeException((charCount < 0) ? "charCount" : "byteCount", "Non-negative number required.");
			}
			return this.GetChars(bytes, byteCount, chars, charCount, null);
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x0008C10C File Offset: 0x0008A30C
		public unsafe override string GetString(byte[] bytes, int index, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", "Array cannot be null.");
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", "Non-negative number required.");
			}
			if (bytes.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("bytes", "Index and count must refer to a location within the buffer.");
			}
			if (count == 0)
			{
				return string.Empty;
			}
			byte* ptr;
			if (bytes == null || bytes.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &bytes[0];
			}
			return string.CreateStringFromEncoding(ptr + index, count, this);
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x0008C192 File Offset: 0x0008A392
		internal unsafe override int GetByteCount(char* chars, int count, EncoderNLS baseEncoder)
		{
			return this.GetBytes(chars, count, null, 0, baseEncoder);
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x0008C1A0 File Offset: 0x0008A3A0
		internal unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, EncoderNLS baseEncoder)
		{
			UTF7Encoding.Encoder encoder = (UTF7Encoding.Encoder)baseEncoder;
			int num = 0;
			int i = -1;
			Encoding.EncodingByteBuffer encodingByteBuffer = new Encoding.EncodingByteBuffer(this, encoder, bytes, byteCount, chars, charCount);
			if (encoder != null)
			{
				num = encoder.bits;
				i = encoder.bitCount;
				while (i >= 6)
				{
					i -= 6;
					if (!encodingByteBuffer.AddByte(this._base64Bytes[num >> i & 63]))
					{
						base.ThrowBytesOverflow(encoder, encodingByteBuffer.Count == 0);
					}
				}
			}
			while (encodingByteBuffer.MoreData)
			{
				char nextChar = encodingByteBuffer.GetNextChar();
				if (nextChar < '\u0080' && this._directEncode[(int)nextChar])
				{
					if (i >= 0)
					{
						if (i > 0)
						{
							if (!encodingByteBuffer.AddByte(this._base64Bytes[num << 6 - i & 63]))
							{
								break;
							}
							i = 0;
						}
						if (!encodingByteBuffer.AddByte(45))
						{
							break;
						}
						i = -1;
					}
					if (!encodingByteBuffer.AddByte((byte)nextChar))
					{
						break;
					}
				}
				else if (i < 0 && nextChar == '+')
				{
					if (!encodingByteBuffer.AddByte(43, 45))
					{
						break;
					}
				}
				else
				{
					if (i < 0)
					{
						if (!encodingByteBuffer.AddByte(43))
						{
							break;
						}
						i = 0;
					}
					num = (num << 16 | (int)nextChar);
					i += 16;
					while (i >= 6)
					{
						i -= 6;
						if (!encodingByteBuffer.AddByte(this._base64Bytes[num >> i & 63]))
						{
							i += 6;
							nextChar = encodingByteBuffer.GetNextChar();
							break;
						}
					}
					if (i >= 6)
					{
						break;
					}
				}
			}
			if (i >= 0 && (encoder == null || encoder.MustFlush))
			{
				if (i > 0 && encodingByteBuffer.AddByte(this._base64Bytes[num << 6 - i & 63]))
				{
					i = 0;
				}
				if (encodingByteBuffer.AddByte(45))
				{
					num = 0;
					i = -1;
				}
				else
				{
					encodingByteBuffer.GetNextChar();
				}
			}
			if (bytes != null && encoder != null)
			{
				encoder.bits = num;
				encoder.bitCount = i;
				encoder._charsUsed = encodingByteBuffer.CharsUsed;
			}
			return encodingByteBuffer.Count;
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x0008C352 File Offset: 0x0008A552
		internal unsafe override int GetCharCount(byte* bytes, int count, DecoderNLS baseDecoder)
		{
			return this.GetChars(bytes, count, null, 0, baseDecoder);
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x0008C360 File Offset: 0x0008A560
		internal unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount, DecoderNLS baseDecoder)
		{
			UTF7Encoding.Decoder decoder = (UTF7Encoding.Decoder)baseDecoder;
			Encoding.EncodingCharBuffer encodingCharBuffer = new Encoding.EncodingCharBuffer(this, decoder, chars, charCount, bytes, byteCount);
			int num = 0;
			int num2 = -1;
			bool flag = false;
			if (decoder != null)
			{
				num = decoder.bits;
				num2 = decoder.bitCount;
				flag = decoder.firstByte;
			}
			if (num2 >= 16)
			{
				if (!encodingCharBuffer.AddChar((char)(num >> num2 - 16 & 65535)))
				{
					base.ThrowCharsOverflow(decoder, true);
				}
				num2 -= 16;
			}
			while (encodingCharBuffer.MoreData)
			{
				byte nextByte = encodingCharBuffer.GetNextByte();
				int num3;
				if (num2 >= 0)
				{
					sbyte b;
					if (nextByte < 128 && (b = this._base64Values[(int)nextByte]) >= 0)
					{
						flag = false;
						num = (num << 6 | (int)((byte)b));
						num2 += 6;
						if (num2 < 16)
						{
							continue;
						}
						num3 = (num >> num2 - 16 & 65535);
						num2 -= 16;
					}
					else
					{
						num2 = -1;
						if (nextByte != 45)
						{
							if (!encodingCharBuffer.Fallback(nextByte))
							{
								break;
							}
							continue;
						}
						else
						{
							if (!flag)
							{
								continue;
							}
							num3 = 43;
						}
					}
				}
				else
				{
					if (nextByte == 43)
					{
						num2 = 0;
						flag = true;
						continue;
					}
					if (nextByte >= 128)
					{
						if (!encodingCharBuffer.Fallback(nextByte))
						{
							break;
						}
						continue;
					}
					else
					{
						num3 = (int)nextByte;
					}
				}
				if (num3 >= 0 && !encodingCharBuffer.AddChar((char)num3))
				{
					if (num2 >= 0)
					{
						encodingCharBuffer.AdjustBytes(1);
						num2 += 16;
						break;
					}
					break;
				}
			}
			if (chars != null && decoder != null)
			{
				if (decoder.MustFlush)
				{
					decoder.bits = 0;
					decoder.bitCount = -1;
					decoder.firstByte = false;
				}
				else
				{
					decoder.bits = num;
					decoder.bitCount = num2;
					decoder.firstByte = flag;
				}
				decoder._bytesUsed = encodingCharBuffer.BytesUsed;
			}
			return encodingCharBuffer.Count;
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x0008C4E4 File Offset: 0x0008A6E4
		public override System.Text.Decoder GetDecoder()
		{
			return new UTF7Encoding.Decoder(this);
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x0008C4EC File Offset: 0x0008A6EC
		public override System.Text.Encoder GetEncoder()
		{
			return new UTF7Encoding.Encoder(this);
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x0008C4F4 File Offset: 0x0008A6F4
		public override int GetMaxByteCount(int charCount)
		{
			if (charCount < 0)
			{
				throw new ArgumentOutOfRangeException("charCount", "Non-negative number required.");
			}
			long num = (long)charCount * 3L + 2L;
			if (num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("charCount", "Too many characters. The resulting number of bytes is larger than what can be returned as an int.");
			}
			return (int)num;
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x0008C52C File Offset: 0x0008A72C
		public override int GetMaxCharCount(int byteCount)
		{
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount", "Non-negative number required.");
			}
			int num = byteCount;
			if (num == 0)
			{
				num = 1;
			}
			return num;
		}

		// Token: 0x04001DE9 RID: 7657
		private const string base64Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

		// Token: 0x04001DEA RID: 7658
		private const string directChars = "\t\n\r '(),-./0123456789:?ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

		// Token: 0x04001DEB RID: 7659
		private const string optionalChars = "!\"#$%&*;<=>@[]^_`{|}";

		// Token: 0x04001DEC RID: 7660
		internal static readonly UTF7Encoding s_default = new UTF7Encoding();

		// Token: 0x04001DED RID: 7661
		private byte[] _base64Bytes;

		// Token: 0x04001DEE RID: 7662
		private sbyte[] _base64Values;

		// Token: 0x04001DEF RID: 7663
		private bool[] _directEncode;

		// Token: 0x04001DF0 RID: 7664
		private bool _allowOptionals;

		// Token: 0x04001DF1 RID: 7665
		private const int UTF7_CODEPAGE = 65000;

		// Token: 0x020003B0 RID: 944
		[Serializable]
		private sealed class Decoder : DecoderNLS
		{
			// Token: 0x06002722 RID: 10018 RVA: 0x0008BAAF File Offset: 0x00089CAF
			public Decoder(UTF7Encoding encoding) : base(encoding)
			{
			}

			// Token: 0x06002723 RID: 10019 RVA: 0x0008C561 File Offset: 0x0008A761
			public override void Reset()
			{
				this.bits = 0;
				this.bitCount = -1;
				this.firstByte = false;
				if (this._fallbackBuffer != null)
				{
					this._fallbackBuffer.Reset();
				}
			}

			// Token: 0x170004BE RID: 1214
			// (get) Token: 0x06002724 RID: 10020 RVA: 0x0008C58B File Offset: 0x0008A78B
			internal override bool HasState
			{
				get
				{
					return this.bitCount != -1;
				}
			}

			// Token: 0x04001DF2 RID: 7666
			internal int bits;

			// Token: 0x04001DF3 RID: 7667
			internal int bitCount;

			// Token: 0x04001DF4 RID: 7668
			internal bool firstByte;
		}

		// Token: 0x020003B1 RID: 945
		[Serializable]
		private sealed class Encoder : EncoderNLS
		{
			// Token: 0x06002725 RID: 10021 RVA: 0x0008C599 File Offset: 0x0008A799
			public Encoder(UTF7Encoding encoding) : base(encoding)
			{
			}

			// Token: 0x06002726 RID: 10022 RVA: 0x0008C5A2 File Offset: 0x0008A7A2
			public override void Reset()
			{
				this.bitCount = -1;
				this.bits = 0;
				if (this._fallbackBuffer != null)
				{
					this._fallbackBuffer.Reset();
				}
			}

			// Token: 0x170004BF RID: 1215
			// (get) Token: 0x06002727 RID: 10023 RVA: 0x0008C5C5 File Offset: 0x0008A7C5
			internal override bool HasState
			{
				get
				{
					return this.bits != 0 || this.bitCount != -1;
				}
			}

			// Token: 0x04001DF5 RID: 7669
			internal int bits;

			// Token: 0x04001DF6 RID: 7670
			internal int bitCount;
		}

		// Token: 0x020003B2 RID: 946
		[Serializable]
		private sealed class DecoderUTF7Fallback : DecoderFallback
		{
			// Token: 0x06002729 RID: 10025 RVA: 0x0008C5DD File Offset: 0x0008A7DD
			public override DecoderFallbackBuffer CreateFallbackBuffer()
			{
				return new UTF7Encoding.DecoderUTF7FallbackBuffer(this);
			}

			// Token: 0x170004C0 RID: 1216
			// (get) Token: 0x0600272A RID: 10026 RVA: 0x000040F7 File Offset: 0x000022F7
			public override int MaxCharCount
			{
				get
				{
					return 1;
				}
			}

			// Token: 0x0600272B RID: 10027 RVA: 0x0008C5E5 File Offset: 0x0008A7E5
			public override bool Equals(object value)
			{
				return value is UTF7Encoding.DecoderUTF7Fallback;
			}

			// Token: 0x0600272C RID: 10028 RVA: 0x0008C5F2 File Offset: 0x0008A7F2
			public override int GetHashCode()
			{
				return 984;
			}
		}

		// Token: 0x020003B3 RID: 947
		private sealed class DecoderUTF7FallbackBuffer : DecoderFallbackBuffer
		{
			// Token: 0x0600272D RID: 10029 RVA: 0x0008C5F9 File Offset: 0x0008A7F9
			public DecoderUTF7FallbackBuffer(UTF7Encoding.DecoderUTF7Fallback fallback)
			{
			}

			// Token: 0x0600272E RID: 10030 RVA: 0x0008C608 File Offset: 0x0008A808
			public override bool Fallback(byte[] bytesUnknown, int index)
			{
				this.cFallback = (char)bytesUnknown[0];
				if (this.cFallback == '\0')
				{
					return false;
				}
				this.iCount = (this.iSize = 1);
				return true;
			}

			// Token: 0x0600272F RID: 10031 RVA: 0x0008C63C File Offset: 0x0008A83C
			public override char GetNextChar()
			{
				int num = this.iCount;
				this.iCount = num - 1;
				if (num > 0)
				{
					return this.cFallback;
				}
				return '\0';
			}

			// Token: 0x06002730 RID: 10032 RVA: 0x0008C665 File Offset: 0x0008A865
			public override bool MovePrevious()
			{
				if (this.iCount >= 0)
				{
					this.iCount++;
				}
				return this.iCount >= 0 && this.iCount <= this.iSize;
			}

			// Token: 0x170004C1 RID: 1217
			// (get) Token: 0x06002731 RID: 10033 RVA: 0x0008C69A File Offset: 0x0008A89A
			public override int Remaining
			{
				get
				{
					if (this.iCount <= 0)
					{
						return 0;
					}
					return this.iCount;
				}
			}

			// Token: 0x06002732 RID: 10034 RVA: 0x0008C6AD File Offset: 0x0008A8AD
			public override void Reset()
			{
				this.iCount = -1;
				this.byteStart = null;
			}

			// Token: 0x06002733 RID: 10035 RVA: 0x0008C6BE File Offset: 0x0008A8BE
			internal unsafe override int InternalFallback(byte[] bytes, byte* pBytes)
			{
				if (bytes.Length != 1)
				{
					throw new ArgumentException("String contains invalid Unicode code points.");
				}
				if (bytes[0] != 0)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x04001DF7 RID: 7671
			private char cFallback;

			// Token: 0x04001DF8 RID: 7672
			private int iCount = -1;

			// Token: 0x04001DF9 RID: 7673
			private int iSize;
		}
	}
}
