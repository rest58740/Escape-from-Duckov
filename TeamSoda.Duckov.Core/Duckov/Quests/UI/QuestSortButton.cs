using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Quests.UI
{
	// Token: 0x02000361 RID: 865
	public class QuestSortButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001E02 RID: 7682 RVA: 0x0006C3D4 File Offset: 0x0006A5D4
		public Quest.SortingMode SortingMode
		{
			get
			{
				if (this.entries.Length == 0)
				{
					Debug.LogError("Error: Entries not configured for sorting mode button of quest ui.");
					return Quest.SortingMode.Default;
				}
				if (this.index < 0)
				{
					this.index = 0;
				}
				else if (this.index >= this.entries.Length)
				{
					this.index = 0;
				}
				return this.entries[this.index].mode;
			}
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x0006C438 File Offset: 0x0006A638
		private void Start()
		{
			this.Refresh();
			if (this.targetBehaviour == null)
			{
				return;
			}
			IQuestSortable questSortable = this.targetBehaviour as IQuestSortable;
			if (questSortable == null)
			{
				return;
			}
			this.target = questSortable;
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x0006C471 File Offset: 0x0006A671
		public void OnPointerClick(PointerEventData eventData)
		{
			eventData.Use();
			this.index++;
			if (this.index >= this.entries.Length)
			{
				this.index = 0;
			}
			this.Refresh();
			this.Apply();
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x0006C4AC File Offset: 0x0006A6AC
		private void Refresh()
		{
			if (this.entries.Length == 0)
			{
				return;
			}
			if (this.index < 0 || this.index >= this.entries.Length)
			{
				return;
			}
			QuestSortButton.Entry entry = this.entries[this.index];
			this.text.text = LocalizationManager.ToPlainText(entry.displayNameKey);
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x0006C505 File Offset: 0x0006A705
		private void Apply()
		{
			this.target.SortingMode = this.SortingMode;
		}

		// Token: 0x040014DD RID: 5341
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040014DE RID: 5342
		[SerializeField]
		private MonoBehaviour targetBehaviour;

		// Token: 0x040014DF RID: 5343
		[SerializeField]
		private QuestSortButton.Entry[] entries;

		// Token: 0x040014E0 RID: 5344
		private int index;

		// Token: 0x040014E1 RID: 5345
		private IQuestSortable target;

		// Token: 0x02000628 RID: 1576
		[Serializable]
		private struct Entry
		{
			// Token: 0x0400223D RID: 8765
			[LocalizationKey("Default")]
			public string displayNameKey;

			// Token: 0x0400223E RID: 8766
			public Quest.SortingMode mode;
		}
	}
}
