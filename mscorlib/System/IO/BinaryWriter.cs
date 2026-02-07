using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Mono.Security;

namespace System.IO
{
	// Token: 0x02000B58 RID: 2904
	[ComVisible(true)]
	[Serializable]
	public class BinaryWriter : IDisposable, IAsyncDisposable
	{
		// Token: 0x0600691B RID: 26907 RVA: 0x001673D7 File Offset: 0x001655D7
		protected BinaryWriter()
		{
			this.OutStream = Stream.Null;
			this._buffer = new byte[16];
			this._encoding = new UTF8Encoding(false, true);
			this._encoder = this._encoding.GetEncoder();
		}

		// Token: 0x0600691C RID: 26908 RVA: 0x00167415 File Offset: 0x00165615
		public BinaryWriter(Stream output) : this(output, new UTF8Encoding(false, true), false)
		{
		}

		// Token: 0x0600691D RID: 26909 RVA: 0x00167426 File Offset: 0x00165626
		public BinaryWriter(Stream output, Encoding encoding) : this(output, encoding, false)
		{
		}

		// Token: 0x0600691E RID: 26910 RVA: 0x00167434 File Offset: 0x00165634
		public BinaryWriter(Stream output, Encoding encoding, bool leaveOpen)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (!output.CanWrite)
			{
				throw new ArgumentException(Environment.GetResourceString("Stream was not writable."));
			}
			this.OutStream = output;
			this._buffer = new byte[16];
			this._encoding = encoding;
			this._encoder = this._encoding.GetEncoder();
			this._leaveOpen = leaveOpen;
		}

