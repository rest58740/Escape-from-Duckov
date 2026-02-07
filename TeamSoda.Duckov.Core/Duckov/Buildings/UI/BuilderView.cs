using System;
using Cinemachine;
using Cinemachine.Utility;
using Cysharp.Threading.Tasks;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Duckov.Buildings.UI
{
	// Token: 0x02000330 RID: 816
	public class BuilderView : View, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001B2F RID: 6959 RVA: 0x00062B65 File Offset: 0x00060D65
		public static BuilderView Instance
		{
			get
			{
				return View.GetViewInstance<BuilderView>();
			}
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x00062B6C File Offset: 0x00060D6C
		public void SetupAndShow(BuildingArea targetArea)
		{
			this.targetArea = targetArea;
			base.Open(null);
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x00062B7C File Offset: 0x00060D7C
		protected override void Awake()
		{
			base.Awake();
			this.input_Rotate.action.actionMap.Enable();
			this.input_MoveCamera.action.actionMap.Enable();
			this.selectionPanel.onButtonSelected += this.OnButtonSelected;
			this.selectionPanel.onRecycleRequested += this.OnRecycleRequested;
			BuildingManager.OnBuildingListChanged += this.OnBuildingListChanged;
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x00062BF8 File Offset: 0x00060DF8
		private void OnRecycleRequested(BuildingBtnEntry entry)
		{
			BuildingManager.ReturnBuildingsOfType(entry.Info.id, null).Forget<int>();
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x00062C10 File Offset: 0x00060E10
		protected override void OnDestroy()
		{
			base.OnDestroy();
			BuildingManager.OnBuildingListChanged -= this.OnBuildingListChanged;
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x00062C29 File Offset: 0x00060E29
		private void OnBuildingListChanged()
		{
			this.selectionPanel.Refresh();
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x00062C38 File Offset: 0x00060E38
		private void OnButtonSelected(BuildingBtnEntry entry)
		{
			if (!entry.CostEnough)
			{
				this.NotifyCostNotEnough(entry);
				return;
			}
			if (entry.Info.ReachedAmountLimit)
			{
				return;
			}
			this.BeginPlacing(entry.Info);
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x00062C74 File Offset: 0x00060E74
		private void NotifyCostNotEnough(BuildingBtnEntry entry)
		{
			Debug.Log("Resource not enough " + entry.Info.DisplayName);
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x00062C9E File Offset: 0x00060E9E
		private void SetMode(BuilderView.Mode mode)
		{
			this.placingModeInputIndicator.SetActive(false);
			this.OnExitMode(this.mode);
			this.mode = mode;
			switch (mode)
			{
			case BuilderView.Mode.None:
			case BuilderView.Mode.Destroying:
				break;
			case BuilderView.Mode.Placing:
				this.placingModeInputIndicator.SetActive(true);
				break;
			default:
				return;
			}
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x00062CDE File Offset: 0x00060EDE
		private void OnExitMode(BuilderView.Mode mode)
		{
			this.contextMenu.Hide();
			switch (mode)
			{
			case BuilderView.Mode.None:
			case BuilderView.Mode.Destroying:
				break;
			case BuilderView.Mode.Placing:
				this.OnExitPlacing();
				break;
			default:
				return;
			}
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x00062D04 File Offset: 0x00060F04
		public void BeginPlacing(BuildingInfo info)
		{
			if (this.previewBuilding != null)
			{
				Object.Destroy(this.previewBuilding.gameObject);
			}
			this.placingBuildingInfo = info;
			this.SetMode(BuilderView.Mode.Placing);
			if (info.Prefab == null)
			{
				Debug.LogError("建筑 " + info.DisplayName + " 没有prefab");
			}
			this.previewBuilding = Object.Instantiate<Building>(info.Prefab);
			if (this.previewBuilding.ID != info.id)
			{
				Debug.LogError("建筑 " + info.DisplayName + " 的 prefab 上的 ID 设置错误");
			}
			this.SetupPreview(this.previewBuilding);
			this.UpdatePlacing();
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x00062DBE File Offset: 0x00060FBE
		public void BeginDestroying()
		{
			this.SetMode(BuilderView.Mode.Destroying);
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x00062DC7 File Offset: 0x00060FC7
		private void SetupPreview(Building previewBuilding)
		{
			if (previewBuilding == null)
			{
				return;
			}
			previewBuilding.SetupPreview();
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x00062DD9 File Offset: 0x00060FD9
		private void OnExitPlacing()
		{
			if (this.previewBuilding != null)
			{
				Object.Destroy(this.previewBuilding.gameObject);
			}
			GridDisplay.HidePreview();
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x00062E00 File Offset: 0x00061000
		private void Update()
		{
			switch (this.mode)
			{
			case BuilderView.Mode.None:
				this.UpdateNone();
				break;
			case BuilderView.Mode.Placing:
				this.UpdatePlacing();
				break;
			case BuilderView.Mode.Destroying:
				this.UpdateDestroying();
				break;
			}
			this.UpdateCamera();
			this.UpdateContextMenuIndicator();
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x00062E4C File Offset: 0x0006104C
		private unsafe void UpdateContextMenuIndicator()
		{
			Vector2Int coord;
			this.TryGetPointingCoord(out coord, null);
			bool flag = this.targetArea.GetBuildingInstanceAt(coord);
			bool isActiveAndEnabled = this.contextMenu.isActiveAndEnabled;
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.followCursorUI.parent as RectTransform, *Mouse.current.position.value, null, ref vector);
			this.followCursorUI.localPosition = vector;
			bool flag2 = flag && !isActiveAndEnabled;
			if (flag2 && !this.hoveringBuildingFadeGroup.IsShown)
			{
				this.hoveringBuildingFadeGroup.Show();
			}
			if (!flag2 && this.hoveringBuildingFadeGroup.IsShown)
			{
				this.hoveringBuildingFadeGroup.Hide();
			}
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x00062EFC File Offset: 0x000610FC
		private void UpdateNone()
		{
			if (this.input_RequestContextMenu.action.WasPressedThisFrame())
			{
				Vector2Int coord;
				if (!this.TryGetPointingCoord(out coord, null))
				{
					return;
				}
				Building buildingInstanceAt = this.targetArea.GetBuildingInstanceAt(coord);
				if (buildingInstanceAt == null)
				{
					this.contextMenu.Hide();
					return;
				}
				this.contextMenu.Setup(buildingInstanceAt);
			}
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x00062F58 File Offset: 0x00061158
		private void UpdateDestroying()
		{
			Vector2Int coord;
			if (!this.TryGetPointingCoord(out coord, null))
			{
				GridDisplay.HidePreview();
				return;
			}
			BuildingManager.BuildingData buildingAt = this.targetArea.AreaData.GetBuildingAt(coord);
			if (buildingAt == null)
			{
				GridDisplay.HidePreview();
				return;
			}
			this.gridDisplay.SetBuildingPreviewCoord(buildingAt.Coord, buildingAt.Dimensions, buildingAt.Rotation, false);
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x00062FB0 File Offset: 0x000611B0
		private void ConfirmDestroy()
		{
			Vector2Int coord;
			if (!this.TryGetPointingCoord(out coord, null))
			{
				return;
			}
			BuildingManager.BuildingData buildingAt = this.targetArea.AreaData.GetBuildingAt(coord);
			if (buildingAt == null)
			{
				return;
			}
			BuildingManager.ReturnBuilding(buildingAt.GUID, null).Forget<bool>();
			this.SetMode(BuilderView.Mode.None);
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x00062FF8 File Offset: 0x000611F8
		private void ConfirmPlacement()
		{
			if (this.previewBuilding == null)
			{
				Debug.Log("No Previewing Building");
				return;
			}
			Vector2Int coord;
			if (!this.TryGetPointingCoord(out coord, this.previewBuilding))
			{
				this.previewBuilding.gameObject.SetActive(false);
				Debug.Log("Mouse Not in Plane!");
				return;
			}
			if (!this.IsValidPlacement(this.previewBuilding.Dimensions, this.previewRotation, coord))
			{
				Debug.Log("Invalid Placement!");
				return;
			}
			BuildingManager.BuyAndPlace(this.targetArea.AreaID, this.previewBuilding.ID, coord, this.previewRotation);
			this.SetMode(BuilderView.Mode.None);
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x0006309C File Offset: 0x0006129C
		private void UpdatePlacing()
		{
			if (this.previewBuilding)
			{
				Vector2Int coord;
				if (!this.TryGetPointingCoord(out coord, this.previewBuilding))
				{
					this.previewBuilding.gameObject.SetActive(false);
					return;
				}
				bool validPlacement = this.IsValidPlacement(this.previewBuilding.Dimensions, this.previewRotation, coord);
				this.gridDisplay.SetBuildingPreviewCoord(coord, this.previewBuilding.Dimensions, this.previewRotation, validPlacement);
				this.ShowPreview(coord);
				if (this.input_Rotate.action.WasPressedThisFrame())
				{
					float num = this.input_Rotate.action.ReadValue<float>();
					this.previewRotation = (BuildingRotation)(((float)this.previewRotation + num + 4f) % 4f);
				}
				if (this.input_RequestContextMenu.action.WasPressedThisFrame())
				{
					this.SetMode(BuilderView.Mode.None);
					return;
				}
			}
			else
			{
				this.SetMode(BuilderView.Mode.None);
			}
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x0006317C File Offset: 0x0006137C
		private void ShowPreview(Vector2Int coord)
		{
			Vector3 position = this.targetArea.CoordToWorldPosition(coord, this.previewBuilding.Dimensions, this.previewRotation);
			this.previewBuilding.transform.position = position;
			this.previewBuilding.gameObject.SetActive(true);
			Quaternion quaternion = Quaternion.Euler(new Vector3(0f, (float)((BuildingRotation)90 * this.previewRotation), 0f));
			this.previewBuilding.transform.rotation = this.targetArea.transform.rotation * quaternion;
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x00063210 File Offset: 0x00061410
		public bool TryGetPointingCoord(out Vector2Int coord, Building previewBuilding = null)
		{
			coord = default(Vector2Int);
			Ray pointRay = UIInputManager.GetPointRay();
			float num;
			if (!this.targetArea.Plane.Raycast(pointRay, ref num))
			{
				return false;
			}
			Vector3 point = pointRay.GetPoint(num);
			if (previewBuilding != null)
			{
				coord = this.targetArea.CursorToCoord(point, previewBuilding.Dimensions, this.previewRotation);
				return true;
			}
			coord = this.targetArea.CursorToCoord(point, Vector2Int.one, BuildingRotation.Zero);
			return true;
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x00063294 File Offset: 0x00061494
		private bool IsValidPlacement(Vector2Int dimensions, BuildingRotation rotation, Vector2Int coord)
		{
			return this.targetArea.IsPlacementWithinRange(dimensions, rotation, coord) && !this.targetArea.AreaData.Collide(dimensions, rotation, coord) && !this.targetArea.PhysicsCollide(dimensions, rotation, coord, 0f, 2f);
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x000632E8 File Offset: 0x000614E8
		protected override void OnOpen()
		{
			base.OnOpen();
			this.SetMode(BuilderView.Mode.None);
			this.fadeGroup.Show();
			this.selectionPanel.Setup(this.targetArea);
			this.gridDisplay.Setup(this.targetArea);
			this.cameraCursor = this.targetArea.transform.position;
			this.UpdateCamera();
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x0006334B File Offset: 0x0006154B
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
			GridDisplay.Close();
			if (this.previewBuilding != null)
			{
				Object.Destroy(this.previewBuilding.gameObject);
			}
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x00063384 File Offset: 0x00061584
		private void UpdateCamera()
		{
			if (this.input_MoveCamera.action.IsPressed())
			{
				Vector2 vector = this.input_MoveCamera.action.ReadValue<Vector2>();
				Transform transform = this.vcam.transform;
				float num = Mathf.Abs(Vector3.Dot(transform.forward, Vector3.up));
				float num2 = Mathf.Abs(Vector3.Dot(transform.up, Vector3.up));
				Vector3 vector2 = UnityVectorExtensions.ProjectOntoPlane((num > num2) ? transform.up : transform.forward, Vector3.up);
				Vector3 vector3 = UnityVectorExtensions.ProjectOntoPlane(transform.right, Vector3.up);
				this.cameraCursor += (vector3 * vector.x + vector2 * vector.y) * this.cameraSpeed * Time.unscaledDeltaTime;
				this.cameraCursor.x = Mathf.Clamp(this.cameraCursor.x, this.targetArea.transform.position.x - (float)this.targetArea.Size.x, this.targetArea.transform.position.x + (float)this.targetArea.Size.x);
				this.cameraCursor.z = Mathf.Clamp(this.cameraCursor.z, this.targetArea.transform.position.z - (float)this.targetArea.Size.y, this.targetArea.transform.position.z + (float)this.targetArea.Size.y);
			}
			this.vcam.transform.position = this.cameraCursor + Quaternion.Euler(0f, this.yaw, 0f) * Quaternion.Euler(this.pitch, 0f, 0f) * Vector3.forward * this.cameraDistance;
			this.vcam.transform.LookAt(this.cameraCursor, Vector3.up);
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x000635BC File Offset: 0x000617BC
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == null)
			{
				this.contextMenu.Hide();
				BuilderView.Mode mode = this.mode;
				if (mode == BuilderView.Mode.Placing)
				{
					this.ConfirmPlacement();
					return;
				}
				if (mode != BuilderView.Mode.Destroying)
				{
					return;
				}
				this.ConfirmDestroy();
			}
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x000635F9 File Offset: 0x000617F9
		public static void Show(BuildingArea target)
		{
			BuilderView.Instance.SetupAndShow(target);
		}

		// Token: 0x0400137D RID: 4989
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400137E RID: 4990
		[SerializeField]
		private BuildingSelectionPanel selectionPanel;

		// Token: 0x0400137F RID: 4991
		[SerializeField]
		private BuildingContextMenu contextMenu;

		// Token: 0x04001380 RID: 4992
		[SerializeField]
		private GameObject placingModeInputIndicator;

		// Token: 0x04001381 RID: 4993
		[SerializeField]
		private RectTransform followCursorUI;

		// Token: 0x04001382 RID: 4994
		[SerializeField]
		private FadeGroup hoveringBuildingFadeGroup;

		// Token: 0x04001383 RID: 4995
		[SerializeField]
		private CinemachineVirtualCamera vcam;

		// Token: 0x04001384 RID: 4996
		[SerializeField]
		private float cameraSpeed = 10f;

		// Token: 0x04001385 RID: 4997
		[SerializeField]
		private float pitch = 45f;

		// Token: 0x04001386 RID: 4998
		[SerializeField]
		private float cameraDistance = 10f;

		// Token: 0x04001387 RID: 4999
		[SerializeField]
		private float yaw = -45f;

		// Token: 0x04001388 RID: 5000
		[SerializeField]
		private Vector3 cameraCursor;

		// Token: 0x04001389 RID: 5001
		[SerializeField]
		private BuildingInfo placingBuildingInfo;

		// Token: 0x0400138A RID: 5002
		[SerializeField]
		private InputActionReference input_Rotate;

		// Token: 0x0400138B RID: 5003
		[SerializeField]
		private InputActionReference input_RequestContextMenu;

		// Token: 0x0400138C RID: 5004
		[SerializeField]
		private InputActionReference input_MoveCamera;

		// Token: 0x0400138D RID: 5005
		[SerializeField]
		private GridDisplay gridDisplay;

		// Token: 0x0400138E RID: 5006
		[SerializeField]
		private BuildingArea targetArea;

		// Token: 0x0400138F RID: 5007
		[SerializeField]
		private BuilderView.Mode mode;

		// Token: 0x04001390 RID: 5008
		private Building previewBuilding;

		// Token: 0x04001391 RID: 5009
		[SerializeField]
		private BuildingRotation previewRotation;

		// Token: 0x020005DF RID: 1503
		private enum Mode
		{
			// Token: 0x0400215C RID: 8540
			None,
			// Token: 0x0400215D RID: 8541
			Placing,
			// Token: 0x0400215E RID: 8542
			Destroying
		}
	}
}
