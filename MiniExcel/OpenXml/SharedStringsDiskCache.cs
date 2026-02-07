using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MiniExcelLibs.OpenXml
{
	// Token: 0x0200004B RID: 75
	internal class SharedStringsDiskCache : IDictionary<int, string>, ICollection<KeyValuePair<int, string>>, IEnumerable<KeyValuePair<int, string>>, IEnumerable, IDisposable
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000A94E File Offset: 0x00008B4E
		public int Count
		{
			get
			{
				return checked((int)(this._maxIndx + 1L));
			}
		}

		// Token: 0x17000061 RID: 97
		public string this[int key]
		{
			get
			{
				return this.GetValue(key);
			}
			set
			{
				this.Add(key, value);
			}
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000A96D File Offset: 0x00008B6D
		public bool ContainsKey(int key)
		{
			return (long)key <= this._maxIndx;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000A97C File Offset: 0x00008B7C
		public SharedStringsDiskCache()
		{
			string str = Guid.NewGuid().ToString() + "_miniexcelcache";
			this._positionFs = new FileStream(str + "_position", FileMode.OpenOrCreate);
			this._lengthFs = new FileStream(str + "_length", FileMode.OpenOrCreate);
			this._valueFs = new FileStream(str + "_data", FileMode.OpenOrCreate);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000A9FC File Offset: 0x00008BFC
		internal void Add(int index, string value)
		{
			if ((long)index > this._maxIndx)
			{
				this._maxIndx = (long)index;
			}
			byte[] bytes = SharedStringsDiskCache._encoding.GetBytes(value);
			if (value.Length > 32767)
			{
				throw new ArgumentOutOfRangeException("Excel one cell max length is 32,767 characters");
			}
			this._positionFs.Write(BitConverter.GetBytes(this._valueFs.Position), 0, 4);
			this._lengthFs.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
			this._valueFs.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000AA84 File Offset: 0x00008C84
		private string GetValue(int index)
		{
			this._positionFs.Position = (long)(index * 4);
			byte[] array = new byte[4];
			this._positionFs.Read(array, 0, 4);
			int num = BitConverter.ToInt32(array, 0);
			this._lengthFs.Position = (long)(index * 4);
			this._lengthFs.Read(array, 0, 4);
			int num2 = BitConverter.ToInt32(array, 0);
			this._valueFs.Position = (long)num;
			array = new byte[num2];
			this._valueFs.Read(array, 0, num2);
			return SharedStringsDiskCache._encoding.GetString(array);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000AB14 File Offset: 0x00008D14
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposedValue)
			{
				this._positionFs.Dispose();
				if (File.Exists(this._positionFs.Name))
				{
					File.Delete(this._positionFs.Name);
				}
				this._lengthFs.Dispose();
				if (File.Exists(this._lengthFs.Name))
				{
					File.Delete(this._lengthFs.Name);
				}
				this._valueFs.Dispose();
				if (File.Exists(this._valueFs.Name))
				{
					File.Delete(this._valueFs.Name);
				}
				this._disposedValue = true;
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000ABBC File Offset: 0x00008DBC
		~SharedStringsDiskCache()
		{
			this.Dispose(false);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000ABEC File Offset: 0x00008DEC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000ABFB File Offset: 0x00008DFB
		public ICollection<int> Keys
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000AC02 File Offset: 0x00008E02
		public ICollection<string> Values
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000AC09 File Offset: 0x00008E09
		public bool IsReadOnly
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000AC10 File Offset: 0x00008E10
		public bool Remove(int key)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000AC17 File Offset: 0x00008E17
		public bool TryGetValue(int key, out string value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000AC1E File Offset: 0x00008E1E
		public void Add(KeyValuePair<int, string> item)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000AC25 File Offset: 0x00008E25
		public void Clear()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000AC2C File Offset: 0x00008E2C
		public bool Contains(KeyValuePair<int, string> item)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000AC33 File Offset: 0x00008E33
		public void CopyTo(KeyValuePair<int, string>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000AC3A File Offset: 0x00008E3A
		public bool Remove(KeyValuePair<int, string> item)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000AC41 File Offset: 0x00008E41
		public IEnumerator<KeyValuePair<int, string>> GetEnumerator()
		{
			int i = 0;
			while ((long)i < this._maxIndx)
			{
				yield return new KeyValuePair<int, string>(i, this[i]);
				int num = i;
				i = num + 1;
			}
			yield break;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000AC50 File Offset: 0x00008E50
		IEnumerator IEnumerable.GetEnumerator()
		{
			int i = 0;
			while ((long)i < this._maxIndx)
			{
				yield return this[i];
				int num = i;
				i = num + 1;
			}
			yield break;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000AC5F File Offset: 0x00008E5F
		void IDictionary<int, string>.Add(int key, string value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040000D1 RID: 209
		private readonly FileStream _positionFs;

		// Token: 0x040000D2 RID: 210
		private readonly FileStream _lengthFs;

		// Token: 0x040000D3 RID: 211
		private readonly FileStream _valueFs;

		// Token: 0x040000D4 RID: 212
		private bool _disposedValue;

		// Token: 0x040000D5 RID: 213
		private static readonly Encoding _encoding = new UTF8Encoding(true);

		// Token: 0x040000D6 RID: 214
		private long _maxIndx = -1L;
	}
}
