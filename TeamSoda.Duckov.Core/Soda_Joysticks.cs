using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001F0 RID: 496
public class Soda_Joysticks : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler
{
	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06000EDA RID: 3802 RVA: 0x0003C33C File Offset: 0x0003A53C
	public bool Holding
	{
		get
		{
			return this.holding;
		}
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06000EDB RID: 3803 RVA: 0x0003C344 File Offset: 0x0003A544
	public Vector2 InputValue
	{
		get
		{
			return this.inputValue;
		}
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x0003C34C File Offset: 0x0003A54C
	private void Start()
	{
		this.joyImage.gameObject.SetActive(false);
		if (this.hideWhenNotTouch)
		{
			this.canvasGroup.alpha = 0f;
		}
		if (this.cancleRangeCanvasGroup)
		{
			this.cancleRangeCanvasGroup.alpha = 0f;
		}
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x0003C39F File Offset: 0x0003A59F
	private void Update()
	{
		if (this.holding && !this.usable)
		{
			this.Revert();
		}
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x0003C3B7 File Offset: 0x0003A5B7
	private void OnEnable()
	{
		if (this.cancleRangeCanvasGroup)
		{
			this.cancleRangeCanvasGroup.alpha = 0f;
		}
		this.triggeringCancle = false;
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x0003C3E0 File Offset: 0x0003A5E0
	public void OnPointerDown(PointerEventData eventData)
	{
		if (!this.usable)
		{
			return;
		}
		if (this.holding)
		{
			return;
		}
		this.holding = true;
		this.currentPointerID = eventData.pointerId;
		this.downPoint = eventData.position;
		this.verticalRes = Screen.height;
		this.joystickRangePixel = (float)this.verticalRes * this.joystickRangePercent;
		this.cancleRangePixel = (float)this.verticalRes * this.cancleRangePercent;
		if (!this.fixedPositon)
		{
			this.backGround.transform.position = this.downPoint;
		}
		this.joyImage.transform.position = this.backGround.transform.position;
		this.backGround.transform.rotation = Quaternion.Euler(Vector3.zero);
		this.joyImage.gameObject.SetActive(true);
		UnityEvent<Vector2, bool> updateValueEvent = this.UpdateValueEvent;
		if (updateValueEvent != null)
		{
			updateValueEvent.Invoke(Vector2.zero, true);
		}
		if (this.hideWhenNotTouch)
		{
			this.canvasGroup.alpha = 1f;
		}
		if (this.canCancle && this.cancleRangeCanvasGroup)
		{
			this.cancleRangeCanvasGroup.alpha = 0.12f;
		}
		this.triggeringCancle = false;
		UnityEvent onTouchEvent = this.OnTouchEvent;
		if (onTouchEvent == null)
		{
			return;
		}
		onTouchEvent.Invoke();
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x0003C52C File Offset: 0x0003A72C
	public void OnPointerUp(PointerEventData eventData)
	{
		if (!this.usable)
		{
			return;
		}
		UnityEvent<bool> onUpEvent = this.OnUpEvent;
		if (onUpEvent != null)
		{
			onUpEvent.Invoke(!this.triggeringCancle);
		}
		UnityEvent<Vector2, bool> updateValueEvent = this.UpdateValueEvent;
		if (updateValueEvent != null)
		{
			updateValueEvent.Invoke(Vector2.zero, false);
		}
		if (this.holding && this.currentPointerID == eventData.pointerId)
		{
			this.Revert();
		}
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x0003C590 File Offset: 0x0003A790
	private void Revert()
	{
		UnityEvent<Vector2, bool> updateValueEvent = this.UpdateValueEvent;
		if (updateValueEvent != null)
		{
			updateValueEvent.Invoke(Vector2.zero, false);
		}
		if (this.holding)
		{
			UnityEvent<bool> onUpEvent = this.OnUpEvent;
			if (onUpEvent != null)
			{
				onUpEvent.Invoke(false);
			}
		}
		if (!this.usable)
		{
			return;
		}
		this.joyImage.transform.position = this.backGround.transform.position;
		this.inputValue = Vector2.zero;
		this.holding = false;
		this.backGround.transform.rotation = Quaternion.Euler(Vector3.zero);
		if (this.joyImage.gameObject.activeSelf)
		{
			this.joyImage.gameObject.SetActive(false);
		}
		if (this.hideWhenNotTouch)
		{
			this.canvasGroup.alpha = 0f;
		}
		if (this.cancleRangeCanvasGroup)
		{
			this.cancleRangeCanvasGroup.alpha = 0f;
		}
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x0003C67B File Offset: 0x0003A87B
	public void CancleTouch()
	{
		this.Revert();
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x0003C683 File Offset: 0x0003A883
	public void OnDisable()
	{
		this.Revert();
	}

	// Token: 0x06000EE4 RID: 3812 RVA: 0x0003C68C File Offset: 0x0003A88C
	public void OnDrag(PointerEventData eventData)
	{
		if (this.holding && eventData.pointerId == this.currentPointerID)
		{
			Vector2 vector = eventData.position;
			if (vector == this.downPoint)
			{
				this.inputValue = Vector2.zero;
				return;
			}
			float num = Vector2.Distance(vector, this.downPoint);
			float num2 = num;
			Vector2 normalized = (vector - this.downPoint).normalized;
			if (num > this.joystickRangePixel)
			{
				if (this.followFinger)
				{
					this.downPoint += (num - this.joystickRangePixel) * normalized;
				}
				if (!this.fixedPositon && this.followFinger)
				{
					this.backGround.transform.position = this.downPoint;
				}
				num2 = this.joystickRangePixel;
			}
			vector = this.downPoint + normalized * num2;
			Vector2 vector2 = Vector2.zero;
			if (this.joystickRangePixel > 0f)
			{
				vector2 = normalized * num2 / this.joystickRangePixel;
			}
			this.joyImage.transform.position = this.backGround.transform.position + normalized * num2;
			Vector3 vector3 = Vector3.zero;
			vector3.y = -vector2.x;
			vector3.x = vector2.y;
			vector3 *= this.rotValue;
			this.backGround.transform.rotation = Quaternion.Euler(vector3);
			float num3 = vector2.magnitude;
			num3 = Mathf.InverseLerp(this.deadZone, this.fullZone, num3);
			this.inputValue = num3 * normalized;
			UnityEvent<Vector2, bool> updateValueEvent = this.UpdateValueEvent;
			if (updateValueEvent != null)
			{
				updateValueEvent.Invoke(this.inputValue, true);
			}
			if (this.canCancle && this.cancleRangeCanvasGroup)
			{
				if (num >= this.cancleRangePixel)
				{
					this.cancleRangeCanvasGroup.alpha = 1f;
					this.triggeringCancle = true;
					return;
				}
				this.cancleRangeCanvasGroup.alpha = 0.12f;
				this.triggeringCancle = false;
			}
		}
	}

	// Token: 0x04000C50 RID: 3152
	public bool usable = true;

	// Token: 0x04000C51 RID: 3153
	private int verticalRes;

	// Token: 0x04000C52 RID: 3154
	[Range(0f, 0.5f)]
	public float joystickRangePercent = 0.3f;

	// Token: 0x04000C53 RID: 3155
	[Range(0f, 0.5f)]
	public float cancleRangePercent = 0.4f;

	// Token: 0x04000C54 RID: 3156
	public bool fixedPositon = true;

	// Token: 0x04000C55 RID: 3157
	public bool followFinger;

	// Token: 0x04000C56 RID: 3158
	public bool canCancle;

	// Token: 0x04000C57 RID: 3159
	private float joystickRangePixel;

	// Token: 0x04000C58 RID: 3160
	private float cancleRangePixel;

	// Token: 0x04000C59 RID: 3161
	[SerializeField]
	private Transform backGround;

	// Token: 0x04000C5A RID: 3162
	[SerializeField]
	private Image joyImage;

	// Token: 0x04000C5B RID: 3163
	[SerializeField]
	private CanvasGroup cancleRangeCanvasGroup;

	// Token: 0x04000C5C RID: 3164
	private bool holding;

	// Token: 0x04000C5D RID: 3165
	private Vector2 downPoint;

	// Token: 0x04000C5E RID: 3166
	private int currentPointerID;

	// Token: 0x04000C5F RID: 3167
	private Vector2 inputValue;

	// Token: 0x04000C60 RID: 3168
	[SerializeField]
	private float rotValue = 10f;

	// Token: 0x04000C61 RID: 3169
	[Range(0f, 1f)]
	public float deadZone;

	// Token: 0x04000C62 RID: 3170
	[Range(0f, 1f)]
	public float fullZone = 1f;

	// Token: 0x04000C63 RID: 3171
	public bool hideWhenNotTouch;

	// Token: 0x04000C64 RID: 3172
	public CanvasGroup canvasGroup;

	// Token: 0x04000C65 RID: 3173
	private bool triggeringCancle;

	// Token: 0x04000C66 RID: 3174
	public UnityEvent<Vector2, bool> UpdateValueEvent;

	// Token: 0x04000C67 RID: 3175
	public UnityEvent OnTouchEvent;

	// Token: 0x04000C68 RID: 3176
	public UnityEvent<bool> OnUpEvent;
}
