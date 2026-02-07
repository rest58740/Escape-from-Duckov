using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.UI.Animations;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003B7 RID: 951
	public class ItemOperationMenu : ManagedUIElement
	{
		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060021C2 RID: 8642 RVA: 0x00076339 File Offset: 0x00074539
		// (set) Token: 0x060021C3 RID: 8643 RVA: 0x00076340 File Offset: 0x00074540
		public static ItemOperationMenu Instance { get; private set; }

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x060021C4 RID: 8644 RVA: 0x00076348 File Offset: 0x00074548
		private Item TargetItem
		{
			get
			{
				ItemDisplay targetDisplay = this.TargetDisplay;
				if (targetDisplay == null)
				{
					return null;
				}
				return targetDisplay.Target;
			}
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x0007635B File Offset: 0x0007455B
		protected override void Awake()
		{
			base.Awake();
			ItemOperationMenu.Instance = this;
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			this.Initialize();
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x00076389 File Offset: 0x00074589
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x00076394 File Offset: 0x00074594
		private void Update()
		{
			if (this.fadeGroup.IsHidingInProgress)
			{
				return;
			}
			if (!this.fadeGroup.IsShown)
			{
				return;
			}
			if (!Mouse.current.leftButton.wasReleasedThisFrame && !(this.targetView == null) && this.targetView.open)
			{
				if (this.fadeGroup.IsShowingInProgress)
				{
					return;
				}
				if (!Mouse.current.rightButton.wasReleasedThisFrame)
				{
					return;
				}
			}
			base.Close();
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x00076410 File Offset: 0x00074610
		private void Initialize()
		{
			this.btn_Use.onClick.AddListener(new UnityAction(this.Use));
			this.btn_Split.onClick.AddListener(new UnityAction(this.Split));
			this.btn_Dump.onClick.AddListener(new UnityAction(this.Dump));
			this.btn_Equip.onClick.AddListener(new UnityAction(this.Equip));
			this.btn_Modify.onClick.AddListener(new UnityAction(this.Modify));
			this.btn_Unload.onClick.AddListener(new UnityAction(this.Unload));
			this.btn_Wishlist.onClick.AddListener(new UnityAction(this.Wishlist));
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x000764E4 File Offset: 0x000746E4
		private void Wishlist()
		{
			if (this.TargetItem == null)
			{
				return;
			}
			int typeID = this.TargetItem.TypeID;
			if (ItemWishlist.GetWishlistInfo(typeID).isManuallyWishlisted)
			{
				ItemWishlist.RemoveFromWishlist(typeID);
				return;
			}
			ItemWishlist.AddToWishList(this.TargetItem.TypeID);
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x00076531 File Offset: 0x00074731
		private void Use()
		{
			LevelManager instance = LevelManager.Instance;
			if (instance != null)
			{
				CharacterMainControl mainCharacter = instance.MainCharacter;
				if (mainCharacter != null)
				{
					mainCharacter.UseItem(this.TargetItem);
				}
			}
			InventoryView.Hide();
			base.Close();
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x0007655F File Offset: 0x0007475F
		private void Split()
		{
			SplitDialogue.SetupAndShow(this.TargetItem);
			base.Close();
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x00076572 File Offset: 0x00074772
		private void Dump()
		{
			LevelManager instance = LevelManager.Instance;
			if ((instance != null) ? instance.MainCharacter : null)
			{
				this.TargetItem.Drop(LevelManager.Instance.MainCharacter, true);
			}
			base.Close();
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x000765A8 File Offset: 0x000747A8
		private void Modify()
		{
			if (this.TargetItem == null)
			{
				return;
			}
			ItemCustomizeView instance = ItemCustomizeView.Instance;
			if (instance == null)
			{
				return;
			}
			List<Inventory> list = new List<Inventory>();
			LevelManager instance2 = LevelManager.Instance;
			Inventory inventory;
			if (instance2 == null)
			{
				inventory = null;
			}
			else
			{
				CharacterMainControl mainCharacter = instance2.MainCharacter;
				if (mainCharacter == null)
				{
					inventory = null;
				}
				else
				{
					Item characterItem = mainCharacter.CharacterItem;
					inventory = ((characterItem != null) ? characterItem.Inventory : null);
				}
			}
			Inventory inventory2 = inventory;
			if (inventory2)
			{
				list.Add(inventory2);
			}
			instance.Setup(this.TargetItem, list);
			instance.Open(null);
			base.Close();
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x0007662D File Offset: 0x0007482D
		private void Equip()
		{
			LevelManager instance = LevelManager.Instance;
			if (instance != null)
			{
				CharacterMainControl mainCharacter = instance.MainCharacter;
				if (mainCharacter != null)
				{
					Item characterItem = mainCharacter.CharacterItem;
					if (characterItem != null)
					{
						characterItem.TryPlug(this.TargetItem, false, null, 0);
					}
				}
			}
			base.Close();
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x00076668 File Offset: 0x00074868
		private void Unload()
		{
			Item targetItem = this.TargetItem;
			ItemSetting_Gun itemSetting_Gun = (targetItem != null) ? targetItem.GetComponent<ItemSetting_Gun>() : null;
			if (itemSetting_Gun == null)
			{
				return;
			}
			AudioManager.Post("SFX/Combat/Gun/unload");
			itemSetting_Gun.TakeOutAllBullets();
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x000766A3 File Offset: 0x000748A3
		protected override void OnOpen()
		{
			this.fadeGroup.Show();
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x000766B0 File Offset: 0x000748B0
		protected override void OnClose()
		{
			this.fadeGroup.Hide();
			this.displayingItem = null;
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x000766C4 File Offset: 0x000748C4
		public static void Show(ItemDisplay id)
		{
			if (ItemOperationMenu.Instance == null)
			{
				return;
			}
			ItemOperationMenu.Instance.MShow(id);
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x000766DF File Offset: 0x000748DF
		private void MShow(ItemDisplay targetDisplay)
		{
			if (targetDisplay == null)
			{
				return;
			}
			this.TargetDisplay = targetDisplay;
			this.targetView = targetDisplay.GetComponentInParent<View>();
			this.Setup();
			base.Open(null);
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x0007670C File Offset: 0x0007490C
		private void Setup()
		{
			if (this.TargetItem == null)
			{
				return;
			}
			this.displayingItem = this.TargetItem;
			this.icon.sprite = this.TargetItem.Icon;
			this.nameText.text = this.TargetItem.DisplayName;
			this.btn_Use.gameObject.SetActive(this.Usable);
			this.btn_Use.interactable = this.UseButtonInteractable;
			this.btn_Split.gameObject.SetActive(this.Splittable);
			this.btn_Dump.gameObject.SetActive(this.Dumpable);
			this.btn_Equip.gameObject.SetActive(this.Equipable);
			this.btn_Modify.gameObject.SetActive(this.Modifyable);
			this.btn_Unload.gameObject.SetActive(this.Unloadable);
			this.RefreshWeightText();
			this.RefreshPosition();
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x00076804 File Offset: 0x00074A04
		private void RefreshPosition()
		{
			RectTransform rectTransform = this.TargetDisplay.transform as RectTransform;
			Rect rect = rectTransform.rect;
			Vector2 min = rect.min;
			Vector2 max = rect.max;
			Vector3 vector = rectTransform.localToWorldMatrix.MultiplyPoint(min);
			Vector3 vector2 = rectTransform.localToWorldMatrix.MultiplyPoint(max);
			Vector3 vector3 = this.rectTransform.worldToLocalMatrix.MultiplyPoint(vector);
			Vector3 vector4 = this.rectTransform.worldToLocalMatrix.MultiplyPoint(vector2);
			Vector2[] array = new Vector2[]
			{
				new Vector2(vector3.x, vector3.y),
				new Vector2(vector3.x, vector4.y),
				new Vector2(vector4.x, vector3.y),
				new Vector2(vector4.x, vector4.y)
			};
			int num = 0;
			float num2 = float.MaxValue;
			Vector2 center = this.rectTransform.rect.center;
			for (int i = 0; i < array.Length; i++)
			{
				float sqrMagnitude = (array[i] - center).sqrMagnitude;
				if (sqrMagnitude < num2)
				{
					num = i;
					num2 = sqrMagnitude;
				}
			}
			bool flag = (num & 2) > 0;
			bool flag2 = (num & 1) > 0;
			float num3 = flag ? vector4.x : vector3.x;
			float num4 = flag2 ? vector3.y : vector4.y;
			this.contentRectTransform.pivot = new Vector2((float)(flag ? 0 : 1), (float)(flag2 ? 0 : 1));
			this.contentRectTransform.localPosition = new Vector2(num3, num4);
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x000769D8 File Offset: 0x00074BD8
		private void RefreshWeightText()
		{
			if (this.displayingItem == null)
			{
				return;
			}
			this.weightText.text = string.Format(this.weightTextFormat, this.displayingItem.TotalWeight);
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x00076A0F File Offset: 0x00074C0F
		public void OnPointerClick(PointerEventData eventData)
		{
			base.Close();
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x060021D8 RID: 8664 RVA: 0x00076A17 File Offset: 0x00074C17
		private bool Usable
		{
			get
			{
				return this.TargetItem.UsageUtilities != null;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060021D9 RID: 8665 RVA: 0x00076A2A File Offset: 0x00074C2A
		private bool UseButtonInteractable
		{
			get
			{
				if (this.TargetItem)
				{
					Item targetItem = this.TargetItem;
					LevelManager instance = LevelManager.Instance;
					return targetItem.IsUsable((instance != null) ? instance.MainCharacter : null);
				}
				return false;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x060021DA RID: 8666 RVA: 0x00076A58 File Offset: 0x00074C58
		private bool Splittable
		{
			get
			{
				CharacterMainControl main = CharacterMainControl.Main;
				return (main == null || main.CharacterItem.Inventory.GetFirstEmptyPosition(0) >= 0) && (this.TargetItem && this.TargetItem.Stackable) && this.TargetItem.StackCount > 1;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060021DB RID: 8667 RVA: 0x00076AB4 File Offset: 0x00074CB4
		private bool Dumpable
		{
			get
			{
				if (!this.TargetItem.CanDrop)
				{
					return false;
				}
				LevelManager instance = LevelManager.Instance;
				Item item;
				if (instance == null)
				{
					item = null;
				}
				else
				{
					CharacterMainControl mainCharacter = instance.MainCharacter;
					item = ((mainCharacter != null) ? mainCharacter.CharacterItem : null);
				}
				Item item2 = item;
				return ItemTreeExtensions.GetRoot(this.TargetItem) == item2;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060021DC RID: 8668 RVA: 0x00076B04 File Offset: 0x00074D04
		private bool Equipable
		{
			get
			{
				if (this.TargetItem == null)
				{
					return false;
				}
				if (this.TargetItem.PluggedIntoSlot != null)
				{
					return false;
				}
				LevelManager instance = LevelManager.Instance;
				bool? flag;
				if (instance == null)
				{
					flag = null;
				}
				else
				{
					CharacterMainControl mainCharacter = instance.MainCharacter;
					if (mainCharacter == null)
					{
						flag = null;
					}
					else
					{
						Item characterItem = mainCharacter.CharacterItem;
						flag = ((characterItem != null) ? new bool?(characterItem.Slots.Any((Slot e) => e.CanPlug(this.TargetItem))) : null);
					}
				}
				bool? flag2 = flag;
				return flag2 != null && flag2.Value;
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060021DD RID: 8669 RVA: 0x00076B9A File Offset: 0x00074D9A
		private bool Modifyable
		{
			get
			{
				return this.alwaysModifyable;
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060021DE RID: 8670 RVA: 0x00076BA7 File Offset: 0x00074DA7
		private bool Unloadable
		{
			get
			{
				return !(this.TargetItem == null) && this.TargetItem.GetComponent<ItemSetting_Gun>();
			}
		}

		// Token: 0x040016FF RID: 5887
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001700 RID: 5888
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x04001701 RID: 5889
		[SerializeField]
		private RectTransform contentRectTransform;

		// Token: 0x04001702 RID: 5890
		[SerializeField]
		private Image icon;

		// Token: 0x04001703 RID: 5891
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x04001704 RID: 5892
		[SerializeField]
		private TextMeshProUGUI weightText;

		// Token: 0x04001705 RID: 5893
		[SerializeField]
		private string weightTextFormat = "{0:0.#}kg";

		// Token: 0x04001706 RID: 5894
		[SerializeField]
		private Button btn_Use;

		// Token: 0x04001707 RID: 5895
		[SerializeField]
		private Button btn_Split;

		// Token: 0x04001708 RID: 5896
		[SerializeField]
		private Button btn_Dump;

		// Token: 0x04001709 RID: 5897
		[SerializeField]
		private Button btn_Equip;

		// Token: 0x0400170A RID: 5898
		[SerializeField]
		private Button btn_Modify;

		// Token: 0x0400170B RID: 5899
		[SerializeField]
		private Button btn_Unload;

		// Token: 0x0400170C RID: 5900
		[SerializeField]
		private Button btn_Wishlist;

		// Token: 0x0400170D RID: 5901
		[SerializeField]
		private bool alwaysModifyable;

		// Token: 0x0400170E RID: 5902
		private View targetView;

		// Token: 0x0400170F RID: 5903
		private ItemDisplay TargetDisplay;

		// Token: 0x04001710 RID: 5904
		private Item displayingItem;
	}
}
