using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000B8 RID: 184
	[Category("Physics")]
	[Description("Gets a lists of game objects that are in the physics overlap sphere at the position of the agent, excluding the agent")]
	public class GetOverlapSphereObjects : ActionTask<Transform>
	{
		// Token: 0x06000302 RID: 770 RVA: 0x0000C1C4 File Offset: 0x0000A3C4
		protected override void OnExecute()
		{
			Collider[] source = Physics.OverlapSphere(base.agent.position, this.radius.value, this.layerMask);
			this.saveObjectsAs.value = (from c in source
			select c.gameObject).ToList<GameObject>();
			this.saveObjectsAs.value.Remove(base.agent.gameObject);
			if (this.saveObjectsAs.value.Count == 0)
			{
				base.EndAction(false);
				return;
			}
			base.EndAction(true);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000C26C File Offset: 0x0000A46C
		public override void OnDrawGizmosSelected()
		{
			if (base.agent != null)
			{
				Gizmos.color = new Color(1f, 1f, 1f, 0.2f);
				Gizmos.DrawSphere(base.agent.position, this.radius.value);
			}
		}

		// Token: 0x04000233 RID: 563
		public LayerMask layerMask = -1;

		// Token: 0x04000234 RID: 564
		public BBParameter<float> radius = 2f;

		// Token: 0x04000235 RID: 565
		[BlackboardOnly]
		public BBParameter<List<GameObject>> saveObjectsAs;
	}
}
