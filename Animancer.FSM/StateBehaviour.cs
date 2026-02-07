using System;
using UnityEngine;

namespace Animancer.FSM
{
	// Token: 0x02000009 RID: 9
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer.FSM/StateBehaviour")]
	public abstract class StateBehaviour : MonoBehaviour, IState
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002290 File Offset: 0x00000490
		public virtual bool CanEnterState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002293 File Offset: 0x00000493
		public virtual bool CanExitState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002296 File Offset: 0x00000496
		public virtual void OnEnterState()
		{
			base.enabled = true;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000229F File Offset: 0x0000049F
		public virtual void OnExitState()
		{
			if (this == null)
			{
				return;
			}
			base.enabled = false;
		}
	}
}
