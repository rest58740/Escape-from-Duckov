using System;
using System.Collections.Generic;
using Duckov.Scenes;
using Saves;
using UnityEngine;

namespace Duckov.MiniMaps
{
	// Token: 0x0200027F RID: 639
	public class MapMarkerManager : MonoBehaviour
	{
		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001446 RID: 5190 RVA: 0x0004C019 File Offset: 0x0004A219
		// (set) Token: 0x06001447 RID: 5191 RVA: 0x0004C020 File Offset: 0x0004A220
		public static MapMarkerManager Instance { get; private set; }

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06001448 RID: 5192 RVA: 0x0004C028 File Offset: 0x0004A228
		public static int SelectedIconIndex
		{
			get
			{
				if (MapMarkerManager.Instance == null)
				{
					return 0;
				}
				return MapMarkerManager.Instance.selectedIconIndex;
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x0004C043 File Offset: 0x0004A243
		public static Color SelectedColor
		{
			get
			{
				if (MapMarkerManager.Instance == null)
				{
					return Color.white;
				}
				return MapMarkerManager.Instance.selectedColor;
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x0600144A RID: 5194 RVA: 0x0004C062 File Offset: 0x0004A262
		public static Sprite SelectedIcon
		{
			get
			{
				if (MapMarkerManager.Instance == null)
				{
					return null;
				}
				if (MapMarkerManager.Instance.icons.Count <= MapMarkerManager.SelectedIconIndex)
				{
					return null;
				}
				return MapMarkerManager.Instance.icons[MapMarkerManager.SelectedIconIndex];
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x0600144B RID: 5195 RVA: 0x0004C0A0 File Offset: 0x0004A2A0
		public static string SelectedIconName
		{
			get
			{
				if (MapMarkerManager.Instance == null)
				{
					return null;
				}
				Sprite selectedIcon = MapMarkerManager.SelectedIcon;
				if (selectedIcon == null)
				{
					return null;
				}
				return selectedIcon.name;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x0600144C RID: 5196 RVA: 0x0004C0D3 File Offset: 0x0004A2D3
		public static List<Sprite> Icons
		{
			get
			{
				if (MapMarkerManager.Instance == null)
				{
					return null;
				}
				return MapMarkerManager.Instance.icons;
			}
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x0004C0EE File Offset: 0x0004A2EE
		private void Awake()
		{
			MapMarkerManager.Instance = this;
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0004C107 File Offset: 0x0004A307
		private void Start()
		{
			this.Load();
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0004C10F File Offset: 0x0004A30F
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001450 RID: 5200 RVA: 0x0004C122 File Offset: 0x0004A322
		private string SaveKey
		{
			get
			{
				return "MapMarkerManager_" + MultiSceneCore.MainSceneID;
			}
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0004C134 File Offset: 0x0004A334
		private void Load()
		{
			this.loaded = true;
			MapMarkerManager.SaveData saveData = SavesSystem.Load<MapMarkerManager.SaveData>(this.SaveKey);
			if (saveData.pois != null)
			{
				foreach (MapMarkerPOI.RuntimeData data in saveData.pois)
				{
					MapMarkerManager.Request(data);
				}
			}
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0004C1A0 File Offset: 0x0004A3A0
		private void OnCollectSaveData()
		{
			if (!this.loaded)
			{
				return;
			}
			MapMarkerManager.SaveData saveData = new MapMarkerManager.SaveData
			{
				pois = new List<MapMarkerPOI.RuntimeData>()
			};
			foreach (MapMarkerPOI mapMarkerPOI in this.pois)
			{
				if (!(mapMarkerPOI == null))
				{
					saveData.pois.Add(mapMarkerPOI.Data);
				}
			}
			SavesSystem.Save<MapMarkerManager.SaveData>(this.SaveKey, saveData);
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x0004C234 File Offset: 0x0004A434
		public static void Request(MapMarkerPOI.RuntimeData data)
		{
			if (MapMarkerManager.Instance == null)
			{
				return;
			}
			MapMarkerPOI mapMarkerPOI = Object.Instantiate<MapMarkerPOI>(MapMarkerManager.Instance.markerPrefab);
			mapMarkerPOI.Setup(data);
			MapMarkerManager.Instance.pois.Add(mapMarkerPOI);
			MultiSceneCore.MoveToMainScene(mapMarkerPOI.gameObject);
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0004C284 File Offset: 0x0004A484
		public static void Request(Vector3 worldPos)
		{
			if (MapMarkerManager.Instance == null)
			{
				return;
			}
			MapMarkerPOI mapMarkerPOI = Object.Instantiate<MapMarkerPOI>(MapMarkerManager.Instance.markerPrefab);
			mapMarkerPOI.Setup(worldPos, MapMarkerManager.SelectedIconName, MultiSceneCore.ActiveSubSceneID, new Color?(MapMarkerManager.SelectedColor));
			MapMarkerManager.Instance.pois.Add(mapMarkerPOI);
			MultiSceneCore.MoveToMainScene(mapMarkerPOI.gameObject);
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x0004C2E5 File Offset: 0x0004A4E5
		public static void Release(MapMarkerPOI entry)
		{
			if (entry == null)
			{
				return;
			}
			if (MapMarkerManager.Instance != null)
			{
				MapMarkerManager.Instance.pois.Remove(entry);
			}
			if (entry != null)
			{
				Object.Destroy(entry.gameObject);
			}
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x0004C324 File Offset: 0x0004A524
		internal static Sprite GetIcon(string iconName)
		{
			if (MapMarkerManager.Instance == null)
			{
				return null;
			}
			if (MapMarkerManager.Instance.icons == null)
			{
				return null;
			}
			return MapMarkerManager.Instance.icons.Find((Sprite e) => e != null && e.name == iconName);
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x0004C376 File Offset: 0x0004A576
		internal static void SelectColor(Color color)
		{
			if (MapMarkerManager.Instance == null)
			{
				return;
			}
			MapMarkerManager.Instance.selectedColor = color;
			Action<Color> onColorChanged = MapMarkerManager.OnColorChanged;
			if (onColorChanged == null)
			{
				return;
			}
			onColorChanged(color);
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x0004C3A1 File Offset: 0x0004A5A1
		internal static void SelectIcon(int index)
		{
			if (MapMarkerManager.Instance == null)
			{
				return;
			}
			MapMarkerManager.Instance.selectedIconIndex = index;
			Action<int> onIconChanged = MapMarkerManager.OnIconChanged;
			if (onIconChanged == null)
			{
				return;
			}
			onIconChanged(index);
		}

		// Token: 0x04000F1A RID: 3866
		[SerializeField]
		private List<Sprite> icons = new List<Sprite>();

		// Token: 0x04000F1B RID: 3867
		[SerializeField]
		private MapMarkerPOI markerPrefab;

		// Token: 0x04000F1C RID: 3868
		[SerializeField]
		private int selectedIconIndex;

		// Token: 0x04000F1D RID: 3869
		[SerializeField]
		private Color selectedColor = Color.white;

		// Token: 0x04000F1E RID: 3870
		public static Action<int> OnIconChanged;

		// Token: 0x04000F1F RID: 3871
		public static Action<Color> OnColorChanged;

		// Token: 0x04000F20 RID: 3872
		private bool loaded;

		// Token: 0x04000F21 RID: 3873
		private List<MapMarkerPOI> pois = new List<MapMarkerPOI>();

		// Token: 0x02000565 RID: 1381
		[Serializable]
		private struct SaveData
		{
			// Token: 0x04001FA2 RID: 8098
			public string mainSceneName;

			// Token: 0x04001FA3 RID: 8099
			public List<MapMarkerPOI.RuntimeData> pois;
		}
	}
}
