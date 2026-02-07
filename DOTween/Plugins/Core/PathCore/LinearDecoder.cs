using System;
using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
	// Token: 0x02000048 RID: 72
	internal class LinearDecoder : ABSPathDecoder
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000E3E8 File Offset: 0x0000C5E8
		internal override int minInputWaypoints
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000E819 File Offset: 0x0000CA19
		internal override void FinalizePath(Path p, Vector3[] wps, bool isClosedPath)
		{
			p.controlPoints = null;
			p.subdivisions = wps.Length * p.subdivisionsXSegment;
			this.SetTimeToLengthTables(p, p.subdivisions);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000E840 File Offset: 0x0000CA40
		internal override Vector3 GetPoint(float perc, Vector3[] wps, Path p, ControlPoint[] controlPoints)
		{
			if (perc <= 0f)
			{
				p.linearWPIndex = 1;
				return wps[0];
			}
			int num = 0;
			int num2 = 0;
			int num3 = p.timesTable.Length;
			for (int i = 1; i < num3; i++)
			{
				if (p.timesTable[i] >= perc)
				{
					num = i - 1;
					num2 = i;
					break;
				}
			}
			float num4 = p.timesTable[num];
			float num5 = perc - num4;
			float maxLength = p.length * num5;
			Vector3 vector = wps[num];
			Vector3 a = wps[num2];
			p.linearWPIndex = num2;
			return vector + Vector3.ClampMagnitude(a - vector, maxLength);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000E8E0 File Offset: 0x0000CAE0
		internal void SetTimeToLengthTables(Path p, int subdivisions)
		{
			float num = 0f;
			int num2 = p.wps.Length;
			float[] array = new float[num2];
			Vector3 b = p.wps[0];
			for (int i = 0; i < num2; i++)
			{
				Vector3 vector = p.wps[i];
				float num3 = Vector3.Distance(vector, b);
				num += num3;
				b = vector;
				array[i] = num3;
			}
			float[] array2 = new float[num2];
			float num4 = 0f;
			for (int j = 1; j < num2; j++)
			{
				num4 += array[j];
				array2[j] = num4 / num;
			}
			p.length = num;
			p.wpLengths = array;
			p.timesTable = array2;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00008DCD File Offset: 0x00006FCD
		internal void SetWaypointsLengths(Path p, int subdivisions)
		{
		}
	}
}
