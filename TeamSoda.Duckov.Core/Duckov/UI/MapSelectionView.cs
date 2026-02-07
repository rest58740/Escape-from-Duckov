using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.UI.Animations;
using Eflatun.SceneReference;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003D5 RID: 981
	public class MapSelectionView : View
	{
		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x060023AA RID: 9130 RVA: 0x0007D2CB File Offset: 0x0007B4CB
		public static MapSelectionView Instance
		{
			get
			{
				return View.GetViewInstance<MapSelectionView>();
			}
		}

		// Token: 0x060023AB RID: 9131 RVA: 0x0007D2D2 File Offset: 0x0007B4D2
		protected override void Awake()
		{
			base.Awake();
			this.btnConfirm.onClick.AddListener(delegate()
			{
				this.confirmButtonClicked = true;
			});
			this.btnCancel.onClick.AddListener(delegate()
			{
				this.cancelButtonClicked = true;
			});
		}

		// Token: 0x060023AC RID: 9132 RVA: 0x0007D312 File Offset: 0x0007B512
		protected override void OnOpen()
		{
			base.OnOpen();
			this.confirmIndicatorFadeGroup.SkipHide();
			this.mainFadeGroup.Show();
		}

		// Token: 0x060023AD RID: 9133 RVA: 0x0007D330 File Offset: 0x0007B530
		protected override void OnClose()
		{
			base.OnClose();
			this.mainFadeGroup.Hide();
		}

		// Token: 0x060023AE RID: 9134 RVA: 0x0007D344 File Offset: 0x0007B544
		internal void NotifyEntryClicked(MapSelectionEntry mapSelectionEntry, PointerEventData eventData)
		{
			if (!mapSelectionEntry.Cost.Enough)
			{
				return;
			}
			AudioManager.Post(this.sfx_EntryClicked);
			string sceneID = mapSelectionEntry.SceneID;
			LevelManager.loadLevelBeaconIndex = mapSelectionEntry.BeaconIndex;
			this.loading = true;
			this.LoadTask(sceneID, mapSelectionEntry.Cost).Forget();
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x0007D39C File Offset: 0x0007B59C
		private UniTask LoadTask(string sceneID, Cost cost)
		{
			MapSelectionView.<LoadTask>d__18 <LoadTask>d__;
			<LoadTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<LoadTask>d__.<>4__this = this;
			<LoadTask>d__.sceneID = sceneID;
			<LoadTask>d__.cost = cost;
			<LoadTask>d__.<>1__state = -1;
			<LoadTask>d__.<>t__builder.Start<MapSelectionView.<LoadTask>d__18>(ref <LoadTask>d__);
			return <LoadTask>d__.<>t__builder.Task;
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x0007D3F0 File Offset: 0x0007B5F0
		private UniTask<bool> WaitForConfirm()
		{
			MapSelectionView.<WaitForConfirm>d__21 <WaitForConfirm>d__;
			<WaitForConfirm>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<WaitForConfirm>d__.<>4__this = this;
			<WaitForConfirm>d__.<>1__state = -1;
			<WaitForConfirm>d__.<>t__builder.Start<MapSelectionView.<WaitForConfirm>d__21>(ref <WaitForConfirm>d__);
			return <WaitForConfirm>d__.<>t__builder.Task;
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x0007D434 File Offset: 0x0007B634
		private void SetupSceneInfo(SceneInfoEntry info)
		{
			if (info == null)
			{
				return;
			}
			string displayName = info.DisplayName;
			this.destinationDisplayNameText.text = displayName;
			this.destinationDisplayNameText.color = Color.white;
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x0007D468 File Offset: 0x0007B668
		internal override void TryQuit()
		{
			if (!this.loading)
			{
				base.Close();
			}
		}

		// Token: 0x04001837 RID: 6199
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x04001838 RID: 6200
		[SerializeField]
		private FadeGroup confirmIndicatorFadeGroup;

		// Token: 0x04001839 RID: 6201
		[SerializeField]
		private TextMeshProUGUI destinationDisplayNameText;

		// Token: 0x0400183A RID: 6202
		[SerializeField]
		private CostDisplay confirmCostDisplay;

		// Token: 0x0400183B RID: 6203
		private string sfx_EntryClicked = "UI/confirm";

		// Token: 0x0400183C RID: 6204
		private string sfx_ShowDestination = "UI/destination_show";

		// Token: 0x0400183D RID: 6205
		private string sfx_ConfirmDestination = "UI/destination_confirm";

		// Token: 0x0400183E RID: 6206
		[SerializeField]
		private ColorPunch confirmColorPunch;

		// Token: 0x0400183F RID: 6207
		[SerializeField]
		private Button btnConfirm;

		// Token: 0x04001840 RID: 6208
		[SerializeField]
		private Button btnCancel;

		// Token: 0x04001841 RID: 6209
		[SerializeField]
		private SceneReference overrideLoadingScreen;

		// Token: 0x04001842 RID: 6210
		private bool loading;

		// Token: 0x04001843 RID: 6211
		private bool confirmButtonClicked;

		// Token: 0x04001844 RID: 6212
		private bool cancelButtonClicked;
	}
}
