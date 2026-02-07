using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using NodeCanvas.DialogueTrees;
using SodaCraft.Localizations;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Dialogues
{
	// Token: 0x02000224 RID: 548
	public class DialogueUI : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06001091 RID: 4241 RVA: 0x000413DF File Offset: 0x0003F5DF
		// (set) Token: 0x06001092 RID: 4242 RVA: 0x000413E6 File Offset: 0x0003F5E6
		public static DialogueUI instance { get; private set; }

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06001093 RID: 4243 RVA: 0x000413F0 File Offset: 0x0003F5F0
		private PrefabPool<DialogueUIChoice> ChoicePool
		{
			get
			{
				if (this._choicePool == null)
				{
					this._choicePool = new PrefabPool<DialogueUIChoice>(this.choiceTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._choicePool;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x00041429 File Offset: 0x0003F629
		public static bool Active
		{
			get
			{
				return !(DialogueUI.instance == null) && DialogueUI.instance.mainFadeGroup.IsShown;
			}
		}

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x06001095 RID: 4245 RVA: 0x0004144C File Offset: 0x0003F64C
		// (remove) Token: 0x06001096 RID: 4246 RVA: 0x00041480 File Offset: 0x0003F680
		public static event Action OnDialogueStatusChanged;

		// Token: 0x06001097 RID: 4247 RVA: 0x000414B3 File Offset: 0x0003F6B3
		private void Awake()
		{
			DialogueUI.instance = this;
			this.choiceTemplate.gameObject.SetActive(false);
			this.RegisterEvents();
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x000414D2 File Offset: 0x0003F6D2
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x000414DA File Offset: 0x0003F6DA
		private void Update()
		{
			this.RefreshActorPositionIndicator();
			if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
			{
				this.Confirm();
			}
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x00041500 File Offset: 0x0003F700
		private void OnEnable()
		{
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x00041502 File Offset: 0x0003F702
		private void OnDisable()
		{
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x00041504 File Offset: 0x0003F704
		private void RegisterEvents()
		{
			DialogueTree.OnDialogueStarted += this.OnDialogueStarted;
			DialogueTree.OnDialoguePaused += this.OnDialoguePaused;
			DialogueTree.OnDialogueFinished += this.OnDialogueFinished;
			DialogueTree.OnSubtitlesRequest += this.OnSubtitlesRequest;
			DialogueTree.OnMultipleChoiceRequest += this.OnMultipleChoiceRequest;
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x00041568 File Offset: 0x0003F768
		private void UnregisterEvents()
		{
			DialogueTree.OnDialogueStarted -= this.OnDialogueStarted;
			DialogueTree.OnDialoguePaused -= this.OnDialoguePaused;
			DialogueTree.OnDialogueFinished -= this.OnDialogueFinished;
			DialogueTree.OnSubtitlesRequest -= this.OnSubtitlesRequest;
			DialogueTree.OnMultipleChoiceRequest -= this.OnMultipleChoiceRequest;
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x000415CA File Offset: 0x0003F7CA
		private void OnMultipleChoiceRequest(MultipleChoiceRequestInfo info)
		{
			this.DoMultipleChoice(info).Forget();
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x000415D8 File Offset: 0x0003F7D8
		private void OnSubtitlesRequest(SubtitlesRequestInfo info)
		{
			this.DoSubtitle(info).Forget();
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x000415E6 File Offset: 0x0003F7E6
		public static void HideTextFadeGroup()
		{
			DialogueUI.instance.MHideTextFadeGroup();
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x000415F2 File Offset: 0x0003F7F2
		private void MHideTextFadeGroup()
		{
			this.textAreaFadeGroup.Hide();
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x000415FF File Offset: 0x0003F7FF
		private void OnDialogueFinished(DialogueTree tree)
		{
			this.textAreaFadeGroup.Hide();
			InputManager.ActiveInput(base.gameObject);
			this.mainFadeGroup.Hide();
			Action onDialogueStatusChanged = DialogueUI.OnDialogueStatusChanged;
			if (onDialogueStatusChanged == null)
			{
				return;
			}
			onDialogueStatusChanged();
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x00041631 File Offset: 0x0003F831
		private void OnDialoguePaused(DialogueTree tree)
		{
			Action onDialogueStatusChanged = DialogueUI.OnDialogueStatusChanged;
			if (onDialogueStatusChanged == null)
			{
				return;
			}
			onDialogueStatusChanged();
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x00041642 File Offset: 0x0003F842
		private void OnDialogueStarted(DialogueTree tree)
		{
			InputManager.DisableInput(base.gameObject);
			this.mainFadeGroup.Show();
			Action onDialogueStatusChanged = DialogueUI.OnDialogueStatusChanged;
			if (onDialogueStatusChanged != null)
			{
				onDialogueStatusChanged();
			}
			this.actorNameFadeGroup.SkipHide();
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x00041678 File Offset: 0x0003F878
		public UniTask DoSubtitle(SubtitlesRequestInfo info)
		{
			DialogueUI.<DoSubtitle>d__40 <DoSubtitle>d__;
			<DoSubtitle>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoSubtitle>d__.<>4__this = this;
			<DoSubtitle>d__.info = info;
			<DoSubtitle>d__.<>1__state = -1;
			<DoSubtitle>d__.<>t__builder.Start<DialogueUI.<DoSubtitle>d__40>(ref <DoSubtitle>d__);
			return <DoSubtitle>d__.<>t__builder.Task;
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x000416C4 File Offset: 0x0003F8C4
		private void SetupActorInfo(IDialogueActor actor)
		{
			DuckovDialogueActor duckovDialogueActor = actor as DuckovDialogueActor;
			if (duckovDialogueActor == null)
			{
				this.actorNameFadeGroup.Hide();
				this.actorPortraitContainer.gameObject.SetActive(false);
				this.actorPositionIndicator.gameObject.SetActive(false);
				this.talkingActor = null;
				return;
			}
			this.talkingActor = duckovDialogueActor;
			Sprite portraitSprite = duckovDialogueActor.portraitSprite;
			string nameKey = duckovDialogueActor.NameKey;
			Transform transform = duckovDialogueActor.transform;
			this.actorNameText.text = LocalizationManager.ToPlainText(nameKey);
			this.actorNameFadeGroup.Show();
			this.actorPortraitContainer.SetActive(portraitSprite);
			this.actorPortraitDisplay.sprite = portraitSprite;
			if (this.talkingActor.transform != null)
			{
				this.actorPositionIndicator.gameObject.SetActive(true);
			}
			this.RefreshActorPositionIndicator();
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x00041790 File Offset: 0x0003F990
		private void RefreshActorPositionIndicator()
		{
			if (this.talkingActor == null)
			{
				this.actorPositionIndicator.gameObject.SetActive(false);
				return;
			}
			this.actorPositionIndicator.MatchWorldPosition(this.talkingActor.transform.position + this.talkingActor.Offset, default(Vector3));
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x000417F4 File Offset: 0x0003F9F4
		private UniTask DoMultipleChoice(MultipleChoiceRequestInfo info)
		{
			DialogueUI.<DoMultipleChoice>d__43 <DoMultipleChoice>d__;
			<DoMultipleChoice>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoMultipleChoice>d__.<>4__this = this;
			<DoMultipleChoice>d__.info = info;
			<DoMultipleChoice>d__.<>1__state = -1;
			<DoMultipleChoice>d__.<>t__builder.Start<DialogueUI.<DoMultipleChoice>d__43>(ref <DoMultipleChoice>d__);
			return <DoMultipleChoice>d__.<>t__builder.Task;
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x00041840 File Offset: 0x0003FA40
		private UniTask DisplayOptions(Dictionary<IStatement, int> options)
		{
			DialogueUI.<DisplayOptions>d__44 <DisplayOptions>d__;
			<DisplayOptions>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DisplayOptions>d__.<>4__this = this;
			<DisplayOptions>d__.options = options;
			<DisplayOptions>d__.<>1__state = -1;
			<DisplayOptions>d__.<>t__builder.Start<DialogueUI.<DisplayOptions>d__44>(ref <DisplayOptions>d__);
			return <DisplayOptions>d__.<>t__builder.Task;
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0004188B File Offset: 0x0003FA8B
		internal void NotifyChoiceConfirmed(DialogueUIChoice choice)
		{
			this.confirmedChoice = choice.Index;
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0004189C File Offset: 0x0003FA9C
		private UniTask<int> WaitForChoice()
		{
			DialogueUI.<WaitForChoice>d__48 <WaitForChoice>d__;
			<WaitForChoice>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<WaitForChoice>d__.<>4__this = this;
			<WaitForChoice>d__.<>1__state = -1;
			<WaitForChoice>d__.<>t__builder.Start<DialogueUI.<WaitForChoice>d__48>(ref <WaitForChoice>d__);
			return <WaitForChoice>d__.<>t__builder.Task;
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x000418DF File Offset: 0x0003FADF
		public void Confirm()
		{
			this.confirmed = true;
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x000418E8 File Offset: 0x0003FAE8
		private UniTask WaitForConfirm()
		{
			DialogueUI.<WaitForConfirm>d__51 <WaitForConfirm>d__;
			<WaitForConfirm>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WaitForConfirm>d__.<>4__this = this;
			<WaitForConfirm>d__.<>1__state = -1;
			<WaitForConfirm>d__.<>t__builder.Start<DialogueUI.<WaitForConfirm>d__51>(ref <WaitForConfirm>d__);
			return <WaitForConfirm>d__.<>t__builder.Task;
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x0004192B File Offset: 0x0003FB2B
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Confirm();
		}

		// Token: 0x04000D57 RID: 3415
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x04000D58 RID: 3416
		[SerializeField]
		private FadeGroup textAreaFadeGroup;

		// Token: 0x04000D59 RID: 3417
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04000D5A RID: 3418
		[SerializeField]
		private GameObject continueIndicator;

		// Token: 0x04000D5B RID: 3419
		[SerializeField]
		private float speed = 10f;

		// Token: 0x04000D5C RID: 3420
		[SerializeField]
		private RectTransform actorPositionIndicator;

		// Token: 0x04000D5D RID: 3421
		[SerializeField]
		private FadeGroup actorNameFadeGroup;

		// Token: 0x04000D5E RID: 3422
		[SerializeField]
		private TextMeshProUGUI actorNameText;

		// Token: 0x04000D5F RID: 3423
		[SerializeField]
		private GameObject actorPortraitContainer;

		// Token: 0x04000D60 RID: 3424
		[SerializeField]
		private Image actorPortraitDisplay;

		// Token: 0x04000D61 RID: 3425
		[SerializeField]
		private FadeGroup choiceListFadeGroup;

		// Token: 0x04000D62 RID: 3426
		[SerializeField]
		private Menu choiceMenu;

		// Token: 0x04000D63 RID: 3427
		[SerializeField]
		private DialogueUIChoice choiceTemplate;

		// Token: 0x04000D64 RID: 3428
		private PrefabPool<DialogueUIChoice> _choicePool;

		// Token: 0x04000D65 RID: 3429
		private DuckovDialogueActor talkingActor;

		// Token: 0x04000D67 RID: 3431
		private int confirmedChoice;

		// Token: 0x04000D68 RID: 3432
		private bool waitingForChoice;

		// Token: 0x04000D69 RID: 3433
		private bool confirmed;
	}
}
