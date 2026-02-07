using System;
using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
	// Token: 0x02000044 RID: 68
	internal class CubicBezierDecoder : ABSPathDecoder
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000DF10 File Offset: 0x0000C110
		internal override int minInputWaypoints
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000DF14 File Offset: 0x0000C114
		internal override void FinalizePath(Path p, Vector3[] wps, bool isClosedPath)
		{
			if (isClosedPath && !p.addedExtraEndWp)
			{
				isClosedPath = false;
			}
			int num = wps.Length;
			int num2 = p.addedExtraStartWp ? 1 : 0;
			if (p.addedExtraEndWp)
			{
				num2++;
			}
			if (num < 3 + num2 || (num - num2) % 3 != 0)
			{
				Debug.LogError("CubicBezier paths must contain waypoints in multiple of 3 excluding the starting point added automatically by DOTween (1: waypoint, 2: IN control point, 3: OUT control point — the minimum amount of waypoints for a single curve is 3)");
				return;
			}
			int num3 = num2 + (num - num2) / 3;
			Vector3[] array = new Vector3[num3];
			p.controlPoints = new ControlPoint[num3 - 1];
			array[0] = wps[0];
			int num4 = 1;
			int num5 = 0;
			for (int i = 3 + (p.addedExtraStartWp ? 0 : 2); i < num; i += 3)
			{
				array[num4] = wps[i - 2];
				num4++;
				p.controlPoints[num5] = new ControlPoint(wps[i - 1], wps[i]);
				num5++;
			}
			p.wps = array;
			if (isClosedPath)
			{
				Vector3 vector = p.wps[p.wps.Length - 2];
				Vector3 a = p.wps[0];
				Vector3 b = p.controlPoints[p.controlPoints.Length - 2].b;
				Vector3 a2 = p.controlPoints[0].a;
				float magnitude = (a - vector).magnitude;
				p.controlPoints[p.controlPoints.Length - 1] = new ControlPoint(vector + Vector3.ClampMagnitude(vector - b, magnitude), a + Vector3.ClampMagnitude(a - a2, magnitude));
			}
			p.subdivisions = num3 * p.subdivisionsXSegment;
			this.SetTimeToLengthTables(p, p.subdivisions);
			this.SetWaypointsLengths(p, p.subdivisionsXSegment);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000E0DC File Offset: 0x0000C2DC
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
			Vector3 a = wps[num3];
			Vector3 a2 = controlPoints[num3].a;
			Vector3 b = controlPoints[num3].b;
			Vector3 a3 = wps[num3 + 1];
			float num5 = 1f - num4;
			float num6 = num4 * num4;
			float num7 = num5 * num5;
			float d = num7 * num5;
			float d2 = num6 * num4;
			return d * a + 3f * num7 * num4 * a2 + 3f * num5 * num6 * b + d2 * a3;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000E1A4 File Offset: 0x0000C3A4
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

		// Token: 0x06000272 RID: 626 RVA: 0x0000E24C File Offset: 0x0000C44C
		internal void SetWaypointsLengths(Path p, int subdivisions)
		{
			int num = p.wps.Length;
			float[] array = new float[num];
			array[0] = 0f;
			for (int i = 1; i < num; i++)
			{
				CubicBezierDecoder._PartialControlPs[0] = p.controlPoints[i - 1];
				CubicBezierDecoder._PartialWps[0] = p.wps[i - 1];
				CubicBezierDecoder._PartialWps[1] = p.wps[i];
				float num2 = 0f;
				float num3 = 1f / (float)subdivisions;
				Vector3 b = this.GetPoint(0f, CubicBezierDecoder._PartialWps, p, CubicBezierDecoder._PartialControlPs);
				for (int j = 1; j < subdivisions + 1; j++)
				{
					float perc = num3 * (float)j;
					Vector3 point = this.GetPoint(perc, CubicBezierDecoder._PartialWps, p, CubicBezierDecoder._PartialControlPs);
					num2 += Vector3.Distance(point, b);
					b = point;
				}
				array[i] = num2;
			}
			p.wpLengths = array;
		}

		// Token: 0x04000123 RID: 291
		private static readonly ControlPoint[] _PartialControlPs = new ControlPoint[1];

		// Token: 0x04000124 RID: 292
		private static readonly Vector3[] _PartialWps = new Vector3[2];
	}
}
