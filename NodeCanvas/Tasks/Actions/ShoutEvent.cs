using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000D3 RID: 211
	[Category("✫ Utility")]
	[Description("Sends an event to all GraphOwners within range of the agent and over time like a shockwave.")]
	public class ShoutEvent : ActionTask<Transform>
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0000E284 File Offset: 0x0000C484
		protected override string info
		{
			get
			{
				return string.Format("Shout Event [{0}]", this.eventName.ToString());
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000E29B File Offset: 0x0000C49B
		protected override void OnExecute()
		{
			this.owners = Object.FindObjectsByType<GraphOwner>(FindObjectsSortMode.None);
			this.receivedOwners = new bool[this.owners.Length];
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000E2BC File Offset: 0x0000C4BC
		protected override void OnUpdate()
		{
			this.traveledDistance = Mathf.Lerp(0f, this.shoutRange.value, base.elapsedTime / this.completionTime.value);
			for (int i = 0; i < this.owners.Length; i++)
			{
				GraphOwner graphOwner = this.owners[i];
				if ((base.agent.position - graphOwner.transform.position).magnitude <= this.traveledDistance && !this.receivedOwners[i])
				{
					graphOwner.SendEvent(this.eventName.value, null, this);
					this.receivedOwners[i] = true;
				}
			}
			if (base.elapsedTime >= this.completionTime.value)
			{
				base.EndAction();
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000E37C File Offset: 0x0000C57C
		public override void OnDrawGizmosSelected()
		{
			if (base.agent != null)
			{
				Gizmos.color = new Color(1f, 1f, 1f, 0.2f);
				Gizmos.DrawWireSphere(base.agent.position, this.traveledDistance);
				Gizmos.DrawWireSphere(base.agent.position, this.shoutRange.value);
			}
		}

		// Token: 0x04000274 RID: 628
		[RequiredField]
		public BBParameter<string> eventName;

		// Token: 0x04000275 RID: 629
		public BBParameter<float> shoutRange = 10f;

		// Token: 0x04000276 RID: 630
		public BBParameter<float> completionTime = 1f;

		// Token: 0x04000277 RID: 631
		private GraphOwner[] owners;

		// Token: 0x04000278 RID: 632
		private bool[] receivedOwners;

		// Token: 0x04000279 RID: 633
		private float traveledDistance;
	}
}
