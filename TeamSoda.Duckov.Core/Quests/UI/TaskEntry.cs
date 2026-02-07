using System;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x02000367 RID: 871
	public class TaskEntry : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001E51 RID: 7761 RVA: 0x0006D086 File Offset: 0x0006B286
		// (set) Token: 0x06001E52 RID: 7762 RVA: 0x0006D08E File Offset: 0x0006B28E
		public bool Interactable
		{
			get
			{
				return this.interactable;
			}
			internal set
			{
				this.interactable = value;
			}
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x0006D097 File Offset: 0x0006B297
		private void Awake()
		{
			this.interactionButton.onClick.AddListener(new UnityAction(this.OnInteractionButtonClicked));
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x0006D0B5 File Offset: 0x0006B2B5
		private void OnInteractionButtonClicked()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.Interact();
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x0006D0D1 File Offset: 0x0006B2D1
		public void NotifyPooled()
		{
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x0006D0D3 File Offset: 0x0006B2D3
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x0006D0E2 File Offset: 0x0006B2E2
		internal void Setup(Task target)
		{
			this.UnregisterEvents();
			this.target = target;
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x0006D100 File Offset: 0x0006B300
		private void Refresh()
		{
			if (this.target == null)
			{
				return;
			}
			this.description.text = this.target.Description;
			foreach (string str in this.target.ExtraDescriptsions)
			{
				TextMeshProUGUI textMeshProUGUI = this.description;
				textMeshProUGUI.text = textMeshProUGUI.text + "  \n- " + str;
			}
			Sprite icon = this.target.Icon;
			if (icon)
			{
				this.taskIcon.sprite = icon;
				this.taskIcon.gameObject.SetActive(true);
			}
			else
			{
				this.taskIcon.gameObject.SetActive(false);
			}
			bool flag = this.target.IsFinished();
			this.statusIcon.sprite = (flag ? this.satisfiedIcon : this.unsatisfiedIcon);
			if (this.Interactable && !flag && this.target.Interactable)
			{
				bool possibleValidInteraction = this.target.PossibleValidInteraction;
				this.interactionText.text = this.target.InteractText;
				this.interactionPlaceHolderText.text = this.target.InteractText;
				this.interactionButton.gameObject.SetActive(possibleValidInteraction);
				this.targetNotInteractablePlaceHolder.gameObject.SetActive(!possibleValidInteraction);
				return;
			}
			this.interactionButton.gameObject.SetActive(false);
			this.targetNotInteractablePlaceHolder.gameObject.SetActive(false);
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x0006D278 File Offset: 0x0006B478
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged += this.OnTargetStatusChanged;
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x0006D2A0 File Offset: 0x0006B4A0
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged -= this.OnTargetStatusChanged;
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x0006D2C8 File Offset: 0x0006B4C8
		private void OnTargetStatusChanged(Task task)
		{
			if (task != this.target)
			{
				Debug.LogError("目标不匹配。");
				return;
			}
			this.Refresh();
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x0006D2E9 File Offset: 0x0006B4E9
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.used)
			{
				return;
			}
			if (CheatMode.Active && UIInputManager.Ctrl && UIInputManager.Alt && UIInputManager.Shift)
			{
				this.target.ForceFinish();
				eventData.Use();
			}
		}

		// Token: 0x04001511 RID: 5393
		[SerializeField]
		private Image statusIcon;

		// Token: 0x04001512 RID: 5394
		[SerializeField]
		private Image taskIcon;

		// Token: 0x04001513 RID: 5395
		[SerializeField]
		private TextMeshProUGUI description;

		// Token: 0x04001514 RID: 5396
		[SerializeField]
		private Button interactionButton;

		// Token: 0x04001515 RID: 5397
		[SerializeField]
		private GameObject targetNotInteractablePlaceHolder;

		// Token: 0x04001516 RID: 5398
		[SerializeField]
		private TextMeshProUGUI interactionText;

		// Token: 0x04001517 RID: 5399
		[SerializeField]
		private TextMeshProUGUI interactionPlaceHolderText;

		// Token: 0x04001518 RID: 5400
		[SerializeField]
		private Sprite unsatisfiedIcon;

		// Token: 0x04001519 RID: 5401
		[SerializeField]
		private Sprite satisfiedIcon;

		// Token: 0x0400151A RID: 5402
		[SerializeField]
		private bool interactable;

		// Token: 0x0400151B RID: 5403
		private Task target;
	}
}
