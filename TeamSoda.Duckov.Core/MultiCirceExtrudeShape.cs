using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014B RID: 331
public class MultiCirceExtrudeShape : ShapeProvider
{
	// Token: 0x06000A9D RID: 2717 RVA: 0x0002DE2C File Offset: 0x0002C02C
	public override PipeRenderer.OrientedPoint[] GenerateShape()
	{
		List<PipeRenderer.OrientedPoint> list = new List<PipeRenderer.OrientedPoint>();
		float num = 360f / (float)this.subdivision;
		float num2 = 1f / (float)(this.subdivision * this.circles.Length);
		for (int i = 0; i < this.circles.Length; i++)
		{
			MultiCirceExtrudeShape.Circle circle = this.circles[i];
			float radius = circle.radius;
			Vector3 origin = circle.origin;
			Vector3 vector = Vector3.up * radius;
			for (int j = 0; j < this.subdivision; j++)
			{
				Quaternion quaternion = Quaternion.AngleAxis(num * (float)j, Vector3.forward);
				Vector3 position = origin + quaternion * vector;
				list.Add(new PipeRenderer.OrientedPoint
				{
					position = position,
					rotation = quaternion,
					uv = num2 * (float)(i * this.subdivision + j) * Vector2.one
				});
			}
			list.Add(new PipeRenderer.OrientedPoint
			{
				position = vector,
				rotation = Quaternion.AngleAxis(0f, Vector3.forward),
				uv = num2 * (float)((i + 1) * this.subdivision) * Vector2.one
			});
		}
		return list.ToArray();
	}

	// Token: 0x04000954 RID: 2388
	public MultiCirceExtrudeShape.Circle[] circles;

	// Token: 0x04000955 RID: 2389
	public int subdivision = 4;

	// Token: 0x020004C4 RID: 1220
	[Serializable]
	public struct Circle
	{
		// Token: 0x04001D12 RID: 7442
		public Vector3 origin;

		// Token: 0x04001D13 RID: 7443
		public float radius;
	}
}
