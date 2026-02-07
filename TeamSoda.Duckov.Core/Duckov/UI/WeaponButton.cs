using System;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using LeTai.TrueShadow;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003BF RID: 959
	public class WeaponButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x140000ED RID: 237
		// (add) Token: 0x06002247 RID: 8775 RVA: 0x00077F54 File Offset: 0x00076154
		// (remove) Token: 0x06002248 RID: 8776 RVA: 0x00077F88 File Offset: 0x00076188
		public static event Action<WeaponButton> OnWeaponButtonSelected;

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06002249 RID: 8777 RVA: 0x00077FBB File Offset: 0x000761BB
		private CharacterMainControl Character
		{
			get
			{
				return this._character;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x0600224A RID: 8778 RVA: 0x00077FC3 File Offset: 0x000761C3
		private Slot TargetSlot
		{
			get
			{
				return this._targetSlot;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x0600224B RID: 8779 RVA: 0x00077FCB File Offset: 0x000761CB
		private Item TargetItem
		{
			get
			{
				Slot targetSlot = this.TargetSlot;
				if (targetSlot == null)
				{
					return null;
				}
				return targetSlot.Content;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x0600224C RID: 8780 RVA: 0x00077FE0 File Offset: 0x000761E0
		private bool IsSelected
		{
			get
			{
				Item targetItem = this.TargetItem;
				if (((targetItem != null) ? targetItem.ActiveAgent : null) != null)
				{
					Object activeAgent = this.TargetItem.ActiveAgent;
					ItemAgentHolder agentHolder = this._character.agentHolder;
					return activeAgent == ((agentHolder != null) ? agentHolder.CurrentHoldItemAgent : null);
				}
				return false;
			}
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x00078030 File Offset: 0x00076230
		private void Awake()
		{
			this.RegisterStaticEvents();
			LevelManager instance = LevelManager.Instance;
			if (((instance != null) ? instance.MainCharacter : null) != null)
			{
				this.Initialize(LevelManager.Instance.MainCharacter);
			}
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x00078061 File Offset: 0x00076261
		private void OnDestroy()
		{
			this.UnregisterStaticEvents();
			this.isBeingDestroyed = true;
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x00078070 File Offset: 0x00076270
		private void RegisterStaticEvents()
		{
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent = (Action<CharacterMainControl, DuckovItemAgent>)Delegate.Combine(CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent, new Action<CharacterMainControl, DuckovItemAgent>(this.OnMainCharacterChangeHoldItemAgent));
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x000780A3 File Offset: 0x000762A3
		private void UnregisterStaticEvents()
		{
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
			CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent = (Action<CharacterMainControl, DuckovItemAgent>)Delegate.Remove(CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent, new Action<CharacterMainControl, DuckovItemAgent>(this.OnMainCharacterChangeHoldItemAgent));
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x000780D6 File Offset: 0x000762D6
		private void OnMainCharacterChangeHoldItemAgent(CharacterMainControl control, DuckovItemAgent agent)
		{
			if (this._character && control == this._character)
			{
				this.Refresh();
			}
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x000780F9 File Offset: 0x000762F9
		private void OnLevelInitialized()
		{
			LevelManager instance = LevelManager.Instance;
			this.Initialize((instance != null) ? instance.MainCharacter : null);
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x00078114 File Offset: 0x00076314
		private void Initialize(CharacterMainControl character)
		{
			this.UnregisterEvents();
			this._character = character;
			if (character == null)
			{
				Debug.LogError("Character 不存在，初始化失败");
			}
			if (character.CharacterItem == null)
			{
				Debug.LogError("Character item 不存在，初始化失败");
			}
			this._targetSlot = character.CharacterItem.Slots.GetSlot(this.targetSlotKey);
			if (this._targetSlot == null)
			{
				Debug.LogError("Slot " + this.targetSlotKey + " 不存在，初始化失败");
			}
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x000781A3 File Offset: 0x000763A3
		private void RegisterEvents()
		{
			if (this._targetSlot == null)
			{
				return;
			}
			this._targetSlot.onSlotContentChanged += this.OnSlotContentChanged;
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x000781C5 File Offset: 0x000763C5
		private void UnregisterEvents()
		{
			if (this._targetSlot == null)
			{
				return;
			}
			this._targetSlot.onSlotContentChanged -= this.OnSlotContentChanged;
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x000781E7 File Offset: 0x000763E7
		private void OnSlotContentChanged(Slot slot)
		{
			this.Refresh();
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x000781F0 File Offset: 0x000763F0
		private void Refresh()
		{
			if (this.isBeingDestroyed)
			{
				return;
			}
			this.displayParent.SetActive(this.TargetItem);
			bool isSelected = this.IsSelected;
			if (this.TargetItem)
			{
				this.icon.sprite = this.TargetItem.Icon;
				ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = GameplayDataSettings.UIStyle.GetShadowOffsetAndColorOfQuality(this.TargetItem.DisplayQuality);
				this.iconShadow.Inset = shadowOffsetAndColorOfQuality.Item3;
				this.iconShadow.Color = shadowOffsetAndColorOfQuality.Item2;
				this.iconShadow.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
				this.selectionFrame.SetActive(isSelected);
			}
			UnityEvent<WeaponButton> unityEvent = this.onRefresh;
			if (unityEvent != null)
			{
				unityEvent.Invoke(this);
			}
			if (isSelected)
			{
				UnityEvent<WeaponButton> unityEvent2 = this.onSelected;
				if (unityEvent2 != null)
				{
					unityEvent2.Invoke(this);
				}
				Action<WeaponButton> onWeaponButtonSelected = WeaponButton.OnWeaponButtonSelected;
				if (onWeaponButtonSelected == null)
				{
					return;
				}
				onWeaponButtonSelected(this);
			}
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x000782D2 File Offset: 0x000764D2
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.Character == null)
			{
				return;
			}
			UnityEvent<WeaponButton> unityEvent = this.onClick;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(this);
		}

		// Token: 0x04001741 RID: 5953
		[SerializeField]
		private string targetSlotKey = "PrimaryWeapon";

		// Token: 0x04001742 RID: 5954
		[SerializeField]
		private GameObject displayParent;

		// Token: 0x04001743 RID: 5955
		[SerializeField]
		private Image icon;

		// Token: 0x04001744 RID: 5956
		[SerializeField]
		private TrueShadow iconShadow;

		// Token: 0x04001745 RID: 5957
		[SerializeField]
		private GameObject selectionFrame;

		// Token: 0x04001746 RID: 5958
		public UnityEvent<WeaponButton> onClick;

		// Token: 0x04001747 RID: 5959
		public UnityEvent<WeaponButton> onRefresh;

		// Token: 0x04001748 RID: 5960
		public UnityEvent<WeaponButton> onSelected;

		// Token: 0x0400174A RID: 5962
		private CharacterMainControl _character;

		// Token: 0x0400174B RID: 5963
		private Slot _targetSlot;

		// Token: 0x0400174C RID: 5964
		private bool isBeingDestroyed;
	}
}
