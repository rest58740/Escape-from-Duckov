using System;
using UnityEngine;

namespace Drawing
{
	// Token: 0x02000052 RID: 82
	public abstract class MonoBehaviourGizmos : MonoBehaviour, IDrawGizmos
	{
		// Token: 0x060003A7 RID: 935 RVA: 0x0001020F File Offset: 0x0000E40F
		public MonoBehaviourGizmos()
		{
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00002104 File Offset: 0x00000304
		private void OnDrawGizmosSelected()
		{
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00002104 File Offset: 0x00000304
		public virtual void DrawGizmos()
		{
		}
	}
}
