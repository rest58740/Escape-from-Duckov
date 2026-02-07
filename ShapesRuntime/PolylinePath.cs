using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200003A RID: 58
	public class PolylinePath : PointPath<PolylinePoint>
	{
		// Token: 0x06000BE3 RID: 3043 RVA: 0x00017B94 File Offset: 0x00015D94
		public void SetPoint(int index, Vector3 point)
		{
			PolylinePoint point2 = this.path[index];
			point2.point = point;
			base.SetPoint(index, point2);
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00017BC0 File Offset: 0x00015DC0
		public void SetPoint(int index, Vector2 point)
		{
			PolylinePoint point2 = this.path[index];
			point2.point = point;
			base.SetPoint(index, point2);
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00017BF0 File Offset: 0x00015DF0
		public void SetColor(int index, Color color)
		{
			PolylinePoint point = this.path[index];
			point.color = color;
			base.SetPoint(index, point);
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00017C1A File Offset: 0x00015E1A
		[MethodImpl(256)]
		public void AddPoint(float x, float y)
		{
			base.AddPoint(new PolylinePoint(new Vector3(x, y, 0f), Color.white));
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00017C38 File Offset: 0x00015E38
		[MethodImpl(256)]
		public void AddPoint(float x, float y, float z)
		{
			base.AddPoint(new PolylinePoint(new Vector3(x, y, z), Color.white));
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x00017C52 File Offset: 0x00015E52
		[MethodImpl(256)]
		public void AddPoint(float x, float y, Color color)
		{
			base.AddPoint(new PolylinePoint(new Vector3(x, y, 0f), color));
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00017C6C File Offset: 0x00015E6C
		[MethodImpl(256)]
		public void AddPoint(float x, float y, float z, Color color)
		{
			base.AddPoint(new PolylinePoint(new Vector3(x, y, z), color));
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00017C83 File Offset: 0x00015E83
		[MethodImpl(256)]
		public void AddPoint(Vector3 pos)
		{
			base.AddPoint(new PolylinePoint(pos, Color.white));
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00017C96 File Offset: 0x00015E96
		[MethodImpl(256)]
		public void AddPoint(Vector3 pos, Color color)
		{
			base.AddPoint(new PolylinePoint(pos, color));
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00017CA5 File Offset: 0x00015EA5
		[MethodImpl(256)]
		public void AddPoint(Vector3 pos, float thickness)
		{
			base.AddPoint(new PolylinePoint(pos, Color.white, thickness));
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00017CB9 File Offset: 0x00015EB9
		[MethodImpl(256)]
		public void AddPoint(Vector3 pos, float thickness, Color color)
		{
			base.AddPoint(new PolylinePoint(pos, color, thickness));
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00017CC9 File Offset: 0x00015EC9
		[MethodImpl(256)]
		public void AddPoint(Vector2 pos)
		{
			base.AddPoint(new PolylinePoint(pos, Color.white));
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00017CDC File Offset: 0x00015EDC
		[MethodImpl(256)]
		public void AddPoint(Vector2 pos, Color color)
		{
			base.AddPoint(new PolylinePoint(pos, color));
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00017CEB File Offset: 0x00015EEB
		[MethodImpl(256)]
		public void AddPoint(Vector2 pos, float thickness)
		{
			base.AddPoint(new PolylinePoint(pos, Color.white, thickness));
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00017CFF File Offset: 0x00015EFF
		[MethodImpl(256)]
		public void AddPoint(Vector2 pos, float thickness, Color color)
		{
			base.AddPoint(new PolylinePoint(pos, color, thickness));
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00017D0F File Offset: 0x00015F0F
		[MethodImpl(256)]
		public void AddPoints(IEnumerable<Vector3> pts)
		{
			base.AddPoints(from point in pts
			select new PolylinePoint(point, Color.white));
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00017D3C File Offset: 0x00015F3C
		[MethodImpl(256)]
		public void AddPoints(params Vector3[] pts)
		{
			base.AddPoints(from point in pts
			select new PolylinePoint(point, Color.white));
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00017D69 File Offset: 0x00015F69
		[MethodImpl(256)]
		public void AddPoints(IEnumerable<Vector2> pts)
		{
			base.AddPoints(from point in pts
			select new PolylinePoint(point, Color.white));
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x00017D96 File Offset: 0x00015F96
		[MethodImpl(256)]
		public void AddPoints(params Vector2[] pts)
		{
			base.AddPoints(from point in pts
			select new PolylinePoint(point, Color.white));
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00017DC4 File Offset: 0x00015FC4
		[MethodImpl(256)]
		public void AddPoints(IEnumerable<Vector3> pts, Color color)
		{
			base.AddPoints(from point in pts
			select new PolylinePoint(point, color));
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00017DF8 File Offset: 0x00015FF8
		[MethodImpl(256)]
		public void AddPoints(IEnumerable<Vector2> pts, Color color)
		{
			base.AddPoints(from point in pts
			select new PolylinePoint(point, color));
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00017E2A File Offset: 0x0001602A
		[MethodImpl(256)]
		public void AddPoints(IEnumerable<Vector3> pts, IEnumerable<Color> colors)
		{
			base.AddPoints(pts.Zip(colors, (Vector3 p, Color c) => new PolylinePoint(p, c)));
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x00017E58 File Offset: 0x00016058
		[MethodImpl(256)]
		public void AddPoints(IEnumerable<Vector2> pts, IEnumerable<Color> colors)
		{
			base.AddPoints(pts.Zip(colors, (Vector2 p, Color c) => new PolylinePoint(p, c)));
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x00017E86 File Offset: 0x00016086
		[MethodImpl(256)]
		public void AddPoints(IEnumerable<Vector3> pts, IEnumerable<float> thicknesses)
		{
			base.AddPoints(pts.Zip(thicknesses, (Vector3 p, float t) => new PolylinePoint(p, Color.white, t)));
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x00017EB4 File Offset: 0x000160B4
		[MethodImpl(256)]
		public void AddPoints(IEnumerable<Vector2> pts, IEnumerable<float> thicknesses)
		{
			base.AddPoints(pts.Zip(thicknesses, (Vector2 p, float t) => new PolylinePoint(p, Color.white, t)));
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x00017EE2 File Offset: 0x000160E2
		[MethodImpl(256)]
		public void AddPoints(IEnumerable<Vector3> pts, IEnumerable<float> thicknesses, IEnumerable<Color> colors)
		{
			base.AddPoints(pts.Zip(colors, thicknesses, (Vector3 p, Color c, float t) => new PolylinePoint(p, c, t)));
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00017F11 File Offset: 0x00016111
		[MethodImpl(256)]
		public void AddPoints(IEnumerable<Vector2> pts, IEnumerable<float> thicknesses, IEnumerable<Color> colors)
		{
			base.AddPoints(pts.Zip(colors, thicknesses, (Vector2 p, Color c, float t) => new PolylinePoint(p, c, t)));
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x00017F40 File Offset: 0x00016140
		[MethodImpl(256)]
		public void BezierTo(Vector3 startTangent, Vector3 endTangent, Vector3 end)
		{
			this.BezierTo(startTangent, endTangent, end, ShapesConfig.Instance.polylineDefaultPointsPerTurn);
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00017F58 File Offset: 0x00016158
		public void BezierTo(Vector3 startTangent, Vector3 endTangent, Vector3 end, float pointsPerTurn)
		{
			if (base.CheckCanAddContinuePoint("BezierTo"))
			{
				return;
			}
			int pointCount = PolylinePath.CalcBezierPointCount(base.LastPoint.point, startTangent, endTangent, end, pointsPerTurn);
			this.BezierTo(startTangent, endTangent, end, pointCount);
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00017F93 File Offset: 0x00016193
		public void BezierTo(Vector3 startTangent, Vector3 endTangent, Vector3 end, int pointCount)
		{
			if (base.CheckCanAddContinuePoint("BezierTo"))
			{
				return;
			}
			base.AddPoints(ShapesMath.CubicBezierPointsSkipFirstMatchStyle(base.LastPoint, base.LastPoint.point, startTangent, endTangent, end, pointCount));
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00017FC4 File Offset: 0x000161C4
		[MethodImpl(256)]
		public void BezierTo(Vector3 startTangent, Vector3 endTangent, PolylinePoint end)
		{
			this.BezierTo(startTangent, endTangent, end, ShapesConfig.Instance.polylineDefaultPointsPerTurn);
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00017FDC File Offset: 0x000161DC
		public void BezierTo(Vector3 startTangent, Vector3 endTangent, PolylinePoint end, float pointsPerTurn)
		{
			if (base.CheckCanAddContinuePoint("BezierTo"))
			{
				return;
			}
			int pointCount = PolylinePath.CalcBezierPointCount(base.LastPoint.point, startTangent, endTangent, end.point, pointsPerTurn);
			this.BezierTo(startTangent, endTangent, end, pointCount);
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0001801C File Offset: 0x0001621C
		public void BezierTo(Vector3 startTangent, Vector3 endTangent, PolylinePoint end, int pointCount)
		{
			if (base.CheckCanAddContinuePoint("BezierTo"))
			{
				return;
			}
			PolylinePoint startTangent2 = PolylinePoint.Lerp(base.LastPoint, end, 0.33333334f);
			startTangent2.point = startTangent;
			PolylinePoint endTangent2 = PolylinePoint.Lerp(base.LastPoint, end, 0.6666667f);
			endTangent2.point = endTangent;
			this.BezierTo(startTangent2, endTangent2, end, pointCount);
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00018076 File Offset: 0x00016276
		[MethodImpl(256)]
		public void BezierTo(PolylinePoint startTangent, PolylinePoint endTangent, PolylinePoint end)
		{
			this.BezierTo(startTangent, endTangent, end, ShapesConfig.Instance.polylineDefaultPointsPerTurn);
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x0001808C File Offset: 0x0001628C
		public void BezierTo(PolylinePoint startTangent, PolylinePoint endTangent, PolylinePoint end, float pointsPerTurn)
		{
			if (base.CheckCanAddContinuePoint("BezierTo"))
			{
				return;
			}
			int pointCount = PolylinePath.CalcBezierPointCount(base.LastPoint.point, startTangent.point, endTangent.point, end.point, pointsPerTurn);
			this.BezierTo(startTangent, endTangent, end, pointCount);
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x000180D6 File Offset: 0x000162D6
		public void BezierTo(PolylinePoint startTangent, PolylinePoint endTangent, PolylinePoint end, int pointCount)
		{
			if (base.CheckCanAddContinuePoint("BezierTo"))
			{
				return;
			}
			base.AddPoints(ShapesMath.CubicBezierPointsSkipFirst(base.LastPoint, startTangent, endTangent, end, pointCount));
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x000180FC File Offset: 0x000162FC
		private static int CalcBezierPointCount(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float pointsPerTurn)
		{
			int vertCount = ShapesConfig.Instance.polylineBezierAngularSumAccuracy * 2 + 1;
			float num = ShapesMath.GetApproximateAngularCurveSumDegrees(a, b, c, d, vertCount) / 360f;
			return Mathf.Max(2, Mathf.RoundToInt(num * pointsPerTurn));
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00018138 File Offset: 0x00016338
		[MethodImpl(256)]
		public void ArcTo(Vector3 corner, Vector3 next, float radius, int pointCount)
		{
			this.AddArcPoints(corner, next, radius, false, pointCount, 0f);
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0001814B File Offset: 0x0001634B
		[MethodImpl(256)]
		public void ArcTo(Vector3 corner, PolylinePoint next, float radius, int pointCount)
		{
			this.AddArcPoints(corner, next, radius, false, pointCount, 0f);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x0001815E File Offset: 0x0001635E
		[MethodImpl(256)]
		public void ArcTo(Vector3 corner, Vector3 next, float radius)
		{
			this.AddArcPoints(corner, next, radius, true, 0, ShapesConfig.Instance.polylineDefaultPointsPerTurn);
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00018175 File Offset: 0x00016375
		[MethodImpl(256)]
		public void ArcTo(Vector3 corner, PolylinePoint next, float radius)
		{
			this.AddArcPoints(corner, next, radius, true, 0, ShapesConfig.Instance.polylineDefaultPointsPerTurn);
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x0001818C File Offset: 0x0001638C
		[MethodImpl(256)]
		public void ArcTo(Vector3 corner, Vector3 next, float radius, float pointsPerTurn)
		{
			this.AddArcPoints(corner, next, radius, true, 0, pointsPerTurn);
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x0001819B File Offset: 0x0001639B
		[MethodImpl(256)]
		public void ArcTo(Vector3 corner, PolylinePoint next, float radius, float pointsPerTurn)
		{
			this.AddArcPoints(corner, next, radius, true, 0, pointsPerTurn);
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x000181AC File Offset: 0x000163AC
		private void AddArcPoints(Vector3 corner, Vector3 next, float radius, bool useDensity, int targetPointCount, float pointsPerTurn)
		{
			if (base.CheckCanAddContinuePoint("AddArcPoints"))
			{
				return;
			}
			PolylinePoint lastPoint = base.LastPoint;
			lastPoint.point = next;
			this.AddArcPoints(corner, lastPoint, radius, useDensity, targetPointCount, pointsPerTurn);
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x000181E8 File Offset: 0x000163E8
		private void AddArcPoints(Vector3 corner, PolylinePoint next, float radius, bool useDensity, int targetPointCount, float pointsPerTurn)
		{
			if (base.CheckCanAddContinuePoint("AddArcPoints"))
			{
				return;
			}
			PolylinePoint lastPoint = base.LastPoint;
			Vector3 normalized = (corner - lastPoint.point).normalized;
			Vector3 normalized2 = (next.point - corner).normalized;
			Vector3 v = Vector3.Cross(normalized, normalized2);
			if (v.TaxicabMagnitude() <= 0.001f)
			{
				float lineSegmentProjectionT = ShapesMath.GetLineSegmentProjectionT(lastPoint.point, next.point, corner);
				float t = Mathf.Clamp01(lineSegmentProjectionT - 0.0001f);
				float t2 = Mathf.Clamp01(lineSegmentProjectionT + 0.0001f);
				PolylinePoint p = lastPoint;
				PolylinePoint p2 = next;
				p.point = Vector3.Lerp(lastPoint.point, next.point, t);
				p2.point = Vector3.Lerp(lastPoint.point, next.point, t2);
				base.AddPoint(p);
				base.AddPoint(p2);
				return;
			}
			Vector3 normalized3 = v.normalized;
			Vector3 vector = Vector3.Cross(normalized3, normalized);
			Vector3 vector2 = Vector3.Cross(normalized3, normalized2);
			Vector3 normalized4 = (vector + vector2).normalized;
			float num = Vector3.Dot(normalized4, vector2);
			radius = Mathf.Max(radius, 0.0001f);
			Vector3 center = corner + normalized4 * (radius / num);
			if (useDensity)
			{
				targetPointCount = Mathf.RoundToInt(Vector3.Angle(vector, vector2) / 360f * pointsPerTurn);
			}
			base.AddPoints(ShapesMath.GetArcPoints(lastPoint, next, -vector, -vector2, center, radius, targetPointCount));
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00018358 File Offset: 0x00016558
		public bool EnsureMeshIsReadyToRender(bool closed, PolylineJoins renderJoins, out Mesh outMesh)
		{
			if (!this.meshDirty && (renderJoins != this.lastUsedJoins || closed != this.lastUsedClosed))
			{
				this.meshDirty = true;
			}
			return base.EnsureMeshIsReadyToRender(out outMesh, delegate
			{
				this.TryUpdateMesh(closed, renderJoins);
			});
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x000183BE File Offset: 0x000165BE
		private void TryUpdateMesh(bool closed, PolylineJoins joins)
		{
			this.lastUsedClosed = closed;
			this.lastUsedJoins = joins;
			ShapesMeshGen.GenPolylineMesh(this.mesh, this.path, closed, joins, false, true);
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x000183E3 File Offset: 0x000165E3
		[Obsolete("This function no longer exists - either use the overload without a color, where the color will match the previous point, or the one with a PolylinePoint endpoint, where the color will blend between previous point and the target point", true)]
		public void ArcTo(Vector3 corner, Vector3 next, float radius, int pointCount, Color color)
		{
			this.AddArcPoints(corner, next, radius, false, pointCount, 0f);
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x000183F6 File Offset: 0x000165F6
		[Obsolete("This function no longer exists - either use the overload without a color, where the color will match the previous point, or the one with a PolylinePoint endpoint, where the color will blend between previous point and the target point", true)]
		public void ArcTo(Vector3 corner, Vector3 next, float radius, Color color)
		{
			this.AddArcPoints(corner, next, radius, true, 0, ShapesConfig.Instance.polylineDefaultPointsPerTurn);
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x0001840D File Offset: 0x0001660D
		[Obsolete("This function no longer exists - either use the overload without a color, where the color will match the previous point, or the one with a PolylinePoint endpoint, where the color will blend between previous point and the target point", true)]
		public void ArcTo(Vector3 corner, Vector3 next, float radius, float pointsPerTurn, Color color)
		{
			this.AddArcPoints(corner, next, radius, true, 0, pointsPerTurn);
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0001841C File Offset: 0x0001661C
		[Obsolete("This function no longer exists - either use the overload without a color, where the color will match the previous point, or the one with a PolylinePoint endpoint, where the color will blend between previous point and the target point", true)]
		public void BezierTo(Vector3 startTangent, Vector3 endTangent, Vector3 end, float pointsPerTurn, Color color)
		{
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0001841E File Offset: 0x0001661E
		[Obsolete("This function no longer exists - either use the overload without a color, where the color will match the previous point, or the one with a PolylinePoint endpoint, where the color will blend between previous point and the target point", true)]
		public void BezierTo(Vector3 startTangent, Vector3 endTangent, Vector3 end, int pointCount, Color color)
		{
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x00018420 File Offset: 0x00016620
		[Obsolete("This function no longer exists - either use the overload without a color, where the color will match the previous point, or the one with a PolylinePoint endpoint, where the color will blend between previous point and the target point", true)]
		public void BezierTo(Vector3 startTangent, Vector3 endTangent, Vector3 end, Color color)
		{
		}

		// Token: 0x0400019A RID: 410
		private const MethodImplOptions INLINE = 256;

		// Token: 0x0400019B RID: 411
		private bool lastUsedClosed;

		// Token: 0x0400019C RID: 412
		private PolylineJoins lastUsedJoins = PolylineJoins.Miter;
	}
}
