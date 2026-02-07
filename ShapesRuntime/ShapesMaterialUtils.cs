using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000076 RID: 118
	internal static class ShapesMaterialUtils
	{
		// Token: 0x06000CCA RID: 3274 RVA: 0x0001A117 File Offset: 0x00018317
		[MethodImpl(256)]
		public static ShapesMaterials GetDiscMaterial(bool hollow, bool sector)
		{
			if (hollow)
			{
				if (!sector)
				{
					return ShapesMaterialUtils.matRing;
				}
				return ShapesMaterialUtils.matRingSector;
			}
			else
			{
				if (!sector)
				{
					return ShapesMaterialUtils.matDisc;
				}
				return ShapesMaterialUtils.matCircleSector;
			}
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x0001A13C File Offset: 0x0001833C
		public static ShapesMaterials GetDiscMaterial(DiscType type)
		{
			ShapesMaterialUtils.<>c__DisplayClass80_0 CS$<>8__locals1;
			CS$<>8__locals1.type = type;
			return ShapesMaterialUtils.<GetDiscMaterial>g__Load|80_0(ref CS$<>8__locals1);
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0001A158 File Offset: 0x00018358
		public static ShapesMaterials GetRectMaterial(bool hollow, bool rounded)
		{
			if (hollow)
			{
				if (!rounded)
				{
					return ShapesMaterialUtils.matRectBorder;
				}
				return ShapesMaterialUtils.matRectBorderRounded;
			}
			else
			{
				if (!rounded)
				{
					return ShapesMaterialUtils.matRectSimple;
				}
				return ShapesMaterialUtils.matRectRounded;
			}
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0001A17A File Offset: 0x0001837A
		public static ShapesMaterials GetRectMaterial(Rectangle.RectangleType type)
		{
			switch (type)
			{
			case Rectangle.RectangleType.HardSolid:
				return ShapesMaterialUtils.matRectSimple;
			case Rectangle.RectangleType.RoundedSolid:
				return ShapesMaterialUtils.matRectRounded;
			case Rectangle.RectangleType.HardBorder:
				return ShapesMaterialUtils.matRectBorder;
			case Rectangle.RectangleType.RoundedBorder:
				return ShapesMaterialUtils.matRectBorderRounded;
			default:
				return null;
			}
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x0001A1AD File Offset: 0x000183AD
		public static ShapesMaterials GetPolylineMat(PolylineJoins join)
		{
			return ShapesMaterialUtils.matsPolyline[(int)join];
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x0001A1B6 File Offset: 0x000183B6
		public static ShapesMaterials GetPolylineJoinsMat(PolylineJoins join)
		{
			return ShapesMaterialUtils.matsPolylineJoin[(int)join];
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x0001A1BF File Offset: 0x000183BF
		public static ShapesMaterials GetLineMat(LineGeometry geometry, LineEndCap cap)
		{
			if (geometry <= LineGeometry.Billboard)
			{
				return ShapesMaterialUtils.matsLine[(int)cap];
			}
			if (geometry != LineGeometry.Volumetric3D)
			{
				throw new ArgumentOutOfRangeException("geometry", geometry, null);
			}
			return ShapesMaterialUtils.matsLine3D[(int)cap];
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x0001A89C File Offset: 0x00018A9C
		[CompilerGenerated]
		internal static ShapesMaterials <GetDiscMaterial>g__Load|80_0(ref ShapesMaterialUtils.<>c__DisplayClass80_0 A_0)
		{
			switch (A_0.type)
			{
			case DiscType.Disc:
				return ShapesMaterialUtils.matDisc;
			case DiscType.Pie:
				return ShapesMaterialUtils.matCircleSector;
			case DiscType.Ring:
				return ShapesMaterialUtils.matRing;
			case DiscType.Arc:
				return ShapesMaterialUtils.matRingSector;
			default:
				throw new IndexOutOfRangeException(string.Format("Failed to get disc material, invalid enum index of {0} ", (int)A_0.type));
			}
		}

		// Token: 0x0400029F RID: 671
		public static readonly int propZTest = Shader.PropertyToID("_ZTest");

		// Token: 0x040002A0 RID: 672
		public static readonly int propZTestTMP = Shader.PropertyToID("unity_GUIZTestMode");

		// Token: 0x040002A1 RID: 673
		public static readonly int propZOffsetFactor = Shader.PropertyToID("_ZOffsetFactor");

		// Token: 0x040002A2 RID: 674
		public static readonly int propZOffsetUnits = Shader.PropertyToID("_ZOffsetUnits");

		// Token: 0x040002A3 RID: 675
		public static readonly int propColorMask = Shader.PropertyToID("_ColorMask");

		// Token: 0x040002A4 RID: 676
		public static readonly int propStencilComp = Shader.PropertyToID("_StencilComp");

		// Token: 0x040002A5 RID: 677
		public static readonly int propStencilOpPass = Shader.PropertyToID("_StencilOpPass");

		// Token: 0x040002A6 RID: 678
		public static readonly int propStencilID = Shader.PropertyToID("_StencilID");

		// Token: 0x040002A7 RID: 679
		public static readonly int propStencilIDTMP = Shader.PropertyToID("_Stencil");

		// Token: 0x040002A8 RID: 680
		public static readonly int propStencilReadMask = Shader.PropertyToID("_StencilReadMask");

		// Token: 0x040002A9 RID: 681
		public static readonly int propStencilWriteMask = Shader.PropertyToID("_StencilWriteMask");

		// Token: 0x040002AA RID: 682
		public static readonly int propBaseColor = Shader.PropertyToID("_BaseColor");

		// Token: 0x040002AB RID: 683
		public static readonly int propColor = Shader.PropertyToID("_Color");

		// Token: 0x040002AC RID: 684
		public static readonly int propScaleMode = Shader.PropertyToID("_ScaleMode");

		// Token: 0x040002AD RID: 685
		public static readonly int propColorEnd = Shader.PropertyToID("_ColorEnd");

		// Token: 0x040002AE RID: 686
		public static readonly int propColorOuterStart = Shader.PropertyToID("_ColorOuterStart");

		// Token: 0x040002AF RID: 687
		public static readonly int propColorInnerEnd = Shader.PropertyToID("_ColorInnerEnd");

		// Token: 0x040002B0 RID: 688
		public static readonly int propColorOuterEnd = Shader.PropertyToID("_ColorOuterEnd");

		// Token: 0x040002B1 RID: 689
		public static readonly int propColorB = Shader.PropertyToID("_ColorB");

		// Token: 0x040002B2 RID: 690
		public static readonly int propColorC = Shader.PropertyToID("_ColorC");

		// Token: 0x040002B3 RID: 691
		public static readonly int propColorD = Shader.PropertyToID("_ColorD");

		// Token: 0x040002B4 RID: 692
		public static readonly int propPointStart = Shader.PropertyToID("_PointStart");

		// Token: 0x040002B5 RID: 693
		public static readonly int propPointEnd = Shader.PropertyToID("_PointEnd");

		// Token: 0x040002B6 RID: 694
		public static readonly int propA = Shader.PropertyToID("_A");

		// Token: 0x040002B7 RID: 695
		public static readonly int propB = Shader.PropertyToID("_B");

		// Token: 0x040002B8 RID: 696
		public static readonly int propC = Shader.PropertyToID("_C");

		// Token: 0x040002B9 RID: 697
		public static readonly int propD = Shader.PropertyToID("_D");

		// Token: 0x040002BA RID: 698
		public static readonly int propRect = Shader.PropertyToID("_Rect");

		// Token: 0x040002BB RID: 699
		public static readonly int propRadius = Shader.PropertyToID("_Radius");

		// Token: 0x040002BC RID: 700
		public static readonly int propCornerRadii = Shader.PropertyToID("_CornerRadii");

		// Token: 0x040002BD RID: 701
		public static readonly int propLength = Shader.PropertyToID("_Length");

		// Token: 0x040002BE RID: 702
		public static readonly int propBorder = Shader.PropertyToID("_Hollow");

		// Token: 0x040002BF RID: 703
		public static readonly int propSides = Shader.PropertyToID("_Sides");

		// Token: 0x040002C0 RID: 704
		public static readonly int propAng = Shader.PropertyToID("_Angle");

		// Token: 0x040002C1 RID: 705
		public static readonly int propRoundness = Shader.PropertyToID("_Roundness");

		// Token: 0x040002C2 RID: 706
		public static readonly int propAngStart = Shader.PropertyToID("_AngleStart");

		// Token: 0x040002C3 RID: 707
		public static readonly int propAngEnd = Shader.PropertyToID("_AngleEnd");

		// Token: 0x040002C4 RID: 708
		public static readonly int propRoundCaps = Shader.PropertyToID("_RoundCaps");

		// Token: 0x040002C5 RID: 709
		public static readonly int propThickness = Shader.PropertyToID("_Thickness");

		// Token: 0x040002C6 RID: 710
		public static readonly int propThicknessSpace = Shader.PropertyToID("_ThicknessSpace");

		// Token: 0x040002C7 RID: 711
		public static readonly int propRadiusSpace = Shader.PropertyToID("_RadiusSpace");

		// Token: 0x040002C8 RID: 712
		public static readonly int propDashSize = Shader.PropertyToID("_DashSize");

		// Token: 0x040002C9 RID: 713
		public static readonly int propDashOffset = Shader.PropertyToID("_DashOffset");

		// Token: 0x040002CA RID: 714
		public static readonly int propDashSpacing = Shader.PropertyToID("_DashSpacing");

		// Token: 0x040002CB RID: 715
		public static readonly int propDashType = Shader.PropertyToID("_DashType");

		// Token: 0x040002CC RID: 716
		public static readonly int propDashSpace = Shader.PropertyToID("_DashSpace");

		// Token: 0x040002CD RID: 717
		public static readonly int propDashSnap = Shader.PropertyToID("_DashSnap");

		// Token: 0x040002CE RID: 718
		public static readonly int propDashShapeModifier = Shader.PropertyToID("_DashShapeModifier");

		// Token: 0x040002CF RID: 719
		public static readonly int propSize = Shader.PropertyToID("_Size");

		// Token: 0x040002D0 RID: 720
		public static readonly int propSizeSpace = Shader.PropertyToID("_SizeSpace");

		// Token: 0x040002D1 RID: 721
		public static readonly int propAlignment = Shader.PropertyToID("_Alignment");

		// Token: 0x040002D2 RID: 722
		public static readonly int propFillType = Shader.PropertyToID("_FillType");

		// Token: 0x040002D3 RID: 723
		public static readonly int propFillStart = Shader.PropertyToID("_FillStart");

		// Token: 0x040002D4 RID: 724
		public static readonly int propFillEnd = Shader.PropertyToID("_FillEnd");

		// Token: 0x040002D5 RID: 725
		public static readonly int propFillSpace = Shader.PropertyToID("_FillSpace");

		// Token: 0x040002D6 RID: 726
		public static readonly int propMainTex = Shader.PropertyToID("_MainTex");

		// Token: 0x040002D7 RID: 727
		public static readonly int propUvs = Shader.PropertyToID("_Uvs");

		// Token: 0x040002D8 RID: 728
		public static readonly int propScreenParams = Shader.PropertyToID("_ScreenParams");

		// Token: 0x040002D9 RID: 729
		private static readonly ShapesMaterials matDisc = new ShapesMaterials("Disc", Array.Empty<string>());

		// Token: 0x040002DA RID: 730
		private static readonly ShapesMaterials matCircleSector = new ShapesMaterials("Disc", new string[]
		{
			"SECTOR"
		});

		// Token: 0x040002DB RID: 731
		private static readonly ShapesMaterials matRing = new ShapesMaterials("Disc", new string[]
		{
			"INNER_RADIUS"
		});

		// Token: 0x040002DC RID: 732
		private static readonly ShapesMaterials matRingSector = new ShapesMaterials("Disc", new string[]
		{
			"INNER_RADIUS",
			"SECTOR"
		});

		// Token: 0x040002DD RID: 733
		private static readonly ShapesMaterials matRectSimple = new ShapesMaterials("Rect", Array.Empty<string>());

		// Token: 0x040002DE RID: 734
		private static readonly ShapesMaterials matRectRounded = new ShapesMaterials("Rect", new string[]
		{
			"CORNER_RADIUS"
		});

		// Token: 0x040002DF RID: 735
		private static readonly ShapesMaterials matRectBorder = new ShapesMaterials("Rect", new string[]
		{
			"BORDERED"
		});

		// Token: 0x040002E0 RID: 736
		private static readonly ShapesMaterials matRectBorderRounded = new ShapesMaterials("Rect", new string[]
		{
			"CORNER_RADIUS",
			"BORDERED"
		});

		// Token: 0x040002E1 RID: 737
		public static readonly ShapesMaterials matTriangle = new ShapesMaterials("Triangle", Array.Empty<string>());

		// Token: 0x040002E2 RID: 738
		public static readonly ShapesMaterials matQuad = new ShapesMaterials("Quad", Array.Empty<string>());

		// Token: 0x040002E3 RID: 739
		public static readonly ShapesMaterials matSphere = new ShapesMaterials("Sphere", Array.Empty<string>());

		// Token: 0x040002E4 RID: 740
		public static readonly ShapesMaterials matCone = new ShapesMaterials("Cone", Array.Empty<string>());

		// Token: 0x040002E5 RID: 741
		public static readonly ShapesMaterials matCuboid = new ShapesMaterials("Cuboid", Array.Empty<string>());

		// Token: 0x040002E6 RID: 742
		public static readonly ShapesMaterials matTorus = new ShapesMaterials("Torus", Array.Empty<string>());

		// Token: 0x040002E7 RID: 743
		public static readonly ShapesMaterials matPolygon = new ShapesMaterials("Polygon", Array.Empty<string>());

		// Token: 0x040002E8 RID: 744
		public static readonly ShapesMaterials matRegularPolygon = new ShapesMaterials("Regular Polygon", Array.Empty<string>());

		// Token: 0x040002E9 RID: 745
		public static readonly ShapesMaterials matTexture = new ShapesMaterials("Texture", Array.Empty<string>());

		// Token: 0x040002EA RID: 746
		private static readonly ShapesMaterials[] matsLine = new ShapesMaterials[]
		{
			new ShapesMaterials("Line 2D", Array.Empty<string>()),
			new ShapesMaterials("Line 2D", new string[]
			{
				"CAP_SQUARE"
			}),
			new ShapesMaterials("Line 2D", new string[]
			{
				"CAP_ROUND"
			})
		};

		// Token: 0x040002EB RID: 747
		private static readonly ShapesMaterials[] matsLine3D = new ShapesMaterials[]
		{
			new ShapesMaterials("Line 3D", Array.Empty<string>()),
			new ShapesMaterials("Line 3D", new string[]
			{
				"CAP_SQUARE"
			}),
			new ShapesMaterials("Line 3D", new string[]
			{
				"CAP_ROUND"
			})
		};

		// Token: 0x040002EC RID: 748
		private static readonly ShapesMaterials[] matsPolyline = new ShapesMaterials[]
		{
			new ShapesMaterials("Polyline 2D", Array.Empty<string>()),
			new ShapesMaterials("Polyline 2D", new string[]
			{
				"JOIN_MITER"
			}),
			new ShapesMaterials("Polyline 2D", new string[]
			{
				"JOIN_ROUND"
			}),
			new ShapesMaterials("Polyline 2D", new string[]
			{
				"JOIN_BEVEL"
			})
		};

		// Token: 0x040002ED RID: 749
		private static readonly ShapesMaterials[] matsPolylineJoin = new ShapesMaterials[]
		{
			new ShapesMaterials("Polyline 2D", new string[]
			{
				"IS_JOIN_MESH"
			}),
			new ShapesMaterials("Polyline 2D", new string[]
			{
				"IS_JOIN_MESH",
				"JOIN_MITER"
			}),
			new ShapesMaterials("Polyline 2D", new string[]
			{
				"IS_JOIN_MESH",
				"JOIN_ROUND"
			}),
			new ShapesMaterials("Polyline 2D", new string[]
			{
				"IS_JOIN_MESH",
				"JOIN_BEVEL"
			})
		};
	}
}
