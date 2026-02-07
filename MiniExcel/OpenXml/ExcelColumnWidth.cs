using System;
using System.Collections.Generic;
using MiniExcelLibs.Utils;

namespace MiniExcelLibs.OpenXml
{
	// Token: 0x02000045 RID: 69
	public sealed class ExcelColumnWidth
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000A412 File Offset: 0x00008612
		// (set) Token: 0x06000207 RID: 519 RVA: 0x0000A41A File Offset: 0x0000861A
		public int Index { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000A423 File Offset: 0x00008623
		// (set) Token: 0x06000209 RID: 521 RVA: 0x0000A42B File Offset: 0x0000862B
		public double Width { get; set; }

		// Token: 0x0600020A RID: 522 RVA: 0x0000A434 File Offset: 0x00008634
		internal static IEnumerable<ExcelColumnWidth> FromProps(IEnumerable<ExcelColumnInfo> props, double? minWidth = null)
		{
			int i = 1;
			foreach (ExcelColumnInfo excelColumnInfo in props)
			{
				if (excelColumnInfo == null || (excelColumnInfo.ExcelColumnWidth == null && minWidth == null))
				{
					int num = i;
					i = num + 1;
				}
				else
				{
					int index = (excelColumnInfo.ExcelColumnIndex == null) ? i : (excelColumnInfo.ExcelColumnIndex.GetValueOrDefault() + 1);
					yield return new ExcelColumnWidth
					{
						Index = index,
						Width = (excelColumnInfo.ExcelColumnWidth ?? minWidth.Value)
					};
					int num = i;
					i = num + 1;
				}
			}
			IEnumerator<ExcelColumnInfo> enumerator = null;
			yield break;
			yield break;
		}
	}
}
