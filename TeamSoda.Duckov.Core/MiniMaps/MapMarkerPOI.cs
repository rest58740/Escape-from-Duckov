using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.MiniMaps
{
	// Token: 0x02000280 RID: 640
	public class MapMarkerPOI : MonoBehaviour, IPointOfInterest
	{
		// Token: 0x170003AD RID: 941
		// (get) Token: 0x0600145A RID: 5210 RVA: 0x0004C3F5 File Offset: 0x0004A5F5
		public MapMarkerPOI.RuntimeData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x0004C3FD File Offset: 0x0004A5FD
		public Sprite Icon
		{
			get
			{
				return MapMarkerManager.GetIcon(this.data.iconName);
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x0600145C RID: 5212 RVA: 0x0004C40F File Offset: 0x0004A60F
		public int OverrideScene
		{
			get
			{
				return SceneInfoCollection.GetBuildIndex(this.data.overrideSceneKey);
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x0600145D RID: 5213 RVA: 0x0004C421 File Offset: 0x0004A621
		public Color Color
		{
			get
			{
				return this.data.color;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x0004C42E File Offset: 0x0004A62E
		public Color ShadowColor
		{
			get
			{
				return Color.black;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x0600145F RID: 5215 RVA: 0x0004C435 File Offset: 0x0004A635
		public float ScaleFactor
		{
			get
			{
				return 0.8f;
			}
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0004C43C File Offset: 0x0004A63C
		public void Setup(Vector3 worldPosition, string iconName = "", string overrideScene = "", Color? color = null)
		{
			this.data = new MapMarkerPOI.RuntimeData
			{
				worldPosition = worldPosition,
				iconName = iconName,
				overrideSceneKey = overrideScene,
				color = ((color == null) ? Color.white : color.Value)
			};
			base.transform.position = worldPosition;
			PointsOfInterests.Unregister(this);
			PointsOfInterests.Register(this);
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0004C4A6 File Offset: 0x0004A6A6
		public void Setup(MapMarkerPOI.RuntimeData data)
		{
			this.data = data;
			base.transform.position = data.worldPosition;
			PointsOfInterests.Unregister(this);
			PointsOfInterests.Register(this);
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0004C4CC File Offset: 0x0004A6CC
		public void NotifyClicked(PointerEventData eventData)
		{
			MapMarkerManager.Release(this);
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0004C4D4 File Offset: 0x0004A6D4
		private void OnDestroy()
		{
			PointsOfInterests.Unregister(this);
		}

		// Token: 0x04000F22 RID: 3874
		[SerializeField]
		private MapMarkerPOI.RuntimeData data;

		// Token: 0x02000567 RID: 1383
		[Serializable]
		public struct RuntimeData
		{
			// Token: 0x04001FA5 RID: 8101
			public Vector3 worldPosition;

			// Token: 0x04001FA6 RID: 8102
			public string iconName;

			// Token: 0x04001FA7 RID: 8103
			public string overrideSceneKey;

			// Token: 0x04001FA8 RID: 8104
			public Color color;
		}
	}
}
