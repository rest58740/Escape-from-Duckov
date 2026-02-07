using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using Eflatun.SceneReference;
using UnityEngine;

// Token: 0x02000118 RID: 280
public class SceneLoaderProxy : MonoBehaviour
{
	// Token: 0x060009A4 RID: 2468 RVA: 0x0002B115 File Offset: 0x00029315
	public void LoadScene()
	{
		if (SceneLoader.Instance == null)
		{
			Debug.LogWarning("没找到SceneLoader实例，已取消加载场景");
			return;
		}
		InputManager.DisableInput(base.gameObject);
		this.Task().Forget();
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x0002B148 File Offset: 0x00029348
	private UniTask Task()
	{
		SceneLoaderProxy.<Task>d__10 <Task>d__;
		<Task>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Task>d__.<>4__this = this;
		<Task>d__.<>1__state = -1;
		<Task>d__.<>t__builder.Start<SceneLoaderProxy.<Task>d__10>(ref <Task>d__);
		return <Task>d__.<>t__builder.Task;
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x0002B18B File Offset: 0x0002938B
	public void LoadMainMenu()
	{
		SceneLoader.LoadMainMenu(this.circleFade);
	}

	// Token: 0x040008AA RID: 2218
	[SceneID]
	[SerializeField]
	private string sceneID;

	// Token: 0x040008AB RID: 2219
	[SerializeField]
	private bool useLocation;

	// Token: 0x040008AC RID: 2220
	[SerializeField]
	private MultiSceneLocation location;

	// Token: 0x040008AD RID: 2221
	[SerializeField]
	private bool showClosure = true;

	// Token: 0x040008AE RID: 2222
	[SerializeField]
	private bool notifyEvacuation = true;

	// Token: 0x040008AF RID: 2223
	[SerializeField]
	private SceneReference overrideCurtainScene;

	// Token: 0x040008B0 RID: 2224
	[SerializeField]
	private bool hideTips;

	// Token: 0x040008B1 RID: 2225
	[SerializeField]
	private bool circleFade = true;

	// Token: 0x040008B2 RID: 2226
	private bool saveToFile;
}
