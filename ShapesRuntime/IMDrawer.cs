using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200001F RID: 31
	internal struct IMDrawer : IDisposable
	{
		// Token: 0x06000B2D RID: 2861 RVA: 0x00015830 File Offset: 0x00013A30
		private static string[] GetMaterialKeywords(Material m)
		{
			string[] result;
			if (!IMDrawer.matKeywords.TryGetValue(m, ref result))
			{
				result = (IMDrawer.matKeywords[m] = m.shaderKeywords);
			}
			return result;
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x00015860 File Offset: 0x00013A60
		public IMDrawer(MetaMpb metaMpb, Material sourceMat, Mesh sourceMesh, int submesh = 0, IMDrawer.DrawType drawType = IMDrawer.DrawType.Shape, bool allowInstancing = true, int textAutoDisposeId = -1)
		{
			this.mtx = Draw.Matrix;
			this.metaMpb = metaMpb;
			this.allowInstancing = (allowInstancing && ShapesConfig.Instance.useImmediateModeInstancing);
			if (DrawCommand.IsAddingDrawCommandsToBuffer)
			{
				Draw.style.renderState.shader = sourceMat.shader;
				Draw.style.renderState.keywords = IMDrawer.GetMaterialKeywords(sourceMat);
				Draw.style.renderState.isTextMaterial = (drawType == IMDrawer.DrawType.TextPooledPersistent || drawType == IMDrawer.DrawType.TextAssetClone);
				switch (drawType)
				{
				case IMDrawer.DrawType.Custom:
					this.drawState.mat = sourceMat;
					break;
				case IMDrawer.DrawType.TextAssetClone:
					this.drawState.mat = Object.Instantiate<Material>(sourceMat);
					IMDrawer.ApplyGlobalPropertiesTMP(this.drawState.mat);
					DrawCommand.CurrentWritingCommandBuffer.cachedAssets.Add(this.drawState.mat);
					break;
				case IMDrawer.DrawType.TextPooledAuto:
					this.drawState.mat = sourceMat;
					DrawCommand.CurrentWritingCommandBuffer.cachedTextIds.Add(textAutoDisposeId);
					break;
				case IMDrawer.DrawType.TextPooledPersistent:
					this.drawState.mat = sourceMat;
					break;
				default:
					this.drawState.mat = IMMaterialPool.GetMaterial(ref Draw.style.renderState);
					break;
				}
				if (drawType == IMDrawer.DrawType.TextAssetClone)
				{
					this.drawState.mesh = Object.Instantiate<Mesh>(sourceMesh);
					DrawCommand.CurrentWritingCommandBuffer.cachedAssets.Add(this.drawState.mesh);
				}
				else
				{
					this.drawState.mesh = sourceMesh;
				}
				this.drawState.submesh = submesh;
				if (IMDrawer.metaMpbPrevious != metaMpb && IMDrawer.metaMpbPrevious != null && IMDrawer.metaMpbPrevious.HasContent)
				{
					DrawCommand.CurrentWritingCommandBuffer.drawCalls.Add(IMDrawer.metaMpbPrevious.ExtractDrawCall());
				}
				if (!metaMpb.PreAppendCheck(this.drawState, this.mtx))
				{
					ShapeDrawCall shapeDrawCall = metaMpb.ExtractDrawCall();
					DrawCommand.CurrentWritingCommandBuffer.drawCalls.Add(shapeDrawCall);
					if (!metaMpb.PreAppendCheck(this.drawState, this.mtx))
					{
						Debug.LogWarning("MetaMpb somehow not ready to be initialized");
					}
				}
				IMDrawer.metaMpbPrevious = metaMpb;
				return;
			}
			this.drawState.mesh = sourceMesh;
			this.drawState.mat = sourceMat;
			this.drawState.submesh = submesh;
			if (!metaMpb.PreAppendCheck(this.drawState, this.mtx))
			{
				Debug.LogError("Somehow PreAppendCheck failed for this draw");
			}
			if (drawType != IMDrawer.DrawType.Custom)
			{
				IMDrawer.ApplyGlobalProperties(this.drawState.mat);
			}
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x00015ABC File Offset: 0x00013CBC
		private static void ApplyGlobalProperties(Material m)
		{
			if (!DrawCommand.IsAddingDrawCommandsToBuffer)
			{
				m.SetFloat(ShapesMaterialUtils.propZTest, (float)Draw.ZTest);
				m.SetFloat(ShapesMaterialUtils.propZOffsetFactor, Draw.ZOffsetFactor);
				m.SetFloat(ShapesMaterialUtils.propZOffsetUnits, (float)Draw.ZOffsetUnits);
				m.SetInt_Shapes(ShapesMaterialUtils.propColorMask, (int)Draw.ColorMask);
				m.SetFloat(ShapesMaterialUtils.propStencilComp, (float)Draw.StencilComp);
				m.SetFloat(ShapesMaterialUtils.propStencilOpPass, (float)Draw.StencilOpPass);
				m.SetFloat(ShapesMaterialUtils.propStencilID, (float)Draw.StencilRefID);
				m.SetFloat(ShapesMaterialUtils.propStencilReadMask, (float)Draw.StencilReadMask);
				m.SetFloat(ShapesMaterialUtils.propStencilWriteMask, (float)Draw.StencilWriteMask);
			}
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00015B6C File Offset: 0x00013D6C
		private static void ApplyGlobalPropertiesTMP(Material m)
		{
			m.SetInt_Shapes(ShapesMaterialUtils.propZTestTMP, (int)Draw.ZTest);
			m.SetInt_Shapes(ShapesMaterialUtils.propColorMask, (int)Draw.ColorMask);
			m.SetInt_Shapes(ShapesMaterialUtils.propStencilComp, (int)Draw.StencilComp);
			m.SetInt_Shapes(ShapesMaterialUtils.propStencilOpPass, (int)Draw.StencilOpPass);
			m.SetInt_Shapes(ShapesMaterialUtils.propStencilIDTMP, (int)Draw.StencilRefID);
			m.SetInt_Shapes(ShapesMaterialUtils.propStencilReadMask, (int)Draw.StencilReadMask);
			m.SetInt_Shapes(ShapesMaterialUtils.propStencilWriteMask, (int)Draw.StencilWriteMask);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00015BEC File Offset: 0x00013DEC
		public void Dispose()
		{
			if (!DrawCommand.IsAddingDrawCommandsToBuffer)
			{
				this.metaMpb.ApplyDirectlyToMaterial();
				this.drawState.mat.SetPass(0);
				Graphics.DrawMeshNow(this.drawState.mesh, this.mtx, this.drawState.submesh);
				return;
			}
			if (!this.allowInstancing)
			{
				ShapeDrawCall shapeDrawCall = this.metaMpb.ExtractDrawCall();
				DrawCommand.CurrentWritingCommandBuffer.drawCalls.Add(shapeDrawCall);
			}
		}

		// Token: 0x040000FA RID: 250
		internal static MetaMpb metaMpbPrevious;

		// Token: 0x040000FB RID: 251
		private static Dictionary<Material, string[]> matKeywords = new Dictionary<Material, string[]>();

		// Token: 0x040000FC RID: 252
		private MetaMpb metaMpb;

		// Token: 0x040000FD RID: 253
		private ShapeDrawState drawState;

		// Token: 0x040000FE RID: 254
		private Matrix4x4 mtx;

		// Token: 0x040000FF RID: 255
		private bool allowInstancing;

		// Token: 0x0200008D RID: 141
		public enum DrawType
		{
			// Token: 0x04000344 RID: 836
			Shape,
			// Token: 0x04000345 RID: 837
			Custom,
			// Token: 0x04000346 RID: 838
			TextAssetClone,
			// Token: 0x04000347 RID: 839
			TextPooledAuto,
			// Token: 0x04000348 RID: 840
			TextPooledPersistent
		}
	}
}
