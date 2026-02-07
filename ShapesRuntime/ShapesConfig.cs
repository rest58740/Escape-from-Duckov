using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000072 RID: 114
	[CreateAssetMenu]
	public class ShapesConfig : ScriptableObject
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x00019BF5 File Offset: 0x00017DF5
		public static ShapesConfig Instance
		{
			get
			{
				if (ShapesConfig.inst == null)
				{
					ShapesConfig.inst = Resources.Load<ShapesConfig>("Shapes Config");
				}
				return ShapesConfig.inst;
			}
		}

		// Token: 0x04000282 RID: 642
		private static ShapesConfig inst;

		// Token: 0x04000283 RID: 643
		[Tooltip("Whether or not to use HDR color pickers throughout Shapes (This does not affect performance in any way)")]
		public bool useHdrColorPickers;

		// Token: 0x04000284 RID: 644
		[Tooltip("Whether or not to auto-detect and set up render pipeline")]
		public bool autoConfigureRenderPipeline = true;

		// Token: 0x04000285 RID: 645
		[Tooltip("GPU Instancing in immediate mode drawing means if you render lots of similar shapes consecutively, they will get batched into a single draw call. Generally you'll want this on, but there may be cases where the CPU and memory overhead of instancing isn't worth it, which might be the case if you never draw shapes of the same type consecutively")]
		public bool useImmediateModeInstancing = true;

		// Token: 0x04000286 RID: 646
		[Tooltip("Default point density for polyline arcs and beziers in points per full turn\nIf set to 128, then it'll use 64 points for a 180° turn, 32 points for a 90° turn\n\n16 = curves are very jagged, clearly just a bunch of straight lines in a trenchcoat, except they forgot the trenchcoat\n32 = curves visibly have straight segments when looking close, but appear smooth at a quick glance. (trenchcoat is now on)\n64 = curves generally appear smooth, except at the very sharpest of turns. recommended value.\n128 = curves appear smooth in pretty much all cases, beyond this is pretty wild, but I mean, if you're a wild person then go for it\n")]
		public float polylineDefaultPointsPerTurn = 64f;

		// Token: 0x04000287 RID: 647
		[Tooltip("Default accuracy when calculating point density of bezier curves.\nThis is only used for bezier curves where you specify density rather than point count.\nIf you have mostly very simple bezier curves, you can leave this at 3.\nIf you have more complex curves, like those with widely separated control points squishing the curve,\nthen you should use at least 5 samples\n\n1 = ~12% margin of error. this is the minimum value! works for the simplest curves, but generally inaccurate\n2 = ~4% margin of error. this is recommended, good balance between accuracy and speed\n3 = ~2% margin of error\n4 = ~1% margin of error")]
		public int polylineBezierAngularSumAccuracy = 2;

		// Token: 0x04000288 RID: 648
		[Tooltip("If this is on, static properties set inside of Draw.Command will apply only within that draw command. This is usually more intuitive and convenient, but it does come with a slight processing overhead, so if you are running something very performance sensitive you might want to turn this off")]
		public bool pushPopStateInDrawCommands = true;

		// Token: 0x04000289 RID: 649
		public const string TOOLTIP_BOUNDS = "These settings are uh, very esoteric\n*if* you are having trouble with *many* shapes being drawn on screen at the same time,\nmaking the bounds smaller using this parameter might help you optimize your game\n\nThis is like, super technical, so please read every word very carefully below:\nThis value should be set so that *all* shapes using, for instance, the quad mesh (disc, line, rect, etc.),\ncan use *these specific bounds*, so that the bounds would encapsulate the entire shape.\nPractically, this means that these bounds should be set so that it can encapsulate the largest\nshape you have in your project. If this is set too low, larger shapes will pop in/out of existence\n\nThe purpose of this is to gain some benefit in culling, but still keep the benefits of instancing.\nBy default, size is set to a large value of 1 << 16 (65536), practically \"turning off\" frustum culling";

		// Token: 0x0400028A RID: 650
		private const float VERY_LORGE_BOUNDS = 65536f;

		// Token: 0x0400028B RID: 651
		[Tooltip("These settings are uh, very esoteric\n*if* you are having trouble with *many* shapes being drawn on screen at the same time,\nmaking the bounds smaller using this parameter might help you optimize your game\n\nThis is like, super technical, so please read every word very carefully below:\nThis value should be set so that *all* shapes using, for instance, the quad mesh (disc, line, rect, etc.),\ncan use *these specific bounds*, so that the bounds would encapsulate the entire shape.\nPractically, this means that these bounds should be set so that it can encapsulate the largest\nshape you have in your project. If this is set too low, larger shapes will pop in/out of existence\n\nThe purpose of this is to gain some benefit in culling, but still keep the benefits of instancing.\nBy default, size is set to a large value of 1 << 16 (65536), practically \"turning off\" frustum culling")]
		public float boundsSizeQuad = 65536f;

		// Token: 0x0400028C RID: 652
		[Tooltip("These settings are uh, very esoteric\n*if* you are having trouble with *many* shapes being drawn on screen at the same time,\nmaking the bounds smaller using this parameter might help you optimize your game\n\nThis is like, super technical, so please read every word very carefully below:\nThis value should be set so that *all* shapes using, for instance, the quad mesh (disc, line, rect, etc.),\ncan use *these specific bounds*, so that the bounds would encapsulate the entire shape.\nPractically, this means that these bounds should be set so that it can encapsulate the largest\nshape you have in your project. If this is set too low, larger shapes will pop in/out of existence\n\nThe purpose of this is to gain some benefit in culling, but still keep the benefits of instancing.\nBy default, size is set to a large value of 1 << 16 (65536), practically \"turning off\" frustum culling")]
		public float boundsSizeTriangle = 65536f;

		// Token: 0x0400028D RID: 653
		[Tooltip("These settings are uh, very esoteric\n*if* you are having trouble with *many* shapes being drawn on screen at the same time,\nmaking the bounds smaller using this parameter might help you optimize your game\n\nThis is like, super technical, so please read every word very carefully below:\nThis value should be set so that *all* shapes using, for instance, the quad mesh (disc, line, rect, etc.),\ncan use *these specific bounds*, so that the bounds would encapsulate the entire shape.\nPractically, this means that these bounds should be set so that it can encapsulate the largest\nshape you have in your project. If this is set too low, larger shapes will pop in/out of existence\n\nThe purpose of this is to gain some benefit in culling, but still keep the benefits of instancing.\nBy default, size is set to a large value of 1 << 16 (65536), practically \"turning off\" frustum culling")]
		public float boundsSizeSphere = 65536f;

		// Token: 0x0400028E RID: 654
		[Tooltip("These settings are uh, very esoteric\n*if* you are having trouble with *many* shapes being drawn on screen at the same time,\nmaking the bounds smaller using this parameter might help you optimize your game\n\nThis is like, super technical, so please read every word very carefully below:\nThis value should be set so that *all* shapes using, for instance, the quad mesh (disc, line, rect, etc.),\ncan use *these specific bounds*, so that the bounds would encapsulate the entire shape.\nPractically, this means that these bounds should be set so that it can encapsulate the largest\nshape you have in your project. If this is set too low, larger shapes will pop in/out of existence\n\nThe purpose of this is to gain some benefit in culling, but still keep the benefits of instancing.\nBy default, size is set to a large value of 1 << 16 (65536), practically \"turning off\" frustum culling")]
		public float boundsSizeTorus = 65536f;

		// Token: 0x0400028F RID: 655
		[Tooltip("These settings are uh, very esoteric\n*if* you are having trouble with *many* shapes being drawn on screen at the same time,\nmaking the bounds smaller using this parameter might help you optimize your game\n\nThis is like, super technical, so please read every word very carefully below:\nThis value should be set so that *all* shapes using, for instance, the quad mesh (disc, line, rect, etc.),\ncan use *these specific bounds*, so that the bounds would encapsulate the entire shape.\nPractically, this means that these bounds should be set so that it can encapsulate the largest\nshape you have in your project. If this is set too low, larger shapes will pop in/out of existence\n\nThe purpose of this is to gain some benefit in culling, but still keep the benefits of instancing.\nBy default, size is set to a large value of 1 << 16 (65536), practically \"turning off\" frustum culling")]
		public float boundsSizeCuboid = 65536f;

		// Token: 0x04000290 RID: 656
		[Tooltip("These settings are uh, very esoteric\n*if* you are having trouble with *many* shapes being drawn on screen at the same time,\nmaking the bounds smaller using this parameter might help you optimize your game\n\nThis is like, super technical, so please read every word very carefully below:\nThis value should be set so that *all* shapes using, for instance, the quad mesh (disc, line, rect, etc.),\ncan use *these specific bounds*, so that the bounds would encapsulate the entire shape.\nPractically, this means that these bounds should be set so that it can encapsulate the largest\nshape you have in your project. If this is set too low, larger shapes will pop in/out of existence\n\nThe purpose of this is to gain some benefit in culling, but still keep the benefits of instancing.\nBy default, size is set to a large value of 1 << 16 (65536), practically \"turning off\" frustum culling")]
		public float boundsSizeCone = 65536f;

		// Token: 0x04000291 RID: 657
		[Tooltip("These settings are uh, very esoteric\n*if* you are having trouble with *many* shapes being drawn on screen at the same time,\nmaking the bounds smaller using this parameter might help you optimize your game\n\nThis is like, super technical, so please read every word very carefully below:\nThis value should be set so that *all* shapes using, for instance, the quad mesh (disc, line, rect, etc.),\ncan use *these specific bounds*, so that the bounds would encapsulate the entire shape.\nPractically, this means that these bounds should be set so that it can encapsulate the largest\nshape you have in your project. If this is set too low, larger shapes will pop in/out of existence\n\nThe purpose of this is to gain some benefit in culling, but still keep the benefits of instancing.\nBy default, size is set to a large value of 1 << 16 (65536), practically \"turning off\" frustum culling")]
		public float boundsSizeCylinder = 65536f;

		// Token: 0x04000292 RID: 658
		[Tooltip("These settings are uh, very esoteric\n*if* you are having trouble with *many* shapes being drawn on screen at the same time,\nmaking the bounds smaller using this parameter might help you optimize your game\n\nThis is like, super technical, so please read every word very carefully below:\nThis value should be set so that *all* shapes using, for instance, the quad mesh (disc, line, rect, etc.),\ncan use *these specific bounds*, so that the bounds would encapsulate the entire shape.\nPractically, this means that these bounds should be set so that it can encapsulate the largest\nshape you have in your project. If this is set too low, larger shapes will pop in/out of existence\n\nThe purpose of this is to gain some benefit in culling, but still keep the benefits of instancing.\nBy default, size is set to a large value of 1 << 16 (65536), practically \"turning off\" frustum culling")]
		public float boundsSizeCapsule = 65536f;

		// Token: 0x04000293 RID: 659
		public int[] sphereDetail = new int[]
		{
			1,
			2,
			5,
			7,
			12
		};

		// Token: 0x04000294 RID: 660
		public Vector2Int[] torusDivsMinorMajor = new Vector2Int[]
		{
			new Vector2Int(6, 8),
			new Vector2Int(12, 16),
			new Vector2Int(24, 32),
			new Vector2Int(32, 48),
			new Vector2Int(64, 128)
		};

		// Token: 0x04000295 RID: 661
		public int[] coneDivs = new int[]
		{
			8,
			12,
			32,
			64,
			128
		};

		// Token: 0x04000296 RID: 662
		public int[] cylinderDivs = new int[]
		{
			8,
			12,
			32,
			64,
			128
		};

		// Token: 0x04000297 RID: 663
		public int[] capsuleDivs = new int[]
		{
			2,
			3,
			8,
			10,
			32
		};

		// Token: 0x04000298 RID: 664
		[Tooltip("Precision of the fragment shader output.\n\n[fixed4] 11 bit, cheap and very low precision output, range of –2 to +2 and 1/256th precision\n\n[half4] 16 bit, range of –60000 to +60000, with about 3 decimal digits of precision\n\n[float4] 32 bit, full floating point precision")]
		public ShapesConfig.FragOutputPrecision FRAG_OUTPUT_V4 = ShapesConfig.FragOutputPrecision.half4;

		// Token: 0x04000299 RID: 665
		[Tooltip("[Off] Turns off local anti-aliasing\n\n[Medium] Approximate, usually good enough. This uses the approximate partial derivative of fwidth for anti-aliasing\n\n[High] Higher quality, mathematically correct. Primarily handles diagonals better as it uses more precise partial derivative calculations")]
		public ShapesConfig.LocalAAQuality LOCAL_ANTI_ALIASING_QUALITY = ShapesConfig.LocalAAQuality.High;

		// Token: 0x0400029A RID: 666
		[Tooltip("[Low] Direct barycentric interpolation of colors per vertex\n  • super cheap\n  • prone to triangular artifacts\n  • playstation 1 energy\n\n[Medium] Barycentric interpolation of UVs, bilinear interpolation in the fragment shader\n  • this gets you like 80% there\n  • most games settle here\n  • only use quality above this if you really need to\n  • or if you are as pretentious as me with colors\n\n[High2D] 2D only, Z plane only, inverse barycentric interpolation in the fragment shader based on vertex positions.\n  • mathematically correct\n  • ...when restricted to the XY plane\n  • numerically unstable otherwise\n  • utterly and completely broken on the X plane or the Y plane. like, it goes invisible and I don't even know why. I think we're dividing by 0 or something idk\n\n[High] Full 3D inverse barycentric interpolation in the fragment shader based on vertex positions.\n  • mathematically correct method\n  • ...when all points are planar\n  • skew quads use a best-fit 2D projection\n  • the shader gets like way more expensive but the colors are nice and you can look at it and go \"nice\"")]
		public ShapesConfig.QuadInterpolationQuality QUAD_INTERPOLATION_QUALITY = ShapesConfig.QuadInterpolationQuality.Medium;

		// Token: 0x0400029B RID: 667
		[Tooltip("Noots is a unit, in addition to Meters and Pixels, useful for resolution-independent sizing\nA noot is proportional to the shortest dimension of your resolution (note: this is unrelated to physical size)\n\nConverting noots to pixels:\nmin(res.x,res.y)*(noots/NAS)\nres = screen resolution\nNAS = noots across screen\n\nYou can specify how big a single noot is here, though, I recommended leaving it at the default value of 100\n\n1 = 1 noot is 100% of the screen\n50 = 1 noot is 50% of the screen\n100 = 1 noot is 1% of the screen (default)\n(100 is like vmin in CSS)")]
		public int NOOTS_ACROSS_SCREEN = 100;

		// Token: 0x02000094 RID: 148
		public enum FragOutputPrecision
		{
			// Token: 0x0400035D RID: 861
			fixed4,
			// Token: 0x0400035E RID: 862
			half4,
			// Token: 0x0400035F RID: 863
			float4
		}

		// Token: 0x02000095 RID: 149
		public enum LocalAAQuality
		{
			// Token: 0x04000361 RID: 865
			Off,
			// Token: 0x04000362 RID: 866
			Medium,
			// Token: 0x04000363 RID: 867
			High
		}

		// Token: 0x02000096 RID: 150
		public enum QuadInterpolationQuality
		{
			// Token: 0x04000365 RID: 869
			Low,
			// Token: 0x04000366 RID: 870
			Medium,
			// Token: 0x04000367 RID: 871
			High2D,
			// Token: 0x04000368 RID: 872
			High
		}
	}
}
