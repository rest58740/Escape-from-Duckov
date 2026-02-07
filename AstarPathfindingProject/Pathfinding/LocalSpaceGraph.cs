using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000073 RID: 115
	[HelpURL("https://arongranberg.com/astar/documentation/stable/localspacegraph.html")]
	public class LocalSpaceGraph : VersionedMonoBehaviour
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003CD RID: 973 RVA: 0x00013769 File Offset: 0x00011969
		public GraphTransform transformation
		{
			get
			{
				return this.graphTransform;
			}
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00013771 File Offset: 0x00011971
		private void Start()
		{
			this.originalMatrix = base.transform.worldToLocalMatrix;
			base.transform.hasChanged = true;
			this.Refresh();
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00013796 File Offset: 0x00011996
		public void Refresh()
		{
			if (base.transform.hasChanged)
			{
				this.graphTransform.SetMatrix(base.transform.localToWorldMatrix * this.originalMatrix);
				base.transform.hasChanged = false;
			}
		}

		// Token: 0x0400028B RID: 651
		private Matrix4x4 originalMatrix;

		// Token: 0x0400028C RID: 652
		private MutableGraphTransform graphTransform = new MutableGraphTransform(Matrix4x4.identity);
	}
}
