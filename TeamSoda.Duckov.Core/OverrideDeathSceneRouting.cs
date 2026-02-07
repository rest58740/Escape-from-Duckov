using System;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x0200013A RID: 314
public class OverrideDeathSceneRouting : MonoBehaviour
{
	// Token: 0x17000223 RID: 547
	// (get) Token: 0x06000A4E RID: 2638 RVA: 0x0002CABD File Offset: 0x0002ACBD
	// (set) Token: 0x06000A4F RID: 2639 RVA: 0x0002CAC4 File Offset: 0x0002ACC4
	public static OverrideDeathSceneRouting Instance { get; private set; }

	// Token: 0x06000A50 RID: 2640 RVA: 0x0002CACC File Offset: 0x0002ACCC
	private void OnEnable()
	{
		if (OverrideDeathSceneRouting.Instance != null)
		{
			Debug.LogError("存在多个OverrideDeathSceneRouting实例");
		}
		OverrideDeathSceneRouting.Instance = this;
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x0002CAEB File Offset: 0x0002ACEB
	private void OnDisable()
	{
		if (OverrideDeathSceneRouting.Instance == this)
		{
			OverrideDeathSceneRouting.Instance = null;
		}
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x0002CB00 File Offset: 0x0002AD00
	public string GetSceneID()
	{
		return this.sceneID;
	}

	// Token: 0x04000923 RID: 2339
	[SceneID]
	[SerializeField]
	private string sceneID;
}
