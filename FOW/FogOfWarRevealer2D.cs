using System;
using System.Runtime.CompilerServices;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace FOW
{
	// Token: 0x02000010 RID: 16
	public class FogOfWarRevealer2D : FogOfWarRevealer
	{
		// Token: 0x06000087 RID: 135 RVA: 0x00005D86 File Offset: 0x00003F86
		protected override void _InitRevealer(int StepCount)
		{
			this.InitialRayResults = new RaycastHit2D[StepCount];
			this.physicsScene2D = base.gameObject.scene.GetPhysicsScene2D();
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005DAA File Offset: 0x00003FAA
		protected override void CleanupRevealer()
		{
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005DAC File Offset: 0x00003FAC
		protected override void IterationOne(int NumSteps, float firstAngle, float angleStep)
		{
			for (int i = 0; i < NumSteps; i++)
			{
				this.FirstIteration.RayAngles[i] = firstAngle + angleStep * (float)i;
				this.FirstIteration.Directions[i] = this.DirectionFromAngle(this.FirstIteration.RayAngles[i], true);
				this.RayHit = this.physicsScene2D.Raycast(this.EyePosition, this.FirstIteration.Directions[i], this.RayDistance, this.ObstacleMask);
				if (this.RayHit.collider != null)
				{
					this.FirstIteration.Hits[i] = true;
					this.FirstIteration.Normals[i] = this.RayHit.normal;
					this.FirstIteration.Distances[i] = this.RayHit.distance;
					this.FirstIteration.Points[i] = this.RayHit.point;
				}
				else
				{
					this.FirstIteration.Hits[i] = false;
					this.FirstIteration.Normals[i] = -this.FirstIteration.Directions[i];
					this.FirstIteration.Distances[i] = this.RayDistance;
					this.FirstIteration.Points[i] = this.GetPositionxy(this.EyePosition) + this.FirstIteration.Directions[i] * this.RayDistance;
				}
			}
			this.PointsJobHandle = this.PointsJob.Schedule(NumSteps, this.CommandsPerJob, default(JobHandle));
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00005F8C File Offset: 0x0000418C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override void RayCast(float angle, ref FogOfWarRevealer.SightRay ray)
		{
			Vector2 vector = this.DirectionFromAngle(angle, true);
			ray.angle = angle;
			ray.direction = vector;
			this.RayHit = this.physicsScene2D.Raycast(this.EyePosition, vector, this.RayDistance, this.ObstacleMask);
			if (this.RayHit.collider != null)
			{
				ray.hit = true;
				ray.normal = this.RayHit.normal;
				ray.distance = this.RayHit.distance;
				ray.point = this.RayHit.point;
				return;
			}
			ray.hit = false;
			ray.normal = -vector;
			ray.distance = this.RayDistance;
			ray.point = this.GetPositionxy(this.EyePosition) + ray.direction * this.RayDistance;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00006087 File Offset: 0x00004287
		private float2 GetPositionxy(Vector3 pos)
		{
			this.pos2d.x = pos.x;
			this.pos2d.y = pos.y;
			return this.pos2d;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000060B1 File Offset: 0x000042B1
		protected override void _FindEdge()
		{
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000060B4 File Offset: 0x000042B4
		protected override float GetEuler()
		{
			Vector3 up = base.transform.up;
			up.z = 0f;
			up.Normalize();
			return -Vector3.SignedAngle(up, Vector3.up, -Vector3.forward);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000060F8 File Offset: 0x000042F8
		public override Vector3 GetEyePosition()
		{
			Vector3 vector = base.transform.position;
			if (FogOfWarWorld.instance.PixelateFog && FogOfWarWorld.instance.RoundRevealerPosition)
			{
				vector *= FogOfWarWorld.instance.PixelDensity;
				Vector3 b = new Vector3(FogOfWarWorld.instance.PixelGridOffset.x, FogOfWarWorld.instance.PixelGridOffset.y, 0f);
				vector -= b;
				vector = Vector3Int.RoundToInt(vector);
				vector += b;
				vector /= FogOfWarWorld.instance.PixelDensity;
			}
			return vector;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00006194 File Offset: 0x00004394
		protected override void _RevealHiders()
		{
			this.EyePosition = this.GetEyePosition();
			this.ForwardVectorCached = this.GetForward();
			float num = this.ViewRadius;
			if (FogOfWarWorld.instance.UsingSoftening)
			{
				num += this.RevealHiderInFadeOutZonePercentage * this.SoftenDistance;
			}
			this.unobscuredSightDist = this.UnobscuredRadius;
			if (FogOfWarWorld.instance.UsingSoftening)
			{
				this.unobscuredSightDist += this.RevealHiderInFadeOutZonePercentage * FogOfWarWorld.instance.UnobscuredSoftenDistance;
			}
			for (int i = 0; i < Mathf.Min(this.MaxHidersSampledPerFrame, FogOfWarWorld.NumHiders); i++)
			{
				this._lastHiderIndex = (this._lastHiderIndex + 1) % FogOfWarWorld.NumHiders;
				FogOfWarHider fogOfWarHider = FogOfWarWorld.HidersList[this._lastHiderIndex];
				bool flag = false;
				float num2 = this.distBetweenVectors(fogOfWarHider.transform.position, this.EyePosition) - fogOfWarHider.MaxDistBetweenSamplePoints;
				if (num2 < this.UnobscuredRadius || num2 < num)
				{
					for (int j = 0; j < fogOfWarHider.SamplePoints.Length; j++)
					{
						Transform transform = fogOfWarHider.SamplePoints[j];
						float num3 = this.distBetweenVectors(transform.position, this.EyePosition);
						if (num3 < this.UnobscuredRadius || (num3 < num && Mathf.Abs(this.AngleBetweenVector2(transform.position - this.EyePosition, this.ForwardVectorCached)) < this.ViewAngle / 2f))
						{
							this.SetHiderPosition(transform.position);
							if (!this.physicsScene2D.Raycast(this.EyePosition, this.hiderPosition - this.EyePosition, num3, this.ObstacleMask))
							{
								flag = true;
								break;
							}
						}
					}
				}
				if (this.UnobscuredRadius < 0f && num2 + 1.5f * fogOfWarHider.MaxDistBetweenSamplePoints < -this.UnobscuredRadius)
				{
					flag = false;
				}
				if (flag)
				{
					if (!this.HidersSeen.Contains(fogOfWarHider))
					{
						this.HidersSeen.Add(fogOfWarHider);
						fogOfWarHider.AddObserver(this);
					}
				}
				else if (this.HidersSeen.Contains(fogOfWarHider))
				{
					this.HidersSeen.Remove(fogOfWarHider);
					fogOfWarHider.RemoveObserver(this);
				}
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000063CD File Offset: 0x000045CD
		private void SetHiderPosition(Vector3 point)
		{
			this.hiderPosition.x = point.x;
			this.hiderPosition.y = point.y;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000063F4 File Offset: 0x000045F4
		protected override bool _TestPoint(Vector3 point)
		{
			float num = this.ViewRadius;
			if (FogOfWarWorld.instance.UsingSoftening)
			{
				num += this.RevealHiderInFadeOutZonePercentage * this.SoftenDistance;
			}
			this.EyePosition = this.GetEyePosition();
			float num2 = this.distBetweenVectors(point, this.EyePosition);
			if (num2 < this.UnobscuredRadius || (num2 < num && Mathf.Abs(this.AngleBetweenVector2(point - this.EyePosition, this.GetForward())) < this.ViewAngle / 2f))
			{
				this.SetHiderPosition(point);
				if (!this.physicsScene2D.Raycast(this.EyePosition, this.hiderPosition - base.transform.position, num2, this.ObstacleMask))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000064C4 File Offset: 0x000046C4
		protected override void SetCenterAndHeight()
		{
			this.center.x = this.EyePosition.x;
			this.center.y = this.EyePosition.y;
			this.heightPos = base.transform.position.z;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00006514 File Offset: 0x00004714
		protected override float AngleBetweenVector2(Vector3 _vec1, Vector3 _vec2)
		{
			this.vec1.x = _vec1.x;
			this.vec1.y = _vec1.y;
			this.vec2.x = _vec2.x;
			this.vec2.y = _vec2.y;
			this._vec1Rotated90.x = -this.vec1.y;
			this._vec1Rotated90.y = this.vec1.x;
			float num = (Vector2.Dot(this._vec1Rotated90, this.vec2) < 0f) ? -1f : 1f;
			return Vector2.Angle(this.vec1, this.vec2) * num;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000065CC File Offset: 0x000047CC
		private float distBetweenVectors(Vector3 _vec1, Vector3 _vec2)
		{
			this.vec1.x = _vec1.x;
			this.vec1.y = _vec1.y;
			this.vec2.x = _vec2.x;
			this.vec2.y = _vec2.y;
			return Vector2.Distance(this.vec1, this.vec2);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00006630 File Offset: 0x00004830
		private Vector3 GetForward()
		{
			return new Vector3(base.transform.up.x, base.transform.up.y, 0f).normalized;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00006670 File Offset: 0x00004870
		private Vector2 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
		{
			if (!angleIsGlobal)
			{
				angleInDegrees += base.transform.eulerAngles.z;
			}
			this.direction2d.x = Mathf.Cos(angleInDegrees * 0.017453292f);
			this.direction2d.y = Mathf.Sin(angleInDegrees * 0.017453292f);
			return this.direction2d;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000066C8 File Offset: 0x000048C8
		public override Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
		{
			if (!angleIsGlobal)
			{
				angleInDegrees += base.transform.eulerAngles.z;
			}
			this.direction.x = Mathf.Cos(angleInDegrees * 0.017453292f);
			this.direction.y = Mathf.Sin(angleInDegrees * 0.017453292f);
			return this.direction;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00006720 File Offset: 0x00004920
		protected override Vector3 _Get3Dfrom2D(Vector2 pos)
		{
			return new Vector3(pos.x, pos.y, 0f);
		}

		// Token: 0x040000CD RID: 205
		private RaycastHit2D[] InitialRayResults;

		// Token: 0x040000CE RID: 206
		private PhysicsScene2D physicsScene2D;

		// Token: 0x040000CF RID: 207
		private RaycastHit2D RayHit;

		// Token: 0x040000D0 RID: 208
		private float2 pos2d;

		// Token: 0x040000D1 RID: 209
		private Vector3 hiderPosition;

		// Token: 0x040000D2 RID: 210
		private float unobscuredSightDist;

		// Token: 0x040000D3 RID: 211
		private Vector2 vec1;

		// Token: 0x040000D4 RID: 212
		private Vector2 vec2;

		// Token: 0x040000D5 RID: 213
		private Vector2 _vec1Rotated90;

		// Token: 0x040000D6 RID: 214
		private Vector3 ForwardVectorCached;

		// Token: 0x040000D7 RID: 215
		private RaycastHit2D rayHit;

		// Token: 0x040000D8 RID: 216
		private Vector2 direction2d = Vector3.zero;

		// Token: 0x040000D9 RID: 217
		private Vector3 direction = Vector3.zero;
	}
}
