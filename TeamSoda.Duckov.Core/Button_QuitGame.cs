using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000174 RID: 372
public class Button_QuitGame : MonoBehaviour
{
	// Token: 0x06000B6A RID: 2922 RVA: 0x00031277 File Offset: 0x0002F477
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.BeginQuitting));
		if (this.dialogue)
		{
			this.dialogue.SkipHide();
		}
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x000312AD File Offset: 0x0002F4AD
	private void BeginQuitting()
	{
		if (this.task.Status == UniTaskStatus.Pending)
		{
			return;
		}
		Debug.Log("Quitting");
		this.task = this.QuitTask();
	}

	// Token: 0x06000B6C RID: 2924 RVA: 0x000312D4 File Offset: 0x0002F4D4
	private UniTask QuitTask()
	{
		Button_QuitGame.<QuitTask>d__5 <QuitTask>d__;
		<QuitTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<QuitTask>d__.<>4__this = this;
		<QuitTask>d__.<>1__state = -1;
		<QuitTask>d__.<>t__builder.Start<Button_QuitGame.<QuitTask>d__5>(ref <QuitTask>d__);
		return <QuitTask>d__.<>t__builder.Task;
	}

	// Token: 0x040009DF RID: 2527
	[SerializeField]
	private Button button;

	// Token: 0x040009E0 RID: 2528
	[SerializeField]
	private ConfirmDialogue dialogue;

	// Token: 0x040009E1 RID: 2529
	private UniTask task;
}
