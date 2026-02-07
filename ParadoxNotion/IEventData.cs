using System;
using UnityEngine;

namespace ParadoxNotion
{
	// Token: 0x02000071 RID: 113
	public interface IEventData
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000409 RID: 1033
		GameObject receiver { get; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600040A RID: 1034
		object sender { get; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600040B RID: 1035
		object valueBoxed { get; }
	}
}
