using System;
using Duckov.Buffs;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x0200038F RID: 911
	public class BuffDetailsOverlay : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001FA0 RID: 8096 RVA: 0x0006FB11 File Offset: 0x0006DD11
		public Buff Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x0006FB19 File Offset: 0x0006DD19
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
			BuffsDisplayEntry.OnBuffsDisplayEntryClicked += this.OnBuffsDisplayEntryClicked;
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x0006FB3D File Offset: 0x0006DD3D
		private void OnDestroy()
		{
			BuffsDisplayEntry.OnBuffsDisplayEntryClicked -= this.OnBuffsDisplayEntryClicked;
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x0006FB50 File Offset: 0x0006DD50
		private void OnBuffsDisplayEntryClicked(BuffsDisplayEntry entry, PointerEventData eventData)
		{
			if (this.fadeGroup.IsShown && this.target == entry.Target)
			{
				this.fadeGroup.Hide();
				this.punchReceiver.Punch();
				return;
			}
			this.Setup(entry);
			this.Show();
			this.punchReceiver.Punch();
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x0006FBAC File Offset: 0x0006DDAC
		public void Setup(Buff target)
		{
			this.target = target;
			if (target == null)
			{
				return;
			}
			this.text_BuffName.text = target.DisplayName;
			this.text_BuffDescription.text = target.Description;
			this.RefreshCountDown();
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x0006FBE8 File Offset: 0x0006DDE8
		private void Update()
		{
			if (this.fadeGroup.IsShown || this.fadeGroup.IsShowingInProgress)
			{
				if (this.target != null)
				{
					this.RefreshCountDown();
				}
				else
				{
					this.fadeGroup.Hide();
				}
				if (this.TimeSinceShowStarted > this.disappearAfterSeconds)
				{
					this.fadeGroup.Hide();
				}
			}
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x0006FC4C File Offset: 0x0006DE4C
		public void Setup(BuffsDisplayEntry target)
		{
			if (target == null)
			{
				return;
			}
			this.Setup((target != null) ? target.Target : null);
			RectTransform rectTransform = target.Icon.rectTransform;
			Vector3 position = rectTransform.TransformPoint(rectTransform.rect.max);
			this.rectTransform.pivot = Vector2.up;
			this.rectTransform.position = position;
			this.rectTransform.SetAsLastSibling();
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x0006FCC0 File Offset: 0x0006DEC0
		private void RefreshCountDown()
		{
			if (this.target == null)
			{
				return;
			}
			if (this.target.LimitedLifeTime)
			{
				float remainingTime = this.target.RemainingTime;
				this.text_CountDown.text = string.Format("{0:0.0}s", remainingTime);
				return;
			}
			this.text_CountDown.text = "";
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x0006FD21 File Offset: 0x0006DF21
		private float TimeSinceShowStarted
		{
			get
			{
				return Time.unscaledTime - this.timeWhenShowStarted;
			}
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x0006FD2F File Offset: 0x0006DF2F
		public void Show()
		{
			this.fadeGroup.Show();
			this.timeWhenShowStarted = Time.unscaledTime;
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x0006FD47 File Offset: 0x0006DF47
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.fadeGroup.IsShown || this.fadeGroup.IsShowingInProgress)
			{
				this.punchReceiver.Punch();
				this.fadeGroup.Hide();
			}
		}

		// Token: 0x040015A0 RID: 5536
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040015A1 RID: 5537
		[SerializeField]
		private TextMeshProUGUI text_BuffName;

		// Token: 0x040015A2 RID: 5538
		[SerializeField]
		private TextMeshProUGUI text_BuffDescription;

		// Token: 0x040015A3 RID: 5539
		[SerializeField]
		private TextMeshProUGUI text_CountDown;

		// Token: 0x040015A4 RID: 5540
		[SerializeField]
		private PunchReceiver punchReceiver;

		// Token: 0x040015A5 RID: 5541
		[SerializeField]
		private float disappearAfterSeconds = 5f;

		// Token: 0x040015A6 RID: 5542
		private RectTransform rectTransform;

		// Token: 0x040015A7 RID: 5543
		private Buff target;

		// Token: 0x040015A8 RID: 5544
		private float timeWhenShowStarted;
	}
}
