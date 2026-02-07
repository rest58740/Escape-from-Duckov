using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000023 RID: 35
	[Name("Target In Line Of Sight", 0)]
	[Category("GameObject")]
	[Description("Check of agent is in line of sight with target by doing a linecast and optionaly save the distance")]
	public class CheckLOS : ConditionTask<Transform>
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00004162 File Offset: 0x00002362
		protected override string info
		{
			get
			{
				return "LOS with " + this.LOSTarget.ToString();
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000417C File Offset: 0x0000237C
		protected override bool OnCheck()
		{
			Transform transform = this.LOSTarget.value.transform;
			if (Physics.Linecast(base.agent.position + this.offset, transform.position + this.offset, out this.hit, this.layerMask.value))
			{
				Collider component = transform.GetComponent<Collider>();
				if (component == null || this.hit.collider != component)
				{
					this.saveDistanceAs.value = this.hit.distance;
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000421C File Offset: 0x0000241C
		public override void OnDrawGizmosSelected()
		{
			if (base.agent && this.LOSTarget.value)
			{
				Gizmos.DrawLine(base.agent.position + this.offset, this.LOSTarget.value.transform.position + this.offset);
			}
		}

		// Token: 0x0400006D RID: 109
		[RequiredField]
		public BBParameter<GameObject> LOSTarget;

		// Token: 0x0400006E RID: 110
		public BBParameter<LayerMask> layerMask = -1;

		// Token: 0x0400006F RID: 111
		public Vector3 offset;

		// Token: 0x04000070 RID: 112
		[BlackboardOnly]
		public BBParameter<float> saveDistanceAs;

		// Token: 0x04000071 RID: 113
		private RaycastHit hit;
	}
}
