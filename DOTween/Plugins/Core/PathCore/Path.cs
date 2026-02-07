using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins.Core.PathCore
{
	// Token: 0x02000049 RID: 73
	[Serializable]
	public class Path
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000E989 File Offset: 0x0000CB89
		internal int minInputWaypoints
		{
			get
			{
				return this._decoder.minInputWaypoints;
			}
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000E998 File Offset: 0x0000CB98
		public Path(PathType type, Vector3[] waypoints, int subdivisionsXSegment, Color? gizmoColor = null)
		{
			this.type = type;
			this.subdivisionsXSegment = subdivisionsXSegment;
			if (gizmoColor != null)
			{
				this.gizmoColor = gizmoColor.Value;
			}
			this.AssignWaypoints(waypoints, true);
			this.AssignDecoder(type);
			if (TweenManager.isUnityEditor)
			{
				DOTween.GizmosDelegates.Add(new TweenCallback(this.Draw));
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000EA21 File Offset: 0x0000CC21
		internal Path()
		{
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000EA50 File Offset: 0x0000CC50
		internal void FinalizePath(bool isClosedPath, AxisConstraint lockPositionAxes, Vector3 currTargetVal)
		{
			if (lockPositionAxes != AxisConstraint.None)
			{
				bool flag = (lockPositionAxes & AxisConstraint.X) == AxisConstraint.X;
				bool flag2 = (lockPositionAxes & AxisConstraint.Y) == AxisConstraint.Y;
				bool flag3 = (lockPositionAxes & AxisConstraint.Z) == AxisConstraint.Z;
				for (int i = 0; i < this.wps.Length; i++)
				{
					Vector3 vector = this.wps[i];
					this.wps[i] = new Vector3(flag ? currTargetVal.x : vector.x, flag2 ? currTargetVal.y : vector.y, flag3 ? currTargetVal.z : vector.z);
				}
			}
			this._decoder.FinalizePath(this, this.wps, isClosedPath);
			this.isFinalized = true;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000EAF7 File Offset: 0x0000CCF7
		internal Vector3 GetPoint(float perc, bool convertToConstantPerc = false)
		{
			if (convertToConstantPerc)
			{
				perc = this.ConvertToConstantPathPerc(perc);
			}
			return this._decoder.GetPoint(perc, this.wps, this, this.controlPoints);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000EB20 File Offset: 0x0000CD20
		internal float ConvertToConstantPathPerc(float perc)
		{
			if (this.type == PathType.Linear)
			{
				return perc;
			}
			if (perc > 0f && perc < 1f)
			{
				if (this.length <= 0f)
				{
					return perc;
				}
				float num = this.length * perc;
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				float num5 = 0f;
				int num6 = this.lengthsTable.Length;
				int i = 0;
				while (i < num6)
				{
					if (this.lengthsTable[i] > num)
					{
						num4 = this.timesTable[i];
						num5 = this.lengthsTable[i];
						if (i > 0)
						{
							num3 = this.lengthsTable[i - 1];
							break;
						}
						break;
					}
					else
					{
						num2 = this.timesTable[i];
						i++;
					}
				}
				perc = num2 + (num - num3) / (num5 - num3) * (num4 - num2);
			}
			if (perc > 1f)
			{
				perc = 1f;
			}
			else if (perc < 0f)
			{
				perc = 0f;
			}
			return perc;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000EC08 File Offset: 0x0000CE08
		internal int GetWaypointIndexFromPerc(float perc, bool isMovingForward)
		{
			if (perc >= 1f)
			{
				return this.wps.Length - 1;
			}
			if (perc <= 0f)
			{
				return 0;
			}
			float num = this.length * perc;
			float num2 = 0f;
			int i = 0;
			int num3 = this.wpLengths.Length;
			while (i < num3)
			{
				num2 += this.wpLengths[i];
				if (i == num3 - 1)
				{
					if (!isMovingForward)
					{
						return i;
					}
					return i - 1;
				}
				else if (num2 >= num)
				{
					if (num2 <= num)
					{
						return i;
					}
					if (!isMovingForward)
					{
						return i;
					}
					return i - 1;
				}
				else
				{
					i++;
				}
			}
			return 0;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000EC84 File Offset: 0x0000CE84
		internal static Vector3[] GetDrawPoints(Path p, int drawSubdivisionsXSegment)
		{
			int num = p.wps.Length;
			if (p.type == PathType.Linear)
			{
				return p.wps;
			}
			int num2 = num * drawSubdivisionsXSegment;
			Vector3[] array = new Vector3[num2 + 1];
			for (int i = 0; i <= num2; i++)
			{
				float perc = (float)i / (float)num2;
				Vector3 point = p.GetPoint(perc, false);
				array[i] = point;
			}
			return array;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000ECE0 File Offset: 0x0000CEE0
		internal static void RefreshNonLinearDrawWps(Path p)
		{
			int num = p.wps.Length * 10;
			if (p.nonLinearDrawWps == null || p.nonLinearDrawWps.Length != num + 1)
			{
				p.nonLinearDrawWps = new Vector3[num + 1];
			}
			for (int i = 0; i <= num; i++)
			{
				float perc = (float)i / (float)num;
				Vector3 point = p.GetPoint(perc, false);
				p.nonLinearDrawWps[i] = point;
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000ED44 File Offset: 0x0000CF44
		internal void Destroy()
		{
			if (TweenManager.isUnityEditor)
			{
				DOTween.GizmosDelegates.Remove(new TweenCallback(this.Draw));
			}
			this.wps = null;
			this.wpLengths = (this.timesTable = (this.lengthsTable = null));
			this.nonLinearDrawWps = null;
			this.isFinalized = false;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000EDA0 File Offset: 0x0000CFA0
		internal Path CloneIncremental(int loopIncrement)
		{
			if (this._incrementalClone != null)
			{
				if (this._incrementalIndex == loopIncrement)
				{
					return this._incrementalClone;
				}
				this._incrementalClone.Destroy();
			}
			int num = this.wps.Length;
			Vector3 a = this.wps[num - 1] - this.wps[0];
			Vector3[] array = new Vector3[this.wps.Length];
			for (int i = 0; i < num; i++)
			{
				array[i] = this.wps[i] + a * (float)loopIncrement;
			}
			int num2 = this.controlPoints.Length;
			ControlPoint[] array2 = new ControlPoint[num2];
			for (int j = 0; j < num2; j++)
			{
				array2[j] = this.controlPoints[j] + a * (float)loopIncrement;
			}
			Vector3[] array3 = null;
			if (this.nonLinearDrawWps != null)
			{
				int num3 = this.nonLinearDrawWps.Length;
				array3 = new Vector3[num3];
				for (int k = 0; k < num3; k++)
				{
					array3[k] = this.nonLinearDrawWps[k] + a * (float)loopIncrement;
				}
			}
			this._incrementalClone = new Path();
			this._incrementalIndex = loopIncrement;
			this._incrementalClone.type = this.type;
			this._incrementalClone.subdivisionsXSegment = this.subdivisionsXSegment;
			this._incrementalClone.subdivisions = this.subdivisions;
			this._incrementalClone.wps = array;
			this._incrementalClone.controlPoints = array2;
			if (TweenManager.isUnityEditor)
			{
				DOTween.GizmosDelegates.Add(new TweenCallback(this._incrementalClone.Draw));
			}
			this._incrementalClone.length = this.length;
			this._incrementalClone.wpLengths = this.wpLengths;
			this._incrementalClone.timesTable = this.timesTable;
			this._incrementalClone.lengthsTable = this.lengthsTable;
			this._incrementalClone._decoder = this._decoder;
			this._incrementalClone.nonLinearDrawWps = array3;
			this._incrementalClone.targetPosition = this.targetPosition;
			this._incrementalClone.lookAtPosition = this.lookAtPosition;
			this._incrementalClone.isFinalized = true;
			return this._incrementalClone;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000EFE8 File Offset: 0x0000D1E8
		internal void AssignWaypoints(Vector3[] newWps, bool cloneWps = false)
		{
			if (cloneWps)
			{
				int num = newWps.Length;
				this.wps = new Vector3[num];
				for (int i = 0; i < num; i++)
				{
					this.wps[i] = newWps[i];
				}
				return;
			}
			this.wps = newWps;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000F030 File Offset: 0x0000D230
		internal void AssignDecoder(PathType pathType)
		{
			this.type = pathType;
			if (pathType == PathType.Linear)
			{
				if (Path._linearDecoder == null)
				{
					Path._linearDecoder = new LinearDecoder();
				}
				this._decoder = Path._linearDecoder;
				return;
			}
			if (pathType != PathType.CubicBezier)
			{
				if (Path._catmullRomDecoder == null)
				{
					Path._catmullRomDecoder = new CatmullRomDecoder();
				}
				this._decoder = Path._catmullRomDecoder;
				return;
			}
			if (Path._cubicBezierDecoder == null)
			{
				Path._cubicBezierDecoder = new CubicBezierDecoder();
			}
			this._decoder = Path._cubicBezierDecoder;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000F0A3 File Offset: 0x0000D2A3
		internal void Draw()
		{
			Path.Draw(this);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000F0AC File Offset: 0x0000D2AC
		private static void Draw(Path p)
		{
			if (p.timesTable == null)
			{
				return;
			}
			Color color = p.gizmoColor;
			color.a *= 0.5f;
			Gizmos.color = p.gizmoColor;
			int num = p.wps.Length;
			if (p._changed || (p.type != PathType.Linear && p.nonLinearDrawWps == null))
			{
				p._changed = false;
				if (p.type != PathType.Linear)
				{
					Path.RefreshNonLinearDrawWps(p);
				}
			}
			if (p.type == PathType.Linear)
			{
				Vector3 to = Path.ConvertToDrawPoint(p.wps[0], p.plugOptions);
				for (int i = 0; i < num; i++)
				{
					Vector3 vector = Path.ConvertToDrawPoint(p.wps[i], p.plugOptions);
					Gizmos.DrawLine(vector, to);
					to = vector;
				}
			}
			else
			{
				Vector3 to = Path.ConvertToDrawPoint(p.nonLinearDrawWps[0], p.plugOptions);
				int num2 = p.nonLinearDrawWps.Length;
				for (int j = 1; j < num2; j++)
				{
					Vector3 vector2 = Path.ConvertToDrawPoint(p.nonLinearDrawWps[j], p.plugOptions);
					Gizmos.DrawLine(vector2, to);
					to = vector2;
				}
			}
			Gizmos.color = color;
			for (int k = 0; k < num; k++)
			{
				Gizmos.DrawSphere(Path.ConvertToDrawPoint(p.wps[k], p.plugOptions), 0.075f);
			}
			if (p.lookAtPosition != null)
			{
				Vector3 value = p.lookAtPosition.Value;
				Gizmos.DrawLine(p.targetPosition, value);
				Gizmos.DrawWireSphere(value, 0.075f);
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000F228 File Offset: 0x0000D428
		private static Vector3 ConvertToDrawPoint(Vector3 wp, PathOptions plugOptions)
		{
			if (!plugOptions.useLocalPosition || plugOptions.parent == null)
			{
				return wp;
			}
			return plugOptions.parent.TransformPoint(wp);
		}

		// Token: 0x04000129 RID: 297
		private static CatmullRomDecoder _catmullRomDecoder;

		// Token: 0x0400012A RID: 298
		private static LinearDecoder _linearDecoder;

		// Token: 0x0400012B RID: 299
		private static CubicBezierDecoder _cubicBezierDecoder;

		// Token: 0x0400012C RID: 300
		public float[] wpLengths;

		// Token: 0x0400012D RID: 301
		[SerializeField]
		public Vector3[] wps;

		// Token: 0x0400012E RID: 302
		[SerializeField]
		internal PathType type;

		// Token: 0x0400012F RID: 303
		[SerializeField]
		internal int subdivisionsXSegment;

		// Token: 0x04000130 RID: 304
		[SerializeField]
		internal int subdivisions;

		// Token: 0x04000131 RID: 305
		[SerializeField]
		internal ControlPoint[] controlPoints;

		// Token: 0x04000132 RID: 306
		[SerializeField]
		internal float length;

		// Token: 0x04000133 RID: 307
		[SerializeField]
		internal bool isFinalized;

		// Token: 0x04000134 RID: 308
		[SerializeField]
		internal float[] timesTable;

		// Token: 0x04000135 RID: 309
		[SerializeField]
		internal float[] lengthsTable;

		// Token: 0x04000136 RID: 310
		internal int linearWPIndex = -1;

		// Token: 0x04000137 RID: 311
		internal bool addedExtraStartWp;

		// Token: 0x04000138 RID: 312
		internal bool addedExtraEndWp;

		// Token: 0x04000139 RID: 313
		internal PathOptions plugOptions;

		// Token: 0x0400013A RID: 314
		private Path _incrementalClone;

		// Token: 0x0400013B RID: 315
		private int _incrementalIndex;

		// Token: 0x0400013C RID: 316
		private ABSPathDecoder _decoder;

		// Token: 0x0400013D RID: 317
		private bool _changed;

		// Token: 0x0400013E RID: 318
		internal Vector3[] nonLinearDrawWps;

		// Token: 0x0400013F RID: 319
		internal Vector3 targetPosition;

		// Token: 0x04000140 RID: 320
		internal Vector3? lookAtPosition;

		// Token: 0x04000141 RID: 321
		internal Color gizmoColor = new Color(1f, 1f, 1f, 0.7f);
	}
}
