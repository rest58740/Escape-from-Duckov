using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200010C RID: 268
	[Obsolete("This modifier is deprecated")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/advancedsmooth.html")]
	[Serializable]
	public class AdvancedSmooth : MonoModifier
	{
		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x0002D2C5 File Offset: 0x0002B4C5
		public override int Order
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0002D2CC File Offset: 0x0002B4CC
		public override void Apply(Path p)
		{
			Vector3[] array = p.vectorPath.ToArray();
			if (array.Length <= 2)
			{
				return;
			}
			List<Vector3> list = new List<Vector3>();
			list.Add(array[0]);
			AdvancedSmooth.TurnConstructor.turningRadius = this.turningRadius;
			for (int i = 1; i < array.Length - 1; i++)
			{
				List<AdvancedSmooth.Turn> turnList = new List<AdvancedSmooth.Turn>();
				AdvancedSmooth.TurnConstructor.Setup(i, array);
				this.turnConstruct1.Prepare(i, array);
				this.turnConstruct2.Prepare(i, array);
				AdvancedSmooth.TurnConstructor.PostPrepare();
				if (i == 1)
				{
					this.turnConstruct1.PointToTangent(turnList);
					this.turnConstruct2.PointToTangent(turnList);
				}
				else
				{
					this.turnConstruct1.TangentToTangent(turnList);
					this.turnConstruct2.TangentToTangent(turnList);
				}
				this.EvaluatePaths(turnList, list);
				if (i == array.Length - 2)
				{
					this.turnConstruct1.TangentToPoint(turnList);
					this.turnConstruct2.TangentToPoint(turnList);
				}
				this.EvaluatePaths(turnList, list);
			}
			list.Add(array[array.Length - 1]);
			p.vectorPath = list;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0002D3CC File Offset: 0x0002B5CC
		private void EvaluatePaths(List<AdvancedSmooth.Turn> turnList, List<Vector3> output)
		{
			turnList.Sort();
			for (int i = 0; i < turnList.Count; i++)
			{
				if (i == 0)
				{
					turnList[i].GetPath(output);
				}
			}
			turnList.Clear();
			if (AdvancedSmooth.TurnConstructor.changedPreviousTangent)
			{
				this.turnConstruct1.OnTangentUpdate();
				this.turnConstruct2.OnTangentUpdate();
			}
		}

		// Token: 0x04000596 RID: 1430
		public float turningRadius = 1f;

		// Token: 0x04000597 RID: 1431
		public AdvancedSmooth.MaxTurn turnConstruct1 = new AdvancedSmooth.MaxTurn();

		// Token: 0x04000598 RID: 1432
		public AdvancedSmooth.ConstantTurn turnConstruct2 = new AdvancedSmooth.ConstantTurn();

		// Token: 0x0200010D RID: 269
		[Serializable]
		public class MaxTurn : AdvancedSmooth.TurnConstructor
		{
			// Token: 0x060008A0 RID: 2208 RVA: 0x0002D450 File Offset: 0x0002B650
			public override void OnTangentUpdate()
			{
				this.rightCircleCenter = AdvancedSmooth.TurnConstructor.current + AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius;
				this.leftCircleCenter = AdvancedSmooth.TurnConstructor.current - AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius;
				this.vaRight = base.Atan2(AdvancedSmooth.TurnConstructor.current - this.rightCircleCenter);
				this.vaLeft = this.vaRight + 3.141592653589793;
			}

			// Token: 0x060008A1 RID: 2209 RVA: 0x0002D4D0 File Offset: 0x0002B6D0
			public override void Prepare(int i, Vector3[] vectorPath)
			{
				this.preRightCircleCenter = this.rightCircleCenter;
				this.preLeftCircleCenter = this.leftCircleCenter;
				this.rightCircleCenter = AdvancedSmooth.TurnConstructor.current + AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius;
				this.leftCircleCenter = AdvancedSmooth.TurnConstructor.current - AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius;
				this.preVaRight = this.vaRight;
				this.preVaLeft = this.vaLeft;
				this.vaRight = base.Atan2(AdvancedSmooth.TurnConstructor.current - this.rightCircleCenter);
				this.vaLeft = this.vaRight + 3.141592653589793;
			}

			// Token: 0x060008A2 RID: 2210 RVA: 0x0002D580 File Offset: 0x0002B780
			public override void TangentToTangent(List<AdvancedSmooth.Turn> turnList)
			{
				this.alfaRightRight = base.Atan2(this.rightCircleCenter - this.preRightCircleCenter);
				this.alfaLeftLeft = base.Atan2(this.leftCircleCenter - this.preLeftCircleCenter);
				this.alfaRightLeft = base.Atan2(this.leftCircleCenter - this.preRightCircleCenter);
				this.alfaLeftRight = base.Atan2(this.rightCircleCenter - this.preLeftCircleCenter);
				double num = (double)(this.leftCircleCenter - this.preRightCircleCenter).magnitude;
				double num2 = (double)(this.rightCircleCenter - this.preLeftCircleCenter).magnitude;
				bool flag = false;
				bool flag2 = false;
				if (num < (double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f))
				{
					num = (double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f);
					flag = true;
				}
				if (num2 < (double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f))
				{
					num2 = (double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f);
					flag2 = true;
				}
				this.deltaRightLeft = (flag ? 0.0 : (1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f) / num)));
				this.deltaLeftRight = (flag2 ? 0.0 : (1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f) / num2)));
				this.betaRightRight = base.ClockwiseAngle(this.preVaRight, this.alfaRightRight - 1.5707963267948966);
				this.betaRightLeft = base.ClockwiseAngle(this.preVaRight, this.alfaRightLeft - this.deltaRightLeft);
				this.betaLeftRight = base.CounterClockwiseAngle(this.preVaLeft, this.alfaLeftRight + this.deltaLeftRight);
				this.betaLeftLeft = base.CounterClockwiseAngle(this.preVaLeft, this.alfaLeftLeft + 1.5707963267948966);
				this.betaRightRight += base.ClockwiseAngle(this.alfaRightRight - 1.5707963267948966, this.vaRight);
				this.betaRightLeft += base.CounterClockwiseAngle(this.alfaRightLeft + this.deltaRightLeft, this.vaLeft);
				this.betaLeftRight += base.ClockwiseAngle(this.alfaLeftRight - this.deltaLeftRight, this.vaRight);
				this.betaLeftLeft += base.CounterClockwiseAngle(this.alfaLeftLeft + 1.5707963267948966, this.vaLeft);
				this.betaRightRight = base.GetLengthFromAngle(this.betaRightRight, (double)AdvancedSmooth.TurnConstructor.turningRadius);
				this.betaRightLeft = base.GetLengthFromAngle(this.betaRightLeft, (double)AdvancedSmooth.TurnConstructor.turningRadius);
				this.betaLeftRight = base.GetLengthFromAngle(this.betaLeftRight, (double)AdvancedSmooth.TurnConstructor.turningRadius);
				this.betaLeftLeft = base.GetLengthFromAngle(this.betaLeftLeft, (double)AdvancedSmooth.TurnConstructor.turningRadius);
				Vector3 a = base.AngleToVector(this.alfaRightRight - 1.5707963267948966) * AdvancedSmooth.TurnConstructor.turningRadius + this.preRightCircleCenter;
				Vector3 a2 = base.AngleToVector(this.alfaRightLeft - this.deltaRightLeft) * AdvancedSmooth.TurnConstructor.turningRadius + this.preRightCircleCenter;
				Vector3 a3 = base.AngleToVector(this.alfaLeftRight + this.deltaLeftRight) * AdvancedSmooth.TurnConstructor.turningRadius + this.preLeftCircleCenter;
				Vector3 a4 = base.AngleToVector(this.alfaLeftLeft + 1.5707963267948966) * AdvancedSmooth.TurnConstructor.turningRadius + this.preLeftCircleCenter;
				Vector3 b = base.AngleToVector(this.alfaRightRight - 1.5707963267948966) * AdvancedSmooth.TurnConstructor.turningRadius + this.rightCircleCenter;
				Vector3 b2 = base.AngleToVector(this.alfaRightLeft - this.deltaRightLeft + 3.141592653589793) * AdvancedSmooth.TurnConstructor.turningRadius + this.leftCircleCenter;
				Vector3 b3 = base.AngleToVector(this.alfaLeftRight + this.deltaLeftRight + 3.141592653589793) * AdvancedSmooth.TurnConstructor.turningRadius + this.rightCircleCenter;
				Vector3 b4 = base.AngleToVector(this.alfaLeftLeft + 1.5707963267948966) * AdvancedSmooth.TurnConstructor.turningRadius + this.leftCircleCenter;
				this.betaRightRight += (double)(a - b).magnitude;
				this.betaRightLeft += (double)(a2 - b2).magnitude;
				this.betaLeftRight += (double)(a3 - b3).magnitude;
				this.betaLeftLeft += (double)(a4 - b4).magnitude;
				if (flag)
				{
					this.betaRightLeft += 10000000.0;
				}
				if (flag2)
				{
					this.betaLeftRight += 10000000.0;
				}
				turnList.Add(new AdvancedSmooth.Turn((float)this.betaRightRight, this, 2));
				turnList.Add(new AdvancedSmooth.Turn((float)this.betaRightLeft, this, 3));
				turnList.Add(new AdvancedSmooth.Turn((float)this.betaLeftRight, this, 4));
				turnList.Add(new AdvancedSmooth.Turn((float)this.betaLeftLeft, this, 5));
			}

			// Token: 0x060008A3 RID: 2211 RVA: 0x0002DAD0 File Offset: 0x0002BCD0
			public override void PointToTangent(List<AdvancedSmooth.Turn> turnList)
			{
				bool flag = false;
				bool flag2 = false;
				float magnitude = (AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter).magnitude;
				float magnitude2 = (AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter).magnitude;
				if (magnitude < AdvancedSmooth.TurnConstructor.turningRadius)
				{
					flag = true;
				}
				if (magnitude2 < AdvancedSmooth.TurnConstructor.turningRadius)
				{
					flag2 = true;
				}
				double num = flag ? 0.0 : base.Atan2(AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter);
				double num2 = flag ? 0.0 : (1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius / (AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter).magnitude)));
				this.gammaRight = num + num2;
				double num3 = flag ? 0.0 : base.ClockwiseAngle(this.gammaRight, this.vaRight);
				double num4 = flag2 ? 0.0 : base.Atan2(AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter);
				double num5 = flag2 ? 0.0 : (1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius / (AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter).magnitude)));
				this.gammaLeft = num4 - num5;
				double num6 = flag2 ? 0.0 : base.CounterClockwiseAngle(this.gammaLeft, this.vaLeft);
				if (!flag)
				{
					turnList.Add(new AdvancedSmooth.Turn((float)num3, this, 0));
				}
				if (!flag2)
				{
					turnList.Add(new AdvancedSmooth.Turn((float)num6, this, 1));
				}
			}

			// Token: 0x060008A4 RID: 2212 RVA: 0x0002DC70 File Offset: 0x0002BE70
			public override void TangentToPoint(List<AdvancedSmooth.Turn> turnList)
			{
				bool flag = false;
				bool flag2 = false;
				float magnitude = (AdvancedSmooth.TurnConstructor.next - this.rightCircleCenter).magnitude;
				float magnitude2 = (AdvancedSmooth.TurnConstructor.next - this.leftCircleCenter).magnitude;
				if (magnitude < AdvancedSmooth.TurnConstructor.turningRadius)
				{
					flag = true;
				}
				if (magnitude2 < AdvancedSmooth.TurnConstructor.turningRadius)
				{
					flag2 = true;
				}
				if (!flag)
				{
					double num = base.Atan2(AdvancedSmooth.TurnConstructor.next - this.rightCircleCenter);
					double num2 = 1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius / magnitude));
					this.gammaRight = num - num2;
					double num3 = base.ClockwiseAngle(this.vaRight, this.gammaRight);
					turnList.Add(new AdvancedSmooth.Turn((float)num3, this, 6));
				}
				if (!flag2)
				{
					double num4 = base.Atan2(AdvancedSmooth.TurnConstructor.next - this.leftCircleCenter);
					double num5 = 1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius / magnitude2));
					this.gammaLeft = num4 + num5;
					double num6 = base.CounterClockwiseAngle(this.vaLeft, this.gammaLeft);
					turnList.Add(new AdvancedSmooth.Turn((float)num6, this, 7));
				}
			}

			// Token: 0x060008A5 RID: 2213 RVA: 0x0002DD90 File Offset: 0x0002BF90
			public override void GetPath(AdvancedSmooth.Turn turn, List<Vector3> output)
			{
				switch (turn.id)
				{
				case 0:
					base.AddCircleSegment(this.gammaRight, this.vaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 1:
					base.AddCircleSegment(this.gammaLeft, this.vaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 2:
					base.AddCircleSegment(this.preVaRight, this.alfaRightRight - 1.5707963267948966, true, this.preRightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					base.AddCircleSegment(this.alfaRightRight - 1.5707963267948966, this.vaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 3:
					base.AddCircleSegment(this.preVaRight, this.alfaRightLeft - this.deltaRightLeft, true, this.preRightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					base.AddCircleSegment(this.alfaRightLeft - this.deltaRightLeft + 3.141592653589793, this.vaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 4:
					base.AddCircleSegment(this.preVaLeft, this.alfaLeftRight + this.deltaLeftRight, false, this.preLeftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					base.AddCircleSegment(this.alfaLeftRight + this.deltaLeftRight + 3.141592653589793, this.vaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 5:
					base.AddCircleSegment(this.preVaLeft, this.alfaLeftLeft + 1.5707963267948966, false, this.preLeftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					base.AddCircleSegment(this.alfaLeftLeft + 1.5707963267948966, this.vaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 6:
					base.AddCircleSegment(this.vaRight, this.gammaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 7:
					base.AddCircleSegment(this.vaLeft, this.gammaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				default:
					return;
				}
			}

			// Token: 0x04000599 RID: 1433
			private Vector3 preRightCircleCenter = Vector3.zero;

			// Token: 0x0400059A RID: 1434
			private Vector3 preLeftCircleCenter = Vector3.zero;

			// Token: 0x0400059B RID: 1435
			private Vector3 rightCircleCenter;

			// Token: 0x0400059C RID: 1436
			private Vector3 leftCircleCenter;

			// Token: 0x0400059D RID: 1437
			private double vaRight;

			// Token: 0x0400059E RID: 1438
			private double vaLeft;

			// Token: 0x0400059F RID: 1439
			private double preVaLeft;

			// Token: 0x040005A0 RID: 1440
			private double preVaRight;

			// Token: 0x040005A1 RID: 1441
			private double gammaLeft;

			// Token: 0x040005A2 RID: 1442
			private double gammaRight;

			// Token: 0x040005A3 RID: 1443
			private double betaRightRight;

			// Token: 0x040005A4 RID: 1444
			private double betaRightLeft;

			// Token: 0x040005A5 RID: 1445
			private double betaLeftRight;

			// Token: 0x040005A6 RID: 1446
			private double betaLeftLeft;

			// Token: 0x040005A7 RID: 1447
			private double deltaRightLeft;

			// Token: 0x040005A8 RID: 1448
			private double deltaLeftRight;

			// Token: 0x040005A9 RID: 1449
			private double alfaRightRight;

			// Token: 0x040005AA RID: 1450
			private double alfaLeftLeft;

			// Token: 0x040005AB RID: 1451
			private double alfaRightLeft;

			// Token: 0x040005AC RID: 1452
			private double alfaLeftRight;
		}

		// Token: 0x0200010E RID: 270
		[Serializable]
		public class ConstantTurn : AdvancedSmooth.TurnConstructor
		{
			// Token: 0x060008A7 RID: 2215 RVA: 0x000035CE File Offset: 0x000017CE
			public override void Prepare(int i, Vector3[] vectorPath)
			{
			}

			// Token: 0x060008A8 RID: 2216 RVA: 0x0002DFBC File Offset: 0x0002C1BC
			public override void TangentToTangent(List<AdvancedSmooth.Turn> turnList)
			{
				Vector3 dir = Vector3.Cross(AdvancedSmooth.TurnConstructor.t1, Vector3.up);
				Vector3 vector = AdvancedSmooth.TurnConstructor.current - AdvancedSmooth.TurnConstructor.prev;
				Vector3 start = vector * 0.5f + AdvancedSmooth.TurnConstructor.prev;
				vector = Vector3.Cross(vector, Vector3.up);
				bool flag;
				this.circleCenter = VectorMath.LineDirIntersectionPointXZ(AdvancedSmooth.TurnConstructor.prev, dir, start, vector, out flag);
				if (!flag)
				{
					return;
				}
				this.gamma1 = base.Atan2(AdvancedSmooth.TurnConstructor.prev - this.circleCenter);
				this.gamma2 = base.Atan2(AdvancedSmooth.TurnConstructor.current - this.circleCenter);
				this.clockwise = !VectorMath.RightOrColinearXZ(this.circleCenter, AdvancedSmooth.TurnConstructor.prev, AdvancedSmooth.TurnConstructor.prev + AdvancedSmooth.TurnConstructor.t1);
				double num = this.clockwise ? base.ClockwiseAngle(this.gamma1, this.gamma2) : base.CounterClockwiseAngle(this.gamma1, this.gamma2);
				num = base.GetLengthFromAngle(num, (double)(this.circleCenter - AdvancedSmooth.TurnConstructor.current).magnitude);
				turnList.Add(new AdvancedSmooth.Turn((float)num, this, 0));
			}

			// Token: 0x060008A9 RID: 2217 RVA: 0x0002E0E8 File Offset: 0x0002C2E8
			public override void GetPath(AdvancedSmooth.Turn turn, List<Vector3> output)
			{
				base.AddCircleSegment(this.gamma1, this.gamma2, this.clockwise, this.circleCenter, output, (this.circleCenter - AdvancedSmooth.TurnConstructor.current).magnitude);
				AdvancedSmooth.TurnConstructor.normal = (AdvancedSmooth.TurnConstructor.current - this.circleCenter).normalized;
				AdvancedSmooth.TurnConstructor.t2 = Vector3.Cross(AdvancedSmooth.TurnConstructor.normal, Vector3.up).normalized;
				AdvancedSmooth.TurnConstructor.normal = -AdvancedSmooth.TurnConstructor.normal;
				if (!this.clockwise)
				{
					AdvancedSmooth.TurnConstructor.t2 = -AdvancedSmooth.TurnConstructor.t2;
					AdvancedSmooth.TurnConstructor.normal = -AdvancedSmooth.TurnConstructor.normal;
				}
				AdvancedSmooth.TurnConstructor.changedPreviousTangent = true;
			}

			// Token: 0x040005AD RID: 1453
			private Vector3 circleCenter;

			// Token: 0x040005AE RID: 1454
			private double gamma1;

			// Token: 0x040005AF RID: 1455
			private double gamma2;

			// Token: 0x040005B0 RID: 1456
			private bool clockwise;
		}

		// Token: 0x0200010F RID: 271
		public abstract class TurnConstructor
		{
			// Token: 0x060008AB RID: 2219
			public abstract void Prepare(int i, Vector3[] vectorPath);

			// Token: 0x060008AC RID: 2220 RVA: 0x000035CE File Offset: 0x000017CE
			public virtual void OnTangentUpdate()
			{
			}

			// Token: 0x060008AD RID: 2221 RVA: 0x000035CE File Offset: 0x000017CE
			public virtual void PointToTangent(List<AdvancedSmooth.Turn> turnList)
			{
			}

			// Token: 0x060008AE RID: 2222 RVA: 0x000035CE File Offset: 0x000017CE
			public virtual void TangentToPoint(List<AdvancedSmooth.Turn> turnList)
			{
			}

			// Token: 0x060008AF RID: 2223 RVA: 0x000035CE File Offset: 0x000017CE
			public virtual void TangentToTangent(List<AdvancedSmooth.Turn> turnList)
			{
			}

			// Token: 0x060008B0 RID: 2224
			public abstract void GetPath(AdvancedSmooth.Turn turn, List<Vector3> output);

			// Token: 0x060008B1 RID: 2225 RVA: 0x0002E1A8 File Offset: 0x0002C3A8
			public static void Setup(int i, Vector3[] vectorPath)
			{
				AdvancedSmooth.TurnConstructor.current = vectorPath[i];
				AdvancedSmooth.TurnConstructor.prev = vectorPath[i - 1];
				AdvancedSmooth.TurnConstructor.next = vectorPath[i + 1];
				AdvancedSmooth.TurnConstructor.prev.y = AdvancedSmooth.TurnConstructor.current.y;
				AdvancedSmooth.TurnConstructor.next.y = AdvancedSmooth.TurnConstructor.current.y;
				AdvancedSmooth.TurnConstructor.t1 = AdvancedSmooth.TurnConstructor.t2;
				AdvancedSmooth.TurnConstructor.t2 = (AdvancedSmooth.TurnConstructor.next - AdvancedSmooth.TurnConstructor.current).normalized - (AdvancedSmooth.TurnConstructor.prev - AdvancedSmooth.TurnConstructor.current).normalized;
				AdvancedSmooth.TurnConstructor.t2 = AdvancedSmooth.TurnConstructor.t2.normalized;
				AdvancedSmooth.TurnConstructor.prevNormal = AdvancedSmooth.TurnConstructor.normal;
				AdvancedSmooth.TurnConstructor.normal = Vector3.Cross(AdvancedSmooth.TurnConstructor.t2, Vector3.up);
				AdvancedSmooth.TurnConstructor.normal = AdvancedSmooth.TurnConstructor.normal.normalized;
			}

			// Token: 0x060008B2 RID: 2226 RVA: 0x0002E283 File Offset: 0x0002C483
			public static void PostPrepare()
			{
				AdvancedSmooth.TurnConstructor.changedPreviousTangent = false;
			}

			// Token: 0x060008B3 RID: 2227 RVA: 0x0002E28C File Offset: 0x0002C48C
			public void AddCircleSegment(double startAngle, double endAngle, bool clockwise, Vector3 center, List<Vector3> output, float radius)
			{
				double num = 0.06283185307179587;
				if (clockwise)
				{
					while (endAngle > startAngle + 6.283185307179586)
					{
						endAngle -= 6.283185307179586;
					}
					while (endAngle < startAngle)
					{
						endAngle += 6.283185307179586;
					}
				}
				else
				{
					while (endAngle < startAngle - 6.283185307179586)
					{
						endAngle += 6.283185307179586;
					}
					while (endAngle > startAngle)
					{
						endAngle -= 6.283185307179586;
					}
				}
				if (clockwise)
				{
					for (double num2 = startAngle; num2 < endAngle; num2 += num)
					{
						output.Add(this.AngleToVector(num2) * radius + center);
					}
				}
				else
				{
					for (double num3 = startAngle; num3 > endAngle; num3 -= num)
					{
						output.Add(this.AngleToVector(num3) * radius + center);
					}
				}
				output.Add(this.AngleToVector(endAngle) * radius + center);
			}

			// Token: 0x060008B4 RID: 2228 RVA: 0x0002E378 File Offset: 0x0002C578
			public void DebugCircleSegment(Vector3 center, double startAngle, double endAngle, double radius, Color color)
			{
				double num = 0.06283185307179587;
				while (endAngle < startAngle)
				{
					endAngle += 6.283185307179586;
				}
				Vector3 start = this.AngleToVector(startAngle) * (float)radius + center;
				for (double num2 = startAngle + num; num2 < endAngle; num2 += num)
				{
					Debug.DrawLine(start, this.AngleToVector(num2) * (float)radius + center);
				}
				Debug.DrawLine(start, this.AngleToVector(endAngle) * (float)radius + center);
			}

			// Token: 0x060008B5 RID: 2229 RVA: 0x0002E3FC File Offset: 0x0002C5FC
			public void DebugCircle(Vector3 center, double radius, Color color)
			{
				double num = 0.06283185307179587;
				Vector3 start = this.AngleToVector(-num) * (float)radius + center;
				for (double num2 = 0.0; num2 < 6.283185307179586; num2 += num)
				{
					Vector3 vector = this.AngleToVector(num2) * (float)radius + center;
					Debug.DrawLine(start, vector, color);
					start = vector;
				}
			}

			// Token: 0x060008B6 RID: 2230 RVA: 0x0002E464 File Offset: 0x0002C664
			public double GetLengthFromAngle(double angle, double radius)
			{
				return radius * angle;
			}

			// Token: 0x060008B7 RID: 2231 RVA: 0x0002E469 File Offset: 0x0002C669
			public double ClockwiseAngle(double from, double to)
			{
				return this.ClampAngle(to - from);
			}

			// Token: 0x060008B8 RID: 2232 RVA: 0x0002E474 File Offset: 0x0002C674
			public double CounterClockwiseAngle(double from, double to)
			{
				return this.ClampAngle(from - to);
			}

			// Token: 0x060008B9 RID: 2233 RVA: 0x0002E47F File Offset: 0x0002C67F
			public Vector3 AngleToVector(double a)
			{
				return new Vector3((float)Math.Cos(a), 0f, (float)Math.Sin(a));
			}

			// Token: 0x060008BA RID: 2234 RVA: 0x0002E499 File Offset: 0x0002C699
			public double ToDegrees(double rad)
			{
				return rad * 57.295780181884766;
			}

			// Token: 0x060008BB RID: 2235 RVA: 0x0002E4A6 File Offset: 0x0002C6A6
			public double ClampAngle(double a)
			{
				while (a < 0.0)
				{
					a += 6.283185307179586;
				}
				while (a > 6.283185307179586)
				{
					a -= 6.283185307179586;
				}
				return a;
			}

			// Token: 0x060008BC RID: 2236 RVA: 0x0002E4DF File Offset: 0x0002C6DF
			public double Atan2(Vector3 v)
			{
				return Math.Atan2((double)v.z, (double)v.x);
			}

			// Token: 0x040005B1 RID: 1457
			public float constantBias;

			// Token: 0x040005B2 RID: 1458
			public float factorBias = 1f;

			// Token: 0x040005B3 RID: 1459
			public static float turningRadius = 1f;

			// Token: 0x040005B4 RID: 1460
			public const double ThreeSixtyRadians = 6.283185307179586;

			// Token: 0x040005B5 RID: 1461
			public static Vector3 prev;

			// Token: 0x040005B6 RID: 1462
			public static Vector3 current;

			// Token: 0x040005B7 RID: 1463
			public static Vector3 next;

			// Token: 0x040005B8 RID: 1464
			public static Vector3 t1;

			// Token: 0x040005B9 RID: 1465
			public static Vector3 t2;

			// Token: 0x040005BA RID: 1466
			public static Vector3 normal;

			// Token: 0x040005BB RID: 1467
			public static Vector3 prevNormal;

			// Token: 0x040005BC RID: 1468
			public static bool changedPreviousTangent = false;
		}

		// Token: 0x02000110 RID: 272
		public struct Turn : IComparable<AdvancedSmooth.Turn>
		{
			// Token: 0x17000171 RID: 369
			// (get) Token: 0x060008BF RID: 2239 RVA: 0x0002E519 File Offset: 0x0002C719
			public float score
			{
				get
				{
					return this.length * this.constructor.factorBias + this.constructor.constantBias;
				}
			}

			// Token: 0x060008C0 RID: 2240 RVA: 0x0002E539 File Offset: 0x0002C739
			public Turn(float length, AdvancedSmooth.TurnConstructor constructor, int id = 0)
			{
				this.length = length;
				this.id = id;
				this.constructor = constructor;
			}

			// Token: 0x060008C1 RID: 2241 RVA: 0x0002E550 File Offset: 0x0002C750
			public void GetPath(List<Vector3> output)
			{
				this.constructor.GetPath(this, output);
			}

			// Token: 0x060008C2 RID: 2242 RVA: 0x0002E564 File Offset: 0x0002C764
			public int CompareTo(AdvancedSmooth.Turn t)
			{
				if (t.score > this.score)
				{
					return -1;
				}
				if (t.score >= this.score)
				{
					return 0;
				}
				return 1;
			}

			// Token: 0x060008C3 RID: 2243 RVA: 0x0002E589 File Offset: 0x0002C789
			public static bool operator <(AdvancedSmooth.Turn lhs, AdvancedSmooth.Turn rhs)
			{
				return lhs.score < rhs.score;
			}

			// Token: 0x060008C4 RID: 2244 RVA: 0x0002E59B File Offset: 0x0002C79B
			public static bool operator >(AdvancedSmooth.Turn lhs, AdvancedSmooth.Turn rhs)
			{
				return lhs.score > rhs.score;
			}

			// Token: 0x040005BD RID: 1469
			public float length;

			// Token: 0x040005BE RID: 1470
			public int id;

			// Token: 0x040005BF RID: 1471
			public AdvancedSmooth.TurnConstructor constructor;
		}
	}
}
