using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.Endowment.UI
{
	// Token: 0x0200030A RID: 778
	public class EndowmentSelectionPanel : View
	{
		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x0600198B RID: 6539 RVA: 0x0005D840 File Offset: 0x0005BA40
		private PrefabPool<EndowmentSelectionEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<EndowmentSelectionEntry>(this.entryTemplate, null, null, null, null, true, 10, 10000, delegate(EndowmentSelectionEntry e)
					{
						e.onClicked = (Action<EndowmentSelectionEntry, PointerEventData>)Delegate.Combine(e.onClicked, new Action<EndowmentSelectionEntry, PointerEventData>(this.OnEntryClicked));
					});
				}
				return this._pool;
			}
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x0005D884 File Offset: 0x0005BA84
		protected override void Awake()
		{
			base.Awake();
			this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
			this.cancelButton.onClick.AddListener(new UnityAction(this.OnCancelButtonClicked));
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x0005D8C4 File Offset: 0x0005BAC4
		protected override void OnCancel()
		{
			base.OnCancel();
			this.canceled = true;
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x0005D8D3 File Offset: 0x0005BAD3
		private void OnCancelButtonClicked()
		{
			this.canceled = true;
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x0005D8DC File Offset: 0x0005BADC
		private void OnConfirmButtonClicked()
		{
			this.confirmed = true;
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x0005D8E5 File Offset: 0x0005BAE5
		private void OnEntryClicked(EndowmentSelectionEntry entry, PointerEventData data)
		{
			if (entry.Locked)
			{
				return;
			}
			this.Select(entry);
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x0005D8F8 File Offset: 0x0005BAF8
		private void Select(EndowmentSelectionEntry entry)
		{
			this.Selection = entry;
			foreach (EndowmentSelectionEntry endowmentSelectionEntry in this.Pool.ActiveEntries)
			{
				endowmentSelectionEntry.SetSelection(endowmentSelectionEntry == entry);
			}
			this.RefreshDescription();
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06001992 RID: 6546 RVA: 0x0005D95C File Offset: 0x0005BB5C
		// (set) Token: 0x06001993 RID: 6547 RVA: 0x0005D964 File Offset: 0x0005BB64
		public EndowmentSelectionEntry Selection { get; private set; }

		// Token: 0x06001994 RID: 6548 RVA: 0x0005D970 File Offset: 0x0005BB70
		public void Setup()
		{
			if (EndowmentManager.Instance == null)
			{
				return;
			}
			this.Pool.ReleaseAll();
			foreach (EndowmentEntry endowmentEntry in EndowmentManager.Instance.Entries)
			{
				if (!(endowmentEntry == null))
				{
					this.Pool.Get(null).Setup(endowmentEntry);
				}
			}
			foreach (EndowmentSelectionEntry endowmentSelectionEntry in this.Pool.ActiveEntries)
			{
				if (endowmentSelectionEntry.Target.Index == EndowmentManager.SelectedIndex)
				{
					this.Select(endowmentSelectionEntry);
					break;
				}
			}
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x0005DA44 File Offset: 0x0005BC44
		private void RefreshDescription()
		{
			if (this.Selection == null)
			{
				this.descriptionText.text = "-";
			}
			this.descriptionText.text = this.Selection.DescriptionAndEffects;
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x0005DA7A File Offset: 0x0005BC7A
		protected override void OnOpen()
		{
			base.OnOpen();
			this.Execute().Forget();
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x0005DA8D File Offset: 0x0005BC8D
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x0005DAA0 File Offset: 0x0005BCA0
		public UniTask Execute()
		{
			EndowmentSelectionPanel.<Execute>d__22 <Execute>d__;
			<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Execute>d__.<>4__this = this;
			<Execute>d__.<>1__state = -1;
			<Execute>d__.<>t__builder.Start<EndowmentSelectionPanel.<Execute>d__22>(ref <Execute>d__);
			return <Execute>d__.<>t__builder.Task;
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x0005DAE4 File Offset: 0x0005BCE4
		private UniTask WaitForConfirm()
		{
			EndowmentSelectionPanel.<WaitForConfirm>d__25 <WaitForConfirm>d__;
			<WaitForConfirm>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WaitForConfirm>d__.<>4__this = this;
			<WaitForConfirm>d__.<>1__state = -1;
			<WaitForConfirm>d__.<>t__builder.Start<EndowmentSelectionPanel.<WaitForConfirm>d__25>(ref <WaitForConfirm>d__);
			return <WaitForConfirm>d__.<>t__builder.Task;
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x0005DB27 File Offset: 0x0005BD27
		internal void SkipHide()
		{
			if (this.fadeGroup != null)
			{
				this.fadeGroup.SkipHide();
			}
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x0005DB44 File Offset: 0x0005BD44
		public static void Show()
		{
			EndowmentSelectionPanel viewInstance = View.GetViewInstance<EndowmentSelectionPanel>();
			if (viewInstance == null)
			{
				return;
			}
			viewInstance.Open(null);
		}

		// Token: 0x0400129A RID: 4762
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400129B RID: 4763
		[SerializeField]
		private EndowmentSelectionEntry entryTemplate;

		// Token: 0x0400129C RID: 4764
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		// Token: 0x0400129D RID: 4765
		[SerializeField]
		private Button confirmButton;

		// Token: 0x0400129E RID: 4766
		[SerializeField]
		private Button cancelButton;

		// Token: 0x0400129F RID: 4767
		private PrefabPool<EndowmentSelectionEntry> _pool;

		// Token: 0x040012A1 RID: 4769
		private bool confirmed;

		// Token: 0x040012A2 RID: 4770
		private bool canceled;
	}
}
