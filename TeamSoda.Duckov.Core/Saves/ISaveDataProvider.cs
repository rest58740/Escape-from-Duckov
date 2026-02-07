using System;

namespace Saves
{
	// Token: 0x0200022D RID: 557
	public interface ISaveDataProvider
	{
		// Token: 0x060010F2 RID: 4338
		object GenerateSaveData();

		// Token: 0x060010F3 RID: 4339
		void SetupSaveData(object data);
	}
}
