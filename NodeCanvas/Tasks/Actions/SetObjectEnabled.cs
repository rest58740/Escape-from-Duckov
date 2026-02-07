using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x020000A2 RID: 162
	[Name("Set Enabled", 0)]
	[Category("GameObject")]
	[Description("Set the monobehaviour's enabled state.")]
	public class SetObjectEnabled : ActionTask<MonoBehaviour>
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000A798 File Offset: 0x00008998
		protected override string info
		{
			get
			{
				return string.Format("{0} {1}", this.setTo, base.agentInfo);
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000A7B8 File Offset: 0x000089B8
		protected override void OnExecute()
		{
			bool enabled;
			if (this.setTo == SetObjectEnabled.SetEnableMode.Toggle)
			{
				enabled = !base.agent.enabled;
			}
			else
			{
				enabled = (this.setTo == SetObjectEnabled.SetEnableMode.Enable);
			}
			base.agent.enabled = enabled;
			base.EndAction();
		}

		// Token: 0x040001C5 RID: 453
		public SetObjectEnabled.SetEnableMode setTo = SetObjectEnabled.SetEnableMode.Toggle;

		// Token: 0x02000139 RID: 313
		public enum SetEnableMode
		{
			// Token: 0x0400037F RID: 895
			Disable,
			// Token: 0x04000380 RID: 896
			Enable,
			// Token: 0x04000381 RID: 897
			Toggle
		}
	}
}
