using System;
using Duckov;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000C9 RID: 201
public class ActionProgressHUD : MonoBehaviour
{
	// Token: 0x1700013B RID: 315
	// (get) Token: 0x06000672 RID: 1650 RVA: 0x0001D231 File Offset: 0x0001B431
	public bool InProgress
	{
		get
		{
			return this.inProgress;
		}
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x0001D23C File Offset: 0x0001B43C
	public void Update()
	{
		if (!this.characterMainControl)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (this.characterMainControl)
			{
				this.characterMainControl.OnActionStartEvent += this.OnActionStart;
				this.characterMainControl.OnActionProgressFinishEvent += this.OnActionFinish;
			}
		}
		this.inProgress = false;
		float num = 0f;
		if (this.currentProgressInterface as Object != null)
		{
			Progress progress = this.currentProgressInterface.GetProgress();
			this.inProgress = progress.inProgress;
			num = progress.progress;
			if (!this.inProgress)
			{
				this.currentProgressInterface = null;
			}
		}
		if (this.inProgress)
		{
			this.targetAlpha = 1f;
			this.fillImage.fillAmount = num;
			if (num >= 1f)
			{
				this.targetAlpha = 0f;
			}
		}
		else
		{
			this.targetAlpha = 0f;
		}
		this.parentCanvasGroup.alpha = Mathf.MoveTowards(this.parentCanvasGroup.alpha, this.targetAlpha, 8f * Time.deltaTime);
		if (this.stopIndicator && this.characterMainControl)
		{
			bool flag = false;
			CharacterActionBase currentAction = this.characterMainControl.CurrentAction;
			if (currentAction && currentAction.Running && currentAction.IsStopable())
			{
				flag = true;
			}
			if (flag != this.stopIndicator.activeSelf && this.targetAlpha != 0f)
			{
				this.stopIndicator.SetActive(flag);
			}
		}
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0001D3C4 File Offset: 0x0001B5C4
	private void OnDestroy()
	{
		if (this.characterMainControl)
		{
			this.characterMainControl.OnActionStartEvent -= this.OnActionStart;
			this.characterMainControl.OnActionProgressFinishEvent -= this.OnActionFinish;
		}
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x0001D404 File Offset: 0x0001B604
	private void OnActionStart(CharacterActionBase action)
	{
		this.currentProgressInterface = (action as IProgress);
		if (this.specificActionType != CharacterActionBase.ActionPriorities.Whatever && action.ActionPriority() != this.specificActionType)
		{
			this.currentProgressInterface = null;
		}
		if (action && !action.progressHUD)
		{
			this.currentProgressInterface = null;
		}
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x0001D451 File Offset: 0x0001B651
	private void OnActionFinish(CharacterActionBase action)
	{
		UnityEvent onFinishEvent = this.OnFinishEvent;
		if (onFinishEvent != null)
		{
			onFinishEvent.Invoke();
		}
		if (this.fillImage)
		{
			this.fillImage.fillAmount = 1f;
		}
	}

	// Token: 0x04000636 RID: 1590
	public CharacterActionBase.ActionPriorities specificActionType;

	// Token: 0x04000637 RID: 1591
	public ProceduralImage fillImage;

	// Token: 0x04000638 RID: 1592
	public CanvasGroup parentCanvasGroup;

	// Token: 0x04000639 RID: 1593
	private CharacterMainControl characterMainControl;

	// Token: 0x0400063A RID: 1594
	private IProgress currentProgressInterface;

	// Token: 0x0400063B RID: 1595
	private float targetAlpha;

	// Token: 0x0400063C RID: 1596
	private bool inProgress;

	// Token: 0x0400063D RID: 1597
	public UnityEvent OnFinishEvent;

	// Token: 0x0400063E RID: 1598
	[FormerlySerializedAs("cancleIndicator")]
	public GameObject stopIndicator;
}
