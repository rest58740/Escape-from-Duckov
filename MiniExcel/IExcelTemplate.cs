using System;

namespace MiniExcelLibs
{
	// Token: 0x02000010 RID: 16
	internal interface IExcelTemplate
	{
		// Token: 0x0600003B RID: 59
		void SaveAsByTemplate(string templatePath, object value);

		// Token: 0x0600003C RID: 60
		void SaveAsByTemplate(byte[] templateBtyes, object value);

		// Token: 0x0600003D RID: 61
		void MergeSameCells(string path);

		// Token: 0x0600003E RID: 62
		void MergeSameCells(byte[] fileInBytes);
	}
}
