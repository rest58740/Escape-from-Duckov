using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014F RID: 335
public class BezierSpline : ShapeProvider
{
	// Token: 0x06000AAD RID: 2733 RVA: 0x0002F0CC File Offset: 0x0002D2CC
	public static Vector3 GetPoint(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
	{
		float num = Mathf.Pow(1f - t, 3f);
		float num2 = 3f * t * (1f - t) * (1f - t);
		float num3 = 3f * t * t * (1f - t);
		float num4 = t * t * t;
		return num * p1 + num2 * p2 + num3 * p3 + num4 * p4;
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x0002F150 File Offset: 0x0002D350
	public static Vector3 GetTangent(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
	{
		float num = -3f * (1f - t) * (1f - t);
		float num2 = 3f * (1f - t) * (1f - t) - 6f * t * (1f - t);
		float num3 = 6f * t * (1f - t) - 3f * t * t;
		float num4 = 3f * t * t;
		return num * p1 + num2 * p2 + num3 * p3 + num4 * p4;
	}

	// Token: 0x06000AAF RID: 2735 RVA: 0x0002F1F4 File Offset: 0x0002D3F4
	public static Vector3 GetNormal(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
	{
		Vector3 tangent = BezierSpline.GetTangent(p1, p2, p3, p4, t);
		Vector3 vector = 6f * (1f - t) * p1 - (6f * (1f - t) + (6f - 12f * t)) * p2 + (6f - 12f * t - 6f * t) * p3 + 6f * t * p4;
		Vector3 normalized = tangent.normalized;
		Vector3 normalized2 = (normalized + vector).normalized;
		return Vector3.Cross(Vector3.Cross(normalized, normalized2), normalized).normalized;
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x0002F2B0 File Offset: 0x0002D4B0
	public static PipeRenderer.OrientedPoint[] GenerateShape(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int subdivisions)
	{
		List<PipeRenderer.OrientedPoint> list = new List<PipeRenderer.OrientedPoint>();
		float num = 1f / (float)subdivisions;
		float num2 = 0f;
		Vector3 vector = Vector3.zero;
		for (int i = 0; i <= subdivisions; i++)
		{
			float t = (float)i * num;
			Vector3 point = BezierSpline.GetPoint(p0, p1, p2, p3, t);
			Vector3 tangent = BezierSpline.GetTangent(p0, p1, p2, p3, t);
			Vector3 normal = BezierSpline.GetNormal(p0, p1, p2, p3, t);
			if (i > 0)
			{
				num2 += (point - vector).magnitude;
			}
			Quaternion rotation = Quaternion.identity;
			rotation = Quaternion.LookRotation(tangent, normal);
			PipeRenderer.OrientedPoint item = new PipeRenderer.OrientedPoint
			{
				position = point,
				tangent = tangent,
				normal = normal,
				rotation = rotation,
				rotationalAxisVector = Vector3.forward,
				uv = Vector2.one * num2
			};
			list.Add(item);
			vector = point;
		}
		PipeRenderer.OrientedPoint[] result = list.ToArray();
		PipeHelperFunctions.RecalculateNormals(ref result);
		return result;
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x0002F3B4 File Offset: 0x0002D5B4
	public override PipeRenderer.OrientedPoint[] GenerateShape()
	{
		List<PipeRenderer.OrientedPoint> list = new List<PipeRenderer.OrientedPoint>();
		float num = 1f / (float)this.subdivisions;
		Vector3 p = this.points[0];
		Vector3 p2 = this.points[1];
		Vector3 p3 = this.points[2];
		Vector3 p4 = this.points[3];
		float num2 = 0f;
		Vector3 vector = Vector3.zero;
		for (int i = 0; i <= this.subdivisions; i++)
		{
			float t = (float)i * num;
			Vector3 point = BezierSpline.GetPoint(p, p2, p3, p4, t);
			Vector3 tangent = BezierSpline.GetTangent(p, p2, p3, p4, t);
			Vector3 normal = BezierSpline.GetNormal(p, p2, p3, p4, t);
			if (i > 0)
			{
				num2 += (point - vector).magnitude;
			}
			Quaternion rotation = Quaternion.identity;
			rotation = Quaternion.LookRotation(tangent, normal);
			PipeRenderer.OrientedPoint item = new PipeRenderer.OrientedPoint
			{
				position = point,
				tangent = tangent,
				normal = normal,
				rotation = rotation,
				rotationalAxisVector = Vector3.forward,
				uv = Vector2.one * num2
			};
			list.Add(item);
			vector = point;
		}
		PipeRenderer.OrientedPoint[] result = list.ToArray();
		PipeHelperFunctions.RecalculateNormals(ref result);
		return result;
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x0002F504 File Offset: 0x0002D704
	private void OnDrawGizmosSelected()
	{
		if (this.drawGizmos)
		{
			Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
			for (int i = 0; i < this.points.Length; i++)
			{
				Gizmos.DrawWireCube(localToWorldMatrix.MultiplyPoint(this.points[i]), Vector3.one * 0.1f);
			}
			float num = 1f / (float)this.subdivisions;
			for (int j = 0; j < this.subdivisions; j++)
			{
				Vector3 vector = BezierSpline.GetPoint(this.points[0], this.points[1], this.points[2], this.points[3], num * (float)j);
				Vector3 vector2 = BezierSpline.GetPoint(this.points[0], this.points[1], this.points[2], this.points[3], num * (float)(j + 1));
				vector = localToWorldMatrix.MultiplyPoint(vector);
				vector2 = localToWorldMatrix.MultiplyPoint(vector2);
				Gizmos.DrawLine(vector, vector2);
				Vector3 vector3 = BezierSpline.GetTangent(this.points[0], this.points[1], this.points[2], this.points[3], num * (float)j);
				vector3 = localToWorldMatrix.MultiplyVector(vector3);
				Vector3 vector4 = vector + vector3 * 0.1f;
				Gizmos.DrawLine(vector, vector4);
			}
		}
	}

	// Token: 0x0400096A RID: 2410
	public PipeRenderer pipeRenderer;

	// Token: 0x0400096B RID: 2411
	public Vector3[] points = new Vector3[4];

	// Token: 0x0400096C RID: 2412
	public int subdivisions = 12;

	// Token: 0x0400096D RID: 2413
	public bool drawGizmos;
}
