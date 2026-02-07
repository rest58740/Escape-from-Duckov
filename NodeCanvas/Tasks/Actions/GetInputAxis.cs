using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000A4 RID: 164
	[Category("Input (Legacy System)")]
	public class GetInputAxis : ActionTask
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x0000A87A File Offset: 0x00008A7A
		protected override void OnExecute()
		{
			this.Do();
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000A882 File Offset: 0x00008A82
		protected override void OnUpdate()
		{
			this.Do();
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000A88C File Offset: 0x00008A8C
		private void Do()
		{
			float num = string.IsNullOrEmpty(this.xAxisName.value) ? 0f : Input.GetAxis(this.xAxisName.value);
			float num2 = string.IsNullOrEmpty(this.yAxisName.value) ? 0f : Input.GetAxis(this.yAxisName.value);
			float num3 = string.IsNullOrEmpty(this.zAxisName.value) ? 0f : Input.GetAxis(this.zAxisName.value);
			this.saveXAs.value = num * this.multiplier.value;
			this.saveYAs.value = num2 * this.multiplier.value;
			this.saveZAs.value = num3 * this.multiplier.value;
			this.saveAs.value = new Vector3(num, num2, num3) * this.multiplier.value;
			if (!this.repeat)
			{
				base.EndAction();
			}
		}

		// Token: 0x040001C7 RID: 455
		public BBParameter<string> xAxisName = "Horizontal";

		// Token: 0x040001C8 RID: 456
		public BBParameter<string> yAxisName;

		// Token: 0x040001C9 RID: 457
		public BBParameter<string> zAxisName = "Vertical";

		// Token: 0x040001CA RID: 458
		public BBParameter<float> multiplier = 1f;

		// Token: 0x040001CB RID: 459
		[BlackboardOnly]
		public BBParameter<Vector3> saveAs;

		// Token: 0x040001CC RID: 460
		[BlackboardOnly]
		public BBParameter<float> saveXAs;

		// Token: 0x040001CD RID: 461
		[BlackboardOnly]
		public BBParameter<float> saveYAs;

		// Token: 0x040001CE RID: 462
		[BlackboardOnly]
		public BBParameter<float> saveZAs;

		// Token: 0x040001CF RID: 463
		public bool repeat;
	}
}
