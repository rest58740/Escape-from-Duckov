using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000A6 RID: 166
	[Category("Input (Legacy System)")]
	public class GetMouseScrollDelta : ActionTask
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000AA00 File Offset: 0x00008C00
		protected override string info
		{
			get
			{
				string text = "Get Scroll Delta as ";
				BBParameter<float> bbparameter = this.saveAs;
				return text + ((bbparameter != null) ? bbparameter.ToString() : null);
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000AA1E File Offset: 0x00008C1E
		protected override void OnExecute()
		{
			this.Do();
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000AA26 File Offset: 0x00008C26
		protected override void OnUpdate()
		{
			this.Do();
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000AA2E File Offset: 0x00008C2E
		private void Do()
		{
			this.saveAs.value = Input.GetAxis("Mouse ScrollWheel");
			if (!this.repeat)
			{
				base.EndAction();
			}
		}

		// Token: 0x040001D2 RID: 466
		[BlackboardOnly]
		public BBParameter<float> saveAs;

		// Token: 0x040001D3 RID: 467
		public bool repeat;
	}
}
