using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000150 RID: 336
public class MultipleBezierShape : ShapeProvider
{
	// Token: 0x06000AB4 RID: 2740 RVA: 0x0002F6A0 File Offset: 0x0002D8A0
	public override PipeRenderer.OrientedPoint[] GenerateShape()
	{
		List<PipeRenderer.OrientedPoint> list = new List<PipeRenderer.OrientedPoint>();
		for (int i = 0; i < this.points.Length / 4; i++)
		{
			Vector3 p = this.points[i * 4];
			Vector3 p2 = this.points[i * 4 + 1];
			Vector3 p3 = this.points[i * 4 + 2];
			Vector3 p4 = this.points[i * 4 + 3];
			PipeRenderer.OrientedPoint[] collection = BezierSpline.GenerateShape(p, p2, p3, p4, this.subdivisions);
			if (list.Count > 0)
			{
				list.RemoveAt(list.Count - 1);
			}
			list.AddRange(collection);
		}
		PipeRenderer.OrientedPoint[] result = list.ToArray();
		PipeHelperFunctions.RecalculateNormals(ref result);
		PipeHelperFunctions.RecalculateUvs(ref result, 1f, 0f);
		PipeHelperFunctions.RotatePoints(ref result, this.rotationOffset, this.twist);
		return result;
	}

	// Token: 0x0400096E RID: 2414
	public Vector3[] points;

	// Token: 0x0400096F RID: 2415
	public int subdivisions = 16;

	// Token: 0x04000970 RID: 2416
	public bool lockedHandles;

	// Token: 0x04000971 RID: 2417
	public float rotationOffset;

	// Token: 0x04000972 RID: 2418
	public float twist;

	// Token: 0x04000973 RID: 2419
	public bool edit;
}
