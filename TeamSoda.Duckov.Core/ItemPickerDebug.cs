using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

// Token: 0x02000168 RID: 360
public class ItemPickerDebug : MonoBehaviour
{
	// Token: 0x06000B2B RID: 2859 RVA: 0x00030AC7 File Offset: 0x0002ECC7
	public void PickPlayerInventoryAndLog()
	{
		this.Pick().Forget();
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x00030AD4 File Offset: 0x0002ECD4
	private UniTask Pick()
	{
		ItemPickerDebug.<Pick>d__1 <Pick>d__;
		<Pick>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Pick>d__.<>1__state = -1;
		<Pick>d__.<>t__builder.Start<ItemPickerDebug.<Pick>d__1>(ref <Pick>d__);
		return <Pick>d__.<>t__builder.Task;
	}
}
