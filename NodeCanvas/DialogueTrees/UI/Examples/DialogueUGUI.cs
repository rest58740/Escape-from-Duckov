using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NodeCanvas.DialogueTrees.UI.Examples
{
	// Token: 0x02000108 RID: 264
	public class DialogueUGUI : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x00011B40 File Offset: 0x0000FD40
		private AudioSource localSource
		{
			get
			{
				if (!(this._localSource != null))
				{
					return this._localSource = base.gameObject.AddComponent<AudioSource>();
				}
				return this._localSource;
			}
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00011B76 File Offset: 0x0000FD76
		public void OnPointerClick(PointerEventData eventData)
		{
			this.anyKeyDown = true;
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00011B7F File Offset: 0x0000FD7F
		private void LateUpdate()
		{
			this.anyKeyDown = false;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00011B88 File Offset: 0x0000FD88
		private void Awake()
		{
			this.Subscribe();
			this.Hide();
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00011B96 File Offset: 0x0000FD96
		private void OnEnable()
		{
			this.UnSubscribe();
			this.Subscribe();
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00011BA4 File Offset: 0x0000FDA4
		private void OnDisable()
		{
			this.UnSubscribe();
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00011BAC File Offset: 0x0000FDAC
		private void Subscribe()
		{
			DialogueTree.OnDialogueStarted += new Action<DialogueTree>(this.OnDialogueStarted);
			DialogueTree.OnDialoguePaused += new Action<DialogueTree>(this.OnDialoguePaused);
			DialogueTree.OnDialogueFinished += new Action<DialogueTree>(this.OnDialogueFinished);
			DialogueTree.OnSubtitlesRequest += new Action<SubtitlesRequestInfo>(this.OnSubtitlesRequest);
			DialogueTree.OnMultipleChoiceRequest += new Action<MultipleChoiceRequestInfo>(this.OnMultipleChoiceRequest);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00011C10 File Offset: 0x0000FE10
		private void UnSubscribe()
		{
			DialogueTree.OnDialogueStarted -= new Action<DialogueTree>(this.OnDialogueStarted);
			DialogueTree.OnDialoguePaused -= new Action<DialogueTree>(this.OnDialoguePaused);
			DialogueTree.OnDialogueFinished -= new Action<DialogueTree>(this.OnDialogueFinished);
			DialogueTree.OnSubtitlesRequest -= new Action<SubtitlesRequestInfo>(this.OnSubtitlesRequest);
			DialogueTree.OnMultipleChoiceRequest -= new Action<MultipleChoiceRequestInfo>(this.OnMultipleChoiceRequest);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00011C74 File Offset: 0x0000FE74
		private void Hide()
		{
			this.subtitlesGroup.gameObject.SetActive(false);
			this.optionsGroup.gameObject.SetActive(false);
			this.optionButton.gameObject.SetActive(false);
			this.waitInputIndicator.gameObject.SetActive(false);
			this.originalSubsPosition = this.subtitlesGroup.transform.position;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00011CE0 File Offset: 0x0000FEE0
		private void OnDialogueStarted(DialogueTree dlg)
		{
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00011CE4 File Offset: 0x0000FEE4
		private void OnDialoguePaused(DialogueTree dlg)
		{
			this.subtitlesGroup.gameObject.SetActive(false);
			this.optionsGroup.gameObject.SetActive(false);
			base.StopAllCoroutines();
			if (this.playSource != null)
			{
				this.playSource.Stop();
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00011D34 File Offset: 0x0000FF34
		private void OnDialogueFinished(DialogueTree dlg)
		{
			this.subtitlesGroup.gameObject.SetActive(false);
			this.optionsGroup.gameObject.SetActive(false);
			if (this.cachedButtons != null)
			{
				foreach (Button button in this.cachedButtons.Keys)
				{
					if (button != null)
					{
						Object.Destroy(button.gameObject);
					}
				}
				this.cachedButtons = null;
			}
			base.StopAllCoroutines();
			if (this.playSource != null)
			{
				this.playSource.Stop();
			}
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00011DEC File Offset: 0x0000FFEC
		private void OnSubtitlesRequest(SubtitlesRequestInfo info)
		{
			base.StartCoroutine(this.Internal_OnSubtitlesRequestInfo(info));
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00011DFC File Offset: 0x0000FFFC
		private IEnumerator Internal_OnSubtitlesRequestInfo(SubtitlesRequestInfo info)
		{
			string text = info.statement.text;
			AudioClip audio = info.statement.audio;
			IDialogueActor actor = info.actor;
			this.subtitlesGroup.gameObject.SetActive(true);
			this.subtitlesGroup.position = this.originalSubsPosition;
			this.actorSpeech.text = "";
			this.actorName.text = actor.name;
			this.actorSpeech.color = actor.dialogueColor;
			this.actorPortrait.gameObject.SetActive(actor.portraitSprite != null);
			this.actorPortrait.sprite = actor.portraitSprite;
			if (audio != null)
			{
				AudioSource audioSource = (actor.transform != null) ? actor.transform.GetComponent<AudioSource>() : null;
				this.playSource = ((audioSource != null) ? audioSource : this.localSource);
				this.playSource.clip = audio;
				this.playSource.Play();
				this.actorSpeech.text = text;
				float timer = 0f;
				while (timer < audio.length)
				{
					if (this.skipOnInput && this.anyKeyDown)
					{
						this.playSource.Stop();
						break;
					}
					timer += Time.deltaTime;
					yield return null;
				}
			}
			if (audio == null)
			{
				DialogueUGUI.<>c__DisplayClass32_0 CS$<>8__locals1 = new DialogueUGUI.<>c__DisplayClass32_0();
				string tempText = "";
				CS$<>8__locals1.inputDown = false;
				if (this.skipOnInput)
				{
					base.StartCoroutine(this.CheckInput(delegate
					{
						CS$<>8__locals1.inputDown = true;
					}));
				}
				int num;
				for (int i = 0; i < text.Length; i = num + 1)
				{
					if (this.skipOnInput & CS$<>8__locals1.inputDown)
					{
						this.actorSpeech.text = text;
						yield return null;
						break;
					}
					if (!this.subtitlesGroup.gameObject.activeSelf)
					{
						yield break;
					}
					char c = text.get_Chars(i);
					tempText += c.ToString();
					yield return base.StartCoroutine(this.DelayPrint(this.subtitleDelays.characterDelay));
					this.PlayTypeSound();
					if (c == '.' || c == '!' || c == '?')
					{
						yield return base.StartCoroutine(this.DelayPrint(this.subtitleDelays.sentenceDelay));
						this.PlayTypeSound();
					}
					if (c == ',')
					{
						yield return base.StartCoroutine(this.DelayPrint(this.subtitleDelays.commaDelay));
						this.PlayTypeSound();
					}
					this.actorSpeech.text = tempText;
					num = i;
				}
				if (!this.waitForInput)
				{
					yield return base.StartCoroutine(this.DelayPrint(this.subtitleDelays.finalDelay));
				}
				CS$<>8__locals1 = null;
				tempText = null;
			}
			if (this.waitForInput)
			{
				this.waitInputIndicator.gameObject.SetActive(true);
				while (!this.anyKeyDown)
				{
					yield return null;
				}
				this.waitInputIndicator.gameObject.SetActive(false);
			}
			yield return null;
			this.subtitlesGroup.gameObject.SetActive(false);
			info.Continue.Invoke();
			yield break;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00011E12 File Offset: 0x00010012
		private void PlayTypeSound()
		{
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00011E14 File Offset: 0x00010014
		private IEnumerator CheckInput(Action Do)
		{
			while (!this.anyKeyDown)
			{
				yield return null;
			}
			Do.Invoke();
			yield break;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00011E2A File Offset: 0x0001002A
		private IEnumerator DelayPrint(float time)
		{
			float timer = 0f;
			while (timer < time)
			{
				timer += Time.deltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00011E3C File Offset: 0x0001003C
		private void OnMultipleChoiceRequest(MultipleChoiceRequestInfo info)
		{
			this.optionsGroup.gameObject.SetActive(true);
			float height = this.optionButton.GetComponent<RectTransform>().rect.height;
			this.optionsGroup.sizeDelta = new Vector2(this.optionsGroup.sizeDelta.x, (float)info.options.Values.Count * height + 20f);
			this.cachedButtons = new Dictionary<Button, int>();
			int num = 0;
			foreach (KeyValuePair<IStatement, int> keyValuePair in info.options)
			{
				Button btn = Object.Instantiate<Button>(this.optionButton);
				btn.gameObject.SetActive(true);
				btn.transform.SetParent(this.optionsGroup.transform, false);
				btn.transform.localPosition = this.optionButton.transform.localPosition - new Vector3(0f, height * (float)num, 0f);
				btn.GetComponentInChildren<Text>().text = keyValuePair.Key.text;
				this.cachedButtons.Add(btn, keyValuePair.Value);
				btn.onClick.AddListener(delegate()
				{
					this.Finalize(info, this.cachedButtons[btn]);
				});
				num++;
			}
			if (info.showLastStatement)
			{
				this.subtitlesGroup.gameObject.SetActive(true);
				float y = this.optionsGroup.position.y + this.optionsGroup.sizeDelta.y + 1f;
				this.subtitlesGroup.position = new Vector3(this.subtitlesGroup.position.x, y, this.subtitlesGroup.position.z);
			}
			if (info.availableTime > 0f)
			{
				base.StartCoroutine(this.CountDown(info));
			}
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x000120A0 File Offset: 0x000102A0
		private IEnumerator CountDown(MultipleChoiceRequestInfo info)
		{
			this.isWaitingChoice = true;
			float timer = 0f;
			while (timer < info.availableTime)
			{
				if (!this.isWaitingChoice)
				{
					yield break;
				}
				timer += Time.deltaTime;
				this.SetMassAlpha(this.optionsGroup, Mathf.Lerp(1f, 0f, timer / info.availableTime));
				yield return null;
			}
			if (this.isWaitingChoice)
			{
				this.Finalize(info, info.options.Values.Last<int>());
			}
			yield break;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x000120B8 File Offset: 0x000102B8
		private void Finalize(MultipleChoiceRequestInfo info, int index)
		{
			this.isWaitingChoice = false;
			this.SetMassAlpha(this.optionsGroup, 1f);
			this.optionsGroup.gameObject.SetActive(false);
			this.subtitlesGroup.gameObject.SetActive(false);
			foreach (Button button in this.cachedButtons.Keys)
			{
				Object.Destroy(button.gameObject);
			}
			info.SelectOption.Invoke(index);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00012158 File Offset: 0x00010358
		private void SetMassAlpha(RectTransform root, float alpha)
		{
			CanvasRenderer[] componentsInChildren = root.GetComponentsInChildren<CanvasRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SetAlpha(alpha);
			}
		}

		// Token: 0x040002F0 RID: 752
		[Header("Input Options")]
		public bool skipOnInput;

		// Token: 0x040002F1 RID: 753
		public bool waitForInput;

		// Token: 0x040002F2 RID: 754
		[Header("Subtitles")]
		public RectTransform subtitlesGroup;

		// Token: 0x040002F3 RID: 755
		public Text actorSpeech;

		// Token: 0x040002F4 RID: 756
		public Text actorName;

		// Token: 0x040002F5 RID: 757
		public Image actorPortrait;

		// Token: 0x040002F6 RID: 758
		public RectTransform waitInputIndicator;

		// Token: 0x040002F7 RID: 759
		public DialogueUGUI.SubtitleDelays subtitleDelays = new DialogueUGUI.SubtitleDelays();

		// Token: 0x040002F8 RID: 760
		public List<AudioClip> typingSounds;

		// Token: 0x040002F9 RID: 761
		private AudioSource playSource;

		// Token: 0x040002FA RID: 762
		[Header("Multiple Choice")]
		public RectTransform optionsGroup;

		// Token: 0x040002FB RID: 763
		public Button optionButton;

		// Token: 0x040002FC RID: 764
		private Dictionary<Button, int> cachedButtons;

		// Token: 0x040002FD RID: 765
		private Vector2 originalSubsPosition;

		// Token: 0x040002FE RID: 766
		private bool isWaitingChoice;

		// Token: 0x040002FF RID: 767
		private AudioSource _localSource;

		// Token: 0x04000300 RID: 768
		private bool anyKeyDown;

		// Token: 0x0200015E RID: 350
		[Serializable]
		public class SubtitleDelays
		{
			// Token: 0x040003E8 RID: 1000
			public float characterDelay = 0.05f;

			// Token: 0x040003E9 RID: 1001
			public float sentenceDelay = 0.5f;

			// Token: 0x040003EA RID: 1002
			public float commaDelay = 0.1f;

			// Token: 0x040003EB RID: 1003
			public float finalDelay = 1.2f;
		}
	}
}
