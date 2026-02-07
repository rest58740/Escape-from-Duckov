using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniExcelLibs.OpenXml.Styles
{
	// Token: 0x02000054 RID: 84
	internal interface ISheetStyleBuilder
	{
		// Token: 0x0600029A RID: 666
		SheetStyleBuildResult Build();

		// Token: 0x0600029B RID: 667
		Task<SheetStyleBuildResult> BuildAsync(CancellationToken cancellationToken = default(CancellationToken));
	}
}
