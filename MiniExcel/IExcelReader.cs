using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiniExcelLibs
{
	// Token: 0x0200000F RID: 15
	internal interface IExcelReader : IDisposable
	{
		// Token: 0x06000033 RID: 51
		IEnumerable<IDictionary<string, object>> Query(bool UseHeaderRow, string sheetName, string startCell);

		// Token: 0x06000034 RID: 52
		IEnumerable<T> Query<T>(string sheetName, string startCell) where T : class, new();

		// Token: 0x06000035 RID: 53
		Task<IEnumerable<IDictionary<string, object>>> QueryAsync(bool UseHeaderRow, string sheetName, string startCell, CancellationToken cancellationToken = default(CancellationToken));

		// Token: 0x06000036 RID: 54
		Task<IEnumerable<T>> QueryAsync<T>(string sheetName, string startCell, CancellationToken cancellationToken = default(CancellationToken)) where T : class, new();

		// Token: 0x06000037 RID: 55
		IEnumerable<IDictionary<string, object>> QueryRange(bool UseHeaderRow, string sheetName, string startCell, string endCell);

		// Token: 0x06000038 RID: 56
		IEnumerable<T> QueryRange<T>(string sheetName, string startCell, string endCell) where T : class, new();

		// Token: 0x06000039 RID: 57
		Task<IEnumerable<IDictionary<string, object>>> QueryAsyncRange(bool UseHeaderRow, string sheetName, string startCell, string endCell, CancellationToken cancellationToken = default(CancellationToken));

		// Token: 0x0600003A RID: 58
		Task<IEnumerable<T>> QueryAsyncRange<T>(string sheetName, string startCell, string endCell, CancellationToken cancellationToken = default(CancellationToken)) where T : class, new();
	}
}
