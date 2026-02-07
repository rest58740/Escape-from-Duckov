using System;
using System.Runtime.CompilerServices;
using Duckov.Economy;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.Crops.UI
{
	// Token: 0x02000302 RID: 770
	public class GardenView : View, IPointerClickHandler, IEventSystemHandler, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, ICursorDataProvider
	{
		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001904 RID: 6404 RVA: 0x0005C30D File Offset: 0x0005A50D
		// (set) Token: 0x06001905 RID: 6405 RVA: 0x0005C314 File Offset: 0x0005A514
		public static GardenView Instance { get; private set; }

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001906 RID: 6406 RVA: 0x0005C31C File Offset: 0x0005A51C
		// (set) Token: 0x06001907 RID: 6407 RVA: 0x0005C324 File Offset: 0x0005A524
		public Garden Target { get; private set; }

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001908 RID: 6408 RVA: 0x0005C32D File Offset: 0x0005A52D
		// (set) Token: 0x06001909 RID: 6409 RVA: 0x0005C335 File Offset: 0x0005A535
		public bool SeedSelected { get; private set; }

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x0600190A RID: 6410 RVA: 0x0005C33E File Offset: 0x0005A53E
		// (set) Token: 0x0600190B RID: 6411 RVA: 0x0005C346 File Offset: 0x0005A546
		public int PlantingSeedTypeID
		{
			get
			{
				return this._plantingSeedTypeID;
			}
			private set
			{
				this._plantingSeedTypeID = value;
				this.SeedMeta = ItemAssetsCollection.GetMetaData(value);
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x0600190C RID: 6412 RVA: 0x0005C35B File Offset: 0x0005A55B
		// (set) Token: 0x0600190D RID: 6413 RVA: 0x0005C363 File Offset: 0x0005A563
		public ItemMetaData SeedMeta { get; private set; }

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x0600190E RID: 6414 RVA: 0x0005C36C File Offset: 0x0005A56C
		// (set) Token: 0x0600190F RID: 6415 RVA: 0x0005C374 File Offset: 0x0005A574
		public GardenView.ToolType Tool { get; private set; }

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06001910 RID: 6416 RVA: 0x0005C37D File Offset: 0x0005A57D
		// (set) Token: 0x06001911 RID: 6417 RVA: 0x0005C385 File Offset: 0x0005A585
		public bool Hovering { get; private set; }

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06001912 RID: 6418 RVA: 0x0005C38E File Offset: 0x0005A58E
		// (set) Token: 0x06001913 RID: 6419 RVA: 0x0005C396 File Offset: 0x0005A596
		public Vector2Int HoveringCoord { get; private set; }

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06001914 RID: 6420 RVA: 0x0005C39F File Offset: 0x0005A59F
		// (set) Token: 0x06001915 RID: 6421 RVA: 0x0005C3A7 File Offset: 0x0005A5A7
		public Crop HoveringCrop { get; private set; }

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001916 RID: 6422 RVA: 0x0005C3B0 File Offset: 0x0005A5B0
		public string ToolDisplayName
		{
			get
			{
				switch (this.Tool)
				{
				case GardenView.ToolType.None:
					return "...";
				case GardenView.ToolType.Plant:
					return LocalizationManager.ToPlainText(this.textKey_Plant);
				case GardenView.ToolType.Harvest:
					return LocalizationManager.ToPlainText(this.textKey_Harvest);
				case GardenView.ToolType.Water:
					return LocalizationManager.ToPlainText(this.textKey_Water);
				case GardenView.ToolType.Destroy:
					return LocalizationManager.ToPlainText(this.textKey_Destroy);
				default:
					return "?";
				}
			}
		}

		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x06001917 RID: 6423 RVA: 0x0005C41C File Offset: 0x0005A61C
		// (remove) Token: 0x06001918 RID: 6424 RVA: 0x0005C454 File Offset: 0x0005A654
		public event Action onContextChanged;

		// Token: 0x140000AA RID: 170
		// (add) Token: 0x06001919 RID: 6425 RVA: 0x0005C48C File Offset: 0x0005A68C
		// (remove) Token: 0x0600191A RID: 6426 RVA: 0x0005C4C4 File Offset: 0x0005A6C4
		public event Action onToolChanged;

		// Token: 0x0600191B RID: 6427 RVA: 0x0005C4F9 File Offset: 0x0005A6F9
		protected override void Awake()
		{
			base.Awake();
			this.btn_ChangePlant.onClick.AddListener(new UnityAction(this.OnBtnChangePlantClicked));
			ItemUtilities.OnPlayerItemOperation += this.OnPlayerItemOperation;
			GardenView.Instance = this;
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x0005C534 File Offset: 0x0005A734
		protected override void OnDestroy()
		{
			base.OnDestroy();
			ItemUtilities.OnPlayerItemOperation -= this.OnPlayerItemOperation;
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x0005C54D File Offset: 0x0005A74D
		private void OnDisable()
		{
			if (this.cellHoveringGizmos)
			{
				this.cellHoveringGizmos.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600191E RID: 6430 RVA: 0x0005C56D File Offset: 0x0005A76D
		private void OnPlayerItemOperation()
		{
			if (base.gameObject.activeSelf && this.SeedSelected)
			{
				this.RefreshSeedAmount();
			}
		}

		// Token: 0x0600191F RID: 6431 RVA: 0x0005C58A File Offset: 0x0005A78A
		public static void Show(Garden target)
		{
			GardenView.Instance.Target = target;
			GardenView.Instance.Open(null);
		}

		// Token: 0x06001920 RID: 6432 RVA: 0x0005C5A4 File Offset: 0x0005A7A4
		protected override void OnOpen()
		{
			base.OnOpen();
			if (this.Target == null)
			{
				this.Target = Object.FindObjectOfType<Garden>();
			}
			if (this.Target == null)
			{
				Debug.Log("No Garden instance found. Aborting..");
				base.Close();
			}
			this.fadeGroup.Show();
			this.RefreshSeedInfoDisplay();
			this.EnableCursor();
			this.SetTool(this.Tool);
			this.CenterCamera();
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x0005C617 File Offset: 0x0005A817
		protected override void OnClose()
		{
			base.OnClose();
			this.cropSelector.Hide();
			this.fadeGroup.Hide();
			this.ReleaseCursor();
		}

		// Token: 0x06001922 RID: 6434 RVA: 0x0005C63B File Offset: 0x0005A83B
		private void EnableCursor()
		{
			CursorManager.Register(this);
		}

		// Token: 0x06001923 RID: 6435 RVA: 0x0005C643 File Offset: 0x0005A843
		private void ReleaseCursor()
		{
			CursorManager.Unregister(this);
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x0005C64C File Offset: 0x0005A84C
		private void ChangeCursor()
		{
			CursorManager.NotifyRefresh();
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x0005C653 File Offset: 0x0005A853
		private void Update()
		{
			this.UpdateContext();
			this.UpdateCursor3D();
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x0005C661 File Offset: 0x0005A861
		private void OnBtnChangePlantClicked()
		{
			this.cropSelector.Show();
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0005C670 File Offset: 0x0005A870
		private void OnContextChanged()
		{
			Action action = this.onContextChanged;
			if (action != null)
			{
				action();
			}
			this.RefreshHoveringGizmos();
			this.RefreshCursor();
			if (this.dragging && this.Hovering)
			{
				this.ExecuteTool(this.HoveringCoord);
			}
			this.ChangeCursor();
			this.RefreshCursor3DActive();
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x0005C6C4 File Offset: 0x0005A8C4
		private void RefreshCursor()
		{
			this.cursorIcon.gameObject.SetActive(false);
			this.cursorAmountDisplay.gameObject.SetActive(false);
			this.cursorItemDisplay.gameObject.SetActive(false);
			switch (this.Tool)
			{
			case GardenView.ToolType.None:
				break;
			case GardenView.ToolType.Plant:
				this.cursorAmountDisplay.gameObject.SetActive(this.SeedSelected);
				this.cursorItemDisplay.gameObject.SetActive(this.SeedSelected);
				this.cursorIcon.sprite = this.iconPlant;
				return;
			case GardenView.ToolType.Harvest:
				this.cursorIcon.gameObject.SetActive(true);
				this.cursorIcon.sprite = this.iconHarvest;
				return;
			case GardenView.ToolType.Water:
				this.cursorIcon.gameObject.SetActive(true);
				this.cursorIcon.sprite = this.iconWater;
				return;
			case GardenView.ToolType.Destroy:
				this.cursorIcon.gameObject.SetActive(true);
				this.cursorIcon.sprite = this.iconDestroy;
				break;
			default:
				return;
			}
		}

		// Token: 0x06001929 RID: 6441 RVA: 0x0005C7CC File Offset: 0x0005A9CC
		private void RefreshHoveringGizmos()
		{
			if (!this.cellHoveringGizmos)
			{
				return;
			}
			if (!this.Hovering || !base.enabled)
			{
				this.cellHoveringGizmos.gameObject.SetActive(false);
				return;
			}
			this.cellHoveringGizmos.gameObject.SetActive(true);
			this.cellHoveringGizmos.SetParent(null);
			this.cellHoveringGizmos.localScale = Vector3.one;
			this.cellHoveringGizmos.position = this.Target.CoordToWorldPosition(this.HoveringCoord);
			this.cellHoveringGizmos.rotation = Quaternion.LookRotation(-Vector3.up);
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x0005C86C File Offset: 0x0005AA6C
		public void SetTool(GardenView.ToolType action)
		{
			this.Tool = action;
			this.OnContextChanged();
			this.plantModePanel.SetActive(action == GardenView.ToolType.Plant);
			Action action2 = this.onToolChanged;
			if (action2 != null)
			{
				action2();
			}
			this.RefreshSeedAmount();
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x0005C8A1 File Offset: 0x0005AAA1
		private CursorData GetCursorByTool(GardenView.ToolType action)
		{
			return null;
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x0005C8A4 File Offset: 0x0005AAA4
		private void UpdateContext()
		{
			bool hovering = this.Hovering;
			Crop hoveringCrop = this.HoveringCrop;
			Vector2Int hoveringCoord = this.HoveringCoord;
			Vector2Int? pointingCoord = this.GetPointingCoord();
			if (pointingCoord == null)
			{
				this.HoveringCrop = null;
				return;
			}
			this.HoveringCoord = pointingCoord.Value;
			this.HoveringCrop = this.Target[this.HoveringCoord];
			this.Hovering = this.hoveringBG;
			if (!this.HoveringCrop)
			{
				this.Hovering &= this.Target.IsCoordValid(this.HoveringCoord);
			}
			if (hovering != this.HoveringCrop || hoveringCrop != this.HoveringCrop || hoveringCoord != this.HoveringCoord)
			{
				this.OnContextChanged();
			}
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x0005C96C File Offset: 0x0005AB6C
		private void UpdateCursor3D()
		{
			Vector3 vector;
			bool flag = this.TryPointerOnPlanePoint(UIInputManager.Point, out vector);
			this.show3DCursor = (flag && this.Hovering);
			this.cursor3DTransform.gameObject.SetActive(this.show3DCursor);
			if (!flag)
			{
				return;
			}
			Vector3 position = this.cursor3DTransform.position;
			Vector3 vector2 = vector + this.cursor3DOffset;
			Vector3 position2;
			if (this.show3DCursor)
			{
				position2 = Vector3.Lerp(position, vector2, 0.25f);
			}
			else
			{
				position2 = vector2;
			}
			this.cursor3DTransform.position = position2;
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x0005C9F4 File Offset: 0x0005ABF4
		private void RefreshCursor3DActive()
		{
			this.cursor3D_Plant.SetActive(this.<RefreshCursor3DActive>g__ShouldShowCursor|99_0(GardenView.ToolType.Plant));
			this.cursor3D_Water.SetActive(this.<RefreshCursor3DActive>g__ShouldShowCursor|99_0(GardenView.ToolType.Water));
			this.cursor3D_Harvest.SetActive(this.<RefreshCursor3DActive>g__ShouldShowCursor|99_0(GardenView.ToolType.Harvest));
			this.cursor3D_Destory.SetActive(this.<RefreshCursor3DActive>g__ShouldShowCursor|99_0(GardenView.ToolType.Destroy));
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x0005CA49 File Offset: 0x0005AC49
		public void SelectSeed(int seedTypeID)
		{
			this.PlantingSeedTypeID = seedTypeID;
			if (seedTypeID > 0)
			{
				this.SeedSelected = true;
			}
			this.RefreshSeedInfoDisplay();
			this.OnContextChanged();
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x0005CA6C File Offset: 0x0005AC6C
		private void RefreshSeedInfoDisplay()
		{
			if (this.SeedSelected)
			{
				this.seedItemDisplay.Setup(this.PlantingSeedTypeID);
				this.cursorItemDisplay.Setup(this.PlantingSeedTypeID);
			}
			this.seedItemDisplay.gameObject.SetActive(this.SeedSelected);
			this.seedItemPlaceHolder.gameObject.SetActive(!this.SeedSelected);
			this.RefreshSeedAmount();
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x0005CAD8 File Offset: 0x0005ACD8
		private bool TryPointerOnPlanePoint(Vector2 pointerPos, out Vector3 planePoint)
		{
			planePoint = default(Vector3);
			if (this.Target == null)
			{
				return false;
			}
			Ray ray = RectTransformUtility.ScreenPointToRay(Camera.main, pointerPos);
			Plane plane;
			plane..ctor(this.Target.transform.up, this.Target.transform.position);
			float num;
			if (!plane.Raycast(ray, ref num))
			{
				return false;
			}
			planePoint = ray.GetPoint(num);
			return true;
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x0005CB4C File Offset: 0x0005AD4C
		private bool TryPointerPosToCoord(Vector2 pointerPos, out Vector2Int result)
		{
			result = default(Vector2Int);
			if (this.Target == null)
			{
				return false;
			}
			Ray ray = RectTransformUtility.ScreenPointToRay(Camera.main, pointerPos);
			Plane plane;
			plane..ctor(this.Target.transform.up, this.Target.transform.position);
			float num;
			if (!plane.Raycast(ray, ref num))
			{
				return false;
			}
			Vector3 point = ray.GetPoint(num);
			result = this.Target.WorldPositionToCoord(point);
			return true;
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x0005CBD0 File Offset: 0x0005ADD0
		private Vector2Int? GetPointingCoord()
		{
			Vector2Int value;
			if (!this.TryPointerPosToCoord(UIInputManager.Point, out value))
			{
				return null;
			}
			return new Vector2Int?(value);
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x0005CBFC File Offset: 0x0005ADFC
		public void OnPointerClick(PointerEventData eventData)
		{
			Vector2Int coord;
			if (!this.TryPointerPosToCoord(eventData.position, out coord))
			{
				return;
			}
			this.ExecuteTool(coord);
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x0005CC24 File Offset: 0x0005AE24
		private void ExecuteTool(Vector2Int coord)
		{
			switch (this.Tool)
			{
			case GardenView.ToolType.None:
				break;
			case GardenView.ToolType.Plant:
				this.CropActionPlant(coord);
				return;
			case GardenView.ToolType.Harvest:
				this.CropActionHarvest(coord);
				return;
			case GardenView.ToolType.Water:
				this.CropActionWater(coord);
				return;
			case GardenView.ToolType.Destroy:
				this.CropActionDestroy(coord);
				break;
			default:
				return;
			}
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x0005CC74 File Offset: 0x0005AE74
		private void CropActionDestroy(Vector2Int coord)
		{
			Crop crop = this.Target[coord];
			if (crop == null)
			{
				return;
			}
			if (crop.Ripen)
			{
				crop.Harvest();
				return;
			}
			crop.DestroyCrop();
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x0005CCB0 File Offset: 0x0005AEB0
		private void CropActionWater(Vector2Int coord)
		{
			Crop crop = this.Target[coord];
			if (crop == null)
			{
				return;
			}
			crop.Water();
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x0005CCDC File Offset: 0x0005AEDC
		private void CropActionHarvest(Vector2Int coord)
		{
			Crop crop = this.Target[coord];
			if (crop == null)
			{
				return;
			}
			crop.Harvest();
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x0005CD08 File Offset: 0x0005AF08
		private void CropActionPlant(Vector2Int coord)
		{
			if (!this.Target.IsCoordValid(coord))
			{
				return;
			}
			if (this.Target[coord] != null)
			{
				return;
			}
			CropInfo? cropInfoFromSeedType = this.GetCropInfoFromSeedType(this.PlantingSeedTypeID);
			if (cropInfoFromSeedType == null)
			{
				return;
			}
			Cost cost = new Cost(new ValueTuple<int, long>[]
			{
				new ValueTuple<int, long>(this.PlantingSeedTypeID, 1L)
			});
			if (!cost.Pay(true, true))
			{
				return;
			}
			this.Target.Plant(coord, cropInfoFromSeedType.Value.id);
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x0005CD98 File Offset: 0x0005AF98
		private CropInfo? GetCropInfoFromSeedType(int plantingSeedTypeID)
		{
			SeedInfo seedInfo = CropDatabase.GetSeedInfo(plantingSeedTypeID);
			if (seedInfo.cropIDs == null)
			{
				return null;
			}
			if (seedInfo.cropIDs.Count <= 0)
			{
				return null;
			}
			return CropDatabase.GetCropInfo(seedInfo.GetRandomCropID());
		}

		// Token: 0x0600193B RID: 6459 RVA: 0x0005CDE4 File Offset: 0x0005AFE4
		public void OnPointerMove(PointerEventData eventData)
		{
			if (eventData.pointerCurrentRaycast.gameObject == this.mainEventReceiver)
			{
				this.hoveringBG = true;
				return;
			}
			this.hoveringBG = false;
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x0005CE1C File Offset: 0x0005B01C
		private void RefreshSeedAmount()
		{
			if (this.SeedSelected)
			{
				int itemCount = ItemUtilities.GetItemCount(this.PlantingSeedTypeID);
				this.seedAmount = itemCount;
				string text = string.Format("x{0}", itemCount);
				this.seedAmountText.text = text;
				this.cursorAmountDisplay.text = text;
				return;
			}
			this.seedAmountText.text = "";
			this.cursorAmountDisplay.text = "";
			this.seedAmount = 0;
		}

		// Token: 0x0600193D RID: 6461 RVA: 0x0005CE95 File Offset: 0x0005B095
		public void OnPointerDown(PointerEventData eventData)
		{
			this.dragging = true;
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x0005CE9E File Offset: 0x0005B09E
		public void OnPointerUp(PointerEventData eventData)
		{
			this.dragging = false;
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x0005CEA7 File Offset: 0x0005B0A7
		public void OnPointerExit(PointerEventData eventData)
		{
			this.dragging = false;
		}

		// Token: 0x06001940 RID: 6464 RVA: 0x0005CEB0 File Offset: 0x0005B0B0
		private void UpdateCamera()
		{
			this.cameraRig.transform.position = this.camFocusPos;
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x0005CEC8 File Offset: 0x0005B0C8
		private void CenterCamera()
		{
			if (this.Target == null)
			{
				return;
			}
			this.camFocusPos = this.Target.transform.TransformPoint(this.Target.cameraRigCenter);
			this.UpdateCamera();
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x0005CF00 File Offset: 0x0005B100
		public CursorData GetCursorData()
		{
			return this.GetCursorByTool(this.Tool);
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x0005CF68 File Offset: 0x0005B168
		[CompilerGenerated]
		private bool <RefreshCursor3DActive>g__ShouldShowCursor|99_0(GardenView.ToolType toolType)
		{
			if (this.Tool != toolType)
			{
				return false;
			}
			if (!this.Hovering)
			{
				return false;
			}
			switch (toolType)
			{
			case GardenView.ToolType.None:
				return false;
			case GardenView.ToolType.Plant:
				return this.SeedSelected && this.seedAmount > 0 && !this.HoveringCrop;
			case GardenView.ToolType.Harvest:
				return this.HoveringCrop && this.HoveringCrop.Ripen;
			case GardenView.ToolType.Water:
				return this.HoveringCrop;
			case GardenView.ToolType.Destroy:
				return this.HoveringCrop;
			default:
				return false;
			}
		}

		// Token: 0x04001244 RID: 4676
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001245 RID: 4677
		[SerializeField]
		private GameObject mainEventReceiver;

		// Token: 0x04001246 RID: 4678
		[SerializeField]
		private Button btn_ChangePlant;

		// Token: 0x04001247 RID: 4679
		[SerializeField]
		private GameObject plantModePanel;

		// Token: 0x04001248 RID: 4680
		[SerializeField]
		private ItemMetaDisplay seedItemDisplay;

		// Token: 0x04001249 RID: 4681
		[SerializeField]
		private GameObject seedItemPlaceHolder;

		// Token: 0x0400124A RID: 4682
		[SerializeField]
		private TextMeshProUGUI seedAmountText;

		// Token: 0x0400124B RID: 4683
		[SerializeField]
		private GardenViewCropSelector cropSelector;

		// Token: 0x0400124C RID: 4684
		[SerializeField]
		private Transform cellHoveringGizmos;

		// Token: 0x0400124D RID: 4685
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey_Plant = "Garden_Plant";

		// Token: 0x0400124E RID: 4686
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey_Harvest = "Garden_Harvest";

		// Token: 0x0400124F RID: 4687
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey_Destroy = "Garden_Destroy";

		// Token: 0x04001250 RID: 4688
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey_Water = "Garden_Water";

		// Token: 0x04001251 RID: 4689
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey_TargetOccupied = "Garden_TargetOccupied";

		// Token: 0x04001252 RID: 4690
		[SerializeField]
		private Transform cameraRig;

		// Token: 0x04001253 RID: 4691
		[SerializeField]
		private Image cursorIcon;

		// Token: 0x04001254 RID: 4692
		[SerializeField]
		private TextMeshProUGUI cursorAmountDisplay;

		// Token: 0x04001255 RID: 4693
		[SerializeField]
		private ItemMetaDisplay cursorItemDisplay;

		// Token: 0x04001256 RID: 4694
		[SerializeField]
		private Sprite iconPlant;

		// Token: 0x04001257 RID: 4695
		[SerializeField]
		private Sprite iconHarvest;

		// Token: 0x04001258 RID: 4696
		[SerializeField]
		private Sprite iconWater;

		// Token: 0x04001259 RID: 4697
		[SerializeField]
		private Sprite iconDestroy;

		// Token: 0x0400125A RID: 4698
		[SerializeField]
		private CursorData cursorPlant;

		// Token: 0x0400125B RID: 4699
		[SerializeField]
		private CursorData cursorHarvest;

		// Token: 0x0400125C RID: 4700
		[SerializeField]
		private CursorData cursorWater;

		// Token: 0x0400125D RID: 4701
		[SerializeField]
		private CursorData cursorDestroy;

		// Token: 0x0400125E RID: 4702
		[SerializeField]
		private Transform cursor3DTransform;

		// Token: 0x0400125F RID: 4703
		[SerializeField]
		private Vector3 cursor3DOffset = Vector3.up;

		// Token: 0x04001260 RID: 4704
		[SerializeField]
		private GameObject cursor3D_Plant;

		// Token: 0x04001261 RID: 4705
		[SerializeField]
		private GameObject cursor3D_Harvest;

		// Token: 0x04001262 RID: 4706
		[SerializeField]
		private GameObject cursor3D_Water;

		// Token: 0x04001263 RID: 4707
		[SerializeField]
		private GameObject cursor3D_Destory;

		// Token: 0x04001264 RID: 4708
		private Vector3 camFocusPos;

		// Token: 0x04001267 RID: 4711
		private int _plantingSeedTypeID;

		// Token: 0x0400126F RID: 4719
		private bool enabledCursor;

		// Token: 0x04001270 RID: 4720
		private bool show3DCursor;

		// Token: 0x04001271 RID: 4721
		private bool hoveringBG;

		// Token: 0x04001272 RID: 4722
		private int seedAmount;

		// Token: 0x04001273 RID: 4723
		private bool dragging;

		// Token: 0x020005AA RID: 1450
		public enum ToolType
		{
			// Token: 0x040020B0 RID: 8368
			None,
			// Token: 0x040020B1 RID: 8369
			Plant,
			// Token: 0x040020B2 RID: 8370
			Harvest,
			// Token: 0x040020B3 RID: 8371
			Water,
			// Token: 0x040020B4 RID: 8372
			Destroy
		}
	}
}
