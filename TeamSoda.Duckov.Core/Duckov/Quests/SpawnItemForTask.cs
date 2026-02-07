using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.Quests
{
	// Token: 0x02000356 RID: 854
	public class SpawnItemForTask : MonoBehaviour
	{
		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001D89 RID: 7561 RVA: 0x0006AF68 File Offset: 0x00069168
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

		// Token: 0x06001D8A RID: 7562 RVA: 0x0006AF8A File Offset: 0x0006918A
		private void Awake()
		{
			SceneLoader.onFinishedLoadingScene += this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x0006AFAE File Offset: 0x000691AE
		private void Start()
		{
			this.SpawnIfNeeded();
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x0006AFB6 File Offset: 0x000691B6
		private void OnDestroy()
		{
			SceneLoader.onFinishedLoadingScene -= this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x0006AFDA File Offset: 0x000691DA
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Spawning item for task";
			this.SpawnIfNeeded();
		}

		// Token: 0x06001D8E RID: 7566 RVA: 0x0006AFEC File Offset: 0x000691EC
		private void OnFinishedLoadingScene(SceneLoadingContext context)
		{
			this.SpawnIfNeeded();
		}

		// Token: 0x06001D8F RID: 7567 RVA: 0x0006AFF4 File Offset: 0x000691F4
		private void SpawnIfNeeded()
		{
			if (this.itemID < 0)
			{
				return;
			}
			if (this.task == null)
			{
				Debug.Log("spawn item task is null");
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

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001D90 RID: 7568 RVA: 0x0006B044 File Offset: 0x00069244
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

		// Token: 0x06001D91 RID: 7569 RVA: 0x0006B0A4 File Offset: 0x000692A4
		private bool IsSpawned()
		{
			object obj;
			return this.spawned || (!(MultiSceneCore.Instance == null) && MultiSceneCore.Instance.inLevelData.TryGetValue(this.SpawnKey, out obj) && obj is bool && (bool)obj);
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x0006B0F8 File Offset: 0x000692F8
		private void Spawn()
		{
			MultiSceneLocation random = this.locations.GetRandom<MultiSceneLocation>();
			Vector3 pos;
			if (!random.TryGetLocationPosition(out pos))
			{
				return;
			}
			if (MultiSceneCore.Instance)
			{
				MultiSceneCore.Instance.inLevelData[this.SpawnKey] = true;
			}
			this.spawned = true;
			this.SpawnItem(pos, base.transform.gameObject.scene, random).Forget();
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x0006B16C File Offset: 0x0006936C
		private UniTaskVoid SpawnItem(Vector3 pos, Scene scene, MultiSceneLocation location)
		{
			SpawnItemForTask.<SpawnItem>d__18 <SpawnItem>d__;
			<SpawnItem>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<SpawnItem>d__.<>4__this = this;
			<SpawnItem>d__.pos = pos;
			<SpawnItem>d__.location = location;
			<SpawnItem>d__.<>1__state = -1;
			<SpawnItem>d__.<>t__builder.Start<SpawnItemForTask.<SpawnItem>d__18>(ref <SpawnItem>d__);
			return <SpawnItem>d__.<>t__builder.Task;
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x0006B1BF File Offset: 0x000693BF
		private void OnItemTreeChanged(Item selfItem)
		{
			if (this.mapElement && selfItem.ParentItem)
			{
				this.mapElement.SetVisibility(false);
				selfItem.onItemTreeChanged -= this.OnItemTreeChanged;
			}
		}

		// Token: 0x04001492 RID: 5266
		[SerializeField]
		private string componentID = "SpawnItemForTask";

		// Token: 0x04001493 RID: 5267
		private Task _taskCache;

		// Token: 0x04001494 RID: 5268
		[SerializeField]
		private List<MultiSceneLocation> locations;

		// Token: 0x04001495 RID: 5269
		[ItemTypeID]
		[SerializeField]
		private int itemID = -1;

		// Token: 0x04001496 RID: 5270
		[SerializeField]
		private MapElementForTask mapElement;

		// Token: 0x04001497 RID: 5271
		private bool spawned;
	}
}
