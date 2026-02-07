using System;
using System.Collections.Generic;
using Duckov.Quests;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.BasketBalls
{
	// Token: 0x02000325 RID: 805
	public class Basket : MonoBehaviour
	{
		// Token: 0x06001AB7 RID: 6839 RVA: 0x0006108A File Offset: 0x0005F28A
		private void Awake()
		{
			this.trigger.onGoal.AddListener(new UnityAction<BasketBall>(this.OnGoal));
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x000610A8 File Offset: 0x0005F2A8
		private void OnGoal(BasketBall ball)
		{
			if (!this.conditions.Satisfied())
			{
				return;
			}
			this.onGoal.Invoke(ball);
			this.netAnimator.SetTrigger("Goal");
		}

		// Token: 0x04001348 RID: 4936
		[SerializeField]
		private Animator netAnimator;

		// Token: 0x04001349 RID: 4937
		[SerializeField]
		private List<Condition> conditions = new List<Condition>();

		// Token: 0x0400134A RID: 4938
		[SerializeField]
		private BasketTrigger trigger;

		// Token: 0x0400134B RID: 4939
		public UnityEvent<BasketBall> onGoal;
	}
}
