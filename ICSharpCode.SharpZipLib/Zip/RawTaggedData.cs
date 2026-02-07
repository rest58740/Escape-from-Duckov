using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x0200005F RID: 95
	public class RawTaggedData : ITaggedData
	{
		// Token: 0x06000422 RID: 1058 RVA: 0x000179B0 File Offset: 0x00015BB0
		public RawTaggedData(short tag)
		{
			this._tag = tag;
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x000179C0 File Offset: 0x00015BC0
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x000179C8 File Offset: 0x00015BC8
		public short TagID
		{
			get
			{
				return this._tag;
			}
			set
			{
				this._tag = value;
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x000179D4 File Offset: 0x00015BD4
		public void SetData(byte[] data, int offset, int count)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this._data = new byte[count];
			Array.Copy(data, offset, this._data, 0, count);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00017A10 File Offset: 0x00015C10
		public byte[] GetData()
		{
			return this._data;
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00017A18 File Offset: 0x00015C18
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x00017A20 File Offset: 0x00015C20
		public byte[] Data
		{
			get
			{
				return this._data;
			}
			set
			{
				this._data = value;
			}
		}

		// Token: 0x040002D0 RID: 720
		private short _tag;

		// Token: 0x040002D1 RID: 721
		private byte[] _data;
	}
}
