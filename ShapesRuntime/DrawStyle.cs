using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Shapes
{
	// Token: 0x0200001C RID: 28
	internal struct DrawStyle
	{
		// Token: 0x040000D9 RID: 217
		private const float DEFAULT_THICKNESS = 0.05f;

		// Token: 0x040000DA RID: 218
		private const ThicknessSpace DEFAULT_THICKNESS_SPACE = ThicknessSpace.Meters;

		// Token: 0x040000DB RID: 219
		public static DrawStyle @default = new DrawStyle
		{
			color = Color.white,
			renderState = new RenderState
			{
				zTest = CompareFunction.LessEqual,
				zOffsetFactor = 0f,
				zOffsetUnits = 0,
				colorMask = ColorWriteMask.All,
				stencilComp = CompareFunction.Always,
				stencilOpPass = StencilOp.Keep,
				stencilRefID = 0,
				stencilReadMask = byte.MaxValue,
				stencilWriteMask = byte.MaxValue
			},
			blendMode = ShapesBlendMode.Transparent,
			scaleMode = ScaleMode.Uniform,
			detailLevel = DetailLevel.Medium,
			useDashes = false,
			dashStyle = DashStyle.defaultDashStyle,
			useGradients = false,
			gradientFill = GradientFill.defaultFill,
			thickness = 0.05f,
			thicknessSpace = ThicknessSpace.Meters,
			radiusSpace = ThicknessSpace.Meters,
			sizeSpace = ThicknessSpace.Meters,
			radius = 1f,
			lineEndCaps = LineEndCap.Round,
			lineGeometry = LineGeometry.Billboard,
			polygonTriangulation = PolygonTriangulation.EarClipping,
			polylineGeometry = PolylineGeometry.Billboard,
			polylineJoins = PolylineJoins.Round,
			discGeometry = DiscGeometry.Flat2D,
			regularPolygonSideCount = 6,
			regularPolygonGeometry = RegularPolygonGeometry.Flat2D,
			textStyle = TextStyle.defaultTextStyle
		};

		// Token: 0x040000DC RID: 220
		public RenderState renderState;

		// Token: 0x040000DD RID: 221
		public Color color;

		// Token: 0x040000DE RID: 222
		public ShapesBlendMode blendMode;

		// Token: 0x040000DF RID: 223
		public ScaleMode scaleMode;

		// Token: 0x040000E0 RID: 224
		public DetailLevel detailLevel;

		// Token: 0x040000E1 RID: 225
		public bool useDashes;

		// Token: 0x040000E2 RID: 226
		public DashStyle dashStyle;

		// Token: 0x040000E3 RID: 227
		public bool useGradients;

		// Token: 0x040000E4 RID: 228
		public GradientFill gradientFill;

		// Token: 0x040000E5 RID: 229
		public float radius;

		// Token: 0x040000E6 RID: 230
		public float thickness;

		// Token: 0x040000E7 RID: 231
		public ThicknessSpace thicknessSpace;

		// Token: 0x040000E8 RID: 232
		public ThicknessSpace radiusSpace;

		// Token: 0x040000E9 RID: 233
		public ThicknessSpace sizeSpace;

		// Token: 0x040000EA RID: 234
		public LineEndCap lineEndCaps;

		// Token: 0x040000EB RID: 235
		public LineGeometry lineGeometry;

		// Token: 0x040000EC RID: 236
		public PolygonTriangulation polygonTriangulation;

		// Token: 0x040000ED RID: 237
		public PolylineGeometry polylineGeometry;

		// Token: 0x040000EE RID: 238
		public PolylineJoins polylineJoins;

		// Token: 0x040000EF RID: 239
		public DiscGeometry discGeometry;

		// Token: 0x040000F0 RID: 240
		public int regularPolygonSideCount;

		// Token: 0x040000F1 RID: 241
		public RegularPolygonGeometry regularPolygonGeometry;

		// Token: 0x040000F2 RID: 242
		public TextStyle textStyle;
	}
}
