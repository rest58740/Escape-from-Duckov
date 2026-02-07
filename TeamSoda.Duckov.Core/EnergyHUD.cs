using System;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000BE RID: 190
public class EnergyHUD : MonoBehaviour
{
	// Token: 0x17000134 RID: 308
	// (get) Token: 0x06000645 RID: 1605 RVA: 0x0001C51E File Offset: 0x0001A71E
	private Item item
	{
		get
		{
			return this.characterMainControl.CharacterItem;
		}
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x0001C52C File Offset: 0x0001A72C
	private void Update()
	{
		if (!this.characterMainControl)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (!this.characterMainControl)
			{
				return;
			}
		}
		float num = this.characterMainControl.CurrentEnergy / this.characterMainControl.MaxEnergy;
		if (!Mathf.Approximately(num, this.percent))
		{
			this.percent = num;
			this.fillImage.fillAmount = this.percent;
			if (this.percent <= 0f)
			{
				this.backgroundImage.color = this.emptyBackgroundColor;
				return;
			}
			this.backgroundImage.color = this.backgroundColor;
		}
	}

	// Token: 0x040005E7 RID: 1511
	private CharacterMainControl characterMainControl;

	// Token: 0x040005E8 RID: 1512
	private float percent = -1f;

	// Token: 0x040005E9 RID: 1513
	public ProceduralImage fillImage;

	// Token: 0x040005EA RID: 1514
	public ProceduralImage backgroundImage;

	// Token: 0x040005EB RID: 1515
	public Color backgroundColor;

	// Token: 0x040005EC RID: 1516
	public Color emptyBackgroundColor;
}
