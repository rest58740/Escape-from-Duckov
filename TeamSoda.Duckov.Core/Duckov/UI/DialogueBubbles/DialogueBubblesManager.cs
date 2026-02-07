using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI.DialogueBubbles
{
	// Token: 0x02000403 RID: 1027
	public class DialogueBubblesManager : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x0600253F RID: 9535 RVA: 0x0008238F File Offset: 0x0008058F
		// (set) Token: 0x06002540 RID: 9536 RVA: 0x00082396 File Offset: 0x00080596
		public static DialogueBubblesManager Instance { get; private set; }

		// Token: 0x140000FE RID: 254
		// (add) Token: 0x06002541 RID: 9537 RVA: 0x000823A0 File Offset: 0x000805A0
		// (remove) Token: 0x06002542 RID: 9538 RVA: 0x000823D4 File Offset: 0x000805D4
		public static event Action<PointerEventData> onPointerClick;

		// Token: 0x06002543 RID: 9539 RVA: 0x00082407 File Offset: 0x00080607
		private void Awake()
		{
			if (DialogueBubblesManager.Instance == null)
			{
				DialogueBubblesManager.Instance = this;
			}
			this.prefab.gameObject.SetActive(false);
			this.raycastReceiver.enabled = false;
		}

		// Token: 0x06002544 RID: 9540 RVA: 0x0008243C File Offset: 0x0008063C
		public static UniTask Show(string text, Transform target, float yOffset = -1f, bool needInteraction = false, bool skippable = false, float speed = -1f, float duration = 2f)
		{
			DialogueBubblesManager.<Show>d__11 <Show>d__;
			<Show>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Show>d__.text = text;
			<Show>d__.target = target;
			<Show>d__.yOffset = yOffset;
			<Show>d__.needInteraction = needInteraction;
			<Show>d__.skippable = skippable;
			<Show>d__.speed = speed;
			<Show>d__.duration = duration;
			<Show>d__.<>1__state = -1;
			<Show>d__.<>t__builder.Start<DialogueBubblesManager.<Show>d__11>(ref <Show>d__);
			return <Show>d__.<>t__builder.Task;
		}

		// Token: 0x06002545 RID: 9541 RVA: 0x000824B2 File Offset: 0x000806B2
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<PointerEventData> action = DialogueBubblesManager.onPointerClick;
			if (action == null)
			{
				return;
			}
			action(eventData);
		}

		// Token: 0x04001950 RID: 6480
		[SerializeField]
		private DialogueBubble prefab;

		// Token: 0x04001951 RID: 6481
		[SerializeField]
		private Graphic raycastReceiver;

		// Token: 0x04001952 RID: 6482
		private List<DialogueBubble> bubbles = new List<DialogueBubble>();
	}
}
