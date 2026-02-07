using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000173 RID: 371
public class Button_LoadMainMenu : MonoBehaviour
{
	// Token: 0x06000B66 RID: 2918 RVA: 0x000311DA File Offset: 0x0002F3DA
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.BeginQuitting));
		this.dialogue.SkipHide();
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x00031203 File Offset: 0x0002F403
	private void BeginQuitting()
	{
		if (this.task.Status == UniTaskStatus.Pending)
		{
			return;
		}
		Debug.Log("Quitting");
		this.task = this.QuitTask();
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x0003122C File Offset: 0x0002F42C
	private UniTask QuitTask()
	{
		Button_LoadMainMenu.<QuitTask>d__5 <QuitTask>d__;
		<QuitTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<QuitTask>d__.<>4__this = this;
		<QuitTask>d__.<>1__state = -1;
		<QuitTask>d__.<>t__builder.Start<Button_LoadMainMenu.<QuitTask>d__5>(ref <QuitTask>d__);
		return <QuitTask>d__.<>t__builder.Task;
	}

	// Token: 0x040009DC RID: 2524
	[SerializeField]
	private Button button;

	// Token: 0x040009DD RID: 2525
	[SerializeField]
	private ConfirmDialogue dialogue;

	// Token: 0x040009DE RID: 2526
	private UniTask task;
}
