using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace UI_Spline_Renderer
{
	// Token: 0x0200000E RID: 14
	public static class SplineExtensions
	{
		// Token: 0x0600003B RID: 59 RVA: 0x000032EC File Offset: 0x000014EC
		public static void ReorientKnot(this Spline spline, int index, bool withoutNotify = false)
		{
			BezierKnot bezierKnot = spline[index];
			Quaternion quaternion = Quaternion.LookRotation(Vector3.ProjectOnPlane(bezierKnot.Rotation * Vector3.forward, Vector3.back), Vector3.back);
			bezierKnot.Rotation = quaternion;
			if (withoutNotify)
			{
				spline.SetKnotNoNotify(index, bezierKnot, 1);
				return;
			}
			spline.SetKnot(index, bezierKnot, 1);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003350 File Offset: 0x00001550
		public static void ReorientKnots(this SplineContainer container, bool withoutNotify = false)
		{
			foreach (Spline spline in container.Splines)
			{
				for (int i = 0; i < spline.Count; i++)
				{
					spline.ReorientKnot(i, withoutNotify);
				}
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000033B0 File Offset: 0x000015B0
		public static void ReorientKnotsAndSmooth(this SplineContainer container)
		{
			foreach (Spline spline in container.Splines)
			{
				for (int i = 0; i < spline.Count; i++)
				{
					BezierKnot bezierKnot = spline[i];
					float3 @float = (i == 0) ? bezierKnot.Position : spline[i - 1].Position;
					float3 float2 = (i == spline.Count - 1) ? spline[i].Position : spline[i + 1].Position;
					bezierKnot = SplineUtility.GetAutoSmoothKnot(bezierKnot.Position, @float, float2, new float3(0f, 0f, -1f));
					spline.SetKnot(i, bezierKnot, 1);
				}
			}
		}
	}
}
