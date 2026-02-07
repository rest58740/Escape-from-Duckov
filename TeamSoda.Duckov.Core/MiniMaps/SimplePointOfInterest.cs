using System;
using Duckov.Scenes;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Duckov.MiniMaps
{
	// Token: 0x02000285 RID: 645
	public class SimplePointOfInterest : MonoBehaviour, IPointOfInterest
	{
		// Token: 0x1400008D RID: 141
		// (add) Token: 0x06001492 RID: 5266 RVA: 0x0004CBC0 File Offset: 0x0004ADC0
		// (remove) Token: 0x06001493 RID: 5267 RVA: 0x0004CBF8 File Offset: 0x0004ADF8
		public event Action<PointerEventData> OnClicked;

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06001494 RID: 5268 RVA: 0x0004CC2D File Offset: 0x0004AE2D
		// (set) Token: 0x06001495 RID: 5269 RVA: 0x0004CC35 File Offset: 0x0004AE35
		public float ScaleFactor
		{
			get
			{
				return this.scaleFactor;
			}
			set
			{
				this.scaleFactor = value;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001496 RID: 5270 RVA: 0x0004CC3E File Offset: 0x0004AE3E
		// (set) Token: 0x06001497 RID: 5271 RVA: 0x0004CC46 File Offset: 0x0004AE46
		public Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				this.color = value;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001498 RID: 5272 RVA: 0x0004CC4F File Offset: 0x0004AE4F
		// (set) Token: 0x06001499 RID: 5273 RVA: 0x0004CC57 File Offset: 0x0004AE57
		public Color ShadowColor
		{
			get
			{
				return this.shadowColor;
			}
			set
			{
				this.shadowColor = value;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x0600149A RID: 5274 RVA: 0x0004CC60 File Offset: 0x0004AE60
		// (set) Token: 0x0600149B RID: 5275 RVA: 0x0004CC68 File Offset: 0x0004AE68
		public float ShadowDistance
		{
			get
			{
				return this.shadowDistance;
			}
			set
			{
				this.shadowDistance = value;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x0600149C RID: 5276 RVA: 0x0004CC71 File Offset: 0x0004AE71
		public string DisplayName
		{
			get
			{
				return LocalizationManager.ToPlainText(this.displayName);
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x0600149D RID: 5277 RVA: 0x0004CC7E File Offset: 0x0004AE7E
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x0600149E RID: 5278 RVA: 0x0004CC88 File Offset: 0x0004AE88
		public int OverrideScene
		{
			get
			{
				if (this.followActiveScene && MultiSceneCore.ActiveSubScene != null)
				{
					return MultiSceneCore.ActiveSubScene.Value.buildIndex;
				}
				if (!string.IsNullOrEmpty(this.overrideSceneID))
				{
					return SceneInfoCollection.GetBuildIndex(this.overrideSceneID);
				}
				return -1;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x0600149F RID: 5279 RVA: 0x0004CCDC File Offset: 0x0004AEDC
		// (set) Token: 0x060014A0 RID: 5280 RVA: 0x0004CCE4 File Offset: 0x0004AEE4
		public bool IsArea
		{
			get
			{
				return this.isArea;
			}
			set
			{
				this.isArea = value;
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060014A1 RID: 5281 RVA: 0x0004CCED File Offset: 0x0004AEED
		// (set) Token: 0x060014A2 RID: 5282 RVA: 0x0004CCF5 File Offset: 0x0004AEF5
		public float AreaRadius
		{
			get
			{
				return this.areaRadius;
			}
			set
			{
				this.areaRadius = value;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060014A3 RID: 5283 RVA: 0x0004CCFE File Offset: 0x0004AEFE
		// (set) Token: 0x060014A4 RID: 5284 RVA: 0x0004CD06 File Offset: 0x0004AF06
		public bool HideIcon
		{
			get
			{
				return this.hideIcon;
			}
			set
			{
				this.hideIcon = value;
			}
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x0004CD0F File Offset: 0x0004AF0F
		private void OnEnable()
		{
			PointsOfInterests.Register(this);
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x0004CD17 File Offset: 0x0004AF17
		private void OnDisable()
		{
			PointsOfInterests.Unregister(this);
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x0004CD1F File Offset: 0x0004AF1F
		public void Setup(Sprite icon = null, string displayName = null, bool followActiveScene = false, string overrideSceneID = null)
		{
			if (icon != null)
			{
				this.icon = icon;
			}
			this.displayName = displayName;
			this.followActiveScene = followActiveScene;
			this.overrideSceneID = overrideSceneID;
			PointsOfInterests.Unregister(this);
			PointsOfInterests.Register(this);
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x0004CD53 File Offset: 0x0004AF53
		public void SetColor(Color color)
		{
			this.color = color;
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x0004CD5C File Offset: 0x0004AF5C
		public bool SetupMultiSceneLocation(MultiSceneLocation location, bool moveToMainScene = true)
		{
			Vector3 position;
			if (!location.TryGetLocationPosition(out position))
			{
				return false;
			}
			base.transform.position = position;
			this.overrideSceneID = location.SceneID;
			if (moveToMainScene && MultiSceneCore.MainScene != null)
			{
				SceneManager.MoveGameObjectToScene(base.gameObject, MultiSceneCore.MainScene.Value);
			}
			return true;
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x0004CDBC File Offset: 0x0004AFBC
		public static SimplePointOfInterest Create(Vector3 position, string sceneID, string displayName, Sprite icon = null, bool hideIcon = false)
		{
			GameObject gameObject = new GameObject("POI_" + displayName);
			gameObject.transform.position = position;
			SimplePointOfInterest simplePointOfInterest = gameObject.AddComponent<SimplePointOfInterest>();
			simplePointOfInterest.overrideSceneID = sceneID;
			simplePointOfInterest.displayName = displayName;
			simplePointOfInterest.hideIcon = hideIcon;
			simplePointOfInterest.icon = icon;
			SceneManager.MoveGameObjectToScene(gameObject, MultiSceneCore.MainScene.Value);
			return simplePointOfInterest;
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x0004CE1C File Offset: 0x0004B01C
		public void NotifyClicked(PointerEventData pointerEventData)
		{
			Action<PointerEventData> onClicked = this.OnClicked;
			if (onClicked == null)
			{
				return;
			}
			onClicked(pointerEventData);
		}

		// Token: 0x04000F2E RID: 3886
		[SerializeField]
		private Sprite icon;

		// Token: 0x04000F2F RID: 3887
		[SerializeField]
		private Color color = Color.white;

		// Token: 0x04000F30 RID: 3888
		[SerializeField]
		private Color shadowColor = Color.white;

		// Token: 0x04000F31 RID: 3889
		[SerializeField]
		private float shadowDistance;

		// Token: 0x04000F32 RID: 3890
		[LocalizationKey("Default")]
		[SerializeField]
		private string displayName = "";

		// Token: 0x04000F33 RID: 3891
		[SerializeField]
		private bool followActiveScene;

		// Token: 0x04000F34 RID: 3892
		[SceneID]
		[SerializeField]
		private string overrideSceneID;

		// Token: 0x04000F35 RID: 3893
		[SerializeField]
		private bool isArea;

		// Token: 0x04000F36 RID: 3894
		[SerializeField]
		private float areaRadius;

		// Token: 0x04000F37 RID: 3895
		[SerializeField]
		private float scaleFactor = 1f;

		// Token: 0x04000F39 RID: 3897
		[SerializeField]
		private bool hideIcon;
	}
}
