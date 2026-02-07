using System;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using ItemStatsSystem;
using ItemStatsSystem.Data;
using ItemStatsSystem.Items;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.MiniGames
{
	// Token: 0x0200028E RID: 654
	public class GamingConsole : InteractableBase
	{
		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001506 RID: 5382 RVA: 0x0004E82F File Offset: 0x0004CA2F
		public MiniGame SelectedGame
		{
			get
			{
				if (this.CatridgeGameID == null)
				{
					return null;
				}
				return this.possibleGames.Find((MiniGame e) => e != null && e.ID == this.CatridgeGameID);
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001507 RID: 5383 RVA: 0x0004E852 File Offset: 0x0004CA52
		public MiniGame Game
		{
			get
			{
				return this.game;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001508 RID: 5384 RVA: 0x0004E85A File Offset: 0x0004CA5A
		public Slot MonitorSlot
		{
			get
			{
				return this.mainItem.Slots["Monitor"];
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001509 RID: 5385 RVA: 0x0004E871 File Offset: 0x0004CA71
		public Slot ConsoleSlot
		{
			get
			{
				return this.mainItem.Slots["Console"];
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x0600150A RID: 5386 RVA: 0x0004E888 File Offset: 0x0004CA88
		public bool controllerConnected
		{
			get
			{
				if (this.mainItem == null)
				{
					return false;
				}
				if (this.ConsoleSlot == null)
				{
					return false;
				}
				Item content = this.ConsoleSlot.Content;
				if (content == null)
				{
					return false;
				}
				Slot slot = content.Slots["FcController"];
				return slot != null && slot.Content != null;
			}
		}

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x0600150B RID: 5387 RVA: 0x0004E8E8 File Offset: 0x0004CAE8
		// (remove) Token: 0x0600150C RID: 5388 RVA: 0x0004E920 File Offset: 0x0004CB20
		public event Action<GamingConsole> onContentChanged;

		// Token: 0x1400008F RID: 143
		// (add) Token: 0x0600150D RID: 5389 RVA: 0x0004E958 File Offset: 0x0004CB58
		// (remove) Token: 0x0600150E RID: 5390 RVA: 0x0004E990 File Offset: 0x0004CB90
		public event Action<GamingConsole> OnAfterAnimateIn;

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x0600150F RID: 5391 RVA: 0x0004E9C8 File Offset: 0x0004CBC8
		// (remove) Token: 0x06001510 RID: 5392 RVA: 0x0004EA00 File Offset: 0x0004CC00
		public event Action<GamingConsole> OnBeforeAnimateOut;

		// Token: 0x14000091 RID: 145
		// (add) Token: 0x06001511 RID: 5393 RVA: 0x0004EA38 File Offset: 0x0004CC38
		// (remove) Token: 0x06001512 RID: 5394 RVA: 0x0004EA6C File Offset: 0x0004CC6C
		public static event Action<bool> OnGamingConsoleInteractChanged;

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001513 RID: 5395 RVA: 0x0004EA9F File Offset: 0x0004CC9F
		public Item Monitor
		{
			get
			{
				if (this.MonitorSlot == null)
				{
					return null;
				}
				return this.MonitorSlot.Content;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06001514 RID: 5396 RVA: 0x0004EAB6 File Offset: 0x0004CCB6
		public Item Console
		{
			get
			{
				if (this.ConsoleSlot == null)
				{
					return null;
				}
				return this.ConsoleSlot.Content;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06001515 RID: 5397 RVA: 0x0004EAD0 File Offset: 0x0004CCD0
		public Item Cartridge
		{
			get
			{
				if (this.Console == null)
				{
					return null;
				}
				if (!this.Console.Slots)
				{
					Debug.LogError(this.Console.DisplayName + " has no catridge slot");
					return null;
				}
				Slot slot = this.Console.Slots["Cartridge"];
				if (slot == null)
				{
					Debug.LogError(this.Console.DisplayName + " has no catridge slot");
					return null;
				}
				return slot.Content;
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001516 RID: 5398 RVA: 0x0004EB56 File Offset: 0x0004CD56
		public string CatridgeGameID
		{
			get
			{
				if (this.Cartridge == null)
				{
					return null;
				}
				return this.Cartridge.Constants.GetString("GameID", null);
			}
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x0004EB80 File Offset: 0x0004CD80
		private UniTask Load()
		{
			GamingConsole.<Load>d__50 <Load>d__;
			<Load>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Load>d__.<>4__this = this;
			<Load>d__.<>1__state = -1;
			<Load>d__.<>t__builder.Start<GamingConsole.<Load>d__50>(ref <Load>d__);
			return <Load>d__.<>t__builder.Task;
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x0004EBC4 File Offset: 0x0004CDC4
		private void Save()
		{
			if (this.loading)
			{
				return;
			}
			if (!this.loaded)
			{
				return;
			}
			GamingConsole.SaveData saveData = new GamingConsole.SaveData();
			if (this.Console != null)
			{
				saveData.consoleData = ItemTreeData.FromItem(this.Console);
			}
			if (this.Monitor != null)
			{
				saveData.monitorData = ItemTreeData.FromItem(this.Monitor);
			}
			SavesSystem.Save<GamingConsole.SaveData>(this.SaveKey, saveData);
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x0004EC34 File Offset: 0x0004CE34
		protected override void Awake()
		{
			base.Awake();
			UIInputManager.OnCancel += this.OnUICancel;
			SavesSystem.OnCollectSaveData += this.Save;
			this.inputHandler.enabled = false;
			this.mainItem.onItemTreeChanged += this.OnContentChanged;
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x0004EC8C File Offset: 0x0004CE8C
		protected override void OnDestroy()
		{
			base.OnDestroy();
			Action<bool> onGamingConsoleInteractChanged = GamingConsole.OnGamingConsoleInteractChanged;
			if (onGamingConsoleInteractChanged != null)
			{
				onGamingConsoleInteractChanged(false);
			}
			UIInputManager.OnCancel -= this.OnUICancel;
			SavesSystem.OnCollectSaveData -= this.Save;
			this.isBeingDestroyed = true;
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x0004ECD9 File Offset: 0x0004CED9
		private void OnDisable()
		{
			Action<bool> onGamingConsoleInteractChanged = GamingConsole.OnGamingConsoleInteractChanged;
			if (onGamingConsoleInteractChanged == null)
			{
				return;
			}
			onGamingConsoleInteractChanged(false);
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x0004ECEB File Offset: 0x0004CEEB
		protected override void Start()
		{
			base.Start();
			this.Load().Forget();
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x0004ECFE File Offset: 0x0004CEFE
		private void OnContentChanged(Item item)
		{
			Action<GamingConsole> action = this.onContentChanged;
			if (action != null)
			{
				action(this);
			}
			this.RefreshGame();
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x0004ED18 File Offset: 0x0004CF18
		private void OnUICancel(UIInputEventData data)
		{
			if (data.Used)
			{
				return;
			}
			if (base.Interacting)
			{
				base.StopInteract();
				data.Use();
			}
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x0004ED38 File Offset: 0x0004CF38
		protected override void OnInteractStart(CharacterMainControl interactCharacter)
		{
			base.OnInteractStart(interactCharacter);
			Action<bool> onGamingConsoleInteractChanged = GamingConsole.OnGamingConsoleInteractChanged;
			if (onGamingConsoleInteractChanged != null)
			{
				onGamingConsoleInteractChanged(this);
			}
			if (this.Console == null || this.Monitor == null || this.Cartridge == null)
			{
				NotificationText.Push(LocalizationManager.ToPlainText(this.incompleteNotificationText));
				base.StopInteract();
				return;
			}
			if (this.SelectedGame == null)
			{
				NotificationText.Push(LocalizationManager.ToPlainText(this.noGameNotificationText));
				base.StopInteract();
				return;
			}
			this.RefreshGame();
			this.inputHandler.enabled = this.controllerConnected;
			this.AnimateCameraIn().Forget();
			HUDManager.RegisterHideToken(this);
			CharacterMainControl.Main.SetPosition(this.teleportToPositionWhenBegin.position);
			GamingConsoleHUD.Show();
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x0004EE0C File Offset: 0x0004D00C
		private UniTask AnimateCameraIn()
		{
			GamingConsole.<AnimateCameraIn>d__61 <AnimateCameraIn>d__;
			<AnimateCameraIn>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimateCameraIn>d__.<>4__this = this;
			<AnimateCameraIn>d__.<>1__state = -1;
			<AnimateCameraIn>d__.<>t__builder.Start<GamingConsole.<AnimateCameraIn>d__61>(ref <AnimateCameraIn>d__);
			return <AnimateCameraIn>d__.<>t__builder.Task;
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x0004EE50 File Offset: 0x0004D050
		private UniTask AnimateCameraOut()
		{
			GamingConsole.<AnimateCameraOut>d__62 <AnimateCameraOut>d__;
			<AnimateCameraOut>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimateCameraOut>d__.<>4__this = this;
			<AnimateCameraOut>d__.<>1__state = -1;
			<AnimateCameraOut>d__.<>t__builder.Start<GamingConsole.<AnimateCameraOut>d__62>(ref <AnimateCameraOut>d__);
			return <AnimateCameraOut>d__.<>t__builder.Task;
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x0004EE93 File Offset: 0x0004D093
		protected override void OnInteractStop()
		{
			base.OnInteractStop();
			Action<bool> onGamingConsoleInteractChanged = GamingConsole.OnGamingConsoleInteractChanged;
			if (onGamingConsoleInteractChanged != null)
			{
				onGamingConsoleInteractChanged(false);
			}
			this.inputHandler.enabled = false;
			this.AnimateCameraOut().Forget();
			HUDManager.UnregisterHideToken(this);
			GamingConsoleHUD.Hide();
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x0004EED0 File Offset: 0x0004D0D0
		private void RefreshGame()
		{
			if (this.game == null)
			{
				this.CreateGame(this.SelectedGame);
				return;
			}
			if (this.SelectedGame == null || this.SelectedGame.ID != this.game.ID)
			{
				this.CreateGame(this.SelectedGame);
			}
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x0004EF30 File Offset: 0x0004D130
		private void CreateGame(MiniGame prefab)
		{
			if (this.isBeingDestroyed)
			{
				return;
			}
			if (this.game != null)
			{
				Object.Destroy(this.game.gameObject);
			}
			if (prefab == null)
			{
				return;
			}
			this.game = Object.Instantiate<MiniGame>(prefab);
			this.game.transform.SetParent(base.transform, true);
			this.game.SetRenderTexture(this.rt);
			this.game.SetConsole(this);
			this.inputHandler.SetGame(this.game);
		}

		// Token: 0x04000F75 RID: 3957
		[SerializeField]
		private List<MiniGame> possibleGames;

		// Token: 0x04000F76 RID: 3958
		[SerializeField]
		private RenderTexture rt;

		// Token: 0x04000F77 RID: 3959
		[SerializeField]
		private MiniGameInputHandler inputHandler;

		// Token: 0x04000F78 RID: 3960
		[SerializeField]
		private CinemachineVirtualCamera virtualCamera;

		// Token: 0x04000F79 RID: 3961
		[SerializeField]
		private float transitionTime = 1f;

		// Token: 0x04000F7A RID: 3962
		[SerializeField]
		private Transform vcamEndPosition;

		// Token: 0x04000F7B RID: 3963
		[SerializeField]
		private Transform vcamLookTarget;

		// Token: 0x04000F7C RID: 3964
		[SerializeField]
		private AnimationCurve posCurve;

		// Token: 0x04000F7D RID: 3965
		[SerializeField]
		private AnimationCurve rotCurve;

		// Token: 0x04000F7E RID: 3966
		[SerializeField]
		private AnimationCurve fovCurve;

		// Token: 0x04000F7F RID: 3967
		[SerializeField]
		private float activeFov = 45f;

		// Token: 0x04000F80 RID: 3968
		[SerializeField]
		private Transform teleportToPositionWhenBegin;

		// Token: 0x04000F81 RID: 3969
		[SerializeField]
		private Item mainItem;

		// Token: 0x04000F82 RID: 3970
		[SerializeField]
		[LocalizationKey("Default")]
		private string incompleteNotificationText = "GamingConsole_Incomplete";

		// Token: 0x04000F83 RID: 3971
		[SerializeField]
		[LocalizationKey("Default")]
		private string noGameNotificationText = "GamingConsole_NoGame";

		// Token: 0x04000F84 RID: 3972
		private MiniGame game;

		// Token: 0x04000F89 RID: 3977
		private string SaveKey = "GamingConsoleData";

		// Token: 0x04000F8A RID: 3978
		private bool loading;

		// Token: 0x04000F8B RID: 3979
		private bool loaded;

		// Token: 0x04000F8C RID: 3980
		private bool isBeingDestroyed;

		// Token: 0x04000F8D RID: 3981
		private int animateToken;

		// Token: 0x02000575 RID: 1397
		[Serializable]
		private class SaveData
		{
			// Token: 0x04001FBD RID: 8125
			public ItemTreeData monitorData;

			// Token: 0x04001FBE RID: 8126
			public ItemTreeData consoleData;
		}
	}
}
