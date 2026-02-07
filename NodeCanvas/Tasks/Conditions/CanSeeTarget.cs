using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200001B RID: 27
	[Category("GameObject")]
	[Description("A combination of line of sight and view angle check")]
	public class CanSeeTarget : ConditionTask<Transform>
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002B7B File Offset: 0x00000D7B
		protected override string info
		{
			get
			{
				string text = "Can See ";
				BBParameter<GameObject> bbparameter = this.target;
				return text + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002B9C File Offset: 0x00000D9C
		protected override bool OnCheck()
		{
			Transform transform = this.target.value.transform;
			if (!transform.gameObject.activeInHierarchy)
			{
				return false;
			}
			if (Vector3.Distance(base.agent.position, transform.position) <= this.awarnessDistance.value)
			{
				return !Physics.Linecast(base.agent.position + this.offset, transform.position + this.offset, out this.hit, this.layerMask.value) || !(this.hit.collider != transform.GetComponent<Collider>());
			}
			return Vector3.Distance(base.agent.position, transform.position) <= this.maxDistance.value && Vector3.Angle(transform.position - base.agent.position, base.agent.forward) <= this.viewAngle.value && (!Physics.Linecast(base.agent.position + this.offset, transform.position + this.offset, out this.hit, this.layerMask.value) || !(this.hit.collider != transform.GetComponent<Collider>()));
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002D08 File Offset: 0x00000F08
		public override void OnDrawGizmosSelected()
		{
			if (base.agent != null)
			{
				Gizmos.DrawLine(base.agent.position, base.agent.position + this.offset);
				Gizmos.DrawLine(base.agent.position + this.offset, base.agent.position + this.offset + base.agent.forward * this.maxDistance.value);
				Gizmos.DrawWireSphere(base.agent.position + this.offset + base.agent.forward * this.maxDistance.value, 0.1f);
				Gizmos.DrawWireSphere(base.agent.position, this.awarnessDistance.value);
				Gizmos.matrix = Matrix4x4.TRS(base.agent.position + this.offset, base.agent.rotation, Vector3.one);
				Gizmos.DrawFrustum(Vector3.zero, this.viewAngle.value, 5f, 0f, 1f);
			}
		}

		// Token: 0x0400003A RID: 58
		[RequiredField]
		public BBParameter<GameObject> target;

		// Token: 0x0400003B RID: 59
		[Tooltip("Distance within which to look out for.")]
		public BBParameter<float> maxDistance = 50f;

		// Token: 0x0400003C RID: 60
		[Tooltip("A layer mask to use for line of sight check.")]
		public BBParameter<LayerMask> layerMask = -1;

		// Token: 0x0400003D RID: 61
		[Tooltip("Distance within which the target can be seen (or rather sensed) regardless of view angle.")]
		public BBParameter<float> awarnessDistance = 0f;

		// Token: 0x0400003E RID: 62
		[SliderField(1, 180)]
		public BBParameter<float> viewAngle = 70f;

		// Token: 0x0400003F RID: 63
		public Vector3 offset;

		// Token: 0x04000040 RID: 64
		private RaycastHit hit;
	}
}
