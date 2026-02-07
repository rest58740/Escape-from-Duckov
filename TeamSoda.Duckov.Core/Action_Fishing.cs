using System;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020000A4 RID: 164
public class Action_Fishing : CharacterActionBase
{
	// Token: 0x14000024 RID: 36
	// (add) Token: 0x06000581 RID: 1409 RVA: 0x00018BC4 File Offset: 0x00016DC4
	// (remove) Token: 0x06000582 RID: 1410 RVA: 0x00018BF8 File Offset: 0x00016DF8
	public static event Action<Action_Fishing, ICollection<Item>, Func<Item, bool>> OnPlayerStartSelectBait;

	// Token: 0x14000025 RID: 37
	// (add) Token: 0x06000583 RID: 1411 RVA: 0x00018C2C File Offset: 0x00016E2C
	// (remove) Token: 0x06000584 RID: 1412 RVA: 0x00018C60 File Offset: 0x00016E60
	public static event Action<Action_Fishing> OnPlayerStartFishing;

	// Token: 0x14000026 RID: 38
	// (add) Token: 0x06000585 RID: 1413 RVA: 0x00018C94 File Offset: 0x00016E94
	// (remove) Token: 0x06000586 RID: 1414 RVA: 0x00018CC8 File Offset: 0x00016EC8
	public static event Action<Action_Fishing, float, Func<float>> OnPlayerStartCatching;

	// Token: 0x14000027 RID: 39
	// (add) Token: 0x06000587 RID: 1415 RVA: 0x00018CFC File Offset: 0x00016EFC
	// (remove) Token: 0x06000588 RID: 1416 RVA: 0x00018D30 File Offset: 0x00016F30
	public static event Action<Action_Fishing, Item, Action<bool>> OnPlayerStopCatching;

