using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Drawing
{
	// Token: 0x0200004D RID: 77
	public static class DrawingUtilities
	{
		// Token: 0x06000277 RID: 631 RVA: 0x0000B738 File Offset: 0x00009938
		public static Bounds BoundsFrom(GameObject gameObject)
		{
			return DrawingUtilities.BoundsFrom(gameObject.transform);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000B748 File Offset: 0x00009948
		public static Bounds BoundsFrom(Transform transform)
		{
			transform.gameObject.GetComponents<Component>(DrawingUtilities.componentBuffer);
			Bounds result = new Bounds(transform.position, Vector3.zero);
			for (int i = 0; i < DrawingUtilities.componentBuffer.Count; i++)
			{
				Component component = DrawingUtilities.componentBuffer[i];
				Collider collider = component as Collider;
				if (collider != null)
				{
					result.Encapsulate(collider.bounds);
				}
				else
				{
					Collider2D collider2D = component as Collider2D;
					if (collider2D != null)
					{
						result.Encapsulate(collider2D.bounds);
					}
					else
					{
						MeshRenderer meshRenderer = component as MeshRenderer;
						if (meshRenderer != null)
						{
							result.Encapsulate(meshRenderer.bounds);
						}
						else
						{
							SpriteRenderer spriteRenderer = component as SpriteRenderer;
							if (spriteRenderer != null)
							{
								result.Encapsulate(spriteRenderer.bounds);
							}
						}
					}
				}
			}
			DrawingUtilities.componentBuffer.Clear();
			int childCount = transform.childCount;
			for (int j = 0; j < childCount; j++)
			{
				result.Encapsulate(DrawingUtilities.BoundsFrom(transform.GetChild(j)));
			}
			return result;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000B840 File Offset: 0x00009A40
		public static Bounds BoundsFrom(List<Vector3> points)
		{
			if (points.Count == 0)
			{
				throw new ArgumentException("At least 1 point is required");
			}
			Vector3 vector = points[0];
			Vector3 vector2 = points[0];
			for (int i = 0; i < points.Count; i++)
			{
				vector = Vector3.Min(vector, points[i]);
				vector2 = Vector3.Max(vector2, points[i]);
			}
			return new Bounds((vector2 + vector) * 0.5f, (vector2 - vector) * 0.5f);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000B8C4 File Offset: 0x00009AC4
		public static Bounds BoundsFrom(Vector3[] points)
		{
			if (points.Length == 0)
			{
				throw new ArgumentException("At least 1 point is required");
			}
			Vector3 vector = points[0];
			Vector3 vector2 = points[0];
			for (int i = 0; i < points.Length; i++)
			{
				vector = Vector3.Min(vector, points[i]);
				vector2 = Vector3.Max(vector2, points[i]);
			}
			return new Bounds((vector2 + vector) * 0.5f, (vector2 - vector) * 0.5f);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000B944 File Offset: 0x00009B44
		public static Bounds BoundsFrom(NativeArray<float3> points)
		{
			if (points.Length == 0)
			{
				throw new ArgumentException("At least 1 point is required");
			}
			float3 @float = points[0];
			float3 float2 = points[0];
			for (int i = 0; i < points.Length; i++)
			{
				@float = math.min(@float, points[i]);
				float2 = math.max(float2, points[i]);
			}
			return new Bounds((float2 + @float) * 0.5f, (float2 - @float) * 0.5f);
		}

		// Token: 0x04000133 RID: 307
		private static List<Component> componentBuffer = new List<Component>();
	}
}
