using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.MasterKeys.UI
{
	// Token: 0x020002EE RID: 750
	public class MasterKeysIndexEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x0600183F RID: 6207 RVA: 0x000598B0 File Offset: 0x00057AB0
		public int ItemID
		{
			get
			{
				return this.itemID;
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001840 RID: 6208 RVA: 0x000598B8 File Offset: 0x00057AB8
		public string DisplayName
		{
			get
			{
				if (this.status == null)
				{
					return "???";
				}
				if (!this.status.active)
				{
					return "???";
				}
				return this.metaData.DisplayName;
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001841 RID: 6209 RVA: 0x000598E6 File Offset: 0x00057AE6
		public Sprite Icon
		{
			get
			{
				if (this.status == null)
				{
					return this.undiscoveredIcon;
				}
				if (!this.status.active)
				{
					return this.undiscoveredIcon;
				}
				return this.metaData.icon;
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001842 RID: 6210 RVA: 0x00059916 File Offset: 0x00057B16
		public string Description
		{
			get
			{
				if (this.status == null)
				{
					return "???";
				}
				if (!this.status.active)
				{
					return "???";
				}
				return this.metaData.Description;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001843 RID: 6211 RVA: 0x00059944 File Offset: 0x00057B44
		public bool Active
		{
			get
			{
				return this.status != null && this.status.active;
			}
		}

		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x06001844 RID: 6212 RVA: 0x0005995C File Offset: 0x00057B5C
		// (remove) Token: 0x06001845 RID: 6213 RVA: 0x00059994 File Offset: 0x00057B94
		internal event Action<MasterKeysIndexEntry> onPointerClicked;

		// Token: 0x06001846 RID: 6214 RVA: 0x000599C9 File Offset: 0x00057BC9
		public void Setup(int itemID, ISingleSelectionMenu<MasterKeysIndexEntry> menu)
		{
			this.itemID = itemID;
			this.metaData = ItemAssetsCollection.GetMetaData(itemID);
			this.menu = menu;
			this.Refresh();
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x000599EC File Offset: 0x00057BEC
		private void SetupNotDiscovered()
		{
			this.icon.sprite = (this.undiscoveredIcon ? this.undiscoveredIcon : this.metaData.icon);
			this.notDiscoveredLook.ApplyTo(this.icon);
			this.nameText.text = "???";
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x00059A45 File Offset: 0x00057C45
		private void SetupActive()
		{
			this.icon.sprite = this.metaData.icon;
			this.activeLook.ApplyTo(this.icon);
			this.nameText.text = this.metaData.DisplayName;
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x00059A84 File Offset: 0x00057C84
		private void Refresh()
		{
			this.status = MasterKeysManager.GetStatus(this.itemID);
			if (this.status != null)
			{
				if (this.status.active)
				{
					this.SetupActive();
					return;
				}
			}
			else
			{
				this.SetupNotDiscovered();
			}
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x00059AB9 File Offset: 0x00057CB9
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Refresh();
			ISingleSelectionMenu<MasterKeysIndexEntry> singleSelectionMenu = this.menu;
			if (singleSelectionMenu != null)
			{
				singleSelectionMenu.SetSelection(this);
			}
			Action<MasterKeysIndexEntry> action = this.onPointerClicked;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x040011B8 RID: 4536
		[SerializeField]
		private Image icon;

		// Token: 0x040011B9 RID: 4537
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x040011BA RID: 4538
		[SerializeField]
		private MasterKeysIndexEntry.Look notDiscoveredLook;

		// Token: 0x040011BB RID: 4539
		[SerializeField]
		private MasterKeysIndexEntry.Look activeLook;

		// Token: 0x040011BC RID: 4540
		[SerializeField]
		private Sprite undiscoveredIcon;

		// Token: 0x040011BD RID: 4541
		[ItemTypeID]
		private int itemID;

		// Token: 0x040011BE RID: 4542
		private ItemMetaData metaData;

		// Token: 0x040011BF RID: 4543
		private MasterKeysManager.Status status;

		// Token: 0x040011C1 RID: 4545
		private ISingleSelectionMenu<MasterKeysIndexEntry> menu;

		// Token: 0x020005A1 RID: 1441
		[Serializable]
		public struct Look
		{
			// Token: 0x0600299B RID: 10651 RVA: 0x0009A5A9 File Offset: 0x000987A9
			public void ApplyTo(Graphic graphic)
			{
				graphic.material = this.material;
				graphic.color = this.color;
			}

			// Token: 0x0400209B RID: 8347
			public Color color;

			// Token: 0x0400209C RID: 8348
			public Material material;
		}
	}
}
