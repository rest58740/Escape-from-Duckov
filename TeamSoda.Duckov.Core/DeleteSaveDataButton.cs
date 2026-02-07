using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Saves;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200016C RID: 364
public class DeleteSaveDataButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x17000232 RID: 562
	// (get) Token: 0x06000B3C RID: 2876 RVA: 0x00030CCA File Offset: 0x0002EECA
	private float TimeSinceStartedHolding
	{
		get
		{
			return Time.unscaledTime - this.timeWhenStartedHolding;
		}
	}

	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06000B3D RID: 2877 RVA: 0x00030CD8 File Offset: 0x0002EED8
	private float T
	{
		get
		{
			if (this.totalTime <= 0f)
			{
				return 1f;
			}
			return Mathf.Clamp01(this.TimeSinceStartedHolding / this.totalTime);
		}
	}

	// Token: 0x06000B3E RID: 2878 RVA: 0x00030CFF File Offset: 0x0002EEFF
	public void OnPointerDown(PointerEventData eventData)
	{
		this.holding = true;
		this.timeWhenStartedHolding = Time.unscaledTime;
	}

	// Token: 0x06000B3F RID: 2879 RVA: 0x00030D13 File Offset: 0x0002EF13
	public void OnPointerUp(PointerEventData eventData)
	{
		this.holding = false;
		this.timeWhenStartedHolding = float.MaxValue;
		this.RefreshProgressBar();
	}

	// Token: 0x06000B40 RID: 2880 RVA: 0x00030D2D File Offset: 0x0002EF2D
	private void Start()
	{
		this.barFill.fillAmount = 0f;
	}

	// Token: 0x06000B41 RID: 2881 RVA: 0x00030D3F File Offset: 0x0002EF3F
	private void Update()
	{
		if (this.holding)
		{
			this.RefreshProgressBar();
			if (this.T >= 1f)
			{
				this.Execute();
			}
		}
	}

	// Token: 0x06000B42 RID: 2882 RVA: 0x00030D62 File Offset: 0x0002EF62
	private void Execute()
	{
		this.holding = false;
		this.DeleteCurrentSaveData();
		this.RefreshProgressBar();
		this.NotifySaveDeleted().Forget();
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x00030D84 File Offset: 0x0002EF84
	private UniTask NotifySaveDeleted()
	{
		DeleteSaveDataButton.<NotifySaveDeleted>d__14 <NotifySaveDeleted>d__;
		<NotifySaveDeleted>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<NotifySaveDeleted>d__.<>4__this = this;
		<NotifySaveDeleted>d__.<>1__state = -1;
		<NotifySaveDeleted>d__.<>t__builder.Start<DeleteSaveDataButton.<NotifySaveDeleted>d__14>(ref <NotifySaveDeleted>d__);
		return <NotifySaveDeleted>d__.<>t__builder.Task;
	}

	// Token: 0x06000B44 RID: 2884 RVA: 0x00030DC7 File Offset: 0x0002EFC7
	private void DeleteCurrentSaveData()
	{
		SavesSystem.DeleteCurrentSave();
	}

	// Token: 0x06000B45 RID: 2885 RVA: 0x00030DCE File Offset: 0x0002EFCE
	private void RefreshProgressBar()
	{
		this.barFill.fillAmount = this.T;
	}

	// Token: 0x040009C7 RID: 2503
	[SerializeField]
	private float totalTime = 3f;

	// Token: 0x040009C8 RID: 2504
	[SerializeField]
	private Image barFill;

	// Token: 0x040009C9 RID: 2505
	[SerializeField]
	private FadeGroup saveDeletedNotifierFadeGroup;

	// Token: 0x040009CA RID: 2506
	private float timeWhenStartedHolding = float.MaxValue;

	// Token: 0x040009CB RID: 2507
	private bool holding;
}
