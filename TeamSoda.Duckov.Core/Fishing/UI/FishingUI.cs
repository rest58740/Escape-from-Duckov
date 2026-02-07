using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;

namespace Fishing.UI
{
	// Token: 0x02000223 RID: 547
	public class FishingUI : View
	{
		// Token: 0x06001086 RID: 4230 RVA: 0x00041248 File Offset: 0x0003F448
		protected override void Awake()
		{
			base.Awake();
			Action_Fishing.OnPlayerStartSelectBait += this.OnStartSelectBait;
			Action_Fishing.OnPlayerStopCatching += this.OnStopCatching;
			Action_Fishing.OnPlayerStopFishing += this.OnStopFishing;
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x00041283 File Offset: 0x0003F483
		protected override void OnDestroy()
		{
			Action_Fishing.OnPlayerStopFishing -= this.OnStopFishing;
			Action_Fishing.OnPlayerStartSelectBait -= this.OnStartSelectBait;
			Action_Fishing.OnPlayerStopCatching -= this.OnStopCatching;
			base.OnDestroy();
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x000412BE File Offset: 0x0003F4BE
		internal override void TryQuit()
		{
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x000412C0 File Offset: 0x0003F4C0
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			Debug.Log("Open Fishing Panel");
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x000412DD File Offset: 0x0003F4DD
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x000412F0 File Offset: 0x0003F4F0
		private void OnStopFishing(Action_Fishing fishing)
		{
			this.baitSelectPanel.NotifyStop();
			this.confirmPanel.NotifyStop();
			base.Close();
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x0004130E File Offset: 0x0003F50E
		private void OnStartSelectBait(Action_Fishing fishing, ICollection<Item> availableBaits, Func<Item, bool> baitSelectionResultCallback)
		{
			this.SelectBaitTask(availableBaits, baitSelectionResultCallback).Forget();
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00041320 File Offset: 0x0003F520
		private UniTask SelectBaitTask(ICollection<Item> availableBaits, Func<Item, bool> baitSelectionResultCallback)
		{
			FishingUI.<SelectBaitTask>d__10 <SelectBaitTask>d__;
			<SelectBaitTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SelectBaitTask>d__.<>4__this = this;
			<SelectBaitTask>d__.availableBaits = availableBaits;
			<SelectBaitTask>d__.baitSelectionResultCallback = baitSelectionResultCallback;
			<SelectBaitTask>d__.<>1__state = -1;
			<SelectBaitTask>d__.<>t__builder.Start<FishingUI.<SelectBaitTask>d__10>(ref <SelectBaitTask>d__);
			return <SelectBaitTask>d__.<>t__builder.Task;
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x00041373 File Offset: 0x0003F573
		private void OnStopCatching(Action_Fishing fishing, Item catchedItem, Action<bool> confirmCallback)
		{
			this.ConfirmTask(catchedItem, confirmCallback).Forget();
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x00041384 File Offset: 0x0003F584
		private UniTask ConfirmTask(Item catchedItem, Action<bool> confirmCallback)
		{
			FishingUI.<ConfirmTask>d__12 <ConfirmTask>d__;
			<ConfirmTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ConfirmTask>d__.<>4__this = this;
			<ConfirmTask>d__.catchedItem = catchedItem;
			<ConfirmTask>d__.confirmCallback = confirmCallback;
			<ConfirmTask>d__.<>1__state = -1;
			<ConfirmTask>d__.<>t__builder.Start<FishingUI.<ConfirmTask>d__12>(ref <ConfirmTask>d__);
			return <ConfirmTask>d__.<>t__builder.Task;
		}

		// Token: 0x04000D53 RID: 3411
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04000D54 RID: 3412
		[SerializeField]
		private BaitSelectPanel baitSelectPanel;

		// Token: 0x04000D55 RID: 3413
		[SerializeField]
		private ConfirmPanel confirmPanel;
	}
}
