using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200009E RID: 158
	[Category("GameObject")]
	public class InstantiateGameObject : ActionTask<Transform>
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000A514 File Offset: 0x00008714
		protected override string info
		{
			get
			{
				string[] array = new string[8];
				array[0] = "Instantiate ";
				array[1] = base.agentInfo;
				array[2] = " under ";
				array[3] = (this.parent.value ? this.parent.ToString() : "World");
				array[4] = " at ";
				int num = 5;
				BBParameter<Vector3> bbparameter = this.clonePosition;
				array[num] = ((bbparameter != null) ? bbparameter.ToString() : null);
				array[6] = " as ";
				int num2 = 7;
				BBParameter<GameObject> bbparameter2 = this.saveCloneAs;
				array[num2] = ((bbparameter2 != null) ? bbparameter2.ToString() : null);
				return string.Concat(array);
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000A5A8 File Offset: 0x000087A8
		protected override void OnExecute()
		{
			GameObject gameObject = Object.Instantiate<GameObject>(base.agent.gameObject, this.parent.value, false);
			gameObject.transform.position = this.clonePosition.value;
			gameObject.transform.eulerAngles = this.cloneRotation.value;
			this.saveCloneAs.value = gameObject;
			base.EndAction();
		}

		// Token: 0x040001BD RID: 445
		public BBParameter<Transform> parent;

		// Token: 0x040001BE RID: 446
		public BBParameter<Vector3> clonePosition;

		// Token: 0x040001BF RID: 447
		public BBParameter<Vector3> cloneRotation;

		// Token: 0x040001C0 RID: 448
		[BlackboardOnly]
		public BBParameter<GameObject> saveCloneAs;
	}
}
