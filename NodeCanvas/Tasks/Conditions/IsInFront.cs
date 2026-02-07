using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions
{
	// Token: 0x02000027 RID: 39
	[Name("Target In View Angle", 0)]
	[Category("GameObject")]
	[Description("Checks whether the target is in the view angle of the agent")]
	public class IsInFront : ConditionTask<Transform>
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0000448A File Offset: 0x0000268A
		protected override string info
		{
			get
			{
				BBParameter<GameObject> bbparameter = this.checkTarget;
				return ((bbparameter != null) ? bbparameter.ToString() : null) + " in view angle";
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000044A8 File Offset: 0x000026A8
		protected override bool OnCheck()
		{
			return Vector3.Angle(this.checkTarget.value.transform.position - base.agent.position, base.agent.forward) < this.viewAngle.value;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000044F8 File Offset: 0x000026F8
		public override void OnDrawGizmosSelected()
		{
			if (base.agent != null)
			{
				Gizmos.matrix = Matrix4x4.TRS(base.agent.position, base.agent.rotation, Vector3.one);
				Gizmos.DrawFrustum(Vector3.zero, this.viewAngle.value, 5f, 0f, 1f);
			}
		}

		// Token: 0x04000076 RID: 118
		[RequiredField]
		public BBParameter<GameObject> checkTarget;

		// Token: 0x04000077 RID: 119
		[SliderField(1, 180)]
		public BBParameter<float> viewAngle = 70f;
	}
}
