using System;
using Pathfinding.Drawing;
using Pathfinding.ECS.RVO;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002C1 RID: 705
	public struct RVOQuadtreeBurst
	{
		// Token: 0x060010C6 RID: 4294 RVA: 0x00067E08 File Offset: 0x00066008
		static RVOQuadtreeBurst()
		{
			for (int i = 0; i < 256; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if ((i >> j & 1) != 0)
					{
						RVOQuadtreeBurst.ChildLookup[i] = (byte)j;
						break;
					}
				}
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060010C7 RID: 4295 RVA: 0x00067E58 File Offset: 0x00066058
		public Rect bounds
		{
			get
			{
				if (!this.boundingBoxBuffer.IsCreated)
				{
					return default(Rect);
				}
				return Rect.MinMaxRect(this.boundingBoxBuffer[0].x, this.boundingBoxBuffer[0].y, this.boundingBoxBuffer[1].x, this.boundingBoxBuffer[1].y);
			}
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x00067EC5 File Offset: 0x000660C5
		private static int InnerNodeCountUpperBound(int numAgents, MovementPlane movementPlane)
		{
			return (((movementPlane == MovementPlane.Arbitrary) ? 8 : 4) * 10 * numAgents + 16 - 1) / 16;
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x00067EDC File Offset: 0x000660DC
		public void Dispose()
		{
			this.agents.Dispose();
			this.childPointers.Dispose();
			this.boundingBoxBuffer.Dispose();
			this.agentCountBuffer.Dispose();
			this.maxSpeeds.Dispose();
			this.maxRadius.Dispose();
			this.nodeAreas.Dispose();
			this.agentPositions.Dispose();
			this.agentRadii.Dispose();
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x00067F4C File Offset: 0x0006614C
		private void Reserve(int minSize)
		{
			if (!this.boundingBoxBuffer.IsCreated)
			{
				this.boundingBoxBuffer = new NativeArray<float3>(4, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.agentCountBuffer = new NativeArray<int>(1, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}
			int num = math.ceilpow2(minSize);
			Memory.Realloc<int>(ref this.agents, num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			Memory.Realloc<float3>(ref this.agentPositions, num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			Memory.Realloc<float>(ref this.agentRadii, num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			Memory.Realloc<int>(ref this.childPointers, RVOQuadtreeBurst.InnerNodeCountUpperBound(num, this.movementPlane), Allocator.Persistent, NativeArrayOptions.ClearMemory);
			Memory.Realloc<float>(ref this.maxSpeeds, this.childPointers.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			Memory.Realloc<float>(ref this.nodeAreas, this.childPointers.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			Memory.Realloc<float>(ref this.maxRadius, this.childPointers.Length, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x00068014 File Offset: 0x00066214
		public RVOQuadtreeBurst.JobBuild BuildJob(NativeArray<float3> agentPositions, NativeArray<AgentIndex> agentVersions, NativeArray<float> agentSpeeds, NativeArray<float> agentRadii, int numAgents, MovementPlane movementPlane)
		{
			if (numAgents >= 32767)
			{
				throw new Exception("Too many agents. Cannot have more than " + 32767.ToString());
			}
			this.Reserve(numAgents);
			this.movementPlane = movementPlane;
			return new RVOQuadtreeBurst.JobBuild
			{
				agents = this.agents,
				agentVersions = agentVersions,
				agentPositions = agentPositions,
				agentSpeeds = agentSpeeds,
				agentRadii = agentRadii,
				outMaxSpeeds = this.maxSpeeds,
				outMaxRadius = this.maxRadius,
				outArea = this.nodeAreas,
				outAgentRadii = this.agentRadii,
				outAgentPositions = this.agentPositions,
				outBoundingBox = this.boundingBoxBuffer,
				outAgentCount = this.agentCountBuffer,
				outChildPointers = this.childPointers,
				numAgents = numAgents,
				movementPlane = movementPlane
			};
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x00068108 File Offset: 0x00066308
		public int QueryKNearest(RVOQuadtreeBurst.QuadtreeQuery query)
		{
			if (!this.agents.IsCreated)
			{
				return 0;
			}
			float num = 1E+30f;
			for (int i = 0; i < query.maxCount; i++)
			{
				query.result[query.outputStartIndex + i] = -1;
			}
			for (int j = 0; j < query.maxCount; j++)
			{
				query.resultDistances[j] = 1E+30f;
			}
			this.QueryRec(ref query, 0, this.boundingBoxBuffer[0], this.boundingBoxBuffer[1], ref num);
			int num2 = 0;
			while (num2 < query.maxCount && query.resultDistances[num2] < 1E+30f)
			{
				num2++;
			}
			return num2;
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x000681BC File Offset: 0x000663BC
		private void QueryRec(ref RVOQuadtreeBurst.QuadtreeQuery query, int treeNodeIndex, float3 nodeMin, float3 nodeMax, ref float maxRadius)
		{
			float num = math.min(math.max((this.maxSpeeds[treeNodeIndex] + query.speed) * query.timeHorizon, query.agentRadius) + query.agentRadius, maxRadius);
			float3 position = query.position;
			if ((this.childPointers[treeNodeIndex] & 1073741824) != 0)
			{
				int maxCount = query.maxCount;
				int num2 = this.childPointers[treeNodeIndex] & 32767;
				int num3 = this.childPointers[treeNodeIndex] >> 15 & 32767;
				NativeArray<int> result = query.result;
				NativeArray<float> resultDistances = query.resultDistances;
				for (int i = num2; i < num3; i++)
				{
					int num4 = this.agents[i];
					float num5 = math.lengthsq(position - this.agentPositions[num4]);
					if (num5 < num * num && (query.layers[num4] & query.layerMask) != (RVOLayer)0)
					{
						int j = 0;
						while (j < maxCount)
						{
							if (num5 < resultDistances[j])
							{
								for (int k = maxCount - 1; k > j; k--)
								{
									result[query.outputStartIndex + k] = result[query.outputStartIndex + k - 1];
									resultDistances[k] = resultDistances[k - 1];
								}
								result[query.outputStartIndex + j] = num4;
								resultDistances[j] = num5;
								if (j == maxCount - 1)
								{
									maxRadius = math.min(maxRadius, math.sqrt(num5));
									num = math.min(num, maxRadius);
									break;
								}
								break;
							}
							else
							{
								j++;
							}
						}
					}
				}
				return;
			}
			int num6 = this.childPointers[treeNodeIndex];
			float3 @float = (nodeMin + nodeMax) * 0.5f;
			if (this.movementPlane == MovementPlane.Arbitrary)
			{
				int num7 = ((position.x < @float.x) ? 0 : 4) | ((position.y < @float.y) ? 0 : 2) | ((position.z < @float.z) ? 0 : 1);
				bool3 test = new bool3((num7 & 4) != 0, (num7 & 2) != 0, (num7 & 1) != 0);
				float3 nodeMin2 = math.select(nodeMin, @float, test);
				float3 nodeMax2 = math.select(@float, nodeMax, test);
				this.QueryRec(ref query, num6 + num7, nodeMin2, nodeMax2, ref maxRadius);
				num = math.min(num, maxRadius);
				bool3 test2 = position - num < @float;
				bool3 test3 = position + num > @float;
				int3 lhs = math.select(new int3(240, 204, 170), new int3(255, 255, 255), test2);
				int3 rhs = math.select(new int3(15, 51, 85), new int3(255, 255, 255), test3);
				int3 @int = lhs & rhs;
				int num8 = @int.x & @int.y & @int.z;
				byte b;
				for (num8 &= ~(1 << num7); num8 != 0; num8 &= ~(1 << (int)b))
				{
					b = RVOQuadtreeBurst.ChildLookup[num8];
					bool3 test4 = new bool3((b & 4) > 0, (b & 2) > 0, (b & 1) > 0);
					float3 nodeMin3 = math.select(nodeMin, @float, test4);
					float3 nodeMax3 = math.select(@float, nodeMax, test4);
					this.QueryRec(ref query, num6 + (int)b, nodeMin3, nodeMax3, ref maxRadius);
					num = math.min(num, maxRadius);
				}
				return;
			}
			if (this.movementPlane == MovementPlane.XY)
			{
				int num9 = ((position.x < @float.x) ? 0 : 2) | ((position.y < @float.y) ? 0 : 1);
				bool3 test5 = new bool3((num9 & 2) != 0, (num9 & 1) != 0, false);
				float3 nodeMin4 = math.select(nodeMin, @float, test5);
				float3 nodeMax4 = math.select(@float, nodeMax, test5);
				this.QueryRec(ref query, num6 + num9, nodeMin4, nodeMax4, ref maxRadius);
				num = math.min(num, maxRadius);
				bool2 @bool = position.xy - num < @float.xy;
				bool2 bool2 = position.xy + num > @float.xy;
				bool4 bool3 = new bool4(@bool.x & @bool.y, @bool.x & bool2.y, bool2.x & @bool.y, bool2.x & bool2.y);
				int num10 = (bool3.x ? 1 : 0) | (bool3.y ? 2 : 0) | (bool3.z ? 4 : 0) | (bool3.w ? 8 : 0);
				byte b2;
				for (num10 &= ~(1 << num9); num10 != 0; num10 &= ~(1 << (int)b2))
				{
					b2 = RVOQuadtreeBurst.ChildLookup[num10];
					bool3 test6 = new bool3((b2 & 2) > 0, (b2 & 1) > 0, false);
					float3 nodeMin5 = math.select(nodeMin, @float, test6);
					float3 nodeMax5 = math.select(@float, nodeMax, test6);
					this.QueryRec(ref query, num6 + (int)b2, nodeMin5, nodeMax5, ref maxRadius);
					num = math.min(num, maxRadius);
				}
				return;
			}
			int num11 = ((position.x < @float.x) ? 0 : 2) | ((position.z < @float.z) ? 0 : 1);
			bool3 test7 = new bool3((num11 & 2) != 0, false, (num11 & 1) != 0);
			float3 nodeMin6 = math.select(nodeMin, @float, test7);
			float3 nodeMax6 = math.select(@float, nodeMax, test7);
			this.QueryRec(ref query, num6 + num11, nodeMin6, nodeMax6, ref maxRadius);
			num = math.min(num, maxRadius);
			bool2 bool4 = position.xz - num < @float.xz;
			bool2 bool5 = position.xz + num > @float.xz;
			bool4 bool6 = new bool4(bool4.x & bool4.y, bool4.x & bool5.y, bool5.x & bool4.y, bool5.x & bool5.y);
			int num12 = (bool6.x ? 1 : 0) | (bool6.y ? 2 : 0) | (bool6.z ? 4 : 0) | (bool6.w ? 8 : 0);
			byte b3;
			for (num12 &= ~(1 << num11); num12 != 0; num12 &= ~(1 << (int)b3))
			{
				b3 = RVOQuadtreeBurst.ChildLookup[num12];
				bool3 test8 = new bool3((b3 & 2) > 0, false, (b3 & 1) > 0);
				float3 nodeMin7 = math.select(nodeMin, @float, test8);
				float3 nodeMax7 = math.select(@float, nodeMax, test8);
				this.QueryRec(ref query, num6 + (int)b3, nodeMin7, nodeMax7, ref maxRadius);
				num = math.min(num, maxRadius);
			}
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x00068884 File Offset: 0x00066A84
		public float QueryArea(float3 position, float radius)
		{
			if (!this.agents.IsCreated || this.agentCountBuffer[0] == 0)
			{
				return 0f;
			}
			return 3.1415927f * this.QueryAreaRec(0, position, radius, this.boundingBoxBuffer[0], this.boundingBoxBuffer[1]);
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x000688DC File Offset: 0x00066ADC
		private float QueryAreaRec(int treeNodeIndex, float3 p, float radius, float3 nodeMin, float3 nodeMax)
		{
			float3 @float = (nodeMin + nodeMax) * 0.5f;
			float num = math.length(nodeMax - @float);
			float num2 = math.lengthsq(@float - p);
			float num3 = this.maxRadius[treeNodeIndex];
			float num4 = radius - (num + num3);
			if (num4 > 0f && num2 < num4 * num4)
			{
				return this.nodeAreas[treeNodeIndex];
			}
			if (num2 > (radius + (num + num3)) * (radius + (num + num3)))
			{
				return 0f;
			}
			if ((this.childPointers[treeNodeIndex] & 1073741824) != 0)
			{
				int num5 = this.childPointers[treeNodeIndex] & 32767;
				int num6 = this.childPointers[treeNodeIndex] >> 15 & 32767;
				float num7 = 0f;
				float num8 = 0f;
				for (int i = num5; i < num6; i++)
				{
					int index = this.agents[i];
					num7 += this.agentRadii[index] * this.agentRadii[index];
					float num9 = math.lengthsq(p - this.agentPositions[index]);
					float num10 = this.agentRadii[index];
					if (num9 < (radius + num10) * (radius + num10))
					{
						float num11 = radius - num10;
						float num12 = (num9 < num11 * num11) ? 1f : (1f - (math.sqrt(num9) - num11) / (2f * num10));
						num8 += num10 * num10 * num12;
					}
				}
				return num8;
			}
			float num13 = 0f;
			int num14 = this.childPointers[treeNodeIndex];
			float rhs = radius + num3;
			if (this.movementPlane == MovementPlane.Arbitrary)
			{
				bool3 @bool = p - rhs < @float;
				bool3 bool2 = p + rhs > @float;
				if (@bool[0])
				{
					if (@bool[1])
					{
						if (@bool[2])
						{
							num13 += this.QueryAreaRec(num14, p, radius, new float3(nodeMin.x, nodeMin.y, nodeMin.z), new float3(@float.x, @float.y, @float.z));
						}
						if (bool2[2])
						{
							num13 += this.QueryAreaRec(num14 + 1, p, radius, new float3(nodeMin.x, nodeMin.y, @float.z), new float3(@float.x, @float.y, nodeMax.z));
						}
					}
					if (bool2[1])
					{
						if (@bool[2])
						{
							num13 += this.QueryAreaRec(num14 + 2, p, radius, new float3(nodeMin.x, @float.y, nodeMin.z), new float3(@float.x, nodeMax.y, @float.z));
						}
						if (bool2[2])
						{
							num13 += this.QueryAreaRec(num14 + 3, p, radius, new float3(nodeMin.x, @float.y, @float.z), new float3(@float.x, nodeMax.y, nodeMax.z));
						}
					}
				}
				if (bool2[0])
				{
					if (@bool[1])
					{
						if (@bool[2])
						{
							num13 += this.QueryAreaRec(num14 + 4, p, radius, new float3(@float.x, nodeMin.y, nodeMin.z), new float3(nodeMax.x, @float.y, @float.z));
						}
						if (bool2[2])
						{
							num13 += this.QueryAreaRec(num14 + 5, p, radius, new float3(@float.x, nodeMin.y, @float.z), new float3(nodeMax.x, @float.y, nodeMax.z));
						}
					}
					if (bool2[1])
					{
						if (@bool[2])
						{
							num13 += this.QueryAreaRec(num14 + 6, p, radius, new float3(@float.x, @float.y, nodeMin.z), new float3(nodeMax.x, nodeMax.y, @float.z));
						}
						if (bool2[2])
						{
							num13 += this.QueryAreaRec(num14 + 7, p, radius, new float3(@float.x, @float.y, @float.z), new float3(nodeMax.x, nodeMax.y, nodeMax.z));
						}
					}
				}
			}
			else if (this.movementPlane == MovementPlane.XY)
			{
				bool2 bool3 = (p - rhs).xy < @float.xy;
				bool2 bool4 = (p + rhs).xy > @float.xy;
				if (bool3[0])
				{
					if (bool3[1])
					{
						num13 += this.QueryAreaRec(num14, p, radius, new float3(nodeMin.x, nodeMin.y, nodeMin.z), new float3(@float.x, @float.y, nodeMax.z));
					}
					if (bool4[1])
					{
						num13 += this.QueryAreaRec(num14 + 1, p, radius, new float3(nodeMin.x, @float.y, nodeMin.z), new float3(@float.x, nodeMax.y, nodeMax.z));
					}
				}
				if (bool4[0])
				{
					if (bool3[1])
					{
						num13 += this.QueryAreaRec(num14 + 2, p, radius, new float3(@float.x, nodeMin.y, nodeMin.z), new float3(nodeMax.x, @float.y, nodeMax.z));
					}
					if (bool4[1])
					{
						num13 += this.QueryAreaRec(num14 + 3, p, radius, new float3(@float.x, @float.y, nodeMin.z), new float3(nodeMax.x, nodeMax.y, nodeMax.z));
					}
				}
			}
			else
			{
				bool2 bool5 = (p - rhs).xz < @float.xz;
				bool2 bool6 = (p + rhs).xz > @float.xz;
				if (bool5[0])
				{
					if (bool5[1])
					{
						num13 += this.QueryAreaRec(num14, p, radius, new float3(nodeMin.x, nodeMin.y, nodeMin.z), new float3(@float.x, nodeMax.y, @float.z));
					}
					if (bool6[1])
					{
						num13 += this.QueryAreaRec(num14 + 1, p, radius, new float3(nodeMin.x, nodeMin.y, @float.z), new float3(@float.x, nodeMax.y, nodeMax.z));
					}
				}
				if (bool6[0])
				{
					if (bool5[1])
					{
						num13 += this.QueryAreaRec(num14 + 2, p, radius, new float3(@float.x, nodeMin.y, nodeMin.z), new float3(nodeMax.x, nodeMax.y, @float.z));
					}
					if (bool6[1])
					{
						num13 += this.QueryAreaRec(num14 + 3, p, radius, new float3(@float.x, nodeMin.y, @float.z), new float3(nodeMax.x, nodeMax.y, nodeMax.z));
					}
				}
			}
			return num13;
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x0006908C File Offset: 0x0006728C
		public void DebugDraw(CommandBuilder draw)
		{
			if (!this.agentCountBuffer.IsCreated)
			{
				return;
			}
			int num = this.agentCountBuffer[0];
			if (num == 0)
			{
				return;
			}
			this.DebugDraw(0, this.boundingBoxBuffer[0], this.boundingBoxBuffer[1], draw);
			for (int i = 0; i < num; i++)
			{
				draw.Cross(this.agentPositions[this.agents[i]], 0.5f, Palette.Colorbrewer.Set1.Red);
			}
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x0006910C File Offset: 0x0006730C
		private void DebugDraw(int nodeIndex, float3 nodeMin, float3 nodeMax, CommandBuilder draw)
		{
			float3 @float = (nodeMin + nodeMax) * 0.5f;
			draw.WireBox(@float, nodeMax - nodeMin, Palette.Colorbrewer.Set1.Orange);
			if ((this.childPointers[nodeIndex] & 1073741824) != 0)
			{
				int num = this.childPointers[nodeIndex] & 32767;
				int num2 = this.childPointers[nodeIndex] >> 15 & 32767;
				for (int i = num; i < num2; i++)
				{
					draw.Line(@float, this.agentPositions[this.agents[i]], Color.black);
				}
				return;
			}
			int num3 = this.childPointers[nodeIndex];
			if (this.movementPlane == MovementPlane.Arbitrary)
			{
				this.DebugDraw(num3, new float3(nodeMin.x, nodeMin.y, nodeMin.z), new float3(@float.x, @float.y, @float.z), draw);
				this.DebugDraw(num3 + 1, new float3(nodeMin.x, nodeMin.y, @float.z), new float3(@float.x, @float.y, nodeMax.z), draw);
				this.DebugDraw(num3 + 2, new float3(nodeMin.x, @float.y, nodeMin.z), new float3(@float.x, nodeMax.y, @float.z), draw);
				this.DebugDraw(num3 + 3, new float3(nodeMin.x, @float.y, @float.z), new float3(@float.x, nodeMax.y, nodeMax.z), draw);
				this.DebugDraw(num3 + 4, new float3(@float.x, nodeMin.y, nodeMin.z), new float3(nodeMax.x, @float.y, @float.z), draw);
				this.DebugDraw(num3 + 5, new float3(@float.x, nodeMin.y, @float.z), new float3(nodeMax.x, @float.y, nodeMax.z), draw);
				this.DebugDraw(num3 + 6, new float3(@float.x, @float.y, nodeMin.z), new float3(nodeMax.x, nodeMax.y, @float.z), draw);
				this.DebugDraw(num3 + 7, new float3(@float.x, @float.y, @float.z), new float3(nodeMax.x, nodeMax.y, nodeMax.z), draw);
				return;
			}
			if (this.movementPlane == MovementPlane.XY)
			{
				this.DebugDraw(num3, new float3(nodeMin.x, nodeMin.y, nodeMin.z), new float3(@float.x, @float.y, nodeMax.z), draw);
				this.DebugDraw(num3 + 1, new float3(nodeMin.x, @float.y, nodeMin.z), new float3(@float.x, nodeMax.y, nodeMax.z), draw);
				this.DebugDraw(num3 + 2, new float3(@float.x, nodeMin.y, nodeMin.z), new float3(nodeMax.x, @float.y, nodeMax.z), draw);
				this.DebugDraw(num3 + 3, new float3(@float.x, @float.y, nodeMin.z), new float3(nodeMax.x, nodeMax.y, nodeMax.z), draw);
				return;
			}
			this.DebugDraw(num3, new float3(nodeMin.x, nodeMin.y, nodeMin.z), new float3(@float.x, nodeMax.y, @float.z), draw);
			this.DebugDraw(num3 + 1, new float3(nodeMin.x, nodeMin.y, @float.z), new float3(@float.x, nodeMax.y, nodeMax.z), draw);
			this.DebugDraw(num3 + 2, new float3(@float.x, nodeMin.y, nodeMin.z), new float3(nodeMax.x, nodeMax.y, @float.z), draw);
			this.DebugDraw(num3 + 3, new float3(@float.x, nodeMin.y, @float.z), new float3(nodeMax.x, nodeMax.y, nodeMax.z), draw);
		}

		// Token: 0x04000C83 RID: 3203
		private const int LeafSize = 16;

		// Token: 0x04000C84 RID: 3204
		private const int MaxDepth = 10;

		// Token: 0x04000C85 RID: 3205
		private NativeArray<int> agents;

		// Token: 0x04000C86 RID: 3206
		private NativeArray<int> childPointers;

		// Token: 0x04000C87 RID: 3207
		private NativeArray<float3> boundingBoxBuffer;

		// Token: 0x04000C88 RID: 3208
		private NativeArray<int> agentCountBuffer;

		// Token: 0x04000C89 RID: 3209
		private NativeArray<float3> agentPositions;

		// Token: 0x04000C8A RID: 3210
		private NativeArray<float> agentRadii;

		// Token: 0x04000C8B RID: 3211
		private NativeArray<float> maxSpeeds;

		// Token: 0x04000C8C RID: 3212
		private NativeArray<float> maxRadius;

		// Token: 0x04000C8D RID: 3213
		private NativeArray<float> nodeAreas;

		// Token: 0x04000C8E RID: 3214
		private MovementPlane movementPlane;

		// Token: 0x04000C8F RID: 3215
		private const int LeafNodeBit = 1073741824;

		// Token: 0x04000C90 RID: 3216
		private const int BitPackingShift = 15;

		// Token: 0x04000C91 RID: 3217
		private const int BitPackingMask = 32767;

		// Token: 0x04000C92 RID: 3218
		private const int MaxAgents = 32767;

		// Token: 0x04000C93 RID: 3219
		private static readonly byte[] ChildLookup = new byte[256];

		// Token: 0x04000C94 RID: 3220
		private const float DistanceInfinity = 1E+30f;

		// Token: 0x020002C2 RID: 706
		[BurstCompile(CompileSynchronously = true, FloatMode = FloatMode.Fast)]
		public struct JobBuild : IJob
		{
			// Token: 0x060010D2 RID: 4306 RVA: 0x0006955C File Offset: 0x0006775C
			private static int Partition(NativeSlice<int> indices, int startIndex, int endIndex, NativeSlice<float> coordinates, float splitPoint)
			{
				for (int i = startIndex; i < endIndex; i++)
				{
					if (coordinates[indices[i]] > splitPoint)
					{
						endIndex--;
						int value = indices[i];
						indices[i] = indices[endIndex];
						indices[endIndex] = value;
						i--;
					}
				}
				return endIndex;
			}

			// Token: 0x060010D3 RID: 4307 RVA: 0x000695B4 File Offset: 0x000677B4
			private void BuildNode(float3 boundsMin, float3 boundsMax, int depth, int agentsStart, int agentsEnd, int nodeOffset, ref int firstFreeChild)
			{
				if (agentsEnd - agentsStart <= 16 || depth >= 10)
				{
					this.outChildPointers[nodeOffset] = (agentsStart | agentsEnd << 15 | 1073741824);
					return;
				}
				if (this.movementPlane == MovementPlane.Arbitrary)
				{
					NativeSlice<float> coordinates = new NativeSlice<float3>(this.agentPositions).SliceWithStride<float>(0);
					NativeSlice<float> coordinates2 = new NativeSlice<float3>(this.agentPositions).SliceWithStride<float>(4);
					NativeSlice<float> coordinates3 = new NativeSlice<float3>(this.agentPositions).SliceWithStride<float>(8);
					float3 @float = (boundsMin + boundsMax) * 0.5f;
					int num = RVOQuadtreeBurst.JobBuild.Partition(this.agents, agentsStart, agentsEnd, coordinates, @float.x);
					int num2 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, agentsStart, num, coordinates2, @float.y);
					int num3 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, num, agentsEnd, coordinates2, @float.y);
					int num4 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, agentsStart, num2, coordinates3, @float.z);
					int num5 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, num2, num, coordinates3, @float.z);
					int num6 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, num, num3, coordinates3, @float.z);
					int num7 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, num3, agentsEnd, coordinates3, @float.z);
					int num8 = firstFreeChild;
					this.outChildPointers[nodeOffset] = num8;
					firstFreeChild += 8;
					float3 float2 = @float;
					this.BuildNode(new float3(boundsMin.x, boundsMin.y, boundsMin.z), new float3(float2.x, float2.y, float2.z), depth + 1, agentsStart, num4, num8, ref firstFreeChild);
					this.BuildNode(new float3(boundsMin.x, boundsMin.y, float2.z), new float3(float2.x, float2.y, boundsMax.z), depth + 1, num4, num2, num8 + 1, ref firstFreeChild);
					this.BuildNode(new float3(boundsMin.x, float2.y, boundsMin.z), new float3(float2.x, boundsMax.y, float2.z), depth + 1, num2, num5, num8 + 2, ref firstFreeChild);
					this.BuildNode(new float3(boundsMin.x, float2.y, float2.z), new float3(float2.x, boundsMax.y, boundsMax.z), depth + 1, num5, num, num8 + 3, ref firstFreeChild);
					this.BuildNode(new float3(float2.x, boundsMin.y, boundsMin.z), new float3(boundsMax.x, float2.y, float2.z), depth + 1, num, num6, num8 + 4, ref firstFreeChild);
					this.BuildNode(new float3(float2.x, boundsMin.y, float2.z), new float3(boundsMax.x, float2.y, boundsMax.z), depth + 1, num6, num3, num8 + 5, ref firstFreeChild);
					this.BuildNode(new float3(float2.x, float2.y, boundsMin.z), new float3(boundsMax.x, boundsMax.y, float2.z), depth + 1, num3, num7, num8 + 6, ref firstFreeChild);
					this.BuildNode(new float3(float2.x, float2.y, float2.z), new float3(boundsMax.x, boundsMax.y, boundsMax.z), depth + 1, num7, agentsEnd, num8 + 7, ref firstFreeChild);
					return;
				}
				if (this.movementPlane == MovementPlane.XY)
				{
					NativeSlice<float> coordinates4 = new NativeSlice<float3>(this.agentPositions).SliceWithStride<float>(0);
					NativeSlice<float> coordinates5 = new NativeSlice<float3>(this.agentPositions).SliceWithStride<float>(4);
					float3 float3 = (boundsMin + boundsMax) * 0.5f;
					int num9 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, agentsStart, agentsEnd, coordinates4, float3.x);
					int num10 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, agentsStart, num9, coordinates5, float3.y);
					int num11 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, num9, agentsEnd, coordinates5, float3.y);
					int num12 = firstFreeChild;
					this.outChildPointers[nodeOffset] = num12;
					firstFreeChild += 4;
					this.BuildNode(new float3(boundsMin.x, boundsMin.y, boundsMin.z), new float3(float3.x, float3.y, boundsMax.z), depth + 1, agentsStart, num10, num12, ref firstFreeChild);
					this.BuildNode(new float3(boundsMin.x, float3.y, boundsMin.z), new float3(float3.x, boundsMax.y, boundsMax.z), depth + 1, num10, num9, num12 + 1, ref firstFreeChild);
					this.BuildNode(new float3(float3.x, boundsMin.y, boundsMin.z), new float3(boundsMax.x, float3.y, boundsMax.z), depth + 1, num9, num11, num12 + 2, ref firstFreeChild);
					this.BuildNode(new float3(float3.x, float3.y, boundsMin.z), new float3(boundsMax.x, boundsMax.y, boundsMax.z), depth + 1, num11, agentsEnd, num12 + 3, ref firstFreeChild);
					return;
				}
				NativeSlice<float> coordinates6 = new NativeSlice<float3>(this.agentPositions).SliceWithStride<float>(0);
				NativeSlice<float> coordinates7 = new NativeSlice<float3>(this.agentPositions).SliceWithStride<float>(8);
				float3 float4 = (boundsMin + boundsMax) * 0.5f;
				int num13 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, agentsStart, agentsEnd, coordinates6, float4.x);
				int num14 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, agentsStart, num13, coordinates7, float4.z);
				int num15 = RVOQuadtreeBurst.JobBuild.Partition(this.agents, num13, agentsEnd, coordinates7, float4.z);
				int num16 = firstFreeChild;
				this.outChildPointers[nodeOffset] = num16;
				firstFreeChild += 4;
				this.BuildNode(new float3(boundsMin.x, boundsMin.y, boundsMin.z), new float3(float4.x, boundsMax.y, float4.z), depth + 1, agentsStart, num14, num16, ref firstFreeChild);
				this.BuildNode(new float3(boundsMin.x, boundsMin.y, float4.z), new float3(float4.x, boundsMax.y, boundsMax.z), depth + 1, num14, num13, num16 + 1, ref firstFreeChild);
				this.BuildNode(new float3(float4.x, boundsMin.y, boundsMin.z), new float3(boundsMax.x, boundsMax.y, float4.z), depth + 1, num13, num15, num16 + 2, ref firstFreeChild);
				this.BuildNode(new float3(float4.x, boundsMin.y, float4.z), new float3(boundsMax.x, boundsMax.y, boundsMax.z), depth + 1, num15, agentsEnd, num16 + 3, ref firstFreeChild);
			}

			// Token: 0x060010D4 RID: 4308 RVA: 0x00069D28 File Offset: 0x00067F28
			private void CalculateSpeeds(int nodeCount)
			{
				for (int i = nodeCount - 1; i >= 0; i--)
				{
					if ((this.outChildPointers[i] & 1073741824) != 0)
					{
						int num = this.outChildPointers[i] & 32767;
						int num2 = this.outChildPointers[i] >> 15 & 32767;
						float num3 = 0f;
						for (int j = num; j < num2; j++)
						{
							num3 = math.max(num3, this.agentSpeeds[this.agents[j]]);
						}
						this.outMaxSpeeds[i] = num3;
						float num4 = 0f;
						for (int k = num; k < num2; k++)
						{
							num4 = math.max(num4, this.agentRadii[this.agents[k]]);
						}
						this.outMaxRadius[i] = num4;
						float num5 = 0f;
						for (int l = num; l < num2; l++)
						{
							num5 += this.agentRadii[this.agents[l]] * this.agentRadii[this.agents[l]];
						}
						this.outArea[i] = num5;
					}
					else
					{
						int num6 = this.outChildPointers[i];
						if (this.movementPlane == MovementPlane.Arbitrary)
						{
							float num7 = 0f;
							float num8 = 0f;
							float num9 = 0f;
							for (int m = 0; m < 8; m++)
							{
								num7 = math.max(num7, this.outMaxSpeeds[num6 + m]);
								num8 = math.max(num8, this.outMaxSpeeds[num6 + m]);
								num9 += this.outArea[num6 + m];
							}
							this.outMaxSpeeds[i] = num7;
							this.outMaxRadius[i] = num8;
							this.outArea[i] = num9;
						}
						else
						{
							this.outMaxSpeeds[i] = math.max(math.max(this.outMaxSpeeds[num6], this.outMaxSpeeds[num6 + 1]), math.max(this.outMaxSpeeds[num6 + 2], this.outMaxSpeeds[num6 + 3]));
							this.outMaxRadius[i] = math.max(math.max(this.outMaxRadius[num6], this.outMaxRadius[num6 + 1]), math.max(this.outMaxRadius[num6 + 2], this.outMaxRadius[num6 + 3]));
							this.outArea[i] = this.outArea[num6] + this.outArea[num6 + 1] + this.outArea[num6 + 2] + this.outArea[num6 + 3];
						}
					}
				}
			}

			// Token: 0x060010D5 RID: 4309 RVA: 0x0006A01C File Offset: 0x0006821C
			public void Execute()
			{
				float3 @float = float.PositiveInfinity;
				float3 float2 = float.NegativeInfinity;
				int num = 0;
				for (int i = 0; i < this.numAgents; i++)
				{
					if (this.agentVersions[i].Valid)
					{
						this.agents[num++] = i;
						@float = math.min(@float, this.agentPositions[i]);
						float2 = math.max(float2, this.agentPositions[i]);
					}
				}
				this.outAgentCount[0] = num;
				if (num == 0)
				{
					this.outBoundingBox[0] = (this.outBoundingBox[1] = float3.zero);
					return;
				}
				this.outBoundingBox[0] = @float;
				this.outBoundingBox[1] = float2;
				int nodeCount = 1;
				this.BuildNode(@float, float2, 0, 0, num, 0, ref nodeCount);
				this.CalculateSpeeds(nodeCount);
				NativeArray<float3>.Copy(this.agentPositions, this.outAgentPositions, this.numAgents);
				NativeArray<float>.Copy(this.agentRadii, this.outAgentRadii, this.numAgents);
			}

			// Token: 0x04000C95 RID: 3221
			public NativeArray<int> agents;

			// Token: 0x04000C96 RID: 3222
			[ReadOnly]
			public NativeArray<float3> agentPositions;

			// Token: 0x04000C97 RID: 3223
			[ReadOnly]
			public NativeArray<AgentIndex> agentVersions;

			// Token: 0x04000C98 RID: 3224
			[ReadOnly]
			public NativeArray<float> agentSpeeds;

			// Token: 0x04000C99 RID: 3225
			[ReadOnly]
			public NativeArray<float> agentRadii;

			// Token: 0x04000C9A RID: 3226
			[WriteOnly]
			public NativeArray<float3> outBoundingBox;

			// Token: 0x04000C9B RID: 3227
			[WriteOnly]
			public NativeArray<int> outAgentCount;

			// Token: 0x04000C9C RID: 3228
			public NativeArray<int> outChildPointers;

			// Token: 0x04000C9D RID: 3229
			public NativeArray<float> outMaxSpeeds;

			// Token: 0x04000C9E RID: 3230
			public NativeArray<float> outMaxRadius;

			// Token: 0x04000C9F RID: 3231
			public NativeArray<float> outArea;

			// Token: 0x04000CA0 RID: 3232
			[WriteOnly]
			public NativeArray<float3> outAgentPositions;

			// Token: 0x04000CA1 RID: 3233
			[WriteOnly]
			public NativeArray<float> outAgentRadii;

			// Token: 0x04000CA2 RID: 3234
			public int numAgents;

			// Token: 0x04000CA3 RID: 3235
			public MovementPlane movementPlane;
		}

		// Token: 0x020002C3 RID: 707
		public struct QuadtreeQuery
		{
			// Token: 0x04000CA4 RID: 3236
			public float3 position;

			// Token: 0x04000CA5 RID: 3237
			public float speed;

			// Token: 0x04000CA6 RID: 3238
			public float timeHorizon;

			// Token: 0x04000CA7 RID: 3239
			public float agentRadius;

			// Token: 0x04000CA8 RID: 3240
			public int outputStartIndex;

			// Token: 0x04000CA9 RID: 3241
			public int maxCount;

			// Token: 0x04000CAA RID: 3242
			public RVOLayer layerMask;

			// Token: 0x04000CAB RID: 3243
			public NativeArray<RVOLayer> layers;

			// Token: 0x04000CAC RID: 3244
			public NativeArray<int> result;

			// Token: 0x04000CAD RID: 3245
			public NativeArray<float> resultDistances;
		}

		// Token: 0x020002C4 RID: 708
		[BurstCompile]
		public struct DebugDrawJob : IJob
		{
			// Token: 0x060010D6 RID: 4310 RVA: 0x0006A13A File Offset: 0x0006833A
			public void Execute()
			{
				this.quadtree.DebugDraw(this.draw);
			}

			// Token: 0x04000CAE RID: 3246
			public CommandBuilder draw;

			// Token: 0x04000CAF RID: 3247
			[ReadOnly]
			public RVOQuadtreeBurst quadtree;
		}
	}
}
