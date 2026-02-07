using System;
using UnityEngine;

// Token: 0x0200014A RID: 330
[RequireComponent(typeof(PipeRenderer))]
public class CircularExtrudeShape : ShapeProvider
{
	// Token: 0x06000A9A RID: 2714 RVA: 0x0002DCA4 File Offset: 0x0002BEA4
	public override PipeRenderer.OrientedPoint[] GenerateShape()
	{
		Vector3 vector = Vector3.up * this.radius;
		float num = 360f / (float)this.subdivision;
		float num2 = 1f / (float)this.subdivision;
		PipeRenderer.OrientedPoint[] array = new PipeRenderer.OrientedPoint[this.subdivision + 1];
		for (int i = 0; i < this.subdivision; i++)
		{
			Quaternion quaternion = Quaternion.AngleAxis(num * (float)i, Vector3.forward);
			Vector3 position = quaternion * vector + this.offset;
			array[i] = new PipeRenderer.OrientedPoint
			{
				position = position,
				rotation = quaternion,
				uv = num2 * (float)i * Vector2.one
			};
		}
		array[this.subdivision] = new PipeRenderer.OrientedPoint
		{
			position = vector + this.offset,
			rotation = Quaternion.AngleAxis(0f, Vector3.forward),
			uv = Vector2.one
		};
		return array;
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x0002DDB0 File Offset: 0x0002BFB0
	private void OnDrawGizmosSelected()
	{
		if (this.pipeRenderer == null)
		{
			this.pipeRenderer = base.GetComponent<PipeRenderer>();
		}
		if (this.pipeRenderer != null && this.pipeRenderer.extrudeShapeProvider == null)
		{
			this.pipeRenderer.extrudeShapeProvider = this;
		}
	}

	// Token: 0x04000950 RID: 2384
	public PipeRenderer pipeRenderer;

	// Token: 0x04000951 RID: 2385
	public float radius = 1f;

	// Token: 0x04000952 RID: 2386
	public int subdivision = 12;

	// Token: 0x04000953 RID: 2387
	public Vector3 offset = Vector3.zero;
}
