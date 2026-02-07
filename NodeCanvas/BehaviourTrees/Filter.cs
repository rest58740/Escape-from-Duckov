using System;
using System.Collections;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x0200011B RID: 283
	[Name("Filter", 0)]
	[Category("Decorators")]
	[Description("Filters the access of its child either a specific number of times, or every specific amount of time.")]
	[ParadoxNotion.Design.Icon("Filter", false, "")]
	public class Filter : BTDecorator
	{
		// Token: 0x06000601 RID: 1537 RVA: 0x00013277 File Offset: 0x00011477
		public override void OnGraphStoped()
		{
			this.executedCount = 0;
			this.currentTime = 0f;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001328C File Offset: 0x0001148C
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (base.decoratedConnection == null)
			{
				return Status.Optional;
			}
			Filter.FilterMode filterMode = this.filterMode;
			if (filterMode != Filter.FilterMode.LimitNumberOfTimes)
			{
				if (filterMode == Filter.FilterMode.CoolDown)
				{
					if (this.currentTime > 0f)
					{
						if (!this.inactiveWhenLimited)
						{
							return Status.Failure;
						}
						return Status.Optional;
					}
					else
					{
						base.status = base.decoratedConnection.Execute(agent, blackboard);
						if (base.status == Status.Success || base.status == Status.Failure)
						{
							base.StartCoroutine(this.Cooldown());
						}
					}
				}
			}
			else if (this.executedCount >= this.maxCount.value)
			{
				if (!this.inactiveWhenLimited)
				{
					return Status.Failure;
				}
				return Status.Optional;
			}
			else
			{
				base.status = base.decoratedConnection.Execute(agent, blackboard);
				if ((base.status == Status.Success && this.policy == Filter.Policy.SuccessOnly) || (base.status == Status.Failure && this.policy == Filter.Policy.FailureOnly) || ((base.status == Status.Success || base.status == Status.Failure) && this.policy == Filter.Policy.SuccessOrFailure))
				{
					this.executedCount++;
				}
			}
			return base.status;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00013385 File Offset: 0x00011585
		private IEnumerator Cooldown()
		{
			this.currentTime = this.coolDownTime.value;
			while (this.currentTime > 0f)
			{
				yield return null;
				this.currentTime -= Time.deltaTime;
			}
			yield break;
		}

		// Token: 0x04000330 RID: 816
		[Tooltip("The mode to use.")]
		public Filter.FilterMode filterMode = Filter.FilterMode.CoolDown;

		// Token: 0x04000331 RID: 817
		[ShowIf("filterMode", 0)]
		[Name("Max Times", 0)]
		[Tooltip("The max ammount of times to allow the child to execute until the tree is completely restarted.")]
		public BBParameter<int> maxCount = 1;

		// Token: 0x04000332 RID: 818
		[ShowIf("filterMode", 0)]
		[Name("Increase Count When", 0)]
		[Tooltip("Only increase count if the selected status is returned from the child.")]
		public Filter.Policy policy;

		// Token: 0x04000333 RID: 819
		[ShowIf("filterMode", 1)]
		[Tooltip("The time to disallow execution for.")]
		public BBParameter<float> coolDownTime = 5f;

		// Token: 0x04000334 RID: 820
		[Name("Optional When Filtered", 0)]
		[Tooltip("If enabled, the Filter Decorator will return an Optional status when it is filtered. Otherwise it will return Failure.")]
		public bool inactiveWhenLimited = true;

		// Token: 0x04000335 RID: 821
		private int executedCount;

		// Token: 0x04000336 RID: 822
		private float currentTime;

		// Token: 0x0200016C RID: 364
		public enum FilterMode
		{
			// Token: 0x0400041B RID: 1051
			LimitNumberOfTimes,
			// Token: 0x0400041C RID: 1052
			CoolDown
		}

		// Token: 0x0200016D RID: 365
		public enum Policy
		{
			// Token: 0x0400041E RID: 1054
			SuccessOrFailure,
			// Token: 0x0400041F RID: 1055
			SuccessOnly,
			// Token: 0x04000420 RID: 1056
			FailureOnly
		}
	}
}
