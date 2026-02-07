using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000063 RID: 99
	[Serializable]
	public struct PolylinePoint
	{
		// Token: 0x06000C9C RID: 3228 RVA: 0x00019431 File Offset: 0x00017631
		public static PolylinePoint operator +(PolylinePoint a, PolylinePoint b)
		{
			return new PolylinePoint(a.point + b.point, a.color + b.color, a.thickness + b.thickness);
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x00019467 File Offset: 0x00017667
		public static PolylinePoint operator *(PolylinePoint a, float b)
		{
			return new PolylinePoint(a.point * b, a.color * b, a.thickness * b);
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x0001948E File Offset: 0x0001768E
		public static PolylinePoint operator *(float b, PolylinePoint a)
		{
			return a * b;
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00019498 File Offset: 0x00017698
		public static PolylinePoint Lerp(PolylinePoint a, PolylinePoint b, float t)
		{
			return new PolylinePoint
			{
				point = Vector3.LerpUnclamped(a.point, b.point, t),
				color = Color.LerpUnclamped(a.color, b.color, t),
				thickness = Mathf.LerpUnclamped(a.thickness, b.thickness, t)
			};
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x000194F9 File Offset: 0x000176F9
		public PolylinePoint(Vector3 point)
		{
			this.point = point;
			this.color = Color.white;
			this.thickness = 1f;
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x00019518 File Offset: 0x00017718
		public PolylinePoint(Vector2 point)
		{
			this.point = point;
			this.color = Color.white;
			this.thickness = 1f;
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x0001953C File Offset: 0x0001773C
		public PolylinePoint(Vector3 point, Color color)
		{
			this.point = point;
			this.color = color;
			this.thickness = 1f;
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x00019557 File Offset: 0x00017757
		public PolylinePoint(Vector2 point, Color color)
		{
			this.point = point;
			this.color = color;
			this.thickness = 1f;
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x00019577 File Offset: 0x00017777
		public PolylinePoint(Vector3 point, Color color, float thickness)
		{
			this.point = point;
			this.color = color;
			this.thickness = thickness;
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0001958E File Offset: 0x0001778E
		public PolylinePoint(Vector2 point, Color color, float thickness)
		{
			this.point = point;
			this.color = color;
			this.thickness = thickness;
		}

		// Token: 0x04000212 RID: 530
		public Vector3 point;

		// Token: 0x04000213 RID: 531
		[ShapesColorField(true)]
		public Color color;

		// Token: 0x04000214 RID: 532
		public float thickness;
	}
}
