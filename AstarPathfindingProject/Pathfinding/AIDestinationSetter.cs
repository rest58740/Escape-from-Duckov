using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000011 RID: 17
	[UniqueComponent(tag = "ai.destination")]
	[AddComponentMenu("Pathfinding/AI/Behaviors/AIDestinationSetter")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/aidestinationsetter.html")]
	public class AIDestinationSetter : VersionedMonoBehaviour
	{
		// Token: 0x06000078 RID: 120 RVA: 0x000040CC File Offset: 0x000022CC
		private void OnEnable()
		{
			this.ai = base.GetComponent<IAstarAI>();
			if (this.ai != null)
			{
				IAstarAI astarAI = this.ai;
				astarAI.onSearchPath = (Action)Delegate.Combine(astarAI.onSearchPath, new Action(this.UpdateDestination));
			}
			BatchedEvents.Add<AIDestinationSetter>(this, BatchedEvents.Event.Update, new Action<AIDestinationSetter[], int>(AIDestinationSetter.OnUpdate), 0);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004128 File Offset: 0x00002328
		private void OnDisable()
		{
			if (this.ai != null)
			{
				IAstarAI astarAI = this.ai;
				astarAI.onSearchPath = (Action)Delegate.Remove(astarAI.onSearchPath, new Action(this.UpdateDestination));
			}
			BatchedEvents.Remove<AIDestinationSetter>(this);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004160 File Offset: 0x00002360
		private static void OnUpdate(AIDestinationSetter[] components, int count)
		{
			for (int i = 0; i < count; i++)
			{
				components[i].UpdateDestination();
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004181 File Offset: 0x00002381
		private void UpdateDestination()
		{
			if (this.target != null && this.ai != null)
			{
				this.ai.destination = this.target.position;
			}
		}

		// Token: 0x0400006E RID: 110
		public Transform target;

		// Token: 0x0400006F RID: 111
		public bool useRotation;

		// Token: 0x04000070 RID: 112
		private IAstarAI ai;
	}
}
