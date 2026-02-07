using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LeTai.TrueShadow
{
	// Token: 0x02000007 RID: 7
	public class AnimatedBiStateButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000022 RID: 34 RVA: 0x0000265C File Offset: 0x0000085C
		// (remove) Token: 0x06000023 RID: 35 RVA: 0x00002694 File Offset: 0x00000894
		public event Action willPress;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000024 RID: 36 RVA: 0x000026CC File Offset: 0x000008CC
		// (remove) Token: 0x06000025 RID: 37 RVA: 0x00002704 File Offset: 0x00000904
		public event Action willRelease;

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002739 File Offset: 0x00000939
		protected bool IsAnimating
		{
			get
			{
				return this.state == AnimatedBiStateButton.State.AnimateDown || this.state == AnimatedBiStateButton.State.AnimateUp;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000274F File Offset: 0x0000094F
		private void Update()
		{
			this.PollPointerUp();
			this.DoAnimation();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002760 File Offset: 0x00000960
		private void DoAnimation()
		{
			if (!this.IsAnimating)
			{
				return;
			}
			if (this.state == AnimatedBiStateButton.State.AnimateDown)
			{
				this.pressAmount += Time.deltaTime / this.animationDuration;
			}
			else if (this.state == AnimatedBiStateButton.State.AnimateUp)
			{
				this.pressAmount -= Time.deltaTime / this.animationDuration;
			}
			this.pressAmount = Mathf.Clamp01(this.pressAmount);
			float num = this.pressAmount;
			if (this.state == AnimatedBiStateButton.State.AnimateUp)
			{
				num = 1f - num;
			}
			num = this.animationCurve.Evaluate(num);
			if (this.state == AnimatedBiStateButton.State.AnimateUp)
			{
				num = 1f - num;
			}
			this.Animate(num);
			if (this.state == AnimatedBiStateButton.State.AnimateDown && this.pressAmount == 1f)
			{
				this.state = AnimatedBiStateButton.State.Down;
			}
			if (this.state == AnimatedBiStateButton.State.AnimateUp && this.pressAmount == 0f)
			{
				this.state = AnimatedBiStateButton.State.Up;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002844 File Offset: 0x00000A44
		protected void Press()
		{
			if (this.state != AnimatedBiStateButton.State.Down && this.state != AnimatedBiStateButton.State.AnimateDown)
			{
				this.OnWillPress();
				this.state = AnimatedBiStateButton.State.AnimateDown;
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002865 File Offset: 0x00000A65
		protected void Release()
		{
			if (this.state != AnimatedBiStateButton.State.Up && this.state != AnimatedBiStateButton.State.AnimateUp)
			{
				this.OnWillRelease();
				this.state = AnimatedBiStateButton.State.AnimateUp;
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002885 File Offset: 0x00000A85
		private void PollPointerUp()
		{
			if (this.useEnterExitEvents && (this.state == AnimatedBiStateButton.State.Down || this.state == AnimatedBiStateButton.State.AnimateDown) && !Input.GetMouseButton(0))
			{
				this.Release();
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000028AF File Offset: 0x00000AAF
		protected virtual void Animate(float visualPressAmount)
		{
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000028B1 File Offset: 0x00000AB1
		public void OnPointerDown(PointerEventData eventData)
		{
			this.Press();
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000028B9 File Offset: 0x00000AB9
		public void OnPointerUp(PointerEventData eventData)
		{
			this.Release();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000028C1 File Offset: 0x00000AC1
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.useEnterExitEvents && Input.GetMouseButton(0))
			{
				this.Press();
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000028D9 File Offset: 0x00000AD9
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.useEnterExitEvents)
			{
				this.Release();
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000028E9 File Offset: 0x00000AE9
		protected virtual void OnWillPress()
		{
			Action action = this.willPress;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000028FB File Offset: 0x00000AFB
		protected virtual void OnWillRelease()
		{
			Action action = this.willRelease;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x04000010 RID: 16
		public float animationDuration = 0.1f;

		// Token: 0x04000011 RID: 17
		public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x04000012 RID: 18
		public bool useEnterExitEvents = true;

		// Token: 0x04000015 RID: 21
		protected AnimatedBiStateButton.State state;

		// Token: 0x04000016 RID: 22
		protected float pressAmount;

		// Token: 0x02000030 RID: 48
		public enum State
		{
			// Token: 0x040000CF RID: 207
			Up,
			// Token: 0x040000D0 RID: 208
			AnimateDown,
			// Token: 0x040000D1 RID: 209
			Down,
			// Token: 0x040000D2 RID: 210
			AnimateUp
		}
	}
}
