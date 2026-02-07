using System;
using ItemStatsSystem;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;

// Token: 0x020001FC RID: 508
public class BulletTypeDisplay : MonoBehaviour
{
	// Token: 0x170002BB RID: 699
	// (get) Token: 0x06000F13 RID: 3859 RVA: 0x0003D00A File Offset: 0x0003B20A
	[LocalizationKey("Default")]
	private string NotAssignedTextKey
	{
		get
		{
			return "UI_Bullet_NotAssigned";
		}
	}

	// Token: 0x06000F14 RID: 3860 RVA: 0x0003D014 File Offset: 0x0003B214
	internal void Setup(int targetBulletID)
	{
		if (targetBulletID < 0)
		{
			this.bulletDisplayName.text = LocalizationManager.ToPlainText(this.NotAssignedTextKey);
			return;
		}
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(targetBulletID);
		this.bulletDisplayName.text = metaData.DisplayName;
	}

	// Token: 0x04000C8E RID: 3214
	[SerializeField]
	private TextMeshProUGUI bulletDisplayName;
}
