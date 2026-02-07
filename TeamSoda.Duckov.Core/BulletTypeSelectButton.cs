using System;
using ItemStatsSystem;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;

// Token: 0x020000BD RID: 189
public class BulletTypeSelectButton : MonoBehaviour
{
	// Token: 0x17000133 RID: 307
	// (get) Token: 0x0600063F RID: 1599 RVA: 0x0001C47D File Offset: 0x0001A67D
	public int BulletTypeID
	{
		get
		{
			return this.bulletTypeID;
		}
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x0001C485 File Offset: 0x0001A685
	public void SetSelection(bool selected)
	{
		this.selectShadow.enabled = selected;
		this.indicator.SetActive(selected);
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x0001C49F File Offset: 0x0001A69F
	public void Init(int id, int count)
	{
		this.bulletTypeID = id;
		this.bulletCount = count;
		this.SetSelection(false);
		this.RefreshContent();
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x0001C4BC File Offset: 0x0001A6BC
	public void RefreshContent()
	{
		this.nameText.text = this.GetBulletName(this.bulletTypeID);
		this.countText.text = this.bulletCount.ToString();
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x0001C4EC File Offset: 0x0001A6EC
	public string GetBulletName(int id)
	{
		if (id > 0)
		{
			return ItemAssetsCollection.GetMetaData(id).DisplayName;
		}
		return LocalizationManager.ToPlainText("UI_Bullet_NotAssigned");
	}

	// Token: 0x040005E0 RID: 1504
	private int bulletTypeID;

	// Token: 0x040005E1 RID: 1505
	private int bulletCount;

	// Token: 0x040005E2 RID: 1506
	public BulletTypeHUD bulletTypeHUD;

	// Token: 0x040005E3 RID: 1507
	public TextMeshProUGUI nameText;

	// Token: 0x040005E4 RID: 1508
	public TextMeshProUGUI countText;

	// Token: 0x040005E5 RID: 1509
	public TrueShadow selectShadow;

	// Token: 0x040005E6 RID: 1510
	public GameObject indicator;
}
