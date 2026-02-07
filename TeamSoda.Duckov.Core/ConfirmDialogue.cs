using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000175 RID: 373
public class ConfirmDialogue : MonoBehaviour
{
	// Token: 0x06000B6E RID: 2926 RVA: 0x0003131F File Offset: 0x0002F51F
	private void Awake()
	{
		this.btnConfirm.onClick.AddListener(new UnityAction(this.OnConfirmed));
		this.btnCancel.onClick.AddListener(new UnityAction(this.OnCanceled));
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x00031359 File Offset: 0x0002F559
	private void OnCanceled()
	{
		this.canceled = true;
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x00031362 File Offset: 0x0002F562
	private void OnConfirmed()
	{
		this.confirmed = true;
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x0003136C File Offset: 0x0002F56C
	public UniTask<bool> Execute()
	{
		ConfirmDialogue.<Execute>d__9 <Execute>d__;
		<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<Execute>d__.<>4__this = this;
		<Execute>d__.<>1__state = -1;
		<Execute>d__.<>t__builder.Start<ConfirmDialogue.<Execute>d__9>(ref <Execute>d__);
		return <Execute>d__.<>t__builder.Task;
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x000313B0 File Offset: 0x0002F5B0
	private UniTask<bool> DoExecute()
	{
		ConfirmDialogue.<DoExecute>d__10 <DoExecute>d__;
		<DoExecute>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<DoExecute>d__.<>4__this = this;
		<DoExecute>d__.<>1__state = -1;
		<DoExecute>d__.<>t__builder.Start<ConfirmDialogue.<DoExecute>d__10>(ref <DoExecute>d__);
		return <DoExecute>d__.<>t__builder.Task;
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x000313F3 File Offset: 0x0002F5F3
	internal void SkipHide()
	{
		this.fadeGroup.SkipHide();
	}

	// Token: 0x040009E2 RID: 2530
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x040009E3 RID: 2531
	[SerializeField]
	private Button btnConfirm;

	// Token: 0x040009E4 RID: 2532
	[SerializeField]
	private Button btnCancel;

	// Token: 0x040009E5 RID: 2533
	private bool canceled;

	// Token: 0x040009E6 RID: 2534
	private bool confirmed;

	// Token: 0x040009E7 RID: 2535
	private bool executing;
}
