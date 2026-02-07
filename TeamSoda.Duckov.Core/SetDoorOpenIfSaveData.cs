using System;
using Saves;
using UnityEngine;

// Token: 0x020000E4 RID: 228
public class SetDoorOpenIfSaveData : MonoBehaviour
{
	// Token: 0x0600075C RID: 1884 RVA: 0x000213A2 File Offset: 0x0001F5A2
	private void Start()
	{
		if (LevelManager.LevelInited)
		{
			this.OnSet();
			return;
		}
		LevelManager.OnLevelInitialized += this.OnSet;
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x000213C3 File Offset: 0x0001F5C3
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.OnSet;
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x000213D8 File Offset: 0x0001F5D8
	private void OnSet()
	{
		bool flag = SavesSystem.Load<bool>(this.key);
		Debug.Log(string.Format("Load door data:{0}  {1}", this.key, flag));
		this.door.ForceSetClosed(flag != this.openIfDataTure, false);
	}

	// Token: 0x0400070B RID: 1803
	public Door door;

	// Token: 0x0400070C RID: 1804
	public string key;

	// Token: 0x0400070D RID: 1805
	public bool openIfDataTure = true;
}
