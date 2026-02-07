using System;
using Duckov.Economy;
using TMPro;
using UnityEngine;

namespace Duckov.Crops.UI
{
	// Token: 0x02000301 RID: 769
	public class CellContextDisplay : MonoBehaviour
	{
		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x060018F2 RID: 6386 RVA: 0x0005BF3F File Offset: 0x0005A13F
		private Garden Garden
		{
			get
			{
				if (this.master == null)
				{
					return null;
				}
				return this.master.Target;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x060018F3 RID: 6387 RVA: 0x0005BF5C File Offset: 0x0005A15C
		private Vector2Int HoveringCoord
		{
			get
			{
				if (this.master == null)
				{
					return default(Vector2Int);
				}
				return this.master.HoveringCoord;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x060018F4 RID: 6388 RVA: 0x0005BF8C File Offset: 0x0005A18C
		private Crop HoveringCrop
		{
			get
			{
				if (this.master == null)
				{
					return null;
				}
				return this.master.HoveringCrop;
			}
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x0005BFA9 File Offset: 0x0005A1A9
		private void Show()
		{
			this.canvasGroup.alpha = 1f;
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x0005BFBB File Offset: 0x0005A1BB
		private void Hide()
		{
			this.canvasGroup.alpha = 0f;
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x0005BFCD File Offset: 0x0005A1CD
		private void Awake()
		{
			this.master.onContextChanged += this.OnContextChanged;
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x0005BFE6 File Offset: 0x0005A1E6
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x060018F9 RID: 6393 RVA: 0x0005BFEE File Offset: 0x0005A1EE
		private bool AnyContent
		{
			get
			{
				return this.plantInfo.activeSelf || this.currentCropInfo.activeSelf || this.operationInfo.activeSelf;
			}
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x0005C017 File Offset: 0x0005A217
		private void Update()
		{
			if (this.master.Hovering && this.AnyContent)
			{
				this.Show();
			}
			else
			{
				this.Hide();
			}
			if (this.HoveringCrop)
			{
				this.UpdateCurrentCropInfo();
			}
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x0005C050 File Offset: 0x0005A250
		private void LateUpdate()
		{
			Vector3 vector = this.Garden.CoordToWorldPosition(this.HoveringCoord) + Vector3.up * 2f;
			Vector2 vector2 = RectTransformUtility.WorldToScreenPoint(Camera.main, vector);
			base.transform.position = vector2;
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x0005C0A0 File Offset: 0x0005A2A0
		private void OnContextChanged()
		{
			this.Refresh();
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x0005C0A8 File Offset: 0x0005A2A8
		private void Refresh()
		{
			this.HideAll();
			switch (this.master.Tool)
			{
			case GardenView.ToolType.None:
				break;
			case GardenView.ToolType.Plant:
				if (this.HoveringCrop)
				{
					this.SetupCurrentCropInfo();
					return;
				}
				this.SetupPlantInfo();
				if (this.master.PlantingSeedTypeID > 0)
				{
					this.SetupOperationInfo();
					return;
				}
				break;
			case GardenView.ToolType.Harvest:
				if (this.HoveringCrop == null)
				{
					return;
				}
				this.SetupCurrentCropInfo();
				if (this.HoveringCrop.Ripen)
				{
					this.SetupOperationInfo();
					return;
				}
				break;
			case GardenView.ToolType.Water:
				if (this.HoveringCrop == null)
				{
					return;
				}
				this.SetupCurrentCropInfo();
				this.SetupOperationInfo();
				return;
			case GardenView.ToolType.Destroy:
				if (this.HoveringCrop == null)
				{
					return;
				}
				this.SetupCurrentCropInfo();
				this.SetupOperationInfo();
				break;
			default:
				return;
			}
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x0005C171 File Offset: 0x0005A371
		private void SetupCurrentCropInfo()
		{
			this.currentCropInfo.SetActive(true);
			this.cropNameText.text = this.HoveringCrop.DisplayName;
			this.UpdateCurrentCropInfo();
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x0005C19C File Offset: 0x0005A39C
		private void UpdateCurrentCropInfo()
		{
			if (this.HoveringCrop == null)
			{
				return;
			}
			this.cropCountdownText.text = this.HoveringCrop.RemainingTime.ToString("hh\\:mm\\:ss");
			this.cropCountdownText.gameObject.SetActive(!this.HoveringCrop.Ripen && this.HoveringCrop.Data.watered);
			this.noWaterIndicator.SetActive(!this.HoveringCrop.Data.watered);
			this.ripenIndicator.SetActive(this.HoveringCrop.Ripen);
		}

		// Token: 0x06001900 RID: 6400 RVA: 0x0005C23F File Offset: 0x0005A43F
		private void SetupOperationInfo()
		{
			this.operationInfo.SetActive(true);
			this.operationNameText.text = this.master.ToolDisplayName;
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x0005C264 File Offset: 0x0005A464
		private void SetupPlantInfo()
		{
			if (!this.master.SeedSelected)
			{
				return;
			}
			this.plantInfo.SetActive(true);
			this.plantingCropNameText.text = this.master.SeedMeta.DisplayName;
			this.plantCostDisplay.Setup(new Cost(new ValueTuple<int, long>[]
			{
				new ValueTuple<int, long>(this.master.PlantingSeedTypeID, 1L)
			}), 1);
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x0005C2D9 File Offset: 0x0005A4D9
		private void HideAll()
		{
			this.plantInfo.SetActive(false);
			this.currentCropInfo.SetActive(false);
			this.operationInfo.SetActive(false);
			this.Hide();
		}

		// Token: 0x04001237 RID: 4663
		[SerializeField]
		private GardenView master;

		// Token: 0x04001238 RID: 4664
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x04001239 RID: 4665
		[SerializeField]
		private GameObject plantInfo;

		// Token: 0x0400123A RID: 4666
		[SerializeField]
		private TextMeshProUGUI plantingCropNameText;

		// Token: 0x0400123B RID: 4667
		[SerializeField]
		private CostDisplay plantCostDisplay;

		// Token: 0x0400123C RID: 4668
		[SerializeField]
		private GameObject currentCropInfo;

		// Token: 0x0400123D RID: 4669
		[SerializeField]
		private TextMeshProUGUI cropNameText;

		// Token: 0x0400123E RID: 4670
		[SerializeField]
		private TextMeshProUGUI cropCountdownText;

		// Token: 0x0400123F RID: 4671
		[SerializeField]
		private GameObject noWaterIndicator;

		// Token: 0x04001240 RID: 4672
		[SerializeField]
		private GameObject ripenIndicator;

		// Token: 0x04001241 RID: 4673
		[SerializeField]
		private GameObject operationInfo;

		// Token: 0x04001242 RID: 4674
		[SerializeField]
		private TextMeshProUGUI operationNameText;
	}
}
