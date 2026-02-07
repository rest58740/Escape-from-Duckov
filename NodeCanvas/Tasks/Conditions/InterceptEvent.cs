using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000050 RID: 80
	[Category("UGUI")]
	[Description("Returns true when the selected event is triggered on the selected agent.\nYou can use this for both GUI and 3D objects.\nPlease make sure that Unity Event Systems are setup correctly")]
	public class InterceptEvent : ConditionTask<Transform>
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600018D RID: 397 RVA: 0x000075BE File Offset: 0x000057BE
		protected override string info
		{
			get
			{
				return string.Format("{0} on {1}", this.eventType.ToString(), base.agentInfo);
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000075E4 File Offset: 0x000057E4
		protected override void OnEnable()
		{
			switch (this.eventType)
			{
			case EventTriggerType.PointerEnter:
				base.router.onPointerEnter += this.OnPointerEnter;
				return;
			case EventTriggerType.PointerExit:
				base.router.onPointerExit += this.OnPointerExit;
				return;
			case EventTriggerType.PointerDown:
				base.router.onPointerDown += this.OnPointerDown;
				return;
			case EventTriggerType.PointerUp:
				base.router.onPointerUp += this.OnPointerUp;
				return;
			case EventTriggerType.PointerClick:
				base.router.onPointerClick += this.OnPointerClick;
				return;
			case EventTriggerType.Drag:
				base.router.onDrag += this.OnDrag;
				return;
			case EventTriggerType.Drop:
				base.router.onDrop += this.OnDrop;
				return;
			case EventTriggerType.Scroll:
				base.router.onScroll += this.OnScroll;
				return;
			case EventTriggerType.UpdateSelected:
				base.router.onUpdateSelected += this.OnUpdateSelected;
				return;
			case EventTriggerType.Select:
				base.router.onSelect += this.OnSelect;
				return;
			case EventTriggerType.Deselect:
				base.router.onDeselect += this.OnDeselect;
				return;
			case EventTriggerType.Move:
				base.router.onMove += this.OnMove;
				return;
			case EventTriggerType.InitializePotentialDrag:
			case EventTriggerType.BeginDrag:
			case EventTriggerType.EndDrag:
				break;
			case EventTriggerType.Submit:
				base.router.onSubmit += this.OnSubmit;
				break;
			default:
				return;
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00007778 File Offset: 0x00005978
		protected override void OnDisable()
		{
			switch (this.eventType)
			{
			case EventTriggerType.PointerEnter:
				base.router.onPointerEnter -= this.OnPointerEnter;
				return;
			case EventTriggerType.PointerExit:
				base.router.onPointerExit -= this.OnPointerExit;
				return;
			case EventTriggerType.PointerDown:
				base.router.onPointerDown -= this.OnPointerDown;
				return;
			case EventTriggerType.PointerUp:
				base.router.onPointerUp -= this.OnPointerUp;
				return;
			case EventTriggerType.PointerClick:
				base.router.onPointerClick -= this.OnPointerClick;
				return;
			case EventTriggerType.Drag:
				base.router.onDrag -= this.OnDrag;
				return;
			case EventTriggerType.Drop:
				base.router.onDrop -= this.OnDrop;
				return;
			case EventTriggerType.Scroll:
				base.router.onScroll -= this.OnScroll;
				return;
			case EventTriggerType.UpdateSelected:
				base.router.onUpdateSelected -= this.OnUpdateSelected;
				return;
			case EventTriggerType.Select:
				base.router.onSelect -= this.OnSelect;
				return;
			case EventTriggerType.Deselect:
				base.router.onDeselect -= this.OnDeselect;
				return;
			case EventTriggerType.Move:
				base.router.onMove -= this.OnMove;
				return;
			case EventTriggerType.InitializePotentialDrag:
			case EventTriggerType.BeginDrag:
			case EventTriggerType.EndDrag:
				break;
			case EventTriggerType.Submit:
				base.router.onSubmit -= this.OnSubmit;
				break;
			default:
				return;
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000790A File Offset: 0x00005B0A
		protected override bool OnCheck()
		{
			return false;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000790D File Offset: 0x00005B0D
		private void OnPointerEnter(EventData<PointerEventData> data)
		{
			base.YieldReturn(true);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00007916 File Offset: 0x00005B16
		private void OnPointerExit(EventData<PointerEventData> data)
		{
			base.YieldReturn(true);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000791F File Offset: 0x00005B1F
		private void OnPointerDown(EventData<PointerEventData> data)
		{
			base.YieldReturn(true);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00007928 File Offset: 0x00005B28
		private void OnPointerUp(EventData<PointerEventData> data)
		{
			base.YieldReturn(true);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00007931 File Offset: 0x00005B31
		private void OnPointerClick(EventData<PointerEventData> data)
		{
			base.YieldReturn(true);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000793A File Offset: 0x00005B3A
		private void OnDrag(EventData<PointerEventData> data)
		{
			base.YieldReturn(true);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007943 File Offset: 0x00005B43
		private void OnDrop(EventData<PointerEventData> eventData)
		{
			base.YieldReturn(true);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000794C File Offset: 0x00005B4C
		private void OnScroll(EventData<PointerEventData> data)
		{
			base.YieldReturn(true);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00007955 File Offset: 0x00005B55
		private void OnUpdateSelected(EventData<BaseEventData> eventData)
		{
			base.YieldReturn(true);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000795E File Offset: 0x00005B5E
		private void OnSelect(EventData<BaseEventData> eventData)
		{
			base.YieldReturn(true);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00007967 File Offset: 0x00005B67
		private void OnDeselect(EventData<BaseEventData> eventData)
		{
			base.YieldReturn(true);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00007970 File Offset: 0x00005B70
		private void OnMove(EventData<AxisEventData> eventData)
		{
			base.YieldReturn(true);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00007979 File Offset: 0x00005B79
		private void OnSubmit(EventData<BaseEventData> eventData)
		{
			base.YieldReturn(true);
		}

		// Token: 0x040000F8 RID: 248
		public EventTriggerType eventType;
	}
}
