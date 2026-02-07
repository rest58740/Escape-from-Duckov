using System;
using Cysharp.Threading.Tasks;
using Duckov.Economy;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002F7 RID: 759
	public class Crop : MonoBehaviour
	{
		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001896 RID: 6294 RVA: 0x0005A9B8 File Offset: 0x00058BB8
		public CropData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001897 RID: 6295 RVA: 0x0005A9C0 File Offset: 0x00058BC0
		public CropInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001898 RID: 6296 RVA: 0x0005A9C8 File Offset: 0x00058BC8
		public float Progress
		{
			get
			{
				return (float)this.data.growTicks / (float)this.info.totalGrowTicks;
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001899 RID: 6297 RVA: 0x0005A9E3 File Offset: 0x00058BE3
		public bool Ripen
		{
			get
			{
				return this.initialized && this.data.growTicks >= this.info.totalGrowTicks;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x0600189A RID: 6298 RVA: 0x0005AA0A File Offset: 0x00058C0A
		public bool Watered
		{
			get
			{
				return this.data.watered;
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x0600189B RID: 6299 RVA: 0x0005AA18 File Offset: 0x00058C18
		public string DisplayName
		{
			get
			{
				return this.Info.DisplayName;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x0600189C RID: 6300 RVA: 0x0005AA34 File Offset: 0x00058C34
		public TimeSpan RemainingTime
		{
			get
			{
				if (!this.initialized)
				{
					return TimeSpan.Zero;
				}
				long num = this.info.totalGrowTicks - this.data.growTicks;
				if (num < 0L)
				{
					return TimeSpan.Zero;
				}
				return TimeSpan.FromTicks(num);
			}
		}

		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x0600189D RID: 6301 RVA: 0x0005AA78 File Offset: 0x00058C78
		// (remove) Token: 0x0600189E RID: 6302 RVA: 0x0005AAAC File Offset: 0x00058CAC
		public static event Action<Crop, Crop.CropEvent> onCropStatusChange;

		// Token: 0x0600189F RID: 6303 RVA: 0x0005AAE0 File Offset: 0x00058CE0
		public bool Harvest()
		{
			if (!this.Ripen)
			{
				return false;
			}
			if (this.Watered)
			{
				this.data.score = this.data.score + 50;
			}
			int product = this.info.GetProduct(this.data.Ranking);
			if (product <= 0)
			{
				Debug.LogError("Crop product is invalid:\ncrop:" + this.info.id);
				return false;
			}
			Cost cost = new Cost(new ValueTuple<int, long>[]
			{
				new ValueTuple<int, long>(product, (long)this.info.resultAmount)
			});
			cost.Return(false, false, 1, null).Forget();
			this.DestroyCrop();
			Action<Crop> action = this.onHarvest;
			if (action != null)
			{
				action(this);
			}
			Action<Crop, Crop.CropEvent> action2 = Crop.onCropStatusChange;
			if (action2 != null)
			{
				action2(this, Crop.CropEvent.Harvest);
			}
			return true;
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x0005ABA8 File Offset: 0x00058DA8
		public void DestroyCrop()
		{
			Action<Crop> action = this.onBeforeDestroy;
			if (action != null)
			{
				action(this);
			}
			Action<Crop, Crop.CropEvent> action2 = Crop.onCropStatusChange;
			if (action2 != null)
			{
				action2(this, Crop.CropEvent.BeforeDestroy);
			}
			this.garden.Release(this);
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x0005ABDC File Offset: 0x00058DDC
		public void InitializeNew(Garden garden, string id, Vector2Int coord)
		{
			CropData cropData = new CropData
			{
				gardenID = garden.GardenID,
				cropID = id,
				coord = coord,
				LastUpdateDateTime = DateTime.Now
			};
			this.Initialize(garden, cropData);
			Action<Crop> action = this.onPlant;
			if (action != null)
			{
				action(this);
			}
			Action<Crop, Crop.CropEvent> action2 = Crop.onCropStatusChange;
			if (action2 == null)
			{
				return;
			}
			action2(this, Crop.CropEvent.Plant);
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x0005AC48 File Offset: 0x00058E48
		public void Initialize(Garden garden, CropData data)
		{
			this.garden = garden;
			string cropID = data.cropID;
			CropInfo? cropInfo = CropDatabase.GetCropInfo(cropID);
			if (cropInfo == null)
			{
				Debug.LogError("找不到 corpInfo id: " + cropID);
				return;
			}
			this.info = cropInfo.Value;
			this.data = data;
			this.RefreshDisplayInstance();
			this.initialized = true;
			Vector3 localPosition = garden.CoordToLocalPosition(data.coord);
			base.transform.localPosition = localPosition;
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x0005ACC0 File Offset: 0x00058EC0
		private void RefreshDisplayInstance()
		{
			if (this.displayInstance != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this.displayInstance.gameObject);
				}
				else
				{
					Object.DestroyImmediate(this.displayInstance.gameObject);
				}
			}
			if (this.info.displayPrefab == null)
			{
				Debug.LogError("找不到Display Prefab: " + this.info.DisplayName);
				return;
			}
			this.displayInstance = Object.Instantiate<GameObject>(this.info.displayPrefab, this.displayParent);
			this.displayInstance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x0005AD68 File Offset: 0x00058F68
		public void Water()
		{
			if (this.data.watered)
			{
				return;
			}
			this.data.watered = true;
			Action<Crop> action = this.onWater;
			if (action != null)
			{
				action(this);
			}
			Action<Crop, Crop.CropEvent> action2 = Crop.onCropStatusChange;
			if (action2 == null)
			{
				return;
			}
			action2(this, Crop.CropEvent.Water);
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x0005ADA7 File Offset: 0x00058FA7
		private void FixedUpdate()
		{
			this.Tick();
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x0005ADB0 File Offset: 0x00058FB0
		private void Tick()
		{
			if (!this.initialized)
			{
				return;
			}
			TimeSpan timeSpan = DateTime.Now - this.data.LastUpdateDateTime;
			this.data.LastUpdateDateTime = DateTime.Now;
			if (!this.data.watered)
			{
				return;
			}
			if (this.Ripen)
			{
				return;
			}
			long ticks = timeSpan.Ticks;
			this.data.growTicks = this.data.growTicks + ticks;
			if (this.Ripen)
			{
				this.OnRipen();
			}
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x0005AE29 File Offset: 0x00059029
		private void OnRipen()
		{
			Action<Crop> action = this.onRipen;
			if (action != null)
			{
				action(this);
			}
			Action<Crop, Crop.CropEvent> action2 = Crop.onCropStatusChange;
			if (action2 == null)
			{
				return;
			}
			action2(this, Crop.CropEvent.Ripen);
		}

		// Token: 0x040011F2 RID: 4594
		[SerializeField]
		private Transform displayParent;

		// Token: 0x040011F3 RID: 4595
		private Garden garden;

		// Token: 0x040011F4 RID: 4596
		private bool initialized;

		// Token: 0x040011F5 RID: 4597
		private CropData data;

		// Token: 0x040011F6 RID: 4598
		private CropInfo info;

		// Token: 0x040011F7 RID: 4599
		private GameObject displayInstance;

		// Token: 0x040011F8 RID: 4600
		public Action<Crop> onPlant;

		// Token: 0x040011F9 RID: 4601
		public Action<Crop> onWater;

		// Token: 0x040011FA RID: 4602
		public Action<Crop> onRipen;

		// Token: 0x040011FB RID: 4603
		public Action<Crop> onHarvest;

		// Token: 0x040011FC RID: 4604
		public Action<Crop> onBeforeDestroy;

		// Token: 0x020005A4 RID: 1444
		public enum CropEvent
		{
			// Token: 0x040020A4 RID: 8356
			Plant,
			// Token: 0x040020A5 RID: 8357
			Water,
			// Token: 0x040020A6 RID: 8358
			Ripen,
			// Token: 0x040020A7 RID: 8359
			Harvest,
			// Token: 0x040020A8 RID: 8360
			BeforeDestroy
		}
	}
}
