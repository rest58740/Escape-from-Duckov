using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem.Data;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003CB RID: 971
	public class StorageDockEntry : MonoBehaviour
	{
		// Token: 0x06002301 RID: 8961 RVA: 0x0007A5AA File Offset: 0x000787AA
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClick));
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x0007A5C8 File Offset: 0x000787C8
		private void OnButtonClick()
		{
			if (!PlayerStorage.IsAccessableAndNotFull())
			{
				return;
			}
			this.TakeTask().Forget();
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x0007A5E0 File Offset: 0x000787E0
		private UniTask TakeTask()
		{
			StorageDockEntry.<TakeTask>d__13 <TakeTask>d__;
			<TakeTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<TakeTask>d__.<>4__this = this;
			<TakeTask>d__.<>1__state = -1;
			<TakeTask>d__.<>t__builder.Start<StorageDockEntry.<TakeTask>d__13>(ref <TakeTask>d__);
			return <TakeTask>d__.<>t__builder.Task;
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x0007A624 File Offset: 0x00078824
		public void Setup(int index, ItemTreeData item)
		{
			this.index = index;
			this.item = item;
			ItemTreeData.DataEntry rootData = item.RootData;
			this.itemDisplay.Setup(rootData.typeID);
			int stackCount = rootData.StackCount;
			if (stackCount > 1)
			{
				this.countText.text = stackCount.ToString();
				this.countDisplay.SetActive(true);
			}
			else
			{
				this.countDisplay.SetActive(false);
			}
			if (PlayerStorage.IsAccessableAndNotFull())
			{
				this.bgImage.color = this.colorNormal;
				this.text.text = LocalizationManager.ToPlainText(this.textKeyNormal);
			}
			else
			{
				this.bgImage.color = this.colorFull;
				this.text.text = LocalizationManager.ToPlainText(this.textKeyInventoryFull);
			}
			this.text.gameObject.SetActive(true);
			this.loadingIndicator.SetActive(false);
		}

		// Token: 0x040017A3 RID: 6051
		[SerializeField]
		private ItemMetaDisplay itemDisplay;

		// Token: 0x040017A4 RID: 6052
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040017A5 RID: 6053
		[SerializeField]
		private GameObject countDisplay;

		// Token: 0x040017A6 RID: 6054
		[SerializeField]
		private TextMeshProUGUI countText;

		// Token: 0x040017A7 RID: 6055
		[SerializeField]
		private Image bgImage;

		// Token: 0x040017A8 RID: 6056
		[SerializeField]
		private Button button;

		// Token: 0x040017A9 RID: 6057
		[SerializeField]
		private GameObject loadingIndicator;

		// Token: 0x040017AA RID: 6058
		[SerializeField]
		private Color colorNormal;

		// Token: 0x040017AB RID: 6059
		[SerializeField]
		private Color colorFull;

		// Token: 0x040017AC RID: 6060
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKeyNormal;

		// Token: 0x040017AD RID: 6061
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKeyInventoryFull;

		// Token: 0x040017AE RID: 6062
		private int index;

		// Token: 0x040017AF RID: 6063
		private ItemTreeData item;
	}
}
