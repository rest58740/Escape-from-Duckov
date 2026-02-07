using System;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000C1 RID: 193
public class StaminaHUD : MonoBehaviour
{
	// Token: 0x17000136 RID: 310
	// (get) Token: 0x06000650 RID: 1616 RVA: 0x0001C7BB File Offset: 0x0001A9BB
	private Item item
	{
		get
		{
			return this.characterMainControl.CharacterItem;
		}
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x0001C7C8 File Offset: 0x0001A9C8
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
		float num = this.characterMainControl.CurrentStamina / this.characterMainControl.MaxStamina;
		if (!Mathf.Approximately(num, this.percent))
		{
			this.percent = num;
			this.fillImage.fillAmount = this.percent;
			this.SetColor();
			if (Mathf.Approximately(num, 1f))
			{
				this.targetAlpha = 0f;
			}
			else
			{
				this.targetAlpha = 1f;
			}
		}
		this.UpdateAlpha(Time.unscaledDeltaTime);
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x0001C874 File Offset: 0x0001AA74
	private void SetColor()
	{
		float num;
		float num2;
		float num3;
		Color.RGBToHSV(this.glowColor.Evaluate(this.percent), ref num, ref num2, ref num3);
		num2 = 0.4f;
		num3 = 1f;
		Color color = Color.HSVToRGB(num, num2, num3);
		this.fillImage.color = color;
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x0001C8BE File Offset: 0x0001AABE
	private void UpdateAlpha(float deltaTime)
	{
		if (this.targetAlpha != this.canvasGroup.alpha)
		{
			this.canvasGroup.alpha = Mathf.MoveTowards(this.canvasGroup.alpha, this.targetAlpha, 5f * deltaTime);
		}
	}

	// Token: 0x040005F9 RID: 1529
	private CharacterMainControl characterMainControl;

	// Token: 0x040005FA RID: 1530
	private float percent;

	// Token: 0x040005FB RID: 1531
	public CanvasGroup canvasGroup;

	// Token: 0x040005FC RID: 1532
	private float targetAlpha;

	// Token: 0x040005FD RID: 1533
	public ProceduralImage fillImage;

	// Token: 0x040005FE RID: 1534
	public Gradient glowColor;
}
