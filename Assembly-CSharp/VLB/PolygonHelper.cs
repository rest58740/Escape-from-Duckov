using System;
using System.Collections.Generic;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000036 RID: 54
	public class PolygonHelper : MonoBehaviour
	{
		// Token: 0x020000B7 RID: 183
		public struct Plane2D
		{
			// Token: 0x060004C7 RID: 1223 RVA: 0x00013917 File Offset: 0x00011B17
			public float Distance(Vector2 point)
			{
				return Vector2.Dot(this.normal, point) + this.distance;
			}

			// Token: 0x060004C8 RID: 1224 RVA: 0x0001392C File Offset: 0x00011B2C
			public Vector2 ClosestPoint(Vector2 pt)
			{
				return pt - this.normal * this.Distance(pt);
			}

			// Token: 0x060004C9 RID: 1225 RVA: 0x00013948 File Offset: 0x00011B48
			public Vector2 Intersect(Vector2 p1, Vector2 p2)
			{
				float num = Vector2.Dot(this.normal, p1 - p2);
				if (Utils.IsAlmostZero(num))
				{
					return (p1 + p2) * 0.5f;
				}
				float d = (this.normal.x * p1.x + this.normal.y * p1.y + this.distance) / num;
				return p1 + d * (p2 - p1);
			}

			// Token: 0x060004CA RID: 1226 RVA: 0x000139C4 File Offset: 0x00011BC4
			public bool GetSide(Vector2 point)
			{
				return this.Distance(point) > 0f;
			}

			// Token: 0x060004CB RID: 1227 RVA: 0x000139D4 File Offset: 0x00011BD4
			public static PolygonHelper.Plane2D FromPoints(Vector3 p1, Vector3 p2)
			{
				Vector3 normalized = (p2 - p1).normalized;
				return new PolygonHelper.Plane2D
				{
					normal = new Vector2(normalized.y, -normalized.x),
					distance = -normalized.y * p1.x + normalized.x * p1.y
				};
			}

			// Token: 0x060004CC RID: 1228 RVA: 0x00013A38 File Offset: 0x00011C38
			public static PolygonHelper.Plane2D FromNormalAndPoint(Vector3 normalizedNormal, Vector3 p1)
			{
				return new PolygonHelper.Plane2D
				{
					normal = normalizedNormal,
					distance = -normalizedNormal.x * p1.x - normalizedNormal.y * p1.y
				};
			}

			// Token: 0x060004CD RID: 1229 RVA: 0x00013A7E File Offset: 0x00011C7E
			public void Flip()
			{
				this.normal = -this.normal;
				this.distance = -this.distance;
			}

			// Token: 0x060004CE RID: 1230 RVA: 0x00013AA0 File Offset: 0x00011CA0
			public Vector2[] CutConvex(Vector2[] poly)
			{
				List<Vector2> list = new List<Vector2>(poly.Length);
				Vector2 vector = poly[poly.Length - 1];
				foreach (Vector2 vector2 in poly)
				{
					bool side = this.GetSide(vector);
					bool side2 = this.GetSide(vector2);
					if (side && side2)
					{
						list.Add(vector2);
					}
					else if (side && !side2)
					{
						list.Add(this.Intersect(vector, vector2));
					}
					else if (!side && side2)
					{
						list.Add(this.Intersect(vector, vector2));
						list.Add(vector2);
					}
					vector = vector2;
				}
				return list.ToArray();
			}

			// Token: 0x060004CF RID: 1231 RVA: 0x00013B42 File Offset: 0x00011D42
			public override string ToString()
			{
				return string.Format("{0} x {1} + {2}", this.normal.x, this.normal.y, this.distance);
			}

			// Token: 0x040003C0 RID: 960
			public Vector2 normal;

			// Token: 0x040003C1 RID: 961
			public float distance;
		}
	}
}
