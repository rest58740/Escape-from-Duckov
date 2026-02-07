using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000128 RID: 296
public class QuestRequiredItem : MonoBehaviour
{
	// Token: 0x060009E0 RID: 2528 RVA: 0x0002B724 File Offset: 0x00029924
	public void Set(int itemTypeID, int count = 1)
	{
		if (itemTypeID <= 0 || count <= 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(itemTypeID);
		if (metaData.id == 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.icon.sprite = metaData.icon;
		this.text.text = string.Format("{0} x{1}", metaData.DisplayName, count);
		base.gameObject.SetActive(true);
	}

	// Token: 0x040008CB RID: 2251
	[SerializeField]
	private Image icon;

	// Token: 0x040008CC RID: 2252
	[SerializeField]
	private TextMeshProUGUI text;
}
