using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Cinemachine.Utility;
using Duckov.Scenes;
using Duckov.Utilities;
using UI_Spline_Renderer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Splines;

namespace Duckov.MiniMaps.UI
{
	// Token: 0x02000287 RID: 647
	public class MiniMapDisplay : MonoBehaviour, IScrollHandler, IEventSystemHandler
	{
		// Token: 0x060014B0 RID: 5296 RVA: 0x0004CED4 File Offset: 0x0004B0D4
		public bool NoSignal()
		{
			foreach (MiniMapDisplayEntry miniMapDisplayEntry in this.MapEntryPool.ActiveEntries)
			{
				if (!(miniMapDisplayEntry == null) && !(miniMapDisplayEntry.SceneID != MultiSceneCore.ActiveSubSceneID) && miniMapDisplayEntry.NoSignal())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x0004CF4C File Offset: 0x0004B14C
		private PrefabPool<MiniMapDisplayEntry> MapEntryPool
		{
			get
			{
				if (this._mapEntryPool == null)
				{
					this._mapEntryPool = new PrefabPool<MiniMapDisplayEntry>(this.mapDisplayEntryPrefab, base.transform, new Action<MiniMapDisplayEntry>(this.OnGetMapEntry), null, null, true, 10, 10000, null);
				}
				return this._mapEntryPool;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x060014B2 RID: 5298 RVA: 0x0004CF98 File Offset: 0x0004B198
		private PrefabPool<PointOfInterestEntry> PointOfInterestEntryPool
		{
			get
			{
				if (this._pointOfInterestEntryPool == null)
				{
					this._pointOfInterestEntryPool = new PrefabPool<PointOfInterestEntry>(this.pointOfInterestEntryPrefab, base.transform, new Action<PointOfInterestEntry>(this.OnGetPointOfInterestEntry), null, null, true, 10, 10000, null);
				}
				return this._pointOfInterestEntryPool;
			}
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x0004CFE1 File Offset: 0x0004B1E1
		private void OnGetPointOfInterestEntry(PointOfInterestEntry entry)
		{
			entry.gameObject.hideFlags |= 52;
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x0004CFF7 File Offset: 0x0004B1F7
		private void OnGetMapEntry(MiniMapDisplayEntry entry)
		{
			entry.gameObject.hideFlags |= 52;
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x0004D00D File Offset: 0x0004B20D
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponentInParent<MiniMapView>();
			}
			this.mapDisplayEntryPrefab.gameObject.SetActive(false);
			this.pointOfInterestEntryPrefab.gameObject.SetActive(false);
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x0004D04B File Offset: 0x0004B24B
		private void OnEnable()
		{
			if (this.autoSetupOnEnable)
			{
				this.AutoSetup();
			}
			this.RegisterEvents();
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x0004D061 File Offset: 0x0004B261
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x0004D069 File Offset: 0x0004B269
		private void RegisterEvents()
		{
			PointsOfInterests.OnPointRegistered += this.HandlePointOfInterest;
			PointsOfInterests.OnPointUnregistered += this.ReleasePointOfInterest;
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x0004D08D File Offset: 0x0004B28D
		private void UnregisterEvents()
		{
			PointsOfInterests.OnPointRegistered -= this.HandlePointOfInterest;
			PointsOfInterests.OnPointUnregistered -= this.ReleasePointOfInterest;
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x0004D0B4 File Offset: 0x0004B2B4
		internal void AutoSetup()
		{
			MiniMapSettings miniMapSettings = Object.FindAnyObjectByType<MiniMapSettings>();
			if (miniMapSettings)
			{
				this.Setup(miniMapSettings);
			}
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x0004D0D8 File Offset: 0x0004B2D8
		public void Setup(IMiniMapDataProvider dataProvider)
		{
			if (dataProvider == null)
			{
				return;
			}
			this.MapEntryPool.ReleaseAll();
			bool flag = dataProvider.CombinedSprite != null;
			foreach (IMiniMapEntry cur in dataProvider.Maps)
			{
				MiniMapDisplayEntry miniMapDisplayEntry = this.MapEntryPool.Get(null);
				miniMapDisplayEntry.Setup(this, cur, !flag);
				miniMapDisplayEntry.gameObject.SetActive(true);
			}
			if (flag)
			{
				MiniMapDisplayEntry miniMapDisplayEntry2 = this.MapEntryPool.Get(null);
				miniMapDisplayEntry2.SetupCombined(this, dataProvider);
				miniMapDisplayEntry2.gameObject.SetActive(true);
				miniMapDisplayEntry2.transform.SetAsFirstSibling();
			}
			this.SetupRotation();
			this.FitContent();
			this.HandlePointsOfInterests();
			this.HandleTeleporters();
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x0004D1A8 File Offset: 0x0004B3A8
		private void SetupRotation()
		{
			Vector3 vector = UnityVectorExtensions.ProjectOntoPlane(LevelManager.Instance.GameCamera.mainVCam.transform.up, Vector3.up);
			float num = Vector3.SignedAngle(Vector3.forward, vector, Vector3.up);
			base.transform.localRotation = Quaternion.Euler(0f, 0f, num);
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0004D208 File Offset: 0x0004B408
		private void HandlePointsOfInterests()
		{
			this.PointOfInterestEntryPool.ReleaseAll();
			foreach (MonoBehaviour monoBehaviour in PointsOfInterests.Points)
			{
				if (!(monoBehaviour == null))
				{
					this.HandlePointOfInterest(monoBehaviour);
				}
			}
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0004D268 File Offset: 0x0004B468
		private void HandlePointOfInterest(MonoBehaviour poi)
		{
			int targetSceneIndex = poi.gameObject.scene.buildIndex;
			IPointOfInterest pointOfInterest = poi as IPointOfInterest;
			if (pointOfInterest != null && pointOfInterest.OverrideScene >= 0)
			{
				targetSceneIndex = pointOfInterest.OverrideScene;
			}
			if (MultiSceneCore.ActiveSubScene == null || targetSceneIndex != MultiSceneCore.ActiveSubScene.Value.buildIndex)
			{
				return;
			}
			MiniMapDisplayEntry miniMapDisplayEntry = this.MapEntryPool.ActiveEntries.FirstOrDefault((MiniMapDisplayEntry e) => e.SceneReference != null && e.SceneReference.BuildIndex == targetSceneIndex);
			if (miniMapDisplayEntry == null)
			{
				return;
			}
			if (miniMapDisplayEntry.Hide)
			{
				return;
			}
			this.PointOfInterestEntryPool.Get(null).Setup(this, poi, miniMapDisplayEntry);
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x0004D328 File Offset: 0x0004B528
		private void ReleasePointOfInterest(MonoBehaviour poi)
		{
			PointOfInterestEntry pointOfInterestEntry = this.PointOfInterestEntryPool.ActiveEntries.FirstOrDefault((PointOfInterestEntry e) => e != null && e.Target == poi);
			if (!pointOfInterestEntry)
			{
				return;
			}
			this.PointOfInterestEntryPool.Release(pointOfInterestEntry);
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x0004D374 File Offset: 0x0004B574
		private void HandleTeleporters()
		{
			this.teleporterSplines.gameObject.SetActive(false);
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x0004D394 File Offset: 0x0004B594
		private void FitContent()
		{
			ReadOnlyCollection<MiniMapDisplayEntry> activeEntries = this.MapEntryPool.ActiveEntries;
			Vector2 vector;
			vector..ctor(float.MinValue, float.MinValue);
			Vector2 vector2;
			vector2..ctor(float.MaxValue, float.MaxValue);
			foreach (MiniMapDisplayEntry miniMapDisplayEntry in activeEntries)
			{
				RectTransform rectTransform = miniMapDisplayEntry.transform as RectTransform;
				Vector2 vector3 = rectTransform.anchoredPosition + rectTransform.rect.min;
				Vector2 vector4 = rectTransform.anchoredPosition + rectTransform.rect.max;
				vector.x = MathF.Max(vector4.x, vector.x);
				vector.y = MathF.Max(vector4.y, vector.y);
				vector2.x = MathF.Min(vector3.x, vector2.x);
				vector2.y = MathF.Min(vector3.y, vector2.y);
			}
			Vector2 vector5 = (vector + vector2) / 2f;
			foreach (MiniMapDisplayEntry miniMapDisplayEntry2 in activeEntries)
			{
				miniMapDisplayEntry2.transform.localPosition -= vector5;
			}
			(base.transform as RectTransform).sizeDelta = new Vector2(vector.x - vector2.x + this.padding * 2f, vector.y - vector2.y + this.padding * 2f);
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0004D564 File Offset: 0x0004B764
		public bool TryConvertWorldToMinimap(Vector3 worldPosition, string sceneID, out Vector3 result)
		{
			result = worldPosition;
			MiniMapDisplayEntry miniMapDisplayEntry = this.MapEntryPool.ActiveEntries.FirstOrDefault((MiniMapDisplayEntry e) => e != null && e.SceneID == sceneID);
			if (miniMapDisplayEntry == null)
			{
				return false;
			}
			Vector3 center = MiniMapCenter.GetCenter(sceneID);
			Vector3 vector = worldPosition - center;
			Vector3 vector2;
			vector2..ctor(vector.x, vector.z);
			Vector3 vector3 = miniMapDisplayEntry.transform.localToWorldMatrix.MultiplyPoint(vector2);
			result = base.transform.worldToLocalMatrix.MultiplyPoint(vector3);
			return true;
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x0004D60C File Offset: 0x0004B80C
		public bool TryConvertToWorldPosition(Vector3 displayPosition, out Vector3 result)
		{
			result = default(Vector3);
			string activeSubsceneID = MultiSceneCore.ActiveSubSceneID;
			MiniMapDisplayEntry miniMapDisplayEntry = this.MapEntryPool.ActiveEntries.FirstOrDefault((MiniMapDisplayEntry e) => e != null && e.SceneID == activeSubsceneID);
			if (miniMapDisplayEntry == null)
			{
				return false;
			}
			Vector3 vector = miniMapDisplayEntry.transform.worldToLocalMatrix.MultiplyPoint(displayPosition);
			Vector3 vector2;
			vector2..ctor(vector.x, 0f, vector.y);
			Vector3 center = MiniMapCenter.GetCenter(activeSubsceneID);
			result = center + vector2;
			return true;
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x0004D6A4 File Offset: 0x0004B8A4
		internal void Center(Vector3 minimapPos)
		{
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			Vector3 vector = rectTransform.localToWorldMatrix.MultiplyPoint(minimapPos);
			Vector3 vector2 = (rectTransform.parent as RectTransform).position - vector;
			rectTransform.position += vector2;
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0004D700 File Offset: 0x0004B900
		public void OnScroll(PointerEventData eventData)
		{
			this.master.OnScroll(eventData);
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x0004D721 File Offset: 0x0004B921
		[CompilerGenerated]
		internal static void <HandleTeleporters>g__ClearSplines|26_0(SplineContainer splineContainer)
		{
			while (splineContainer.Splines.Count > 0)
			{
				SplineUtility.RemoveSplineAt<SplineContainer>(splineContainer, 0);
			}
		}

		// Token: 0x04000F3B RID: 3899
		[SerializeField]
		private MiniMapView master;

		// Token: 0x04000F3C RID: 3900
		[SerializeField]
		private MiniMapDisplayEntry mapDisplayEntryPrefab;

		// Token: 0x04000F3D RID: 3901
		[SerializeField]
		private PointOfInterestEntry pointOfInterestEntryPrefab;

		// Token: 0x04000F3E RID: 3902
		[SerializeField]
		private UISplineRenderer teleporterSplines;

		// Token: 0x04000F3F RID: 3903
		[SerializeField]
		private bool autoSetupOnEnable;

		// Token: 0x04000F40 RID: 3904
		[SerializeField]
		private float padding = 25f;

		// Token: 0x04000F41 RID: 3905
		private PrefabPool<MiniMapDisplayEntry> _mapEntryPool;

		// Token: 0x04000F42 RID: 3906
		private PrefabPool<PointOfInterestEntry> _pointOfInterestEntryPool;
	}
}
