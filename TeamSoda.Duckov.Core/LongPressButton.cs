using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200015F RID: 351
public class LongPressButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerExitHandler
{
	// Token: 0x1700022C RID: 556
	// (get) Token: 0x06000AED RID: 2797 RVA: 0x000301DB File Offset: 0x0002E3DB
	private float TimeSincePressStarted
	{
		get
		{
			return Time.unscaledTime - this.timeWhenPressStarted;
		}
	}

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x06000AEE RID: 2798 RVA: 0x000301E9 File Offset: 0x0002E3E9
	private float Progress
	{
		get
		{
			if (!this.pressed)
			{
				return 0f;
			}
			return this.TimeSincePressStarted / this.pressTime;
		}
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x00030206 File Offset: 0x0002E406
	private void Update()
	{
		this.fill.fillAmount = this.Progress;
		if (this.pressed && this.Progress >= 1f)
		{
			UnityEvent unityEvent = this.onPressFullfilled;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.pressed = false;
		}
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x00030246 File Offset: 0x0002E446
	public void OnPointerDown(PointerEventData eventData)
	{
		this.pressed = true;
		this.timeWhenPressStarted = Time.unscaledTime;
		UnityEvent unityEvent = this.onPressStarted;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x0003026A File Offset: 0x0002E46A
	public void OnPointerExit(PointerEventData eventData)
	{
		if (!this.pressed)
		{
			return;
		}
		this.pressed = false;
		UnityEvent unityEvent = this.onPressCanceled;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x0003028C File Offset: 0x0002E48C
	public void OnPointerUp(PointerEventData eventData)
	{
		if (!this.pressed)
		{
			return;
		}
		this.pressed = false;
		UnityEvent unityEvent = this.onPressCanceled;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x04000998 RID: 2456
	[SerializeField]
	private Image fill;

	// Token: 0x04000999 RID: 2457
	[SerializeField]
	private float pressTime = 1f;

	// Token: 0x0400099A RID: 2458
	public UnityEvent onPressStarted;

	// Token: 0x0400099B RID: 2459
	public UnityEvent onPressCanceled;

	// Token: 0x0400099C RID: 2460
	public UnityEvent onPressFullfilled;

	// Token: 0x0400099D RID: 2461
	private float timeWhenPressStarted;

	// Token: 0x0400099E RID: 2462
	private bool pressed;
}
