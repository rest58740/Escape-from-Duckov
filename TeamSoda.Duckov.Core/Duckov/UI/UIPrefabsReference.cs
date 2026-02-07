using System;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x0200038E RID: 910
	[CreateAssetMenu]
	public class UIPrefabsReference : ScriptableObject
	{
		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001F99 RID: 8089 RVA: 0x0006FAD9 File Offset: 0x0006DCD9
		public ItemDisplay ItemDisplay
		{
			get
			{
				return this.itemDisplay;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001F9A RID: 8090 RVA: 0x0006FAE1 File Offset: 0x0006DCE1
		public SlotIndicator SlotIndicator
		{
			get
			{
				return this.slotIndicator;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001F9B RID: 8091 RVA: 0x0006FAE9 File Offset: 0x0006DCE9
		public SlotDisplay SlotDisplay
		{
			get
			{
				return this.slotDisplay;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001F9C RID: 8092 RVA: 0x0006FAF1 File Offset: 0x0006DCF1
		public InventoryEntry InventoryEntry
		{
			get
			{
				return this.inventoryEntry;
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001F9D RID: 8093 RVA: 0x0006FAF9 File Offset: 0x0006DCF9
		public Button Button
		{
			get
			{
				return this.button;
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001F9E RID: 8094 RVA: 0x0006FB01 File Offset: 0x0006DD01
		public ScrollRect ScrollRect
		{
			get
			{
				return this.scrollRect;
			}
		}

		// Token: 0x0400159A RID: 5530
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x0400159B RID: 5531
		[SerializeField]
		private SlotIndicator slotIndicator;

		// Token: 0x0400159C RID: 5532
		[SerializeField]
		private SlotDisplay slotDisplay;

		// Token: 0x0400159D RID: 5533
		[SerializeField]
		private InventoryEntry inventoryEntry;

		// Token: 0x0400159E RID: 5534
		[SerializeField]
		private Button button;

		// Token: 0x0400159F RID: 5535
		[SerializeField]
		private ScrollRect scrollRect;
	}
}
