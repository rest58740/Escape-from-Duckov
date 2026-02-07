using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Duckov.MiniMaps
{
	// Token: 0x02000281 RID: 641
	public class MiniMapCenter : MonoBehaviour
	{
		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001465 RID: 5221 RVA: 0x0004C4E4 File Offset: 0x0004A6E4
		public float WorldSize
		{
			get
			{
				return this.worldSize;
			}
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0004C4EC File Offset: 0x0004A6EC
		private void OnEnable()
		{
			MiniMapCenter.activeMiniMapCenters.Add(this);
			if (MiniMapCenter.activeMiniMapCenters.Count > 1)
			{
				if (MiniMapCenter.activeMiniMapCenters.Find((MiniMapCenter e) => e != null && e != this && e.gameObject.scene.buildIndex == base.gameObject.scene.buildIndex))
				{
					Debug.LogError("场景 " + base.gameObject.scene.name + " 似乎存在两个MiniMapCenter！");
				}
				return;
			}
			this.CacheThisCenter();
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0004C55C File Offset: 0x0004A75C
		private void CacheThisCenter()
		{
			MiniMapSettings instance = MiniMapSettings.Instance;
			if (instance == null)
			{
				return;
			}
			Vector3 position = base.transform.position;
			instance.Cache(this);
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x0004C58C File Offset: 0x0004A78C
		private void OnDisable()
		{
			MiniMapCenter.activeMiniMapCenters.Remove(this);
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x0004C59C File Offset: 0x0004A79C
		internal static Vector3 GetCenterOfObjectScene(MonoBehaviour target)
		{
			int sceneBuildIndex = target.gameObject.scene.buildIndex;
			IPointOfInterest pointOfInterest = target as IPointOfInterest;
			if (pointOfInterest != null && pointOfInterest.OverrideScene >= 0)
			{
				sceneBuildIndex = pointOfInterest.OverrideScene;
			}
			return MiniMapCenter.GetCenter(sceneBuildIndex);
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0004C5E0 File Offset: 0x0004A7E0
		internal static string GetSceneID(MonoBehaviour target)
		{
			int sceneBuildIndex = target.gameObject.scene.buildIndex;
			IPointOfInterest pointOfInterest = target as IPointOfInterest;
			if (pointOfInterest != null && pointOfInterest.OverrideScene >= 0)
			{
				sceneBuildIndex = pointOfInterest.OverrideScene;
			}
			MiniMapSettings instance = MiniMapSettings.Instance;
			if (instance == null)
			{
				return null;
			}
			MiniMapSettings.MapEntry mapEntry = instance.maps.Find((MiniMapSettings.MapEntry e) => e.SceneReference.UnsafeReason == null && e.SceneReference.BuildIndex == sceneBuildIndex);
			if (mapEntry == null)
			{
				return null;
			}
			return mapEntry.sceneID;
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0004C660 File Offset: 0x0004A860
		internal static Vector3 GetCenter(int sceneBuildIndex)
		{
			MiniMapSettings instance = MiniMapSettings.Instance;
			if (instance == null)
			{
				return Vector3.zero;
			}
			MiniMapSettings.MapEntry mapEntry = instance.maps.FirstOrDefault((MiniMapSettings.MapEntry e) => e.SceneReference.UnsafeReason == null && e.SceneReference.BuildIndex == sceneBuildIndex);
			if (mapEntry != null)
			{
				return mapEntry.mapWorldCenter;
			}
			return instance.combinedCenter;
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x0004C6B7 File Offset: 0x0004A8B7
		internal static Vector3 GetCenter(string sceneID)
		{
			return MiniMapCenter.GetCenter(SceneInfoCollection.GetBuildIndex(sceneID));
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0004C6C4 File Offset: 0x0004A8C4
		internal static Vector3 GetCombinedCenter()
		{
			MiniMapSettings instance = MiniMapSettings.Instance;
			if (instance == null)
			{
				return Vector3.zero;
			}
			return instance.combinedCenter;
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0004C6EC File Offset: 0x0004A8EC
		private void OnDrawGizmosSelected()
		{
			if (this.WorldSize < 0f)
			{
				return;
			}
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(this.WorldSize, 1f, this.WorldSize));
		}

		// Token: 0x04000F23 RID: 3875
		private static List<MiniMapCenter> activeMiniMapCenters = new List<MiniMapCenter>();

		// Token: 0x04000F24 RID: 3876
		[SerializeField]
		private float worldSize = -1f;
	}
}
