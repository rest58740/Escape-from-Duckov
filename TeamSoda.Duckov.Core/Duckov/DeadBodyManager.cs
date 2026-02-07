using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Rules;
using Duckov.Scenes;
using ItemStatsSystem.Data;
using Saves;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov
{
	// Token: 0x0200024A RID: 586
	public class DeadBodyManager : MonoBehaviour
	{
		// Token: 0x17000338 RID: 824
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x000473DF File Offset: 0x000455DF
		// (set) Token: 0x0600126D RID: 4717 RVA: 0x000473E6 File Offset: 0x000455E6
		public static DeadBodyManager Instance { get; private set; }

		// Token: 0x0600126E RID: 4718 RVA: 0x000473EE File Offset: 0x000455EE
		private void AppendDeathInfo(DeadBodyManager.DeathInfo deathInfo)
		{
			while (this.deaths.Count >= GameRulesManager.Current.SaveDeadbodyCount)
			{
				this.deaths.RemoveAt(0);
			}
			this.deaths.Add(deathInfo);
			this.Save();
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x00047427 File Offset: 0x00045627
		private static List<DeadBodyManager.DeathInfo> LoadDeathInfos()
		{
			return SavesSystem.Load<List<DeadBodyManager.DeathInfo>>("DeathList");
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x00047434 File Offset: 0x00045634
		internal static void RecordDeath(CharacterMainControl mainCharacter)
		{
			if (DeadBodyManager.Instance == null)
			{
				Debug.LogError("DeadBodyManager Instance is null");
				return;
			}
			DeadBodyManager.DeathInfo deathInfo = new DeadBodyManager.DeathInfo();
			deathInfo.valid = true;
			deathInfo.raidID = RaidUtilities.CurrentRaid.ID;
			deathInfo.subSceneID = MultiSceneCore.ActiveSubSceneID;
			deathInfo.worldPosition = mainCharacter.transform.position;
			deathInfo.itemTreeData = ItemTreeData.FromItem(mainCharacter.CharacterItem);
			DeadBodyManager.Instance.AppendDeathInfo(deathInfo);
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x000474B0 File Offset: 0x000456B0
		private void Awake()
		{
			DeadBodyManager.Instance = this;
			LevelManager.OnLevelBeginInitializing += this.LevelInitialize;
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
			this.deaths.Clear();
			List<DeadBodyManager.DeathInfo> list = DeadBodyManager.LoadDeathInfos();
			if (list != null)
			{
				this.deaths.AddRange(list);
			}
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x00047516 File Offset: 0x00045716
		private void OnDestroy()
		{
			LevelManager.OnLevelBeginInitializing -= this.LevelInitialize;
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x0004754B File Offset: 0x0004574B
		private void LevelInitialize()
		{
			this.spawnedBodies.Clear();
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x00047558 File Offset: 0x00045758
		private void Save()
		{
			SavesSystem.Save<List<DeadBodyManager.DeathInfo>>("DeathList", this.deaths);
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x0004756C File Offset: 0x0004576C
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Spawning bodies";
			if (!LevelConfig.SpawnTomb)
			{
				return;
			}
			foreach (DeadBodyManager.DeathInfo info in this.deaths)
			{
				if (this.ShouldSpawnDeadBody(info))
				{
					this.SpawnDeadBody(info).Forget();
				}
			}
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x000475E0 File Offset: 0x000457E0
		private UniTask SpawnDeadBody(DeadBodyManager.DeathInfo info)
		{
			DeadBodyManager.<SpawnDeadBody>d__15 <SpawnDeadBody>d__;
			<SpawnDeadBody>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SpawnDeadBody>d__.<>4__this = this;
			<SpawnDeadBody>d__.info = info;
			<SpawnDeadBody>d__.<>1__state = -1;
			<SpawnDeadBody>d__.<>t__builder.Start<DeadBodyManager.<SpawnDeadBody>d__15>(ref <SpawnDeadBody>d__);
			return <SpawnDeadBody>d__.<>t__builder.Task;
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x0004762B File Offset: 0x0004582B
		private static void NotifyDeadbodyTouched(DeadBodyManager.DeathInfo info)
		{
			if (DeadBodyManager.Instance == null)
			{
				return;
			}
			DeadBodyManager.Instance.OnDeadbodyTouched(info);
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x00047648 File Offset: 0x00045848
		private void OnDeadbodyTouched(DeadBodyManager.DeathInfo info)
		{
			DeadBodyManager.DeathInfo deathInfo = this.deaths.Find((DeadBodyManager.DeathInfo e) => e.raidID == info.raidID);
			if (deathInfo == null)
			{
				return;
			}
			deathInfo.touched = true;
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00047688 File Offset: 0x00045888
		private bool ShouldSpawnDeadBody(DeadBodyManager.DeathInfo info)
		{
			return info != null && GameRulesManager.Current.SpawnDeadBody && LevelManager.Instance && LevelManager.Instance.IsRaidMap && !this.spawnedBodies.Contains(info) && info != null && info.valid && !info.touched && !(MultiSceneCore.ActiveSubSceneID != info.subSceneID);
		}

		// Token: 0x04000E30 RID: 3632
		private List<DeadBodyManager.DeathInfo> deaths = new List<DeadBodyManager.DeathInfo>();

		// Token: 0x04000E31 RID: 3633
		private List<DeadBodyManager.DeathInfo> spawnedBodies = new List<DeadBodyManager.DeathInfo>();

		// Token: 0x0200054B RID: 1355
		[Serializable]
		public class DeathInfo
		{
			// Token: 0x04001F45 RID: 8005
			public bool valid;

			// Token: 0x04001F46 RID: 8006
			public uint raidID;

			// Token: 0x04001F47 RID: 8007
			public string subSceneID;

			// Token: 0x04001F48 RID: 8008
			public Vector3 worldPosition;

			// Token: 0x04001F49 RID: 8009
			public ItemTreeData itemTreeData;

			// Token: 0x04001F4A RID: 8010
			public bool spawned;

			// Token: 0x04001F4B RID: 8011
			public bool touched;
		}
	}
}
