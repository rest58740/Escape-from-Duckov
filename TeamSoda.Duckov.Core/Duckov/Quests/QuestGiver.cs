using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Buildings;
using Duckov.Quests.UI;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x0200034C RID: 844
	public class QuestGiver : InteractableBase
	{
		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001CF7 RID: 7415 RVA: 0x000697B5 File Offset: 0x000679B5
		private IEnumerable<Quest> PossibleQuests
		{
			get
			{
				if (this._possibleQuests == null && QuestManager.Instance != null)
				{
					this._possibleQuests = QuestManager.Instance.GetAllQuestsByQuestGiverID(this.questGiverID);
				}
				return this._possibleQuests;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x000697E8 File Offset: 0x000679E8
		public QuestGiverID ID
		{
			get
			{
				return this.questGiverID;
			}
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x000697F0 File Offset: 0x000679F0
		protected override void Awake()
		{
			base.Awake();
			QuestManager.onQuestListsChanged += this.OnQuestListsChanged;
			BuildingManager.OnBuildingBuilt += this.OnBuildingBuilt;
			QuestManager.OnTaskFinishedEvent = (Action<Quest, Task>)Delegate.Combine(QuestManager.OnTaskFinishedEvent, new Action<Quest, Task>(this.OnTaskFinished));
			this.inspectionIndicator = Object.Instantiate<GameObject>(GameplayDataSettings.Prefabs.QuestMarker);
			this.inspectionIndicator.transform.SetParent(base.transform);
			this.inspectionIndicator.transform.position = base.transform.TransformPoint(this.interactMarkerOffset + Vector3.up * 0.5f);
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x000698A5 File Offset: 0x00067AA5
		protected override void Start()
		{
			base.Start();
			this.RefreshInspectionIndicator();
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x000698B4 File Offset: 0x00067AB4
		protected override void OnDestroy()
		{
			base.OnDestroy();
			QuestManager.onQuestListsChanged -= this.OnQuestListsChanged;
			BuildingManager.OnBuildingBuilt -= this.OnBuildingBuilt;
			QuestManager.OnTaskFinishedEvent = (Action<Quest, Task>)Delegate.Remove(QuestManager.OnTaskFinishedEvent, new Action<Quest, Task>(this.OnTaskFinished));
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x00069909 File Offset: 0x00067B09
		private void OnTaskFinished(Quest quest, Task task)
		{
			this.RefreshInspectionIndicator();
		}

		// Token: 0x06001CFD RID: 7421 RVA: 0x00069911 File Offset: 0x00067B11
		private void OnBuildingBuilt(int buildingID)
		{
			this.RefreshInspectionIndicator();
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x00069919 File Offset: 0x00067B19
		private bool AnyQuestNeedsInspection()
		{
			return QuestManager.GetActiveQuestsFromGiver(this.questGiverID).Any((Quest e) => e != null && e.NeedInspection);
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x0006994C File Offset: 0x00067B4C
		private bool AnyQuestAvaliable()
		{
			using (IEnumerator<Quest> enumerator = this.PossibleQuests.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (QuestManager.IsQuestAvaliable(enumerator.Current.ID))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x000699A4 File Offset: 0x00067BA4
		private void OnQuestListsChanged(QuestManager manager)
		{
			this.RefreshInspectionIndicator();
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x000699AC File Offset: 0x00067BAC
		private void RefreshInspectionIndicator()
		{
			if (this.inspectionIndicator)
			{
				bool flag = this.AnyQuestNeedsInspection();
				bool flag2 = this.AnyQuestAvaliable();
				bool active = flag || flag2;
				this.inspectionIndicator.gameObject.SetActive(active);
			}
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x000699E7 File Offset: 0x00067BE7
		public void ActivateQuest(Quest quest)
		{
			QuestManager.Instance.ActivateQuest(quest.ID, new QuestGiverID?(this.questGiverID));
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x00069A04 File Offset: 0x00067C04
		internal List<Quest> GetAvaliableQuests()
		{
			List<Quest> list = new List<Quest>();
			foreach (Quest quest in this.PossibleQuests)
			{
				if (QuestManager.IsQuestAvaliable(quest.ID))
				{
					list.Add(quest);
				}
			}
			return list;
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x00069A68 File Offset: 0x00067C68
		protected override void OnInteractStart(CharacterMainControl interactCharacter)
		{
			base.OnInteractStart(interactCharacter);
			QuestGiverView instance = QuestGiverView.Instance;
			if (instance == null)
			{
				base.StopInteract();
				return;
			}
			instance.Setup(this);
			instance.Open(null);
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x00069AA0 File Offset: 0x00067CA0
		protected override void OnInteractStop()
		{
			base.OnInteractStop();
			if (QuestGiverView.Instance && QuestGiverView.Instance.open)
			{
				QuestGiverView instance = QuestGiverView.Instance;
				if (instance == null)
				{
					return;
				}
				instance.Close();
			}
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x00069ACF File Offset: 0x00067CCF
		protected override void OnUpdate(CharacterMainControl _interactCharacter, float deltaTime)
		{
			base.OnUpdate(_interactCharacter, deltaTime);
			if (!QuestGiverView.Instance || !QuestGiverView.Instance.open)
			{
				base.StopInteract();
			}
		}

		// Token: 0x0400145C RID: 5212
		[SerializeField]
		private QuestGiverID questGiverID;

		// Token: 0x0400145D RID: 5213
		private GameObject inspectionIndicator;

		// Token: 0x0400145E RID: 5214
		private IEnumerable<Quest> _possibleQuests;

		// Token: 0x0400145F RID: 5215
		private List<Quest> avaliableQuests = new List<Quest>();
	}
}
