using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x0200011C RID: 284
	[Name("Guard", 0)]
	[Category("Decorators")]
	[ParadoxNotion.Design.Icon("Shield", false, "")]
	[Description("Protects the decorated child from running if another Guard with the same token is already guarding (Running) that token.\nGuarding is global for all of the agent Behaviour Trees.")]
	public class Guard : BTDecorator
	{
		// Token: 0x06000605 RID: 1541 RVA: 0x000133C6 File Offset: 0x000115C6
		private static List<Guard> AgentGuards(Component agent)
		{
			return Guard.guards[agent.gameObject];
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x000133D8 File Offset: 0x000115D8
		public override void OnGraphStarted()
		{
			this.SetGuards(base.graphAgent);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x000133E8 File Offset: 0x000115E8
		public override void OnGraphStoped()
		{
			foreach (Graph graph in Graph.runningGraphs)
			{
				if (graph.agent != null && graph.agent.gameObject == base.graphAgent.gameObject)
				{
					return;
				}
			}
			Guard.guards.Remove(base.graphAgent.gameObject);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00013470 File Offset: 0x00011670
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (base.decoratedConnection == null)
			{
				return Status.Optional;
			}
			if (agent != base.graphAgent)
			{
				this.SetGuards(agent);
			}
			int i = 0;
			while (i < Guard.AgentGuards(agent).Count)
			{
				Guard guard = Guard.AgentGuards(agent)[i];
				if (guard != this && guard.isGuarding && guard.token.value == this.token.value)
				{
					if (this.ifGuarded != Guard.GuardMode.ReturnFailure)
					{
						return Status.Running;
					}
					return Status.Failure;
				}
				else
				{
					i++;
				}
			}
			base.status = base.decoratedConnection.Execute(agent, blackboard);
			if (base.status == Status.Running)
			{
				this.isGuarding = true;
				return Status.Running;
			}
			this.isGuarding = false;
			return base.status;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00013526 File Offset: 0x00011726
		protected override void OnReset()
		{
			this.isGuarding = false;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00013530 File Offset: 0x00011730
		private void SetGuards(Component guardAgent)
		{
			if (!Guard.guards.ContainsKey(guardAgent.gameObject))
			{
				Guard.guards[guardAgent.gameObject] = new List<Guard>();
			}
			if (!Guard.AgentGuards(guardAgent).Contains(this) && !string.IsNullOrEmpty(this.token.value))
			{
				Guard.AgentGuards(guardAgent).Add(this);
			}
		}

		// Token: 0x04000337 RID: 823
		[Tooltip("A unique Token to use for guarding.")]
		public BBParameter<string> token;

		// Token: 0x04000338 RID: 824
		[Tooltip("What to return in case the token is already guarded by another Guard.")]
		public Guard.GuardMode ifGuarded;

		// Token: 0x04000339 RID: 825
		private bool isGuarding;

		// Token: 0x0400033A RID: 826
		private static readonly Dictionary<GameObject, List<Guard>> guards = new Dictionary<GameObject, List<Guard>>();

		// Token: 0x0200016F RID: 367
		public enum GuardMode
		{
			// Token: 0x04000425 RID: 1061
			ReturnFailure,
			// Token: 0x04000426 RID: 1062
			WaitUntilReleased
		}
	}
}
