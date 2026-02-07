using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000D6 RID: 214
	[Category("✫ Utility")]
	public class Wait : ActionTask
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600039F RID: 927 RVA: 0x0000E480 File Offset: 0x0000C680
		protected override string info
		{
			get
			{
				string text = this.random ? string.Format(" {0} to {1}", this.waitTimeRange.value.x, this.waitTimeRange.value.y) : this.waitTime.value.ToString();
				return string.Format("Wait " + text + " sec.", Array.Empty<object>());
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000E4FC File Offset: 0x0000C6FC
		protected override void OnExecute()
		{
			this.realWaitTime = (this.random ? Random.Range(this.waitTimeRange.value.x, this.waitTimeRange.value.y) : this.waitTime.value);
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000E549 File Offset: 0x0000C749
		protected override void OnUpdate()
		{
			if (base.elapsedTime >= this.realWaitTime)
			{
				base.EndAction(this.finishStatus == CompactStatus.Success);
			}
		}

		// Token: 0x0400027C RID: 636
		public bool random;

		// Token: 0x0400027D RID: 637
		[ShowIf("random", 0)]
		public BBParameter<float> waitTime = 1f;

		// Token: 0x0400027E RID: 638
		[ShowIf("random", 1)]
		public BBParameter<Vector2> waitTimeRange = Vector2.one;

		// Token: 0x0400027F RID: 639
		public CompactStatus finishStatus = CompactStatus.Success;

		// Token: 0x04000280 RID: 640
		private float realWaitTime;
	}
}
