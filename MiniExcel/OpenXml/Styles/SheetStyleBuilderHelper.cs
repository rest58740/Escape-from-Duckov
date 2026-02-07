using System;
using System.Collections.Generic;
using System.Linq;
using MiniExcelLibs.Attributes;
using MiniExcelLibs.Utils;

namespace MiniExcelLibs.OpenXml.Styles
{
	// Token: 0x02000058 RID: 88
	public static class SheetStyleBuilderHelper
	{
		// Token: 0x060002E7 RID: 743 RVA: 0x00012280 File Offset: 0x00010480
		public static IEnumerable<ExcelColumnAttribute> GenerateStyleIds(int startUpCellXfs, ICollection<ExcelColumnAttribute> dynamicColumns)
		{
			if (dynamicColumns == null)
			{
				yield break;
			}
			int index = 0;
			IEnumerable<IGrouping<string, ExcelColumnAttribute>> enumerable;
			if (dynamicColumns == null)
			{
				enumerable = null;
			}
			else
			{
				enumerable = from x in dynamicColumns
				where !string.IsNullOrWhiteSpace(x.Format) && new ExcelNumberFormat(x.Format).IsValid
				group x by x.Format;
			}
			foreach (IGrouping<string, ExcelColumnAttribute> grouping in enumerable)
			{
				foreach (ExcelColumnAttribute excelColumnAttribute in grouping)
				{
					excelColumnAttribute.FormatId = startUpCellXfs + index;
				}
				yield return grouping.First<ExcelColumnAttribute>();
				int num = index;
				index = num + 1;
			}
			IEnumerator<IGrouping<string, ExcelColumnAttribute>> enumerator = null;
			yield break;
			yield break;
		}
	}
}
