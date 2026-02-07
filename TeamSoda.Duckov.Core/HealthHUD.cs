using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000BF RID: 191
public class HealthHUD : MonoBehaviour
{
	// Token: 0x17000135 RID: 309
	// (get) Token: 0x06000648 RID: 1608 RVA: 0x0001C5E5 File Offset: 0x0001A7E5
	private Item item
	{
		get
		{
			return this.characterMainControl.CharacterItem;
		}
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x0001C5F4 File Offset: 0x0001A7F4
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
		float num = this.characterMainControl.Health.MaxHealth;
		float currentHealth = this.characterMainControl.Health.CurrentHealth;
		float num2 = currentHealth / num;
		if (!Mathf.Approximately(num2, this.percent))
		{
			this.percent = num2;
			this.fillImage.fillAmount = this.percent;
			if (this.percent <= 0f)
			{
				this.backgroundImage.color = this.emptyBackgroundColor;
			}
			else
			{
				this.backgroundImage.color = this.backgroundColor;
			}
		}
		if (num != this.maxHealth || currentHealth != this.currenthealth)
		{
			this.maxHealth = num;
			this.currenthealth = currentHealth;
			this.text.text = this.currenthealth.ToString("0.#") + " / " + this.maxHealth.ToString("0.#");
		}
	}

	// Token: 0x040005ED RID: 1517
	private CharacterMainControl characterMainControl;

	// Token: 0x040005EE RID: 1518
	private float percent = -1f;

	// Token: 0x040005EF RID: 1519
	private float maxHealth;

	// Token: 0x040005F0 RID: 1520
	private float currenthealth;

	// Token: 0x040005F1 RID: 1521
	public ProceduralImage fillImage;

	// Token: 0x040005F2 RID: 1522
	public ProceduralImage backgroundImage;

	// Token: 0x040005F3 RID: 1523
	public Color backgroundColor;

	// Token: 0x040005F4 RID: 1524
	public Color emptyBackgroundColor;

	// Token: 0x040005F5 RID: 1525
	public TextMeshProUGUI text;
}
