using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using Saves;
using UnityEngine;

namespace Debugging
{
	// Token: 0x0200022A RID: 554
	public class InventorySaveLoad : MonoBehaviour
	{
		// Token: 0x060010D8 RID: 4312 RVA: 0x00041DD9 File Offset: 0x0003FFD9
		public void Save()
		{
			this.inventory.Save(this.key);
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00041DEC File Offset: 0x0003FFEC
		public UniTask Load()
		{
			InventorySaveLoad.<Load>d__4 <Load>d__;
			<Load>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Load>d__.<>4__this = this;
			<Load>d__.<>1__state = -1;
			<Load>d__.<>t__builder.Start<InventorySaveLoad.<Load>d__4>(ref <Load>d__);
			return <Load>d__.<>t__builder.Task;
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00041E2F File Offset: 0x0004002F
		private void OnLoadFinished()
		{
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x00041E31 File Offset: 0x00040031
		public void BeginLoad()
		{
			this.Load().Forget();
		}

		// Token: 0x04000D7E RID: 3454
		public Inventory inventory;

		// Token: 0x04000D7F RID: 3455
		public string key = "helloInventory";

		// Token: 0x04000D80 RID: 3456
		private bool loading;
	}
}
