using System;
using Duckov.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

// Token: 0x020001F4 RID: 500
public class TaskSkipperUI : MonoBehaviour
{
	// Token: 0x06000EF2 RID: 3826 RVA: 0x0003CAB0 File Offset: 0x0003ACB0
	private void Awake()
	{
		UIInputManager.OnInteractInputContext += this.OnInteractInputContext;
		this.anyButtonListener = Observable.Call<InputControl>(InputSystem.onAnyButtonPress, new Action<InputControl>(this.OnAnyButton));
		this.skipped = false;
		this.alpha = 0f;
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x0003CAFC File Offset: 0x0003ACFC
	private void OnAnyButton(InputControl control)
	{
		this.Show();
	}

	// Token: 0x06000EF4 RID: 3828 RVA: 0x0003CB04 File Offset: 0x0003AD04
	private void OnDestroy()
	{
		UIInputManager.OnInteractInputContext -= this.OnInteractInputContext;
		this.anyButtonListener.Dispose();
	}

	// Token: 0x06000EF5 RID: 3829 RVA: 0x0003CB22 File Offset: 0x0003AD22
	private void OnInteractInputContext(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			this.pressing = true;
		}
		if (context.canceled)
		{
			this.pressing = false;
		}
	}

	// Token: 0x06000EF6 RID: 3830 RVA: 0x0003CB44 File Offset: 0x0003AD44
	private void Update()
	{
		this.UpdatePressing();
		this.UpdateFill();
		this.UpdateCanvasGroup();
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x0003CB58 File Offset: 0x0003AD58
	private void Show()
	{
		this.show = true;
		this.hideTimer = this.hideAfterSeconds;
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x0003CB70 File Offset: 0x0003AD70
	private void UpdatePressing()
	{
		if (UIInputManager.Instance == null)
		{
			this.pressing = Keyboard.current.fKey.isPressed;
		}
		if (this.pressing && !this.skipped)
		{
			this.pressTime += Time.deltaTime;
			if (this.pressTime >= this.totalTime)
			{
				this.skipped = true;
				this.target.Skip();
			}
			this.Show();
			return;
		}
		if (!this.skipped)
		{
			this.pressTime = Mathf.MoveTowards(this.pressTime, 0f, Time.deltaTime);
		}
	}

	// Token: 0x06000EF9 RID: 3833 RVA: 0x0003CC0C File Offset: 0x0003AE0C
	private void UpdateFill()
	{
		float fillAmount = this.pressTime / this.totalTime;
		this.fill.fillAmount = fillAmount;
	}

	// Token: 0x06000EFA RID: 3834 RVA: 0x0003CC34 File Offset: 0x0003AE34
	private void UpdateCanvasGroup()
	{
		if (this.show)
		{
			this.alpha = Mathf.MoveTowards(this.alpha, 1f, 10f * Time.deltaTime);
			this.hideTimer = Mathf.MoveTowards(this.hideTimer, 0f, Time.deltaTime);
			if (this.hideTimer < 0.01f)
			{
				this.show = false;
			}
		}
		else
		{
			this.alpha = Mathf.MoveTowards(this.alpha, 0f, 10f * Time.deltaTime);
		}
		this.canvasGroup.alpha = this.alpha;
	}

	// Token: 0x04000C70 RID: 3184
	[SerializeField]
	private TaskList target;

	// Token: 0x04000C71 RID: 3185
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x04000C72 RID: 3186
	[SerializeField]
	private Image fill;

	// Token: 0x04000C73 RID: 3187
	[SerializeField]
	private float totalTime = 2f;

	// Token: 0x04000C74 RID: 3188
	[SerializeField]
	private float hideAfterSeconds = 2f;

	// Token: 0x04000C75 RID: 3189
	private float pressTime;

	// Token: 0x04000C76 RID: 3190
	private float alpha;

	// Token: 0x04000C77 RID: 3191
	private float hideTimer;

	// Token: 0x04000C78 RID: 3192
	private bool show;

	// Token: 0x04000C79 RID: 3193
	private IDisposable anyButtonListener;

	// Token: 0x04000C7A RID: 3194
	private bool pressing;

	// Token: 0x04000C7B RID: 3195
	private bool skipped;
}
