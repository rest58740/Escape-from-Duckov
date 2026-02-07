using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200007A RID: 122
	[Category("✫ Blackboard")]
	public class NormalizeVector : ActionTask
	{
		// Token: 0x06000235 RID: 565 RVA: 0x000092F8 File Offset: 0x000074F8
		protected override void OnExecute()
		{
			this.targetVector.value = this.targetVector.value.normalized * this.multiply.value;
			base.EndAction(true);
		}

		// Token: 0x0400016D RID: 365
		public BBParameter<Vector3> targetVector;

		// Token: 0x0400016E RID: 366
		public BBParameter<float> multiply = 1f;
	}
}
