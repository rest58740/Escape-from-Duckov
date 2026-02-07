using System;
using System.Collections.Generic;
using System.Threading;
using MiniExcelLibs.Utils;

namespace MiniExcelLibs.WriteAdapter
{
	// Token: 0x02000022 RID: 34
	internal interface IMiniExcelWriteAdapter
	{
		// Token: 0x060000E5 RID: 229
		bool TryGetKnownCount(out int count);

		// Token: 0x060000E6 RID: 230
		List<ExcelColumnInfo> GetColumns();

		// Token: 0x060000E7 RID: 231
		IEnumerable<IEnumerable<CellWriteInfo>> GetRows(List<ExcelColumnInfo> props, CancellationToken cancellationToken = default(CancellationToken));
	}
}
