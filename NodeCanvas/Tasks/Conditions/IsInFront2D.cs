using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000028 RID: 40
	[Name("Target In View Angle 2D", 0)]
	[Category("GameObject")]
	[Description("Checks whether the target is in the view angle of the agent")]
	public class IsInFront2D : ConditionTask<Transform>
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00004574 File Offset: 0x00002774
		protected override string info
		{
			get
			{
				BBParameter<GameObject> bbparameter = this.checkTarget;
				return ((bbparameter != null) ? bbparameter.ToString() : null) + " in view angle";
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004594 File Offset: 0x00002794
		protected override bool OnCheck()
		{
			return Vector2.Angle(this.checkTarget.value.transform.position - base.agent.position, base.agent.right) < this.viewAngle.value;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000045F4 File Offset: 0x000027F4
		public override void OnDrawGizmosSelected()
		{
			if (base.agent != null)
			{
				Gizmos.matrix = Matrix4x4.TRS(base.agent.position, base.agent.rotation, Vector3.one);
				Gizmos.DrawFrustum(Vector3.zero, this.viewAngle.value, 5f, 0f, 0f);
			}
		}

		// Token: 0x04000078 RID: 120
		[RequiredField]
		public BBParameter<GameObject> checkTarget;

		// Token: 0x04000079 RID: 121
		[SliderField(1, 180)]
		public BBParameter<float> viewAngle = 70f;
	}
}
