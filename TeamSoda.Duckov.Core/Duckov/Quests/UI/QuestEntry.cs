using System;
using Duckov.Utilities;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Quests.UI
{
	// Token: 0x0200035C RID: 860
	public class QuestEntry : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001DBF RID: 7615 RVA: 0x0006B7B8 File Offset: 0x000699B8
		public Quest Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x140000D8 RID: 216
		// (add) Token: 0x06001DC0 RID: 7616 RVA: 0x0006B7C0 File Offset: 0x000699C0
		// (remove) Token: 0x06001DC1 RID: 7617 RVA: 0x0006B7F8 File Offset: 0x000699F8
		public event Action<QuestEntry, PointerEventData> onClick;

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001DC2 RID: 7618 RVA: 0x0006B82D File Offset: 0x00069A2D
		public bool Selected
		{
			get
			{
				return this.menu.GetSelection() == this;
			}
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x0006B840 File Offset: 0x00069A40
		public void NotifyPooled()
		{
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x0006B842 File Offset: 0x00069A42
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x0006B851 File Offset: 0x00069A51
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x0006B859 File Offset: 0x00069A59
		internal void Setup(Quest quest)
		{
			this.UnregisterEvents();
			this.target = quest;
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x0006B874 File Offset: 0x00069A74
		internal void SetMenu(ISingleSelectionMenu<QuestEntry> menu)
		{
			this.menu = menu;
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x0006B87D File Offset: 0x00069A7D
		private void RegisterEvents()
		{
			if (this.target != null)
			{
				this.target.onStatusChanged += this.OnTargetStatusChanged;
				this.target.onNeedInspectionChanged += this.OnNeedInspectionChanged;
			}
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x0006B8BB File Offset: 0x00069ABB
		private void UnregisterEvents()
		{
			if (this.target != null)
			{
				this.target.onStatusChanged -= this.OnTargetStatusChanged;
				this.target.onNeedInspectionChanged -= this.OnNeedInspectionChanged;
			}
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x0006B8F9 File Offset: 0x00069AF9
		private void OnNeedInspectionChanged(Quest obj)
		{
			this.Refresh();
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x0006B901 File Offset: 0x00069B01
		private void OnTargetStatusChanged(Quest quest)
		{
			this.Refresh();
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x0006B909 File Offset: 0x00069B09
		private void OnMasterSelectionChanged(QuestView view, Quest oldSelection, Quest newSelection)
		{
			this.Refresh();
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x0006B914 File Offset: 0x00069B14
		private void Refresh()
		{
			this.selectionIndicator.SetActive(this.Selected);
			this.displayName.text = this.target.DisplayName;
			this.questIDDisplay.text = string.Format("{0:0000}", this.target.ID);
			SceneInfoEntry requireSceneInfo = this.target.RequireSceneInfo;
			if (requireSceneInfo == null)
			{
				this.locationName.text = LocalizationManager.ToPlainText(this.anyLocationKey);
			}
			else
			{
				this.locationName.text = requireSceneInfo.DisplayName;
			}
			this.redDot.SetActive(this.target.NeedInspection);
			this.claimableIndicator.SetActive(this.target.Complete || this.target.AreTasksFinished());
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x0006B9E1 File Offset: 0x00069BE1
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<QuestEntry, PointerEventData> action = this.onClick;
			if (action != null)
			{
				action(this, eventData);
			}
			this.menu.SetSelection(this);
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x0006BA03 File Offset: 0x00069C03
		public void NotifyRefresh()
		{
			this.Refresh();
		}

		// Token: 0x040014AB RID: 5291
		private ISingleSelectionMenu<QuestEntry> menu;

		// Token: 0x040014AC RID: 5292
		private Quest target;

		// Token: 0x040014AD RID: 5293
		[SerializeField]
		private GameObject selectionIndicator;

		// Token: 0x040014AE RID: 5294
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x040014AF RID: 5295
		[SerializeField]
		private TextMeshProUGUI locationName;

		// Token: 0x040014B0 RID: 5296
		[SerializeField]
		[LocalizationKey("Default")]
		private string anyLocationKey;

		// Token: 0x040014B1 RID: 5297
		[SerializeField]
		private GameObject redDot;

		// Token: 0x040014B2 RID: 5298
		[SerializeField]
		private GameObject claimableIndicator;

		// Token: 0x040014B3 RID: 5299
		[SerializeField]
		private TextMeshProUGUI questIDDisplay;
	}
}
