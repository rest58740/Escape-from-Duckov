using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Mono.Security;

namespace System.IO
{
	// Token: 0x02000B57 RID: 2903
	[ComVisible(true)]
	public class BinaryReader : IDisposable
	{
		// Token: 0x060068FA RID: 26874 RVA: 0x0016686B File Offset: 0x00164A6B
		public BinaryReader(Stream input) : this(input, new UTF8Encoding(), false)
		{
		}

		// Token: 0x060068FB RID: 26875 RVA: 0x0016687A File Offset: 0x00164A7A
		public BinaryReader(Stream input, Encoding encoding) : this(input, encoding, false)
		{
		}

		// Token: 0x060068FC RID: 26876 RVA: 0x00166888 File Offset: 0x00164A88
		public BinaryReader(Stream input, Encoding encoding, bool leaveOpen)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (!input.CanRead)
			{
				throw new ArgumentException(Environment.GetResourceString("Stream was not readable."));
			}
			this.m_stream = input;
			this.m_decoder = encoding.GetDecoder();
			this.m_maxCharsSize = encoding.GetMaxCharCount(128);
			int num = encoding.GetMaxByteCount(1);
			if (num < 16)
			{
				num = 16;
			}
			this.m_buffer = new byte[num];
			this.m_2BytesPerChar = (encoding is UnicodeEncoding);
			this.m_isMemoryStream = (this.m_stream.GetType() == typeof(MemoryStream));
			this.m_leaveOpen = leaveOpen;
		}

		// Token: 0x17001225 RID: 4645
		// (get) Token: 0x060068FD RID: 26877 RVA: 0x00166945 File Offset: 0x00164B45
		public virtual Stream BaseStream
		{
			get
			{
				return this.m_stream;
			}
		}

