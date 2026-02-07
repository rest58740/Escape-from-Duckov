using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.Quests.Conditions;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000A5 RID: 165
public class Action_FishingV2 : CharacterActionBase
{
	// Token: 0x060005A6 RID: 1446 RVA: 0x00019236 File Offset: 0x00017436
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.Fishing;
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x00019239 File Offset: 0x00017439
	public override bool CanControlAim()
	{
		return false;
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x0001923C File Offset: 0x0001743C
	public override bool CanMove()
	{
		return false;
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x0001923F File Offset: 0x0001743F
	public override bool CanRun()
	{
		return false;
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x00019242 File Offset: 0x00017442
	public override bool CanUseHand()
	{
		return false;
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x00019248 File Offset: 0x00017448
	private void Awake()
	{
		this.interactable.OnInteractTimeoutEvent.AddListener(new UnityAction<CharacterMainControl, InteractableBase>(this.OnInteractTimeOut));
		this.interactable.finishWhenTimeOut = false;
		this.fishingHudCanvas.gameObject.SetActive(false);
		this.baitVisual.gameObject.SetActive(false);
		this.baitTrail.gameObject.SetActive(false);
		this.dropParticle.SetActive(false);
		this.bucketParticle.SetActive(false);
		this.gotFx.SetActive(false);
		this.SyncInteractable(CharacterMainControl.Main);
		CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent = (Action<CharacterMainControl, DuckovItemAgent>)Delegate.Combine(CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent, new Action<CharacterMainControl, DuckovItemAgent>(this.OnMainCharacterChangeItemAgent));
		this.TransToNon();
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x00019305 File Offset: 0x00017505
	private void OnMainCharacterChangeItemAgent(CharacterMainControl character, DuckovItemAgent agent)
	{
		this.SyncInteractable(character);
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x00019310 File Offset: 0x00017510
	private void SyncInteractable(CharacterMainControl character)
	{
		if (!character)
		{
			this.interactable.gameObject.SetActive(false);
			return;
		}
		DuckovItemAgent currentHoldItemAgent = character.CurrentHoldItemAgent;
		if (!currentHoldItemAgent)
		{
			this.interactable.gameObject.SetActive(false);
			return;
		}
		FishingRod component = currentHoldItemAgent.GetComponent<FishingRod>();
		this.interactable.gameObject.SetActive(component != null);
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x00019378 File Offset: 0x00017578
	private void SetWaveEmissionRate(float rate)
	{
		this.waveParticle.emission.rateOverTime = rate;
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x000193A0 File Offset: 0x000175A0
	private void OnDestroy()
	{
		if (this.interactable)
		{
			this.interactable.OnInteractTimeoutEvent.RemoveListener(new UnityAction<CharacterMainControl, InteractableBase>(this.OnInteractTimeOut));
		}
		CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent = (Action<CharacterMainControl, DuckovItemAgent>)Delegate.Remove(CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent, new Action<CharacterMainControl, DuckovItemAgent>(this.OnMainCharacterChangeItemAgent));
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x000193F6 File Offset: 0x000175F6
	public void TryCatch()
	{
		Debug.Log("TryCatch");
		if (this.fishingState == Action_FishingV2.FishingStates.waiting || this.fishingState == Action_FishingV2.FishingStates.ring)
		{
			this.catchInput = true;
		}
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x0001941B File Offset: 0x0001761B
	private void OnInteractTimeOut(CharacterMainControl target, InteractableBase interactable)
	{
		interactable.StopInteract();
		target.StartAction(this);
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x0001942B File Offset: 0x0001762B
	public override bool IsReady()
	{
		return !base.Running;
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x00019438 File Offset: 0x00017638
	protected override bool OnStart()
	{
		if (this.characterController == null)
		{
			base.StopAction();
		}
		this.waitTime = Random.Range(this.waitTimeRange.x, this.waitTimeRange.y);
		this.ringAnimator.SetInteger("State", 0);
		this.rodAgent = this.characterController.CurrentHoldItemAgent;
		if (!this.rodAgent)
		{
			this.characterController.PopText(LocalizationManager.ToPlainText(this.noRodText), -1f);
			return false;
		}
		this.rod = this.rodAgent.GetComponent<FishingRod>();
		if (!this.rod)
		{
			this.characterController.PopText(LocalizationManager.ToPlainText(this.noRodText), -1f);
			return false;
		}
		this.baitItem = this.rod.Bait;
		if (!this.baitItem)
		{
			this.characterController.PopText(LocalizationManager.ToPlainText(this.noBaitText), -1f);
			return false;
		}
		this.characterController.characterModel.ForcePlayAttackAnimation();
		Vector3 direction = this.targetPoint.position - this.characterController.transform.position;
		direction.y = 0f;
		direction.Normalize();
		this.characterController.movementControl.ForceTurnTo(direction);
		this.fishingHudCanvas.worldCamera = Camera.main;
		this.fishingHudCanvas.gameObject.SetActive(true);
		this.hookStartPoint = this.rod.lineStart.position;
		this.TransToThrowing();
		return true;
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x000195D0 File Offset: 0x000177D0
	protected override void OnStop()
	{
		this.TransToNon();
		this.fishingHudCanvas.gameObject.SetActive(false);
		this.ringAnimator.gameObject.SetActive(false);
		this.lineRenderer.gameObject.SetActive(false);
		this.baitVisual.gameObject.SetActive(false);
		this.gotFx.SetActive(false);
		this.SetWaveEmissionRate(0f);
		this.ringAnimator.SetInteger("State", 0);
		if (this.currentFish)
		{
			ItemTreeExtensions.DestroyTree(this.currentFish);
			this.currentFish = null;
		}
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x0001966E File Offset: 0x0001786E
	private void SpawnDropParticle()
	{
		Object.Instantiate<GameObject>(this.dropParticle, this.targetPoint).SetActive(true);
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x00019687 File Offset: 0x00017887
	private void SpawnBucketParticle()
	{
		Object.Instantiate<GameObject>(this.bucketParticle, this.bucketPoint).SetActive(true);
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x000196A0 File Offset: 0x000178A0
	private void OnDisable()
	{
		if (base.Running)
		{
			base.StopAction();
		}
		this.fishingHudCanvas.gameObject.SetActive(false);
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x000196C2 File Offset: 0x000178C2
	public override bool IsStopable()
	{
		return this.needStopAction;
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x000196CC File Offset: 0x000178CC
	private Vector3 GetHookOutPos(float lerpValue)
	{
		lerpValue = Mathf.Clamp01(lerpValue);
		Vector3 vector = this.hookStartPoint;
		Vector3 position = this.targetPoint.position;
		Vector3 result = Vector3.Lerp(vector, position, lerpValue);
		float y = Mathf.LerpUnclamped(position.y, vector.y, this.outYCurve.Evaluate(lerpValue));
		result.y = y;
		return result;
	}

	// Token: 0x060005BA RID: 1466 RVA: 0x00019724 File Offset: 0x00017924
	private Vector3 GetHookBackPos(float lerpValue)
	{
		lerpValue = Mathf.Clamp01(lerpValue);
		Vector3 position = this.rod.lineStart.position;
		Vector3 position2 = this.targetPoint.position;
		Vector3 result = Vector3.Lerp(position2, position, lerpValue);
		float y = Mathf.LerpUnclamped(position2.y, position.y, this.backYCurve.Evaluate(lerpValue));
		result.y = y;
		return result;
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x00019784 File Offset: 0x00017984
	protected override void OnUpdateAction(float deltaTime)
	{
		if (!this.characterController || !this.rod)
		{
			this.needStopAction = true;
			base.StopAction();
			return;
		}
		this.lineRenderer.SetPosition(0, this.rod.lineStart.position);
		Vector3 vector = this.rod.lineStart.position;
		this.needStopAction = false;
		if (this.rod == null)
		{
			this.needStopAction = true;
			base.StopAction();
			return;
		}
		switch (this.fishingState)
		{
		case Action_FishingV2.FishingStates.throwing:
			if (!this.baitItem || this.catchInput)
			{
				this.TransToCancleBack();
			}
			else if (this.stateTimer < this.throwStartTime)
			{
				this.hookStartPoint = this.rod.lineStart.position;
				vector = this.hookStartPoint;
				this.baitTrail.Clear();
			}
			else if (this.stateTimer < this.outTime)
			{
				vector = this.GetHookOutPos((this.stateTimer - this.throwStartTime) / (this.outTime - this.throwStartTime));
				if (!this.baitVisual.gameObject.activeInHierarchy)
				{
					this.baitVisual.gameObject.SetActive(true);
					this.baitTrail.gameObject.SetActive(true);
				}
				this.baitVisual.transform.position = vector;
				this.baitTrail.transform.position = vector;
			}
			else
			{
				this.TransToWaiting();
			}
			break;
		case Action_FishingV2.FishingStates.waiting:
			if (this.catchInput)
			{
				this.TransToCancleBack();
			}
			else
			{
				vector = this.targetPoint.position;
				this.baitVisual.transform.position = vector;
				this.baitTrail.transform.position = vector;
				if (this.stateTimer >= this.waitTime)
				{
					if (this.currentFish != null)
					{
						this.TransToRing();
					}
					else
					{
						this.characterController.PopText("Error:Spawn fish failed", -1f);
						this.TransToCancleBack();
					}
				}
				if (this.waitTime - this.stateTimer < 0.25f && !this.hookFxSpawned)
				{
					this.hookFxSpawned = true;
					this.SpawnHookFx();
				}
			}
			break;
		case Action_FishingV2.FishingStates.ring:
		{
			vector = this.targetPoint.position;
			float num = Mathf.Lerp(this.scaleRange.y, this.scaleRange.x, 1f - this.stateTimer / this.scaleTime);
			this.scaleRing.localScale = Vector3.one * num;
			if (this.catchInput)
			{
				if (num < this.successRange.x || num > this.successRange.y)
				{
					this.TransToFailBack();
					break;
				}
				this.TransToSuccessback();
			}
			if (this.stateTimer > this.scaleTime)
			{
				this.TransToFailBack();
			}
			break;
		}
		case Action_FishingV2.FishingStates.cancleBack:
			vector = this.GetHookBackPos(this.stateTimer / this.backTime);
			this.baitVisual.transform.position = vector;
			this.baitTrail.transform.position = vector;
			if (this.stateTimer > this.backTime)
			{
				this.needStopAction = true;
			}
			break;
		case Action_FishingV2.FishingStates.successBack:
		{
			float num2 = 0.2f;
			if (this.stateTimer >= num2)
			{
				vector = this.GetHookBackPos((this.stateTimer - num2) / this.backTime);
				this.baitVisual.transform.position = vector;
				this.baitTrail.transform.position = vector;
				if (this.stateTimer - num2 > this.backTime)
				{
					this.needStopAction = true;
				}
			}
			break;
		}
		case Action_FishingV2.FishingStates.failBack:
			vector = this.GetHookBackPos(this.stateTimer / this.backTime);
			this.baitVisual.transform.position = vector;
			this.baitTrail.transform.position = vector;
			if (this.stateTimer > this.backTime)
			{
				NotificationText.Push(LocalizationManager.ToPlainText(this.failText));
				this.needStopAction = true;
			}
			break;
		}
		this.lineRenderer.SetPosition(1, vector);
		this.catchInput = false;
		this.stateTimer += deltaTime;
		if (this.needStopAction)
		{
			this.baitVisual.gameObject.SetActive(false);
			this.baitTrail.gameObject.SetActive(false);
			this.baitTrail.Clear();
			base.StopAction();
		}
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x00019BEF File Offset: 0x00017DEF
	private void TransToNon()
	{
		this.fishingState = Action_FishingV2.FishingStates.non;
		this.SetWaveEmissionRate(0f);
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x00019C04 File Offset: 0x00017E04
	private void TransToThrowing()
	{
		AudioManager.Post(this.throwSoundKey, base.gameObject);
		this.stateTimer = 0f;
		this.lineRenderer.gameObject.SetActive(true);
		this.lineRenderer.positionCount = 2;
		this.lineRenderer.SetPosition(0, this.rod.lineStart.position);
		this.lineRenderer.SetPosition(1, this.rod.lineStart.position);
		this.ringAnimator.gameObject.SetActive(true);
		this.ringAnimator.SetInteger("State", 0);
		this.fishingState = Action_FishingV2.FishingStates.throwing;
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x00019CAC File Offset: 0x00017EAC
	private void TransToWaiting()
	{
		if (this.baitItem == null)
		{
			this.needStopAction = true;
			base.StopAction();
		}
		AudioManager.Post(this.startFishingSoundKey, this.targetPoint.gameObject);
		this.hookFxSpawned = false;
		this.SpawnDropParticle();
		this.SetWaveEmissionRate(1.5f);
		this.stateTimer = 0f;
		this.ringAnimator.SetInteger("State", 0);
		this.luck = this.characterController.CharacterItem.GetStatValue(this.fishingQualityFactorHash);
		this.SpawnFish(this.luck).Forget();
		this.fishingState = Action_FishingV2.FishingStates.waiting;
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x00019D58 File Offset: 0x00017F58
	private void SpawnHookFx()
	{
		if (this.hookFx == null)
		{
			return;
		}
		Object.Instantiate<GameObject>(this.hookFx, this.targetPoint.position + Vector3.up * 3f, Quaternion.identity);
		AudioManager.Post(this.baitSoundKey, this.targetPoint.gameObject);
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x00019DBC File Offset: 0x00017FBC
	private void TransToRing()
	{
		this.scaleTime = this.characterController.CharacterItem.GetStatValue(this.fishingTimeHash) * this.scaleTimeFactor;
		this.scaleTime = Mathf.Max(0.01f, this.scaleTime);
		float num = this.currentFish.GetStatValue(this.fishingDifficultyHash);
		if (num < 0.02f)
		{
			num = 1f;
		}
		this.scaleTime /= num;
		if (this.scaleTime > 7f)
		{
			this.scaleTime = 7f;
		}
		this.stateTimer = 0f;
		this.catchInput = false;
		this.fishingState = Action_FishingV2.FishingStates.ring;
		this.ringAnimator.SetInteger("State", 1);
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x00019E74 File Offset: 0x00018074
	private void TransToCancleBack()
	{
		this.stateTimer = 0f;
		this.ringAnimator.SetInteger("State", 0);
		this.fishingState = Action_FishingV2.FishingStates.cancleBack;
		this.SetWaveEmissionRate(0f);
		this.SpawnDropParticle();
		this.fishingHudCanvas.gameObject.SetActive(false);
		AudioManager.Post(this.pulloutSoundKey, this.targetPoint.gameObject);
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x00019EE0 File Offset: 0x000180E0
	private void TransToSuccessback()
	{
		this.stateTimer = 0f;
		this.ringAnimator.SetInteger("State", 2);
		AudioManager.Post(this.successSoundKey, this.targetPoint.gameObject);
		this.fishingState = Action_FishingV2.FishingStates.successBack;
		this.SetWaveEmissionRate(0f);
		this.SpawnDropParticle();
		this.gotFx.SetActive(true);
		this.fishingHudCanvas.gameObject.SetActive(false);
		this.CatchFish().Forget();
		RequireHasFished.SetHasFished();
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x00019F68 File Offset: 0x00018168
	private void TransToFailBack()
	{
		this.stateTimer = 0f;
		this.ringAnimator.SetInteger("State", 3);
		AudioManager.Post(this.failSoundKey, this.targetPoint.gameObject);
		this.fishingState = Action_FishingV2.FishingStates.failBack;
		this.SetWaveEmissionRate(0f);
		this.SpawnDropParticle();
		this.fishingHudCanvas.gameObject.SetActive(false);
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x00019FD4 File Offset: 0x000181D4
	private UniTaskVoid SpawnFish(float luck)
	{
		Action_FishingV2.<SpawnFish>d__85 <SpawnFish>d__;
		<SpawnFish>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<SpawnFish>d__.<>4__this = this;
		<SpawnFish>d__.luck = luck;
		<SpawnFish>d__.<>1__state = -1;
		<SpawnFish>d__.<>t__builder.Start<Action_FishingV2.<SpawnFish>d__85>(ref <SpawnFish>d__);
		return <SpawnFish>d__.<>t__builder.Task;
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x0001A020 File Offset: 0x00018220
	private UniTaskVoid CatchFish()
	{
		Action_FishingV2.<CatchFish>d__86 <CatchFish>d__;
		<CatchFish>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<CatchFish>d__.<>4__this = this;
		<CatchFish>d__.<>1__state = -1;
		<CatchFish>d__.<>t__builder.Start<Action_FishingV2.<CatchFish>d__86>(ref <CatchFish>d__);
		return <CatchFish>d__.<>t__builder.Task;
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x0001A063 File Offset: 0x00018263
	public override bool CanEditInventory()
	{
		return false;
	}

	// Token: 0x04000517 RID: 1303
	public InteractableBase interactable;

	// Token: 0x04000518 RID: 1304
	public Transform baitVisual;

	// Token: 0x04000519 RID: 1305
	public TrailRenderer baitTrail;

	// Token: 0x0400051A RID: 1306
	public Canvas fishingHudCanvas;

	// Token: 0x0400051B RID: 1307
	public Transform targetPoint;

	// Token: 0x0400051C RID: 1308
	public Transform bucketPoint;

	// Token: 0x0400051D RID: 1309
	[LocalizationKey("Default")]
	public string noRodText = "Pop_NoRod";

	// Token: 0x0400051E RID: 1310
	[LocalizationKey("Default")]
	public string noBaitText = "Pop_NoBait";

	// Token: 0x0400051F RID: 1311
	[LocalizationKey("Default")]
	public string gotFishText = "Notify_GotFish";

	// Token: 0x04000520 RID: 1312
	[LocalizationKey("Default")]
	public string failText = "Notify_FishRunAway";

	// Token: 0x04000521 RID: 1313
	private FishingRod rod;

	// Token: 0x04000522 RID: 1314
	private ItemAgent rodAgent;

	// Token: 0x04000523 RID: 1315
	private Item baitItem;

	// Token: 0x04000524 RID: 1316
	public Animator ringAnimator;

	// Token: 0x04000525 RID: 1317
	public Vector2 waitTimeRange = new Vector2(3f, 9f);

	// Token: 0x04000526 RID: 1318
	private float waitTime;

	// Token: 0x04000527 RID: 1319
	public Vector2 scaleRange = new Vector2(0.5f, 3f);

	// Token: 0x04000528 RID: 1320
	public Vector2 successRange = new Vector2(0.75f, 1.1f);

	// Token: 0x04000529 RID: 1321
	private float ringScaling = 2.5f;

	// Token: 0x0400052A RID: 1322
	private float stateTimer;

	// Token: 0x0400052B RID: 1323
	private bool catchInput;

	// Token: 0x0400052C RID: 1324
	public Transform scaleRing;

	// Token: 0x0400052D RID: 1325
	public LineRenderer lineRenderer;

	// Token: 0x0400052E RID: 1326
	public float throwStartTime = 0.1f;

	// Token: 0x0400052F RID: 1327
	public float outTime;

	// Token: 0x04000530 RID: 1328
	public AnimationCurve outYCurve;

	// Token: 0x04000531 RID: 1329
	public ParticleSystem waveParticle;

	// Token: 0x04000532 RID: 1330
	public GameObject dropParticle;

	// Token: 0x04000533 RID: 1331
	public GameObject bucketParticle;

	// Token: 0x04000534 RID: 1332
	public InteractableLootbox lootbox;

	// Token: 0x04000535 RID: 1333
	private bool hookFxSpawned;

	// Token: 0x04000536 RID: 1334
	public GameObject hookFx;

	// Token: 0x04000537 RID: 1335
	public float backTime;

	// Token: 0x04000538 RID: 1336
	public AnimationCurve backYCurve;

	// Token: 0x04000539 RID: 1337
	private Vector3 hookStartPoint;

	// Token: 0x0400053A RID: 1338
	public GameObject gotFx;

	// Token: 0x0400053B RID: 1339
	public FishSpawner lootSpawner;

	// Token: 0x0400053C RID: 1340
	private Item currentFish;

	// Token: 0x0400053D RID: 1341
	private float luck = 1f;

	// Token: 0x0400053E RID: 1342
	private float scaleTime;

	// Token: 0x0400053F RID: 1343
	private float scaleTimeFactor = 1.25f;

	// Token: 0x04000540 RID: 1344
	private int fishingTimeHash = "FishingTime".GetHashCode();

	// Token: 0x04000541 RID: 1345
	private int fishingDifficultyHash = "FishingDifficulty".GetHashCode();

	// Token: 0x04000542 RID: 1346
	private int fishingQualityFactorHash = "FishingQualityFactor".GetHashCode();

	// Token: 0x04000543 RID: 1347
	private Slot characterMeleeWeaponSlot;

	// Token: 0x04000544 RID: 1348
	private string currentStateInfo;

	// Token: 0x04000545 RID: 1349
	private string throwSoundKey = "SFX/Actions/Fishing_Throw";

	// Token: 0x04000546 RID: 1350
	private string startFishingSoundKey = "SFX/Actions/Fishing_Start";

	// Token: 0x04000547 RID: 1351
	private string pulloutSoundKey = "SFX/Actions/Fishing_PullOut";

	// Token: 0x04000548 RID: 1352
	private string baitSoundKey = "SFX/Actions/Fishing_Bait";

	// Token: 0x04000549 RID: 1353
	private string successSoundKey = "SFX/Actions/Fishing_Success";

	// Token: 0x0400054A RID: 1354
	private string failSoundKey = "SFX/Actions/Fishing_Failed";

	// Token: 0x0400054B RID: 1355
	public Action_FishingV2.FishingStates fishingState = Action_FishingV2.FishingStates.waiting;

	// Token: 0x0400054C RID: 1356
	private bool needStopAction;

	// Token: 0x02000472 RID: 1138
	public enum FishingStates
	{
		// Token: 0x04001BB9 RID: 7097
		non,
		// Token: 0x04001BBA RID: 7098
		throwing,
		// Token: 0x04001BBB RID: 7099
		waiting,
		// Token: 0x04001BBC RID: 7100
		ring,
		// Token: 0x04001BBD RID: 7101
		cancleBack,
		// Token: 0x04001BBE RID: 7102
		successBack,
		// Token: 0x04001BBF RID: 7103
		failBack
	}
}
