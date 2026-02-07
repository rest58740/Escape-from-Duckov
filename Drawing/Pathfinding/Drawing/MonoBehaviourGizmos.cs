using System;
using UnityEngine;

namespace Pathfinding.Drawing
{
	// Token: 0x02000054 RID: 84
	public abstract class MonoBehaviourGizmos : MonoBehaviour, IDrawGizmos
	{
		// Token: 0x06000299 RID: 665 RVA: 0x0000E60B File Offset: 0x0000C80B
		public MonoBehaviourGizmos()
		{
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00002104 File Offset: 0x00000304
		private void OnDrawGizmosSelected()
		{
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00002104 File Offset: 0x00000304
		public virtual void DrawGizmos()
		{
		}
	}
}