		// Token: 0x060068FE RID: 26878 RVA: 0x0016694D File Offset: 0x00164B4D
		public virtual void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x060068FF RID: 26879 RVA: 0x00166958 File Offset: 0x00164B58
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Stream stream = this.m_stream;
				this.m_stream = null;
				if (stream != null && !this.m_leaveOpen)
				{
					stream.Close();
				}
			}
			this.m_stream = null;
			this.m_buffer = null;
			this.m_decoder = null;
			this.m_charBytes = null;
			this.m_singleChar = null;
			this.m_charBuffer = null;
		}

		// Token: 0x06006900 RID: 26880 RVA: 0x0016694D File Offset: 0x00164B4D
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06006901 RID: 26881 RVA: 0x001669B4 File Offset: 0x00164BB4
		public virtual int PeekChar()
		{
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			if (!this.m_stream.CanSeek)
			{
				return -1;
			}
			long position = this.m_stream.Position;
			int result = this.Read();
			this.m_stream.Position = position;
			return result;
		}

		// Token: 0x06006902 RID: 26882 RVA: 0x001669FB File Offset: 0x00164BFB
		public virtual int Read()
		{
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			return this.InternalReadOneChar();
		}

		// Token: 0x06006903 RID: 26883 RVA: 0x00166A10 File Offset: 0x00164C10
		public virtual bool ReadBoolean()
		{
			this.FillBuffer(1);
			return this.m_buffer[0] > 0;
		}

		// Token: 0x06006904 RID: 26884 RVA: 0x00166A24 File Offset: 0x00164C24
		public virtual byte ReadByte()
		{
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			int num = this.m_stream.ReadByte();
			if (num == -1)
			{
				__Error.EndOfFile();
			}
			return (byte)num;
		}

		// Token: 0x06006905 RID: 26885 RVA: 0x00166A48 File Offset: 0x00164C48
		[CLSCompliant(false)]
		public virtual sbyte ReadSByte()
		{
			this.FillBuffer(1);
			return (sbyte)this.m_buffer[0];
		}

		// Token: 0x06006906 RID: 26886 RVA: 0x00166A5A File Offset: 0x00164C5A
		public virtual char ReadChar()
		{
			int num = this.Read();
			if (num == -1)
			{
				__Error.EndOfFile();
			}
			return (char)num;
		}

		// Token: 0x06006907 RID: 26887 RVA: 0x00166A6C File Offset: 0x00164C6C
		public virtual short ReadInt16()
		{
			this.FillBuffer(2);
			return (short)((int)this.m_buffer[0] | (int)this.m_buffer[1] << 8);
		}

		// Token: 0x06006908 RID: 26888 RVA: 0x00166A89 File Offset: 0x00164C89
		[CLSCompliant(false)]
		public virtual ushort ReadUInt16()
		{
			this.FillBuffer(2);
			return (ushort)((int)this.m_buffer[0] | (int)this.m_buffer[1] << 8);
		}

		// Token: 0x06006909 RID: 26889 RVA: 0x00166AA8 File Offset: 0x00164CA8
		public virtual int ReadInt32()
		{
			if (this.m_isMemoryStream)
			{
				if (this.m_stream == null)
				{
					__Error.FileNotOpen();
				}
				return (this.m_stream as MemoryStream).InternalReadInt32();
			}
			this.FillBuffer(4);
			return (int)this.m_buffer[0] | (int)this.m_buffer[1] << 8 | (int)this.m_buffer[2] << 16 | (int)this.m_buffer[3] << 24;
		}

		// Token: 0x0600690A RID: 26890 RVA: 0x00166B0D File Offset: 0x00164D0D
		[CLSCompliant(false)]
		public virtual uint ReadUInt32()
		{
			this.FillBuffer(4);
			return (uint)((int)this.m_buffer[0] | (int)this.m_buffer[1] << 8 | (int)this.m_buffer[2] << 16 | (int)this.m_buffer[3] << 24);
		}

		// Token: 0x0600690B RID: 26891 RVA: 0x00166B44 File Offset: 0x00164D44
		public virtual long ReadInt64()
		{
			this.FillBuffer(8);
			uint num = (uint)((int)this.m_buffer[0] | (int)this.m_buffer[1] << 8 | (int)this.m_buffer[2] << 16 | (int)this.m_buffer[3] << 24);
			return (long)((ulong)((int)this.m_buffer[4] | (int)this.m_buffer[5] << 8 | (int)this.m_buffer[6] << 16 | (int)this.m_buffer[7] << 24) << 32 | (ulong)num);
		}

		// Token: 0x0600690C RID: 26892 RVA: 0x00166BB8 File Offset: 0x00164DB8
		[CLSCompliant(false)]
		public virtual ulong ReadUInt64()
		{
			this.FillBuffer(8);
			uint num = (uint)((int)this.m_buffer[0] | (int)this.m_buffer[1] << 8 | (int)this.m_buffer[2] << 16 | (int)this.m_buffer[3] << 24);
			return (ulong)((int)this.m_buffer[4] | (int)this.m_buffer[5] << 8 | (int)this.m_buffer[6] << 16 | (int)this.m_buffer[7] << 24) << 32 | (ulong)num;
		}

		// Token: 0x0600690D RID: 26893 RVA: 0x00166C2A File Offset: 0x00164E2A
		[SecuritySafeCritical]
		public virtual float ReadSingle()
		{
			this.FillBuffer(4);
			return BitConverterLE.ToSingle(this.m_buffer, 0);
		}

		// Token: 0x0600690E RID: 26894 RVA: 0x00166C3F File Offset: 0x00164E3F
		[SecuritySafeCritical]
		public virtual double ReadDouble()
		{
			this.FillBuffer(8);
			return BitConverterLE.ToDouble(this.m_buffer, 0);
		}

		// Token: 0x0600690F RID: 26895 RVA: 0x00166C54 File Offset: 0x00164E54
		public virtual decimal ReadDecimal()
		{
			this.FillBuffer(16);
			decimal result;
			try
			{
				int[] array = new int[4];
				Buffer.BlockCopy(this.m_buffer, 0, array, 0, 16);
				if (!BitConverter.IsLittleEndian)
				{
					for (int i = 0; i < 4; i++)
					{
						array[i] = BinaryPrimitives.ReverseEndianness(array[i]);
					}
				}
				result = new decimal(array);
			}
			catch (ArgumentException innerException)
			{
				throw new IOException(Environment.GetResourceString("Decimal byte array constructor requires an array of length four containing valid decimal bytes."), innerException);
			}
			return result;
		}

		// Token: 0x06006910 RID: 26896 RVA: 0x00166CCC File Offset: 0x00164ECC
		public virtual string ReadString()
		{
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			int num = 0;
			int num2 = this.Read7BitEncodedInt();
			if (num2 < 0)
			{
				throw new IOException(Environment.GetResourceString("BinaryReader encountered an invalid string length of {0} characters.", new object[]
				{
					num2
				}));
			}
			if (num2 == 0)
			{
				return string.Empty;
			}
			if (this.m_charBytes == null)
			{
				this.m_charBytes = new byte[128];
			}
			if (this.m_charBuffer == null)
			{
				this.m_charBuffer = new char[this.m_maxCharsSize];
			}
			StringBuilder stringBuilder = null;
			int chars;
			for (;;)
			{
				int count = (num2 - num > 128) ? 128 : (num2 - num);
				int num3 = this.m_stream.Read(this.m_charBytes, 0, count);
				if (num3 == 0)
				{
					__Error.EndOfFile();
				}
				chars = this.m_decoder.GetChars(this.m_charBytes, 0, num3, this.m_charBuffer, 0);
				if (num == 0 && num3 == num2)
				{
					break;
				}
				if (stringBuilder == null)
				{
					stringBuilder = StringBuilderCache.Acquire(num2);
				}
				stringBuilder.Append(this.m_charBuffer, 0, chars);
				num += num3;
				if (num >= num2)
				{
					goto Block_11;
				}
			}
			return new string(this.m_charBuffer, 0, chars);
			Block_11:
			return StringBuilderCache.GetStringAndRelease(stringBuilder);
		}

		// Token: 0x06006911 RID: 26897 RVA: 0x00166DE4 File Offset: 0x00164FE4
		[SecuritySafeCritical]
		public virtual int Read(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("Buffer cannot be null."));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("Non-negative number required."));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("Non-negative number required."));
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection."));
			}
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			return this.InternalReadChars(buffer, index, count);
		}

		// Token: 0x06006912 RID: 26898 RVA: 0x00166E6C File Offset: 0x0016506C
		[SecurityCritical]
		private unsafe int InternalReadChars(char[] buffer, int index, int count)
		{
			int i = count;
			if (this.m_charBytes == null)
			{
				this.m_charBytes = new byte[128];
			}
			while (i > 0)
			{
				int num = i;
				DecoderNLS decoderNLS = this.m_decoder as DecoderNLS;
				if (decoderNLS != null && decoderNLS.HasState && num > 1)
				{
					num--;
				}
				if (this.m_2BytesPerChar)
				{
					num <<= 1;
				}
				if (num > 128)
				{
					num = 128;
				}
				int num2 = 0;
				byte[] array;
				if (this.m_isMemoryStream)
				{
					MemoryStream memoryStream = this.m_stream as MemoryStream;
					num2 = memoryStream.InternalGetPosition();
					num = memoryStream.InternalEmulateRead(num);
					array = memoryStream.InternalGetBuffer();
				}
				else
				{
					num = this.m_stream.Read(this.m_charBytes, 0, num);
					array = this.m_charBytes;
				}
				if (num == 0)
				{
					return count - i;
				}
				int chars;
				checked
				{
					if (num2 < 0 || num < 0 || num2 + num > array.Length)
					{
						throw new ArgumentOutOfRangeException("byteCount");
					}
					if (index < 0 || i < 0 || index + i > buffer.Length)
					{
						throw new ArgumentOutOfRangeException("charsRemaining");
					}
					byte[] array2;
					byte* ptr;
					if ((array2 = array) == null || array2.Length == 0)
					{
						ptr = null;
					}
					else
					{
						ptr = &array2[0];
					}
					fixed (char[] array3 = buffer)
					{
						char* ptr2;
						if (buffer == null || array3.Length == 0)
						{
							ptr2 = null;
						}
						else
						{
							ptr2 = &array3[0];
						}
						chars = this.m_decoder.GetChars(ptr + num2, num, ptr2 + index, i, false);
					}
					array2 = null;
				}
				i -= chars;
				index += chars;
			}
			return count - i;
		}

		// Token: 0x06006913 RID: 26899 RVA: 0x00166FD8 File Offset: 0x001651D8
		private int InternalReadOneChar()
		{
			int num = 0;
			long num2 = 0L;
			if (this.m_stream.CanSeek)
			{
				num2 = this.m_stream.Position;
			}
			if (this.m_charBytes == null)
			{
				this.m_charBytes = new byte[128];
			}
			if (this.m_singleChar == null)
			{
				this.m_singleChar = new char[1];
			}
			while (num == 0)
			{
				int num3 = this.m_2BytesPerChar ? 2 : 1;
				int num4 = this.m_stream.ReadByte();
				this.m_charBytes[0] = (byte)num4;
				if (num4 == -1)
				{
					num3 = 0;
				}
				if (num3 == 2)
				{
					num4 = this.m_stream.ReadByte();
					this.m_charBytes[1] = (byte)num4;
					if (num4 == -1)
					{
						num3 = 1;
					}
				}
				if (num3 == 0)
				{
					return -1;
				}
				try
				{
					num = this.m_decoder.GetChars(this.m_charBytes, 0, num3, this.m_singleChar, 0);
				}
				catch
				{
					if (this.m_stream.CanSeek)
					{
						this.m_stream.Seek(num2 - this.m_stream.Position, SeekOrigin.Current);
					}
					throw;
				}
			}
			if (num == 0)
			{
				return -1;
			}
			return (int)this.m_singleChar[0];
		}

		// Token: 0x06006914 RID: 26900 RVA: 0x001670F4 File Offset: 0x001652F4
		[SecuritySafeCritical]
		public virtual char[] ReadChars(int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("Non-negative number required."));
			}
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			if (count == 0)
			{
				return EmptyArray<char>.Value;
			}
			char[] array = new char[count];
			int num = this.InternalReadChars(array, 0, count);
			if (num != count)
			{
				char[] array2 = new char[num];
				Buffer.InternalBlockCopy(array, 0, array2, 0, 2 * num);
				array = array2;
			}
			return array;
		}

		// Token: 0x06006915 RID: 26901 RVA: 0x0016715C File Offset: 0x0016535C
		public virtual int Read(Span<char> buffer)
		{
			char[] array = ArrayPool<char>.Shared.Rent(buffer.Length);
			int result;
			try
			{
				int num = this.InternalReadChars(array, 0, buffer.Length);
				if (num > buffer.Length)
				{
					throw new IOException("Stream was too long.");
				}
				new ReadOnlySpan<char>(array, 0, num).CopyTo(buffer);
				result = num;
			}
			finally
			{
				ArrayPool<char>.Shared.Return(array, false);
			}
			return result;
		}

		// Token: 0x06006916 RID: 26902 RVA: 0x001671D4 File Offset: 0x001653D4
		public virtual int Read(Span<byte> buffer)
		{
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			return this.m_stream.Read(buffer);
		}

		// Token: 0x06006917 RID: 26903 RVA: 0x001671F0 File Offset: 0x001653F0
		public virtual int Read(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("Buffer cannot be null."));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("Non-negative number required."));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("Non-negative number required."));
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection."));
			}
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			return this.m_stream.Read(buffer, index, count);
		}

		// Token: 0x06006918 RID: 26904 RVA: 0x0016727C File Offset: 0x0016547C
		public virtual byte[] ReadBytes(int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("Non-negative number required."));
			}
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			if (count == 0)
			{
				return EmptyArray<byte>.Value;
			}
			byte[] array = new byte[count];
			int num = 0;
			do
			{
				int num2 = this.m_stream.Read(array, num, count);
				if (num2 == 0)
				{
					break;
				}
				num += num2;
				count -= num2;
			}
			while (count > 0);
			if (num != array.Length)
			{
				byte[] array2 = new byte[num];
				Buffer.InternalBlockCopy(array, 0, array2, 0, num);
				array = array2;
			}
			return array;
		}

		// Token: 0x06006919 RID: 26905 RVA: 0x001672FC File Offset: 0x001654FC
		protected virtual void FillBuffer(int numBytes)
		{
			if (this.m_buffer != null && (numBytes < 0 || numBytes > this.m_buffer.Length))
			{
				throw new ArgumentOutOfRangeException("numBytes", Environment.GetResourceString("The number of bytes requested does not fit into BinaryReader's internal buffer."));
			}
			int num = 0;
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			if (numBytes == 1)
			{
				int num2 = this.m_stream.ReadByte();
				if (num2 == -1)
				{
					__Error.EndOfFile();
				}
				this.m_buffer[0] = (byte)num2;
				return;
			}
			do
			{
				int num2 = this.m_stream.Read(this.m_buffer, num, numBytes - num);
				if (num2 == 0)
				{
					__Error.EndOfFile();
				}
				num += num2;
			}
			while (num < numBytes);
		}

		// Token: 0x0600691A RID: 26906 RVA: 0x00167390 File Offset: 0x00165590
		protected internal int Read7BitEncodedInt()
		{
			int num = 0;
			int num2 = 0;
			while (num2 != 35)
			{
				byte b = this.ReadByte();
				num |= (int)(b & 127) << num2;
				num2 += 7;
				if ((b & 128) == 0)
				{
					return num;
				}
			}
			throw new FormatException(Environment.GetResourceString("Too many bytes in what should have been a 7 bit encoded Int32."));
		}

		// Token: 0x04003D0F RID: 15631
		private const int MaxCharBytesSize = 128;

		// Token: 0x04003D10 RID: 15632
		private Stream m_stream;

		// Token: 0x04003D11 RID: 15633
		private byte[] m_buffer;

		// Token: 0x04003D12 RID: 15634
		private Decoder m_decoder;

		// Token: 0x04003D13 RID: 15635
		private byte[] m_charBytes;

		// Token: 0x04003D14 RID: 15636
		private char[] m_singleChar;

		// Token: 0x04003D15 RID: 15637
		private char[] m_charBuffer;

		// Token: 0x04003D16 RID: 15638
		private int m_maxCharsSize;

		// Token: 0x04003D17 RID: 15639
		private bool m_2BytesPerChar;

		// Token: 0x04003D18 RID: 15640
		private bool m_isMemoryStream;

		// Token: 0x04003D19 RID: 15641
		private bool m_leaveOpen;
	}
}
