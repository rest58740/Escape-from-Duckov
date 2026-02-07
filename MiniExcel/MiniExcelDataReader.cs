using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MiniExcelLibs
{
	// Token: 0x02000015 RID: 21
	public class MiniExcelDataReader : MiniExcelDataReaderBase
	{
		// Token: 0x0600007C RID: 124 RVA: 0x00003810 File Offset: 0x00001A10
		internal MiniExcelDataReader(Stream stream, bool useHeaderRow = false, string sheetName = null, ExcelType excelType = ExcelType.UNKNOWN, string startCell = "A1", IConfiguration configuration = null)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this._stream = stream;
			this._source = this._stream.Query(useHeaderRow, sheetName, excelType, startCell, configuration).Cast<IDictionary<string, object>>().GetEnumerator();
			if (this._source.MoveNext())
			{
				IDictionary<string, object> dictionary = this._source.Current;
				this._keys = (((dictionary != null) ? dictionary.Keys.ToList<string>() : null) ?? new List<string>());
				this._fieldCount = this._keys.Count;
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000038AC File Offset: 0x00001AAC
		public override object GetValue(int i)
		{
			if (this._source.Current == null)
			{
				throw new InvalidOperationException("No current row available.");
			}
			return this._source.Current[this._keys[i]];
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000038E2 File Offset: 0x00001AE2
		public override int FieldCount
		{
			get
			{
				return this._fieldCount;
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000038EA File Offset: 0x00001AEA
		public override bool Read()
		{
			if (this._isFirst)
			{
				this._isFirst = false;
				return true;
			}
			return this._source.MoveNext();
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003908 File Offset: 0x00001B08
		public override string GetName(int i)
		{
			return this._keys[i];
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003916 File Offset: 0x00001B16
		public override int GetOrdinal(string name)
		{
			this._keys.IndexOf(name);
			return this._keys.IndexOf(name);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003931 File Offset: 0x00001B31
		protected override void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					Stream stream = this._stream;
					if (stream != null)
					{
						stream.Dispose();
					}
				}
				this._disposed = true;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000395D File Offset: 0x00001B5D
		public new void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x04000016 RID: 22
		private readonly IEnumerator<IDictionary<string, object>> _source;

		// Token: 0x04000017 RID: 23
		private readonly int _fieldCount;

		// Token: 0x04000018 RID: 24
		private readonly List<string> _keys;

		// Token: 0x04000019 RID: 25
		private readonly Stream _stream;

		// Token: 0x0400001A RID: 26
		private bool _isFirst = true;

		// Token: 0x0400001B RID: 27
		private bool _disposed;
	}
}
