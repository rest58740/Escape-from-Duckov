using System;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x0200010A RID: 266
	[AddComponentMenu("NodeCanvas/Behaviour Tree Owner")]
	public class BehaviourTreeOwner : GraphOwner<BehaviourTree>
	{
		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x0001237A File Offset: 0x0001057A
		// (set) Token: 0x060005A0 RID: 1440 RVA: 0x00012397 File Offset: 0x00010597
		public bool repeat
		{
			get
			{
				return !(base.behaviour != null) || base.behaviour.repeat;
			}
			set
			{
				if (base.behaviour != null)
				{
					base.behaviour.repeat = value;
				}
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x000123B3 File Offset: 0x000105B3
		// (set) Token: 0x060005A2 RID: 1442 RVA: 0x000123D4 File Offset: 0x000105D4
		public float updateInterval
		{
			get
			{
				if (!(base.behaviour != null))
				{
					return 0f;
				}
				return base.behaviour.updateInterval;
			}
			set
			{
				if (base.behaviour != null)
				{
					base.behaviour.updateInterval = value;
				}
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x000123F0 File Offset: 0x000105F0
		public Status rootStatus
		{
			get
			{
				if (!(base.behaviour != null))
				{
					return Status.Resting;
				}
				return base.behaviour.rootStatus;
			}
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001240D File Offset: 0x0001060D
		public Status Tick()
		{
			if (base.behaviour == null)
			{
				return Status.Resting;
			}
			base.UpdateBehaviour();
			return base.behaviour.rootStatus;
		}
	}
}
