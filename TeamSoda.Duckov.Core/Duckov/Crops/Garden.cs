using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Utilities;
using Saves;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002FE RID: 766
	public class Garden : MonoBehaviour
	{
		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x060018C7 RID: 6343 RVA: 0x0005B3C6 File Offset: 0x000595C6
		public string GardenID
		{
			get
			{
				return this.gardenID;
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x060018C8 RID: 6344 RVA: 0x0005B3CE File Offset: 0x000595CE
		public string SaveKey
		{
			get
			{
				return "Garden_" + this.gardenID;
			}
		}

		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x060018C9 RID: 6345 RVA: 0x0005B3E0 File Offset: 0x000595E0
		// (remove) Token: 0x060018CA RID: 6346 RVA: 0x0005B414 File Offset: 0x00059614
		public static event Action OnSizeAddersChanged;

		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x060018CB RID: 6347 RVA: 0x0005B448 File Offset: 0x00059648
		// (remove) Token: 0x060018CC RID: 6348 RVA: 0x0005B47C File Offset: 0x0005967C
		public static event Action OnAutoWatersChanged;

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x060018CD RID: 6349 RVA: 0x0005B4AF File Offset: 0x000596AF
		// (set) Token: 0x060018CE RID: 6350 RVA: 0x0005B4B7 File Offset: 0x000596B7
		public bool AutoWater
		{
			get
			{
				return this.autoWater;
			}
			set
			{
				this.autoWater = value;
				if (value)
				{
					this.WaterAll();
				}
			}
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x0005B4CC File Offset: 0x000596CC
		private void WaterAll()
		{
			foreach (Crop crop in this.dictioanry.Values)
			{
				if (!(crop == null) && !crop.Watered)
				{
					crop.Water();
				}
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x060018D0 RID: 6352 RVA: 0x0005B534 File Offset: 0x00059734
		// (set) Token: 0x060018D1 RID: 6353 RVA: 0x0005B53C File Offset: 0x0005973C
		public Vector2Int Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
				this.sizeDirty = true;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x060018D2 RID: 6354 RVA: 0x0005B54C File Offset: 0x0005974C
		public PrefabPool<CellDisplay> CellPool
		{
			get
			{
				if (this._cellPool == null)
				{
					this._cellPool = new PrefabPool<CellDisplay>(this.cellDisplayTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._cellPool;
			}
		}

		// Token: 0x17000476 RID: 1142
		public Crop this[Vector2Int coord]
		{
			get
			{
				Crop result;
				if (this.dictioanry.TryGetValue(coord, out result))
				{
					return result;
				}
				return null;
			}
			private set
			{
				this.dictioanry[coord] = value;
			}
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x0005B5B8 File Offset: 0x000597B8
		private void Awake()
		{
			Garden.gardens[this.gardenID] = this;
			SavesSystem.OnCollectSaveData += this.Save;
			Garden.OnSizeAddersChanged += this.RefreshSize;
			Garden.OnAutoWatersChanged += this.RefreshAutowater;
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x0005B609 File Offset: 0x00059809
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
			Garden.OnSizeAddersChanged -= this.RefreshSize;
			Garden.OnAutoWatersChanged -= this.RefreshAutowater;
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x0005B63E File Offset: 0x0005983E
		private void Start()
		{
			this.RegenerateCellDisplays();
			this.Load();
			this.RefreshSize();
			this.RefreshAutowater();
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x0005B658 File Offset: 0x00059858
		private void FixedUpdate()
		{
			if (this.sizeDirty)
			{
				this.RegenerateCellDisplays();
			}
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x0005B668 File Offset: 0x00059868
		private void RefreshAutowater()
		{
			bool flag = false;
			using (List<IGardenAutoWaterProvider>.Enumerator enumerator = Garden.autoWaters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.TakeEffect(this.gardenID))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag != this.AutoWater)
			{
				this.AutoWater = flag;
			}
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x0005B6D8 File Offset: 0x000598D8
		private void RefreshSize()
		{
			Vector2Int vector2Int = Vector2Int.zero;
			foreach (IGardenSizeAdder gardenSizeAdder in Garden.sizeAdders)
			{
				if (gardenSizeAdder != null)
				{
					vector2Int += gardenSizeAdder.GetValue(this.gardenID);
				}
			}
			this.Size = new Vector2Int(3 + vector2Int.x, 3 + vector2Int.y);
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x0005B75C File Offset: 0x0005995C
		public void SetSize(int x, int y)
		{
			this.RegenerateCellDisplays();
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x0005B764 File Offset: 0x00059964
		private void RegenerateCellDisplays()
		{
			this.sizeDirty = false;
			this.CellPool.ReleaseAll();
			Vector2Int vector2Int = this.Size;
			for (int i = 0; i < vector2Int.y; i++)
			{
				for (int j = 0; j < vector2Int.x; j++)
				{
					Vector3 localPosition = this.CoordToLocalPosition(new Vector2Int(j, i));
					CellDisplay cellDisplay = this.CellPool.Get(null);
					cellDisplay.transform.localPosition = localPosition;
					cellDisplay.Setup(this, j, i);
				}
			}
			Vector3 vector = this.CoordToLocalPosition(new Vector2Int(0, 0)) - new Vector3(this.grid.cellSize.x, 0f, this.grid.cellSize.y) / 2f;
			Vector3 vector2 = this.CoordToLocalPosition(new Vector2Int(vector2Int.x, vector2Int.y)) - new Vector3(this.grid.cellSize.x, 0f, this.grid.cellSize.y) / 2f;
			float num = vector2.x - vector.x;
			float num2 = vector2.z - vector.z;
			Vector3 localPosition2 = vector;
			Vector3 localPosition3;
			localPosition3..ctor(vector.x, 0f, vector2.z);
			Vector3 localPosition4 = vector2;
			Vector3 localPosition5;
			localPosition5..ctor(vector2.x, 0f, vector.z);
			Vector3 localScale;
			localScale..ctor(1f, 1f, num2);
			Vector3 localScale2;
			localScale2..ctor(1f, 1f, num);
			Vector3 localScale3;
			localScale3..ctor(1f, 1f, num2);
			Vector3 localScale4;
			localScale4..ctor(1f, 1f, num);
			this.border00.localPosition = localPosition2;
			this.border01.localPosition = localPosition3;
			this.border11.localPosition = localPosition4;
			this.border10.localPosition = localPosition5;
			this.corner00.localPosition = localPosition2;
			this.corner01.localPosition = localPosition3;
			this.corner11.localPosition = localPosition4;
			this.corner10.localPosition = localPosition5;
			this.border00.localScale = localScale;
			this.border01.localScale = localScale2;
			this.border11.localScale = localScale3;
			this.border10.localScale = localScale4;
			this.border00.localRotation = Quaternion.Euler(0f, 0f, 0f);
			this.border01.localRotation = Quaternion.Euler(0f, 90f, 0f);
			this.border11.localRotation = Quaternion.Euler(0f, 180f, 0f);
			this.border10.localRotation = Quaternion.Euler(0f, 270f, 0f);
			Vector3 localPosition6 = (vector + vector2) / 2f;
			this.interactBox.transform.localPosition = localPosition6;
			this.interactBox.center = Vector3.zero;
			this.interactBox.size = new Vector3(num + 0.5f, 1f, num2 + 0.5f);
		}

		// Token: 0x060018DD RID: 6365 RVA: 0x0005BA92 File Offset: 0x00059C92
		private Crop CreateCropInstance(string id)
		{
			return Object.Instantiate<Crop>(this.cropTemplate, base.transform);
		}

		// Token: 0x060018DE RID: 6366 RVA: 0x0005BAA8 File Offset: 0x00059CA8
		public void Save()
		{
			if (!LevelManager.LevelInited)
			{
				return;
			}
			Garden.SaveData value = new Garden.SaveData(this);
			SavesSystem.Save<Garden.SaveData>(this.SaveKey, value);
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x0005BAD0 File Offset: 0x00059CD0
		public void Load()
		{
			this.Clear();
			this.dictioanry.Clear();
			Garden.SaveData saveData = SavesSystem.Load<Garden.SaveData>(this.SaveKey);
			if (saveData == null)
			{
				return;
			}
			foreach (CropData cropData in saveData.crops)
			{
				Crop crop = this.CreateCropInstance(cropData.cropID);
				crop.Initialize(this, cropData);
				this[cropData.coord] = crop;
			}
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x0005BB60 File Offset: 0x00059D60
		private void Clear()
		{
			foreach (Crop crop in this.dictioanry.Values.ToList<Crop>())
			{
				if (!(crop == null))
				{
					Object.Destroy(crop.gameObject);
				}
			}
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x0005BBCC File Offset: 0x00059DCC
		public bool IsCoordValid(Vector2Int coord)
		{
			Vector2Int vector2Int = this.Size;
			return vector2Int.x <= 0 || vector2Int.y <= 0 || (coord.x < vector2Int.x && coord.y < vector2Int.y && coord.x >= 0 && coord.y >= 0);
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x0005BC2F File Offset: 0x00059E2F
		public bool IsCoordOccupied(Vector2Int coord)
		{
			return this[coord] != null;
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x0005BC40 File Offset: 0x00059E40
		public bool Plant(Vector2Int coord, string cropID)
		{
			if (!this.IsCoordValid(coord))
			{
				return false;
			}
			if (this.IsCoordOccupied(coord))
			{
				return false;
			}
			if (!CropDatabase.IsIdValid(cropID))
			{
				Debug.Log("[Garden] Invalid crop id " + cropID, this);
				return false;
			}
			Crop crop = this.CreateCropInstance(cropID);
			crop.InitializeNew(this, cropID, coord);
			this[coord] = crop;
			if (this.autoWater)
			{
				crop.Water();
			}
			return true;
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x0005BCA8 File Offset: 0x00059EA8
		public void Water(Vector2Int coord)
		{
			Crop crop = this[coord];
			if (crop == null)
			{
				return;
			}
			crop.Water();
		}

		// Token: 0x060018E5 RID: 6373 RVA: 0x0005BCD0 File Offset: 0x00059ED0
		public Vector3 CoordToWorldPosition(Vector2Int coord)
		{
			Vector3 vector = this.CoordToLocalPosition(coord);
			return base.transform.TransformPoint(vector);
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x0005BCF4 File Offset: 0x00059EF4
		public Vector3 CoordToLocalPosition(Vector2Int coord)
		{
			Vector3 cellCenterLocal = this.grid.GetCellCenterLocal((Vector3Int)coord);
			float z = this.grid.cellSize.z;
			float y = cellCenterLocal.y - z / 2f;
			Vector3 result = cellCenterLocal;
			result.y = y;
			return result;
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x0005BD3C File Offset: 0x00059F3C
		public Vector2Int WorldPositionToCoord(Vector3 wPos)
		{
			Vector3 vector = wPos + Vector3.up * 0.1f * this.grid.cellSize.z;
			return (Vector2Int)this.grid.WorldToCell(vector);
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x0005BD85 File Offset: 0x00059F85
		internal void Release(Crop crop)
		{
			Object.Destroy(crop.gameObject);
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x0005BD94 File Offset: 0x00059F94
		private void OnDrawGizmosSelected()
		{
			Gizmos.matrix = base.transform.localToWorldMatrix;
			float x = this.grid.cellSize.x;
			float y = this.grid.cellSize.y;
			Vector2Int vector2Int = this.Size;
			for (int i = 0; i <= vector2Int.x; i++)
			{
				Vector3 vector = Vector3.right * (float)i * x;
				Vector3 vector2 = vector + Vector3.forward * (float)vector2Int.y * y;
				Gizmos.DrawLine(vector, vector2);
			}
			for (int j = 0; j <= vector2Int.y; j++)
			{
				Vector3 vector3 = Vector3.forward * (float)j * y;
				Vector3 vector4 = vector3 + Vector3.right * (float)vector2Int.x * x;
				Gizmos.DrawLine(vector3, vector4);
			}
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x0005BE75 File Offset: 0x0005A075
		internal static void Register(IGardenSizeAdder obj)
		{
			Garden.sizeAdders.Add(obj);
			Action onSizeAddersChanged = Garden.OnSizeAddersChanged;
			if (onSizeAddersChanged == null)
			{
				return;
			}
			onSizeAddersChanged();
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x0005BE91 File Offset: 0x0005A091
		internal static void Register(IGardenAutoWaterProvider obj)
		{
			Garden.autoWaters.Add(obj);
			Action onAutoWatersChanged = Garden.OnAutoWatersChanged;
			if (onAutoWatersChanged == null)
			{
				return;
			}
			onAutoWatersChanged();
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x0005BEAD File Offset: 0x0005A0AD
		internal static void Unregister(IGardenSizeAdder obj)
		{
			Garden.sizeAdders.Remove(obj);
			Action onSizeAddersChanged = Garden.OnSizeAddersChanged;
			if (onSizeAddersChanged == null)
			{
				return;
			}
			onSizeAddersChanged();
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x0005BECA File Offset: 0x0005A0CA
		internal static void Unregister(IGardenAutoWaterProvider obj)
		{
			Garden.autoWaters.Remove(obj);
			Action onAutoWatersChanged = Garden.OnAutoWatersChanged;
			if (onAutoWatersChanged == null)
			{
				return;
			}
			onAutoWatersChanged();
		}

		// Token: 0x0400121F RID: 4639
		[SerializeField]
		private string gardenID = "Default";

		// Token: 0x04001220 RID: 4640
		public static List<IGardenSizeAdder> sizeAdders = new List<IGardenSizeAdder>();

		// Token: 0x04001221 RID: 4641
		public static List<IGardenAutoWaterProvider> autoWaters = new List<IGardenAutoWaterProvider>();

		// Token: 0x04001224 RID: 4644
		public static Dictionary<string, Garden> gardens = new Dictionary<string, Garden>();

		// Token: 0x04001225 RID: 4645
		[SerializeField]
		private Grid grid;

		// Token: 0x04001226 RID: 4646
		[SerializeField]
		private Crop cropTemplate;

		// Token: 0x04001227 RID: 4647
		[SerializeField]
		private Transform border00;

		// Token: 0x04001228 RID: 4648
		[SerializeField]
		private Transform border01;

		// Token: 0x04001229 RID: 4649
		[SerializeField]
		private Transform border11;

		// Token: 0x0400122A RID: 4650
		[SerializeField]
		private Transform border10;

		// Token: 0x0400122B RID: 4651
		[SerializeField]
		private Transform corner00;

		// Token: 0x0400122C RID: 4652
		[SerializeField]
		private Transform corner01;

		// Token: 0x0400122D RID: 4653
		[SerializeField]
		private Transform corner11;

		// Token: 0x0400122E RID: 4654
		[SerializeField]
		private Transform corner10;

		// Token: 0x0400122F RID: 4655
		[SerializeField]
		private BoxCollider interactBox;

		// Token: 0x04001230 RID: 4656
		[SerializeField]
		private Vector2Int size;

		// Token: 0x04001231 RID: 4657
		[SerializeField]
		private bool autoWater;

		// Token: 0x04001232 RID: 4658
		public Vector3 cameraRigCenter = new Vector3(3f, 0f, 3f);

		// Token: 0x04001233 RID: 4659
		private bool sizeDirty;

		// Token: 0x04001234 RID: 4660
		[SerializeField]
		private CellDisplay cellDisplayTemplate;

		// Token: 0x04001235 RID: 4661
		private PrefabPool<CellDisplay> _cellPool;

		// Token: 0x04001236 RID: 4662
		private Dictionary<Vector2Int, Crop> dictioanry = new Dictionary<Vector2Int, Crop>();

		// Token: 0x020005A9 RID: 1449
		[Serializable]
		private class SaveData
		{
			// Token: 0x060029A5 RID: 10661 RVA: 0x0009A71C File Offset: 0x0009891C
			public SaveData(Garden garden)
			{
				this.crops = new List<CropData>();
				foreach (Crop crop in garden.dictioanry.Values)
				{
					if (!(crop == null))
					{
						this.crops.Add(crop.Data);
					}
				}
			}

			// Token: 0x040020AE RID: 8366
			[SerializeField]
			public List<CropData> crops;
		}
	}
}
