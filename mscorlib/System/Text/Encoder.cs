using System;
using System.Runtime.InteropServices;

namespace System.Text
{
	// Token: 0x0200039B RID: 923
	[Serializable]
	public abstract class Encoder
	{
		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060025E8 RID: 9704 RVA: 0x000868CA File Offset: 0x00084ACA
		// (set) Token: 0x060025E9 RID: 9705 RVA: 0x000868D4 File Offset: 0x00084AD4
		public EncoderFallback Fallback
		{
			get
			{
				return this._fallback;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this._fallbackBuffer != null && this._fallbackBuffer.Remaining > 0)
				{
					throw new ArgumentException("Cannot change fallback when buffer is not empty. Previous Convert() call left data in the fallback buffer.", "value");
				}
				this._fallback = value;
				this._fallbackBuffer = null;
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060025EA RID: 9706 RVA: 0x00086923 File Offset: 0x00084B23
		public EncoderFallbackBuffer FallbackBuffer
		{
			get
			{
				if (this._fallbackBuffer == null)
				{
					if (this._fallback != null)
					{
						this._fallbackBuffer = this._fallback.CreateFallbackBuffer();
					}
					else
					{
						this._fallbackBuffer = EncoderFallback.ReplacementFallback.CreateFallbackBuffer();
					}
				}
				return this._fallbackBuffer;
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060025EB RID: 9707 RVA: 0x0008695E File Offset: 0x00084B5E
		internal bool InternalHasFallbackBuffer
		{
			get
			{
				return this._fallbackBuffer != null;
			}
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x0008696C File Offset: 0x00084B6C
		public virtual void Reset()
		{
			char[] chars = new char[0];
			byte[] bytes = new byte[this.GetByteCount(chars, 0, 0, true)];
			this.GetBytes(chars, 0, 0, bytes, 0, true);
			if (this._fallbackBuffer != null)
			{
				this._fallbackBuffer.Reset();
			}
		}

		// Token: 0x060025ED RID: 9709
		public abstract int GetByteCount(char[] chars, int index, int count, bool flush);

		// Token: 0x060025EE RID: 9710 RVA: 0x000869B0 File Offset: 0x00084BB0
		[CLSCompliant(false)]
		public unsafe virtual int GetByteCount(char* chars, int count, bool flush)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars", "Array cannot be null.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			char[] array = new char[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = chars[i];
			}
			return this.GetByteCount(array, 0, count, flush);
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x00086A10 File Offset: 0x00084C10
		public unsafe virtual int GetByteCount(ReadOnlySpan<char> chars, bool flush)
		{
			fixed (char* nonNullPinnableReference = MemoryMarshal.GetNonNullPinnableReference<char>(chars))
			{
				char* chars2 = nonNullPinnableReference;
				return this.GetByteCount(chars2, chars.Length, flush);
			}
		}

		// Token: 0x060025F0 RID: 9712
		public abstract int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush);

		// Token: 0x060025F1 RID: 9713 RVA: 0x00086A38 File Offset: 0x00084C38
		[CLSCompliant(false)]
		public unsafe virtual int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, bool flush)
		{
			if (bytes == null || chars == null)
			{
				throw new ArgumentNullException((bytes == null) ? "bytes" : "chars", "Array cannot be null.");
			}
			if (charCount < 0 || byteCount < 0)
			{
				throw new ArgumentOutOfRangeException((charCount < 0) ? "charCount" : "byteCount", "Non-negative number required.");
			}
			char[] array = new char[charCount];
			for (int i = 0; i < charCount; i++)
			{
				array[i] = chars[i];
			}
			byte[] array2 = new byte[byteCount];
			int bytes2 = this.GetBytes(array, 0, charCount, array2, 0, flush);
			if (bytes2 < byteCount)
			{
				byteCount = bytes2;
			}
			for (int i = 0; i < byteCount; i++)
			{
				bytes[i] = array2[i];
			}
			return byteCount;
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x00086AE0 File Offset: 0x00084CE0
		public unsafe virtual int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes, bool flush)
		{
			fixed (char* nonNullPinnableReference = MemoryMarshal.GetNonNullPinnableReference<char>(chars))
			{
				char* chars2 = nonNullPinnableReference;
				fixed (byte* nonNullPinnableReference2 = MemoryMarshal.GetNonNullPinnableReference<byte>(bytes))
				{
					byte* bytes2 = nonNullPinnableReference2;
					return this.GetBytes(chars2, chars.Length, bytes2, bytes.Length, flush);
				}
			}
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x00086B18 File Offset: 0x00084D18
		public virtual void Convert(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, int byteCount, bool flush, out int charsUsed, out int bytesUsed, out bool completed)
		{
			if (chars == null || bytes == null)
			{
				throw new ArgumentNullException((chars == null) ? "chars" : "bytes", "Array cannot be null.");
			}
			if (charIndex < 0 || charCount < 0)
			{
				throw new ArgumentOutOfRangeException((charIndex < 0) ? "charIndex" : "charCount", "Non-negative number required.");
			}
			if (byteIndex < 0 || byteCount < 0)
			{
				throw new ArgumentOutOfRangeException((byteIndex < 0) ? "byteIndex" : "byteCount", "Non-negative number required.");
			}
			if (chars.Length - charIndex < charCount)
			{
				throw new ArgumentOutOfRangeException("chars", "Index and count must refer to a location within the buffer.");
			}
			if (bytes.Length - byteIndex < byteCount)
			{
				throw new ArgumentOutOfRangeException("bytes", "Index and count must refer to a location within the buffer.");
			}
			for (charsUsed = charCount; charsUsed > 0; charsUsed /= 2)
			{
				if (this.GetByteCount(chars, charIndex, charsUsed, flush) <= byteCount)
				{
					bytesUsed = this.GetBytes(chars, charIndex, charsUsed, bytes, byteIndex, flush);
					completed = (charsUsed == charCount && (this._fallbackBuffer == null || this._fallbackBuffer.Remaining == 0));
					return;
				}
				flush = false;
			}
			throw new ArgumentException("Conversion buffer overflow.");
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x00086C30 File Offset: 0x00084E30
		[CLSCompliant(false)]
		public unsafe virtual void Convert(char* chars, int charCount, byte* bytes, int byteCount, bool flush, out int charsUsed, out int bytesUsed, out bool completed)
		{
			if (bytes == null || chars == null)
			{
				throw new ArgumentNullException((bytes == null) ? "bytes" : "chars", "Array cannot be null.");
			}
			if (charCount < 0 || byteCount < 0)
			{
				throw new ArgumentOutOfRangeException((charCount < 0) ? "charCount" : "byteCount", "Non-negative number required.");
			}
			for (charsUsed = charCount; charsUsed > 0; charsUsed /= 2)
			{
				if (this.GetByteCount(chars, charsUsed, flush) <= byteCount)
				{
					bytesUsed = this.GetBytes(chars, charsUsed, bytes, byteCount, flush);
					completed = (charsUsed == charCount && (this._fallbackBuffer == null || this._fallbackBuffer.Remaining == 0));
					return;
				}
				flush = false;
			}
			throw new ArgumentException("Conversion buffer overflow.");
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x00086CF0 File Offset: 0x00084EF0
		public unsafe virtual void Convert(ReadOnlySpan<char> chars, Span<byte> bytes, bool flush, out int charsUsed, out int bytesUsed, out bool completed)
		{
			fixed (char* nonNullPinnableReference = MemoryMarshal.GetNonNullPinnableReference<char>(chars))
			{
				char* chars2 = nonNullPinnableReference;
				fixed (byte* nonNullPinnableReference2 = MemoryMarshal.GetNonNullPinnableReference<byte>(bytes))
				{
					byte* bytes2 = nonNullPinnableReference2;
					this.Convert(chars2, chars.Length, bytes2, bytes.Length, flush, out charsUsed, out bytesUsed, out completed);
				}
			}
		}

		// Token: 0x04001DA4 RID: 7588
		internal EncoderFallback _fallback;

		// Token: 0x04001DA5 RID: 7589
		internal EncoderFallbackBuffer _fallbackBuffer;
	}
}
