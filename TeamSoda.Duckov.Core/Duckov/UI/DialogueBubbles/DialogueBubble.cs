using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI.DialogueBubbles
{
	// Token: 0x02000402 RID: 1026
	public class DialogueBubble : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06002533 RID: 9523 RVA: 0x000820EA File Offset: 0x000802EA
		public Transform Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06002534 RID: 9524 RVA: 0x000820F2 File Offset: 0x000802F2
		private float YOffset
		{
			get
			{
				if (this._yOffset < 0f)
				{
					return this.defaultYOffset;
				}
				return this._yOffset;
			}
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x0008210E File Offset: 0x0008030E
		private void LateUpdate()
		{
			this.UpdatePosition();
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x00082118 File Offset: 0x00080318
		private void UpdatePosition()
		{
			if (this.target == null)
			{
				return;
			}
			Camera main = Camera.main;
			if (main == null)
			{
				return;
			}
			Vector3 vector = this.target.position + Vector3.up * this.YOffset;
			if (Vector3.Dot(vector - main.transform.position, main.transform.forward) < 0f)
			{
				base.transform.localPosition = Vector3.one * 1000000f;
				return;
			}
			Vector2 vector2 = RectTransformUtility.WorldToScreenPoint(Camera.main, vector);
			vector2.y += this.screenYOffset * (float)Screen.height;
			Vector2 vector3;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform.parent as RectTransform, vector2, null, ref vector3))
			{
				return;
			}
			base.transform.localPosition = vector3;
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x000821FC File Offset: 0x000803FC
		public UniTask Show(string text, Transform target, float yOffset = -1f, bool needInteraction = false, bool skippable = false, float speed = -1f, float duration = 2f)
		{
			this.task = this.ShowTask(text, target, yOffset, needInteraction, skippable, speed, duration);
			return this.task;
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x00082228 File Offset: 0x00080428
		public UniTask ShowTask(string text, Transform target, float yOffset = -1f, bool needInteraction = false, bool skippable = false, float speed = -1f, float duration = 2f)
		{
			DialogueBubble.<ShowTask>d__20 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.text = text;
			<ShowTask>d__.target = target;
			<ShowTask>d__.yOffset = yOffset;
			<ShowTask>d__.needInteraction = needInteraction;
			<ShowTask>d__.skippable = skippable;
			<ShowTask>d__.speed = speed;
			<ShowTask>d__.duration = duration;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<DialogueBubble.<ShowTask>d__20>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x000822A8 File Offset: 0x000804A8
		private UniTask WaitForInteraction(int currentToken)
		{
			DialogueBubble.<WaitForInteraction>d__21 <WaitForInteraction>d__;
			<WaitForInteraction>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WaitForInteraction>d__.<>4__this = this;
			<WaitForInteraction>d__.currentToken = currentToken;
			<WaitForInteraction>d__.<>1__state = -1;
			<WaitForInteraction>d__.<>t__builder.Start<DialogueBubble.<WaitForInteraction>d__21>(ref <WaitForInteraction>d__);
			return <WaitForInteraction>d__.<>t__builder.Task;
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x000822F3 File Offset: 0x000804F3
		public void Interact()
		{
			this.interacted = true;
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x000822FC File Offset: 0x000804FC
		private UniTask Hide()
		{
			DialogueBubble.<Hide>d__23 <Hide>d__;
			<Hide>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Hide>d__.<>4__this = this;
			<Hide>d__.<>1__state = -1;
			<Hide>d__.<>t__builder.Start<DialogueBubble.<Hide>d__23>(ref <Hide>d__);
			return <Hide>d__.<>t__builder.Task;
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x0008233F File Offset: 0x0008053F
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Interact();
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x00082347 File Offset: 0x00080547
		private void Awake()
		{
			DialogueBubblesManager.onPointerClick += this.OnPointerClick;
		}

		// Token: 0x04001942 RID: 6466
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001943 RID: 6467
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001944 RID: 6468
		[SerializeField]
		private float defaultSpeed = 10f;

		// Token: 0x04001945 RID: 6469
		[SerializeField]
		private float sustainDuration = 2f;

		// Token: 0x04001946 RID: 6470
		[SerializeField]
		private float defaultYOffset = 2f;

		// Token: 0x04001947 RID: 6471
		[SerializeField]
		private GameObject interactIndicator;

		// Token: 0x04001948 RID: 6472
		private bool interacted;

		// Token: 0x04001949 RID: 6473
		private bool animating;

		// Token: 0x0400194A RID: 6474
		private int taskToken;

		// Token: 0x0400194B RID: 6475
		private Transform target;

		// Token: 0x0400194C RID: 6476
		private float _yOffset;

		// Token: 0x0400194D RID: 6477
		private float screenYOffset = 0.06f;

		// Token: 0x0400194E RID: 6478
		private UniTask task;
	}
}
