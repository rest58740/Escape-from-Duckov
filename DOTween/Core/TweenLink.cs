using System;
using UnityEngine;

namespace DG.Tweening.Core
{
	// Token: 0x02000055 RID: 85
	internal class TweenLink
	{
		// Token: 0x060002E4 RID: 740 RVA: 0x0000FE1C File Offset: 0x0000E01C
		public TweenLink(GameObject target, LinkBehaviour behaviour)
		{
			this.target = target;
			this.behaviour = behaviour;
			this.lastSeenActive = target.activeInHierarchy;
		}

		// Token: 0x04000171 RID: 369
		public readonly GameObject target;

		// Token: 0x04000172 RID: 370
		public readonly LinkBehaviour behaviour;

		// Token: 0x04000173 RID: 371
		public bool lastSeenActive;
	}
}
