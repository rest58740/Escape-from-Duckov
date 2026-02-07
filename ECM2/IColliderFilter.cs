using System;
using UnityEngine;

namespace ECM2
{
	// Token: 0x02000011 RID: 17
	public interface IColliderFilter
	{
		// Token: 0x06000262 RID: 610
		bool Filter(Collider otherCollider);
	}
}
