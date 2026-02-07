using System;
using System.Collections.Generic;
using Duckov.UI;
using Duckov.Utilities;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000BC RID: 188
public class BulletTypeHUD : MonoBehaviour
{
	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000632 RID: 1586 RVA: 0x0001BD40 File Offset: 0x00019F40
	private PrefabPool<BulletTypeSelectButton> Selections
	{
		get
		{
			if (this._selectionsCache == null)
			{
				this._selectionsCache = new PrefabPool<BulletTypeSelectButton>(this.originSelectButton, null, null, null, null, true, 10, 10000, null);
			}
			return this._selectionsCache;
		}
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x06000633 RID: 1587 RVA: 0x0001BD7C File Offset: 0x00019F7C
	private bool CanOpenList
	{
		get
		{
			return this.characterMainControl && (!this.characterMainControl.CurrentAction || !this.characterMainControl.CurrentAction.Running) && InputManager.InputActived;
		}
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x0001BDC8 File Offset: 0x00019FC8
	private void Awake()
	{
		this.selectionsHUD = new List<BulletTypeSelectButton>();
		this.originSelectButton.gameObject.SetActive(false);
		WeaponButton.OnWeaponButtonSelected += this.OnWeaponButtonSelected;
		this.typeList.SetActive(false);
		InputManager.OnSwitchBulletTypeInput += this.OnSwitchInput;
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x0001BE20 File Offset: 0x0001A020
	private void OnDestroy()
	{
		WeaponButton.OnWeaponButtonSelected -= this.OnWeaponButtonSelected;
		if (this.characterMainControl)
		{
			this.characterMainControl.OnHoldAgentChanged -= this.OnHoldAgentChanged;
		}
		InputManager.OnSwitchBulletTypeInput -= this.OnSwitchInput;
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x0001BE74 File Offset: 0x0001A074
	private void OnWeaponButtonSelected(WeaponButton button)
	{
		Transform transform = this.canvasGroup.transform as RectTransform;
		RectTransform rectTransform = button.transform as RectTransform;
		transform.position = rectTransform.position + (rectTransform.rect.center + (rectTransform.rect.height / 2f + 8f) * rectTransform.up) * rectTransform.lossyScale;
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x0001BF04 File Offset: 0x0001A104
	public void Update()
	{
		if (!this.characterMainControl)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (this.characterMainControl)
			{
				this.characterMainControl.OnHoldAgentChanged += this.OnHoldAgentChanged;
				if (this.characterMainControl.CurrentHoldItemAgent != null)
				{
					this.OnHoldAgentChanged(this.characterMainControl.CurrentHoldItemAgent);
				}
			}
		}
		if (this.gunAgent == null)
		{
			this.canvasGroup.alpha = 0f;
			this.canvasGroup.interactable = false;
			return;
		}
		this.canvasGroup.alpha = 1f;
		this.canvasGroup.interactable = true;
		if (this.bulletTypeText != null && this.gunAgent.GunItemSetting != null)
		{
			int targetBulletID = this.gunAgent.GunItemSetting.TargetBulletID;
			if (this.bulletTpyeID != targetBulletID)
			{
				this.bulletTpyeID = targetBulletID;
				if (this.bulletTpyeID >= 0)
				{
					this.bulletTypeText.text = this.gunAgent.GunItemSetting.CurrentBulletName;
					this.bulletTypeText.color = Color.black;
					this.background.color = this.normalColor;
				}
				else
				{
					this.bulletTypeText.text = LocalizationManager.ToPlainText("UI_Bullet_NotAssigned");
					this.bulletTypeText.color = Color.white;
					this.background.color = this.emptyColor;
				}
				UnityEvent onTypeChangeEvent = this.OnTypeChangeEvent;
				if (onTypeChangeEvent != null)
				{
					onTypeChangeEvent.Invoke();
				}
			}
		}
		if (this.listOpen && !this.CanOpenList)
		{
			this.CloseList();
		}
		if (CharacterInputControl.GetChangeBulletTypeWasPressed())
		{
			if (!this.listOpen)
			{
				this.OpenList();
				return;
			}
			if (this.selectIndex < this.selectionsHUD.Count && this.selectionsHUD[this.selectIndex] != null)
			{
				this.SetBulletType(this.selectionsHUD[this.selectIndex].BulletTypeID);
			}
			this.CloseList();
		}
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x0001C110 File Offset: 0x0001A310
	private void OnHoldAgentChanged(DuckovItemAgent newAgent)
	{
		if (newAgent == null)
		{
			this.gunAgent = null;
		}
		this.gunAgent = (newAgent as ItemAgent_Gun);
		this.CloseList();
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x0001C134 File Offset: 0x0001A334
	private void OnSwitchInput(int dir)
	{
		if (!this.listOpen)
		{
			return;
		}
		this.selectIndex -= dir;
		if (this.totalSelctionCount == 0)
		{
			this.selectIndex = 0;
		}
		else if (this.selectIndex >= this.totalSelctionCount)
		{
			this.selectIndex = 0;
		}
		else if (this.selectIndex < 0)
		{
			this.selectIndex = this.totalSelctionCount - 1;
		}
		for (int i = 0; i < this.selectionsHUD.Count; i++)
		{
			this.selectionsHUD[i].SetSelection(i == this.selectIndex);
		}
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x0001C1C8 File Offset: 0x0001A3C8
	private void OpenList()
	{
		Debug.Log("OpenList");
		if (!this.CanOpenList)
		{
			return;
		}
		if (this.listOpen)
		{
			return;
		}
		this.typeList.SetActive(true);
		this.listOpen = true;
		this.indicator.SetActive(false);
		this.RefreshContent();
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x0001C216 File Offset: 0x0001A416
	public void CloseList()
	{
		if (!this.listOpen)
		{
			return;
		}
		this.typeList.SetActive(false);
		this.listOpen = false;
		this.indicator.SetActive(true);
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x0001C240 File Offset: 0x0001A440
	private void RefreshContent()
	{
		this.selectionsHUD.Clear();
		this.Selections.ReleaseAll();
		Dictionary<int, BulletTypeInfo> dictionary = new Dictionary<int, BulletTypeInfo>();
		ItemSetting_Gun gunItemSetting = this.gunAgent.GunItemSetting;
		if (gunItemSetting != null)
		{
			dictionary = gunItemSetting.GetBulletTypesInInventory(this.characterMainControl.CharacterItem.Inventory);
		}
		if (this.bulletTpyeID > 0 && !dictionary.ContainsKey(this.bulletTpyeID))
		{
			BulletTypeInfo bulletTypeInfo = new BulletTypeInfo();
			bulletTypeInfo.bulletTypeID = this.bulletTpyeID;
			bulletTypeInfo.count = 0;
			dictionary.Add(this.bulletTpyeID, bulletTypeInfo);
		}
		if (dictionary.Count <= 0)
		{
			dictionary.Add(-1, new BulletTypeInfo
			{
				bulletTypeID = -1,
				count = 0
			});
		}
		this.totalSelctionCount = dictionary.Count;
		int num = 0;
		this.selectIndex = 0;
		foreach (KeyValuePair<int, BulletTypeInfo> keyValuePair in dictionary)
		{
			BulletTypeSelectButton bulletTypeSelectButton = this.Selections.Get(this.typeList.transform);
			bulletTypeSelectButton.gameObject.SetActive(true);
			bulletTypeSelectButton.transform.SetAsLastSibling();
			bulletTypeSelectButton.Init(keyValuePair.Value.bulletTypeID, keyValuePair.Value.count);
			if (this.bulletTpyeID == keyValuePair.Value.bulletTypeID)
			{
				bulletTypeSelectButton.SetSelection(true);
				this.selectIndex = num;
			}
			this.selectionsHUD.Add(bulletTypeSelectButton);
			Debug.Log(string.Format("BUlletType {0}:{1}", this.selectIndex, keyValuePair.Value.bulletTypeID));
			num++;
		}
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x0001C404 File Offset: 0x0001A604
	public void SetBulletType(int typeID)
	{
		this.CloseList();
		if (!this.gunAgent || !this.gunAgent.GunItemSetting)
		{
			return;
		}
		bool flag = this.gunAgent.GunItemSetting.TargetBulletID != typeID;
		this.gunAgent.GunItemSetting.SetTargetBulletType(typeID);
		if (flag)
		{
			this.characterMainControl.TryToReload(null);
		}
	}

	// Token: 0x040005CF RID: 1487
	private CharacterMainControl characterMainControl;

	// Token: 0x040005D0 RID: 1488
	private ItemAgent_Gun gunAgent;

	// Token: 0x040005D1 RID: 1489
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x040005D2 RID: 1490
	[SerializeField]
	private TextMeshProUGUI bulletTypeText;

	// Token: 0x040005D3 RID: 1491
	[SerializeField]
	private ProceduralImage background;

	// Token: 0x040005D4 RID: 1492
	[SerializeField]
	private Color normalColor;

	// Token: 0x040005D5 RID: 1493
	[SerializeField]
	private Color emptyColor;

	// Token: 0x040005D6 RID: 1494
	private int bulletTpyeID = -2;

	// Token: 0x040005D7 RID: 1495
	[SerializeField]
	private GameObject typeList;

	// Token: 0x040005D8 RID: 1496
	public UnityEvent OnTypeChangeEvent;

	// Token: 0x040005D9 RID: 1497
	public GameObject indicator;

	// Token: 0x040005DA RID: 1498
	private int selectIndex;

	// Token: 0x040005DB RID: 1499
	private int totalSelctionCount;

	// Token: 0x040005DC RID: 1500
	[SerializeField]
	private BulletTypeSelectButton originSelectButton;

	// Token: 0x040005DD RID: 1501
	private List<BulletTypeSelectButton> selectionsHUD;

	// Token: 0x040005DE RID: 1502
	private PrefabPool<BulletTypeSelectButton> _selectionsCache;

	// Token: 0x040005DF RID: 1503
	private bool listOpen;
}
