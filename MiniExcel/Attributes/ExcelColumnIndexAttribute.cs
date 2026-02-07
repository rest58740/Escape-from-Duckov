using System;
using MiniExcelLibs.Utils;

namespace MiniExcelLibs.Attributes
{
	// Token: 0x0200006B RID: 107
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class ExcelColumnIndexAttribute : Attribute
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000393 RID: 915 RVA: 0x00013517 File Offset: 0x00011717
		// (set) Token: 0x06000394 RID: 916 RVA: 0x0001351F File Offset: 0x0001171F
		public int ExcelColumnIndex { get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000395 RID: 917 RVA: 0x00013528 File Offset: 0x00011728
		// (set) Token: 0x06000396 RID: 918 RVA: 0x00013530 File Offset: 0x00011730
		internal string ExcelXName { get; set; }

		// Token: 0x06000397 RID: 919 RVA: 0x00013539 File Offset: 0x00011739
		public ExcelColumnIndexAttribute(string columnName)
		{
			this.Init(ColumnHelper.GetColumnIndex(columnName), columnName);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0001354E File Offset: 0x0001174E
		public ExcelColumnIndexAttribute(int columnIndex)
		{
			this.Init(columnIndex, null);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00013560 File Offset: 0x00011760
		private void Init(int columnIndex, string columnName = null)
		{
			if (columnIndex < 0)
			{
				throw new ArgumentOutOfRangeException("columnIndex", columnIndex, string.Format("Column index {0} must be greater or equal to zero.", columnIndex));
			}
			if (this.ExcelXName == null)
			{
				if (columnName != null)
				{
					this.ExcelXName = columnName;
				}
				else
				{
					this.ExcelXName = ColumnHelper.GetAlphabetColumnName(columnIndex);
				}
			}
			this.ExcelColumnIndex = columnIndex;
		}
	}
}
