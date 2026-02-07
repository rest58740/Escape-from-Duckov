using System;
using Duckov.Scenes;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Duckov.MiniMaps.UI
{
	// Token: 0x02000288 RID: 648
	public class MiniMapDisplayEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x060014C8 RID: 5320 RVA: 0x0004D73C File Offset: 0x0004B93C
		public SceneReference SceneReference
		{
			get
			{
				SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(this.SceneID);
				if (sceneInfo == null)
				{
					return null;
				}
				return sceneInfo.SceneReference;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x060014C9 RID: 5321 RVA: 0x0004D760 File Offset: 0x0004B960
		public string SceneID
		{
			get
			{
				return this.sceneID;
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x060014CA RID: 5322 RVA: 0x0004D768 File Offset: 0x0004B968
		private RectTransform rectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = (base.transform as RectTransform);
				}
				return this._rectTransform;
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x060014CB RID: 5323 RVA: 0x0004D78F File Offset: 0x0004B98F
		// (set) Token: 0x060014CC RID: 5324 RVA: 0x0004D797 File Offset: 0x0004B997
		public MiniMapDisplay Master { get; private set; }

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x060014CD RID: 5325 RVA: 0x0004D7A0 File Offset: 0x0004B9A0
		public bool Hide
		{
			get
			{
				return this.target != null && this.target.Hide;
			}
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0004D7B7 File Offset: 0x0004B9B7
		private void Awake()
		{
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0004D7CA File Offset: 0x0004B9CA
		private void OnDestroy()
		{
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0004D7DD File Offset: 0x0004B9DD
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Mapping entries";
			Debug.Log("Mapping entries", this);
			this.RefreshGraphics();
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0004D7FA File Offset: 0x0004B9FA
		public bool NoSignal()
		{
			return this.target != null && this.target.NoSignal;
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0004D814 File Offset: 0x0004BA14
		internal void Setup(MiniMapDisplay master, IMiniMapEntry cur, bool showGraphics = true)
		{
			this.Master = master;
			this.target = cur;
			if (cur.Sprite != null)
			{
				this.image.sprite = cur.Sprite;
				this.rectTransform.sizeDelta = Vector2.one * (float)cur.Sprite.texture.width * cur.PixelSize;
				this.showGraphics = showGraphics;
			}
			else
			{
				this.showGraphics = false;
			}
			if (cur.Hide)
			{
				this.showGraphics = false;
			}
			this.rectTransform.anchoredPosition = cur.Offset;
			this.sceneID = cur.SceneID;
			this.isCombined = false;
			this.RefreshGraphics();
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0004D8C8 File Offset: 0x0004BAC8
		internal void SetupCombined(MiniMapDisplay master, IMiniMapDataProvider dataProvider)
		{
			this.target = null;
			this.Master = master;
			if (dataProvider == null)
			{
				return;
			}
			if (dataProvider.CombinedSprite == null)
			{
				return;
			}
			this.image.sprite = dataProvider.CombinedSprite;
			this.rectTransform.sizeDelta = Vector2.one * (float)dataProvider.CombinedSprite.texture.width * dataProvider.PixelSize;
			this.rectTransform.anchoredPosition = dataProvider.CombinedCenter;
			this.sceneID = "";
			this.image.enabled = true;
			this.showGraphics = true;
			this.isCombined = true;
			this.RefreshGraphics();
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0004D97C File Offset: 0x0004BB7C
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != 1)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.sceneID))
			{
				return;
			}
			Vector3 vector;
			RectTransformUtility.ScreenPointToWorldPointInRectangle(base.transform as RectTransform, eventData.position, null, ref vector);
			Vector3 worldPos;
			if (!this.Master.TryConvertToWorldPosition(eventData.position, out worldPos))
			{
				return;
			}
			MiniMapView.RequestMarkPOI(worldPos);
			eventData.Use();
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0004D9E4 File Offset: 0x0004BBE4
		private void RefreshGraphics()
		{
			bool flag = this.ShouldShow();
			if (flag)
			{
				this.image.color = Color.white;
			}
			else
			{
				this.image.color = Color.clear;
			}
			this.image.enabled = flag;
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0004DA29 File Offset: 0x0004BC29
		public bool ShouldShow()
		{
			if (!this.showGraphics)
			{
				return false;
			}
			if (this.isCombined)
			{
				return this.showGraphics;
			}
			return MultiSceneCore.ActiveSubSceneID == this.SceneID;
		}

		// Token: 0x04000F43 RID: 3907
		[SerializeField]
		private Image image;

		// Token: 0x04000F44 RID: 3908
		private string sceneID;

		// Token: 0x04000F45 RID: 3909
		private RectTransform _rectTransform;

		// Token: 0x04000F47 RID: 3911
		private bool showGraphics;

		// Token: 0x04000F48 RID: 3912
		private bool isCombined;

		// Token: 0x04000F49 RID: 3913
		private IMiniMapEntry target;
	}
}
