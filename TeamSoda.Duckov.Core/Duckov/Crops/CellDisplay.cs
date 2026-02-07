using System;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002F6 RID: 758
	public class CellDisplay : MonoBehaviour
	{
		// Token: 0x0600188F RID: 6287 RVA: 0x0005A840 File Offset: 0x00058A40
		internal void Setup(Garden garden, int coordx, int coordy)
		{
			this.garden = garden;
			this.coord = new Vector2Int(coordx, coordy);
			bool watered = false;
			Crop crop = garden[this.coord];
			if (crop != null)
			{
				watered = crop.Watered;
			}
			this.RefreshGraphics(watered);
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x0005A887 File Offset: 0x00058A87
		private void OnEnable()
		{
			Crop.onCropStatusChange += this.HandleCropEvent;
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x0005A89A File Offset: 0x00058A9A
		private void OnDisable()
		{
			Crop.onCropStatusChange -= this.HandleCropEvent;
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x0005A8B0 File Offset: 0x00058AB0
		private void HandleCropEvent(Crop crop, Crop.CropEvent e)
		{
			if (crop == null)
			{
				return;
			}
			if (this.garden == null)
			{
				return;
			}
			CropData data = crop.Data;
			if (data.gardenID != this.garden.GardenID || data.coord != this.coord)
			{
				return;
			}
			this.RefreshGraphics(crop.Watered && e != Crop.CropEvent.BeforeDestroy && e != Crop.CropEvent.Harvest);
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x0005A925 File Offset: 0x00058B25
		private void RefreshGraphics(bool watered)
		{
			if (watered)
			{
				this.ApplyGraphicsStype(this.styleWatered);
				return;
			}
			this.ApplyGraphicsStype(this.styleDry);
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x0005A944 File Offset: 0x00058B44
		private void ApplyGraphicsStype(CellDisplay.GraphicsStyle style)
		{
			if (this.propertyBlock == null)
			{
				this.propertyBlock = new MaterialPropertyBlock();
			}
			this.propertyBlock.Clear();
			string text = "_TintColor";
			string text2 = "_Smoothness";
			this.propertyBlock.SetColor(text, style.color);
			this.propertyBlock.SetFloat(text2, style.smoothness);
			this.renderer.SetPropertyBlock(this.propertyBlock);
		}

		// Token: 0x040011EC RID: 4588
		[SerializeField]
		private Renderer renderer;

		// Token: 0x040011ED RID: 4589
		[SerializeField]
		private CellDisplay.GraphicsStyle styleDry;

		// Token: 0x040011EE RID: 4590
		[SerializeField]
		private CellDisplay.GraphicsStyle styleWatered;

		// Token: 0x040011EF RID: 4591
		private Garden garden;

		// Token: 0x040011F0 RID: 4592
		private Vector2Int coord;

		// Token: 0x040011F1 RID: 4593
		private MaterialPropertyBlock propertyBlock;

		// Token: 0x020005A3 RID: 1443
		[Serializable]
		private struct GraphicsStyle
		{
			// Token: 0x040020A1 RID: 8353
			public Color color;

			// Token: 0x040020A2 RID: 8354
			public float smoothness;
		}
	}
}
