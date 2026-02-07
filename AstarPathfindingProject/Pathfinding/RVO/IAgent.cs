using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002AE RID: 686
	public interface IAgent
	{
		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600103B RID: 4155
		int AgentIndex { get; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x0600103C RID: 4156
		// (set) Token: 0x0600103D RID: 4157
		Vector3 Position { get; set; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x0600103E RID: 4158
		Vector3 CalculatedTargetPoint { get; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x0600103F RID: 4159
		bool AvoidingAnyAgents { get; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06001040 RID: 4160
		float CalculatedSpeed { get; }

		// Token: 0x06001041 RID: 4161
		void SetTarget(Vector3 targetPoint, float desiredSpeed, float maxSpeed, Vector3 endOfPath);

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06001042 RID: 4162
		// (set) Token: 0x06001043 RID: 4163
		SimpleMovementPlane MovementPlane { get; set; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06001044 RID: 4164
		// (set) Token: 0x06001045 RID: 4165
		bool Locked { get; set; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06001046 RID: 4166
		// (set) Token: 0x06001047 RID: 4167
		float Radius { get; set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06001048 RID: 4168
		// (set) Token: 0x06001049 RID: 4169
		float Height { get; set; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x0600104A RID: 4170
		// (set) Token: 0x0600104B RID: 4171
		float AgentTimeHorizon { get; set; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x0600104C RID: 4172
		// (set) Token: 0x0600104D RID: 4173
		float ObstacleTimeHorizon { get; set; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x0600104E RID: 4174
		// (set) Token: 0x0600104F RID: 4175
		int MaxNeighbours { get; set; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06001050 RID: 4176
		int NeighbourCount { get; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06001051 RID: 4177
		// (set) Token: 0x06001052 RID: 4178
		RVOLayer Layer { get; set; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06001053 RID: 4179
		// (set) Token: 0x06001054 RID: 4180
		RVOLayer CollidesWith { get; set; }

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06001055 RID: 4181
		// (set) Token: 0x06001056 RID: 4182
		float FlowFollowingStrength { get; set; }

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06001057 RID: 4183
		// (set) Token: 0x06001058 RID: 4184
		AgentDebugFlags DebugFlags { get; set; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06001059 RID: 4185
		// (set) Token: 0x0600105A RID: 4186
		float Priority { get; set; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x0600105B RID: 4187
		// (set) Token: 0x0600105C RID: 4188
		int HierarchicalNodeIndex { get; set; }

		// Token: 0x17000248 RID: 584
		// (set) Token: 0x0600105D RID: 4189
		Action PreCalculationCallback { set; }

		// Token: 0x17000249 RID: 585
		// (set) Token: 0x0600105E RID: 4190
		Action DestroyedCallback { set; }

		// Token: 0x0600105F RID: 4191
		void SetCollisionNormal(Vector3 normal);

		// Token: 0x06001060 RID: 4192
		void ForceSetVelocity(Vector3 velocity);

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06001061 RID: 4193
		ReachedEndOfPath CalculatedEffectivelyReachedDestination { get; }

		// Token: 0x06001062 RID: 4194
		void SetObstacleQuery(GraphNode sourceNode);
	}
}
