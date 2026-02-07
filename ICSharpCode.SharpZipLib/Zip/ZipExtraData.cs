using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000064 RID: 100
	public sealed class ZipExtraData : IDisposable
	{
		// Token: 0x06000442 RID: 1090 RVA: 0x00018148 File Offset: 0x00016348
		public ZipExtraData()
		{
			this.Clear();
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00018158 File Offset: 0x00016358
		public ZipExtraData(byte[] data)
		{
			if (data == null)
			{
				this._data = new byte[0];
			}
			else
			{
				this._data = data;
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0001818C File Offset: 0x0001638C
		public byte[] GetEntryData()
		{
			if (this.Length > 65535)
			{
				throw new ZipException("Data exceeds maximum length");
			}
			return (byte[])this._data.Clone();
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x000181BC File Offset: 0x000163BC
		public void Clear()
		{
			if (this._data == null || this._data.Length != 0)
			{
				this._data = new byte[0];
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x000181F0 File Offset: 0x000163F0
		public int Length
		{
			get
			{
				return this._data.Length;
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x000181FC File Offset: 0x000163FC
		public Stream GetStreamForTag(int tag)
		{
			Stream result = null;
			if (this.Find(tag))
			{
				result = new MemoryStream(this._data, this._index, this._readValueLength, false);
			}
			return result;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00018234 File Offset: 0x00016434
		private ITaggedData GetData(short tag)
		{
			ITaggedData result = null;
			if (this.Find((int)tag))
			{
				result = ZipExtraData.Create(tag, this._data, this._readValueStart, this._readValueLength);
			}
			return result;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0001826C File Offset: 0x0001646C
		private static ITaggedData Create(short tag, byte[] data, int offset, int count)
		{
			ITaggedData taggedData;
			if (tag != 10)
			{
				if (tag != 21589)
				{
					taggedData = new RawTaggedData(tag);
				}
				else
				{
					taggedData = new ExtendedUnixData();
				}
			}
			else
			{
				taggedData = new NTTaggedData();
			}
			taggedData.SetData(data, offset, count);
			return taggedData;
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x000182C4 File Offset: 0x000164C4
		public int ValueLength
		{
			get
			{
				return this._readValueLength;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x000182CC File Offset: 0x000164CC
		public int CurrentReadIndex
		{
			get
			{
				return this._index;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x000182D4 File Offset: 0x000164D4
		public int UnreadCount
		{
			get
			{
				if (this._readValueStart > this._data.Length || this._readValueStart < 4)
				{
					throw new ZipException("Find must be called before calling a Read method");
				}
				return this._readValueStart + this._readValueLength - this._index;
			}
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00018320 File Offset: 0x00016520
		public bool Find(int headerID)
		{
			this._readValueStart = this._data.Length;
			this._readValueLength = 0;
			this._index = 0;
			int num = this._readValueStart;
			int num2 = headerID - 1;
			while (num2 != headerID && this._index < this._data.Length - 3)
			{
				num2 = this.ReadShortInternal();
				num = this.ReadShortInternal();
				if (num2 != headerID)
				{
					this._index += num;
				}
			}
			bool flag = num2 == headerID && this._index + num <= this._data.Length;
			if (flag)
			{
				this._readValueStart = this._index;
				this._readValueLength = num;
			}
			return flag;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x000183D4 File Offset: 0x000165D4
		public void AddEntry(ITaggedData taggedData)
		{
			if (taggedData == null)
			{
				throw new ArgumentNullException("taggedData");
			}
			this.AddEntry((int)taggedData.TagID, taggedData.GetData());
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x000183FC File Offset: 0x000165FC
		public void AddEntry(int headerID, byte[] fieldData)
		{
			if (headerID > 65535 || headerID < 0)
			{
				throw new ArgumentOutOfRangeException("headerID");
			}
			int num = (fieldData != null) ? fieldData.Length : 0;
			if (num > 65535)
			{
				throw new ArgumentOutOfRangeException("fieldData", "exceeds maximum length");
			}
			int num2 = this._data.Length + num + 4;
			if (this.Find(headerID))
			{
				num2 -= this.ValueLength + 4;
			}
			if (num2 > 65535)
			{
				throw new ZipException("Data exceeds maximum length");
			}
			this.Delete(headerID);
			byte[] array = new byte[num2];
			this._data.CopyTo(array, 0);
			int index = this._data.Length;
			this._data = array;
			this.SetShort(ref index, headerID);
			this.SetShort(ref index, num);
			if (fieldData != null)
			{
				fieldData.CopyTo(array, index);
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x000184D8 File Offset: 0x000166D8
		public void StartNewEntry()
		{
			this._newEntry = new MemoryStream();
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x000184E8 File Offset: 0x000166E8
		public void AddNewEntry(int headerID)
		{
			byte[] fieldData = this._newEntry.ToArray();
			this._newEntry = null;
			this.AddEntry(headerID, fieldData);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00018510 File Offset: 0x00016710
		public void AddData(byte data)
		{
			this._newEntry.WriteByte(data);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00018520 File Offset: 0x00016720
		public void AddData(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this._newEntry.Write(data, 0, data.Length);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00018544 File Offset: 0x00016744
		public void AddLeShort(int toAdd)
		{
			this._newEntry.WriteByte((byte)toAdd);
			this._newEntry.WriteByte((byte)(toAdd >> 8));
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00018564 File Offset: 0x00016764
		public void AddLeInt(int toAdd)
		{
			this.AddLeShort((int)((short)toAdd));
			this.AddLeShort((int)((short)(toAdd >> 16)));
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0001857C File Offset: 0x0001677C
		public void AddLeLong(long toAdd)
		{
			this.AddLeInt((int)(toAdd & (long)((ulong)-1)));
			this.AddLeInt((int)(toAdd >> 32));
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00018594 File Offset: 0x00016794
		public bool Delete(int headerID)
		{
			bool result = false;
			if (this.Find(headerID))
			{
				result = true;
				int num = this._readValueStart - 4;
				byte[] array = new byte[this._data.Length - (this.ValueLength + 4)];
				Array.Copy(this._data, 0, array, 0, num);
				int num2 = num + this.ValueLength + 4;
				Array.Copy(this._data, num2, array, num, this._data.Length - num2);
				this._data = array;
			}
			return result;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0001860C File Offset: 0x0001680C
		public long ReadLong()
		{
			this.ReadCheck(8);
			return ((long)this.ReadInt() & (long)((ulong)-1)) | (long)this.ReadInt() << 32;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00018638 File Offset: 0x00016838
		public int ReadInt()
		{
			this.ReadCheck(4);
			int result = (int)this._data[this._index] + ((int)this._data[this._index + 1] << 8) + ((int)this._data[this._index + 2] << 16) + ((int)this._data[this._index + 3] << 24);
			this._index += 4;
			return result;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000186A4 File Offset: 0x000168A4
		public int ReadShort()
		{
			this.ReadCheck(2);
			int result = (int)this._data[this._index] + ((int)this._data[this._index + 1] << 8);
			this._index += 2;
			return result;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x000186E8 File Offset: 0x000168E8
		public int ReadByte()
		{
			int result = -1;
			if (this._index < this._data.Length && this._readValueStart + this._readValueLength > this._index)
			{
				result = (int)this._data[this._index];
				this._index++;
			}
			return result;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00018740 File Offset: 0x00016940
		public void Skip(int amount)
		{
			this.ReadCheck(amount);
			this._index += amount;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00018758 File Offset: 0x00016958
		private void ReadCheck(int length)
		{
			if (this._readValueStart > this._data.Length || this._readValueStart < 4)
			{
				throw new ZipException("Find must be called before calling a Read method");
			}
			if (this._index > this._readValueStart + this._readValueLength - length)
			{
				throw new ZipException("End of extra data");
			}
			if (this._index + length < 4)
			{
				throw new ZipException("Cannot read before start of tag");
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x000187D0 File Offset: 0x000169D0
		private int ReadShortInternal()
		{
			if (this._index > this._data.Length - 2)
			{
				throw new ZipException("End of extra data");
			}
			int result = (int)this._data[this._index] + ((int)this._data[this._index + 1] << 8);
			this._index += 2;
			return result;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0001882C File Offset: 0x00016A2C
		private void SetShort(ref int index, int source)
		{
			this._data[index] = (byte)source;
			this._data[index + 1] = (byte)(source >> 8);
			index += 2;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0001885C File Offset: 0x00016A5C
		public void Dispose()
		{
			if (this._newEntry != null)
			{
				this._newEntry.Close();
			}
		}

		// Token: 0x040002DD RID: 733
		private int _index;

		// Token: 0x040002DE RID: 734
		private int _readValueStart;

		// Token: 0x040002DF RID: 735
		private int _readValueLength;

		// Token: 0x040002E0 RID: 736
		private MemoryStream _newEntry;

		// Token: 0x040002E1 RID: 737
		private byte[] _data;
	}
}
