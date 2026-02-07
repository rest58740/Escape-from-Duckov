using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.MiniGames.BubblePoppers
{
	// Token: 0x020002EA RID: 746
	public class BubblePopperLayout : MiniGameBehaviour
	{
		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x0600181A RID: 6170 RVA: 0x00059056 File Offset: 0x00057256
		public Vector2 XPositionBorder
		{
			get
			{
				return new Vector2((float)this.xBorder.x * this.BubbleRadius * 2f - this.BubbleRadius, (float)this.xBorder.y * this.BubbleRadius * 2f);
			}
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x00059098 File Offset: 0x00057298
		public Vector2 CoordToLocalPosition(Vector2Int coord)
		{
			float bubbleRadius = this.BubbleRadius;
			return new Vector2(((coord.y % 2 != 0) ? bubbleRadius : 0f) + (float)coord.x * bubbleRadius * 2f, (float)coord.y * bubbleRadius * BubblePopperLayout.YOffsetFactor);
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x000590E8 File Offset: 0x000572E8
		public Vector2Int LocalPositionToCoord(Vector2 localPosition)
		{
			float bubbleRadius = this.BubbleRadius;
			int num = Mathf.RoundToInt(localPosition.y / bubbleRadius / BubblePopperLayout.YOffsetFactor);
			float num2 = (num % 2 != 0) ? bubbleRadius : 0f;
			return new Vector2Int(Mathf.RoundToInt((localPosition.x - num2) / bubbleRadius / 2f), num);
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x0005913C File Offset: 0x0005733C
		public Vector2Int WorldPositionToCoord(Vector2 position)
		{
			Vector3 vector = base.transform.worldToLocalMatrix.MultiplyPoint(position);
			return this.LocalPositionToCoord(vector);
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x00059170 File Offset: 0x00057370
		public Vector2Int[] GetAllNeighbourCoords(Vector2Int center, bool includeCenter)
		{
			int num = (center.y % 2 != 0) ? 0 : -1;
			Vector2Int[] result;
			if (includeCenter)
			{
				result = new Vector2Int[]
				{
					new Vector2Int(center.x + num, center.y + 1),
					new Vector2Int(center.x + num + 1, center.y + 1),
					new Vector2Int(center.x - 1, center.y),
					center,
					new Vector2Int(center.x + 1, center.y),
					new Vector2Int(center.x + num, center.y - 1),
					new Vector2Int(center.x + num + 1, center.y - 1)
				};
			}
			else
			{
				result = new Vector2Int[]
				{
					new Vector2Int(center.x + num, center.y + 1),
					new Vector2Int(center.x + num + 1, center.y + 1),
					new Vector2Int(center.x - 1, center.y),
					new Vector2Int(center.x + 1, center.y),
					new Vector2Int(center.x + num, center.y - 1),
					new Vector2Int(center.x + num + 1, center.y - 1)
				};
			}
			return result;
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x0005931C File Offset: 0x0005751C
		public List<Vector2Int> GetAllPassingCoords(Vector2 localOrigin, Vector2 direction, float length)
		{
			float num = this.BubbleRadius * 2f;
			List<Vector2Int> list = new List<Vector2Int>
			{
				this.LocalPositionToCoord(localOrigin)
			};
			if (num > 0f)
			{
				float num2 = -num;
				while (num2 < length)
				{
					num2 += num;
					Vector2 localPosition = localOrigin + num2 * direction;
					Vector2Int center = this.LocalPositionToCoord(localPosition);
					list.AddRange(this.GetAllNeighbourCoords(center, true));
				}
			}
			return list;
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x00059388 File Offset: 0x00057588
		private void OnDrawGizmos()
		{
			float bubbleRadius = this.BubbleRadius;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(new Vector3(this.XPositionBorder.x, 0f), new Vector3(this.XPositionBorder.x, -100f));
			Gizmos.DrawLine(new Vector3(this.XPositionBorder.y, 0f), new Vector3(this.XPositionBorder.y, -100f));
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x00059414 File Offset: 0x00057614
		public void GizmosDrawCoord(Vector2Int coord, float ratio)
		{
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawSphere(this.CoordToLocalPosition(coord), this.BubbleRadius * ratio);
			Gizmos.matrix = matrix;
		}

		// Token: 0x040011A7 RID: 4519
		[SerializeField]
		private Vector2Int xBorder;

		// Token: 0x040011A8 RID: 4520
		public Vector2Int XCoordBorder;

		// Token: 0x040011A9 RID: 4521
		public float BubbleRadius = 8f;

		// Token: 0x040011AA RID: 4522
		public static readonly float YOffsetFactor = Mathf.Tan(1.0471976f);

		// Token: 0x040011AB RID: 4523
		[SerializeField]
		private Transform tester;

		// Token: 0x040011AC RID: 4524
		[SerializeField]
		private float distance = 10f;

		// Token: 0x040011AD RID: 4525
		[SerializeField]
		private Vector2Int min;

		// Token: 0x040011AE RID: 4526
		[SerializeField]
		private Vector2Int max;
	}
}
