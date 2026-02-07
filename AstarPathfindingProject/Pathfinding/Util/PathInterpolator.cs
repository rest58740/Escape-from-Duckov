using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x02000281 RID: 641
	public class PathInterpolator
	{
		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000F2D RID: 3885 RVA: 0x0005DA5B File Offset: 0x0005BC5B
		public bool valid
		{
			get
			{
				return this.path != null;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000F2E RID: 3886 RVA: 0x0005DA66 File Offset: 0x0005BC66
		public PathInterpolator.Cursor start
		{
			get
			{
				return PathInterpolator.Cursor.StartOfPath(this);
			}
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0005DA70 File Offset: 0x0005BC70
		public PathInterpolator.Cursor AtDistanceFromStart(float distance)
		{
			PathInterpolator.Cursor start = this.start;
			start.distance = distance;
			return start;
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0005DA90 File Offset: 0x0005BC90
		public void SetPath(List<Vector3> path)
		{
			this.version++;
			if (this.path == null)
			{
				this.path = new List<Vector3>();
			}
			this.path.Clear();
			if (path == null)
			{
				this.totalDistance = float.PositiveInfinity;
				return;
			}
			if (path.Count < 2)
			{
				throw new ArgumentException("Path must have a length of at least 2");
			}
			Vector3 vector = path[0];
			this.totalDistance = 0f;
			this.path.Capacity = Mathf.Max(this.path.Capacity, path.Count);
			this.path.Add(path[0]);
			for (int i = 1; i < path.Count; i++)
			{
				Vector3 vector2 = path[i];
				if (vector2 != vector)
				{
					this.totalDistance += (vector2 - vector).magnitude;
					this.path.Add(vector2);
					vector = vector2;
				}
			}
			if (this.path.Count < 2)
			{
				this.path.Add(path[0]);
			}
			if (float.IsNaN(this.totalDistance))
			{
				throw new ArgumentException("Path contains NaN values");
			}
		}

		// Token: 0x04000B53 RID: 2899
		private List<Vector3> path;

		// Token: 0x04000B54 RID: 2900
		private int version = 1;

		// Token: 0x04000B55 RID: 2901
		private float totalDistance;

		// Token: 0x02000282 RID: 642
		public struct Cursor
		{
			// Token: 0x17000216 RID: 534
			// (get) Token: 0x06000F32 RID: 3890 RVA: 0x0005DBC5 File Offset: 0x0005BDC5
			// (set) Token: 0x06000F33 RID: 3891 RVA: 0x0005DBCD File Offset: 0x0005BDCD
			private int segmentIndex { readonly get; set; }

			// Token: 0x17000217 RID: 535
			// (get) Token: 0x06000F34 RID: 3892 RVA: 0x0005DBD6 File Offset: 0x0005BDD6
			public int segmentCount
			{
				get
				{
					this.AssertValid();
					return this.interpolator.path.Count - 1;
				}
			}

			// Token: 0x17000218 RID: 536
			// (get) Token: 0x06000F35 RID: 3893 RVA: 0x0005DBF0 File Offset: 0x0005BDF0
			public Vector3 endPoint
			{
				get
				{
					this.AssertValid();
					return this.interpolator.path[this.interpolator.path.Count - 1];
				}
			}

			// Token: 0x17000219 RID: 537
			// (get) Token: 0x06000F36 RID: 3894 RVA: 0x0005DC1A File Offset: 0x0005BE1A
			// (set) Token: 0x06000F37 RID: 3895 RVA: 0x0005DC43 File Offset: 0x0005BE43
			public float fractionAlongCurrentSegment
			{
				get
				{
					if (this.currentSegmentLength <= 0f)
					{
						return 1f;
					}
					return (this.currentDistance - this.distanceToSegmentStart) / this.currentSegmentLength;
				}
				set
				{
					this.currentDistance = this.distanceToSegmentStart + Mathf.Clamp01(value) * this.currentSegmentLength;
				}
			}

			// Token: 0x06000F38 RID: 3896 RVA: 0x0005DC60 File Offset: 0x0005BE60
			public static PathInterpolator.Cursor StartOfPath(PathInterpolator interpolator)
			{
				if (!interpolator.valid)
				{
					throw new InvalidOperationException("PathInterpolator has no path set");
				}
				return new PathInterpolator.Cursor
				{
					interpolator = interpolator,
					version = interpolator.version,
					segmentIndex = 0,
					currentDistance = 0f,
					distanceToSegmentStart = 0f,
					currentSegmentLength = (interpolator.path[1] - interpolator.path[0]).magnitude
				};
			}

			// Token: 0x1700021A RID: 538
			// (get) Token: 0x06000F39 RID: 3897 RVA: 0x0005DCEA File Offset: 0x0005BEEA
			public bool valid
			{
				get
				{
					return this.interpolator != null && this.interpolator.version == this.version;
				}
			}

			// Token: 0x1700021B RID: 539
			// (get) Token: 0x06000F3A RID: 3898 RVA: 0x0005DD09 File Offset: 0x0005BF09
			public Vector3 tangent
			{
				get
				{
					this.AssertValid();
					return this.interpolator.path[this.segmentIndex + 1] - this.interpolator.path[this.segmentIndex];
				}
			}

			// Token: 0x1700021C RID: 540
			// (get) Token: 0x06000F3B RID: 3899 RVA: 0x0005DD44 File Offset: 0x0005BF44
			// (set) Token: 0x06000F3C RID: 3900 RVA: 0x0005DD5E File Offset: 0x0005BF5E
			public float remainingDistance
			{
				get
				{
					this.AssertValid();
					return this.interpolator.totalDistance - this.distance;
				}
				set
				{
					this.AssertValid();
					this.distance = this.interpolator.totalDistance - value;
				}
			}

			// Token: 0x1700021D RID: 541
			// (get) Token: 0x06000F3D RID: 3901 RVA: 0x0005DD79 File Offset: 0x0005BF79
			// (set) Token: 0x06000F3E RID: 3902 RVA: 0x0005DD84 File Offset: 0x0005BF84
			public float distance
			{
				get
				{
					return this.currentDistance;
				}
				set
				{
					this.AssertValid();
					this.currentDistance = value;
					while (this.currentDistance < this.distanceToSegmentStart)
					{
						if (this.segmentIndex <= 0)
						{
							break;
						}
						this.PrevSegment();
					}
					while (this.currentDistance > this.distanceToSegmentStart + this.currentSegmentLength && this.segmentIndex < this.interpolator.path.Count - 2)
					{
						this.NextSegment();
					}
				}
			}

			// Token: 0x1700021E RID: 542
			// (get) Token: 0x06000F3F RID: 3903 RVA: 0x0005DDF4 File Offset: 0x0005BFF4
			public Vector3 position
			{
				get
				{
					this.AssertValid();
					float t = (this.currentSegmentLength > 0.0001f) ? ((this.currentDistance - this.distanceToSegmentStart) / this.currentSegmentLength) : 0f;
					return Vector3.Lerp(this.interpolator.path[this.segmentIndex], this.interpolator.path[this.segmentIndex + 1], t);
				}
			}

			// Token: 0x06000F40 RID: 3904 RVA: 0x0005DE64 File Offset: 0x0005C064
			public void GetRemainingPath(List<Vector3> buffer)
			{
				this.AssertValid();
				buffer.Add(this.position);
				for (int i = this.segmentIndex + 1; i < this.interpolator.path.Count; i++)
				{
					buffer.Add(this.interpolator.path[i]);
				}
			}

			// Token: 0x06000F41 RID: 3905 RVA: 0x0005DEBC File Offset: 0x0005C0BC
			private void AssertValid()
			{
				if (!this.valid)
				{
					throw new InvalidOperationException("The cursor has been invalidated because SetPath has been called on the interpolator. Please create a new cursor.");
				}
			}

			// Token: 0x06000F42 RID: 3906 RVA: 0x0005DED4 File Offset: 0x0005C0D4
			public void GetTangents(out Vector3 t1, out Vector3 t2)
			{
				this.AssertValid();
				bool flag = this.currentDistance <= this.distanceToSegmentStart + 0.001f;
				bool flag2 = this.currentDistance >= this.distanceToSegmentStart + this.currentSegmentLength - 0.001f;
				if (flag || flag2)
				{
					int num;
					int num2;
					if (flag)
					{
						num = ((this.segmentIndex > 0) ? (this.segmentIndex - 1) : this.segmentIndex);
						num2 = this.segmentIndex;
					}
					else
					{
						num = this.segmentIndex;
						num2 = ((this.segmentIndex < this.interpolator.path.Count - 2) ? (this.segmentIndex + 1) : this.segmentIndex);
					}
					t1 = this.interpolator.path[num + 1] - this.interpolator.path[num];
					t2 = this.interpolator.path[num2 + 1] - this.interpolator.path[num2];
					return;
				}
				t1 = this.tangent;
				t2 = t1;
			}

			// Token: 0x1700021F RID: 543
			// (get) Token: 0x06000F43 RID: 3907 RVA: 0x0005DFF8 File Offset: 0x0005C1F8
			public Vector3 curvatureDirection
			{
				get
				{
					Vector3 lhs;
					Vector3 rhs;
					this.GetTangents(out lhs, out rhs);
					Vector3 result = Vector3.Cross(lhs, rhs);
					if (result.sqrMagnitude > 1E-06f)
					{
						return result;
					}
					return Vector3.zero;
				}
			}

			// Token: 0x06000F44 RID: 3908 RVA: 0x0005E02C File Offset: 0x0005C22C
			public void MoveToNextCorner()
			{
				this.AssertValid();
				List<Vector3> path = this.interpolator.path;
				while (this.currentDistance >= this.distanceToSegmentStart + this.currentSegmentLength)
				{
					if (this.segmentIndex >= path.Count - 2)
					{
						break;
					}
					this.NextSegment();
				}
				while (this.segmentIndex < path.Count - 2 && VectorMath.IsColinear(path[this.segmentIndex], path[this.segmentIndex + 1], path[this.segmentIndex + 2]))
				{
					this.NextSegment();
				}
				this.currentDistance = this.distanceToSegmentStart + this.currentSegmentLength;
			}

			// Token: 0x06000F45 RID: 3909 RVA: 0x0005E0D4 File Offset: 0x0005C2D4
			public bool MoveToClosestIntersectionWithLineSegment(Vector3 origin, Vector3 direction, Vector2 range)
			{
				this.AssertValid();
				float distance = float.PositiveInfinity;
				float num = float.PositiveInfinity;
				float num2 = 0f;
				for (int i = 0; i < this.interpolator.path.Count - 1; i++)
				{
					Vector3 vector = this.interpolator.path[i];
					Vector3 a = this.interpolator.path[i + 1];
					float magnitude = (a - vector).magnitude;
					float num3;
					float num4;
					if (VectorMath.LineLineIntersectionFactors(vector.xz, (a - vector).xz, origin.xz, direction.xz, out num3, out num4) && num3 >= 0f && num3 <= 1f && num4 >= range.x && num4 <= range.y)
					{
						float num5 = num2 + num3 * magnitude;
						float num6 = Mathf.Abs(num5 - this.currentDistance);
						if (num6 < num)
						{
							distance = num5;
							num = num6;
						}
					}
					num2 += magnitude;
				}
				if (num != float.PositiveInfinity)
				{
					this.distance = distance;
					return true;
				}
				return false;
			}

			// Token: 0x06000F46 RID: 3910 RVA: 0x0005E20C File Offset: 0x0005C40C
			private void MoveToSegment(int index, float fractionAlongSegment)
			{
				this.AssertValid();
				if (index < 0 || index >= this.interpolator.path.Count - 1)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				while (this.segmentIndex > index)
				{
					this.PrevSegment();
				}
				while (this.segmentIndex < index)
				{
					this.NextSegment();
				}
				this.currentDistance = this.distanceToSegmentStart + Mathf.Clamp01(fractionAlongSegment) * this.currentSegmentLength;
			}

			// Token: 0x06000F47 RID: 3911 RVA: 0x0005E280 File Offset: 0x0005C480
			public void MoveToClosestPoint(Vector3 point)
			{
				this.AssertValid();
				float num = float.PositiveInfinity;
				float fractionAlongSegment = 0f;
				int index = 0;
				List<Vector3> path = this.interpolator.path;
				for (int i = 0; i < path.Count - 1; i++)
				{
					float num2 = VectorMath.ClosestPointOnLineFactor(path[i], path[i + 1], point);
					Vector3 b = Vector3.Lerp(path[i], path[i + 1], num2);
					float sqrMagnitude = (point - b).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						fractionAlongSegment = num2;
						index = i;
					}
				}
				this.MoveToSegment(index, fractionAlongSegment);
			}

			// Token: 0x06000F48 RID: 3912 RVA: 0x0005E324 File Offset: 0x0005C524
			public void MoveToLocallyClosestPoint(Vector3 point, bool allowForwards = true, bool allowBackwards = true)
			{
				this.AssertValid();
				List<Vector3> path = this.interpolator.path;
				this.segmentIndex = Mathf.Min(this.segmentIndex, path.Count - 2);
				float num;
				for (;;)
				{
					int segmentIndex = this.segmentIndex;
					num = VectorMath.ClosestPointOnLineFactor(path[segmentIndex], path[segmentIndex + 1], point);
					if (num > 1f && allowForwards && this.segmentIndex < path.Count - 2)
					{
						this.NextSegment();
						allowBackwards = false;
					}
					else
					{
						if (num >= 0f || !allowBackwards || this.segmentIndex <= 0)
						{
							break;
						}
						this.PrevSegment();
						allowForwards = false;
					}
				}
				if (num > 0.5f && this.segmentIndex < path.Count - 2)
				{
					this.NextSegment();
				}
				float num2 = 0f;
				float num3 = float.PositiveInfinity;
				if (this.segmentIndex > 0)
				{
					int num4 = this.segmentIndex - 1;
					num2 = VectorMath.ClosestPointOnLineFactor(path[num4], path[num4 + 1], point);
					num3 = (Vector3.Lerp(path[num4], path[num4 + 1], num2) - point).sqrMagnitude;
				}
				float num5 = VectorMath.ClosestPointOnLineFactor(path[this.segmentIndex], path[this.segmentIndex + 1], point);
				float sqrMagnitude = (Vector3.Lerp(path[this.segmentIndex], path[this.segmentIndex + 1], num5) - point).sqrMagnitude;
				if (num3 < sqrMagnitude)
				{
					this.MoveToSegment(this.segmentIndex - 1, num2);
					return;
				}
				this.MoveToSegment(this.segmentIndex, num5);
			}

			// Token: 0x06000F49 RID: 3913 RVA: 0x0005E4BC File Offset: 0x0005C6BC
			public void MoveToCircleIntersection2D<T>(Vector3 circleCenter3D, float radius, T transform) where T : IMovementPlane
			{
				this.AssertValid();
				List<Vector3> path = this.interpolator.path;
				while (this.segmentIndex < path.Count - 2 && VectorMath.ClosestPointOnLineFactor(path[this.segmentIndex], path[this.segmentIndex + 1], circleCenter3D) > 1f)
				{
					this.NextSegment();
				}
				Vector2 vector = transform.ToPlane(circleCenter3D);
				while (this.segmentIndex < path.Count - 2 && (transform.ToPlane(path[this.segmentIndex + 1]) - vector).sqrMagnitude <= radius * radius)
				{
					this.NextSegment();
				}
				float fractionAlongSegment = VectorMath.LineCircleIntersectionFactor(vector, transform.ToPlane(path[this.segmentIndex]), transform.ToPlane(path[this.segmentIndex + 1]), radius);
				this.MoveToSegment(this.segmentIndex, fractionAlongSegment);
			}

			// Token: 0x06000F4A RID: 3914 RVA: 0x0005E5C8 File Offset: 0x0005C7C8
			private static float IntegrateSmoothingKernel(float a, float b, float smoothingDistance)
			{
				if (smoothingDistance <= 0f)
				{
					return (float)((a <= 0f && b > 0f) ? 1 : 0);
				}
				float num = (a < 0f) ? Mathf.Exp(a / smoothingDistance) : (2f - Mathf.Exp(-a / smoothingDistance));
				float num2 = (b < 0f) ? Mathf.Exp(b / smoothingDistance) : (2f - Mathf.Exp(-b / smoothingDistance));
				return 0.5f * (num2 - num);
			}

			// Token: 0x06000F4B RID: 3915 RVA: 0x0005E640 File Offset: 0x0005C840
			private static float IntegrateSmoothingKernel2(float a, float b, float smoothingDistance)
			{
				if (smoothingDistance <= 0f)
				{
					return 0f;
				}
				float num = -Mathf.Exp(-a / smoothingDistance) * smoothingDistance;
				float num2 = -Mathf.Exp(-b / smoothingDistance) * (smoothingDistance + b - a);
				return 0.5f * (num2 - num);
			}

			// Token: 0x06000F4C RID: 3916 RVA: 0x0005E684 File Offset: 0x0005C884
			private static Vector3 IntegrateSmoothTangent(Vector3 p1, Vector3 p2, ref Vector3 tangent, ref float distance, float expectedRadius, float smoothingDistance)
			{
				Vector3 a = p2 - p1;
				float magnitude = a.magnitude;
				if (magnitude <= 1E-05f)
				{
					return Vector3.zero;
				}
				Vector3 vector = a * (1f / magnitude);
				float f = Vector3.Angle(tangent, vector) * 0.017453292f;
				float num = expectedRadius * Mathf.Abs(f);
				Vector3 vector2 = Vector3.zero;
				if (num > 1E-45f)
				{
					Vector3 b = tangent * PathInterpolator.Cursor.IntegrateSmoothingKernel(distance, distance + num, smoothingDistance) + (vector - tangent) * PathInterpolator.Cursor.IntegrateSmoothingKernel2(distance, distance + num, smoothingDistance) / num;
					vector2 += b;
					distance += num;
				}
				vector2 += vector * PathInterpolator.Cursor.IntegrateSmoothingKernel(distance, distance + magnitude, smoothingDistance);
				tangent = vector;
				distance += magnitude;
				return vector2;
			}

			// Token: 0x06000F4D RID: 3917 RVA: 0x0005E770 File Offset: 0x0005C970
			public Vector3 EstimateSmoothTangent(Vector3 normalizedTangent, float smoothingDistance, float expectedRadius, Vector3 beforePathStartContribution, bool forward = true, bool backward = true)
			{
				this.AssertValid();
				if (expectedRadius <= 1E-45f || smoothingDistance <= 0f)
				{
					return normalizedTangent;
				}
				List<Vector3> path = this.interpolator.path;
				Vector3 vector = Vector3.zero;
				while (this.currentDistance >= this.distanceToSegmentStart + this.currentSegmentLength && this.segmentIndex < this.interpolator.path.Count - 2)
				{
					this.NextSegment();
				}
				if (forward)
				{
					float num = 0f;
					Vector3 p = this.position;
					Vector3 vector2 = normalizedTangent;
					for (int i = this.segmentIndex + 1; i < path.Count; i++)
					{
						vector += PathInterpolator.Cursor.IntegrateSmoothTangent(p, path[i], ref vector2, ref num, expectedRadius, smoothingDistance);
						p = path[i];
					}
				}
				if (backward)
				{
					float num2 = 0f;
					Vector3 vector3 = -normalizedTangent;
					Vector3 p2 = this.position;
					for (int j = this.segmentIndex; j >= 0; j--)
					{
						vector -= PathInterpolator.Cursor.IntegrateSmoothTangent(p2, path[j], ref vector3, ref num2, expectedRadius, smoothingDistance);
						p2 = path[j];
					}
					vector += beforePathStartContribution * PathInterpolator.Cursor.IntegrateSmoothingKernel(float.NegativeInfinity, -this.currentDistance, smoothingDistance);
				}
				return vector;
			}

			// Token: 0x06000F4E RID: 3918 RVA: 0x0005E8AC File Offset: 0x0005CAAC
			public Vector3 EstimateSmoothCurvature(Vector3 tangent, float smoothingDistance, float expectedRadius)
			{
				this.AssertValid();
				if (expectedRadius <= 1E-45f)
				{
					return Vector3.zero;
				}
				List<Vector3> path = this.interpolator.path;
				tangent = tangent.normalized;
				Vector3 vector = Vector3.zero;
				while (this.currentDistance >= this.distanceToSegmentStart + this.currentSegmentLength && this.segmentIndex < this.interpolator.path.Count - 2)
				{
					this.NextSegment();
				}
				float num = 0f;
				Vector3 b = this.position;
				Vector3 vector2 = tangent.normalized;
				for (int i = this.segmentIndex + 1; i < path.Count; i++)
				{
					Vector3 vector3 = path[i] - b;
					Vector3 normalized = vector3.normalized;
					float f = Vector3.Angle(vector2, normalized) * 0.017453292f;
					Vector3 normalized2 = Vector3.Cross(vector2, normalized).normalized;
					float num2 = 1f / expectedRadius;
					float num3 = expectedRadius * Mathf.Abs(f);
					float d = num2 * PathInterpolator.Cursor.IntegrateSmoothingKernel(num, num + num3, smoothingDistance);
					vector -= d * normalized2;
					vector2 = normalized;
					num += num3;
					num += vector3.magnitude;
					b = path[i];
				}
				num = float.Epsilon;
				vector2 = -tangent.normalized;
				b = this.position;
				for (int j = this.segmentIndex; j >= 0; j--)
				{
					Vector3 lhs = path[j] - b;
					if (!(lhs == Vector3.zero))
					{
						Vector3 normalized3 = lhs.normalized;
						float f2 = Vector3.Angle(vector2, normalized3) * 0.017453292f;
						Vector3 normalized4 = Vector3.Cross(vector2, normalized3).normalized;
						float num4 = 1f / expectedRadius;
						float num5 = expectedRadius * Mathf.Abs(f2);
						float d2 = num4 * PathInterpolator.Cursor.IntegrateSmoothingKernel(num, num + num5, smoothingDistance);
						vector += d2 * normalized4;
						vector2 = normalized3;
						num += num5;
						num += lhs.magnitude;
						b = path[j];
					}
				}
				return vector;
			}

			// Token: 0x06000F4F RID: 3919 RVA: 0x0005EAB0 File Offset: 0x0005CCB0
			public void MoveWithTurningSpeed(float time, float speed, float turningSpeed, ref Vector3 tangent)
			{
				if (turningSpeed <= 0f)
				{
					throw new ArgumentException("turningSpeed must be greater than zero");
				}
				if (speed <= 0f)
				{
					throw new ArgumentException("speed must be greater than zero");
				}
				this.AssertValid();
				float num = speed / turningSpeed;
				float num2 = time * speed;
				int num3 = 0;
				while (num2 > 0f && this.currentDistance >= this.distanceToSegmentStart + this.currentSegmentLength)
				{
					if (this.segmentIndex >= this.interpolator.path.Count - 2)
					{
						break;
					}
					this.NextSegment();
				}
				while (num2 < 0f && this.currentDistance <= this.distanceToSegmentStart)
				{
					if (this.segmentIndex <= 0)
					{
						break;
					}
					this.PrevSegment();
				}
				while (num2 != 0f)
				{
					num3++;
					if (num3 > 100)
					{
						throw new Exception("Infinite Loop " + num2.ToString() + " " + time.ToString());
					}
					Vector3 tangent2 = this.tangent;
					if (tangent != tangent2 && this.currentSegmentLength > 0f)
					{
						float num4 = Vector3.Angle(tangent, tangent2) * 0.017453292f * num;
						if (Mathf.Abs(num2) <= num4)
						{
							tangent = Vector3.Slerp(tangent, tangent2, Mathf.Abs(num2) / num4);
							return;
						}
						num2 -= num4 * Mathf.Sign(num2);
						tangent = tangent2;
					}
					if (num2 > 0f)
					{
						float num5 = this.currentSegmentLength - (this.currentDistance - this.distanceToSegmentStart);
						if (num2 < num5)
						{
							this.currentDistance += num2;
							return;
						}
						num2 -= num5;
						if (this.segmentIndex + 1 >= this.interpolator.path.Count - 1)
						{
							this.MoveToSegment(this.segmentIndex, 1f);
							return;
						}
						this.MoveToSegment(this.segmentIndex + 1, 0f);
					}
					else
					{
						float num6 = this.currentDistance - this.distanceToSegmentStart;
						if (-num2 <= num6)
						{
							this.currentDistance += num2;
							return;
						}
						num2 += num6;
						if (this.segmentIndex - 1 < 0)
						{
							this.MoveToSegment(this.segmentIndex, 0f);
							return;
						}
						this.MoveToSegment(this.segmentIndex - 1, 1f);
					}
				}
			}

			// Token: 0x06000F50 RID: 3920 RVA: 0x0005ECE8 File Offset: 0x0005CEE8
			private void PrevSegment()
			{
				int segmentIndex = this.segmentIndex;
				this.segmentIndex = segmentIndex - 1;
				this.currentSegmentLength = (this.interpolator.path[this.segmentIndex + 1] - this.interpolator.path[this.segmentIndex]).magnitude;
				this.distanceToSegmentStart -= this.currentSegmentLength;
			}

			// Token: 0x06000F51 RID: 3921 RVA: 0x0005ED5C File Offset: 0x0005CF5C
			private void NextSegment()
			{
				int segmentIndex = this.segmentIndex;
				this.segmentIndex = segmentIndex + 1;
				this.distanceToSegmentStart += this.currentSegmentLength;
				this.currentSegmentLength = (this.interpolator.path[this.segmentIndex + 1] - this.interpolator.path[this.segmentIndex]).magnitude;
			}

			// Token: 0x04000B56 RID: 2902
			private PathInterpolator interpolator;

			// Token: 0x04000B57 RID: 2903
			private int version;

			// Token: 0x04000B58 RID: 2904
			private float currentDistance;

			// Token: 0x04000B59 RID: 2905
			private float distanceToSegmentStart;

			// Token: 0x04000B5A RID: 2906
			private float currentSegmentLength;
		}
	}
}
