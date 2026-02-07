using System;
using Duckov.Economy;
using Saves;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x02000398 RID: 920
	public class MoneyDisplay : MonoBehaviour
	{
		// Token: 0x0600200B RID: 8203 RVA: 0x00070F6F File Offset: 0x0006F16F
		private void Awake()
		{
			EconomyManager.OnMoneyChanged += this.OnMoneyChanged;
			SavesSystem.OnSetFile += this.OnSaveFileChanged;
			this.Refresh();
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x00070F99 File Offset: 0x0006F199
		private void OnDestroy()
		{
			EconomyManager.OnMoneyChanged -= this.OnMoneyChanged;
			SavesSystem.OnSetFile -= this.OnSaveFileChanged;
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x00070FBD File Offset: 0x0006F1BD
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x00070FC8 File Offset: 0x0006F1C8
		private void Refresh()
		{
			this.text.text = EconomyManager.Money.ToString(this.format);
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x00070FF3 File Offset: 0x0006F1F3
		private void OnMoneyChanged(long arg1, long arg2)
		{
			this.Refresh();
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x00070FFB File Offset: 0x0006F1FB
		private void OnSaveFileChanged()
		{
			this.Refresh();
		}

		// Token: 0x040015F2 RID: 5618
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040015F3 RID: 5619
		[SerializeField]
		private string format = "n0";
	}
}
