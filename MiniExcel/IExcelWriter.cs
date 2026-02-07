using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniExcelLibs
{
	// Token: 0x02000012 RID: 18
	internal interface IExcelWriter
	{
		// Token: 0x06000043 RID: 67
		void SaveAs();

		// Token: 0x06000044 RID: 68
		Task SaveAsAsync(CancellationToken cancellationToken = default(CancellationToken));

		// Token: 0x06000045 RID: 69
		void Insert(bool overwriteSheet = false);

		// Token: 0x06000046 RID: 70
		Task InsertAsync(bool overwriteSheet = false, CancellationToken cancellationToken = default(CancellationToken));
	}
}
