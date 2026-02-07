using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Rules.UI;
using Duckov.Scenes;
using Eflatun.SceneReference;
using UnityEngine;

// Token: 0x020001EF RID: 495
public class GamePrepareProcess : MonoBehaviour
{
	// Token: 0x06000ED7 RID: 3799 RVA: 0x0003C2E4 File Offset: 0x0003A4E4
	private UniTask Execute()
	{
		GamePrepareProcess.<Execute>d__6 <Execute>d__;
		<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Execute>d__.<>4__this = this;
		<Execute>d__.<>1__state = -1;
		<Execute>d__.<>t__builder.Start<GamePrepareProcess.<Execute>d__6>(ref <Execute>d__);
		return <Execute>d__.<>t__builder.Task;
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x0003C327 File Offset: 0x0003A527
	private void Start()
	{
		this.Execute().Forget();
	}

	// Token: 0x04000C4A RID: 3146
	[SerializeField]
	private DifficultySelection difficultySelection;

	// Token: 0x04000C4B RID: 3147
	[SerializeField]
	[SceneID]
	private string introScene;

	// Token: 0x04000C4C RID: 3148
	[SerializeField]
	[SceneID]
	private string guideScene;

	// Token: 0x04000C4D RID: 3149
	public bool goToBaseSceneIfVisted;

	// Token: 0x04000C4E RID: 3150
	[SerializeField]
	[SceneID]
	private string baseScene;

	// Token: 0x04000C4F RID: 3151
	public SceneReference overrideCurtainScene;
}
