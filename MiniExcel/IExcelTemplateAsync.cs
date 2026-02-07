using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniExcelLibs
{
	// Token: 0x02000011 RID: 17
	internal interface IExcelTemplateAsync : IExcelTemplate
	{
		// Token: 0x0600003F RID: 63
		Task SaveAsByTemplateAsync(string templatePath, object value, CancellationToken cancellationToken = default(CancellationToken));

		// Token: 0x06000040 RID: 64
		Task SaveAsByTemplateAsync(byte[] templateBtyes, object value, CancellationToken cancellationToken = default(CancellationToken));

		// Token: 0x06000041 RID: 65
		Task MergeSameCellsAsync(string path, CancellationToken cancellationToken = default(CancellationToken));

		// Token: 0x06000042 RID: 66
		Task MergeSameCellsAsync(byte[] fileInBytes, CancellationToken cancellationToken = default(CancellationToken));
	}
}
