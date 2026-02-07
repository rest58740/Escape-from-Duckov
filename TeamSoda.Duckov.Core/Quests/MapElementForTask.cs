using System;
using System.Collections.Generic;
using Duckov.MiniMaps;
using Duckov.Scenes;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000353 RID: 851
	public class MapElementForTask : MonoBehaviour
	{
		// Token: 0x06001D74 RID: 7540 RVA: 0x0006AA91 File Offset: 0x00068C91
		public void SetVisibility(bool _visable)
		{
			if (this.visable == _visable)
			{
				return;
			}
			this.visable = _visable;
			if (MultiSceneCore.Instance == null)
			{
				LevelManager.OnLevelInitialized += this.OnLevelInitialized;
				return;
			}
			this.SyncVisibility();
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x0006AAC9 File Offset: 0x00068CC9
		private void OnLevelInitialized()
		{
			this.SyncVisibility();
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x0006AAD1 File Offset: 0x00068CD1
		private void OnDestroy()
		{
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x0006AAE4 File Offset: 0x00068CE4
		private void OnDisable()
		{
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x0006AAF7 File Offset: 0x00068CF7
		private void SyncVisibility()
		{
			if (this.visable)
			{
				if (this.pointsInstance != null && this.pointsInstance.Count > 0)
				{
					this.DespawnAll();
				}
				this.Spawn();
				return;
			}
			this.DespawnAll();
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x0006AB2C File Offset: 0x00068D2C
		private void Spawn()
		{
			foreach (MultiSceneLocation location in this.locations)
			{
				this.SpawnOnePoint(location, this.name);
			}
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x0006AB88 File Offset: 0x00068D88
		private void SpawnOnePoint(MultiSceneLocation _location, string name)
		{
			if (this.pointsInstance == null)
			{
				this.pointsInstance = new List<SimplePointOfInterest>();
			}
			if (MultiSceneCore.Instance == null)
			{
				return;
			}
			Vector3 vector;
			if (!_location.TryGetLocationPosition(out vector))
			{
				return;
			}
			SimplePointOfInterest simplePointOfInterest = new GameObject("MapElement:" + name).AddComponent<SimplePointOfInterest>();
			Debug.Log("Spawning " + simplePointOfInterest.name + " for task", this);
			simplePointOfInterest.Color = this.iconColor;
			simplePointOfInterest.ShadowColor = this.shadowColor;
			simplePointOfInterest.ShadowDistance = this.shadowDistance;
			if (this.range > 0f)
			{
				simplePointOfInterest.IsArea = true;
				simplePointOfInterest.AreaRadius = this.range;
			}
			simplePointOfInterest.Setup(this.icon, name, false, null);
			simplePointOfInterest.SetupMultiSceneLocation(_location, true);
			this.pointsInstance.Add(simplePointOfInterest);
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x0006AC5C File Offset: 0x00068E5C
		public void DespawnAll()
		{
			if (this.pointsInstance == null || this.pointsInstance.Count == 0)
			{
				return;
			}
			foreach (SimplePointOfInterest simplePointOfInterest in this.pointsInstance)
			{
				Object.Destroy(simplePointOfInterest.gameObject);
			}
			this.pointsInstance.Clear();
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x0006ACD4 File Offset: 0x00068ED4
		public void DespawnPoint(SimplePointOfInterest point)
		{
			if (this.pointsInstance != null && this.pointsInstance.Contains(point))
			{
				this.pointsInstance.Remove(point);
			}
			Object.Destroy(point.gameObject);
		}

		// Token: 0x04001484 RID: 5252
		private bool visable;

		// Token: 0x04001485 RID: 5253
		public string name;

		// Token: 0x04001486 RID: 5254
		public List<MultiSceneLocation> locations;

		// Token: 0x04001487 RID: 5255
		public float range;

		// Token: 0x04001488 RID: 5256
		private List<SimplePointOfInterest> pointsInstance;

		// Token: 0x04001489 RID: 5257
		public Sprite icon;

		// Token: 0x0400148A RID: 5258
		public Color iconColor = Color.white;

		// Token: 0x0400148B RID: 5259
		public Color shadowColor = Color.white;

		// Token: 0x0400148C RID: 5260
		public float shadowDistance;
	}
}
