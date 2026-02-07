using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace System.IO
{
	// Token: 0x02000B59 RID: 2905
	[ComVisible(true)]
	[Serializable]
	public class StringReader : TextReader
	{
		// Token: 0x0600693C RID: 26940 RVA: 0x00167AB3 File Offset: 0x00165CB3
		public StringReader(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			this._s = s;
			this._length = ((s == null) ? 0 : s.Length);
		}

		// Token: 0x0600693D RID: 26941 RVA: 0x001588BD File Offset: 0x00156ABD
		public override void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x0600693E RID: 26942 RVA: 0x00167AE2 File Offset: 0x00165CE2
		protected override void Dispose(bool disposing)
		{
			this._s = null;
			this._pos = 0;
			this._length = 0;
			base.Dispose(disposing);
		}

		// Token: 0x0600693F RID: 26943 RVA: 0x00167B00 File Offset: 0x00165D00
		public override int Peek()
		{
			if (this._s == null)
			{
				__Error.ReaderClosed();
			}
			if (this._pos == this._length)
			{
				return -1;
			}
			return (int)this._s[this._pos];
		}

		// Token: 0x06006940 RID: 26944 RVA: 0x00167B30 File Offset: 0x00165D30
		public override int Read()
		{
			if (this._s == null)
			{
				__Error.ReaderClosed();
			}
			if (this._pos == this._length)
			{
				return -1;
			}
			string s = this._s;
			int pos = this._pos;
			this._pos = pos + 1;
			return (int)s[pos];
		}

		// Token: 0x06006941 RID: 26945 RVA: 0x00167B78 File Offset: 0x00165D78
		public override int Read([In] [Out] char[] buffer, int index, int count)
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
			if (this._s == null)
			{
				__Error.ReaderClosed();
			}
			int num = this._length - this._pos;
			if (num > 0)
			{
				if (num > count)
				{
					num = count;
				}
				this._s.CopyTo(this._pos, buffer, index, num);
				this._pos += num;
			}
			return num;
		}

		// Token: 0x06006942 RID: 26946 RVA: 0x00167C30 File Offset: 0x00165E30
		public override string ReadToEnd()
		{
			if (this._s == null)
			{
				__Error.ReaderClosed();
			}
			string result;
			if (this._pos == 0)
			{
				result = this._s;
			}
			else
			{
				result = this._s.Substring(this._pos, this._length - this._pos);
			}
			this._pos = this._length;
			return result;
		}

		// Token: 0x06006943 RID: 26947 RVA: 0x00167C88 File Offset: 0x00165E88
		public override string ReadLine()
		{
			if (this._s == null)
			{
				__Error.ReaderClosed();
			}
			int i;
			for (i = this._pos; i < this._length; i++)
			{
				char c = this._s[i];
				if (c == '\r' || c == '\n')
				{
					string result = this._s.Substring(this._pos, i - this._pos);
					this._pos = i + 1;
					if (c == '\r' && this._pos < this._length && this._s[this._pos] == '\n')
					{
						this._pos++;
					}
					return result;
				}
			}
			if (i > this._pos)
			{
				string result2 = this._s.Substring(this._pos, i - this._pos);
				this._pos = i;
				return result2;
			}
			return null;
		}

		// Token: 0x06006944 RID: 26948 RVA: 0x0015C7EF File Offset: 0x0015A9EF
		[ComVisible(false)]
		public override Task<string> ReadLineAsync()
		{
			return Task.FromResult<string>(this.ReadLine());
		}

		// Token: 0x06006945 RID: 26949 RVA: 0x0015C7FC File Offset: 0x0015A9FC
		[ComVisible(false)]
		public override Task<string> ReadToEndAsync()
		{
			return Task.FromResult<string>(this.ReadToEnd());
		}

		// Token: 0x06006946 RID: 26950 RVA: 0x00167D54 File Offset: 0x00165F54
		[ComVisible(false)]
		public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("Buffer cannot be null."));
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("Non-negative number required."));
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection."));
			}
			return Task.FromResult<int>(this.ReadBlock(buffer, index, count));
		}

		// Token: 0x06006947 RID: 26951 RVA: 0x00167DC8 File Offset: 0x00165FC8
		[ComVisible(false)]
		public override Task<int> ReadAsync(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("Buffer cannot be null."));
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException((index < 0) ? "index" : "count", Environment.GetResourceString("Non-negative number required."));
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection."));
			}
			return Task.FromResult<int>(this.Read(buffer, index, count));
		}

		// Token: 0x04003D24 RID: 15652
		private string _s;

		// Token: 0x04003D25 RID: 15653
		private int _pos;

		// Token: 0x04003D26 RID: 15654
		private int _length;
	}
}
