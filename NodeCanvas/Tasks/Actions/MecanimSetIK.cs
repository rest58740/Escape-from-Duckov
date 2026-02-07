using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200005E RID: 94
	[Name("Set IK", 0)]
	[Category("Animator")]
	public class MecanimSetIK : ActionTask<Animator>
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001DF RID: 479 RVA: 0x000085B5 File Offset: 0x000067B5
		protected override string info
		{
			get
			{
				string text = "Set '";
				string text2 = this.IKGoal.ToString();
				string text3 = "' ";
				BBParameter<GameObject> bbparameter = this.goal;
				return text + text2 + text3 + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000085E9 File Offset: 0x000067E9
		protected override void OnExecute()
		{
			base.router.onAnimatorIK += this.OnAnimatorIK;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00008602 File Offset: 0x00006802
		protected override void OnStop()
		{
			base.router.onAnimatorIK -= this.OnAnimatorIK;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000861C File Offset: 0x0000681C
		private void OnAnimatorIK(EventData<int> msg)
		{
			base.agent.SetIKPositionWeight(this.IKGoal, this.weight.value);
			base.agent.SetIKPosition(this.IKGoal, this.goal.value.transform.position);
			base.EndAction();
		}

		// Token: 0x04000126 RID: 294
		public AvatarIKGoal IKGoal;

		// Token: 0x04000127 RID: 295
		[RequiredField]
		public BBParameter<GameObject> goal;

		// Token: 0x04000128 RID: 296
		public BBParameter<float> weight;
	}
}
