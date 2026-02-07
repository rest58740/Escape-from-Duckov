using System;
using MiniExcelLibs.Utils;

namespace MiniExcelLibs.Attributes
{
	// Token: 0x02000068 RID: 104
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class ExcelColumnAttribute : Attribute
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600037A RID: 890 RVA: 0x000133C8 File Offset: 0x000115C8
		// (set) Token: 0x0600037B RID: 891 RVA: 0x000133D0 File Offset: 0x000115D0
		internal int FormatId { get; set; } = -1;

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600037C RID: 892 RVA: 0x000133D9 File Offset: 0x000115D9
		// (set) Token: 0x0600037D RID: 893 RVA: 0x000133E1 File Offset: 0x000115E1
		public string Name { get; set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600037E RID: 894 RVA: 0x000133EA File Offset: 0x000115EA
		// (set) Token: 0x0600037F RID: 895 RVA: 0x000133F2 File Offset: 0x000115F2
		public string[] Aliases { get; set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000380 RID: 896 RVA: 0x000133FB File Offset: 0x000115FB
		// (set) Token: 0x06000381 RID: 897 RVA: 0x00013403 File Offset: 0x00011603
		public double Width { get; set; } = 9.28515625;

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000382 RID: 898 RVA: 0x0001340C File Offset: 0x0001160C
		// (set) Token: 0x06000383 RID: 899 RVA: 0x00013414 File Offset: 0x00011614
		public string Format { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000384 RID: 900 RVA: 0x0001341D File Offset: 0x0001161D
		// (set) Token: 0x06000385 RID: 901 RVA: 0x00013425 File Offset: 0x00011625
		public bool Ignore { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0001342E File Offset: 0x0001162E
		// (set) Token: 0x06000387 RID: 903 RVA: 0x00013436 File Offset: 0x00011636
		public ColumnType Type { get; set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000388 RID: 904 RVA: 0x0001343F File Offset: 0x0001163F
		// (set) Token: 0x06000389 RID: 905 RVA: 0x00013447 File Offset: 0x00011647
		public int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this.Init(value, null);
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600038A RID: 906 RVA: 0x00013451 File Offset: 0x00011651
		// (set) Token: 0x0600038B RID: 907 RVA: 0x00013459 File Offset: 0x00011659
		public string IndexName
		{
			get
			{
				return this._xName;
			}
			set
			{
				this.Init(ColumnHelper.GetColumnIndex(value), value);
			}
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00013468 File Offset: 0x00011668
		private void Init(int index, string columnName = null)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", index, string.Format("Column index {0} must be greater or equal to zero.", index));
			}
			if (this._xName == null)
			{
				if (columnName != null)
				{
					this._xName = columnName;
				}
				else
				{
					this._xName = ColumnHelper.GetAlphabetColumnName(index);
				}
			}
			this._index = index;
		}

		// Token: 0x04000163 RID: 355
		private int _index = -1;

		// Token: 0x04000164 RID: 356
		private string _xName;
	}
}
