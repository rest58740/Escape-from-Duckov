using System;
using System.Collections.Generic;
using UnityEngine;

namespace DG.Tweening
{
	// Token: 0x02000008 RID: 8
	public static class DOCurve
	{
		// Token: 0x02000066 RID: 102
		public static class CubicBezier
		{
			// Token: 0x06000336 RID: 822 RVA: 0x00013270 File Offset: 0x00011470
			public static Vector3 GetPointOnSegment(Vector3 startPoint, Vector3 startControlPoint, Vector3 endPoint, Vector3 endControlPoint, float factor)
			{
				float num = 1f - factor;
				float num2 = factor * factor;
				float num3 = num * num;
				float d = num3 * num;
				float d2 = num2 * factor;
				return d * startPoint + 3f * num3 * factor * startControlPoint + 3f * num * num2 * endControlPoint + d2 * endPoint;
			}

			// Token: 0x06000337 RID: 823 RVA: 0x000132D4 File Offset: 0x000114D4
			public static Vector3[] GetSegmentPointCloud(Vector3 startPoint, Vector3 startControlPoint, Vector3 endPoint, Vector3 endControlPoint, int resolution = 10)
			{
				if (resolution < 2)
				{
					resolution = 2;
				}
				Vector3[] array = new Vector3[resolution];
				float num = 1f / (float)(resolution - 1);
				for (int i = 0; i < resolution; i++)
				{
					array[i] = DOCurve.CubicBezier.GetPointOnSegment(startPoint, startControlPoint, endPoint, endControlPoint, num * (float)i);
				}
				return array;
			}

			// Token: 0x06000338 RID: 824 RVA: 0x00013320 File Offset: 0x00011520
			public static void GetSegmentPointCloud(List<Vector3> addToList, Vector3 startPoint, Vector3 startControlPoint, Vector3 endPoint, Vector3 endControlPoint, int resolution = 10)
			{
				if (resolution < 2)
				{
					resolution = 2;
				}
				float num = 1f / (float)(resolution - 1);
				for (int i = 0; i < resolution; i++)
				{
					addToList.Add(DOCurve.CubicBezier.GetPointOnSegment(startPoint, startControlPoint, endPoint, endControlPoint, num * (float)i));
				}
			}
		}
	}
}
