using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Shapes
{
	// Token: 0x02000018 RID: 24
	public static class Draw
	{
		// Token: 0x06000297 RID: 663 RVA: 0x00007782 File Offset: 0x00005982
		public static DrawCommand Command(Camera cam, RenderPassEvent cameraEvent = RenderPassEvent.BeforeRenderingPostProcessing)
		{
			return ObjectPool<DrawCommand>.Alloc().Initialize(cam, cameraEvent);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00007790 File Offset: 0x00005990
		public static void PrepareForIMGUI()
		{
			float num = (float)Screen.width;
			float num2 = (float)Screen.height;
			float num3 = num;
			float num4 = num2;
			Vector4 value = new Vector4(num3, num4, 1f + 1f / num3, 1f + 1f / num4);
			Shader.SetGlobalVector(ShapesMaterialUtils.propScreenParams, value);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x000077DC File Offset: 0x000059DC
		[OvldGenCallTarget]
		private static void Line_Internal([OvldDefault("LineEndCaps")] LineEndCap endCaps, [OvldDefault("ThicknessSpace")] ThicknessSpace thicknessSpace, Vector3 start, Vector3 end, [OvldDefault("Color")] Color colorStart, [OvldDefault("Color")] Color colorEnd, [OvldDefault("Thickness")] float thickness)
		{
			using (new IMDrawer(Draw.mpbLine, ShapesMaterialUtils.GetLineMat(Draw.LineGeometry, endCaps)[Draw.BlendMode], ShapesMeshUtils.GetLineMesh(Draw.LineGeometry, endCaps, Draw.DetailLevel), 0, IMDrawer.DrawType.Shape, true, -1))
			{
				MetaMpb.ApplyDashSettings<MpbLine2D>(Draw.mpbLine, thickness);
				Draw.mpbLine.color.Add(colorStart.ColorSpaceAdjusted());
				Draw.mpbLine.colorEnd.Add(colorEnd.ColorSpaceAdjusted());
				Draw.mpbLine.pointStart.Add(start);
				Draw.mpbLine.pointEnd.Add(end);
				Draw.mpbLine.thickness.Add(thickness);
				Draw.mpbLine.alignment.Add((float)Draw.LineGeometry);
				Draw.mpbLine.thicknessSpace.Add((float)thicknessSpace);
				Draw.mpbLine.scaleMode.Add((float)Draw.ScaleMode);
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x000078F8 File Offset: 0x00005AF8
		[OvldGenCallTarget]
		private static void Polyline_Internal(PolylinePath path, [OvldDefault("false")] bool closed, [OvldDefault("PolylineGeometry")] PolylineGeometry geometry, [OvldDefault("PolylineJoins")] PolylineJoins joins, [OvldDefault("Thickness")] float thickness, [OvldDefault("ThicknessSpace")] ThicknessSpace thicknessSpace, [OvldDefault("Color")] Color color)
		{
			Draw.<>c__DisplayClass7_0 CS$<>8__locals1;
			CS$<>8__locals1.thickness = thickness;
			CS$<>8__locals1.thicknessSpace = thicknessSpace;
			CS$<>8__locals1.color = color;
			CS$<>8__locals1.geometry = geometry;
			Mesh sourceMesh;
			if (!path.EnsureMeshIsReadyToRender(closed, joins, out sourceMesh))
			{
				return;
			}
			int count = path.Count;
			if (count == 0)
			{
				Debug.LogWarning("Tried to draw polyline with no points");
				return;
			}
			if (count != 1)
			{
				if (DrawCommand.IsAddingDrawCommandsToBuffer)
				{
					path.RegisterToCommandBuffer(DrawCommand.CurrentWritingCommandBuffer);
				}
				using (new IMDrawer(Draw.mpbPolyline, ShapesMaterialUtils.GetPolylineMat(joins)[Draw.BlendMode], sourceMesh, 0, IMDrawer.DrawType.Shape, true, -1))
				{
					Draw.<Polyline_Internal>g__ApplyToMpb|7_0(Draw.mpbPolyline, ref CS$<>8__locals1);
				}
				if (joins.HasJoinMesh())
				{
					using (new IMDrawer(Draw.mpbPolylineJoins, ShapesMaterialUtils.GetPolylineJoinsMat(joins)[Draw.BlendMode], sourceMesh, 1, IMDrawer.DrawType.Shape, true, -1))
					{
						Draw.<Polyline_Internal>g__ApplyToMpb|7_0(Draw.mpbPolylineJoins, ref CS$<>8__locals1);
					}
				}
				return;
			}
			Debug.LogWarning("Tried to draw polyline with only one point");
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00007A0C File Offset: 0x00005C0C
		[OvldGenCallTarget]
		private static void Polygon_Internal(PolygonPath path, [OvldDefault("PolygonTriangulation")] PolygonTriangulation triangulation, [OvldDefault("Color")] Color color)
		{
			Mesh sourceMesh;
			if (!path.EnsureMeshIsReadyToRender(triangulation, out sourceMesh))
			{
				return;
			}
			switch (path.Count)
			{
			case 0:
				Debug.LogWarning("Tried to draw polygon with no points");
				return;
			case 1:
				Debug.LogWarning("Tried to draw polygon with only one point");
				return;
			case 2:
				Debug.LogWarning("Tried to draw polygon with only two points");
				return;
			default:
				if (DrawCommand.IsAddingDrawCommandsToBuffer)
				{
					path.RegisterToCommandBuffer(DrawCommand.CurrentWritingCommandBuffer);
				}
				using (new IMDrawer(Draw.mpbPolygon, ShapesMaterialUtils.matPolygon[Draw.BlendMode], sourceMesh, 0, IMDrawer.DrawType.Shape, true, -1))
				{
					MetaMpb.ApplyColorOrFill<MpbPolygon>(Draw.mpbPolygon, color);
				}
				return;
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00007AC0 File Offset: 0x00005CC0
		[OvldGenCallTarget]
		[MethodImpl(256)]
		private static void Disc_Internal([OvldDefault("Radius")] float radius, [OvldDefault("Color")] DiscColors colors)
		{
			Draw.DiscCore(false, false, radius, 0f, colors, 0f, 0f, ArcEndCap.None);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00007ADB File Offset: 0x00005CDB
		[OvldGenCallTarget]
		[MethodImpl(256)]
		private static void Ring_Internal([OvldDefault("Radius")] float radius, [OvldDefault("Thickness")] float thickness, [OvldDefault("Color")] DiscColors colors)
		{
			Draw.DiscCore(true, false, radius, thickness, colors, 0f, 0f, ArcEndCap.None);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00007AF2 File Offset: 0x00005CF2
		[OvldGenCallTarget]
		[MethodImpl(256)]
		private static void Pie_Internal([OvldDefault("Radius")] float radius, [OvldDefault("Color")] DiscColors colors, float angleRadStart, float angleRadEnd)
		{
			Draw.DiscCore(false, true, radius, 0f, colors, angleRadStart, angleRadEnd, ArcEndCap.None);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00007B05 File Offset: 0x00005D05
		[OvldGenCallTarget]
		[MethodImpl(256)]
		private static void Arc_Internal([OvldDefault("Radius")] float radius, [OvldDefault("Thickness")] float thickness, [OvldDefault("Color")] DiscColors colors, float angleRadStart, float angleRadEnd, [OvldDefault("ArcEndCap.None")] ArcEndCap endCaps)
		{
			Draw.DiscCore(true, true, radius, thickness, colors, angleRadStart, angleRadEnd, endCaps);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00007B18 File Offset: 0x00005D18
		private static void DiscCore(bool hollow, bool sector, float radius, float thickness, DiscColors colors, float angleRadStart = 0f, float angleRadEnd = 0f, ArcEndCap arcEndCaps = ArcEndCap.None)
		{
			if (sector && Mathf.Abs(angleRadEnd - angleRadStart) < 0.0001f)
			{
				return;
			}
			using (new IMDrawer(Draw.mpbDisc, ShapesMaterialUtils.GetDiscMaterial(hollow, sector)[Draw.BlendMode], ShapesMeshUtils.QuadMesh[0], 0, IMDrawer.DrawType.Shape, true, -1))
			{
				MetaMpb.ApplyDashSettings<MpbDisc>(Draw.mpbDisc, thickness);
				Draw.mpbDisc.radius.Add(radius);
				Draw.mpbDisc.radiusSpace.Add((float)Draw.RadiusSpace);
				Draw.mpbDisc.alignment.Add((float)Draw.DiscGeometry);
				Draw.mpbDisc.thicknessSpace.Add((float)Draw.ThicknessSpace);
				Draw.mpbDisc.thickness.Add(thickness);
				Draw.mpbDisc.scaleMode.Add((float)Draw.ScaleMode);
				Draw.mpbDisc.angleStart.Add(angleRadStart);
				Draw.mpbDisc.angleEnd.Add(angleRadEnd);
				Draw.mpbDisc.roundCaps.Add((float)arcEndCaps);
				Draw.mpbDisc.color.Add(colors.innerStart.ColorSpaceAdjusted());
				Draw.mpbDisc.colorOuterStart.Add(colors.outerStart.ColorSpaceAdjusted());
				Draw.mpbDisc.colorInnerEnd.Add(colors.innerEnd.ColorSpaceAdjusted());
				Draw.mpbDisc.colorOuterEnd.Add(colors.outerEnd.ColorSpaceAdjusted());
			}
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00007CC4 File Offset: 0x00005EC4
		[OvldGenCallTarget]
		private static void RegularPolygon_Internal([OvldDefault("RegularPolygonSideCount")] int sideCount, [OvldDefault("Radius")] float radius, [OvldDefault("Thickness")] float thickness, [OvldDefault("Color")] Color color, bool hollow, [OvldDefault("0f")] float roundness, [OvldDefault("0f")] float angle)
		{
			using (new IMDrawer(Draw.mpbRegularPolygon, ShapesMaterialUtils.matRegularPolygon[Draw.BlendMode], ShapesMeshUtils.QuadMesh[0], 0, IMDrawer.DrawType.Shape, true, -1))
			{
				MetaMpb.ApplyColorOrFill<MpbRegularPolygon>(Draw.mpbRegularPolygon, color);
				MetaMpb.ApplyDashSettings<MpbRegularPolygon>(Draw.mpbRegularPolygon, thickness);
				Draw.mpbRegularPolygon.radius.Add(radius);
				Draw.mpbRegularPolygon.radiusSpace.Add((float)Draw.RadiusSpace);
				Draw.mpbRegularPolygon.alignment.Add((float)Draw.RegularPolygonGeometry);
				Draw.mpbRegularPolygon.sides.Add((float)Mathf.Max(3, sideCount));
				Draw.mpbRegularPolygon.angle.Add(angle);
				Draw.mpbRegularPolygon.roundness.Add(roundness);
				Draw.mpbRegularPolygon.hollow.Add((float)hollow.AsInt());
				Draw.mpbRegularPolygon.thicknessSpace.Add((float)Draw.ThicknessSpace);
				Draw.mpbRegularPolygon.thickness.Add(thickness);
				Draw.mpbRegularPolygon.scaleMode.Add((float)Draw.ScaleMode);
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00007DF4 File Offset: 0x00005FF4
		[OvldGenCallTarget]
		private static void Rectangle_Internal([OvldDefault("BlendMode")] ShapesBlendMode blendMode, [OvldDefault("false")] bool hollow, Rect rect, [OvldDefault("Color")] Color color, [OvldDefault("Thickness")] float thickness, [OvldDefault("default")] Vector4 cornerRadii)
		{
			bool rounded = ShapesMath.MaxComp(cornerRadii) >= 0.0001f;
			if (rect.width < 0f)
			{
				rect.x -= (rect.width *= -1f);
			}
			if (rect.height < 0f)
			{
				rect.y -= (rect.height *= -1f);
			}
			using (new IMDrawer(Draw.mpbRect, ShapesMaterialUtils.GetRectMaterial(hollow, rounded)[blendMode], ShapesMeshUtils.QuadMesh[0], 0, IMDrawer.DrawType.Shape, true, -1))
			{
				MetaMpb.ApplyColorOrFill<MpbRect>(Draw.mpbRect, color);
				MetaMpb.ApplyDashSettings<MpbRect>(Draw.mpbRect, thickness);
				Draw.mpbRect.rect.Add(rect.ToVector4());
				Draw.mpbRect.cornerRadii.Add(cornerRadii);
				Draw.mpbRect.thickness.Add(thickness);
				Draw.mpbRect.thicknessSpace.Add((float)Draw.ThicknessSpace);
				Draw.mpbRect.scaleMode.Add((float)Draw.ScaleMode);
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00007F34 File Offset: 0x00006134
		[OvldGenCallTarget]
		private static void Triangle_Internal(Vector3 a, Vector3 b, Vector3 c, bool hollow, [OvldDefault("Thickness")] float thickness, [OvldDefault("0f")] float roundness, [OvldDefault("Color")] Color colorA, [OvldDefault("Color")] Color colorB, [OvldDefault("Color")] Color colorC)
		{
			using (new IMDrawer(Draw.mpbTriangle, ShapesMaterialUtils.matTriangle[Draw.BlendMode], ShapesMeshUtils.TriangleMesh[0], 0, IMDrawer.DrawType.Shape, true, -1))
			{
				MetaMpb.ApplyDashSettings<MpbTriangle>(Draw.mpbTriangle, thickness);
				Draw.mpbTriangle.a.Add(a);
				Draw.mpbTriangle.b.Add(b);
				Draw.mpbTriangle.c.Add(c);
				Draw.mpbTriangle.color.Add(colorA.ColorSpaceAdjusted());
				Draw.mpbTriangle.colorB.Add(colorB.ColorSpaceAdjusted());
				Draw.mpbTriangle.colorC.Add(colorC.ColorSpaceAdjusted());
				Draw.mpbTriangle.roundness.Add(roundness);
				Draw.mpbTriangle.hollow.Add((float)hollow.AsInt());
				Draw.mpbTriangle.thicknessSpace.Add((float)Draw.ThicknessSpace);
				Draw.mpbTriangle.thickness.Add(thickness);
				Draw.mpbTriangle.scaleMode.Add((float)Draw.ScaleMode);
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00008094 File Offset: 0x00006294
		[OvldGenCallTarget]
		private static void Quad_Internal(Vector3 a, Vector3 b, Vector3 c, [OvldDefault("a + ( c - b )")] Vector3 d, [OvldDefault("Color")] Color colorA, [OvldDefault("Color")] Color colorB, [OvldDefault("Color")] Color colorC, [OvldDefault("Color")] Color colorD)
		{
			using (new IMDrawer(Draw.mpbQuad, ShapesMaterialUtils.matQuad[Draw.BlendMode], ShapesMeshUtils.QuadMesh[0], 0, IMDrawer.DrawType.Shape, true, -1))
			{
				Draw.mpbQuad.a.Add(a);
				Draw.mpbQuad.b.Add(b);
				Draw.mpbQuad.c.Add(c);
				Draw.mpbQuad.d.Add(d);
				Draw.mpbQuad.color.Add(colorA.ColorSpaceAdjusted());
				Draw.mpbQuad.colorB.Add(colorB.ColorSpaceAdjusted());
				Draw.mpbQuad.colorC.Add(colorC.ColorSpaceAdjusted());
				Draw.mpbQuad.colorD.Add(colorD.ColorSpaceAdjusted());
			}
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x000081A8 File Offset: 0x000063A8
		[OvldGenCallTarget]
		private static void Sphere_Internal([OvldDefault("Radius")] float radius, [OvldDefault("Color")] Color color)
		{
			using (new IMDrawer(Draw.metaMpbSphere, ShapesMaterialUtils.matSphere[Draw.BlendMode], ShapesMeshUtils.SphereMesh[(int)Draw.DetailLevel], 0, IMDrawer.DrawType.Shape, true, -1))
			{
				Draw.metaMpbSphere.color.Add(color.ColorSpaceAdjusted());
				Draw.metaMpbSphere.radius.Add(radius);
				Draw.metaMpbSphere.radiusSpace.Add((float)Draw.RadiusSpace);
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00008240 File Offset: 0x00006440
		[OvldGenCallTarget]
		private static void Cone_Internal(float radius, float length, [OvldDefault("true")] bool fillCap, [OvldDefault("Color")] Color color)
		{
			Mesh sourceMesh = fillCap ? ShapesMeshUtils.ConeMesh[(int)Draw.DetailLevel] : ShapesMeshUtils.ConeMeshUncapped[(int)Draw.DetailLevel];
			using (new IMDrawer(Draw.mpbCone, ShapesMaterialUtils.matCone[Draw.BlendMode], sourceMesh, 0, IMDrawer.DrawType.Shape, true, -1))
			{
				Draw.mpbCone.color.Add(color.ColorSpaceAdjusted());
				Draw.mpbCone.radius.Add(radius);
				Draw.mpbCone.length.Add(length);
				Draw.mpbCone.sizeSpace.Add((float)Draw.SizeSpace);
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x000082F8 File Offset: 0x000064F8
		[OvldGenCallTarget]
		private static void Cuboid_Internal(Vector3 size, [OvldDefault("Color")] Color color)
		{
			using (new IMDrawer(Draw.mpbCuboid, ShapesMaterialUtils.matCuboid[Draw.BlendMode], ShapesMeshUtils.CuboidMesh[0], 0, IMDrawer.DrawType.Shape, true, -1))
			{
				Draw.mpbCuboid.color.Add(color.ColorSpaceAdjusted());
				Draw.mpbCuboid.size.Add(size);
				Draw.mpbCuboid.sizeSpace.Add((float)Draw.SizeSpace);
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00008390 File Offset: 0x00006590
		[OvldGenCallTarget]
		private static void Torus_Internal(float radius, float thickness, [OvldDefault("0")] float angleRadStart, [OvldDefault("ShapesMath.TAU")] float angleRadEnd, [OvldDefault("Color")] Color color)
		{
			if (thickness < 0.0001f)
			{
				return;
			}
			if (radius < 1E-05f)
			{
				ThicknessSpace radiusSpace = Draw.RadiusSpace;
				Draw.RadiusSpace = Draw.ThicknessSpace;
				Draw.Sphere(thickness / 2f, color);
				Draw.RadiusSpace = radiusSpace;
				return;
			}
			using (new IMDrawer(Draw.mpbTorus, ShapesMaterialUtils.matTorus[Draw.BlendMode], ShapesMeshUtils.TorusMesh[(int)Draw.DetailLevel], 0, IMDrawer.DrawType.Shape, true, -1))
			{
				Draw.mpbTorus.color.Add(color.ColorSpaceAdjusted());
				Draw.mpbTorus.radius.Add(radius);
				Draw.mpbTorus.thickness.Add(thickness);
				Draw.mpbTorus.radiusSpace.Add((float)Draw.RadiusSpace);
				Draw.mpbTorus.thicknessSpace.Add((float)Draw.ThicknessSpace);
				Draw.mpbTorus.scaleMode.Add((float)Draw.ScaleMode);
				Draw.mpbTorus.angleStart.Add(angleRadStart);
				Draw.mpbTorus.angleEnd.Add(angleRadEnd);
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x000084B8 File Offset: 0x000066B8
		[OvldGenCallTarget]
		private static void TextRect_Internal(string content, [OvldDefault("null")] TextElement element, Rect rect, [OvldDefault("Font")] TMP_FontAsset font, [OvldDefault("FontSize")] float fontSize, [OvldDefault("TextAlign")] TextAlign align, [OvldDefault("Color")] Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(rect.x, rect.y);
			Draw.Text_Internal(true, content, element, default(Vector2), rect.size, font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00008504 File Offset: 0x00006704
		[OvldGenCallTarget]
		private static void Text_Internal(bool isRect, string content, [OvldDefault("null")] TextElement element, [OvldDefault("default")] Vector2 pivot, [OvldDefault("default")] Vector2 size, [OvldDefault("Font")] TMP_FontAsset font, [OvldDefault("FontSize")] float fontSize, [OvldDefault("TextAlign")] TextAlign align, [OvldDefault("Color")] Color color)
		{
			int num;
			TextMeshProShapes tmp;
			IMDrawer.DrawType drawType;
			if (element == null)
			{
				num = TextElement.GetNextId();
				tmp = ShapesObjPool<TextMeshProShapes, ShapesTextPool>.Instance.AllocateElement(num);
				drawType = IMDrawer.DrawType.TextPooledAuto;
			}
			else
			{
				num = element.id;
				tmp = element.Tmp;
				drawType = IMDrawer.DrawType.TextPooledPersistent;
			}
			Draw.ApplyTextValuesToInstance(tmp, isRect, content, font, fontSize, align, pivot, size, color);
			Draw.Text_Internal(tmp, drawType, num);
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060002AB RID: 683 RVA: 0x00008555 File Offset: 0x00006755
		private static Draw.OnPreRenderTmpDelegate OnPreRenderTmp
		{
			get
			{
				if (Draw.onPreRenderTmp == null)
				{
					Draw.onPreRenderTmp = (Draw.OnPreRenderTmpDelegate)typeof(TextMeshPro).GetMethod("OnPreRenderObject", 36).CreateDelegate(typeof(Draw.OnPreRenderTmpDelegate));
				}
				return Draw.onPreRenderTmp;
			}
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00008594 File Offset: 0x00006794
		private static void ApplyTextValuesToInstance(TextMeshProShapes tmp, bool isRect, string content, TMP_FontAsset font, float fontSize, TextAlign align, Vector2 pivot, Vector2 size, Color color)
		{
			tmp.fontStyle = Draw.FontStyle;
			tmp.characterSpacing = Draw.TextCharacterSpacing;
			tmp.wordSpacing = Draw.TextWordSpacing;
			tmp.lineSpacing = Draw.TextLineSpacing;
			tmp.paragraphSpacing = Draw.TextParagraphSpacing;
			tmp.margin = Draw.TextMargins;
			tmp.font = font;
			tmp.color = color;
			tmp.fontSize = fontSize;
			tmp.alignment = align.GetTMPAlignment();
			tmp.text = content;
			tmp.Curvature = Draw.TextCurvature;
			tmp.CurvaturePivot = Draw.TextCurvaturePivot;
			if (isRect)
			{
				tmp.textWrappingMode = Draw.TextWrap;
				tmp.overflowMode = Draw.TextOverflow;
				tmp.rectTransform.pivot = pivot;
				tmp.rectTransform.sizeDelta = size;
			}
			else
			{
				tmp.textWrappingMode = TextWrappingModes.NoWrap;
				tmp.overflowMode = TextOverflowModes.Overflow;
				tmp.rectTransform.sizeDelta = default(Vector2);
			}
			tmp.rectTransform.position = Draw.Matrix.GetColumn(3);
			tmp.rectTransform.rotation = Draw.Matrix.rotation;
			Draw.OnPreRenderTmp(tmp);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x000086BC File Offset: 0x000068BC
		private static void Text_Internal(TextMeshPro tmp, IMDrawer.DrawType drawType, int disposeId = -1)
		{
			using (new IMDrawer(Draw.mpbText, tmp.fontSharedMaterial, tmp.mesh, 0, drawType, false, disposeId))
			{
			}
			for (int i = 0; i < tmp.transform.childCount; i++)
			{
				tmp.transform.GetChild(i).GetComponent<TMP_SubMesh>().renderer.enabled = false;
			}
			if (tmp.textInfo.materialCount > 1)
			{
				for (int j = 0; j < tmp.transform.childCount; j++)
				{
					TMP_SubMesh component = tmp.transform.GetChild(j).GetComponent<TMP_SubMesh>();
					component.renderer.enabled = false;
					if (!(component.sharedMaterial == null))
					{
						using (new IMDrawer(Draw.mpbText, component.sharedMaterial, component.mesh, 0, drawType, false, -1))
						{
						}
					}
				}
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x000087C0 File Offset: 0x000069C0
		public static void Mesh(Mesh mesh, Material mat)
		{
			Draw.CustomMesh_Internal(mesh, mat, null);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x000087CA File Offset: 0x000069CA
		public static void Mesh(Mesh mesh, Material mat, MaterialPropertyBlock mpb)
		{
			Draw.CustomMesh_Internal(mesh, mat, mpb);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x000087D4 File Offset: 0x000069D4
		private static void CustomMesh_Internal(Mesh mesh, Material mat, MaterialPropertyBlock mpb)
		{
			if (mesh == null)
			{
				throw new NullReferenceException("null mesh passed into Draw.Mesh");
			}
			using (new IMDrawer(Draw.mpbCustomMesh, mat, mesh, 0, IMDrawer.DrawType.Custom, false, -1))
			{
				Draw.mpbCustomMesh.mpbOverride = mpb;
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00008834 File Offset: 0x00006A34
		[OvldGenCallTarget]
		private static void Texture_Internal(Texture texture, Rect rect, Rect uvs, [OvldDefault("Color")] Color color)
		{
			if (texture == null)
			{
				return;
			}
			Material sourceMat = ShapesMaterialUtils.matTexture[Draw.BlendMode];
			if (Draw.mpbTexture.texture != null && Draw.mpbTexture.texture != texture)
			{
				DrawCommand.CurrentWritingCommandBuffer.drawCalls.Add(Draw.mpbTexture.ExtractDrawCall());
			}
			using (new IMDrawer(Draw.mpbTexture, sourceMat, ShapesMeshUtils.QuadMesh[0], 0, IMDrawer.DrawType.Shape, true, -1))
			{
				Draw.mpbTexture.texture = texture;
				Draw.mpbTexture.color.Add(color.ColorSpaceAdjusted());
				Draw.mpbTexture.rect.Add(rect.ToVector4());
				Draw.mpbTexture.uvs.Add(uvs.ToVector4());
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00008920 File Offset: 0x00006B20
		[MethodImpl(256)]
		private static void Texture_Placement_Internal(Texture texture, [TupleElementNames(new string[]
		{
			"rect",
			"uvs"
		})] ValueTuple<Rect, Rect> placement, Color color)
		{
			Draw.Texture_Internal(texture, placement.Item1, placement.Item2, color);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00008935 File Offset: 0x00006B35
		[OvldGenCallTarget]
		[MethodImpl(256)]
		private static void Texture_RectFill_Internal(Texture texture, Rect rect, [OvldDefault("TextureFillMode.ScaleToFit")] TextureFillMode fillMode, [OvldDefault("Color")] Color color)
		{
			Draw.Texture_Placement_Internal(texture, TexturePlacement.Fit(texture, rect, fillMode), color);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00008946 File Offset: 0x00006B46
		[OvldGenCallTarget]
		[MethodImpl(256)]
		private static void Texture_PosSize_Internal(Texture texture, Vector2 center, float size, [OvldDefault("TextureSizeMode.LongestSide")] TextureSizeMode sizeMode, [OvldDefault("Color")] Color color)
		{
			Draw.Texture_Placement_Internal(texture, TexturePlacement.Size(texture, center, size, sizeMode), color);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00008959 File Offset: 0x00006B59
		[MethodImpl(256)]
		public static void Line(Vector3 start, Vector3 end)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.ThicknessSpace, start, end, Draw.Color, Draw.Color, Draw.Thickness);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000897B File Offset: 0x00006B7B
		[MethodImpl(256)]
		public static void Line(Vector3 start, Vector3 end, Color color)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.ThicknessSpace, start, end, color, color, Draw.Thickness);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00008995 File Offset: 0x00006B95
		[MethodImpl(256)]
		public static void Line(Vector3 start, Vector3 end, Color colorStart, Color colorEnd)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.ThicknessSpace, start, end, colorStart, colorEnd, Draw.Thickness);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x000089AF File Offset: 0x00006BAF
		[MethodImpl(256)]
		public static void Line(Vector3 start, Vector3 end, float thickness)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.ThicknessSpace, start, end, Draw.Color, Draw.Color, thickness);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x000089CD File Offset: 0x00006BCD
		[MethodImpl(256)]
		public static void Line(Vector3 start, Vector3 end, float thickness, Color color)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.ThicknessSpace, start, end, color, color, thickness);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000089E3 File Offset: 0x00006BE3
		[MethodImpl(256)]
		public static void Line(Vector3 start, Vector3 end, float thickness, Color colorStart, Color colorEnd)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.ThicknessSpace, start, end, colorStart, colorEnd, thickness);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x000089FA File Offset: 0x00006BFA
		[MethodImpl(256)]
		public static void Line(Vector3 start, Vector3 end, LineEndCap endCaps)
		{
			Draw.Line_Internal(endCaps, Draw.ThicknessSpace, start, end, Draw.Color, Draw.Color, Draw.Thickness);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00008A18 File Offset: 0x00006C18
		[MethodImpl(256)]
		public static void Line(Vector3 start, Vector3 end, LineEndCap endCaps, Color color)
		{
			Draw.Line_Internal(endCaps, Draw.ThicknessSpace, start, end, color, color, Draw.Thickness);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00008A2E File Offset: 0x00006C2E
		[MethodImpl(256)]
		public static void Line(Vector3 start, Vector3 end, LineEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.Line_Internal(endCaps, Draw.ThicknessSpace, start, end, colorStart, colorEnd, Draw.Thickness);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00008A45 File Offset: 0x00006C45
		[MethodImpl(256)]
		public static void Line(Vector3 start, Vector3 end, float thickness, LineEndCap endCaps)
		{
			Draw.Line_Internal(endCaps, Draw.ThicknessSpace, start, end, Draw.Color, Draw.Color, thickness);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00008A5F File Offset: 0x00006C5F
		[MethodImpl(256)]
		public static void Line(Vector3 start, Vector3 end, float thickness, LineEndCap endCaps, Color color)
		{
			Draw.Line_Internal(endCaps, Draw.ThicknessSpace, start, end, color, color, thickness);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00008A73 File Offset: 0x00006C73
		[MethodImpl(256)]
		public static void Line(Vector3 start, Vector3 end, float thickness, LineEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.Line_Internal(endCaps, Draw.ThicknessSpace, start, end, colorStart, colorEnd, thickness);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00008A87 File Offset: 0x00006C87
		[MethodImpl(256)]
		public static void Polyline(PolylinePath path)
		{
			Draw.Polyline_Internal(path, false, Draw.PolylineGeometry, Draw.PolylineJoins, Draw.Thickness, Draw.ThicknessSpace, Draw.Color);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00008AA9 File Offset: 0x00006CA9
		[MethodImpl(256)]
		public static void Polyline(PolylinePath path, bool closed)
		{
			Draw.Polyline_Internal(path, closed, Draw.PolylineGeometry, Draw.PolylineJoins, Draw.Thickness, Draw.ThicknessSpace, Draw.Color);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00008ACB File Offset: 0x00006CCB
		[MethodImpl(256)]
		public static void Polyline(PolylinePath path, float thickness)
		{
			Draw.Polyline_Internal(path, false, Draw.PolylineGeometry, Draw.PolylineJoins, thickness, Draw.ThicknessSpace, Draw.Color);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00008AE9 File Offset: 0x00006CE9
		[MethodImpl(256)]
		public static void Polyline(PolylinePath path, bool closed, float thickness)
		{
			Draw.Polyline_Internal(path, closed, Draw.PolylineGeometry, Draw.PolylineJoins, thickness, Draw.ThicknessSpace, Draw.Color);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00008B07 File Offset: 0x00006D07
		[MethodImpl(256)]
		public static void Polyline(PolylinePath path, PolylineJoins joins)
		{
			Draw.Polyline_Internal(path, false, Draw.PolylineGeometry, joins, Draw.Thickness, Draw.ThicknessSpace, Draw.Color);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00008B25 File Offset: 0x00006D25
		[MethodImpl(256)]
		public static void Polyline(PolylinePath path, bool closed, PolylineJoins joins)
		{
			Draw.Polyline_Internal(path, closed, Draw.PolylineGeometry, joins, Draw.Thickness, Draw.ThicknessSpace, Draw.Color);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00008B43 File Offset: 0x00006D43
		[MethodImpl(256)]
		public static void Polyline(PolylinePath path, float thickness, PolylineJoins joins)
		{
			Draw.Polyline_Internal(path, false, Draw.PolylineGeometry, joins, thickness, Draw.ThicknessSpace, Draw.Color);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00008B5D File Offset: 0x00006D5D
		[MethodImpl(256)]
		public static void Polyline(PolylinePath path, bool closed, float thickness, PolylineJoins joins)
		{
			Draw.Polyline_Internal(path, closed, Draw.PolylineGeometry, joins, thickness, Draw.ThicknessSpace, Draw.Color);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00008B77 File Offset: 0x00006D77
		[MethodImpl(256)]
		public static void Polyline(PolylinePath path, Color color)
		{
			Draw.Polyline_Internal(path, false, Draw.PolylineGeometry, Draw.PolylineJoins, Draw.Thickness, Draw.ThicknessSpace, color);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00008B95 File Offset: 0x00006D95
		[MethodImpl(256)]
		public static void Polyline(PolylinePath path, bool closed, Color color)
		{
			Draw.Polyline_Internal(path, closed, Draw.PolylineGeometry, Draw.PolylineJoins, Draw.Thickness, Draw.ThicknessSpace, color);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00008BB3 File Offset: 0x00006DB3
		[MethodImpl(256)]
		public static void Polyline(PolylinePath path, float thickness, Color color)
		{
			Draw.Polyline_Internal(path, false, Draw.PolylineGeometry, Draw.PolylineJoins, thickness, Draw.ThicknessSpace, color);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00008BCD File Offset: 0x00006DCD
		[MethodImpl(256)]
		public static void Polyline(PolylinePath path, bool closed, float thickness, Color color)
		{
			Draw.Polyline_Internal(path, closed, Draw.PolylineGeometry, Draw.PolylineJoins, thickness, Draw.ThicknessSpace, color);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00008BE7 File Offset: 0x00006DE7
		[MethodImpl(256)]
		public static void Polyline(PolylinePath path, PolylineJoins joins, Color color)
		{
			Draw.Polyline_Internal(path, false, Draw.PolylineGeometry, joins, Draw.Thickness, Draw.ThicknessSpace, color);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00008C01 File Offset: 0x00006E01
		[MethodImpl(256)]
		public static void Polyline(PolylinePath path, bool closed, PolylineJoins joins, Color color)
		{
			Draw.Polyline_Internal(path, closed, Draw.PolylineGeometry, joins, Draw.Thickness, Draw.ThicknessSpace, color);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00008C1B File Offset: 0x00006E1B
		[MethodImpl(256)]
		public static void Polyline(PolylinePath path, float thickness, PolylineJoins joins, Color color)
		{
			Draw.Polyline_Internal(path, false, Draw.PolylineGeometry, joins, thickness, Draw.ThicknessSpace, color);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00008C31 File Offset: 0x00006E31
		[MethodImpl(256)]
		public static void Polyline(PolylinePath path, bool closed, float thickness, PolylineJoins joins, Color color)
		{
			Draw.Polyline_Internal(path, closed, Draw.PolylineGeometry, joins, thickness, Draw.ThicknessSpace, color);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00008C48 File Offset: 0x00006E48
		[MethodImpl(256)]
		public static void Polygon(PolygonPath path)
		{
			Draw.Polygon_Internal(path, Draw.PolygonTriangulation, Draw.Color);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00008C5A File Offset: 0x00006E5A
		[MethodImpl(256)]
		public static void Polygon(PolygonPath path, Color color)
		{
			Draw.Polygon_Internal(path, Draw.PolygonTriangulation, color);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00008C68 File Offset: 0x00006E68
		[MethodImpl(256)]
		public static void Polygon(PolygonPath path, PolygonTriangulation triangulation)
		{
			Draw.Polygon_Internal(path, triangulation, Draw.Color);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00008C76 File Offset: 0x00006E76
		[MethodImpl(256)]
		public static void Polygon(PolygonPath path, PolygonTriangulation triangulation, Color color)
		{
			Draw.Polygon_Internal(path, triangulation, color);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00008C80 File Offset: 0x00006E80
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.Radius, Draw.Thickness, Draw.Color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00008CB6 File Offset: 0x00006EB6
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.Radius, Draw.Thickness, color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00008CE8 File Offset: 0x00006EE8
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, Draw.Color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00008D1A File Offset: 0x00006F1A
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00008D48 File Offset: 0x00006F48
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, Draw.Color, false, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00008D76 File Offset: 0x00006F76
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, float radius, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, color, false, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00008DA0 File Offset: 0x00006FA0
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, Draw.Color, false, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00008DCA File Offset: 0x00006FCA
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, float radius, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, color, false, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00008DF1 File Offset: 0x00006FF1
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, Draw.Radius, Draw.Thickness, Draw.Color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00008E23 File Offset: 0x00007023
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, int sideCount, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, Draw.Radius, Draw.Thickness, color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00008E51 File Offset: 0x00007051
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, Draw.Color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00008E7F File Offset: 0x0000707F
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, int sideCount, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00008EA9 File Offset: 0x000070A9
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, int sideCount, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, Draw.Color, false, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00008ED3 File Offset: 0x000070D3
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, int sideCount, float radius, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, color, false, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00008EFA File Offset: 0x000070FA
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, int sideCount, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, Draw.Color, false, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00008F21 File Offset: 0x00007121
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, int sideCount, float radius, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, color, false, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00008F48 File Offset: 0x00007148
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Vector3 normal)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.Radius, Draw.Thickness, Draw.Color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00008FA4 File Offset: 0x000071A4
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Vector3 normal, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.Radius, Draw.Thickness, color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00008FFC File Offset: 0x000071FC
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Vector3 normal, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, Draw.Color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00009054 File Offset: 0x00007254
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Vector3 normal, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x000090A8 File Offset: 0x000072A8
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Vector3 normal, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, Draw.Color, false, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002EA RID: 746 RVA: 0x000090FC File Offset: 0x000072FC
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Vector3 normal, float radius, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, color, false, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000914C File Offset: 0x0000734C
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Vector3 normal, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, Draw.Color, false, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000919C File Offset: 0x0000739C
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Vector3 normal, float radius, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, color, false, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002ED RID: 749 RVA: 0x000091EC File Offset: 0x000073EC
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Vector3 normal, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.Radius, Draw.Thickness, Draw.Color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00009244 File Offset: 0x00007444
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Vector3 normal, int sideCount, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.Radius, Draw.Thickness, color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00009298 File Offset: 0x00007498
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Vector3 normal, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, Draw.Color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x000092EC File Offset: 0x000074EC
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Vector3 normal, int sideCount, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000933C File Offset: 0x0000753C
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, Draw.Color, false, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000938C File Offset: 0x0000758C
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, color, false, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x000093DC File Offset: 0x000075DC
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, Draw.Color, false, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00009429 File Offset: 0x00007629
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, color, false, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00009468 File Offset: 0x00007668
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Quaternion rot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.Radius, Draw.Thickness, Draw.Color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x000094C0 File Offset: 0x000076C0
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Quaternion rot, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.Radius, Draw.Thickness, color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00009514 File Offset: 0x00007714
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Quaternion rot, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, Draw.Color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00009568 File Offset: 0x00007768
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Quaternion rot, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x000095B8 File Offset: 0x000077B8
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Quaternion rot, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, Draw.Color, false, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00009606 File Offset: 0x00007806
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Quaternion rot, float radius, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, color, false, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00009646 File Offset: 0x00007846
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Quaternion rot, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, Draw.Color, false, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00009686 File Offset: 0x00007886
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Quaternion rot, float radius, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, color, false, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x060002FD RID: 765 RVA: 0x000096C4 File Offset: 0x000078C4
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Quaternion rot, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.Radius, Draw.Thickness, Draw.Color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00009718 File Offset: 0x00007918
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Quaternion rot, int sideCount, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.Radius, Draw.Thickness, color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00009768 File Offset: 0x00007968
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Quaternion rot, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, Draw.Color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000300 RID: 768 RVA: 0x000097B6 File Offset: 0x000079B6
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Quaternion rot, int sideCount, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, color, false, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000301 RID: 769 RVA: 0x000097F6 File Offset: 0x000079F6
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, Draw.Color, false, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00009836 File Offset: 0x00007A36
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, color, false, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00009873 File Offset: 0x00007A73
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, Draw.Color, false, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000304 RID: 772 RVA: 0x000098B0 File Offset: 0x00007AB0
		[MethodImpl(256)]
		public static void RegularPolygon(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, color, false, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000098EA File Offset: 0x00007AEA
		[MethodImpl(256)]
		public static void RegularPolygon()
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.Radius, Draw.Thickness, Draw.Color, false, 0f, 0f);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00009910 File Offset: 0x00007B10
		[MethodImpl(256)]
		public static void RegularPolygon(Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.Radius, Draw.Thickness, color, false, 0f, 0f);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00009932 File Offset: 0x00007B32
		[MethodImpl(256)]
		public static void RegularPolygon(float radius)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, Draw.Color, false, 0f, 0f);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00009954 File Offset: 0x00007B54
		[MethodImpl(256)]
		public static void RegularPolygon(float radius, Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, color, false, 0f, 0f);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00009972 File Offset: 0x00007B72
		[MethodImpl(256)]
		public static void RegularPolygon(float radius, float angle)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, Draw.Color, false, 0f, angle);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00009990 File Offset: 0x00007B90
		[MethodImpl(256)]
		public static void RegularPolygon(float radius, float angle, Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, color, false, 0f, angle);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x000099AA File Offset: 0x00007BAA
		[MethodImpl(256)]
		public static void RegularPolygon(float radius, float angle, float roundness)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, Draw.Color, false, roundness, angle);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x000099C4 File Offset: 0x00007BC4
		[MethodImpl(256)]
		public static void RegularPolygon(float radius, float angle, float roundness, Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, color, false, roundness, angle);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x000099DA File Offset: 0x00007BDA
		[MethodImpl(256)]
		public static void RegularPolygon(int sideCount)
		{
			Draw.RegularPolygon_Internal(sideCount, Draw.Radius, Draw.Thickness, Draw.Color, false, 0f, 0f);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x000099FC File Offset: 0x00007BFC
		[MethodImpl(256)]
		public static void RegularPolygon(int sideCount, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, Draw.Radius, Draw.Thickness, color, false, 0f, 0f);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00009A1A File Offset: 0x00007C1A
		[MethodImpl(256)]
		public static void RegularPolygon(int sideCount, float radius)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, Draw.Color, false, 0f, 0f);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00009A38 File Offset: 0x00007C38
		[MethodImpl(256)]
		public static void RegularPolygon(int sideCount, float radius, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, color, false, 0f, 0f);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00009A52 File Offset: 0x00007C52
		[MethodImpl(256)]
		public static void RegularPolygon(int sideCount, float radius, float angle)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, Draw.Color, false, 0f, angle);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00009A6C File Offset: 0x00007C6C
		[MethodImpl(256)]
		public static void RegularPolygon(int sideCount, float radius, float angle, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, color, false, 0f, angle);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00009A82 File Offset: 0x00007C82
		[MethodImpl(256)]
		public static void RegularPolygon(int sideCount, float radius, float angle, float roundness)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, Draw.Color, false, roundness, angle);
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00009A98 File Offset: 0x00007C98
		[MethodImpl(256)]
		public static void RegularPolygon(int sideCount, float radius, float angle, float roundness, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, color, false, roundness, angle);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00009AAB File Offset: 0x00007CAB
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.Radius, Draw.Thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00009AE1 File Offset: 0x00007CE1
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.Radius, Draw.Thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00009B13 File Offset: 0x00007D13
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00009B45 File Offset: 0x00007D45
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00009B73 File Offset: 0x00007D73
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00009BA1 File Offset: 0x00007DA1
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00009BCB File Offset: 0x00007DCB
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00009BF5 File Offset: 0x00007DF5
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, float radius, float thickness, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00009C1C File Offset: 0x00007E1C
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00009C43 File Offset: 0x00007E43
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, float radius, float thickness, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00009C67 File Offset: 0x00007E67
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, Draw.Radius, Draw.Thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00009C99 File Offset: 0x00007E99
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, int sideCount, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, Draw.Radius, Draw.Thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00009CC7 File Offset: 0x00007EC7
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00009CF5 File Offset: 0x00007EF5
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, int sideCount, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00009D1F File Offset: 0x00007F1F
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, int sideCount, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00009D49 File Offset: 0x00007F49
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, int sideCount, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00009D70 File Offset: 0x00007F70
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, int sideCount, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00009D97 File Offset: 0x00007F97
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, int sideCount, float radius, float thickness, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00009DBB File Offset: 0x00007FBB
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, int sideCount, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00009DDF File Offset: 0x00007FDF
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, int sideCount, float radius, float thickness, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00009E00 File Offset: 0x00008000
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.Radius, Draw.Thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00009E5C File Offset: 0x0000805C
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.Radius, Draw.Thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00009EB4 File Offset: 0x000080B4
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00009F0C File Offset: 0x0000810C
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00009F60 File Offset: 0x00008160
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00009FB4 File Offset: 0x000081B4
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000A004 File Offset: 0x00008204
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000A054 File Offset: 0x00008254
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000A0A4 File Offset: 0x000082A4
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000A0F1 File Offset: 0x000082F1
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000A130 File Offset: 0x00008330
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.Radius, Draw.Thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000A188 File Offset: 0x00008388
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, int sideCount, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.Radius, Draw.Thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000A1DC File Offset: 0x000083DC
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000A230 File Offset: 0x00008430
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, int sideCount, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000A280 File Offset: 0x00008480
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000A2D0 File Offset: 0x000084D0
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000A320 File Offset: 0x00008520
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000A36D File Offset: 0x0000856D
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000A3AC File Offset: 0x000085AC
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000A3EB File Offset: 0x000085EB
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000A428 File Offset: 0x00008628
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.Radius, Draw.Thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000A480 File Offset: 0x00008680
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.Radius, Draw.Thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000A4D4 File Offset: 0x000086D4
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000A528 File Offset: 0x00008728
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000A578 File Offset: 0x00008778
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000A5C6 File Offset: 0x000087C6
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000A606 File Offset: 0x00008806
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000A646 File Offset: 0x00008846
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000A683 File Offset: 0x00008883
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000A6C0 File Offset: 0x000088C0
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000A6FC File Offset: 0x000088FC
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.Radius, Draw.Thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000A750 File Offset: 0x00008950
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, int sideCount, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.Radius, Draw.Thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000A7A0 File Offset: 0x000089A0
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000A7EE File Offset: 0x000089EE
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, int sideCount, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000A82E File Offset: 0x00008A2E
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000A86E File Offset: 0x00008A6E
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, 0f, 0f);
			Draw.PopMatrix();
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000A8AB File Offset: 0x00008AAB
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000A8E8 File Offset: 0x00008AE8
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, 0f, angle);
			Draw.PopMatrix();
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000A922 File Offset: 0x00008B22
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000A95C File Offset: 0x00008B5C
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, roundness, angle);
			Draw.PopMatrix();
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000A993 File Offset: 0x00008B93
		[MethodImpl(256)]
		public static void RegularPolygonBorder()
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.Radius, Draw.Thickness, Draw.Color, true, 0f, 0f);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000A9B9 File Offset: 0x00008BB9
		[MethodImpl(256)]
		public static void RegularPolygonBorder(Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.Radius, Draw.Thickness, color, true, 0f, 0f);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000A9DB File Offset: 0x00008BDB
		[MethodImpl(256)]
		public static void RegularPolygonBorder(float radius)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, Draw.Color, true, 0f, 0f);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000A9FD File Offset: 0x00008BFD
		[MethodImpl(256)]
		public static void RegularPolygonBorder(float radius, Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.Thickness, color, true, 0f, 0f);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000AA1B File Offset: 0x00008C1B
		[MethodImpl(256)]
		public static void RegularPolygonBorder(float radius, float thickness)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000AA39 File Offset: 0x00008C39
		[MethodImpl(256)]
		public static void RegularPolygonBorder(float radius, float thickness, Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, 0f, 0f);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000AA53 File Offset: 0x00008C53
		[MethodImpl(256)]
		public static void RegularPolygonBorder(float radius, float thickness, float angle)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000AA6D File Offset: 0x00008C6D
		[MethodImpl(256)]
		public static void RegularPolygonBorder(float radius, float thickness, float angle, Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, 0f, angle);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000AA83 File Offset: 0x00008C83
		[MethodImpl(256)]
		public static void RegularPolygonBorder(float radius, float thickness, float angle, float roundness)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000AA99 File Offset: 0x00008C99
		[MethodImpl(256)]
		public static void RegularPolygonBorder(float radius, float thickness, float angle, float roundness, Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, roundness, angle);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000AAAC File Offset: 0x00008CAC
		[MethodImpl(256)]
		public static void RegularPolygonBorder(int sideCount)
		{
			Draw.RegularPolygon_Internal(sideCount, Draw.Radius, Draw.Thickness, Draw.Color, true, 0f, 0f);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000AACE File Offset: 0x00008CCE
		[MethodImpl(256)]
		public static void RegularPolygonBorder(int sideCount, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, Draw.Radius, Draw.Thickness, color, true, 0f, 0f);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000AAEC File Offset: 0x00008CEC
		[MethodImpl(256)]
		public static void RegularPolygonBorder(int sideCount, float radius)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, Draw.Color, true, 0f, 0f);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000AB0A File Offset: 0x00008D0A
		[MethodImpl(256)]
		public static void RegularPolygonBorder(int sideCount, float radius, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.Thickness, color, true, 0f, 0f);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000AB24 File Offset: 0x00008D24
		[MethodImpl(256)]
		public static void RegularPolygonBorder(int sideCount, float radius, float thickness)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000AB3E File Offset: 0x00008D3E
		[MethodImpl(256)]
		public static void RegularPolygonBorder(int sideCount, float radius, float thickness, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, 0f, 0f);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000AB54 File Offset: 0x00008D54
		[MethodImpl(256)]
		public static void RegularPolygonBorder(int sideCount, float radius, float thickness, float angle)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000AB6A File Offset: 0x00008D6A
		[MethodImpl(256)]
		public static void RegularPolygonBorder(int sideCount, float radius, float thickness, float angle, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, 0f, angle);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000AB7D File Offset: 0x00008D7D
		[MethodImpl(256)]
		public static void RegularPolygonBorder(int sideCount, float radius, float thickness, float angle, float roundness)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000AB90 File Offset: 0x00008D90
		[MethodImpl(256)]
		public static void RegularPolygonBorder(int sideCount, float radius, float thickness, float angle, float roundness, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, roundness, angle);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000ABA0 File Offset: 0x00008DA0
		[MethodImpl(256)]
		public static void Disc(Vector3 pos)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Disc_Internal(Draw.Radius, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000ABC6 File Offset: 0x00008DC6
		[MethodImpl(256)]
		public static void Disc(Vector3 pos, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Disc_Internal(Draw.Radius, colors);
			Draw.PopMatrix();
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000ABE3 File Offset: 0x00008DE3
		[MethodImpl(256)]
		public static void Disc(Vector3 pos, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Disc_Internal(radius, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000AC05 File Offset: 0x00008E05
		[MethodImpl(256)]
		public static void Disc(Vector3 pos, float radius, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Disc_Internal(radius, colors);
			Draw.PopMatrix();
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000AC1E File Offset: 0x00008E1E
		[MethodImpl(256)]
		public static void Disc(Vector3 pos, Vector3 normal)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Disc_Internal(Draw.Radius, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000AC5E File Offset: 0x00008E5E
		[MethodImpl(256)]
		public static void Disc(Vector3 pos, Vector3 normal, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Disc_Internal(Draw.Radius, colors);
			Draw.PopMatrix();
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000AC95 File Offset: 0x00008E95
		[MethodImpl(256)]
		public static void Disc(Vector3 pos, Vector3 normal, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Disc_Internal(radius, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000ACD1 File Offset: 0x00008ED1
		[MethodImpl(256)]
		public static void Disc(Vector3 pos, Vector3 normal, float radius, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Disc_Internal(radius, colors);
			Draw.PopMatrix();
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000AD04 File Offset: 0x00008F04
		[MethodImpl(256)]
		public static void Disc(Vector3 pos, Quaternion rot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Disc_Internal(Draw.Radius, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000AD3F File Offset: 0x00008F3F
		[MethodImpl(256)]
		public static void Disc(Vector3 pos, Quaternion rot, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Disc_Internal(Draw.Radius, colors);
			Draw.PopMatrix();
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000AD71 File Offset: 0x00008F71
		[MethodImpl(256)]
		public static void Disc(Vector3 pos, Quaternion rot, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Disc_Internal(radius, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000ADA8 File Offset: 0x00008FA8
		[MethodImpl(256)]
		public static void Disc(Vector3 pos, Quaternion rot, float radius, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Disc_Internal(radius, colors);
			Draw.PopMatrix();
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000ADD6 File Offset: 0x00008FD6
		[MethodImpl(256)]
		public static void Disc()
		{
			Draw.Disc_Internal(Draw.Radius, Draw.Color);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000ADEC File Offset: 0x00008FEC
		[MethodImpl(256)]
		public static void Disc(DiscColors colors)
		{
			Draw.Disc_Internal(Draw.Radius, colors);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000ADF9 File Offset: 0x00008FF9
		[MethodImpl(256)]
		public static void Disc(float radius)
		{
			Draw.Disc_Internal(radius, Draw.Color);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000AE0B File Offset: 0x0000900B
		[MethodImpl(256)]
		public static void Disc(float radius, DiscColors colors)
		{
			Draw.Disc_Internal(radius, colors);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000AE14 File Offset: 0x00009014
		[MethodImpl(256)]
		public static void Ring(Vector3 pos)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(Draw.Radius, Draw.Thickness, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000AE3F File Offset: 0x0000903F
		[MethodImpl(256)]
		public static void Ring(Vector3 pos, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(Draw.Radius, Draw.Thickness, colors);
			Draw.PopMatrix();
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000AE61 File Offset: 0x00009061
		[MethodImpl(256)]
		public static void Ring(Vector3 pos, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, Draw.Thickness, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000AE88 File Offset: 0x00009088
		[MethodImpl(256)]
		public static void Ring(Vector3 pos, float radius, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, Draw.Thickness, colors);
			Draw.PopMatrix();
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000AEA6 File Offset: 0x000090A6
		[MethodImpl(256)]
		public static void Ring(Vector3 pos, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, thickness, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000AEC9 File Offset: 0x000090C9
		[MethodImpl(256)]
		public static void Ring(Vector3 pos, float radius, float thickness, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, thickness, colors);
			Draw.PopMatrix();
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000AEE4 File Offset: 0x000090E4
		[MethodImpl(256)]
		public static void Ring(Vector3 pos, Vector3 normal)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(Draw.Radius, Draw.Thickness, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000AF34 File Offset: 0x00009134
		[MethodImpl(256)]
		public static void Ring(Vector3 pos, Vector3 normal, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(Draw.Radius, Draw.Thickness, colors);
			Draw.PopMatrix();
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000AF70 File Offset: 0x00009170
		[MethodImpl(256)]
		public static void Ring(Vector3 pos, Vector3 normal, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, Draw.Thickness, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000AFBC File Offset: 0x000091BC
		[MethodImpl(256)]
		public static void Ring(Vector3 pos, Vector3 normal, float radius, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, Draw.Thickness, colors);
			Draw.PopMatrix();
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000AFF4 File Offset: 0x000091F4
		[MethodImpl(256)]
		public static void Ring(Vector3 pos, Vector3 normal, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, thickness, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000B031 File Offset: 0x00009231
		[MethodImpl(256)]
		public static void Ring(Vector3 pos, Vector3 normal, float radius, float thickness, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, thickness, colors);
			Draw.PopMatrix();
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000B066 File Offset: 0x00009266
		[MethodImpl(256)]
		public static void Ring(Vector3 pos, Quaternion rot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(Draw.Radius, Draw.Thickness, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000B0A6 File Offset: 0x000092A6
		[MethodImpl(256)]
		public static void Ring(Vector3 pos, Quaternion rot, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(Draw.Radius, Draw.Thickness, colors);
			Draw.PopMatrix();
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000B0DD File Offset: 0x000092DD
		[MethodImpl(256)]
		public static void Ring(Vector3 pos, Quaternion rot, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, Draw.Thickness, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000B119 File Offset: 0x00009319
		[MethodImpl(256)]
		public static void Ring(Vector3 pos, Quaternion rot, float radius, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, Draw.Thickness, colors);
			Draw.PopMatrix();
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000B14C File Offset: 0x0000934C
		[MethodImpl(256)]
		public static void Ring(Vector3 pos, Quaternion rot, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, thickness, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000B184 File Offset: 0x00009384
		[MethodImpl(256)]
		public static void Ring(Vector3 pos, Quaternion rot, float radius, float thickness, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, thickness, colors);
			Draw.PopMatrix();
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000B1B4 File Offset: 0x000093B4
		[MethodImpl(256)]
		public static void Ring()
		{
			Draw.Ring_Internal(Draw.Radius, Draw.Thickness, Draw.Color);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000B1CF File Offset: 0x000093CF
		[MethodImpl(256)]
		public static void Ring(DiscColors colors)
		{
			Draw.Ring_Internal(Draw.Radius, Draw.Thickness, colors);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000B1E1 File Offset: 0x000093E1
		[MethodImpl(256)]
		public static void Ring(float radius)
		{
			Draw.Ring_Internal(radius, Draw.Thickness, Draw.Color);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000B1F8 File Offset: 0x000093F8
		[MethodImpl(256)]
		public static void Ring(float radius, DiscColors colors)
		{
			Draw.Ring_Internal(radius, Draw.Thickness, colors);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000B206 File Offset: 0x00009406
		[MethodImpl(256)]
		public static void Ring(float radius, float thickness)
		{
			Draw.Ring_Internal(radius, thickness, Draw.Color);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000B219 File Offset: 0x00009419
		[MethodImpl(256)]
		public static void Ring(float radius, float thickness, DiscColors colors)
		{
			Draw.Ring_Internal(radius, thickness, colors);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000B223 File Offset: 0x00009423
		[MethodImpl(256)]
		public static void Pie(Vector3 pos, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Pie_Internal(Draw.Radius, Draw.Color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000B24B File Offset: 0x0000944B
		[MethodImpl(256)]
		public static void Pie(Vector3 pos, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Pie_Internal(Draw.Radius, colors, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000B26A File Offset: 0x0000946A
		[MethodImpl(256)]
		public static void Pie(Vector3 pos, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Pie_Internal(radius, Draw.Color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000B28E File Offset: 0x0000948E
		[MethodImpl(256)]
		public static void Pie(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Pie_Internal(radius, colors, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000B2AC File Offset: 0x000094AC
		[MethodImpl(256)]
		public static void Pie(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Pie_Internal(Draw.Radius, Draw.Color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000B2F9 File Offset: 0x000094F9
		[MethodImpl(256)]
		public static void Pie(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Pie_Internal(Draw.Radius, colors, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000B333 File Offset: 0x00009533
		[MethodImpl(256)]
		public static void Pie(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Pie_Internal(radius, Draw.Color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000B372 File Offset: 0x00009572
		[MethodImpl(256)]
		public static void Pie(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Pie_Internal(radius, colors, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000B3A9 File Offset: 0x000095A9
		[MethodImpl(256)]
		public static void Pie(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Pie_Internal(Draw.Radius, Draw.Color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000B3E6 File Offset: 0x000095E6
		[MethodImpl(256)]
		public static void Pie(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Pie_Internal(Draw.Radius, colors, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000B41B File Offset: 0x0000961B
		[MethodImpl(256)]
		public static void Pie(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Pie_Internal(radius, Draw.Color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000B455 File Offset: 0x00009655
		[MethodImpl(256)]
		public static void Pie(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Pie_Internal(radius, colors, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000B487 File Offset: 0x00009687
		[MethodImpl(256)]
		public static void Pie(float angleRadStart, float angleRadEnd)
		{
			Draw.Pie_Internal(Draw.Radius, Draw.Color, angleRadStart, angleRadEnd);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000B49F File Offset: 0x0000969F
		[MethodImpl(256)]
		public static void Pie(float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.Pie_Internal(Draw.Radius, colors, angleRadStart, angleRadEnd);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000B4AE File Offset: 0x000096AE
		[MethodImpl(256)]
		public static void Pie(float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.Pie_Internal(radius, Draw.Color, angleRadStart, angleRadEnd);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000B4C2 File Offset: 0x000096C2
		[MethodImpl(256)]
		public static void Pie(float radius, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.Pie_Internal(radius, colors, angleRadStart, angleRadEnd);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000B4CD File Offset: 0x000096CD
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.Radius, Draw.Thickness, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000B4FB File Offset: 0x000096FB
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.Radius, Draw.Thickness, colors, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000B520 File Offset: 0x00009720
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.Radius, Draw.Thickness, Draw.Color, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000B54E File Offset: 0x0000974E
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.Radius, Draw.Thickness, colors, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000B574 File Offset: 0x00009774
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.Thickness, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000B59E File Offset: 0x0000979E
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.Thickness, colors, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000B5C0 File Offset: 0x000097C0
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.Thickness, Draw.Color, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000B5EB File Offset: 0x000097EB
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.Thickness, colors, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000B60E File Offset: 0x0000980E
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000B635 File Offset: 0x00009835
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colors, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000B654 File Offset: 0x00009854
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, Draw.Color, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000B67C File Offset: 0x0000987C
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colors, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000B69C File Offset: 0x0000989C
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.Radius, Draw.Thickness, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000B6EF File Offset: 0x000098EF
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.Radius, Draw.Thickness, colors, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000B730 File Offset: 0x00009930
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.Radius, Draw.Thickness, Draw.Color, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000B784 File Offset: 0x00009984
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.Radius, Draw.Thickness, colors, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000B7D0 File Offset: 0x000099D0
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.Thickness, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000B820 File Offset: 0x00009A20
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.Thickness, colors, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000B860 File Offset: 0x00009A60
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.Thickness, Draw.Color, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000B8B1 File Offset: 0x00009AB1
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.Thickness, colors, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000B8F0 File Offset: 0x00009AF0
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000B93D File Offset: 0x00009B3D
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colors, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000B978 File Offset: 0x00009B78
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, Draw.Color, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000B9C6 File Offset: 0x00009BC6
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colors, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000BA04 File Offset: 0x00009C04
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.Radius, Draw.Thickness, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000BA52 File Offset: 0x00009C52
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.Radius, Draw.Thickness, colors, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000BA90 File Offset: 0x00009C90
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.Radius, Draw.Thickness, Draw.Color, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000BADF File Offset: 0x00009CDF
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.Radius, Draw.Thickness, colors, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000BB1B File Offset: 0x00009D1B
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.Thickness, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000BB5B File Offset: 0x00009D5B
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.Thickness, colors, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000BB94 File Offset: 0x00009D94
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.Thickness, Draw.Color, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000BBE0 File Offset: 0x00009DE0
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.Thickness, colors, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000BC19 File Offset: 0x00009E19
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000BC56 File Offset: 0x00009E56
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colors, angleRadStart, angleRadEnd, ArcEndCap.None);
			Draw.PopMatrix();
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000BC8B File Offset: 0x00009E8B
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, Draw.Color, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000BCC9 File Offset: 0x00009EC9
		[MethodImpl(256)]
		public static void Arc(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, DiscColors colors)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colors, angleRadStart, angleRadEnd, endCaps);
			Draw.PopMatrix();
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000BCFF File Offset: 0x00009EFF
		[MethodImpl(256)]
		public static void Arc(float angleRadStart, float angleRadEnd)
		{
			Draw.Arc_Internal(Draw.Radius, Draw.Thickness, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000BD1D File Offset: 0x00009F1D
		[MethodImpl(256)]
		public static void Arc(float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.Arc_Internal(Draw.Radius, Draw.Thickness, colors, angleRadStart, angleRadEnd, ArcEndCap.None);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000BD32 File Offset: 0x00009F32
		[MethodImpl(256)]
		public static void Arc(float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.Arc_Internal(Draw.Radius, Draw.Thickness, Draw.Color, angleRadStart, angleRadEnd, endCaps);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000BD50 File Offset: 0x00009F50
		[MethodImpl(256)]
		public static void Arc(float angleRadStart, float angleRadEnd, ArcEndCap endCaps, DiscColors colors)
		{
			Draw.Arc_Internal(Draw.Radius, Draw.Thickness, colors, angleRadStart, angleRadEnd, endCaps);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000BD65 File Offset: 0x00009F65
		[MethodImpl(256)]
		public static void Arc(float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.Arc_Internal(radius, Draw.Thickness, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000BD7F File Offset: 0x00009F7F
		[MethodImpl(256)]
		public static void Arc(float radius, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.Arc_Internal(radius, Draw.Thickness, colors, angleRadStart, angleRadEnd, ArcEndCap.None);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000BD90 File Offset: 0x00009F90
		[MethodImpl(256)]
		public static void Arc(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.Arc_Internal(radius, Draw.Thickness, Draw.Color, angleRadStart, angleRadEnd, endCaps);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000BDAA File Offset: 0x00009FAA
		[MethodImpl(256)]
		public static void Arc(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, DiscColors colors)
		{
			Draw.Arc_Internal(radius, Draw.Thickness, colors, angleRadStart, angleRadEnd, endCaps);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000BDBC File Offset: 0x00009FBC
		[MethodImpl(256)]
		public static void Arc(float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.Arc_Internal(radius, thickness, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000BDD2 File Offset: 0x00009FD2
		[MethodImpl(256)]
		public static void Arc(float radius, float thickness, float angleRadStart, float angleRadEnd, DiscColors colors)
		{
			Draw.Arc_Internal(radius, thickness, colors, angleRadStart, angleRadEnd, ArcEndCap.None);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000BDE0 File Offset: 0x00009FE0
		[MethodImpl(256)]
		public static void Arc(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.Arc_Internal(radius, thickness, Draw.Color, angleRadStart, angleRadEnd, endCaps);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000BDF7 File Offset: 0x00009FF7
		[MethodImpl(256)]
		public static void Arc(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, DiscColors colors)
		{
			Draw.Arc_Internal(radius, thickness, colors, angleRadStart, angleRadEnd, endCaps);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000BE08 File Offset: 0x0000A008
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Rect rect)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000BE44 File Offset: 0x0000A044
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Rect rect, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000BE7C File Offset: 0x0000A07C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Rect rect, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000BEB8 File Offset: 0x0000A0B8
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Rect rect, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000BEF0 File Offset: 0x0000A0F0
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Rect rect, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000BF19 File Offset: 0x0000A119
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Rect rect, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000BF40 File Offset: 0x0000A140
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Rect rect)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000BF98 File Offset: 0x0000A198
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Rect rect, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000BFEC File Offset: 0x0000A1EC
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Rect rect, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000C044 File Offset: 0x0000A244
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Rect rect, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000C098 File Offset: 0x0000A298
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Rect rect, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000C0E6 File Offset: 0x0000A2E6
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Rect rect, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000C128 File Offset: 0x0000A328
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Rect rect)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000C17C File Offset: 0x0000A37C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Rect rect, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000C1CC File Offset: 0x0000A3CC
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Rect rect, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000C220 File Offset: 0x0000A420
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Rect rect, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000C26E File Offset: 0x0000A46E
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Rect rect, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000C2AC File Offset: 0x0000A4AC
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Rect rect, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000C2E8 File Offset: 0x0000A4E8
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector2 size)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000C32C File Offset: 0x0000A52C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector2 size, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000C36C File Offset: 0x0000A56C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector2 size, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000C3B0 File Offset: 0x0000A5B0
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector2 size, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000C3EE File Offset: 0x0000A5EE
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector2 size, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000C41D File Offset: 0x0000A61D
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector2 size, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000C448 File Offset: 0x0000A648
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, float width, float height)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000C48C File Offset: 0x0000A68C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, float width, float height, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000C4CC File Offset: 0x0000A6CC
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, float width, float height, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000C510 File Offset: 0x0000A710
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, float width, float height, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000C550 File Offset: 0x0000A750
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, float width, float height, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000C580 File Offset: 0x0000A780
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, float width, float height, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000C5B0 File Offset: 0x0000A7B0
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000C60C File Offset: 0x0000A80C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000C664 File Offset: 0x0000A864
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000C6C0 File Offset: 0x0000A8C0
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000C71C File Offset: 0x0000A91C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000C770 File Offset: 0x0000A970
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000C7C4 File Offset: 0x0000A9C4
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000C824 File Offset: 0x0000AA24
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000C880 File Offset: 0x0000AA80
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000C8E4 File Offset: 0x0000AAE4
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000C944 File Offset: 0x0000AB44
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000C99C File Offset: 0x0000AB9C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000C9F0 File Offset: 0x0000ABF0
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000CA48 File Offset: 0x0000AC48
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000CA9C File Offset: 0x0000AC9C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000CAF4 File Offset: 0x0000ACF4
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000CB48 File Offset: 0x0000AD48
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000CB98 File Offset: 0x0000AD98
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000CBE4 File Offset: 0x0000ADE4
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000CC3C File Offset: 0x0000AE3C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000CC94 File Offset: 0x0000AE94
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000CCF0 File Offset: 0x0000AEF0
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000CD4C File Offset: 0x0000AF4C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000CDA0 File Offset: 0x0000AFA0
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000CDF0 File Offset: 0x0000AFF0
		[MethodImpl(256)]
		public static void Rectangle(Rect rect)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.Thickness, default(Vector4));
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000CE1C File Offset: 0x0000B01C
		[MethodImpl(256)]
		public static void Rectangle(Rect rect, Color color)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.Thickness, default(Vector4));
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000CE44 File Offset: 0x0000B044
		[MethodImpl(256)]
		public static void Rectangle(Rect rect, float cornerRadius)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000CE70 File Offset: 0x0000B070
		[MethodImpl(256)]
		public static void Rectangle(Rect rect, float cornerRadius, Color color)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000CE98 File Offset: 0x0000B098
		[MethodImpl(256)]
		public static void Rectangle(Rect rect, Vector4 cornerRadii)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.Thickness, cornerRadii);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000CEB1 File Offset: 0x0000B0B1
		[MethodImpl(256)]
		public static void Rectangle(Rect rect, Vector4 cornerRadii, Color color)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.Thickness, cornerRadii);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000CEC8 File Offset: 0x0000B0C8
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector2 size, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000CF0C File Offset: 0x0000B10C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector2 size, RectPivot pivot, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000CF4C File Offset: 0x0000B14C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector2 size, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000CF90 File Offset: 0x0000B190
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector2 size, RectPivot pivot, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000CFCF File Offset: 0x0000B1CF
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector2 size, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000CFFE File Offset: 0x0000B1FE
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector2 size, RectPivot pivot, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000D02C File Offset: 0x0000B22C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, float width, float height, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000D070 File Offset: 0x0000B270
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, float width, float height, RectPivot pivot, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000D0B0 File Offset: 0x0000B2B0
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, float width, float height, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000D0F8 File Offset: 0x0000B2F8
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, float width, float height, RectPivot pivot, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000D13C File Offset: 0x0000B33C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, float width, float height, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000D16D File Offset: 0x0000B36D
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, float width, float height, RectPivot pivot, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000D19C File Offset: 0x0000B39C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000D1F8 File Offset: 0x0000B3F8
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000D254 File Offset: 0x0000B454
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000D2B4 File Offset: 0x0000B4B4
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000D314 File Offset: 0x0000B514
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000D36C File Offset: 0x0000B56C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000D3C0 File Offset: 0x0000B5C0
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000D420 File Offset: 0x0000B620
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000D47C File Offset: 0x0000B67C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000D4E0 File Offset: 0x0000B6E0
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0000D540 File Offset: 0x0000B740
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000D598 File Offset: 0x0000B798
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000D5EC File Offset: 0x0000B7EC
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000D644 File Offset: 0x0000B844
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000D698 File Offset: 0x0000B898
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000D6F4 File Offset: 0x0000B8F4
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000D74C File Offset: 0x0000B94C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000D79C File Offset: 0x0000B99C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000D7EC File Offset: 0x0000B9EC
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000D848 File Offset: 0x0000BA48
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.Thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000D8A0 File Offset: 0x0000BAA0
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000D900 File Offset: 0x0000BB00
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.Thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000D95C File Offset: 0x0000BB5C
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000D9B0 File Offset: 0x0000BBB0
		[MethodImpl(256)]
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.Thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000DA00 File Offset: 0x0000BC00
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Rect rect, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000DA38 File Offset: 0x0000BC38
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Rect rect, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000DA6C File Offset: 0x0000BC6C
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Rect rect, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000DAA4 File Offset: 0x0000BCA4
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Rect rect, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000DAD9 File Offset: 0x0000BCD9
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Rect rect, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000DAFE File Offset: 0x0000BCFE
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Rect rect, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000DB20 File Offset: 0x0000BD20
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Rect rect, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000DB74 File Offset: 0x0000BD74
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Rect rect, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000DBC4 File Offset: 0x0000BDC4
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Rect rect, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000DC1C File Offset: 0x0000BE1C
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Rect rect, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000DC6F File Offset: 0x0000BE6F
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Rect rect, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000DCAF File Offset: 0x0000BEAF
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Rect rect, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000DCEC File Offset: 0x0000BEEC
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Rect rect, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000DD3C File Offset: 0x0000BF3C
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Rect rect, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000DD88 File Offset: 0x0000BF88
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Rect rect, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000DDDC File Offset: 0x0000BFDC
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Rect rect, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000DE2A File Offset: 0x0000C02A
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Rect rect, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000DE65 File Offset: 0x0000C065
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Rect rect, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000DEA0 File Offset: 0x0000C0A0
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector2 size, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000DEE0 File Offset: 0x0000C0E0
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector2 size, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000DF1C File Offset: 0x0000C11C
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector2 size, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000DF5C File Offset: 0x0000C15C
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector2 size, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000DF97 File Offset: 0x0000C197
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector2 size, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000DFC2 File Offset: 0x0000C1C2
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector2 size, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000DFEC File Offset: 0x0000C1EC
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, float width, float height, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000E02C File Offset: 0x0000C22C
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, float width, float height, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000E068 File Offset: 0x0000C268
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, float width, float height, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000E0AC File Offset: 0x0000C2AC
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, float width, float height, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0000E0EC File Offset: 0x0000C2EC
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, float width, float height, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000E119 File Offset: 0x0000C319
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, float width, float height, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0000E144 File Offset: 0x0000C344
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000E19C File Offset: 0x0000C39C
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000E1F4 File Offset: 0x0000C3F4
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000E250 File Offset: 0x0000C450
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000E2AC File Offset: 0x0000C4AC
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000E300 File Offset: 0x0000C500
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0000E350 File Offset: 0x0000C550
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000E3AC File Offset: 0x0000C5AC
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000E404 File Offset: 0x0000C604
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000E464 File Offset: 0x0000C664
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000E4C0 File Offset: 0x0000C6C0
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0000E514 File Offset: 0x0000C714
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0000E564 File Offset: 0x0000C764
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000E5B8 File Offset: 0x0000C7B8
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000E608 File Offset: 0x0000C808
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000E660 File Offset: 0x0000C860
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0000E6B4 File Offset: 0x0000C8B4
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000E700 File Offset: 0x0000C900
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0000E740 File Offset: 0x0000C940
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000E798 File Offset: 0x0000C998
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0000E7EC File Offset: 0x0000C9EC
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0000E848 File Offset: 0x0000CA48
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0000E8A0 File Offset: 0x0000CAA0
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0000E8EE File Offset: 0x0000CAEE
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0000E930 File Offset: 0x0000CB30
		[MethodImpl(256)]
		public static void RectangleBorder(Rect rect, float thickness)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, default(Vector4));
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000E958 File Offset: 0x0000CB58
		[MethodImpl(256)]
		public static void RectangleBorder(Rect rect, float thickness, Color color)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, default(Vector4));
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000E97C File Offset: 0x0000CB7C
		[MethodImpl(256)]
		public static void RectangleBorder(Rect rect, float thickness, float cornerRadius)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0000E9A4 File Offset: 0x0000CBA4
		[MethodImpl(256)]
		public static void RectangleBorder(Rect rect, float thickness, float cornerRadius, Color color)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0000E9C8 File Offset: 0x0000CBC8
		[MethodImpl(256)]
		public static void RectangleBorder(Rect rect, float thickness, Vector4 cornerRadii)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, cornerRadii);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000E9DD File Offset: 0x0000CBDD
		[MethodImpl(256)]
		public static void RectangleBorder(Rect rect, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, cornerRadii);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0000E9F0 File Offset: 0x0000CBF0
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector2 size, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0000EA30 File Offset: 0x0000CC30
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0000EA6C File Offset: 0x0000CC6C
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0000EAB0 File Offset: 0x0000CCB0
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0000EAEF File Offset: 0x0000CCEF
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000EB1B File Offset: 0x0000CD1B
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0000EB44 File Offset: 0x0000CD44
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, float width, float height, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0000EB84 File Offset: 0x0000CD84
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, float width, float height, RectPivot pivot, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000EBC4 File Offset: 0x0000CDC4
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, float width, float height, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000EC08 File Offset: 0x0000CE08
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, float width, float height, RectPivot pivot, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000EC49 File Offset: 0x0000CE49
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0000EC77 File Offset: 0x0000CE77
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0000ECA4 File Offset: 0x0000CEA4
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000ED00 File Offset: 0x0000CF00
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0000ED58 File Offset: 0x0000CF58
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000EDB8 File Offset: 0x0000CFB8
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000EE14 File Offset: 0x0000D014
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000EE68 File Offset: 0x0000D068
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000EEB8 File Offset: 0x0000D0B8
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0000EF14 File Offset: 0x0000D114
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0000EF6C File Offset: 0x0000D16C
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000EFCC File Offset: 0x0000D1CC
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0000F028 File Offset: 0x0000D228
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0000F07C File Offset: 0x0000D27C
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0000F0D0 File Offset: 0x0000D2D0
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0000F124 File Offset: 0x0000D324
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000F178 File Offset: 0x0000D378
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0000F1D0 File Offset: 0x0000D3D0
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0000F228 File Offset: 0x0000D428
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0000F275 File Offset: 0x0000D475
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0000F2B4 File Offset: 0x0000D4B4
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0000F30C File Offset: 0x0000D50C
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, default(Vector4));
			Draw.PopMatrix();
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0000F360 File Offset: 0x0000D560
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0000F3BC File Offset: 0x0000D5BC
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius));
			Draw.PopMatrix();
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0000F414 File Offset: 0x0000D614
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0000F464 File Offset: 0x0000D664
		[MethodImpl(256)]
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, cornerRadii);
			Draw.PopMatrix();
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0000F4B0 File Offset: 0x0000D6B0
		[MethodImpl(256)]
		public static void Triangle(Vector3 a, Vector3 b, Vector3 c)
		{
			Draw.Triangle_Internal(a, b, c, false, Draw.Thickness, 0f, Draw.Color, Draw.Color, Draw.Color);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0000F4E0 File Offset: 0x0000D6E0
		[MethodImpl(256)]
		public static void Triangle(Vector3 a, Vector3 b, Vector3 c, Color color)
		{
			Draw.Triangle_Internal(a, b, c, false, Draw.Thickness, 0f, color, color, color);
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000F504 File Offset: 0x0000D704
		[MethodImpl(256)]
		public static void Triangle(Vector3 a, Vector3 b, Vector3 c, Color colorA, Color colorB, Color colorC)
		{
			Draw.Triangle_Internal(a, b, c, false, Draw.Thickness, 0f, colorA, colorB, colorC);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0000F52C File Offset: 0x0000D72C
		[MethodImpl(256)]
		public static void Triangle(Vector3 a, Vector3 b, Vector3 c, float roundness)
		{
			Draw.Triangle_Internal(a, b, c, false, Draw.Thickness, roundness, Draw.Color, Draw.Color, Draw.Color);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000F558 File Offset: 0x0000D758
		[MethodImpl(256)]
		public static void Triangle(Vector3 a, Vector3 b, Vector3 c, float roundness, Color color)
		{
			Draw.Triangle_Internal(a, b, c, false, Draw.Thickness, roundness, color, color, color);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000F57C File Offset: 0x0000D77C
		[MethodImpl(256)]
		public static void Triangle(Vector3 a, Vector3 b, Vector3 c, float roundness, Color colorA, Color colorB, Color colorC)
		{
			Draw.Triangle_Internal(a, b, c, false, Draw.Thickness, roundness, colorA, colorB, colorC);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0000F5A0 File Offset: 0x0000D7A0
		[MethodImpl(256)]
		public static void TriangleBorder(Vector3 a, Vector3 b, Vector3 c)
		{
			Draw.Triangle_Internal(a, b, c, true, Draw.Thickness, 0f, Draw.Color, Draw.Color, Draw.Color);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0000F5D0 File Offset: 0x0000D7D0
		[MethodImpl(256)]
		public static void TriangleBorder(Vector3 a, Vector3 b, Vector3 c, Color color)
		{
			Draw.Triangle_Internal(a, b, c, true, Draw.Thickness, 0f, color, color, color);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0000F5F4 File Offset: 0x0000D7F4
		[MethodImpl(256)]
		public static void TriangleBorder(Vector3 a, Vector3 b, Vector3 c, Color colorA, Color colorB, Color colorC)
		{
			Draw.Triangle_Internal(a, b, c, true, Draw.Thickness, 0f, colorA, colorB, colorC);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0000F61C File Offset: 0x0000D81C
		[MethodImpl(256)]
		public static void TriangleBorder(Vector3 a, Vector3 b, Vector3 c, float thickness)
		{
			Draw.Triangle_Internal(a, b, c, true, thickness, 0f, Draw.Color, Draw.Color, Draw.Color);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0000F648 File Offset: 0x0000D848
		[MethodImpl(256)]
		public static void TriangleBorder(Vector3 a, Vector3 b, Vector3 c, float thickness, Color color)
		{
			Draw.Triangle_Internal(a, b, c, true, thickness, 0f, color, color, color);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0000F66C File Offset: 0x0000D86C
		[MethodImpl(256)]
		public static void TriangleBorder(Vector3 a, Vector3 b, Vector3 c, float thickness, Color colorA, Color colorB, Color colorC)
		{
			Draw.Triangle_Internal(a, b, c, true, thickness, 0f, colorA, colorB, colorC);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0000F690 File Offset: 0x0000D890
		[MethodImpl(256)]
		public static void TriangleBorder(Vector3 a, Vector3 b, Vector3 c, float thickness, float roundness)
		{
			Draw.Triangle_Internal(a, b, c, true, thickness, roundness, Draw.Color, Draw.Color, Draw.Color);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0000F6B8 File Offset: 0x0000D8B8
		[MethodImpl(256)]
		public static void TriangleBorder(Vector3 a, Vector3 b, Vector3 c, float thickness, float roundness, Color color)
		{
			Draw.Triangle_Internal(a, b, c, true, thickness, roundness, color, color, color);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0000F6D8 File Offset: 0x0000D8D8
		[MethodImpl(256)]
		public static void TriangleBorder(Vector3 a, Vector3 b, Vector3 c, float thickness, float roundness, Color colorA, Color colorB, Color colorC)
		{
			Draw.Triangle_Internal(a, b, c, true, thickness, roundness, colorA, colorB, colorC);
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0000F6F7 File Offset: 0x0000D8F7
		[MethodImpl(256)]
		public static void Quad(Vector3 a, Vector3 b, Vector3 c)
		{
			Draw.Quad_Internal(a, b, c, a + (c - b), Draw.Color, Draw.Color, Draw.Color, Draw.Color);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0000F722 File Offset: 0x0000D922
		[MethodImpl(256)]
		public static void Quad(Vector3 a, Vector3 b, Vector3 c, Color color)
		{
			Draw.Quad_Internal(a, b, c, a + (c - b), color, color, color, color);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000F73D File Offset: 0x0000D93D
		[MethodImpl(256)]
		public static void Quad(Vector3 a, Vector3 b, Vector3 c, Color colorA, Color colorB, Color colorC, Color colorD)
		{
			Draw.Quad_Internal(a, b, c, a + (c - b), colorA, colorB, colorC, colorD);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0000F75B File Offset: 0x0000D95B
		[MethodImpl(256)]
		public static void Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
		{
			Draw.Quad_Internal(a, b, c, d, Draw.Color, Draw.Color, Draw.Color, Draw.Color);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0000F77A File Offset: 0x0000D97A
		[MethodImpl(256)]
		public static void Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Color color)
		{
			Draw.Quad_Internal(a, b, c, d, color, color, color, color);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0000F78D File Offset: 0x0000D98D
		[MethodImpl(256)]
		public static void Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Color colorA, Color colorB, Color colorC, Color colorD)
		{
			Draw.Quad_Internal(a, b, c, d, colorA, colorB, colorC, colorD);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0000F7A0 File Offset: 0x0000D9A0
		[MethodImpl(256)]
		public static void Sphere(Vector3 pos)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Sphere_Internal(Draw.Radius, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0000F7C1 File Offset: 0x0000D9C1
		[MethodImpl(256)]
		public static void Sphere(Vector3 pos, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Sphere_Internal(radius, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0000F7DE File Offset: 0x0000D9DE
		[MethodImpl(256)]
		public static void Sphere(Vector3 pos, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Sphere_Internal(Draw.Radius, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0000F7FB File Offset: 0x0000D9FB
		[MethodImpl(256)]
		public static void Sphere(Vector3 pos, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Sphere_Internal(radius, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0000F814 File Offset: 0x0000DA14
		[MethodImpl(256)]
		public static void Sphere()
		{
			Draw.Sphere_Internal(Draw.Radius, Draw.Color);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0000F825 File Offset: 0x0000DA25
		[MethodImpl(256)]
		public static void Sphere(float radius)
		{
			Draw.Sphere_Internal(radius, Draw.Color);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000F832 File Offset: 0x0000DA32
		[MethodImpl(256)]
		public static void Sphere(Color color)
		{
			Draw.Sphere_Internal(Draw.Radius, color);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0000F83F File Offset: 0x0000DA3F
		[MethodImpl(256)]
		public static void Sphere(float radius, Color color)
		{
			Draw.Sphere_Internal(radius, color);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0000F848 File Offset: 0x0000DA48
		[MethodImpl(256)]
		public static void Cuboid(Vector3 pos, Vector3 size)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Cuboid_Internal(size, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0000F865 File Offset: 0x0000DA65
		[MethodImpl(256)]
		public static void Cuboid(Vector3 pos, Vector3 size, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Cuboid_Internal(size, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0000F87E File Offset: 0x0000DA7E
		[MethodImpl(256)]
		public static void Cuboid(Vector3 pos, Vector3 normal, Vector3 size)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Cuboid_Internal(size, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0000F8B5 File Offset: 0x0000DAB5
		[MethodImpl(256)]
		public static void Cuboid(Vector3 pos, Vector3 normal, Vector3 size, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Cuboid_Internal(size, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0000F8E8 File Offset: 0x0000DAE8
		[MethodImpl(256)]
		public static void Cuboid(Vector3 pos, Quaternion rot, Vector3 size)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Cuboid_Internal(size, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0000F91A File Offset: 0x0000DB1A
		[MethodImpl(256)]
		public static void Cuboid(Vector3 pos, Quaternion rot, Vector3 size, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Cuboid_Internal(size, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0000F948 File Offset: 0x0000DB48
		[MethodImpl(256)]
		public static void Cuboid(Vector3 size)
		{
			Draw.Cuboid_Internal(size, Draw.Color);
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0000F955 File Offset: 0x0000DB55
		[MethodImpl(256)]
		public static void Cuboid(Vector3 size, Color color)
		{
			Draw.Cuboid_Internal(size, color);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0000F95E File Offset: 0x0000DB5E
		[MethodImpl(256)]
		public static void Cube(Vector3 pos, float size)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Cuboid_Internal(new Vector3(size, size, size), Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0000F982 File Offset: 0x0000DB82
		[MethodImpl(256)]
		public static void Cube(Vector3 pos, float size, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Cuboid_Internal(new Vector3(size, size, size), color);
			Draw.PopMatrix();
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0000F9A2 File Offset: 0x0000DBA2
		[MethodImpl(256)]
		public static void Cube(Vector3 pos, Vector3 normal, float size)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Cuboid_Internal(new Vector3(size, size, size), Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0000F9E0 File Offset: 0x0000DBE0
		[MethodImpl(256)]
		public static void Cube(Vector3 pos, Vector3 normal, float size, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Cuboid_Internal(new Vector3(size, size, size), color);
			Draw.PopMatrix();
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0000FA1A File Offset: 0x0000DC1A
		[MethodImpl(256)]
		public static void Cube(Vector3 pos, Quaternion rot, float size)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Cuboid_Internal(new Vector3(size, size, size), Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0000FA53 File Offset: 0x0000DC53
		[MethodImpl(256)]
		public static void Cube(Vector3 pos, Quaternion rot, float size, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Cuboid_Internal(new Vector3(size, size, size), color);
			Draw.PopMatrix();
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0000FA88 File Offset: 0x0000DC88
		[MethodImpl(256)]
		public static void Cube(float size)
		{
			Draw.Cuboid_Internal(new Vector3(size, size, size), Draw.Color);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0000FA9C File Offset: 0x0000DC9C
		[MethodImpl(256)]
		public static void Cube(float size, Color color)
		{
			Draw.Cuboid_Internal(new Vector3(size, size, size), color);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0000FAAC File Offset: 0x0000DCAC
		[MethodImpl(256)]
		public static void Cone(Vector3 pos, float radius, float length)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Cone_Internal(radius, length, true, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0000FACB File Offset: 0x0000DCCB
		[MethodImpl(256)]
		public static void Cone(Vector3 pos, float radius, float length, bool fillCap)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Cone_Internal(radius, length, fillCap, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0000FAEA File Offset: 0x0000DCEA
		[MethodImpl(256)]
		public static void Cone(Vector3 pos, float radius, float length, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Cone_Internal(radius, length, true, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0000FB05 File Offset: 0x0000DD05
		[MethodImpl(256)]
		public static void Cone(Vector3 pos, float radius, float length, bool fillCap, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Cone_Internal(radius, length, fillCap, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0000FB21 File Offset: 0x0000DD21
		[MethodImpl(256)]
		public static void Cone(Vector3 pos, Vector3 normal, float radius, float length)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Cone_Internal(radius, length, true, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0000FB5A File Offset: 0x0000DD5A
		[MethodImpl(256)]
		public static void Cone(Vector3 pos, Vector3 normal, float radius, float length, bool fillCap)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Cone_Internal(radius, length, fillCap, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0000FB94 File Offset: 0x0000DD94
		[MethodImpl(256)]
		public static void Cone(Vector3 pos, Vector3 normal, float radius, float length, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Cone_Internal(radius, length, true, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0000FBCA File Offset: 0x0000DDCA
		[MethodImpl(256)]
		public static void Cone(Vector3 pos, Vector3 normal, float radius, float length, bool fillCap, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Cone_Internal(radius, length, fillCap, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0000FC01 File Offset: 0x0000DE01
		[MethodImpl(256)]
		public static void Cone(Vector3 pos, Quaternion rot, float radius, float length)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Cone_Internal(radius, length, true, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0000FC35 File Offset: 0x0000DE35
		[MethodImpl(256)]
		public static void Cone(Vector3 pos, Quaternion rot, float radius, float length, bool fillCap)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Cone_Internal(radius, length, fillCap, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0000FC6A File Offset: 0x0000DE6A
		[MethodImpl(256)]
		public static void Cone(Vector3 pos, Quaternion rot, float radius, float length, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Cone_Internal(radius, length, true, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0000FC9B File Offset: 0x0000DE9B
		[MethodImpl(256)]
		public static void Cone(Vector3 pos, Quaternion rot, float radius, float length, bool fillCap, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Cone_Internal(radius, length, fillCap, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0000FCCD File Offset: 0x0000DECD
		[MethodImpl(256)]
		public static void Cone(float radius, float length)
		{
			Draw.Cone_Internal(radius, length, true, Draw.Color);
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0000FCDC File Offset: 0x0000DEDC
		[MethodImpl(256)]
		public static void Cone(float radius, float length, bool fillCap)
		{
			Draw.Cone_Internal(radius, length, fillCap, Draw.Color);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0000FCEB File Offset: 0x0000DEEB
		[MethodImpl(256)]
		public static void Cone(float radius, float length, Color color)
		{
			Draw.Cone_Internal(radius, length, true, color);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0000FCF6 File Offset: 0x0000DEF6
		[MethodImpl(256)]
		public static void Cone(float radius, float length, bool fillCap, Color color)
		{
			Draw.Cone_Internal(radius, length, fillCap, color);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0000FD01 File Offset: 0x0000DF01
		[MethodImpl(256)]
		public static void Torus(Vector3 pos, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Torus_Internal(radius, thickness, 0f, 6.2831855f, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0000FD29 File Offset: 0x0000DF29
		[MethodImpl(256)]
		public static void Torus(Vector3 pos, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Torus_Internal(radius, thickness, 0f, 6.2831855f, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0000FD50 File Offset: 0x0000DF50
		[MethodImpl(256)]
		public static void Torus(Vector3 pos, Vector3 normal, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Torus_Internal(radius, thickness, 0f, 6.2831855f, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0000FD9D File Offset: 0x0000DF9D
		[MethodImpl(256)]
		public static void Torus(Vector3 pos, Vector3 normal, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Torus_Internal(radius, thickness, 0f, 6.2831855f, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0000FDDC File Offset: 0x0000DFDC
		[MethodImpl(256)]
		public static void Torus(Vector3 pos, Quaternion rot, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Torus_Internal(radius, thickness, 0f, 6.2831855f, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0000FE19 File Offset: 0x0000E019
		[MethodImpl(256)]
		public static void Torus(Vector3 pos, Quaternion rot, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Torus_Internal(radius, thickness, 0f, 6.2831855f, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0000FE53 File Offset: 0x0000E053
		[MethodImpl(256)]
		public static void Torus(float radius, float thickness)
		{
			Draw.Torus_Internal(radius, thickness, 0f, 6.2831855f, Draw.Color);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0000FE6B File Offset: 0x0000E06B
		[MethodImpl(256)]
		public static void Torus(float radius, float thickness, Color color)
		{
			Draw.Torus_Internal(radius, thickness, 0f, 6.2831855f, color);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0000FE7F File Offset: 0x0000E07F
		[MethodImpl(256)]
		public static void Torus(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Torus_Internal(radius, thickness, angleRadStart, angleRadEnd, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0000FEA0 File Offset: 0x0000E0A0
		[MethodImpl(256)]
		public static void Torus(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Torus_Internal(radius, thickness, angleRadStart, angleRadEnd, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0000FEBE File Offset: 0x0000E0BE
		[MethodImpl(256)]
		public static void Torus(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Torus_Internal(radius, thickness, angleRadStart, angleRadEnd, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0000FEFA File Offset: 0x0000E0FA
		[MethodImpl(256)]
		public static void Torus(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Torus_Internal(radius, thickness, angleRadStart, angleRadEnd, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0000FF33 File Offset: 0x0000E133
		[MethodImpl(256)]
		public static void Torus(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Torus_Internal(radius, thickness, angleRadStart, angleRadEnd, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0000FF6A File Offset: 0x0000E16A
		[MethodImpl(256)]
		public static void Torus(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Torus_Internal(radius, thickness, angleRadStart, angleRadEnd, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0000FF9E File Offset: 0x0000E19E
		[MethodImpl(256)]
		public static void Torus(float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.Torus_Internal(radius, thickness, angleRadStart, angleRadEnd, Draw.Color);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0000FFAE File Offset: 0x0000E1AE
		[MethodImpl(256)]
		public static void Torus(float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.Torus_Internal(radius, thickness, angleRadStart, angleRadEnd, color);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0000FFBC File Offset: 0x0000E1BC
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, string content)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00010008 File Offset: 0x0000E208
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, string content, TextAlign align)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00010050 File Offset: 0x0000E250
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, string content, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00010098 File Offset: 0x0000E298
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, string content, TextAlign align, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x000100DC File Offset: 0x0000E2DC
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, string content, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00010124 File Offset: 0x0000E324
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00010168 File Offset: 0x0000E368
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, string content, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x000101AC File Offset: 0x0000E3AC
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x000101F0 File Offset: 0x0000E3F0
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, string content, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00010238 File Offset: 0x0000E438
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, string content, TextAlign align, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001027C File Offset: 0x0000E47C
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, string content, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x000102C0 File Offset: 0x0000E4C0
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, string content, TextAlign align, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00010304 File Offset: 0x0000E504
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, string content, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00010348 File Offset: 0x0000E548
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001038C File Offset: 0x0000E58C
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x000103D0 File Offset: 0x0000E5D0
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00010410 File Offset: 0x0000E610
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, Quaternion rot, string content)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00010470 File Offset: 0x0000E670
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, Quaternion rot, string content, TextAlign align)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000104D0 File Offset: 0x0000E6D0
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, Quaternion rot, string content, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00010530 File Offset: 0x0000E730
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, Quaternion rot, string content, TextAlign align, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001058C File Offset: 0x0000E78C
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, Quaternion rot, string content, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x000105EC File Offset: 0x0000E7EC
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, Quaternion rot, string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00010648 File Offset: 0x0000E848
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, Quaternion rot, string content, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x000106A4 File Offset: 0x0000E8A4
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, Quaternion rot, string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000106FC File Offset: 0x0000E8FC
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, Quaternion rot, string content, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001075C File Offset: 0x0000E95C
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, Quaternion rot, string content, TextAlign align, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x000107B8 File Offset: 0x0000E9B8
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, Quaternion rot, string content, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00010814 File Offset: 0x0000EA14
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, Quaternion rot, string content, TextAlign align, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001086C File Offset: 0x0000EA6C
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, Quaternion rot, string content, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x000108C8 File Offset: 0x0000EAC8
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, Quaternion rot, string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00010920 File Offset: 0x0000EB20
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, Quaternion rot, string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00010978 File Offset: 0x0000EB78
		[MethodImpl(256)]
		public static void Text(TextElement element, Vector3 pos, Quaternion rot, string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x000109CC File Offset: 0x0000EBCC
		[MethodImpl(256)]
		public static void Text(TextElement element, string content)
		{
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00010A08 File Offset: 0x0000EC08
		[MethodImpl(256)]
		public static void Text(TextElement element, string content, TextAlign align)
		{
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, align, Draw.Color);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00010A40 File Offset: 0x0000EC40
		[MethodImpl(256)]
		public static void Text(TextElement element, string content, float fontSize)
		{
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00010A78 File Offset: 0x0000EC78
		[MethodImpl(256)]
		public static void Text(TextElement element, string content, TextAlign align, float fontSize)
		{
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, fontSize, align, Draw.Color);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00010AAC File Offset: 0x0000ECAC
		[MethodImpl(256)]
		public static void Text(TextElement element, string content, TMP_FontAsset font)
		{
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, Draw.FontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00010AE4 File Offset: 0x0000ECE4
		[MethodImpl(256)]
		public static void Text(TextElement element, string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, Draw.FontSize, align, Draw.Color);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00010B18 File Offset: 0x0000ED18
		[MethodImpl(256)]
		public static void Text(TextElement element, string content, float fontSize, TMP_FontAsset font)
		{
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, fontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00010B4C File Offset: 0x0000ED4C
		[MethodImpl(256)]
		public static void Text(TextElement element, string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, fontSize, align, Draw.Color);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00010B7C File Offset: 0x0000ED7C
		[MethodImpl(256)]
		public static void Text(TextElement element, string content, Color color)
		{
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, Draw.TextAlign, color);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00010BB4 File Offset: 0x0000EDB4
		[MethodImpl(256)]
		public static void Text(TextElement element, string content, TextAlign align, Color color)
		{
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, align, color);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00010BE8 File Offset: 0x0000EDE8
		[MethodImpl(256)]
		public static void Text(TextElement element, string content, float fontSize, Color color)
		{
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, fontSize, Draw.TextAlign, color);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00010C1C File Offset: 0x0000EE1C
		[MethodImpl(256)]
		public static void Text(TextElement element, string content, TextAlign align, float fontSize, Color color)
		{
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), Draw.Font, fontSize, align, color);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00010C4C File Offset: 0x0000EE4C
		[MethodImpl(256)]
		public static void Text(TextElement element, string content, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, Draw.FontSize, Draw.TextAlign, color);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00010C80 File Offset: 0x0000EE80
		[MethodImpl(256)]
		public static void Text(TextElement element, string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, Draw.FontSize, align, color);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00010CB0 File Offset: 0x0000EEB0
		[MethodImpl(256)]
		public static void Text(TextElement element, string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, fontSize, Draw.TextAlign, color);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00010CE0 File Offset: 0x0000EEE0
		[MethodImpl(256)]
		public static void Text(TextElement element, string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(false, content, element, default(Vector2), default(Vector2), font, fontSize, align, color);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00010D10 File Offset: 0x0000EF10
		[MethodImpl(256)]
		public static void Text(Vector3 pos, string content)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00010D5C File Offset: 0x0000EF5C
		[MethodImpl(256)]
		public static void Text(Vector3 pos, string content, TextAlign align)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00010DA4 File Offset: 0x0000EFA4
		[MethodImpl(256)]
		public static void Text(Vector3 pos, string content, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00010DEC File Offset: 0x0000EFEC
		[MethodImpl(256)]
		public static void Text(Vector3 pos, string content, TextAlign align, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00010E30 File Offset: 0x0000F030
		[MethodImpl(256)]
		public static void Text(Vector3 pos, string content, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00010E78 File Offset: 0x0000F078
		[MethodImpl(256)]
		public static void Text(Vector3 pos, string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00010EBC File Offset: 0x0000F0BC
		[MethodImpl(256)]
		public static void Text(Vector3 pos, string content, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00010F00 File Offset: 0x0000F100
		[MethodImpl(256)]
		public static void Text(Vector3 pos, string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00010F40 File Offset: 0x0000F140
		[MethodImpl(256)]
		public static void Text(Vector3 pos, string content, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00010F88 File Offset: 0x0000F188
		[MethodImpl(256)]
		public static void Text(Vector3 pos, string content, TextAlign align, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00010FCC File Offset: 0x0000F1CC
		[MethodImpl(256)]
		public static void Text(Vector3 pos, string content, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00011010 File Offset: 0x0000F210
		[MethodImpl(256)]
		public static void Text(Vector3 pos, string content, TextAlign align, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00011050 File Offset: 0x0000F250
		[MethodImpl(256)]
		public static void Text(Vector3 pos, string content, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00011094 File Offset: 0x0000F294
		[MethodImpl(256)]
		public static void Text(Vector3 pos, string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x000110D4 File Offset: 0x0000F2D4
		[MethodImpl(256)]
		public static void Text(Vector3 pos, string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00011114 File Offset: 0x0000F314
		[MethodImpl(256)]
		public static void Text(Vector3 pos, string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00011154 File Offset: 0x0000F354
		[MethodImpl(256)]
		public static void Text(Vector3 pos, Quaternion rot, string content)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000111B4 File Offset: 0x0000F3B4
		[MethodImpl(256)]
		public static void Text(Vector3 pos, Quaternion rot, string content, TextAlign align)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00011210 File Offset: 0x0000F410
		[MethodImpl(256)]
		public static void Text(Vector3 pos, Quaternion rot, string content, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0001126C File Offset: 0x0000F46C
		[MethodImpl(256)]
		public static void Text(Vector3 pos, Quaternion rot, string content, TextAlign align, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x000112C8 File Offset: 0x0000F4C8
		[MethodImpl(256)]
		public static void Text(Vector3 pos, Quaternion rot, string content, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00011324 File Offset: 0x0000F524
		[MethodImpl(256)]
		public static void Text(Vector3 pos, Quaternion rot, string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00011380 File Offset: 0x0000F580
		[MethodImpl(256)]
		public static void Text(Vector3 pos, Quaternion rot, string content, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x000113DC File Offset: 0x0000F5DC
		[MethodImpl(256)]
		public static void Text(Vector3 pos, Quaternion rot, string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00011434 File Offset: 0x0000F634
		[MethodImpl(256)]
		public static void Text(Vector3 pos, Quaternion rot, string content, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00011490 File Offset: 0x0000F690
		[MethodImpl(256)]
		public static void Text(Vector3 pos, Quaternion rot, string content, TextAlign align, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x000114EC File Offset: 0x0000F6EC
		[MethodImpl(256)]
		public static void Text(Vector3 pos, Quaternion rot, string content, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00011548 File Offset: 0x0000F748
		[MethodImpl(256)]
		public static void Text(Vector3 pos, Quaternion rot, string content, TextAlign align, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x000115A0 File Offset: 0x0000F7A0
		[MethodImpl(256)]
		public static void Text(Vector3 pos, Quaternion rot, string content, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x000115FC File Offset: 0x0000F7FC
		[MethodImpl(256)]
		public static void Text(Vector3 pos, Quaternion rot, string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00011654 File Offset: 0x0000F854
		[MethodImpl(256)]
		public static void Text(Vector3 pos, Quaternion rot, string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x000116AC File Offset: 0x0000F8AC
		[MethodImpl(256)]
		public static void Text(Vector3 pos, Quaternion rot, string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00011700 File Offset: 0x0000F900
		[MethodImpl(256)]
		public static void Text(string content)
		{
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001173C File Offset: 0x0000F93C
		[MethodImpl(256)]
		public static void Text(string content, TextAlign align)
		{
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, align, Draw.Color);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00011774 File Offset: 0x0000F974
		[MethodImpl(256)]
		public static void Text(string content, float fontSize)
		{
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x000117AC File Offset: 0x0000F9AC
		[MethodImpl(256)]
		public static void Text(string content, TextAlign align, float fontSize)
		{
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, fontSize, align, Draw.Color);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000117E0 File Offset: 0x0000F9E0
		[MethodImpl(256)]
		public static void Text(string content, TMP_FontAsset font)
		{
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, Draw.FontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00011818 File Offset: 0x0000FA18
		[MethodImpl(256)]
		public static void Text(string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, Draw.FontSize, align, Draw.Color);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001184C File Offset: 0x0000FA4C
		[MethodImpl(256)]
		public static void Text(string content, float fontSize, TMP_FontAsset font)
		{
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, fontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00011880 File Offset: 0x0000FA80
		[MethodImpl(256)]
		public static void Text(string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, fontSize, align, Draw.Color);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x000118B0 File Offset: 0x0000FAB0
		[MethodImpl(256)]
		public static void Text(string content, Color color)
		{
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, Draw.TextAlign, color);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x000118E8 File Offset: 0x0000FAE8
		[MethodImpl(256)]
		public static void Text(string content, TextAlign align, Color color)
		{
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, Draw.FontSize, align, color);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001191C File Offset: 0x0000FB1C
		[MethodImpl(256)]
		public static void Text(string content, float fontSize, Color color)
		{
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, fontSize, Draw.TextAlign, color);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00011950 File Offset: 0x0000FB50
		[MethodImpl(256)]
		public static void Text(string content, TextAlign align, float fontSize, Color color)
		{
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), Draw.Font, fontSize, align, color);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00011980 File Offset: 0x0000FB80
		[MethodImpl(256)]
		public static void Text(string content, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, Draw.FontSize, Draw.TextAlign, color);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x000119B4 File Offset: 0x0000FBB4
		[MethodImpl(256)]
		public static void Text(string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, Draw.FontSize, align, color);
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x000119E4 File Offset: 0x0000FBE4
		[MethodImpl(256)]
		public static void Text(string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, fontSize, Draw.TextAlign, color);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00011A14 File Offset: 0x0000FC14
		[MethodImpl(256)]
		public static void Text(string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(false, content, null, default(Vector2), default(Vector2), font, fontSize, align, color);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00011A40 File Offset: 0x0000FC40
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Vector2 pivot, Vector2 size, string content)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00011A7C File Offset: 0x0000FC7C
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Vector2 pivot, Vector2 size, string content, TextAlign align)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00011AB8 File Offset: 0x0000FCB8
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Vector2 pivot, Vector2 size, string content, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00011AF4 File Offset: 0x0000FCF4
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00011B2C File Offset: 0x0000FD2C
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Vector2 pivot, Vector2 size, string content, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, element, pivot, size, font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00011B68 File Offset: 0x0000FD68
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Vector2 pivot, Vector2 size, string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, element, pivot, size, font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00011BA0 File Offset: 0x0000FDA0
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Vector2 pivot, Vector2 size, string content, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, element, pivot, size, font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00011BD8 File Offset: 0x0000FDD8
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, element, pivot, size, font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00011C0C File Offset: 0x0000FE0C
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Vector2 pivot, Vector2 size, string content, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00011C48 File Offset: 0x0000FE48
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Vector2 pivot, Vector2 size, string content, TextAlign align, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00011C80 File Offset: 0x0000FE80
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Vector2 pivot, Vector2 size, string content, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00011CB8 File Offset: 0x0000FEB8
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00011CEC File Offset: 0x0000FEEC
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Vector2 pivot, Vector2 size, string content, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, element, pivot, size, font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00011D24 File Offset: 0x0000FF24
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Vector2 pivot, Vector2 size, string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, element, pivot, size, font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00011D58 File Offset: 0x0000FF58
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Vector2 pivot, Vector2 size, string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, element, pivot, size, font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00011D8C File Offset: 0x0000FF8C
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, element, pivot, size, font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00011DBC File Offset: 0x0000FFBC
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00011E10 File Offset: 0x00010010
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TextAlign align)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00011E60 File Offset: 0x00010060
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00011EB0 File Offset: 0x000100B0
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00011EFC File Offset: 0x000100FC
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, element, pivot, size, font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00011F4C File Offset: 0x0001014C
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, element, pivot, size, font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00011F98 File Offset: 0x00010198
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, element, pivot, size, font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00011FE4 File Offset: 0x000101E4
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, element, pivot, size, font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00012030 File Offset: 0x00010230
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00012080 File Offset: 0x00010280
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TextAlign align, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x000120CC File Offset: 0x000102CC
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00012118 File Offset: 0x00010318
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00012164 File Offset: 0x00010364
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, element, pivot, size, font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x000121B0 File Offset: 0x000103B0
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, element, pivot, size, font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x000121FC File Offset: 0x000103FC
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, element, pivot, size, font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00012248 File Offset: 0x00010448
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, element, pivot, size, font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00012290 File Offset: 0x00010490
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector2 pivot, Vector2 size, string content)
		{
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x000122BC File Offset: 0x000104BC
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector2 pivot, Vector2 size, string content, TextAlign align)
		{
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, Draw.FontSize, align, Draw.Color);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x000122E4 File Offset: 0x000104E4
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector2 pivot, Vector2 size, string content, float fontSize)
		{
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001230C File Offset: 0x0001050C
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize)
		{
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, fontSize, align, Draw.Color);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00012334 File Offset: 0x00010534
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector2 pivot, Vector2 size, string content, TMP_FontAsset font)
		{
			Draw.Text_Internal(true, content, element, pivot, size, font, Draw.FontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0001235C File Offset: 0x0001055C
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector2 pivot, Vector2 size, string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.Text_Internal(true, content, element, pivot, size, font, Draw.FontSize, align, Draw.Color);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00012384 File Offset: 0x00010584
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector2 pivot, Vector2 size, string content, float fontSize, TMP_FontAsset font)
		{
			Draw.Text_Internal(true, content, element, pivot, size, font, fontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x000123AC File Offset: 0x000105AC
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.Text_Internal(true, content, element, pivot, size, font, fontSize, align, Draw.Color);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x000123D0 File Offset: 0x000105D0
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector2 pivot, Vector2 size, string content, Color color)
		{
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, Draw.FontSize, Draw.TextAlign, color);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x000123F8 File Offset: 0x000105F8
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector2 pivot, Vector2 size, string content, TextAlign align, Color color)
		{
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, Draw.FontSize, align, color);
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00012420 File Offset: 0x00010620
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector2 pivot, Vector2 size, string content, float fontSize, Color color)
		{
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, fontSize, Draw.TextAlign, color);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00012448 File Offset: 0x00010648
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, Color color)
		{
			Draw.Text_Internal(true, content, element, pivot, size, Draw.Font, fontSize, align, color);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001246C File Offset: 0x0001066C
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector2 pivot, Vector2 size, string content, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(true, content, element, pivot, size, font, Draw.FontSize, Draw.TextAlign, color);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00012494 File Offset: 0x00010694
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector2 pivot, Vector2 size, string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(true, content, element, pivot, size, font, Draw.FontSize, align, color);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000124B8 File Offset: 0x000106B8
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector2 pivot, Vector2 size, string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(true, content, element, pivot, size, font, fontSize, Draw.TextAlign, color);
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x000124DC File Offset: 0x000106DC
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(true, content, element, pivot, size, font, fontSize, align, color);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x000124FC File Offset: 0x000106FC
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Vector2 pivot, Vector2 size, string content)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00012538 File Offset: 0x00010738
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Vector2 pivot, Vector2 size, string content, TextAlign align)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00012570 File Offset: 0x00010770
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Vector2 pivot, Vector2 size, string content, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x000125A8 File Offset: 0x000107A8
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x000125E0 File Offset: 0x000107E0
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Vector2 pivot, Vector2 size, string content, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, null, pivot, size, font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00012618 File Offset: 0x00010818
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Vector2 pivot, Vector2 size, string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, null, pivot, size, font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00012650 File Offset: 0x00010850
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Vector2 pivot, Vector2 size, string content, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, null, pivot, size, font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00012688 File Offset: 0x00010888
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, null, pivot, size, font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x000126BC File Offset: 0x000108BC
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Vector2 pivot, Vector2 size, string content, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x000126F4 File Offset: 0x000108F4
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Vector2 pivot, Vector2 size, string content, TextAlign align, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001272C File Offset: 0x0001092C
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Vector2 pivot, Vector2 size, string content, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00012764 File Offset: 0x00010964
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00012798 File Offset: 0x00010998
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Vector2 pivot, Vector2 size, string content, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, null, pivot, size, font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x000127D0 File Offset: 0x000109D0
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Vector2 pivot, Vector2 size, string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, null, pivot, size, font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00012804 File Offset: 0x00010A04
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Vector2 pivot, Vector2 size, string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, null, pivot, size, font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00012838 File Offset: 0x00010A38
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(true, content, null, pivot, size, font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00012868 File Offset: 0x00010A68
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x000128BC File Offset: 0x00010ABC
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TextAlign align)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001290C File Offset: 0x00010B0C
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001295C File Offset: 0x00010B5C
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x000129A8 File Offset: 0x00010BA8
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, null, pivot, size, font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x000129F8 File Offset: 0x00010BF8
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, null, pivot, size, font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00012A44 File Offset: 0x00010C44
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, null, pivot, size, font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00012A90 File Offset: 0x00010C90
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, null, pivot, size, font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00012AD8 File Offset: 0x00010CD8
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00012B28 File Offset: 0x00010D28
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TextAlign align, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00012B74 File Offset: 0x00010D74
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00012BC0 File Offset: 0x00010DC0
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00012C08 File Offset: 0x00010E08
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, null, pivot, size, font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00012C54 File Offset: 0x00010E54
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, null, pivot, size, font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00012C9C File Offset: 0x00010E9C
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, null, pivot, size, font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00012CE4 File Offset: 0x00010EE4
		[MethodImpl(256)]
		public static void TextRect(Vector3 pos, Quaternion rot, Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(true, content, null, pivot, size, font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00012D2C File Offset: 0x00010F2C
		[MethodImpl(256)]
		public static void TextRect(Vector2 pivot, Vector2 size, string content)
		{
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00012D58 File Offset: 0x00010F58
		[MethodImpl(256)]
		public static void TextRect(Vector2 pivot, Vector2 size, string content, TextAlign align)
		{
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, Draw.FontSize, align, Draw.Color);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00012D80 File Offset: 0x00010F80
		[MethodImpl(256)]
		public static void TextRect(Vector2 pivot, Vector2 size, string content, float fontSize)
		{
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00012DA8 File Offset: 0x00010FA8
		[MethodImpl(256)]
		public static void TextRect(Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize)
		{
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, fontSize, align, Draw.Color);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00012DCC File Offset: 0x00010FCC
		[MethodImpl(256)]
		public static void TextRect(Vector2 pivot, Vector2 size, string content, TMP_FontAsset font)
		{
			Draw.Text_Internal(true, content, null, pivot, size, font, Draw.FontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00012DF4 File Offset: 0x00010FF4
		[MethodImpl(256)]
		public static void TextRect(Vector2 pivot, Vector2 size, string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.Text_Internal(true, content, null, pivot, size, font, Draw.FontSize, align, Draw.Color);
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00012E18 File Offset: 0x00011018
		[MethodImpl(256)]
		public static void TextRect(Vector2 pivot, Vector2 size, string content, float fontSize, TMP_FontAsset font)
		{
			Draw.Text_Internal(true, content, null, pivot, size, font, fontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00012E3C File Offset: 0x0001103C
		[MethodImpl(256)]
		public static void TextRect(Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.Text_Internal(true, content, null, pivot, size, font, fontSize, align, Draw.Color);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00012E60 File Offset: 0x00011060
		[MethodImpl(256)]
		public static void TextRect(Vector2 pivot, Vector2 size, string content, Color color)
		{
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, Draw.FontSize, Draw.TextAlign, color);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00012E88 File Offset: 0x00011088
		[MethodImpl(256)]
		public static void TextRect(Vector2 pivot, Vector2 size, string content, TextAlign align, Color color)
		{
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, Draw.FontSize, align, color);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00012EAC File Offset: 0x000110AC
		[MethodImpl(256)]
		public static void TextRect(Vector2 pivot, Vector2 size, string content, float fontSize, Color color)
		{
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, fontSize, Draw.TextAlign, color);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00012ED0 File Offset: 0x000110D0
		[MethodImpl(256)]
		public static void TextRect(Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, Color color)
		{
			Draw.Text_Internal(true, content, null, pivot, size, Draw.Font, fontSize, align, color);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00012EF4 File Offset: 0x000110F4
		[MethodImpl(256)]
		public static void TextRect(Vector2 pivot, Vector2 size, string content, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(true, content, null, pivot, size, font, Draw.FontSize, Draw.TextAlign, color);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x00012F18 File Offset: 0x00011118
		[MethodImpl(256)]
		public static void TextRect(Vector2 pivot, Vector2 size, string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(true, content, null, pivot, size, font, Draw.FontSize, align, color);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x00012F3C File Offset: 0x0001113C
		[MethodImpl(256)]
		public static void TextRect(Vector2 pivot, Vector2 size, string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(true, content, null, pivot, size, font, fontSize, Draw.TextAlign, color);
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00012F60 File Offset: 0x00011160
		[MethodImpl(256)]
		public static void TextRect(Vector2 pivot, Vector2 size, string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(true, content, null, pivot, size, font, fontSize, align, color);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00012F7E File Offset: 0x0001117E
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Rect rect, string content)
		{
			Draw.TextRect_Internal(content, element, rect, Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00012F9C File Offset: 0x0001119C
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Rect rect, string content, TextAlign align)
		{
			Draw.TextRect_Internal(content, element, rect, Draw.Font, Draw.FontSize, align, Draw.Color);
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00012FB6 File Offset: 0x000111B6
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Rect rect, string content, float fontSize)
		{
			Draw.TextRect_Internal(content, element, rect, Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00012FD0 File Offset: 0x000111D0
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Rect rect, string content, TextAlign align, float fontSize)
		{
			Draw.TextRect_Internal(content, element, rect, Draw.Font, fontSize, align, Draw.Color);
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00012FE7 File Offset: 0x000111E7
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Rect rect, string content, TMP_FontAsset font)
		{
			Draw.TextRect_Internal(content, element, rect, font, Draw.FontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00013001 File Offset: 0x00011201
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Rect rect, string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.TextRect_Internal(content, element, rect, font, Draw.FontSize, align, Draw.Color);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00013018 File Offset: 0x00011218
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Rect rect, string content, float fontSize, TMP_FontAsset font)
		{
			Draw.TextRect_Internal(content, element, rect, font, fontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001302F File Offset: 0x0001122F
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Rect rect, string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.TextRect_Internal(content, element, rect, font, fontSize, align, Draw.Color);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00013043 File Offset: 0x00011243
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Rect rect, string content, Color color)
		{
			Draw.TextRect_Internal(content, element, rect, Draw.Font, Draw.FontSize, Draw.TextAlign, color);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001305D File Offset: 0x0001125D
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Rect rect, string content, TextAlign align, Color color)
		{
			Draw.TextRect_Internal(content, element, rect, Draw.Font, Draw.FontSize, align, color);
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00013074 File Offset: 0x00011274
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Rect rect, string content, float fontSize, Color color)
		{
			Draw.TextRect_Internal(content, element, rect, Draw.Font, fontSize, Draw.TextAlign, color);
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001308B File Offset: 0x0001128B
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Rect rect, string content, TextAlign align, float fontSize, Color color)
		{
			Draw.TextRect_Internal(content, element, rect, Draw.Font, fontSize, align, color);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001309F File Offset: 0x0001129F
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Rect rect, string content, TMP_FontAsset font, Color color)
		{
			Draw.TextRect_Internal(content, element, rect, font, Draw.FontSize, Draw.TextAlign, color);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x000130B6 File Offset: 0x000112B6
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Rect rect, string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.TextRect_Internal(content, element, rect, font, Draw.FontSize, align, color);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x000130CA File Offset: 0x000112CA
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Rect rect, string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.TextRect_Internal(content, element, rect, font, fontSize, Draw.TextAlign, color);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x000130DE File Offset: 0x000112DE
		[MethodImpl(256)]
		public static void TextRect(TextElement element, Rect rect, string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.TextRect_Internal(content, element, rect, font, fontSize, align, color);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x000130EF File Offset: 0x000112EF
		[MethodImpl(256)]
		public static void TextRect(Rect rect, string content)
		{
			Draw.TextRect_Internal(content, null, rect, Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001310D File Offset: 0x0001130D
		[MethodImpl(256)]
		public static void TextRect(Rect rect, string content, TextAlign align)
		{
			Draw.TextRect_Internal(content, null, rect, Draw.Font, Draw.FontSize, align, Draw.Color);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00013127 File Offset: 0x00011327
		[MethodImpl(256)]
		public static void TextRect(Rect rect, string content, float fontSize)
		{
			Draw.TextRect_Internal(content, null, rect, Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00013141 File Offset: 0x00011341
		[MethodImpl(256)]
		public static void TextRect(Rect rect, string content, TextAlign align, float fontSize)
		{
			Draw.TextRect_Internal(content, null, rect, Draw.Font, fontSize, align, Draw.Color);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00013157 File Offset: 0x00011357
		[MethodImpl(256)]
		public static void TextRect(Rect rect, string content, TMP_FontAsset font)
		{
			Draw.TextRect_Internal(content, null, rect, font, Draw.FontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00013171 File Offset: 0x00011371
		[MethodImpl(256)]
		public static void TextRect(Rect rect, string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.TextRect_Internal(content, null, rect, font, Draw.FontSize, align, Draw.Color);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00013187 File Offset: 0x00011387
		[MethodImpl(256)]
		public static void TextRect(Rect rect, string content, float fontSize, TMP_FontAsset font)
		{
			Draw.TextRect_Internal(content, null, rect, font, fontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001319D File Offset: 0x0001139D
		[MethodImpl(256)]
		public static void TextRect(Rect rect, string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.TextRect_Internal(content, null, rect, font, fontSize, align, Draw.Color);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x000131B0 File Offset: 0x000113B0
		[MethodImpl(256)]
		public static void TextRect(Rect rect, string content, Color color)
		{
			Draw.TextRect_Internal(content, null, rect, Draw.Font, Draw.FontSize, Draw.TextAlign, color);
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x000131CA File Offset: 0x000113CA
		[MethodImpl(256)]
		public static void TextRect(Rect rect, string content, TextAlign align, Color color)
		{
			Draw.TextRect_Internal(content, null, rect, Draw.Font, Draw.FontSize, align, color);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x000131E0 File Offset: 0x000113E0
		[MethodImpl(256)]
		public static void TextRect(Rect rect, string content, float fontSize, Color color)
		{
			Draw.TextRect_Internal(content, null, rect, Draw.Font, fontSize, Draw.TextAlign, color);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x000131F6 File Offset: 0x000113F6
		[MethodImpl(256)]
		public static void TextRect(Rect rect, string content, TextAlign align, float fontSize, Color color)
		{
			Draw.TextRect_Internal(content, null, rect, Draw.Font, fontSize, align, color);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00013209 File Offset: 0x00011409
		[MethodImpl(256)]
		public static void TextRect(Rect rect, string content, TMP_FontAsset font, Color color)
		{
			Draw.TextRect_Internal(content, null, rect, font, Draw.FontSize, Draw.TextAlign, color);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001321F File Offset: 0x0001141F
		[MethodImpl(256)]
		public static void TextRect(Rect rect, string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.TextRect_Internal(content, null, rect, font, Draw.FontSize, align, color);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00013232 File Offset: 0x00011432
		[MethodImpl(256)]
		public static void TextRect(Rect rect, string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.TextRect_Internal(content, null, rect, font, fontSize, Draw.TextAlign, color);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00013245 File Offset: 0x00011445
		[MethodImpl(256)]
		public static void TextRect(Rect rect, string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.TextRect_Internal(content, null, rect, font, fontSize, align, color);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00013255 File Offset: 0x00011455
		[MethodImpl(256)]
		public static void Texture(Texture texture, Rect rect, Rect uvs)
		{
			Draw.Texture_Internal(texture, rect, uvs, Draw.Color);
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00013264 File Offset: 0x00011464
		[MethodImpl(256)]
		public static void Texture(Texture texture, Rect rect, Rect uvs, Color color)
		{
			Draw.Texture_Internal(texture, rect, uvs, color);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001326F File Offset: 0x0001146F
		[MethodImpl(256)]
		public static void Texture(Texture texture, Rect rect)
		{
			Draw.Texture_RectFill_Internal(texture, rect, TextureFillMode.ScaleToFit, Draw.Color);
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001327E File Offset: 0x0001147E
		[MethodImpl(256)]
		public static void Texture(Texture texture, Rect rect, Color color)
		{
			Draw.Texture_RectFill_Internal(texture, rect, TextureFillMode.ScaleToFit, color);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00013289 File Offset: 0x00011489
		[MethodImpl(256)]
		public static void Texture(Texture texture, Rect rect, TextureFillMode fillMode)
		{
			Draw.Texture_RectFill_Internal(texture, rect, fillMode, Draw.Color);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00013298 File Offset: 0x00011498
		[MethodImpl(256)]
		public static void Texture(Texture texture, Rect rect, TextureFillMode fillMode, Color color)
		{
			Draw.Texture_RectFill_Internal(texture, rect, fillMode, color);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x000132A3 File Offset: 0x000114A3
		[MethodImpl(256)]
		public static void Texture(Texture texture, Vector2 center, float size)
		{
			Draw.Texture_PosSize_Internal(texture, center, size, TextureSizeMode.LongestSide, Draw.Color);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000132B3 File Offset: 0x000114B3
		[MethodImpl(256)]
		public static void Texture(Texture texture, Vector2 center, float size, Color color)
		{
			Draw.Texture_PosSize_Internal(texture, center, size, TextureSizeMode.LongestSide, color);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x000132BF File Offset: 0x000114BF
		[MethodImpl(256)]
		public static void Texture(Texture texture, Vector2 center, float size, TextureSizeMode sizeMode)
		{
			Draw.Texture_PosSize_Internal(texture, center, size, sizeMode, Draw.Color);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x000132CF File Offset: 0x000114CF
		[MethodImpl(256)]
		public static void Texture(Texture texture, Vector2 center, float size, TextureSizeMode sizeMode, Color color)
		{
			Draw.Texture_PosSize_Internal(texture, center, size, sizeMode, color);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x000132DC File Offset: 0x000114DC
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end)
		{
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x000132DE File Offset: 0x000114DE
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, Color color)
		{
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x000132E0 File Offset: 0x000114E0
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x000132E2 File Offset: 0x000114E2
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, float thickness)
		{
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000132E4 File Offset: 0x000114E4
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, float thickness, Color color)
		{
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x000132E6 File Offset: 0x000114E6
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, float thickness, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x000132E8 File Offset: 0x000114E8
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, LineEndCap endCaps)
		{
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000132EA File Offset: 0x000114EA
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, LineEndCap endCaps, Color color)
		{
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x000132EC File Offset: 0x000114EC
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, LineEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x000132EE File Offset: 0x000114EE
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, float thickness, LineEndCap endCaps)
		{
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x000132F0 File Offset: 0x000114F0
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, float thickness, LineEndCap endCaps, Color color)
		{
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x000132F2 File Offset: 0x000114F2
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, float thickness, LineEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x000132F4 File Offset: 0x000114F4
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle)
		{
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x000132F6 File Offset: 0x000114F6
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, Color color)
		{
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x000132F8 File Offset: 0x000114F8
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x000132FA File Offset: 0x000114FA
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, float thickness)
		{
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x000132FC File Offset: 0x000114FC
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, float thickness, Color color)
		{
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x000132FE File Offset: 0x000114FE
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, float thickness, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00013300 File Offset: 0x00011500
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, LineEndCap endCaps)
		{
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00013302 File Offset: 0x00011502
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, LineEndCap endCaps, Color color)
		{
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00013304 File Offset: 0x00011504
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, LineEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00013306 File Offset: 0x00011506
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, float thickness, LineEndCap endCaps)
		{
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00013308 File Offset: 0x00011508
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, float thickness, LineEndCap endCaps, Color color)
		{
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001330A File Offset: 0x0001150A
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, float thickness, LineEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0001330C File Offset: 0x0001150C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void PolygonFill(PolygonPath path)
		{
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0001330E File Offset: 0x0001150E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void PolygonFill(PolygonPath path, GradientFill fill)
		{
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00013310 File Offset: 0x00011510
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void PolygonFill(PolygonPath path, PolygonTriangulation triangulation)
		{
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00013312 File Offset: 0x00011512
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void PolygonFill(PolygonPath path, PolygonTriangulation triangulation, GradientFill fill)
		{
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00013314 File Offset: 0x00011514
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void PolygonFillLinear(PolygonPath path, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00013316 File Offset: 0x00011516
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void PolygonFillLinear(PolygonPath path, PolygonTriangulation triangulation, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00013318 File Offset: 0x00011518
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void PolygonFillRadial(PolygonPath path, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0001331A File Offset: 0x0001151A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void PolygonFillRadial(PolygonPath path, PolygonTriangulation triangulation, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001331C File Offset: 0x0001151C
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos)
		{
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001331E File Offset: 0x0001151E
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Color color)
		{
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00013320 File Offset: 0x00011520
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, float radius)
		{
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00013322 File Offset: 0x00011522
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, float radius, Color color)
		{
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00013324 File Offset: 0x00011524
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, float radius, float thickness)
		{
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00013326 File Offset: 0x00011526
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, float radius, float thickness, Color color)
		{
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00013328 File Offset: 0x00011528
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, float radius, float thickness, float angle)
		{
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0001332A File Offset: 0x0001152A
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, float radius, float thickness, float angle, Color color)
		{
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001332C File Offset: 0x0001152C
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, float radius, float thickness, float angle, float roundness)
		{
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001332E File Offset: 0x0001152E
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, float radius, float thickness, float angle, float roundness, Color color)
		{
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00013330 File Offset: 0x00011530
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, int sideCount)
		{
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00013332 File Offset: 0x00011532
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, Color color)
		{
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00013334 File Offset: 0x00011534
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, float radius)
		{
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00013336 File Offset: 0x00011536
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, float radius, Color color)
		{
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00013338 File Offset: 0x00011538
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, float radius, float thickness)
		{
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001333A File Offset: 0x0001153A
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, float radius, float thickness, Color color)
		{
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001333C File Offset: 0x0001153C
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, float radius, float thickness, float angle)
		{
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0001333E File Offset: 0x0001153E
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, float radius, float thickness, float angle, Color color)
		{
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00013340 File Offset: 0x00011540
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, float radius, float thickness, float angle, float roundness)
		{
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00013342 File Offset: 0x00011542
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, float radius, float thickness, float angle, float roundness, Color color)
		{
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00013344 File Offset: 0x00011544
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal)
		{
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00013346 File Offset: 0x00011546
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, Color color)
		{
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00013348 File Offset: 0x00011548
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, float radius)
		{
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0001334A File Offset: 0x0001154A
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, float radius, Color color)
		{
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0001334C File Offset: 0x0001154C
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, float radius, float thickness)
		{
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001334E File Offset: 0x0001154E
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, float radius, float thickness, Color color)
		{
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00013350 File Offset: 0x00011550
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, float radius, float thickness, float angle)
		{
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00013352 File Offset: 0x00011552
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, Color color)
		{
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00013354 File Offset: 0x00011554
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, float roundness)
		{
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00013356 File Offset: 0x00011556
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, float roundness, Color color)
		{
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00013358 File Offset: 0x00011558
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount)
		{
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001335A File Offset: 0x0001155A
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, Color color)
		{
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0001335C File Offset: 0x0001155C
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, float radius)
		{
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001335E File Offset: 0x0001155E
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, float radius, Color color)
		{
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00013360 File Offset: 0x00011560
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness)
		{
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00013362 File Offset: 0x00011562
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, Color color)
		{
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00013364 File Offset: 0x00011564
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle)
		{
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00013366 File Offset: 0x00011566
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, Color color)
		{
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00013368 File Offset: 0x00011568
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, float roundness)
		{
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0001336A File Offset: 0x0001156A
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, float roundness, Color color)
		{
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0001336C File Offset: 0x0001156C
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot)
		{
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0001336E File Offset: 0x0001156E
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, Color color)
		{
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00013370 File Offset: 0x00011570
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, float radius)
		{
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00013372 File Offset: 0x00011572
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, float radius, Color color)
		{
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00013374 File Offset: 0x00011574
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, float radius, float thickness)
		{
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00013376 File Offset: 0x00011576
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, float radius, float thickness, Color color)
		{
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00013378 File Offset: 0x00011578
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, float radius, float thickness, float angle)
		{
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0001337A File Offset: 0x0001157A
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, Color color)
		{
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0001337C File Offset: 0x0001157C
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, float roundness)
		{
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0001337E File Offset: 0x0001157E
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, float roundness, Color color)
		{
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00013380 File Offset: 0x00011580
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount)
		{
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00013382 File Offset: 0x00011582
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, Color color)
		{
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00013384 File Offset: 0x00011584
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, float radius)
		{
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00013386 File Offset: 0x00011586
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, float radius, Color color)
		{
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00013388 File Offset: 0x00011588
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness)
		{
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0001338A File Offset: 0x0001158A
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, Color color)
		{
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0001338C File Offset: 0x0001158C
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle)
		{
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0001338E File Offset: 0x0001158E
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, Color color)
		{
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00013390 File Offset: 0x00011590
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, float roundness)
		{
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00013392 File Offset: 0x00011592
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, float roundness, Color color)
		{
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00013394 File Offset: 0x00011594
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow()
		{
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00013396 File Offset: 0x00011596
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(Color color)
		{
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00013398 File Offset: 0x00011598
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(float radius)
		{
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0001339A File Offset: 0x0001159A
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(float radius, Color color)
		{
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0001339C File Offset: 0x0001159C
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(float radius, float thickness)
		{
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0001339E File Offset: 0x0001159E
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(float radius, float thickness, Color color)
		{
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x000133A0 File Offset: 0x000115A0
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(float radius, float thickness, float angle)
		{
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x000133A2 File Offset: 0x000115A2
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(float radius, float thickness, float angle, Color color)
		{
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x000133A4 File Offset: 0x000115A4
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(float radius, float thickness, float angle, float roundness)
		{
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x000133A6 File Offset: 0x000115A6
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(float radius, float thickness, float angle, float roundness, Color color)
		{
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x000133A8 File Offset: 0x000115A8
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(int sideCount)
		{
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x000133AA File Offset: 0x000115AA
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(int sideCount, Color color)
		{
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x000133AC File Offset: 0x000115AC
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(int sideCount, float radius)
		{
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x000133AE File Offset: 0x000115AE
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(int sideCount, float radius, Color color)
		{
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x000133B0 File Offset: 0x000115B0
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(int sideCount, float radius, float thickness)
		{
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x000133B2 File Offset: 0x000115B2
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(int sideCount, float radius, float thickness, Color color)
		{
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x000133B4 File Offset: 0x000115B4
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(int sideCount, float radius, float thickness, float angle)
		{
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x000133B6 File Offset: 0x000115B6
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(int sideCount, float radius, float thickness, float angle, Color color)
		{
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x000133B8 File Offset: 0x000115B8
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(int sideCount, float radius, float thickness, float angle, float roundness)
		{
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x000133BA File Offset: 0x000115BA
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollow(int sideCount, float radius, float thickness, float angle, float roundness, Color color)
		{
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x000133BC File Offset: 0x000115BC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos)
		{
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x000133BE File Offset: 0x000115BE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, GradientFill fill)
		{
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x000133C0 File Offset: 0x000115C0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, float radius)
		{
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x000133C2 File Offset: 0x000115C2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, float radius, GradientFill fill)
		{
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x000133C4 File Offset: 0x000115C4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, float radius, float angle)
		{
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x000133C6 File Offset: 0x000115C6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, float radius, float angle, GradientFill fill)
		{
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x000133C8 File Offset: 0x000115C8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, float radius, float angle, float roundness)
		{
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x000133CA File Offset: 0x000115CA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, float radius, float angle, float roundness, GradientFill fill)
		{
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x000133CC File Offset: 0x000115CC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, int sideCount)
		{
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x000133CE File Offset: 0x000115CE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, int sideCount, GradientFill fill)
		{
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x000133D0 File Offset: 0x000115D0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, int sideCount, float radius)
		{
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x000133D2 File Offset: 0x000115D2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, int sideCount, float radius, GradientFill fill)
		{
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x000133D4 File Offset: 0x000115D4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, int sideCount, float radius, float angle)
		{
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x000133D6 File Offset: 0x000115D6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, int sideCount, float radius, float angle, GradientFill fill)
		{
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x000133D8 File Offset: 0x000115D8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, int sideCount, float radius, float angle, float roundness)
		{
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x000133DA File Offset: 0x000115DA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, int sideCount, float radius, float angle, float roundness, GradientFill fill)
		{
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x000133DC File Offset: 0x000115DC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal)
		{
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x000133DE File Offset: 0x000115DE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, GradientFill fill)
		{
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x000133E0 File Offset: 0x000115E0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, float radius)
		{
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x000133E2 File Offset: 0x000115E2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, float radius, GradientFill fill)
		{
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x000133E4 File Offset: 0x000115E4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, float radius, float angle)
		{
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x000133E6 File Offset: 0x000115E6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, float radius, float angle, GradientFill fill)
		{
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x000133E8 File Offset: 0x000115E8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, float radius, float angle, float roundness)
		{
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x000133EA File Offset: 0x000115EA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, float radius, float angle, float roundness, GradientFill fill)
		{
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x000133EC File Offset: 0x000115EC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, int sideCount)
		{
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x000133EE File Offset: 0x000115EE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, int sideCount, GradientFill fill)
		{
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x000133F0 File Offset: 0x000115F0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, int sideCount, float radius)
		{
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x000133F2 File Offset: 0x000115F2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, int sideCount, float radius, GradientFill fill)
		{
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x000133F4 File Offset: 0x000115F4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle)
		{
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x000133F6 File Offset: 0x000115F6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, GradientFill fill)
		{
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x000133F8 File Offset: 0x000115F8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, float roundness)
		{
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x000133FA File Offset: 0x000115FA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, float roundness, GradientFill fill)
		{
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x000133FC File Offset: 0x000115FC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot)
		{
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x000133FE File Offset: 0x000115FE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, GradientFill fill)
		{
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00013400 File Offset: 0x00011600
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, float radius)
		{
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00013402 File Offset: 0x00011602
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, float radius, GradientFill fill)
		{
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00013404 File Offset: 0x00011604
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, float radius, float angle)
		{
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00013406 File Offset: 0x00011606
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, float radius, float angle, GradientFill fill)
		{
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00013408 File Offset: 0x00011608
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, float radius, float angle, float roundness)
		{
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0001340A File Offset: 0x0001160A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, float radius, float angle, float roundness, GradientFill fill)
		{
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0001340C File Offset: 0x0001160C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, int sideCount)
		{
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0001340E File Offset: 0x0001160E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, int sideCount, GradientFill fill)
		{
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00013410 File Offset: 0x00011610
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, int sideCount, float radius)
		{
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00013412 File Offset: 0x00011612
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, int sideCount, float radius, GradientFill fill)
		{
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00013414 File Offset: 0x00011614
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle)
		{
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00013416 File Offset: 0x00011616
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, GradientFill fill)
		{
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00013418 File Offset: 0x00011618
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, float roundness)
		{
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001341A File Offset: 0x0001161A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, float roundness, GradientFill fill)
		{
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0001341C File Offset: 0x0001161C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill()
		{
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0001341E File Offset: 0x0001161E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(GradientFill fill)
		{
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00013420 File Offset: 0x00011620
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(float radius)
		{
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00013422 File Offset: 0x00011622
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(float radius, GradientFill fill)
		{
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00013424 File Offset: 0x00011624
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(float radius, float angle)
		{
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00013426 File Offset: 0x00011626
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(float radius, float angle, GradientFill fill)
		{
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00013428 File Offset: 0x00011628
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(float radius, float angle, float roundness)
		{
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0001342A File Offset: 0x0001162A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(float radius, float angle, float roundness, GradientFill fill)
		{
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0001342C File Offset: 0x0001162C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(int sideCount)
		{
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0001342E File Offset: 0x0001162E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(int sideCount, GradientFill fill)
		{
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00013430 File Offset: 0x00011630
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(int sideCount, float radius)
		{
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00013432 File Offset: 0x00011632
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(int sideCount, float radius, GradientFill fill)
		{
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00013434 File Offset: 0x00011634
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(int sideCount, float radius, float angle)
		{
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00013436 File Offset: 0x00011636
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(int sideCount, float radius, float angle, GradientFill fill)
		{
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00013438 File Offset: 0x00011638
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(int sideCount, float radius, float angle, float roundness)
		{
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0001343A File Offset: 0x0001163A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFill(int sideCount, float radius, float angle, float roundness, GradientFill fill)
		{
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001343C File Offset: 0x0001163C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos)
		{
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001343E File Offset: 0x0001163E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, GradientFill fill)
		{
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00013440 File Offset: 0x00011640
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, float radius)
		{
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00013442 File Offset: 0x00011642
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, float radius, GradientFill fill)
		{
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00013444 File Offset: 0x00011644
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, float radius, float thickness)
		{
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00013446 File Offset: 0x00011646
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, float radius, float thickness, GradientFill fill)
		{
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00013448 File Offset: 0x00011648
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, float radius, float thickness, float angle)
		{
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0001344A File Offset: 0x0001164A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, float radius, float thickness, float angle, GradientFill fill)
		{
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001344C File Offset: 0x0001164C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, float radius, float thickness, float angle, float roundness)
		{
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0001344E File Offset: 0x0001164E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, float radius, float thickness, float angle, float roundness, GradientFill fill)
		{
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00013450 File Offset: 0x00011650
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount)
		{
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00013452 File Offset: 0x00011652
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, GradientFill fill)
		{
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00013454 File Offset: 0x00011654
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, float radius)
		{
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00013456 File Offset: 0x00011656
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, float radius, GradientFill fill)
		{
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00013458 File Offset: 0x00011658
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, float radius, float thickness)
		{
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0001345A File Offset: 0x0001165A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, float radius, float thickness, GradientFill fill)
		{
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0001345C File Offset: 0x0001165C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, float radius, float thickness, float angle)
		{
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001345E File Offset: 0x0001165E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, float radius, float thickness, float angle, GradientFill fill)
		{
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00013460 File Offset: 0x00011660
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, float radius, float thickness, float angle, float roundness)
		{
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00013462 File Offset: 0x00011662
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, float radius, float thickness, float angle, float roundness, GradientFill fill)
		{
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00013464 File Offset: 0x00011664
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal)
		{
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00013466 File Offset: 0x00011666
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, GradientFill fill)
		{
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00013468 File Offset: 0x00011668
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, float radius)
		{
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0001346A File Offset: 0x0001166A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, float radius, GradientFill fill)
		{
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0001346C File Offset: 0x0001166C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, float radius, float thickness)
		{
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001346E File Offset: 0x0001166E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, float radius, float thickness, GradientFill fill)
		{
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00013470 File Offset: 0x00011670
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, float radius, float thickness, float angle)
		{
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00013472 File Offset: 0x00011672
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, GradientFill fill)
		{
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00013474 File Offset: 0x00011674
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, float roundness)
		{
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00013476 File Offset: 0x00011676
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, float roundness, GradientFill fill)
		{
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00013478 File Offset: 0x00011678
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount)
		{
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001347A File Offset: 0x0001167A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, GradientFill fill)
		{
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001347C File Offset: 0x0001167C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, float radius)
		{
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0001347E File Offset: 0x0001167E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, float radius, GradientFill fill)
		{
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00013480 File Offset: 0x00011680
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness)
		{
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00013482 File Offset: 0x00011682
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, GradientFill fill)
		{
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00013484 File Offset: 0x00011684
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle)
		{
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00013486 File Offset: 0x00011686
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, GradientFill fill)
		{
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00013488 File Offset: 0x00011688
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, float roundness)
		{
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001348A File Offset: 0x0001168A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, float roundness, GradientFill fill)
		{
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001348C File Offset: 0x0001168C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot)
		{
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0001348E File Offset: 0x0001168E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, GradientFill fill)
		{
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00013490 File Offset: 0x00011690
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, float radius)
		{
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00013492 File Offset: 0x00011692
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, float radius, GradientFill fill)
		{
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00013494 File Offset: 0x00011694
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, float radius, float thickness)
		{
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00013496 File Offset: 0x00011696
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, float radius, float thickness, GradientFill fill)
		{
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00013498 File Offset: 0x00011698
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, float radius, float thickness, float angle)
		{
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001349A File Offset: 0x0001169A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, GradientFill fill)
		{
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001349C File Offset: 0x0001169C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, float roundness)
		{
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0001349E File Offset: 0x0001169E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, float roundness, GradientFill fill)
		{
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x000134A0 File Offset: 0x000116A0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount)
		{
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x000134A2 File Offset: 0x000116A2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, GradientFill fill)
		{
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x000134A4 File Offset: 0x000116A4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, float radius)
		{
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x000134A6 File Offset: 0x000116A6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, float radius, GradientFill fill)
		{
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x000134A8 File Offset: 0x000116A8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness)
		{
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x000134AA File Offset: 0x000116AA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, GradientFill fill)
		{
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x000134AC File Offset: 0x000116AC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle)
		{
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x000134AE File Offset: 0x000116AE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, GradientFill fill)
		{
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x000134B0 File Offset: 0x000116B0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, float roundness)
		{
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x000134B2 File Offset: 0x000116B2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, float roundness, GradientFill fill)
		{
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x000134B4 File Offset: 0x000116B4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill()
		{
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x000134B6 File Offset: 0x000116B6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(GradientFill fill)
		{
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x000134B8 File Offset: 0x000116B8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(float radius)
		{
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x000134BA File Offset: 0x000116BA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(float radius, GradientFill fill)
		{
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x000134BC File Offset: 0x000116BC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(float radius, float thickness)
		{
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x000134BE File Offset: 0x000116BE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(float radius, float thickness, GradientFill fill)
		{
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x000134C0 File Offset: 0x000116C0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(float radius, float thickness, float angle)
		{
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x000134C2 File Offset: 0x000116C2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(float radius, float thickness, float angle, GradientFill fill)
		{
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x000134C4 File Offset: 0x000116C4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(float radius, float thickness, float angle, float roundness)
		{
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x000134C6 File Offset: 0x000116C6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(float radius, float thickness, float angle, float roundness, GradientFill fill)
		{
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x000134C8 File Offset: 0x000116C8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(int sideCount)
		{
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x000134CA File Offset: 0x000116CA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(int sideCount, GradientFill fill)
		{
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x000134CC File Offset: 0x000116CC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(int sideCount, float radius)
		{
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x000134CE File Offset: 0x000116CE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(int sideCount, float radius, GradientFill fill)
		{
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x000134D0 File Offset: 0x000116D0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(int sideCount, float radius, float thickness)
		{
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x000134D2 File Offset: 0x000116D2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(int sideCount, float radius, float thickness, GradientFill fill)
		{
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000134D4 File Offset: 0x000116D4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(int sideCount, float radius, float thickness, float angle)
		{
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x000134D6 File Offset: 0x000116D6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(int sideCount, float radius, float thickness, float angle, GradientFill fill)
		{
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x000134D8 File Offset: 0x000116D8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(int sideCount, float radius, float thickness, float angle, float roundness)
		{
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x000134DA File Offset: 0x000116DA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFill(int sideCount, float radius, float thickness, float angle, float roundness, GradientFill fill)
		{
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x000134DC File Offset: 0x000116DC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x000134DE File Offset: 0x000116DE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x000134E0 File Offset: 0x000116E0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, float radius, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x000134E2 File Offset: 0x000116E2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, float radius, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x000134E4 File Offset: 0x000116E4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, int sideCount, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x000134E6 File Offset: 0x000116E6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, int sideCount, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x000134E8 File Offset: 0x000116E8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, int sideCount, float radius, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x000134EA File Offset: 0x000116EA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, int sideCount, float radius, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x000134EC File Offset: 0x000116EC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 normal, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x000134EE File Offset: 0x000116EE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 normal, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x000134F0 File Offset: 0x000116F0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 normal, float radius, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x000134F2 File Offset: 0x000116F2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 normal, float radius, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x000134F4 File Offset: 0x000116F4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 normal, int sideCount, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x000134F6 File Offset: 0x000116F6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 normal, int sideCount, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x000134F8 File Offset: 0x000116F8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x000134FA File Offset: 0x000116FA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x000134FC File Offset: 0x000116FC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, Quaternion rot, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x000134FE File Offset: 0x000116FE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, Quaternion rot, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00013500 File Offset: 0x00011700
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, Quaternion rot, float radius, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00013502 File Offset: 0x00011702
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, Quaternion rot, float radius, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00013504 File Offset: 0x00011704
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, Quaternion rot, int sideCount, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00013506 File Offset: 0x00011706
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, Quaternion rot, int sideCount, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00013508 File Offset: 0x00011708
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0001350A File Offset: 0x0001170A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0001350C File Offset: 0x0001170C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0001350E File Offset: 0x0001170E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00013510 File Offset: 0x00011710
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(float radius, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00013512 File Offset: 0x00011712
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(float radius, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00013514 File Offset: 0x00011714
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(int sideCount, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00013516 File Offset: 0x00011716
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(int sideCount, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00013518 File Offset: 0x00011718
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(int sideCount, float radius, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001351A File Offset: 0x0001171A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillLinear(int sideCount, float radius, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001351C File Offset: 0x0001171C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001351E File Offset: 0x0001171E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00013520 File Offset: 0x00011720
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, float radius, float thickness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00013522 File Offset: 0x00011722
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, float radius, float thickness, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00013524 File Offset: 0x00011724
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, float radius, float thickness, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00013526 File Offset: 0x00011726
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, int sideCount, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00013528 File Offset: 0x00011728
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, int sideCount, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0001352A File Offset: 0x0001172A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, int sideCount, float radius, float thickness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0001352C File Offset: 0x0001172C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, int sideCount, float radius, float thickness, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0001352E File Offset: 0x0001172E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, int sideCount, float radius, float thickness, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00013530 File Offset: 0x00011730
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00013532 File Offset: 0x00011732
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00013534 File Offset: 0x00011734
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, float radius, float thickness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00013536 File Offset: 0x00011736
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00013538 File Offset: 0x00011738
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0001353A File Offset: 0x0001173A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, int sideCount, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0001353C File Offset: 0x0001173C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, int sideCount, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0001353E File Offset: 0x0001173E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00013540 File Offset: 0x00011740
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00013542 File Offset: 0x00011742
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00013544 File Offset: 0x00011744
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00013546 File Offset: 0x00011746
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00013548 File Offset: 0x00011748
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, float radius, float thickness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0001354A File Offset: 0x0001174A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0001354C File Offset: 0x0001174C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0001354E File Offset: 0x0001174E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, int sideCount, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00013550 File Offset: 0x00011750
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, int sideCount, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00013552 File Offset: 0x00011752
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00013554 File Offset: 0x00011754
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00013556 File Offset: 0x00011756
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00013558 File Offset: 0x00011758
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0001355A File Offset: 0x0001175A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0001355C File Offset: 0x0001175C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(float radius, float thickness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0001355E File Offset: 0x0001175E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(float radius, float thickness, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00013560 File Offset: 0x00011760
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(float radius, float thickness, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00013562 File Offset: 0x00011762
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(int sideCount, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00013564 File Offset: 0x00011764
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(int sideCount, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00013566 File Offset: 0x00011766
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(int sideCount, float radius, float thickness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00013568 File Offset: 0x00011768
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(int sideCount, float radius, float thickness, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0001356A File Offset: 0x0001176A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillLinear(int sideCount, float radius, float thickness, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0001356C File Offset: 0x0001176C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0001356E File Offset: 0x0001176E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00013570 File Offset: 0x00011770
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, float radius, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00013572 File Offset: 0x00011772
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, float radius, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00013574 File Offset: 0x00011774
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, int sideCount, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00013576 File Offset: 0x00011776
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, int sideCount, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00013578 File Offset: 0x00011778
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, int sideCount, float radius, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0001357A File Offset: 0x0001177A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, int sideCount, float radius, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0001357C File Offset: 0x0001177C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 normal, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0001357E File Offset: 0x0001177E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 normal, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00013580 File Offset: 0x00011780
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 normal, float radius, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00013582 File Offset: 0x00011782
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 normal, float radius, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00013584 File Offset: 0x00011784
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 normal, int sideCount, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00013586 File Offset: 0x00011786
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 normal, int sideCount, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00013588 File Offset: 0x00011788
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0001358A File Offset: 0x0001178A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0001358C File Offset: 0x0001178C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, Quaternion rot, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0001358E File Offset: 0x0001178E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, Quaternion rot, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00013590 File Offset: 0x00011790
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, Quaternion rot, float radius, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x00013592 File Offset: 0x00011792
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, Quaternion rot, float radius, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00013594 File Offset: 0x00011794
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, Quaternion rot, int sideCount, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00013596 File Offset: 0x00011796
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, Quaternion rot, int sideCount, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00013598 File Offset: 0x00011798
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0001359A File Offset: 0x0001179A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0001359C File Offset: 0x0001179C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001359E File Offset: 0x0001179E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x000135A0 File Offset: 0x000117A0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(float radius, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x000135A2 File Offset: 0x000117A2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(float radius, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x000135A4 File Offset: 0x000117A4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(int sideCount, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x000135A6 File Offset: 0x000117A6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(int sideCount, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x000135A8 File Offset: 0x000117A8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(int sideCount, float radius, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x000135AA File Offset: 0x000117AA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RegularPolygonFillRadial(int sideCount, float radius, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x000135AC File Offset: 0x000117AC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x000135AE File Offset: 0x000117AE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x000135B0 File Offset: 0x000117B0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, float radius, float thickness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x000135B2 File Offset: 0x000117B2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, float radius, float thickness, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x000135B4 File Offset: 0x000117B4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, float radius, float thickness, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x000135B6 File Offset: 0x000117B6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, int sideCount, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x000135B8 File Offset: 0x000117B8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, int sideCount, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x000135BA File Offset: 0x000117BA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, int sideCount, float radius, float thickness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x000135BC File Offset: 0x000117BC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, int sideCount, float radius, float thickness, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x000135BE File Offset: 0x000117BE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, int sideCount, float radius, float thickness, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x000135C0 File Offset: 0x000117C0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x000135C2 File Offset: 0x000117C2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x000135C4 File Offset: 0x000117C4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, float radius, float thickness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x000135C6 File Offset: 0x000117C6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x000135C8 File Offset: 0x000117C8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x000135CA File Offset: 0x000117CA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, int sideCount, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x000135CC File Offset: 0x000117CC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, int sideCount, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x000135CE File Offset: 0x000117CE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x000135D0 File Offset: 0x000117D0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x000135D2 File Offset: 0x000117D2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x000135D4 File Offset: 0x000117D4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x000135D6 File Offset: 0x000117D6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x000135D8 File Offset: 0x000117D8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, float radius, float thickness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x000135DA File Offset: 0x000117DA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x000135DC File Offset: 0x000117DC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x000135DE File Offset: 0x000117DE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, int sideCount, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x000135E0 File Offset: 0x000117E0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, int sideCount, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x000135E2 File Offset: 0x000117E2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x000135E4 File Offset: 0x000117E4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x000135E6 File Offset: 0x000117E6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x000135E8 File Offset: 0x000117E8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x000135EA File Offset: 0x000117EA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x000135EC File Offset: 0x000117EC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(float radius, float thickness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x000135EE File Offset: 0x000117EE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(float radius, float thickness, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x000135F0 File Offset: 0x000117F0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(float radius, float thickness, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x000135F2 File Offset: 0x000117F2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(int sideCount, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x000135F4 File Offset: 0x000117F4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(int sideCount, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x000135F6 File Offset: 0x000117F6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(int sideCount, float radius, float thickness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x000135F8 File Offset: 0x000117F8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(int sideCount, float radius, float thickness, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x000135FA File Offset: 0x000117FA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead", true)]
		public static void RegularPolygonHollowFillRadial(int sideCount, float radius, float thickness, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x000135FC File Offset: 0x000117FC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Radial(...) )", true)]
		public static void DiscGradientRadial(Vector3 pos, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x000135FE File Offset: 0x000117FE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Radial(...) )", true)]
		public static void DiscGradientRadial(Vector3 pos, float radius, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00013600 File Offset: 0x00011800
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Radial(...) )", true)]
		public static void DiscGradientRadial(Vector3 pos, Vector3 normal, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00013602 File Offset: 0x00011802
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Radial(...) )", true)]
		public static void DiscGradientRadial(Vector3 pos, Vector3 normal, float radius, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00013604 File Offset: 0x00011804
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Radial(...) )", true)]
		public static void DiscGradientRadial(Vector3 pos, Quaternion rot, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00013606 File Offset: 0x00011806
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Radial(...) )", true)]
		public static void DiscGradientRadial(Vector3 pos, Quaternion rot, float radius, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00013608 File Offset: 0x00011808
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Radial(...) )", true)]
		public static void DiscGradientRadial(Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0001360A File Offset: 0x0001180A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Radial(...) )", true)]
		public static void DiscGradientRadial(float radius, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0001360C File Offset: 0x0001180C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Angular(...) )", true)]
		public static void DiscGradientAngular(Vector3 pos, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0001360E File Offset: 0x0001180E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Angular(...) )", true)]
		public static void DiscGradientAngular(Vector3 pos, float radius, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00013610 File Offset: 0x00011810
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Angular(...) )", true)]
		public static void DiscGradientAngular(Vector3 pos, Vector3 normal, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00013612 File Offset: 0x00011812
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Angular(...) )", true)]
		public static void DiscGradientAngular(Vector3 pos, Vector3 normal, float radius, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00013614 File Offset: 0x00011814
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Angular(...) )", true)]
		public static void DiscGradientAngular(Vector3 pos, Quaternion rot, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00013616 File Offset: 0x00011816
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Angular(...) )", true)]
		public static void DiscGradientAngular(Vector3 pos, Quaternion rot, float radius, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00013618 File Offset: 0x00011818
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Angular(...) )", true)]
		public static void DiscGradientAngular(Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0001361A File Offset: 0x0001181A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Angular(...) )", true)]
		public static void DiscGradientAngular(float radius, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0001361C File Offset: 0x0001181C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Bilinear(...) )", true)]
		public static void DiscGradientBilinear(Vector3 pos, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0001361E File Offset: 0x0001181E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Bilinear(...) )", true)]
		public static void DiscGradientBilinear(Vector3 pos, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00013620 File Offset: 0x00011820
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Bilinear(...) )", true)]
		public static void DiscGradientBilinear(Vector3 pos, Vector3 normal, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00013622 File Offset: 0x00011822
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Bilinear(...) )", true)]
		public static void DiscGradientBilinear(Vector3 pos, Vector3 normal, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00013624 File Offset: 0x00011824
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Bilinear(...) )", true)]
		public static void DiscGradientBilinear(Vector3 pos, Quaternion rot, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00013626 File Offset: 0x00011826
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Bilinear(...) )", true)]
		public static void DiscGradientBilinear(Vector3 pos, Quaternion rot, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00013628 File Offset: 0x00011828
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Bilinear(...) )", true)]
		public static void DiscGradientBilinear(Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0001362A File Offset: 0x0001182A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Bilinear(...) )", true)]
		public static void DiscGradientBilinear(float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0001362C File Offset: 0x0001182C
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos)
		{
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0001362E File Offset: 0x0001182E
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Color color)
		{
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00013630 File Offset: 0x00011830
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, float radius)
		{
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00013632 File Offset: 0x00011832
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, float radius, Color color)
		{
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00013634 File Offset: 0x00011834
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, float radius, float thickness)
		{
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x00013636 File Offset: 0x00011836
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, float radius, float thickness, Color color)
		{
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00013638 File Offset: 0x00011838
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, DashStyle dashStyle)
		{
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0001363A File Offset: 0x0001183A
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, DashStyle dashStyle, Color color)
		{
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0001363C File Offset: 0x0001183C
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, DashStyle dashStyle, float radius)
		{
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0001363E File Offset: 0x0001183E
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, DashStyle dashStyle, float radius, Color color)
		{
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00013640 File Offset: 0x00011840
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness)
		{
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00013642 File Offset: 0x00011842
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, Color color)
		{
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00013644 File Offset: 0x00011844
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Vector3 normal)
		{
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00013646 File Offset: 0x00011846
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Vector3 normal, Color color)
		{
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00013648 File Offset: 0x00011848
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Vector3 normal, float radius)
		{
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0001364A File Offset: 0x0001184A
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Vector3 normal, float radius, Color color)
		{
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0001364C File Offset: 0x0001184C
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Vector3 normal, float radius, float thickness)
		{
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0001364E File Offset: 0x0001184E
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Vector3 normal, float radius, float thickness, Color color)
		{
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00013650 File Offset: 0x00011850
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle)
		{
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00013652 File Offset: 0x00011852
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, Color color)
		{
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00013654 File Offset: 0x00011854
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius)
		{
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00013656 File Offset: 0x00011856
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, Color color)
		{
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00013658 File Offset: 0x00011858
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness)
		{
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0001365A File Offset: 0x0001185A
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, Color color)
		{
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0001365C File Offset: 0x0001185C
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Quaternion rot)
		{
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0001365E File Offset: 0x0001185E
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Quaternion rot, Color color)
		{
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00013660 File Offset: 0x00011860
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Quaternion rot, float radius)
		{
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00013662 File Offset: 0x00011862
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Quaternion rot, float radius, Color color)
		{
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00013664 File Offset: 0x00011864
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Quaternion rot, float radius, float thickness)
		{
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00013666 File Offset: 0x00011866
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Quaternion rot, float radius, float thickness, Color color)
		{
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00013668 File Offset: 0x00011868
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle)
		{
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0001366A File Offset: 0x0001186A
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, Color color)
		{
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0001366C File Offset: 0x0001186C
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius)
		{
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0001366E File Offset: 0x0001186E
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, Color color)
		{
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00013670 File Offset: 0x00011870
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness)
		{
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00013672 File Offset: 0x00011872
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, Color color)
		{
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00013674 File Offset: 0x00011874
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed()
		{
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00013676 File Offset: 0x00011876
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(Color color)
		{
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x00013678 File Offset: 0x00011878
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(float radius)
		{
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0001367A File Offset: 0x0001187A
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(float radius, Color color)
		{
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0001367C File Offset: 0x0001187C
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(float radius, float thickness)
		{
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0001367E File Offset: 0x0001187E
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(float radius, float thickness, Color color)
		{
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00013680 File Offset: 0x00011880
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(DashStyle dashStyle)
		{
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00013682 File Offset: 0x00011882
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(DashStyle dashStyle, Color color)
		{
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00013684 File Offset: 0x00011884
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(DashStyle dashStyle, float radius)
		{
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00013686 File Offset: 0x00011886
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(DashStyle dashStyle, float radius, Color color)
		{
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x00013688 File Offset: 0x00011888
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(DashStyle dashStyle, float radius, float thickness)
		{
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0001368A File Offset: 0x0001188A
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingDashed(DashStyle dashStyle, float radius, float thickness, Color color)
		{
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0001368C File Offset: 0x0001188C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) )", true)]
		public static void RingGradientRadial(Vector3 pos, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0001368E File Offset: 0x0001188E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) )", true)]
		public static void RingGradientRadial(Vector3 pos, float radius, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x00013690 File Offset: 0x00011890
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) )", true)]
		public static void RingGradientRadial(Vector3 pos, float radius, float thickness, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00013692 File Offset: 0x00011892
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) )", true)]
		public static void RingGradientRadial(Vector3 pos, Vector3 normal, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x00013694 File Offset: 0x00011894
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) )", true)]
		public static void RingGradientRadial(Vector3 pos, Vector3 normal, float radius, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x00013696 File Offset: 0x00011896
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) )", true)]
		public static void RingGradientRadial(Vector3 pos, Vector3 normal, float radius, float thickness, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x00013698 File Offset: 0x00011898
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) )", true)]
		public static void RingGradientRadial(Vector3 pos, Quaternion rot, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0001369A File Offset: 0x0001189A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) )", true)]
		public static void RingGradientRadial(Vector3 pos, Quaternion rot, float radius, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0001369C File Offset: 0x0001189C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) )", true)]
		public static void RingGradientRadial(Vector3 pos, Quaternion rot, float radius, float thickness, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0001369E File Offset: 0x0001189E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) )", true)]
		public static void RingGradientRadial(Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x000136A0 File Offset: 0x000118A0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) )", true)]
		public static void RingGradientRadial(float radius, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x000136A2 File Offset: 0x000118A2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) )", true)]
		public static void RingGradientRadial(float radius, float thickness, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x000136A4 File Offset: 0x000118A4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x000136A6 File Offset: 0x000118A6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, float radius, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x000136A8 File Offset: 0x000118A8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, float radius, float thickness, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x000136AA File Offset: 0x000118AA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, DashStyle dashStyle, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x000136AC File Offset: 0x000118AC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, DashStyle dashStyle, float radius, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x000136AE File Offset: 0x000118AE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x000136B0 File Offset: 0x000118B0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, Vector3 normal, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x000136B2 File Offset: 0x000118B2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, Vector3 normal, float radius, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x000136B4 File Offset: 0x000118B4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, Vector3 normal, float radius, float thickness, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x000136B6 File Offset: 0x000118B6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x000136B8 File Offset: 0x000118B8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x000136BA File Offset: 0x000118BA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x000136BC File Offset: 0x000118BC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, Quaternion rot, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x000136BE File Offset: 0x000118BE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, Quaternion rot, float radius, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x000136C0 File Offset: 0x000118C0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, Quaternion rot, float radius, float thickness, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x000136C2 File Offset: 0x000118C2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x000136C4 File Offset: 0x000118C4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x000136C6 File Offset: 0x000118C6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x000136C8 File Offset: 0x000118C8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x000136CA File Offset: 0x000118CA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(float radius, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x000136CC File Offset: 0x000118CC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(float radius, float thickness, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x000136CE File Offset: 0x000118CE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(DashStyle dashStyle, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x000136D0 File Offset: 0x000118D0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(DashStyle dashStyle, float radius, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x000136D2 File Offset: 0x000118D2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientRadialDashed(DashStyle dashStyle, float radius, float thickness, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x000136D4 File Offset: 0x000118D4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) )", true)]
		public static void RingGradientAngular(Vector3 pos, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x000136D6 File Offset: 0x000118D6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) )", true)]
		public static void RingGradientAngular(Vector3 pos, float radius, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x000136D8 File Offset: 0x000118D8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) )", true)]
		public static void RingGradientAngular(Vector3 pos, float radius, float thickness, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x000136DA File Offset: 0x000118DA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) )", true)]
		public static void RingGradientAngular(Vector3 pos, Vector3 normal, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x000136DC File Offset: 0x000118DC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) )", true)]
		public static void RingGradientAngular(Vector3 pos, Vector3 normal, float radius, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x000136DE File Offset: 0x000118DE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) )", true)]
		public static void RingGradientAngular(Vector3 pos, Vector3 normal, float radius, float thickness, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x000136E0 File Offset: 0x000118E0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) )", true)]
		public static void RingGradientAngular(Vector3 pos, Quaternion rot, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x000136E2 File Offset: 0x000118E2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) )", true)]
		public static void RingGradientAngular(Vector3 pos, Quaternion rot, float radius, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x000136E4 File Offset: 0x000118E4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) )", true)]
		public static void RingGradientAngular(Vector3 pos, Quaternion rot, float radius, float thickness, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x000136E6 File Offset: 0x000118E6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) )", true)]
		public static void RingGradientAngular(Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x000136E8 File Offset: 0x000118E8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) )", true)]
		public static void RingGradientAngular(float radius, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x000136EA File Offset: 0x000118EA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) )", true)]
		public static void RingGradientAngular(float radius, float thickness, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x000136EC File Offset: 0x000118EC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x000136EE File Offset: 0x000118EE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, float radius, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x000136F0 File Offset: 0x000118F0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, float radius, float thickness, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x000136F2 File Offset: 0x000118F2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, DashStyle dashStyle, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x000136F4 File Offset: 0x000118F4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, DashStyle dashStyle, float radius, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x000136F6 File Offset: 0x000118F6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x000136F8 File Offset: 0x000118F8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, Vector3 normal, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x000136FA File Offset: 0x000118FA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, Vector3 normal, float radius, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x000136FC File Offset: 0x000118FC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, Vector3 normal, float radius, float thickness, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x000136FE File Offset: 0x000118FE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00013700 File Offset: 0x00011900
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00013702 File Offset: 0x00011902
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00013704 File Offset: 0x00011904
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, Quaternion rot, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00013706 File Offset: 0x00011906
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, Quaternion rot, float radius, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00013708 File Offset: 0x00011908
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, Quaternion rot, float radius, float thickness, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0001370A File Offset: 0x0001190A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0001370C File Offset: 0x0001190C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0001370E File Offset: 0x0001190E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00013710 File Offset: 0x00011910
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00013712 File Offset: 0x00011912
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(float radius, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00013714 File Offset: 0x00011914
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(float radius, float thickness, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00013716 File Offset: 0x00011916
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(DashStyle dashStyle, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00013718 File Offset: 0x00011918
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(DashStyle dashStyle, float radius, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0001371A File Offset: 0x0001191A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientAngularDashed(DashStyle dashStyle, float radius, float thickness, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0001371C File Offset: 0x0001191C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) )", true)]
		public static void RingGradientBilinear(Vector3 pos, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0001371E File Offset: 0x0001191E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) )", true)]
		public static void RingGradientBilinear(Vector3 pos, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00013720 File Offset: 0x00011920
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) )", true)]
		public static void RingGradientBilinear(Vector3 pos, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00013722 File Offset: 0x00011922
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) )", true)]
		public static void RingGradientBilinear(Vector3 pos, Vector3 normal, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00013724 File Offset: 0x00011924
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) )", true)]
		public static void RingGradientBilinear(Vector3 pos, Vector3 normal, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00013726 File Offset: 0x00011926
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) )", true)]
		public static void RingGradientBilinear(Vector3 pos, Vector3 normal, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00013728 File Offset: 0x00011928
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) )", true)]
		public static void RingGradientBilinear(Vector3 pos, Quaternion rot, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001372A File Offset: 0x0001192A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) )", true)]
		public static void RingGradientBilinear(Vector3 pos, Quaternion rot, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0001372C File Offset: 0x0001192C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) )", true)]
		public static void RingGradientBilinear(Vector3 pos, Quaternion rot, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0001372E File Offset: 0x0001192E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) )", true)]
		public static void RingGradientBilinear(Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00013730 File Offset: 0x00011930
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) )", true)]
		public static void RingGradientBilinear(float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00013732 File Offset: 0x00011932
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) )", true)]
		public static void RingGradientBilinear(float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00013734 File Offset: 0x00011934
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00013736 File Offset: 0x00011936
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00013738 File Offset: 0x00011938
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0001373A File Offset: 0x0001193A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0001373C File Offset: 0x0001193C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x0001373E File Offset: 0x0001193E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00013740 File Offset: 0x00011940
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, Vector3 normal, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00013742 File Offset: 0x00011942
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, Vector3 normal, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00013744 File Offset: 0x00011944
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, Vector3 normal, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00013746 File Offset: 0x00011946
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00013748 File Offset: 0x00011948
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0001374A File Offset: 0x0001194A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0001374C File Offset: 0x0001194C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, Quaternion rot, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0001374E File Offset: 0x0001194E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, Quaternion rot, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00013750 File Offset: 0x00011950
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, Quaternion rot, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00013752 File Offset: 0x00011952
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00013754 File Offset: 0x00011954
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00013756 File Offset: 0x00011956
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00013758 File Offset: 0x00011958
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0001375A File Offset: 0x0001195A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0001375C File Offset: 0x0001195C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0001375E File Offset: 0x0001195E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(DashStyle dashStyle, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00013760 File Offset: 0x00011960
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(DashStyle dashStyle, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00013762 File Offset: 0x00011962
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void RingGradientBilinearDashed(DashStyle dashStyle, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00013764 File Offset: 0x00011964
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Radial(...) )", true)]
		public static void PieGradientRadial(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00013766 File Offset: 0x00011966
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Radial(...) )", true)]
		public static void PieGradientRadial(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00013768 File Offset: 0x00011968
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Radial(...) )", true)]
		public static void PieGradientRadial(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0001376A File Offset: 0x0001196A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Radial(...) )", true)]
		public static void PieGradientRadial(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0001376C File Offset: 0x0001196C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Radial(...) )", true)]
		public static void PieGradientRadial(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0001376E File Offset: 0x0001196E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Radial(...) )", true)]
		public static void PieGradientRadial(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00013770 File Offset: 0x00011970
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Radial(...) )", true)]
		public static void PieGradientRadial(float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00013772 File Offset: 0x00011972
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Radial(...) )", true)]
		public static void PieGradientRadial(float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00013774 File Offset: 0x00011974
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Angular(...) )", true)]
		public static void PieGradientAngular(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00013776 File Offset: 0x00011976
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Angular(...) )", true)]
		public static void PieGradientAngular(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00013778 File Offset: 0x00011978
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Angular(...) )", true)]
		public static void PieGradientAngular(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0001377A File Offset: 0x0001197A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Angular(...) )", true)]
		public static void PieGradientAngular(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0001377C File Offset: 0x0001197C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Angular(...) )", true)]
		public static void PieGradientAngular(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0001377E File Offset: 0x0001197E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Angular(...) )", true)]
		public static void PieGradientAngular(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00013780 File Offset: 0x00011980
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Angular(...) )", true)]
		public static void PieGradientAngular(float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00013782 File Offset: 0x00011982
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Angular(...) )", true)]
		public static void PieGradientAngular(float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00013784 File Offset: 0x00011984
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Bilinear(...) )", true)]
		public static void PieGradientBilinear(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x00013786 File Offset: 0x00011986
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Bilinear(...) )", true)]
		public static void PieGradientBilinear(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00013788 File Offset: 0x00011988
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Bilinear(...) )", true)]
		public static void PieGradientBilinear(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0001378A File Offset: 0x0001198A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Bilinear(...) )", true)]
		public static void PieGradientBilinear(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0001378C File Offset: 0x0001198C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Bilinear(...) )", true)]
		public static void PieGradientBilinear(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0001378E File Offset: 0x0001198E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Bilinear(...) )", true)]
		public static void PieGradientBilinear(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00013790 File Offset: 0x00011990
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Bilinear(...) )", true)]
		public static void PieGradientBilinear(float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00013792 File Offset: 0x00011992
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Bilinear(...) )", true)]
		public static void PieGradientBilinear(float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00013794 File Offset: 0x00011994
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00013796 File Offset: 0x00011996
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00013798 File Offset: 0x00011998
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0001379A File Offset: 0x0001199A
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0001379C File Offset: 0x0001199C
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0001379E File Offset: 0x0001199E
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x000137A0 File Offset: 0x000119A0
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x000137A2 File Offset: 0x000119A2
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x000137A4 File Offset: 0x000119A4
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x000137A6 File Offset: 0x000119A6
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x000137A8 File Offset: 0x000119A8
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x000137AA File Offset: 0x000119AA
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x000137AC File Offset: 0x000119AC
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x000137AE File Offset: 0x000119AE
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x000137B0 File Offset: 0x000119B0
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x000137B2 File Offset: 0x000119B2
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x000137B4 File Offset: 0x000119B4
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x000137B6 File Offset: 0x000119B6
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x000137B8 File Offset: 0x000119B8
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x000137BA File Offset: 0x000119BA
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x000137BC File Offset: 0x000119BC
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x000137BE File Offset: 0x000119BE
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x000137C0 File Offset: 0x000119C0
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x000137C2 File Offset: 0x000119C2
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x000137C4 File Offset: 0x000119C4
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x000137C6 File Offset: 0x000119C6
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x000137C8 File Offset: 0x000119C8
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x000137CA File Offset: 0x000119CA
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x000137CC File Offset: 0x000119CC
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x000137CE File Offset: 0x000119CE
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x000137D0 File Offset: 0x000119D0
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x000137D2 File Offset: 0x000119D2
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x000137D4 File Offset: 0x000119D4
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x000137D6 File Offset: 0x000119D6
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x000137D8 File Offset: 0x000119D8
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x000137DA File Offset: 0x000119DA
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x000137DC File Offset: 0x000119DC
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x000137DE File Offset: 0x000119DE
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x000137E0 File Offset: 0x000119E0
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x000137E2 File Offset: 0x000119E2
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x000137E4 File Offset: 0x000119E4
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x000137E6 File Offset: 0x000119E6
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x000137E8 File Offset: 0x000119E8
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x000137EA File Offset: 0x000119EA
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x000137EC File Offset: 0x000119EC
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x000137EE File Offset: 0x000119EE
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x000137F0 File Offset: 0x000119F0
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x000137F2 File Offset: 0x000119F2
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x000137F4 File Offset: 0x000119F4
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x000137F6 File Offset: 0x000119F6
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x000137F8 File Offset: 0x000119F8
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x000137FA File Offset: 0x000119FA
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x000137FC File Offset: 0x000119FC
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x000137FE File Offset: 0x000119FE
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00013800 File Offset: 0x00011A00
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00013802 File Offset: 0x00011A02
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00013804 File Offset: 0x00011A04
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x00013806 File Offset: 0x00011A06
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x00013808 File Offset: 0x00011A08
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0001380A File Offset: 0x00011A0A
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0001380C File Offset: 0x00011A0C
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0001380E File Offset: 0x00011A0E
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x00013810 File Offset: 0x00011A10
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x00013812 File Offset: 0x00011A12
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x00013814 File Offset: 0x00011A14
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x00013816 File Offset: 0x00011A16
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00013818 File Offset: 0x00011A18
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0001381A File Offset: 0x00011A1A
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0001381C File Offset: 0x00011A1C
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0001381E File Offset: 0x00011A1E
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00013820 File Offset: 0x00011A20
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00013822 File Offset: 0x00011A22
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00013824 File Offset: 0x00011A24
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00013826 File Offset: 0x00011A26
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00013828 File Offset: 0x00011A28
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0001382A File Offset: 0x00011A2A
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0001382C File Offset: 0x00011A2C
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(float radius, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0001382E File Offset: 0x00011A2E
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(float radius, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00013830 File Offset: 0x00011A30
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00013832 File Offset: 0x00011A32
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00013834 File Offset: 0x00011A34
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00013836 File Offset: 0x00011A36
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00013838 File Offset: 0x00011A38
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0001383A File Offset: 0x00011A3A
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0001383C File Offset: 0x00011A3C
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0001383E File Offset: 0x00011A3E
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00013840 File Offset: 0x00011A40
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00013842 File Offset: 0x00011A42
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00013844 File Offset: 0x00011A44
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00013846 File Offset: 0x00011A46
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00013848 File Offset: 0x00011A48
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0001384A File Offset: 0x00011A4A
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0001384C File Offset: 0x00011A4C
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0001384E File Offset: 0x00011A4E
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00013850 File Offset: 0x00011A50
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00013852 File Offset: 0x00011A52
		[Obsolete("As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00013854 File Offset: 0x00011A54
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00013856 File Offset: 0x00011A56
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00013858 File Offset: 0x00011A58
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0001385A File Offset: 0x00011A5A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0001385C File Offset: 0x00011A5C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0001385E File Offset: 0x00011A5E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00013860 File Offset: 0x00011A60
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00013862 File Offset: 0x00011A62
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00013864 File Offset: 0x00011A64
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00013866 File Offset: 0x00011A66
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00013868 File Offset: 0x00011A68
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0001386A File Offset: 0x00011A6A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0001386C File Offset: 0x00011A6C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0001386E File Offset: 0x00011A6E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00013870 File Offset: 0x00011A70
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00013872 File Offset: 0x00011A72
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00013874 File Offset: 0x00011A74
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00013876 File Offset: 0x00011A76
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00013878 File Offset: 0x00011A78
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0001387A File Offset: 0x00011A7A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0001387C File Offset: 0x00011A7C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0001387E File Offset: 0x00011A7E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00013880 File Offset: 0x00011A80
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00013882 File Offset: 0x00011A82
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )", true)]
		public static void ArcGradientRadial(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00013884 File Offset: 0x00011A84
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00013886 File Offset: 0x00011A86
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00013888 File Offset: 0x00011A88
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0001388A File Offset: 0x00011A8A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0001388C File Offset: 0x00011A8C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0001388E File Offset: 0x00011A8E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00013890 File Offset: 0x00011A90
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00013892 File Offset: 0x00011A92
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00013894 File Offset: 0x00011A94
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00013896 File Offset: 0x00011A96
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x00013898 File Offset: 0x00011A98
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0001389A File Offset: 0x00011A9A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0001389C File Offset: 0x00011A9C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0001389E File Offset: 0x00011A9E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x000138A0 File Offset: 0x00011AA0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x000138A2 File Offset: 0x00011AA2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x000138A4 File Offset: 0x00011AA4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x000138A6 File Offset: 0x00011AA6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x000138A8 File Offset: 0x00011AA8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x000138AA File Offset: 0x00011AAA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x000138AC File Offset: 0x00011AAC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x000138AE File Offset: 0x00011AAE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x000138B0 File Offset: 0x00011AB0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x000138B2 File Offset: 0x00011AB2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x000138B4 File Offset: 0x00011AB4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x000138B6 File Offset: 0x00011AB6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x000138B8 File Offset: 0x00011AB8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x000138BA File Offset: 0x00011ABA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x000138BC File Offset: 0x00011ABC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x000138BE File Offset: 0x00011ABE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x000138C0 File Offset: 0x00011AC0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x000138C2 File Offset: 0x00011AC2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x000138C4 File Offset: 0x00011AC4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x000138C6 File Offset: 0x00011AC6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x000138C8 File Offset: 0x00011AC8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x000138CA File Offset: 0x00011ACA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x000138CC File Offset: 0x00011ACC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x000138CE File Offset: 0x00011ACE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x000138D0 File Offset: 0x00011AD0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x000138D2 File Offset: 0x00011AD2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x000138D4 File Offset: 0x00011AD4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x000138D6 File Offset: 0x00011AD6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x000138D8 File Offset: 0x00011AD8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x000138DA File Offset: 0x00011ADA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x000138DC File Offset: 0x00011ADC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x000138DE File Offset: 0x00011ADE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x000138E0 File Offset: 0x00011AE0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x000138E2 File Offset: 0x00011AE2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientRadialDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x000138E4 File Offset: 0x00011AE4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x000138E6 File Offset: 0x00011AE6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x000138E8 File Offset: 0x00011AE8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x000138EA File Offset: 0x00011AEA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x000138EC File Offset: 0x00011AEC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x000138EE File Offset: 0x00011AEE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x000138F0 File Offset: 0x00011AF0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x000138F2 File Offset: 0x00011AF2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x000138F4 File Offset: 0x00011AF4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x000138F6 File Offset: 0x00011AF6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x000138F8 File Offset: 0x00011AF8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x000138FA File Offset: 0x00011AFA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x000138FC File Offset: 0x00011AFC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x000138FE File Offset: 0x00011AFE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x00013900 File Offset: 0x00011B00
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00013902 File Offset: 0x00011B02
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00013904 File Offset: 0x00011B04
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x00013906 File Offset: 0x00011B06
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x00013908 File Offset: 0x00011B08
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0001390A File Offset: 0x00011B0A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0001390C File Offset: 0x00011B0C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0001390E File Offset: 0x00011B0E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00013910 File Offset: 0x00011B10
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00013912 File Offset: 0x00011B12
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )", true)]
		public static void ArcGradientAngular(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00013914 File Offset: 0x00011B14
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00013916 File Offset: 0x00011B16
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00013918 File Offset: 0x00011B18
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0001391A File Offset: 0x00011B1A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0001391C File Offset: 0x00011B1C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0001391E File Offset: 0x00011B1E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00013920 File Offset: 0x00011B20
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00013922 File Offset: 0x00011B22
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00013924 File Offset: 0x00011B24
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00013926 File Offset: 0x00011B26
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00013928 File Offset: 0x00011B28
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0001392A File Offset: 0x00011B2A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0001392C File Offset: 0x00011B2C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0001392E File Offset: 0x00011B2E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00013930 File Offset: 0x00011B30
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x00013932 File Offset: 0x00011B32
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00013934 File Offset: 0x00011B34
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x00013936 File Offset: 0x00011B36
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00013938 File Offset: 0x00011B38
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0001393A File Offset: 0x00011B3A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0001393C File Offset: 0x00011B3C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0001393E File Offset: 0x00011B3E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x00013940 File Offset: 0x00011B40
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x00013942 File Offset: 0x00011B42
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00013944 File Offset: 0x00011B44
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x00013946 File Offset: 0x00011B46
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x00013948 File Offset: 0x00011B48
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0001394A File Offset: 0x00011B4A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0001394C File Offset: 0x00011B4C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0001394E File Offset: 0x00011B4E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00013950 File Offset: 0x00011B50
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x00013952 File Offset: 0x00011B52
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00013954 File Offset: 0x00011B54
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x00013956 File Offset: 0x00011B56
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00013958 File Offset: 0x00011B58
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0001395A File Offset: 0x00011B5A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0001395C File Offset: 0x00011B5C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0001395E File Offset: 0x00011B5E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00013960 File Offset: 0x00011B60
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00013962 File Offset: 0x00011B62
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00013964 File Offset: 0x00011B64
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00013966 File Offset: 0x00011B66
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00013968 File Offset: 0x00011B68
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0001396A File Offset: 0x00011B6A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0001396C File Offset: 0x00011B6C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0001396E File Offset: 0x00011B6E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x00013970 File Offset: 0x00011B70
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x00013972 File Offset: 0x00011B72
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientAngularDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x00013974 File Offset: 0x00011B74
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x00013976 File Offset: 0x00011B76
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x00013978 File Offset: 0x00011B78
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0001397A File Offset: 0x00011B7A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0001397C File Offset: 0x00011B7C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0001397E File Offset: 0x00011B7E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x00013980 File Offset: 0x00011B80
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00013982 File Offset: 0x00011B82
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x00013984 File Offset: 0x00011B84
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x00013986 File Offset: 0x00011B86
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x00013988 File Offset: 0x00011B88
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0001398A File Offset: 0x00011B8A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0001398C File Offset: 0x00011B8C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0001398E File Offset: 0x00011B8E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00013990 File Offset: 0x00011B90
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00013992 File Offset: 0x00011B92
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00013994 File Offset: 0x00011B94
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00013996 File Offset: 0x00011B96
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x00013998 File Offset: 0x00011B98
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0001399A File Offset: 0x00011B9A
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0001399C File Offset: 0x00011B9C
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0001399E File Offset: 0x00011B9E
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x000139A0 File Offset: 0x00011BA0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x000139A2 File Offset: 0x00011BA2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )", true)]
		public static void ArcGradientBilinear(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x000139A4 File Offset: 0x00011BA4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x000139A6 File Offset: 0x00011BA6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x000139A8 File Offset: 0x00011BA8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x000139AA File Offset: 0x00011BAA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x000139AC File Offset: 0x00011BAC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x000139AE File Offset: 0x00011BAE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x000139B0 File Offset: 0x00011BB0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x000139B2 File Offset: 0x00011BB2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x000139B4 File Offset: 0x00011BB4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x000139B6 File Offset: 0x00011BB6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x000139B8 File Offset: 0x00011BB8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x000139BA File Offset: 0x00011BBA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x000139BC File Offset: 0x00011BBC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x000139BE File Offset: 0x00011BBE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x000139C0 File Offset: 0x00011BC0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x000139C2 File Offset: 0x00011BC2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x000139C4 File Offset: 0x00011BC4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x000139C6 File Offset: 0x00011BC6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x000139C8 File Offset: 0x00011BC8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x000139CA File Offset: 0x00011BCA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x000139CC File Offset: 0x00011BCC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x000139CE File Offset: 0x00011BCE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x000139D0 File Offset: 0x00011BD0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x000139D2 File Offset: 0x00011BD2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x000139D4 File Offset: 0x00011BD4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x000139D6 File Offset: 0x00011BD6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x000139D8 File Offset: 0x00011BD8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x000139DA File Offset: 0x00011BDA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x000139DC File Offset: 0x00011BDC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x000139DE File Offset: 0x00011BDE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x000139E0 File Offset: 0x00011BE0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x000139E2 File Offset: 0x00011BE2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x000139E4 File Offset: 0x00011BE4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x000139E6 File Offset: 0x00011BE6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x000139E8 File Offset: 0x00011BE8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x000139EA File Offset: 0x00011BEA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x000139EC File Offset: 0x00011BEC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x000139EE File Offset: 0x00011BEE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x000139F0 File Offset: 0x00011BF0
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x000139F2 File Offset: 0x00011BF2
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x000139F4 File Offset: 0x00011BF4
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x000139F6 File Offset: 0x00011BF6
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x000139F8 File Offset: 0x00011BF8
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x000139FA File Offset: 0x00011BFA
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x000139FC File Offset: 0x00011BFC
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x000139FE File Offset: 0x00011BFE
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x00013A00 File Offset: 0x00011C00
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x00013A02 File Offset: 0x00011C02
		[Obsolete("As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) ). In addition: As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle", true)]
		public static void ArcGradientBilinearDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x00013A04 File Offset: 0x00011C04
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Rect rect)
		{
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x00013A06 File Offset: 0x00011C06
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Rect rect, GradientFill fill)
		{
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x00013A08 File Offset: 0x00011C08
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Rect rect, float cornerRadius)
		{
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x00013A0A File Offset: 0x00011C0A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Rect rect, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x00013A0C File Offset: 0x00011C0C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Rect rect, Vector4 cornerRadii)
		{
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x00013A0E File Offset: 0x00011C0E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Rect rect, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x00013A10 File Offset: 0x00011C10
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Rect rect)
		{
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x00013A12 File Offset: 0x00011C12
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Rect rect, GradientFill fill)
		{
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x00013A14 File Offset: 0x00011C14
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Rect rect, float cornerRadius)
		{
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x00013A16 File Offset: 0x00011C16
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Rect rect, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x00013A18 File Offset: 0x00011C18
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Rect rect, Vector4 cornerRadii)
		{
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x00013A1A File Offset: 0x00011C1A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Rect rect, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x00013A1C File Offset: 0x00011C1C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Rect rect)
		{
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00013A1E File Offset: 0x00011C1E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Rect rect, GradientFill fill)
		{
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x00013A20 File Offset: 0x00011C20
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Rect rect, float cornerRadius)
		{
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x00013A22 File Offset: 0x00011C22
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Rect rect, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00013A24 File Offset: 0x00011C24
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Rect rect, Vector4 cornerRadii)
		{
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x00013A26 File Offset: 0x00011C26
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Rect rect, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x00013A28 File Offset: 0x00011C28
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector2 size)
		{
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x00013A2A File Offset: 0x00011C2A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector2 size, GradientFill fill)
		{
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x00013A2C File Offset: 0x00011C2C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector2 size, float cornerRadius)
		{
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x00013A2E File Offset: 0x00011C2E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector2 size, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x00013A30 File Offset: 0x00011C30
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector2 size, Vector4 cornerRadii)
		{
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x00013A32 File Offset: 0x00011C32
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector2 size, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x00013A34 File Offset: 0x00011C34
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, float width, float height)
		{
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x00013A36 File Offset: 0x00011C36
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, float width, float height, GradientFill fill)
		{
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00013A38 File Offset: 0x00011C38
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, float width, float height, float cornerRadius)
		{
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x00013A3A File Offset: 0x00011C3A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, float width, float height, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00013A3C File Offset: 0x00011C3C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, float width, float height, Vector4 cornerRadii)
		{
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x00013A3E File Offset: 0x00011C3E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, float width, float height, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x00013A40 File Offset: 0x00011C40
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size)
		{
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00013A42 File Offset: 0x00011C42
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, GradientFill fill)
		{
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00013A44 File Offset: 0x00011C44
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, float cornerRadius)
		{
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00013A46 File Offset: 0x00011C46
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00013A48 File Offset: 0x00011C48
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, Vector4 cornerRadii)
		{
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00013A4A File Offset: 0x00011C4A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x00013A4C File Offset: 0x00011C4C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height)
		{
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x00013A4E File Offset: 0x00011C4E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, GradientFill fill)
		{
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x00013A50 File Offset: 0x00011C50
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, float cornerRadius)
		{
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00013A52 File Offset: 0x00011C52
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00013A54 File Offset: 0x00011C54
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, Vector4 cornerRadii)
		{
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x00013A56 File Offset: 0x00011C56
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x00013A58 File Offset: 0x00011C58
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size)
		{
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00013A5A File Offset: 0x00011C5A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, GradientFill fill)
		{
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x00013A5C File Offset: 0x00011C5C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, float cornerRadius)
		{
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x00013A5E File Offset: 0x00011C5E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00013A60 File Offset: 0x00011C60
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, Vector4 cornerRadii)
		{
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x00013A62 File Offset: 0x00011C62
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x00013A64 File Offset: 0x00011C64
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height)
		{
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x00013A66 File Offset: 0x00011C66
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, GradientFill fill)
		{
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x00013A68 File Offset: 0x00011C68
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, float cornerRadius)
		{
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x00013A6A File Offset: 0x00011C6A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x00013A6C File Offset: 0x00011C6C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, Vector4 cornerRadii)
		{
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x00013A6E File Offset: 0x00011C6E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x00013A70 File Offset: 0x00011C70
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Rect rect)
		{
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00013A72 File Offset: 0x00011C72
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Rect rect, GradientFill fill)
		{
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00013A74 File Offset: 0x00011C74
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Rect rect, float cornerRadius)
		{
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00013A76 File Offset: 0x00011C76
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Rect rect, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00013A78 File Offset: 0x00011C78
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Rect rect, Vector4 cornerRadii)
		{
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00013A7A File Offset: 0x00011C7A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Rect rect, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x00013A7C File Offset: 0x00011C7C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector2 size, RectPivot pivot)
		{
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x00013A7E File Offset: 0x00011C7E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector2 size, RectPivot pivot, GradientFill fill)
		{
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x00013A80 File Offset: 0x00011C80
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector2 size, RectPivot pivot, float cornerRadius)
		{
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x00013A82 File Offset: 0x00011C82
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector2 size, RectPivot pivot, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x00013A84 File Offset: 0x00011C84
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector2 size, RectPivot pivot, Vector4 cornerRadii)
		{
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x00013A86 File Offset: 0x00011C86
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector2 size, RectPivot pivot, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x00013A88 File Offset: 0x00011C88
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, float width, float height, RectPivot pivot)
		{
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x00013A8A File Offset: 0x00011C8A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, float width, float height, RectPivot pivot, GradientFill fill)
		{
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00013A8C File Offset: 0x00011C8C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, float width, float height, RectPivot pivot, float cornerRadius)
		{
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00013A8E File Offset: 0x00011C8E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, float width, float height, RectPivot pivot, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x00013A90 File Offset: 0x00011C90
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, float width, float height, RectPivot pivot, Vector4 cornerRadii)
		{
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x00013A92 File Offset: 0x00011C92
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, float width, float height, RectPivot pivot, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x00013A94 File Offset: 0x00011C94
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot)
		{
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x00013A96 File Offset: 0x00011C96
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, GradientFill fill)
		{
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00013A98 File Offset: 0x00011C98
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float cornerRadius)
		{
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00013A9A File Offset: 0x00011C9A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00013A9C File Offset: 0x00011C9C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, Vector4 cornerRadii)
		{
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x00013A9E File Offset: 0x00011C9E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x00013AA0 File Offset: 0x00011CA0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot)
		{
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00013AA2 File Offset: 0x00011CA2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, GradientFill fill)
		{
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x00013AA4 File Offset: 0x00011CA4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float cornerRadius)
		{
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00013AA6 File Offset: 0x00011CA6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x00013AA8 File Offset: 0x00011CA8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, Vector4 cornerRadii)
		{
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x00013AAA File Offset: 0x00011CAA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x00013AAC File Offset: 0x00011CAC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot)
		{
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x00013AAE File Offset: 0x00011CAE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, GradientFill fill)
		{
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x00013AB0 File Offset: 0x00011CB0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float cornerRadius)
		{
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x00013AB2 File Offset: 0x00011CB2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x00013AB4 File Offset: 0x00011CB4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, Vector4 cornerRadii)
		{
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00013AB6 File Offset: 0x00011CB6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x00013AB8 File Offset: 0x00011CB8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot)
		{
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x00013ABA File Offset: 0x00011CBA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, GradientFill fill)
		{
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x00013ABC File Offset: 0x00011CBC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float cornerRadius)
		{
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x00013ABE File Offset: 0x00011CBE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x00013AC0 File Offset: 0x00011CC0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, Vector4 cornerRadii)
		{
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x00013AC2 File Offset: 0x00011CC2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x00013AC4 File Offset: 0x00011CC4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Rect rect, float thickness)
		{
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x00013AC6 File Offset: 0x00011CC6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Rect rect, float thickness, GradientFill fill)
		{
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x00013AC8 File Offset: 0x00011CC8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Rect rect, float thickness, float cornerRadius)
		{
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x00013ACA File Offset: 0x00011CCA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Rect rect, float thickness, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x00013ACC File Offset: 0x00011CCC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Rect rect, float thickness, Vector4 cornerRadii)
		{
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x00013ACE File Offset: 0x00011CCE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Rect rect, float thickness, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x00013AD0 File Offset: 0x00011CD0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Rect rect, float thickness)
		{
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x00013AD2 File Offset: 0x00011CD2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Rect rect, float thickness, GradientFill fill)
		{
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x00013AD4 File Offset: 0x00011CD4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Rect rect, float thickness, float cornerRadius)
		{
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x00013AD6 File Offset: 0x00011CD6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Rect rect, float thickness, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x00013AD8 File Offset: 0x00011CD8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Rect rect, float thickness, Vector4 cornerRadii)
		{
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00013ADA File Offset: 0x00011CDA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Rect rect, float thickness, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x00013ADC File Offset: 0x00011CDC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Rect rect, float thickness)
		{
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x00013ADE File Offset: 0x00011CDE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Rect rect, float thickness, GradientFill fill)
		{
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x00013AE0 File Offset: 0x00011CE0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Rect rect, float thickness, float cornerRadius)
		{
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x00013AE2 File Offset: 0x00011CE2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Rect rect, float thickness, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x00013AE4 File Offset: 0x00011CE4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Rect rect, float thickness, Vector4 cornerRadii)
		{
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x00013AE6 File Offset: 0x00011CE6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Rect rect, float thickness, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x00013AE8 File Offset: 0x00011CE8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, float thickness)
		{
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x00013AEA File Offset: 0x00011CEA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, float thickness, GradientFill fill)
		{
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x00013AEC File Offset: 0x00011CEC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, float thickness, float cornerRadius)
		{
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00013AEE File Offset: 0x00011CEE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, float thickness, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x00013AF0 File Offset: 0x00011CF0
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, float thickness, Vector4 cornerRadii)
		{
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x00013AF2 File Offset: 0x00011CF2
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, float thickness, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x00013AF4 File Offset: 0x00011CF4
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, float width, float height, float thickness)
		{
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x00013AF6 File Offset: 0x00011CF6
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, float width, float height, float thickness, GradientFill fill)
		{
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x00013AF8 File Offset: 0x00011CF8
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, float width, float height, float thickness, float cornerRadius)
		{
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x00013AFA File Offset: 0x00011CFA
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, float width, float height, float thickness, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x00013AFC File Offset: 0x00011CFC
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, float width, float height, float thickness, Vector4 cornerRadii)
		{
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x00013AFE File Offset: 0x00011CFE
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, float width, float height, float thickness, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x00013B00 File Offset: 0x00011D00
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, float thickness)
		{
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x00013B02 File Offset: 0x00011D02
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, float thickness, GradientFill fill)
		{
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x00013B04 File Offset: 0x00011D04
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, float thickness, float cornerRadius)
		{
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x00013B06 File Offset: 0x00011D06
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, float thickness, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00013B08 File Offset: 0x00011D08
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, float thickness, Vector4 cornerRadii)
		{
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00013B0A File Offset: 0x00011D0A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, float thickness, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00013B0C File Offset: 0x00011D0C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, float thickness)
		{
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00013B0E File Offset: 0x00011D0E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, float thickness, GradientFill fill)
		{
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x00013B10 File Offset: 0x00011D10
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, float thickness, float cornerRadius)
		{
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x00013B12 File Offset: 0x00011D12
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, float thickness, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x00013B14 File Offset: 0x00011D14
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, float thickness, Vector4 cornerRadii)
		{
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00013B16 File Offset: 0x00011D16
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, float thickness, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00013B18 File Offset: 0x00011D18
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, float thickness)
		{
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00013B1A File Offset: 0x00011D1A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, float thickness, GradientFill fill)
		{
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00013B1C File Offset: 0x00011D1C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, float thickness, float cornerRadius)
		{
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x00013B1E File Offset: 0x00011D1E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, float thickness, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x00013B20 File Offset: 0x00011D20
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, float thickness, Vector4 cornerRadii)
		{
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00013B22 File Offset: 0x00011D22
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, float thickness, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x00013B24 File Offset: 0x00011D24
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, float thickness)
		{
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00013B26 File Offset: 0x00011D26
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, float thickness, GradientFill fill)
		{
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x00013B28 File Offset: 0x00011D28
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, float thickness, float cornerRadius)
		{
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x00013B2A File Offset: 0x00011D2A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, float thickness, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00013B2C File Offset: 0x00011D2C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, float thickness, Vector4 cornerRadii)
		{
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00013B2E File Offset: 0x00011D2E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, float thickness, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00013B30 File Offset: 0x00011D30
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Rect rect, float thickness)
		{
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00013B32 File Offset: 0x00011D32
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Rect rect, float thickness, GradientFill fill)
		{
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00013B34 File Offset: 0x00011D34
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Rect rect, float thickness, float cornerRadius)
		{
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x00013B36 File Offset: 0x00011D36
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Rect rect, float thickness, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00013B38 File Offset: 0x00011D38
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Rect rect, float thickness, Vector4 cornerRadii)
		{
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00013B3A File Offset: 0x00011D3A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Rect rect, float thickness, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x00013B3C File Offset: 0x00011D3C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, RectPivot pivot, float thickness)
		{
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00013B3E File Offset: 0x00011D3E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, GradientFill fill)
		{
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00013B40 File Offset: 0x00011D40
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, float cornerRadius)
		{
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x00013B42 File Offset: 0x00011D42
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x00013B44 File Offset: 0x00011D44
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00013B46 File Offset: 0x00011D46
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00013B48 File Offset: 0x00011D48
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, float width, float height, RectPivot pivot, float thickness)
		{
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x00013B4A File Offset: 0x00011D4A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, float width, float height, RectPivot pivot, float thickness, GradientFill fill)
		{
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00013B4C File Offset: 0x00011D4C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, float width, float height, RectPivot pivot, float thickness, float cornerRadius)
		{
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x00013B4E File Offset: 0x00011D4E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, float width, float height, RectPivot pivot, float thickness, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x00013B50 File Offset: 0x00011D50
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00013B52 File Offset: 0x00011D52
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00013B54 File Offset: 0x00011D54
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness)
		{
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00013B56 File Offset: 0x00011D56
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, GradientFill fill)
		{
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x00013B58 File Offset: 0x00011D58
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, float cornerRadius)
		{
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x00013B5A File Offset: 0x00011D5A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x00013B5C File Offset: 0x00011D5C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x00013B5E File Offset: 0x00011D5E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x00013B60 File Offset: 0x00011D60
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness)
		{
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00013B62 File Offset: 0x00011D62
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, GradientFill fill)
		{
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00013B64 File Offset: 0x00011D64
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, float cornerRadius)
		{
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00013B66 File Offset: 0x00011D66
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x00013B68 File Offset: 0x00011D68
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00013B6A File Offset: 0x00011D6A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00013B6C File Offset: 0x00011D6C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness)
		{
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x00013B6E File Offset: 0x00011D6E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, GradientFill fill)
		{
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00013B70 File Offset: 0x00011D70
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, float cornerRadius)
		{
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x00013B72 File Offset: 0x00011D72
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00013B74 File Offset: 0x00011D74
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x00013B76 File Offset: 0x00011D76
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x00013B78 File Offset: 0x00011D78
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness)
		{
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x00013B7A File Offset: 0x00011D7A
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, GradientFill fill)
		{
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x00013B7C File Offset: 0x00011D7C
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, float cornerRadius)
		{
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x00013B7E File Offset: 0x00011D7E
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, float cornerRadius, GradientFill fill)
		{
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x00013B80 File Offset: 0x00011D80
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x00013B82 File Offset: 0x00011D82
		[Obsolete("As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill", true)]
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii, GradientFill fill)
		{
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x00013B84 File Offset: 0x00011D84
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.TriangleBorder instead", true)]
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c)
		{
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x00013B86 File Offset: 0x00011D86
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.TriangleBorder instead", true)]
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c, Color color)
		{
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x00013B88 File Offset: 0x00011D88
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.TriangleBorder instead", true)]
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c, Color colorA, Color colorB, Color colorC)
		{
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x00013B8A File Offset: 0x00011D8A
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.TriangleBorder instead", true)]
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c, float thickness)
		{
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x00013B8C File Offset: 0x00011D8C
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.TriangleBorder instead", true)]
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c, float thickness, Color color)
		{
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00013B8E File Offset: 0x00011D8E
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.TriangleBorder instead", true)]
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c, float thickness, Color colorA, Color colorB, Color colorC)
		{
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00013B90 File Offset: 0x00011D90
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.TriangleBorder instead", true)]
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c, float thickness, float roundness)
		{
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x00013B92 File Offset: 0x00011D92
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.TriangleBorder instead", true)]
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c, float thickness, float roundness, Color color)
		{
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x00013B94 File Offset: 0x00011D94
		[Obsolete("For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.TriangleBorder instead", true)]
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c, float thickness, float roundness, Color colorA, Color colorB, Color colorC)
		{
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x00013B98 File Offset: 0x00011D98
		static Draw()
		{
			Draw.ResetAllDrawStates();
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00013C54 File Offset: 0x00011E54
		public static void ResetAllDrawStates()
		{
			Draw.ResetMatrix();
			Draw.ResetStyle();
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000A23 RID: 2595 RVA: 0x00013C60 File Offset: 0x00011E60
		public static StateStack Scope
		{
			get
			{
				return new StateStack(Draw.style, Draw.matrix);
			}
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x00013C71 File Offset: 0x00011E71
		[MethodImpl(256)]
		public static void Push()
		{
			StateStack.Push(Draw.style, Draw.matrix);
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x00013C82 File Offset: 0x00011E82
		[MethodImpl(256)]
		public static void Pop()
		{
			StateStack.Pop();
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x00013C89 File Offset: 0x00011E89
		// (set) Token: 0x06000A27 RID: 2599 RVA: 0x00013C90 File Offset: 0x00011E90
		public static Matrix4x4 Matrix
		{
			[MethodImpl(256)]
			get
			{
				return Draw.matrix;
			}
			[MethodImpl(256)]
			set
			{
				Draw.matrix = value;
			}
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x00013C98 File Offset: 0x00011E98
		[MethodImpl(256)]
		public static void ResetMatrix()
		{
			Draw.matrix = Matrix4x4.identity;
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000A29 RID: 2601 RVA: 0x00013CA4 File Offset: 0x00011EA4
		public static MatrixStack MatrixScope
		{
			get
			{
				return new MatrixStack(Draw.Matrix);
			}
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x00013CB0 File Offset: 0x00011EB0
		[MethodImpl(256)]
		public static void PushMatrix()
		{
			MatrixStack.Push(Draw.Matrix);
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x00013CBC File Offset: 0x00011EBC
		[MethodImpl(256)]
		public static void PopMatrix()
		{
			MatrixStack.Pop();
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x00013CC3 File Offset: 0x00011EC3
		[MethodImpl(256)]
		public static void ApplyMatrix(Matrix4x4 matrix)
		{
			Draw.Matrix *= matrix;
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000A2D RID: 2605 RVA: 0x00013CD5 File Offset: 0x00011ED5
		// (set) Token: 0x06000A2E RID: 2606 RVA: 0x00013CFA File Offset: 0x00011EFA
		public static Vector3 Position
		{
			get
			{
				return new Vector3(Draw.matrix.m03, Draw.matrix.m13, Draw.matrix.m23);
			}
			set
			{
				Draw.matrix.m03 = value.x;
				Draw.matrix.m13 = value.y;
				Draw.matrix.m23 = value.z;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x00013D2C File Offset: 0x00011F2C
		// (set) Token: 0x06000A30 RID: 2608 RVA: 0x00013D47 File Offset: 0x00011F47
		public static Vector2 Position2D
		{
			get
			{
				return new Vector2(Draw.matrix.m03, Draw.matrix.m13);
			}
			set
			{
				Draw.matrix.m03 = value.x;
				Draw.matrix.m13 = value.y;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x00013D69 File Offset: 0x00011F69
		// (set) Token: 0x06000A32 RID: 2610 RVA: 0x00013D70 File Offset: 0x00011F70
		[Obsolete("Please use Draw.Position instead (I done messed up, did a typo, I'm sorry~)", true)]
		public static Vector3 Postition
		{
			get
			{
				return Draw.Position;
			}
			set
			{
				Draw.Position = value;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x00013D78 File Offset: 0x00011F78
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x00013D7F File Offset: 0x00011F7F
		[Obsolete("Please use Draw.Position2D instead (I done messed up, did a typo, I'm sorry~)", true)]
		public static Vector2 Postition2D
		{
			get
			{
				return Draw.Position2D;
			}
			set
			{
				Draw.Position2D = value;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x00013D87 File Offset: 0x00011F87
		// (set) Token: 0x06000A36 RID: 2614 RVA: 0x00013D93 File Offset: 0x00011F93
		public static Quaternion Rotation
		{
			get
			{
				return Draw.matrix.rotation;
			}
			set
			{
				Draw.MtxSetRotationKeepScale(ref Draw.matrix, value);
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x00013DA0 File Offset: 0x00011FA0
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x00013DB1 File Offset: 0x00011FB1
		public static float Angle2D
		{
			get
			{
				return ShapesMath.DirToAng(Draw.RightBasis);
			}
			set
			{
				Draw.MtxRotateZLhs(ref Draw.matrix, value - Draw.Angle2D);
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x00013DC4 File Offset: 0x00011FC4
		public static Vector3 Right
		{
			get
			{
				return Draw.RightBasis.normalized;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x00013DE0 File Offset: 0x00011FE0
		public static Vector3 Up
		{
			get
			{
				return Draw.UpBasis.normalized;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x00013DFC File Offset: 0x00011FFC
		public static Vector3 Forward
		{
			get
			{
				return Draw.ForwardBasis.normalized;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x00013E16 File Offset: 0x00012016
		public static Vector3 RightBasis
		{
			get
			{
				return Draw.matrix.GetColumn(0);
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x00013E28 File Offset: 0x00012028
		public static Vector3 UpBasis
		{
			get
			{
				return Draw.matrix.GetColumn(1);
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x00013E3A File Offset: 0x0001203A
		public static Vector3 ForwardBasis
		{
			get
			{
				return Draw.matrix.GetColumn(2);
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x00013E4C File Offset: 0x0001204C
		// (set) Token: 0x06000A40 RID: 2624 RVA: 0x00013E88 File Offset: 0x00012088
		public static Vector3 LocalScale
		{
			get
			{
				return new Vector3(Draw.RightBasis.magnitude, Draw.UpBasis.magnitude, Draw.ForwardBasis.magnitude);
			}
			set
			{
				float x = value.x / Draw.RightBasis.magnitude;
				float y = value.y / Draw.UpBasis.magnitude;
				float z = value.z / Draw.ForwardBasis.magnitude;
				Draw.Scale(x, y, z);
			}
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00013EDA File Offset: 0x000120DA
		[MethodImpl(256)]
		public static void Translate(float x, float y)
		{
			Draw.MtxTranslateXY(ref Draw.matrix, (double)x, (double)y);
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x00013EEA File Offset: 0x000120EA
		[MethodImpl(256)]
		public static void Translate(float x, float y, float z)
		{
			Draw.MtxTranslateXYZ(ref Draw.matrix, (double)x, (double)y, (double)z);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00013EFC File Offset: 0x000120FC
		[MethodImpl(256)]
		public static void Translate(Vector2 displacement)
		{
			Draw.Translate(displacement.x, displacement.y);
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00013F0F File Offset: 0x0001210F
		[MethodImpl(256)]
		public static void Translate(Vector3 displacement)
		{
			Draw.Translate(displacement.x, displacement.y, displacement.z);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00013F28 File Offset: 0x00012128
		[MethodImpl(256)]
		public static void Rotate(float angle)
		{
			Draw.MtxRotateZ(ref Draw.matrix, angle);
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00013F35 File Offset: 0x00012135
		[MethodImpl(256)]
		public static void Rotate(float x, float y, float z)
		{
			Draw.Rotate(Quaternion.Euler(x * 57.29578f, y * 57.29578f, z * 57.29578f));
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00013F56 File Offset: 0x00012156
		[MethodImpl(256)]
		public static void Rotate(float angle, Vector3 axis)
		{
			Draw.Rotate(Quaternion.AngleAxis(angle * 57.29578f, axis));
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00013F6A File Offset: 0x0001216A
		[MethodImpl(256)]
		public static void Rotate(Quaternion rotation)
		{
			Draw.matrix *= Matrix4x4.Rotate(rotation);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00013F81 File Offset: 0x00012181
		[MethodImpl(256)]
		public static void Scale(float uniformScale)
		{
			Draw.MtxScaleXYZ(ref Draw.matrix, (double)uniformScale, (double)uniformScale, (double)uniformScale);
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x00013F93 File Offset: 0x00012193
		[MethodImpl(256)]
		public static void Scale(float x, float y)
		{
			Draw.MtxScaleXY(ref Draw.matrix, (double)x, (double)y);
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x00013FA3 File Offset: 0x000121A3
		[MethodImpl(256)]
		public static void Scale(float x, float y, float z)
		{
			Draw.MtxScaleXYZ(ref Draw.matrix, (double)x, (double)y, (double)z);
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x00013FB5 File Offset: 0x000121B5
		[MethodImpl(256)]
		public static void Scale(Vector2 scale)
		{
			Draw.Scale(scale.x, scale.y);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x00013FC8 File Offset: 0x000121C8
		[MethodImpl(256)]
		public static void Scale(Vector3 scale)
		{
			Draw.Scale(scale.x, scale.y, scale.z);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x00013FE1 File Offset: 0x000121E1
		[MethodImpl(256)]
		public static void SetMatrix(Matrix4x4 matrix)
		{
			Draw.Matrix = matrix;
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00013FE9 File Offset: 0x000121E9
		[MethodImpl(256)]
		public static void SetMatrix(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			Draw.Matrix = Matrix4x4.TRS(position, rotation, scale);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x00013FF8 File Offset: 0x000121F8
		[MethodImpl(256)]
		public static void SetMatrix(Transform transform)
		{
			Draw.Matrix = transform.localToWorldMatrix;
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x00014008 File Offset: 0x00012208
		[MethodImpl(256)]
		private static void MtxSetRotationKeepScale(ref Matrix4x4 m, Quaternion rotation)
		{
			Matrix4x4 matrix4x = Matrix4x4.Rotate(rotation);
			float magnitude = m.GetColumn(0).magnitude;
			float magnitude2 = m.GetColumn(1).magnitude;
			float magnitude3 = m.GetColumn(2).magnitude;
			m.m00 = matrix4x.m00 * magnitude;
			m.m10 = matrix4x.m10 * magnitude;
			m.m20 = matrix4x.m20 * magnitude;
			m.m01 = matrix4x.m01 * magnitude2;
			m.m11 = matrix4x.m11 * magnitude2;
			m.m21 = matrix4x.m21 * magnitude2;
			m.m02 = matrix4x.m02 * magnitude3;
			m.m12 = matrix4x.m12 * magnitude3;
			m.m22 = matrix4x.m22 * magnitude3;
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x000140DC File Offset: 0x000122DC
		[MethodImpl(256)]
		private static void MtxRotateZLhs(ref Matrix4x4 rhs, float a)
		{
			double num = Math.Cos((double)a);
			double num2 = Math.Sin((double)a);
			double num3 = (double)rhs.m00;
			double num4 = (double)rhs.m01;
			double num5 = (double)rhs.m02;
			double num6 = (double)rhs.m03;
			rhs.m00 = (float)(num * num3 - num2 * (double)rhs.m10);
			rhs.m01 = (float)(num * num4 - num2 * (double)rhs.m11);
			rhs.m02 = (float)(num * num5 - num2 * (double)rhs.m12);
			rhs.m03 = (float)(num * num6 - num2 * (double)rhs.m13);
			rhs.m10 = (float)(num2 * num3 + num * (double)rhs.m10);
			rhs.m11 = (float)(num2 * num4 + num * (double)rhs.m11);
			rhs.m12 = (float)(num2 * num5 + num * (double)rhs.m12);
			rhs.m13 = (float)(num2 * num6 + num * (double)rhs.m13);
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x000141C0 File Offset: 0x000123C0
		[MethodImpl(256)]
		private static void MtxTranslateXYZ(ref Matrix4x4 lhs, double x, double y, double z)
		{
			lhs.m03 = (float)((double)lhs.m00 * x + (double)lhs.m01 * y + (double)lhs.m02 * z + (double)lhs.m03);
			lhs.m13 = (float)((double)lhs.m10 * x + (double)lhs.m11 * y + (double)lhs.m12 * z + (double)lhs.m13);
			lhs.m23 = (float)((double)lhs.m20 * x + (double)lhs.m21 * y + (double)lhs.m22 * z + (double)lhs.m23);
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00014254 File Offset: 0x00012454
		[MethodImpl(256)]
		private static void MtxTranslateXY(ref Matrix4x4 lhs, double x, double y)
		{
			lhs.m03 = (float)((double)lhs.m00 * x + (double)lhs.m01 * y + (double)lhs.m03);
			lhs.m13 = (float)((double)lhs.m10 * x + (double)lhs.m11 * y + (double)lhs.m13);
			lhs.m23 = (float)((double)lhs.m20 * x + (double)lhs.m21 * y + (double)lhs.m23);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x000142C8 File Offset: 0x000124C8
		[MethodImpl(256)]
		private static void MtxRotateZ(ref Matrix4x4 lhs, float a)
		{
			double num = Math.Cos((double)a);
			double num2 = Math.Sin((double)a);
			float m = lhs.m00;
			float m2 = lhs.m01;
			float m3 = lhs.m10;
			float m4 = lhs.m11;
			float m5 = lhs.m20;
			float m6 = lhs.m21;
			lhs.m00 = (float)((double)m * num + (double)m2 * num2);
			lhs.m01 = (float)((double)m * -(float)num2 + (double)m2 * num);
			lhs.m10 = (float)((double)m3 * num + (double)m4 * num2);
			lhs.m11 = (float)((double)m3 * -(float)num2 + (double)m4 * num);
			lhs.m20 = (float)((double)m5 * num + (double)m6 * num2);
			lhs.m21 = (float)((double)m5 * -(float)num2 + (double)m6 * num);
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x00014380 File Offset: 0x00012580
		[MethodImpl(256)]
		private static void MtxScaleXYZ(ref Matrix4x4 m, double x, double y, double z)
		{
			m.m00 = (float)((double)m.m00 * x);
			m.m10 = (float)((double)m.m10 * x);
			m.m20 = (float)((double)m.m20 * x);
			m.m01 = (float)((double)m.m01 * y);
			m.m11 = (float)((double)m.m11 * y);
			m.m21 = (float)((double)m.m21 * y);
			m.m02 = (float)((double)m.m02 * z);
			m.m12 = (float)((double)m.m12 * z);
			m.m22 = (float)((double)m.m22 * z);
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00014420 File Offset: 0x00012620
		[MethodImpl(256)]
		private static void MtxScaleXY(ref Matrix4x4 m, double x, double y)
		{
			m.m00 = (float)((double)m.m00 * x);
			m.m10 = (float)((double)m.m10 * x);
			m.m20 = (float)((double)m.m20 * x);
			m.m01 = (float)((double)m.m01 * y);
			m.m11 = (float)((double)m.m11 * y);
			m.m21 = (float)((double)m.m21 * y);
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0001448D File Offset: 0x0001268D
		[MethodImpl(256)]
		private static void MtxResetToXYZ(out Matrix4x4 m, float x, float y, float z)
		{
			m = Matrix4x4.identity;
			m.m03 = x;
			m.m13 = y;
			m.m23 = z;
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x000144AF File Offset: 0x000126AF
		[MethodImpl(256)]
		private static void MtxResetToXY(out Matrix4x4 m, float x, float y)
		{
			m = Matrix4x4.identity;
			m.m03 = x;
			m.m13 = y;
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x000144CA File Offset: 0x000126CA
		[MethodImpl(256)]
		private static void MtxResetToPosXYatAngle(out Matrix4x4 lhs, float x, float y, float a)
		{
			Draw.MtxResetToXY(out lhs, x, y);
			Draw.MtxResetScaleSetAngleZ(ref lhs, a);
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x000144DB File Offset: 0x000126DB
		[MethodImpl(256)]
		private static void MtxResetToPosXYatDirection(out Matrix4x4 lhs, float x, float y, Vector2 dir)
		{
			Draw.MtxResetToXY(out lhs, x, y);
			Draw.MtxResetScaleSetDirX(ref lhs, dir);
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x000144EC File Offset: 0x000126EC
		[MethodImpl(256)]
		private static void MtxResetScaleSetAngleZ(ref Matrix4x4 lhs, float a)
		{
			float num = Mathf.Cos(a);
			float num2 = Mathf.Sin(a);
			lhs.m00 = num;
			lhs.m10 = num2;
			lhs.m20 = 0f;
			lhs.m01 = -num2;
			lhs.m11 = num;
			lhs.m21 = 0f;
			lhs.m02 = 0f;
			lhs.m12 = 0f;
			lhs.m22 = 1f;
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0001455C File Offset: 0x0001275C
		[MethodImpl(256)]
		private static void MtxResetScaleSetDirX(ref Matrix4x4 lhs, Vector2 dir)
		{
			dir.Normalize();
			lhs.m00 = dir.x;
			lhs.m10 = dir.y;
			lhs.m20 = 0f;
			lhs.m01 = -dir.y;
			lhs.m11 = dir.x;
			lhs.m21 = 0f;
			lhs.m02 = 0f;
			lhs.m12 = 0f;
			lhs.m22 = 1f;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x000145D8 File Offset: 0x000127D8
		[MethodImpl(256)]
		public static void ResetStyle()
		{
			Draw.style = DrawStyle.@default;
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x000145E4 File Offset: 0x000127E4
		public static StyleStack StyleScope
		{
			get
			{
				return new StyleStack(Draw.style);
			}
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x000145F0 File Offset: 0x000127F0
		[MethodImpl(256)]
		public static void PushStyle()
		{
			StyleStack.Push(Draw.style);
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x000145FC File Offset: 0x000127FC
		[MethodImpl(256)]
		public static void PopStyle()
		{
			StyleStack.Pop();
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x00014603 File Offset: 0x00012803
		public static ColorStack ColorScope
		{
			get
			{
				return new ColorStack(Draw.style.color);
			}
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00014614 File Offset: 0x00012814
		[MethodImpl(256)]
		public static void PushColor()
		{
			ColorStack.Push(Draw.style.color);
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00014625 File Offset: 0x00012825
		[MethodImpl(256)]
		public static void PopColor()
		{
			ColorStack.Pop();
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x0001462C File Offset: 0x0001282C
		public static DashStack DashedScope()
		{
			DashStack result = new DashStack(Draw.UseDashes, Draw.DashStyle);
			Draw.UseDashes = true;
			return result;
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00014643 File Offset: 0x00012843
		public static DashStack DashedScope(DashStyle dashStyle)
		{
			DashStack result = new DashStack(Draw.UseDashes, Draw.DashStyle);
			Draw.UseDashes = true;
			Draw.DashStyle = dashStyle;
			return result;
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00014660 File Offset: 0x00012860
		public static GradientFillStack GradientFillScope()
		{
			GradientFillStack result = new GradientFillStack(Draw.UseGradientFill, Draw.GradientFill);
			Draw.UseGradientFill = true;
			return result;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00014677 File Offset: 0x00012877
		public static GradientFillStack GradientFillScope(GradientFill fill)
		{
			GradientFillStack result = new GradientFillStack(Draw.UseGradientFill, Draw.GradientFill);
			Draw.UseGradientFill = true;
			Draw.GradientFill = fill;
			return result;
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x00014694 File Offset: 0x00012894
		// (set) Token: 0x06000A6A RID: 2666 RVA: 0x000146A5 File Offset: 0x000128A5
		public static CompareFunction ZTest
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.renderState.zTest;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.renderState.zTest = value;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x000146B7 File Offset: 0x000128B7
		// (set) Token: 0x06000A6C RID: 2668 RVA: 0x000146C8 File Offset: 0x000128C8
		public static float ZOffsetFactor
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.renderState.zOffsetFactor;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.renderState.zOffsetFactor = value;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x000146DA File Offset: 0x000128DA
		// (set) Token: 0x06000A6E RID: 2670 RVA: 0x000146EB File Offset: 0x000128EB
		public static int ZOffsetUnits
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.renderState.zOffsetUnits;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.renderState.zOffsetUnits = value;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x000146FD File Offset: 0x000128FD
		// (set) Token: 0x06000A70 RID: 2672 RVA: 0x0001470E File Offset: 0x0001290E
		public static ColorWriteMask ColorMask
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.renderState.colorMask;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.renderState.colorMask = value;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x00014720 File Offset: 0x00012920
		// (set) Token: 0x06000A72 RID: 2674 RVA: 0x00014731 File Offset: 0x00012931
		public static CompareFunction StencilComp
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.renderState.stencilComp;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.renderState.stencilComp = value;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x00014743 File Offset: 0x00012943
		// (set) Token: 0x06000A74 RID: 2676 RVA: 0x00014754 File Offset: 0x00012954
		public static StencilOp StencilOpPass
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.renderState.stencilOpPass;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.renderState.stencilOpPass = value;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x00014766 File Offset: 0x00012966
		// (set) Token: 0x06000A76 RID: 2678 RVA: 0x00014777 File Offset: 0x00012977
		public static byte StencilRefID
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.renderState.stencilRefID;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.renderState.stencilRefID = value;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x00014789 File Offset: 0x00012989
		// (set) Token: 0x06000A78 RID: 2680 RVA: 0x0001479A File Offset: 0x0001299A
		public static byte StencilReadMask
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.renderState.stencilReadMask;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.renderState.stencilReadMask = value;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x000147AC File Offset: 0x000129AC
		// (set) Token: 0x06000A7A RID: 2682 RVA: 0x000147BD File Offset: 0x000129BD
		public static byte StencilWriteMask
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.renderState.stencilWriteMask;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.renderState.stencilWriteMask = value;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x000147CF File Offset: 0x000129CF
		// (set) Token: 0x06000A7C RID: 2684 RVA: 0x000147DB File Offset: 0x000129DB
		public static Color Color
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.color;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.color = value;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x000147E8 File Offset: 0x000129E8
		// (set) Token: 0x06000A7E RID: 2686 RVA: 0x000147F4 File Offset: 0x000129F4
		public static float Opacity
		{
			[MethodImpl(256)]
			get
			{
				return Draw.Color.a;
			}
			[MethodImpl(256)]
			set
			{
				Color color = Draw.Color;
				color.a = value;
				Draw.Color = color;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x00014815 File Offset: 0x00012A15
		// (set) Token: 0x06000A80 RID: 2688 RVA: 0x00014821 File Offset: 0x00012A21
		public static ShapesBlendMode BlendMode
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.blendMode;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.blendMode = value;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x0001482E File Offset: 0x00012A2E
		// (set) Token: 0x06000A82 RID: 2690 RVA: 0x0001483A File Offset: 0x00012A3A
		public static ScaleMode ScaleMode
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.scaleMode;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.scaleMode = value;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000A83 RID: 2691 RVA: 0x00014847 File Offset: 0x00012A47
		// (set) Token: 0x06000A84 RID: 2692 RVA: 0x00014853 File Offset: 0x00012A53
		public static DetailLevel DetailLevel
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.detailLevel;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.detailLevel = value;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x00014860 File Offset: 0x00012A60
		// (set) Token: 0x06000A86 RID: 2694 RVA: 0x0001486C File Offset: 0x00012A6C
		public static float Thickness
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.thickness;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.thickness = value;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x00014879 File Offset: 0x00012A79
		// (set) Token: 0x06000A88 RID: 2696 RVA: 0x00014885 File Offset: 0x00012A85
		public static float Radius
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.radius;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.radius = value;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x00014892 File Offset: 0x00012A92
		// (set) Token: 0x06000A8A RID: 2698 RVA: 0x0001489E File Offset: 0x00012A9E
		public static ThicknessSpace ThicknessSpace
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.thicknessSpace;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.thicknessSpace = value;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000A8B RID: 2699 RVA: 0x000148AB File Offset: 0x00012AAB
		// (set) Token: 0x06000A8C RID: 2700 RVA: 0x000148B7 File Offset: 0x00012AB7
		public static ThicknessSpace RadiusSpace
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.radiusSpace;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.radiusSpace = value;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000A8D RID: 2701 RVA: 0x000148C4 File Offset: 0x00012AC4
		// (set) Token: 0x06000A8E RID: 2702 RVA: 0x000148D0 File Offset: 0x00012AD0
		public static ThicknessSpace SizeSpace
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.sizeSpace;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.sizeSpace = value;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000A8F RID: 2703 RVA: 0x000148DD File Offset: 0x00012ADD
		// (set) Token: 0x06000A90 RID: 2704 RVA: 0x000148E9 File Offset: 0x00012AE9
		public static bool UseGradientFill
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.useGradients;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.useGradients = value;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000A91 RID: 2705 RVA: 0x000148F6 File Offset: 0x00012AF6
		// (set) Token: 0x06000A92 RID: 2706 RVA: 0x00014902 File Offset: 0x00012B02
		public static GradientFill GradientFill
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.gradientFill;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.gradientFill = value;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000A93 RID: 2707 RVA: 0x0001490F File Offset: 0x00012B0F
		// (set) Token: 0x06000A94 RID: 2708 RVA: 0x00014920 File Offset: 0x00012B20
		public static FillType GradientFillType
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.gradientFill.type;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.gradientFill.type = value;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000A95 RID: 2709 RVA: 0x00014932 File Offset: 0x00012B32
		// (set) Token: 0x06000A96 RID: 2710 RVA: 0x00014943 File Offset: 0x00012B43
		public static FillSpace GradientFillSpace
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.gradientFill.space;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.gradientFill.space = value;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000A97 RID: 2711 RVA: 0x00014955 File Offset: 0x00012B55
		// (set) Token: 0x06000A98 RID: 2712 RVA: 0x00014966 File Offset: 0x00012B66
		public static Color GradientFillColorStart
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.gradientFill.colorStart;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.gradientFill.colorStart = value;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x00014978 File Offset: 0x00012B78
		// (set) Token: 0x06000A9A RID: 2714 RVA: 0x00014989 File Offset: 0x00012B89
		public static Color GradientFillColorEnd
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.gradientFill.colorEnd;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.gradientFill.colorEnd = value;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000A9B RID: 2715 RVA: 0x0001499B File Offset: 0x00012B9B
		// (set) Token: 0x06000A9C RID: 2716 RVA: 0x000149AC File Offset: 0x00012BAC
		public static Vector3 GradientFillLinearStart
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.gradientFill.linearStart;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.gradientFill.linearStart = value;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x000149BE File Offset: 0x00012BBE
		// (set) Token: 0x06000A9E RID: 2718 RVA: 0x000149CF File Offset: 0x00012BCF
		public static Vector3 GradientFillLinearEnd
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.gradientFill.linearEnd;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.gradientFill.linearEnd = value;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000A9F RID: 2719 RVA: 0x000149E1 File Offset: 0x00012BE1
		// (set) Token: 0x06000AA0 RID: 2720 RVA: 0x000149F2 File Offset: 0x00012BF2
		public static Vector3 GradientFillRadialOrigin
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.gradientFill.radialOrigin;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.gradientFill.radialOrigin = value;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x00014A04 File Offset: 0x00012C04
		// (set) Token: 0x06000AA2 RID: 2722 RVA: 0x00014A15 File Offset: 0x00012C15
		public static float GradientFillRadialRadius
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.gradientFill.radialRadius;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.gradientFill.radialRadius = value;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x00014A27 File Offset: 0x00012C27
		// (set) Token: 0x06000AA4 RID: 2724 RVA: 0x00014A33 File Offset: 0x00012C33
		public static bool UseDashes
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.useDashes;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.useDashes = value;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x00014A40 File Offset: 0x00012C40
		// (set) Token: 0x06000AA6 RID: 2726 RVA: 0x00014A4C File Offset: 0x00012C4C
		public static DashStyle DashStyle
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.dashStyle;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.dashStyle = value;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000AA7 RID: 2727 RVA: 0x00014A59 File Offset: 0x00012C59
		// (set) Token: 0x06000AA8 RID: 2728 RVA: 0x00014A6A File Offset: 0x00012C6A
		public static DashType DashType
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.dashStyle.type;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.dashStyle.type = value;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000AA9 RID: 2729 RVA: 0x00014A7C File Offset: 0x00012C7C
		// (set) Token: 0x06000AAA RID: 2730 RVA: 0x00014A8D File Offset: 0x00012C8D
		public static DashSpace DashSpace
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.dashStyle.space;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.dashStyle.space = value;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000AAB RID: 2731 RVA: 0x00014A9F File Offset: 0x00012C9F
		// (set) Token: 0x06000AAC RID: 2732 RVA: 0x00014AB0 File Offset: 0x00012CB0
		public static DashSnapping DashSnap
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.dashStyle.snap;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.dashStyle.snap = value;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x00014AC2 File Offset: 0x00012CC2
		// (set) Token: 0x06000AAE RID: 2734 RVA: 0x00014AD3 File Offset: 0x00012CD3
		public static float DashSize
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.dashStyle.size;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.dashStyle.size = value;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000AAF RID: 2735 RVA: 0x00014AE5 File Offset: 0x00012CE5
		// (set) Token: 0x06000AB0 RID: 2736 RVA: 0x00014AF6 File Offset: 0x00012CF6
		public static float DashSizeUniform
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.dashStyle.size;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.dashStyle.size = value;
				Draw.style.dashStyle.spacing = ((Draw.style.dashStyle.space == DashSpace.FixedCount) ? 0.5f : value);
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x00014B32 File Offset: 0x00012D32
		// (set) Token: 0x06000AB2 RID: 2738 RVA: 0x00014B43 File Offset: 0x00012D43
		public static float DashSpacing
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.dashStyle.spacing;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.dashStyle.spacing = value;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x00014B55 File Offset: 0x00012D55
		// (set) Token: 0x06000AB4 RID: 2740 RVA: 0x00014B66 File Offset: 0x00012D66
		public static float DashOffset
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.dashStyle.offset;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.dashStyle.offset = value;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x00014B78 File Offset: 0x00012D78
		// (set) Token: 0x06000AB6 RID: 2742 RVA: 0x00014B89 File Offset: 0x00012D89
		public static float DashShapeModifier
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.dashStyle.shapeModifier;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.dashStyle.shapeModifier = value;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x00014B9B File Offset: 0x00012D9B
		// (set) Token: 0x06000AB8 RID: 2744 RVA: 0x00014BA7 File Offset: 0x00012DA7
		public static LineEndCap LineEndCaps
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.lineEndCaps;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.lineEndCaps = value;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x00014BB4 File Offset: 0x00012DB4
		// (set) Token: 0x06000ABA RID: 2746 RVA: 0x00014BC0 File Offset: 0x00012DC0
		public static LineGeometry LineGeometry
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.lineGeometry;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.lineGeometry = value;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x00014BCD File Offset: 0x00012DCD
		// (set) Token: 0x06000ABC RID: 2748 RVA: 0x00014BD9 File Offset: 0x00012DD9
		public static PolygonTriangulation PolygonTriangulation
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.polygonTriangulation;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.polygonTriangulation = value;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x00014BE6 File Offset: 0x00012DE6
		// (set) Token: 0x06000ABE RID: 2750 RVA: 0x00014BF2 File Offset: 0x00012DF2
		public static PolylineGeometry PolylineGeometry
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.polylineGeometry;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.polylineGeometry = value;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x00014BFF File Offset: 0x00012DFF
		// (set) Token: 0x06000AC0 RID: 2752 RVA: 0x00014C0B File Offset: 0x00012E0B
		public static PolylineJoins PolylineJoins
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.polylineJoins;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.polylineJoins = value;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x00014C18 File Offset: 0x00012E18
		// (set) Token: 0x06000AC2 RID: 2754 RVA: 0x00014C24 File Offset: 0x00012E24
		public static DiscGeometry DiscGeometry
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.discGeometry;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.discGeometry = value;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x00014C31 File Offset: 0x00012E31
		// (set) Token: 0x06000AC4 RID: 2756 RVA: 0x00014C3D File Offset: 0x00012E3D
		public static int RegularPolygonSideCount
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.regularPolygonSideCount;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.regularPolygonSideCount = value;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x00014C4A File Offset: 0x00012E4A
		// (set) Token: 0x06000AC6 RID: 2758 RVA: 0x00014C56 File Offset: 0x00012E56
		public static RegularPolygonGeometry RegularPolygonGeometry
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.regularPolygonGeometry;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.regularPolygonGeometry = value;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x00014C63 File Offset: 0x00012E63
		// (set) Token: 0x06000AC8 RID: 2760 RVA: 0x00014C6F File Offset: 0x00012E6F
		public static TextStyle TextStyle
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.textStyle;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.textStyle = value;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x00014C7C File Offset: 0x00012E7C
		// (set) Token: 0x06000ACA RID: 2762 RVA: 0x00014C8D File Offset: 0x00012E8D
		public static TMP_FontAsset Font
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.textStyle.font;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.textStyle.font = value;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000ACB RID: 2763 RVA: 0x00014C9F File Offset: 0x00012E9F
		// (set) Token: 0x06000ACC RID: 2764 RVA: 0x00014CB0 File Offset: 0x00012EB0
		public static float FontSize
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.textStyle.size;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.textStyle.size = value;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x00014CC2 File Offset: 0x00012EC2
		// (set) Token: 0x06000ACE RID: 2766 RVA: 0x00014CD3 File Offset: 0x00012ED3
		public static FontStyles FontStyle
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.textStyle.style;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.textStyle.style = value;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x00014CE5 File Offset: 0x00012EE5
		// (set) Token: 0x06000AD0 RID: 2768 RVA: 0x00014CF6 File Offset: 0x00012EF6
		public static TextAlign TextAlign
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.textStyle.alignment;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.textStyle.alignment = value;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x00014D08 File Offset: 0x00012F08
		// (set) Token: 0x06000AD2 RID: 2770 RVA: 0x00014D19 File Offset: 0x00012F19
		public static float TextCharacterSpacing
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.textStyle.characterSpacing;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.textStyle.characterSpacing = value;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x00014D2B File Offset: 0x00012F2B
		// (set) Token: 0x06000AD4 RID: 2772 RVA: 0x00014D3C File Offset: 0x00012F3C
		public static float TextWordSpacing
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.textStyle.wordSpacing;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.textStyle.wordSpacing = value;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x00014D4E File Offset: 0x00012F4E
		// (set) Token: 0x06000AD6 RID: 2774 RVA: 0x00014D5F File Offset: 0x00012F5F
		public static float TextLineSpacing
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.textStyle.lineSpacing;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.textStyle.lineSpacing = value;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x00014D71 File Offset: 0x00012F71
		// (set) Token: 0x06000AD8 RID: 2776 RVA: 0x00014D82 File Offset: 0x00012F82
		public static float TextParagraphSpacing
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.textStyle.paragraphSpacing;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.textStyle.paragraphSpacing = value;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x00014D94 File Offset: 0x00012F94
		// (set) Token: 0x06000ADA RID: 2778 RVA: 0x00014DA5 File Offset: 0x00012FA5
		public static Vector4 TextMargins
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.textStyle.margins;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.textStyle.margins = value;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000ADB RID: 2779 RVA: 0x00014DB7 File Offset: 0x00012FB7
		// (set) Token: 0x06000ADC RID: 2780 RVA: 0x00014DC8 File Offset: 0x00012FC8
		public static TextWrappingModes TextWrap
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.textStyle.wrap;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.textStyle.wrap = value;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x00014DDA File Offset: 0x00012FDA
		// (set) Token: 0x06000ADE RID: 2782 RVA: 0x00014DEB File Offset: 0x00012FEB
		public static TextOverflowModes TextOverflow
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.textStyle.overflow;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.textStyle.overflow = value;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x00014DFD File Offset: 0x00012FFD
		// (set) Token: 0x06000AE0 RID: 2784 RVA: 0x00014E0E File Offset: 0x0001300E
		public static float TextCurvature
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.textStyle.curvature;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.textStyle.curvature = value;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x00014E20 File Offset: 0x00013020
		// (set) Token: 0x06000AE2 RID: 2786 RVA: 0x00014E31 File Offset: 0x00013031
		public static Vector2 TextCurvaturePivot
		{
			[MethodImpl(256)]
			get
			{
				return Draw.style.textStyle.curvaturePivot;
			}
			[MethodImpl(256)]
			set
			{
				Draw.style.textStyle.curvaturePivot = value;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x00014E43 File Offset: 0x00013043
		// (set) Token: 0x06000AE4 RID: 2788 RVA: 0x00014E4F File Offset: 0x0001304F
		[Obsolete("All shapes now use the same static Thickness property", true)]
		public static float LineThickness
		{
			get
			{
				return Draw.style.thickness;
			}
			set
			{
				Draw.style.thickness = value;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x00014E5C File Offset: 0x0001305C
		// (set) Token: 0x06000AE6 RID: 2790 RVA: 0x00014E68 File Offset: 0x00013068
		[Obsolete("All shapes now use the same static ThicknessSpace property", true)]
		public static ThicknessSpace LineThicknessSpace
		{
			get
			{
				return Draw.style.thicknessSpace;
			}
			set
			{
				Draw.style.thicknessSpace = value;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x00014E75 File Offset: 0x00013075
		// (set) Token: 0x06000AE8 RID: 2792 RVA: 0x00014E81 File Offset: 0x00013081
		[Obsolete("All shapes now use the same static DashStyle property by default, when UseDashes is enabled", true)]
		public static DashStyle LineDashStyle
		{
			get
			{
				return Draw.style.dashStyle;
			}
			set
			{
				Draw.style.dashStyle = value;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x00014E8E File Offset: 0x0001308E
		// (set) Token: 0x06000AEA RID: 2794 RVA: 0x00014E9A File Offset: 0x0001309A
		[Obsolete("All shapes now use the same static Radius property", true)]
		public static float DiscRadius
		{
			get
			{
				return Draw.style.radius;
			}
			set
			{
				Draw.style.radius = value;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x00014EA7 File Offset: 0x000130A7
		// (set) Token: 0x06000AEC RID: 2796 RVA: 0x00014EB3 File Offset: 0x000130B3
		[Obsolete("All shapes now use the same static DashStyle property by default, when UseDashes is enabled", true)]
		public static DashStyle RingDashStyle
		{
			get
			{
				return Draw.style.dashStyle;
			}
			set
			{
				Draw.style.dashStyle = value;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x00014EC0 File Offset: 0x000130C0
		// (set) Token: 0x06000AEE RID: 2798 RVA: 0x00014ECC File Offset: 0x000130CC
		[Obsolete("All shapes now use the same static GradientFill property by default. If you want to override shape fill per shape, use the draw overload with a fill input", true)]
		public static GradientFill PolygonShapeFill
		{
			get
			{
				return Draw.style.gradientFill;
			}
			set
			{
				Draw.style.gradientFill = value;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x00014ED9 File Offset: 0x000130D9
		// (set) Token: 0x06000AF0 RID: 2800 RVA: 0x00014EE5 File Offset: 0x000130E5
		[Obsolete("All shapes now use the same static GradientFill property by default. If you want to override shape fill per shape, use the draw overload with a fill input", true)]
		public static GradientFill RegularPolygonShapeFill
		{
			get
			{
				return Draw.style.gradientFill;
			}
			set
			{
				Draw.style.gradientFill = value;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x00014EF2 File Offset: 0x000130F2
		// (set) Token: 0x06000AF2 RID: 2802 RVA: 0x00014EFE File Offset: 0x000130FE
		[Obsolete("All shapes now use the same static GradientFill property by default. If you want to override shape fill per shape, use the draw overload with a fill input", true)]
		public static GradientFill RectangleShapeFill
		{
			get
			{
				return Draw.style.gradientFill;
			}
			set
			{
				Draw.style.gradientFill = value;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x00014F0B File Offset: 0x0001310B
		// (set) Token: 0x06000AF4 RID: 2804 RVA: 0x00014F17 File Offset: 0x00013117
		[Obsolete("All shapes now use the same static Thickness property", true)]
		public static float RingThickness
		{
			get
			{
				return Draw.style.thickness;
			}
			set
			{
				Draw.style.thickness = value;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x00014F24 File Offset: 0x00013124
		// (set) Token: 0x06000AF6 RID: 2806 RVA: 0x00014F30 File Offset: 0x00013130
		[Obsolete("All shapes now use the same static ThicknessSpace property", true)]
		public static ThicknessSpace RingThicknessSpace
		{
			get
			{
				return Draw.style.thicknessSpace;
			}
			set
			{
				Draw.style.thicknessSpace = value;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x00014F3D File Offset: 0x0001313D
		// (set) Token: 0x06000AF8 RID: 2808 RVA: 0x00014F49 File Offset: 0x00013149
		[Obsolete("All shapes now use the same static RadiusSpace property", true)]
		public static ThicknessSpace DiscRadiusSpace
		{
			get
			{
				return Draw.style.radiusSpace;
			}
			set
			{
				Draw.style.radiusSpace = value;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x00014F56 File Offset: 0x00013156
		// (set) Token: 0x06000AFA RID: 2810 RVA: 0x00014F62 File Offset: 0x00013162
		[Obsolete("All shapes now use the same static Radius property", true)]
		public static float RegularPolygonRadius
		{
			get
			{
				return Draw.style.radius;
			}
			set
			{
				Draw.style.radius = value;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x00014F6F File Offset: 0x0001316F
		// (set) Token: 0x06000AFC RID: 2812 RVA: 0x00014F7B File Offset: 0x0001317B
		[Obsolete("All shapes now use the same static Thickness property", true)]
		public static float RegularPolygonThickness
		{
			get
			{
				return Draw.style.thickness;
			}
			set
			{
				Draw.style.thickness = value;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x00014F88 File Offset: 0x00013188
		// (set) Token: 0x06000AFE RID: 2814 RVA: 0x00014F94 File Offset: 0x00013194
		[Obsolete("All shapes now use the same static ThicknessSpace property", true)]
		public static ThicknessSpace RegularPolygonThicknessSpace
		{
			get
			{
				return Draw.style.thicknessSpace;
			}
			set
			{
				Draw.style.thicknessSpace = value;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x00014FA1 File Offset: 0x000131A1
		// (set) Token: 0x06000B00 RID: 2816 RVA: 0x00014FAD File Offset: 0x000131AD
		[Obsolete("All shapes now use the same static RadiusSpace property", true)]
		public static ThicknessSpace RegularPolygonRadiusSpace
		{
			get
			{
				return Draw.style.radiusSpace;
			}
			set
			{
				Draw.style.radiusSpace = value;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x00014FBA File Offset: 0x000131BA
		// (set) Token: 0x06000B02 RID: 2818 RVA: 0x00014FC6 File Offset: 0x000131C6
		[Obsolete("All shapes now use the same static Thickness property", true)]
		public static float RectangleThickness
		{
			get
			{
				return Draw.style.thickness;
			}
			set
			{
				Draw.style.thickness = value;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x00014FD3 File Offset: 0x000131D3
		// (set) Token: 0x06000B04 RID: 2820 RVA: 0x00014FDF File Offset: 0x000131DF
		[Obsolete("All shapes now use the same static ThicknessSpace property", true)]
		public static ThicknessSpace RectangleThicknessSpace
		{
			get
			{
				return Draw.style.thicknessSpace;
			}
			set
			{
				Draw.style.thicknessSpace = value;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x00014FEC File Offset: 0x000131EC
		// (set) Token: 0x06000B06 RID: 2822 RVA: 0x00014FF8 File Offset: 0x000131F8
		[Obsolete("All shapes now use the same static Thickness property", true)]
		public static float TriangleThickness
		{
			get
			{
				return Draw.style.thickness;
			}
			set
			{
				Draw.style.thickness = value;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x00015005 File Offset: 0x00013205
		// (set) Token: 0x06000B08 RID: 2824 RVA: 0x00015011 File Offset: 0x00013211
		[Obsolete("All shapes now use the same static ThicknessSpace property", true)]
		public static ThicknessSpace TriangleThicknessSpace
		{
			get
			{
				return Draw.style.thicknessSpace;
			}
			set
			{
				Draw.style.thicknessSpace = value;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x0001501E File Offset: 0x0001321E
		// (set) Token: 0x06000B0A RID: 2826 RVA: 0x0001502A File Offset: 0x0001322A
		[Obsolete("All shapes now use the same static Radius property", true)]
		public static float SphereRadius
		{
			get
			{
				return Draw.style.radius;
			}
			set
			{
				Draw.style.radius = value;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x00015037 File Offset: 0x00013237
		// (set) Token: 0x06000B0C RID: 2828 RVA: 0x00015043 File Offset: 0x00013243
		[Obsolete("All shapes now use the same static RadiusSpace property", true)]
		public static ThicknessSpace SphereRadiusSpace
		{
			get
			{
				return Draw.style.radiusSpace;
			}
			set
			{
				Draw.style.radiusSpace = value;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x00015050 File Offset: 0x00013250
		// (set) Token: 0x06000B0E RID: 2830 RVA: 0x0001505C File Offset: 0x0001325C
		[Obsolete("All shapes now use the same static SizeSpace property", true)]
		public static ThicknessSpace CuboidSizeSpace
		{
			get
			{
				return Draw.style.sizeSpace;
			}
			set
			{
				Draw.style.sizeSpace = value;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x00015069 File Offset: 0x00013269
		// (set) Token: 0x06000B10 RID: 2832 RVA: 0x00015075 File Offset: 0x00013275
		[Obsolete("All shapes now use the same static ThicknessSpace property", true)]
		public static ThicknessSpace TorusThicknessSpace
		{
			get
			{
				return Draw.style.thicknessSpace;
			}
			set
			{
				Draw.style.thicknessSpace = value;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x00015082 File Offset: 0x00013282
		// (set) Token: 0x06000B12 RID: 2834 RVA: 0x0001508E File Offset: 0x0001328E
		[Obsolete("All shapes now use the same static RadiusSpace property", true)]
		public static ThicknessSpace TorusRadiusSpace
		{
			get
			{
				return Draw.style.radiusSpace;
			}
			set
			{
				Draw.style.radiusSpace = value;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x0001509B File Offset: 0x0001329B
		// (set) Token: 0x06000B14 RID: 2836 RVA: 0x000150A7 File Offset: 0x000132A7
		[Obsolete("All shapes now use the same static SizeSpace property", true)]
		public static ThicknessSpace ConeSizeSpace
		{
			get
			{
				return Draw.style.sizeSpace;
			}
			set
			{
				Draw.style.sizeSpace = value;
			}
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x000150B4 File Offset: 0x000132B4
		[CompilerGenerated]
		internal static void <Polyline_Internal>g__ApplyToMpb|7_0(MpbPolyline2D mpb, ref Draw.<>c__DisplayClass7_0 A_1)
		{
			mpb.thickness.Add(A_1.thickness);
			mpb.thicknessSpace.Add((float)A_1.thicknessSpace);
			mpb.color.Add(A_1.color.ColorSpaceAdjusted());
			mpb.alignment.Add((float)A_1.geometry);
			mpb.scaleMode.Add((float)Draw.ScaleMode);
		}

		// Token: 0x040000A3 RID: 163
		private const MethodImplOptions INLINE = 256;

		// Token: 0x040000A4 RID: 164
		private static MpbLine2D mpbLine = new MpbLine2D();

		// Token: 0x040000A5 RID: 165
		private static MpbPolyline2D mpbPolyline = new MpbPolyline2D();

		// Token: 0x040000A6 RID: 166
		private static MpbPolyline2D mpbPolylineJoins = new MpbPolyline2D();

		// Token: 0x040000A7 RID: 167
		private static MpbPolygon mpbPolygon = new MpbPolygon();

		// Token: 0x040000A8 RID: 168
		private static readonly MpbDisc mpbDisc = new MpbDisc();

		// Token: 0x040000A9 RID: 169
		private static readonly MpbRegularPolygon mpbRegularPolygon = new MpbRegularPolygon();

		// Token: 0x040000AA RID: 170
		private static readonly MpbRect mpbRect = new MpbRect();

		// Token: 0x040000AB RID: 171
		private static MpbTriangle mpbTriangle = new MpbTriangle();

		// Token: 0x040000AC RID: 172
		private static MpbQuad mpbQuad = new MpbQuad();

		// Token: 0x040000AD RID: 173
		private static readonly MpbSphere metaMpbSphere = new MpbSphere();

		// Token: 0x040000AE RID: 174
		private static readonly MpbCone mpbCone = new MpbCone();

		// Token: 0x040000AF RID: 175
		private static readonly MpbCuboid mpbCuboid = new MpbCuboid();

		// Token: 0x040000B0 RID: 176
		private static MpbTorus mpbTorus = new MpbTorus();

		// Token: 0x040000B1 RID: 177
		private static MpbText mpbText = new MpbText();

		// Token: 0x040000B2 RID: 178
		private static Draw.OnPreRenderTmpDelegate onPreRenderTmp;

		// Token: 0x040000B3 RID: 179
		private static MpbCustomMesh mpbCustomMesh = new MpbCustomMesh();

		// Token: 0x040000B4 RID: 180
		private static MpbTexture mpbTexture = new MpbTexture();

		// Token: 0x040000B5 RID: 181
		private const string OBS_DASH = "As of Shapes 4.0.0, dash state is now set using the global Draw.UseDashes and Draw.DashStyle";

		// Token: 0x040000B6 RID: 182
		private const string OBS_FILL = "As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill";

		// Token: 0x040000B7 RID: 183
		private const string OBS_REGPOLRENAME = "For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead";

		// Token: 0x040000B8 RID: 184
		private const string OBS_TRIRENAME = "For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.TriangleBorder instead";

		// Token: 0x040000B9 RID: 185
		private const string JOINER = ". In addition: ";

		// Token: 0x040000BA RID: 186
		private const string OBS_REGPOLRENAME_AND_FILL = "As of Shapes 4.0.0, color fill is now set using the global Draw.UseGradientFill and Draw.GradientFill. In addition: For consistency, this has been renamed as of Shapes 4.0.0. Please use Draw.RegularPolygonBorder instead";

		// Token: 0x040000BB RID: 187
		private const string OBS_DISC_GRADIENT_PREFIX = "As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.";

		// Token: 0x040000BC RID: 188
		private const string OBS_DISC_GRADIENT_DISC_RADIAL = "As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Radial(...) )";

		// Token: 0x040000BD RID: 189
		private const string OBS_DISC_GRADIENT_DISC_ANGULAR = "As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Angular(...) )";

		// Token: 0x040000BE RID: 190
		private const string OBS_DISC_GRADIENT_DISC_BILINEAR = "As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Disc( ..., DiscColors.Bilinear(...) )";

		// Token: 0x040000BF RID: 191
		private const string OBS_DISC_GRADIENT_RING_RADIAL = "As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Radial(...) )";

		// Token: 0x040000C0 RID: 192
		private const string OBS_DISC_GRADIENT_RING_ANGULAR = "As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Angular(...) )";

		// Token: 0x040000C1 RID: 193
		private const string OBS_DISC_GRADIENT_RING_BILINEAR = "As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Ring( ..., DiscColors.Bilinear(...) )";

		// Token: 0x040000C2 RID: 194
		private const string OBS_DISC_GRADIENT_PIE_RADIAL = "As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Radial(...) )";

		// Token: 0x040000C3 RID: 195
		private const string OBS_DISC_GRADIENT_PIE_ANGULAR = "As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Angular(...) )";

		// Token: 0x040000C4 RID: 196
		private const string OBS_DISC_GRADIENT_PIE_BILINEAR = "As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Pie( ..., DiscColors.Bilinear(...) )";

		// Token: 0x040000C5 RID: 197
		private const string OBS_DISC_GRADIENT_ARC_RADIAL = "As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Radial(...) )";

		// Token: 0x040000C6 RID: 198
		private const string OBS_DISC_GRADIENT_ARC_ANGULAR = "As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Angular(...) )";

		// Token: 0x040000C7 RID: 199
		private const string OBS_DISC_GRADIENT_ARC_BILINEAR = "As of Shapes 4.0.0, disc gradients are now defined using a DiscColors as the last parameter. Instead, please use Draw.Arc( ..., DiscColors.Bilinear(...) )";

		// Token: 0x040000C8 RID: 200
		private static Matrix4x4 matrix = Matrix4x4.identity;

		// Token: 0x040000C9 RID: 201
		internal static DrawStyle style;

		// Token: 0x0200008A RID: 138
		// (Invoke) Token: 0x06000D46 RID: 3398
		private delegate void OnPreRenderTmpDelegate(TextMeshProShapes tmp);
	}
}
