using System;
using Drawing;
using Duckov.Utilities;
using SodaCraft.Localizations;
using Unity.Mathematics;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x02000329 RID: 809
	public class Building : MonoBehaviour, IDrawGizmos
	{
		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06001AD4 RID: 6868 RVA: 0x00061806 File Offset: 0x0005FA06
		private int guid
		{
			get
			{
				return this.data.GUID;
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001AD5 RID: 6869 RVA: 0x00061813 File Offset: 0x0005FA13
		public int GUID
		{
			get
			{
				return this.guid;
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001AD6 RID: 6870 RVA: 0x0006181B File Offset: 0x0005FA1B
		public string ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001AD7 RID: 6871 RVA: 0x00061823 File Offset: 0x0005FA23
		public Vector2Int Dimensions
		{
			get
			{
				return this.dimensions;
			}
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x0006182C File Offset: 0x0005FA2C
		public Vector3 GetOffset(BuildingRotation rotation = BuildingRotation.Zero)
		{
			bool flag = rotation % BuildingRotation.Half > BuildingRotation.Zero;
			float num = (float)((flag ? this.dimensions.y : this.dimensions.x) - 1);
			float num2 = (float)((flag ? this.dimensions.x : this.dimensions.y) - 1);
			return new Vector3(num / 2f, 0f, num2 / 2f);
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001ADA RID: 6874 RVA: 0x00061896 File Offset: 0x0005FA96
		// (set) Token: 0x06001AD9 RID: 6873 RVA: 0x00061894 File Offset: 0x0005FA94
		[LocalizationKey("Default")]
		public string DisplayNameKey
		{
			get
			{
				return "Building_" + this.ID;
			}
			set
			{
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001ADB RID: 6875 RVA: 0x000618A8 File Offset: 0x0005FAA8
		public string DisplayName
		{
			get
			{
				return LocalizationManager.ToPlainText(this.DisplayNameKey);
			}
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x000618B5 File Offset: 0x0005FAB5
		public static string GetDisplayName(string id)
		{
			return LocalizationManager.ToPlainText("Building_" + id);
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06001ADE RID: 6878 RVA: 0x000618C9 File Offset: 0x0005FAC9
		// (set) Token: 0x06001ADD RID: 6877 RVA: 0x000618C7 File Offset: 0x0005FAC7
		[LocalizationKey("Default")]
		public string DescriptionKey
		{
			get
			{
				return "Building_" + this.ID + "_Desc";
			}
			set
			{
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001ADF RID: 6879 RVA: 0x000618E0 File Offset: 0x0005FAE0
		public string Description
		{
			get
			{
				return LocalizationManager.ToPlainText(this.DescriptionKey);
			}
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x000618F0 File Offset: 0x0005FAF0
		private void Awake()
		{
			if (this.graphicsContainer == null)
			{
				Debug.LogError("建筑" + this.DisplayName + "未配置 Graphics Container");
				Transform transform = base.transform.Find("Graphics");
				this.graphicsContainer = ((transform != null) ? transform.gameObject : null);
			}
			if (this.functionContainer == null)
			{
				Debug.LogError("建筑" + this.DisplayName + "未配置 Function Container");
				Transform transform2 = base.transform.Find("Function");
				this.functionContainer = ((transform2 != null) ? transform2.gameObject : null);
			}
			this.CreateAreaMesh();
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x00061998 File Offset: 0x0005FB98
		private void CreateAreaMesh()
		{
			if (this.areaMesh == null)
			{
				this.areaMesh = Object.Instantiate<GameObject>(GameplayDataSettings.Prefabs.BuildingBlockAreaMesh, base.transform);
				this.areaMesh.transform.localPosition = Vector3.zero;
				this.areaMesh.transform.localRotation = quaternion.identity;
				this.areaMesh.transform.localScale = new Vector3((float)this.dimensions.x - 0.02f, 1f, (float)this.dimensions.y - 0.02f);
				this.areaMesh.transform.SetParent(this.functionContainer.transform, true);
			}
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x00061A5A File Offset: 0x0005FC5A
		private void RegisterEvents()
		{
			BuildingManager.OnBuildingDestroyed += this.OnBuildingDestroyed;
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x00061A6D File Offset: 0x0005FC6D
		private void OnBuildingDestroyed(int guid)
		{
			if (guid == this.GUID)
			{
				this.Release();
			}
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x00061A7E File Offset: 0x0005FC7E
		private void Release()
		{
			Object.Destroy(base.gameObject);
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x00061A8B File Offset: 0x0005FC8B
		private void UnregisterEvents()
		{
			BuildingManager.OnBuildingDestroyed -= this.OnBuildingDestroyed;
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x00061AA0 File Offset: 0x0005FCA0
		public void DrawGizmos()
		{
			if (!GizmoContext.InSelection(this))
			{
				return;
			}
			using (Draw.WithColor(new Color(1f, 1f, 1f, 0.5f)))
			{
				using (Draw.InLocalSpace(base.transform))
				{
					float3 @float = this.GetOffset(BuildingRotation.Zero);
					float2 float2;
					float2..ctor(0.9f, 0.9f);
					for (int i = 0; i < this.Dimensions.y; i++)
					{
						for (int j = 0; j < this.Dimensions.x; j++)
						{
							Draw.SolidPlane(new float3((float)j, 0f, (float)i) - @float, Vector3.up, float2);
						}
					}
				}
			}
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x00061B9C File Offset: 0x0005FD9C
		internal void Setup(BuildingManager.BuildingData data)
		{
			this.data = data;
			base.transform.localRotation = Quaternion.Euler(0f, (float)(data.Rotation * (BuildingRotation)90), 0f);
			this.RegisterEvents();
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x00061BCF File Offset: 0x0005FDCF
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x00061BD8 File Offset: 0x0005FDD8
		internal void SetupPreview()
		{
			this.functionContainer.SetActive(false);
			Collider[] componentsInChildren = this.graphicsContainer.GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}

		// Token: 0x04001353 RID: 4947
		[SerializeField]
		private string id;

		// Token: 0x04001354 RID: 4948
		[SerializeField]
		private Vector2Int dimensions;

		// Token: 0x04001355 RID: 4949
		[SerializeField]
		private GameObject graphicsContainer;

		// Token: 0x04001356 RID: 4950
		[SerializeField]
		private GameObject functionContainer;

		// Token: 0x04001357 RID: 4951
		private BuildingManager.BuildingData data;

		// Token: 0x04001358 RID: 4952
		public bool unlockAchievement;

		// Token: 0x04001359 RID: 4953
		private GameObject areaMesh;
	}
}
