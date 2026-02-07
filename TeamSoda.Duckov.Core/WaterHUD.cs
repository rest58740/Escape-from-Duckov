using System;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000C2 RID: 194
public class WaterHUD : MonoBehaviour
{
	// Token: 0x17000137 RID: 311
	// (get) Token: 0x06000655 RID: 1621 RVA: 0x0001C903 File Offset: 0x0001AB03
	private Item item
	{
		get
		{
			return this.characterMainControl.CharacterItem;
		}
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x0001C910 File Offset: 0x0001AB10
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
		float num = this.characterMainControl.CurrentWater / this.characterMainControl.MaxWater;
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

	// Token: 0x040005FF RID: 1535
	private CharacterMainControl characterMainControl;

	// Token: 0x04000600 RID: 1536
	private float percent = -1f;

	// Token: 0x04000601 RID: 1537
	public ProceduralImage fillImage;

	// Token: 0x04000602 RID: 1538
	public ProceduralImage backgroundImage;

	// Token: 0x04000603 RID: 1539
	public Color backgroundColor;

	// Token: 0x04000604 RID: 1540
	public Color emptyBackgroundColor;
}
