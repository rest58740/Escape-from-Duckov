using System;
using System.Collections;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x0200011F RID: 287
	[Name("Iterate", 0)]
	[Category("Decorators")]
	[Description("Iterates a list and executes its child once for each element in that list. Keeps iterating until the Termination Policy is met or until the whole list is iterated, in which case the last iteration child status is returned.")]
	[ParadoxNotion.Design.Icon("List", false, "")]
	public class Iterator : BTDecorator
	{
		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x000136AE File Offset: 0x000118AE
		private IList list
		{
			get
			{
				if (this.targetList == null)
				{
					return null;
				}
				return this.targetList.value;
			}
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x000136C8 File Offset: 0x000118C8
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (base.decoratedConnection == null)
			{
				return Status.Optional;
			}
			if (this.list == null || this.list.Count == 0)
			{
				return Status.Failure;
			}
			for (int i = this.currentIndex; i < this.list.Count; i++)
			{
				this.current.value = this.list[i];
				this.storeIndex.value = i;
				base.status = base.decoratedConnection.Execute(agent, blackboard);
				if (base.status == Status.Success && this.terminationCondition == Iterator.TerminationConditions.FirstSuccess)
				{
					return Status.Success;
				}
				if (base.status == Status.Failure && this.terminationCondition == Iterator.TerminationConditions.FirstFailure)
				{
					return Status.Failure;
				}
				if (base.status == Status.Running)
				{
					this.currentIndex = i;
					return Status.Running;
				}
				if (this.currentIndex == this.list.Count - 1 || this.currentIndex == this.maxIteration.value - 1)
				{
					if (this.resetIndex)
					{
						this.currentIndex = 0;
					}
					return base.status;
				}
				base.decoratedConnection.Reset(true);
				this.currentIndex++;
			}
			return Status.Running;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x000137E1 File Offset: 0x000119E1
		protected override void OnReset()
		{
			if (this.resetIndex)
			{
				this.currentIndex = 0;
			}
		}

		// Token: 0x0400033C RID: 828
		[RequiredField]
		[BlackboardOnly]
		[Tooltip("The list to iterate.")]
		public BBParameter<IList> targetList;

		// Token: 0x0400033D RID: 829
		[BlackboardOnly]
		[Name("Current Element", 0)]
		[Tooltip("Store the currently iterated list element in a variable.")]
		public BBObjectParameter current;

		// Token: 0x0400033E RID: 830
		[BlackboardOnly]
		[Name("Current Index", 0)]
		[Tooltip("Store the currently iterated list index in a variable.")]
		public BBParameter<int> storeIndex;

		// Token: 0x0400033F RID: 831
		[Name("Termination Policy", 0)]
		[Tooltip("The condition for when to terminate the iteration and return status.")]
		public Iterator.TerminationConditions terminationCondition;

		// Token: 0x04000340 RID: 832
		[Tooltip("The maximum allowed iterations. Leave at -1 to iterate the whole list.")]
		public BBParameter<int> maxIteration = -1;

		// Token: 0x04000341 RID: 833
		[Tooltip("Should the iteration start from the begining after the Iterator node resets?")]
		public bool resetIndex = true;

		// Token: 0x04000342 RID: 834
		private int currentIndex;

		// Token: 0x02000170 RID: 368
		public enum TerminationConditions
		{
			// Token: 0x04000428 RID: 1064
			None,
			// Token: 0x04000429 RID: 1065
			FirstSuccess,
			// Token: 0x0400042A RID: 1066
			FirstFailure
		}
	}
}
