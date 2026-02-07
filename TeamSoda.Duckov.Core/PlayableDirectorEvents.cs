using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

// Token: 0x02000141 RID: 321
public class PlayableDirectorEvents : MonoBehaviour
{
	// Token: 0x06000A77 RID: 2679 RVA: 0x0002D3D0 File Offset: 0x0002B5D0
	private void OnEnable()
	{
		this.playableDirector.played += this.OnPlayed;
		this.playableDirector.paused += this.OnPaused;
		this.playableDirector.stopped += this.OnStopped;
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x0002D424 File Offset: 0x0002B624
	private void OnDisable()
	{
		this.playableDirector.played -= this.OnPlayed;
		this.playableDirector.paused -= this.OnPaused;
		this.playableDirector.stopped -= this.OnStopped;
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x0002D476 File Offset: 0x0002B676
	private void OnStopped(PlayableDirector director)
	{
		UnityEvent unityEvent = this.onStopped;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x0002D488 File Offset: 0x0002B688
	private void OnPaused(PlayableDirector director)
	{
		UnityEvent unityEvent = this.onPaused;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x0002D49A File Offset: 0x0002B69A
	private void OnPlayed(PlayableDirector director)
	{
		UnityEvent unityEvent = this.onPlayed;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x04000938 RID: 2360
	[SerializeField]
	private PlayableDirector playableDirector;

	// Token: 0x04000939 RID: 2361
	[SerializeField]
	private UnityEvent onPlayed;

	// Token: 0x0400093A RID: 2362
	[SerializeField]
	private UnityEvent onPaused;

	// Token: 0x0400093B RID: 2363
	[SerializeField]
	private UnityEvent onStopped;
}
