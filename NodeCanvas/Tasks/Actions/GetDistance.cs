using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200009C RID: 156
	[Category("GameObject")]
	public class GetDistance : ActionTask<Transform>
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000A46B File Offset: 0x0000866B
		protected override string info
		{
			get
			{
				return string.Format("Get Distance to {0}", this.target.ToString());
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000A482 File Offset: 0x00008682
		protected override void OnExecute()
		{
			this.saveAs.value = Vector3.Distance(base.agent.position, this.target.value.transform.position);
			base.EndAction();
		}

		// Token: 0x040001BA RID: 442
		[RequiredField]
		public BBParameter<GameObject> target;

		// Token: 0x040001BB RID: 443
		[BlackboardOnly]
		public BBParameter<float> saveAs;
	}
}
