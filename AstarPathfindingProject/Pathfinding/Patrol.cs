using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000014 RID: 20
	[UniqueComponent(tag = "ai.destination")]
	[AddComponentMenu("Pathfinding/AI/Behaviors/Patrol")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/patrol.html")]
	public class Patrol : VersionedMonoBehaviour
	{
		// Token: 0x06000088 RID: 136 RVA: 0x000046E4 File Offset: 0x000028E4
		protected override void Awake()
		{
			base.Awake();
			this.agent = base.GetComponent<IAstarAI>();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000046F8 File Offset: 0x000028F8
		private void Update()
		{
			if (this.targets.Length == 0)
			{
				return;
			}
			if (this.agent.reachedEndOfPath && !this.agent.pathPending && float.IsPositiveInfinity(this.switchTime))
			{
				this.switchTime = Time.time + this.delay;
			}
			if (Time.time >= this.switchTime)
			{
				this.index++;
				this.switchTime = float.PositiveInfinity;
				this.index %= this.targets.Length;
				this.agent.destination = this.targets[this.index].position;
				this.agent.SearchPath();
				return;
			}
			if (this.updateDestinationEveryFrame)
			{
				this.index %= this.targets.Length;
				this.agent.destination = this.targets[this.index].position;
			}
		}

		// Token: 0x04000076 RID: 118
		public Transform[] targets;

		// Token: 0x04000077 RID: 119
		public float delay;

		// Token: 0x04000078 RID: 120
		public bool updateDestinationEveryFrame;

		// Token: 0x04000079 RID: 121
		private int index = -1;

		// Token: 0x0400007A RID: 122
		private IAstarAI agent;

		// Token: 0x0400007B RID: 123
		private float switchTime = float.NegativeInfinity;
	}
}
