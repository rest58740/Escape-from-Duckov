using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000039 RID: 57
	public class PolygonPath : PointPath<Vector2>
	{
		// Token: 0x06000BD8 RID: 3032 RVA: 0x00017928 File Offset: 0x00015B28
		public void AddPoint(float x, float y)
		{
			base.AddPoint(new Vector2(x, y));
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00017937 File Offset: 0x00015B37
		public void BezierTo(Vector2 startTangent, Vector2 endTangent, Vector2 end, int pointCount)
		{
			if (base.CheckCanAddContinuePoint("BezierTo"))
			{
				return;
			}
			base.AddPoints(ShapesMath.CubicBezierPointsSkipFirst(base.LastPoint, startTangent, endTangent, end, pointCount));
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x00017960 File Offset: 0x00015B60
		public void BezierTo(Vector2 startTangent, Vector2 endTangent, Vector2 end, float pointsPerTurn)
		{
			int vertCount = ShapesConfig.Instance.polylineBezierAngularSumAccuracy * 2 + 1;
			float num = ShapesMath.GetApproximateAngularCurveSumDegrees(base.LastPoint, startTangent, endTangent, end, vertCount) / 360f;
			int pointCount = Mathf.Max(2, Mathf.RoundToInt(num * ShapesConfig.Instance.polylineDefaultPointsPerTurn));
			this.BezierTo(startTangent, endTangent, end, pointCount);
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x000179C8 File Offset: 0x00015BC8
		public void ArcTo(Vector2 corner, Vector2 next, float radius, float pointsPerTurn)
		{
			if (base.CheckCanAddContinuePoint("ArcTo"))
			{
				return;
			}
			this.AddArcPoints(corner, next, radius, true, 0, pointsPerTurn);
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x000179E5 File Offset: 0x00015BE5
		public void ArcTo(Vector2 corner, Vector2 next, float radius, int pointCount)
		{
			if (base.CheckCanAddContinuePoint("ArcTo"))
			{
				return;
			}
			this.AddArcPoints(corner, next, radius, false, pointCount, 0f);
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00017A06 File Offset: 0x00015C06
		public void ArcTo(Vector2 corner, Vector2 next, float radius)
		{
			if (base.CheckCanAddContinuePoint("ArcTo"))
			{
				return;
			}
			this.AddArcPoints(corner, next, radius, true, 0, ShapesConfig.Instance.polylineDefaultPointsPerTurn);
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00017A2B File Offset: 0x00015C2B
		public void ArcTo(Vector2 corner, Vector2 next, float radius, float pointsPerTurn, Color color)
		{
			if (base.CheckCanAddContinuePoint("ArcTo"))
			{
				return;
			}
			this.AddArcPoints(corner, next, radius, true, 0, pointsPerTurn);
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00017A48 File Offset: 0x00015C48
		private void AddArcPoints(Vector2 corner, Vector2 next, float radius, bool useDensity, int targetPointCount, float pointsPerTurn)
		{
			if (radius <= 0.0001f)
			{
				base.AddPoint(corner);
				return;
			}
			Vector2 normalized = (corner - base.LastPoint).normalized;
			Vector2 normalized2 = (next - corner).normalized;
			if (Vector2.Dot(normalized, normalized2) > 0.999f)
			{
				base.AddPoint(corner);
				return;
			}
			Vector2 vector = ShapesMath.Rotate90CW(normalized);
			Vector2 vector2 = ShapesMath.Rotate90CW(normalized2);
			Vector2 normalized3 = (vector + vector2).normalized;
			float num = Vector2.Dot(normalized3, vector2);
			Vector2 center = corner + normalized3 * (radius / num);
			if (useDensity)
			{
				targetPointCount = Mathf.RoundToInt(Vector2.Angle(vector, vector2) / 360f * pointsPerTurn);
			}
			base.AddPoints(ShapesMath.GetArcPoints(-vector, -vector2, center, radius, targetPointCount));
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00017B18 File Offset: 0x00015D18
		public bool EnsureMeshIsReadyToRender(PolygonTriangulation triangulation, out Mesh outMesh)
		{
			if (!this.meshDirty && triangulation != this.lastUsedTriangulationMode)
			{
				this.meshDirty = true;
			}
			return base.EnsureMeshIsReadyToRender(out outMesh, delegate
			{
				this.TryUpdateMesh(triangulation);
			});
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00017B69 File Offset: 0x00015D69
		private void TryUpdateMesh(PolygonTriangulation triangulation)
		{
			this.lastUsedTriangulationMode = triangulation;
			ShapesMeshGen.GenPolygonMesh(this.mesh, this.path, triangulation);
		}

		// Token: 0x04000199 RID: 409
		private PolygonTriangulation lastUsedTriangulationMode = PolygonTriangulation.EarClipping;
	}
}
