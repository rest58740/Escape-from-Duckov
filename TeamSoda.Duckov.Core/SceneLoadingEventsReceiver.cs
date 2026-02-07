using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000132 RID: 306
public class SceneLoadingEventsReceiver : MonoBehaviour
{
	// Token: 0x06000A32 RID: 2610 RVA: 0x0002C39A File Offset: 0x0002A59A
	private void OnEnable()
	{
		SceneLoader.onStartedLoadingScene += this.OnStartedLoadingScene;
		SceneLoader.onFinishedLoadingScene += this.OnFinishedLoadingScene;
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x0002C3BE File Offset: 0x0002A5BE
	private void OnDisable()
	{
		SceneLoader.onStartedLoadingScene -= this.OnStartedLoadingScene;
		SceneLoader.onFinishedLoadingScene -= this.OnFinishedLoadingScene;
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x0002C3E2 File Offset: 0x0002A5E2
	private void OnStartedLoadingScene(SceneLoadingContext context)
	{
		UnityEvent unityEvent = this.onStartLoadingScene;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x0002C3F4 File Offset: 0x0002A5F4
	private void OnFinishedLoadingScene(SceneLoadingContext context)
	{
		UnityEvent unityEvent = this.onFinishedLoadingScene;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x040008F7 RID: 2295
	public UnityEvent onStartLoadingScene;

	// Token: 0x040008F8 RID: 2296
	public UnityEvent onFinishedLoadingScene;
}