		// Token: 0x0600691F RID: 26911 RVA: 0x001674AE File Offset: 0x001656AE
		public virtual void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06006920 RID: 26912 RVA: 0x001674B7 File Offset: 0x001656B7
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._leaveOpen)
				{
					this.OutStream.Flush();
					return;
				}
				this.OutStream.Close();
			}
		}

		// Token: 0x06006921 RID: 26913 RVA: 0x001674AE File Offset: 0x001656AE
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x17001226 RID: 4646
		// (get) Token: 0x06006922 RID: 26914 RVA: 0x001674DB File Offset: 0x001656DB
		public virtual Stream BaseStream
		{
			get
			{
				this.Flush();
				return this.OutStream;
			}
		}

		// Token: 0x06006923 RID: 26915 RVA: 0x001674E9 File Offset: 0x001656E9
		public virtual void Flush()
		{
			this.OutStream.Flush();
		}

		// Token: 0x06006924 RID: 26916 RVA: 0x001674F6 File Offset: 0x001656F6
		public virtual long Seek(int offset, SeekOrigin origin)
		{
			return this.OutStream.Seek((long)offset, origin);
		}

		// Token: 0x06006925 RID: 26917 RVA: 0x00167506 File Offset: 0x00165706
		public virtual void Write(ReadOnlySpan<byte> buffer)
		{
			this.Write(buffer.ToArray());
		}

		// Token: 0x06006926 RID: 26918 RVA: 0x00167515 File Offset: 0x00165715
		public virtual void Write(ReadOnlySpan<char> buffer)
		{
			this.Write(buffer.ToArray());
		}

		// Token: 0x06006927 RID: 26919 RVA: 0x00167524 File Offset: 0x00165724
		public virtual ValueTask DisposeAsync()
		{
			ValueTask result;
			try
			{
				if (base.GetType() == typeof(BinaryWriter))
				{
					if (this._leaveOpen)
					{
						return new ValueTask(this.OutStream.FlushAsync());
					}
					this.OutStream.Close();
				}
				else
				{
					this.Dispose();
				}
				result = default(ValueTask);
			}
			catch (Exception exception)
			{
				result = new ValueTask(Task.FromException(exception));
			}
			return result;
		}

		// Token: 0x06006928 RID: 26920 RVA: 0x001675A0 File Offset: 0x001657A0
		public virtual void Write(bool value)
		{
			this._buffer[0] = (value ? 1 : 0);
			this.OutStream.Write(this._buffer, 0, 1);
		}

		// Token: 0x06006929 RID: 26921 RVA: 0x001675C5 File Offset: 0x001657C5
		public virtual void Write(byte value)
		{
			this.OutStream.WriteByte(value);
		}

		// Token: 0x0600692A RID: 26922 RVA: 0x001675D3 File Offset: 0x001657D3
		[CLSCompliant(false)]
		public virtual void Write(sbyte value)
		{
			this.OutStream.WriteByte((byte)value);
		}

		// Token: 0x0600692B RID: 26923 RVA: 0x001675E2 File Offset: 0x001657E2
		public virtual void Write(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			this.OutStream.Write(buffer, 0, buffer.Length);
		}

		// Token: 0x0600692C RID: 26924 RVA: 0x00167602 File Offset: 0x00165802
		public virtual void Write(byte[] buffer, int index, int count)
		{
			this.OutStream.Write(buffer, index, count);
		}

		// Token: 0x0600692D RID: 26925 RVA: 0x00167614 File Offset: 0x00165814
		[SecuritySafeCritical]
		public unsafe virtual void Write(char ch)
		{
			if (char.IsSurrogate(ch))
			{
				throw new ArgumentException(Environment.GetResourceString("Unicode surrogate characters must be written out as pairs together in the same call, not individually. Consider passing in a character array instead."));
			}
			byte[] array;
			byte* bytes;
			if ((array = this._buffer) == null || array.Length == 0)
			{
				bytes = null;
			}
			else
			{
				bytes = &array[0];
			}
			int bytes2 = this._encoder.GetBytes(&ch, 1, bytes, this._buffer.Length, true);
			array = null;
			this.OutStream.Write(this._buffer, 0, bytes2);
		}

		// Token: 0x0600692E RID: 26926 RVA: 0x00167688 File Offset: 0x00165888
		public virtual void Write(char[] chars)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			byte[] bytes = this._encoding.GetBytes(chars, 0, chars.Length);
			this.OutStream.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x0600692F RID: 26927 RVA: 0x001676C4 File Offset: 0x001658C4
		public virtual void Write(char[] chars, int index, int count)
		{
			byte[] bytes = this._encoding.GetBytes(chars, index, count);
			this.OutStream.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06006930 RID: 26928 RVA: 0x001676F0 File Offset: 0x001658F0
		[SecuritySafeCritical]
		public virtual void Write(double value)
		{
			this.OutStream.Write(BitConverterLE.GetBytes(value), 0, 8);
		}

		// Token: 0x06006931 RID: 26929 RVA: 0x00167705 File Offset: 0x00165905
		public virtual void Write(decimal value)
		{
			decimal.GetBytes(value, this._buffer);
			this.OutStream.Write(this._buffer, 0, 16);
		}

		// Token: 0x06006932 RID: 26930 RVA: 0x00167728 File Offset: 0x00165928
		public virtual void Write(short value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this.OutStream.Write(this._buffer, 0, 2);
		}

		// Token: 0x06006933 RID: 26931 RVA: 0x00167728 File Offset: 0x00165928
		[CLSCompliant(false)]
		public virtual void Write(ushort value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this.OutStream.Write(this._buffer, 0, 2);
		}

		// Token: 0x06006934 RID: 26932 RVA: 0x00167754 File Offset: 0x00165954
		public virtual void Write(int value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this._buffer[2] = (byte)(value >> 16);
			this._buffer[3] = (byte)(value >> 24);
			this.OutStream.Write(this._buffer, 0, 4);
		}

		// Token: 0x06006935 RID: 26933 RVA: 0x001677A4 File Offset: 0x001659A4
		[CLSCompliant(false)]
		public virtual void Write(uint value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this._buffer[2] = (byte)(value >> 16);
			this._buffer[3] = (byte)(value >> 24);
			this.OutStream.Write(this._buffer, 0, 4);
		}

		// Token: 0x06006936 RID: 26934 RVA: 0x001677F4 File Offset: 0x001659F4
		public virtual void Write(long value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this._buffer[2] = (byte)(value >> 16);
			this._buffer[3] = (byte)(value >> 24);
			this._buffer[4] = (byte)(value >> 32);
			this._buffer[5] = (byte)(value >> 40);
			this._buffer[6] = (byte)(value >> 48);
			this._buffer[7] = (byte)(value >> 56);
			this.OutStream.Write(this._buffer, 0, 8);
		}

		// Token: 0x06006937 RID: 26935 RVA: 0x00167878 File Offset: 0x00165A78
		[CLSCompliant(false)]
		public virtual void Write(ulong value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this._buffer[2] = (byte)(value >> 16);
			this._buffer[3] = (byte)(value >> 24);
			this._buffer[4] = (byte)(value >> 32);
			this._buffer[5] = (byte)(value >> 40);
			this._buffer[6] = (byte)(value >> 48);
			this._buffer[7] = (byte)(value >> 56);
			this.OutStream.Write(this._buffer, 0, 8);
		}

		// Token: 0x06006938 RID: 26936 RVA: 0x001678FC File Offset: 0x00165AFC
		[SecuritySafeCritical]
		public virtual void Write(float value)
		{
			this.OutStream.Write(BitConverterLE.GetBytes(value), 0, 4);
		}

		// Token: 0x06006939 RID: 26937 RVA: 0x00167914 File Offset: 0x00165B14
		[SecuritySafeCritical]
		public unsafe virtual void Write(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int byteCount = this._encoding.GetByteCount(value);
			this.Write7BitEncodedInt(byteCount);
			if (this._largeByteBuffer == null)
			{
				this._largeByteBuffer = new byte[256];
				this._maxChars = this._largeByteBuffer.Length / this._encoding.GetMaxByteCount(1);
			}
			if (byteCount <= this._largeByteBuffer.Length)
			{
				this._encoding.GetBytes(value, 0, value.Length, this._largeByteBuffer, 0);
				this.OutStream.Write(this._largeByteBuffer, 0, byteCount);
				return;
			}
			int num = 0;
			int num2;
			for (int i = value.Length; i > 0; i -= num2)
			{
				num2 = ((i > this._maxChars) ? this._maxChars : i);
				if (num < 0 || num2 < 0 || checked(num + num2) > value.Length)
				{
					throw new ArgumentOutOfRangeException("charCount");
				}
				int bytes2;
				fixed (string text = value)
				{
					char* ptr = text;
					if (ptr != null)
					{
						ptr += RuntimeHelpers.OffsetToStringData / 2;
					}
					byte[] array;
					byte* bytes;
					if ((array = this._largeByteBuffer) == null || array.Length == 0)
					{
						bytes = null;
					}
					else
					{
						bytes = &array[0];
					}
					bytes2 = this._encoder.GetBytes(checked(ptr + num), num2, bytes, this._largeByteBuffer.Length, num2 == i);
					array = null;
				}
				this.OutStream.Write(this._largeByteBuffer, 0, bytes2);
				num += num2;
			}
		}

		// Token: 0x0600693A RID: 26938 RVA: 0x00167A74 File Offset: 0x00165C74
		protected void Write7BitEncodedInt(int value)
		{
			uint num;
			for (num = (uint)value; num >= 128U; num >>= 7)
			{
				this.Write((byte)(num | 128U));
			}
			this.Write((byte)num);
		}

		// Token: 0x04003D1A RID: 15642
		public static readonly BinaryWriter Null = new BinaryWriter();

		// Token: 0x04003D1B RID: 15643
		protected Stream OutStream;

		// Token: 0x04003D1C RID: 15644
		private byte[] _buffer;

		// Token: 0x04003D1D RID: 15645
		private Encoding _encoding;

		// Token: 0x04003D1E RID: 15646
		private Encoder _encoder;

		// Token: 0x04003D1F RID: 15647
		[OptionalField]
		private bool _leaveOpen;

		// Token: 0x04003D20 RID: 15648
		[OptionalField]
		private char[] _tmpOneCharBuffer;

		// Token: 0x04003D21 RID: 15649
		private byte[] _largeByteBuffer;

		// Token: 0x04003D22 RID: 15650
		private int _maxChars;

		// Token: 0x04003D23 RID: 15651
		private const int LargeByteBufferSize = 256;
	}
}
