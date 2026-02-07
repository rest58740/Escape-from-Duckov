using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000A5 RID: 165
	[Category("Input (Legacy System)")]
	public class GetMousePosition : ActionTask
	{
		// Token: 0x060002BA RID: 698 RVA: 0x0000A9C8 File Offset: 0x00008BC8
		protected override void OnExecute()
		{
			this.Do();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000A9D0 File Offset: 0x00008BD0
		protected override void OnUpdate()
		{
			this.Do();
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000A9D8 File Offset: 0x00008BD8
		private void Do()
		{
			this.saveAs.value = Input.mousePosition;
			if (!this.repeat)
			{
				base.EndAction();
			}
		}

		// Token: 0x040001D0 RID: 464
		[BlackboardOnly]
		public BBParameter<Vector3> saveAs;

		// Token: 0x040001D1 RID: 465
		public bool repeat;
	}
}
