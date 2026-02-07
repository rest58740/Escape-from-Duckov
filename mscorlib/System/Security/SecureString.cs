using System;
using System.Runtime.ExceptionServices;

namespace System.Security
{
	// Token: 0x020003E4 RID: 996
	[MonoTODO("work in progress - encryption is missing")]
	public sealed class SecureString : IDisposable
	{
		// Token: 0x06002900 RID: 10496 RVA: 0x000948C4 File Offset: 0x00092AC4
		public SecureString()
		{
			this.Alloc(8, false);
		}

		// Token: 0x06002901 RID: 10497 RVA: 0x000948D4 File Offset: 0x00092AD4
		[CLSCompliant(false)]
		public unsafe SecureString(char* value, int length)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (length < 0 || length > 65536)
			{
				throw new ArgumentOutOfRangeException("length", "< 0 || > 65536");
			}
			this.length = length;
			this.Alloc(length, false);
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				char c = *(value++);
				this.data[num++] = (byte)(c >> 8);
				this.data[num++] = (byte)c;
			}
			this.Encrypt();
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06002902 RID: 10498 RVA: 0x0009495C File Offset: 0x00092B5C
		public int Length
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException("SecureString");
				}
				return this.length;
			}
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x00094978 File Offset: 0x00092B78
		[HandleProcessCorruptedStateExceptions]
		public void AppendChar(char c)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("SecureString");
			}
			if (this.read_only)
			{
				throw new InvalidOperationException(Locale.GetText("SecureString is read-only."));
			}
			if (this.length == 65536)
			{
				throw new ArgumentOutOfRangeException("length", "> 65536");
			}
			try
			{
				this.Decrypt();
				int num = this.length * 2;
				int num2 = this.length + 1;
				this.length = num2;
				this.Alloc(num2, true);
				this.data[num++] = (byte)(c >> 8);
				this.data[num++] = (byte)c;
			}
			finally
			{
				this.Encrypt();
			}
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x00094A2C File Offset: 0x00092C2C
		public void Clear()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("SecureString");
			}
			if (this.read_only)
			{
				throw new InvalidOperationException(Locale.GetText("SecureString is read-only."));
			}
			Array.Clear(this.data, 0, this.data.Length);
			this.length = 0;
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x00094A7F File Offset: 0x00092C7F
		public SecureString Copy()
		{
			return new SecureString
			{
				data = (byte[])this.data.Clone(),
				length = this.length
			};
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x00094AA8 File Offset: 0x00092CA8
		[SecuritySafeCritical]
		public void Dispose()
		{
			this.disposed = true;
			if (this.data != null)
			{
				Array.Clear(this.data, 0, this.data.Length);
				this.data = null;
			}
			this.length = 0;
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x00094ADC File Offset: 0x00092CDC
		[HandleProcessCorruptedStateExceptions]
		public void InsertAt(int index, char c)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("SecureString");
			}
			if (this.read_only)
			{
				throw new InvalidOperationException(Locale.GetText("SecureString is read-only."));
			}
			if (index < 0 || index > this.length)
			{
				throw new ArgumentOutOfRangeException("index", "< 0 || > length");
			}
			if (this.length >= 65536)
			{
				string text = Locale.GetText("Maximum string size is '{0}'.", new object[]
				{
					65536
				});
				throw new ArgumentOutOfRangeException("index", text);
			}
			try
			{
				this.Decrypt();
				int num = this.length + 1;
				this.length = num;
				this.Alloc(num, true);
				int num2 = index * 2;
				Buffer.BlockCopy(this.data, num2, this.data, num2 + 2, this.data.Length - num2 - 2);
				this.data[num2++] = (byte)(c >> 8);
				this.data[num2] = (byte)c;
			}
			finally
			{
				this.Encrypt();
			}
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x00094BE0 File Offset: 0x00092DE0
		public bool IsReadOnly()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("SecureString");
			}
			return this.read_only;
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x00094BFB File Offset: 0x00092DFB
		public void MakeReadOnly()
		{
			this.read_only = true;
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x00094C04 File Offset: 0x00092E04
		[HandleProcessCorruptedStateExceptions]
		public void RemoveAt(int index)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("SecureString");
			}
			if (this.read_only)
			{
				throw new InvalidOperationException(Locale.GetText("SecureString is read-only."));
			}
			if (index < 0 || index >= this.length)
			{
				throw new ArgumentOutOfRangeException("index", "< 0 || > length");
			}
			try
			{
				this.Decrypt();
				Buffer.BlockCopy(this.data, index * 2 + 2, this.data, index * 2, this.data.Length - index * 2 - 2);
				int num = this.length - 1;
				this.length = num;
				this.Alloc(num, true);
			}
			finally
			{
				this.Encrypt();
			}
		}

		// Token: 0x0600290B RID: 10507 RVA: 0x00094CB8 File Offset: 0x00092EB8
		[HandleProcessCorruptedStateExceptions]
		public void SetAt(int index, char c)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("SecureString");
			}
			if (this.read_only)
			{
				throw new InvalidOperationException(Locale.GetText("SecureString is read-only."));
			}
			if (index < 0 || index >= this.length)
			{
				throw new ArgumentOutOfRangeException("index", "< 0 || > length");
			}
			try
			{
				this.Decrypt();
				int num = index * 2;
				this.data[num++] = (byte)(c >> 8);
				this.data[num] = (byte)c;
			}
			finally
			{
				this.Encrypt();
			}
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x00094D4C File Offset: 0x00092F4C
		private void Encrypt()
		{
			if (this.data != null)
			{
				int num = this.data.Length;
			}
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x00094D4C File Offset: 0x00092F4C
		private void Decrypt()
		{
			if (this.data != null)
			{
				int num = this.data.Length;
			}
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x00094D60 File Offset: 0x00092F60
		private void Alloc(int length, bool realloc)
		{
			if (length < 0 || length > 65536)
			{
				throw new ArgumentOutOfRangeException("length", "< 0 || > 65536");
			}
			int num = (length >> 3) + (((length & 7) == 0) ? 0 : 1) << 4;
			if (realloc && this.data != null && num == this.data.Length)
			{
				return;
			}
			if (realloc)
			{
				byte[] array = new byte[num];
				Array.Copy(this.data, 0, array, 0, Math.Min(this.data.Length, array.Length));
				Array.Clear(this.data, 0, this.data.Length);
				this.data = array;
				return;
			}
			this.data = new byte[num];
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x00094E00 File Offset: 0x00093000
		internal byte[] GetBuffer()
		{
			byte[] array = new byte[this.length << 1];
			try
			{
				this.Decrypt();
				Buffer.BlockCopy(this.data, 0, array, 0, array.Length);
			}
			finally
			{
				this.Encrypt();
			}
			return array;
		}

		// Token: 0x04001ECC RID: 7884
		private const int BlockSize = 16;

		// Token: 0x04001ECD RID: 7885
		private const int MaxSize = 65536;

		// Token: 0x04001ECE RID: 7886
		private int length;

		// Token: 0x04001ECF RID: 7887
		private bool disposed;

		// Token: 0x04001ED0 RID: 7888
		private bool read_only;

		// Token: 0x04001ED1 RID: 7889
		private byte[] data;
	}
}
