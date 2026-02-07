using System;
using System.Collections.Generic;
using Duckov.Quests.Tasks;
using Duckov.Scenes;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.Quests
{
	// Token: 0x02000357 RID: 855
	public class SpawnPrefabForTask : MonoBehaviour
	{
		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001D96 RID: 7574 RVA: 0x0006B213 File Offset: 0x00069413
		private Task task
		{
			get
			{
				if (this._taskCache == null)
				{
					this._taskCache = base.GetComponent<Task>();
				}
				return this._taskCache;
			}
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x0006B235 File Offset: 0x00069435
		private void Awake()
		{
			SceneLoader.onFinishedLoadingScene += this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x0006B259 File Offset: 0x00069459
		private void Start()
		{
			this.SpawnIfNeeded();
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x0006B261 File Offset: 0x00069461
		private void OnDestroy()
		{
			SceneLoader.onFinishedLoadingScene -= this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x0006B285 File Offset: 0x00069485
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Spawning prefabs for task";
			this.SpawnIfNeeded();
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x0006B297 File Offset: 0x00069497
		private void OnFinishedLoadingScene(SceneLoadingContext context)
		{
			this.SpawnIfNeeded();
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x0006B2A0 File Offset: 0x000694A0
		private void SpawnIfNeeded()
		{
			if (this.prefab == null)
			{
				return;
			}
			if (this.task == null)
			{
				Debug.LogWarning("未配置Task");
				return;
			}
			if (this.task.IsFinished())
			{
				return;
			}
			if (this.IsSpawned())
			{
				return;
			}
			this.Spawn();
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001D9D RID: 7581 RVA: 0x0006B2F4 File Offset: 0x000694F4
		private int SpawnKey
		{
			get
			{
				return string.Format("{0}/{1}/{2}/{3}", new object[]
				{
					"SpawnPrefabForTask",
					this.task.Master.ID,
					this.task.ID,
					this.componentID
				}).GetHashCode();
			}
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x0006B354 File Offset: 0x00069554
		private bool IsSpawned()
		{
			object obj;
			return this.spawned || (!(MultiSceneCore.Instance == null) && MultiSceneCore.Instance.inLevelData.TryGetValue(this.SpawnKey, out obj) && obj is bool && (bool)obj);
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x0006B3A8 File Offset: 0x000695A8
		private void Spawn()
		{
			Vector3 vector;
			if (!this.locations.GetRandom<MultiSceneLocation>().TryGetLocationPosition(out vector))
			{
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.prefab, vector, Quaternion.identity);
			QuestTask_TaskEvent questTask_TaskEvent = this.task as QuestTask_TaskEvent;
			if (questTask_TaskEvent)
			{
				TaskEventEmitter component = gameObject.GetComponent<TaskEventEmitter>();
				if (component)
				{
					component.SetKey(questTask_TaskEvent.EventKey);
				}
			}
			if (MultiSceneCore.Instance)
			{
				MultiSceneCore.MoveToActiveWithScene(gameObject, base.transform.gameObject.scene.buildIndex);
				MultiSceneCore.Instance.inLevelData[this.SpawnKey] = true;
			}
			this.spawned = true;
		}

		// Token: 0x04001498 RID: 5272
		[SerializeField]
		private string componentID = "SpawnPrefabForTask";

		// Token: 0x04001499 RID: 5273
		private Task _taskCache;

		// Token: 0x0400149A RID: 5274
		[SerializeField]
		private List<MultiSceneLocation> locations;

		// Token: 0x0400149B RID: 5275
		[SerializeField]
		private GameObject prefab;

		// Token: 0x0400149C RID: 5276
		private bool spawned;
	}
}
