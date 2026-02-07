using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace FOW
{
	// Token: 0x0200000E RID: 14
	public abstract class FogOfWarRevealer : MonoBehaviour
	{
		// Token: 0x0600005E RID: 94 RVA: 0x0000471A File Offset: 0x0000291A
		private void OnEnable()
		{
			this.RegisterRevealer();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004722 File Offset: 0x00002922
		private void OnDisable()
		{
			this.DeregisterRevealer();
			this.Cleanup();
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004730 File Offset: 0x00002930
		private void OnDestroy()
		{
			this.Cleanup();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004738 File Offset: 0x00002938
		public void RegisterRevealer()
		{
			this.CachedTransform = base.transform;
			if (this.StartRevealerAsStatic)
			{
				this.SetRevealerAsStatic(true);
			}
			else
			{
				this.SetRevealerAsStatic(false);
			}
			this.NumberOfPoints = 0;
			if (FogOfWarWorld.instance == null)
			{
				if (!FogOfWarWorld.RevealersToRegister.Contains(this))
				{
					FogOfWarWorld.RevealersToRegister.Add(this);
				}
				return;
			}
			if (this.IsRegistered)
			{
				Debug.Log("Tried to double register revealer");
				return;
			}
			this.ViewPoints = new FogOfWarRevealer.SightSegment[FogOfWarWorld.instance.MaxPossibleSegmentsPerRevealer];
			this.EdgeAngles = new float[FogOfWarWorld.instance.MaxPossibleSegmentsPerRevealer];
			this.EdgeNormals = new float2[FogOfWarWorld.instance.MaxPossibleSegmentsPerRevealer];
			this.Angles = new float[this.ViewPoints.Length];
			this.Radii = new float[this.ViewPoints.Length];
			this.AreHits = new bool[this.ViewPoints.Length];
			this.IsRegistered = true;
			this.FogOfWarID = FogOfWarWorld.instance.RegisterRevealer(this);
			this.CircleStruct = default(FogOfWarWorld.RevealerStruct);
			this.LineOfSightPhase1();
			this.LineOfSightPhase2();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004854 File Offset: 0x00002A54
		public void DeregisterRevealer()
		{
			if (FogOfWarWorld.instance == null)
			{
				if (FogOfWarWorld.RevealersToRegister.Contains(this))
				{
					FogOfWarWorld.RevealersToRegister.Remove(this);
				}
				return;
			}
			if (!this.IsRegistered)
			{
				return;
			}
			foreach (FogOfWarHider fogOfWarHider in this.HidersSeen)
			{
				if (fogOfWarHider != null)
				{
					fogOfWarHider.RemoveObserver(this);
				}
			}
			this.HidersSeen.Clear();
			this.IsRegistered = false;
			FogOfWarWorld.instance.DeRegisterRevealer(this);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000048F8 File Offset: 0x00002AF8
		public void SetRevealerAsStatic(bool IsStatic)
		{
			if (this.IsRegistered)
			{
				if (this.StaticRevealer && !IsStatic)
				{
					FogOfWarWorld.numDynamicRevealers++;
				}
				else if (!this.StaticRevealer && IsStatic)
				{
					FogOfWarWorld.numDynamicRevealers--;
				}
			}
			this.StaticRevealer = IsStatic;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004946 File Offset: 0x00002B46
		public void ManualCalculateLineOfSight()
		{
			this.LineOfSightPhase1();
			this.LineOfSightPhase2();
		}

		// Token: 0x06000065 RID: 101
		protected abstract void _RevealHiders();

		// Token: 0x06000066 RID: 102 RVA: 0x00004954 File Offset: 0x00002B54
		public void RevealHiders()
		{
			this._RevealHiders();
		}

		// Token: 0x06000067 RID: 103
		protected abstract bool _TestPoint(Vector3 point);

		// Token: 0x06000068 RID: 104 RVA: 0x0000495C File Offset: 0x00002B5C
		public bool TestPoint(Vector3 point)
		{
			return this._TestPoint(point);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004968 File Offset: 0x00002B68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void AddViewPoint(bool hit, float distance, float angle, float step, float2 normal, float2 point, float2 dir)
		{
			if (this.NumberOfPoints == this.ViewPoints.Length)
			{
				Debug.LogError("Sight Segment buffer is full! Increase Maximum Segments per Revealer on Fog Of War World!");
				return;
			}
			this.ViewPoints[this.NumberOfPoints].DidHit = hit;
			this.ViewPoints[this.NumberOfPoints].Radius = distance;
			this.ViewPoints[this.NumberOfPoints].Angle = angle;
			this.ViewPoints[this.NumberOfPoints].Point = point;
			this.ViewPoints[this.NumberOfPoints].Direction = dir;
			this.EdgeAngles[this.NumberOfPoints] = -step;
			this.EdgeNormals[this.NumberOfPoints] = normal;
			this.NumberOfPoints++;
		}

		// Token: 0x0600006A RID: 106
		protected abstract void SetCenterAndHeight();

		// Token: 0x0600006B RID: 107 RVA: 0x00004A38 File Offset: 0x00002C38
		private void ApplyData()
		{
			for (int i = 0; i < this.NumberOfPoints; i++)
			{
				this.Angles[i] = this.ViewPoints[i].Angle;
				this.AreHits[i] = this.ViewPoints[i].DidHit;
				if (!this.AreHits[i])
				{
					this.ViewPoints[i].Radius = Mathf.Min(this.ViewPoints[i].Radius, this.ViewRadius);
				}
				this.Radii[i] = this.ViewPoints[i].Radius;
				if (i == this.NumberOfPoints - 1 && this.CircleIsComplete)
				{
					this.Angles[i] += 360f;
				}
			}
			this.SetCenterAndHeight();
			this.CircleStruct.CircleOrigin = this.center;
			this.CircleStruct.NumSegments = this.NumberOfPoints;
			this.CircleStruct.UnobscuredRadius = this.UnobscuredRadius;
			this.CircleStruct.CircleHeight = this.heightPos + this.ShaderEyeOffset;
			this.CircleStruct.CircleRadius = this.ViewRadius;
			this.CircleStruct.CircleFade = this.SoftenDistance;
			this.CircleStruct.VisionHeight = this.VisionHeight;
			this.CircleStruct.HeightFade = this.VisionHeightSoftenDistance;
			this.CircleStruct.Opacity = this.Opacity;
			FogOfWarWorld.instance.UpdateRevealerData(this.FogOfWarID, this.CircleStruct, this.NumberOfPoints, this.Angles, this.Radii, this.AreHits);
		}

		// Token: 0x0600006C RID: 108
		protected abstract float GetEuler();

		// Token: 0x0600006D RID: 109
		public abstract Vector3 GetEyePosition();

		// Token: 0x0600006E RID: 110
		public abstract Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal);

		// Token: 0x0600006F RID: 111
		protected abstract float AngleBetweenVector2(Vector3 _vec1, Vector3 _vec2);

		// Token: 0x06000070 RID: 112 RVA: 0x00004BDA File Offset: 0x00002DDA
		public float GetRayDistance()
		{
			return this.RayDistance;
		}

		// Token: 0x06000071 RID: 113
		protected abstract void _InitRevealer(int StepCount);

		// Token: 0x06000072 RID: 114 RVA: 0x00004BE4 File Offset: 0x00002DE4
		private void InitRevealer(int StepCount, float AngleStep)
		{
			if (this.FirstIteration != null && this.FirstIteration.Distances.IsCreated)
			{
				this.Cleanup();
			}
			for (int i = 0; i < this.ViewPoints.Length; i++)
			{
				this.ViewPoints[i] = default(FogOfWarRevealer.SightSegment);
			}
			this.FirstIterationStepCount = StepCount;
			this.FirstIteration = new SightIteration();
			this.FirstIteration.InitializeStruct(StepCount);
			this.IterationRayCount = this.NumExtraRaysOnIteration + 2;
			this.PointsJob = new FogOfWarRevealer.CalculateNextPoints
			{
				UpVector = FogOfWarWorld.UpVector,
				AngleStep = AngleStep,
				Distances = this.FirstIteration.Distances,
				Points = this.FirstIteration.Points,
				Directions = this.FirstIteration.Directions,
				Normals = this.FirstIteration.Normals,
				ExpectedNextPoints = this.FirstIteration.NextPoints
			};
			this.FirstIterationConditions = new NativeArray<bool>(this.NumSteps, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.FirstIterationConditionsJob = new FogOfWarRevealer.ConditionCalculations
			{
				Points = this.FirstIteration.Points,
				NextPoints = this.FirstIteration.NextPoints,
				Normals = this.FirstIteration.Normals,
				Hits = this.FirstIteration.Hits,
				IterateConditions = this.FirstIterationConditions
			};
			this.EdgeJob = default(FogOfWarRevealer.FindEdgeJob);
			this.Initialized = true;
			this._InitRevealer(StepCount);
		}

		// Token: 0x06000073 RID: 115
		protected abstract void CleanupRevealer();

		// Token: 0x06000074 RID: 116 RVA: 0x00004D74 File Offset: 0x00002F74
		private void Cleanup()
		{
			this.Initialized = false;
			if (this.FirstIteration == null || !this.FirstIteration.Distances.IsCreated)
			{
				return;
			}
			this.FirstIteration.DisposeStruct();
			this.FirstIterationConditions.Dispose();
			foreach (SightIteration sightIteration in this.SubIterations)
			{
				sightIteration.DisposeStruct();
			}
			this.SubIterations.Clear();
			this.CleanupRevealer();
		}

		// Token: 0x06000075 RID: 117
		protected abstract void IterationOne(int NumSteps, float firstAngle, float angleStep);

		// Token: 0x06000076 RID: 118
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract void RayCast(float angle, ref FogOfWarRevealer.SightRay ray);

		// Token: 0x06000077 RID: 119 RVA: 0x00004E10 File Offset: 0x00003010
		public void LineOfSightPhase1()
		{
			this.EdgeDstThreshold = Mathf.Max(0.001f, this.EdgeDstThreshold);
			this.CircleIsComplete = Mathf.Approximately(this.ViewAngle, 360f);
			this.CommandsPerJob = 32;
			this.EyePosition = this.GetEyePosition();
			this.RayDistance = this.ViewRadius;
			if (FogOfWarWorld.instance.UsingSoftening)
			{
				this.RayDistance += this.SoftenDistance;
			}
			this.NumberOfPoints = 0;
			this.NumSteps = Mathf.Max(2, Mathf.CeilToInt(this.ViewAngle * this.RaycastResolution));
			this.AngleStep = this.ViewAngle / (float)(this.NumSteps - 1);
			if (this.Initialized && this.FirstIteration != null)
			{
				NativeArray<float> rayAngles = this.FirstIteration.RayAngles;
				if (this.FirstIteration.RayAngles.Length == this.NumSteps)
				{
					goto IL_EC;
				}
			}
			this.InitRevealer(this.NumSteps, this.AngleStep);
			IL_EC:
			float firstAngle = (-this.GetEuler() + 360f + 90f) % 360f - this.ViewAngle / 2f;
			this.IterationOne(this.NumSteps, firstAngle, this.AngleStep);
			this.FirstIterationConditionsJob.DoubleHitMaxAngleDelta = this.DoubleHitMaxAngleDelta;
			this.FirstIterationConditionsJob.EdgeDstThreshold = this.EdgeDstThreshold;
			this.FirstIterationConditionsJob.AddCorners = this.AddCorners;
			this.FirstIterationConditionsJobHandle = this.FirstIterationConditionsJob.Schedule(this.NumSteps, this.CommandsPerJob, this.PointsJobHandle);
			JobHandle.ScheduleBatchedJobs();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004FA0 File Offset: 0x000031A0
		public void LineOfSightPhase2()
		{
			this.FirstIterationConditionsJobHandle.Complete();
			this.AddViewPoint(this.FirstIteration.Hits[0], this.FirstIteration.Distances[0], this.FirstIteration.RayAngles[0], 0f, this.FirstIteration.Normals[0], this.FirstIteration.Points[0], this.FirstIteration.Directions[0]);
			this.SortData(ref this.FirstIteration, this.AngleStep, this.NumSteps, 0, true);
			while (this.InUseIterations.Count > 0)
			{
				this.SubIterations.Push(this.InUseIterations.Pop());
			}
			if (this.NumberOfPoints == 1 && !this.ViewPoints[0].DidHit && !this.ViewPoints[1].DidHit)
			{
				this.AddViewPoint(false, this.ViewPoints[0].Radius, this.ViewPoints[0].Angle + this.ViewAngle / 2f, -this.EdgeAngles[0], new float2(0f, 0f), new float2(0f, 0f), new float2(0f, 0f));
			}
			if (this.CircleIsComplete)
			{
				if ((this.FirstIteration.Hits[this.NumSteps - 1] || this.FirstIteration.Hits[0]) && Vector2.Distance(this.FirstIteration.NextPoints[this.NumSteps - 1], this.FirstIteration.Points[0]) > 0.05f)
				{
					this.AddViewPoint(this.FirstIteration.Hits[this.NumSteps - 1], this.FirstIteration.Distances[this.NumSteps - 1], this.FirstIteration.RayAngles[this.NumSteps - 1], 0f, this.FirstIteration.Normals[this.NumSteps - 1], this.FirstIteration.Points[this.NumSteps - 1], this.FirstIteration.Directions[this.NumSteps - 1]);
				}
				this.AddViewPoint(this.FirstIteration.Hits[0], this.FirstIteration.Distances[0], this.FirstIteration.RayAngles[0], 0f, this.FirstIteration.Normals[0], this.FirstIteration.Points[0], this.FirstIteration.Directions[0]);
			}
			else
			{
				int index = this.NumSteps - 1;
				this.AddViewPoint(this.FirstIteration.Hits[index], this.FirstIteration.Distances[index], this.FirstIteration.RayAngles[index], 0f, this.FirstIteration.Normals[index], this.FirstIteration.Points[index], this.FirstIteration.Directions[index]);
			}
			if (this.ResolveEdge)
			{
				this.FindEdges();
			}
			this.ApplyData();
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00005320 File Offset: 0x00003520
		private void SortData(ref SightIteration iteration, float angleStep, int iterationSteps, int iterationNumber, bool FirstIteration = false)
		{
			float angleStep2 = angleStep / (float)(this.IterationRayCount - 1);
			for (int i = 1; i < iterationSteps; i++)
			{
				bool flag4;
				if (!FirstIteration)
				{
					float num = this.AngleBetweenVector2(iteration.Normals[i], iteration.Normals[i - 1]);
					bool flag = math.abs(num) > this.DoubleHitMaxAngleDelta;
					bool flag2 = !this.Vector2Aprox(iteration.Points[i], iteration.NextPoints[i - 1]);
					bool flag3 = ((iteration.Hits[i - 1] || iteration.Hits[i]) && (flag2 || flag)) || iteration.Hits[i - 1] != iteration.Hits[i];
					if (!this.AddCorners && flag && num > 0f && !flag2)
					{
						flag3 = false;
					}
					flag4 = flag3;
				}
				else
				{
					flag4 = this.FirstIterationConditions[i];
				}
				if (flag4)
				{
					if (iterationNumber == this.NumExtraIterations)
					{
						this.AddViewPoint(iteration.Hits[i - 1], iteration.Distances[i - 1], iteration.RayAngles[i - 1], -angleStep, iteration.Normals[i - 1], iteration.Points[i - 1], iteration.Directions[i - 1]);
						this.AddViewPoint(iteration.Hits[i], iteration.Distances[i], iteration.RayAngles[i], angleStep, iteration.Normals[i], iteration.Points[i], iteration.Directions[i]);
					}
					else
					{
						float initialAngle = iteration.RayAngles[i - 1];
						SightIteration sightIteration = this.Iterate(iterationNumber + 1, initialAngle, angleStep2, ref iteration, i - 1);
						this.SortData(ref sightIteration, angleStep2, this.IterationRayCount, iterationNumber + 1, false);
					}
				}
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000552C File Offset: 0x0000372C
		private SightIteration GetSubIteration()
		{
			if (this.SubIterations.Count > 0)
			{
				return this.SubIterations.Pop();
			}
			SightIteration sightIteration = new SightIteration();
			sightIteration.InitializeStruct(this.IterationRayCount);
			return sightIteration;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000555C File Offset: 0x0000375C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private SightIteration Iterate(int iterNumber, float initialAngle, float AngleStep, ref SightIteration PreviousIteration, int PrevIterStartIndex)
		{
			SightIteration subIteration = this.GetSubIteration();
			this.InUseIterations.Push(subIteration);
			subIteration.RayAngles[0] = PreviousIteration.RayAngles[PrevIterStartIndex];
			subIteration.Hits[0] = PreviousIteration.Hits[PrevIterStartIndex];
			subIteration.Distances[0] = PreviousIteration.Distances[PrevIterStartIndex];
			subIteration.Points[0] = PreviousIteration.Points[PrevIterStartIndex];
			subIteration.Directions[0] = PreviousIteration.Directions[PrevIterStartIndex];
			subIteration.Normals[0] = PreviousIteration.Normals[PrevIterStartIndex];
			float2 @float = new float2(-subIteration.Normals[0].y, subIteration.Normals[0].x);
			float x = 180f - (this.AngleBetweenVector2(@float, -subIteration.Directions[0]) + AngleStep);
			float rhs = subIteration.Distances[0] * math.sin(math.radians(AngleStep)) / Mathf.Sin(math.radians(x));
			subIteration.NextPoints[0] = subIteration.Points[0] + @float * rhs;
			for (int i = 1; i < this.IterationRayCount - 1; i++)
			{
				this.RayCast(initialAngle + AngleStep * (float)i, ref this.currentRay);
				subIteration.RayAngles[i] = this.currentRay.angle;
				subIteration.Hits[i] = this.currentRay.hit;
				subIteration.Distances[i] = this.currentRay.distance;
				subIteration.Points[i] = this.currentRay.point;
				subIteration.Directions[i] = this.currentRay.direction;
				subIteration.Normals[i] = this.currentRay.normal;
				@float = new float2(-subIteration.Normals[i].y, subIteration.Normals[i].x);
				x = 180f - (this.AngleBetweenVector2(@float, -subIteration.Directions[i]) + AngleStep);
				rhs = subIteration.Distances[i] * math.sin(math.radians(AngleStep)) / Mathf.Sin(math.radians(x));
				subIteration.NextPoints[i] = subIteration.Points[i] + @float * rhs;
			}
			subIteration.RayAngles[this.IterationRayCount - 1] = PreviousIteration.RayAngles[PrevIterStartIndex + 1];
			subIteration.Hits[this.IterationRayCount - 1] = PreviousIteration.Hits[PrevIterStartIndex + 1];
			subIteration.Distances[this.IterationRayCount - 1] = PreviousIteration.Distances[PrevIterStartIndex + 1];
			subIteration.Points[this.IterationRayCount - 1] = PreviousIteration.Points[PrevIterStartIndex + 1];
			subIteration.Directions[this.IterationRayCount - 1] = PreviousIteration.Directions[PrevIterStartIndex + 1];
			subIteration.Normals[this.IterationRayCount - 1] = PreviousIteration.Normals[PrevIterStartIndex + 1];
			subIteration.NextPoints[this.IterationRayCount - 1] = PreviousIteration.NextPoints[PrevIterStartIndex + 1];
			return subIteration;
		}

		// Token: 0x0600007C RID: 124
		protected abstract void _FindEdge();

		// Token: 0x0600007D RID: 125 RVA: 0x00005909 File Offset: 0x00003B09
		private void FindEdgesJobs()
		{
			this._FindEdge();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00005914 File Offset: 0x00003B14
		private void FindEdges()
		{
			for (int i = 0; i < this.NumberOfPoints; i++)
			{
				float num = this.ViewPoints[i].Angle;
				float num2 = this.EdgeAngles[i];
				num2 /= 2f;
				num += num2;
				for (int j = 0; j < this.MaxEdgeResolveIterations; j++)
				{
					this.RayCast(num, ref this.currentRay);
					this.RotatedNormal.x = -this.EdgeNormals[i].y;
					this.RotatedNormal.y = this.EdgeNormals[i].x;
					float num3 = num - this.ViewPoints[i].Angle;
					float x = 180f - (this.AngleBetweenVector2(this.RotatedNormal, -this.ViewPoints[i].Direction) + num3);
					float rhs = this.ViewPoints[i].Radius * math.sin(math.radians(num3)) / math.sin(math.radians(x));
					float2 v = this.ViewPoints[i].Point + this.RotatedNormal * rhs;
					float num4;
					if (this.ViewPoints[i].DidHit != this.currentRay.hit || Vector2.Angle(this.EdgeNormals[i], this.currentRay.normal) > this.DoubleHitMaxAngleDelta || !this.Vector2Aprox(v, this.currentRay.point))
					{
						num4 = -1f;
					}
					else
					{
						num4 = 1f;
						this.ViewPoints[i].Angle = num;
						this.ViewPoints[i].Radius = this.currentRay.distance;
						this.EdgeNormals[i] = this.currentRay.normal;
						this.ViewPoints[i].Point = this.currentRay.point;
					}
					num2 /= 2f;
					if (math.abs(num2) < this.MaxAcceptableEdgeAngleDifference)
					{
						break;
					}
					num += num2 * num4;
				}
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00005B48 File Offset: 0x00003D48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private float AngleBetweenVector2(float2 vec1, float2 vec2)
		{
			this.vec1Rotated90.x = -vec1.y;
			this.vec1Rotated90.y = vec1.x;
			float num = (math.dot(this.vec1Rotated90, vec2) < 0f) ? -1f : 1f;
			return Vector2.Angle(vec1, vec2) * num;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00005BAB File Offset: 0x00003DAB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool Vector2Aprox(float2 v1, float2 v2)
		{
			return math.distancesq(v1, v2) < this.EdgeDstThreshold;
		}

		// Token: 0x06000081 RID: 129
		protected abstract Vector3 _Get3Dfrom2D(Vector2 twoD);

		// Token: 0x06000082 RID: 130 RVA: 0x00005BBC File Offset: 0x00003DBC
		private Vector3 Get3Dfrom2D(Vector2 twoD)
		{
			return this._Get3Dfrom2D(twoD);
		}

		// Token: 0x04000085 RID: 133
		[Header("Customization Variables")]
		[SerializeField]
		public float ViewRadius = 15f;

		// Token: 0x04000086 RID: 134
		[SerializeField]
		public float SoftenDistance = 3f;

		// Token: 0x04000087 RID: 135
		[Range(1f, 360f)]
		[SerializeField]
		public float ViewAngle = 360f;

		// Token: 0x04000088 RID: 136
		[SerializeField]
		public float UnobscuredRadius = 1f;

		// Token: 0x04000089 RID: 137
		[Range(0f, 1f)]
		[SerializeField]
		public float Opacity = 1f;

		// Token: 0x0400008A RID: 138
		[SerializeField]
		protected bool AddCorners = true;

		// Token: 0x0400008B RID: 139
		[Range(0f, 1f)]
		[SerializeField]
		public float RevealHiderInFadeOutZonePercentage = 0.5f;

		// Token: 0x0400008C RID: 140
		[Tooltip("how high above this object should the sight be calculated from")]
		[SerializeField]
		public float EyeOffset;

		// Token: 0x0400008D RID: 141
		[Tooltip("An offset used only in the shader, to determine how high above the revealer vision height should be calculated at")]
		[SerializeField]
		public float ShaderEyeOffset;

		// Token: 0x0400008E RID: 142
		[SerializeField]
		public float VisionHeight = 3f;

		// Token: 0x0400008F RID: 143
		[SerializeField]
		public float VisionHeightSoftenDistance = 1.5f;

		// Token: 0x04000090 RID: 144
		[SerializeField]
		public bool SampleHidersAtRevealerHeight = true;

		// Token: 0x04000091 RID: 145
		[SerializeField]
		public bool CalculateHidersAtHiderHeight;

		// Token: 0x04000092 RID: 146
		[SerializeField]
		public LayerMask ObstacleMask;

		// Token: 0x04000093 RID: 147
		[Header("Quality Variables")]
		[SerializeField]
		public float RaycastResolution = 0.5f;

		// Token: 0x04000094 RID: 148
		public bool ResolveEdge = true;

		// Token: 0x04000095 RID: 149
		[Range(1f, 30f)]
		[Tooltip("Higher values will lead to more accurate edge detection, especially at higher distances. however, this will also result in more raycasts.")]
		[SerializeField]
		protected int MaxEdgeResolveIterations = 10;

		// Token: 0x04000096 RID: 150
		[Range(0f, 10f)]
		[SerializeField]
		protected int NumExtraIterations = 4;

		// Token: 0x04000097 RID: 151
		[Range(1f, 5f)]
		[SerializeField]
		protected int NumExtraRaysOnIteration = 3;

		// Token: 0x04000098 RID: 152
		protected int IterationRayCount;

		// Token: 0x04000099 RID: 153
		[SerializeField]
		protected int MaxHidersSampledPerFrame = 50;

		// Token: 0x0400009A RID: 154
		[Header("Technical Variables")]
		[Range(0.001f, 1f)]
		[Tooltip("Lower values will lead to more accurate edge detection, especially at higher distances. however, this will also result in more raycasts.")]
		[SerializeField]
		protected float MaxAcceptableEdgeAngleDifference = 0.005f;

		// Token: 0x0400009B RID: 155
		[Range(0.001f, 1f)]
		[SerializeField]
		protected float EdgeDstThreshold = 0.1f;

		// Token: 0x0400009C RID: 156
		[SerializeField]
		protected float DoubleHitMaxAngleDelta = 15f;

		// Token: 0x0400009D RID: 157
		[Tooltip("Static revealers are revealers that need the sight function to be called manually, similar to the 'Called Elsewhere' option on FOW world. To change this at runtime, use the SetRevealerAsStatic(bool IsStatic) Method.")]
		[SerializeField]
		public bool StartRevealerAsStatic;

		// Token: 0x0400009E RID: 158
		[HideInInspector]
		public bool StaticRevealer;

		// Token: 0x0400009F RID: 159
		[HideInInspector]
		public int FogOfWarID;

		// Token: 0x040000A0 RID: 160
		[HideInInspector]
		public int IndexID;

		// Token: 0x040000A1 RID: 161
		protected FogOfWarWorld.RevealerStruct CircleStruct;

		// Token: 0x040000A2 RID: 162
		protected bool IsRegistered;

		// Token: 0x040000A3 RID: 163
		public FogOfWarRevealer.SightSegment[] ViewPoints;

		// Token: 0x040000A4 RID: 164
		protected float[] EdgeAngles;

		// Token: 0x040000A5 RID: 165
		protected float2[] EdgeNormals;

		// Token: 0x040000A6 RID: 166
		[HideInInspector]
		public int NumberOfPoints;

		// Token: 0x040000A7 RID: 167
		[HideInInspector]
		public float[] Angles;

		// Token: 0x040000A8 RID: 168
		[HideInInspector]
		public float[] Radii;

		// Token: 0x040000A9 RID: 169
		[HideInInspector]
		public bool[] AreHits;

		// Token: 0x040000AA RID: 170
		[Header("Debugging")]
		public HashSet<FogOfWarHider> HidersSeen = new HashSet<FogOfWarHider>();

		// Token: 0x040000AB RID: 171
		protected Transform CachedTransform;

		// Token: 0x040000AC RID: 172
		protected int _lastHiderIndex;

		// Token: 0x040000AD RID: 173
		protected float heightPos;

		// Token: 0x040000AE RID: 174
		protected Vector2 center;

		// Token: 0x040000AF RID: 175
		protected bool CircleIsComplete;

		// Token: 0x040000B0 RID: 176
		protected bool Initialized;

		// Token: 0x040000B1 RID: 177
		protected Vector3 EyePosition;

		// Token: 0x040000B2 RID: 178
		protected int FirstIterationStepCount;

		// Token: 0x040000B3 RID: 179
		protected SightIteration FirstIteration;

		// Token: 0x040000B4 RID: 180
		protected int CommandsPerJob;

		// Token: 0x040000B5 RID: 181
		protected FogOfWarRevealer.CalculateNextPoints PointsJob;

		// Token: 0x040000B6 RID: 182
		protected JobHandle PointsJobHandle;

		// Token: 0x040000B7 RID: 183
		public NativeArray<bool> FirstIterationConditions;

		// Token: 0x040000B8 RID: 184
		public FogOfWarRevealer.ConditionCalculations FirstIterationConditionsJob;

		// Token: 0x040000B9 RID: 185
		public JobHandle FirstIterationConditionsJobHandle;

		// Token: 0x040000BA RID: 186
		protected float RayDistance;

		// Token: 0x040000BB RID: 187
		protected FogOfWarRevealer.SightRay currentRay;

		// Token: 0x040000BC RID: 188
		private int NumSteps;

		// Token: 0x040000BD RID: 189
		private float AngleStep;

		// Token: 0x040000BE RID: 190
		private float2 RotatedNormal;

		// Token: 0x040000BF RID: 191
		private Stack<SightIteration> SubIterations = new Stack<SightIteration>();

		// Token: 0x040000C0 RID: 192
		private Stack<SightIteration> InUseIterations = new Stack<SightIteration>();

		// Token: 0x040000C1 RID: 193
		private bool ProfileExtraIterations;

		// Token: 0x040000C2 RID: 194
		protected FogOfWarRevealer.FindEdgeJob EdgeJob;

		// Token: 0x040000C3 RID: 195
		protected JobHandle EdgeJobHandle;

		// Token: 0x040000C4 RID: 196
		private float2 vec1Rotated90;

		// Token: 0x02000024 RID: 36
		public enum RevealerMode
		{
			// Token: 0x04000136 RID: 310
			ConstantDensity,
			// Token: 0x04000137 RID: 311
			EdgeDetect
		}

		// Token: 0x02000025 RID: 37
		public struct SightRay
		{
			// Token: 0x060000BE RID: 190 RVA: 0x00007CE1 File Offset: 0x00005EE1
			public void SetData(bool _hit, Vector2 _point, float _distance, Vector2 _normal, Vector2 _direction)
			{
				this.hit = _hit;
				this.point = _point;
				this.distance = _distance;
				this.normal = _normal;
				this.direction = _direction;
			}

			// Token: 0x04000138 RID: 312
			public bool hit;

			// Token: 0x04000139 RID: 313
			public float2 point;

			// Token: 0x0400013A RID: 314
			public float distance;

			// Token: 0x0400013B RID: 315
			public float angle;

			// Token: 0x0400013C RID: 316
			public float2 normal;

			// Token: 0x0400013D RID: 317
			public float2 direction;
		}

		// Token: 0x02000026 RID: 38
		public struct SightSegment
		{
			// Token: 0x060000BF RID: 191 RVA: 0x00007D17 File Offset: 0x00005F17
			public SightSegment(float rad, float ang, bool hit, float2 point, float2 dir)
			{
				this.Radius = rad;
				this.Angle = ang;
				this.DidHit = hit;
				this.Point = point;
				this.Direction = dir;
			}

			// Token: 0x0400013E RID: 318
			public float Radius;

			// Token: 0x0400013F RID: 319
			public float Angle;

			// Token: 0x04000140 RID: 320
			public bool DidHit;

			// Token: 0x04000141 RID: 321
			public float2 Point;

			// Token: 0x04000142 RID: 322
			public float2 Direction;
		}

		// Token: 0x02000027 RID: 39
		[BurstCompile]
		public struct ConditionCalculations : IJobParallelFor
		{
			// Token: 0x060000C0 RID: 192 RVA: 0x00007D40 File Offset: 0x00005F40
			public void Execute(int id)
			{
				if (id == 0)
				{
					return;
				}
				float num = this.AngleBetweenVector2(this.Normals[id], this.Normals[id - 1]);
				bool flag = math.abs(num) > this.DoubleHitMaxAngleDelta;
				bool flag2 = !this.Vector2Aprox(this.Points[id], this.NextPoints[id - 1]);
				bool value = ((this.Hits[id - 1] || this.Hits[id]) && (flag2 || flag)) || this.Hits[id - 1] != this.Hits[id];
				if (!this.AddCorners && flag && num > 0f && !flag2)
				{
					value = false;
				}
				this.IterateConditions[id] = value;
			}

			// Token: 0x060000C1 RID: 193 RVA: 0x00007E14 File Offset: 0x00006014
			private float AngleBetweenVector2(float2 vec1, float2 vec2)
			{
				float num = (math.dot(new float2
				{
					x = -vec1.y,
					y = vec1.x
				}, vec2) < 0f) ? -1f : 1f;
				return Vector2.Angle(vec1, vec2) * num;
			}

			// Token: 0x060000C2 RID: 194 RVA: 0x00007E72 File Offset: 0x00006072
			private bool Vector2Aprox(float2 v1, float2 v2)
			{
				return math.distancesq(v1, v2) < this.EdgeDstThreshold;
			}

			// Token: 0x04000143 RID: 323
			public float DoubleHitMaxAngleDelta;

			// Token: 0x04000144 RID: 324
			public float EdgeDstThreshold;

			// Token: 0x04000145 RID: 325
			public bool AddCorners;

			// Token: 0x04000146 RID: 326
			[ReadOnly]
			[NativeDisableParallelForRestriction]
			public NativeArray<float2> Points;

			// Token: 0x04000147 RID: 327
			[ReadOnly]
			[NativeDisableParallelForRestriction]
			public NativeArray<float2> NextPoints;

			// Token: 0x04000148 RID: 328
			[ReadOnly]
			[NativeDisableParallelForRestriction]
			public NativeArray<float2> Normals;

			// Token: 0x04000149 RID: 329
			[ReadOnly]
			[NativeDisableParallelForRestriction]
			public NativeArray<bool> Hits;

			// Token: 0x0400014A RID: 330
			[WriteOnly]
			[NativeDisableParallelForRestriction]
			public NativeArray<bool> IterateConditions;
		}

		// Token: 0x02000028 RID: 40
		public struct EdgeResolveData
		{
			// Token: 0x0400014B RID: 331
			public float CurrentAngle;

			// Token: 0x0400014C RID: 332
			public float AngleAdd;

			// Token: 0x0400014D RID: 333
			public float Sign;

			// Token: 0x0400014E RID: 334
			public bool Break;
		}

		// Token: 0x02000029 RID: 41
		[BurstCompile]
		protected struct FindEdgeJob : IJobParallelFor
		{
			// Token: 0x060000C3 RID: 195 RVA: 0x00007E84 File Offset: 0x00006084
			public void Execute(int index)
			{
				FogOfWarRevealer.EdgeResolveData edgeResolveData = this.EdgeData[index];
				if (edgeResolveData.Break)
				{
					return;
				}
				FogOfWarRevealer.SightSegment sightSegment = this.SightSegments[index];
				FogOfWarRevealer.SightRay sightRay = this.SightRays[index];
				float2 @float = this.EdgeNormals[index];
				float num = edgeResolveData.CurrentAngle - sightSegment.Angle;
				float2 float2 = new float2(-@float.y, @float.x);
				float x = 180f - (this.AngleBetweenVector2(float2, -sightSegment.Direction) + num);
				float rhs = sightSegment.Radius * math.sin(math.radians(num)) / Mathf.Sin(math.radians(x));
				float2 v = sightSegment.Point + float2 * rhs;
				if (sightSegment.DidHit != sightRay.hit || Vector2.Angle(@float, sightRay.normal) > this.DoubleHitMaxAngleDelta || !this.Vector2Aprox(v, sightRay.point))
				{
					edgeResolveData.Sign = -1f;
				}
				else
				{
					edgeResolveData.Sign = 1f;
					sightSegment.Angle = edgeResolveData.CurrentAngle;
					sightSegment.Radius = sightRay.distance;
					this.EdgeNormals[index] = sightRay.normal;
					sightSegment.Point = sightRay.point;
				}
				this.SightSegments[index] = sightSegment;
				edgeResolveData.AngleAdd /= 2f;
				if (math.abs(edgeResolveData.AngleAdd) < this.MaxAcceptableEdgeAngleDifference)
				{
					edgeResolveData.Break = true;
				}
				edgeResolveData.CurrentAngle += edgeResolveData.AngleAdd * edgeResolveData.Sign;
				this.EdgeData[index] = edgeResolveData;
			}

			// Token: 0x060000C4 RID: 196 RVA: 0x00008034 File Offset: 0x00006234
			private float AngleBetweenVector2(float2 vec1, float2 vec2)
			{
				float num = (math.dot(new float2(-vec1.y, vec1.x), vec2) < 0f) ? -1f : 1f;
				return Vector2.Angle(vec1, vec2) * num;
			}

			// Token: 0x060000C5 RID: 197 RVA: 0x00008080 File Offset: 0x00006280
			private bool Vector2Aprox(float2 v1, float2 v2)
			{
				return math.distancesq(v1, v2) < this.EdgeDstThreshold;
			}

			// Token: 0x0400014F RID: 335
			public float MaxAcceptableEdgeAngleDifference;

			// Token: 0x04000150 RID: 336
			public float DoubleHitMaxAngleDelta;

			// Token: 0x04000151 RID: 337
			public float EdgeDstThreshold;

			// Token: 0x04000152 RID: 338
			[ReadOnly]
			public NativeArray<FogOfWarRevealer.SightRay> SightRays;

			// Token: 0x04000153 RID: 339
			public NativeArray<FogOfWarRevealer.SightSegment> SightSegments;

			// Token: 0x04000154 RID: 340
			public NativeArray<float2> EdgeNormals;

			// Token: 0x04000155 RID: 341
			public NativeArray<FogOfWarRevealer.EdgeResolveData> EdgeData;
		}

		// Token: 0x0200002A RID: 42
		[BurstCompile]
		public struct CalculateNextPoints : IJobParallelFor
		{
			// Token: 0x060000C6 RID: 198 RVA: 0x00008094 File Offset: 0x00006294
			public void Execute(int id)
			{
				float2 @float = this.Normals[id];
				float2 float2 = new float2(-@float.y, @float.x);
				float x = 180f - (this.AngleBetweenVector2(float2, -this.Directions[id]) + this.AngleStep);
				float rhs = this.Distances[id] * math.sin(math.radians(this.AngleStep)) / Mathf.Sin(math.radians(x));
				this.ExpectedNextPoints[id] = this.Points[id] + float2 * rhs;
			}

			// Token: 0x060000C7 RID: 199 RVA: 0x00008138 File Offset: 0x00006338
			private float AngleBetweenVector2(float2 vec1, float2 vec2)
			{
				float num = (math.dot(new float2(-vec1.y, vec1.x), vec2) < 0f) ? -1f : 1f;
				return Vector2.Angle(vec1, vec2) * num;
			}

			// Token: 0x04000156 RID: 342
			public Vector3 UpVector;

			// Token: 0x04000157 RID: 343
			public float AngleStep;

			// Token: 0x04000158 RID: 344
			[ReadOnly]
			public NativeArray<float> Distances;

			// Token: 0x04000159 RID: 345
			[ReadOnly]
			public NativeArray<float2> Points;

			// Token: 0x0400015A RID: 346
			[ReadOnly]
			public NativeArray<float2> Normals;

			// Token: 0x0400015B RID: 347
			[ReadOnly]
			public NativeArray<float2> Directions;

			// Token: 0x0400015C RID: 348
			[WriteOnly]
			public NativeArray<float2> ExpectedNextPoints;
		}
	}
}
