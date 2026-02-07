using System;
using System.Runtime.InteropServices;

namespace System.Text
{
	// Token: 0x02000390 RID: 912
	[Serializable]
	public abstract class Decoder
	{
		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06002589 RID: 9609 RVA: 0x000857DC File Offset: 0x000839DC
		// (set) Token: 0x0600258A RID: 9610 RVA: 0x000857E4 File Offset: 0x000839E4
		public DecoderFallback Fallback
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

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x0600258B RID: 9611 RVA: 0x00085833 File Offset: 0x00083A33
		public DecoderFallbackBuffer FallbackBuffer
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
						this._fallbackBuffer = DecoderFallback.ReplacementFallback.CreateFallbackBuffer();
					}
				}
				return this._fallbackBuffer;
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x0600258C RID: 9612 RVA: 0x0008586E File Offset: 0x00083A6E
		internal bool InternalHasFallbackBuffer
		{
			get
			{
				return this._fallbackBuffer != null;
			}
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x0008587C File Offset: 0x00083A7C
		public virtual void Reset()
		{
			byte[] bytes = Array.Empty<byte>();
			char[] chars = new char[this.GetCharCount(bytes, 0, 0, true)];
			this.GetChars(bytes, 0, 0, chars, 0, true);
			DecoderFallbackBuffer fallbackBuffer = this._fallbackBuffer;
			if (fallbackBuffer == null)
			{
				return;
			}
			fallbackBuffer.Reset();
		}

		// Token: 0x0600258E RID: 9614
		public abstract int GetCharCount(byte[] bytes, int index, int count);

		// Token: 0x0600258F RID: 9615 RVA: 0x000858BC File Offset: 0x00083ABC
		public virtual int GetCharCount(byte[] bytes, int index, int count, bool flush)
		{
			return this.GetCharCount(bytes, index, count);
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x000858C8 File Offset: 0x00083AC8
		[CLSCompliant(false)]
		public unsafe virtual int GetCharCount(byte* bytes, int count, bool flush)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes", "Array cannot be null.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			byte[] array = new byte[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = bytes[i];
			}
			return this.GetCharCount(array, 0, count);
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x00085924 File Offset: 0x00083B24
		public unsafe virtual int GetCharCount(ReadOnlySpan<byte> bytes, bool flush)
		{
			fixed (byte* nonNullPinnableReference = MemoryMarshal.GetNonNullPinnableReference<byte>(bytes))
			{
				byte* bytes2 = nonNullPinnableReference;
				return this.GetCharCount(bytes2, bytes.Length, flush);
			}
		}

		// Token: 0x06002592 RID: 9618
		public abstract int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex);

		// Token: 0x06002593 RID: 9619 RVA: 0x0008594A File Offset: 0x00083B4A
		public virtual int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, bool flush)
		{
			return this.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x0008595C File Offset: 0x00083B5C
		[CLSCompliant(false)]
		public unsafe virtual int GetChars(byte* bytes, int byteCount, char* chars, int charCount, bool flush)
		{
			if (chars == null || bytes == null)
			{
				throw new ArgumentNullException((chars == null) ? "chars" : "bytes", "Array cannot be null.");
			}
			if (byteCount < 0 || charCount < 0)
			{
				throw new ArgumentOutOfRangeException((byteCount < 0) ? "byteCount" : "charCount", "Non-negative number required.");
			}
			byte[] array = new byte[byteCount];
			for (int i = 0; i < byteCount; i++)
			{
				array[i] = bytes[i];
			}
			char[] array2 = new char[charCount];
			int chars2 = this.GetChars(array, 0, byteCount, array2, 0, flush);
			if (chars2 < charCount)
			{
				charCount = chars2;
			}
			for (int i = 0; i < charCount; i++)
			{
				chars[i] = array2[i];
			}
			return charCount;
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x00085A04 File Offset: 0x00083C04
		public unsafe virtual int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars, bool flush)
		{
			fixed (byte* nonNullPinnableReference = MemoryMarshal.GetNonNullPinnableReference<byte>(bytes))
			{
				byte* bytes2 = nonNullPinnableReference;
				fixed (char* nonNullPinnableReference2 = MemoryMarshal.GetNonNullPinnableReference<char>(chars))
				{
					char* chars2 = nonNullPinnableReference2;
					return this.GetChars(bytes2, bytes.Length, chars2, chars.Length, flush);
				}
			}
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x00085A3C File Offset: 0x00083C3C
		public virtual void Convert(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, int charCount, bool flush, out int bytesUsed, out int charsUsed, out bool completed)
		{
			if (bytes == null || chars == null)
			{
				throw new ArgumentNullException((bytes == null) ? "bytes" : "chars", "Array cannot be null.");
			}
			if (byteIndex < 0 || byteCount < 0)
			{
				throw new ArgumentOutOfRangeException((byteIndex < 0) ? "byteIndex" : "byteCount", "Non-negative number required.");
			}
			if (charIndex < 0 || charCount < 0)
			{
				throw new ArgumentOutOfRangeException((charIndex < 0) ? "charIndex" : "charCount", "Non-negative number required.");
			}
			if (bytes.Length - byteIndex < byteCount)
			{
				throw new ArgumentOutOfRangeException("bytes", "Index and count must refer to a location within the buffer.");
			}
			if (chars.Length - charIndex < charCount)
			{
				throw new ArgumentOutOfRangeException("chars", "Index and count must refer to a location within the buffer.");
			}
			for (bytesUsed = byteCount; bytesUsed > 0; bytesUsed /= 2)
			{
				if (this.GetCharCount(bytes, byteIndex, bytesUsed, flush) <= charCount)
				{
					charsUsed = this.GetChars(bytes, byteIndex, bytesUsed, chars, charIndex, flush);
					completed = (bytesUsed == byteCount && (this._fallbackBuffer == null || this._fallbackBuffer.Remaining == 0));
					return;
				}
				flush = false;
			}
			throw new ArgumentException("Conversion buffer overflow.");
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x00085B54 File Offset: 0x00083D54
		[CLSCompliant(false)]
		public unsafe virtual void Convert(byte* bytes, int byteCount, char* chars, int charCount, bool flush, out int bytesUsed, out int charsUsed, out bool completed)
		{
			if (chars == null || bytes == null)
			{
				throw new ArgumentNullException((chars == null) ? "chars" : "bytes", "Array cannot be null.");
			}
			if (byteCount < 0 || charCount < 0)
			{
				throw new ArgumentOutOfRangeException((byteCount < 0) ? "byteCount" : "charCount", "Non-negative number required.");
			}
			for (bytesUsed = byteCount; bytesUsed > 0; bytesUsed /= 2)
			{
				if (this.GetCharCount(bytes, bytesUsed, flush) <= charCount)
				{
					charsUsed = this.GetChars(bytes, bytesUsed, chars, charCount, flush);
					completed = (bytesUsed == byteCount && (this._fallbackBuffer == null || this._fallbackBuffer.Remaining == 0));
					return;
				}
				flush = false;
			}
			throw new ArgumentException("Conversion buffer overflow.");
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x00085C14 File Offset: 0x00083E14
		public unsafe virtual void Convert(ReadOnlySpan<byte> bytes, Span<char> chars, bool flush, out int bytesUsed, out int charsUsed, out bool completed)
		{
			fixed (byte* nonNullPinnableReference = MemoryMarshal.GetNonNullPinnableReference<byte>(bytes))
			{
				byte* bytes2 = nonNullPinnableReference;
				fixed (char* nonNullPinnableReference2 = MemoryMarshal.GetNonNullPinnableReference<char>(chars))
				{
					char* chars2 = nonNullPinnableReference2;
					this.Convert(bytes2, bytes.Length, chars2, chars.Length, flush, out bytesUsed, out charsUsed, out completed);
				}
			}
		}

		// Token: 0x04001D8C RID: 7564
		internal DecoderFallback _fallback;

		// Token: 0x04001D8D RID: 7565
		internal DecoderFallbackBuffer _fallbackBuffer;
	}
}
