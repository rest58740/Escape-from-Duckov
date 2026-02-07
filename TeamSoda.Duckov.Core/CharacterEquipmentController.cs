using System;
using System.Linq;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class CharacterEquipmentController : MonoBehaviour
{
	// Token: 0x14000009 RID: 9
	// (add) Token: 0x0600028A RID: 650 RVA: 0x0000B6DC File Offset: 0x000098DC
	// (remove) Token: 0x0600028B RID: 651 RVA: 0x0000B714 File Offset: 0x00009914
	public event Action<Slot> OnHelmatSlotContentChanged;

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x0600028C RID: 652 RVA: 0x0000B74C File Offset: 0x0000994C
	// (remove) Token: 0x0600028D RID: 653 RVA: 0x0000B784 File Offset: 0x00009984
	public event Action<Slot> OnFaceMaskSlotContentChanged;

	// Token: 0x0600028E RID: 654 RVA: 0x0000B7BC File Offset: 0x000099BC
	public void SetItem(Item _item)
	{
		this.characterItem = _item;
		this.armorSlot = this.characterItem.Slots.GetSlot(CharacterEquipmentController.armorHash);
		this.helmatSlot = this.characterItem.Slots.GetSlot(CharacterEquipmentController.helmatHash);
		this.faceMaskSlot = this.characterItem.Slots.GetSlot(CharacterEquipmentController.faceMaskHash);
		this.backpackSlot = this.characterItem.Slots.GetSlot(CharacterEquipmentController.backpackHash);
		this.headsetSlot = this.characterItem.Slots.GetSlot(CharacterEquipmentController.headsetHash);
		this.armorSlot.onSlotContentChanged += this.ChangeArmorModel;
		this.helmatSlot.onSlotContentChanged += this.ChangeHelmatModel;
		this.faceMaskSlot.onSlotContentChanged += this.ChangeFaceMaskModel;
		this.backpackSlot.onSlotContentChanged += this.ChangeBackpackModel;
		this.headsetSlot.onSlotContentChanged += this.ChangeHeadsetModel;
		Slot slot = this.armorSlot;
		if (((slot != null) ? slot.Content : null) != null)
		{
			this.ChangeArmorModel(this.armorSlot);
		}
		Slot slot2 = this.helmatSlot;
		if (((slot2 != null) ? slot2.Content : null) != null)
		{
			this.ChangeHelmatModel(this.helmatSlot);
		}
		Slot slot3 = this.faceMaskSlot;
		if (((slot3 != null) ? slot3.Content : null) != null)
		{
			this.ChangeFaceMaskModel(this.faceMaskSlot);
		}
		Slot slot4 = this.backpackSlot;
		if (((slot4 != null) ? slot4.Content : null) != null)
		{
			this.ChangeBackpackModel(this.backpackSlot);
		}
		Slot slot5 = this.headsetSlot;
		if (((slot5 != null) ? slot5.Content : null) != null)
		{
			this.ChangeHeadsetModel(this.headsetSlot);
		}
	}

	// Token: 0x0600028F RID: 655 RVA: 0x0000B988 File Offset: 0x00009B88
	private void OnDestroy()
	{
		if (this.armorSlot != null)
		{
			this.armorSlot.onSlotContentChanged -= this.ChangeArmorModel;
		}
		if (this.helmatSlot != null)
		{
			this.helmatSlot.onSlotContentChanged -= this.ChangeHelmatModel;
		}
		if (this.backpackSlot != null)
		{
			this.backpackSlot.onSlotContentChanged -= this.ChangeBackpackModel;
		}
		if (this.faceMaskSlot != null)
		{
			this.faceMaskSlot.onSlotContentChanged -= this.ChangeFaceMaskModel;
		}
	}

	// Token: 0x06000290 RID: 656 RVA: 0x0000BA14 File Offset: 0x00009C14
	private void ChangeArmorModel(Slot slot)
	{
		if (this.characterMainControl.characterModel == null)
		{
			return;
		}
		Transform armorSocket = this.characterMainControl.characterModel.ArmorSocket;
		this.ChangeEquipmentModel(slot, armorSocket);
	}

	// Token: 0x06000291 RID: 657 RVA: 0x0000BA50 File Offset: 0x00009C50
	private void ChangeHelmatModel(Slot slot)
	{
		Action<Slot> onHelmatSlotContentChanged = this.OnHelmatSlotContentChanged;
		if (onHelmatSlotContentChanged != null)
		{
			onHelmatSlotContentChanged(slot);
		}
		if (this.characterMainControl.characterModel == null)
		{
			return;
		}
		Transform helmatSocket = this.characterMainControl.characterModel.HelmatSocket;
		this.ChangeEquipmentModel(slot, helmatSocket);
	}

	// Token: 0x06000292 RID: 658 RVA: 0x0000BA9C File Offset: 0x00009C9C
	private void ChangeHeadsetModel(Slot slot)
	{
		if (this.characterMainControl.characterModel == null)
		{
			return;
		}
		Transform helmatSocket = this.characterMainControl.characterModel.HelmatSocket;
		this.ChangeEquipmentModel(slot, helmatSocket);
	}

	// Token: 0x06000293 RID: 659 RVA: 0x0000BAD8 File Offset: 0x00009CD8
	private void ChangeBackpackModel(Slot slot)
	{
		if (this.characterMainControl.characterModel == null)
		{
			return;
		}
		Transform backpackSocket = this.characterMainControl.characterModel.BackpackSocket;
		this.ChangeEquipmentModel(slot, backpackSocket);
	}

	// Token: 0x06000294 RID: 660 RVA: 0x0000BB14 File Offset: 0x00009D14
	private void ChangeFaceMaskModel(Slot slot)
	{
		Action<Slot> onFaceMaskSlotContentChanged = this.OnFaceMaskSlotContentChanged;
		if (onFaceMaskSlotContentChanged != null)
		{
			onFaceMaskSlotContentChanged(slot);
		}
		if (this.characterMainControl.characterModel == null)
		{
			return;
		}
		Transform faceMaskSocket = this.characterMainControl.characterModel.FaceMaskSocket;
		this.ChangeEquipmentModel(slot, faceMaskSocket);
	}

	// Token: 0x06000295 RID: 661 RVA: 0x0000BB60 File Offset: 0x00009D60
	private void ChangeEquipmentModel(Slot slot, Transform socket)
	{
		if (slot == null)
		{
			return;
		}
		if (slot.Content == null)
		{
			return;
		}
		ItemAgent itemAgent = slot.Content.AgentUtilities.CreateAgent(CharacterEquipmentController.equipmentModelHash, 3);
		if (itemAgent == null)
		{
			Debug.LogError("生成的装备Item没有装备agent，Item名称：" + slot.Content.gameObject.name);
		}
		if (itemAgent != null)
		{
			itemAgent.transform.SetParent(socket, false);
			itemAgent.transform.localRotation = Quaternion.identity;
			itemAgent.transform.localPosition = Vector3.zero;
		}
	}

	// Token: 0x06000296 RID: 662 RVA: 0x0000BBF8 File Offset: 0x00009DF8
	private bool IsSlotRequireTag(Slot slot, Tag tag)
	{
		return slot.requireTags.Any((Tag e) => e.Hash == tag.Hash);
	}

	// Token: 0x04000208 RID: 520
	[SerializeField]
	private CharacterMainControl characterMainControl;

	// Token: 0x04000209 RID: 521
	private Item characterItem;

	// Token: 0x0400020A RID: 522
	public static int equipmentModelHash = "EquipmentModel".GetHashCode();

	// Token: 0x0400020B RID: 523
	public static int armorHash = "Armor".GetHashCode();

	// Token: 0x0400020C RID: 524
	public static int helmatHash = "Helmat".GetHashCode();

	// Token: 0x0400020D RID: 525
	public static int faceMaskHash = "FaceMask".GetHashCode();

	// Token: 0x0400020E RID: 526
	public static int backpackHash = "Backpack".GetHashCode();

	// Token: 0x0400020F RID: 527
	public static int headsetHash = "Headset".GetHashCode();

	// Token: 0x04000210 RID: 528
	private Slot armorSlot;

	// Token: 0x04000211 RID: 529
	private Slot helmatSlot;

	// Token: 0x04000212 RID: 530
	private Slot backpackSlot;

	// Token: 0x04000213 RID: 531
	private Slot faceMaskSlot;

	// Token: 0x04000214 RID: 532
	private Slot headsetSlot;
}
