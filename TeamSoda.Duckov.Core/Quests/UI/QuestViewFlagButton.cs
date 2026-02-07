using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x02000365 RID: 869
	public class QuestViewFlagButton : MonoBehaviour
	{
		// Token: 0x06001E40 RID: 7744 RVA: 0x0006CDAF File Offset: 0x0006AFAF
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
			this.master.onShowingContentChanged += this.OnMasterShowingContentChanged;
			this.Refresh();
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x0006CDEA File Offset: 0x0006AFEA
		private void OnButtonClicked()
		{
			this.master.SetShowingContent(this.content);
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x0006CDFD File Offset: 0x0006AFFD
		private void OnMasterShowingContentChanged(QuestView view, QuestView.ShowContent content)
		{
			this.Refresh();
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x0006CE08 File Offset: 0x0006B008
		private void Refresh()
		{
			bool active = this.master.ShowingContentType == this.content;
			this.selectionIndicator.SetActive(active);
		}

		// Token: 0x04001500 RID: 5376
		[SerializeField]
		private QuestView master;

		// Token: 0x04001501 RID: 5377
		[SerializeField]
		private Button button;

		// Token: 0x04001502 RID: 5378
		[SerializeField]
		private QuestView.ShowContent content;

		// Token: 0x04001503 RID: 5379
		[SerializeField]
		private GameObject selectionIndicator;
	}
}
