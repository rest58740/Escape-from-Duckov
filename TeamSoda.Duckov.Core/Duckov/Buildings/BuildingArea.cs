using System;
using System.Collections.Generic;
using Drawing;
using Duckov.Achievements;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x02000328 RID: 808
	public class BuildingArea : MonoBehaviour, IDrawGizmos
	{
		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001ABE RID: 6846 RVA: 0x00061162 File Offset: 0x0005F362
		public string AreaID
		{
			get
			{
				return this.areaID;
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001ABF RID: 6847 RVA: 0x0006116A File Offset: 0x0005F36A
		public Vector2Int Size
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001AC0 RID: 6848 RVA: 0x00061172 File Offset: 0x0005F372
		public Vector2Int LowerLeftCorner
		{
			get
			{
				return this.CenterCoord - (this.size - Vector2Int.one);
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x0006118F File Offset: 0x0005F38F
		private Vector2Int CenterCoord
		{
			get
			{
				return new Vector2Int(Mathf.RoundToInt(base.transform.position.x), Mathf.RoundToInt(base.transform.position.z));
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001AC2 RID: 6850 RVA: 0x000611C0 File Offset: 0x0005F3C0
		private int Width
		{
			get
			{
				return this.size.x;
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x000611CD File Offset: 0x0005F3CD
		private int Height
		{
			get
			{
				return this.size.y;
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001AC4 RID: 6852 RVA: 0x000611DA File Offset: 0x0005F3DA
		public BuildingManager.BuildingAreaData AreaData
		{
			get
			{
				return BuildingManager.GetOrCreateAreaData(this.AreaID);
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06001AC5 RID: 6853 RVA: 0x000611E7 File Offset: 0x0005F3E7
		public Plane Plane
		{
			get
			{
				return new Plane(base.transform.up, base.transform.position);
			}
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x00061204 File Offset: 0x0005F404
		private void Awake()
		{
			BuildingManager.OnBuildingBuilt += this.OnBuildingBuilt;
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x00061217 File Offset: 0x0005F417
		private void OnDestroy()
		{
			BuildingManager.OnBuildingBuilt -= this.OnBuildingBuilt;
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x0006122C File Offset: 0x0005F42C
		private void OnBuildingBuilt(int guid)
		{
			BuildingManager.BuildingData buildingData = BuildingManager.GetBuildingData(guid, null);
			if (buildingData == null)
			{
				return;
			}
			this.Display(buildingData);
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x0006124C File Offset: 0x0005F44C
		private void Start()
		{
			this.RepaintAll();
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x00061254 File Offset: 0x0005F454
		public void DrawGizmos()
		{
			if (!GizmoContext.InSelection(this))
			{
				return;
			}
			int num = this.CenterCoord.x - (this.size.x - 1);
			int num2 = this.CenterCoord.x + (this.size.x - 1) + 1;
			int num3 = this.CenterCoord.y - (this.size.y - 1);
			int num4 = this.CenterCoord.y + (this.size.y - 1) + 1;
			Vector3 vector;
			vector..ctor(-0.5f, 0f, -0.5f);
			for (int i = num; i <= num2; i++)
			{
				Draw.Line(new Vector3((float)i, 0f, (float)num3) + vector, new Vector3((float)i, 0f, (float)num4) + vector);
			}
			for (int j = num3; j <= num4; j++)
			{
				Draw.Line(new Vector3((float)num, 0f, (float)j) + vector, new Vector3((float)num2, 0f, (float)j) + vector);
			}
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x00061380 File Offset: 0x0005F580
		public bool IsPlacementWithinRange(Vector2Int dimensions, BuildingRotation rotation, Vector2Int coord)
		{
			if (rotation % BuildingRotation.Half > BuildingRotation.Zero)
			{
				dimensions..ctor(dimensions.y, dimensions.x);
			}
			coord -= this.CenterCoord;
			return coord.x > -this.size.x && coord.y > -this.size.y && coord.x + dimensions.x <= this.size.x && coord.y + dimensions.y <= this.size.y;
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x00061420 File Offset: 0x0005F620
		public Vector2Int CursorToCoord(Vector3 point, Vector2Int dimensions, BuildingRotation rotation)
		{
			if (rotation % BuildingRotation.Half > BuildingRotation.Zero)
			{
				dimensions..ctor(dimensions.y, dimensions.x);
			}
			int num = Mathf.RoundToInt(point.x) - dimensions.x / 2;
			int num2 = Mathf.RoundToInt(point.z) - dimensions.y / 2;
			return new Vector2Int(num, num2);
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x0006147C File Offset: 0x0005F67C
		private void ReleaseAllBuildings()
		{
			for (int i = this.activeBuildings.Count - 1; i >= 0; i--)
			{
				Building building = this.activeBuildings[i];
				if (!(building == null))
				{
					Object.Destroy(building.gameObject);
				}
			}
			this.activeBuildings.Clear();
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x000614D0 File Offset: 0x0005F6D0
		public void RepaintAll()
		{
			this.ReleaseAllBuildings();
			BuildingManager.BuildingAreaData areaData = this.AreaData;
			if (areaData == null)
			{
				return;
			}
			foreach (BuildingManager.BuildingData building in areaData.Buildings)
			{
				this.Display(building);
			}
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x00061534 File Offset: 0x0005F734
		private void Display(BuildingManager.BuildingData building)
		{
			if (building == null)
			{
				return;
			}
			Building prefab = building.Info.Prefab;
			if (prefab == null)
			{
				Debug.LogError("No prefab for building " + building.ID);
				return;
			}
			for (int i = this.activeBuildings.Count - 1; i >= 0; i--)
			{
				Building building2 = this.activeBuildings[i];
				if (building2 == null)
				{
					this.activeBuildings.RemoveAt(i);
				}
				else if (building2.GUID == building.GUID)
				{
					Debug.LogError(string.Format("重复显示建筑{0}({1})", building.Info.DisplayName, building.GUID));
					return;
				}
			}
			Building building3 = Object.Instantiate<Building>(prefab, base.transform);
			building3.Setup(building);
			building3.transform.position = building.GetTransformPosition();
			this.activeBuildings.Add(building3);
			if (building3.unlockAchievement && AchievementManager.Instance)
			{
				AchievementManager.Instance.Unlock("Building_" + building3.ID.Trim());
			}
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x00061650 File Offset: 0x0005F850
		internal Vector3 CoordToWorldPosition(Vector2Int coord, Vector2Int dimensions, BuildingRotation rotation)
		{
			if (rotation % BuildingRotation.Half > BuildingRotation.Zero)
			{
				dimensions..ctor(dimensions.y, dimensions.x);
			}
			return new Vector3((float)coord.x - 0.5f + (float)dimensions.x / 2f, 0f, (float)coord.y - 0.5f + (float)dimensions.y / 2f);
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x000616BC File Offset: 0x0005F8BC
		internal bool PhysicsCollide(Vector2Int dimensions, BuildingRotation rotation, Vector2Int coord, float castBeginHeight = 0f, float castHeight = 2f)
		{
			if (rotation % BuildingRotation.Half != BuildingRotation.Zero)
			{
				dimensions..ctor(dimensions.y, dimensions.x);
			}
			this.raycastHitCount = 0;
			for (int i = coord.y; i < coord.y + dimensions.y; i++)
			{
				for (int j = coord.x; j < coord.x + dimensions.x; j++)
				{
					Vector3 vector;
					vector..ctor((float)j, castBeginHeight, (float)i);
					this.raycastHitCount += Physics.RaycastNonAlloc(vector, Vector3.up, this.raycastHitBuffer, castHeight, this.physicsCollisionLayers);
					this.raycastHitCount += Physics.RaycastNonAlloc(vector + Vector3.up * castHeight, Vector3.down, this.raycastHitBuffer, castHeight, this.physicsCollisionLayers);
					if (this.raycastHitCount > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x000617B8 File Offset: 0x0005F9B8
		internal Building GetBuildingInstanceAt(Vector2Int coord)
		{
			BuildingManager.BuildingData buildingData = this.AreaData.GetBuildingAt(coord);
			if (buildingData == null)
			{
				return null;
			}
			return this.activeBuildings.Find((Building e) => e != null && e.GUID == buildingData.GUID);
		}

		// Token: 0x0400134D RID: 4941
		[SerializeField]
		private string areaID;

		// Token: 0x0400134E RID: 4942
		[SerializeField]
		private Vector2Int size;

		// Token: 0x0400134F RID: 4943
		[SerializeField]
		private LayerMask physicsCollisionLayers = -1;

		// Token: 0x04001350 RID: 4944
		private List<Building> activeBuildings = new List<Building>();

		// Token: 0x04001351 RID: 4945
		private int raycastHitCount;

		// Token: 0x04001352 RID: 4946
		private RaycastHit[] raycastHitBuffer = new RaycastHit[5];
	}
}
