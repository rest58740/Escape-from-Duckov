using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace MiniExcelLibs
{
	// Token: 0x02000013 RID: 19
	public interface IMiniExcelDataReader : IDataReader, IDisposable, IDataRecord
	{
		// Token: 0x06000047 RID: 71
		Task CloseAsync();

		// Token: 0x06000048 RID: 72
		Task<string> GetNameAsync(int i, CancellationToken cancellationToken = default(CancellationToken));

		// Token: 0x06000049 RID: 73
		Task<object> GetValueAsync(int i, CancellationToken cancellationToken = default(CancellationToken));

		// Token: 0x0600004A RID: 74
		Task<bool> NextResultAsync(CancellationToken cancellationToken = default(CancellationToken));

		// Token: 0x0600004B RID: 75
		Task<bool> ReadAsync(CancellationToken cancellationToken = default(CancellationToken));
	}
}
