using System;
using UnityEngine;

namespace ParadoxNotion
{
	// Token: 0x0200007C RID: 124
	public static class RectUtils
	{
		// Token: 0x060004AC RID: 1196 RVA: 0x0000D340 File Offset: 0x0000B540
		public static Rect GetBoundRect(params Rect[] rects)
		{
			float num = float.PositiveInfinity;
			float num2 = float.NegativeInfinity;
			float num3 = float.PositiveInfinity;
			float num4 = float.NegativeInfinity;
			for (int i = 0; i < rects.Length; i++)
			{
				num = Mathf.Min(num, rects[i].xMin);
				num2 = Mathf.Max(num2, rects[i].xMax);
				num3 = Mathf.Min(num3, rects[i].yMin);
				num4 = Mathf.Max(num4, rects[i].yMax);
			}
			return Rect.MinMaxRect(num, num3, num2, num4);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0000D3D0 File Offset: 0x0000B5D0
		public static Rect GetBoundRect(params Vector2[] positions)
		{
			float num = float.PositiveInfinity;
			float num2 = float.NegativeInfinity;
			float num3 = float.PositiveInfinity;
			float num4 = float.NegativeInfinity;
			for (int i = 0; i < positions.Length; i++)
			{
				num = Mathf.Min(num, positions[i].x);
				num2 = Mathf.Max(num2, positions[i].x);
				num3 = Mathf.Min(num3, positions[i].y);
				num4 = Mathf.Max(num4, positions[i].y);
			}
			return Rect.MinMaxRect(num, num3, num2, num4);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0000D460 File Offset: 0x0000B660
		public static bool Encapsulates(this Rect a, Rect b)
		{
			return a.x < b.x && a.xMax > b.xMax && a.y < b.y && a.yMax > b.yMax;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0000D4AF File Offset: 0x0000B6AF
		public static Rect ExpandBy(this Rect rect, float margin)
		{
			return rect.ExpandBy(margin, margin);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0000D4B9 File Offset: 0x0000B6B9
		public static Rect ExpandBy(this Rect rect, float xMargin, float yMargin)
		{
			return rect.ExpandBy(xMargin, yMargin, xMargin, yMargin);
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0000D4C5 File Offset: 0x0000B6C5
		public static Rect ExpandBy(this Rect rect, float left, float top, float right, float bottom)
		{
			return Rect.MinMaxRect(rect.xMin - left, rect.yMin - top, rect.xMax + right, rect.yMax + bottom);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0000D4F4 File Offset: 0x0000B6F4
		public static Rect TransformSpace(this Rect rect, Rect oldContainer, Rect newContainer)
		{
			return new Rect
			{
				xMin = Mathf.Lerp(newContainer.xMin, newContainer.xMax, Mathf.InverseLerp(oldContainer.xMin, oldContainer.xMax, rect.xMin)),
				xMax = Mathf.Lerp(newContainer.xMin, newContainer.xMax, Mathf.InverseLerp(oldContainer.xMin, oldContainer.xMax, rect.xMax)),
				yMin = Mathf.Lerp(newContainer.yMin, newContainer.yMax, Mathf.InverseLerp(oldContainer.yMin, oldContainer.yMax, rect.yMin)),
				yMax = Mathf.Lerp(newContainer.yMin, newContainer.yMax, Mathf.InverseLerp(oldContainer.yMin, oldContainer.yMax, rect.yMax))
			};
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0000D5DC File Offset: 0x0000B7DC
		public static Vector2 TransformSpace(this Vector2 vector, Rect oldContainer, Rect newContainer)
		{
			return new Vector2
			{
				x = Mathf.Lerp(newContainer.xMin, newContainer.xMax, Mathf.InverseLerp(oldContainer.xMin, oldContainer.xMax, vector.x)),
				y = Mathf.Lerp(newContainer.yMin, newContainer.yMax, Mathf.InverseLerp(oldContainer.yMin, oldContainer.yMax, vector.y))
			};
		}
	}
}
