using System;
using UnityEngine;

// Token: 0x02000111 RID: 273
public class PlayerPositionBackupProxy : MonoBehaviour
{
	// Token: 0x06000980 RID: 2432 RVA: 0x0002AAF6 File Offset: 0x00028CF6
	public void StartRecoverInteract()
	{
		PauseMenu.Instance.Close();
		PlayerPositionBackupManager.StartRecover();
	}
}
