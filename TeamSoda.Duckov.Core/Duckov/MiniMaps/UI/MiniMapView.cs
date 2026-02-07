using System;
using Duckov.Scenes;
using Duckov.UI;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Duckov.MiniMaps.UI
{
	// Token: 0x02000289 RID: 649
	public class MiniMapView : View
	{
		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x060014D8 RID: 5336 RVA: 0x0004DA61 File Offset: 0x0004BC61
		public static MiniMapView Instance
		{
			get
			{
				return View.GetViewInstance<MiniMapView>();
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x060014DA RID: 5338 RVA: 0x0004DA80 File Offset: 0x0004BC80
		// (set) Token: 0x060014D9 RID: 5337 RVA: 0x0004DA68 File Offset: 0x0004BC68
		private float Zoom
		{
			get
			{
				return this._zoom;
			}
			set
			{
				value = Mathf.Clamp01(value);
				this._zoom = value;
				this.OnSetZoom(value);
			}
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x0004DA88 File Offset: 0x0004BC88
		private void OnSetZoom(float scale)
		{
			this.RefreshZoom();
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x0004DA90 File Offset: 0x0004BC90
		private void RefreshZoom()
		{
			if (this.display == null)
			{
				return;
			}
			RectTransform rectTransform = base.transform as RectTransform;
			Transform transform = this.display.transform;
			Vector3 vector = rectTransform.localToWorldMatrix.MultiplyPoint(rectTransform.rect.center);
			Vector3 vector2 = transform.worldToLocalMatrix.MultiplyPoint(vector);
			this.display.transform.localScale = Vector3.one * Mathf.Lerp(this.zoomMin, this.zoomMax, this.zoomCurve.Evaluate(this.Zoom));
			Vector3 vector3 = transform.localToWorldMatrix.MultiplyPoint(vector2) - vector;
			this.display.transform.position -= vector3;
			this.zoomSlider.SetValueWithoutNotify(this.Zoom);
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0004DB78 File Offset: 0x0004BD78
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			this.display.AutoSetup();
			MultiSceneCore instance = MultiSceneCore.Instance;
			SceneInfoEntry sceneInfoEntry = (instance != null) ? instance.SceneInfo : null;
			if (sceneInfoEntry != null)
			{
				this.mapNameText.text = sceneInfoEntry.DisplayName;
				this.mapInfoText.text = sceneInfoEntry.Description;
			}
			else
			{
				this.mapNameText.text = "";
				this.mapInfoText.text = "";
			}
			this.zoomSlider.SetValueWithoutNotify(this.Zoom);
			this.RefreshZoom();
			this.CeneterPlayer();
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x0004DC17 File Offset: 0x0004BE17
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x0004DC2A File Offset: 0x0004BE2A
		protected override void Awake()
		{
			base.Awake();
			this.zoomSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnZoomSliderValueChanged));
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x0004DC4E File Offset: 0x0004BE4E
		private void FixedUpdate()
		{
			this.RefreshNoSignalIndicator();
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0004DC56 File Offset: 0x0004BE56
		private void RefreshNoSignalIndicator()
		{
			this.noSignalIndicator.SetActive(this.display.NoSignal());
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x0004DC6E File Offset: 0x0004BE6E
		private void OnZoomSliderValueChanged(float value)
		{
			this.Zoom = value;
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x0004DC77 File Offset: 0x0004BE77
		public static void Show()
		{
			if (MiniMapView.Instance == null)
			{
				return;
			}
			if (MiniMapSettings.Instance == null)
			{
				return;
			}
			MiniMapView.Instance.Open(null);
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x0004DCA0 File Offset: 0x0004BEA0
		public void CeneterPlayer()
		{
			CharacterMainControl main = CharacterMainControl.Main;
			if (main == null)
			{
				return;
			}
			Vector3 minimapPos;
			if (!this.display.TryConvertWorldToMinimap(main.transform.position, SceneInfoCollection.GetSceneID(SceneManager.GetActiveScene().buildIndex), out minimapPos))
			{
				return;
			}
			this.display.Center(minimapPos);
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x0004DCF6 File Offset: 0x0004BEF6
		public static bool TryConvertWorldToMinimapPosition(Vector3 worldPosition, string sceneID, out Vector3 result)
		{
			result = default(Vector3);
			return !(MiniMapView.Instance == null) && MiniMapView.Instance.display.TryConvertWorldToMinimap(worldPosition, sceneID, out result);
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x0004DD20 File Offset: 0x0004BF20
		public static bool TryConvertWorldToMinimapPosition(Vector3 worldPosition, out Vector3 result)
		{
			result = default(Vector3);
			if (MiniMapView.Instance == null)
			{
				return false;
			}
			string sceneID = SceneInfoCollection.GetSceneID(SceneManager.GetActiveScene().buildIndex);
			return MiniMapView.TryConvertWorldToMinimapPosition(worldPosition, sceneID, out result);
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x0004DD5E File Offset: 0x0004BF5E
		internal void OnScroll(PointerEventData eventData)
		{
			this.Zoom += eventData.scrollDelta.y * this.scrollSensitivity;
			eventData.Use();
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x0004DD85 File Offset: 0x0004BF85
		internal static void RequestMarkPOI(Vector3 worldPos)
		{
			MapMarkerManager.Request(worldPos);
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x0004DD8D File Offset: 0x0004BF8D
		public void LoadData(PackedMapData mapData)
		{
			if (mapData == null)
			{
				return;
			}
			this.display.Setup(mapData);
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x0004DDA5 File Offset: 0x0004BFA5
		public void LoadCurrent()
		{
			this.display.AutoSetup();
		}

		// Token: 0x04000F4A RID: 3914
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04000F4B RID: 3915
		[SerializeField]
		private MiniMapDisplay display;

		// Token: 0x04000F4C RID: 3916
		[SerializeField]
		private TextMeshProUGUI mapNameText;

		// Token: 0x04000F4D RID: 3917
		[SerializeField]
		private TextMeshProUGUI mapInfoText;

		// Token: 0x04000F4E RID: 3918
		[SerializeField]
		private Slider zoomSlider;

		// Token: 0x04000F4F RID: 3919
		[SerializeField]
		private float zoomMin = 5f;

		// Token: 0x04000F50 RID: 3920
		[SerializeField]
		private float zoomMax = 20f;

		// Token: 0x04000F51 RID: 3921
		[SerializeField]
		[HideInInspector]
		private float _zoom = 5f;

		// Token: 0x04000F52 RID: 3922
		[SerializeField]
		[Range(0f, 0.01f)]
		private float scrollSensitivity = 0.01f;

		// Token: 0x04000F53 RID: 3923
		[SerializeField]
		private SimplePointOfInterest markPoiTemplate;

		// Token: 0x04000F54 RID: 3924
		[SerializeField]
		private AnimationCurve zoomCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x04000F55 RID: 3925
		[SerializeField]
		private GameObject noSignalIndicator;
	}
}
