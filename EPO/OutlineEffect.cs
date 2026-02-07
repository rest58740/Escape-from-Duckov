using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace EPOOutline
{
	// Token: 0x02000013 RID: 19
	public static class OutlineEffect
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000067 RID: 103 RVA: 0x0000387D File Offset: 0x00001A7D
		public static RTHandleSystem HandleSystem
		{
			get
			{
				if (OutlineEffect.system != null)
				{
					return OutlineEffect.system;
				}
				OutlineEffect.system = new RTHandleSystem();
				OutlineEffect.system.Initialize(1, 1);
				return OutlineEffect.system;
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000038A8 File Offset: 0x00001AA8
		private static Material LoadMaterial(string shaderName)
		{
			Material material = new Material(Resources.Load<Shader>(string.Format("Easy performant outline/Shaders/{0}", shaderName)));
			if (SystemInfo.supportsInstancing)
			{
				material.enableInstancing = true;
			}
			return material;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000038DC File Offset: 0x00001ADC
		[RuntimeInitializeOnLoadMethod]
		private static void InitMaterials()
		{
			if (OutlineEffect.ObstacleMaterial == null)
			{
				OutlineEffect.ObstacleMaterial = OutlineEffect.LoadMaterial("Obstacle");
			}
			if (OutlineEffect.OutlineMaterial == null)
			{
				OutlineEffect.OutlineMaterial = OutlineEffect.LoadMaterial("Outline");
			}
			if (OutlineEffect.ZPrepassMaterial == null)
			{
				OutlineEffect.ZPrepassMaterial = OutlineEffect.LoadMaterial("ZPrepass");
			}
			if (OutlineEffect.DilateMaterial == null)
			{
				OutlineEffect.DilateMaterial = OutlineEffect.LoadMaterial("Dilate");
			}
			if (OutlineEffect.BlurMaterial == null)
			{
				OutlineEffect.BlurMaterial = OutlineEffect.LoadMaterial("Blur");
			}
			if (OutlineEffect.FinalBlitMaterial == null)
			{
				OutlineEffect.FinalBlitMaterial = OutlineEffect.LoadMaterial("FinalBlit");
			}
			if (OutlineEffect.BasicBlitMaterial == null)
			{
				OutlineEffect.BasicBlitMaterial = OutlineEffect.LoadMaterial("BasicBlit");
			}
			if (OutlineEffect.FillMaskMaterial == null)
			{
				OutlineEffect.FillMaskMaterial = OutlineEffect.LoadMaterial("Fills/FillMask");
			}
			if (OutlineEffect.ClearStencilMaterial == null)
			{
				OutlineEffect.ClearStencilMaterial = OutlineEffect.LoadMaterial("ClearStencil");
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000039E8 File Offset: 0x00001BE8
		private static void Postprocess(OutlineParameters parameters, RTHandle first, RTHandle second, Material material, int iterations, int eyeSlice, bool additionalShift, float shiftValue, ref int stencil, Rect viewport, float scale)
		{
			if (iterations <= 0)
			{
				return;
			}
			parameters.Buffer.SetGlobalInt(OutlineEffect.ComparisonHash, 3);
			for (int i = 1; i <= iterations; i++)
			{
				parameters.Buffer.SetGlobalInt(OutlineEffect.RefHash, stencil);
				float num = additionalShift ? ((float)i) : 1f;
				parameters.Buffer.SetGlobalVector(OutlineEffect.ShiftHash, new Vector4(num * scale, 0f));
				OutlineEffect.Blit(parameters, first, second, first, material, shiftValue, eyeSlice, -1, new Rect?(viewport));
				stencil = (stencil + 1) % 255;
				parameters.Buffer.SetGlobalInt(OutlineEffect.RefHash, stencil);
				parameters.Buffer.SetGlobalVector(OutlineEffect.ShiftHash, new Vector4(0f, num * scale));
				OutlineEffect.Blit(parameters, second, first, first, material, shiftValue, eyeSlice, -1, new Rect?(viewport));
				stencil = (stencil + 1) % 255;
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003AD7 File Offset: 0x00001CD7
		private static void Blit(OutlineParameters parameters, RTHandle source, RTHandle destination, RTHandle destinationDepth, Material material, float effectSize, int eyeSlice, int pass = -1, Rect? viewport = null)
		{
			parameters.Buffer.SetGlobalFloat(OutlineEffect.EffectSizeHash, effectSize);
			BlitUtility.Blit(parameters, source, destination, destinationDepth, eyeSlice, material, pass, viewport);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003AFC File Offset: 0x00001CFC
		private static void Draw(OutlineParameters parameters, RTHandle destination, RTHandle destinationDepth, Material material, float effectSize, int eyeSlice, int pass = -1, Rect? viewport = null)
		{
			parameters.Buffer.SetGlobalFloat(OutlineEffect.EffectSizeHash, effectSize);
			BlitUtility.Draw(parameters, destination, destinationDepth, eyeSlice, material, pass, viewport);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003B20 File Offset: 0x00001D20
		private static float GetBlurShift(BlurType blurType, int iterationsCount)
		{
			switch (blurType)
			{
			case BlurType.Box:
				return (float)iterationsCount * 0.65f + 1f;
			case BlurType.Gaussian5x5:
				return 3f * (float)iterationsCount;
			case BlurType.Gaussian9x9:
				return 5f + (float)iterationsCount;
			case BlurType.Gaussian13x13:
				return 7f + (float)iterationsCount;
			default:
				throw new ArgumentException("Unknown blur type");
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003B7C File Offset: 0x00001D7C
		private static float GetMaskingValueForMode(OutlinableDrawingMode mode)
		{
			if ((mode & OutlinableDrawingMode.Mask) != (OutlinableDrawingMode)0)
			{
				return 0.6f;
			}
			if ((mode & OutlinableDrawingMode.Obstacle) == (OutlinableDrawingMode)0)
			{
				return 1f;
			}
			return 0.25f;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003B9A File Offset: 0x00001D9A
		private static float ComputeEffectShift(OutlineParameters parameters)
		{
			return (OutlineEffect.GetBlurShift(parameters.BlurType, parameters.BlurIterations) * parameters.BlurShift + (float)parameters.DilateIterations * 4f * parameters.DilateShift) * 1.1f;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003BD0 File Offset: 0x00001DD0
		private static void PrepareTargets(OutlineParameters parameters)
		{
			OutlineEffect.targets.Clear();
			foreach (Outlinable outlinable in parameters.OutlinablesToRender)
			{
				for (int i = 0; i < outlinable.OutlineTargets.Count; i++)
				{
					OutlineTarget outlineTarget = outlinable.OutlineTargets[i];
					Renderer renderer = outlineTarget.Renderer;
					if (outlineTarget.IsVisible || ((outlinable.DrawingMode & OutlinableDrawingMode.GenericMask) != (OutlinableDrawingMode)0 && !(renderer == null)))
					{
						OutlineEffect.targets.Add(new OutlineEffect.OutlineTargetGroup(outlinable, outlineTarget));
					}
				}
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003C80 File Offset: 0x00001E80
		public static void SetupOutline(OutlineParameters parameters)
		{
			parameters.Buffer.SetRenderTarget(parameters.Handles.Target, -1);
			parameters.Buffer.SetGlobalVector(OutlineEffect.ScaleHash, parameters.Scale);
			OutlineEffect.PrepareTargets(parameters);
			OutlineEffect.InitMaterials();
			float num = OutlineEffect.ComputeEffectShift(parameters);
			float num2 = num + 3f;
			float effectSize = num;
			int depthSliceForEye = RenderTargetUtility.GetDepthSliceForEye(parameters.EyeMask);
			parameters.Buffer.ClearRenderTarget(true, true, Color.clear);
			parameters.Buffer.SetRenderTarget(parameters.Handles.PrimaryTarget, -1);
			parameters.Buffer.ClearRenderTarget(true, true, Color.clear);
			parameters.Buffer.SetRenderTarget(parameters.Handles.SecondaryTarget, -1);
			parameters.Buffer.ClearRenderTarget(true, true, Color.clear);
			if (parameters.UseInfoBuffer)
			{
				parameters.Buffer.SetRenderTarget(parameters.Handles.InfoTarget, -1);
				parameters.Buffer.ClearRenderTarget(true, true, Color.clear);
				parameters.Buffer.SetRenderTarget(parameters.Handles.PrimaryInfoBufferTarget, -1);
				parameters.Buffer.ClearRenderTarget(true, true, Color.clear);
				parameters.Buffer.SetRenderTarget(parameters.Handles.SecondaryInfoBufferTarget, -1);
				parameters.Buffer.ClearRenderTarget(true, true, Color.clear);
			}
			parameters.Buffer.SetGlobalInt(OutlineEffect.SrcBlendHash, 1);
			parameters.Buffer.SetGlobalInt(OutlineEffect.DstBlendHash, 0);
			int value = 1;
			parameters.Buffer.SetGlobalInt(OutlineEffect.OutlineRefHash, value);
			OutlineEffect.SetupDilateKeyword(parameters);
			Vector2Int vector2Int = new Vector2Int(parameters.ScaledBufferWidth, parameters.ScaledBufferHeight);
			BlitUtility.PrepareForRendering(parameters);
			parameters.Buffer.SetRenderTarget(parameters.Handles.Target, parameters.DepthTarget, depthSliceForEye);
			parameters.Buffer.ClearRenderTarget(false, true, Color.clear);
			parameters.Buffer.SetViewport(parameters.Viewport);
			OutlineEffect.DrawOutlineables(parameters, CompareFunction.LessEqual, (Outlinable x) => true, (Outlinable x) => Color.clear, (Outlinable x) => OutlineEffect.ZPrepassMaterial, (RenderStyle)3, OutlinableDrawingMode.ZOnly);
			parameters.Buffer.DisableShaderKeyword(KeywordsUtility.GetEnabledInfoBufferKeyword());
			if (parameters.UseInfoBuffer)
			{
				parameters.Buffer.EnableShaderKeyword(KeywordsUtility.GetInfoBufferStageKeyword());
				parameters.Buffer.SetRenderTarget(parameters.Handles.InfoTarget, parameters.DepthTarget, depthSliceForEye);
				parameters.Buffer.ClearRenderTarget(false, true, Color.clear);
				parameters.Buffer.SetViewport(parameters.Viewport);
				OutlineEffect.DrawOutlineables(parameters, CompareFunction.Always, (Outlinable x) => x.OutlineParameters.Enabled, (Outlinable x) => new Color(x.OutlineParameters.DilateShift, x.OutlineParameters.BlurShift, 0f, 1f), (Outlinable x) => OutlineEffect.OutlineMaterial, RenderStyle.Single, OutlinableDrawingMode.Normal);
				OutlineEffect.DrawOutlineables(parameters, CompareFunction.NotEqual, (Outlinable x) => x.BackParameters.Enabled, (Outlinable x) => new Color(x.BackParameters.DilateShift, x.BackParameters.BlurShift, 0f, 1f), (Outlinable x) => OutlineEffect.OutlineMaterial, RenderStyle.FrontBack, OutlinableDrawingMode.Normal);
				OutlineEffect.DrawOutlineables(parameters, CompareFunction.LessEqual, (Outlinable x) => x.FrontParameters.Enabled, (Outlinable x) => new Color(x.FrontParameters.DilateShift, x.FrontParameters.BlurShift, 0f, 1f), (Outlinable x) => OutlineEffect.OutlineMaterial, RenderStyle.FrontBack, OutlinableDrawingMode.Normal);
				OutlineEffect.DrawOutlineables(parameters, CompareFunction.LessEqual, (Outlinable x) => true, (Outlinable x) => new Color(0f, 0f, OutlineEffect.GetMaskingValueForMode(x.DrawingMode), 1f), (Outlinable x) => OutlineEffect.ObstacleMaterial, (RenderStyle)3, OutlinableDrawingMode.Obstacle | OutlinableDrawingMode.Mask);
				parameters.Buffer.SetGlobalInt(OutlineEffect.ComparisonHash, 8);
				parameters.Buffer.SetGlobalInt(OutlineEffect.OperationHash, 0);
				OutlineEffect.Blit(parameters, parameters.Handles.InfoTarget, parameters.Handles.PrimaryInfoBufferTarget, parameters.Handles.PrimaryInfoBufferTarget, OutlineEffect.BasicBlitMaterial, num2, -1, -1, new Rect?(new Rect(0f, 0f, (float)vector2Int.x, (float)vector2Int.y)));
				int iterations = ((parameters.DilateQuality == DilateQuality.Base) ? parameters.DilateIterations : (parameters.DilateIterations * 2)) + parameters.BlurIterations;
				int num3 = 0;
				OutlineEffect.Postprocess(parameters, parameters.Handles.PrimaryInfoBufferTarget, parameters.Handles.SecondaryInfoBufferTarget, OutlineEffect.DilateMaterial, iterations, depthSliceForEye, true, num2, ref num3, new Rect(0f, 0f, (float)vector2Int.x, (float)vector2Int.y), 1f);
				parameters.Buffer.SetRenderTarget(parameters.Handles.InfoTarget, parameters.DepthTarget, depthSliceForEye);
				parameters.Buffer.SetViewport(parameters.Viewport);
				parameters.Buffer.SetGlobalTexture(OutlineEffect.InfoBufferHash, parameters.Handles.PrimaryInfoBufferTarget);
				parameters.Buffer.DisableShaderKeyword(KeywordsUtility.GetInfoBufferStageKeyword());
			}
			if (parameters.UseInfoBuffer)
			{
				parameters.Buffer.EnableShaderKeyword(KeywordsUtility.GetEnabledInfoBufferKeyword());
			}
			parameters.Buffer.SetRenderTarget(parameters.Handles.Target, parameters.DepthTarget, depthSliceForEye);
			parameters.Buffer.ClearRenderTarget(false, true, Color.clear);
			parameters.Buffer.SetViewport(parameters.Viewport);
			int num4 = 0 + OutlineEffect.DrawOutlineables(parameters, CompareFunction.Always, (Outlinable x) => x.OutlineParameters.Enabled, (Outlinable x) => x.OutlineParameters.Color, (Outlinable x) => OutlineEffect.OutlineMaterial, RenderStyle.Single, OutlinableDrawingMode.Normal) + OutlineEffect.DrawOutlineables(parameters, CompareFunction.NotEqual, (Outlinable x) => x.BackParameters.Enabled, (Outlinable x) => x.BackParameters.Color, (Outlinable x) => OutlineEffect.OutlineMaterial, RenderStyle.FrontBack, OutlinableDrawingMode.Normal) + OutlineEffect.DrawOutlineables(parameters, CompareFunction.LessEqual, (Outlinable x) => x.FrontParameters.Enabled, (Outlinable x) => x.FrontParameters.Color, (Outlinable x) => OutlineEffect.OutlineMaterial, RenderStyle.FrontBack, OutlinableDrawingMode.Normal);
			int num5 = 0;
			if (num4 > 0)
			{
				parameters.Buffer.SetGlobalInt(OutlineEffect.ComparisonHash, 8);
				parameters.Buffer.SetGlobalInt(OutlineEffect.OperationHash, 0);
				OutlineEffect.Blit(parameters, parameters.Handles.Target, parameters.Handles.PrimaryTarget, parameters.Handles.PrimaryTarget, OutlineEffect.BasicBlitMaterial, num2, depthSliceForEye, -1, new Rect?(new Rect(0f, 0f, (float)vector2Int.x, (float)vector2Int.y)));
				OutlineEffect.Postprocess(parameters, parameters.Handles.PrimaryTarget, parameters.Handles.SecondaryTarget, OutlineEffect.DilateMaterial, parameters.DilateIterations, depthSliceForEye, false, num2, ref num5, new Rect(0f, 0f, (float)vector2Int.x, (float)vector2Int.y), parameters.DilateShift);
			}
			parameters.Buffer.SetRenderTarget(parameters.Handles.Target, parameters.DepthTarget, depthSliceForEye);
			parameters.Buffer.ClearRenderTarget(false, true, Color.clear);
			parameters.Buffer.SetViewport(parameters.Viewport);
			if (num4 > 0)
			{
				OutlineEffect.SetupBlurKeyword(parameters);
				OutlineEffect.Postprocess(parameters, parameters.Handles.PrimaryTarget, parameters.Handles.SecondaryTarget, OutlineEffect.BlurMaterial, parameters.BlurIterations, depthSliceForEye, false, num2, ref num5, new Rect(0f, 0f, (float)vector2Int.x, (float)vector2Int.y), parameters.BlurShift);
			}
			parameters.Buffer.SetGlobalInt(OutlineEffect.ComparisonHash, 6);
			parameters.Buffer.SetGlobalInt(OutlineEffect.ReadMaskHash, 255);
			parameters.Buffer.SetGlobalInt(OutlineEffect.OperationHash, 2);
			OutlineEffect.Blit(parameters, parameters.Handles.PrimaryTarget, parameters.Target, parameters.DepthTarget, OutlineEffect.FinalBlitMaterial, effectSize, depthSliceForEye, -1, new Rect?(parameters.Viewport));
			OutlineEffect.DrawFill(parameters, parameters.Target);
			OutlineEffect.Draw(parameters, parameters.Target, parameters.DepthTarget, OutlineEffect.ClearStencilMaterial, effectSize, depthSliceForEye, -1, new Rect?(parameters.Viewport));
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000045A0 File Offset: 0x000027A0
		private static void SetupDilateKeyword(OutlineParameters parameters)
		{
			KeywordsUtility.GetAllDilateKeywords(OutlineEffect.keywords);
			foreach (string keyword in OutlineEffect.keywords)
			{
				parameters.Buffer.DisableShaderKeyword(keyword);
			}
			parameters.Buffer.EnableShaderKeyword(KeywordsUtility.GetDilateQualityKeyword(parameters.DilateQuality));
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004618 File Offset: 0x00002818
		private static void SetupBlurKeyword(OutlineParameters parameters)
		{
			KeywordsUtility.GetAllBlurKeywords(OutlineEffect.keywords);
			foreach (string keyword in OutlineEffect.keywords)
			{
				parameters.Buffer.DisableShaderKeyword(keyword);
			}
			parameters.Buffer.EnableShaderKeyword(KeywordsUtility.GetBlurKeyword(parameters.BlurType));
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004690 File Offset: 0x00002890
		private static int DrawOutlineables(OutlineParameters parameters, CompareFunction function, Func<Outlinable, bool> shouldRender, Func<Outlinable, Color> colorProvider, Func<Outlinable, Material> materialProvider, RenderStyle styleMask, OutlinableDrawingMode modeMask = OutlinableDrawingMode.Normal)
		{
			int num = 0;
			parameters.Buffer.SetGlobalInt(OutlineEffect.ZTestHash, (int)function);
			OutlineEffect.SetMaskingMasking(parameters.Buffer, ComplexMaskingMode.None);
			ComplexMaskingMode complexMaskingMode = ComplexMaskingMode.None;
			foreach (OutlineEffect.OutlineTargetGroup outlineTargetGroup in OutlineEffect.targets)
			{
				Outlinable outlinable = outlineTargetGroup.Outlinable;
				if ((outlinable.RenderStyle & styleMask) != (RenderStyle)0 && (outlinable.DrawingMode & modeMask) != (OutlinableDrawingMode)0)
				{
					if ((function == CompareFunction.NotEqual || function == CompareFunction.Always) && outlinable.ComplexMaskingMode != complexMaskingMode)
					{
						OutlineEffect.SetMaskingMasking(parameters.Buffer, outlinable.ComplexMaskingMode);
						complexMaskingMode = outlinable.ComplexMaskingMode;
					}
					Color color = shouldRender(outlinable) ? colorProvider(outlinable) : Color.clear;
					parameters.Buffer.SetGlobalColor(OutlineEffect.ColorHash, color);
					OutlineTarget target = outlineTargetGroup.Target;
					parameters.Buffer.SetGlobalInt(OutlineEffect.ColorMaskHash, 255);
					OutlineEffect.SetupCutout(parameters, target);
					OutlineEffect.SetupCull(parameters, target);
					num++;
					Material material = materialProvider(outlinable);
					parameters.Buffer.DrawRenderer(target.Renderer, material, target.ShiftedSubmeshIndex);
				}
			}
			OutlineEffect.SetMaskingMasking(parameters.Buffer, ComplexMaskingMode.None);
			return num;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000047E8 File Offset: 0x000029E8
		private static void SetMaskingMasking(CommandBufferWrapper buffer, ComplexMaskingMode maskingMode)
		{
			buffer.DisableShaderKeyword(KeywordsUtility.GetBackKeyword(ComplexMaskingMode.MaskingMode));
			buffer.DisableShaderKeyword(KeywordsUtility.GetBackKeyword(ComplexMaskingMode.ObstaclesMode));
			if (maskingMode != ComplexMaskingMode.None)
			{
				buffer.EnableShaderKeyword(KeywordsUtility.GetBackKeyword(maskingMode));
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004814 File Offset: 0x00002A14
		private static void DrawFill(OutlineParameters parameters, RTHandle targetSurface)
		{
			int depthSliceForEye = RenderTargetUtility.GetDepthSliceForEye(parameters.EyeMask);
			parameters.Buffer.SetRenderTarget(targetSurface, parameters.DepthTarget, depthSliceForEye);
			parameters.Buffer.SetViewport(parameters.Viewport);
			int value = 1;
			int value2 = 2;
			int value3 = 3;
			parameters.Buffer.SetGlobalInt(OutlineEffect.ZTestHash, 5);
			parameters.Buffer.SetGlobalInt(OutlineEffect.FillRefHash, value3);
			foreach (Outlinable outlinable in parameters.OutlinablesToRender)
			{
				if ((outlinable.DrawingMode & OutlinableDrawingMode.Normal) != (OutlinableDrawingMode)0)
				{
					for (int i = 0; i < outlinable.OutlineTargets.Count; i++)
					{
						OutlineTarget outlineTarget = outlinable.OutlineTargets[i];
						if (outlineTarget.IsVisible)
						{
							Renderer renderer = outlineTarget.Renderer;
							if (outlinable.NeedsFillMask)
							{
								OutlineEffect.SetupCutout(parameters, outlineTarget);
								OutlineEffect.SetupCull(parameters, outlineTarget);
								parameters.Buffer.DrawRenderer(renderer, OutlineEffect.FillMaskMaterial, outlineTarget.ShiftedSubmeshIndex);
							}
						}
					}
				}
			}
			parameters.Buffer.SetGlobalInt(OutlineEffect.ZTestHash, 4);
			parameters.Buffer.SetGlobalInt(OutlineEffect.FillRefHash, value2);
			foreach (Outlinable outlinable2 in parameters.OutlinablesToRender)
			{
				if ((outlinable2.DrawingMode & OutlinableDrawingMode.Normal) != (OutlinableDrawingMode)0)
				{
					for (int j = 0; j < outlinable2.OutlineTargets.Count; j++)
					{
						OutlineTarget outlineTarget2 = outlinable2.OutlineTargets[j];
						if (outlineTarget2.IsVisible && outlinable2.NeedsFillMask)
						{
							Renderer renderer2 = outlineTarget2.Renderer;
							OutlineEffect.SetupCutout(parameters, outlineTarget2);
							OutlineEffect.SetupCull(parameters, outlineTarget2);
							parameters.Buffer.DrawRenderer(renderer2, OutlineEffect.FillMaskMaterial, outlineTarget2.ShiftedSubmeshIndex);
						}
					}
				}
			}
			ComplexMaskingMode complexMaskingMode = ComplexMaskingMode.None;
			OutlineEffect.SetMaskingMasking(parameters.Buffer, ComplexMaskingMode.None);
			foreach (Outlinable outlinable3 in parameters.OutlinablesToRender)
			{
				if ((outlinable3.DrawingMode & OutlinableDrawingMode.Normal) != (OutlinableDrawingMode)0)
				{
					if (outlinable3.ComplexMaskingMode != complexMaskingMode)
					{
						OutlineEffect.SetMaskingMasking(parameters.Buffer, outlinable3.ComplexMaskingMode);
						complexMaskingMode = ComplexMaskingMode.None;
					}
					if (outlinable3.RenderStyle == RenderStyle.FrontBack)
					{
						if ((outlinable3.BackParameters.FillPass.Material == null || !outlinable3.BackParameters.Enabled) && (outlinable3.FrontParameters.FillPass.Material == null || !outlinable3.FrontParameters.Enabled))
						{
							continue;
						}
						Material material = outlinable3.FrontParameters.FillPass.Material;
						parameters.Buffer.SetGlobalInt(OutlineEffect.FillRefHash, value2);
						if (material != null && outlinable3.FrontParameters.Enabled)
						{
							for (int k = 0; k < outlinable3.OutlineTargets.Count; k++)
							{
								OutlineTarget outlineTarget3 = outlinable3.OutlineTargets[k];
								if (outlineTarget3.IsVisible)
								{
									Renderer renderer3 = outlineTarget3.Renderer;
									OutlineEffect.SetupCutout(parameters, outlineTarget3);
									OutlineEffect.SetupCull(parameters, outlineTarget3);
									parameters.Buffer.DrawRenderer(renderer3, material, outlineTarget3.ShiftedSubmeshIndex);
								}
							}
						}
						Material material2 = outlinable3.BackParameters.FillPass.Material;
						parameters.Buffer.SetGlobalInt(OutlineEffect.FillRefHash, value3);
						if (material2 == null || !outlinable3.BackParameters.Enabled)
						{
							continue;
						}
						for (int l = 0; l < outlinable3.OutlineTargets.Count; l++)
						{
							OutlineTarget outlineTarget4 = outlinable3.OutlineTargets[l];
							if (outlineTarget4.IsVisible)
							{
								Renderer renderer4 = outlineTarget4.Renderer;
								OutlineEffect.SetupCutout(parameters, outlineTarget4);
								OutlineEffect.SetupCull(parameters, outlineTarget4);
								parameters.Buffer.DrawRenderer(renderer4, material2, outlineTarget4.ShiftedSubmeshIndex);
							}
						}
					}
					else
					{
						if (outlinable3.OutlineParameters.FillPass.Material == null || !outlinable3.OutlineParameters.Enabled)
						{
							continue;
						}
						parameters.Buffer.SetGlobalInt(OutlineEffect.ZTestHash, 8);
						parameters.Buffer.SetGlobalInt(OutlineEffect.FillRefHash, value);
						Material material3 = outlinable3.OutlineParameters.FillPass.Material;
						for (int m = 0; m < outlinable3.OutlineTargets.Count; m++)
						{
							OutlineTarget outlineTarget5 = outlinable3.OutlineTargets[m];
							if (outlineTarget5.IsVisible)
							{
								Renderer renderer5 = outlineTarget5.Renderer;
								OutlineEffect.SetupCutout(parameters, outlineTarget5);
								OutlineEffect.SetupCull(parameters, outlineTarget5);
								parameters.Buffer.DrawRenderer(renderer5, material3, outlineTarget5.ShiftedSubmeshIndex);
							}
						}
					}
					if (outlinable3.ComplexMaskingMode != ComplexMaskingMode.None)
					{
						OutlineEffect.SetMaskingMasking(parameters.Buffer, ComplexMaskingMode.None);
					}
				}
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004D44 File Offset: 0x00002F44
		private static void SetupCutout(OutlineParameters parameters, OutlineTarget target)
		{
			if (target.Renderer == null)
			{
				return;
			}
			Vector4 value = new Vector4(((target.CutoutMask & ColorMask.R) != ColorMask.None) ? 1f : 0f, ((target.CutoutMask & ColorMask.G) != ColorMask.None) ? 1f : 0f, ((target.CutoutMask & ColorMask.B) != ColorMask.None) ? 1f : 0f, ((target.CutoutMask & ColorMask.A) != ColorMask.None) ? 1f : 0f);
			parameters.Buffer.SetGlobalVector(OutlineEffect.CutoutMaskHash, value);
			SpriteRenderer spriteRenderer = target.Renderer as SpriteRenderer;
			if (spriteRenderer != null)
			{
				if (spriteRenderer.sprite == null)
				{
					parameters.Buffer.DisableShaderKeyword(KeywordsUtility.GetCutoutKeyword());
					return;
				}
				parameters.Buffer.EnableShaderKeyword(KeywordsUtility.GetCutoutKeyword());
				parameters.Buffer.SetGlobalFloat(OutlineEffect.CutoutThresholdHash, target.CutoutThreshold);
				parameters.Buffer.SetGlobalTexture(OutlineEffect.CutoutTextureHash, parameters.TextureHandleMap[spriteRenderer.sprite.texture]);
				return;
			}
			else
			{
				if (target.IsValidForCutout)
				{
					Material sharedMaterial = target.SharedMaterial;
					parameters.Buffer.EnableShaderKeyword(KeywordsUtility.GetCutoutKeyword());
					parameters.Buffer.SetGlobalFloat(OutlineEffect.CutoutThresholdHash, target.CutoutThreshold);
					Vector2 textureOffset = sharedMaterial.GetTextureOffset(target.CutoutTextureId);
					Vector2 textureScale = sharedMaterial.GetTextureScale(target.CutoutTextureId);
					parameters.Buffer.SetGlobalVector(OutlineEffect.CutoutTextureSTHash, new Vector4(textureScale.x, textureScale.y, textureOffset.x, textureOffset.y));
					Texture cutoutTexture = target.CutoutTexture;
					if (cutoutTexture == null || cutoutTexture.dimension != TextureDimension.Tex2DArray)
					{
						parameters.Buffer.DisableShaderKeyword(KeywordsUtility.GetTextureArrayCutoutKeyword());
					}
					else
					{
						parameters.Buffer.SetGlobalFloat(OutlineEffect.TextureIndexHash, (float)target.CutoutTextureIndex);
						parameters.Buffer.EnableShaderKeyword(KeywordsUtility.GetTextureArrayCutoutKeyword());
					}
					parameters.Buffer.SetGlobalTexture(OutlineEffect.CutoutTextureHash, parameters.TextureHandleMap[cutoutTexture]);
					return;
				}
				parameters.Buffer.DisableShaderKeyword(KeywordsUtility.GetCutoutKeyword());
				return;
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004F4B File Offset: 0x0000314B
		private static void SetupCull(OutlineParameters parameters, OutlineTarget target)
		{
			parameters.Buffer.SetGlobalInt(OutlineEffect.CullHash, (int)target.CullMode);
		}

		// Token: 0x0400004C RID: 76
		public static readonly int FillRefHash = Shader.PropertyToID("_FillRef");

		// Token: 0x0400004D RID: 77
		public static readonly int ColorMaskHash = Shader.PropertyToID("_ColorMask");

		// Token: 0x0400004E RID: 78
		public static readonly int OutlineRefHash = Shader.PropertyToID("_OutlineRef");

		// Token: 0x0400004F RID: 79
		public static readonly int RefHash = Shader.PropertyToID("_Ref");

		// Token: 0x04000050 RID: 80
		public static readonly int EffectSizeHash = Shader.PropertyToID("_EffectSize");

		// Token: 0x04000051 RID: 81
		public static readonly int CullHash = Shader.PropertyToID("_Cull");

		// Token: 0x04000052 RID: 82
		public static readonly int ZTestHash = Shader.PropertyToID("_ZTest");

		// Token: 0x04000053 RID: 83
		public static readonly int ColorHash = Shader.PropertyToID("_EPOColor");

		// Token: 0x04000054 RID: 84
		public static readonly int ScaleHash = Shader.PropertyToID("_Scale");

		// Token: 0x04000055 RID: 85
		public static readonly int ShiftHash = Shader.PropertyToID("_Shift");

		// Token: 0x04000056 RID: 86
		public static readonly int InfoBufferHash = Shader.PropertyToID("_InfoBuffer");

		// Token: 0x04000057 RID: 87
		public static readonly int ComparisonHash = Shader.PropertyToID("_Comparison");

		// Token: 0x04000058 RID: 88
		public static readonly int ReadMaskHash = Shader.PropertyToID("_ReadMask");

		// Token: 0x04000059 RID: 89
		public static readonly int OperationHash = Shader.PropertyToID("_Operation");

		// Token: 0x0400005A RID: 90
		public static readonly int CutoutThresholdHash = Shader.PropertyToID("_CutoutThreshold");

		// Token: 0x0400005B RID: 91
		public static readonly int CutoutMaskHash = Shader.PropertyToID("_CutoutMask");

		// Token: 0x0400005C RID: 92
		public static readonly int TextureIndexHash = Shader.PropertyToID("_TextureIndex");

		// Token: 0x0400005D RID: 93
		public static readonly int CutoutTextureHash = Shader.PropertyToID("_CutoutTexture");

		// Token: 0x0400005E RID: 94
		public static readonly int CutoutTextureSTHash = Shader.PropertyToID("_CutoutTexture_ST");

		// Token: 0x0400005F RID: 95
		public static readonly int SrcBlendHash = Shader.PropertyToID("_SrcBlend");

		// Token: 0x04000060 RID: 96
		public static readonly int DstBlendHash = Shader.PropertyToID("_DstBlend");

		// Token: 0x04000061 RID: 97
		private static Material OutlineMaterial;

		// Token: 0x04000062 RID: 98
		private static Material ObstacleMaterial;

		// Token: 0x04000063 RID: 99
		private static Material FillMaskMaterial;

		// Token: 0x04000064 RID: 100
		private static Material ZPrepassMaterial;

		// Token: 0x04000065 RID: 101
		private static Material DilateMaterial;

		// Token: 0x04000066 RID: 102
		private static Material BlurMaterial;

		// Token: 0x04000067 RID: 103
		private static Material FinalBlitMaterial;

		// Token: 0x04000068 RID: 104
		private static Material BasicBlitMaterial;

		// Token: 0x04000069 RID: 105
		private static Material ClearStencilMaterial;

		// Token: 0x0400006A RID: 106
		private static RTHandleSystem system;

		// Token: 0x0400006B RID: 107
		private static List<OutlineEffect.OutlineTargetGroup> targets = new List<OutlineEffect.OutlineTargetGroup>();

		// Token: 0x0400006C RID: 108
		private static List<string> keywords = new List<string>();

		// Token: 0x0200002D RID: 45
		private struct OutlineTargetGroup
		{
			// Token: 0x06000105 RID: 261 RVA: 0x00006F7A File Offset: 0x0000517A
			public OutlineTargetGroup(Outlinable outlinable, OutlineTarget target)
			{
				this.Outlinable = outlinable;
				this.Target = target;
			}

			// Token: 0x040000E8 RID: 232
			public readonly Outlinable Outlinable;

			// Token: 0x040000E9 RID: 233
			public readonly OutlineTarget Target;
		}
	}
}
