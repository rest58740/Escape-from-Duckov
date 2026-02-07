using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace FOW
{
	// Token: 0x02000011 RID: 17
	public class FogOfWarRevealer3D : FogOfWarRevealer
	{
		// Token: 0x0600009A RID: 154 RVA: 0x0000675C File Offset: 0x0000495C
		protected override void _InitRevealer(int StepCount)
		{
			this.physicsScene = base.gameObject.scene.GetPhysicsScene();
			if (this.RaycastCommandsNative.IsCreated)
			{
				this.CleanupRevealer();
			}
			this.RaycastCommandsNative = new NativeArray<RaycastCommand>(StepCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.RaycastHits = new NativeArray<RaycastHit>(StepCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.Vector3Directions = new NativeArray<float3>(StepCount, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.RayQueryParameters = new QueryParameters(this.ObstacleMask, false, QueryTriggerInteraction.UseGlobal, false);
			this.SetupJob = new FogOfWarRevealer3D.Phase1SetupJob
			{
				GamePlane = (int)FogOfWarWorld.instance.gamePlane,
				RayAngles = this.FirstIteration.RayAngles,
				Vector3Directions = this.Vector3Directions,
				RaycastCommandsNative = this.RaycastCommandsNative
			};
			this.DataJob = new FogOfWarRevealer3D.GetVector2Data
			{
				GamePlane = (int)FogOfWarWorld.instance.gamePlane,
				RaycastHits = this.RaycastHits,
				Hits = this.FirstIteration.Hits,
				Distances = this.FirstIteration.Distances,
				InDirections = this.Vector3Directions,
				OutPoints = this.FirstIteration.Points,
				OutDirections = this.FirstIteration.Directions,
				OutNormals = this.FirstIteration.Normals
			};
			for (int i = 0; i < StepCount; i++)
			{
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000068C1 File Offset: 0x00004AC1
		protected override void CleanupRevealer()
		{
			if (!this.RaycastCommandsNative.IsCreated)
			{
				return;
			}
			this.RaycastCommandsNative.Dispose();
			this.RaycastHits.Dispose();
			this.Vector3Directions.Dispose();
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000068F4 File Offset: 0x00004AF4
		protected override void IterationOne(int NumSteps, float firstAngle, float angleStep)
		{
			this.SetupJob.FirstAngle = firstAngle;
			this.SetupJob.AngleStep = angleStep;
			this.SetupJob.RayDistance = this.RayDistance;
			this.SetupJob.EyePosition = this.EyePosition;
			this.RayQueryParameters.layerMask = this.ObstacleMask;
			this.SetupJob.Parameters = this.RayQueryParameters;
			this.SetupJobJobHandle = this.SetupJob.Schedule(NumSteps, this.CommandsPerJob, default(JobHandle));
			this.IterationOneJobHandle = RaycastCommand.ScheduleBatch(this.RaycastCommandsNative, this.RaycastHits, this.CommandsPerJob, this.SetupJobJobHandle);
			this.DataJob.RayDistance = this.RayDistance;
			this.DataJob.EyePosition = this.EyePosition;
			this.Vector2NormalJobHandle = this.DataJob.Schedule(NumSteps, this.CommandsPerJob, this.IterationOneJobHandle);
			this.PointsJobHandle = this.PointsJob.Schedule(NumSteps, this.CommandsPerJob, this.Vector2NormalJobHandle);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00006A0C File Offset: 0x00004C0C
		protected override void _FindEdge()
		{
			NativeArray<RaycastCommand> commands = new NativeArray<RaycastCommand>(this.NumberOfPoints, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			NativeArray<float3> inDirections = new NativeArray<float3>(this.NumberOfPoints, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			NativeArray<FogOfWarRevealer.SightRay> sightRays = new NativeArray<FogOfWarRevealer.SightRay>(this.NumberOfPoints, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			NativeArray<FogOfWarRevealer.SightSegment> sightSegments = new NativeArray<FogOfWarRevealer.SightSegment>(this.ViewPoints, Allocator.TempJob);
			NativeArray<FogOfWarRevealer.EdgeResolveData> edgeData = new NativeArray<FogOfWarRevealer.EdgeResolveData>(this.NumberOfPoints, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			this.EdgeJob.SightRays = sightRays;
			this.EdgeJob.SightSegments = sightSegments;
			this.EdgeJob.EdgeData = edgeData;
			this.EdgeJob.EdgeNormals = new NativeArray<float2>(this.EdgeNormals, Allocator.TempJob);
			this.EdgeJob.MaxAcceptableEdgeAngleDifference = this.MaxAcceptableEdgeAngleDifference;
			this.EdgeJob.DoubleHitMaxAngleDelta = this.DoubleHitMaxAngleDelta;
			this.EdgeJob.EdgeDstThreshold = this.EdgeDstThreshold;
			for (int i = 0; i < this.NumberOfPoints; i++)
			{
				FogOfWarRevealer.EdgeResolveData edgeResolveData = default(FogOfWarRevealer.EdgeResolveData);
				edgeResolveData.CurrentAngle = this.ViewPoints[i].Angle;
				edgeResolveData.AngleAdd = this.EdgeAngles[i];
				edgeResolveData.Sign = 1f;
				edgeResolveData.AngleAdd /= 2f;
				edgeResolveData.CurrentAngle += edgeResolveData.AngleAdd;
				edgeResolveData.Break = false;
				edgeData[i] = edgeResolveData;
			}
			for (int j = 0; j < this.MaxEdgeResolveIterations; j++)
			{
				for (int k = 0; k < this.NumberOfPoints; k++)
				{
					inDirections[k] = this.DirFromAngle(edgeData[k].CurrentAngle, true);
					commands[k] = new RaycastCommand(this.EyePosition, inDirections[k], this.RayQueryParameters, this.RayDistance);
				}
				JobHandle dependsOn = RaycastCommand.ScheduleBatch(commands, this.RaycastHits, this.CommandsPerJob, default(JobHandle));
				JobHandle dependsOn2 = new FogOfWarRevealer3D.SightRayFromRaycastHit
				{
					GamePlane = (int)FogOfWarWorld.instance.gamePlane,
					RayDistance = this.RayDistance,
					EyePosition = this.EyePosition,
					RaycastHits = this.RaycastHits,
					SightRays = sightRays,
					InDirections = inDirections
				}.Schedule(this.NumberOfPoints, this.CommandsPerJob, dependsOn);
				this.EdgeJobHandle = this.EdgeJob.Schedule(this.NumberOfPoints, this.CommandsPerJob, dependsOn2);
				this.EdgeJobHandle.Complete();
			}
			this.ViewPoints = sightSegments.ToArray();
			commands.Dispose();
			inDirections.Dispose();
			sightRays.Dispose();
			sightSegments.Dispose();
			edgeData.Dispose();
			this.EdgeJob.EdgeNormals.Dispose();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00006CD4 File Offset: 0x00004ED4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override void RayCast(float angle, ref FogOfWarRevealer.SightRay ray)
		{
			Vector3 vector = this.DirFromAngle(angle, true);
			ray.angle = angle;
			ray.direction = this.GetVector2D(vector);
			if (this.physicsScene.Raycast(this.EyePosition, vector, out this.RayHit, this.RayDistance, this.ObstacleMask, QueryTriggerInteraction.UseGlobal))
			{
				ray.hit = true;
				ray.normal = this.GetVector2D(this.RayHit.normal);
				ray.distance = this.RayHit.distance;
				ray.point = this.GetVector2D(this.RayHit.point);
				return;
			}
			ray.hit = false;
			ray.normal = -ray.direction;
			ray.distance = this.RayDistance;
			ray.point = this.GetVector2D(this.CachedTransform.position) + ray.direction * this.RayDistance;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00006DC4 File Offset: 0x00004FC4
		private float2 GetVector2D(Vector3 vector)
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				this.vec2d.x = vector.x;
				this.vec2d.y = vector.z;
				return this.vec2d;
			case FogOfWarWorld.GamePlane.XY:
				this.vec2d.x = vector.x;
				this.vec2d.y = vector.y;
				return this.vec2d;
			case FogOfWarWorld.GamePlane.ZY:
				this.vec2d.x = vector.z;
				this.vec2d.y = vector.y;
				return this.vec2d;
			default:
				this.vec2d.x = vector.x;
				this.vec2d.y = vector.z;
				return this.vec2d;
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00006E94 File Offset: 0x00005094
		protected override float GetEuler()
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				return this.CachedTransform.eulerAngles.y;
			case FogOfWarWorld.GamePlane.XY:
			{
				Vector3 up = this.CachedTransform.up;
				up.z = 0f;
				up.Normalize();
				return -Vector3.SignedAngle(up, Vector3.up, FogOfWarWorld.UpVector);
			}
			case FogOfWarWorld.GamePlane.ZY:
			{
				Vector3 up2 = this.CachedTransform.up;
				up2.x = 0f;
				up2.Normalize();
				return -Vector3.SignedAngle(up2, Vector3.up, FogOfWarWorld.UpVector);
			}
			default:
				return this.CachedTransform.eulerAngles.y;
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00006F44 File Offset: 0x00005144
		public override Vector3 GetEyePosition()
		{
			Vector3 vector = this.CachedTransform.position + FogOfWarWorld.UpVector * this.EyeOffset;
			if (FogOfWarWorld.instance.PixelateFog && FogOfWarWorld.instance.RoundRevealerPosition)
			{
				vector *= FogOfWarWorld.instance.PixelDensity;
				Vector3 b = new Vector3(FogOfWarWorld.instance.PixelGridOffset.x, 0f, FogOfWarWorld.instance.PixelGridOffset.y);
				vector -= b;
				vector = Vector3Int.RoundToInt(vector);
				vector += b;
				vector /= FogOfWarWorld.instance.PixelDensity;
			}
			return vector;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00006FF4 File Offset: 0x000051F4
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
			num = Mathf.Max(num, this.UnobscuredRadius);
			for (int i = 0; i < Mathf.Min(this.MaxHidersSampledPerFrame, FogOfWarWorld.NumHiders); i++)
			{
				this._lastHiderIndex = (this._lastHiderIndex + 1) % FogOfWarWorld.NumHiders;
				FogOfWarHider fogOfWarHider = FogOfWarWorld.HidersList[this._lastHiderIndex];
				float num2 = this.DistBetweenVectors(fogOfWarHider.CachedTransform.position, this.EyePosition) - fogOfWarHider.MaxDistBetweenSamplePoints;
				bool flag = this.CanSeeHider(fogOfWarHider, num, num2);
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

		// Token: 0x060000A3 RID: 163 RVA: 0x00007160 File Offset: 0x00005360
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool CanSeeHider(FogOfWarHider hiderInQuestion, float sightDist, float minDistToHider)
		{
			if (minDistToHider > sightDist)
			{
				return false;
			}
			for (int i = 0; i < hiderInQuestion.SamplePoints.Length; i++)
			{
				if (this.CanSeeHiderSamplePoint(hiderInQuestion.SamplePoints[i], sightDist))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000719C File Offset: 0x0000539C
		private bool CanSeeHiderSamplePoint(Transform samplePoint, float sightDist)
		{
			float num = this.DistBetweenVectors(samplePoint.position, this.EyePosition);
			if (num < this.unobscuredSightDist)
			{
				return true;
			}
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				this.heightDist = Mathf.Abs(this.EyePosition.y - samplePoint.position.y);
				break;
			case FogOfWarWorld.GamePlane.XY:
				this.heightDist = Mathf.Abs(this.EyePosition.z - samplePoint.position.z);
				break;
			case FogOfWarWorld.GamePlane.ZY:
				this.heightDist = Mathf.Abs(this.EyePosition.x - samplePoint.position.x);
				break;
			}
			if (this.heightDist > this.VisionHeight)
			{
				return false;
			}
			if (Mathf.Abs(this.AngleBetweenVector2(samplePoint.position - this.EyePosition, this.ForwardVectorCached)) <= this.ViewAngle / 2f)
			{
				this.revealerOrigin = this.EyePosition;
				if (this.CalculateHidersAtHiderHeight)
				{
					this.SetRevealerOrigin(this.EyePosition, samplePoint.position);
				}
				this.hiderPosition = samplePoint.position;
				if (this.SampleHidersAtRevealerHeight)
				{
					this.SetHiderPosition(samplePoint.position, this.EyePosition);
				}
				if (!this.physicsScene.Raycast(this.revealerOrigin, this.hiderPosition - this.revealerOrigin, out this.RayHit, num, this.ObstacleMask, QueryTriggerInteraction.UseGlobal))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000731C File Offset: 0x0000551C
		private void SetHiderPosition(Vector3 point, Vector3 eyePosition)
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				this.hiderPosition.y = eyePosition.y;
				return;
			case FogOfWarWorld.GamePlane.XY:
				this.hiderPosition.z = eyePosition.z;
				return;
			case FogOfWarWorld.GamePlane.ZY:
				this.hiderPosition.x = eyePosition.x;
				return;
			default:
				return;
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000737C File Offset: 0x0000557C
		private void SetRevealerOrigin(Vector3 point, Vector3 _hiderPosition)
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				this.revealerOrigin.y = _hiderPosition.y;
				return;
			case FogOfWarWorld.GamePlane.XY:
				this.revealerOrigin.z = _hiderPosition.z;
				return;
			case FogOfWarWorld.GamePlane.ZY:
				this.revealerOrigin.x = _hiderPosition.x;
				return;
			default:
				return;
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000073DC File Offset: 0x000055DC
		protected override bool _TestPoint(Vector3 point)
		{
			float num = this.ViewRadius;
			if (FogOfWarWorld.instance.UsingSoftening)
			{
				num += this.RevealHiderInFadeOutZonePercentage * this.SoftenDistance;
			}
			this.EyePosition = this.GetEyePosition();
			this.ForwardVectorCached = this.GetForward();
			float num2 = this.DistBetweenVectors(point, this.EyePosition);
			if (num2 < this.UnobscuredRadius || (num2 < num && Mathf.Abs(this.AngleBetweenVector2(point - this.EyePosition, this.ForwardVectorCached)) < this.ViewAngle / 2f))
			{
				this.SetHiderPosition(point, this.EyePosition);
				if (!this.physicsScene.Raycast(this.EyePosition, this.hiderPosition - this.CachedTransform.position, num2, this.ObstacleMask, QueryTriggerInteraction.UseGlobal))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000074B0 File Offset: 0x000056B0
		protected override void SetCenterAndHeight()
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				this.center.x = this.EyePosition.x;
				this.center.y = this.EyePosition.z;
				this.heightPos = this.EyePosition.y;
				return;
			case FogOfWarWorld.GamePlane.XY:
				this.center.x = this.EyePosition.x;
				this.center.y = this.EyePosition.y;
				this.heightPos = this.EyePosition.z;
				return;
			case FogOfWarWorld.GamePlane.ZY:
				this.center.x = this.EyePosition.z;
				this.center.y = this.EyePosition.y;
				this.heightPos = this.EyePosition.x;
				return;
			default:
				return;
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00007594 File Offset: 0x00005794
		protected override float AngleBetweenVector2(Vector3 _vec1, Vector3 _vec2)
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				this.vec1.x = _vec1.x;
				this.vec1.y = _vec1.z;
				this.vec2.x = _vec2.x;
				this.vec2.y = _vec2.z;
				break;
			case FogOfWarWorld.GamePlane.XY:
				this.vec1.x = _vec1.x;
				this.vec1.y = _vec1.y;
				this.vec2.x = _vec2.x;
				this.vec2.y = _vec2.y;
				break;
			case FogOfWarWorld.GamePlane.ZY:
				this.vec1.x = _vec1.z;
				this.vec1.y = _vec1.y;
				this.vec2.x = _vec2.z;
				this.vec2.y = _vec2.y;
				break;
			}
			this._vec1Rotated90.x = -this.vec1.y;
			this._vec1Rotated90.y = this.vec1.x;
			float num = (Vector2.Dot(this._vec1Rotated90, this.vec2) < 0f) ? -1f : 1f;
			return Vector2.Angle(this.vec1, this.vec2) * num;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000076FC File Offset: 0x000058FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private float DistBetweenVectors(Vector3 _vec1, Vector3 _vec2)
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				this.vec1.x = _vec1.x;
				this.vec1.y = _vec1.z;
				this.vec2.x = _vec2.x;
				this.vec2.y = _vec2.z;
				break;
			case FogOfWarWorld.GamePlane.ZY:
				this.vec1.x = _vec1.z;
				this.vec1.y = _vec1.y;
				this.vec2.x = _vec2.z;
				this.vec2.y = _vec2.y;
				break;
			}
			return Vector2.Distance(this.vec1, this.vec2);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000077C8 File Offset: 0x000059C8
		private Vector3 GetForward()
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				return this.CachedTransform.forward;
			case FogOfWarWorld.GamePlane.XY:
				return this.CachedTransform.up;
			case FogOfWarWorld.GamePlane.ZY:
				return this.CachedTransform.up;
			default:
				return this.CachedTransform.forward;
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00007824 File Offset: 0x00005A24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				if (!angleIsGlobal)
				{
					angleInDegrees += this.CachedTransform.eulerAngles.y;
				}
				this.direction.x = Mathf.Cos(angleInDegrees * 0.017453292f);
				this.direction.z = Mathf.Sin(angleInDegrees * 0.017453292f);
				return this.direction;
			case FogOfWarWorld.GamePlane.XY:
				if (!angleIsGlobal)
				{
					angleInDegrees += this.CachedTransform.eulerAngles.z;
				}
				this.direction.x = Mathf.Cos(angleInDegrees * 0.017453292f);
				this.direction.y = Mathf.Sin(angleInDegrees * 0.017453292f);
				return this.direction;
			}
			if (!angleIsGlobal)
			{
				angleInDegrees += this.CachedTransform.eulerAngles.x;
			}
			this.direction.z = Mathf.Cos(angleInDegrees * 0.017453292f);
			this.direction.y = Mathf.Sin(angleInDegrees * 0.017453292f);
			return this.direction;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00007938 File Offset: 0x00005B38
		protected override Vector3 _Get3Dfrom2D(Vector2 pos)
		{
			switch (FogOfWarWorld.instance.gamePlane)
			{
			case FogOfWarWorld.GamePlane.XZ:
				return new Vector3(pos.x, this.CachedTransform.position.y, pos.y);
			case FogOfWarWorld.GamePlane.XY:
				return new Vector3(pos.x, pos.y, this.CachedTransform.position.z);
			case FogOfWarWorld.GamePlane.ZY:
				return new Vector3(this.CachedTransform.position.x, pos.y, pos.x);
			default:
				return new Vector3(pos.x, this.CachedTransform.position.y, pos.y);
			}
		}

		// Token: 0x040000DA RID: 218
		private NativeArray<RaycastCommand> RaycastCommandsNative;

		// Token: 0x040000DB RID: 219
		private NativeArray<RaycastHit> RaycastHits;

		// Token: 0x040000DC RID: 220
		private NativeArray<float3> Vector3Directions;

		// Token: 0x040000DD RID: 221
		private JobHandle IterationOneJobHandle;

		// Token: 0x040000DE RID: 222
		private FogOfWarRevealer3D.Phase1SetupJob SetupJob;

		// Token: 0x040000DF RID: 223
		private JobHandle SetupJobJobHandle;

		// Token: 0x040000E0 RID: 224
		private FogOfWarRevealer3D.GetVector2Data DataJob;

		// Token: 0x040000E1 RID: 225
		private JobHandle Vector2NormalJobHandle;

		// Token: 0x040000E2 RID: 226
		private PhysicsScene physicsScene;

		// Token: 0x040000E3 RID: 227
		public QueryParameters RayQueryParameters;

		// Token: 0x040000E4 RID: 228
		private RaycastHit RayHit;

		// Token: 0x040000E5 RID: 229
		private float2 vec2d;

		// Token: 0x040000E6 RID: 230
		private Vector3 hiderPosition;

		// Token: 0x040000E7 RID: 231
		private Vector3 revealerOrigin;

		// Token: 0x040000E8 RID: 232
		private float unobscuredSightDist;

		// Token: 0x040000E9 RID: 233
		private float heightDist;

		// Token: 0x040000EA RID: 234
		private Vector2 vec1;

		// Token: 0x040000EB RID: 235
		private Vector2 vec2;

		// Token: 0x040000EC RID: 236
		private Vector2 _vec1Rotated90;

		// Token: 0x040000ED RID: 237
		private Vector3 ForwardVectorCached;

		// Token: 0x040000EE RID: 238
		private Vector3 direction = Vector3.zero;

		// Token: 0x0200002B RID: 43
		[BurstCompile]
		private struct Phase1SetupJob : IJobParallelFor
		{
			// Token: 0x060000C8 RID: 200 RVA: 0x00008184 File Offset: 0x00006384
			public void Execute(int id)
			{
				float num = this.FirstAngle + this.AngleStep * (float)id;
				this.RayAngles[id] = num;
				float3 @float = this.DirFromAngle(num);
				this.Vector3Directions[id] = @float;
				this.RaycastCommandsNative[id] = new RaycastCommand(this.EyePosition, @float, this.Parameters, this.RayDistance);
			}

			// Token: 0x060000C9 RID: 201 RVA: 0x000081F0 File Offset: 0x000063F0
			private float3 DirFromAngle(float angleInDegrees)
			{
				float3 result = default(float3);
				switch (this.GamePlane)
				{
				case 0:
					result.x = Mathf.Cos(angleInDegrees * 0.017453292f);
					result.z = Mathf.Sin(angleInDegrees * 0.017453292f);
					return result;
				case 1:
					result.x = Mathf.Cos(angleInDegrees * 0.017453292f);
					result.y = Mathf.Sin(angleInDegrees * 0.017453292f);
					return result;
				}
				result.z = Mathf.Cos(angleInDegrees * 0.017453292f);
				result.y = Mathf.Sin(angleInDegrees * 0.017453292f);
				return result;
			}

			// Token: 0x0400015D RID: 349
			public int GamePlane;

			// Token: 0x0400015E RID: 350
			public float FirstAngle;

			// Token: 0x0400015F RID: 351
			public float AngleStep;

			// Token: 0x04000160 RID: 352
			public float RayDistance;

			// Token: 0x04000161 RID: 353
			public Vector3 EyePosition;

			// Token: 0x04000162 RID: 354
			public QueryParameters Parameters;

			// Token: 0x04000163 RID: 355
			[WriteOnly]
			public NativeArray<float> RayAngles;

			// Token: 0x04000164 RID: 356
			[WriteOnly]
			public NativeArray<float3> Vector3Directions;

			// Token: 0x04000165 RID: 357
			[WriteOnly]
			public NativeArray<RaycastCommand> RaycastCommandsNative;

			// Token: 0x04000166 RID: 358
			public PhysicsScene PhysicsScene;
		}

		// Token: 0x0200002C RID: 44
		[BurstCompile]
		private struct GetVector2Data : IJobParallelFor
		{
			// Token: 0x060000CA RID: 202 RVA: 0x00008298 File Offset: 0x00006498
			public void Execute(int id)
			{
				float3 @float;
				float3 float2;
				if (!this.approximately(this.RaycastHits[id].distance, 0f))
				{
					this.Hits[id] = true;
					this.Distances[id] = this.RaycastHits[id].distance;
					@float = this.RaycastHits[id].point;
					float2 = this.RaycastHits[id].normal;
				}
				else
				{
					this.Hits[id] = false;
					this.Distances[id] = this.RayDistance;
					@float = this.EyePosition + this.InDirections[id] * this.RayDistance;
					float2 = -this.InDirections[id];
				}
				float2 value = default(float2);
				float2 x = default(float2);
				float2 x2 = default(float2);
				switch (this.GamePlane)
				{
				case 0:
					value.x = @float.x;
					value.y = @float.z;
					x.x = this.InDirections[id].x;
					x.y = this.InDirections[id].z;
					x2.x = float2.x;
					x2.y = float2.z;
					break;
				case 1:
					value.x = @float.x;
					value.y = @float.y;
					x.x = this.InDirections[id].x;
					x.y = this.InDirections[id].y;
					x2.x = float2.x;
					x2.y = float2.y;
					break;
				case 2:
					value.x = @float.z;
					value.y = @float.y;
					x.x = this.InDirections[id].z;
					x.y = this.InDirections[id].y;
					x2.x = float2.z;
					x2.y = float2.y;
					break;
				}
				this.OutPoints[id] = value;
				this.OutDirections[id] = math.normalize(x);
				this.OutNormals[id] = math.normalize(x2);
			}

			// Token: 0x060000CB RID: 203 RVA: 0x0000851E File Offset: 0x0000671E
			private bool approximately(float a, float b)
			{
				return math.abs(b - a) < math.max(1E-06f * math.max(math.abs(a), math.abs(b)), 9.536743E-07f);
			}

			// Token: 0x04000167 RID: 359
			public int GamePlane;

			// Token: 0x04000168 RID: 360
			public float RayDistance;

			// Token: 0x04000169 RID: 361
			public float3 EyePosition;

			// Token: 0x0400016A RID: 362
			[ReadOnly]
			public NativeArray<RaycastHit> RaycastHits;

			// Token: 0x0400016B RID: 363
			[WriteOnly]
			public NativeArray<bool> Hits;

			// Token: 0x0400016C RID: 364
			[WriteOnly]
			public NativeArray<float> Distances;

			// Token: 0x0400016D RID: 365
			[ReadOnly]
			public NativeArray<float3> InDirections;

			// Token: 0x0400016E RID: 366
			[WriteOnly]
			public NativeArray<float2> OutPoints;

			// Token: 0x0400016F RID: 367
			[WriteOnly]
			public NativeArray<float2> OutDirections;

			// Token: 0x04000170 RID: 368
			[WriteOnly]
			public NativeArray<float2> OutNormals;
		}

		// Token: 0x0200002D RID: 45
		[BurstCompile]
		private struct SightRayFromRaycastHit : IJobParallelFor
		{
			// Token: 0x060000CC RID: 204 RVA: 0x0000854C File Offset: 0x0000674C
			public void Execute(int id)
			{
				FogOfWarRevealer.SightRay value = default(FogOfWarRevealer.SightRay);
				float3 @float;
				float3 float2;
				if (!this.approximately(this.RaycastHits[id].distance, 0f))
				{
					value.hit = true;
					value.distance = this.RaycastHits[id].distance;
					@float = this.RaycastHits[id].point;
					float2 = this.RaycastHits[id].normal;
				}
				else
				{
					value.hit = false;
					value.distance = this.RayDistance;
					@float = this.EyePosition + this.InDirections[id] * this.RayDistance;
					float2 = -this.InDirections[id];
				}
				float2 point = default(float2);
				float2 x = default(float2);
				float2 x2 = default(float2);
				switch (this.GamePlane)
				{
				case 0:
					point.x = @float.x;
					point.y = @float.z;
					x.x = this.InDirections[id].x;
					x.y = this.InDirections[id].z;
					x2.x = float2.x;
					x2.y = float2.z;
					break;
				case 1:
					point.x = @float.x;
					point.y = @float.y;
					x.x = this.InDirections[id].x;
					x.y = this.InDirections[id].y;
					x2.x = float2.x;
					x2.y = float2.y;
					break;
				case 2:
					point.x = @float.z;
					point.y = @float.y;
					x.x = this.InDirections[id].z;
					x.y = this.InDirections[id].y;
					x2.x = float2.z;
					x2.y = float2.y;
					break;
				}
				value.point = point;
				value.direction = math.normalize(x);
				value.normal = math.normalize(x2);
				this.SightRays[id] = value;
			}

			// Token: 0x060000CD RID: 205 RVA: 0x000087C5 File Offset: 0x000069C5
			private bool approximately(float a, float b)
			{
				return math.abs(b - a) < math.max(1E-06f * math.max(math.abs(a), math.abs(b)), 9.536743E-07f);
			}

			// Token: 0x04000171 RID: 369
			public int GamePlane;

			// Token: 0x04000172 RID: 370
			public float RayDistance;

			// Token: 0x04000173 RID: 371
			public float3 EyePosition;

			// Token: 0x04000174 RID: 372
			[ReadOnly]
			public NativeArray<RaycastHit> RaycastHits;

			// Token: 0x04000175 RID: 373
			[ReadOnly]
			public NativeArray<float3> InDirections;

			// Token: 0x04000176 RID: 374
			[WriteOnly]
			public NativeArray<FogOfWarRevealer.SightRay> SightRays;
		}
	}
}
