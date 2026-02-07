using System;
using Duckov.Economy;
using Duckov.Quests;
using Duckov.Scenes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x020003D4 RID: 980
	public class MapSelectionEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x060023A0 RID: 9120 RVA: 0x0007D1EA File Offset: 0x0007B3EA
		public Cost Cost
		{
			get
			{
				return this.cost;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x060023A1 RID: 9121 RVA: 0x0007D1F2 File Offset: 0x0007B3F2
		public bool ConditionsSatisfied
		{
			get
			{
				return this.conditions == null || this.conditions.Satisfied();
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x060023A2 RID: 9122 RVA: 0x0007D209 File Offset: 0x0007B409
		public string SceneID
		{
			get
			{
				return this.sceneID;
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x060023A3 RID: 9123 RVA: 0x0007D211 File Offset: 0x0007B411
		public int BeaconIndex
		{
			get
			{
				return this.beaconIndex;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x060023A4 RID: 9124 RVA: 0x0007D219 File Offset: 0x0007B419
		public Sprite FullScreenImage
		{
			get
			{
				return this.fullScreenImage;
			}
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x0007D221 File Offset: 0x0007B421
		public void Setup(MapSelectionView master)
		{
			this.master = master;
			this.Refresh();
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x0007D230 File Offset: 0x0007B430
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x0007D238 File Offset: 0x0007B438
		public void OnPointerClick(PointerEventData eventData)
		{
			if (!this.ConditionsSatisfied)
			{
				return;
			}
			this.master.NotifyEntryClicked(this, eventData);
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x0007D250 File Offset: 0x0007B450
		private void Refresh()
		{
			SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(this.sceneID);
			this.displayNameText.text = sceneInfo.DisplayName;
			this.lockedIndicator.gameObject.SetActive(!this.ConditionsSatisfied);
			this.costDisplay.Setup(this.cost, 1);
			this.costDisplay.gameObject.SetActive(!this.cost.IsFree);
		}

		// Token: 0x0400182E RID: 6190
		[SerializeField]
		private MapSelectionView master;

		// Token: 0x0400182F RID: 6191
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x04001830 RID: 6192
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x04001831 RID: 6193
		[SerializeField]
		private GameObject lockedIndicator;

		// Token: 0x04001832 RID: 6194
		[SerializeField]
		private Condition[] conditions;

		// Token: 0x04001833 RID: 6195
		[SerializeField]
		private Cost cost;

		// Token: 0x04001834 RID: 6196
		[SerializeField]
		[SceneID]
		private string sceneID;

		// Token: 0x04001835 RID: 6197
		[SerializeField]
		private int beaconIndex;

		// Token: 0x04001836 RID: 6198
		[SerializeField]
		private Sprite fullScreenImage;
	}
}
