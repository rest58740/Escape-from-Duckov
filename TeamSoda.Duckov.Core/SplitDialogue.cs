using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001FF RID: 511
public class SplitDialogue : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06000F24 RID: 3876 RVA: 0x0003D1C2 File Offset: 0x0003B3C2
	public static SplitDialogue Instance
	{
		get
		{
			if (GameplayUIManager.Instance == null)
			{
				return null;
			}
			return GameplayUIManager.Instance.SplitDialogue;
		}
	}

	// Token: 0x06000F25 RID: 3877 RVA: 0x0003D1DD File Offset: 0x0003B3DD
	private void OnEnable()
	{
		View.OnActiveViewChanged += this.OnActiveViewChanged;
	}

	// Token: 0x06000F26 RID: 3878 RVA: 0x0003D1F0 File Offset: 0x0003B3F0
	private void OnDisable()
	{
		View.OnActiveViewChanged -= this.OnActiveViewChanged;
	}

	// Token: 0x06000F27 RID: 3879 RVA: 0x0003D203 File Offset: 0x0003B403
	private void OnActiveViewChanged()
	{
		this.Hide();
	}

	// Token: 0x06000F28 RID: 3880 RVA: 0x0003D20B File Offset: 0x0003B40B
	private void Awake()
	{
		this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
		this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
	}

	// Token: 0x06000F29 RID: 3881 RVA: 0x0003D245 File Offset: 0x0003B445
	private void OnSliderValueChanged(float value)
	{
		this.RefreshCountText();
	}

	// Token: 0x06000F2A RID: 3882 RVA: 0x0003D250 File Offset: 0x0003B450
	private void RefreshCountText()
	{
		this.countText.text = this.slider.value.ToString("0");
	}

	// Token: 0x06000F2B RID: 3883 RVA: 0x0003D280 File Offset: 0x0003B480
	private void OnConfirmButtonClicked()
	{
		if (this.status != SplitDialogue.Status.Normal)
		{
			return;
		}
		this.Confirm().Forget();
	}

	// Token: 0x06000F2C RID: 3884 RVA: 0x0003D298 File Offset: 0x0003B498
	private void Setup(Item target, Inventory destination = null, int destinationIndex = -1)
	{
		this.target = target;
		this.destination = destination;
		this.destinationIndex = destinationIndex;
		this.slider.minValue = 1f;
		this.slider.maxValue = (float)target.StackCount;
		this.slider.value = (float)(target.StackCount - 1) / 2f;
		this.RefreshCountText();
		this.SwitchStatus(SplitDialogue.Status.Normal);
		this.cachedInInventory = target.InInventory;
	}

	// Token: 0x06000F2D RID: 3885 RVA: 0x0003D30F File Offset: 0x0003B50F
	public void Cancel()
	{
		if (this.status != SplitDialogue.Status.Normal)
		{
			return;
		}
		this.SwitchStatus(SplitDialogue.Status.Canceled);
		this.Hide();
	}

	// Token: 0x06000F2E RID: 3886 RVA: 0x0003D328 File Offset: 0x0003B528
	private UniTask Confirm()
	{
		SplitDialogue.<Confirm>d__22 <Confirm>d__;
		<Confirm>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Confirm>d__.<>4__this = this;
		<Confirm>d__.<>1__state = -1;
		<Confirm>d__.<>t__builder.Start<SplitDialogue.<Confirm>d__22>(ref <Confirm>d__);
		return <Confirm>d__.<>t__builder.Task;
	}

	// Token: 0x06000F2F RID: 3887 RVA: 0x0003D36B File Offset: 0x0003B56B
	private void Hide()
	{
		this.fadeGroup.Hide();
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x0003D378 File Offset: 0x0003B578
	private UniTask DoSplit(int value)
	{
		SplitDialogue.<DoSplit>d__24 <DoSplit>d__;
		<DoSplit>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<DoSplit>d__.<>4__this = this;
		<DoSplit>d__.value = value;
		<DoSplit>d__.<>1__state = -1;
		<DoSplit>d__.<>t__builder.Start<SplitDialogue.<DoSplit>d__24>(ref <DoSplit>d__);
		return <DoSplit>d__.<>t__builder.Task;
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x0003D3C4 File Offset: 0x0003B5C4
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerCurrentRaycast.gameObject == base.gameObject)
		{
			this.Cancel();
		}
	}

	// Token: 0x06000F32 RID: 3890 RVA: 0x0003D3F4 File Offset: 0x0003B5F4
	private void SwitchStatus(SplitDialogue.Status status)
	{
		this.status = status;
		this.normalIndicator.SetActive(status == SplitDialogue.Status.Normal);
		this.busyIndicator.SetActive(status == SplitDialogue.Status.Busy);
		this.completeIndicator.SetActive(status == SplitDialogue.Status.Complete);
		switch (status)
		{
		default:
			return;
		}
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x0003D44F File Offset: 0x0003B64F
	public static void SetupAndShow(Item item)
	{
		if (SplitDialogue.Instance == null)
		{
			return;
		}
		SplitDialogue.Instance.Setup(item, null, -1);
		SplitDialogue.Instance.fadeGroup.Show();
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x0003D47B File Offset: 0x0003B67B
	public static void SetupAndShow(Item item, Inventory destinationInventory, int destinationIndex)
	{
		if (SplitDialogue.Instance == null)
		{
			return;
		}
		SplitDialogue.Instance.Setup(item, destinationInventory, destinationIndex);
		SplitDialogue.Instance.fadeGroup.Show();
	}

	// Token: 0x06000F36 RID: 3894 RVA: 0x0003D4B0 File Offset: 0x0003B6B0
	[CompilerGenerated]
	private void <DoSplit>g__Send|24_0(Item item)
	{
		item.Detach();
		if (this.destination != null && this.destination.Capacity > this.destinationIndex && this.destination.GetItemAt(this.destinationIndex) == null)
		{
			this.destination.AddAt(item, this.destinationIndex);
			return;
		}
		ItemUtilities.SendToPlayerCharacterInventory(item, true);
	}

	// Token: 0x04000C94 RID: 3220
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000C95 RID: 3221
	[SerializeField]
	private Button confirmButton;

	// Token: 0x04000C96 RID: 3222
	[SerializeField]
	private TextMeshProUGUI countText;

	// Token: 0x04000C97 RID: 3223
	[SerializeField]
	private GameObject normalIndicator;

	// Token: 0x04000C98 RID: 3224
	[SerializeField]
	private GameObject busyIndicator;

	// Token: 0x04000C99 RID: 3225
	[SerializeField]
	private GameObject completeIndicator;

	// Token: 0x04000C9A RID: 3226
	[SerializeField]
	private Slider slider;

	// Token: 0x04000C9B RID: 3227
	private Item target;

	// Token: 0x04000C9C RID: 3228
	private Inventory destination;

	// Token: 0x04000C9D RID: 3229
	private int destinationIndex;

	// Token: 0x04000C9E RID: 3230
	private Inventory cachedInInventory;

	// Token: 0x04000C9F RID: 3231
	private SplitDialogue.Status status;

	// Token: 0x020004FF RID: 1279
	private enum Status
	{
		// Token: 0x04001DFD RID: 7677
		Idle,
		// Token: 0x04001DFE RID: 7678
		Normal,
		// Token: 0x04001DFF RID: 7679
		Busy,
		// Token: 0x04001E00 RID: 7680
		Complete,
		// Token: 0x04001E01 RID: 7681
		Canceled
	}
}
