using System;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000024 RID: 36
	[Name("Target In Line Of Sight 2D", 0)]
	[Category("GameObject")]
	[Description("Check of agent is in line of sight with target by doing a linecast and optionaly save the distance")]
	public class CheckLOS2D : ConditionTask<Transform>
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000087 RID: 135 RVA: 0x0000429C File Offset: 0x0000249C
		protected override string info
		{
			get
			{
				return "LOS with " + this.LOSTarget.ToString();
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000042B4 File Offset: 0x000024B4
		protected override bool OnCheck()
		{
			foreach (Collider2D x in from h in Physics2D.LinecastAll(base.agent.position, this.LOSTarget.value.transform.position, this.layerMask.value)
			select h.collider)
			{
				if (x != this.agentCollider && x != this.LOSTarget.value.GetComponent<Collider2D>())
				{
					return false;
				}
			}
			this.saveDistanceAs.value = Vector2.Distance(this.LOSTarget.value.transform.position, base.agent.position);
			return true;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000043C0 File Offset: 0x000025C0
		public override void OnDrawGizmosSelected()
		{
			if (base.agent && this.LOSTarget.value)
			{
				Gizmos.DrawLine(base.agent.position, this.LOSTarget.value.transform.position);
			}
		}

		// Token: 0x04000072 RID: 114
		[RequiredField]
		public BBParameter<GameObject> LOSTarget;

		// Token: 0x04000073 RID: 115
		public BBParameter<LayerMask> layerMask = -1;

		// Token: 0x04000074 RID: 116
		[BlackboardOnly]
		public BBParameter<float> saveDistanceAs;

		// Token: 0x04000075 RID: 117
		[Task.GetFromAgentAttribute]
		protected Collider2D agentCollider;
	}
}
