using System;
using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
	// Token: 0x02000047 RID: 71
	internal class CatmullRomDecoder : ABSPathDecoder
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000E3E8 File Offset: 0x0000C5E8
		internal override int minInputWaypoints
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000E3EC File Offset: 0x0000C5EC
		internal override void FinalizePath(Path p, Vector3[] wps, bool isClosedPath)
		{
			int num = wps.Length;
			if (p.controlPoints == null || p.controlPoints.Length != 2)
			{
				p.controlPoints = new ControlPoint[2];
			}
			if (isClosedPath)
			{
				p.controlPoints[0] = new ControlPoint(wps[num - 2], Vector3.zero);
				p.controlPoints[1] = new ControlPoint(wps[1], Vector3.zero);
			}
			else
			{
				p.controlPoints[0] = new ControlPoint(wps[1], Vector3.zero);
				Vector3 a = wps[num - 1];
				Vector3 b = a - wps[num - 2];
				p.controlPoints[1] = new ControlPoint(a + b, Vector3.zero);
			}
			p.subdivisions = num * p.subdivisionsXSegment;
			this.SetTimeToLengthTables(p, p.subdivisions);
			this.SetWaypointsLengths(p, p.subdivisionsXSegment);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000E4DC File Offset: 0x0000C6DC
		internal override Vector3 GetPoint(float perc, Vector3[] wps, Path p, ControlPoint[] controlPoints)
		{
			int num = wps.Length - 1;
			int num2 = (int)Math.Floor((double)(perc * (float)num));
			int num3 = num - 1;
			if (num3 > num2)
			{
				num3 = num2;
			}
			float num4 = perc * (float)num - (float)num3;
			Vector3 a = (num3 == 0) ? controlPoints[0].a : wps[num3 - 1];
			Vector3 a2 = wps[num3];
			Vector3 vector = wps[num3 + 1];
			Vector3 b = (num3 + 2 > wps.Length - 1) ? controlPoints[1].a : wps[num3 + 2];
			return 0.5f * ((-a + 3f * a2 - 3f * vector + b) * (num4 * num4 * num4) + (2f * a - 5f * a2 + 4f * vector - b) * (num4 * num4) + (-a + vector) * num4 + 2f * a2);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000E614 File Offset: 0x0000C814
		internal void SetTimeToLengthTables(Path p, int subdivisions)
		{
			float num = 0f;
			float num2 = 1f / (float)subdivisions;
			float[] array = new float[subdivisions];
			float[] array2 = new float[subdivisions];
			Vector3 b = this.GetPoint(0f, p.wps, p, p.controlPoints);
			for (int i = 1; i < subdivisions + 1; i++)
			{
				float num3 = num2 * (float)i;
				Vector3 point = this.GetPoint(num3, p.wps, p, p.controlPoints);
				num += Vector3.Distance(point, b);
				b = point;
				array[i - 1] = num3;
				array2[i - 1] = num;
			}
			p.length = num;
			p.timesTable = array;
			p.lengthsTable = array2;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000E6BC File Offset: 0x0000C8BC
		internal void SetWaypointsLengths(Path p, int subdivisions)
		{
			int num = p.wps.Length;
			float[] array = new float[num];
			array[0] = 0f;
			for (int i = 1; i < num; i++)
			{
				CatmullRomDecoder._PartialControlPs[0].a = ((i == 1) ? p.controlPoints[0].a : p.wps[i - 2]);
				CatmullRomDecoder._PartialWps[0] = p.wps[i - 1];
				CatmullRomDecoder._PartialWps[1] = p.wps[i];
				CatmullRomDecoder._PartialControlPs[1].a = ((i == num - 1) ? p.controlPoints[1].a : p.wps[i + 1]);
				float num2 = 0f;
				float num3 = 1f / (float)subdivisions;
				Vector3 b = this.GetPoint(0f, CatmullRomDecoder._PartialWps, p, CatmullRomDecoder._PartialControlPs);
				for (int j = 1; j < subdivisions + 1; j++)
				{
					float perc = num3 * (float)j;
					Vector3 point = this.GetPoint(perc, CatmullRomDecoder._PartialWps, p, CatmullRomDecoder._PartialControlPs);
					num2 += Vector3.Distance(point, b);
					b = point;
				}
				array[i] = num2;
			}
			p.wpLengths = array;
		}

		// Token: 0x04000127 RID: 295
		private static readonly ControlPoint[] _PartialControlPs = new ControlPoint[2];

		// Token: 0x04000128 RID: 296
		private static readonly Vector3[] _PartialWps = new Vector3[2];
	}
}