	// Token: 0x14000028 RID: 40
	// (add) Token: 0x06000589 RID: 1417 RVA: 0x00018D64 File Offset: 0x00016F64
	// (remove) Token: 0x0600058A RID: 1418 RVA: 0x00018D98 File Offset: 0x00016F98
	public static event Action<Action_Fishing> OnPlayerStopFishing;

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x0600058B RID: 1419 RVA: 0x00018DCB File Offset: 0x00016FCB
	public Action_Fishing.FishingStates FishingState
	{
		get
		{
			return this.fishingState;
		}
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x00018DD3 File Offset: 0x00016FD3
	private void Awake()
	{
		this.fishingCamera.gameObject.SetActive(false);
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x00018DE6 File Offset: 0x00016FE6
	public override bool CanEditInventory()
	{
		return false;
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x00018DE9 File Offset: 0x00016FE9
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.Fishing;
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x00018DEC File Offset: 0x00016FEC
	protected override bool OnStart()
	{
		if (!this.characterController)
		{
			return false;
		}
		this.fishingCamera.gameObject.SetActive(true);
		this.fishingRod = this.characterController.CurrentHoldItemAgent.GetComponent<FishingRod>();
		bool result = this.fishingRod != null;
		this.currentTask = this.Fishing();
		InputManager.OnInteractButtonDown = (Action)Delegate.Remove(InputManager.OnInteractButtonDown, new Action(this.OnCatchButton));
		InputManager.OnInteractButtonDown = (Action)Delegate.Combine(InputManager.OnInteractButtonDown, new Action(this.OnCatchButton));
		UIInputManager.OnCancel -= this.UIOnCancle;
		UIInputManager.OnCancel += this.UIOnCancle;
		return result;
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x00018EA9 File Offset: 0x000170A9
	private void OnCatchButton()
	{
		if (this.fishingState != Action_Fishing.FishingStates.catching)
		{
			return;
		}
		this.catchInput = true;
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x00018EBC File Offset: 0x000170BC
	private void UIOnCancle(UIInputEventData data)
	{
		data.Use();
		this.Quit();
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x00018ECC File Offset: 0x000170CC
	protected override void OnStop()
	{
		base.OnStop();
		this.fishingState = Action_Fishing.FishingStates.notStarted;
		Action<Action_Fishing> onPlayerStopFishing = Action_Fishing.OnPlayerStopFishing;
		if (onPlayerStopFishing != null)
		{
			onPlayerStopFishing(this);
		}
		InputManager.OnInteractButtonDown = (Action)Delegate.Remove(InputManager.OnInteractButtonDown, new Action(this.OnCatchButton));
		UIInputManager.OnCancel -= this.UIOnCancle;
		this.fishingCamera.gameObject.SetActive(false);
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x00018F39 File Offset: 0x00017139
	public override bool CanControlAim()
	{
		return false;
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x00018F3C File Offset: 0x0001713C
	public override bool CanMove()
	{
		return false;
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x00018F3F File Offset: 0x0001713F
	public override bool CanRun()
	{
		return false;
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x00018F42 File Offset: 0x00017142
	public override bool CanUseHand()
	{
		return false;
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x00018F45 File Offset: 0x00017145
	public override bool IsReady()
	{
		return true;
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x00018F48 File Offset: 0x00017148
	private int NewToken()
	{
		this.fishingTaskToken++;
		this.fishingTaskToken %= 1000;
		return this.fishingTaskToken;
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x00018F70 File Offset: 0x00017170
	private UniTask Fishing()
	{
		Action_Fishing.<Fishing>d__48 <Fishing>d__;
		<Fishing>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Fishing>d__.<>4__this = this;
		<Fishing>d__.<>1__state = -1;
		<Fishing>d__.<>t__builder.Start<Action_Fishing.<Fishing>d__48>(ref <Fishing>d__);
		return <Fishing>d__.<>t__builder.Task;
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x00018FB4 File Offset: 0x000171B4
	private UniTask SingleFishingLoop(Func<bool> IsTaskValid)
	{
		Action_Fishing.<SingleFishingLoop>d__49 <SingleFishingLoop>d__;
		<SingleFishingLoop>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<SingleFishingLoop>d__.<>4__this = this;
		<SingleFishingLoop>d__.IsTaskValid = IsTaskValid;
		<SingleFishingLoop>d__.<>1__state = -1;
		<SingleFishingLoop>d__.<>t__builder.Start<Action_Fishing.<SingleFishingLoop>d__49>(ref <SingleFishingLoop>d__);
		return <SingleFishingLoop>d__.<>t__builder.Task;
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x00018FFF File Offset: 0x000171FF
	private void ResultConfirm(bool _continueFishing)
	{
		this.resultConfirmed = true;
		this.continueFishing = _continueFishing;
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x00019010 File Offset: 0x00017210
	private UniTask<bool> Catching(Func<bool> IsTaskValid)
	{
		Action_Fishing.<Catching>d__51 <Catching>d__;
		<Catching>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<Catching>d__.<>4__this = this;
		<Catching>d__.IsTaskValid = IsTaskValid;
		<Catching>d__.<>1__state = -1;
		<Catching>d__.<>t__builder.Start<Action_Fishing.<Catching>d__51>(ref <Catching>d__);
		return <Catching>d__.<>t__builder.Task;
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x0001905C File Offset: 0x0001725C
	private UniTask<bool> WaitForSelectBait()
	{
		Action_Fishing.<WaitForSelectBait>d__52 <WaitForSelectBait>d__;
		<WaitForSelectBait>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<WaitForSelectBait>d__.<>4__this = this;
		<WaitForSelectBait>d__.<>1__state = -1;
		<WaitForSelectBait>d__.<>t__builder.Start<Action_Fishing.<WaitForSelectBait>d__52>(ref <WaitForSelectBait>d__);
		return <WaitForSelectBait>d__.<>t__builder.Task;
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x000190A0 File Offset: 0x000172A0
	public List<Item> GetAllBaits()
	{
		List<Item> list = new List<Item>();
		if (!this.characterController)
		{
			return list;
		}
		foreach (Item item in this.characterController.CharacterItem.Inventory)
		{
			if (item.Tags.Contains(GameplayDataSettings.Tags.Bait))
			{
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x00019124 File Offset: 0x00017324
	public void CatchButton()
	{
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x00019126 File Offset: 0x00017326
	public void Quit()
	{
		Debug.Log("Quit");
		this.quit = true;
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x0001913C File Offset: 0x0001733C
	private bool SelectBaitAndStartFishing(Item _bait)
	{
		if (_bait == null)
		{
			Debug.Log("鱼饵选了个null, 退出");
			this.Quit();
			return false;
		}
		if (!_bait.Tags.Contains(GameplayDataSettings.Tags.Bait))
		{
			this.Quit();
			return false;
		}
		this.bait = _bait;
		return true;
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x0001918C File Offset: 0x0001738C
	private void OnDestroy()
	{
		if (base.Running)
		{
			Action<Action_Fishing> onPlayerStopFishing = Action_Fishing.OnPlayerStopFishing;
			if (onPlayerStopFishing != null)
			{
				onPlayerStopFishing(this);
			}
		}
		InputManager.OnInteractButtonDown = (Action)Delegate.Remove(InputManager.OnInteractButtonDown, new Action(this.OnCatchButton));
		UIInputManager.OnCancel -= this.UIOnCancle;
	}

	// Token: 0x04000501 RID: 1281
	[SerializeField]
	private CinemachineVirtualCamera fishingCamera;

	// Token: 0x04000502 RID: 1282
	private FishingRod fishingRod;

	// Token: 0x04000503 RID: 1283
	[SerializeField]
	private FishingPoint fishingPoint;

	// Token: 0x04000504 RID: 1284
	[SerializeField]
	private float introTime = 0.2f;

	// Token: 0x04000505 RID: 1285
	private float fishingWaitTime = 2f;

	// Token: 0x04000506 RID: 1286
	private float catchTime = 0.5f;

	// Token: 0x04000507 RID: 1287
	private Item bait;

	// Token: 0x04000508 RID: 1288
	private Transform socket;

	// Token: 0x04000509 RID: 1289
	[SerializeField]
	[ItemTypeID]
	private int testCatchItem;

	// Token: 0x0400050A RID: 1290
	private Item catchedItem;

	// Token: 0x0400050B RID: 1291
	private bool quit;

	// Token: 0x0400050C RID: 1292
	private UniTask currentTask;

	// Token: 0x0400050D RID: 1293
	private bool catchInput;

	// Token: 0x0400050E RID: 1294
	private bool resultConfirmed;

	// Token: 0x0400050F RID: 1295
	private bool continueFishing;

	// Token: 0x04000515 RID: 1301
	private Action_Fishing.FishingStates fishingState;

	// Token: 0x04000516 RID: 1302
	private int fishingTaskToken;

	// Token: 0x0200046B RID: 1131
	public enum FishingStates
	{
		// Token: 0x04001B97 RID: 7063
		notStarted,
		// Token: 0x04001B98 RID: 7064
		intro,
		// Token: 0x04001B99 RID: 7065
		selectingBait,
		// Token: 0x04001B9A RID: 7066
		fishing,
		// Token: 0x04001B9B RID: 7067
		catching,
		// Token: 0x04001B9C RID: 7068
		over
	}
}
