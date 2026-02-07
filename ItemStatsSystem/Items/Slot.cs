using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;

namespace ItemStatsSystem.Items
{
	// Token: 0x02000029 RID: 41
	[Serializable]
	public class Slot
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000214 RID: 532 RVA: 0x000083F8 File Offset: 0x000065F8
		public Item Master
		{
			get
			{
				SlotCollection slotCollection = this.collection;
				if (slotCollection == null)
				{
					return null;
				}
				return slotCollection.Master;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000840B File Offset: 0x0000660B
		public Item Content
		{
			get
			{
				return this.content;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00008413 File Offset: 0x00006613
		private StringList referenceKeys
		{
			get
			{
				return StringLists.SlotNames;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000841A File Offset: 0x0000661A
		// (set) Token: 0x06000218 RID: 536 RVA: 0x00008422 File Offset: 0x00006622
		public Sprite SlotIcon
		{
			get
			{
				return this.slotIcon;
			}
			set
			{
				this.slotIcon = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0000842B File Offset: 0x0000662B
		public bool ForbidItemsWithSameID
		{
			get
			{
				return this.forbidItemsWithSameID;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600021A RID: 538 RVA: 0x00008433 File Offset: 0x00006633
		public string Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000843C File Offset: 0x0000663C
		public string DisplayName
		{
			get
			{
				if (this.requireTags == null || this.requireTags.Count < 1)
				{
					return "?";
				}
				Tag tag = this.requireTags[0];
				if (tag == null)
				{
					return "?";
				}
				return tag.DisplayName;
			}
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x0600021C RID: 540 RVA: 0x00008488 File Offset: 0x00006688
		// (remove) Token: 0x0600021D RID: 541 RVA: 0x000084C0 File Offset: 0x000066C0
		public event Action<Slot> onSlotContentChanged;

		// Token: 0x0600021E RID: 542 RVA: 0x000084F5 File Offset: 0x000066F5
		public void Initialize(SlotCollection collection)
		{
			this.collection = collection;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x000084FE File Offset: 0x000066FE
		public void ForceInvokeSlotContentChangedEvent()
		{
			Action<Slot> action = this.onSlotContentChanged;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00008514 File Offset: 0x00006714
		public bool Plug(Item otherItem, out Item unpluggedItem)
		{
			unpluggedItem = null;
			if (!this.CheckAbleToPlug(otherItem))
			{
				Debug.Log("Unable to Plug");
				return false;
			}
			if (this.content != null)
			{
				if (this.content.Stackable && this.content.TypeID == otherItem.TypeID)
				{
					this.content.Combine(otherItem);
					this.Master.NotifySlotPlugged(this);
					Action<Slot> action = this.onSlotContentChanged;
					if (action != null)
					{
						action(this);
					}
					this.content.InitiateNotifyItemTreeChanged();
					return otherItem.StackCount <= 0;
				}
				unpluggedItem = this.Unplug();
			}
			if (otherItem.PluggedIntoSlot != null)
			{
				otherItem.Detach();
			}
			if (otherItem.InInventory != null)
			{
				otherItem.Detach();
			}
			this.content = otherItem;
			otherItem.transform.SetParent(this.collection.transform);
			otherItem.NotifyPluggedTo(this);
			this.Master.NotifySlotPlugged(this);
			otherItem.InitiateNotifyItemTreeChanged();
			Action<Slot> action2 = this.onSlotContentChanged;
			if (action2 != null)
			{
				action2(this);
			}
			return true;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000861C File Offset: 0x0000681C
		private bool CheckAbleToPlug(Item otherItem)
		{
			if (otherItem == null)
			{
				return false;
			}
			if (otherItem == this.content)
			{
				return false;
			}
			if (this.forbidItemsWithSameID && this.collection != null)
			{
				foreach (Slot slot in this.collection)
				{
					if (slot != null && slot != this && slot.ForbidItemsWithSameID)
					{
						Item item = slot.Content;
						if (!(item == null) && !(item == otherItem) && item.TypeID == otherItem.TypeID)
						{
							return false;
						}
					}
				}
			}
			return !this.Master.GetAllParents(false).Contains(otherItem) && otherItem.Tags.Check(this.requireTags, this.excludeTags);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00008704 File Offset: 0x00006904
		public Item Unplug()
		{
			Item item = this.content;
			this.content = null;
			if (item != null)
			{
				if (!item.IsBeingDestroyed)
				{
					item.transform.SetParent(null);
				}
				item.NotifyUnpluggedFrom(this);
				this.Master.NotifySlotUnplugged(this);
				item.InitiateNotifyItemTreeChanged();
				this.Master.InitiateNotifyItemTreeChanged();
				Action<Slot> action = this.onSlotContentChanged;
				if (action != null)
				{
					action(this);
				}
			}
			return item;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00008773 File Offset: 0x00006973
		public bool CanPlug(Item item)
		{
			return !(item == null) && this.CheckAbleToPlug(item);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00008787 File Offset: 0x00006987
		public Slot()
		{
		}

		// Token: 0x06000225 RID: 549 RVA: 0x000087A5 File Offset: 0x000069A5
		public Slot(string key)
		{
			this.key = key;
		}

		// Token: 0x040000C1 RID: 193
		[NonSerialized]
		private SlotCollection collection;

		// Token: 0x040000C2 RID: 194
		[SerializeField]
		private string key;

		// Token: 0x040000C3 RID: 195
		[SerializeField]
		private Sprite slotIcon;

		// Token: 0x040000C4 RID: 196
		private Item content;

		// Token: 0x040000C5 RID: 197
		public List<Tag> requireTags = new List<Tag>();

		// Token: 0x040000C6 RID: 198
		public List<Tag> excludeTags = new List<Tag>();

		// Token: 0x040000C7 RID: 199
		[SerializeField]
		private bool forbidItemsWithSameID;
	}
}
