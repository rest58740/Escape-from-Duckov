using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x0200001C RID: 28
	[Category("GameObject")]
	[Description("A combination of line of sight and view angle check")]
	public class CanSeeTarget2D : ConditionTask<Transform>
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002EA0 File Offset: 0x000010A0
		protected override string info
		{
			get
			{
				string text = "Can See ";
				BBParameter<GameObject> bbparameter = this.target;
				return text + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002EC0 File Offset: 0x000010C0
		protected override bool OnCheck()
		{
			Transform transform = this.target.value.transform;
			if (!transform.gameObject.activeInHierarchy)
			{
				return false;
			}
			if (Vector2.Distance(base.agent.position, transform.position) <= this.awarnessDistance.value)
			{
				return !(Physics2D.Linecast(base.agent.position + this.offset, transform.position + this.offset, this.layerMask.value).collider != transform.GetComponent<Collider2D>());
			}
			return Vector2.Distance(base.agent.position, transform.position) <= this.maxDistance.value && Vector2.Angle(transform.position - base.agent.position, base.agent.right) <= this.viewAngle.value && !(Physics2D.Linecast(base.agent.position + this.offset, transform.position + this.offset, this.layerMask.value).collider != transform.GetComponent<Collider2D>());
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000304C File Offset: 0x0000124C
		public override void OnDrawGizmosSelected()
		{
			if (base.agent != null)
			{
				Gizmos.DrawLine(base.agent.position, base.agent.position + this.offset);
				Gizmos.DrawLine(base.agent.position + this.offset, base.agent.position + this.offset + base.agent.right * this.maxDistance.value);
				Gizmos.DrawWireSphere(base.agent.position + this.offset + base.agent.right * this.maxDistance.value, 0.1f);
				Gizmos.DrawWireSphere(base.agent.position, this.awarnessDistance.value);
				Gizmos.matrix = Matrix4x4.TRS(base.agent.position + this.offset, Quaternion.LookRotation(base.agent.right), Vector3.one);
				Gizmos.DrawFrustum(Vector3.zero, this.viewAngle.value, 5f, 0f, 1f);
			}
		}

		// Token: 0x04000041 RID: 65
		[RequiredField]
		public BBParameter<GameObject> target;

		// Token: 0x04000042 RID: 66
		[Tooltip("Distance within which to look out for.")]
		public BBParameter<float> maxDistance = 50f;

		// Token: 0x04000043 RID: 67
		[Tooltip("A layer mask to use for the line of sight check.")]
		public BBParameter<LayerMask> layerMask = -1;

		// Token: 0x04000044 RID: 68
		[Tooltip("Distance within which the target can be seen (or rather sensed) regardless of view angle.")]
		public BBParameter<float> awarnessDistance = 0f;

		// Token: 0x04000045 RID: 69
		[SliderField(1, 180)]
		public BBParameter<float> viewAngle = 70f;

		// Token: 0x04000046 RID: 70
		public Vector2 offset;

		// Token: 0x04000047 RID: 71
		private RaycastHit2D hit;
	}
}
