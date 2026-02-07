using System;
using UnityEngine;

namespace ParadoxNotion
{
	// Token: 0x02000078 RID: 120
	public static class CurveUtils
	{
		// Token: 0x0600048E RID: 1166 RVA: 0x0000CC38 File Offset: 0x0000AE38
		public static Vector2 GetPosAlongCurve(Vector2 from, Vector2 to, Vector2 fromTangent, Vector2 toTangent, float t)
		{
			float num = 1f - t;
			float num2 = t * t;
			float num3 = num * num;
			float d = num3 * num;
			float d2 = num2 * t;
			return d * from + 3f * num3 * t * (from + fromTangent) + 3f * num * num2 * (to + toTangent) + d2 * to;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000CCA8 File Offset: 0x0000AEA8
		public static bool IsPosAlongCurve(Vector2 from, Vector2 to, Vector2 fromTangent, Vector2 toTangent, Vector2 targetPosition)
		{
			float num = 0f;
			return CurveUtils.IsPosAlongCurve(from, to, fromTangent, toTangent, targetPosition, out num);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0000CCC8 File Offset: 0x0000AEC8
		public static bool IsPosAlongCurve(Vector2 from, Vector2 to, Vector2 fromTangent, Vector2 toTangent, Vector2 targetPosition, out float norm)
		{
			if (RectUtils.GetBoundRect(new Vector2[]
			{
				from,
				to
			}).ExpandBy(10f).Contains(targetPosition))
			{
				for (float num = 0f; num <= 100f; num += 1f)
				{
					Vector2 posAlongCurve = CurveUtils.GetPosAlongCurve(from, to, fromTangent, toTangent, num / 100f);
					if (Vector2.Distance(targetPosition, posAlongCurve) < 10f)
					{
						norm = num / 100f;
						return true;
					}
				}
			}
			norm = 0f;
			return false;
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000CD54 File Offset: 0x0000AF54
		public static void ResolveTangents(Vector2 from, Vector2 to, float rigidMlt, PlanarDirection direction, out Vector2 fromTangent, out Vector2 toTangent)
		{
			Rect fromRect = new Rect(0f, 0f, 1f, 1f);
			Rect toRect = new Rect(0f, 0f, 1f, 1f);
			fromRect.center = from;
			toRect.center = to;
			CurveUtils.ResolveTangents(from, to, fromRect, toRect, rigidMlt, direction, out fromTangent, out toTangent);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000CDB8 File Offset: 0x0000AFB8
		public static void ResolveTangents(Vector2 from, Vector2 to, Rect fromRect, Rect toRect, float rigidMlt, PlanarDirection direction, out Vector2 fromTangent, out Vector2 toTangent)
		{
			float num = Mathf.Abs(from.x - to.x) * rigidMlt;
			num = Mathf.Max(num, 25f);
			float num2 = Mathf.Abs(from.y - to.y) * rigidMlt;
			num2 = Mathf.Max(num2, 25f);
			switch (direction)
			{
			case PlanarDirection.Horizontal:
				fromTangent = new Vector2(num, 0f);
				toTangent = new Vector2(-num, 0f);
				return;
			case PlanarDirection.Vertical:
				fromTangent = new Vector2(0f, num2);
				toTangent = new Vector2(0f, -num2);
				return;
			case PlanarDirection.Auto:
			{
				Vector2 vector = default(Vector2);
				if (from.x <= fromRect.xMin)
				{
					vector = new Vector2(-num, 0f);
				}
				if (from.x >= fromRect.xMax)
				{
					vector = new Vector2(num, 0f);
				}
				if (from.y <= fromRect.yMin)
				{
					vector = new Vector2(0f, -num2);
				}
				if (from.y >= fromRect.yMax)
				{
					vector = new Vector2(0f, num2);
				}
				Vector2 vector2 = default(Vector2);
				if (to.x <= toRect.xMin)
				{
					vector2 = new Vector2(-num, 0f);
				}
				if (to.x >= toRect.xMax)
				{
					vector2 = new Vector2(num, 0f);
				}
				if (to.y <= toRect.yMin)
				{
					vector2 = new Vector2(0f, -num2);
				}
				if (to.y >= toRect.yMax)
				{
					vector2 = new Vector2(0f, num2);
				}
				fromTangent = vector;
				toTangent = vector2;
				return;
			}
			default:
				fromTangent = default(Vector2);
				toTangent = default(Vector2);
				return;
			}
		}

		// Token: 0x04000178 RID: 376
		private const float POS_CHECK_RES = 100f;

		// Token: 0x04000179 RID: 377
		private const float POS_CHECK_DISTANCE = 10f;
	}
}
