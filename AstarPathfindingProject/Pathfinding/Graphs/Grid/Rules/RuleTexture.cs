using System;
using Pathfinding.Jobs;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Rules
{
	// Token: 0x0200020D RID: 525
	[Preserve]
	public class RuleTexture : GridGraphRule
	{
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x00050E3A File Offset: 0x0004F03A
		public override int Hash
		{
			get
			{
				return base.Hash ^ ((this.texture != null) ? (31 * this.texture.GetInstanceID() ^ (int)this.texture.updateCount) : 0);
			}
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00050E70 File Offset: 0x0004F070
		public override void Register(GridGraphRules rules)
		{
			if (this.texture == null)
			{
				return;
			}
			if (!this.texture.isReadable)
			{
				Debug.LogError("Texture for the texture rule on a grid graph is not marked as readable.", this.texture);
				return;
			}
			if (this.colors.IsCreated)
			{
				this.colors.Dispose();
			}
			this.colors = new NativeArray<Color32>(this.texture.GetPixels32(), Allocator.Persistent).Reinterpret<int>();
			int2 textureSize = new int2(this.texture.width, this.texture.height);
			float4 channelPenaltiesCombined = float4.zero;
			bool4 channelDeterminesWalkability = false;
			float4 channelPositionScalesCombined = float4.zero;
			for (int i = 0; i < 4; i++)
			{
				channelPenaltiesCombined[i] = ((this.channels[i] == RuleTexture.ChannelUse.Penalty || this.channels[i] == RuleTexture.ChannelUse.WalkablePenalty) ? this.channelScales[i] : 0f);
				channelDeterminesWalkability[i] = (this.channels[i] == RuleTexture.ChannelUse.Walkable || this.channels[i] == RuleTexture.ChannelUse.WalkablePenalty);
				channelPositionScalesCombined[i] = ((this.channels[i] == RuleTexture.ChannelUse.Position) ? this.channelScales[i] : 0f);
			}
			channelPositionScalesCombined /= 255f;
			channelPenaltiesCombined /= 255f;
			if (math.any(channelPositionScalesCombined))
			{
				rules.AddJobSystemPass(GridGraphRule.Pass.BeforeCollision, delegate(GridGraphRules.Context context)
				{
					new RuleTexture.JobTexturePosition
					{
						colorData = this.colors,
						nodePositions = context.data.nodes.positions,
						nodeNormals = context.data.nodes.normals,
						bounds = context.data.nodes.bounds,
						colorDataSize = textureSize,
						scale = ((this.scalingMode == RuleTexture.ScalingMode.FixedScale) ? (1f / math.max(0.01f, this.nodesPerPixel)) : (textureSize / new float2((float)context.graph.width, (float)context.graph.depth))),
						channelPositionScale = channelPositionScalesCombined,
						graphToWorld = context.data.transform.matrix
					}.Schedule(context.tracker);
				});
			}
			rules.AddJobSystemPass(GridGraphRule.Pass.BeforeConnections, delegate(GridGraphRules.Context context)
			{
				new RuleTexture.JobTexturePenalty
				{
					colorData = this.colors,
					penalty = context.data.nodes.penalties,
					walkable = context.data.nodes.walkable,
					nodeNormals = context.data.nodes.normals,
					bounds = context.data.nodes.bounds,
					colorDataSize = textureSize,
					scale = ((this.scalingMode == RuleTexture.ScalingMode.FixedScale) ? (1f / math.max(0.01f, this.nodesPerPixel)) : (textureSize / new float2((float)context.graph.width, (float)context.graph.depth))),
					channelPenalties = channelPenaltiesCombined,
					channelDeterminesWalkability = channelDeterminesWalkability
				}.Schedule(context.tracker);
			});
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0005101E File Offset: 0x0004F21E
		public override void DisposeUnmanagedData()
		{
			if (this.colors.IsCreated)
			{
				this.colors.Dispose();
			}
		}

		// Token: 0x04000989 RID: 2441
		public Texture2D texture;

		// Token: 0x0400098A RID: 2442
		public RuleTexture.ChannelUse[] channels = new RuleTexture.ChannelUse[4];

		// Token: 0x0400098B RID: 2443
		public float[] channelScales = new float[]
		{
			1000f,
			1000f,
			1000f,
			1000f
		};

		// Token: 0x0400098C RID: 2444
		public RuleTexture.ScalingMode scalingMode = RuleTexture.ScalingMode.StretchToFitGraph;

		// Token: 0x0400098D RID: 2445
		public float nodesPerPixel = 1f;

		// Token: 0x0400098E RID: 2446
		private NativeArray<int> colors;

		// Token: 0x0200020E RID: 526
		public enum ScalingMode
		{
			// Token: 0x04000990 RID: 2448
			FixedScale,
			// Token: 0x04000991 RID: 2449
			StretchToFitGraph
		}

		// Token: 0x0200020F RID: 527
		public enum ChannelUse
		{
			// Token: 0x04000993 RID: 2451
			None,
			// Token: 0x04000994 RID: 2452
			Penalty,
			// Token: 0x04000995 RID: 2453
			Position,
			// Token: 0x04000996 RID: 2454
			WalkablePenalty,
			// Token: 0x04000997 RID: 2455
			Walkable
		}

		// Token: 0x02000210 RID: 528
		[BurstCompile]
		public struct JobTexturePosition : IJob, GridIterationUtilities.INodeModifier
		{
			// Token: 0x06000CF4 RID: 3316 RVA: 0x00051078 File Offset: 0x0004F278
			public void ModifyNode(int dataIndex, int dataX, int dataLayer, int dataZ)
			{
				int2 xz = this.bounds.min.xz;
				int2 @int = math.clamp((int2)math.round((new float2((float)dataX, (float)dataZ) + xz) * this.scale), int2.zero, this.colorDataSize - new int2(1, 1));
				int index = @int.y * this.colorDataSize.x + @int.x;
				int4 v = new int4(this.colorData[index] & 255, this.colorData[index] >> 8 & 255, this.colorData[index] >> 16 & 255, this.colorData[index] >> 24 & 255);
				float y = math.dot(this.channelPositionScale, v);
				this.nodePositions[dataIndex] = this.graphToWorld.MultiplyPoint3x4(new Vector3((float)(this.bounds.min.x + dataX) + 0.5f, y, (float)(this.bounds.min.z + dataZ) + 0.5f));
			}

			// Token: 0x06000CF5 RID: 3317 RVA: 0x000511B2 File Offset: 0x0004F3B2
			public void Execute()
			{
				GridIterationUtilities.ForEachNode<RuleTexture.JobTexturePosition>(this.bounds.size, this.nodeNormals, ref this);
			}

			// Token: 0x04000998 RID: 2456
			[ReadOnly]
			public NativeArray<int> colorData;

			// Token: 0x04000999 RID: 2457
			[WriteOnly]
			public NativeArray<Vector3> nodePositions;

			// Token: 0x0400099A RID: 2458
			[ReadOnly]
			public NativeArray<float4> nodeNormals;

			// Token: 0x0400099B RID: 2459
			public Matrix4x4 graphToWorld;

			// Token: 0x0400099C RID: 2460
			public IntBounds bounds;

			// Token: 0x0400099D RID: 2461
			public int2 colorDataSize;

			// Token: 0x0400099E RID: 2462
			public float2 scale;

			// Token: 0x0400099F RID: 2463
			public float4 channelPositionScale;
		}

		// Token: 0x02000211 RID: 529
		[BurstCompile]
		public struct JobTexturePenalty : IJob, GridIterationUtilities.INodeModifier
		{
			// Token: 0x06000CF6 RID: 3318 RVA: 0x000511CC File Offset: 0x0004F3CC
			public void ModifyNode(int dataIndex, int dataX, int dataLayer, int dataZ)
			{
				int2 xz = this.bounds.min.xz;
				int2 @int = math.clamp((int2)math.round((new float2((float)dataX, (float)dataZ) + xz) * this.scale), int2.zero, this.colorDataSize - new int2(1, 1));
				int index = @int.y * this.colorDataSize.x + @int.x;
				int4 int2 = new int4(this.colorData[index] & 255, this.colorData[index] >> 8 & 255, this.colorData[index] >> 16 & 255, this.colorData[index] >> 24 & 255);
				ref NativeArray<uint> ptr = ref this.penalty;
				ptr[dataIndex] += (uint)math.dot(this.channelPenalties, int2);
				this.walkable[dataIndex] = (this.walkable[dataIndex] & !math.any(this.channelDeterminesWalkability & int2 == 0));
			}

			// Token: 0x06000CF7 RID: 3319 RVA: 0x00051305 File Offset: 0x0004F505
			public void Execute()
			{
				GridIterationUtilities.ForEachNode<RuleTexture.JobTexturePenalty>(this.bounds.size, this.nodeNormals, ref this);
			}

			// Token: 0x040009A0 RID: 2464
			[ReadOnly]
			public NativeArray<int> colorData;

			// Token: 0x040009A1 RID: 2465
			public NativeArray<uint> penalty;

			// Token: 0x040009A2 RID: 2466
			public NativeArray<bool> walkable;

			// Token: 0x040009A3 RID: 2467
			[ReadOnly]
			public NativeArray<float4> nodeNormals;

			// Token: 0x040009A4 RID: 2468
			public IntBounds bounds;

			// Token: 0x040009A5 RID: 2469
			public int2 colorDataSize;

			// Token: 0x040009A6 RID: 2470
			public float2 scale;

			// Token: 0x040009A7 RID: 2471
			public float4 channelPenalties;

			// Token: 0x040009A8 RID: 2472
			public bool4 channelDeterminesWalkability;
		}
	}
}
