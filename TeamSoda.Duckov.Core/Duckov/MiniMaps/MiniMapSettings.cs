using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Scenes;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.MiniMaps
{
	// Token: 0x02000282 RID: 642
	public class MiniMapSettings : MonoBehaviour, IMiniMapDataProvider
	{
		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001472 RID: 5234 RVA: 0x0004C795 File Offset: 0x0004A995
		public Sprite CombinedSprite
		{
			get
			{
				return this.combinedSprite;
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001473 RID: 5235 RVA: 0x0004C79D File Offset: 0x0004A99D
		public Vector3 CombinedCenter
		{
			get
			{
				return this.combinedCenter;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001474 RID: 5236 RVA: 0x0004C7A5 File Offset: 0x0004A9A5
		public List<IMiniMapEntry> Maps
		{
			get
			{
				return this.maps.ToList<IMiniMapEntry>();
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001475 RID: 5237 RVA: 0x0004C7B2 File Offset: 0x0004A9B2
		// (set) Token: 0x06001476 RID: 5238 RVA: 0x0004C7B9 File Offset: 0x0004A9B9
		public static MiniMapSettings Instance { get; private set; }

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001477 RID: 5239 RVA: 0x0004C7C4 File Offset: 0x0004A9C4
		public float PixelSize
		{
			get
			{
				int width = this.combinedSprite.texture.width;
				if (width > 0 && this.combinedSize > 0f)
				{
					return this.combinedSize / (float)width;
				}
				return -1f;
			}
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x0004C804 File Offset: 0x0004AA04
		private void Awake()
		{
			foreach (MiniMapSettings.MapEntry mapEntry in this.maps)
			{
				SpriteRenderer offsetReference = mapEntry.offsetReference;
				if (offsetReference != null)
				{
					offsetReference.gameObject.SetActive(false);
				}
			}
			if (MiniMapSettings.Instance == null)
			{
				MiniMapSettings.Instance = this;
			}
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0004C880 File Offset: 0x0004AA80
		public static bool TryGetMinimapPosition(Vector3 worldPosition, string sceneID, out Vector3 result)
		{
			result = worldPosition;
			if (MiniMapSettings.Instance == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(sceneID))
			{
				return false;
			}
			MiniMapSettings.MapEntry mapEntry = MiniMapSettings.Instance.maps.FirstOrDefault((MiniMapSettings.MapEntry e) => e != null && e.sceneID == sceneID);
			if (mapEntry == null)
			{
				return false;
			}
			Vector3 vector = worldPosition - mapEntry.mapWorldCenter;
			Vector3 vector2 = mapEntry.mapWorldCenter - MiniMapSettings.Instance.combinedCenter;
			vector + vector2;
			return true;
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0004C90C File Offset: 0x0004AB0C
		public static bool TryGetWorldPosition(Vector3 minimapPosition, string sceneID, out Vector3 result)
		{
			result = minimapPosition;
			if (MiniMapSettings.Instance == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(sceneID))
			{
				return false;
			}
			MiniMapSettings.MapEntry mapEntry = MiniMapSettings.Instance.maps.FirstOrDefault((MiniMapSettings.MapEntry e) => e != null && e.sceneID == sceneID);
			if (mapEntry == null)
			{
				return false;
			}
			result = mapEntry.mapWorldCenter + minimapPosition;
			return true;
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0004C980 File Offset: 0x0004AB80
		public static bool TryGetMinimapPosition(Vector3 worldPosition, out Vector3 result)
		{
			result = worldPosition;
			Scene activeScene = SceneManager.GetActiveScene();
			if (!activeScene.isLoaded)
			{
				return false;
			}
			string sceneID = SceneInfoCollection.GetSceneID(activeScene.buildIndex);
			return MiniMapSettings.TryGetMinimapPosition(worldPosition, sceneID, out result);
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x0004C9BC File Offset: 0x0004ABBC
		internal void Cache(MiniMapCenter miniMapCenter)
		{
			int scene = miniMapCenter.gameObject.scene.buildIndex;
			MiniMapSettings.MapEntry mapEntry = this.maps.FirstOrDefault((MiniMapSettings.MapEntry e) => e.SceneReference != null && e.SceneReference.UnsafeReason == null && e.SceneReference.BuildIndex == scene);
			if (mapEntry == null)
			{
				return;
			}
			mapEntry.mapWorldCenter = miniMapCenter.transform.position;
		}

		// Token: 0x04000F25 RID: 3877
		public List<MiniMapSettings.MapEntry> maps;

		// Token: 0x04000F26 RID: 3878
		public Vector3 combinedCenter;

		// Token: 0x04000F27 RID: 3879
		public float combinedSize;

		// Token: 0x04000F28 RID: 3880
		public Sprite combinedSprite;

		// Token: 0x0200056A RID: 1386
		[Serializable]
		public class MapEntry : IMiniMapEntry
		{
			// Token: 0x17000791 RID: 1937
			// (get) Token: 0x06002929 RID: 10537 RVA: 0x00097C1C File Offset: 0x00095E1C
			public SceneReference SceneReference
			{
				get
				{
					SceneInfoEntry sceneInfo = SceneInfoCollection.GetSceneInfo(this.sceneID);
					if (sceneInfo == null)
					{
						return null;
					}
					return sceneInfo.SceneReference;
				}
			}

			// Token: 0x17000792 RID: 1938
			// (get) Token: 0x0600292A RID: 10538 RVA: 0x00097C40 File Offset: 0x00095E40
			public string SceneID
			{
				get
				{
					return this.sceneID;
				}
			}

			// Token: 0x17000793 RID: 1939
			// (get) Token: 0x0600292B RID: 10539 RVA: 0x00097C48 File Offset: 0x00095E48
			public Sprite Sprite
			{
				get
				{
					return this.sprite;
				}
			}

			// Token: 0x17000794 RID: 1940
			// (get) Token: 0x0600292C RID: 10540 RVA: 0x00097C50 File Offset: 0x00095E50
			public bool Hide
			{
				get
				{
					return this.hide;
				}
			}

			// Token: 0x17000795 RID: 1941
			// (get) Token: 0x0600292D RID: 10541 RVA: 0x00097C58 File Offset: 0x00095E58
			public bool NoSignal
			{
				get
				{
					return this.noSignal;
				}
			}

			// Token: 0x17000796 RID: 1942
			// (get) Token: 0x0600292E RID: 10542 RVA: 0x00097C60 File Offset: 0x00095E60
			public float PixelSize
			{
				get
				{
					int width = this.sprite.texture.width;
					if (width > 0 && this.imageWorldSize > 0f)
					{
						return this.imageWorldSize / (float)width;
					}
					return -1f;
				}
			}

			// Token: 0x17000797 RID: 1943
			// (get) Token: 0x0600292F RID: 10543 RVA: 0x00097C9E File Offset: 0x00095E9E
			public Vector2 Offset
			{
				get
				{
					if (this.offsetReference == null)
					{
						return Vector2.zero;
					}
					return this.offsetReference.transform.localPosition;
				}
			}

			// Token: 0x06002930 RID: 10544 RVA: 0x00097CC9 File Offset: 0x00095EC9
			public MapEntry()
			{
			}

			// Token: 0x06002931 RID: 10545 RVA: 0x00097CD1 File Offset: 0x00095ED1
			public MapEntry(MiniMapSettings.MapEntry copyFrom)
			{
				this.imageWorldSize = copyFrom.imageWorldSize;
				this.sceneID = copyFrom.sceneID;
				this.sprite = copyFrom.sprite;
			}

			// Token: 0x04001FAB RID: 8107
			public float imageWorldSize;

			// Token: 0x04001FAC RID: 8108
			[SceneID]
			public string sceneID;

			// Token: 0x04001FAD RID: 8109
			public Sprite sprite;

			// Token: 0x04001FAE RID: 8110
			public SpriteRenderer offsetReference;

			// Token: 0x04001FAF RID: 8111
			public Vector3 mapWorldCenter;

			// Token: 0x04001FB0 RID: 8112
			public bool hide;

			// Token: 0x04001FB1 RID: 8113
			public bool noSignal;
		}

		// Token: 0x0200056B RID: 1387
		public struct Data
		{
		}
	}
}
