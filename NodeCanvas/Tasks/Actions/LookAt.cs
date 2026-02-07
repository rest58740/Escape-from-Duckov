using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200009F RID: 159
	[Category("GameObject")]
	public class LookAt : ActionTask<Transform>
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000A618 File Offset: 0x00008818
		protected override string info
		{
			get
			{
				string text = "LookAt ";
				BBParameter<GameObject> bbparameter = this.lookTarget;
				return text + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000A636 File Offset: 0x00008836
		protected override void OnExecute()
		{
			this.DoLook();
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000A63E File Offset: 0x0000883E
		protected override void OnUpdate()
		{
			this.DoLook();
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000A648 File Offset: 0x00008848
		private void DoLook()
		{
			Vector3 position = this.lookTarget.value.transform.position;
			position.y = base.agent.position.y;
			base.agent.LookAt(position);
			if (!this.repeat)
			{
				base.EndAction(true);
			}
		}

		// Token: 0x040001C1 RID: 449
		[RequiredField]
		public BBParameter<GameObject> lookTarget;

		// Token: 0x040001C2 RID: 450
		public bool repeat;
	}
}
