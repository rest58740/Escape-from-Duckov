using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000C9 RID: 201
	[Category("✫ Utility")]
	public class DebugDrawLine : ActionTask
	{
		// Token: 0x06000372 RID: 882 RVA: 0x0000DA5C File Offset: 0x0000BC5C
		protected override void OnExecute()
		{
			Debug.DrawLine(this.from.value, this.to.value, this.color, this.timeToShow);
			base.EndAction(true);
		}

		// Token: 0x04000257 RID: 599
		public BBParameter<Vector3> from;

		// Token: 0x04000258 RID: 600
		public BBParameter<Vector3> to;

		// Token: 0x04000259 RID: 601
		public Color color = Color.white;

		// Token: 0x0400025A RID: 602
		public float timeToShow = 0.1f;
	}
}
