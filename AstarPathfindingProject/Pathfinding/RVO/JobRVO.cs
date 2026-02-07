using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.Jobs;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002A5 RID: 677
	[BurstCompile(CompileSynchronously = true, FloatMode = FloatMode.Default)]
	public struct JobRVO<MovementPlaneWrapper> : IJobParallelForBatched where MovementPlaneWrapper : struct, IMovementPlaneWrapper
	{
		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06001007 RID: 4103 RVA: 0x0001797A File Offset: 0x00015B7A
		public bool allowBoundsChecks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0006300B File Offset: 0x0006120B
		public void Execute(int startIndex, int batchSize)
		{
			this.ExecuteORCA(startIndex, batchSize);
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x00063018 File Offset: 0x00061218
		private unsafe static void InsertionSort<[IsUnmanaged] T, U>(UnsafeSpan<T> data, U comparer) where T : struct, ValueType where U : IComparer<T>
		{
			for (int i = 1; i < data.Length; i++)
			{
				T t = *data[i];
				int num = i - 1;
				while (num >= 0 && comparer.Compare(*data[num], t) > 0)
				{
					*data[num + 1] = *data[num];
					num--;
				}
				*data[num + 1] = t;
			}
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x000630A0 File Offset: 0x000612A0
		private unsafe void GenerateObstacleVOs(int agentIndex, NativeList<int> adjacentObstacleIdsScratch, NativeArray<int2> adjacentObstacleVerticesScratch, NativeArray<float> segmentDistancesScratch, NativeArray<int> sortedVerticesScratch, NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> orcaLines, NativeArray<int> orcaLineToAgent, [NoAlias] ref int numLines, [NoAlias] in MovementPlaneWrapper movementPlane, float2 optimalVelocity)
		{
			if (!this.useNavmeshAsObstacle)
			{
				return;
			}
			MovementPlaneWrapper movementPlaneWrapper = movementPlane;
			float num;
			float2 @float = movementPlaneWrapper.ToPlane(this.agentData.position[agentIndex], out num);
			float num2 = this.agentData.height[agentIndex];
			float num3 = this.agentData.radius[agentIndex];
			float num4 = num3 * 0.01f;
			float num5 = math.rcp(this.agentData.obstacleTimeHorizon[agentIndex]);
			Aliasing.ExpectNotAliased<NativeArray<float3>, NativeArray<float3>>(this.agentData.collisionNormal, this.agentData.position);
			int num6 = this.agentData.hierarchicalNodeIndex[agentIndex];
			if (num6 == -1)
			{
				return;
			}
			float3 float2 = (num4 + num3 + this.agentData.obstacleTimeHorizon[agentIndex] * this.agentData.maxSpeed[agentIndex]) * new float3(2f, 0f, 2f);
			float2.y = this.agentData.height[agentIndex] * 2f;
			Bounds bounds = new Bounds(new Vector3(@float.x, num, @float.y), float2);
			float num7 = math.lengthsq(bounds.extents);
			adjacentObstacleIdsScratch.Clear();
			movementPlaneWrapper = movementPlane;
			Bounds bounds2 = movementPlaneWrapper.ToWorld(bounds);
			this.navmeshEdgeData.GetObstaclesInRange(num6, bounds2, adjacentObstacleIdsScratch);
			for (int i = 0; i < adjacentObstacleIdsScratch.Length; i++)
			{
				int index = adjacentObstacleIdsScratch[i];
				UnmanagedObstacle unmanagedObstacle = this.navmeshEdgeData.obstacleData.obstacles[index];
				UnsafeSpan<float3> span = this.navmeshEdgeData.obstacleData.obstacleVertices.GetSpan(unmanagedObstacle.verticesAllocation);
				UnsafeSpan<ObstacleVertexGroup> span2 = this.navmeshEdgeData.obstacleData.obstacleVertexGroups.GetSpan(unmanagedObstacle.groupsAllocation);
				int num8 = 0;
				int num9 = 0;
				for (int j = 0; j < span2.Length; j++)
				{
					ObstacleVertexGroup obstacleVertexGroup = *span2[j];
					if (!math.all(obstacleVertexGroup.boundsMx >= bounds2.min & obstacleVertexGroup.boundsMn <= bounds2.max))
					{
						num8 += obstacleVertexGroup.vertexCount;
					}
					else
					{
						int num10 = num8;
						int num11 = num8 + obstacleVertexGroup.vertexCount - 1;
						if (num11 >= adjacentObstacleVerticesScratch.Length)
						{
							break;
						}
						for (int k = num10; k < num10 + obstacleVertexGroup.vertexCount; k++)
						{
							adjacentObstacleVerticesScratch[k] = new int2(k - 1, k + 1);
						}
						adjacentObstacleVerticesScratch[num10] = new int2((obstacleVertexGroup.type == ObstacleType.Loop) ? num11 : num10, adjacentObstacleVerticesScratch[num10].y);
						adjacentObstacleVerticesScratch[num11] = new int2(adjacentObstacleVerticesScratch[num11].x, (obstacleVertexGroup.type == ObstacleType.Loop) ? num10 : num11);
						for (int l = 0; l < obstacleVertexGroup.vertexCount; l++)
						{
							float3 p = *span[l + num8];
							int y = adjacentObstacleVerticesScratch[l + num10].y;
							movementPlaneWrapper = movementPlane;
							float2 float3 = movementPlaneWrapper.ToPlane(p) - @float;
							movementPlaneWrapper = movementPlane;
							float2 float4 = movementPlaneWrapper.ToPlane(*span[y]) - @float - float3;
							float rhs = JobRVO<MovementPlaneWrapper>.ClosestPointOnSegment(float3, float4 / math.lengthsq(float4), float2.zero, 0f, 1f);
							float num12 = math.lengthsq(float3 + float4 * rhs);
							segmentDistancesScratch[l + num10] = num12;
							if (num12 <= num7 && num9 < sortedVerticesScratch.Length)
							{
								sortedVerticesScratch[num9] = l + num10;
								num9++;
							}
						}
						num8 += obstacleVertexGroup.vertexCount;
					}
				}
				JobRVO<MovementPlaneWrapper>.InsertionSort<int, JobRVO<MovementPlaneWrapper>.SortByKey>(sortedVerticesScratch.AsUnsafeSpan<int>().Slice(0, num9), new JobRVO<MovementPlaneWrapper>.SortByKey
				{
					keys = segmentDistancesScratch.AsUnsafeSpan<float>().Slice(0, num8)
				});
				int num13 = 0;
				while (num13 < num9 && numLines < 50)
				{
					int num14 = sortedVerticesScratch[num13];
					if (segmentDistancesScratch[num14] > 0.25f * float2.x * float2.x)
					{
						break;
					}
					int x = adjacentObstacleVerticesScratch[num14].x;
					int y2 = adjacentObstacleVerticesScratch[num14].y;
					if (y2 != num14)
					{
						int y3 = adjacentObstacleVerticesScratch[y2].y;
						float3 p2 = *span[x];
						float3 p3 = *span[num14];
						float3 p4 = *span[y2];
						float3 p5 = *span[y3];
						movementPlaneWrapper = movementPlane;
						float2 float5 = movementPlaneWrapper.ToPlane(p2) - @float;
						movementPlaneWrapper = movementPlane;
						float x2;
						float2 float6 = movementPlaneWrapper.ToPlane(p3, out x2) - @float;
						movementPlaneWrapper = movementPlane;
						float y4;
						float2 float7 = movementPlaneWrapper.ToPlane(p4, out y4) - @float;
						movementPlaneWrapper = movementPlane;
						float2 lhs = movementPlaneWrapper.ToPlane(p5) - @float;
						if (math.max(x2, y4) + num2 >= num && math.min(x2, y4) <= num + num2)
						{
							float num15 = math.length(float7 - float6);
							if (num15 >= 0.0001f)
							{
								float2 float8 = (float7 - float6) * math.rcp(num15);
								if (JobRVO<MovementPlaneWrapper>.det(float8, -float6) <= num4)
								{
									bool flag = false;
									for (int m = 0; m < numLines; m++)
									{
										JobRVO<MovementPlaneWrapper>.ORCALine orcaline = orcaLines[m];
										if (JobRVO<MovementPlaneWrapper>.det(num5 * float6 - orcaline.point, orcaline.direction) - num5 * num4 >= -0.0001f && JobRVO<MovementPlaneWrapper>.det(num5 * float7 - orcaline.point, orcaline.direction) - num5 * num4 >= -0.0001f)
										{
											flag = true;
											break;
										}
									}
									if (!flag)
									{
										float2 zero = float2.zero;
										float num16 = math.dot(zero - float6, float8);
										float2 float9 = float6 + num16 * float8;
										float num17 = math.lengthsq(float9 - zero);
										float num18 = math.lengthsq(float6 + math.clamp(num16, 0f, num15) * float8);
										bool flag2 = JobRVO<MovementPlaneWrapper>.leftOrColinear(float6 - float5, float8);
										bool flag3 = JobRVO<MovementPlaneWrapper>.leftOrColinear(float8, lhs - float7);
										if (num18 < num4 * num4)
										{
											if (num16 < 0f)
											{
												if (flag2)
												{
													orcaLineToAgent[numLines] = -1;
													int num19 = numLines;
													numLines = num19 + 1;
													orcaLines[num19] = new JobRVO<MovementPlaneWrapper>.ORCALine
													{
														point = -float6 * 0.1f,
														direction = math.normalizesafe(JobRVO<MovementPlaneWrapper>.rot90(float6), default(float2))
													};
												}
											}
											else if (num16 > num15)
											{
												if (flag3 && JobRVO<MovementPlaneWrapper>.leftOrColinear(float7, lhs - float7))
												{
													orcaLineToAgent[numLines] = -1;
													int num19 = numLines;
													numLines = num19 + 1;
													orcaLines[num19] = new JobRVO<MovementPlaneWrapper>.ORCALine
													{
														point = -float7 * 0.1f,
														direction = math.normalizesafe(JobRVO<MovementPlaneWrapper>.rot90(float7), default(float2))
													};
												}
											}
											else
											{
												orcaLineToAgent[numLines] = -1;
												int num19 = numLines;
												numLines = num19 + 1;
												orcaLines[num19] = new JobRVO<MovementPlaneWrapper>.ORCALine
												{
													point = -float9 * 0.1f,
													direction = -float8
												};
											}
										}
										else
										{
											float2 float10;
											float2 float11;
											if ((num16 < 0f || num16 > 1f) && num17 <= num4 * num4)
											{
												if (num16 < 0f)
												{
													if (!flag2)
													{
														goto IL_D6A;
													}
													lhs = float7;
													float7 = float6;
													flag3 = flag2;
												}
												else
												{
													if (!flag3)
													{
														goto IL_D6A;
													}
													float5 = float6;
													float6 = float7;
													flag2 = flag3;
												}
												float num20 = math.lengthsq(float6);
												float rhs2 = math.sqrt(num20 - num4 * num4);
												float2 lhs2 = new float2(-float6.y, float6.x);
												float10 = (float6 * rhs2 + lhs2 * num4) / num20;
												float11 = (float6 * rhs2 - lhs2 * num4) / num20;
											}
											else
											{
												if (flag2)
												{
													float num21 = math.lengthsq(float6);
													float rhs3 = math.sqrt(num21 - num4 * num4);
													float2 lhs3 = new float2(-float6.y, float6.x);
													float10 = (float6 * rhs3 + lhs3 * num4) / num21;
												}
												else
												{
													float10 = -float8;
												}
												if (flag3)
												{
													float num22 = math.lengthsq(float7);
													float rhs4 = math.sqrt(num22 - num4 * num4);
													float2 lhs4 = new float2(-float7.y, float7.x);
													float11 = (float7 * rhs4 - lhs4 * num4) / num22;
												}
												else
												{
													float11 = float8;
												}
											}
											bool flag4 = false;
											bool flag5 = false;
											if (flag2 && JobRVO<MovementPlaneWrapper>.left(float10, float5 - float6))
											{
												float10 = float5 - float6;
												flag4 = true;
											}
											if (flag3 && JobRVO<MovementPlaneWrapper>.right(float11, lhs - float7))
											{
												float11 = lhs - float7;
												flag5 = true;
											}
											float2 float12 = num5 * float6;
											float2 float13 = num5 * float7;
											float2 float14 = float13 - float12;
											float num23 = math.lengthsq(float14);
											float num24 = (num23 <= 1E-05f) ? 0.5f : (math.dot(optimalVelocity - float12, float14) / num23);
											float num25 = math.dot(optimalVelocity - float12, float10);
											float num26 = math.dot(optimalVelocity - float13, float11);
											if ((num24 < 0f && num25 < 0f) || (num24 > 1f && num26 < 0f) || (num23 <= 1E-05f && num25 < 0f && num26 < 0f))
											{
												float2 float15 = (num24 <= 0.5f) ? float12 : float13;
												float2 float16 = math.normalizesafe(optimalVelocity - float15, default(float2));
												orcaLineToAgent[numLines] = -1;
												int num19 = numLines;
												numLines = num19 + 1;
												orcaLines[num19] = new JobRVO<MovementPlaneWrapper>.ORCALine
												{
													point = float15 + num4 * num5 * float16,
													direction = new float2(float16.y, -float16.x)
												};
											}
											else
											{
												float num27 = (num24 > 1f || num24 < 0f || num23 < 0.0001f) ? float.PositiveInfinity : math.lengthsq(optimalVelocity - (float12 + num24 * float14));
												float num28 = (num25 < 0f) ? float.PositiveInfinity : math.lengthsq(optimalVelocity - (float12 + num25 * float10));
												float num29 = (num26 < 0f) ? float.PositiveInfinity : math.lengthsq(optimalVelocity - (float13 + num26 * float11));
												int num30 = 0;
												float num31 = num27;
												if (num28 < num31)
												{
													num31 = num28;
													num30 = 1;
												}
												if (num29 < num31)
												{
													num30 = 2;
												}
												if (num30 == 0)
												{
													orcaLineToAgent[numLines] = -1;
													int num19 = numLines;
													numLines = num19 + 1;
													orcaLines[num19] = new JobRVO<MovementPlaneWrapper>.ORCALine
													{
														point = float12 + num4 * num5 * new float2(float8.y, -float8.x),
														direction = -float8
													};
												}
												else if (num30 == 1)
												{
													if (!flag4)
													{
														orcaLineToAgent[numLines] = -1;
														int num19 = numLines;
														numLines = num19 + 1;
														orcaLines[num19] = new JobRVO<MovementPlaneWrapper>.ORCALine
														{
															point = float12 + num4 * num5 * new float2(-float10.y, float10.x),
															direction = float10
														};
													}
												}
												else if (num30 == 2 && !flag5)
												{
													orcaLineToAgent[numLines] = -1;
													int num19 = numLines;
													numLines = num19 + 1;
													orcaLines[num19] = new JobRVO<MovementPlaneWrapper>.ORCALine
													{
														point = float13 + num4 * num5 * new float2(float11.y, -float11.x),
														direction = -float11
													};
												}
											}
										}
									}
								}
							}
						}
					}
					IL_D6A:
					num13++;
				}
			}
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x00063E3C File Offset: 0x0006203C
		public void ExecuteORCA(int startIndex, int batchSize)
		{
			int num = startIndex + batchSize;
			NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> nativeArray = new NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine>(100, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> scratchBuffer = new NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine>(100, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<float> segmentDistancesScratch = new NativeArray<float>(256, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<int> sortedVerticesScratch = new NativeArray<int>(256, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<int2> adjacentObstacleVerticesScratch = new NativeArray<int2>(1024, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<int> orcaLineToAgent = new NativeArray<int>(100, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeList<int> adjacentObstacleIdsScratch = new NativeList<int>(16, Allocator.Temp);
			for (int i = startIndex; i < num; i++)
			{
				if (this.agentData.version[i].Valid)
				{
					if (this.agentData.manuallyControlled[i])
					{
						this.output.speed[i] = this.agentData.desiredSpeed[i];
						this.output.targetPoint[i] = this.agentData.targetPoint[i];
						this.output.blockedByAgents[i * 7] = -1;
					}
					else
					{
						float3 @float = this.agentData.position[i];
						if (this.agentData.locked[i])
						{
							this.output.speed[i] = 0f;
							this.output.targetPoint[i] = @float;
							this.output.blockedByAgents[i * 7] = -1;
						}
						else
						{
							MovementPlaneWrapper movementPlane = default(MovementPlaneWrapper);
							movementPlane.Set(this.agentData.movementPlane[i]);
							float2 float2 = movementPlane.ToPlane(this.temporaryAgentData.currentVelocity[i]);
							int num2 = 0;
							this.GenerateObstacleVOs(i, adjacentObstacleIdsScratch, adjacentObstacleVerticesScratch, segmentDistancesScratch, sortedVerticesScratch, nativeArray, orcaLineToAgent, ref num2, movementPlane, float2);
							int num3 = num2;
							NativeSlice<int> neighbours = this.temporaryAgentData.neighbours.Slice(i * 50, 50);
							float num4 = this.agentData.agentTimeHorizon[i];
							float num5 = math.rcp(num4);
							float num6 = this.agentData.priority[i];
							float2 position = movementPlane.ToPlane(@float);
							float num7 = this.agentData.radius[i];
							float num8 = math.all(math.isfinite(this.agentData.endOfPath[i])) ? math.lengthsq(this.agentData.endOfPath[i] - @float) : float.PositiveInfinity;
							for (int j = 0; j < neighbours.Length; j++)
							{
								int num9 = neighbours[j];
								if (num9 == -1)
								{
									break;
								}
								float3 lhs = this.agentData.position[num9];
								float2 float3 = movementPlane.ToPlane(lhs - @float);
								float num10 = num7 + this.agentData.radius[num9];
								float num11 = this.agentData.priority[num9] * this.priorityMultiplier;
								float num12;
								if (this.agentData.locked[num9] || this.agentData.manuallyControlled[num9])
								{
									num12 = 1f;
								}
								else if (num11 > 1E-05f || num6 > 1E-05f)
								{
									num12 = num11 / (num6 + num11);
								}
								else
								{
									num12 = 0.5f;
								}
								float2 float4 = movementPlane.ToPlane(math.lerp(this.temporaryAgentData.currentVelocity[num9], this.temporaryAgentData.desiredVelocity[num9], math.clamp(2f * num12 - 1f, 0f, 1f)));
								if (this.agentData.flowFollowingStrength[num9] > 0f)
								{
									float num13 = this.agentData.flowFollowingStrength[num9] * this.agentData.flowFollowingStrength[i];
									float2 float5 = math.normalizesafe(float3, default(float2));
									float4 -= float5 * (num13 * math.min(0f, math.dot(float4, float5)));
								}
								float num14 = math.length(float3);
								float num15 = math.max(0f, num14 - num10);
								if (!this.agentData.locked[num9] || num15 * num15 <= num8)
								{
									float num16 = num15 / math.max(num10, this.agentData.desiredSpeed[i] + this.agentData.desiredSpeed[num9]);
									float num17 = math.clamp((num16 * num5 - 0.5f) * 2f, 0f, 1f);
									num10 *= 1f - num17;
									float invTimeHorizon = 1f / math.max(0.1f * num4, num4 * math.clamp(math.sqrt(2f * num16), 0f, 1f));
									nativeArray[num2] = new JobRVO<MovementPlaneWrapper>.ORCALine(position, float3, float2, float4, num10, 0.1f, invTimeHorizon);
									orcaLineToAgent[num2] = num9;
									num2++;
								}
							}
							float2 float6 = math.normalizesafe(movementPlane.ToPlane(this.agentData.collisionNormal[i]), default(float2));
							if (math.any(float6 != 0f))
							{
								nativeArray[num2] = new JobRVO<MovementPlaneWrapper>.ORCALine
								{
									point = float2.zero,
									direction = new float2(float6.y, -float6.x)
								};
								orcaLineToAgent[num2] = -1;
								num2++;
							}
							float2 float7 = movementPlane.ToPlane(this.temporaryAgentData.desiredVelocity[i]);
							float2 p = this.temporaryAgentData.desiredTargetPointInVelocitySpace[i];
							float maxBiasRadians = this.symmetryBreakingBias * (1f - this.agentData.flowFollowingStrength[i]);
							if (!JobRVO<MovementPlaneWrapper>.BiasDesiredVelocity(nativeArray.AsUnsafeSpan<JobRVO<MovementPlaneWrapper>.ORCALine>().Slice(num3, num2 - num3), ref float7, ref p, maxBiasRadians) && JobRVO<MovementPlaneWrapper>.DistanceInsideVOs(nativeArray.AsUnsafeSpan<JobRVO<MovementPlaneWrapper>.ORCALine>().Slice(0, num2), float7) <= 0f && math.all(math.abs(this.temporaryAgentData.collisionVelocityOffsets[i]) < 0.001f))
							{
								this.output.targetPoint[i] = @float + movementPlane.ToWorld(p, 0f);
								this.output.speed[i] = this.agentData.desiredSpeed[i];
								this.output.blockedByAgents[i * 7] = -1;
								this.output.forwardClearance[i] = float.PositiveInfinity;
							}
							else
							{
								float num18 = this.agentData.maxSpeed[i];
								float2 float8 = this.agentData.allowedVelocityDeviationAngles[i];
								JobRVO<MovementPlaneWrapper>.LinearProgram2Output linearProgram2Output;
								if (math.all(float8 == 0f))
								{
									linearProgram2Output = JobRVO<MovementPlaneWrapper>.LinearProgram2D(nativeArray, num2, num18, float7, false);
								}
								else
								{
									float2 rhs;
									float2 rhs2;
									math.sincos(float8, out rhs, out rhs2);
									float2 float9 = float7.x * rhs2 - float7.y * rhs;
									float2 float10 = float7.x * rhs + float7.y * rhs2;
									float2 float11 = new float2(float9.x, float10.x);
									float2 float12 = new float2(float9.y, float10.y);
									float2 float13 = float7 - float11;
									float num19 = math.length(float13);
									float13 = math.select(float2.zero, float13 * math.rcp(num19), num19 > 1.1754944E-38f);
									float2 float14 = float7 - float12;
									float num20 = math.length(float14);
									float14 = math.select(float2.zero, float14 * math.rcp(num20), num20 > 1.1754944E-38f);
									JobRVO<MovementPlaneWrapper>.LinearProgram2Output linearProgram2Output2 = JobRVO<MovementPlaneWrapper>.LinearProgram2DSegment(nativeArray, num2, num18, float11, float13, 0f, num19, 1f);
									JobRVO<MovementPlaneWrapper>.LinearProgram2Output linearProgram2Output3 = JobRVO<MovementPlaneWrapper>.LinearProgram2DSegment(nativeArray, num2, num18, float12, float14, 0f, num20, 1f);
									if (linearProgram2Output2.firstFailedLineIndex < linearProgram2Output3.firstFailedLineIndex)
									{
										linearProgram2Output = linearProgram2Output2;
									}
									else if (linearProgram2Output3.firstFailedLineIndex < linearProgram2Output2.firstFailedLineIndex)
									{
										linearProgram2Output = linearProgram2Output3;
									}
									else
									{
										linearProgram2Output = ((math.lengthsq(linearProgram2Output2.velocity - float7) < math.lengthsq(linearProgram2Output3.velocity - float7)) ? linearProgram2Output2 : linearProgram2Output3);
									}
								}
								float2 float15;
								if (linearProgram2Output.firstFailedLineIndex < num2)
								{
									float15 = linearProgram2Output.velocity;
									JobRVO<MovementPlaneWrapper>.LinearProgram3D(nativeArray, num2, num3, linearProgram2Output.firstFailedLineIndex, num18, ref float15, scratchBuffer);
								}
								else
								{
									float15 = linearProgram2Output.velocity;
								}
								int num21 = 0;
								int num22 = 0;
								while (num22 < num2 && num21 < 7)
								{
									if (orcaLineToAgent[num22] != -1 && JobRVO<MovementPlaneWrapper>.det(nativeArray[num22].direction, nativeArray[num22].point - float15) >= -0.001f)
									{
										this.output.blockedByAgents[i * 7 + num21] = orcaLineToAgent[num22];
										num21++;
									}
									num22++;
								}
								if (num21 < 7)
								{
									this.output.blockedByAgents[i * 7 + num21] = -1;
								}
								if (math.any(this.temporaryAgentData.collisionVelocityOffsets[i] != 0f))
								{
									float15 += this.temporaryAgentData.collisionVelocityOffsets[i];
									float15 = JobRVO<MovementPlaneWrapper>.LinearProgram2D(nativeArray, num3, num18, float15, false).velocity;
								}
								this.output.targetPoint[i] = @float + movementPlane.ToWorld(float15, 0f);
								this.output.speed[i] = math.min(math.length(float15), num18);
								float2 float16 = math.normalizesafe(movementPlane.ToPlane(this.agentData.targetPoint[i] - @float), default(float2));
								float num23 = this.CalculateForwardClearance(neighbours, movementPlane, @float, num7, float16);
								this.output.forwardClearance[i] = num23;
								if (this.agentData.HasDebugFlag(i, AgentDebugFlags.ForwardClearance) && num23 < float.PositiveInfinity)
								{
									this.draw.PushLineWidth(2f, true);
									this.draw.Ray(@float, movementPlane.ToWorld(float16, 0f) * num23, Color.red);
									this.draw.PopLineWidth();
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0006495C File Offset: 0x00062B5C
		private float CalculateForwardClearance(NativeSlice<int> neighbours, MovementPlaneWrapper movementPlane, float3 position, float radius, float2 targetDir)
		{
			float num = float.PositiveInfinity;
			for (int i = 0; i < neighbours.Length; i++)
			{
				int num2 = neighbours[i];
				if (num2 == -1)
				{
					break;
				}
				float3 lhs = this.agentData.position[num2];
				float num3 = radius + this.agentData.radius[num2];
				float2 x = movementPlane.ToPlane(lhs - position);
				float num4 = math.dot(math.normalizesafe(x, default(float2)), targetDir);
				if (num4 >= 0f)
				{
					float num5 = math.lengthsq(x);
					float num6 = math.sqrt(num5) * num4;
					float num7 = num3 * num3 - (num5 - num6 * num6);
					if (num7 >= 0f)
					{
						float y = num6 - math.sqrt(num7);
						num = math.min(num, y);
					}
				}
			}
			return num;
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x00064A3C File Offset: 0x00062C3C
		private static bool leftOrColinear(float2 vector1, float2 vector2)
		{
			return JobRVO<MovementPlaneWrapper>.det(vector1, vector2) >= 0f;
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x00064A4F File Offset: 0x00062C4F
		private static bool left(float2 vector1, float2 vector2)
		{
			return JobRVO<MovementPlaneWrapper>.det(vector1, vector2) > 0f;
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00064A5F File Offset: 0x00062C5F
		private static bool rightOrColinear(float2 vector1, float2 vector2)
		{
			return JobRVO<MovementPlaneWrapper>.det(vector1, vector2) <= 0f;
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x00064A72 File Offset: 0x00062C72
		private static bool right(float2 vector1, float2 vector2)
		{
			return JobRVO<MovementPlaneWrapper>.det(vector1, vector2) < 0f;
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0000C307 File Offset: 0x0000A507
		private static float det(float2 vector1, float2 vector2)
		{
			return vector1.x * vector2.y - vector1.y * vector2.x;
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x00064A82 File Offset: 0x00062C82
		private static float2 rot90(float2 v)
		{
			return new float2(-v.y, v.x);
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x00064A98 File Offset: 0x00062C98
		private static float DistanceInsideVOs(UnsafeSpan<JobRVO<MovementPlaneWrapper>.ORCALine> lines, float2 velocity)
		{
			float num = 0f;
			for (int i = 0; i < lines.Length; i++)
			{
				float y = JobRVO<MovementPlaneWrapper>.det(lines[i].direction, lines[i].point - velocity);
				num = math.max(num, y);
			}
			return num;
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x00064AEC File Offset: 0x00062CEC
		private static bool BiasDesiredVelocity(UnsafeSpan<JobRVO<MovementPlaneWrapper>.ORCALine> lines, ref float2 desiredVelocity, ref float2 targetPointInVelocitySpace, float maxBiasRadians)
		{
			float num = JobRVO<MovementPlaneWrapper>.DistanceInsideVOs(lines, desiredVelocity);
			if (num == 0f)
			{
				return false;
			}
			float num2 = math.length(desiredVelocity);
			if (num2 >= 0.001f)
			{
				float rhs = math.min(maxBiasRadians, num / num2);
				desiredVelocity += new float2(desiredVelocity.y, -desiredVelocity.x) * rhs;
				targetPointInVelocitySpace += new float2(targetPointInVelocitySpace.y, -targetPointInVelocitySpace.x) * rhs;
			}
			return true;
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00064B84 File Offset: 0x00062D84
		private static bool ClipLine(JobRVO<MovementPlaneWrapper>.ORCALine line, JobRVO<MovementPlaneWrapper>.ORCALine clipper, ref float tLeft, ref float tRight)
		{
			float num = JobRVO<MovementPlaneWrapper>.det(line.direction, clipper.direction);
			float num2 = JobRVO<MovementPlaneWrapper>.det(clipper.direction, line.point - clipper.point);
			if (math.abs(num) < 0.0001f)
			{
				return false;
			}
			float y = num2 / num;
			if (num >= 0f)
			{
				tRight = math.min(tRight, y);
			}
			else
			{
				tLeft = math.max(tLeft, y);
			}
			return true;
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00064BF4 File Offset: 0x00062DF4
		private static bool ClipBoundary(NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> lines, int lineIndex, float radius, out float tLeft, out float tRight)
		{
			JobRVO<MovementPlaneWrapper>.ORCALine orcaline = lines[lineIndex];
			if (!VectorMath.LineCircleIntersectionFactors(orcaline.point, orcaline.direction, radius, out tLeft, out tRight))
			{
				return false;
			}
			for (int i = 0; i < lineIndex; i++)
			{
				float num = JobRVO<MovementPlaneWrapper>.det(orcaline.direction, lines[i].direction);
				float num2 = JobRVO<MovementPlaneWrapper>.det(lines[i].direction, orcaline.point - lines[i].point);
				if (math.abs(num) < 0.0001f)
				{
					if (num2 < 0f)
					{
						return false;
					}
				}
				else
				{
					float y = num2 / num;
					if (num >= 0f)
					{
						tRight = math.min(tRight, y);
					}
					else
					{
						tLeft = math.max(tLeft, y);
					}
					if (tLeft > tRight)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x00064CC4 File Offset: 0x00062EC4
		private static bool LinearProgram1D(NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> lines, int lineIndex, float radius, float2 optimalVelocity, bool directionOpt, ref float2 result)
		{
			float num;
			float num2;
			if (!JobRVO<MovementPlaneWrapper>.ClipBoundary(lines, lineIndex, radius, out num, out num2))
			{
				return false;
			}
			JobRVO<MovementPlaneWrapper>.ORCALine orcaline = lines[lineIndex];
			if (directionOpt)
			{
				if (math.dot(optimalVelocity, orcaline.direction) > 0f)
				{
					result = orcaline.point + num2 * orcaline.direction;
				}
				else
				{
					result = orcaline.point + num * orcaline.direction;
				}
			}
			else
			{
				float valueToClamp = math.dot(orcaline.direction, optimalVelocity - orcaline.point);
				result = orcaline.point + math.clamp(valueToClamp, num, num2) * orcaline.direction;
			}
			return true;
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x00064D80 File Offset: 0x00062F80
		private static JobRVO<MovementPlaneWrapper>.LinearProgram2Output LinearProgram2D(NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> lines, int numLines, float radius, float2 optimalVelocity, bool directionOpt)
		{
			float2 @float;
			if (directionOpt)
			{
				@float = optimalVelocity * radius;
			}
			else if (math.lengthsq(optimalVelocity) > radius * radius)
			{
				@float = math.normalize(optimalVelocity) * radius;
			}
			else
			{
				@float = optimalVelocity;
			}
			for (int i = 0; i < numLines; i++)
			{
				if (JobRVO<MovementPlaneWrapper>.det(lines[i].direction, lines[i].point - @float) > 0f)
				{
					float2 velocity = @float;
					if (!JobRVO<MovementPlaneWrapper>.LinearProgram1D(lines, i, radius, optimalVelocity, directionOpt, ref @float))
					{
						return new JobRVO<MovementPlaneWrapper>.LinearProgram2Output
						{
							velocity = velocity,
							firstFailedLineIndex = i
						};
					}
				}
			}
			return new JobRVO<MovementPlaneWrapper>.LinearProgram2Output
			{
				velocity = @float,
				firstFailedLineIndex = numLines
			};
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x00064E33 File Offset: 0x00063033
		private static float ClosestPointOnSegment(float2 a, float2 dir, float2 p, float t0, float t1)
		{
			return math.clamp(math.dot(p - a, dir), t0, t1);
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x00064E4C File Offset: 0x0006304C
		private static float2 ClosestSegmentSegmentPointNonIntersecting(JobRVO<MovementPlaneWrapper>.ORCALine a, JobRVO<MovementPlaneWrapper>.ORCALine b, float ta1, float ta2, float tb1, float tb2)
		{
			float2 @float = a.point + a.direction * ta1;
			float2 float2 = a.point + a.direction * ta2;
			float2 float3 = b.point + b.direction * tb1;
			float2 float4 = b.point + b.direction * tb2;
			float rhs = JobRVO<MovementPlaneWrapper>.ClosestPointOnSegment(a.point, a.direction, float3, ta1, ta2);
			float rhs2 = JobRVO<MovementPlaneWrapper>.ClosestPointOnSegment(a.point, a.direction, float4, ta1, ta2);
			float rhs3 = JobRVO<MovementPlaneWrapper>.ClosestPointOnSegment(b.point, b.direction, @float, tb1, tb2);
			float rhs4 = JobRVO<MovementPlaneWrapper>.ClosestPointOnSegment(b.point, b.direction, float2, tb1, tb2);
			float2 float5 = a.point + a.direction * rhs;
			float2 float6 = a.point + a.direction * rhs2;
			float2 lhs = b.point + b.direction * rhs3;
			float2 lhs2 = b.point + b.direction * rhs4;
			float num = math.lengthsq(float5 - float3);
			float num2 = math.lengthsq(float6 - float4);
			float num3 = math.lengthsq(lhs - @float);
			float num4 = math.lengthsq(lhs2 - float2);
			float2 result = float5;
			float num5 = num;
			if (num2 < num5)
			{
				result = float6;
				num5 = num2;
			}
			if (num3 < num5)
			{
				result = @float;
				num5 = num3;
			}
			if (num4 < num5)
			{
				result = float2;
			}
			return result;
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x00064FEC File Offset: 0x000631EC
		private static JobRVO<MovementPlaneWrapper>.LinearProgram2Output LinearProgram2DCollapsedSegment(NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> lines, int numLines, int startLine, float radius, float2 currentResult, float2 optimalVelocityStart, float2 optimalVelocityDir, float optimalTLeft, float optimalTRight)
		{
			for (int i = startLine; i < numLines; i++)
			{
				if (JobRVO<MovementPlaneWrapper>.det(lines[i].direction, lines[i].point - currentResult) > 0f)
				{
					float ta;
					float ta2;
					if (!JobRVO<MovementPlaneWrapper>.ClipBoundary(lines, i, radius, out ta, out ta2))
					{
						return new JobRVO<MovementPlaneWrapper>.LinearProgram2Output
						{
							velocity = currentResult,
							firstFailedLineIndex = i
						};
					}
					currentResult = JobRVO<MovementPlaneWrapper>.ClosestSegmentSegmentPointNonIntersecting(lines[i], new JobRVO<MovementPlaneWrapper>.ORCALine
					{
						point = optimalVelocityStart,
						direction = optimalVelocityDir
					}, ta, ta2, optimalTLeft, optimalTRight);
				}
			}
			return new JobRVO<MovementPlaneWrapper>.LinearProgram2Output
			{
				velocity = currentResult,
				firstFailedLineIndex = numLines
			};
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x000650AC File Offset: 0x000632AC
		private static JobRVO<MovementPlaneWrapper>.LinearProgram2Output LinearProgram2DSegment(NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> lines, int numLines, float radius, float2 optimalVelocityStart, float2 optimalVelocityDir, float optimalTLeft, float optimalTRight, float optimalT)
		{
			float num;
			float num2;
			bool flag = VectorMath.LineCircleIntersectionFactors(optimalVelocityStart, optimalVelocityDir, radius, out num, out num2);
			num = math.max(num, optimalTLeft);
			num2 = math.min(num2, optimalTRight);
			if (!(flag & num <= num2))
			{
				float rhs = math.clamp(math.dot(-optimalVelocityStart, optimalVelocityDir), optimalTLeft, optimalTRight);
				float2 currentResult = math.normalizesafe(optimalVelocityStart + optimalVelocityDir * rhs, default(float2)) * radius;
				return JobRVO<MovementPlaneWrapper>.LinearProgram2DCollapsedSegment(lines, numLines, 0, radius, currentResult, optimalVelocityStart, optimalVelocityDir, optimalTLeft, optimalTRight);
			}
			for (int i = 0; i < numLines; i++)
			{
				JobRVO<MovementPlaneWrapper>.ORCALine orcaline = lines[i];
				bool flag2 = JobRVO<MovementPlaneWrapper>.det(orcaline.direction, orcaline.point - (optimalVelocityStart + optimalVelocityDir * num)) > 0f;
				bool flag3 = JobRVO<MovementPlaneWrapper>.det(orcaline.direction, orcaline.point - (optimalVelocityStart + optimalVelocityDir * num2)) > 0f;
				if (flag2 || flag3)
				{
					float num3;
					float num4;
					if (!JobRVO<MovementPlaneWrapper>.ClipBoundary(lines, i, radius, out num3, out num4))
					{
						return new JobRVO<MovementPlaneWrapper>.LinearProgram2Output
						{
							velocity = optimalVelocityStart + optimalVelocityDir * math.clamp(optimalT, num, num2),
							firstFailedLineIndex = i
						};
					}
					if (flag2 && flag3)
					{
						if (math.abs(JobRVO<MovementPlaneWrapper>.det(orcaline.direction, optimalVelocityDir)) >= 0.001f)
						{
							float2 currentResult2 = JobRVO<MovementPlaneWrapper>.ClosestSegmentSegmentPointNonIntersecting(orcaline, new JobRVO<MovementPlaneWrapper>.ORCALine
							{
								point = optimalVelocityStart,
								direction = optimalVelocityDir
							}, num3, num4, optimalTLeft, optimalTRight);
							return JobRVO<MovementPlaneWrapper>.LinearProgram2DCollapsedSegment(lines, numLines, i + 1, radius, currentResult2, optimalVelocityStart, optimalVelocityDir, optimalTLeft, optimalTRight);
						}
						float num5 = JobRVO<MovementPlaneWrapper>.ClosestPointOnSegment(orcaline.point, orcaline.direction, optimalVelocityStart + optimalVelocityDir * num, num3, num4);
						float num6 = JobRVO<MovementPlaneWrapper>.ClosestPointOnSegment(orcaline.point, orcaline.direction, optimalVelocityStart + optimalVelocityDir * num2, num3, num4);
						float num7 = JobRVO<MovementPlaneWrapper>.ClosestPointOnSegment(orcaline.point, orcaline.direction, optimalVelocityStart + optimalVelocityDir * optimalT, num3, num4);
						optimalVelocityStart = orcaline.point;
						optimalVelocityDir = orcaline.direction;
						num = num5;
						num2 = num6;
						optimalT = num7;
					}
					else
					{
						JobRVO<MovementPlaneWrapper>.ClipLine(new JobRVO<MovementPlaneWrapper>.ORCALine
						{
							point = optimalVelocityStart,
							direction = optimalVelocityDir
						}, orcaline, ref num, ref num2);
					}
				}
			}
			float rhs2 = math.clamp(optimalT, num, num2);
			return new JobRVO<MovementPlaneWrapper>.LinearProgram2Output
			{
				velocity = optimalVelocityStart + optimalVelocityDir * rhs2,
				firstFailedLineIndex = numLines
			};
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0006534C File Offset: 0x0006354C
		private static void LinearProgram3D(NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> lines, int numLines, int numFixedLines, int beginLine, float radius, ref float2 result, NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> scratchBuffer)
		{
			float num = 0f;
			NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine> nativeArray = scratchBuffer;
			NativeArray<JobRVO<MovementPlaneWrapper>.ORCALine>.Copy(lines, nativeArray, numFixedLines);
			for (int i = beginLine; i < numLines; i++)
			{
				if (JobRVO<MovementPlaneWrapper>.det(lines[i].direction, lines[i].point - result) > num)
				{
					int num2 = numFixedLines;
					for (int j = numFixedLines; j < i; j++)
					{
						float num3 = JobRVO<MovementPlaneWrapper>.det(lines[i].direction, lines[j].direction);
						if (math.abs(num3) < 0.001f)
						{
							if (math.dot(lines[i].direction, lines[j].direction) <= 0f)
							{
								nativeArray[num2] = new JobRVO<MovementPlaneWrapper>.ORCALine
								{
									point = 0.5f * (lines[i].point + lines[j].point),
									direction = math.normalize(lines[j].direction - lines[i].direction)
								};
								num2++;
							}
						}
						else
						{
							nativeArray[num2] = new JobRVO<MovementPlaneWrapper>.ORCALine
							{
								point = lines[i].point + JobRVO<MovementPlaneWrapper>.det(lines[j].direction, lines[i].point - lines[j].point) / num3 * lines[i].direction,
								direction = math.normalize(lines[j].direction - lines[i].direction)
							};
							num2++;
						}
					}
					JobRVO<MovementPlaneWrapper>.LinearProgram2Output linearProgram2Output = JobRVO<MovementPlaneWrapper>.LinearProgram2D(nativeArray, num2, radius, new float2(-lines[i].direction.y, lines[i].direction.x), true);
					if (linearProgram2Output.firstFailedLineIndex >= num2)
					{
						result = linearProgram2Output.velocity;
					}
					num = JobRVO<MovementPlaneWrapper>.det(lines[i].direction, lines[i].point - result);
				}
			}
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x000035CE File Offset: 0x000017CE
		private static void DrawVO(CommandBuilder draw, float2 circleCenter, float radius, float2 origin, Color color)
		{
		}

		// Token: 0x04000BF1 RID: 3057
		[ReadOnly]
		public SimulatorBurst.AgentData agentData;

		// Token: 0x04000BF2 RID: 3058
		[ReadOnly]
		public SimulatorBurst.TemporaryAgentData temporaryAgentData;

		// Token: 0x04000BF3 RID: 3059
		[ReadOnly]
		public NavmeshEdges.NavmeshBorderData navmeshEdgeData;

		// Token: 0x04000BF4 RID: 3060
		[WriteOnly]
		public SimulatorBurst.AgentOutputData output;

		// Token: 0x04000BF5 RID: 3061
		public float deltaTime;

		// Token: 0x04000BF6 RID: 3062
		public float symmetryBreakingBias;

		// Token: 0x04000BF7 RID: 3063
		public float priorityMultiplier;

		// Token: 0x04000BF8 RID: 3064
		public bool useNavmeshAsObstacle;

		// Token: 0x04000BF9 RID: 3065
		private const int MaxObstacleCount = 50;

		// Token: 0x04000BFA RID: 3066
		public CommandBuilder draw;

		// Token: 0x04000BFB RID: 3067
		private static readonly ProfilerMarker MarkerConvertObstacles1 = new ProfilerMarker("RVOConvertObstacles1");

		// Token: 0x04000BFC RID: 3068
		private static readonly ProfilerMarker MarkerConvertObstacles2 = new ProfilerMarker("RVOConvertObstacles2");

		// Token: 0x020002A6 RID: 678
		private struct SortByKey : IComparer<int>
		{
			// Token: 0x06001020 RID: 4128 RVA: 0x000655D7 File Offset: 0x000637D7
			public unsafe int Compare(int x, int y)
			{
				return this.keys[x].CompareTo(*this.keys[y]);
			}

			// Token: 0x04000BFD RID: 3069
			public UnsafeSpan<float> keys;
		}

		// Token: 0x020002A7 RID: 679
		private struct ORCALine
		{
			// Token: 0x06001021 RID: 4129 RVA: 0x000655F8 File Offset: 0x000637F8
			public void DrawAsHalfPlane(CommandBuilder draw, float halfPlaneLength, float halfPlaneWidth, Color color)
			{
				float2 lhs = new float2(this.direction.y, -this.direction.x);
				draw.xy.Line(this.point - this.direction * 10f, this.point + this.direction * 10f, color);
				float2 xy = this.point + lhs * halfPlaneWidth * 0.5f;
				draw.SolidBox(new float3(xy, 0f), quaternion.RotateZ(math.atan2(this.direction.y, this.direction.x)), new float3(halfPlaneLength, halfPlaneWidth, 0.01f), new Color(0f, 0f, 0f, 0.5f));
			}

			// Token: 0x06001022 RID: 4130 RVA: 0x000656E0 File Offset: 0x000638E0
			public ORCALine(float2 position, float2 relativePosition, float2 velocity, float2 otherVelocity, float combinedRadius, float timeStep, float invTimeHorizon)
			{
				float2 @float = velocity - otherVelocity;
				float num = combinedRadius * combinedRadius;
				float num2 = math.lengthsq(relativePosition);
				if (num2 <= num)
				{
					float rhs = math.rcp(timeStep);
					float num3 = math.sqrt(num2);
					float2 float2 = math.select(0, relativePosition / num3, num3 > 1.1754944E-38f) * (num3 - combinedRadius - 0.001f) * 0.3f * rhs;
					this.direction = math.normalizesafe(new float2(float2.y, -float2.x), default(float2));
					this.point = math.lerp(velocity, otherVelocity, 0.5f) + float2 * 0.5f;
					return;
				}
				combinedRadius *= 1.001f;
				float2 float3 = @float - invTimeHorizon * relativePosition;
				float num4 = math.lengthsq(float3);
				float num5 = math.dot(float3, relativePosition);
				if (num5 < 0f && num5 * num5 > num * num4)
				{
					float num6 = math.sqrt(num4);
					float2 float4 = float3 / num6;
					this.direction = new float2(float4.y, -float4.x);
					float2 rhs2 = (combinedRadius * invTimeHorizon - num6) * float4;
					this.point = velocity + 0.5f * rhs2;
					return;
				}
				float rhs3 = math.sqrt(num2 - num);
				if (JobRVO<MovementPlaneWrapper>.det(relativePosition, float3) > 0f)
				{
					this.direction = (relativePosition * rhs3 + new float2(-relativePosition.y, relativePosition.x) * combinedRadius) / num2;
				}
				else
				{
					this.direction = (-relativePosition * rhs3 + new float2(-relativePosition.y, relativePosition.x) * combinedRadius) / num2;
				}
				float2 rhs4 = math.dot(@float, this.direction) * this.direction - @float;
				this.point = velocity + 0.5f * rhs4;
			}

			// Token: 0x04000BFE RID: 3070
			public float2 point;

			// Token: 0x04000BFF RID: 3071
			public float2 direction;
		}

		// Token: 0x020002A8 RID: 680
		private struct LinearProgram2Output
		{
			// Token: 0x04000C00 RID: 3072
			public float2 velocity;

			// Token: 0x04000C01 RID: 3073
			public int firstFailedLineIndex;
		}
	}
}
