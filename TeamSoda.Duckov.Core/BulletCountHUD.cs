using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000BB RID: 187
public class BulletCountHUD : MonoBehaviour
{
	// Token: 0x0600062B RID: 1579 RVA: 0x0001BADE File Offset: 0x00019CDE
	private void Awake()
	{
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x0001BAE0 File Offset: 0x00019CE0
	public void Update()
	{
		if (!this.characterMainControl)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (this.characterMainControl)
			{
				this.characterMainControl.OnHoldAgentChanged += this.OnHoldAgentChanged;
				this.characterMainControl.CharacterItem.Inventory.onContentChanged += this.OnInventoryChanged;
				if (this.characterMainControl.CurrentHoldItemAgent != null)
				{
					this.OnHoldAgentChanged(this.characterMainControl.CurrentHoldItemAgent);
				}
				this.ChangeTotalCount();
				this.capacityText.text = this.totalCount.ToString("D2");
			}
		}
		if (this.gunAgnet == null)
		{
			this.canvasGroup.alpha = 0f;
			return;
		}
		bool flag = false;
		this.canvasGroup.alpha = 1f;
		int num = this.gunAgnet.BulletCount;
		if (this.bulletCount != num)
		{
			this.bulletCount = num;
			this.bulletCountText.text = num.ToString("D2");
			flag = true;
		}
		if (flag)
		{
			UnityEvent onValueChangeEvent = this.OnValueChangeEvent;
			if (onValueChangeEvent != null)
			{
				onValueChangeEvent.Invoke();
			}
			if (this.bulletCount <= 0 && (this.totalCount <= 0 || !this.capacityText.gameObject.activeInHierarchy))
			{
				this.background.color = this.emptyBackgroundColor;
				return;
			}
			this.background.color = this.normalBackgroundColor;
		}
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x0001BC58 File Offset: 0x00019E58
	private void OnInventoryChanged(Inventory inventory, int index)
	{
		this.ChangeTotalCount();
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x0001BC60 File Offset: 0x00019E60
	private void ChangeTotalCount()
	{
		int num = 0;
		if (this.gunAgnet)
		{
			num = this.gunAgnet.GetBulletCountInInventory();
		}
		if (this.totalCount != num)
		{
			this.totalCount = num;
			this.capacityText.text = this.totalCount.ToString("D2");
		}
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x0001BCB4 File Offset: 0x00019EB4
	private void OnDestroy()
	{
		if (this.characterMainControl)
		{
			this.characterMainControl.OnHoldAgentChanged -= this.OnHoldAgentChanged;
			this.characterMainControl.CharacterItem.Inventory.onContentChanged -= this.OnInventoryChanged;
		}
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x0001BD06 File Offset: 0x00019F06
	private void OnHoldAgentChanged(DuckovItemAgent newAgent)
	{
		if (newAgent == null)
		{
			this.gunAgnet = null;
		}
		this.gunAgnet = (newAgent as ItemAgent_Gun);
		this.ChangeTotalCount();
	}

	// Token: 0x040005C3 RID: 1475
	private ItemAgent_Gun gunAgent;

	// Token: 0x040005C4 RID: 1476
	private CharacterMainControl characterMainControl;

	// Token: 0x040005C5 RID: 1477
	private ItemAgent_Gun gunAgnet;

	// Token: 0x040005C6 RID: 1478
	public CanvasGroup canvasGroup;

	// Token: 0x040005C7 RID: 1479
	public TextMeshProUGUI bulletCountText;

	// Token: 0x040005C8 RID: 1480
	public TextMeshProUGUI capacityText;

	// Token: 0x040005C9 RID: 1481
	public ProceduralImage background;

	// Token: 0x040005CA RID: 1482
	public Color normalBackgroundColor;

	// Token: 0x040005CB RID: 1483
	public Color emptyBackgroundColor;

	// Token: 0x040005CC RID: 1484
	private int bulletCount = -1;

	// Token: 0x040005CD RID: 1485
	private int totalCount = -1;

	// Token: 0x040005CE RID: 1486
	public UnityEvent OnValueChangeEvent;
}
