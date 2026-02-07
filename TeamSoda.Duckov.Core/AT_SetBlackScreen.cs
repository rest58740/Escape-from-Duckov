using System;
using Cysharp.Threading.Tasks;
using Duckov.UI;
using NodeCanvas.Framework;

// Token: 0x020001BB RID: 443
public class AT_SetBlackScreen : ActionTask
{
	// Token: 0x06000D5B RID: 3419 RVA: 0x00038292 File Offset: 0x00036492
	protected override void OnExecute()
	{
		if (this.show)
		{
			this.task = BlackScreen.ShowAndReturnTask(null, 0f, 0.5f);
			return;
		}
		this.task = BlackScreen.HideAndReturnTask(null, 0f, 0.5f);
	}

	// Token: 0x06000D5C RID: 3420 RVA: 0x000382C9 File Offset: 0x000364C9
	protected override void OnUpdate()
	{
		if (this.task.Status != UniTaskStatus.Pending)
		{
			base.EndAction();
		}
	}

	// Token: 0x04000B91 RID: 2961
	public bool show;

	// Token: 0x04000B92 RID: 2962
	private UniTask task;
}
