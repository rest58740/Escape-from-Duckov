using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x02000061 RID: 97
	[Name("Set Look At", 0)]
	[Category("Animator")]
	public class MecanimSetLookAt : ActionTask<Animator>
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00008813 File Offset: 0x00006A13
		protected override string info
		{
			get
			{
				string text = "Mec.SetLookAt ";
				BBParameter<GameObject> bbparameter = this.targetPosition;
				return text + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00008831 File Offset: 0x00006A31
		protected override void OnExecute()
		{
			base.router.onAnimatorIK += this.OnAnimatorIK;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000884A File Offset: 0x00006A4A
		protected override void OnStop()
		{
			base.router.onAnimatorIK -= this.OnAnimatorIK;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00008863 File Offset: 0x00006A63
		private void OnAnimatorIK(EventData<int> msg)
		{
			base.agent.SetLookAtPosition(this.targetPosition.value.transform.position);
			base.agent.SetLookAtWeight(this.targetWeight.value);
			base.EndAction();
		}

		// Token: 0x04000130 RID: 304
		public BBParameter<GameObject> targetPosition;

		// Token: 0x04000131 RID: 305
		public BBParameter<float> targetWeight;
	}
}
