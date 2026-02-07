using System;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x02000117 RID: 279
public class LevelManagerProxy : MonoBehaviour
{
	// Token: 0x060009A2 RID: 2466 RVA: 0x0002B0E7 File Offset: 0x000292E7
	public void NotifyEvacuated()
	{
		LevelManager instance = LevelManager.Instance;
		if (instance == null)
		{
			return;
		}
		instance.NotifyEvacuated(new EvacuationInfo(MultiSceneCore.ActiveSubSceneID, base.transform.position));
	}
}
