using System;
using System.Collections.Generic;
using Duckov.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000110 RID: 272
public class PlayerPositionBackupManager : MonoBehaviour
{
	// Token: 0x14000048 RID: 72
	// (add) Token: 0x06000974 RID: 2420 RVA: 0x0002A774 File Offset: 0x00028974
	// (remove) Token: 0x06000975 RID: 2421 RVA: 0x0002A7A8 File Offset: 0x000289A8
	private static event Action OnStartRecoverEvent;

	// Token: 0x06000976 RID: 2422 RVA: 0x0002A7DB File Offset: 0x000289DB
	private void Awake()
	{
		this.backups = new List<PlayerPositionBackupManager.PlayerPositionBackupEntry>();
		MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
		PlayerPositionBackupManager.OnStartRecoverEvent += this.OnStartRecover;
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x0002A80A File Offset: 0x00028A0A
	private void OnDestroy()
	{
		MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
		PlayerPositionBackupManager.OnStartRecoverEvent -= this.OnStartRecover;
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x0002A830 File Offset: 0x00028A30
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (!this.mainCharacter)
		{
			this.mainCharacter = CharacterMainControl.Main;
		}
		if (!this.mainCharacter)
		{
			return;
		}
		this.backupTimer -= Time.deltaTime;
		if (this.backupTimer < 0f && this.CheckCanBackup())
		{
			this.BackupCurrentPos();
		}
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x0002A898 File Offset: 0x00028A98
	private bool CheckCanBackup()
	{
		if (!this.mainCharacter)
		{
			return false;
		}
		if (!this.mainCharacter.IsOnGround)
		{
			return false;
		}
		if (Mathf.Abs(this.mainCharacter.Velocity.y) > 2f)
		{
			return false;
		}
		int count = this.backups.Count;
		if (count > 0)
		{
			Vector3 position = this.backups[count - 1].position;
			if (Vector3.Distance(this.mainCharacter.transform.position, position) < this.minBackupDistance)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x0002A926 File Offset: 0x00028B26
	private void OnSubSceneLoaded(MultiSceneCore multiSceneCore, Scene scene)
	{
		this.backups.Clear();
		this.backupTimer = this.backupTimeSpace;
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x0002A940 File Offset: 0x00028B40
	public void BackupCurrentPos()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (!this.mainCharacter)
		{
			return;
		}
		this.backupTimer = this.backupTimeSpace;
		PlayerPositionBackupManager.PlayerPositionBackupEntry item = default(PlayerPositionBackupManager.PlayerPositionBackupEntry);
		item.position = this.mainCharacter.transform.position;
		item.sceneID = SceneManager.GetActiveScene().buildIndex;
		this.backups.Add(item);
		if (this.backups.Count > this.listSize)
		{
			this.backups.RemoveAt(0);
		}
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x0002A9CD File Offset: 0x00028BCD
	public static void StartRecover()
	{
		Action onStartRecoverEvent = PlayerPositionBackupManager.OnStartRecoverEvent;
		if (onStartRecoverEvent == null)
		{
			return;
		}
		onStartRecoverEvent();
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x0002A9E0 File Offset: 0x00028BE0
	private void OnStartRecover()
	{
		if (this.mainCharacter.CurrentAction != null && this.mainCharacter.CurrentAction.Running)
		{
			this.mainCharacter.CurrentAction.StopAction();
		}
		this.mainCharacter.Interact(this.backupInteract);
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x0002AA34 File Offset: 0x00028C34
	public void SetPlayerToBackupPos()
	{
		if (this.backups.Count <= 0)
		{
			return;
		}
		int buildIndex = SceneManager.GetActiveScene().buildIndex;
		Vector3 position = this.mainCharacter.transform.position;
		ref PlayerPositionBackupManager.PlayerPositionBackupEntry ptr = this.backups[this.backups.Count - 1];
		this.backups.RemoveAt(this.backups.Count - 1);
		Vector3 position2 = ptr.position;
		if (Vector3.Distance(position, position2) > this.minBackupDistance)
		{
			this.mainCharacter.SetPosition(position2);
			return;
		}
		this.SetPlayerToBackupPos();
	}

	// Token: 0x04000891 RID: 2193
	private List<PlayerPositionBackupManager.PlayerPositionBackupEntry> backups;

	// Token: 0x04000892 RID: 2194
	private CharacterMainControl mainCharacter;

	// Token: 0x04000893 RID: 2195
	public float backupTimeSpace = 3f;

	// Token: 0x04000894 RID: 2196
	public float minBackupDistance = 3f;

	// Token: 0x04000895 RID: 2197
	private float backupTimer = 3f;

	// Token: 0x04000896 RID: 2198
	public InteractableBase backupInteract;

	// Token: 0x04000897 RID: 2199
	public int listSize = 20;

	// Token: 0x020004B1 RID: 1201
	private struct PlayerPositionBackupEntry
	{
		// Token: 0x04001CB4 RID: 7348
		public int sceneID;

		// Token: 0x04001CB5 RID: 7349
		public Vector3 position;
	}
}
