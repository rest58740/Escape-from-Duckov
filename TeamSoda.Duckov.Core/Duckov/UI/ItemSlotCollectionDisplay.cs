using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003B8 RID: 952
	public class ItemSlotCollectionDisplay : MonoBehaviour
	{
		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060021E1 RID: 8673 RVA: 0x00076BEF File Offset: 0x00074DEF
		// (set) Token: 0x060021E2 RID: 8674 RVA: 0x00076BF7 File Offset: 0x00074DF7
		public bool Editable
		{
			get
			{
				return this.editable;
			}
			internal set
			{
				this.editable = value;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060021E3 RID: 8675 RVA: 0x00076C00 File Offset: 0x00074E00
		// (set) Token: 0x060021E4 RID: 8676 RVA: 0x00076C08 File Offset: 0x00074E08
		public bool ContentSelectable
		{
			get
			{
				return this.contentSelectable;
			}
			set
			{
				this.contentSelectable = value;
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060021E5 RID: 8677 RVA: 0x00076C11 File Offset: 0x00074E11
		public bool ShowOperationMenu
		{
			get
			{
				return this.showOperationMenu;
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x060021E6 RID: 8678 RVA: 0x00076C19 File Offset: 0x00074E19
		// (set) Token: 0x060021E7 RID: 8679 RVA: 0x00076C21 File Offset: 0x00074E21
		public bool Movable { get; private set; }

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060021E8 RID: 8680 RVA: 0x00076C2A File Offset: 0x00074E2A
		// (set) Token: 0x060021E9 RID: 8681 RVA: 0x00076C32 File Offset: 0x00074E32
		public Item Target { get; private set; }

		// Token: 0x140000E8 RID: 232
		// (add) Token: 0x060021EA RID: 8682 RVA: 0x00076C3C File Offset: 0x00074E3C
		// (remove) Token: 0x060021EB RID: 8683 RVA: 0x00076C74 File Offset: 0x00074E74
		public event Action<ItemSlotCollectionDisplay, SlotDisplay> onElementClicked;

		// Token: 0x140000E9 RID: 233
		// (add) Token: 0x060021EC RID: 8684 RVA: 0x00076CAC File Offset: 0x00074EAC
		// (remove) Token: 0x060021ED RID: 8685 RVA: 0x00076CE4 File Offset: 0x00074EE4
		public event Action<ItemSlotCollectionDisplay, SlotDisplay> onElementDoubleClicked;

		// Token: 0x060021EE RID: 8686 RVA: 0x00076D1C File Offset: 0x00074F1C
		public void Setup(Item target, bool movable = false)
		{
			this.Target = target;
			this.Clear();
			if (this.Target == null)
			{
				return;
			}
			if (this.Target.Slots == null)
			{
				return;
			}
			this.Movable = movable;
			for (int i = 0; i < this.Target.Slots.Count; i++)
			{
				Slot slot = this.Target.Slots[i];
				if (slot != null)
				{
					SlotDisplay slotDisplay = SlotDisplay.Get();
					slotDisplay.onSlotDisplayClicked += this.OnSlotDisplayClicked;
					slotDisplay.onSlotDisplayDoubleClicked += this.OnSlotDisplayDoubleClicked;
					slotDisplay.ShowOperationMenu = this.ShowOperationMenu;
					slotDisplay.Setup(slot);
					slotDisplay.Editable = this.editable;
					slotDisplay.ContentSelectable = this.contentSelectable;
					slotDisplay.transform.SetParent(this.entriesParent, false);
					slotDisplay.Movable = this.Movable;
					this.slots.Add(slotDisplay);
				}
			}
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x00076E15 File Offset: 0x00075015
		private void OnSlotDisplayDoubleClicked(SlotDisplay display)
		{
			Action<ItemSlotCollectionDisplay, SlotDisplay> action = this.onElementDoubleClicked;
			if (action == null)
			{
				return;
			}
			action(this, display);
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x00076E2C File Offset: 0x0007502C
		private void Clear()
		{
			foreach (SlotDisplay slotDisplay in this.slots)
			{
				slotDisplay.onSlotDisplayClicked -= this.OnSlotDisplayClicked;
				SlotDisplay.Release(slotDisplay);
			}
			this.slots.Clear();
			this.entriesParent.DestroyAllChildren();
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x00076EA4 File Offset: 0x000750A4
		private void OnSlotDisplayClicked(SlotDisplay display)
		{
			Action<ItemSlotCollectionDisplay, SlotDisplay> action = this.onElementClicked;
			if (action != null)
			{
				action(this, display);
			}
			if (!this.editable && this.notifyNotEditable)
			{
				this.ShowNotEditableIndicator().Forget();
			}
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x00076ED4 File Offset: 0x000750D4
		private UniTask ShowNotEditableIndicator()
		{
			ItemSlotCollectionDisplay.<ShowNotEditableIndicator>d__36 <ShowNotEditableIndicator>d__;
			<ShowNotEditableIndicator>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowNotEditableIndicator>d__.<>4__this = this;
			<ShowNotEditableIndicator>d__.<>1__state = -1;
			<ShowNotEditableIndicator>d__.<>t__builder.Start<ItemSlotCollectionDisplay.<ShowNotEditableIndicator>d__36>(ref <ShowNotEditableIndicator>d__);
			return <ShowNotEditableIndicator>d__.<>t__builder.Task;
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x00076F55 File Offset: 0x00075155
		[CompilerGenerated]
		private bool <ShowNotEditableIndicator>g__TokenChanged|36_0(ref ItemSlotCollectionDisplay.<>c__DisplayClass36_0 A_1)
		{
			return A_1.token != this.currentToken;
		}

		// Token: 0x04001711 RID: 5905
		[SerializeField]
		private Transform entriesParent;

		// Token: 0x04001712 RID: 5906
		[SerializeField]
		private CanvasGroup notEditableIndicator;

		// Token: 0x04001713 RID: 5907
		[SerializeField]
		private bool editable = true;

		// Token: 0x04001714 RID: 5908
		[SerializeField]
		private bool contentSelectable = true;

		// Token: 0x04001715 RID: 5909
		[SerializeField]
		private bool showOperationMenu = true;

		// Token: 0x04001716 RID: 5910
		[SerializeField]
		private bool notifyNotEditable;

		// Token: 0x04001717 RID: 5911
		[SerializeField]
		private float fadeDuration = 1f;

		// Token: 0x04001718 RID: 5912
		[SerializeField]
		private float sustainDuration = 1f;

		// Token: 0x0400171B RID: 5915
		private List<SlotDisplay> slots = new List<SlotDisplay>();

		// Token: 0x0400171E RID: 5918
		private int currentToken;
	}
}
