using System;
using Duckov.MasterKeys.UI;

namespace Duckov.MasterKeys
{
	// Token: 0x020002EC RID: 748
	public class MasterKeyRegisterViewInvoker : InteractableBase
	{
		// Token: 0x06001827 RID: 6183 RVA: 0x000594DE File Offset: 0x000576DE
		protected override void Awake()
		{
			base.Awake();
			this.finishWhenTimeOut = true;
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x000594ED File Offset: 0x000576ED
		protected override void OnInteractFinished()
		{
			MasterKeysRegisterView.Show();
		}
	}
}
