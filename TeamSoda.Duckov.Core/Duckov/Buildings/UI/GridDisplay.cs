using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.Buildings.UI
{
	// Token: 0x02000335 RID: 821
	public class GridDisplay : MonoBehaviour
	{
		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001B78 RID: 7032 RVA: 0x00063E10 File Offset: 0x00062010
		// (set) Token: 0x06001B79 RID: 7033 RVA: 0x00063E17 File Offset: 0x00062017
		public static GridDisplay Instance { get; private set; }

		// Token: 0x06001B7A RID: 7034 RVA: 0x00063E1F File Offset: 0x0006201F
		private void Awake()
		{
			GridDisplay.Instance = this;
			GridDisplay.Close();
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x00063E2C File Offset: 0x0006202C
		public void Setup(BuildingArea buildingArea)
		{
			Vector2Int lowerLeftCorner = buildingArea.LowerLeftCorner;
			Vector4 vector;
			vector..ctor((float)lowerLeftCorner.x, (float)lowerLeftCorner.y, (float)(buildingArea.Size.x * 2 - 1), (float)(buildingArea.Size.y * 2 - 1));
			Shader.SetGlobalVector("BuildingGrid_AreaPosAndSize", vector);
			GridDisplay.ShowGrid();
			GridDisplay.HidePreview();
			GridDisplay.ShowGrid();
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x00063E97 File Offset: 0x00062097
		public static void Close()
		{
			GridDisplay.HidePreview();
			GridDisplay.HideGrid();
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x00063EA4 File Offset: 0x000620A4
		public static UniTask SetGridShowHide(bool show, AnimationCurve curve, float duration)
		{
			GridDisplay.<SetGridShowHide>d__12 <SetGridShowHide>d__;
			<SetGridShowHide>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SetGridShowHide>d__.show = show;
			<SetGridShowHide>d__.curve = curve;
			<SetGridShowHide>d__.duration = duration;
			<SetGridShowHide>d__.<>1__state = -1;
			<SetGridShowHide>d__.<>t__builder.Start<GridDisplay.<SetGridShowHide>d__12>(ref <SetGridShowHide>d__);
			return <SetGridShowHide>d__.<>t__builder.Task;
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x00063EF7 File Offset: 0x000620F7
		public static void HideGrid()
		{
			if (GridDisplay.Instance)
			{
				GridDisplay.SetGridShowHide(false, GridDisplay.Instance.hideCurve, GridDisplay.Instance.animationDuration).Forget();
				return;
			}
			Shader.SetGlobalFloat("BuildingGrid_Building", 0f);
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x00063F34 File Offset: 0x00062134
		public static void ShowGrid()
		{
			if (GridDisplay.Instance)
			{
				GridDisplay.SetGridShowHide(true, GridDisplay.Instance.showCurve, GridDisplay.Instance.animationDuration).Forget();
				return;
			}
			Shader.SetGlobalFloat("BuildingGrid_Building", 1f);
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x00063F71 File Offset: 0x00062171
		public static void HidePreview()
		{
			Shader.SetGlobalVector("BuildingGrid_BuildingPosAndSize", Vector4.zero);
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x00063F84 File Offset: 0x00062184
		internal void SetBuildingPreviewCoord(Vector2Int coord, Vector2Int dimensions, BuildingRotation rotation, bool validPlacement)
		{
			if (rotation % BuildingRotation.Half > BuildingRotation.Zero)
			{
				dimensions..ctor(dimensions.y, dimensions.x);
			}
			Vector4 vector;
			vector..ctor((float)coord.x, (float)coord.y, (float)dimensions.x, (float)dimensions.y);
			Shader.SetGlobalVector("BuildingGrid_BuildingPosAndSize", vector);
			Shader.SetGlobalFloat("BuildingGrid_CanBuild", (float)(validPlacement ? 1 : 0));
		}

		// Token: 0x040013AF RID: 5039
		[HideInInspector]
		[SerializeField]
		private BuildingArea targetArea;

		// Token: 0x040013B0 RID: 5040
		[SerializeField]
		private float animationDuration;

		// Token: 0x040013B1 RID: 5041
		[SerializeField]
		private AnimationCurve showCurve;

		// Token: 0x040013B2 RID: 5042
		[SerializeField]
		private AnimationCurve hideCurve;

		// Token: 0x040013B3 RID: 5043
		private static int gridShowHideTaskToken;
	}
}
