using System;
using System.Collections.Generic;
using Duckov.Quests;
using NodeCanvas.DialogueTrees;
using NodeCanvas.StateMachines;
using Saves;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001B9 RID: 441
public class CutScene : MonoBehaviour
{
	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06000D3B RID: 3387 RVA: 0x00037E62 File Offset: 0x00036062
	private string SaveKey
	{
		get
		{
			return "CutScene_" + this.id;
		}
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06000D3C RID: 3388 RVA: 0x00037E74 File Offset: 0x00036074
	private bool UseTrigger
	{
		get
		{
			return this.playTiming == CutScene.PlayTiming.OnTriggerEnter;
		}
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06000D3D RID: 3389 RVA: 0x00037E7F File Offset: 0x0003607F
	private bool HideFSMOwnerField
	{
		get
		{
			return !this.fsmOwner && this.dialogueTreeOwner;
		}
	}

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06000D3E RID: 3390 RVA: 0x00037E9B File Offset: 0x0003609B
	private bool HideDialogueTreeOwnerField
	{
		get
		{
			return this.fsmOwner && !this.dialogueTreeOwner;
		}
	}

	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06000D3F RID: 3391 RVA: 0x00037EBA File Offset: 0x000360BA
	private bool Played
	{
		get
		{
			return SavesSystem.Load<bool>(this.SaveKey);
		}
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x00037EC7 File Offset: 0x000360C7
	public void MarkPlayed()
	{
		if (string.IsNullOrWhiteSpace(this.id))
		{
			return;
		}
		SavesSystem.Save<bool>(this.SaveKey, true);
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x00037EE3 File Offset: 0x000360E3
	private void OnEnable()
	{
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x00037EE5 File Offset: 0x000360E5
	private void Awake()
	{
		if (this.UseTrigger)
		{
			this.InitializeTrigger();
		}
	}

	// Token: 0x06000D43 RID: 3395 RVA: 0x00037EF8 File Offset: 0x000360F8
	private void InitializeTrigger()
	{
		if (this.trigger == null)
		{
			Debug.LogError("CutScene想要使用Trigger触发，但没有配置Trigger引用。", this);
		}
		OnTriggerEnterEvent onTriggerEnterEvent = ComponentHolderProtocol.AddComponent<OnTriggerEnterEvent>(this.trigger);
		onTriggerEnterEvent.onlyMainCharacter = true;
		onTriggerEnterEvent.triggerOnce = true;
		onTriggerEnterEvent.DoOnTriggerEnter.AddListener(new UnityAction(this.PlayIfNessisary));
	}

	// Token: 0x06000D44 RID: 3396 RVA: 0x00037F4D File Offset: 0x0003614D
	private void Start()
	{
		if (this.playTiming == CutScene.PlayTiming.Start)
		{
			this.PlayIfNessisary();
		}
	}

	// Token: 0x06000D45 RID: 3397 RVA: 0x00037F60 File Offset: 0x00036160
	private void Update()
	{
		if (this.playing)
		{
			if (this.fsmOwner)
			{
				if (!this.fsmOwner.isRunning)
				{
					this.playing = false;
					this.OnPlayFinished();
					return;
				}
			}
			else if (this.dialogueTreeOwner && !this.dialogueTreeOwner.isRunning)
			{
				this.playing = false;
				this.OnPlayFinished();
			}
		}
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x00037FC4 File Offset: 0x000361C4
	private void OnPlayFinished()
	{
		this.MarkPlayed();
		if (this.setActiveFalseWhenFinished)
		{
			base.gameObject.SetActive(false);
		}
		if (this.playOnce && string.IsNullOrWhiteSpace(this.id))
		{
			Debug.LogError("CutScene没有填写ID，无法记录", base.gameObject);
		}
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x00038010 File Offset: 0x00036210
	public void PlayIfNessisary()
	{
		if (this.playOnce && this.Played)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (!this.prerequisites.Satisfied())
		{
			return;
		}
		this.Play();
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x00038044 File Offset: 0x00036244
	public void Play()
	{
		if (this.fsmOwner)
		{
			this.fsmOwner.StartBehaviour();
			this.playing = true;
			return;
		}
		if (this.dialogueTreeOwner)
		{
			if (this.setupActorReferencesUsingIDs)
			{
				this.SetupActors();
			}
			this.dialogueTreeOwner.StartBehaviour();
			this.playing = true;
		}
	}

	// Token: 0x06000D49 RID: 3401 RVA: 0x000380A0 File Offset: 0x000362A0
	private void SetupActors()
	{
		if (this.dialogueTreeOwner == null)
		{
			return;
		}
		if (this.dialogueTreeOwner.behaviour == null)
		{
			Debug.LogError("Dialoguetree没有配置", this.dialogueTreeOwner);
			return;
		}
		foreach (DialogueTree.ActorParameter actorParameter in this.dialogueTreeOwner.behaviour.actorParameters)
		{
			string name = actorParameter.name;
			if (!string.IsNullOrEmpty(name))
			{
				DuckovDialogueActor duckovDialogueActor = DuckovDialogueActor.Get(name);
				if (duckovDialogueActor == null)
				{
					Debug.LogError("未找到actor ID:" + name);
				}
				else
				{
					this.dialogueTreeOwner.SetActorReference(name, duckovDialogueActor);
				}
			}
		}
	}

	// Token: 0x04000B82 RID: 2946
	[SerializeField]
	private string id;

	// Token: 0x04000B83 RID: 2947
	[SerializeField]
	private bool playOnce = true;

	// Token: 0x04000B84 RID: 2948
	[SerializeField]
	private bool setActiveFalseWhenFinished = true;

	// Token: 0x04000B85 RID: 2949
	[SerializeField]
	private bool setupActorReferencesUsingIDs;

	// Token: 0x04000B86 RID: 2950
	[SerializeField]
	private Collider trigger;

	// Token: 0x04000B87 RID: 2951
	[SerializeField]
	private List<Condition> prerequisites = new List<Condition>();

	// Token: 0x04000B88 RID: 2952
	[SerializeField]
	private FSMOwner fsmOwner;

	// Token: 0x04000B89 RID: 2953
	[SerializeField]
	private DialogueTreeController dialogueTreeOwner;

	// Token: 0x04000B8A RID: 2954
	[SerializeField]
	private CutScene.PlayTiming playTiming;

	// Token: 0x04000B8B RID: 2955
	private bool playing;

	// Token: 0x020004EC RID: 1260
	public enum PlayTiming
	{
		// Token: 0x04001DB2 RID: 7602
		Start,
		// Token: 0x04001DB3 RID: 7603
		OnTriggerEnter = 2,
		// Token: 0x04001DB4 RID: 7604
		Manual
	}
}
